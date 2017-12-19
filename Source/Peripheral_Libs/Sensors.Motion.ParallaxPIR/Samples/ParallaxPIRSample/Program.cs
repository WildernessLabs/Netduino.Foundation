using Microsoft.SPOT;
using Netduino.Foundation.Sensors.Motion;
using SecretLabs.NETMF.Hardware.NetduinoPlus;
using System.Threading;

namespace ParallaxPIRSample
{
    public class Program
    {
        public static void Main()
        {
            ParallaxPIR pir = new ParallaxPIR(Pins.GPIO_PIN_D8);

            pir.OnMotionStart += pir_OnMotionStart;
            pir.OnMotionEnd += pir_OnMotionEnd;
            Thread.Sleep(Timeout.Infinite);
        }

        static void pir_OnMotionEnd(object sender)
        {
            Debug.Print("Motion stopped.");
        }

        static void pir_OnMotionStart(object sender)
        {
            Debug.Print("Motion detected.");
        }
    }
}
