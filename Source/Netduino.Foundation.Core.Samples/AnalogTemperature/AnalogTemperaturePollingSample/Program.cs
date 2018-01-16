using System.Threading;
using Microsoft.SPOT;
using Netduino.Foundation.Sensors.Temperature;
using SecretLabs.NETMF.Hardware.NetduinoPlus;

namespace AnalogTemperaturePollingSample
{
    /// <summary>
    ///     Illustrate how to read the TMP35 temperature sensor (polling mode).
    /// </summary>
    public class Program
    {
        public static void Main()
        {
            Debug.Print("Read TMP35");
            var tmp35 = new AnalogTemperature(AnalogChannels.ANALOG_PIN_A0,
                                              AnalogTemperature.SensorType.TMP35, 
                                              updateInterval: 0);
            //
            //  Now read the sensor every 5 seconds.
            //
            while (true)
            {
                tmp35.Update();
                Debug.Print("Reading: " + tmp35.Temperature.ToString("f2"));
                Thread.Sleep(5000);
            }
        }
    }
}