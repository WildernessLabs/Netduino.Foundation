using System;
using System.Threading;
using Microsoft.SPOT;
using H = Microsoft.SPOT.Hardware;
using N = SecretLabs.NETMF.Hardware.Netduino;
using Netduino.Foundation.Sensors.Switches;

namespace SpstSwitch_Sample
{
    public class Program
    {
        public static void Main()
        {
            var spstSwitch = new SpstSwitch(N.Pins.GPIO_PIN_D0, SwitchCircuitTerminationType.High);

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
