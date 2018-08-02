using System;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using Netduino.Foundation;
using System.Threading;

namespace Netduino.Foundation.Sensors.Temperature
{
    public class DS18B20 : ITemperatureSensor
    {
        #region Enums

        /// <summary>
        ///     Type of one wire buses allowed.
        /// </summary>
        public enum BusModeType
        {
            /// <summary>
            ///     Indicate that the OneWire bus has a single device attached.
            /// </summary>
            SingleDevice,

            /// <summary>
            ///     Indicate that the OneWire bus has multiple devices attached.
            /// </summary>
            MultimpleDevices
        }

        #endregion Enums

        #region Commands class

        /// <summary>
        ///     Constants representing the various commands that can be issued to
        ///     the DS18B20.
        /// </summary>
        protected class Commands
        {
            /// <summary>
            ///     Start the A to D conversion of the current temperature into
            ///     digital value.
            /// </summary>
            public const byte StartConversion = 0x44;

            /// <summary>
            ///     Read bytes from the scratch pad.
            /// </summary>
            /// <remarks>
            ///     This command can be used to read the temperature, alarms and
            ///     configuration bytes from the scratch pad.
            /// </remarks>
            public const byte ReadScratchPad = 0xbe;

            /// <summary>
            ///     Write data to the scratch pad.
            /// </summary>
            /// <remarks>
            ///     Write the temperature high, and low alarms along with the device
            ///     configuration byte to the scratch pad.
            ///
            ///     Note that all three bytes must be written with this command in order
            ///     to prevent the data from being corrupted.
            /// </remarks>
            public const byte WriteScratchPad = 0x4e;

            /// <summary>
            ///     Issue the following command(s) to all devices on the bus.
            /// </summary>
            /// <remarks>
            ///     This command is useful in two cases:
            ///         - Writing the same command to all devices (say StartConversion)
            ///         - When there is only one device on the bus, the device ID does
            ///           not have to be written.
            /// </remarks>
            public const byte SkipROM = 0xcc;

            /// <summary>
            ///     Retrieve the device ID.
            /// </summary>
            /// <remarks>
            ///     This is useful when there is only one device on the bus and allows
            ///     the device ID to be retrieved.
            /// </remarks>
            public const byte ReadID = 0x33;

            /// <summary>
            ///     Inform the devices on the bus that the following commands (up to a reset)
            ///     are to be processed by a specific device.
            /// </summary>
            /// <remarks>
            ///     The device ID (64-bits) should follow this command.
            /// </remarks>
            public const byte MatchID = 0x55;

            /// <summary>
            ///     Copy the alarm high, alarm low and configuration bytes from the
            ///     scratch pad into the EEPROM.
            /// </summary>
            /// <remarks>
            ///     This can be used to store the data in to the EEPROM to survive a
            ///     power on reset.
            /// </remarks>
            public const byte CopyScratchPadToEEPROM = 0x48;

            /// <summary>
            ///     Copy the alarm high, alarm low and configuration bytes from the
            ///     EEPROM into the scratch pad.
            /// </summary>
            /// <remarks>
            ///     Copy the stored data from the EEPROM into the scratch pad.
            /// </remarks>
            public const byte CopyEEPROMToScratchPad = 0xb8;
        }

        #endregion Command class

        #region Constants

        /// <summary>
        ///     Minimum value that should be used for the polling frequency.
        /// </summary>
        /// <remarks>>
        ///     Default assumes that the sensor is working in 12-bit mode (factory setting).
        /// </remarks>
        public const ushort MinimumPollingPeriod = 750;

        #endregion Constants

        #region Member variables

        /// <summary>
        ///     Update interval in milliseconds
        /// </summary>
        protected ushort _updateInterval = 100;

        /// <summary>
        ///     Output port connected to the DS18B20 sensor.
        /// </summary>
        /// <remarks>
        ///     This object is used by the Sensor object.
        /// </remarks>
        private readonly OutputPort _sensorPort;

        #endregion Member variables

        #region Properties

        /// <summary>
        ///     Instance of the DS18B20 temperature sensor
        /// </summary>
        protected OneWire Sensor { get; private set; }

        /// <summary>
        ///     Bus mode type, default is single device on the bus.
        /// </summary>
        protected BusModeType BusMode = BusModeType.SingleDevice;

        /// <summary>
        ///     Temperature (in degrees centigrade).
        /// </summary>
        public float Temperature
        {
            get { return _temperature; }
            private set
            {
                _temperature = value;
                //
                //  Check to see if the change merits raising an event.
                //
                if ((_updateInterval > 0) && (System.Math.Abs(_lastNotifiedTemperature - value) >= TemperatureChangeNotificationThreshold))
                {
                    TemperatureChanged(this, new SensorFloatEventArgs(_lastNotifiedTemperature, value));
                    _lastNotifiedTemperature = value;
                }
            }
        }
        private float _temperature;
        private float _lastNotifiedTemperature = 0.0F;

        /// <summary>
        ///     Any changes in the temperature that are greater than the temperature
        ///     threshold will cause an event to be raised when the instance is
        ///     set to update automatically.
        /// </summary>
        public float TemperatureChangeNotificationThreshold { get; set; } = 0.001F;

        /// <summary>
        ///     Resolution in bit of the DS18B20.
        /// </summary>
        /// <remarks>
        ///     Returns (or sets) the number of bits used to hold a sensor reading.
        ///     Possible values are 9, 10, 11 or 12.
        /// </remarks>
        public int Resolution { get; set; }

        /// <summary>
        ///     Maximum conversion time (in milliseconds) for the DS18B20 based upon the resolution.
        /// </summary>
        public ushort MaximumConversionPeriod
        {
            get
            {
                ushort period = 750;      //  Default for 12-bit (default configuration).

                switch (Resolution)
                {
                    case 9:
                        period = 94;
                        break;
                    case 10:
                        period = 188;
                        break;
                    case 11:
                        period = 375;
                        break;
                }

                return (period);
            }
        }

        #endregion Properties

        #region Events and delegates

        /// <summary>
        ///     Event raised when the temperature change is greater than the 
        ///     TemperatureChangeNotificationThreshold value.
        /// </summary>
        public event SensorFloatEventHandler TemperatureChanged = delegate { };

        #endregion Events and delegates

        #region Constructor(s)

        /// <summary>
        ///     Default constructor is private to prevent it from being called.
        /// </summary>
        private DS18B20()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="oneWirePin"></param>
        /// <param name="busMode"></param>
        /// <param name="address"></param>
        /// <param name="updateInterval"></param>
        /// <param name="temperatureChangeNotificationThreshold"></param>
        public DS18B20(Cpu.Pin oneWirePin, BusModeType busMode = BusModeType.SingleDevice, byte[] address = null,
            ushort updateInterval = MinimumPollingPeriod, float temperatureChangeNotificationThreshold = 0.001F)
        {
            if (oneWirePin == Cpu.Pin.GPIO_NONE)
            {
                throw new ArgumentException("OneWire pin cannot be null.", nameof(oneWirePin));
            }
            //
            //  Create the OutputPort and OneWire objects necessary to talk to the sensor.
            //
            _sensorPort = new OutputPort(oneWirePin, false);
            Sensor = new OneWire(_sensorPort);
            if (Sensor.TouchReset() == 0)
            {
                throw new Exception("Cannot find DS18B20 sensor on the OneWire interface.");
            }
            ReadConfiguration();
            if ((updateInterval != 0) && (MaximumConversionPeriod > updateInterval))
            {
                throw new ArgumentOutOfRangeException(nameof(updateInterval), "Temperature readings can take " + MaximumConversionPeriod + "ms at this resolution.");
            }

            TemperatureChangeNotificationThreshold = temperatureChangeNotificationThreshold;
            _updateInterval = updateInterval;

            if (updateInterval > 0)
            {
                StartUpdating();
            }
            else
            {
                Update();
            }
        }

        #endregion Constructor(s)

        #region Methods

        /// <summary>
        ///     Start the update process.
        /// </summary>
        private void StartUpdating()
        {
            Thread t = new Thread(() => {
                while (true)
                {
                    Update();
                    Thread.Sleep(_updateInterval);
                }
            });
            t.Start();
        }

        /// <summary>
        ///     Update the Temperature property.
        /// </summary>
        public void Update()
        {
            Sensor.TouchReset();
            if (BusMode == BusModeType.SingleDevice)
            {
                Sensor.WriteByte(Commands.SkipROM);
            }
            else
            {
                //
                //  Need to send the device address here.
                //
            }
            Sensor.WriteByte(Commands.StartConversion);
            while (Sensor.ReadByte() == 0) ;
            Sensor.TouchReset();
            Sensor.WriteByte(Commands.SkipROM);
            Sensor.WriteByte(Commands.ReadScratchPad);
            //
            //  The conversions in the next two lines are required as the ReadByte
            //  method returns an int !
            //
            UInt16 temperature = (byte)Sensor.ReadByte();
            temperature |= (UInt16)(Sensor.ReadByte() << 8);
            Temperature = ((float)temperature) / 16;
        }

        /// <summary>
        ///     Read the configuration from the temperature sensor.
        /// </summary>
        /// <remarks>
        ///     This method will also update the Resolution property.
        /// </remarks>
        public void ReadConfiguration()
        {
            Resolution = 12;
        }

        #endregion Methods
    }
}
