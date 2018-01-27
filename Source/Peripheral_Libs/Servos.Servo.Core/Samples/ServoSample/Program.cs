using System.Threading;
using SecretLabs.NETMF.Hardware.NetduinoPlus;
using Netduino.Foundation.Servos;

namespace ServoSample
{
    public class Program
    {
        public static void Main()
        {
            Servo servo = new Servo(PWMChannels.PWM_PIN_D9, 1000, 2000);
            while (true)
            {
                for (int angle = 0; angle <= 180; angle++)
                {
                    servo.Angle = angle;
                    Thread.Sleep(40);
                }
                for (int angle = 179; angle > 0; angle--)
                {
                    servo.Angle = angle;
                    Thread.Sleep(40);
                }
            }
        }
    }
}