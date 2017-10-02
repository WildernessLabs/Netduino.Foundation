using System;
using Microsoft.SPOT.Hardware;
using Netduino.Foundation.Core;

namespace Netduino.Foundation.Sensors.Light
{
    /// <summary>
    /// Driver for the TSL2561 light-to-digital converter.
    /// </summary>
    public class TSL2561
    {
        #region Constants
        
        /// <summary>
        /// The command bit in the Command Register.
        /// 
        /// See page 13 of the datasheet.
        /// </summary>
        public byte COMMAND_BIT = 0x80;

        /// <summary>
        /// The interrupt clear bit in the Command Register.
        /// 
        /// See page 13 of the datasheet.
        /// </summary>
        private byte CLEAR_INTERRUPT_BIT = 0x40;

        #endregion Constants

        #region Enums

        /// <summary>
        /// Valid addresses for the sensor.
        /// </summary>
        public enum Addresses : byte { Default = 0x39, Address0 = 0x29, Address1 = 0x49 };

        /// <summary>
        /// Integration timing.
        ///
        /// See Timing Register on page 14 of the data sheet.
        /// </summary>
        /// <remarks>
        /// Valid integration times are 13.7ms, 101ms, 402ms and Manual.
        /// </remarks>
        public enum IntegrationTiming : byte { Ms13 = 0, Ms101, Ms402, Manual };

        /// <summary>
        /// Possible gain setting for the sensor.
        ///
        /// See Timing Register on page 14 of the data sheet.
        /// </summary>
        /// <remarks>
        /// Possible gain values are low (x1) and high (x16).
        /// </remarks>
        public enum Gain { Low, High };

        /// <summary>
        /// TSL2561 register locations.
        ///
        /// See Register Set on page 12 and Command Register on page 13 of the datasheet.
        /// </summary>
        /// <remarks>
        /// All of the register numbers have 0x80 added to the register.  When reading
        /// or witing to a register the application must set the CMD bit in the command 
        /// register (see page 13) and the register address is written into the lower
        /// four bits of the Command Register.
        /// </remarks>
        public enum Registers : byte
        {
            Control = 0x80,
            Timing = 0x81,
            ThresholdLowLow = 0x82,
            ThresholdLowHigh = 0x83,
            ThresholdHighLow = 0x84,
            ThresholdHighHigh = 0x85,
            InterruptControl = 0x86,
            ID = 0x8a,
            Data0Low = 0x8c,
            Data0High = 0x8d,
            Data1Low = 0x8e,
            Data1High = 0x8f
        }

        /// <summary>
        /// Determine if interrupts are enabled or not.
        ///
        /// See Interrupt Control Register on page 15 of the datasheet.
        /// </summary>
        public enum InterruptMode : byte { Disable = 0, Enable };

        #endregion Enums

        #region Properties

        /// <summary>
        /// Address of the TSL2561 sensor on the I2C bus.
        /// </summary>
        private byte _address;
        private byte Address
        {
            get { return _address; }
            set
            {
                if ((value != (byte) Addresses.Address0) && (value != (byte) Addresses.Default) && (value != (byte) Addresses.Address1))
                {
                    throw new ArgumentOutOfRangeException("Address", "Address should be 0x29, 0x39 or 0x49.");
                }
                _address = value;
            }
        }

        /// <summary>
        /// Lux reading from the TSL2561 sensor.
        /// </summary>
        public double Lux
        {
            get
            {
                ushort data0 = _tsl2561.ReadUShort((byte) Registers.Data0Low, ByteOrder.LittleEndian);
                ushort data1 = _tsl2561.ReadUShort((byte) Registers.Data1Low, ByteOrder.LittleEndian);
                if ((data0 == 0xffff) | (data1 == 0xffff))
                {
                    return(0.0);
                }
                double d0 = data0;
                double d1 = data1;
                double ratio = d1 / d0;

                double milliseconds = 0;
                switch (Timing)
                {
                    case IntegrationTiming.Ms13:
                        milliseconds = 14;
                        break;
                    case IntegrationTiming.Ms101:
                        milliseconds = 101;
                        break;
                    case IntegrationTiming.Ms402:
                        milliseconds = 402;
                        break;
                    case IntegrationTiming.Manual:
                        milliseconds = 0;
                        break;
                }
                d0 *= (402.0 / milliseconds);
                d1 *= (402.0 / milliseconds);
                if (SensorGain == Gain.Low)
                {
                    d0 *= 16;
                    d1 *= 16;
                }

                if (ratio < 0.5)
                {
                    //return(0.0304 * d0 - 0.062 * d0 * Microsoft.SPOT.Math(ratio, 1.4));
                }
                if (ratio < 0.61)
                {
                    return(0.0224 * d0 - 0.031 * d1);
                }
                if (ratio < 0.80)
                {
                    return(0.0128 * d0 - 0.0153 * d1);
                }
                if (ratio < 1.30)
                {
                    return(0.00146 * d0 - 0.00112 * d1);
                }
                return(0.0);
            }
        }

        /// <summary>
        /// ID of the sensor.
        /// </summary>
        /// <remarks>
        /// The ID register (page 16 of the datasheet) gives two pieces of the information:
        ///     Part Number: bits 4-7 (0000 = TSL2560, 0001 = TSL2561)
        ///     Revision number: bits 0-3
        /// </remarks>
        public byte ID
        {
            get
            {
                return(_tsl2561.ReadRegister((byte) Registers.ID));
            }
        }

        /// <summary>
        /// Gain of the sensor.
        /// 
        /// The sensor gain can be set to high or low.
        /// </summary>
        /// <remarks>
        /// The sensor Gain bit can be fouond in the Timing Register.  This allows the gain
        /// to be set to High (16x) or Low (1x).
        /// 
        /// See page 14 of the datasheet.
        /// </remarks>
        public Gain SensorGain
        {
            get
            {
                byte data = (byte) (_tsl2561.ReadRegister((byte) Registers.Timing) & 0x10);
                return (data == 0 ? Gain.Low : Gain.High);
            }
            set
            {
                byte data = _tsl2561.ReadRegister((byte) Registers.Timing);
                if (value == Gain.Low)
                {
                    data &= 0xef;       // Set bit 4 to 0.
                }
                else
                {
                    data |= 0x10;       // Set bit 4 to 1.
                }
                _tsl2561.WriteRegister((byte) Registers.Timing, data);
            }
        }

        /// <summary>
        /// Integration timing for the sensor reading.
        /// </summary>
        public IntegrationTiming Timing
        {
            get
            {
                //TODO: Implement this.
                return IntegrationTiming.Manual;
            }
            set
            {
                sbyte timing = (sbyte) _tsl2561.ReadRegister((byte) Registers.Timing);
                if (SensorGain == Gain.High)
                {
                    timing |= 0x10;
                }
                else
                {
                    timing &= ~0x10;
                }
                timing &= ~ 0x03;
                timing |= (sbyte) ((sbyte) value & 0x03);
                _tsl2561.WriteRegister((byte) Registers.Timing, (byte) timing);
            }
        }

        /// <summary>
        /// ICommunicationBus object used to communicate with the sensor.
        /// </summary>
        /// <remarks>
        /// In this case the actual object will always be an I2SBus object.
        /// </remarks>
        private ICommunicationBus _tsl2561 = null;

        #endregion Properties

        #region Constructor(s)

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <remarks>
        /// Default constructor is private to force the setting of the I2C parameters.
        /// </remarks>
        private TSL2561()
        {
        }

        /// <summary>
        /// Create a new instance of the TSL2561 class with the specified I2C address.
        /// </summary>
        /// <remarks>
        /// By default the sensor will be set to low gain.
        /// <remarks>
        public TSL2561(byte address = (byte) Addresses.Default, ushort speed = 100)
        {
            Address = address;
            I2CBus device = new I2CBus(address, speed);
            _tsl2561 = (ICommunicationBus) device;
        }

        #endregion Constructor(s)

        #region Methods

        /// <summary>
        /// Turn the TSL2561 off.
        /// <summary>
        /// <remarks>
        /// Reset the power bits in the control register (page 13 of the datasheet).
        /// </remarks>
        public void TurnOff()
        {
            _tsl2561.WriteRegister((byte) Registers.Control, 0x00);
        }

        /// <summary>
        /// Turn the TSL2561 on.
        /// </summary>
        /// <remarks>
        /// Set the power bits in the control register (page 13 of the datasheet).
        /// </remarks>
        public void TurnOn()
        {
            _tsl2561.WriteRegister((byte) Registers.Control, 0x03);
        }

        /// <summary>
        /// Clear the interrupt flag.
        /// </summary>
        public void ClearInterrupt()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Put the sensor into manual integration mode.
        /// </summary>
        public void ManualStart()
        {
            byte timing = _tsl2561.ReadRegister((byte) Registers.Timing);
            timing |= 0x03;
            _tsl2561.WriteRegister((byte) Registers.Timing, timing);
            timing |= 0xf7; //  ~0x08;
            _tsl2561.WriteRegister((byte) Registers.Timing, timing);
        }

        /// <summary>
        /// Turn off manual integration mode.
        /// </summary>
        public void ManualStop()
        {
            byte timing = _tsl2561.ReadRegister((byte) Registers.Timing);
            timing &= 0xf7; //  ~0x08;
            _tsl2561.WriteRegister((byte) Registers.Timing, timing);
        }

        /// <summary>
        /// Set the upper and lower limits for the threshold values.
        /// </summary>
        /// <remarks>
        /// Interrupts can be set to be generated on every conversion or when the
        /// conversions are outside of the threshold limits for a specified number
        /// of consecutive conversions.
        /// </remarks>
        /// <param name="lowerLimit">Lower threshold, light levels lower than this value will generate and interrupt.</param>
        /// <param name="upperLimit">Upper threshold, light levels higher than this will generate an interrupt.</param>
        public void SetInteruptThreshold(ushort lowerLimit, ushort upperLimit)
        {
            _tsl2561.WriteUShort((byte) Registers.ThresholdLowLow, lowerLimit, ByteOrder.LittleEndian);
            _tsl2561.WriteUShort((byte) Registers.ThresholdHighLow, upperLimit, ByteOrder.LittleEndian);
        }

        /// <summary>
        /// Turn interrupts on or off and set the conversion trigger count.
        /// </summary>
        /// <remarks>
        /// The conversion count is the number of conversions that must be outside
        /// of the upper and lower limits before and interrupt is generated.
        /// 
        /// See Interrupt Control Register on page 15 and 16 of the datasheet.
        /// </remarks>
        /// <param name="mode"></param>
        /// <param name="conversionCount">Number of conversions that must be outside of the threshold before an interrupt is generated.</param>
        public void SetInterruptMode(InterruptMode mode, byte conversionCount)
        {
            if (conversionCount > 15)
            {
                throw new ArgumentOutOfRangeException("conversionCount", "Conversion count must be in the range 0-15 inclusive.");
            }
            int registerValue = ((((byte) mode) | 0x03) << 4) | (conversionCount | 0x0f);
            _tsl2561.WriteRegister((byte) Registers.InterruptControl, (byte) (registerValue & 0xff));
        }

        #endregion
    }
}
