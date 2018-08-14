using System.Threading;
using Microsoft.SPOT;
using SecretLabs.NETMF.Hardware.NetduinoPlus;
using Netduino.Foundation.Sensors.Temperature;

namespace DS18B20_MultipleSensors
{
    /// <summary>
    ///     This application illustrates how to deal with events from multiple DS18B20 temperature
    ///     that are connected to the same digital output pin.
    ///
    ///     The application uses event to capture the temperature changes and then displays the
    ///     current temperature in the Debug output.
    /// </summary>
    /// <remarks>
    ///     Connect two DS18B20 temperature sensors to a Netduino 3 Ethernet or Netduino 3 WiFi using
    ///     GPIO pin D3.
    ///
    ///     Change the sensor ID in the two lines constructing the sensor1 and sensor2 objects in the
    ///     code below.  Note that you can obtain the sensor ID numbers using the DS18B20_ReadConfiguration
    ///     sample.
    /// </remarks>
    public class Program
    {
        /// <summary>
        ///     Main program sets things up and then goes to sleep.
        /// </summary>
        public static void Main()
        {
            DS18B20 sensor1 = new DS18B20(Pins.GPIO_PIN_D3, 0x75000006DE7FE728, 1000);
            sensor1.TemperatureChanged += Sensor1_TemperatureChanged;
            DS18B20 sensor2 = new DS18B20(Pins.GPIO_PIN_D3, 0x64000006DCA35528, 1000, 0.5F);
            sensor2.TemperatureChanged += Sensor2_TemperatureChanged;
            Thread.Sleep(Timeout.Infinite);
        }

        /// <summary>
        ///     Temperature of sensor 1 has changed by more than 0.001 C
        /// </summary>
        private static void Sensor1_TemperatureChanged(object sender, Netduino.Foundation.Sensors.SensorFloatEventArgs e)
        {
            Debug.Print("Sensor1 temperature: " + ((DS18B20) sender).Temperature.ToString("F2"));
        }

        /// <summary>
        ///     Temperature of sensor 2 has changed by over 0.5 C
        /// </summary>
        private static void Sensor2_TemperatureChanged(object sender, Netduino.Foundation.Sensors.SensorFloatEventArgs e)
        {
            Debug.Print("Sensor2 temperature: " + ((DS18B20) sender).Temperature.ToString("F2"));
        }
    }
}
