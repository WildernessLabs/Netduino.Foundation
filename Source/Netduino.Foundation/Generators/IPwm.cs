using System;
using Microsoft.SPOT;

namespace Netduino.Foundation.Generators
{
    public interface IPwm
    {
        InputPort DutyCycleInput { get; }
        InputPort FrequencyInput { get; }
    }
}
