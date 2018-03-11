using System;
using Microsoft.SPOT;
using Netduino.Foundation.Communications;

namespace Netduino.Foundation.ICs
{
    public class MCP23008
    {
        private readonly I2CBus _i2cBus;


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


        protected MCP23008()
        { }

        public MCP23008(bool pinA0, bool pinA1, bool pinA2, ushort speed = 100) 
            : this(MCPAddressTable.GetAddressFromPins(pinA0, pinA1, pinA2), speed)
        {
           // nothing goes here
        }

        public MCP23008(byte address = 0x20, ushort speed = 100)
        {
            this._i2cBus = new I2CBus(address, speed);
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
