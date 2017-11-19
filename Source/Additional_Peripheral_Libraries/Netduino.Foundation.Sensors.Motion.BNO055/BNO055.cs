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
            public static readonly byte AcceleromterID = 0x01;
            public static readonly byte MagnetometerID = 0x02;
            public static readonly byte GyroscopeID = 0x03;
            public static readonly byte SoftwareRevisionIDLSB = 0x04;
            public static readonly byte SoftwareRevisionIDMSB = 0x05;
            public static readonly byte BootloaderRevisionID = 0x06;
            public static readonly byte PageID = 0x07;
            public static readonly byte AccelerometerXLSB = 0x08;
            public static readonly byte AccelerometerXMSB = 0x09;
            public static readonly byte AccelerometerYLSB = 0x0a;
            public static readonly byte AccelerometerYMSB = 0x0b;
            public static readonly byte AccelerometerZLSB = 0x0c;
            public static readonly byte AccelerometerZMSB = 0x0d;
            public static readonly byte MagnetometerXLSB = 0x0e;
            public static readonly byte MagnetometerXMSB = 0x0f;
            public static readonly byte MagnetometerYLSB = 0x10;
            public static readonly byte MagnetometerYMSB = 0x11;
            public static readonly byte MagnetometerZLSB = 0x12;
            public static readonly byte MagnetometerZMSB = 0x13;
            public static readonly byte GyroscopeXLSB = 0x14;
            public static readonly byte GyroscopeXMSB = 0x15;
            public static readonly byte GyroscopeYLSB = 0x16;
            public static readonly byte GyroscopeYMSB = 0x17;
            public static readonly byte GyroscopeZLSB = 0x18;
            public static readonly byte GyroscopeZMSB = 0x19;
            public static readonly byte EulerAngleXLSB = 0x1a;
            public static readonly byte EulerAngleXMSB = 0x1b;
            public static readonly byte EulerAngleYLSB = 0x1c;
            public static readonly byte EulerAngleYMSB = 0x1d;
            public static readonly byte EulerAngleZLSB = 0x1e;
            public static readonly byte EulerAngleZMSB = 0x1f;
            public static readonly byte QuaternionDataWLSB = 0x20;
            public static readonly byte QuaternionDataWMSB = 0x21;
            public static readonly byte QuaternionDataXLSB = 0x22;
            public static readonly byte QuaternionDataXMSB = 0x23;
            public static readonly byte QuaternionDataYLSB = 0x24;
            public static readonly byte QuaternionDataYMSB = 0x25;
            public static readonly byte QuaternionDataZLSB = 0x26;
            public static readonly byte QuaternionDataZMSB = 0x27;
            public static readonly byte LinearAccelerationXLSB = 0x28;
            public static readonly byte LinearAccelerationXMSB = 0x29;
            public static readonly byte LinearAccelerationYLSB = 0x2a;
            public static readonly byte LinearAccelerationYMSB = 0x2b;
            public static readonly byte LinearAccelerationZLSB = 0x2c;
            public static readonly byte LinearAccelerationZMSB = 0x2d;
            public static readonly byte GravityVectorXLSB = 0x2e;
            public static readonly byte GravityVectorXMSB = 0x2f;
            public static readonly byte GravityVectorYLSB = 0x30;
            public static readonly byte GravityVectorYMSB = 0x31;
            public static readonly byte GravityVectorZLSB = 0x32;
            public static readonly byte GravityVectorZMSB = 0x33;
            public static readonly byte Temperature = 0x34;
            public static readonly byte CalibrationStatus = 0x35;
            public static readonly byte SelfTestResult = 0x36;
            public static readonly byte InterruptStatus = 0x37;
            public static readonly byte SystemClockStatus = 0x38;
            public static readonly byte SystemStatus = 0x39;
            public static readonly byte ErrorCode = 0x3a;
            public static readonly byte Units = 0x3b;
            public static readonly byte OperatingMode = 0x3d;
            public static readonly byte PowerMode = 0x3e;
            public static readonly byte SystemTrigger = 0x3f;
            public static readonly byte TemperatureSource = 0x40;
            public static readonly byte AxisMapConfiguration = 0x41;
            public static readonly byte AxisMapSign = 0x42;
            public static readonly byte AccelerometerOffsetXLSB = 0x55;
            public static readonly byte AccelerometerOffsetXMSB = 0x56;
            public static readonly byte AccelerometerOffsetYLSB = 0x57;
            public static readonly byte AccelerometerOffsetYMSB = 0x58;
            public static readonly byte AccelerometerOffsetZLSB = 0x59;
            public static readonly byte AccelerometerOffsetZMSB = 0x5a;
            public static readonly byte MagnetometerOffsetXLSB = 0x5b;
            public static readonly byte MagnetometerOffsetXMSB = 0x5c;
            public static readonly byte MagnetometerOffsetYLSB = 0x5d;
            public static readonly byte MagnetometerOffsetYMSB = 0x5e;
            public static readonly byte MagnetometerOffsetZLSB = 0x5f;
            public static readonly byte MagnetometerOffsetZMSB = 0x60;
            public static readonly byte GyroscopeOffsetXLSB = 0x61;
            public static readonly byte GyroscopeOffsetXMSB = 0x62;
            public static readonly byte GyroscopeOffsetYLSB = 0x63;
            public static readonly byte GyroscopeOffsetYMSB = 0x64;
            public static readonly byte GyroscopeOffsetZLSB = 0x65;
            public static readonly byte GyroscopeOffsetZMSB = 0x66;
            public static readonly byte AccelerometerRadiusLSB = 0x67;
            public static readonly byte AccelerometerRadiusMSB = 0x68;
            public static readonly byte MagnetometerRadiusLSB = 0x69;
            public static readonly byte MagnetometerRadiusMSB = 0x6a;
            public static readonly byte AccelerometerConfiguration = 0x08;
            public static readonly byte MagnetometerConfiguration = 0x09;
            public static readonly byte GyroscopeConfiguration0 = 0x0a;
            public static readonly byte GyroscopeConfiguration1 = 0x0b;
            public static readonly byte AccelerometerSleepConfiguration = 0x0c;
            public static readonly byte GyrosscopeSleepConfiguration = 0x0d;
            public static readonly byte InterruptMask = 0x0f;
            public static readonly byte InterruptEnable = 0x10;
            public static readonly byte AccelerometerMotionThreshold = 0x11;
            public static readonly byte AccelerometerInterruptSettings = 0x12;
            public static readonly byte AccelerometerHighGDuration = 0x013;
            public static readonly byte AccelerometerHighGThreshold = 0x14;
            public static readonly byte AccelerometerNoMotionThreshold = 0x15;
            public static readonly byte AccelerometerNoMotionSetting = 0x16;
            public static readonly byte GyroscopeInterruptSetting = 0x17;
            public static readonly byte GyroscopeHighRateX = 0x18;
            public static readonly byte GyroscopeDurationX = 0x19;
            public static readonly byte GyroscopeHighRateY = 0x1a;
            public static readonly byte GyroscopeDurationY = 0x1b;
            public static readonly byte GyroscopeHighRateZ = 0x1c;
            public static readonly byte GyroscopeDurationZ = 0x1d;
            public static readonly byte GuroscopeAnyMotionThreshold = 0x1e;
            public static readonly byte GyroscopeAnyMotionSetting = 0x1f;
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
                throw new ArgumentOutOfRangeException("address", "Address should be 0x28 or 0x29.");
            }
		    if (speed > 400)
		    {
		        throw new ArgumentOutOfRangeException("speed", "Maximum speed is 400kHz.");
		    }
            I2CBus device = new I2CBus(address, speed);
            _bno055 = (ICommunicationBus) device;
		    _bno055.WriteRegister(Registers.PageID, 0x00);
            if (_bno055.ReadRegister(Registers.ChipID) != 0xa0)
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
