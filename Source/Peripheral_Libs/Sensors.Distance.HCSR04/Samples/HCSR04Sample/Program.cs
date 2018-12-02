using System;
using Microsoft.SPOT;
using System.Threading;
using Netduino.Foundation.Sensors.Motion;
using SecretLabs.NETMF.Hardware.Netduino;

namespace HCSR04Sample
{
    public class Program
    {
        public static void Main()
        {
            //OutputPort p = new OutputPort(Pins.ONBOARD_LED, true);
            HCSR04 mUS = new HCSR04(Pins.GPIO_PIN_D5, Pins.GPIO_PIN_D4);

            while (true)
            {
                //  Debug.Print(mUS.MeasureDistance().ToString());
                mUS.MeasureDistance();
                Thread.Sleep(500);
                Debug.Print(mUS.DistanceOutput.ToString());
            }
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
