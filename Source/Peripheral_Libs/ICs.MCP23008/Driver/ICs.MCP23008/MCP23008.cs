using System;
using Microsoft.SPOT;
using Netduino.Foundation.Communications;

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
            // configure our i2c bus so we can talk to the chip
            this._i2cBus = new I2CBus(address, speed);

            // read in the initial state of the chip
            _iodir = this._i2cBus.ReadRegister(_IODirectionRegister);
            _gpio = this._i2cBus.ReadRegister(_GPIORegister);
            _olat = this._i2cBus.ReadRegister(_OutputLatchRegister);
        }

        protected DigitalOutputPort CreateOutputPort(byte pin, bool initialState)
        {
            // create the convenience class
            DigitalOutputPort port = new DigitalOutputPort(this, pin, initialState);
            
            // setup the port internally for output in a thread safe way
            lock (_lock)
            {
              
                // read the IODIR registers to get IO direction config
                byte iodir = this._i2cBus.ReadRegister(_IODirectionRegister);

                // configure that pin for output
                iodir |= (byte)(1 << pin); //is that the right pin mask?

                // write our new setting and update our state tracking
                this._i2cBus.WriteRegister(_IODirectionRegister, iodir);
                _iodir = iodir;
            }

            // return the port
            return port;
        }

        public bool WriteToPort(int pin, bool value)
        {
            bool success = true;

            // check to see if that port is configured
            if ((_iodir & (byte)(1 << pin)) == 1)
            {
                // write
            }



            return success;
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
