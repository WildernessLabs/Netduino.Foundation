using Netduino.Foundation.Devices;

namespace Netduino.Foundation.Sensors.Temperature
{
    /// <summary>
    ///     TMP102 Temperature sensor object.
    /// </summary>    
    public class TMP102
    {
        #region Enums

        /// <summary>
        ///     Indicate the resolution of the sensor.
        /// </summary>
        public enum Resolution : byte
        {
            /// <summary>
            ///     Operate in 12-bit mode.
            /// </summary>
            Resolution12Bits,

            /// <summary>
            ///     Operate in 13-bit mode.
            /// </summary>
            Resolution13Bits
        }

        #endregion Enums

        #region Member variables / fields

        /// <summary>
        ///     TMP102 sensor.
        /// </summary>
        private readonly ICommunicationBus _tmp102;

        #endregion Member variables / fields

        #region Properties

        /// <summary>
        ///     Backing variable for the SensorResolution property.
        /// </summary>
        private Resolution _sensorResolution;

        /// <summary>
        ///     Get / set the resolution of the sensor.
        /// </summary>
        public Resolution SensorResolution
        {
            get { return _sensorResolution; }
            set
            {
                var configuration = _tmp102.ReadRegisters(0x01, 2);
                if (value == Resolution.Resolution12Bits)
                {
                    configuration[1] &= 0xef;
                }
                else
                {
                    configuration[1] |= 0x10;
                }
                _tmp102.WriteRegisters(0x01, configuration);
                _sensorResolution = value;
            }
        }

        /// <summary>
        ///     Temperature (in degrees centigrade).
        /// </summary>
        public double Temperature
        {
            get
            {
                var temperatureData = _tmp102.ReadRegisters(0x00, 2);
                var sensorReading = 0;
                if (SensorResolution == Resolution.Resolution12Bits)
                {
                    sensorReading = (temperatureData[0] << 4) | (temperatureData[1] >> 4);
                }
                else
                {
                    sensorReading = (temperatureData[0] << 5) | (temperatureData[1] >> 3);
                }
                return sensorReading * 0.0625;
            }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        ///     Default constructor (private to prevent it being called).
        /// </summary>
        private TMP102()
        {
        }

        /// <summary>
        ///     Create a new TMP102 object using the default configuration for the sensor.
        /// </summary>
        /// <param name="address">I2C address of the sensor.</param>
        /// <param name="speed">Speed of the communication with the sensor.</param>
        public TMP102(byte address = 0x48, ushort speed = 100)
        {
            _tmp102 = new I2CBus(address, speed);
            var configuration = _tmp102.ReadRegisters(0x01, 2);
            _sensorResolution = (configuration[1] & 0x10) > 0
                ? Resolution.Resolution13Bits
                : Resolution.Resolution12Bits;
        }

        #endregion Constructors
    }
}