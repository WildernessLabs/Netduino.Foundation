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
        DateTime _lastClicked = DateTime.MinValue;

		/// <summary>
		/// This duration controls the debounce filter. It also has the effect
		/// of rate limiting clicks. Decrease this time to allow users to click
		/// more quickly.
		/// </summary>
		public TimeSpan DebounceDuration { get; set; }

        public H.InterruptPort DigitalIn { get; private set; }

        public event EventHandler PressStarted = delegate { };
        public event EventHandler PressEnded = delegate { };
		public event EventHandler Clicked = delegate { };

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputPin"></param>
        /// <param name="type"></param>
        /// <param name="debounceDuration">in milliseconds</param>
		public PushButton(H.Cpu.Pin inputPin, CircuitTerminationType type, int debounceDuration = 20) 
		{
            this.DebounceDuration = new TimeSpan(0, 0, 0, 0, debounceDuration);

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
            // check how much time has elapsed since the last click
            var timeSinceLast = time - _lastClicked;
            if (timeSinceLast <= DebounceDuration)
            {
                //
                return;
            }
            this._lastClicked = time;


            // 0 is press, 1 is release
            switch (state) {
                case 0:
                    this.OnPressStarted();
                    break;
                case 1:
                    this.OnPressEnded();
                    this.OnClicked();
                    break;
            }
        }

		protected virtual void OnClicked ()
		{
			this.Clicked (this, EventArgs.Empty);
		}

        protected virtual void OnPressStarted()
        {
            this.PressStarted(this, new EventArgs());
        }

        protected virtual void OnPressEnded()
        {
            this.PressEnded(this, new EventArgs());
        }
	}
}
