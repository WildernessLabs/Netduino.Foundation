using System;
using System.Threading;
//using Netduino.Foundation.GPIO;
using Microsoft.SPOT.Hardware;

namespace Netduino.Foundation.Sensors.Motion
{
    public class HCSR04
    {
        #region Member variables / fields

        /// <summary>
        ///     Trigger Pin.
        /// </summary>
        protected OutputPort _outputPort;

        /// <summary>
        ///     Echo Pin.
        /// </summary>
        protected InterruptPort _interruptPort;

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
                _outputPort = new OutputPort(triggerPin, false);

                _interruptPort = new InterruptPort(echoPin, false, Port.ResistorMode.Disabled, Port.InterruptMode.InterruptEdgeLow);
                _interruptPort.OnInterrupt += _interruptPort_OnInterrupt;
            }
            else
            {
                throw new Exception("Invalid pin for the HCSR04.");
            }
        }

        #endregion

        public long Ping()
        {
            // Reset Sensor
            _outputPort.Write(true);
            Thread.Sleep(1);

            // Start Clock
            _tickEnd = 0L;
            _tickStart = System.DateTime.Now.Ticks;
            // Trigger Sonic Pulse
            _outputPort.Write(false);

            // Wait 1/20 second (this could be set as a variable instead of constant)
            Thread.Sleep(50);

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
                return elapsed * 10 / 636;
            }

            // Sonic pulse wasn't detected within 1/20 second
            return -1L;
        }

        void _interruptPort_OnInterrupt(uint data1, uint data2, DateTime time)
        {
            _tickEnd = time.Ticks;
        }
    }
}
