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
            var _si7021 = new SI7021();
            Debug.Print("Serial number: " + _si7021.SerialNumber);
            Debug.Print("Firmware revision: " + _si7021.FirmwareRevision);
            Debug.Print("Sensor type: " + _si7021.SensorType);
            Debug.Print("Current resolution: " + _si7021.Resolution);
            while (true)
            {
                _si7021.Read();
                Debug.Print("Temperature: " + _si7021.Temperature.ToString("f2") + ", Humidity: " +
                            _si7021.Humidity.ToString("f2"));
                Thread.Sleep(1000);
            }
        }
    }
}