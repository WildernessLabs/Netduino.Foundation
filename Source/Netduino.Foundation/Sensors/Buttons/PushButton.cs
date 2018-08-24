using System;
using Microsoft.SPOT;
using H = Microsoft.SPOT.Hardware;

namespace Netduino.Foundation.Sensors.Buttons
{
	/// <summary>
	/// A simple push button. 
	/// </summary>
	public class PushButton : IButton
	{
        protected DateTime _lastClicked = DateTime.MinValue;
        protected DateTime _buttonPressStart = DateTime.MaxValue;
        protected CircuitTerminationType _circuitType;

        /// <summary>
        /// This duration controls the debounce filter. It also has the effect
        /// of rate limiting clicks. Decrease this time to allow users to click
        /// more quickly.
        /// </summary>
        public TimeSpan DebounceDuration { get; set; }

        /// <summary>
        /// Returns the current raw state of the switch. If the switch 
        /// is pressed (connected), returns true, otherwise false.
        /// </summary>
        public bool State
        {
            get
            {
                if (DigitalIn != null)
                {
                    // have to invert, closed is 0, open is 1. SMH
                    return !DigitalIn.Read();
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// The minimum duration for a long press.
        /// </summary>
        public TimeSpan LongPressThreshold { get; set; } = new TimeSpan(0, 0, 0, 0, 500);

        public H.InterruptPort DigitalIn { get; private set; }

        public event EventHandler PressStarted = delegate { };
        public event EventHandler PressEnded = delegate { };
		public event EventHandler Clicked = delegate { };
        public event EventHandler LongPressClicked = delegate { };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputPin"></param>
        /// <param name="type"></param>
        /// <param name="debounceDuration">in milliseconds</param>
		public PushButton(H.Cpu.Pin inputPin, CircuitTerminationType type, int debounceDuration = 20) 
		{
            _circuitType = type;
            DebounceDuration = new TimeSpan(0, 0, 0, 0, debounceDuration);

            // if we terminate in ground, we need to pull the port high to test for circuit completion, otherwise down.
            H.Port.ResistorMode resistorMode = H.Port.ResistorMode.Disabled;
            switch (type)
            {
                case CircuitTerminationType.CommonGround:
                    resistorMode = H.Port.ResistorMode.PullUp;
                    break;
                case CircuitTerminationType.High:
                    resistorMode = H.Port.ResistorMode.PullDown;
                    break;
                case CircuitTerminationType.Floating:
                    resistorMode = H.Port.ResistorMode.Disabled;
                    break;
            }

            // create the interrupt port from the pin and resistor type
            this.DigitalIn = new H.InterruptPort(inputPin, true, resistorMode, H.Port.InterruptMode.InterruptEdgeBoth);

            // wire up the interrupt handler
            this.DigitalIn.OnInterrupt += DigitalIn_OnInterrupt;
		}

        private void DigitalIn_OnInterrupt(uint port, uint state, DateTime time)
        {
            uint STATE_PRESSED = 0;
            uint STATE_RELEASED = 1;

            // check how much time has elapsed since the last click
            var timeSinceLast = time - _lastClicked;
            if (timeSinceLast <= DebounceDuration)
            {
                //
                return;
            }
            _lastClicked = time;

            if(_circuitType == CircuitTerminationType.High)
            {
                STATE_PRESSED = 1;
                STATE_RELEASED = 0;
            }

            if(state == STATE_PRESSED)
            {
                // save our press start time (for long press event)
                _buttonPressStart = DateTime.Now;
                // raise our event in an inheritance friendly way
                this.RaisePressStarted();
            }
            else if(state == STATE_RELEASED)
            {
                // calculate the press duration
                TimeSpan pressDuration = DateTime.Now - _buttonPressStart;

                // reset press start time
                _buttonPressStart = DateTime.MaxValue;

                // if it's a long press, raise our long press event
                if (pressDuration > LongPressThreshold) this.RaiseLongPress();

                // raise the other events
                this.RaisePressEnded();
                this.RaiseClicked();
            }
        }

		protected virtual void RaiseClicked ()
		{
			this.Clicked (this, EventArgs.Empty);
		}

        protected virtual void RaisePressStarted()
        {
            // raise the press started event
            this.PressStarted(this, new EventArgs());
        }

        protected virtual void RaisePressEnded()
        {
            this.PressEnded(this, new EventArgs());
        }

        protected virtual void RaiseLongPress()
        {
            this.LongPressClicked(this, new EventArgs());
        }
	}
}
