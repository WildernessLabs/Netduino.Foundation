using System;
using Netduino.Foundation.Devices;

namespace Netduino.Foundation.Sensors.Motion
{
    /// <summary>
    /// Driver for the ADXL345 3-axis digital accelerometer capable of measuring
    //  up to +/-16g.
    /// </summary>
    public class ADXL345
    {
        #region Enums

        /// <summary>
        /// Control registers for the ADXL345 chip.
        /// </summary>
        /// <remarks>
        /// Taken from table 19 on page 23 of the data sheet.
        /// </remarks>
        private enum Registers : byte { DeviceID = 0x00, TAPThreshold = 0x1d, OffsetX = 0x1e, OffsetY = 0x1f, OffsetZ = 0x20,
                                        TAPDuration = 0x21, TAPLatency = 0x22, TAPWindow = 0x23, ActivityThreshold = 0x24, 
                                        InactivityThreshold = 0x25, InactivityTime = 0x26, ActivityInactivityControl = 0x27,
                                        FreeFallThreshold = 0x28, FreeFallTime = 0x29, TAPAxes = 0x2a, TAPActivityStatus = 0x2a,
                                        DataRate = 0x2c, PowerControl = 0x2d, InterruptEnable = 0x2e, InterruptMap = 0x2f,
                                        InterruptSource = 0x30, DataFormat = 0x31, X0 = 0x32, X1 = 0x33, Y0 = 0x33, Y1 = 0x34,
                                        Z0 = 0x36, Z1 = 0x37, FirstInFirstOutControl = 0x38, FirstInFirstOutStatus = 0x39};

        /// <summary>
        /// Possible values for the range (see DataFormat register).
        /// </summary>
        /// <remarks>
        /// See page 27 of the data sheet.
        /// </remarks>
        public enum Range { TwoG = 0x00, FourG = 0x01, EightG = 0x02, SixteenG = 0x03 };

        /// <summary>
        /// Frequency of the sensor readings when the device is in sleep mode.
        /// </summary>
        /// <remarks>
        /// See page 26 of the data sheet.
        /// </remarks>
        public enum Frequency { EightHz = 0x00, FourHz = 0x01, TwoHz = 0x02, OneHz = 0x03 };

        /// <summary>
        /// Valid FIFO modes (see page 27 of the datasheet).
        /// </summary>
        public enum FIFOMode { Bypass = 0x00, FIFO = 0x40, Stream = 0x80, Trigger = 0xc0 };

        #endregion Enums

        #region Member variables / fields

        /// <summary>
        /// Communication bus used to communicate with the sensor.
        /// </summary>
        private ICommunicationBus _adxl345;

        #endregion Member variables / fields

        #region Properties

        /// <summary>
        /// Get the device ID, this should return 0xe5.
        /// </summary>
        public byte DeviceID
        {
            get
            {
                byte deviceID = _adxl345.ReadRegister((byte) Registers.DeviceID);
                if (deviceID != 0xe5)
                {
                    throw new Exception("Invalid device ID: " + deviceID.ToString() + ", expected 229 (0xe5)");
                }
                return (deviceID);
            }
        }

        /// <summary>
        /// Acceleration along the X-axis.
        /// </summary>
        /// <remarks>
        /// This property will only contain valid data after a call to Read or after
        /// an interrupt has been generated.
        /// </remarks>
        public short X { get; private set; }

        /// <summary>
        /// Acceleration along the Y-axis.
        /// </summary>
        /// <remarks>
        /// This property will only contain valid data after a call to Read or after
        /// an interrupt has been generated.
        /// </remarks>
        public short Y { get; private set; }

        /// <summary>
        /// Acceleration along the Z-axis.
        /// </summary>
        /// <remarks>
        /// This property will only contain valid data after a call to Read or after
        /// an interrupt has been generated.
        /// </remarks>
        public short Z { get; private set; }

        /// <summary>
        /// Values stored in this register are automatically added to the X reading.
        /// </summary>
        /// <remarks>
        /// Scale factor is 15.6 mg/LSB so 0x7f represents an offset of 2g.
        /// </remarks>
        public sbyte OffsetX
        {
            get
            {
                return ((sbyte) _adxl345.ReadRegister((byte) Registers.OffsetX));
            }
            set
            {
                _adxl345.WriteRegister((byte) Registers.OffsetX, (byte) value);
            }
        }

        /// <summary>
        /// Values stored in this register are automatically added to the Y reading.
        /// </summary>
        /// <remarks>
        /// Scale factor is 15.6 mg/LSB so 0x7f represents an offset of 2g.
        /// </remarks>
        public sbyte OffsetY
        {
            get
            {
                return ((sbyte) _adxl345.ReadRegister((byte) Registers.OffsetY));
            }
            set
            {
                _adxl345.WriteRegister((byte) Registers.OffsetY, (byte) value);
            }
        }

        /// <summary>
        /// Values stored in this register are automatically added to the Z reading.
        /// </summary>
        /// <remarks>
        /// Scale factor is 15.6 mg/LSB so 0x7f represents an offset of 2g.
        /// </remarks>
        public sbyte OffsetZ
        {
            get
            {
                return ((sbyte) _adxl345.ReadRegister((byte) Registers.OffsetZ));
            }
            set
            {
                _adxl345.WriteRegister((byte) Registers.OffsetZ, (byte) value);
            }
        }

        /// <summary>
        /// Number of samples in the FIFO buffer.
        /// </summary>
        public byte FirstInFirstOutSampleCount
        {
            get
            {
                return ((byte) (_adxl345.ReadRegister((byte) Registers.FirstInFirstOutStatus) & 0x2f));
            }
        }

        /// <summary>
        /// Indicate if a Trigger event has occurred.
        /// </summary>
        public bool FIFOTriggerEventOccurred
        {
            get
            {
                return((_adxl345.ReadRegister((byte) Registers.FirstInFirstOutStatus) & 0x40) == 0);
            }
        }

        #endregion Properties

        #region Constructors

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
        public ADXL345(byte address = 0x53, ushort speed = 100)
        {
			if ((address != 0x1d) && (address != 0x53))
			{
                throw new ArgumentOutOfRangeException("address", "ADXL345 address can only be 0x1d or 0x53.");
			}
			if ((speed < 10) || (speed > 400))
			{
                throw new ArgumentOutOfRangeException("speed", "ADXL345 speed should be between 10 kHz and 400 kHz inclusive.");
			}
			I2CBus device = new I2CBus(address, speed);
			_adxl345 = (ICommunicationBus) device;
            Reset();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Set the PowerControl register (see pages 25 and 26 of the data sheet)
        /// </summary>
        /// <param name="linkActivityAndInactivity">Link the activity and inactivity events.</param>
        /// <param name="autoASleep">Enable / disable auto sleep when the activity and inactivity are linked.</param>
        /// <param name="measuring">Enable or disable measurements (turn on or off).</param>
        /// <param name="sleep">Put the part to sleep (true) or run in normal more (false).</param>
        /// <param name="frequency">Frequency of measurements when the part is in sleep mode.</param>
        public void SetPowerState(bool linkActivityAndInactivity, bool autoASleep, bool measuring, bool sleep, Frequency frequency)
        {
            byte data = 0;
            if (linkActivityAndInactivity)
            {
                data |= 0x20;
            }
            if (autoASleep)
            {
                data |= 0x10;
            }
            if (measuring)
            {
                data |= 0x08;
            }
            if (sleep)
            {
                data |= 0x40;
            }
            data |= (byte) frequency;
            _adxl345.WriteRegister((byte) Registers.PowerControl, data);
        }

        /// <summary>
        /// Configure the data format (see pages 26 and 27 of the data sheet).
        /// </summary>
        /// <param name="selfTest">Put the device into self test mode when true.</param>
        /// <param name="spiMode">Use 3-wire SPI (true) or 4-wire SPI (false).</param>
        /// <param name="invertInterrupts">Interrupts are active high (false) or active low (true).</param>
        /// <param name="fullResolution">Set to full resolution (true) or 10-bit mode using the range determined by the range parameter (false).</param>
        /// <param name="justification">Left-justified when true, right justified with sign extension when false.</param>
        /// <param name="range">Set the range of the sensor to 2g, 4g, 8g or 16g</param>
        /// <remarks>
        /// The range of the sensor is determined by the following table:
        /// 
        /// 0:  +/- 2g
        /// 1:  +/- 4g
        /// 2:  +/- 8g
        /// 3:  +/ 16g
        /// </remarks>
        public void SetDataFormat(bool selfTest, bool spiMode, bool fullResolution, bool justification, ADXL345.Range range)
        {
            byte data = 0;
            if (selfTest)
            {
                data |= 0x80;
            }
            if (spiMode)
            {
                data |= 0x40;
            }
            if (fullResolution)
            {
                data |= 0x04;
            }
            if ( justification)
            {
                data |= 0x02;
            }
            data |= (byte) range;
            _adxl345.WriteRegister((byte) Registers.DataFormat, data);
        }

        /// <summary>
        /// Set the data rate and low power mode for the sensor.
        /// </summary>
        /// <param name="dataRate">Data rate for the sensor.</param>
        /// <param name="lowPower">Setting this to true will enter low power mode (note measurement will encounter more noise in this mode).</param>
        public void SetDataRate(byte dataRate, bool lowPower)
        {
            if (dataRate > 0xff)
            {
                throw new ArgumentOutOfRangeException("dataRate", "Data rate should be in the range 0-15 inclusive");
            }
            byte data = dataRate;
            if (lowPower)
            {
                data |= 0x10;
            }
            _adxl345.WriteRegister((byte) Registers.DataRate, data);
        }

        /// <summary>
        /// Setup the FIFO mode.
        /// </summary>
        /// <param name="mode">FIFO mode (one of Bypass, FIFO, Stream or Trigger).</param>
        /// <param name="trigger">Link the trigger mode to Interrupt1 (false) or Interrupt2 (true).</param>
        /// <param name="samples">Number of FIFO samples (0-32).</param>
        /// <remarks>
        /// The function of these bits depends on the FIFO mode selected (see table below). Entering 
        /// a value of 0 in the samples bits immediately sets the watermark status bit in the InterruptSource
        /// register, regardless of which FIFO mode is selected. Undesirable operation may occur if a 
        /// value of 0 is used for the samples bits when trigger mode is used.
        /// 
        /// FIFO Mode   Samples Bits Function
        /// Bypass      None.
        /// FIFO        Specifies how many FIFO entries are needed to trigger a watermark interrupt. 
        /// Stream      Specifies how many FIFO entries are needed to trigger a watermark interrupt. 
        /// Trigger     Specifies how many FIFO samples are retained in the FIFO buffer before a trigger event. 
        /// </remarks>
        public void SetupFIFO(FIFOMode mode, bool trigger, byte samples)
        {
            byte data = (byte) mode;
            if (trigger)
            {
                data |= 0x20;
            }
            if (samples > 32)
            {
                throw new ArgumentOutOfRangeException("samples", "Number of samples should be between 0 and 32 inclusive.");
            }
            if ((mode == FIFOMode.Trigger) && (samples == 0))
            {
                throw new ArgumentException("Setting number of samples to 0 in Trigger mode can result in unpredicatble behavior.");
            }
            data += samples;
            _adxl345.WriteRegister((byte) Registers.FirstInFirstOutControl, data);
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
            byte[] data = _adxl345.ReadRegisters((byte) Registers.X0, 6);
            X = (short) (data[0] + (data[1] << 8));
            Y = (short) (data[2] + (data[3] << 8));
            Z = (short) (data[4] + (data[5] << 8));
        }

        /// <summary>
        /// There is no reset function on the ADXL345 so this method resets the registers to the
        /// power on values.
        /// </summary>
        public void Reset()
        {
            _adxl345.WriteRegisters((byte) Registers.TAPThreshold, new byte[] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 });
            _adxl345.WriteRegisters((byte) Registers.DataRate, new byte[] { 0x0a, 0, 0, 0 });
            _adxl345.WriteRegister((byte) Registers.DataFormat, 0);
            _adxl345.WriteRegister((byte) Registers.FirstInFirstOutControl, 0);
        }

        /// <summary>
        /// Dump the registers to the debug output stream.
        /// </summary>
        public void DisplayRegisters()
        {
            byte[] registers = _adxl345.ReadRegisters((byte) Registers.TAPThreshold, 29);
            Helpers.DebugInformation.DisplayRegisters((byte) Registers.TAPThreshold, registers);
        }

        #endregion Methods
    }
}