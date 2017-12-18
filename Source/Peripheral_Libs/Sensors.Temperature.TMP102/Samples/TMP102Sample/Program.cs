using System.Threading;
using Microsoft.SPOT;
using Netduino.Foundation.Sensors.Temperature;

namespace TMP102Sample
{
    public class Program
    {
        public static void Main()
        {
            Debug.Print("TMP102 Test");
            var tmp102 = new TMP102();
            while (true)
            {
                Debug.Print("Temperature: " + tmp102.Temperature.ToString("f2"));
                Thread.Sleep(1000);
            }
        }
    }
}