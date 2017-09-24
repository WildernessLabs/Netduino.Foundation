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
        /// <summary>
        /// Control registers for the ADXL345 chip.
        /// </summary>
        /// <remarks>
        /// Taken from table 19 on page 23 of the data sheet.
        /// </remarks>
        enum Registers : byte { DeviceID = 0x00, TAPThreshold = 0x1d, OffsetX = 0x1e, OffsetY = 0x1f, OffsetZ = 0x20,
                                TAPDuration = 0x21, TAPLatency = 0x22, TAPWindow = 0x23, ActivityThreshold = 0x24, 
                                InactivityThreshold = 0x25, InactivityTime = 0x26, ActivityInactivityControl = 0x27,
                                FreeFallThreshold = 0x28, FreeFallTime = 0x29, TAPAxes = 0x2a, TAPActivityStatus = 0x2a,
                                DataRate = 0x2c, PowerControl = 0x2d, InterruptEnable = 0x2e, InterruptMap = 0x2f,
                                InterruptSource = 0x30, DataFormat = 0x31, X0 = 0x32, X1, 0x33, Y0 = 0x33, Y1 = 0x34,
                                Z0 = 0x36, Z1 = 0x37, FirstInForstOutControl = 0x38, FirstInFirstOut = 0x39};

		private ICommunicationBus _adxl345;

        /// <summary>
        /// Get the device ID, this should return 0xe5.
        /// </summary>
        public byte DeviceID
        {
            get
            {
                return (_adxl345.ReadRegister((byte) Registers.DeviceID));
            }
        }

        public int X { get; private set; };

        public int Y { get; private set; };

        public int Z { get; private set; };

        /// <summary>
        /// Threshold for the tap interrupts (62.5 mg/LSB).  A value of 0 may lead to undesirable
        /// results and so will be rejected.
        /// </summary>
        public byte Threshold { get; set; };

        /// <summary>
        /// Values stored in this register are automatically added to the X reading.
        /// </summary>
        public sbyte OffsetX { get; set; };

        /// <summary>
        /// Values stored in this register are automatically added to the Y reading.
        /// </summary>
        public sbyte OffsetY { get; set; };

        /// <summary>
        /// Values stored in this register are automatically added to the Z reading.
        /// </summary>
        public sbyte OffsetZ { get; set; };

        /// <summary>
        /// The maximum time that an event must be above the threshold in order to qualify
        /// as an event.  The scale factor is 625us per LSB.
        /// <remarks>
        /// A value of 0 disables the tap function.
        /// </remarks>
        /// </summary>
        public byte Duration { get; set; };

        /// <summary>
        /// Used in combination with the Window property to control the double tap detection.
        /// This value represents the period of time between the detection of the tap event
        /// until the start of the time window.
        ///
        /// The scale factor is 1.25ms per LSB.
        /// <remarks>
        /// A value of 0 disables the double tap function.
        /// </remarks>
        /// </summary>
        public byte DoubleTapLatency { get; set; };

        /// <summary>
        /// Defines the period in which a second tap event can occur after the expiration
        /// of the latency period.  The time period is measured in 1.25ms per LSB.
        /// </summary>
        public byte DoubleTapWindow { get; set; };

        /// <summary>
        /// Threshold for detecting activity as 62.5mg per LSB.
        /// </summary>
        /// <remarks>
        /// A value of zero is undesirable when interrupts are enabled.
        /// <remarks>
        public byte ActivityThreshold { get; set; };

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// A value of 0 is not recommended when the inactivity interrupt is enabled.
        /// </remarks>
        public byte InactivityThreshold { get; set; };

        /// <summary>
        /// The amount of time that the acceleration must be less than the threshold value
        /// in order for inactivity to be declared.
        /// </summary>
        /// <remarks>
        /// Scale factor is 1s per LSB.
        ///
        /// A value of 0 will allow an interrupt to be generated when the output data is
        /// less than the threshold.
        /// <remarks>
        public byte InactivityTime { get; set; };

        /// <summary>
        /// Free-fall detection threshold value.
        ///
        /// Scale factor is 62.5mg per LSB.
        /// </summary>
        /// <remarks>
        /// A value fo 0 may result in undesirable behavior if free-fall interrupts
        /// are enabled.
        /// <remarks>
        public byte FreeFallThreshold { get; set; };

        /// <summary>
        /// The amount of time that all three axis must 
        /// </summary>
        /// <remarks>
        /// Scale factor is 5ms per LSB.
        ///
        /// A value of 0 may result in undesirable behavior if the free-fall
        /// interrupt is enabled.
        /// <remarks>
        public byte FreeFallTime { get; set; };

        /// <summary>
        /// Determine which interrupts are enabled / disabled.
        /// </summary>
        /// <remarks>
        /// </remarks>
        public byte InterruptEnable { get; set; };

        /// <summary>
        ///
        /// </summary>
        public byte InterruptMap { get; set; };

        /// <summary>
        /// Indicate which interrupts have generated the interrupt.
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        public byte InterruptSource { get; private set; };

        /// <summary>
        /// Determine the format of the data in the X0, X1, Y0, Y1, Z0 and Z1 registers.
        /// </summary>
        public byte DataFormat { get; set; };

        /// <summary>
        ///
        /// </summary>
        public byte FirstInFirstOutControl { get; set; };

        /// <summary>
        /// Register indicating if a First in First our event has occurred.
        /// </summary>
        public byte FirstInFirstOurStatus { get; private set; };

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        /// </remarks>
        public byte BandwidthRate { get; set; };

        /// <summary>
        /// </summary>
        public byte PowerControl { get; set; };

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

        /// <summary>
        /// Read the six sensor bytes and set the values for the X, y and Z acceleration.
        /// </summary>
        /// <remarks>
        /// All six acceleration registers should be read at the same time to ensure consistency
        /// of the measurements.
        /// </remarks>
        public void Read()
        {

        }
    }
}
