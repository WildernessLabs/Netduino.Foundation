using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.SPOT;
using H = Microsoft.SPOT.Hardware;

namespace Netduino.Foundation.Servos
{
    /// <summary>
    /// Represents a servo that can rotate continuously.
    /// </summary>
    public class ContinuousRotationServo : ContinuousRotationServoBase
    {

        /// <summary>
        /// Instantiates a new continuous rotation servo on the specified pin, with the specified configuration.
        /// </summary>
        /// <param name="pin"></param>
        /// <param name="config"></param>
        public ContinuousRotationServo(H.Cpu.PWMChannel pin, ServoConfig config) : base (pin, config)
        {
        }
    }
}
