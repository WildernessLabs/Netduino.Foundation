using System;
using Netduino.Foundation.GPIO;
using H = Microsoft.SPOT.Hardware;
using System.Threading;

namespace Netduino.Foundation.LEDs
{
    public class Led
    {
        public IDigitalOutputPort DigitalOut { get; protected set; }

        protected bool _isOn = false;

        protected H.OutputPort _outPutPort;

        protected Thread _animationThread = null;

        /// <summary>
        /// Creates a LED through a pin directly from the Digital IO of the Netduino
        /// </summary>
        /// <param name="port"></param>
        public Led(H.Cpu.Pin port)
        {
            _outPutPort = new H.OutputPort(port, false);
        }

        /// <summary>
        /// Creates a LED through a DigitalOutPutPort from an IO Expander
        /// </summary>
        /// <param name="port"></param>
        public Led(IDigitalOutputPort port)
        {
            DigitalOut = port;
        }

        /// <summary>
        /// Powers on the LED
        /// </summary>
        public void SetState(bool isOn)
        {
            if (_outPutPort != null)
                _outPutPort.Write(isOn);

            if (DigitalOut != null)
                DigitalOut.State = isOn;
        }

        /// <summary>
        /// Blink animation that turns the LED on and off based on the OnDuration and offDuration values in ms
        /// </summary>
        /// <param name="onDuration"></param>
        /// <param name="offDuration"></param>
        public void StartBlink(uint onDuration = 200, uint offDuration = 200)
        {
            SetState(false);
            _animationThread = new Thread(() => 
            {
                while (true)
                {
                    SetState(true);
                    Thread.Sleep((int)onDuration);
                    SetState(false);
                    Thread.Sleep((int)offDuration);
                }
            });
            _animationThread.Start();
        }

        public void Stop()
        {
            if (_animationThread != null)
            {
                _animationThread.Abort();
                SetState(false);
            }
        }
    }
}