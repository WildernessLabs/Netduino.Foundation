using N = SecretLabs.NETMF.Hardware.Netduino;
using System.Threading;

namespace Netduino.Foundation.Core.Samples
{
    public class HelloWorldProgram
    {
        /// <summary>
        /// Plug a green LED into pins 11 and GND, placing the longer lead (cathode) in pin 11.
        /// </summary>
        public static void Main()
        {
            // create a new pwm controlled LED on pin 11
            var pwmLed = new LEDs.PwmLed(N.PWMChannels.PWM_PIN_D11, LEDs.TypicalForwardVoltage.Green);

            // blink the LED 
            while (true)
            {
                pwmLed.Brightness = 1;
                Thread.Sleep(50);
                pwmLed.Brightness = 0;
                Thread.Sleep(500);
            }
        }
    }
}
