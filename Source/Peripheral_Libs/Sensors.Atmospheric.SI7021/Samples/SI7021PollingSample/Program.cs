using System.Threading;
using Microsoft.SPOT;
using Netduino.Foundation.Sensors.Atmospheric;

namespace Si7021Test
{
    public class Program
    {
        public static void Main()
        {
            Debug.Print("SI7021 Test");
            var si7021 = new SI7021(updateInterval: 0);
            Debug.Print("Serial number: " + si7021.SerialNumber);
            Debug.Print("Firmware revision: " + si7021.FirmwareRevision);
            Debug.Print("Sensor type: " + si7021.SensorType);
            Debug.Print("Current resolution: " + si7021.Resolution);
            while (true)
            {
                Debug.Print("Temperature: " + si7021.Temperature.ToString("f2"));
                Thread.Sleep(1000);
            }
        }
    }
}