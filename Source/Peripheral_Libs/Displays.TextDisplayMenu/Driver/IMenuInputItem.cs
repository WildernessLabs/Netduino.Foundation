using System;
using Microsoft.SPOT;
using Netduino.Foundation.Sensors.Rotary;
using static Netduino.Foundation.Displays.TextDisplayMenu.InputTypes.Time;
using Netduino.Foundation.Sensors.Buttons;

namespace Netduino.Foundation.Displays.TextDisplayMenu
{
    public interface IMenuInputItem
    {
        void Init(ITextDisplay display, IRotaryEncoder encoder, IButton buttonSelect);
        void Init(ITextDisplay display, IButton buttonNext, IButton buttonPrevious, IButton buttonSelect);
        void GetInput(string itemID, object currentValue);
        event ValueChangedHandler ValueChanged;
    }
}
