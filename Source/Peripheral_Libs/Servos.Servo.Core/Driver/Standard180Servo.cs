using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace Netduino.Foundation.Servos
{
    public class Standard180Servo : ServoBase
    {
        public Standard180Servo(Cpu.PWMChannel pin) : base(pin, new ServoConfig())
        {

        }
    }
}
