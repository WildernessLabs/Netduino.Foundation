using Microsoft.SPOT;
using Netduino.Foundation.Sensors.Motion;
using SecretLabs.NETMF.Hardware.NetduinoPlus;
using System.Threading;

namespace MAG3110Sample
{
    public class Program
    {
        public static void Main()
        {
            Debug.Print("MAG3110 Test Application");
            MAG3110 mag3110 = new MAG3110(0x0e, 400, Pins.GPIO_PIN_D8);
            int readingCount = 0;
            while (true)
            {
                mag3110.Read();
                readingCount++;
                Debug.Print("Reading " + readingCount.ToString() + ": x = " + mag3110.X.ToString() + ", y = " + mag3110.Y.ToString() + ", z = " + mag3110.Z.ToString());
                Thread.Sleep(1000);
            }
        }
    }
}
