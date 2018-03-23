using System;
using System.Threading;
using Microsoft.SPOT;
using Netduino.Foundation.GPIO;
using Netduino.Foundation.ICs.MCP23008;
using Netduino.Foundation.Relays;

namespace MCP23008_RelaySample
{
    /// <summary>
    /// Illustrates using a Netduino.Foundation.Relays.Relay object
    /// driven by an MCP23008 I2C output expander
    /// </summary>
    public class Program
    {
        public static void Main()
        {
            // create our MCP23008
            MCP23008 mcp = new MCP23008(39); // all address pins pulled high

            // create a digital output port from that mcp
            DigitalOutputPort relayPort = mcp.CreateOutputPort(0, false);

            // create a new relay using that digital output port
            Relay relay = new Relay(relayPort);

            // loop forever
            while (true) {
                // toggle the relay
                relay.Toggle();

                // wait for 5 seconds
                Thread.Sleep(5000);
            }
        }
    }
}
