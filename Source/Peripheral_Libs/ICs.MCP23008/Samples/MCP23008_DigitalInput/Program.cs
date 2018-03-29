using System.Threading;
using Microsoft.SPOT;
using Netduino.Foundation.ICs.MCP23008;

namespace MCP23008_DigitalInput
{
    public class Program
    {
        static MCP23008 _mcp = null;

        public static void Main()
        {
            _mcp = new MCP23008(true, true, true); // all address pins pulled high (address of 39)

            _mcp.ConfigureInputPort(0, true); // set pin 0 to input with pullup = true

            bool port0Value = false;
            while (true) {
                port0Value = _mcp.ReadPort(0);
                Debug.Print("Port 0: " + ((port0Value) ? "high" : "low") );

                Thread.Sleep(250);
            }
        }
    }
}
