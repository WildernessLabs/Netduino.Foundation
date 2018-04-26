using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace Netduino.Foundation.Servos
{
    public class CustomServo : ServoBase
    {
        public CustomServo(Cpu.PWMChannel pin, ServoConfig config) : base(pin, config)
        {

        }
    }
}
