using Microsoft.SPOT;
using Netduino.Foundation.Audio.Radio;
using Netduino.Foundation.Displays;
using Netduino.Foundation.Sensors.Buttons;
using SecretLabs.NETMF.Hardware.Netduino;
using Microsoft.SPOT.Hardware;
using System.Threading;

namespace TEA5767DisplaySample
{
    public class Program
    {
        static PushButton button;
        static TEA5767 radio;
        static GraphicsLibrary display;

        public static void Main()
        {
            Debug.Print("Simple Radio");

            Init();

            Thread.Sleep(-1);
        }

        static void Init()
        {
            button = new PushButton(Pins.ONBOARD_BTN, Netduino.Foundation.CircuitTerminationType.CommonGround);
            button.Clicked += OnButtonClicked;

            radio = new TEA5767();
            Thread.Sleep(200);
            radio.SetFrequency(94.5f);

            var lcd = new ST7565(chipSelectPin: Pins.GPIO_PIN_D10,
                dcPin: Pins.GPIO_PIN_D8,
                resetPin: Pins.GPIO_PIN_D9,
                spiModule: SPI.SPI_module.SPI1,
                speedKHz: 10000, 
                width: 129,
                height: 65
                );

            lcd.SetContrast(0);

            display = new GraphicsLibrary(lcd);
            display.CurrentFont = new Font8x12();
            
            UpdateDisplay();
        }

        static void OnButtonClicked(object sender, EventArgs e)
        {
            radio.SearchNext();
            UpdateDisplay();
        }

        static void UpdateDisplay ()
        {
            display.Clear();

            display.DrawRectangle(0, 0, 128, 64, true, false);

            display.DrawText(4, 4, "C# Radio");
            display.DrawText(4, 16, radio.GetFrequency().ToString("N1") + " MHz");
            display.DrawText(4, 28, radio.GetSignalLevel() + " db");
            display.DrawText(4, 40, radio.IsStereo() ? "Stereo" : "Mono");

            display.Show();
        }
    }
}
