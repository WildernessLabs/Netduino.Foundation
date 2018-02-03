using System.Threading;
using Microsoft.SPOT;
using Netduino.Foundation.Sensors.Barometric;

namespace MPL115A2InterruptSample
{
    /// <summary>
    ///     This application illustrates how to use the interrupt (event)
    ///     handlers in the MPL115A2 sensor to obtain temperature and pressure
    ///     readings.
    /// </summary>
    public class Program
    {
        public static void Main()
        {
            //
            //  Create a new MPL object and set the temperature change threshold to 0.1C
            //  and leave the pressure threshold set to the default 10 kPa.  Have the
            //  sensor check the current readings every 0.5 seconds (500 milliseconds)
            //
            var mpl115a2 = new MPL115A2(updateInterval: 500, temperatureChangeNotificationThreshold: 0.1F);

            Debug.Print("MPL115A2 Interrupt Example");
            //
            //  Attach interrupt handlers to the temperature and pressure sensor.
            //
            mpl115a2.PressureChanged += (s, e) =>
            {
                Debug.Print("Pressure: " + e.CurrentValue.ToString("f2"));
            };

            mpl115a2.TemperatureChanged += (s, e) =>
            {
                Debug.Print("Temperature: " + e.CurrentValue.ToString("f2") + "C");
            };
            //
            //  Application can go to sleep now as readings will be dealt with by the 
            //  interrupt handlers.
            //
            Thread.Sleep(Timeout.Infinite);
        }
    }
}