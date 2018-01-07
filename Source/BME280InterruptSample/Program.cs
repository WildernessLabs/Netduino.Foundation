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
            _sensor.HumidityChanged += Sensor_HumidityChanged;
            _sensor.PressureChanged += Sensor_PressureChanged;
            _sensor.TemperatureChanged += Sensor_TemperatureChanged;
            Thread.Sleep(Timeout.Infinite);
        }

        /// <summary>
        ///     Temperature change is greater than the defualt threshold.
        /// </summary>
        private static void Sensor_TemperatureChanged(object sender, SensorFloatEventArgs e)
        {
            Debug.Print("Current temperature: " + e.CurrentValue);
        }

        /// <summary>
        ///     Pressure change is greater than the defualt threshold.
        /// </summary>
        private static void Sensor_PressureChanged(object sender, SensorFloatEventArgs e)
        {
            Debug.Print("Current pressure: " + e.CurrentValue);
        }

        /// <summary>
        ///     Humidity change is greater than the defualt threshold.
        /// </summary>
        private static void Sensor_HumidityChanged(object sender, SensorFloatEventArgs e)
        {
            Debug.Print("Current humidity: " + e.CurrentValue);
        }
    }
}
