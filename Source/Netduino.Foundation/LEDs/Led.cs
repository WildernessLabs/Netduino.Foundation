using System;
using Netduino.Foundation.GPIO;
using H = Microsoft.SPOT.Hardware;
using System.Threading;

namespace Netduino.Foundation.LEDs
{
    public class Led
    {
        public IDigitalOutputPort DigitalOut { get; protected set; }

        public bool IsOn
        {
            get { return _isOn; }
            set
            {
                // if turning on,
                if (value)
                {
                    //this.DigitalOut.Write(_onValue); // turn on
                    DigitalOut.State = _onValue; // turn on
                }
                else
                { // if turning off
                    //this.DigitalOut.Write(!_onValue); // turn off
                    DigitalOut.State = !_onValue; // turn off
                }
                this._isOn = value;
            }
        }
        protected bool _isOn = false;
        protected bool _onValue = true;

        protected Thread _animationThread = null;

        /// <summary>
        /// Creates a LED through a DigitalOutPutPort from an IO Expander
        /// </summary>
        /// <param name="port"></param>
        public Led(IDigitalOutputPort port)
        {
            DigitalOut = port;
        }

        /// <summary>
        /// Creates a LED through a pin directly from the Digital IO of the Netduino
        /// </summary>
        /// <param name="pin"></param>
        public Led(H.Cpu.Pin pin)
        {
            IDigitalOutputPort shim = GPIO.SPOT.DigitalOutputPort.FromPin(pin, !_onValue);
        }

        /// <summary>
        /// Blink animation that turns the LED on and off based on the OnDuration and offDuration values in ms
        /// </summary>
        /// <param name="onDuration"></param>
        /// <param name="offDuration"></param>
        public void StartBlink(uint onDuration = 200, uint offDuration = 200)
        {
            _isOn = false;
            _animationThread = new Thread(() => 
            {
                while (true)
                {
                    _isOn = true;
                    Thread.Sleep((int)onDuration);
                    _isOn = false;
                    Thread.Sleep((int)offDuration);
                }
            });
            _animationThread.Start();
        }

        /// <summary>
        /// Stops blink animation.
        /// </summary>
        public void Stop()
        {
            if (_animationThread != null)
            {
                _animationThread.Abort();
                _isOn = false;
            }
        }
    }
}