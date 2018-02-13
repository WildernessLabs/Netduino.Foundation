using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;
using Netduino.Foundation.Sensors.Motion;

namespace ADXL362InterruptSample
{
    public class Program
    {
        public static void Main()
        {
            Debug.Print("ADXL362 - Interrupt Sample");
            var sensor = new ADXL362(SPI_Devices.SPI1, Pins.GPIO_PIN_D7, 100);
            sensor.Stop();
            sensor.ConfigureActivityThreshold(50, 10);
            sensor.AccelerationChanged += (s, e) =>
            {
                Debug.Print("Current acceleration: X = " + e.CurrentValue.X + ", Y = " + e.CurrentValue.Y + ", Z = " + e.CurrentValue.Z);
            };
            sensor.ConfigureInterrupts(ADXL362.InterruptMask.Activity, Pins.GPIO_PIN_D2);
            sensor.DisplayRegisters();
            sensor.Start();
            Thread.Sleep(Timeout.Infinite);
        }
    }
}
