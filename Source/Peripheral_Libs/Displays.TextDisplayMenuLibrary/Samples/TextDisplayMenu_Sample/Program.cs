using System;
using System.Collections;
using System.Threading;
using Microsoft.SPOT;
using H = Microsoft.SPOT.Hardware;
using N = SecretLabs.NETMF.Hardware.Netduino;
using Netduino.Foundation.Sensors.Rotary;
using Netduino.Foundation.Displays;
using Netduino.Foundation.Displays.TextDisplayMenu;

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

            MenuPage menuTree = CreateMenuTree();

            _menu = new Menu(_display, menuTree);

            _encoder.Rotated += HandlEncoderRotation;

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

        protected MenuPage CreateMenuTree()
        {
            MenuPage menuTree = new MenuPage();
            menuTree.MenuItems.Add(new MenuItemBase("Menu Item 1"));
            menuTree.MenuItems.Add(new MenuItemBase("Item the 2nd"));
            menuTree.MenuItems.Add(new MenuItemBase("3rd menu item") {
                SubMenu = new MenuPage() { MenuItems =
                    {
                        new MenuItemBase ("I'm first!"),
                        new MenuItemBase ("I'm second."),
                        new MenuItemBase ("where the party at?")
                    }
                }
            });
            menuTree.MenuItems.Add(new MenuItemBase("Item 4"));
            menuTree.MenuItems.Add(new MenuItemBase("Item 5"));
            menuTree.MenuItems.Add(new MenuItemBase("Item 6"));

            return menuTree;
        }
    }
}
