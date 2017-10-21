using System;
using Microsoft.SPOT;
using Netduino.Foundation.Devices;

namespace Netduino.Foundation.Sensors.Barometric
{
	/// <summary>
	/// Provide access to the SI7021 temperature and humidity sensor.
	/// </summary>
	public class SI7021
	{
		#region Member variables / fields

		/// <summary>
		/// SI7021 is an I2C device.
		/// </summary>
		private I2CBus _si7021 = null;

		#endregion Member variables / fields

		#region Enums

		/// <summary>
		/// Device registers.
		/// </summary>
		private enum Registers : byte
		{
			MeasureHumidityWithHold = 0xe5, MeasureHumidityNoHold = 0xf5,
			MeasureTemperatureWithHold = 0xe3, MeasureTemperatureNoHold = 0xf3,
			ReadPreviousTemperatureMeasurement = 0xe0, Reset = 0xfe,
			WriteUserRegister1 = 0xe6, WriteUserRegister2 = 0xe7,
			ReadIDFirstBytePart1 = 0xfa, ReadIDFirstBytePart2 = 0x0f,
			ReadIDSecondBytePart1 = 0xfc, ReadIDSecondBytePart2 = 0xc9,
			ReadFirmwareRevisionPart1 = 0x84, ReadFirmwareRevisionPart2 = 0xb8
		}

		#endregion Enums

		#region Properties

		/// <summary>
		/// Relative humidity.
		/// </summary>
		/// <remarks>
		/// This value is only valid after a call to Read.
		/// </remarks>
		public float Humidity { get; private set; }

		/// <summary>
		/// Temperature.
		/// </summary>
		/// <remarks>
		/// This value is only valid after a call to Read.
		/// </remarks>
		public float Temperature { get; private set; }

		#endregion Properties

		#region Constructors

		/// <summary>
		/// Default constructor (private to prevent the user from calling this).
		/// </summary>
		private SI7021()
		{
		}

		/// <summary>
		/// Create a new SI7021 temperature and humidity sensor.
		/// </summary>
		/// <param name="address">Sensor address (default to 0x40).</param>
		/// <param name="speed">Speed of the I2C interface (default to 100 KHz).</param>
		public SI7021(byte address = 0x40, ushort speed = 100)
		{
			_si7021 = new I2CBus(address, speed);
			Read();
		}

		#endregion Constructors

		#region Methods

		/// <summary>
		/// Make a temperature and humidity reading.
		/// </summary>
		public void Read()
		{
		}

		/// <summary>
		/// Reset the sensor and take a fresh reading.
		/// </summary>
		public void Reset()
		{
		}

		#endregion Methods
	}
}
