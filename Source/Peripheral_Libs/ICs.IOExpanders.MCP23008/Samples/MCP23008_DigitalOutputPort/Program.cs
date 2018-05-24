using System;
using System.Threading;
using Microsoft.SPOT;
using Netduino.Foundation.ICs.IOExpanders.MCP23008;

namespace MCP23008_DigitalOutputPort
{
    public class Program
    {
        static MCP23008 _mcp = null;

        public static void Main()
        {
            _mcp = new MCP23008(39);

            // create an array of ports
            DigitalOutputPort[] ports = new DigitalOutputPort[8];
            for (byte i = 0; i <= 7; i++) {
                ports[i] = _mcp.CreateOutputPort(i, false);
            }

            while (true)
            {
                // count from 0 to 7 (8 leds)
                for (int i = 0; i <= 7; i++)
                {
                    // turn on the LED that matches the count
                    for (byte j = 0; j <= 7; j++)
                    {
                        ports[j].State = (i == j);
                    }

                    Debug.Print("i: " + i.ToString());
                    Thread.Sleep(250);
                }
            }
        }
    }
}
