using System.IO.Ports;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using Netduino.Foundation.Sensors.Motion;
using System.Text;

namespace DancingBunny
{
    public class Program
    {
        private static OutputPort _resetPort = new OutputPort(Cpu.Pin.GPIO_Pin2, true);
            
        private static SerialPort _comPort = new SerialPort("COM1", 115200, Parity.None, 8, StopBits.One);

        public static string BooleanToIntString(bool b)
        {
            return b ? "1" : "0";
        }

        private static void WriteText(string text)
        {
            string message = text + "\n";
            _comPort.Write(Encoding.UTF8.GetBytes(message), 0, message.Length);
        }

        public static void Main()
        {
            Debug.Print("BNO055 Test Application.");
            //
            _resetPort.Write(false);
            Thread.Sleep(10);
            _resetPort.Write(true);
            Thread.Sleep(50);
            //
            _comPort.Open();
            WriteText("Orientation Sensor Test");
            WriteText("");
            WriteText("Sensor:       BNO050");
            WriteText("Driver Ver:   1");
            WriteText("Unique ID:    ");
            WriteText("Max Value:    0.0");
            WriteText("Min Value:    0.0");
            WriteText("Resolution:   0.01");
            WriteText("");
            Thread.Sleep(500);
            //
            var bno055 = new BNO055(0x29);
            bno055.DisplayRegisters();
            bno055.PowerMode = BNO055.PowerModes.Normal;
            bno055.OperatingMode = BNO055.OperatingModes.ConfigurationMode;
            bno055.OperatingMode = BNO055.OperatingModes.InertialMeasurementUnit;
            bno055.DisplayRegisters();
            Debug.Print("Current temperature: " + bno055.Temperature.ToString("f2"));
            while (true)
            {
                bno055.Read();
                var reading = bno055.EulerOrientation;
                string orientationMessage = "Orientation: " + reading.Heading + " " + (-1 * reading.Roll) + " " + reading.Pitch;
                string calibrationMessage = "Calibration: " + BooleanToIntString(bno055.IsSystemCalibrated) + " " +
                            BooleanToIntString(bno055.IsGyroscopeCalibrated) + " " +
                            BooleanToIntString(bno055.IsAccelerometerCalibrated) + " " +
                            BooleanToIntString(bno055.IsMagnetometerCalibrated);
                Debug.Print(orientationMessage);
                Debug.Print(calibrationMessage);
                WriteText(orientationMessage);
                WriteText(calibrationMessage);
                Thread.Sleep(100);
            }
        }
    }
}