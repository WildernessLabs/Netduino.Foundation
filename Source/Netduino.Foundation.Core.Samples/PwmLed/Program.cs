using N = SecretLabs.NETMF.Hardware.Netduino;
using System.Threading;

namespace Netduino.Foundation.Core.Samples
{
    public class Program
    {
        public static void Main()
        {
            // create a new pwm controlled LED on pin 11
            var pwmLed = new LEDs.PwmLed(N.PWMChannels.PWM_PIN_D11, LEDs.TypicalForwardVoltage.Green);

            // turn the LED on at 50% power
            pwmLed.SetBrightness(0.5F);

            // keep the program alive.
            Thread.Sleep(Timeout.Infinite);
        }
    }
}
