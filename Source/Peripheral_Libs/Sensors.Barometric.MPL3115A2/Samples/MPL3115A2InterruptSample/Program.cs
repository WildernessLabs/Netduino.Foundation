using System.Threading;
using Microsoft.SPOT;
using Netduino.Foundation.Sensors.Barometric;

namespace MPL3115A2InterruptSample
{
    public class Program
    {
        public static void Main()
        {
            Debug.Print("MPL3115A2 Test");
            var mpl3115a2 = new MPL3115A2(temperatureChangeNotificationThreshold: 0.1F);
            mpl3115a2.TemperatureChanged += (s, e) =>
            {
                Debug.Print("Temperature: " + mpl3115a2.Temperature.ToString("f2"));
            };
            mpl3115a2.PressureChanged += (s, e) =>
            {
                Debug.Print("Pressure: " + mpl3115a2.Pressure.ToString("f2")); 
            };
            Thread.Sleep(Timeout.Infinite);
        }
    }
}