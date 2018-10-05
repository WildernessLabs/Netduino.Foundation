using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using System.Threading;
using SecretLabs.NETMF.Hardware.Netduino;
using Netduino.Foundation;
using Netduino.Foundation.Displays;
using Netduino.Foundation.Sensors.Buttons;
using Netduino.Foundation.LEDs;

namespace WaveShareEPaper
{
    public class Program
    {
        static EPDColorBase ePaper;
        static GraphicsLibrary display;
        static PushButton button;
        static Led led;

        public static void Main()
        {
            ePaper = new EPD2i13b(chipSelectPin: Pins.GPIO_PIN_D4,
                dcPin: Pins.GPIO_PIN_D7,
                resetPin: Pins.GPIO_PIN_D6,
                busyPin: Pins.GPIO_PIN_D5,
                spiModule: SPI.SPI_module.SPI1,
                speedKHz: 4000);
            
       //     ePaper.Clear(false, true);

            display = new GraphicsLibrary(ePaper);

            display.DrawLine(10, 10, 94, 202);
         //   display.Show();
         //   Thread.Sleep(500);

            display.DrawCircle(52, 60, 32);
          //  display.Show();
         //   Thread.Sleep(500);

            display.DrawRectangle(5, 40, 60, 60);
          //  display.Show();
          //  Thread.Sleep(1000);

            display.DrawFilledRectangle(20, 100, 40, 25, Color.Red);
         //   display.Show();
        //    Thread.Sleep(500);

            display.CurrentFont = new Font8x12();
            display.DrawText(2, 2, "Wilderness");
            display.CurrentFont = new Font8x8();
            display.DrawText(2, 16, "Netduino 3", Color.Red);

            display.Show();

            // tft.ClearScreen(31);
            //    ePaper.Refresh();

            /*  display = new GraphicsLibrary(tft);

              led = new Led(Pins.ONBOARD_LED);
              button = new PushButton(Pins.ONBOARD_BTN, CircuitTerminationType.CommonGround);
              button.Clicked += Button_Clicked;

              UITest();
              Thread.Sleep(-1);*/
        }

        private static void Button_Clicked(object sender, EventArgs e)
        {
            led.IsOn = !led.IsOn;
            display.DrawText(4, 145, ("LED is: " + (led.IsOn ? "On ":"Off")));
            display.Show();
        }

        static void UITest()
        {
            display.Clear();

            display.DrawLine(10, 10, 118, 150, Color.OrangeRed);
            display.Show();
       //     Thread.Sleep(500);
            display.DrawLine(118, 10, 10, 150, Color.OrangeRed);
            display.Show();
         //   Thread.Sleep(500);

            display.DrawCircle(64, 64, 25, Color.Purple);
            display.Show();
        //    Thread.Sleep(1000);

            display.DrawRectangle(5, 5, 118, 150, Color.Aquamarine);
            display.Show();
       //     Thread.Sleep(1000);

            display.DrawFilledRectangle(10, 125, 108, 25, Color.Yellow);
            display.Show();
        //    Thread.Sleep(1000);

            display.CurrentFont = new Font8x8();
            display.DrawText(4, 10, "NETDUINO 3 WiFi", Color.SkyBlue);
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