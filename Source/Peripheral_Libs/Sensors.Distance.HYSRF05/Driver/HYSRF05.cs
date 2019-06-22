using System;
using System.Threading;
using Microsoft.SPOT.Hardware;

namespace Netduino.Foundation.Sensors.Distance
{
    //also works for US-100
    public class HYSRF05 : IRangeFinder
    {
        #region Properties

        public float CurrentDistance { get; private set; } = -1;

        public float MinimumDistance => 2; //in cm
        public float MaximumDistance => 450; //in cm

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
        private HYSRF05()
        {
        }

        /// <summary>
        ///     Create a new HYSRF05 object and hook up the interrupt handler
        ///     HSSRF05 must be running the default 4/5 pin mode
        ///     3 pin mode is not supported on Netduino
        /// </summary>
        /// <param name="triggerPin"></param>
        /// <param name="echoPin"></param>
        public HYSRF05(Cpu.Pin triggerPin, Cpu.Pin echoPin)
        {
            if (triggerPin == Cpu.Pin.GPIO_NONE || echoPin == Cpu.Pin.GPIO_NONE)
            {
                throw new Exception("Invalid pin for the HYSRF05.");
            }

            _triggerPort = new OutputPort(triggerPin, false);

            _echoPort = new InterruptPort(echoPin, false, Port.ResistorMode.Disabled, Port.InterruptMode.InterruptEdgeBoth);
            _echoPort.OnInterrupt += EchoPortOnInterrupt;
        }

        #endregion

        public void MeasureDistance()
        {
            CurrentDistance = -1;

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

            // Calculate Difference
            float elapsed = time.Ticks - _tickStart;

            // Return elapsed ticks
            // x10 for ticks to micro sec
            // divide by 58 for cm (assume speed of sound is 340m/s)
            CurrentDistance = elapsed / 580f;

            if (CurrentDistance < MinimumDistance || CurrentDistance > MaximumDistance)
                CurrentDistance = -1;

            this.DistanceDetected(this, new DistanceEventArgs(CurrentDistance));
        }
    }
}