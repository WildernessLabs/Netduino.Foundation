using System;
using System.Collections;
using Microsoft.SPOT;

namespace Netduino.Foundation.Displays.TextDisplayMenu
{
    public class MenuItemBase : IMenuItem
    {
        public MenuPage Children
        { get; set; } = new MenuPage();

        //public bool IsSelected { get; set; } = false;

        public string Text { get; set; } = "";

        public MenuItemBase(string displayText)
        {
            Text = displayText;
        }
    }
}
