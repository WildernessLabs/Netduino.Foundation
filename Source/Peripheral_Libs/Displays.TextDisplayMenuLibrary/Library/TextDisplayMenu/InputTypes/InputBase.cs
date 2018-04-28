using System;
using Microsoft.SPOT;
using Netduino.Foundation.Sensors.Rotary;

namespace Netduino.Foundation.Displays.TextDisplayMenu.InputTypes
{
    public abstract class InputBase : IMenuInputItem
    {
        protected RotaryEncoderWithButton _encoder;
        protected ITextDisplay _display = null;
        protected bool _isInit;
        protected string _itemID;

        public abstract event ValueChangedHandler ValueChanged;

        public abstract void GetInput(string itemID, object currentValue);
        protected abstract void ParseValue(object value);

        public void Init(ITextDisplay display, RotaryEncoderWithButton encoder)
        {
            _display = display;
            _encoder = encoder;
            _isInit = true;
        }

        protected void RewriteInputLine(string text)
        {
            _display.Write(text);
            _display.SetCursorPosition(0, 1);
        }
    }
}
