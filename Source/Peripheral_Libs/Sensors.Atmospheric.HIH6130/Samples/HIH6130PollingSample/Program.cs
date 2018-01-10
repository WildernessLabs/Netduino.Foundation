using Microsoft.SPOT;
using Netduino.Foundation.Sensors.Atmospheric;
using System.Threading;

namespace HIH6130PollingSample
{
    /// <summary>
    ///     This application illustrates how to use the HIH6310 sensor in polling mode.
    /// </summary>
    public class Program
    {
        public static void Main()
        {
            //
            //  Create a new HIH6130 in polling mode.
            //
            HIH6130 hih6130 = new HIH6130(updateInterval: 0);
            //
            //  Loop continuously updating the sensor readings and displaying
            //  them on the debug console.
            //
            while (true)
            {
                hih6130.Update();
                Debug.Print("Temperature: " + hih6130.Temperature.ToString("f2") +
                            "C, Humidity: " + hih6130.Humidity.ToString("f2") + "%");
                Thread.Sleep(1000);
            }
        }
    }
}