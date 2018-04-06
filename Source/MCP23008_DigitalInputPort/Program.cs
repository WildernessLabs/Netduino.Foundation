using System.Threading;
using Microsoft.SPOT;
using Netduino.Foundation.ICs.MCP23008;

namespace MCP23008_DigitalInputPort
{
    public class Program
    {
        static MCP23008 _mcp = null;

        public static void Main()
        {
            _mcp = new MCP23008(true, true, true); // all address pins pulled high (address of 39)

            DigitalInputPort port = _mcp.CreateInputPort(0, true);

            while (true) {
                Debug.Print("Port value: " + (port.Value ? "high" : "low"));

                Thread.Sleep(250);
            }
        }
    }
}
