using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace Netduino.Foundation.Servos
{
    public class Servo : ServoBase
    {
        public Servo(Cpu.PWMChannel pin, ServoConfig config) : base(pin, config)
        {

        }
    }
}
