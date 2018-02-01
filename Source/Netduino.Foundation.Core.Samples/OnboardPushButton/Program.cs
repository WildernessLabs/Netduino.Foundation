using System.Threading;
using Microsoft.SPOT;
using N = SecretLabs.NETMF.Hardware.Netduino;
using H = Microsoft.SPOT.Hardware;

namespace Netduino.Foundation.Core.Samples
{
    public class Program
    {
        public static void Main()
        {
            var pushButton = new Netduino.Foundation.Sensors.Buttons.PushButton((H.Cpu.Pin)0x15, CircuitTerminationType.Floating);

            pushButton.PressStarted += (s, e) =>
            {
                Debug.Print("Press started");
            };

            pushButton.PressEnded += (s, e) =>
            {
                Debug.Print("Press ended");
            };

            pushButton.Clicked += (s, e) =>
            {
                Debug.Print("Button Clicked");
            };

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
