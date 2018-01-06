using System;
using H = Microsoft.SPOT.Hardware;

namespace Netduino.Foundation.LEDs
{
    /// <summary>
    /// Represents an LED whose voltage is limited by the duty-cycle of a PWM
    /// signal.
    /// </summary>
    public class PwmLed
    {
        public float Brightness {
            get { return _brightness; }
            set {
                if (value < 0 || value > 1) {
                    throw new ArgumentOutOfRangeException("value", "err: brightness must be between 0 and 1, inclusive.");
                }

                this._brightness = value;

                // if 0, shut down the PWM (is this a good idea?)
                if (value == 0) {
                    this._pwm.Stop();
                    this._isOn = false;
                    this._pwm.DutyCycle = 0;
                } else {
                    this._pwm.DutyCycle = this._maximumPwmDuty * Brightness;
                    if (!_isOn) {
                        this._pwm.Start();
                        this._isOn = true;
                    }
                }
            }
        }
        protected float _brightness = 0;
        protected bool _isOn = false;

        public float ForwardVoltage { get; protected set; }

        //
        protected float _maximumPwmDuty = 1;

        protected H.PWM _pwm = null;

        public PwmLed(H.Cpu.PWMChannel pin, float forwardVoltage)
        {
            // validate and persist forward voltage
            if (forwardVoltage < 0 || forwardVoltage > 3.3F) {
                throw new ArgumentOutOfRangeException("forwardVoltage", "error, forward voltage must be between 0, and 3.3");
            }
            this.ForwardVoltage = forwardVoltage;

            this._maximumPwmDuty = CalculateMaximumDutyCycle(forwardVoltage);

            this._pwm = new H.PWM(pin, 100, this._maximumPwmDuty, false);
        }

        /// <summary>
        /// Calculates the maximum duty cycle based on the voltage drop/Forward Voltage/Vf
        /// of the LED.
        /// </summary>
        /// <param name="Vf"></param>
        /// <returns></returns>
        protected float CalculateMaximumDutyCycle(float forwardVoltage)
        {
            // clamp to our maximum output voltage
            float Vf = forwardVoltage;
            if (Vf > 3.3) { Vf = 3.3F; }

            // 1.8V / 3.3V = .55 = 55%
            float maxDutyPercent = Vf / 3.3F;

            return maxDutyPercent;           
        }

    }
}
