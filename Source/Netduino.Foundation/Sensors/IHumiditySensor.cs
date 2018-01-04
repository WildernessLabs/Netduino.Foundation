using System;
using Microsoft.SPOT;

namespace Netduino.Foundation.Sensors
{
    public interface IHumiditySensor : ISensor
    {
        event EventHandler TemperatureChanged;

        float Humidity { get; }

        float HumidityChangeNotificationThreshold { get; set; }
    }
}
