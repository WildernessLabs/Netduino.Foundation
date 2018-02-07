using Microsoft.SPOT.Hardware;
using Netduino.Foundation.Communications;

namespace Netduino.Foundation.Sensors.Motion
{
    public class ADXL362
    {
        #region Member variables / fields

        /// <summary>
        ///     ADXL362 sensor object.
        /// </summary>
        private readonly ICommunicationBus _adxl362;

        #endregion Member variables / fields

        #region Methods

        /// <summary>
        ///     Read the sensors and make the readings available through the
        ///     X, Y and Z properties.
        /// </summary>
        public void Read()
        {
            var sensorReading = _adxl362.WriteRead(new byte[] { 0x0b, Registers.XAxisLSB }, 8);
            X = (short) ((sensorReading[3] << 8) | sensorReading[2]);
            Y = (short) ((sensorReading[5] << 8) | sensorReading[4]);
            Z = (short) ((sensorReading[7] << 8) | sensorReading[6]);
        }

        #endregion Methods

        #region Classes / structures

        /// <summary>
        ///     Registers in the ADXL362 sensor.
        /// </summary>
        protected static class Registers
        {
            /// <summary>
            ///     Device ID (should be 0xad).
            /// </summary>
            public const byte DeviceID = 0x00;

            /// <summary>
            ///     Device IS MST (should be 0x1d).
            /// </summary>
            public const byte DeviceIDMST = 0x01;

            /// <summary>
            ///     Part ID (should be 0xf2).
            /// </summary>
            public const byte PartID = 0x03;

            /// <summary>
            ///     Revision ID (starts with 0x01 and increments for each change to the silicon).
            /// </summary>
            public const byte SiliconRevisionID = 0x03;

            /// <summary>
            ///     X-axis MSB (8-bits used when limited resolution is acceptable).
            /// </summary>
            public const byte XAxis8Bits = 0x08;

            /// <summary>
            ///     Y-axis MSB (8-bits used when limited resolution is acceptable).
            /// </summary>
            public const byte YAxis8Bits = 0x09;

            /// <summary>
            ///     Z-axis MSB (8-bits used when limited resolution is acceptable).
            /// </summary>
            public const byte ZAxis8Bits = 0x0a;

            /// <summary>
            ///     Status register
            /// </summary>
            public const byte Status = 0x0b;

            /// <summary>
            ///     FIFO entires (LSB)
            /// </summary>
            public const byte FIFORCEntriesLSB = 0x0c;

            /// <summary>
            ///     FIFO entries (MSB)
            /// </summary>
            public const byte FIFOEntriesMSB = 0x0d;

            /// <summary>
            ///     X-axis (LSB)
            /// </summary>
            public const byte XAxisLSB = 0x0e;

            /// <summary>
            ///     X-axis MSB
            /// </summary>
            public const byte XAxisMSB = 0x0f;

            /// <summary>
            ///     Y-axis (LSB)
            /// </summary>
            public const byte YAxisLSB = 0x10;

            /// <summary>
            ///     Y-Axis (MSB)
            /// </summary>
            public const byte YAxisMSB = 0x11;

            /// <summary>
            ///     Z-axis (LSB)
            /// </summary>
            public const byte ZAxisLSB = 0x12;

            /// <summary>
            ///     Z-axis (MSB)
            /// </summary>
            public const byte ZAxisMSB = 0x13;

            /// <summary>
            ///     Temperature (LSB)
            /// </summary>
            public const byte TemperatureLSB = 0x14;

            /// <summary>
            ///     Temperature (MSB)
            /// </summary>
            public const byte TemperatureMSB = 0x15;

            /// <summary>
            ///     Soft reset register.
            /// </summary>
            /// <remarks>
            ///     Writing 0x52 (ASCII for R) resets the sensor.
            ///     All register settings are cleared, the sensor is placed into standby mode.
            /// </remarks>
            public const byte SoftReset = 0x1f;

            /// <summary>
            ///     Activity threshold (LSB)
            /// </summary>
            public const byte ActivityThresholdLSB = 0x20;

            /// <summary>
            ///     Activity threshold (MSB)
            /// </summary>
            public const byte ActivityThresholdMSB = 0x21;

            /// <summary>
            ///     Activity time count.
            /// </summary>
            /// <remarks>
            ///     The contents of this register indicates the number of readings in any
            ///     of the axis that must exceed the activity threshold before an interrupt
            ///     is generated.
            /// </remarks>
            public const byte ActivityTimeCount = 0x22;

            /// <summary>
            ///     Inactivity threshold (LSB)
            /// </summary>
            public const byte InactivityThresholdLSB = 0x23;

            /// <summary>
            ///     Inactivity threshold (MSB)
            /// </summary>
            public const byte InactivityThresholdMSB = 0x24;

            /// <summary>
            ///     Inactivity time count (LSB).
            /// </summary>
            /// <remarks>
            ///     The contents of this register indicates the number of readings in any
            ///     of the axis that must be below the inactivity threshold before an
            ///     interrupt is generated.
            /// </remarks>
            public const byte InactivityCountLSB = 0x25;

            /// <summary>
            ///     Inactivity time count (MSB).
            /// </summary>
            /// <remarks>
            ///     The contents of this register indicates the number of readings in any
            ///     of the axis that must be below the inactivity threshold before an
            ///     interrupt is generated.
            /// </remarks>
            public const byte InactivityCountMSB = 0x26;

            /// <summary>
            ///     Activity / Inactivity control.
            /// </summary>
            public const byte ActivityInactivityControl = 0x27;

            /// <summary>
            ///     FIFO Control.
            /// </summary>
            public const byte FIFOControl = 0x28;

            /// <summary>
            ///     FIFO samples to store.
            /// </summary>
            public const byte FIFOSampleCount = 0x29;

            /// <summary>
            ///     Interrupt map register (1)
            /// </summary>
            public const byte InterruptMap1 = 0x2a;

            /// <summary>
            ///     Interrupt map register (2)
            /// </summary>
            public const byte InterruptMap2 = 0x2b;

            /// <summary>
            ///     Filter control register.
            /// </summary>
            public const byte FilterControl = 0x2c;

            /// <summary>
            ///     Power control.
            /// </summary>
            public const byte PowerControl = 0x2d;

            /// <summary>
            ///     Self test.
            /// </summary>
            /// <remarks>
            ///     Setting this register to 0x01 forces a self test on th X, Y
            ///     and Z axes.
            /// </remarks>
            public const byte SelfTest = 0x26;
        }

        /// <summary>
        ///     Masks for the bits in the Power Control register.
        /// </summary>
        protected static class PowerControlMask
        {
            /// <summary>
            ///     Place the sensor inStandby.
            /// </summary>
            public const byte Standby = 0x00;

            /// <summary>
            ///     Make measurements.
            /// </summary>
            public const byte Measure = 0x01;

            /// <summary>
            ///     Auto-sleep.
            /// </summary>
            public const byte AutoSleep = 0x04;

            /// <summary>
            ///     Wakeup mode.
            /// </summary>
            public const byte WakeupMode = 0x08;

            /// <summary>
            ///     Low noise mode.
            /// </summary>
            public const byte LowNoise = 0x10;

            /// <summary>
            ///     Ultra-low noise mode.
            /// </summary>
            public const byte UltralowNoise = 0x20;

            /// <summary>
            ///     External clock enabled on the INT1 pin.
            /// </summary>
            public const byte ExternalClock = 0x40;
        }

        /// <summary>
        ///     Masks for the bit in the filter control register.
        /// </summary>
        protected static class FilterControlMask
        {
            /// <summary>
            ///     Data rate of 12.5Hz
            /// </summary>
            public const byte DataRate12HalfHz = 0x00;

            /// <summary>
            ///     Data rate of 25 Hz
            /// </summary>
            public const byte DataRate25Hz = 0x01;

            /// <summary>
            ///     Data rate of 50 Hz.
            /// </summary>
            public const byte DataRate50Hz = 0x02;

            /// <summary>
            ///     Data rate 100 Hz.
            /// </summary>
            public const byte DataRate100Hz = 0x03;

            /// <summary>
            ///     Data rate of 200 Hz.
            /// </summary>
            public const byte DataRate200Hz = 0x04;

            /// <summary>
            ///     Data rate of 400 Hz
            /// </summary>
            public const byte DataRate400Hz = 0x05;

            /// <summary>
            ///     Enable the external sampling trigger.
            /// </summary>
            /// <remarks>
            ///     Setting this bit to 1 enables the sampling to be controlled by the INT2 pin.
            /// </remarks>
            public const byte ExternalSampling = 0x08;

            /// <summary>
            ///     Half or quarter bandwidth.
            /// </summary>
            /// <remarks>
            ///     Setting this bit to 1 changes the anti-aliasing filters from 1/2 the output
            ///     data rate to 1/4 the output data rate.
            /// </remarks>
            public const byte HalfBandwidth = 0x10;

            /// <summary>
            ///     Set the range to +/- 2g.
            /// </summary>
            public const byte Range2g = 0x00;

            /// <summary>
            ///     Set the range to +/- 4g
            /// </summary>
            public const byte Range4g = 0x40;

            /// <summary>
            ///     Set the range to +/- 8g
            /// </summary>
            public const byte Range8g = 0x80;
        }

        /// <summary>
        ///     Bit masks for the interrupt 1 / 2 control.
        /// </summary>
        protected static class InterruptMask
        {
            /// <summary>
            ///     Bit indicating that data is ready for processing.
            /// </summary>
            public const byte DataReady = 0x01;

            /// <summary>
            ///     Bit indicating that data is ready in the FIFO buffer.
            /// </summary>
            public const byte FIFODataReady = 0x02;

            /// <summary>
            ///     Bit indicating that the FIFO buffer has reached the high watermark.
            /// </summary>
            public const byte FIFOHighWatermarkReached = 0x04;

            /// <summary>
            ///     Bit indicating that the FIFO buffer has overrun.
            /// </summary>
            public const byte FIFOOverrun = 0x08;

            /// <summary>
            ///     Activity interrupt bit.
            /// </summary>
            public const byte Activity = 0x10;

            /// <summary>
            ///     Inactivity interrupt.
            /// </summary>
            public const byte Inactivity = 0x20;

            /// <summary>
            ///     Awake interrupt.
            /// </summary>
            public const byte Awake = 0x40;

            /// <summary>
            ///     Interrupt active high / low (1 = low, 0 = high).
            /// </summary>
            public const byte ActiveLow = 0x80;
        }

        /// <summary>
        ///     FIFO control bits.
        /// </summary>
        protected static class FIFOControlMask
        {
            /// <summary>
            ///     Disable FIFO mode.
            /// </summary>
            public const byte FIFODisabled = 0x00;

            /// <summary>
            ///     Enable FiFO oldest saved first mode.
            /// </summary>
            public const byte FIFOOldestSaved = 0x01;

            /// <summary>
            ///     Enable FIFOI stream mode.
            /// </summary>
            public const byte FIFOStreamMode = 0x02;

            /// <summary>
            ///     Enable FIFO triggered mode.
            /// </summary>
            public const byte FIFOTriggeredMode = 0x03;

            /// <summary>
            ///     When this bit is set to 1, the temperature data is stored in the FIFO
            ///     buffer as well as the x, y and z axis data.
            /// </summary>
            public const byte StoreTemperatureData = 0x04;

            /// <summary>
            ///     MSB of the FIFO sample count.  This allows the FIFO buffer to contain 512 samples.
            /// </summary>
            public const byte AboveHalf = 0x08;
        }

        /// <summary>
        ///     Control bits determining how the activity / inactivity functionality is configured.
        /// </summary>
        protected static class ActivityInactivityControlMask
        {
            /// <summary>
            ///     Determine if the activity functionality is enabled (1) or disabled (0).
            /// </summary>
            public const byte ActivityEnable = 0x01;

            /// <summary>
            ///     Determine is activity mode is in reference (1) or absolute mode (0).
            /// </summary>
            public const byte ActivityMode = 0x02;

            /// <summary>
            ///     Determine if inactivity mode is enabled (1) or disabled (0).
            /// </summary>
            public const byte InactivityEnable = 0x04;

            /// <summary>
            ///     Determine is inactivity mode is in reference (1) or absolute mode (0).
            /// </summary>
            public const byte Inactivitymode = 0x08;

            /// <summary>
            ///     Default mode.
            /// </summary>
            /// <remarks>
            ///     Activity and inactivity detection are both enabled, and their interrupts
            ///     (if mapped) must be acknowledged by the host processor by reading the STATUS
            ///     register. Auto-sleep is disabled in this mode. Use this mode for free fall
            ///     detection applications.
            /// </remarks>
            public const byte DefaultMode = 0x00;

            /// <summary>
            ///     Link activity and inactivity.
            /// </summary>
            /// <remarks>
            ///     Activity and inactivity detection are linked sequentially such that only one
            ///     is enabled at a time. Their interrupts (if mapped) must be acknowledged by
            ///     the host processor by reading the STATUS register.
            /// </remarks>
            public const byte LinkedMode = 0x10;

            /// <summary>
            /// </summary>
            /// <remarks>
            ///     Activity and inactivity detection are linked sequentially such that only one is
            ///     enabled at a time, and their interrupts are internally acknowledged (do not
            ///     need to be serviced by the host processor).
            ///     To use either linked or looped mode, both ACT_EN (Bit 0) and INACT_EN (Bit 2)
            ///     must be set to 1; otherwise, the default mode is used. For additional information,
            ///     refer to the Linking Activity and Inactivity Detection section.
            /// </remarks>
            public const byte LoopMode = 0x30;
        }

        #endregion Classes / structures

        #region Properties

        /// <summary>
        ///     Read the device ID, MEMS ID, Part ID and silicon revision ID and
        ///     encode the value in a 32-bit integer.
        /// </summary>
        public int DeviceID
        {
            get
            {
                var deviceID = _adxl362.WriteRead(new byte[] { 0x0b, 0x00 }, 6);
                int result = deviceID[0];
                result |= deviceID[1] << 8;
                result |= deviceID[2] << 16;
                result |= deviceID[3] << 24;
                return result;
            }
        }

        /// <summary>
        ///     X-axis sensor reading.
        /// </summary>
        /// <remarks>
        ///     Read must be called before this property is valid.
        /// </remarks>
        public short X { get; private set; }

        /// <summary>
        ///     Y-axis sensor reading.
        /// </summary>
        /// <remarks>
        ///     Read must be called before this property is valid.
        /// </remarks>
        public short Y { get; private set; }

        /// <summary>
        ///     Z-axis sensor reading.
        /// </summary>
        /// <remarks>
        ///     Read must be called before this property is valid.
        /// </remarks>
        public short Z { get; private set; }

        #endregion Properties

        #region Constructors

        /// <summary>
        ///     Default constructor is private to prevent it being used.
        /// </summary>
        private ADXL362()
        {
        }

        /// <summary>
        ///     Create a new ADXL362 object using the specified SPI module.
        /// </summary>
        /// <param name="module">SPI module to use.</param>
        /// <param name="chipSelect">Chip select pin.</param>
        /// <param name="speed">Speed of the SPI bus.</param>
        public ADXL362(SPI.SPI_module module, Cpu.Pin chipSelect, ushort speed = 10)
        {
            _adxl362 = new SPIBus(module, chipSelect, speed);
        }

        #endregion Constructors
    }
}