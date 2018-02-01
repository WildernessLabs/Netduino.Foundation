using Microsoft.SPOT;
using N = SecretLabs.NETMF.Hardware.Netduino;
using System.Threading;

namespace Netduino.Foundation.Core.Samples
{
    public class Program
    {
        public static void Main()
        {
            // create a new pwm controlled RGB LED on pins Red = 9, Green = 10, and Blue = 11.
            var rgbPwmLed = new Netduino.Foundation.LEDs.RgbPwmLed(N.PWMChannels.PWM_PIN_D9, N.PWMChannels.PWM_PIN_D10, N.PWMChannels.PWM_PIN_D11,
                2.0f, 2.5f, 2.7f);

            // run forever
            while (true)
            {

                // loop through the entire hue spectrum (360 degrees)
                for (int i = 0; i < 360; i++)
                {
                    var hue = ((double)i / 360F);
                    Debug.Print(hue.ToString());

                    // set the color of the RGB
                    rgbPwmLed.SetColor(Color.FromHsba(((double)i/360F), 1, 1));

                    // for a fun, fast rotation through the hue spectrum:
                    //Thread.Sleep (1);
                    // for a gentle walk through the forest of colors;
                    Thread.Sleep(18);
                }
            }
        }
    }
}
