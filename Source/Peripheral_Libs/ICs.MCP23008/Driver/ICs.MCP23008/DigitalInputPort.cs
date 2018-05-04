using Netduino.Foundation.GPIO;

namespace Netduino.Foundation.ICs.IOExpanders.MCP23008
{
    public class DigitalInputPort : DigitalInputPortBase
    {
        protected readonly byte _pin;
        protected readonly MCP23008 _mcp;

        public override bool Value
        {
            get
            {
                return true;
            }
        }

        protected DigitalInputPort() : base(false) { }

        internal DigitalInputPort(MCP23008 mcp, byte pin, bool enableIntterupt) : base(enableIntterupt)
        {
            _mcp = mcp;
            _pin = pin;

            if (enableIntterupt)
            {
                // enable the interrupt stuff on the MCP
            }
        }
    }
}
