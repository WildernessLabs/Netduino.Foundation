using System.Threading;
using Microsoft.SPOT;
using Netduino.Foundation.Sensors.Light;

namespace TSL2561InterruptExample
{
    /// <summary>
    ///     This application illustrates how to use the TSL2561 light sensor
    ///     operating in interrupt mode for changes in light level.
    /// </summary>
    public class Program
    {
        public static void Main()
        {
            //
            //  Create a TSL2561 object operating in interrupt mode.
            //
            TSL2561 tsl2561 = new TSL2561();
            //
            //  Set up the sensor.
            //
            Debug.Print("TSL2561 Interrupt Application.");
            Debug.Print("Device ID: " + tsl2561.ID);
            tsl2561.TurnOff();
            tsl2561.SensorGain = TSL2561.Gain.High;
            tsl2561.Timing = TSL2561.IntegrationTiming.Ms402;
            tsl2561.TurnOn();
            tsl2561.LightLevelChanged += (s, e) =>
            {
                Debug.Print("Light intensity: " + e.CurrentValue.ToString("f2"));
            };
            //
            //  Put the application to sleep as the interrupt handler will deal
            //  with any changes in sensor data.
            //
            Thread.Sleep(Timeout.Infinite);
        }
    }
}