using System.Threading;
using Microsoft.SPOT.Hardware;
using Netduino.Foundation.ICs;
using SecretLabs.NETMF.Hardware.NetduinoPlus;

namespace ShiftRegisterTest
{
    public class Program
    {
        public static void Main()
        {
            var config = new SPI.Configuration(SPI_mod: SPI_Devices.SPI1,
                                               ChipSelect_Port: Pins.GPIO_PIN_D8,
                                               ChipSelect_ActiveState: false,
                                               ChipSelect_SetupTime: 0,
                                               ChipSelect_HoldTime: 0,
                                               Clock_IdleState: true,
                                               Clock_Edge: true,
                                               Clock_RateKHz: 10);
            var shiftRegister = new ShiftRegister74595(8, config);
            while (true)
            {
                shiftRegister.Clear(true);
                for (byte index = 0; index <= 7; index++)
                {
                    shiftRegister[index] = true;
                    shiftRegister.LatchData();
                    Thread.Sleep(500);
                    shiftRegister[index] = false;
                }
            }
        }
    }
}