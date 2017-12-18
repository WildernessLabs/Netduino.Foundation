using System.Threading;
using Microsoft.SPOT;
using Netduino.Foundation.Sensors.Barometric;

namespace MPL115A2Test
{
    public class Program
    {
        public static void Main()
        {
            var mpl115a2 = new MPL115A2();

            Debug.Print("MPL115A2 Test");
            while (true)
            {
                mpl115a2.Read();
                Debug.Print("Pressure: " + mpl115a2.Pressure.ToString("f2") + " kPa, Temperature: " +
                            mpl115a2.Temperature.ToString("f2") + "C");
                Thread.Sleep(1000);
            }
        }
    }
}