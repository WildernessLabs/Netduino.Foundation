using System;
using Microsoft.SPOT;

namespace Netduino.Foundation.Sensors.Distance
{
    public class DistanceEventArgs : EventArgs
    {
        public float DistanceMearurement { get; set; }

        public DistanceEventArgs(float distanceMearurement)
        {
            DistanceMearurement = distanceMearurement;
        }
    }

    public delegate void DistanceDetectedEventHandler(object sender, DistanceEventArgs e);
}
