using System.Threading;
using N = SecretLabs.NETMF.Hardware.Netduino;

namespace Netduino.Foundation.Core.Samples
{
    public class RgbPwmLed_BlikAndPulseProgram
    {
        public static void Main()
        {
            // create a new pwm controlled RGB LED on pins Red = 9, Green = 10, and Blue = 11.
            var rgbPwmLed = new Netduino.Foundation.LEDs.RgbPwmLed(N.PWMChannels.PWM_PIN_D9, N.PWMChannels.PWM_PIN_D10, N.PWMChannels.PWM_PIN_D11);

            rgbPwmLed.SetColor(Color.Aqua);
            rgbPwmLed.StartBlink();

            // run forever
            while (true)
            {
                Thread.Sleep(1000);
            }

        }
    }
}
