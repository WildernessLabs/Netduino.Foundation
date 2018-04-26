using System;
using Microsoft.SPOT.Hardware;

namespace Netduino.Foundation.Servos
{
    /// <summary>
    ///     Implement a Servo class.
    /// </summary>
    public class Servo
    {
        #region Constants

        /// <summary>
        ///     Default frequency for the servo.
        /// </summary>
        public const double SERVO_FREQUENCY = 50;

        /// <summary>
        ///     Default minimum servo angle.
        /// </summary>
        public const int MINIMUM_ANGLE = 0;

        /// <summary>
        ///     Default maximum servo angle.
        /// </summary>
        public const int MAXIMUM_ANGLE = 180;

        #endregion

        #region Properties

        /// <Summary>
        ///     Determine if this instance of the servo class is associated with a digital output pin.
        /// </Summary>
        /// <Returns>true if the instance is attached to a PWM pin, false otherwise.</Returns>
        public bool Attached
        {
            get { return Pin != Cpu.PWMChannel.PWM_NONE; }
        }

        /// <summary>
        ///     PWM pin that the servo is attached to.
        /// </summary>
        /// <value>PWM pin connected to the servo.</value>
        private Cpu.PWMChannel Pin { get; set; }

        /// <summary>
        ///     Angle of the servo.
        /// </summary>
        /// <value>Angle of the servo in the range 0 to 180 degrees inclusive.</value>
        public double Angle
        {
            get
            {
                if (!Attached)
                {
                    throw new InvalidOperationException("Cannot read angle of servo when not attached.");
                }
                return _angle;
            }
            set
            {
                //if ((_angle < MINIMUM_ANGLE) || (_angle > MAXIMUM_ANGLE))
                //{
                //    throw new ArgumentOutOfRangeException("angle", "Servo angle out of range 0 to 180 degrees.");
                //}
                _angle = value;
                var pulseWidth = MinimumPulseWidth + (_angle * ((MaximumPulseWidth - MinimumPulseWidth) / 181));
                var dutyCycle = pulseWidth / 20000;
                if (PWMPin == null)
                {
                    PWMPin = new PWM(Pin, SERVO_FREQUENCY, dutyCycle, false);
                    PWMPin.Start();
                }
                else
                {
                    PWMPin.DutyCycle = dutyCycle;
                }
            }
        }
        protected double _angle;

        /// <summary>
        ///     PWM pin used to control the servo.
        /// </summary>
        private PWM PWMPin { get; set; }

        /// <summary>
        ///     Minimum pulse width for this servo.  This value normally represents 0 degrees.
        /// </summary>
        private int MinimumPulseWidth { get; set; }

        /// <summary>
        ///     Maximum pulse width for the servo.  This value normally represents 180 degrees.
        /// </summary>
        /// <value>Pulse width representing 180 degrees.</value>
        private int MaximumPulseWidth { get; set; }

        #endregion

        #region Constructor(s)

        /// <summary>
        ///     Initialise a new instance of the Servo class.  Use the default settings for the properties.
        /// </summary>
        /// <remarks>
        ///     Defaults are:
        ///     Not connected to a pin.
        ///     Minimum pulse width set to 544
        ///     Maximum pulse width set to 2400.
        ///     Minimum angle set to 0 degrees.
        ///     Maximum angle set to 180 degrees.
        /// </remarks>
        public Servo()
        {
            Pin = Cpu.PWMChannel.PWM_NONE;
            PWMPin = null;
            MinimumPulseWidth = 544;
            MaximumPulseWidth = 2400;
        }

        /// <summary>
        ///     Create a new instance of the Servo class.  This call is equivalent to creating a new instance and
        ///     then calling the <i>Attach</i> method.
        /// </summary>
        /// <param name="pin">PWM pin to which the servo is attached.</param>
        /// <param name="minimum">Minimum value for the pulse width, in microseconds, the default is 544.</param>
        /// <param name="maximum">Maximum value for the pulse width, in microseconds, the default value is 2400.</param>
        public Servo(Cpu.PWMChannel pin, int minimum = 544, int maximum = 2400)
        {
            Pin = pin;
            MinimumPulseWidth = minimum;
            MaximumPulseWidth = maximum;
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Set the angle of the servo to the specified number of degrees.
        /// </summary>
        /// <param name="angle">Angle for the servo which should be between 0 and 180 degrees inclusive.</param>
        private void Write(int angle)
        {
            Angle = angle;
        }

        /// <summary>
        ///     Read the angle of the servo.
        /// </summary>
        /// <returns>The read.</returns>
        private double Read()
        {
            return Angle;
        }

        /// <summary>
        ///     Detach the pin from the servo.
        /// </summary>
        private void Detach()
        {
            if (Attached)
            {
                PWMPin.Stop();
                PWMPin = null;
                Pin = Cpu.PWMChannel.PWM_NONE;
            }
        }

        /// <summary>
        ///     Stop the PWM pulse from this instance of the Servo class.
        /// </summary>
        private void Stop()
        {
            PWMPin.Stop();
        }

        /// <summary>
        ///     Set the pulse width to the specified number of microseconds
        /// </summary>
        /// <remarks>
        ///     This method is not currently supported.
        /// </remarks>
        /// <param name="microseconds">Microsecond pulse width.</param>
        private void WriteMicroseconds(int microseconds)
        {
            throw new NotImplementedException();
        }

        #endregion


        #region deprecated stuff


        /// <summary>
        ///     Attach the servo to a specific PWM pin and set the minimum and maximum pulse
        ///     widths for the 0 and 180 degree angles.
        /// </summary>
        /// <param name="pin">PWM pin to use for this servo.</param>
        /// <param name="minimum">Minimum pulse width, in microseconds, for the servo.  The minimum width define the value used for 0 degrees.</param>
        /// <param name="maximum">Maximum pulse width, in microseconds, for the servo.  The maximum value determines the value used for 180 degrees.</param>
        [Obsolete("Use the constructor, instead. This method is going away.")]
        private void Attach(Cpu.PWMChannel pin, int minimum = 544, int maximum = 2400)
        {
            Pin = pin;
            MinimumPulseWidth = minimum;
            MaximumPulseWidth = maximum;
        }


        #endregion
    }
}