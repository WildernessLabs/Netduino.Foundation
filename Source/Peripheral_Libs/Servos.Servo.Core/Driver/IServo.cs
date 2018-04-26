using System;
using Microsoft.SPOT;

namespace Netduino.Foundation.Servos
{
    public interface IServo
    {
        void RotateTo(int angle);

        void RotateTo(int angle, double speed);

        int Angle { get; }
    }
}
