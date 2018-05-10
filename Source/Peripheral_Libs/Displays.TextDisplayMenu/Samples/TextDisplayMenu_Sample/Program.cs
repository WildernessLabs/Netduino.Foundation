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
using Netduino.Foundation.Sensors.Buttons;

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
        MCP23008 _mcp = null;

        IButton _next = null;
        IButton _previous = null;
        IButton _select = null;

        Menu _menu = null;

        public App()
        {
            // SETUP CONTROL

            // use encoder with push button
            _encoder = new RotaryEncoderWithButton(
                N.Pins.GPIO_PIN_D7, N.Pins.GPIO_PIN_D6, N.Pins.GPIO_PIN_D5,
                Netduino.Foundation.CircuitTerminationType.CommonGround);

            // use buttons
            //_next = new PushButton(N.Pins.GPIO_PIN_D6, Netduino.Foundation.CircuitTerminationType.CommonGround);
            //_previous = new PushButton(N.Pins.GPIO_PIN_D7, Netduino.Foundation.CircuitTerminationType.CommonGround);
            //_select = new PushButton(N.Pins.GPIO_PIN_D5, Netduino.Foundation.CircuitTerminationType.CommonGround);

            // SETUP DISPLAY

            // GPIO
            _display = new Lcd2004(N.Pins.GPIO_PIN_D13, N.Pins.GPIO_PIN_D12, N.Pins.GPIO_PIN_D11, N.Pins.GPIO_PIN_D10, N.Pins.GPIO_PIN_D9, N.Pins.GPIO_PIN_D8);

            // SERIAL LCD
            //_display = new SerialLCD(new TextDisplayConfig() { Height = 4, Width = 20 }) as ITextDisplay;

            // MCP (i2c)
            //_mcp = new MCP23008();
            //_display = new Lcd2004(_mcp);

            // set display brightness
            _display.SetBrightness();

            // Create menu with encoder
            _menu = new Menu(_display, _encoder, Resources.GetBytes(Resources.BinaryResources.menu), true);

            // Create menu with buttons
            //_menu = new Menu(_display, _next, _previous, _select, Resources.GetBytes(Resources.BinaryResources.menu));

            _menu.Selected += HandleMenuSelected;
            _menu.ValueChanged += HandleMenuValueChanged;

            _menu.Enable();
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
            if(e.Command == "Exit")
            {
                Debug.Print("Exit menu");
            }
        }
    }
}
