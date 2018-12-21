using System;
using Microsoft.SPOT;
using System.Threading;
using Netduino.Foundation.Sensors.Distance;
using SecretLabs.NETMF.Hardware.Netduino;

namespace HCSR04Sample
{
    public class Program
    {
        public static void Main()
        {
            var  _HCSR04 = new HCSR04(Pins.GPIO_PIN_D12, Pins.GPIO_PIN_D11);
            _HCSR04.DistanceDetected += OnDistanceDetected;

            while (true)
            {
                // Send a echo
                _HCSR04.MeasureDistance();
                Thread.Sleep(500);
            }
        }

        // Fired when detecting an obstacle
        private static void OnDistanceDetected(object sender, DistanceEventArgs e) 
        {
            Debug.Print(e.Distance.ToString());
        }
    }
}

namespace System.Diagnostics
{
    public enum DebuggerBrowsableState
    {
        Never = 0,
        Collapsed = 2,
        RootHidden = 3
    }
}
