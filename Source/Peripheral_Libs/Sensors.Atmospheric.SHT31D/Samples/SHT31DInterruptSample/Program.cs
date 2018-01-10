using System.Threading;
using Microsoft.SPOT;
using Netduino.Foundation.Sensors.Atmospheric;

namespace SHT31DInterruptSample
{
    /// <summary>
    ///     Illustrate the use of the SHT31D operating in interrupt mode.
    /// </summary>
    public class Program
    {
        public static void Main()
        {
            //
            //  Create a new SHT31D object that will generate interrupts when
            //  the temperature changes by more than +/- 0.1C or the humidity
            //  changes by more than 1%.
            //
            SHT31D sht31d = new SHT31D(temperatureChangeNotificationThreshold: 0.1F,
                humidityChangeNotificationThreshold: 1.0F);
            //
            //  Hook up the two interrupt handlers to display the changes in
            //  temperature and humidity.
            //
            sht31d.HumidityChanged += (s, e) =>
            {
                Debug.Print("Current humidity: " + e.CurrentValue.ToString("f2"));
            };

            sht31d.TemperatureChanged += (s, e) =>
            {
                Debug.Print("Current temperature: " + e.CurrentValue.ToString("f2"));
            };
            //
            //  Main program loop can now go to sleep as the work
            //  is being performed by the interrupt handlers.
            //
            Thread.Sleep(Timeout.Infinite);
        }
    }
}
