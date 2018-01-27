using Microsoft.SPOT;
using System;
using System.Threading;
using H = Microsoft.SPOT.Hardware;

namespace Netduino.Foundation.Motors
{
    public class HBridgeMotor : IDCMotor
    {
        /// <summary>
        /// 0 - 1 for the speed.
        /// </summary>
        public float Speed {
            get { return this._speed; }
            set
            {
                this._speed = value;
                
                var calibratedSpeed = this._speed * this.MotorCalibrationMultiplier;
                var absoluteSpeed = System.Math.Min(System.Math.Abs(calibratedSpeed), 1);
                var isForward = calibratedSpeed > 0;

                //Debug.Print("calibrated speed: " + calibratedSpeed.ToString() + ", isForward: " + isForward.ToString());

                // set speed. if forward, disable right pwm. otherwise disable left
                _motorLeftPwm.DutyCycle = (isForward) ? absoluteSpeed : 0;
                _motorRighPwm.DutyCycle = (isForward) ? 0 : absoluteSpeed;

                // clear our neutral to enable
                this.IsNeutral = false;

                _motorLeftPwm.Start();
                _motorRighPwm.Start();
            }
        }
        protected float _speed = 0;

        /// <summary>
        /// Not all motors are created equally. This number scales the Speed Input so
        /// that you can match motor speeds without changing your logic.
        /// </summary>
        public float MotorCalibrationMultiplier { get; set; } = 1;

        /// <summary>
        /// When true, the wheels spin "freely"
        /// </summary>
        public bool IsNeutral {
            get { return this._isNeutral; }
            set
            {
                this._isNeutral = value;
                // if neutral, we disable the port
                this._enablePort.Write(!this._isNeutral);
            }
        } protected bool _isNeutral = true;

        /// <summary>
        /// The frequency of the PWM used to drive the motors. 
        /// Default value is 1600.
        /// </summary>
        public float PwmFrequency
        {
            get { return _pwmFrequency; }
        } protected readonly float _pwmFrequency;

        protected H.PWM _motorLeftPwm = null; // H-Bridge 1A pin
        protected H.PWM _motorRighPwm = null; // H-Bridge 2A pin
        protected H.OutputPort _enablePort = null; // if enabled, then IsNeutral = false

 
        public HBridgeMotor(H.Cpu.PWMChannel a1Pin, H.Cpu.PWMChannel a2Pin, H.Cpu.Pin enablePin, float pwmFrequency = 1600)
        {
            this._pwmFrequency = pwmFrequency;
            // create our PWM outputs
            this._motorLeftPwm = new H.PWM(a1Pin, _pwmFrequency, 0, false);
            this._motorRighPwm = new H.PWM(a2Pin, _pwmFrequency, 0, false);
            this._enablePort = new H.OutputPort(enablePin, false);
        }
    }
}
