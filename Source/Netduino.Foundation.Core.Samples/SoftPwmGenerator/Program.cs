using System.Threading;
using Microsoft.SPOT;
using H = Microsoft.SPOT.Hardware;
using N = SecretLabs.NETMF.Hardware.Netduino;
using Netduino.Foundation.Generators;

namespace Netduino.Foundation.Core.Samples
{
    /// <summary>
    /// This sample illustrates using the SoftPwm by flashing an LED (resistor backed) 
    /// on pin 12 to indicate the PWM signal.
    /// </summary>
    public class Program
    {
        public static void Main()
        {
            SoftPwm _pwm = new SoftPwm(N.Pins.GPIO_PIN_D12);

            while (true)
            {
                // start our PWM with defaults (50% duty cycle, 1hz) and run for 
                // 5 seconds
                Debug.Print("Start @ 1hz, 50% dutycycle.");
                _pwm.Start();
                Thread.Sleep(5000);

                // manually stop/start at 4hz fequency and a 25% dutycycle
                Debug.Print("4hz, 25% dutycycle");
                _pwm.Stop();
                _pwm.Frequency = 4;
                _pwm.DutyCycle = 0.25f;
                _pwm.Start();
                Thread.Sleep(5000);

                // change it up again, while it's still running
                Debug.Print("8hz, 50% dutycycle");
                _pwm.Frequency = 8;
                _pwm.DutyCycle = 0.5f;
                Thread.Sleep(5000);

                // stop and reset to defaults
                _pwm.Stop();
                _pwm.Frequency = 1.0f;
                _pwm.DutyCycle = 0.5f;
            }
        }
    }
}
