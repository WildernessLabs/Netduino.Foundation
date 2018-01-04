using System;
using Microsoft.SPOT;

namespace Netduino.Foundation.Sensors
{
    public interface ITemperatureSensor : ISensor
    {
        event EventHandler TemperatureChanged;

        float Temperature { get; }

        float TemperatureChangeNotificationThreshold { get; set; }
    }
}
