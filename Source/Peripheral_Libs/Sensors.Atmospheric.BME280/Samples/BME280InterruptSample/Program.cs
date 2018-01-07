using System;
using System.Threading;
using Microsoft.SPOT;
using Netduino.Foundation.Sensors.Atmospheric;
using Netduino.Foundation.Sensors;

namespace BME280InterruptSample
{
    public class Program
    {
        /// <summary>
        ///     BME280 temperature, humidity and pressure object.
        /// </summary>
        private static readonly BME280 _sensor = new BME280();

        /// <summary>
        ///     Main program loop.
        /// </summary>
        public static void Main()
        {
            _sensor.HumidityChanged += (s, e) =>
            {
                Debug.Print("Current humidity: " + e.CurrentValue);
            };
            _sensor.PressureChanged += (s, e) =>
            {
                Debug.Print("Current pressure: " + e.CurrentValue);
            };
            _sensor.TemperatureChanged += (s, e) =>
            {
                Debug.Print("Current temperature: " + e.CurrentValue);
            };
            Thread.Sleep(Timeout.Infinite);
        }
    }
}
