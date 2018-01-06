using System;
using System.Threading;
using H = Microsoft.SPOT.Hardware;

namespace Netduino.Foundation.LEDs
{
    /// <summary>
    /// Represents an LED whose voltage is limited by the duty-cycle of a PWM
    /// signal.
    /// </summary>
    public class PwmLed
    {
        /// <summary>
        /// The brightness of the LED, controlled by a PWM signal, and limited by the 
        /// calculated maximum voltage. Valid values are from 0 to 1, inclusive.
        /// </summary>
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
        protected Thread _animationThread = null;


        /// <summary>
        /// Creates a new PwmLed on the specified PWM pin and limited to the appropriate 
        /// voltage based on the passed `forwardVoltage`. Typical LED forward voltages 
        /// can be found in the `TypicalForwardVoltage` class.
        /// </summary>
        /// <param name="pin"></param>
        /// <param name="forwardVoltage"></param>
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


        public void StartBlink(int onDuration = 200, int offDuration = 200, float onBrightness = 1, float offBrightness = 0)
        {
            if (onBrightness > 1 || onBrightness <= 0)
            {
                throw new ArgumentOutOfRangeException("onBrightness", "onBrightness must be > 0 and <= 1");
            }
            if (offBrightness >= 1 || offBrightness < 0)
            {
                throw new ArgumentOutOfRangeException("offBrightness", "lowBrightness must be >= 0 and < 1");
            }
            if (offBrightness >= onBrightness)
            {
                throw new Exception("offBrightness must be less than onBrightness");
            }

            // stop any existing animations
            this.Stop();
            this._animationThread = new Thread(() => {
                while (true)
                {
                    this.Brightness = onBrightness;
                    Thread.Sleep(onDuration);
                    this.Brightness = offBrightness;
                    Thread.Sleep(offDuration);
                }
            });
            this._animationThread.Start();
        }

        public void StartPulse(int pulseDuration = 600, float highBrightness = 1, float lowBrightness = 0.15F)
        {
            if (highBrightness > 1 || highBrightness <= 0) {
                throw new ArgumentOutOfRangeException("highBrightness", "highBrightness must be > 0 and <= 1");
            }
            if (lowBrightness >= 1 || lowBrightness < 0)
            {
                throw new ArgumentOutOfRangeException("lowBrightness", "lowBrightness must be >= 0 and < 1");
            }
            if (lowBrightness >= highBrightness)
            {
                throw new Exception("lowBrightness must be less than highbrightness");
            }

            // stop any existing animations
            this.Stop();
            this._animationThread = new Thread(() => {
                // pulse the LED by taking the brightness from low to high and back again.
                float brightness = lowBrightness;
                bool ascending = true;
                int intervalTime = 60; // 60 miliseconds is probably the fastest update we want to do, given that threads are given 20 miliseconds by default. 
                float steps = pulseDuration / intervalTime;
                float changeAmount = (highBrightness - lowBrightness) / steps;
                float changeUp = changeAmount;
                float changeDown = -1 * changeAmount;

                while (true)
                {
                    // are we brightening or dimming?
                    if (brightness <= lowBrightness) { ascending = true; }
                    else if (brightness == highBrightness) { ascending = false; }
                    brightness += (ascending) ? changeUp : changeDown;

                    // float math error clamps
                    if (brightness < 0) { brightness = 0; }
                    else if (brightness > 1) { brightness = 1; }

                    // set our actual brightness
                    this.Brightness = brightness;

                    // go to sleep, my friend.
                    Thread.Sleep(intervalTime);
                }
            });
            this._animationThread.Start();
        }

        public void Stop()
        {
            if(this._animationThread != null)
            {
                this._animationThread.Abort();
            }
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
