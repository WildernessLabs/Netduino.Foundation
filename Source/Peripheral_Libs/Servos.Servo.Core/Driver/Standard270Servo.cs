using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace Netduino.Foundation.Servos
{
    public class Standard270Servo : ServoBase
    {
        public Standard270Servo(Cpu.PWMChannel pin) : base(pin, new ServoConfig(maximumAngle: 270))
        {

        }
    }
}
