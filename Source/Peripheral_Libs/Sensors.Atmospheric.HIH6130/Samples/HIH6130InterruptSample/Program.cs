using Microsoft.SPOT;
using Netduino.Foundation.Sensors.Atmospheric;
using System.Threading;

namespace HIH6130InterruptSample
{
    public class Program
    {
        public static void Main()
        {
            //
            //  Create a new HIH6130 and set the temperature change threshold to half a degree.
            //
            HIH6130 hih6130 = new HIH6130(temperatureChangeNotificationThreshold: 0.5F);
            //
            //  Hook up the temperature interrupt handler.
            //
            hih6130.TemperatureChanged += (s, e) =>
            {
                Debug.Print("Temperature changed: " + e.CurrentValue.ToString("f2"));
            };
            //
            //  Hook up the humidity interrupt handler.
            //
            hih6130.HumidityChanged += (s, e) =>
            {
                Debug.Print("Humidity changed: " + e.CurrentValue.ToString("f2"));
            };
            //
            //  Now put the main application to sleep.  The temperature changes will be dealt
            //  with by the event handler above.
            //
            Thread.Sleep(Timeout.Infinite);
        }
    }
}