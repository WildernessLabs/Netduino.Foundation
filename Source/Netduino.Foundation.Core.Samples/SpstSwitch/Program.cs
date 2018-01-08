using Microsoft.SPOT;
using Netduino.Foundation;
using Netduino.Foundation.Sensors.Switches;
using System.Threading;
using N = SecretLabs.NETMF.Hardware.Netduino;

namespace SpstSwitch_Sample
{
    public class Program
    {
        public static void Main()
        {
            var spstSwitch = new SpstSwitch(N.Pins.GPIO_PIN_D0, CircuitTerminationType.High);

            Debug.Print("Initial switch state, isOn: " + spstSwitch.IsOn.ToString());

            spstSwitch.Changed += (s, e) =>
            {
                Debug.Print("Switch Changed");
                Debug.Print("Switch on: " + spstSwitch.IsOn.ToString());
            };

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
