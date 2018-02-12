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
            var _adxl362 = new ADXL362(SPI.SPI_module.SPI1, Cpu.Pin.GPIO_Pin7);
            _adxl362.ConfigureActivityThreshold(50, 10);
            _adxl362.AccelerationChanged += (s, e) =>
            {
                Debug.Print("Current acceleration: X = " + e.CurrentValue.X + ", Y = " + e.CurrentValue.Y + ", Z = " + e.CurrentValue.Z);
            };
            _adxl362.ConfigureInterrupts(ADXL362.InterruptMask.Activity, Pins.GPIO_PIN_D2);
            Thread.Sleep(Timeout.Infinite);
        }
    }
}
