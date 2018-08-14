using System.Threading;
using Microsoft.SPOT;
using SecretLabs.NETMF.Hardware.NetduinoPlus;
using Netduino.Foundation.Sensors.Temperature;

namespace DS18B20_PollingSample
{
    public class Program
    {
        //
        //  This application demonstrates how to create a new DS18B20object and read
        //  the temperature from the sensor.  The current temperature will be displayed
        //  in the Debug Output window every 500-1250ms.
        //
        //  Note that the rate of display is dependent upon the time it take to perform
        //  the conversion of the temperature into a digital value.
        //
        //  Connect the DS18B20 data pin to GPIO D3 and then apply power and ground to
        //  the sensor.
        //
        public static void Main()
        {
            DS18B20 ds18b20 = new DS18B20(Pins.GPIO_PIN_D3, updateInterval: 0);
            while (true)
            {
                ds18b20.Update();
                Debug.Print("Current temperature: " + ds18b20.Temperature);
                Thread.Sleep(500);
            }
        }
    }
}
