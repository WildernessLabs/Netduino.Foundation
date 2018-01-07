using Microsoft.SPOT;
using Netduino.Foundation.Sensors.Atmospheric;
using System.Threading;

namespace BME280_Sample
{
    public class Program
    {
        public static void Main()
        {
            BME280 sensor = new BME280();

            string message;
            while (true)
            {
                sensor.Update();
                message = "Temperature: " + sensor.Temperature.ToString("F1") + " C\n";
                message += "Humidity: " + sensor.Humidity.ToString("F1") + " %\n";
                message += "Pressure: " + (sensor.Pressure / 100).ToString("F0") + " hPa\n\n";
                Debug.Print(message);
                Thread.Sleep(1000);
            }
        }
    }
}

