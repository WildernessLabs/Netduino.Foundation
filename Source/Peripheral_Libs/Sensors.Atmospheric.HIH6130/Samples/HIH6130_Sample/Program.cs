using Microsoft.SPOT;
using Netduino.Foundation.Sensors.Atmospheric;
using System.Threading;

namespace HIH6130_Sample
{
    public class Program
    {
        public static void Main()
        {
            HIH6130 hih6130 = new HIH6130();
            while (true)
            {
                hih6130.Read();
                Debug.Print("Temperature: " + hih6130.Temperature.ToString("f2") + ", Humidity: " + hih6130.Humidity.ToString("f2"));
                Thread.Sleep(1000);
            }
        }
    }
}