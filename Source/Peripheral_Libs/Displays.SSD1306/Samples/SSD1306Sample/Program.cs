using System.Threading;
using Netduino.Foundation.Displays;

namespace SSD1306Test
{
    public class Program
    {
        public static void Main()
        {
            var oled = new SSD1306(0x3c, 400, SSD1306.DisplayType.OLED128x64);
            var display = new GraphicsLibrary(oled);

            display.Clear(true);
            display.DrawLine(0, 30, 80, 60, true);
            display.Show();
            Thread.Sleep(1000);

            display.Clear(true);
            display.DrawFilledCircle(63, 31, 20, true);
            display.Show();
            Thread.Sleep(1000);

            display.Clear(true);
            display.DrawRectangle(20, 20, 60, 40);
            display.Show();
            Thread.Sleep(1000);

            display.Clear(true);
            display.DrawFilledRectangle(30, 10, 50, 40);
            display.Show();
            Thread.Sleep(1000);

            display.Clear(true);
            display.CurrentFont = new Font8x8();
            display.DrawText(4, 10, 0, "NETDUINO 3 WiFi");
            display.Show();
            Thread.Sleep(Timeout.Infinite);
        }
    }
}