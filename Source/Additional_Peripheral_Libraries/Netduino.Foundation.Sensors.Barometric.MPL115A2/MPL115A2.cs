using System;
using Microsoft.SPOT;
using Netduino.Foundation.Devices;
using System.Threading;

namespace Netduino.Foundation.Sensors.Barometric
{
	public class MPL115A2
	{
		#region Enums

		/// <summary>
		/// Device registers.
		/// </summary>
		private enum Registers : byte
		{
			PressureMSB = 0x00, PressureLSB = 0x01, TemperatureMSB = 0x02, TemperatureLSB = 0x03,
			A0MSB = 0x04, A0LSB = 0x05, B1MSB = 0x06, B1LSB = 0x07, B2MSB = 0x08, B2LSB = 0x09,
			C12MSB = 0x0a, C12LSB = 0x0b, StartConversion = 0x12
		}

		#endregion Enums

		#region Structures

		/// <summary>
		/// Structure holding the floating point equivalent of the compensation 
		/// coefficients for the sensor.
		/// </summary>
		struct Coefficients
		{
			public float A0;
			public float B1;
			public float B2;
			public float C12;
		}

		#endregion Structures

		#region Member variables / fields

		/// <summary>
		/// SI7021 is an I2C device.
		/// </summary>
		private ICommunicationBus _mpl115a2 = null;

		/// <summary>
		/// Floating point variants of the compensation coefficients from the sensor.
		/// </summary>
		private Coefficients _coefficients = new Coefficients();

		#endregion Member variables / fields

		#region Properties

		/// <summary>
		/// Air pressure (kPa)
		/// </summary>
		/// <remarks>
		/// This value is only valid after a call to Read.
		/// </remarks>
		public float Pressure { get; private set; }

		#endregion Properties

		#region Constructors

		/// <summary>
		/// Default constructor (private to prevent the user from calling this).
		/// </summary>
		private MPL115A2()
		{
		}

		/// <summary>
		/// Create a new MPL115A2 temperature and humidity sensor object.
		/// </summary>
		/// <param name="address">Sensor address (default to 0x60).</param>
		/// <param name="speed">Speed of the I2C interface (default to 100 KHz).</param>
		public MPL115A2(byte address = 0x60, ushort speed = 100)
		{
			I2CBus device = new I2CBus(address, speed);
			_mpl115a2 = (ICommunicationBus)device;
			//
			//  Read the compensation data from the sensor.  The location and format of the
			//  compensation data can be found on pages 5 and 6 of the datasheet.
			//
			byte[] data = _mpl115a2.ReadRegisters((byte)Registers.A0MSB, 8);
			short a0 = (short)((ushort)((data[0] << 8) + data[1]));
			short b1 = (short)((ushort)((data[2] << 8) + data[3]));
			short b2 = (short)((ushort)((data[4] << 8) + data[5]));
			short c12 = (short)((ushort)((data[6] << 8) + data[7]));
			//
			//  Convert the raw compensation coefficients from the sensor into the
			//  floating point equivalents to speed up the calculations when readings
			//  are made.
			//
			//  Datasheet, section 3.1
			//  a0 is signed with 12 integer bits followed by 3 fractional bits so divide by 2^3 (8)
			//
			_coefficients.A0 = ((float)a0) / 8;
			//
			//  b1 is 2 integer bits followed by 7 fractional bits.  The lower bits are all 0
			//  so the format is:
			//      sign i1 I0 F12...F0
			//
			//  So we need to divide by 2^13 (8192)
			//
			_coefficients.B1 = ((float)b1) / 8192;
			//
			//  b2 is signed integer (1 bit) followed by 14 fractional bits so divide by 2^14 (16384).
			//
			_coefficients.B2 = ((float)b2) / 16384;
			//
			//  c12 is signed with no integer bits but padded with 9 zeroes:
			//      sign 0.000 000 000 f12...f0
			//
			//  So we need to divide by 2^22 (4,194,304) - 13 floating point bits 
			//  plus 9 leading zeroes.
			//
			_coefficients.C12 = ((float)c12) / 4194304;
			//
			//  Now make the first measurement.
			//
			Read();
		}

		#endregion

		#region Methods

		/// <summary>
		/// Read the temperature and pressure from the sensor and set the Pressure property.
		/// </summary>
		public void Read()
		{
			//
			//  Tell the sensor to take a temperature and pressure reading, wait for
			//  3ms (see section 2.2 of the datasheet) and then read the ADC values.
			//
			_mpl115a2.WriteBytes(new byte[] { (byte)Registers.StartConversion, 0x00 });
			Thread.Sleep(5);
			byte[] data = _mpl115a2.ReadRegisters((byte)Registers.PressureMSB, 4);
			//
			//  Extract the sensor data, note that this is a 10-bit reading so move
			//  the data right 6 bits (see section 3.1 of the datasheet).
			//
			ushort pressure = (ushort)(((data[0] << 8) + data[1]) >> 6);
			ushort temperature = (ushort)(((data[2] << 8) + data[3]) >> 6);
			//
			//  Now use the calculations in section 3.2 to determine the
			//  current pressure reading.
			//
			const float PRESSURE_CONSTANT = (float)(65.0 / 1023.0);
			float compensatedPressure = _coefficients.A0 + (_coefficients.B1 + (_coefficients.C12 * temperature))
					* pressure + (_coefficients.B2 * temperature);
			Pressure = (PRESSURE_CONSTANT * compensatedPressure) + 50;
		}

		#endregion Methods
	}
}
