using System;
using System.Threading;
using Microsoft.SPOT;
using N = SecretLabs.NETMF.Hardware.Netduino;
using H = Microsoft.SPOT.Hardware;
using Netduino.Foundation.Displays;

namespace SerialLCD4x20_Sample
{
    public class Program
    {
        // counts from 1-100 and displays it in a scrolling manner on a display
        public static void Main()
        {
            SerialLCD _lcd = new SerialLCD(new TextDisplayConfig { Height = 4, Width = 20 });

            _lcd.SetBrightness();

            while (true) {

                // count to 100 
                for (int i = 1; i <= (100 - _lcd.DisplayConfig.Height + 1); i++) {
                    // write to all lines
                    for (byte l = 0; l < _lcd.DisplayConfig.Height; l++) {
                        _lcd.WriteLine((i + l).ToString(), l);
                    }
                    Thread.Sleep(250);
                }

            }
            
        }
    }
}
