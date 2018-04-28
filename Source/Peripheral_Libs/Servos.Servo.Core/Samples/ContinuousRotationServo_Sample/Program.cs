using Microsoft.SPOT;
using Netduino.Foundation;
using Netduino.Foundation.Sensors.Buttons;
using Netduino.Foundation.Servos;
using System.Threading;
using H = Microsoft.SPOT.Hardware;
using N = SecretLabs.NETMF.Hardware.Netduino;


namespace ContinuousRotationServoSample

{
    public class Program
    {
        static ContinuousRotationServo _servo = null;
        static PushButton _button = null;
        static bool _running = false;

        public static void Main()
        {
            _servo = new ContinuousRotationServo(N.PWMChannels.PWM_PIN_D9, NamedServoConfigs.IdealContinuousRotationServo);
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
            if (!_running)
            {
                Thread th = new Thread(() => {
                    _running = true;
                    while (_running)
                    {
                        Debug.Print("Rotating clockwise at increasing speeds.");
                        for (float speed = 1; speed <= 10; speed++)
                        {
                            if (!_running) break;
                            _servo.Rotate(Direction.Clockwise, (speed / 10.0f));
                            Thread.Sleep(500);
                        }

                        if (!_running) break;
                        Debug.Print("Stopping for half a sec.");
                        _servo.Stop();
                        Thread.Sleep(500);

                        Debug.Print("Rotating counter-clockwise at increasing speeds.");
                        for (float speed = 1; speed <= 10; speed++)
                        {
                            if (!_running) break;
                            _servo.Rotate(Direction.CounterClockwise, (speed / 10.0f));
                            Thread.Sleep(500);
                        }
                    }
                });
                th.Start();
            } else {
                Debug.Print("Stopping.");
                _running = false;
                Thread.Sleep(550); // wait for the loop to break
                _servo.Stop();
            }
        }
    }
}