using System.Threading;
using Microsoft.SPOT;
using N = SecretLabs.NETMF.Hardware.Netduino;

namespace Netduino.Foundation.Core.Samples
{
    public class Program
    {
        public static void Main()
        {
            var pushButton = new Netduino.Foundation.Sensors.Buttons.PushButton(N.Pins.GPIO_PIN_D4, CircuitTerminationType.CommonGround);

            pushButton.PressStarted += (s, e) =>
            {
                Debug.Print("Press started");
            };

            pushButton.PressEnded += (s, e) =>
            {
                Debug.Print("Press ended");
            };

            pushButton.Clicked += (s, e) =>
            {
                Debug.Print("Button Clicked");
            };

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
