using Microsoft.SPOT;
using Netduino.Foundation.Sensors.Atmospheric;
using System.Threading;

namespace HIH6130_Sample
{
    public class Program
    {
        public static void Main()
        {
            // create a new HIH6130 and set the temp change threshold to half a degree
            HIH6130 hih6130 = new HIH6130(temperatureChangeNotificationThreshold: 0.5F);

            hih6130.TemperatureChanged += (s, e) => {
                Debug.Print("temp changed: " + hih6130.Temperature.ToString());
            };
            
            while (true)
            {
                //hih6130.Read();
                Debug.Print("Temperature: " + hih6130.Temperature.ToString("f2") + ", Humidity: " + hih6130.Humidity.ToString("f2"));
                Thread.Sleep(1000);
            }
        }
    }
}