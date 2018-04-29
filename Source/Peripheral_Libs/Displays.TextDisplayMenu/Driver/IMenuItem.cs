using System;
using System.Collections;
using Microsoft.SPOT;

namespace Netduino.Foundation.Displays.TextDisplayMenu
{
    public interface IMenuItem
    {
        MenuPage SubMenu { get; set; }

        string Text { get; set; }

        object Value { get; set; }
    }
}
