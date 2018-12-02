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
                //p.Write(true);
                //Thread.Sleep(300);
                //p.Write(false);
                //Thread.Sleep(300);

              //  Debug.Print(mUS.MeasureDistance().ToString());
                Thread.Sleep(1000);
            }
        }
    }
}
