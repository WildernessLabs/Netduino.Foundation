using System.Threading;
using Netduino.Foundation.Displays;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;

namespace SSD1306Test
{
    public class Program
    {
        public static void Main()
        {
            // I2C constructor
            // var oled = new SSD1306(Cpu.Pin.GPIO_Pin9, Cpu.Pin.GPIO_Pin10, Cpu.Pin.GPIO_Pin7, SPI.SPI_module.SPI1, 4000, SSD1306.DisplayType.OLED128x32);

            // SPI constructor
            var oled = new SSD1306(chipSelectPin: Pins.GPIO_PIN_D9,
                dcPin: Pins.GPIO_PIN_D10,
                resetPin: Pins.GPIO_PIN_D7,
                spiModule: SPI.SPI_module.SPI1,
                speedKHz: 400000, 
                displayType: SSD1306.DisplayType.OLED128x32);

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
            display.DrawText(4, 10, "NETDUINO 3 WiFi");
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