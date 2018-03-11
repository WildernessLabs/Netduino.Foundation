using System;
using System.Threading;
using Microsoft.SPOT;
using Netduino.Foundation.ICs.MCP23008;

namespace MCP2308_SimpleDigitalWrites
{
    public class Program
    {
        static MCP23008 _mcp = null;

        public static void Main()
        {
            _mcp = new MCP23008();

            for (int i = 0; i <= 7; i++)
            {
                //clear ports (TEMP)
                for (int j = 0; j <= 7; j++)
                {
                    _mcp.WriteToPort(j, false);
                }

                _mcp.WriteToPort(i, true);
                Thread.Sleep(250);
            }
        }
    }
}
