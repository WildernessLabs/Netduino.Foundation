using System;
using System.Collections;
using System.Threading;
using Microsoft.SPOT;
using H = Microsoft.SPOT.Hardware;
using N = SecretLabs.NETMF.Hardware.Netduino;
using Netduino.Foundation.Sensors.Rotary;
using Netduino.Foundation.Displays;
using Netduino.Foundation.Displays.TextDisplayMenu;
using System.IO;
using TextDisplayMenu_Sample.Properties;

namespace TextDisplayMenu_Sample
{
    public class Program
    {
        public static void Main()
        {
            App app = new App();
            Thread.Sleep(Timeout.Infinite);
        }
    }

    public class App
    {
        RotaryEncoderWithButton _encoder = null;
        ITextDisplay _display = null;
        Menu _menu = null;

        public App()
        {
            _encoder = new RotaryEncoderWithButton(
                N.Pins.GPIO_PIN_D2, N.Pins.GPIO_PIN_D3, N.Pins.GPIO_PIN_D4,
                Netduino.Foundation.CircuitTerminationType.CommonGround);
            _display = new SerialLCD(new TextDisplayConfig() { Height = 4, Width = 20 }) as ITextDisplay;

            // set display brightness
            _display.SetBrightness();

            var menuJson = new string(System.Text.Encoding.UTF8.GetChars(Resources.GetBytes(Resources.BinaryResources.menu)));
            var menuData = Json.NETMF.JsonSerializer.DeserializeString(menuJson) as Hashtable;

            _menu = new Menu(_display, menuData);

            _encoder.Rotated += HandlEncoderRotation;
            _encoder.Clicked += HandleEncoderClick;

            _menu.Clicked += HandleMenuClicked;
        }

        private void HandleMenuClicked(object sender, MenuClickedEventArgs e)
        {
            Debug.Print(e.Command);
        }

        private void HandleEncoderClick(object sender, EventArgs e)
        {
            _menu.SelectCurrentItem();
        }

        private void HandlEncoderRotation(object sender, RotaryTurnedEventArgs e)
        {
            bool moved = false;
            if (e.Direction == RotationDirection.Clockwise)
            {
                moved = _menu.MoveNext();
            } else
            {
                moved = _menu.MovePrevious();
            }

            if (!moved) {
                // play a sound?
                Debug.Print("end of items");
            }
        }
        
    }
}
