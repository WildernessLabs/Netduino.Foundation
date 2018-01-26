using System.Threading;
using Microsoft.SPOT;
using Netduino.Foundation.Sensors.Motion;

namespace ADXL345InterruptSample
{
    public class Program
    {
        public static void Main()
        {
            Debug.Print("\n\n");
            Debug.Print("ADXL345 Interrupt Example.");
            Debug.Print("--------------------------");
            var adxl345 = new ADXL345();
            Debug.Print("Device ID: " + adxl345.DeviceID);
            //
            //  Attach an interrupt handler.
            //
            adxl345.AccelerationChanged += (s, e) =>
            {
                Debug.Print("X: " + e.CurrentValue.X.ToString() +
                            ", Y: " + e.CurrentValue.Y.ToString() +
                            ", Z: " + e.CurrentValue.Z.ToString());
            };
            //
            //  Interrupts are attached so power on the sensor.
            //
            adxl345.SetPowerState(false, false, true, false, ADXL345.Frequency.EightHz);
            //
            //  Put the Netduino to sleep as the interrupt handler will deal 
            //  with changes in acceleration.
            //
            Thread.Sleep(Timeout.Infinite);
        }
    }
}