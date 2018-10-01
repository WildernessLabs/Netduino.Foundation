using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using System.Threading;
using Netduino.Foundation.Displays;
using SecretLabs.NETMF.Hardware.Netduino;
using Netduino.Foundation;
using ILI9163Sample.Properties;
using Netduino.Foundation.Sensors.Buttons;
using Netduino.Foundation.LEDs;

namespace ILI9163Test
{
    public class Program
    {
        static ILI9163 tft;
        static GraphicsLibrary display;
        static PushButton button;
        static Led led;

        public static void Main()
        {
            tft = new ILI9163(chipSelectPin: Pins.GPIO_PIN_D4,
                dcPin: Pins.GPIO_PIN_D7,
                resetPin: Pins.GPIO_PIN_D6,
                width: 128,
                height: 160,
                spiModule: SPI.SPI_module.SPI1,
                speedKHz: 15000);

            tft.ClearScreen(31);
            tft.Refresh();

            display = new GraphicsLibrary(tft);

            led = new Led(Pins.ONBOARD_LED);
            button = new PushButton(Pins.ONBOARD_BTN, CircuitTerminationType.CommonGround);
            button.Clicked += Button_Clicked;

            UITest();
            Thread.Sleep(-1);
        }

        private static void Button_Clicked(object sender, EventArgs e)
        {
            led.IsOn = !led.IsOn;
            display.DrawText(4, 145, 0, ("LED is: " + (led.IsOn ? "On ":"Off")));
            display.Show();
        }

        static void UITest()
        {
            display.CurrentFont = new Font8x12();
            //display.DrawText(4, 10, "Meadow 3 WiFi", Color.SkyBlue);
            
            display.DrawText(4, 5, "abcdefghijklm", Color.SkyBlue);
            display.DrawText(4, 20, "nopqrstuvwxyz", Color.SkyBlue);
            display.DrawText(4, 35, "`1234567890-=", Color.SkyBlue);
            display.DrawText(4, 50, "~!@#$%^&*()_+", Color.SkyBlue);
            display.DrawText(4, 65, "[]\\;',.//", Color.SkyBlue);
            display.DrawText(4, 80, "{}|:\"<>?", Color.SkyBlue);
            display.DrawText(4, 95, "ABCDEFGHIJKLM", Color.SkyBlue);
            display.DrawText(4, 110, "NOPQRSTUVWXYZ", Color.SkyBlue);
            display.Show();
            Thread.Sleep(20000);

            display.Clear();

            var bytes = Resources.GetBytes(Resources.BinaryResources.trees);

            display.DrawLine(10, 10, 118, 150, Color.OrangeRed);
            display.Show();
            Thread.Sleep(500);
            display.DrawLine(118, 10, 10, 150, Color.OrangeRed);
            display.Show();
            Thread.Sleep(500);

            display.DrawCircle(64, 64, 25, Color.Purple);
            display.Show();
            Thread.Sleep(1000);

            display.DrawRectangle(5, 5, 118, 150, Color.Aquamarine);
            display.Show();
            Thread.Sleep(1000);

            display.DrawFilledRectangle(10, 100, 108, 50, Color.Yellow);
            display.Show();
            Thread.Sleep(1000);

            DrawBitmap(bytes, tft);
            tft.Refresh();
            Thread.Sleep(1000);

            display.CurrentFont = new Font8x12();
            display.DrawText(4, 10, "abgjkyz", Color.SkyBlue);
            display.Show();
            Thread.Sleep(Timeout.Infinite);
        }

        static void DrawBitmap(byte[] bytes, ILI9163 display)
        {
            byte r, g, b;

            int offset = 14 + bytes[14];

            int width = bytes[18];
            int height = bytes[22];

            for(int j = 0; j < height; j++)
            {
                for(int i = 0; i < width; i++)
                {
                    b = bytes[i * 3 + j * width * 3 + offset];
                    g = bytes[i * 3 + j * width * 3 + offset + 1];
                    r = bytes[i * 3 + j * width * 3 + offset + 2];

                    display.DrawPixel(20 + i, 145 - j, r, g, b);
                }
            }
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