using Microsoft.SPOT;
using Netduino.Foundation;
using Netduino.Foundation.Sensors.Switches;
using System.Threading;
using N = SecretLabs.NETMF.Hardware.Netduino;

namespace SpdtSwitch_Sample
{
    public class Program
    {
        public static void Main()
        {
            var spdtSwitch = new SpdtSwitch(N.Pins.GPIO_PIN_D5);

            Debug.Print("Initial switch state, isOn: " + spdtSwitch.IsOn.ToString());

            spdtSwitch.Changed += (s, e) =>
            {
                Debug.Print("Switch Changed");
                Debug.Print("Switch on: " + spdtSwitch.IsOn.ToString());
            };

            Thread.Sleep(Timeout.Infinite);
        }
    }
}