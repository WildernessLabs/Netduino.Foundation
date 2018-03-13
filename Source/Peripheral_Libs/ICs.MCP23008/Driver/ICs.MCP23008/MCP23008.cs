using System;
using System.Threading;
using Microsoft.SPOT;
using H = Microsoft.SPOT.Hardware;
using Netduino.Foundation.Communications;
using Netduino.Foundation.GPIO;

namespace Netduino.Foundation.ICs.MCP23008
{
    public class MCP23008
    {
        private readonly I2CBus _i2cBus;
        

        // state
        byte _iodir;
        byte _gpio;
        byte _olat;



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



        // protected properties
        protected bool SequentialAddressOperationEnabled
        {
            get {
                return _sequentialAddressOperationEnabled;
            }
            set {
                this._i2cBus.WriteRegister(_IOConfigurationRegister, (byte)(value ? 1 : 0));
            }
        } private bool _sequentialAddressOperationEnabled = false;


        protected MCP23008()
        { }

        public MCP23008(bool pinA0, bool pinA1, bool pinA2, ushort speed = 100)
            : this(MCPAddressTable.GetAddressFromPins(pinA0, pinA1, pinA2), speed)
        {
            // nothing goes here
        }

        public MCP23008(byte address = 0x20, ushort speed = 100)
        {
            // tried this, based on a forum post, but seems to have no effect.
            //H.OutputPort SDA = new H.OutputPort(N.Pins.GPIO_PIN_A4, false);
            //H.OutputPort SCK = new H.OutputPort(N.Pins.GPIO_PIN_A5, false);
            //SDA.Dispose();
            //SCK.Dispose();

            // configure our i2c bus so we can talk to the chip
            this._i2cBus = new I2CBus(address, speed);

            Debug.Print("initialized.");

            // make sure the chip is in a default state
            ResetChip();
            Debug.Print("Chip Reset.");
            //Thread.Sleep(100);

            // read in the initial state of the chip
            _iodir = this._i2cBus.ReadRegister(_IODirectionRegister);
            //Thread.Sleep(100);
            //Debug.Print("IODIR: " + _iodir.ToString("X"));
            _gpio = this._i2cBus.ReadRegister(_GPIORegister);
            //Thread.Sleep(100);
            //Debug.Print("GPIO: " + _gpio.ToString("X"));
            _olat = this._i2cBus.ReadRegister(_OutputLatchRegister);
            //Thread.Sleep(100);
            //Debug.Print("OLAT: " + _olat.ToString("X"));
        }

        protected void ResetChip()
        {
            byte[] buffers = new byte[10];

            // IO Direction
            buffers[0] = 0xFF; //all input `11111111`

            // set all the other registers to zeros (we skip the last one, output latch)
            for (int i = 1; i < 10; i++ )
            {
                buffers[i] = 0x00; //all zero'd out `00000000`
            }

            // the chip will automatically write all registers sequentially.
            this._i2cBus.WriteRegisters(_IODirectionRegister, buffers);
        }

        public DigitalOutputPort CreateOutputPort(byte pin, bool initialState)
        {
            // setup the port internally for output
            this.ConfigureOutputPort(pin);

            // create the convenience class
            DigitalOutputPort port = new DigitalOutputPort(this, pin, initialState);

            // return the port
            return port;
        }

        protected void ConfigureOutputPort(byte pin)
        {
            // if it's already configured, get out. (1 = input, 0 = output)
            if ((_iodir & (byte)(1 << pin)) == 0) return; //actually checking if the 1 is set, and negating that
            
            // setup the port internally for output in a thread safe way
            // note this is only thread safe for this method, we nee readerWriterLockSlim, but 
            // it's not available in NETMF
            lock (_lock)
            {
                // configure that pin for output (1 = input, 0 = output)
                //_iodir &= (byte)~(1 << pin);
                SetRegisterBit(ref _iodir, (byte)pin, false);

                // write our new setting
                this._i2cBus.WriteRegister(_IODirectionRegister, _iodir);
            }
        }

        public void SetPortDirection(byte pin, PortDirectionType direction)
        {
            // if it's already configured, get out. (1 = input, 0 = output)
            if (direction == PortDirectionType.Input) {
                if ((_iodir & (byte)(1 << pin)) != 0) return;
            } else {
                if ((_iodir & (byte)(1 << pin)) == 0) return;
            }

            // set the IODIR bit and write the setting
            SetRegisterBit(ref _iodir, (byte)pin, (byte)direction);
            this._i2cBus.WriteRegister(_IODirectionRegister, _iodir);
        }

        public void WriteToPort(int pin, bool value)
        {
            // if the pin isn't configured for output, configure it
            this.ConfigureOutputPort((byte)pin);

            // update our output latch 
            SetRegisterBit(ref _olat, (byte)pin, value);
            // write to the output latch (actually does the output setting)
            this._i2cBus.WriteRegister(_OutputLatchRegister, _olat);
        }

        public void OutputWrite(byte outputMask)
        {
            // set all IO to output
            if (_iodir != 0) {
                _iodir = 0;
                this._i2cBus.WriteRegister(_IODirectionRegister, _iodir);
            }
            // write the output
            _olat = outputMask;
            this._i2cBus.WriteRegister(_OutputLatchRegister, _olat);
        }

        protected void SetRegisterBit(ref byte registerMask, byte bitIndex, byte value)
        {
            SetRegisterBit(ref registerMask, bitIndex, (value == 0) ? false : true);
        }

        protected void SetRegisterBit(ref byte registerMask, byte bitIndex, bool value)
        {
            if (value) {
                registerMask |= (byte)(1 << bitIndex);
            } else {
                registerMask &= (byte)~(1 << bitIndex); // tricky to zero
            }
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
