using System.Threading;
using N = SecretLabs.NETMF.Hardware.Netduino;

namespace LedSample
{
    public class Program
    {
        public static void Main()
        {
            var led = new Netduino.Foundation.LEDs.Led(N.Pins.GPIO_PIN_D8);

            while(true)
            {
                led.IsOn = true;
                Thread.Sleep(3000);
                led.IsOn = false;
                Thread.Sleep(2000);
                led.IsOn = true;
                Thread.Sleep(1000);

                led.StartBlink();
                Thread.Sleep(5000);
                led.Stop();
            }
        }
    }
}
