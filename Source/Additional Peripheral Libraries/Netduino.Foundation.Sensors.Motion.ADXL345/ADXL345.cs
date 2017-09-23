using System;
using Microsoft.SPOT;

namespace Netduino.Foundation.Sensors.Motion
{
    /// <summary>
    /// Driver for the ADXL345 3-axis digital accelerometer capable of measuring
    //  up to +/-16g.
    /// </summary>
    public class ADXL345
    {
        /// <summary>
        /// Make the default constructor private so that it cannot be used.
        /// </summary>
        private ADXL345()
        {
        }

        /// <summary>
        /// Create a new instance of the ADXL345 communicating over the I2C interface.
        /// </summary>
        /// <param name="address">Address of the I2C sensor</param>
        /// <param name="speed">Speed of the I2C bus in KHz</param>
        public ADXL345(byte address, ushort speed)
        {
        }
    }
}
