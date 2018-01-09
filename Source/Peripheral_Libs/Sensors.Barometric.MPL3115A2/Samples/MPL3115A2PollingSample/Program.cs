using System.Threading;
using Microsoft.SPOT;
using Netduino.Foundation.Sensors.Barometric;

namespace MPL3115A2Test
{
    public class Program
    {
        public static void Main()
        {
            Debug.Print("MPL3115A2 Test");
            var mpl3115a2 = new MPL3115A2(updateInterval: 0);
            while (true)
            {
                mpl3115a2.Update();
                Debug.Print("Temperature: " + mpl3115a2.Temperature.ToString("f2") + ", Pressure: " +
                            mpl3115a2.Pressure.ToString("f2"));
                Thread.Sleep(1000);
            }
        }
    }
}