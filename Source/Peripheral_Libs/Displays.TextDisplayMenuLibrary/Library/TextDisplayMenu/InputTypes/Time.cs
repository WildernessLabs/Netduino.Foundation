using System;
using Microsoft.SPOT;
using Netduino.Foundation.Sensors.Rotary;
using System.Threading;

namespace Netduino.Foundation.Displays.TextDisplayMenu.InputTypes
{
    public class Time : IMenuInputItem
    {
        RotaryEncoderWithButton _encoder;
        ITextDisplay _display = null;
        int[] _timeParts;
        byte _pos = 0;
        string _itemID;
        bool _isInit;

        protected TimeMode _timeMode;

        public event ValueChangedHandler ValueChanged;

        public Time()
        {
            this._timeMode = TimeMode.HH_MM;
        }

        string TimeDisplay
        {
            get
            {
                string value = string.Empty;
                for (int i = 0; i < _timeParts.Length; i++)
                {
                    if (i > 0) value += ":";
                    value += PadLeft(_timeParts[i].ToString(), '0', 2);
                }
                return value;
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
        /// Initialize the input type
        /// </summary>
        /// <param name="display"></param>
        /// <param name="encoder"></param>
        public void Init(ITextDisplay display, RotaryEncoderWithButton encoder)
        {
            if (_timeMode == TimeMode.HH_MM_SS)
            {
                _timeParts = new int[3];
            }
            else
            {
                _timeParts = new int[2];
            }

            _display = display;
            _encoder = encoder;
            _isInit = true;
        }

        /// <summary>
        /// Get input from user
        /// </summary>
        /// <param name="itemID">id of the menu item</param>
        /// <param name="currentValue">current value of the menu item</param>
        public void GetInput(string itemID, object currentValue)
        {
            if (!_isInit)
            {
                throw new InvalidOperationException("Init() must be called before getting input.");
            }

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
            WriteTimeDisplay();
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

            WriteTimeDisplay();
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

        private void ParseValue(string text)
        {
            var parts = text.Split(new char[] { ':' });

            for (int i = 0; i < parts.Length; i++)
            {
                _timeParts[i] = int.Parse(parts[i]);
            }
        }

        private void WriteTimeDisplay()
        {
            _display.Write(TimeDisplay);
            _display.SetCursorPosition(0, 1);
        }

        private string PadLeft(string text, char filler, int size)
        {
            string padded = string.Empty;
            for (int i = text.Length; i < size; i++)
            {
                padded += filler;
            }
            return padded + text;
        }

    }
}
