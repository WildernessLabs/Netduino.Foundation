using System;

namespace Netduino.Foundation.Sensors.GPS
{
	/// <summary>
	/// Provide common functionality for the decode classes.
	/// </summary>
	public class NMEAHelpers
	{
		#region Constructors

		/// <summary>
		/// Default constructor for the NMEA Helpers class.
		/// </summary>
		public NMEAHelpers()
		{
		}

		#endregion Constructors

		#region Methods

		/// <summary>
		/// Extract the time of the reading.
		/// </summary>
		/// <param name="time">String containing the time of the reading in the format hhmmss.sss</param>
		/// <returns>DateTime object containing the time.</returns>
		public static DateTime TimeOfReading(string time)
		{
			double t = 0;
			if (double.TryParse(time, out t))
			{
				int hour = (int) (t / 10000);
				int minute = (int) ((t - (hour * 10000)) / 100);
				int second = (int) (t - (hour * 10000) - (minute * 100));
				int milliseconds = (int) (t - (int) t) * 100;
				
				return (new DateTime(1964, 9, 1, hour, minute, second, milliseconds));
			}
			else
			{
				throw new ArgumentException("Unable to decode the time");
			}
		}

		/// <summary>
		/// Decode the degrees / minutes location and return a DMPosition.
		/// </summary>
		/// <param name="location">Location in the format dddmm.mmmm or ddmm.mmmm</param>
		/// <param name="direction">Direction of the reading, one of N, S, E, W.</param>
		/// <exception cref="ArgumentException">Throw if the location string cannot be decoded.</exception>
		/// <returns>DMPosition in degrees and minutes.</returns>
		public static GPSLocation.DMPosition DegreesMinutesDecode(string location, string direction)
		{
			double loc = 0;
			var position = new GPSLocation.DMPosition();

			if (double.TryParse(location, out loc))
			{
				position.Degrees = (int) (loc / 100);
				position.Minutes = (loc - (position.Degrees * 100));
				switch (direction.ToLower())
				{
					case "n":
						position.Direction = GPSLocation.DirectionIndicator.North;
						break;
					case "s":
						position.Direction = GPSLocation.DirectionIndicator.South;
						break;
					case "e":
						position.Direction = GPSLocation.DirectionIndicator.East;
						break;
					case "w":
						position.Direction = GPSLocation.DirectionIndicator.West;
						break;
					default:
						throw new ArgumentException("Invalid direction");	
				}
			}
			else
			{
				throw new ArgumentException("Invalid location");
			}
			return (position);
		}

		#endregion Methods

	}
}
