using Microsoft.SPOT;
using Netduino.Foundation.Communications;
using Netduino.Foundation.GPIO;
using H = Microsoft.SPOT.Hardware;

namespace Netduino.Foundation.ICs.IOExpanders.MCP23008
{
    public class MCP23008
    {
        public event DeviceInterruptEventHandler InterruptRaised = delegate { };

        private readonly I2CBus _i2cBus;

        // state
        byte _iodir;
        byte _gpio;
        byte _olat;
        byte _gppu;

        /// <summary>
        ///     object for using lock() to do thread synch
        /// </summary>
        protected object _lock = new object();

        // register addresses
        // IO Direction - controls the direction of the GPIO
        private const byte _IODirectionRegister = 0x00; //IODIR
        private const byte _InputPolarityRegister = 0x01; //IPOL
        private const byte _InterruptOnChangeRegister = 0x02; //GPINTEN
        private const byte _DefaultComparisonValueRegister = 0x03; //DEFVAL
        private const byte _InterruptControlRegister = 0x04; //INTCON
        private const byte _IOConfigurationRegister = 0x05; //IOCON
        private const byte _PullupResistorConfigurationRegister = 0x06; //GPPU
        private const byte _InterruptFlagRegister = 0x07; //INTF
        private const byte _InterruptCaptureRegister = 0x08; //INTCAP
        private const byte _GPIORegister = 0x09; //GPIO
        private const byte _OutputLatchRegister = 0x0A; //OLAT

        // TODO: do we want this public? it's so we can use IOExpanders
        //public IDigitalInputPort DigitalIn { get; protected set; }
        protected H.InterruptPort _interruptPort = null;

        /// <summary>
        /// We track these so we can raise events on them.
        /// </summary>
        protected DigitalInputPort[] _inputPorts = new DigitalInputPort[7]; 

        protected MCP23008()
        { }

        public MCP23008(bool pinA0, bool pinA1, bool pinA2, ushort speed = 100)
            : this(MCPAddressTable.GetAddressFromPins(pinA0, pinA1, pinA2), speed)
        {
            // nothing goes here
        }
        public MCP23008(bool pinA0, bool pinA1, bool pinA2, H.Cpu.Pin interruptPin, ushort speed = 100)
            : this(interruptPin, MCPAddressTable.GetAddressFromPins(pinA0, pinA1, pinA2), speed)
        {
            // nothing goes here
        }

        public MCP23008(H.Cpu.Pin interruptPin, byte address = 0x20, ushort speed = 100)
            : this(address, speed)
        {
            //TODO: should I do this here, or in the main CTOR below?
            // 
            _interruptPort = new H.InterruptPort(interruptPin, false, H.Port.ResistorMode.PullDown, H.Port.InterruptMode.InterruptEdgeHigh);
            _interruptPort.OnInterrupt += OnInterrupt;
        }

        protected void OnInterrupt(uint data1, uint data2, System.DateTime time)
        {
            // TODO: right now, we're only raising the first interrupt we find,
            // but multiple interrupts can come in at once.

            // 1) determine which pin it was
            byte interruptFlags = _i2cBus.ReadRegister(_InterruptFlagRegister);
            for (byte i = 0; i <= 7; i++) // loop through each flag
            {
                // if the flag is set, that pin has an interrupt 
                // (multiple pins can have interrupt at same time)
                if (BitHelpers.GetBitValue(interruptFlags, i))
                {
                    // 2) get value (note, this clears on read. it also clears on 
                    // GPIO read, so we will need to think about this more later).
                    byte interruptCapture = _i2cBus.ReadRegister(_InterruptCaptureRegister);

                    // 3) raise events
                    bool valueAtInterrupt = BitHelpers.GetBitValue(interruptCapture, i);

                    //   a) raise chip wide event
                    this.InterruptRaised(this, new DeviceInterruptEventArgs() { Pin = i, ValueAtInterrupt = valueAtInterrupt });

                    //   b) raise on the port, if interrupt is enabled
                    if (this._inputPorts[i].InterruptEnabled)
                    {
                        this._inputPorts[i].RaiseInterrupt(valueAtInterrupt);
                    }

                    break;
                }
            }
        
        }

        public MCP23008(byte address = 0x20, ushort speed = 100)
        {
            // configure our i2c bus so we can talk to the chip
            this._i2cBus = new I2CBus(address, speed);

            // make sure the chip is in a default state
            Initialize();
            Debug.Print("Chip Reset.");

            // read in the initial state of the chip
            _iodir = this._i2cBus.ReadRegister(_IODirectionRegister);
            _gpio = this._i2cBus.ReadRegister(_GPIORegister);
            _olat = this._i2cBus.ReadRegister(_OutputLatchRegister);
        }

        protected void Initialize()
        {
            byte[] buffers = new byte[10];

            // IO Direction
            buffers[0] = 0xFF; //all input `11111111`

            // set all the other registers to zeros (we skip the last one, output latch)
            for (int i = 1; i < 10; i++ ) {
                buffers[i] = 0x00; //all zero'd out `00000000`
            }

            // the chip will automatically write all registers sequentially.
            this._i2cBus.WriteRegisters(_IODirectionRegister, buffers);

            // save our state
            this._iodir = buffers[0];
            this._gpio = 0x00;
            this._olat = 0x00;
            this._gppu = 0x00;
        }

        /// <summary>
        /// Creates a new DigitalOutputPort using the specified pin and initial state.
        /// </summary>
        /// <param name="pin">The pin number to create the port on.</param>
        /// <param name="initialState">Whether the pin is initially high or low.</param>
        /// <returns></returns>
        public DigitalOutputPort CreateOutputPort(byte pin, bool initialState)
        {
            if (IsValidPin(pin))
            {
                // setup the port internally for output
                this.SetPortDirection(pin, PortDirectionType.Output);

                // create the convenience class
                DigitalOutputPort port = new DigitalOutputPort(this, pin, initialState);

                // return the port
                return port;
            }

            throw new System.Exception("Pin is out of range");
        }

        public DigitalInputPort CreateInputPort(byte pin, bool enablePullUp = false, bool enableInterrupt = false)
        {
            if (IsValidPin(pin))
            {
                // configure the pin
                this.ConfigureInputPort(pin, enablePullUp, false);

                // create the convenience class
                DigitalInputPort port = new DigitalInputPort(this, pin, false);

                // persist out port locally
                // TODO: we really need to check to see if one already exists
                // and use that, but they generally never change once setup
                // so we can get away with what we're doing here for now.
                // we should be a little more sophisticated with Meadow
                // they should probably also release when finalized and all that.
                this._inputPorts[pin] = port;

                // if interrupts are enabled, we have additional work
                if (enableInterrupt)
                {
                    ConfigureInterrupt(pin);
                }


                return port;
            }

            throw new System.Exception("Pin is out of range");
        }

        /// <summary>
        /// Sets the direction of a particulare port.
        /// </summary>
        /// <param name="pin"></param>
        /// <param name="direction"></param>
        public void SetPortDirection(byte pin, PortDirectionType direction)
        {
            if (IsValidPin(pin))
            {
                // if it's already configured, get out. (1 = input, 0 = output)
                if (direction == PortDirectionType.Input)
                {
                    if (BitHelpers.GetBitValue(_iodir, pin)) return;
                    //if ((_iodir & (byte)(1 << pin)) != 0) return;
                }
                else
                {
                    if (!BitHelpers.GetBitValue(_iodir, pin)) return;
                    //if ((_iodir & (byte)(1 << pin)) == 0) return;
                }

                // set the IODIR bit and write the setting
                _iodir = BitHelpers.SetBit(_iodir, (byte)pin, (byte)direction);
                this._i2cBus.WriteRegister(_IODirectionRegister, _iodir);
            }
            else
            {
                throw new System.Exception("Pin is out of range");
            }
        }

        public void ConfigureInputPort(byte pin, bool enablePullUp = false, bool enableInterrupt = false)
        {
            if (IsValidPin(pin))
            {
                // set the port direction
                this.SetPortDirection(pin, PortDirectionType.Input);

                // refresh out pull up state
                // TODO: do away with this and trust internal state?
                _gppu = this._i2cBus.ReadRegister(_PullupResistorConfigurationRegister);

                _gppu = BitHelpers.SetBit(_gppu, pin, enablePullUp);

                this._i2cBus.WriteRegister(_PullupResistorConfigurationRegister, _gppu);

                // if interrupts are enabled, we have additional work
                if (enableInterrupt)
                {
                    ConfigureInterrupt(pin);
                }
            }
            else
            {
                throw new System.Exception("Pin is out of range");
            }
        }

        protected void ConfigureInterrupt(byte pin)
        {
            if (!IsInterruptValid()) { throw new System.Exception("interrupt pin must be set to use interrupts. specifiy the interrupt pin in the ctor.")}

            // interrupt on change (whether or not we want to raise an interrupt on the interrupt pin on change)
            byte gpinten = this._i2cBus.ReadRegister(_InterruptOnChangeRegister);
            gpinten = BitHelpers.SetBit(gpinten, pin, true);

            // interrupt control register; whether or not the change is based 
            // on default comparison value, or if a change from previous. We 
            // want to raise on change, so we set it to 0, always.
            byte interruptControl = this._i2cBus.ReadRegister(_InterruptControlRegister);
            interruptControl = BitHelpers.SetBit(interruptControl, pin, false);

            // update IOCON: ODR and INTPOL settings
            byte ioConfiguration = this._i2cBus.ReadRegister(_IOConfigurationRegister);
            // set ODR = 0
            ioConfiguration = BitHelpers.SetBit(ioConfiguration, 2, false);
            // set INTPOL = 1
            ioConfiguration = BitHelpers.SetBit(ioConfiguration, 1, true);

            // write all the registers
            this._i2cBus.WriteRegister(_InterruptOnChangeRegister, gpinten);
            this._i2cBus.WriteRegister(_InterruptControlRegister, interruptControl);
            this._i2cBus.WriteRegister(_IOConfigurationRegister, ioConfiguration);

        }

        /// <summary>
        /// Sets a particular pin's value. If that pin is not 
        /// in output mode, this method will first set its 
        /// mode to output.
        /// </summary>
        /// <param name="pin">The pin to write to.</param>
        /// <param name="value">The value to write. True for high, false for low.</param>
        public void WriteToPort(byte pin, bool value)
        {
            if (IsValidPin(pin))
            {
                // if the pin isn't configured for output, configure it
                this.SetPortDirection((byte)pin, PortDirectionType.Output);

                // update our output latch 
                _olat = BitHelpers.SetBit(_olat, (byte)pin, value);

                // write to the output latch (actually does the output setting)
                this._i2cBus.WriteRegister(_OutputLatchRegister, _olat);
            }
            else
            {
                throw new System.Exception("Pin is out of range");
            }
        }

        public bool ReadPort(byte pin)
        {
            if (!IsValidPin(pin))
            {
                throw new System.Exception("Pin is out of range");
            }

            // if the pin isn't set for input, configure it
            this.SetPortDirection((byte)pin, PortDirectionType.Input);

            // update our GPIO values
            _gpio = this._i2cBus.ReadRegister(_GPIORegister);

            // return the value on that port
            return BitHelpers.GetBitValue(_gpio, (byte)pin);
        }

        /// <summary>
        /// Outputs a byte value across all of the pins by writing directly 
        /// to the output latch (OLAT) register.
        /// </summary>
        /// <param name="mask"></param>
        public void WriteToPorts(byte mask)
        {
            // set all IO to output
            if (_iodir != 0) {
                _iodir = 0;
                this._i2cBus.WriteRegister(_IODirectionRegister, _iodir);
            }
            // write the output
            _olat = mask;
            this._i2cBus.WriteRegister(_OutputLatchRegister, _olat);
        }

        protected bool IsValidPin(byte pin)
        {
            return (pin >= 0 && pin <= 7);
        }

        /// <summary>
        /// Checks to see whether or not the interrupt port is configured.
        /// </summary>
        /// <returns></returns>
        protected bool IsInterruptValid()
        {
            return !(_interruptPort == null);
        }

        // what's a good way to do this? maybe constants? how to name?
        public enum ValidSpeeds : ushort
        {
            hundredk = 100,
            fourhundredk = 400,
            onepointsevenmegs = 17000,
        }
    }
}
