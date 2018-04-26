using System.Threading;
using Microsoft.SPOT;
using Netduino.Foundation.ICs.IOExpanders.MCP23008;

namespace MCP23008_DigitalInput
{
    /// <summary>
    /// Illustrates simple digital input with an MCP23008
    /// </summary>
    public class Program
    {
        static MCP23008 _mcp = null;

        public static void Main()
        {
            // create a new MCP23008 with all address pins pulled high (address of 39)
            _mcp = new MCP23008(true, true, true); 

            // set pin 0 to input with pullup = true, interrupt = false
            _mcp.ConfigureInputPort(0, true, false); 

            bool port0Value = false;

            // loop forever
            while (true) {
                // read the port 0's value
                port0Value = _mcp.ReadPort(0);
                Debug.Print("Port 0: " + ((port0Value) ? "high" : "low") );

                Thread.Sleep(250);
            }
        }
    }
}
