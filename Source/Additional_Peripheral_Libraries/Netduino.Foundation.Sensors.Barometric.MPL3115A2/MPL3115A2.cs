using System;
using System.Threading;
using Netduino.Foundation.Devices;

namespace Netduino.Foundation.Sensors.Barometric
{
    public class MPL3115A2
    {
        #region Enums

        /// <summary>
        ///     Registers for non-FIFO mode.
        /// </summary>
        private enum Registers : byte
        {
            Status = 0x06,
            PressureMSB = 0x01,
            PressureCSB = 0x02,
            PressureLSB = 0x03,
            TemperatureMSB = 0x04,
            TemperatureLSB = 0x05,
            DataReadyStatus = 0x06,
            PressureDeltaMSB = 0x07,
            PressureDeltaCSB = 0x08,
            PressureDeltaLSB = 0x09,
            TemperatureDeltaMSB = 0x0a,
            TemperatureDeltaLSB = 0x0b,
            WhoAmI = 0x0c,
            FifoStatus = 0x0d,
            FiFoDataAccess = 0x0e,
            FifoSetup = 0x0f,
            TimeDelay = 0x11,
            InterruptSource = 0x12,
            DataConfiguration = 0x13,
            BarometricMSB = 0x14,
            BarometricLSB = 0x15,
            PressureTargetMSB = 0x16,
            PressureTargetLSB = 0x17,
            TemperatureTarget = 0x18,
            PressureWindowMSB = 0x19,
            PressureWindowLSB = 0x1a,
            TemperatureWindow = 0x1b,
            PressureMinimumMSB = 0x1c,
            PressureMinimumCSB = 0x1d,
            PressureMinimumLSB = 0x1e,
            TemperatureMinimumMSB = 0x1f,
            TemperatureMinimumLSB = 0x20,
            PressureMaximumMSB = 0x21,
            PressureMaximumCSB = 0x22,
            PressureMaximumSB = 0x23,
            TemperatureMaximumMSB = 0x24,
            TemperatureMaximumLSB = 0x25,
            Control1 = 0x26,
            Control2 = 0x27,
            Control3 = 0x28,
            Control4 = 0x29,
            Control5 = 0x2a,
            PressureOffset = 0x2b,
            TemperatureOffset = 0x2c,
            AltitudeOffset = 0x2d
        }

        /// <summary>
        ///     Status register bits.
        /// </summary>
        private enum ReadingStatus : byte
        {
            NewTemperatureDataReady = 0x02,
            NewPressureDataAvailable = 0x04,
            NewTemperatureOrPressureDataReady = 0x08,
            TemperatureDataOverwrite = 0x20,
            PressureDataOverwrite = 0x40,
            PressureOrTemperatureOverwrite = 0x80
        }

        #endregion Enums

        #region Classes / structures

        /// <summary>
        ///     Byte values for the various masks in the control registers.
        /// </summary>
        /// <remarks>
        ///     For further information see section 7.17 of the datasheet.
        /// </remarks>
        private class ControlRegisterBits
        {
            /// <summary>
            ///     Control1 - Device in standby when bit 0 is 0.
            /// </summary>
            public static readonly byte Standby = 0x00;

            /// <summary>
            ///     Control1 - Device in active when bit 0 is set to 1
            /// </summary>
            public static readonly byte Active = 0x01;

            /// <summary>
            ///     Control1 - Initiate a single measurement immediately.
            /// </summary>
            public static readonly byte OneShot = 0x02;

            /// <summary>
            ///     Control1 - Perform a software reset when in standby mode.
            /// </summary>
            public static readonly byte SoftwareResetEnable = 0x04;

            /// <summary>
            ///     Control1 - Set the oversample rate to 1.
            /// </summary>
            public static readonly byte OverSample1 = 0x00;

            /// <summary>
            ///     Control1 - Set the oversample rate to 2.
            /// </summary>
            public static readonly byte OverSample2 = 0x08;

            /// <summary>
            ///     Control1 - Set the oversample rate to 4.
            /// </summary>
            public static readonly byte OverSample4 = 0x10;

            /// <summary>
            ///     Control1 - Set the oversample rate to 8.
            /// </summary>
            public static readonly byte OverSample8 = 0x18;

            /// <summary>
            ///     Control1 - Set the oversample rate to 16.
            /// </summary>
            public static readonly byte OverSample16 = 0x20;

            /// <summary>
            ///     Control1 - Set the oversample rate to 32.
            /// </summary>
            public static readonly byte OverSample32 = 0x28;

            /// <summary>
            ///     Control1 - Set the oversample rate to 64.
            /// </summary>
            public static readonly byte OverSample64 = 0x30;

            /// <summary>
            ///     Control1 - Set the oversample rate to 128.
            /// </summary>
            public static readonly byte OverSample128 = 0x38;

            /// <summary>
            ///     Control1 - Altimeter or Barometer mode (Altimeter = 1, Barometer = 0);
            /// </summary>
            public static readonly byte AlimeterMode = 0x80;
        }

        /// <summary>
        ///     Pressure/Temperature data configuration register bits.
        /// </summary>
        /// <remarks>
        ///     For more information see section 7.7 of the datasheet.
        /// </remarks>
        public class ConfigurationRegisterBits
        {
            /// <summary>
            ///     PT_DATA_CFG - Enable the event detection.
            /// </summary>
            public static readonly byte DataReadyEvent = 0x01;

            /// <summary>
            ///     PT_DATA_CFG - Enable the pressure data ready events.
            /// </summary>
            public static readonly byte EnablePressureEvent = 0x02;

            /// <summary>
            ///     PT_DATA_CFG - Enable the temperature data ready events.
            /// </summary>
            public static readonly byte EnableTemperatureEvent = 0x04;
        }

        #endregion Classes / structures

        #region Member variables / fields

        /// <summary>
        ///     Object used to communicate with the sensor.
        /// </summary>
        private readonly ICommunicationBus _mpl3115a2;

        #endregion Member variables / fields

        #region Properties

        /// <summary>
        ///     Last temperature reading in degrees C (note that Read should be called before
        ///     this value is accessed.)
        /// </summary>
        public double Temperature { get; private set; }

        /// <summary>
        ///     Minimum temperature since the sensor was last reset.
        /// </summary>
        public double TemperatureMinimum
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        ///     Maximum temperature since the sensor was last reset.
        /// </summary>
        public double TemperatureMaximum
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        ///     Target temperature.
        /// </summary>
        public double TemperatureTarget
        {
            set { throw new NotImplementedException(); }
        }

        /// <summary>
        ///     Temperature window.
        /// </summary>
        public double TemperatureWindow
        {
            set { throw new NotImplementedException(); }
        }

        /// <summary>
        ///     Last pressure reading in Pascals (note that Read should be called before
        ///     this value is accessed.)
        /// </summary>
        public double Pressure { get; private set; }

        /// <summary>
        ///     Maximum pressure reading since the last reset.
        /// </summary>
        public double PressureMaximum
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        ///     Minimum pressure reading since the last reset.
        /// </summary>
        public double PressureMinimum
        {
            get { throw new NotImplementedException(); }
        }

        /// <summary>
        ///     Set the pressure target.
        /// </summary>
        /// <remarks>
        ///     An interrupt will be generated when the pressure reaches the
        ///     target pressure +/- the pressure window value.
        /// </remarks>
        /// ]
        public double PressureTarget
        {
            set { throw new NotImplementedException(); }
        }

        /// <summary>
        ///     Set the Pressure Window
        /// </summary>
        /// <remarks>
        ///     See section 6.6.2 of the data sheet.
        /// </remarks>
        public double PressureWindow
        {
            set { throw new NotImplementedException(); }
        }

        /// <summary>
        ///     Check if the part is in standby mode or change the standby mode.
        /// </summary>
        /// <remarks>
        ///     Changes the SBYB bit in Control register 1 to put the device to sleep
        ///     or to allow measurements to be made.
        /// </remarks>
        public bool Standby
        {
            get { return(_mpl3115a2.ReadRegister((byte) Registers.Control1) & 0x01) > 0; }
            set
            {
                var status = _mpl3115a2.ReadRegister((byte) Registers.Control1);
                if (value)
                {
                    status &= (byte) ~(ControlRegisterBits.Active);
                }
                else
                {
                    status |= ControlRegisterBits.Active;
                }
                _mpl3115a2.WriteRegister((byte) Registers.Control1, status);
            }
        }

        /// <summary>
        ///     Get the status register from the sensor.
        /// </summary>
        public byte Status
        {
            get { return _mpl3115a2.ReadRegister((byte) Registers.Status); }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        ///     Default constructor (private to prevent it being called).
        /// </summary>
        private MPL3115A2()
        {
        }

        /// <summary>
        ///     Create a new MPL3115A2 object with the default address and speed settings.
        /// </summary>
        /// <param name="address">Address of the sensor (default = 0x60).</param>
        /// <param name="speed">Bus speed to use when communicating with the sensor (Maximum is 400 kHz).</param>
        public MPL3115A2(byte address = 0x60, ushort speed = 400)
        {
            var device = new I2CBus(address, speed);
            _mpl3115a2 = device;
            if (_mpl3115a2.ReadRegister((byte) Registers.WhoAmI) != 0xc4)
            {
                throw new Exception("Unexpected device ID, expected 0xc4");
            }
            _mpl3115a2.WriteRegister((byte) Registers.Control1,
                                     (byte) (ControlRegisterBits.Active | ControlRegisterBits.OverSample128));
            _mpl3115a2.WriteRegister((byte) Registers.DataConfiguration,
                                     (byte) (ConfigurationRegisterBits.DataReadyEvent |
                                             ConfigurationRegisterBits.EnablePressureEvent |
                                             ConfigurationRegisterBits.EnableTemperatureEvent));
            Read();
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Decode the three data bytes representing the pressure into a doubleing
        ///     point pressure value.
        /// </summary>
        /// <param name="msb">MSB for the pressure sensor reading.</param>
        /// <param name="csb">CSB for the pressure sensor reading.</param>
        /// <param name="lsb">LSB of the pressure sensor reading.</param>
        /// <returns>Pressure in Pascals.</returns>
        private double DecodePresssure(byte msb, byte csb, byte lsb)
        {
            uint pressure = msb;
            pressure <<= 8;
            pressure |= csb;
            pressure <<= 8;
            pressure |= lsb;
            return pressure / 64.0;
        }

        /// <summary>
        ///     Encode the pressure into the sensor reading byes.
        ///     This method is used to allow the target pressure and pressure window
        ///     properties to be set.
        /// </summary>
        /// <param name="pressure">Pressure in Pascals to encode.</param>
        /// <returns>Array holding the three byte values for the sensor.</returns>
        private byte[] EncodePressure(double pressure)
        {
            var result = new byte[3];
            var temp = (uint) (pressure * 64);
            result[2] = (byte) (temp & 0xff);
            temp >>= 8;
            result[1] = (byte) (temp & 0xff);
            temp >>= 8;
            result[0] = (byte) (temp & 0xff);
            return result;
        }

        /// <summary>
        ///     Decode the two bytes representing the temperature into degrees C.
        /// </summary>
        /// <param name="msb">MSB of the temperature sensor reading.</param>
        /// <param name="lsb">LSB of the temperature sensor reading.</param>
        /// <returns>Temperature in degrees C.</returns>
        private double DecodeTemperature(byte msb, byte lsb)
        {
            ushort temperature = msb;
            temperature <<= 8;
            temperature |= lsb;
            return temperature / 256.0;
        }

        /// <summary>
        ///     Encode a temperature into sensor reading bytes.
        ///     This method is needed in order to allow the temperature target
        ///     and window properties to work.
        /// </summary>
        /// <param name="temperature">Temperature to encode.</param>
        /// <returns>Temperature tuple containing the two bytes for the sensor.</returns>
        private byte[] EncodeTemperature(double temperature)
        {
            var result = new byte[2];
            var temp = (ushort) (temperature * 256);
            result[1] = (byte) (temp & 0xff);
            temp >>= 8;
            result[0] = (byte) (temp & 0xff);
            return result;
        }

        /// <summary>
        ///     Force a read of the current sensor values and update the Temperature
        ///     and Pressure properties.
        /// </summary>
        public void Read()
        {
            //
            //  Force the sensor to make a reading by setting the OST bit in Control
            //  register 1 (see 7.17.1 of the datasheet).
            //
            Standby = false;
            //
            //  Pause until both temperature and pressure readings are available.
            //            
            while ((Status & 0x06) != 0x06)
            {
                Thread.Sleep(5);
            }
            Thread.Sleep(100);
            var data = _mpl3115a2.ReadRegisters((byte) Registers.PressureMSB, 5);
            Pressure = DecodePresssure(data[0], data[1], data[2]);
            Temperature = DecodeTemperature(data[3], data[4]);
        }

        /// <summary>
        ///     Reset the sensor.
        /// </summary>
        public void Reset()
        {
            var data = _mpl3115a2.ReadRegister((byte) Registers.Control1);
            data |= 0x04;
            _mpl3115a2.WriteRegister((byte) Registers.Control1, data);
        }

        #endregion Methods
    }
}