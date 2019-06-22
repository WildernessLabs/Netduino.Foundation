using System.Threading;
using Netduino.Foundation.Displays;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace ST7565Test
{
    public class Program
    {
        public static void Main()
        {
            // SPI constructor
            var oled = new ST7565(chipSelectPin: Pins.GPIO_PIN_D10,
                dcPin: Pins.GPIO_PIN_D8,
                resetPin: Pins.GPIO_PIN_D9,
                spiModule: SPI.SPI_module.SPI1,
                speedKHz: 10000);

            oled.SetContrast(24);
            oled.SetContrast(12);
            oled.SetContrast(0);


            oled.Clear(true);
            oled.InvertDisplay(true);


            oled.Clear(true);
            oled.InvertDisplay(false);

            oled.IgnoreOutOfBoundsPixels = true;

            var display = new GraphicsLibrary(oled);

            display.Clear(true);
            display.DrawLine(0, 0, 60, 28, true);
            display.Show();
            Thread.Sleep(1000);

            display.Clear(true);
            display.DrawCircle(63, 31, 20, true, true);
            display.Show();
            Thread.Sleep(1000);

            display.Clear(true);
            display.DrawRectangle(20, 20, 60, 40);
            display.Show();
            Thread.Sleep(1000);

            display.Clear(true);
            display.DrawRectangle(30, 10, 50, 40, true, true);
            display.Show();
            Thread.Sleep(1000);

            display.Clear(true);
            display.CurrentFont = new Font8x8();
            display.DrawText(4, 0, "NETDUINO 3 WiFi");
            display.DrawCircle(64, 32, 16, true, true);
            display.Show();
            Thread.Sleep(Timeout.Infinite);
        }
    }
}

namespace System.Diagnostics
{
    public enum DebuggerBrowsableState
    {
        Never = 0,
        Collapsed = 2,
        RootHidden = 3
    }
}