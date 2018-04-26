using System;
using Microsoft.SPOT;
using Netduino.Foundation.Sensors.Rotary;

namespace Netduino.Foundation.Displays.TextDisplayMenu.InputTypes
{
    public abstract class TimeBase : InputBase
    {
        int[] _timeParts;
        byte _pos = 0;

        protected TimeMode _timeMode;

        public override event ValueChangedHandler ValueChanged;

        public TimeBase(TimeMode timeMode)
        {
            this._timeMode = timeMode;
        }

        string TimeDisplay
        {
            get
            {
                string value = string.Empty;
                for (int i = 0; i < _timeParts.Length; i++)
                {
                    if (i > 0) value += ":";
                    value += InputHelpers.PadLeft(_timeParts[i].ToString(), '0', 2);
                }
                return InputHelpers.PadLeft(value, ' ', _display.DisplayConfig.Width);
            }
        }

        string TimeModeDisplay
        {
            get
            {
                switch (_timeMode)
                {
                    case TimeMode.HH_MM_SS: return "hh:mm:ss";
                    case TimeMode.HH_MM: return "hh:mm";
                    case TimeMode.MM_SS: return "mm:ss";
                    default: throw new ArgumentException();
                }
            }
        }

        /// <summary>
        /// Time mode for the input, HH:MM:SS, HH:MM, or MM:SS
        /// </summary>
        public enum TimeMode
        {
            HH_MM_SS,
            HH_MM,
            MM_SS
        }

        /// <summary>
        /// Get input from user
        /// </summary>
        /// <param name="itemID">id of the menu item</param>
        /// <param name="currentValue">current value of the menu item</param>
        public override void GetInput(string itemID, object currentValue)
        {
            if (!_isInit)
            {
                throw new InvalidOperationException("Init() must be called before getting input.");
            }

            _timeParts = new int[_timeMode == TimeMode.HH_MM_SS ? 3 : 2];
            _itemID = itemID;
            _display.Clear();
            _display.WriteLine("Enter " + this.TimeModeDisplay, 0);
            _display.SetCursorPosition(0, 1);
            _encoder.Clicked += HandleClicked;
            _encoder.Rotated += HandleRotated;

            if (currentValue != null)
            {
                ParseValue(currentValue.ToString());
            }
            RewriteInputLine(TimeDisplay);
        }

        private void HandleRotated(object sender, RotaryTurnedEventArgs e)
        {
            int min = 0, max = 0;

            if (_pos == 0)
            {
                if (_timeMode == TimeMode.HH_MM_SS) max = 23;
                if (_timeMode == TimeMode.HH_MM) max = 23;
                if (_timeMode == TimeMode.MM_SS) max = 59;
            }
            else
            {
                max = 59;
            }

            if (e.Direction == RotationDirection.Clockwise)
            {
                if (_timeParts[_pos] < max) _timeParts[_pos]++;
            }
            else
            {
                if (_timeParts[_pos] > min) _timeParts[_pos]--;
            }

            RewriteInputLine(TimeDisplay);
        }

        private void HandleClicked(object sender, EventArgs e)
        {
            if (_pos < _timeParts.Length - 1)
            {
                _pos++;
            }
            else
            {
                _encoder.Clicked -= HandleClicked;
                _encoder.Rotated -= HandleRotated;
                ValueChanged(this, new ValueChangedEventArgs(_itemID, TimeDisplay));
            }
        }

        protected override void ParseValue(string text)
        {
            var parts = text.Split(new char[] { ':' });

            for (int i = 0; i < parts.Length; i++)
            {
                _timeParts[i] = int.Parse(parts[i]);
            }
        }
    }
}
