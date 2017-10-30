using System;
using Microsoft.SPOT.Hardware;

namespace Netduino.Foundation.Sensors.Motion
{
    /// <summary>
    /// Create a new Parallax PIR object.
    /// </summary>
    public class ParallaxPIR
    {
        #region Member variables and fields

        /// <summary>
        /// Interrupt port 
        /// </summary>
        private InterruptPort _interruptPort = null;

        #endregion Member variables and fields

        #region Delegates and events

        /// <summary>
        /// Delgate for the motion start and end events.
        /// </summary>
        public delegate void MotionChange(object sender);

        /// <summary>
        /// Event raied when motion is detected.
        /// </summary>
        public event MotionChange OnMotionStart = null;

        /// <summary>
        /// Event raised when the PIR indicates that there is not longer any motion.
        /// </summary>
        public event MotionChange OnMotionEnd = null;

        #endregion Delegates and events

        #region Constructors

        /// <summary>
        /// Default constructor is private to prevent it being called.
        /// </summary>
        private ParallaxPIR()
        {
        }

        /// <summary>
        /// Create a new Parallax PIR object and hook up the interrupt handler.
        /// </summary>
        /// <param name="interruptPin"></param>
        public ParallaxPIR(Cpu.Pin interruptPin)
        {
            if (interruptPin != Cpu.Pin.GPIO_NONE)
            {
                _interruptPort = new InterruptPort(interruptPin, false, Port.ResistorMode.Disabled, Port.InterruptMode.InterruptEdgeBoth);
                _interruptPort.OnInterrupt += _interruptPort_OnInterrupt;
            }
            else
            {
                throw new Exception("Invalid pin for the PIR interrupts.");
            }
        }

        #endregion Constructors

        #region Interrupt handlers

        /// <summary>
        /// Catch the PIR motion change interrupts and work out which interrupt should be raised.
        /// </summary>
        void _interruptPort_OnInterrupt(uint data1, uint data2, DateTime time)
        {
            if (_interruptPort.Read())
            {
                if (OnMotionStart != null)
                {
                    OnMotionStart(this);
                }
            }
            else
            {
                if (OnMotionEnd != null)
                {
                    OnMotionEnd(this);
                }
            }
        }

        #endregion Interrupt handlers
    }
}
