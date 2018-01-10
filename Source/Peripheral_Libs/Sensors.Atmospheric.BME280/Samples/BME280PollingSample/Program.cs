using Microsoft.SPOT;
using Netduino.Foundation.Sensors.Atmospheric;
using System.Threading;

namespace BME280_Sample
{
    /// <summary>
    ///     Illustrate how to use the BME280 in polling mode.
    /// </summary>
    public class Program
    {
        public static void Main()
        {
            //
            //  Create a new BME280 object and put the sensor into polling
            //  mode (update intervale set to 0ms).
            //
            BME280 sensor = new BME280(updateInterval: 0);

            string message;
            while (true)
            {
                //
                //  Make the sensor take new readings.
                //
                sensor.Update();
                //
                //  Prepare a message for the user and output to the debug console.
                //
                message = "Temperature: " + sensor.Temperature.ToString("F1") + " C\n";
                message += "Humidity: " + sensor.Humidity.ToString("F1") + " %\n";
                message += "Pressure: " + (sensor.Pressure / 100).ToString("F0") + " hPa\n\n";
                Debug.Print(message);
                //
                //  Sleep for 1000ms before repeating the process.
                //
                Thread.Sleep(1000);
            }
        }
    }
}

