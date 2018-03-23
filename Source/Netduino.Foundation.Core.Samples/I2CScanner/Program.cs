using System;
using System.Threading;
using Microsoft.SPOT;
using Netduino.Foundation.Communications;

namespace I2CScanner
{
    public class Program
    {
        public static void Main()
        {
            ushort speed = 100;
            ushort timeout = 100;
            ushort numberOfDevices = 0;

            while (true)
            {
                numberOfDevices = 0;

                // loop through all the possible I2C addresses (0-127)
                for (byte i = 0; i < 127; i++)
                {
                    I2CBus i2C = new I2CBus(i, speed, timeout);
                    try
                    {
                        i2C.WriteByte(0);
                        Debug.Print("Found I2C device at: " + i.ToString("X"));
                        numberOfDevices++;
                    }
                    catch (Exception e) {

                    }
                }

                if (numberOfDevices == 0)
                {
                    Debug.Print("No I2C devices found.");
                }

                // wait five seconds before scanning again.
                Thread.Sleep(5000);
            }
        }
    }
}
