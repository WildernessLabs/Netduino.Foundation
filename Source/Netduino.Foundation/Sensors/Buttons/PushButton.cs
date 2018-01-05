using System;
using Microsoft.SPOT;

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

		public Microsoft.SPOT.Hardware.InputPort DigitalInput { get; private set; }

		public event EventHandler Clicked = delegate { };

		DateTime clickTime;

		public PushButton(Microsoft.SPOT.Hardware.InputPort input) 
		{
            DigitalInput = input;
			DebounceDuration = TimeSpan.FromTicks (500 * 10000);

			clickTime = DateTime.UtcNow;

            DigitalInput.OnInterrupt += DigitalInput_OnInterrupt;
		}

        //TODO: Wire this up
        private void DigitalInput_OnInterrupt(uint data1, uint data2, DateTime time)
        {
            throw new NotImplementedException();
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
