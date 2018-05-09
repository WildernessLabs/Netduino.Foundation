using System;
using System.Collections;
using System.Threading;
using Microsoft.SPOT;
using H = Microsoft.SPOT.Hardware;
using N = SecretLabs.NETMF.Hardware.Netduino;
using Netduino.Foundation.Sensors.Rotary;
using Netduino.Foundation.Displays;
using System.IO;
using TextDisplayMenu_Sample.Properties;
using Netduino.Foundation.Displays.TextDisplayMenu;
using Netduino.Foundation.Displays.LCD;
using Netduino.Foundation.ICs.IOExpanders.MCP23008;

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
        //MCP23008 _mcp = null;

        Menu _menu = null;

        public App()
        {
            _encoder = new RotaryEncoderWithButton(
                N.Pins.GPIO_PIN_D7, N.Pins.GPIO_PIN_D6, N.Pins.GPIO_PIN_D5,
                Netduino.Foundation.CircuitTerminationType.CommonGround);
            //_display = new SerialLCD(new TextDisplayConfig() { Height = 4, Width = 20 }) as ITextDisplay;
            //_mcp = new MCP23008();
            //_display = new Lcd2004(_mcp);
            _display = new Lcd2004(N.Pins.GPIO_PIN_D13, N.Pins.GPIO_PIN_D12, N.Pins.GPIO_PIN_D11, N.Pins.GPIO_PIN_D10, N.Pins.GPIO_PIN_D9, N.Pins.GPIO_PIN_D8);

            // set display brightness
            _display.SetBrightness();

            _menu = new Menu(_display, _encoder, Resources.GetBytes(Resources.BinaryResources.menu));
            _menu.Selected += HandleMenuSelected;
            _menu.ValueChanged += HandleMenuValueChanged;
        }

        private void HandleMenuValueChanged(object sender, ValueChangedEventArgs e)
        {
            Debug.Print(e.ItemID + " changed with value: " + e.Value);
            if (e.ItemID == "age")
            {
                _menu.UpdateItemValue("displayAge", e.Value);
            }
            else if (e.ItemID == "temp")
            {
                // le sigh, doubles... make the display look nice
                var value = e.Value.ToString();
                _menu.UpdateItemValue("displayTemp", value.Substring(0, value.IndexOf('.') + 3));
            }
        }

        private void HandleMenuSelected(object sender, MenuSelectedEventArgs e)
        {
            Debug.Print("menu selected: " + e.Command);
        }
    }
}
