using System.Threading;
using Microsoft.SPOT;
using Netduino.Foundation.ICs.IOExpanders.MCP23008;
using Netduino.Foundation.GPIO;

namespace MCP23008_DigitalInputPort_Events
{
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

            // wire up a changed handler to output to the console when the port changes.
            port.Changed += (object sender, PortInterruptEventArgs e) => {
                Debug.Print("Port changed event, value: " + ((e.ValueAtInterrupt) ? "high" : "low"));
            };

            // wait forever
            while (true)
            {
                Thread.Sleep(Timeout.Infinite);
            }
        }

    }
}
