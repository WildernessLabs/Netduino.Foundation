using System;
using System.Threading;
using Microsoft.SPOT.Hardware;
using Netduino.Foundation.Sensors.Proximity;
using Microsoft.SPOT;

namespace Netduino.Foundation.Sensors.Distance
{
    public class HCSR04 : IRangeFinder
    {
        #region Properties

        public float DistanceOutput { get; private set; } = -1;

        public event DistanceDetectedEventHandler DistanceDetected = delegate { };

        #endregion

        #region Member variables / fields


        /// <summary>
        ///     Trigger Pin.
        /// </summary>
        protected OutputPort _triggerPort;

        /// <summary>
        ///     Echo Pin.
        /// </summary>
        protected InterruptPort _echoPort;

        protected long _tickStart;

        #endregion

        #region Constructors

        /// <summary>
        ///     Default constructor is private to prevent it being called.
        /// </summary>
        private HCSR04()
        {
        }

        /// <summary>
        ///     Create a new HCSR04 object and hook up the interrupt handler.
        /// </summary>
        /// <param name="triggerPin"></param>
        /// <param name="echoPin"></param>
        public HCSR04(Cpu.Pin triggerPin, Cpu.Pin echoPin)
        {
            if (triggerPin == Cpu.Pin.GPIO_NONE || echoPin == Cpu.Pin.GPIO_NONE)
            {
                throw new Exception("Invalid pin for the HCSR04.");
            }

            _triggerPort = new OutputPort(triggerPin, false);

            _echoPort = new InterruptPort(echoPin, false, Port.ResistorMode.Disabled, Port.InterruptMode.InterruptEdgeBoth);
            _echoPort.OnInterrupt += EchoPortOnInterrupt;
        }

        #endregion

        public void MeasureDistance()
        {
            DistanceOutput = -1;

            // Raise trigger port to high for 10+ micro-seconds
            _triggerPort.Write(true);
            Thread.Sleep(1); //smallest amount of time we can wait

            // Start Clock
            _tickStart = DateTime.Now.Ticks;
            // Trigger device to measure distance via sonic pulse
            _triggerPort.Write(false);
        }

        void EchoPortOnInterrupt(uint data1, uint data2, DateTime time)
        {
            if (data2 == 1) //echo is high
            {
                _tickStart = time.Ticks;
                return;
            }

            Microsoft.SPOT.Debug.Print("Echo interrupt = data1: " + data1 + " data2: " + data2);

            // Calculate Difference
            float elapsed = time.Ticks - _tickStart;

            // Return elapsed ticks
            // x10 for ticks to micro sec
            // divide by 58 for cm
            DistanceOutput = elapsed / 580f;

            this.RaiseDistanceDetected();
        }

        protected virtual void RaiseDistanceDetected()
        {
            Debug.Print("RaiseDistanceDetected: " + DistanceOutput);
            this.DistanceDetected(this, new DistanceEventArgs(DistanceOutput));
        }
    }
}