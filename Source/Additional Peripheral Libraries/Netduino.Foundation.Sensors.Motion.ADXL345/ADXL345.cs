using System;
using Microsoft.SPOT;
using Netduino.Foundation.Core;

namespace Netduino.Foundation.Sensors.Motion
{
    /// <summary>
    /// Driver for the ADXL345 3-axis digital accelerometer capable of measuring
    //  up to +/-16g.
    /// </summary>
    public class ADXL345
    {
		private ICommunictionBus _adxl345;

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
        public ADXL345(byte address = 0x1d, ushort speed = 100)
        {
			if ((address != 0x1d) && (address != 0x53))
			{
			}
			if ((speed != 100) && (speed != 400))
			{
			}
			I2CBus device = new I2CBus(address, speed);
			_adxl345 = (ICommunicationBus) device;
        }
    }
}
