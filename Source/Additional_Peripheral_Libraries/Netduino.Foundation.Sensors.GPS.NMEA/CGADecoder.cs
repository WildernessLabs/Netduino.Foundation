using System;
using Netduino.Foundation.Sensors.GPS;

namespace Netduino.Foundation.Sensors.GenericGPS
{
	/// <summary>
	/// Decoder for CGA messages.
	/// </summary>
	public class CGADecoder : INMEADecoder
	{
		#region Constructors

		public CGADecoder()
		{
		}

		#endregion Constructors

		#region Methods

		/// <summary>
		/// Process the data from a CGA message.
		/// </summary>
		/// <param name="data">String array of the message components for a CGA message.</param>
		public void Process(string[] data)
		{
		}

		/// <summary>
		/// Friendly name for the CGA messages.
		/// </summary>
		/// <returns>The name.</returns>
		public string GetName()
		{
			return ("Global Postioning System Fixed Data");
		}

		/// <summary>
		/// Prefix for the CGA decoder.
		/// </summary>
		/// <returns>The prefix.</returns>
		public string GetPrefix()
		{
			return ("$GPCGA");
		}

		#endregion Methods
	}
}
