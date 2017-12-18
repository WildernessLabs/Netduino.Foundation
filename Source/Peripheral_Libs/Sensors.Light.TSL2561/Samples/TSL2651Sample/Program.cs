using System.Threading;
using Microsoft.SPOT;
using Netduino.Foundation.Sensors.Light;

namespace TSL2561Example
{
    public class Program
    {
        private static readonly TSL2561 tsl2561 = new TSL2561();

        public static void Main()
        {
            Debug.Print("Polled TSL2561 Application.");
            Debug.Print("Device ID: " + tsl2561.ID);
            tsl2561.TurnOff();
            tsl2561.SensorGain = TSL2561.Gain.High;
            tsl2561.Timing = TSL2561.IntegrationTiming.Ms402;
            tsl2561.TurnOn();
            //
            //  Wait for at least one integration cycle (set to 402 milliseconds above).
            //
            Thread.Sleep(500);
            //
            //  Repeatedly read the light intensity.
            //
            while (true)
            {
                var adcData = tsl2561.SensorReading;
                Debug.Print("Data0: " + adcData[0] + ", Data1: " + adcData[1]);
                Debug.Print("Light intensity: " + tsl2561.Lux);
                Thread.Sleep(2000);
            }
        }
    }
}