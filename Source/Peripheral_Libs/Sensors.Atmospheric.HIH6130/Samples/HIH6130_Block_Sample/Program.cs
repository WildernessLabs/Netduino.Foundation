using System;
using System.Threading;
using Netduino.Foundation;
using Netduino.Foundation.SpecializedBlocks;
using Netduino.Foundation.Sensors.Atmospheric;
using Microsoft.SPOT;

namespace HIH6130_Block_Sample
{
    public class Program
    {
        public static void Main()
        {
            HIH6130 hih6130 = new HIH6130();
            var debugScope = new DebugScope();
            var cToF = new CelsiusToFahrenheit();

            hih6130.BlockTemperature.ConnectTo(cToF.Celsius);

            debugScope.ConnectTo(cToF.Fahrenheit);

            while (true)
            {
                Thread.Sleep(500);
            }
        }
    }
}
