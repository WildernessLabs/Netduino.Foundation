using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using System.Threading;
using Netduino.Foundation.Displays;
using SecretLabs.NETMF.Hardware.Netduino;
using Netduino.Foundation;
using TFTSPISample.Properties;
using Netduino.Foundation.Sensors.Buttons;
using Netduino.Foundation.LEDs;

namespace TFTSPISample
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
            //DitherTest(tft);
            Thread.Sleep(-1);
        }

        private static void Button_Clicked(object sender, EventArgs e)
        {
            led.IsOn = !led.IsOn;
            display.DrawText(4, 145, ("LED is: " + (led.IsOn ? "On " : "Off")));
            display.Show();
        }

        static void UITest()
        {
              display.CurrentFont = new Font8x12();

              display.DrawText(4, 4, "abcdefghijklm", Color.SkyBlue);
              display.DrawText(4, 18, "nopqrstuvwxyz", Color.SkyBlue);
              display.DrawText(4, 32, "`1234567890-=", Color.SkyBlue);
              display.DrawText(4, 46, "~!@#$%^&*()_+", Color.SkyBlue);
              display.DrawText(4, 60, "[]\\;',./", Color.SkyBlue);
              display.DrawText(4, 74, "{}|:\"<>?", Color.SkyBlue);
              display.DrawText(4, 88, "ABCDEFGHIJKLM", Color.SkyBlue);
              display.DrawText(4, 102, "NOPQRSTUVWXYZ", Color.SkyBlue);

              display.CurrentFont = new Font4x8();
              display.DrawText(4, 116, "ABCDEFGHIJKLMNOPQRSTUVWXYZ", Color.White);
              display.DrawText(4, 126, "abcdefghijklmnopqrstuvwxyz", Color.White);
              display.DrawText(4, 136, "01234567890!@#$%^&*()_+-=", Color.White);
              display.DrawText(4, 146, "\\|;:'\",<.>/?[]{}", Color.White);
              display.Show();
              Thread.Sleep(20000);

              display.Clear();

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

            var bytes = Resources.GetBytes(Resources.BinaryResources.trees);

            DrawBitmap(10, 120, bytes, tft);
            tft.Refresh();
        }

        static void DitherTest(ILI9163 tft)
        {
            var bytes = Resources.GetBytes(Resources.BinaryResources.meadow);
            int width = bytes[18];
            int height = bytes[22];
            DrawBitmap(5, 65, bytes, tft);

            bytes = NFBitmap.Get8bppGreyScale(bytes);
            Draw8bppGrayscaleBitmap(5, 110, bytes, width, height, tft);

            bytes = NFBitmap.Dither8bppto1bpp(bytes, width, height, true);
            Draw8bppGrayscaleBitmap(5, 155, bytes, width, height, tft);

            tft.Refresh();
            Thread.Sleep(1000);

            display.CurrentFont = new Font8x12();
            display.DrawText(5, 5, "Dither test", Color.White);
            display.Show();
            Thread.Sleep(Timeout.Infinite);
        }

        //luminosity method
        static byte[] Get8bppGreyScale(byte[] bitmap24bbp)
        {
            int offset = 14 + bitmap24bbp[14];
            int width = bitmap24bbp[18];
            int height = bitmap24bbp[22];

            var dataLength = (bitmap24bbp.Length - offset) / 3;
            var greyScale = new byte[dataLength];

            for (int i = 0; i < dataLength; i++)
            {
                greyScale[i] = (byte)(bitmap24bbp[3 * i + offset] * 7 / 100 +
                                      bitmap24bbp[3 * i + 1 + offset] * 72 / 100 +
                                      bitmap24bbp[3 * i + 2 + offset] * 21 / 100);
            }
            return greyScale;
        }

        static void Draw8bppGrayscaleBitmap(int x, int y, byte[] data, int width, int height, ILI9163 display)
        { 
            byte c;

            //test by drawing to screen
            for (int j = 0; j < height; j++)
            {
                for (int k = 0; k < width; k++)
                {
                    c = data[k + j * width];

                    display.DrawPixel(x + k, y + height - j, c, c, c);
                }
            }
        }

        static void DrawBitmap(int x, int y, byte[] data, ILI9163 display)
        {
            byte r, g, b;

            int offset = 14 + data[14];

            int width = data[18];
            int height = data[22];

            for(int j = 0; j < height; j++)
            {
                for(int i = 0; i < width; i++)
                {
                    b = data[i * 3 + j * width * 3 + offset];
                    g = data[i * 3 + j * width * 3 + offset + 1];
                    r = data[i * 3 + j * width * 3 + offset + 2];

                    display.DrawPixel(x + i, y + height - j, r, g, b);
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