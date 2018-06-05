using System;
using Microsoft.SPOT;
using Netduino.Foundation.Sensors.Buttons;

namespace Netduino.Foundation.Sensors.Rotary
{
    public interface IRotaryEncoderWithButton : IRotaryEncoder, IButton
    {
    }
}
