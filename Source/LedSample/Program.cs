using System.Threading;
using N = SecretLabs.NETMF.Hardware.Netduino;

namespace LedSample
{
    public class Program
    {
        public static void Main()
        {
            var led = new Netduino.Foundation.LEDs.Led(N.Pins.GPIO_PIN_D8);

            //while (true)
            //{
                led.StartBlink(500, 1000);
                Thread.Sleep(Timeout.Infinite);
            //}
        }
    }
}
