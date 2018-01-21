using System;
using Microsoft.SPOT;
using H = Microsoft.SPOT.Hardware;
using Netduino.Foundation.Sensors.Buttons;

namespace Netduino.Foundation.Sensors.Rotary
{
    public class RotaryEncoderWithButton : RotaryEncoder, IButton
    {
        public event EventHandler PressStarted = delegate { };
        public event EventHandler PressEnded = delegate { };
        public event EventHandler Clicked = delegate { };

        public PushButton Button
        {
            get { return this._button; }
        } readonly PushButton _button;

        public RotaryEncoderWithButton(H.Cpu.Pin aPhasePin, H.Cpu.Pin bPhasePin, 
            H.Cpu.Pin buttonPin, CircuitTerminationType buttonCircuitTerminationType, int debounceDuration = 20) : base(aPhasePin, bPhasePin)
        {
            this._button = new PushButton(buttonPin, buttonCircuitTerminationType, debounceDuration);

            this._button.Clicked += Button_Clicked;
            this._button.PressStarted += Button_PressStarted;
            this._button.PressEnded += Button_PressEnded;
        }

        protected void Button_PressEnded(object sender, EventArgs e)
        {
            this.PressEnded(this, e);
        }

        protected void Button_PressStarted(object sender, EventArgs e)
        {
            this.PressStarted(this, e);
        }

        protected void Button_Clicked(object sender, EventArgs e)
        {
            this.Clicked(this, e);
        }
    }
}
