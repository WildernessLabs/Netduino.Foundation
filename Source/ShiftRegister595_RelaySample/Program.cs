using System.Threading;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;
using Netduino.Foundation.ICs.IOExpanders.x74595;
using Netduino.Foundation.Relays;
using Microsoft.SPOT;

namespace x74595_RelaySample
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

            DigitalOutputPort relayPort = shiftRegister.CreateOutputPort(0, false);

            var relay = new Relay(relayPort);

            while (true)
            {
                // toggle the relay
                relay.Toggle();

                Debug.Print("Relay on: " + relay.IsOn.ToString());

                // wait for 5 seconds
                Thread.Sleep(1000);
            }
        }
    }
}