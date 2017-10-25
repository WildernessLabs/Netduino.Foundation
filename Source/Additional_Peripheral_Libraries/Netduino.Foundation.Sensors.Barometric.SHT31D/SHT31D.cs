using Netduino.Foundation.Devices;
using System;

namespace Netduino.Foundation.Sensors.Barometric
{
    /// <summary>
    /// Provide a mechanism for reading the temperature and humidity from
    /// a SHT31D temperature / humidity sensor.
    /// </summary>
    /// <remarks>
    /// Readings from the sensor are made in Single-shot mode.
    /// </remarks>
    public class SHT31D
    {
        #region Member variables / fields

        /// <summary>
        /// SH31D sensor communicates using I2C.
        /// </summary>
        private I2CBus _sht31d = null;

        #endregion Member variables / fields

        #region Properties

        /// <summary>
        /// Get the last humidity reading from the sensor.
        /// </summary>
        /// <remarks>
        /// The Read method should be called before the data in this property
        /// contains valid data.
        /// </remarks>
        public float Humidity { get; private set; }

        /// <summary>
        /// Get the last temperature reading.
        /// </summary>
        /// <remarks>
        /// The Read method should be called before the data in this property
        /// contains valid data.
        /// </remarks>
        public float Temperature { get; private set; }

        #endregion Properties

        #region Constructors

        /// <summary>
        /// Default constructor (made private to prevent it being called).
        /// </summary>
        private SHT31D()
        {
        }

        /// <summary>
        /// Create a new SHT31D object.
        /// </summary>
        /// <param name="address">Sensor address (should be 0x44 or 0x45).</param>
        /// <param name="speed">Bus speed (0-1000 KHz).</param>
        public SHT31D(byte address = 0x44, ushort speed = 100)
        {
            if ((address != 0x44) && (address != 0x45))
            {
                throw new ArgumentOutOfRangeException("address", "Address should be 0x44 or 0x45");
            }
            if (speed > 1000)
            {
                throw new ArgumentOutOfRangeException("speed", "Speed should be between 0 and 1000 KHz");
            }
            _sht31d = new I2CBus(address, speed);
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Get a reading from the sensor and set the Temperature and Humidity properties.
        /// </summary>
        public void Read()
        {
            byte[] data = _sht31d.WriteRead(new byte[] { 0x2c, 0x06 }, 6);
            Humidity = 100 * ((float) ((data[3] << 8) + data[4])) / 65535;
            Temperature = (175 * ((float) ((data[0] << 8) + data[1])) / 65535) - 45;
        }

        #endregion
    }
}