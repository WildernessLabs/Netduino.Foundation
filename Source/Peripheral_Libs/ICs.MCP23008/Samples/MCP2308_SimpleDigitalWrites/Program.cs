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
            _mcp = new MCP23008(39);

            while (true)
            {
                for (int i = 0; i <= 7; i++)
                {
                    // can write a byte mask that specifies all the pin
                    // values in one byte
                    _mcp.OutputWrite((byte)(1 << i));

                    // or you can write to individual pins:
                    //for (int j = 0; j <= 7; j++) {
                    //    _mcp.WriteToPort(j, false);
                    //}
                    //_mcp.WriteToPort(i, true);

                    Debug.Print("i: " + i.ToString());
                    Thread.Sleep(250);
                }
            }
        }
    }
}
