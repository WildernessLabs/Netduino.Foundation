using System.Threading;
using Microsoft.SPOT;
using Netduino.Foundation.Sensors.Motion;
using SecretLabs.NETMF.Hardware.NetduinoPlus;

namespace ADXL335InterruptSample
{
    public class Program
    {
        public static void Main()
        {
            var adxl335 = new ADXL335(AnalogChannels.ANALOG_PIN_A0, AnalogChannels.ANALOG_PIN_A1,
                                      AnalogChannels.ANALOG_PIN_A2);
            adxl335.SupplyVoltage = 3.3;
            adxl335.XVoltsPerG = 0.343;
            adxl335.YVoltsPerG = 0.287;
            adxl335.ZVoltsPerG = 0.541;
            //
            //  Attach an interrupt handler.
            //
            adxl335.AccelerationChanged += (s, e) =>
            {
                var rawData = adxl335.GetRawSensorData();
                Debug.Print("\n");
                Debug.Print("X: " + adxl335.X.ToString("F2") + ", " + rawData.X.ToString("F2"));
                Debug.Print("Y: " + adxl335.Y.ToString("F2") + ", " + rawData.Y.ToString("F2"));
                Debug.Print("Z: " + adxl335.Z.ToString("F2") + ", " + rawData.Z.ToString("F2"));
            };
            //
            //  Netduino can now go to sleep as the data will be processed
            //  by the interrupt handler.
            //
            Thread.Sleep(Timeout.Infinite);
        }
    }
}