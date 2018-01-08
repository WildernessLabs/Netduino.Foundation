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

            //pushButton.Clicked += (s, e) =>
            //{
            //    Debug.Print("Switch Changed");
            //    Debug.Print("Switch on: " + spstSwitch.IsOn.ToString());
            //};

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
