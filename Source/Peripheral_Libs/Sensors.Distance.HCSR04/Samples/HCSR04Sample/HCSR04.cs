using System;
using System.Threading;
//using Netduino.Foundation.GPIO;
using Microsoft.SPOT.Hardware;
using Netduino.Foundation.Sensors.Proximity;

namespace Netduino.Foundation.Sensors.Motion
{
    public class HCSR04 : IRangeFinder
    {
        #region Properties
        public float DistanceOutput { get; private set; } = -1;

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
        protected long _tickEnd;
        protected long _minTicks = 4000L;
   
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
            if (triggerPin != Cpu.Pin.GPIO_NONE && echoPin != Cpu.Pin.GPIO_NONE)
            {
                throw new Exception("Invalid pin for the HCSR04.");
            }
            
            _triggerPort = new OutputPort(triggerPin, false);

            _echoPort = new InterruptPort(echoPin, false, Port.ResistorMode.Disabled, Port.InterruptMode.InterruptEdgeLow);
            _echoPort.OnInterrupt += _echoPort_OnInterrupt;
        }

        #endregion

        public void MeasureDistance()
        {
            DistanceOutput = -1L;

            // Raise trigger port to high for 10+ micro-seconds
            _triggerPort.Write(true);
            Thread.Sleep(1); //smallest amount of time we can wait

            // Start Clock
            _tickEnd = 0L;
            _tickStart = DateTime.Now.Ticks;
            // Trigger device to measure distance via sonic pulse
            _triggerPort.Write(false);
        }

        void _echoPort_OnInterrupt(uint data1, uint data2, DateTime time)
        {
            _tickEnd = time.Ticks;

            if (_tickEnd > 0L)
            {
                // Calculate Difference
                long elapsed = _tickEnd - _tickStart;

                // Subtract out fixed overhead (interrupt lag, etc.)
                elapsed -= _minTicks;
                if (elapsed < 0L)
                {
                    elapsed = 0L;
                }

                // Return elapsed ticks
                DistanceOutput = elapsed * 10 / 636;
            }
        }
    }
}