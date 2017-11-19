using System;
using Microsoft.SPOT;
using Netduino.Foundation.Devices;

namespace Netduino.Foundation.Sensors.Motion
{
	public class BNO055
	{
		#region Classes / structures

		/// <summary>
		///     Register addresses in the sensor.
		/// </summary>
        private static class Registers 
        {
            public static readonly byte ChipID = 0x00;
            public static readonly byte AccelleromterID = 0x01;
        }

		#endregion Classes  / structures

		#region Member variables / fields

		/// <summary>
		///     BNO055 object.
		/// </summary>
		private ICommunicationBus _bno055 = null;

		#endregion Member variables / fields

		#region Properties


		#endregion

		#region Constructors

		/// <summary>
		///     Default constructor is private to prevent the developer from calling it.
		/// </summary>
		private BNO055()
		{
		}

		/// <summary>
		///     Create a new BNO055 object using the default parameters for the component.
		/// </summary>
		/// <param name="address">Address of the BNO055 (default = 0x28).</param>
		/// <param name="speed">Speed of the I2C bus (default = 400 KHz).</param>
		public BNO055(byte address = 0x28, ushort speed = 400)
		{
            if ((address != 0x28) && (address != 0x29))
            {
                throw new Exception("Address should be 0x28 or 0x29.");
            }
            I2CBus device = new I2CBus(address, speed);
            _bno055 = (ICommunicationBus) device;
            if (_bno055.ReadRegister(Registers.ChipID) != 0x40)
            {
                throw new Exception("Sensor ID should be 0x40.");
            }
		}

		#endregion Constructors

		#region Methods

		/// <summary>
		///     Force the sensor to make a reading and update the relevant properties.
		/// </summary>
		public void Read()
		{

		}

		#endregion
	}
}
