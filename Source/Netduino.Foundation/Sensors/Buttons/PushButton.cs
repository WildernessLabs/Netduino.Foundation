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
		/// <summary>
		/// This duration controls the debounce filter. It also has the effect
		/// of rate limiting clicks. Decrease this time to allow users to click
		/// more quickly.
		/// </summary>
		public TimeSpan DebounceDuration { get; set; }

        public H.InterruptPort DigitalIn { get; private set; }

		public event EventHandler Clicked = delegate { };

		DateTime clickTime;

		public PushButton(H.Cpu.Pin inputPin, CircuitTerminationType type) 
		{
            // if we terminate in ground, we need to pull the port high to test for circuit completion, otherwise down.
            var resistorMode = (type == CircuitTerminationType.CommonGround) ? H.Port.ResistorMode.PullUp : H.Port.ResistorMode.PullDown;

            // create the interrupt port from the pin and resistor type
            this.DigitalIn = new H.InterruptPort(inputPin, true, resistorMode, H.Port.InterruptMode.InterruptEdgeLow);

            // wire up the interrupt handler
            this.DigitalIn.OnInterrupt += DigitalIn_OnInterrupt;

		}

        private void DigitalIn_OnInterrupt(uint port, uint state, DateTime time)
        {
            Debug.Print("Pin=" + port + " State=" + state + " Time=" + time);
        }

        void HandleValueChanged (object sender, EventArgs e)
		{
			var time = DateTime.UtcNow;

			var sinceClick = (time - clickTime);

			if (sinceClick >= DebounceDuration) {
				clickTime = time;
				OnClicked ();
			}
		}

		protected virtual void OnClicked ()
		{
			this.Clicked (this, EventArgs.Empty);
		}
	}
}
