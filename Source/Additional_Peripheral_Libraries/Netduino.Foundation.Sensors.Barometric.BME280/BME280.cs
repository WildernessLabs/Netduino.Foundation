using System;
using Netduino.Foundation.Devices;

namespace Netduino.Foundation.Sensors.Barometric
{
	/// <summary>
	/// BME280 Temperature, Pressure and Humidity Sensor.
	/// </summary>
	/// <remarks>
	/// This class implements the functionality necessary to read the temperature, pressure and humidity
	/// from the Bosch BME280 sensor.
	/// </remarks>
	public class BME280
    {
        #region Enums

        /// <summary>
		/// Registers used to control the BME280.
		/// </summary>
		private enum Registers : byte { Humidity = 0xf2, Status = 0xf3, Measurement = 0xf4, Configuration = 0xf5, Reset = 0xe0 };

        /// <summary>
        /// Valid oversampling values.
        /// </summary>
        /// <remarks>
        ///     000 - Data output set to 0x8000
        ///     001 - Oversampling x1
        ///     010 - Oversampling x2
        ///     011 - Oversampling x4
        ///     100 - Oversampling x8
        ///     101, 110, 111 - Oversampling x16
        /// </remarks>
        public enum Oversample : byte { Skip = 0, OversampleX1, OversampleX2, OversampleX4, OversampleX8, OversampleX16 };

        /// <summary>
        /// Valid values for the operating mode of the sensor.
        /// </summary>
        /// <remarks>
        ///     00 - Sleep mode
        ///     01 and 10 - Forced mode
        ///     11 - Normal mode
        /// </remarks>
        public enum Modes : byte { Sleep = 0, Forced = 1, Normal = 3 };

        /// <summary>
        /// Valid values for the inactive duration in normal mode.
        /// </summary>
        /// <remarks>
        ///     000 - 0.5 milliseconds
        ///     001 - 62.5 milliseconds
        ///     010 - 125 milliseconds
        ///     011 - 250 milliseconds
        ///     100 - 500 milliseconds
        ///     101 - 1000 milliseconds
        ///     110 - 10 milliseconds
        ///     111 - 20 milliseconds
        ///     
        /// See section 3.4 of the datasheet.
        /// </remarks>
        public enum StandbyDuration : byte { MsHalf = 0, Ms62Half, Ms125, Ms250, Ms500, Ms1000, Ms10, Ms20 };

        /// <summary>
        /// Valid filter co-efficient values.
        /// </summary>
        public enum FilterCoefficient : byte { Off = 0, Two, Four, Eight, Sixteen };

        #endregion Enums

        #region Internal Structures

        /// <summary>
		/// Compensation data.
		/// </summary>
		private struct CompensationData
		{
			public ushort T1;
			public short T2;
			public short T3;
			public ushort P1;
			public short P2;
			public short P3;
			public short P4;
			public short P5;
			public short P6;
			public short P7;
			public short P8;
			public short P9;
			public byte H1;
			public short H2;
			public byte H3;
			public short H4;
			public short H5;
			public sbyte H6;
		};

		#endregion Internal Structures

		#region Member Variables

		/// <summary>
		/// Communication bus used to read and write to the BME280 sensor.
		/// </summary>
		/// <remarks>
		/// The BME has both I2C and SPI interfaces. The ICommunicationBus allows the
		/// selection to be made in the constructor.
		/// </remarks>
		ICommunicationBus _bme280 = null;

		/// <summary>
		/// Compensation data from the sensor.
		/// </summary>
		CompensationData _compensationData = new CompensationData();

		#endregion Member Variables

		#region Properties

		/// <summary>
		/// Temperature reading from the sensor.
		/// </summary>
		/// <value>Current temperature reading from the sensor in degrees C.</value>
		public float Temperature { get; private set; }

		/// <summary>
		/// Pressure reading from the sensor.
		/// </summary>
		/// <value>Current pressure reading from the sensor in Pascals (divide by 100 for hPa).</value>
		public float Pressure { get; private set; }

		/// <summary>
		/// Humidity reading from the sensor.
		/// </summary>
		/// <value>Current humidity reading from the sensor as a percentage.</value>
		public float Humidity { get; private set; }

		/// <summary>
		/// Temperature over sampling configuration.
		/// </summary>
        public Oversample TemperatureOverSampling { get; set; }

		/// <summary>
		/// Pressure over sampling configuration.
		/// </summary>
        /// <remarks>
        public Oversample PressureOversampling { get; set; }

		/// <summary>
		/// Humidity over sampling configuration.
		/// </summary>
        public Oversample HumidityOverSampling { get; set; }

		/// <summary>
		/// Set the operating mode for the sensor.
		/// </summary>
		public Modes Mode { get; set; }

		/// <summary>
        /// Set the standby period for the sensor.
		/// </summary>
		public StandbyDuration Standby { get; set; }

		/// <summary>
        /// Determine the time constant for the IIR filter.
		/// </summary>
        /// <remarks>
        /// See section 3.44 of the datasheet for more informaiton.
        /// </remarks>
        public FilterCoefficient Filter { get; set; }

		#endregion Properties

		#region Constructors

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Netduino.Foundation.Sensors.Barometric.BME280"/> class.
		/// </summary>
		/// <remarks>
		/// This constructor is private to force the use of the constructor which defines the 
		/// communication parameters for the sensor.
		/// </remarks>
		private BME280()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Netduino.Foundation.Sensors.Barometric.BME280"/> class.
		/// </summary>
		/// <param name="address">I2C address of the sensor (default = 0x77).</param>
		/// <param name="speed">Speed of the I2C bus (default = 100KHz).</param>
		public BME280(byte address = 0x77, ushort speed = 100)
		{
			if ((address != 0x76) && (address != 0x77))
			{
				throw new ArgumentOutOfRangeException("address", "Address should be 0x76 or 0x77");
			}
			if ((speed < 10) || (speed > 3400))
			{
				throw new ArgumentOutOfRangeException("speed", "Speed should be 10 KHz to 3,400 KHz.");
			}

			_bme280 = (ICommunicationBus) (new I2CBus(address, speed));
			ReadCompensationData();
			//
			//  Update the configuration information and start sampling.
			//
            TemperatureOverSampling = Oversample.OversampleX1;
            PressureOversampling = Oversample.OversampleX1;
			HumidityOverSampling = Oversample.OversampleX1;
			Mode = Modes.Normal;
            Filter = FilterCoefficient.Off;
            Standby = StandbyDuration.MsHalf;
			UpdateConfiguration();
		}

		#endregion Constructors

		#region Methods

		/// <summary>
		/// Update the configuration for the BME280.
		/// </summary>
		/// <remarks>
		/// This method uses the data in the configuration properties in order to set up the
		/// BME280.  Ensure that the following are set correctly before calling this method:
		///     - Standby
		///     - Filter
		///     - HumidityOverSampling
		///     - TemperatureOverSampling
		///     - PressureOverSampling
		///     - Mode
		/// </remarks>
		public void UpdateConfiguration()
		{
			//
			//  Put to sleep to allow the configuration to be changed.
			//
			_bme280.WriteRegister((byte) Registers.Measurement, 0x00);

			byte data = (byte) (((((byte) Standby) << 5) & 0xe0) | ((((byte) Filter) << 2) & 0x1c));
			_bme280.WriteRegister((byte) Registers.Configuration, data);
			data = (byte) (((byte) HumidityOverSampling) & 0x07);
			_bme280.WriteRegister((byte) Registers.Humidity, data);
			data = (byte) (((((byte) TemperatureOverSampling) << 5) & 0xe0) |
                   ((((byte) PressureOversampling) << 2) & 0x1c) |
				   (((byte) Mode) & 0x03));
			_bme280.WriteRegister((byte) Registers.Measurement, data);
		}

		/// <summary>
		/// Reset the sensor.
		/// </summary>
		/// <remarks>
		/// Perform a full power-on-reset of the sensor and reset the configuration of the sensor.
		/// </remarks>
		public void Reset()
		{
			_bme280.WriteRegister((byte) Registers.Reset, 0xb6);
			UpdateConfiguration();
		}

		/// <summary>
		/// Reads the compensation data.
		/// </summary>
		/// <remarks>
		/// The compensation data is written to the chip at the time of manufacture and cannot be changed.
		/// This information is used to convert the readings from the sensor into actual temperature,
		/// pressure and humidity readings.
		/// 
		/// From the data sheet, the register addresses and length are:
		///     Temperature and pressure: start address 0x88, end address 0x9F (length = 24)
		///     Humidity 1: 0xa1, length = 1
		///     Humidity 2 and 3: start address 0xe1, end address 0xe7, (length = 8)
		/// </remarks>
		private void ReadCompensationData()
		{
			byte[] temperatureAndPressureData = _bme280.ReadRegisters(0x88, 24);
			byte[] humidityData1 = _bme280.ReadRegisters(0xa1, 1);
			byte[] humidityData2To6 = _bme280.ReadRegisters(0xe1, 7);

			_compensationData.T1 = (ushort)(temperatureAndPressureData[0] + (temperatureAndPressureData[1] << 8));
			_compensationData.T2 = (short)(temperatureAndPressureData[2] + (temperatureAndPressureData[3] << 8));
			_compensationData.T3 = (short)(temperatureAndPressureData[4] + (temperatureAndPressureData[5] << 8));
			//
			_compensationData.P1 = (ushort)(temperatureAndPressureData[6] + (temperatureAndPressureData[7] << 8));
			_compensationData.P2 = (short)(temperatureAndPressureData[8] + (temperatureAndPressureData[9] << 8));
			_compensationData.P3 = (short)(temperatureAndPressureData[10] + (temperatureAndPressureData[11] << 8));
			_compensationData.P4 = (short)(temperatureAndPressureData[12] + (temperatureAndPressureData[13] << 8));
			_compensationData.P5 = (short)(temperatureAndPressureData[14] + (temperatureAndPressureData[15] << 8));
			_compensationData.P6 = (short)(temperatureAndPressureData[16] + (temperatureAndPressureData[17] << 8));
			_compensationData.P7 = (short)(temperatureAndPressureData[18] + (temperatureAndPressureData[19] << 8));
			_compensationData.P8 = (short)(temperatureAndPressureData[20] + (temperatureAndPressureData[21] << 8));
			_compensationData.P9 = (short)(temperatureAndPressureData[22] + (temperatureAndPressureData[23] << 8));
			//
			_compensationData.H1 = humidityData1[0];
			_compensationData.H2 = (short)(humidityData2To6[0] + (humidityData2To6[1] << 8));
			_compensationData.H3 = humidityData2To6[2];
			_compensationData.H4 = (short)((humidityData2To6[3] << 4) + (humidityData2To6[4] & 0xf));
			_compensationData.H5 = (short)(((humidityData2To6[4] & 0xf) >> 4) + (humidityData2To6[5] << 4));
			_compensationData.H6 = (sbyte)humidityData2To6[6];
		}

		/// <summary>
		/// Read the sensor information from the BME280.
		/// </summary>
		/// <remarks>
		/// Reads the raw temperature, pressure and humidity data from the BME280 and applies
		/// the compensation data to get the actual readings.  These are made available through the
		/// Temperature, Pressure and Humidity properties.
		/// 
		/// All three readings are taken at once to ensure that the three readings are consistent.
		/// 
		/// Reegister locations and formulas taken from the Bosch BME280 datasheet revision 1.1, May 2015.
		///     Register locations - section 5.3 Memory Map
		///     Formulas - section 4.2.3 Compensation Formulas
		/// 
		/// The integer formulas have been used to try and keep the calcuations performant.
		/// </remarks>
		public void Read()
		{
			byte[] readings = _bme280.ReadRegisters(0xf7, 8);
			//
			//  Temperature calculation from section 4.2.3 of the datasheet.
			//
			// Returns temperature in DegC, resolution is 0.01 DegC. Output value of “5123” equals 51.23 DegC.
			// t_fine carries fine temperature as global value:
			//
			// BME280_S32_t t_fine;
			// BME280_S32_t BME280_compensate_T_int32(BME280_S32_t adc_T)
			// {
			//     BME280_S32_t var1, var2, T;
			//     var1 = ((((adc_T>>3) - ((BME280_S32_t)dig_T1<<1))) * ((BME280_S32_t)dig_T2)) >> 11;
			//     var2 = (((((adc_T>>4) - ((BME280_S32_t)dig_T1)) * ((adc_T>>4) - ((BME280_S32_t)dig_T1))) >> 12) *
			//     ((BME280_S32_t)dig_T3)) >> 14;
			//     t_fine = var1 + var2;
			//     T = (t_fine * 5 + 128) >> 8;
			//     return T;
			// }
			//
			int adcTemperature = (readings[3] << 12) | (readings[4] << 4) | ((readings[5] >> 4) & 0x0f);
			int tvar1 = (((adcTemperature >> 3) - (((int)_compensationData.T1) << 1)) * ((int)_compensationData.T2)) >> 11;
			int tvar2 = (((((adcTemperature >> 4) - ((int)_compensationData.T1)) *
						((adcTemperature >> 4) - _compensationData.T1)) >> 12) * ((int)_compensationData.T3)) >> 14;
			int tfine = tvar1 + tvar2;
			//
			Temperature = ((float)((tfine * 5 + 128) >> 8)) / 100;
			//
			// Pressure calculation from section 4.2.3 of the datasheet.
			//
			// Returns pressure in Pa as unsigned 32 bit integer in Q24.8 format (24 integer bits and 8 fractional bits).
			// Output value of “24674867” represents 24674867/256 = 96386.2 Pa = 963.862 hPa
			//
			// BME280_U32_t BME280_compensate_P_int64(BME280_S32_t adc_P)
			// {
			//     BME280_S64_t var1, var2, p;
			//     var1 = ((BME280_S64_t)t_fine) - 128000;
			//     var2 = var1 * var1 * (BME280_S64_t)dig_P6;
			//     var2 = var2 + ((var1*(BME280_S64_t)dig_P5)<<17);
			//     var2 = var2 + (((BME280_S64_t)dig_P4)<<35);
			//     var1 = ((var1 * var1 * (BME280_S64_t)dig_P3)>>8) + ((var1 * (BME280_S64_t)dig_P2)<<12);
			//     var1 = (((((BME280_S64_t)1)<<47)+var1))*((BME280_S64_t)dig_P1)>>33;
			//     if (var1 == 0)
			//     {
			//         return 0; // avoid exception caused by division by zero
			//     }
			//     p = 1048576-adc_P;
			//     p = (((p<<31)-var2)*3125)/var1;
			//     var1 = (((BME280_S64_t)dig_P9) * (p>>13) * (p>>13)) >> 25;
			//     var2 = (((BME280_S64_t)dig_P8) * p) >> 19;
			//     p = ((p + var1 + var2) >> 8) + (((BME280_S64_t)dig_P7)<<4);
			//     return (BME280_U32_t)p;
			// }
			//
			long pvar1 = tfine - 128000;
			long pvar2 = pvar1 * pvar1 * _compensationData.P6;
			pvar2 += (pvar1 * (long)_compensationData.P5) << 17;
			pvar2 += ((long)_compensationData.P4) << 35;
			pvar1 = ((pvar1 * pvar1 * (long)_compensationData.P8) >> 8) + ((pvar1 * (long)_compensationData.P2) << 12);
			pvar1 = (((((long)1) << 47) + pvar1)) * ((long)_compensationData.P1) >> 33;
			if (pvar1 == 0)
			{
				Pressure = 0;
			}
			else
			{
				int adcPressure = (readings[0] << 12) | (readings[1] << 4) | ((readings[2] >> 4) & 0x0f);
				long pressure = 1048576 - adcPressure;
				pressure = (((pressure << 31) - pvar2) * 3125) / pvar1;
				pvar1 = (((long)_compensationData.P9) * (pressure >> 13) * (pressure >> 13)) >> 25;
				pvar2 = (((long)_compensationData.P8) * pressure) >> 19;
				pressure = ((pressure + pvar1 + pvar2) >> 8) + (((long)_compensationData.P7) << 4);
				//
				Pressure = ((float)pressure) / 256;
			}
			//
			// Humidity calculations from section 4.2.3 of the datasheet.
			//
			// Returns humidity in %RH as unsigned 32 bit integer in Q22.10 format (22 integer and 10 fractional bits).
			// Output value of “47445” represents 47445/1024 = 46.333 %RH
			//
			// BME280_U32_t bme280_compensate_H_int32(BME280_S32_t adc_H)
			// {
			//     BME280_S32_t v_x1_u32r;
			//     v_x1_u32r = (t_fine - ((BME280_S32_t)76800));
			//     v_x1_u32r = (((((adc_H << 14) - (((BME280_S32_t)dig_H4) << 20) - (((BME280_S32_t)dig_H5) * v_x1_u32r)) +
			//         ((BME280_S32_t)16384)) >> 15) * (((((((v_x1_u32r * ((BME280_S32_t)dig_H6)) >> 10) * (((v_x1_u32r *
			//         ((BME280_S32_t)dig_H3)) >> 11) + ((BME280_S32_t)32768))) >> 10) + ((BME280_S32_t)2097152)) *
			//         ((BME280_S32_t)dig_H2) + 8192) >> 14));
			//     v_x1_u32r = (v_x1_u32r - (((((v_x1_u32r >> 15) * (v_x1_u32r >> 15)) >> 7) * ((BME280_S32_t)dig_H1)) >> 4));
			//     v_x1_u32r = (v_x1_u32r < 0 ? 0 : v_x1_u32r);
			//     v_x1_u32r = (v_x1_u32r > 419430400 ? 419430400 : v_x1_u32r);
			//     return (BME280_U32_t)(v_x1_u32r>>12);
			// }
			//
			int adcHumidity = (readings[6] << 8) | readings[7];
			int v_x1_u32r = (tfine - ((int)76800));

			v_x1_u32r = (((((adcHumidity << 14) - (((int)_compensationData.H4) << 20) - (((int)_compensationData.H5) * v_x1_u32r)) +
						((int)16384)) >> 15) * (((((((v_x1_u32r * ((int)_compensationData.H6)) >> 10) * (((v_x1_u32r * ((int)_compensationData.H3)) >> 11) + ((int)32768))) >> 10) + ((int)2097152)) *
						((int)_compensationData.H2) + 8192) >> 14));
			v_x1_u32r = (v_x1_u32r - (((((v_x1_u32r >> 15) * (v_x1_u32r >> 15)) >> 7) * ((int)_compensationData.H1)) >> 4));

			//v_x1_u32r = (((((adcHumidity << 14) - (((int) _compensationData.H4) << 20) - (((int) _compensationData.H5) * v_x1_u32r)) +
			//            ((int) 16384)) >> 15) * (((((((v_x1_u32r * ((int) _compensationData.H6)) >> 10) * (((v_x1_u32r *
			//            ((int) _compensationData.H3)) >> 11) + ((int) 32768))) >> 10) + ((int) 2097152)) *
			//            ((int) _compensationData.H2) + 8192) >> 14));
			//v_x1_u32r = (v_x1_u32r - (((((v_x1_u32r >> 15) * (v_x1_u32r >> 15)) >> 7) * ((int) _compensationData.H1)) >> 4));
			//
			//  Makes sure the humidity reading is in the range [0..100].
			//
			v_x1_u32r = (v_x1_u32r < 0 ? 0 : v_x1_u32r);
			v_x1_u32r = (v_x1_u32r > 419430400 ? 419430400 : v_x1_u32r);
			//
			Humidity = ((float)(v_x1_u32r >> 12)) / 1024;
		}

		#endregion Methods
	}
}
