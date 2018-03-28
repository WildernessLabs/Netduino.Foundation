using System;
using Microsoft.SPOT;

namespace Netduino.Foundation.Sensors.Proximity
{
    public interface IRangeFinder
    {
        float DistanceOutput { get; }
    }
}
