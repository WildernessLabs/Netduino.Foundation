using System.Threading;
using Microsoft.SPOT;
using Netduino.Foundation.Sensors.Motion;

namespace ADXL345Sample
{
    public class Program
    {
        public static void Main()
        {
            Debug.Print("\n\n");
            Debug.Print("ADXL345 Register Test Application.");
            Debug.Print("----------------------------------");
            var adxl345 = new ADXL345();
            Debug.Print("Device ID: " + adxl345.DeviceID);
            adxl345.SetPowerState(false, false, true, false, ADXL345.Frequency.EightHz);
            while (true)
            {
                adxl345.Update();
                Debug.Print("X: " + adxl345.X + ", Y: " + adxl345.Y + ", Z: " + adxl345.Z);
                Thread.Sleep(500);
            }
        }
    }
}