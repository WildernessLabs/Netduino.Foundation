using System;
using Microsoft.SPOT;

namespace Netduino.Foundation.Sensors.Proximity
{
    public interface IRangeFinder
    {
        float CurrentDistance { get; }
        float MinimumDistance { get; }
        float MaximumDistance { get; }
    }
}
