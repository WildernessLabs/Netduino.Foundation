using System;
using Microsoft.SPOT;

namespace Netduino.Foundation.Servos
{
    public interface IContinuousRotationServo
    {
        ServoConfig Config { get; }

        RotationDirection CurrentDirection { get; }

        float CurrentSpeed { get; }

        void Rotate(RotationDirection direction, float speed);

        void Stop();

    }
}
