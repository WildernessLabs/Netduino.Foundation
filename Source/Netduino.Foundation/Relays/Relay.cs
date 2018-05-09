using System;
using Microsoft.SPOT;
using H = Microsoft.SPOT.Hardware;
using Netduino.Foundation.GPIO;

namespace Netduino.Foundation.Relays
{
    public class Relay : IRelay
    {
        public IDigitalOutputPort DigitalOut { get; protected set; }

        public RelayType Type { get; protected set; }

        public bool IsOn {
            get { return _isOn; }
            set
            {
                // if turning on,
                if (value) {
                    this.DigitalOut.State = _onValue; // turn on
                } else { // if turning off
                    this.DigitalOut.State = !_onValue; // turn off
                }
                this._isOn = value;
            }
        } protected bool _isOn = false;
        protected bool _onValue = true;

        /// <summary>
        /// Creates a new Relay on an IDigitalOutputPort. Allows you 
        /// to use any peripheral that exposes ports that conform to the
        /// IDigitalOutputPort, such as the MCP23008.
        /// </summary>
        /// <param name="port"></param>
        /// <param name="type"></param>
        public Relay(IDigitalOutputPort port, RelayType type = RelayType.NormallyOpen)
        {
            // if it's normally closed, we have to invert the "on" value
            this.Type = type;
            if (this.Type == RelayType.NormallyClosed)
            {
                _onValue = false;
            }

            DigitalOut = port;
        }

        public Relay(H.Cpu.Pin pin, RelayType type = RelayType.NormallyOpen)
        {
            // if it's normally closed, we have to invert the "on" value
            this.Type = type;
            if (this.Type == RelayType.NormallyClosed) {
                _onValue = false;
            }

            // create a digital output port shim
            DigitalOut = new GPIO.SPOT.DigitalOutputPort(pin, !_onValue);
        }

        public void Toggle()
        {
            this.IsOn = !this.IsOn;
        }
        
    }
}
