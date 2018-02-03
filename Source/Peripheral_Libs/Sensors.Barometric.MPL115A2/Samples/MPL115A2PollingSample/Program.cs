using System.Threading;
using Microsoft.SPOT;
using Netduino.Foundation.Sensors.Barometric;

namespace MPL115A2PollingSample
{
    /// <summary>
    ///     This application illustrates how to use the MPL115A2 temperature and
    ///     pressure in polling mode.  It is the responsibility of the application
    ///     to check the sensor readings as required.
    /// </summary>
    public class Program
    {
        public static void Main()
        {
            //
            //  Create a new MPL115A2 sensor object and set to polling mode
            //  i.e. update period is 0 milliseconds.
            //
            var mpl115a2 = new MPL115A2(updateInterval: 0);

            Debug.Print("MPL115A2 Polling Example");
            while (true)
            {
                //
                //  Have the sensor make new readings.
                //
                mpl115a2.Update();
                //
                //  Display the values in the debug console.
                //
                Debug.Print("Pressure: " + mpl115a2.Pressure.ToString("f2") + " kPa, Temperature: " +
                            mpl115a2.Temperature.ToString("f2") + "C");
                //
                //  Sleep for a while (1 second) before taking the next readins.
                //
                Thread.Sleep(1000);
            }
        }
    }
}