using Microsoft.SPOT;
using System;
using System.Threading;

namespace Netduino.Foundation.Controllers.PID
{
    public class PidController
    {
        // state vars
        protected DateTime _lastUpdateTime;
        protected float _lastError = 0.0f;
        protected float _integral = 0.0f;
        protected float _lastControlOutputValue = 0.0f;

        /// <summary>
        /// Represents the ProcessVariable (PV), or the actual signal
        /// reading of the system in its current state. For example, 
        /// when heating a cup of coffee to 75º, if the temp sensor
        /// says the coffee is currently at 40º, the 40º is the 
        /// actual input value.
        /// </summary>
        public float ActualInput { get; set; }
        /// <summary>
        /// Represents the SetPoint (SP), or the reference target signal
        /// to acheive. For example, when heating a cup of coffee to 
        /// 75º, 75º is the target input value.
        /// </summary>
        public float TargetInput { get; set; }

        public float OutputMin { get; set; } = -1;
        public float OutputMax { get; set; } = 1;

        public float ProportionalGain { get; set; } = 1;
        public float IntegralGain { get; set; } = 0;
        public float DerivativeGain { get; set; } = 0;

        /// <summary>
        /// Whether or not to print the calculation information to the
        /// output console in an comma-delimited form. Useful for 
        /// pasting into a spreadsheet to graph the system control 
        /// performance when tuning the PID controller corrective
        /// action gains.
        /// </summary>
        public bool OutputTuningInformation { get; set; } = false;

        public bool AutoResetIntegrator { get; set; } = true;

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
            // init vars
            float control = 0.0f;
            var now = DateTime.Now;

            // time delta (how long since last calculation)
            var dt = now - _lastUpdateTime;
            // seconds is better than ticks to bring our calculations into perspective
            var seconds = (float)(dt.Ticks / 10000 / 1000);

            // if no time has passed, don't make any changes.
            if (dt.Ticks <= 0.0) return _lastControlOutputValue;

            // copy vars
            var input = ActualInput;
            var target = TargetInput;

            // calculate the error (how far we are from target)
            var error = target - input;
            //Debug.Print("Actual: " + ActualInput.ToString("N1") + ", Error: " + error.ToString("N1"));

            // if the error has passed 0, we should reset the integrator
            if (AutoResetIntegrator)
            {
                if ((_lastError <= 0 && error > 0) || (_lastError > 0 && error <= 0))
                {
                    ResetIntegrator();
                }
            }

            // calculate the proportional term
            var proportional = ProportionalGain * error;
            //Debug.Print("Proportional: " + proportional.ToString("N2"));

            // calculate the integral
            _integral += error * seconds; // add to the integral history
            var integral = IntegralGain * _integral; // calcuate the integral action

            // calculate the derivative (rate of change, slop of line) term
            var diff = error - _lastError / seconds;
            var derivative = DerivativeGain * diff;

            // add the appropriate corrections
            if ((correctionActions & PIDActionType.Proportional) == PIDActionType.Proportional) control += proportional;
            if ((correctionActions & PIDActionType.Integral) == PIDActionType.Integral) control += integral;
            if ((correctionActions & PIDActionType.Derivative) == PIDActionType.Derivative) control += derivative;

            //
            //Debug.Print("PID Control (preclamp): " + control.ToString("N4"));

            // clamp
            if (control > OutputMax) control = OutputMax;
            if (control < OutputMin) control = OutputMin;

            //Debug.Print("PID Control (postclamp): " + control.ToString("N4"));

            if (OutputTuningInformation)
            {
                Debug.Print("SP+PV+PID+O," + target.ToString() + "," + input.ToString() + "," +
                    proportional.ToString() + "," + integral.ToString() + "," +
                    derivative.ToString() + "," + control.ToString());
            }

            // persist our state variables
            _lastControlOutputValue = control;
            _lastError = error;
            _lastUpdateTime = now;

            return control;
        }
    }
}