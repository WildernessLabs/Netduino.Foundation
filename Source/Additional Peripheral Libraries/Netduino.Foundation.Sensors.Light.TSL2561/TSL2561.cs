using System;
using Microsoft.SPOT;

namespace Netduino.Foundation.Sensors.Light
{
    public class TSL2561
    {
        /// <summary>
        /// Valid addresses for the sensor.
        /// </summary>
        enum Addresses : byte { Default = 0x39, Address0 = 0x29, Address1 = 0x49 };

        /// <summary>
        /// Integration timing.
        /// </summary>
        public enum IntegrationTiming : byte { Ms13 = 0, Ms101, Ms402, Manual };

        /// <summary>
        /// Possible gain setting for the sensor.
        /// </summary>
        public enum Gain { Low, High };

        /// <summary>
        /// TSL2561 register locations.
        /// </summary>
        enum Registers : byte
        {
            Command = 0x80,
            Clear = 0xc0,
            Control = 0x00,
            Timing = 0x01,
            ThresholdLow = 0x02,
            ThresholdHigh = 0x04,
            InterruptControl = 0x06,
            ID = 0x0a,
            Data0 = 0x0c,
            Data1 = 0x0e,
        }

        /// <summary>
        /// Determine if interrupts are enabled or not.
        /// </summary>
        enum InterruptMode : byte { Disable = 0, Enable };

        /// <summary>
        /// Address of the TSL2561 sensor on the I2C bus.
        /// </summary>
        private byte _address;
        private byte Address
        {
            get { return _address; }
            set
            {
                if ((value != Addresses.Address0) && (value != Addresses.Address) && (value != Addresses.Address1))
                {
                    throw new ArgumentOutOfRangeException("Address", "Address should be 0x29, 0x39 or 0x49.");
                }
                _address = value;
            }
        }

        /// <summary>
        /// Lux reading from the TSL2561 sensor.
        /// </summary>
        public double Lux()
        {
            get
            {
                ushort data0 = ReadUShort(Registers.Data0);
                ushort data1 = ReadUShort(Registers.Data1);
                if ((data0 == 0xffff) | (data1 == 0xffff))
                {
                    return(0.0);
                }
                double d0 = data0;
                double d1 = data1;
                double ratio = d1 / d0;

                double milliseconds;
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
                    return(0.0304 * d0 - 0.062 * d0 * Math.Pow(ratio, 1.4));
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
        public byte ID
        {
            get
            {
                return(ReadByte(Registers.ID));
            }
        }

        /// <summary>
        /// Gain of the sensor.
        /// </summary>
        /// <remarks>
        /// Gain for the sensor.
        /// </remarks>
        public Gain SensorGain { get; set; }

        /// <summary>
        /// Integration timing for the sensor reading.
        /// </summary>
        public IntegrationTiming Timing
        {
            set
            {
                byte timing = ReadByte(Registers.Timing);
                if (Gain == SensorGain.High)
                {
                    timing |= 0x10;
                }
                else
                {
                    timing &= ~0x10;
                }
                timing &= ~ 0x03;
                timing |= (((byte) value) & 0x03);
                WriteByte(Registers.Timing, timing);
            }
        }

        /// <summary>
        /// I2C object used to communicate with the sensor.
        /// </summary>
        private I2CDevice Device { get; set; }

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
        public TSL2561(byte address, ushort speed)
        {
            Address = address;
            SensorGain = Gain.Low;
            Device = new I2CDevice(I2CDevice.Configuration(address, speed));
        }

        /// <summary>
        /// Read a byte from the specified register.
        /// </summary>
        private ReadByte(byte register)
        {
            byte[] data = new byte[1];
            byte[] registerAddress = { register };
            I2CDevice.I2CTransaction[] readData =
            {
                I2CDevice.CreateWriteTransaction(registerAddress),       
                I2CDevice.CreateReadTransaction(data)
            };
            int bytesTransferred = 0;
            int retryCount = 0;
            while (bytesTransferred != 2
            {
                if (retryCount > 3)
                {
                    throw new Exception("ReadByte: Retry count exceeded.");
                }
                retryCount++;
                bytesTransferred = Device.Execute(readData, 100);
            }
            return data[0];
        }

        /// <summary>
        /// Read an unsigned short from the specified register.
        /// </summary>
        private ushort ReadUShort(byte register)
        {
            byte[] data = new byte[2];
            byte[] registerAddress = { register };
            I2CDevice.I2CTransaction[] readData =
            {
                I2CDevice.CreateWriteTransaction(registerAddress),       
                I2CDevice.CreateReadTransaction(data)
            };
            int bytesTransferred = 0;
            int retryCount = 0;
            while (bytesTransferred != 2
            {
                if (retryCount > 3)
                {
                    throw new Exception("ReadUShort: Retry count exceeded.");
                }
                retryCount++;
                bytesTransferred = Device.Execute(readData, 100);
            }
            value = (ushort) ((data[0] << 8) + data[1]);
            return value;
        }

        /// <summary>
        /// Write a stream of bytes to the I2C device.
        /// </summary>
        private void Write(byte[] data)
        {
            I2CDevice.I2CTransaction[] writeData =
            {
                I2CDevice.CreateWriteTransaction(data)
            };
            int retryCount = 0;
            while (Device.Execute(writeData, 100) != data.Length)
            {
                if (retryCount > 3)
                {
                    throw new Exception("Write: Retry count exceeded.");
                }
                retryCount++;
            }
        }

        /// <summary>
        /// Write the specified value to the register.
        /// </summary>
        private void WriteByte(byte register, byte value)
        {
            byte[] data = new byte[2];
            data[0] = register;
            data[1] = value;
            Write(data);
        }

        /// <summary>
        /// Write the specified value to the register.
        /// </summary>
        private void WriteUShort(byte register, ushort value)
        {
            byte[] data = new byte[3];
            data[0] = register;
            data[1] = (ushort) (value & 0xff);
            data[2] = (ushort) (value >> 8);
            Write*data);
        }

        /// <summary>
        /// Turn the TSL2561 off.
        /// <summary>
        public void TurnOff()
        {
            WriteByte(Registers.Control, 0x00);
        }

        /// <summary>
        /// Turn the TSL2561 on.
        /// </summary>
        public TurnOn()
        {
            WriteByte(Registers.Control, 0x03);
        }

        /// <summary>
        /// Clear the interrupt flag.
        /// </summary>
        public void ClearInterrupt()
        {
            byte[] data = { Registers.Clear };
            Write(data);
        }

        /// <summary>
        /// Put the sensor into manual integration mode.
        /// </summary>
        public void ManualStart()
        {
            byte timing = ReadByte(Registers.Timing);
            timing |= 0x03;
            WriteByte(Registers.Timing, timing);
            timing |= ~0x08;
            WriteByte(Registers.Timing, timing);
        }

        /// <summary>
        /// Turn off manual integration mode.
        /// </summary>
        public void ManualStop()
        {
            byte timing = ReadByte(Registers.Timing);
            timing &= ~0x08;
            WriteByte(Registers.Timing, timing);
        }

        /// <summary>
        /// Set the upper and lower limits for the threshold values.
        /// </summary>
        /// <remarks>
        /// Interrupts can be set to be generated on every conversion or when the
        /// conversions are outside of the threshold limits for a specified number
        /// of consecutive conversions.
        /// </remarks>
        public void SetInteruptThreshold(ushort lowerLimit, ushort upperLimit)
        {
            WriteShort(Registers.ThresholdLow, lowerLimit);
            WriteShort(Registers.ThresholdHigh, upperLimit);
        }

        /// <summary>
        /// Turn interrupts on or off and set the conversion trigger count.
        /// </summary>
        /// <remarks>
        /// The conversion count is the number of conversions that must be outside
        /// of the upper and lower limits.  WAn interrupt will be generated when
        /// the conversion count reaches the specified value.
        /// <summary>
        public void SetInterruptMode(InterruptMode mode, byte conversionCount)
        {
            if (conversionCount > 15)
            {
                throw new ArgumentOutOfRangeException("conversionCount", "Conversion count must be in the range 0-15 inclusive.");
            }
            byte registerValue = ((mode | 0x03) << 4) | (conversionCount | 0x0f);
            WriteByte(Registers.InterruptControl, registerValue);
        }
    }
}
