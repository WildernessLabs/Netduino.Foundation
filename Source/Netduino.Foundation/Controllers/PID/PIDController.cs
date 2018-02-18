using System;
using System.Threading;

namespace Netduino.Foundation.Controllers.PID
{
    public class PidController
    {
        protected DateTime _lastUpdateTime;
        protected float _lastError;
        protected float _integral;

        public float ActualInput { get; set; }
        public float TargetInput { get; set; }

        public float OutputMin { get; set; } = -1;
        public float OutputMax { get; set; } = 1;

        public float ProportionalGain { get; set; } = 1;
        public float IntegralGain { get; set; } = 0;
        public float DerivativeGain { get; set; } = 0;

        public PidController()
        {
            _lastUpdateTime = DateTime.Now;
            _lastError = 0;
            _integral = 0;
        }

        public void ResetIntegrator()
        {
            _integral = 0;
        }

        /// <summary>
        /// Calculates the control output based on the Target and Actual, using the current PID values
        /// 
        /// </summary>
        /// <param name="correctionActions">
        ///     The corrective actions to use in the Calculation. By default, it uses
        ///     P, I, and D. To just use PI control pass:
        ///     `PIDActionType.Proportional | PIDActionType.Integral`.
        /// </param>
        /// <returns></returns>
        public float CalculateControlOutput(PIDActionType correctionActions = PIDActionType.Proportional | PIDActionType.Integral | PIDActionType.Derivative)
        {
            var now = DateTime.Now;
            // time delta (how long since last calculation)
            var dt = now - _lastUpdateTime;

            // if no time has passed, don't make any changes.
            if (dt.Ticks <= 0.0) return ActualInput;

            // copy vars
            var input = ActualInput;
            var target = TargetInput;

            // calculate the error (how far we are from target)
            var error = target - input;

            // calculate the proportional term
            var proportional = ProportionalGain * error;

            // calculate the integral
            _integral += error * (float)dt.Ticks;
            var integral = IntegralGain * _integral;

            // calculate the derivative term
            var diff = error / (float)dt.Ticks;
            var derivative = DerivativeGain * diff;

            float control = 0.0f;

            // add the appropriate corrections
            if ((correctionActions & PIDActionType.Proportional) == PIDActionType.Proportional) control += proportional;
            if ((correctionActions & PIDActionType.Integral) == PIDActionType.Integral) control += _integral;
            if ((correctionActions & PIDActionType.Derivative) == PIDActionType.Derivative) control += derivative;

            _lastUpdateTime = now;

            // clamp
            if (control > OutputMax) control = OutputMax;
            if (control < OutputMin) control = OutputMin;

            return control;
        }
    }
}