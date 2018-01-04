using System;
using System.Threading;
using Netduino.Foundation.Devices;
using Microsoft.SPOT;

namespace Netduino.Foundation.Sensors.Atmospheric
{
    /// <summary>
    ///     Provide a mechanism for reading the Temperature and Humidity from
    ///     a HIH6130 temperature and Humidity sensor.
    /// </summary>
    public class HIH6130 : ITemperatureSensor, IHumiditySensor
    {
        #region Member variables / fields

        public event EventHandler TemperatureChanged = delegate {};
        public event EventHandler HumidityChanged = delegate { };

        /// <summary>
        ///     MAG3110 object.
        /// </summary>
        private readonly ICommunicationBus _hih6130;
        /// <summary>
        /// Update interval in miliseconds
        /// </summary>
        protected int _updateInterval = 100;

        #endregion Member variables / fields

        #region Properties

        /// <summary>
        ///     Humidity reading from the last call to Read.
        /// </summary>
        public float Humidity {
            get { return _humidity; }
            protected set
            {
                _humidity = value;

                // if it's changed enough to trigger an event
                if (value != _lastNotifiedHumidity && System.Math.Abs(_lastNotifiedHumidity - value) >= HumidityChangeNotificationThreshold)
                {
                    this.HumidityChanged(this, new EventArgs());
                    _lastNotifiedHumidity = value;
                }
            }
        }
        protected float _humidity;
        protected float _lastNotifiedHumidity = 0.0F;

        /// <summary>
        ///     Temperature reading from last call to Read.
        /// </summary>
        public float Temperature {
            get { return _temperature; }
            protected set {
                _temperature = value;

                // if it's changed enough to trigger an event
                if (value != _lastNotifiedTemperature && System.Math.Abs(_lastNotifiedTemperature - value) >= TemperatureChangeNotificationThreshold)
                {
                    this.TemperatureChanged(this, new EventArgs());
                    _lastNotifiedTemperature = value;
                }
            }
        }
        protected float _temperature;
        protected float _lastNotifiedTemperature = 0.0F;

        public float TemperatureChangeNotificationThreshold { get; set; } = 0.001F;

        public float HumidityChangeNotificationThreshold { get; set; } = 0.001F;

        #endregion

        #region Constructors

        /// <summary>
        ///     Default constructor is private to prevent the developer from calling it.
        /// </summary>
        private HIH6130()
        {
        }

        /// <summary>
        ///     Create a new HIH6130 object using the default parameters for the component.
        /// </summary>
        /// <param name="address">Address of the HIH6130 (default = 0x27).</param>
        /// <param name="speed">Speed of the I2C bus (default = 100 KHz).</param>
        public HIH6130(byte address = 0x27, ushort speed = 100, int updateInterval = 100
            , float humidityChangeNotificationThreshold = 0.001F, float temperatureChangeNotificationThreshold = 0.001F)
        {
            this._updateInterval = updateInterval;
            this.HumidityChangeNotificationThreshold = humidityChangeNotificationThreshold;
            this.TemperatureChangeNotificationThreshold = temperatureChangeNotificationThreshold;

            _hih6130 = new I2CBus(address, speed);
            //Read();
            StartUpdating();
        }

        #endregion Constructors

        #region Methods

        private void StartUpdating()
        {
            Thread t = new Thread(() => {
                while (true)
                {
                    Update();
                    Thread.Sleep(this._updateInterval);
                }
            }); t.Start();
        }

        /// <summary>
        ///     Force the sensor to make a reading and update the relevant properties.
        /// </summary>
        public void Update()
        {
            _hih6130.WriteByte(0);
            //
            //  Sensor takes 35ms to make a valid reading.
            //
            Thread.Sleep(50);
            var data = _hih6130.ReadBytes(4);
            //
            //  Data format:
            //
            //  Byte 0: S1  S0  H13 H12 H11 H10 H9 H8
            //  Byte 1: H7  H6  H5  H4  H3  H2  H1 H0
            //  Byte 2: T13 T12 T11 T10 T9  T8  T7 T6
            //  Byte 4: T5  T4  T3  T2  T1  T0  XX XX
            //
            if ((data[0] & 0xc0) != 0)
            {
                throw new Exception("Status indicates readings are invalid.");
            }
            var reading = ((data[0] << 8) | data[1]) & 0x3fff;
            Humidity = ((float) reading / 16383) * 100;
            reading = ((data[2] << 8) | data[3]) >> 2;
            Temperature = (((float) reading / 16383) * 165) - 40;
        }

        #endregion Methods
    }
}