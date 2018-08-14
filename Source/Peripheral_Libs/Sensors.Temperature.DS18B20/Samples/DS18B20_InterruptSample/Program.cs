using System.Threading;
using Microsoft.SPOT;
using SecretLabs.NETMF.Hardware.NetduinoPlus;
using Netduino.Foundation.Sensors.Temperature;

namespace DS18B20_InterruptSample
{
    /// <summary>
    ///     This application illustrates how to deal with events from a DS18B20 temperature
    ///     sensor.
    ///
    ///     The application uses event to capture the temperature changes by +/- 0.5 C (or more)
    ///     and then displays the current temperature in the Debug output.
    /// </summary>
    /// <remarks>
    ///     Connect a single DS18B20 temperature sensor to a Netduino 3 Ethernet or Netduino 3 WiFi using
    ///     GPIO pin D3.
    ///
    ///     Note that the sensor ID is not required when only one OneWire device is connected
    ///     to a single digital pin.
    /// </remarks>
    public class Program
    {
        /// <summary>
        ///     Main program sets things up and then goes to sleep.
        /// </summary>
        public static void Main()
        {
            DS18B20 ds18B20 = new DS18B20(Pins.GPIO_PIN_D3, updateInterval: 1000, temperatureChangeNotificationThreshold: 0.5F);
            ds18B20.TemperatureChanged += ds18b20_TemperatureChanged;
            Thread.Sleep(Timeout.Infinite);
        }

        /// <summary>
        ///     Temperature of ds18b20 has changed by more than 0.001 C
        /// </summary>
        private static void ds18b20_TemperatureChanged(object sender, Netduino.Foundation.Sensors.SensorFloatEventArgs e)
        {
            Debug.Print("Temperature: " + ((DS18B20) sender).Temperature.ToString("F2"));
        }
    }
}
