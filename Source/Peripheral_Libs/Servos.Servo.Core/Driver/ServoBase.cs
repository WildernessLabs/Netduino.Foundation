using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.SPOT;
using H = Microsoft.SPOT.Hardware;


namespace Netduino.Foundation.Servos
{
    public abstract class ServoBase : IServo
    {
        protected ServoConfig _config = null;
        protected H.PWM _pwm = null;

        public int Angle {
            get { return _angle; }
        } protected int _angle;

        public ServoBase(H.Cpu.PWMChannel pin, ServoConfig config)
        {
            _config = config;
            _pwm = new H.PWM(pin, config.Frequency, 0, false);
        }

        public void RotateTo(int angle)
        {
            double pulseDuration = CalculatePulseDuration(angle);
            Debug.Print("Pulse Duration: " + pulseDuration.ToString());
            SendCommandPulse(pulseDuration);
            _angle = angle;
        }

        public void RotateTo(int angle, double speed)
        {
        }

        protected double CalculatePulseDuration(int angle)
        {
            // offset + (angle percent * duration length)
            return _config.MinimumPulseDuration + ((angle / _config.MaximumAngle) * (_config.MaximumPulseDuration - _config.MinimumPulseDuration));
            // 0 degrees time = 1000 + ( (0 / 180) * 1000 ) = 1,000 microseconds
            // 90 degrees time = 1000 + ( (90 / 180) * 1000 ) = 1,500 microseconds
            // 180 degrees time = 1000 + ( (180 / 180) * 1000 ) = 2,000 microseconds
        }

        protected double CalculateDutyCycle(double pulseDuration)
        {
            var dutyCycle = pulseDuration / 20000;
            Debug.Print("Duty cycle: " + dutyCycle.ToString());
            return pulseDuration / 20000; // why this? because frequency is 50? 
        }

        protected void SendCommandPulse(double pulseDuration)
        {
            Debug.Print("Sending Command Pulse");
            _pwm.DutyCycle = CalculateDutyCycle(pulseDuration);
            _pwm.Start();
            //Thread.Sleep(pulseDuration + 5); // may or may not be necessary
            //_pwm.Stop();

        }
    }
}
