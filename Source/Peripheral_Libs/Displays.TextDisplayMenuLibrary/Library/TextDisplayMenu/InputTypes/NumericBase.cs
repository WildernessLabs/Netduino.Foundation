using System;
using Microsoft.SPOT;
using Netduino.Foundation.Sensors.Rotary;
using System.Text;

namespace Netduino.Foundation.Displays.TextDisplayMenu.InputTypes
{
    public abstract class NumericBase : InputBase
    {
        byte _scale = 0;
        int[] _numberParts;
        
        int _pos = 0;
        int _max = 0;
        int _min = 0;

        public override event ValueChangedHandler ValueChanged;

        public NumericBase(int min, int max, byte scale)
        {
            this._max = max;
            this._min = min;
            this._scale = scale;
        }

        string NumericDisplay
        {
            get
            {
                string value = string.Empty;
                value += _numberParts[0].ToString();

                if(_scale > 0)
                {
                    value += ".";
                    value += InputHelpers.PadLeft(_numberParts[1].ToString(), '0', _scale);
                }
                
                return InputHelpers.PadLeft(value, ' ', _display.DisplayConfig.Width);
            }
        }

        public override void GetInput(string itemID, object currentValue)
        {
            if (!_isInit)
            {
                throw new InvalidOperationException("Init() must be called before getting input.");
            }

            _numberParts = new int[_scale > 0 ? 2 : 1];
            _itemID = itemID;
            _display.Clear();
            _display.WriteLine("Enter " + itemID, 0);
            _display.SetCursorPosition(0, 1);
            _encoder.Clicked += HandleClicked;
            _encoder.Rotated += HandleRotated;
            ParseValue(currentValue);
            RewriteInputLine(NumericDisplay);
        }

        private void HandleRotated(object sender, RotaryTurnedEventArgs e)
        {
            if(e.Direction == RotationDirection.Clockwise)
            {
                if(_pos == 0)
                {
                    if (_numberParts[_pos] < _max) { _numberParts[_pos]++; }
                    else { _numberParts[_pos + 1] = 0;  }
                }
                else
                {
                    if (_numberParts[_pos-1] != _max && _numberParts[_pos] < (InputHelpers.Exp(10, _scale)-1)) { _numberParts[_pos]++; }
                }
            }
            else
            {
                if (_pos == 0)
                {
                    if (_numberParts[_pos] > _min) { _numberParts[_pos]--; }
                }
                else
                {
                    if(_numberParts[_pos] > 0) { _numberParts[_pos]--; }
                }
            }
            RewriteInputLine(NumericDisplay);
        }

        private void HandleClicked(object sender, EventArgs e)
        {
            if (_pos < _numberParts.Length - 1)
            {
                _pos++;
            }
            else
            {
                _encoder.Clicked -= HandleClicked;
                _encoder.Rotated -= HandleRotated;
                ValueChanged(this, new ValueChangedEventArgs(_itemID, double.Parse(NumericDisplay)));
            }
        }

        protected override void ParseValue(object value)
        {
            if (value == null || value.ToString() == string.Empty) return;

            string currentValue = value.ToString();

            if (currentValue.IndexOf('.') > 0)
            {
                var parts = currentValue.Split(new char[] { '.' });
                _numberParts[0] = int.Parse(parts[0]);
                _numberParts[1] = int.Parse(parts[1].Substring(0, _scale));
            }
            else
            {
                _numberParts[0] = int.Parse(currentValue);
            }
        }
    }
}
