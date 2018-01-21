using System.Threading;
using Microsoft.SPOT;
using N = SecretLabs.NETMF.Hardware.Netduino;
using Netduino.Foundation.Sensors.Rotary;

namespace Netduino.Foundation.Core.Samples
{
    public class RotaryEncoderSample
    {
        public static void Main()
        {
            var rotary = new RotaryEncoder(N.Pins.GPIO_PIN_D6, N.Pins.GPIO_PIN_D7);

            rotary.Rotated += Rotary_Rotated;

            Thread.Sleep(Timeout.Infinite);
        }

        private static void Rotary_Rotated(object sender, RotaryTurnedEventArgs e)
        {
            Debug.Print("Rotated " + ((e.Direction == RotationDirection.Clockwise) ? "clockwise." : "counter-clockwise.") );
        }
    }
}
