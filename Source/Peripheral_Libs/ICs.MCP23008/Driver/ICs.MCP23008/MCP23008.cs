using System;
using Microsoft.SPOT;
using Netduino.Foundation.Communications;

namespace Netduino.Foundation.ICs
{
    public class MCP23008
    {
        private readonly I2CBus _i2cBus;

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
