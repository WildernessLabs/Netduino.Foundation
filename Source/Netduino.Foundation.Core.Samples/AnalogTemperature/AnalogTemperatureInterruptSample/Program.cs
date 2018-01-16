using System.Threading;
using Microsoft.SPOT;
using Netduino.Foundation.Sensors.Temperature;
using SecretLabs.NETMF.Hardware.NetduinoPlus;

namespace AnalogTemperatureInterruptSample
{
    /// <summary>
    ///     Illustrate how to read the TMP35 temperature sensor in interrupt mode.
    /// </summary>
    public class Program
    {
        public static void Main()
        {
            Debug.Print("Read TMP35");
            //
            //  Create a new TMP35 object to check the temperature every 1s and
            //  to report any changes over 0.1C.
            //
            var tmp35 = new AnalogTemperature(AnalogChannels.ANALOG_PIN_A0,
                AnalogTemperature.SensorType.TMP35, updateInterval: 1000, 
                temperatureChangeNotificationThreshold: 0.1F);
            //
            //  Connect an interrupt handler.
            //
            tmp35.TemperatureChanged += (s, e) =>
            {
                Debug.Print("Temperature: " + e.CurrentValue.ToString("f2"));
            };
            //
            //  Now put the application to sleep as the data is processed
            //  by the interrupt handler above.
            //
            Thread.Sleep(Timeout.Infinite);
        }
    }
}