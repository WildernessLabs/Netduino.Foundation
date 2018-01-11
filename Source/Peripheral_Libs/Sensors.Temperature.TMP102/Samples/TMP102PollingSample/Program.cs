using System.Threading;
using Microsoft.SPOT;
using Netduino.Foundation.Sensors.Temperature;

namespace TMP102PollingSample
{
    /// <summary>
    ///     Illustrate how to read the temperature from a TMP102
    ///     by polling the sensor.
    /// </summary>
    public class Program
    {
        public static void Main()
        {
            Debug.Print("TMP102 Polling Sample");
            var tmp102 = new TMP102(updateInterval: 0);
            //
            //  Read the TMP102 every second and display the temperature.
            //
            while (true)
            {
                tmp102.Update();
                Debug.Print("Temperature: " + tmp102.Temperature.ToString("f2"));
                Thread.Sleep(1000);
            }
        }
    }
}