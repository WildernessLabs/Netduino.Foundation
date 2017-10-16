using System;

namespace Netduino.Foundation.Sensors.GPS
{
	/// <summary>
	/// Hold the location taken from a GPS reading.
	/// </summary>
	public class GPSLocation
	{
		#region Enums

		/// <summary>
		/// Direction indicator.
		/// </summary>
		public enum DirectionIndicator { North, South, East, West };

		#endregion Enums

		/// <summary>
		/// Position recorded in degrees, minutes and seconds.
		/// </summary>
		public struct DMPosition
		{
			public int Degrees;
			public double Minutes;
			public DirectionIndicator Direction;
		}

		#region Properties

		/// <summary>
		/// Latitude of the reading.
		/// </summary>
		public DMPosition Latitude { get; set; }

		/// <summary>
		/// Longitude of the reading.
		/// </summary>
		public DMPosition Longitude { get; set; }

		#endregion Properties

		#region Constructors

		/// <summary>
		/// Default constructor is private to prevent it being used.
		/// </summary>
		private GPSLocation()
		{
		}

		/// <summary>
		/// Create a new GPSLocation by decoding the GPS strings.
		/// </summary>
		/// <param name="latitude">Latitude of the reading in the format ddmm.mmmm</param>
		/// <param name="northSouth">North or south indicator (N/S)</param>
		/// <param name="longitude">Longitude of the reading in the format dddmm.mmmm</param>
		/// <param name="eastWest">East / west indicator (E/W).</param>
		public GPSLocation(string latitude, string northSouth, string longitude, string eastWest)
		{
			Latitude = NMEAHelpers.DegreesMinutesDecode(latitude, northSouth);
			Longitude = NMEAHelpers.DegreesMinutesDecode(longitude, eastWest);
		}

		#endregion Constructors
	}
}
