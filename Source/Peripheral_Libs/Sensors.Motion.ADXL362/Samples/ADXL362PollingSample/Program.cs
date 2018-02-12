using System.Threading;
using Microsoft.SPOT;
using Netduino.Foundation.Sensors.Motion;
using SecretLabs.NETMF.Hardware.NetduinoPlus;

namespace ADXL362Test
{
    public class Program
    {
        public static void Main()
        {
            Debug.Print("ADXL362 - Test");
            var sensor = new ADXL362(SPI_Devices.SPI1, Pins.GPIO_PIN_D7, 100);
            while (true)
            {
                sensor.Update();
                Debug.Print("X: " + sensor.X + ", Y: " + sensor.Y + ", Z: " + sensor.Z);
                Thread.Sleep(1000);
            }
        }
    }
}