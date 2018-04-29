using System;
using Microsoft.SPOT;
using Netduino.Foundation.Sensors.Rotary;
using static Netduino.Foundation.Displays.TextDisplayMenu.InputTypes.Time;

namespace Netduino.Foundation.Displays.TextDisplayMenu
{
    public interface IMenuInputItem
    {
        void Init(ITextDisplay display, RotaryEncoderWithButton encoder);
        void GetInput(string itemID, object currentValue);
        event ValueChangedHandler ValueChanged;
    }
}
