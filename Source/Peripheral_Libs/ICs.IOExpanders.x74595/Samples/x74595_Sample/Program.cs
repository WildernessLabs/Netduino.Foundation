using System.Threading;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;
using Netduino.Foundation.ICs.IOExpanders.x74595;

namespace x74595_Sample
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

            var shiftRegister = new x74595(8, config);

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
