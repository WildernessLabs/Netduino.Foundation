using System;
using System.Threading;
using Netduino.Foundation.Devices;

namespace Netduino.Foundation.Sensors.Barometric
{
    /// <summary>
    ///     Provide a mechanism for reading the Temperature and Humidity from
    ///     a HIH6130 temperature and Humidity sensor.
    /// </summary>
    public class HIH6130
    {
        #region Member variables / fields

        /// <summary>
        ///     MAG3110 object.
        /// </summary>
        private readonly ICommunicationBus _hih6130;

        #endregion Member variables / fields

        #region Properties

        /// <summary>
        ///     Humidity reading from the last call to Read.
        /// </summary>
        public float Humidity { get; private set; }

        /// <summary>
        ///     Temperature reading from last call to Read.
        /// </summary>
        public float Temperature { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        ///     Default constructor is private to prevent the developer from calling it.
        /// </summary>
        private HIH6130()
        {
        }

        /// <summary>
        ///     Create a new HIH6130 object using the default parameters for the component.
        /// </summary>
        /// <param name="address">Address of the HIH6130 (default = 0x27).</param>
        /// <param name="speed">Speed of the I2C bus (default = 100 KHz).</param>
        public HIH6130(byte address = 0x27, ushort speed = 100)
        {
            _hih6130 = new I2CBus(address, speed);
            Read();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        ///     Force the sensor to make a reading and update the relevant properties.
        /// </summary>
        public void Read()
        {
            _hih6130.WriteByte(0);
            //
            //  Sensor takes 35ms to make a valid reading.
            //
            Thread.Sleep(50);
            var data = _hih6130.ReadBytes(4);
            //
            //  Data format:
            //
            //  Byte 0: S1  S0  H13 H12 H11 H10 H9 H8
            //  Byte 1: H7  H6  H5  H4  H3  H2  H1 H0
            //  Byte 2: T13 T12 T11 T10 T9  T8  T7 T6
            //  Byte 4: T5  T4  T3  T2  T1  T0  XX XX
            //
            if ((data[0] & 0xc0) != 0)
            {
                throw new Exception("Status indicates readings are invalid.");
            }
            var reading = ((data[0] << 8) | data[1]) & 0x3fff;
            Humidity = ((float) reading / 16383) * 100;
            reading = ((data[2] << 8) | data[3]) >> 2;
            Temperature = (((float) reading / 16383) * 165) - 40;
        }

        #endregion Methods
    }
}