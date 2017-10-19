using System;
using Netduino.Foundation;

namespace Netduino.Foundation.Sensors.GPS
{
    /// <summary>
    /// Decoder for GGA messages.
    /// </summary>
    public class GGADecoder : NMEADecoder
    {
        #region Delegates and events.

        /// <summary>
        /// Delegate for the position update received event.
        /// </summary>
        /// <param name="location">Location data received.</param>
        public delegate void PositionReceived(GPSLocation location);

        /// <summary>
        /// Position update received event.
        /// </summary>
        public event PositionReceived OnPositionReceived = null;

        #endregion Delegates and events.

        #region Constructors

        /// <summary>
        /// Default constructors
        /// </summary>
        public GGADecoder()
        {
        }

        #endregion Constructors

        #region NMEADecoder methods & properties

        /// <summary>
        /// Prefix for the GGA decoder.
        /// </summary>
        public override string Prefix { get {return ("$GPGGA"); } }

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
            if (OnPositionReceived != null)
            {
                try
                {
                    GPSLocation location = new GPSLocation();
                    location.ReadingTime = NMEAHelpers.TimeOfReading(null, data[1]);
                    location.Latitude = NMEAHelpers.DegreesMinutesDecode(data[2], data[3]);
                    location.Longitude = NMEAHelpers.DegreesMinutesDecode(data[4], data[5]);
//                    location.FixQuality = (FixType) Helpers.IntegerOrDefault(data[6]);
//                    location.NumberOfSatellites = Helpers.IntegerOrDefault(data[7]);
//                    location.HorizontalDilutionOfPrecision = Helpers.DoubleOrDefault(data[8]);
//                    location.Altitude = Helpers.DoubleOrDefault(data[9]);
                    OnPositionReceived(location);
                }
                catch (Exception ex)
                {
                    //  Throw the exception away, the data will not generate an event.
                }
            }
        }

        #endregion NMEADecoder methods & properties
    }
}
