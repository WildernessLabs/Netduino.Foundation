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
            var  _HCSR04 = new HCSR04(Pins.GPIO_PIN_D5, Pins.GPIO_PIN_D4);
            //_HCSR04.DistanceDetected += OnDistanceDetected; // <- Event sample

            while (true)
            {
                _HCSR04.MeasureDistance();
                Thread.Sleep(500);
            }
        }

        //private static void OnDistanceDetected(object sender, DistanceEventArgs e) // <- Event sample
        //{
        //    Debug.Print(e.Distance.ToString());
        //}
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
