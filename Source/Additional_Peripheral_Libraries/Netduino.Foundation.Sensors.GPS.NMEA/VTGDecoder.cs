using System;
using Netduino.Foundation;

namespace Netduino.Foundation.Sensors.GPS
{
    /// <summary>
    /// Provice a mechanism for dealing with VTG messages from a GPS receiver.
    /// </summary>
    public class VTGDecoder : NMEADecoder
    {
        #region Delegates and events

        /// <summary>
        /// Delegate for the Course and Velocity events.
        /// </summary>
        /// <param name="courseAndVelocity"></param>
        public delegate void CourseAndVelocityReceived(CourseOverGround courseAndVelocity);

        /// <summary>
        /// Event to be raised when a course and velocity message is received and decoded.
        /// </summary>
        public event CourseAndVelocityReceived OnCourseAndVelocityReceived = null;

        #endregion Delegates and events

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public VTGDecoder()
        {
        }

        #endregion Constructors

        #region NMEADecoder methods & properties

        /// <summary>
        /// Prefix for the VTG decoder.
        /// </summary>
        public override string Prefix { get {return ("$GPVTG"); } }

        /// <summary>
        /// Friendly name for the VTG messages.
        /// </summary>
        public override string Name { get { return ("Velocity made good"); } }

        /// <summary>
        /// Process the data from a VTG message.
        /// </summary>
        /// <param name="data">String array of the message components for a VTG message.</param>
        public override void Process(string[] data)
        {
            if (OnCourseAndVelocityReceived != null)
            {
                CourseOverGround course = new CourseOverGround();
//                course.TrueHeading = Helpers.DoubleOrDefault(data[1]);
//                course.MagneticHeading = Helpers.DoubleOrDefault(data[3]);
//               course.Knots = Helpers.DoubleOrDefault(data[5]);
//                course.KPH = Helpers.DoubleOrDefault(data[7]);
                OnCourseAndVelocityReceived(course);
            }
        }

        #endregion NMEADecoder methods & properties 
    }
}
