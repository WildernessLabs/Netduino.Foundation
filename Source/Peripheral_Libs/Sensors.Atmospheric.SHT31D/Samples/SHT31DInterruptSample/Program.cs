using System;
using System.Threading;
using Microsoft.SPOT;
using Netduino.Foundation.Sensors.Atmospheric;;

namespace SHT31DInterruptSample
{
    public class Program
    {
        public static void Main()
        {
            SHT31D sht31d = new SHT31D();
            sht31d.HumidityChanged += (s, e) =>
            {
                Debug.Print("Current humidity: " + e.CurrentValue);
            };

            sht31d.TemperatureChanged += (s, e) =>
            {
                Debug.Print("Current temperature: " + e.CurrentValue);
            };

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
