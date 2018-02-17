using System;
using System.Collections;
using Microsoft.SPOT;

namespace Netduino.Foundation.Displays.TextDisplayMenu
{
    public interface IMenuItem
    {
        MenuPage SubMenu { get; set; }

        //bool IsSelected { get; set; }

        string Text { get; set; }
    }
}
