using System.Threading;
using N = SecretLabs.NETMF.Hardware.Netduino;

namespace Netduino.Foundation.Core.Samples
{
    public class RgbPwmLed_BlikAndPulseProgram
    {
        public static void Main()
        {
            // create a new pwm controlled RGB LED on pins Red = 9, Green = 10, and Blue = 11.

            // if your LED is in series with resistors, use this constructor
            var rgbPwmLed = new LEDs.RgbPwmLed(N.PWMChannels.PWM_PIN_D9, N.PWMChannels.PWM_PIN_D10, N.PWMChannels.PWM_PIN_D11);
            // if your LED doesn't have any current limiting resistors in series, use this constructor:
            // var rgbPwmLed = new LEDs.RgbPwmLed(N.PWMChannels.PWM_PIN_D9, N.PWMChannels.PWM_PIN_D10, N.PWMChannels.PWM_PIN_D11, 2.1F, 3.0F, 3.0F);

            // run forever
            while (true)
            {
                // blink for 10 secondd
                rgbPwmLed.StartBlink(Color.AliceBlue);
                Thread.Sleep(10000);

                // alternate between two colors for 10 seconds
                rgbPwmLed.StartAlternatingColors(Color.Bisque, Color.Purple, 200, 200);
                Thread.Sleep(10000);

                // run through various colors for 10 seconds
                rgbPwmLed.StartRunningColors(new System.Collections.ArrayList { Color.White, Color.YellowGreen, Color.Tomato, Color.SteelBlue } , new int []{ 250, 150, 100, 200 });
                Thread.Sleep(10000);


            }

        }
    }
}
