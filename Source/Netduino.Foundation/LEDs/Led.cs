using System;
using Microsoft.SPOT;
using Netduino.Foundation.GPIO;
using H = Microsoft.SPOT.Hardware;
using System.Threading;

namespace Netduino.Foundation.LEDs
{
    public class Led
    {
        protected bool _isOn = false;

        protected H.OutputPort _outPutPort;

        protected Thread _animationThread = null;

        public IDigitalOutputPort DigitalOut { get; protected set; }

        public Led() { }

        /// <summary>
        /// Creates and LED through a pin directly from the Digital IO of the Netduino
        /// </summary>
        /// <param name="port"></param>
        public Led(H.Cpu.Pin port)
        {
            _outPutPort = new H.OutputPort(port, false);
        }

        /// <summary>
        /// Creates and LED through a DigitalOutPutPort from an IO Expander
        /// </summary>
        /// <param name="port"></param>
        public Led(IDigitalOutputPort port)
        {
            DigitalOut = port;
        }

        /// <summary>
        /// Powers on the LED
        /// </summary>
        public void TurnOn()
        {
            if (_outPutPort != null)
                _outPutPort.Write(true);

            if (DigitalOut != null)
                DigitalOut.State = true;
        }

        /// <summary>
        /// Blink animation that turns the LED on and off based on the OnDuration and offDuration values
        /// </summary>
        /// <param name="onDuration"></param>
        /// <param name="offDuration"></param>
        public void StartBlink(int onDuration = 200, int offDuration = 200)
        {
            if (onDuration <= 0)
            {
                throw new ArgumentOutOfRangeException("onDuration", "onDuration must be > 0");
            }
            if (offDuration <= 0)
            {
                throw new ArgumentOutOfRangeException("offDuration", "offDuration must be > 0");
            }

            Stop();
            _animationThread = new Thread(() => 
            {
                while (true)
                {
                    TurnOn();
                    Thread.Sleep(onDuration);
                    Stop();
                    Thread.Sleep(offDuration);
                }
            });
            _animationThread.Start();
        }

        /// <summary>
        /// Turn off LED
        /// </summary>
        public void Stop()
        {
            if (_outPutPort != null)
                _outPutPort.Write(false);

            if (DigitalOut != null)
                DigitalOut.State = false;
        }
    }
}