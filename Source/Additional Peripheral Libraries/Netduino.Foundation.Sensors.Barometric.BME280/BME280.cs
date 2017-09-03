using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;

namespace Netduino.Foundation.Sensors.Barometric
{
    /// <summary>
    /// BME280 Temperature, Pressure and Humidity Sensor.
    /// </summary>
    /// <remarks>
    /// This class implements the functionality necessary to read the temperature, pressure and humidty
    /// from the Bosch BME280 sensor.
    /// </remarks>
    public class BME280
    {
        #region Constants

        /// <summary>
        /// Address of the humidity control register.
        /// </summary>
        private const byte CTRL_HUMIDITY_REGISTER = 0xf2;

        /// <summary>
        /// Address of the status register.
        /// </summary>
        private const byte STATUS_REGISTER = 0xf3;

        /// <summary>
        /// Address of the temperature and pressure measurement register.
        /// </summary>
        private const byte CTRL_MEASUSREMENT_REGISTER = 0xf4;

        /// <summary>
        /// Address of the configuration register.
        /// </summary>
        private const byte CONFIG_REGISTER = 0xf5;

        /// <summary>
        /// Address of the reset register.
        /// </summary>
        private const byte RESET_REGISTER = 0xe0;

        #endregion Constants

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
        /// Compensation data from the sensor.
        /// </summary>
        CompensationData _compensationData = new CompensationData();

        #endregion Member Variables

        #region Properties

        /// <summary>
        /// Gets or sets the I2C Address.
        /// </summary>
        /// <value>I2C address for the sensor.  This should be one of 0x76 or 0x77 (default).</value>
        private byte _address;
        public byte Address
        {
            get { return _address; }
            set
            {
                if ((value != 0x76) && (value != 0x77))
                {
                    throw new ArgumentOutOfRangeException("Address", "Address should be 0x76 or 0x77");
                }
                _address = value;
            }
        }

        /// <summary>
        /// Speed of the I2C bus in KHz.
        /// </summary>
        /// <value>Speed of the I2C bus in KHz.</value>
        private ushort _speed;
        public ushort Speed
        {
            get { return _speed; }
            set
            {
                if ((value < 10) || (value > 3400))
                {
                    throw new ArgumentOutOfRangeException("Speed", "Speed should be 10 KHz to 3,400 KHz.");
                }
                _speed = value;
            }
        }

        /// <summary>
        /// Instance of the I2CDevice used to communicate with the BME280 sensor.
        /// </summary>
        /// <value>The device.</value>
        private I2CDevice Device { get; set; }

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
        public byte TemperatureOverSampling { get; set; }

        /// <summary>
        /// Pressure over sampling configuration.
        /// </summary>
        public byte PressureOversampling { get; set; }

        /// <summary>
        /// Humidity over sampling configuration.
        /// </summary>
        public byte HumidityOverSampling { get; set; }

        /// <summary>
        /// Set the operating mode for the sensor.
        /// </summary>
        public byte Mode { get; set; }

        /// <summary>
        /// </summary>
        public byte Standby { get; set; }

        /// <summary>
        /// </summary>
        public byte Filter { get; set; }

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
            Address = address;
            Speed = speed;
            Device = new I2CDevice(new I2CDevice.Configuration(address, speed));
            ReadCompensationData();
            //
            //  Update the configuration information and start sampling.
            //
            TemperatureOverSampling = 1;
            PressureOversampling = 1;
            HumidityOverSampling = 1;
            Mode = 3;
            Filter = 0;
            Standby = 0;
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
            WriteRegister(CTRL_MEASUSREMENT_REGISTER, 0x00);
    
            byte data = (byte) (((Standby << 5) & 0xe0) | ((Filter << 2) & 0x1c));
            WriteRegister(CONFIG_REGISTER, data);
            data = (byte) (HumidityOverSampling & 0x07);
            WriteRegister(CTRL_HUMIDITY_REGISTER, data);
            data = (byte) (((TemperatureOverSampling << 5) & 0xe0) | ((PressureOversampling << 2) & 0x1c) |
                    (Mode & 0x03));
            WriteRegister(CTRL_MEASUSREMENT_REGISTER, data);
        }

        /// <summary>
        /// Reset the sensor.
        /// </summary>
        /// <remarks>
        /// Perform a full power-on-reset of the sensor and reset the configuration of the sensor.
        /// </remarks>
        public void Reset()
        {
            WriteRegister(RESET_REGISTER, 0xb6);
            UpdateConfiguration();
        }

        /// <summary>
        /// Read a block of data from the sensor.
        /// </summary>
        /// <param name="address">Address of the first register to read.</param>
        /// <param name="length">Number of bytes to read.</param>
        private byte[] ReadRegisters(byte address, int length)
        {
            byte[] registers = new byte[length];
            byte[] registerAddress = { address };
            I2CDevice.I2CTransaction[] readCompensationData =
            {
                I2CDevice.CreateWriteTransaction(registerAddress),       
                I2CDevice.CreateReadTransaction(registers)
            };
            int bytesTransferred = 0;
            int retryCount = 0;
            while (bytesTransferred != (length + 1))
            {
                if (retryCount > 3)
                {
                    throw new Exception("ReadRegisters: Retry count exceeded.");
                }
                retryCount++;
                bytesTransferred = Device.Execute(readCompensationData, 100);
            }
            return registers;
        }

        /// <summary>
        /// Write the specified value to the registers in the sensor.
        /// </summary>
        /// <param name="address">Register address.</param>
        /// <param name="value">Value to write into the register.</param>
        private void WriteRegister(byte address, byte value)
        {
            byte[] data = new byte[2];
            data[0] = address;
            data[1] = value;
            I2CDevice.I2CTransaction[] writeRegisters =
            {
                I2CDevice.CreateWriteTransaction(data)
            };
            int retryCount = 0;
            while (Device.Execute(writeRegisters, 100) != 2)
            {
                if (retryCount > 3)
                {
                    throw new Exception("WriteRegister: Retry count exceeded.");
                }
                retryCount++;
            }
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
            byte[] temperatureAndPressureData = ReadRegisters(0x88, 24);
            byte[] humidityData1 = ReadRegisters(0xa1, 1);
            byte[] humidityData2To6 = ReadRegisters(0xe1, 7);

            _compensationData.T1 = (ushort) (temperatureAndPressureData[0] + (temperatureAndPressureData[1] << 8));
            _compensationData.T2 = (short) (temperatureAndPressureData[2] + (temperatureAndPressureData[3] << 8));
            _compensationData.T3 = (short) (temperatureAndPressureData[4] + (temperatureAndPressureData[5] << 8));
            //
            _compensationData.P1 = (ushort) (temperatureAndPressureData[6] + (temperatureAndPressureData[7] << 8));
            _compensationData.P2 = (short) (temperatureAndPressureData[8] + (temperatureAndPressureData[9] << 8));
            _compensationData.P3 = (short) (temperatureAndPressureData[10] + (temperatureAndPressureData[11] << 8));
            _compensationData.P4 = (short) (temperatureAndPressureData[12] + (temperatureAndPressureData[13] << 8));
            _compensationData.P5 = (short) (temperatureAndPressureData[14] + (temperatureAndPressureData[15] << 8));
            _compensationData.P6 = (short) (temperatureAndPressureData[16] + (temperatureAndPressureData[17] << 8));
            _compensationData.P7 = (short) (temperatureAndPressureData[18] + (temperatureAndPressureData[19] << 8));
            _compensationData.P8 = (short) (temperatureAndPressureData[20] + (temperatureAndPressureData[21] << 8));
            _compensationData.P9 = (short) (temperatureAndPressureData[22] + (temperatureAndPressureData[23] << 8));
            //
            _compensationData.H1 = humidityData1[0];
            _compensationData.H2 = (short) (humidityData2To6[0] + (humidityData2To6[1] << 8));
            _compensationData.H3 = humidityData2To6[2];
            _compensationData.H4 = (short) ((humidityData2To6[3] << 4) + (humidityData2To6[4] & 0xf));
            _compensationData.H5 = (short) (((humidityData2To6[4] & 0xf) >> 4) + (humidityData2To6[5] << 4));
            _compensationData.H6 = (sbyte) humidityData2To6[6];
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
            byte[] readings = ReadRegisters(0xf7, 8);
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
            int tvar1 = (((adcTemperature >> 3) - (((int) _compensationData.T1) << 1)) * ((int) _compensationData.T2)) >> 11;
            int tvar2 = (((((adcTemperature >> 4) - ((int) _compensationData.T1)) * 
                        ((adcTemperature >> 4) - _compensationData.T1)) >> 12) * ((int) _compensationData.T3)) >> 14;
            int tfine = tvar1 + tvar2;
            //
            Temperature = ((tfine * 5 + 128) >> 8) / 100;
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
            pvar2 += (pvar1 * (long) _compensationData.P5) << 17;
            pvar2 += ((long) _compensationData.P4) << 35;
            pvar1 = ((pvar1 * pvar1 * (long) _compensationData.P8) >> 8) + ((pvar1 * (long) _compensationData.P2) << 12);
            pvar1 = (((((long) 1) << 47) + pvar1)) * ((long) _compensationData.P1) >> 33;
            if (pvar1 == 0)
            {
                Pressure = 0;
            }
            else
            {
                int adcPressure = (readings[0] << 12) | (readings[1] << 4) | ((readings[2] >> 4) & 0x0f);
                long pressure = 1048576 - adcPressure;
                pressure = (((pressure << 31) - pvar2) * 3125) / pvar1;
                pvar1 = (((long) _compensationData.P9) * (pressure >> 13) * (pressure >> 13)) >> 25;
                pvar2 = (((long) _compensationData.P8) * pressure) >> 19;
                pressure = ((pressure + pvar1 + pvar2) >> 8) + (((long) _compensationData.P7) << 4);
                //
                Pressure = (pressure / 256);
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
            int v_x1_u32r = (tfine - ((int) 76800));

            v_x1_u32r = (((((adcHumidity << 14) - (((int) _compensationData.H4) << 20) - (((int) _compensationData.H5) * v_x1_u32r)) +
                        ((int) 16384)) >> 15) * (((((((v_x1_u32r * ((int) _compensationData.H6)) >> 10) * (((v_x1_u32r * ((int) _compensationData.H3)) >> 11) + ((int) 32768))) >> 10) + ((int) 2097152)) *
                        ((int) _compensationData.H2) + 8192) >> 14));
            v_x1_u32r = (v_x1_u32r - (((((v_x1_u32r >> 15) * (v_x1_u32r >> 15)) >> 7) * ((int) _compensationData.H1)) >> 4));

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
            Humidity = ((float) (v_x1_u32r >> 12)) / 1024;
        }

        #endregion Methods
    }
}
