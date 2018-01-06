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

            // pulse the LED by taking the brightness from 15% too 100% and back again.
            float brightness = 0.15F;
            bool ascending = true;
            float change = 0;
            while (true)
            {
                if (brightness <= 0.15)
                {
                    ascending = true;
                }
                else if (brightness == 1)
                {
                    ascending = false;
                }
                change = (ascending) ? 0.1F : -0.1F;
                brightness += change;

                //float error clamp
                if (brightness < 0) { brightness = 0; }
                else if (brightness > 1) { brightness = 1; }

                pwmLed.Brightness = brightness;

                // for very fast, try 20
                Thread.Sleep(50);

            }
        }
    }
}
