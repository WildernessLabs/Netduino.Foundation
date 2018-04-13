using System;
using System.Collections;
using Microsoft.SPOT;

namespace Netduino.Foundation.Displays.TextDisplayMenu
{
    public class MenuItemBase : IMenuItem
    {
        public MenuPage SubMenu
        { get; set; } = new MenuPage();

        //public bool IsSelected { get; set; } = false;

        public string Text { get; set; } = "";

        public string Command { get; set; } = "";

        public MenuItemBase(string displayText, string command)
        {
            Text = displayText;
            Command = command ?? string.Empty;
        }
    }
}
