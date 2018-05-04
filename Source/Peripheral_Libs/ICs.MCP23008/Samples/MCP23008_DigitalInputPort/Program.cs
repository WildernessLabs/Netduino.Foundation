using System.Threading;
using Microsoft.SPOT;
using Netduino.Foundation.ICs.IOExpanders.MCP23008;

namespace MCP23008_DigitalInputPort
{
    /// <summary>
    /// Illustrates using a DigitalInputPort with an MCP23008. 
    /// 
    /// To use this sample, wire a push button from pin 0 to ground. 
    /// </summary>
    public class Program
    {
        static MCP23008 _mcp = null;

        public static void Main()
        {
            // Create a new MCP23008. This constructor shows how to pass
            // the address pin configuration instead of an address.
            _mcp = new MCP23008(true, true, true); // all address pins pulled high (address of 39)

            // Create a new DigitalInputPort from pin 0, pulled high
            DigitalInputPort port = _mcp.CreateInputPort(0, true);

            // loop forever
            while (true) {
                // Print the port value
                Debug.Print("Port value: " + (port.Value ? "high" : "low"));

                Thread.Sleep(250);
            }
        }
    }
}
