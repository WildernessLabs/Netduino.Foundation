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

            _menu = new Menu(_display, _encoder, Resources.GetBytes(Resources.BinaryResources.menu));
            _menu.Selected += HandleMenuSelected;
            _menu.ValueChanged += HandleMenuValueChanged;
        }

        private void HandleMenuValueChanged(object sender, ValueChangedEventArgs e)
        {
            Debug.Print(e.ItemID + " changed with value: " + e.Value);
        }

        private void HandleMenuSelected(object sender, MenuSelectedEventArgs e)
        {
            Debug.Print("menu selected: " + e.Command);
        }
    }
}
