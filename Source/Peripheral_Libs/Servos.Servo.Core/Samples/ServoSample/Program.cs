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
            //_servo = new Standard180Servo(N.PWMChannels.PWM_PIN_D9);
            //_servo = new CustomServo(N.PWMChannels.PWM_PIN_D9, new ServoConfig(minimumPulseDuration: 550, maximumPulseDuration: 2400 ));
            _servo = new Servo(N.PWMChannels.PWM_PIN_D9, new ServoConfig(minimumPulseDuration: 900, maximumPulseDuration: 2100));
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
                Debug.Print("Rotating to 180");
                _servo.RotateTo(180);
            }
            else
            {
                Debug.Print("Rotating to 0");
                _servo.RotateTo(0);
            }
        }
    }
}