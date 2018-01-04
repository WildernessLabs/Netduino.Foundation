using System;
using Microsoft.SPOT;
using H = Microsoft.SPOT.Hardware;

namespace Netduino.Foundation.Relays
{
    public class Relay
    {
        public H.OutputPort DigitalOut { get; protected set; }

        public RelayType Type { get; protected set; }

        public bool IsOn {
            get { return _isOn; }
            set
            {
                // if turning on,
                if (value) {
                    this.DigitalOut.Write(_onValue); // turn on
                } else { // if turning off
                    this.DigitalOut.Write(!_onValue); // turn off
                }
                this._isOn = value;
            }
        } protected bool _isOn = false;
        protected bool _onValue = true;

        public Relay(H.Cpu.Pin pin, RelayType type = RelayType.NormallyOpen)
        {
            // if it's normally closed, we have to invert the "on" value
            this.Type = type;
            if (this.Type == RelayType.NormallyClosed) {
                _onValue = false;
            }

            // initialize the pin as whatever off is.
            this.DigitalOut = new Microsoft.SPOT.Hardware.OutputPort(pin, !_onValue);
        }

        public void Toggle()
        {
            this.IsOn = !this.IsOn;
        }
        
    }

    public enum RelayType
    {
        NormallyOpen,
        NormallyClosed
    }
}
