using System.Threading;
using Microsoft.SPOT;
using Netduino.Foundation.Sensors.Temperature;

namespace TMP102InterruptSample
{
    /// <summary>
    ///     Illustrate how to read the TMP102 using interrupts.
    /// </summary>
    public class Program
    {
        public static void Main()
        {
            Debug.Print("TMP102 Interrupt Sample");
            //
            //  Create a new TMP102 object, check the termperature every
            //  1s and report any changes grater than +/- 0.1C.
            //
            var tmp102 = new TMP102(updateInterval: 1000, temperatureChangeNotificationThreshold: 0.1F);
            //
            //  Hook up the interrupt handler.
            //
            tmp102.TemperatureChanged += (s, e) =>
            {
                Debug.Print("Temperature: " + e.CurrentValue.ToString("f2"));
            };
            //
            //  Now put the main program to sleep as the interrupt handler
            //  above deals with processing the sensor data.
            //
            Thread.Sleep(Timeout.Infinite);
        }
    }
}