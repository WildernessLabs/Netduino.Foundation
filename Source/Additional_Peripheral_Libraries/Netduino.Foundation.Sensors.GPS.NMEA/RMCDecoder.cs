using System;
namespace Netduino.Foundation.Sensors.GPS
{
    /// <summary>
    ///  Decode RMC - Recommended Minimum Specific GPS messages.
    /// </summary>
    public class RMCDecoder : NMEADecoder
    {
        #region Delegates and events.

        /// <summary>
        /// Delegate for the position update received event.
        /// </summary>
        /// <param name="positionCourseAndTime">Position, course and time information.</param>
        public delegate void PositionCourseAndTimeReceived(PositionCourseAndTime positionCourseAndTime);

        /// <summary>
        /// Position update received event.
        /// </summary>
        public event PositionCourseAndTimeReceived OnPositionCourseAndTimeReceived = null;

        #endregion Delegates and events.

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public RMCDecoder()
        {
        }

        #endregion Constructors

        #region NMEADecoder methods & properties

        /// <summary>
        /// Prefix for the GGA decoder.
        /// </summary>
        public override string Prefix { get { return ("$GPRMC"); } }

        /// <summary>
        /// Friendly name for the GGA messages.
        /// </summary>
        public override string Name { get { return ("Global Postioning System Fix Data"); } }

        /// <summary>
        /// Process the data from a GGA message.
        /// </summary>
        /// <param name="data">String array of the message components for a CGA message.</param>
        public override void Process(string[] data)
        {
            if (OnPositionCourseAndTimeReceived != null)
            {
                PositionCourseAndTime position = new PositionCourseAndTime();
                position.TimeOfReading = NMEAHelpers.TimeOfReading(data[9], data[1]);
                if (data[2].ToLower() == "a")
                {
                    position.Valid = true;
                }
                else
                {
                    position.Valid = false;
                }
                position.Latitude = NMEAHelpers.DegreesMinutesDecode(data[3], data[4]);
                position.Longitude = NMEAHelpers.DegreesMinutesDecode(data[5], data[6]);
                position.Speed = double.Parse(data[7]);
                position.Course = double.Parse(data[8]);
                if (data[10].ToLower() == "e")
                {
                    position.MagneticVariation = DirectionIndicator.East;
                }
                else
                {
                    position.MagneticVariation = DirectionIndicator.West;
                }
                OnPositionCourseAndTimeReceived(position);
            }
        }

        #endregion NMEADecoder methods & properties
    }
}
