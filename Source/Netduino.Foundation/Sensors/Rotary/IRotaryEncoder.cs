using Microsoft.SPOT;
using System;
using H = Microsoft.SPOT.Hardware;

namespace Netduino.Foundation.Sensors.Rotary
{
    public interface IRotaryEncoder
    {
        event RotaryTurnedEventHandler Rotated;

        H.InterruptPort APhasePin { get; }
        H.InterruptPort BPhasePin { get; }
    }
}
