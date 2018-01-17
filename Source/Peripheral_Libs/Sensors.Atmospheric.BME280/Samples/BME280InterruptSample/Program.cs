using System;
using System.Threading;
using Microsoft.SPOT;
using Netduino.Foundation.Sensors.Atmospheric;
using Netduino.Foundation.Sensors;

namespace BME280InterruptSample
{
    /// <summary>
    ///     This sample illustrates how to use the interrupt (events) to obtain
    ///     temperature, humidity and pressures reading when the values change
    ///     outside of the specified thresholds.
    /// </summary>
    public class Program
    {
        public static void Main()
        {
            //
            //  BME280 temperature, humidity and pressure object.  This object should 
            //  raise an interrupt when the changes in the sensor readings exceed the 
            //  following:
            //
            //  Temperature: +/- 1 C
            //  Humidity: +/- 1 %
            //  Pressure: +/- 10 kPa (the default value for this threshold)
            //
            BME280 sensor = new BME280(temperatureChangeNotificationThreshold: 0.1F,
                humidityChangeNotificationThreshold: 1.0f);
            //
            //  Attach interrupt handlers to the temperature and pressure sensor.
            //
            sensor.HumidityChanged += (s, e) =>
            {
                Debug.Print("Current humidity: " + e.CurrentValue.ToString("f2"));
            };
            sensor.PressureChanged += (s, e) =>
            {
                Debug.Print("Current pressure: " + (e.CurrentValue / 100).ToString("f2"));
            };
            sensor.TemperatureChanged += (s, e) =>
            {
                Debug.Print("Current temperature: " + e.CurrentValue.ToString("f2"));
            };
            //
            //  Application can go to sleep now as readings will be dealt with by the 
            //  interrupt handlers.
            //
            Thread.Sleep(Timeout.Infinite);
        }
    }
}
