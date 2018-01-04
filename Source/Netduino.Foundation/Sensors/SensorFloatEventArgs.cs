using System;
using Microsoft.SPOT;

namespace Netduino.Foundation.Sensors
{
    public class SensorFloatEventArgs : EventArgs
    {
        public float LastNotifiedValue { get; set; }
        public float CurrentValue { get; set; }

        public SensorFloatEventArgs (float lastValue, float currentValue)
        {
            LastNotifiedValue = lastValue;
            CurrentValue = currentValue;
        }
    }

    public delegate void SensorFloatEventHandler(object sender, SensorFloatEventArgs e);
}
