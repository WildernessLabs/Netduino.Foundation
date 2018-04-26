using Microsoft.SPOT;
using Netduino.Foundation;
using Netduino.Foundation.Sensors.Buttons;
using Netduino.Foundation.Servos;
using System.Threading;
using H = Microsoft.SPOT.Hardware;
using N = SecretLabs.NETMF.Hardware.Netduino;


namespace ServoSample
{
    public class Program
    {
        static IServo _servo = null;
        static PushButton _button = null;

        public static void Main()
        {
            _servo = new Standard180Servo(N.PWMChannels.PWM_PIN_D9);
            _button = new PushButton((H.Cpu.Pin)0x15, CircuitTerminationType.Floating);

            _button.Clicked += (object sender, Microsoft.SPOT.EventArgs e) =>
            {
                Debug.Print("Button Clicked");
                ToggleServo();
            };

            Thread.Sleep(Timeout.Infinite);
        }

        static void ToggleServo()
        {
            if (_servo.Angle == 0)
            {
                _servo.RotateTo(180);
            }
            else
            {
                _servo.RotateTo(0);
            }
        }
    }
}