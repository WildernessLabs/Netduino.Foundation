using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using Netduino.Fountation.Sensors.Light;

namespace ALSPT19315CTest
{
    public class Program
    {
        public static void Main()
        {
            Debug.Print("ALSPT19315C Test");
            var sensor = new ALSPT19315C(Cpu.AnalogChannel.ANALOG_1, 3.3);
            while (true)
            {
                Debug.Print("Sensor reading: " + sensor.Voltage.ToString("f2"));
                Thread.Sleep(1000);
            }
        }
    }
}