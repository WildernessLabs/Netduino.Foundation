using System;
using System.Threading;
using Microsoft.SPOT.Hardware;

namespace Netduino.Foundation.Sensors.Temperature.Analog
{
    /// <summary>
    ///     Provide the ability to read the temperature from the following sensors:
    ///     - TMP35 / 36 / 37
    ///     - LM35 / 45
    /// </summary>
    /// <remarks>
    ///     <i>AnalogTemperatureSensor</i> provides a method of reading the temperature from
    ///     linear analog temperature sensors.  There are a number of these sensors available
    ///     including the commonly available TMP and LM series.
    ///     Sensors of this type obey the following equation:
    ///     y = mx + c
    ///     where y is the reading in millivolts, m is the gradient (number of millivolts per
    ///     degree centigrade and C is the point where the line would intercept the y axis.
    ///     The <i>SensorType</i> enum defines the list of sensors with default settings in the
    ///     library.  Unsupported sensors that use the same linear algorithm can be constructed
    ///     by setting the sensor type to <i>SensorType.Custom</i> and providing the settings for
    ///     the linear calculations.
    ///     The default sensors have the following settings:
    ///     Sensor              Millvolts at 25C    Millivolts per degree C
    ///     TMP35, LM35, LM45       250                     10
    ///     TMP36, LM50             750                     10
    ///     TMP37                   500                     20
    /// </remarks>
    public class AnalogTemperatureSensor : IDisposable
    {
        #region Constants

        /// <summary>
        ///     Minimum value that should be used for the polling frequency.
        /// </summary>
        public const ushort MINIMUM_POLLING_PERIOD = 100;

        #endregion Constants

        #region Member variables /fields

        /// <summary>
        ///     Millivolts per degree centigrade for the sensor attached to the analog port.
        /// </summary>
        /// <remarks>
        ///     This will be the gradient of the
        /// </remarks>
        private readonly int _millivoltsPerDegreeCentigrade;

        /// <summary>
        ///     Point where the line y = mx +c would intercept the y-axis.
        /// </summary>
        private readonly float _yIntercept;

        /// <summary>
        ///     Update interval in milliseconds
        /// </summary>
        private readonly ushort _updateInterval = 100;

        #endregion Member variables / fields

        #region Properties

        /// <summary>
        ///     Type of temperature sensor.
        /// </summary>
        public enum SensorType
        {
            Custom,
            TMP35,
            TMP36,
            TMP37,
            LM35,
            LM45,
            LM50
        }

        /// <summary>
        ///     Analog port that the temperature sensor is attached to.
        /// </summary>
        /// <value>Analog port connected to the temperature sensor.</value>
        private AnalogInput AnalogPort { get; set; }

        /// <summary>
        ///     Temperature in degrees centigrade.
        /// </summary>
        /// <remarks>
        ///     The temperature is given by the following calculation:
        ///     temperature = (reading in millivolts - yIntercept) / millivolts per degree centigrade
        /// </remarks>
        public float Temperature
        {
            get { return _temperature; }
            private set
            {
                _temperature = value;
                //
                //  Check to see if the change merits raising an event.
                //
                if ((_updateInterval > 0) && (Math.Abs(_lastNotifiedTemperature - value) >= TemperatureChangeNotificationThreshold))
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

        #endregion Properties

        #region Events and delegates

        /// <summary>
        ///     Event raised when the temperature change is greater than the 
        ///     TemperatureChangeNotificationThreshold value.
        /// </summary>
        public event SensorFloatEventHandler TemperatureChanged = delegate { };

        /// <summary>
        ///     Event raised when the humidity change is greater than the
        ///     HumidityChangeNotificationThreshold value.
        /// </summary>
        public event SensorFloatEventHandler HumidityChanged = delegate { };

        /// <summary>
        ///     Event raised when the change in pressure is greater than the
        ///     PresshureChangeNotificationThreshold value.
        /// </summary>
        public event SensorFloatEventHandler PressureChanged = delegate { };

        #endregion Events and delegates

        #region Constructor(s)

        /// <summary>
        ///     Default constructor, private to prevent this being used.
        /// </summary>
        private AnalogTemperatureSensor()
        {
        }

        /// <summary>
        ///     New instance of the AnalogTemperatureSensor class.
        /// </summary>
        /// <param name="analogPin">Analog pin the temperature sensor is connected to.</param>
        /// <param name="sensor">Type of sensor attached to the analog port.</param>
        /// <param name="sampleReading">Sample sensor reading in degrees centigrade (Optional)</param>
        /// <param name="millivoltsAtSampleReading">Number of millivolts representing the sample reading (Optional)</param>
        /// <param name="millivoltsPerDegreeCentigrade">Number of millivolts pre degree centigrade (Optional)</param>
        /// <param name="updateInterval">Number of milliseconds between samples (0 indicates polling to be used)</param>
        /// <param name="humidityChangeNotificationThreshold">Changes in humidity greater than this value will trigger an event when updatePeriod > 0.</param>
        /// <param name="temperatureChangeNotificationThreshold">Changes in temperature greater than this value will trigger an event when updatePeriod > 0.</param>
        public AnalogTemperatureSensor(Cpu.AnalogChannel analogPin, SensorType sensor, int sampleReading = 25,
            int millivoltsAtSampleReading = 250, int millivoltsPerDegreeCentigrade = 10, 
            ushort updateInterval = MINIMUM_POLLING_PERIOD, float temperatureChangeNotificationThreshold = 0.001F)
        {
            if (temperatureChangeNotificationThreshold < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(temperatureChangeNotificationThreshold), "Temperature threshold should be >= 0");
            }
            if ((updateInterval != 0) && (updateInterval < MINIMUM_POLLING_PERIOD))
            {
                throw new ArgumentOutOfRangeException(nameof(updateInterval), "Update period should be 0 or >= than " + MINIMUM_POLLING_PERIOD);
            }

            TemperatureChangeNotificationThreshold = temperatureChangeNotificationThreshold;
            _updateInterval = updateInterval;

            AnalogPort = new AnalogInput(analogPin);
            switch (sensor)
            {
                case SensorType.TMP35:
                case SensorType.LM35:
                case SensorType.LM45:
                    _yIntercept = 0;
                    _millivoltsPerDegreeCentigrade = 10;
                    break;
                case SensorType.LM50:
                case SensorType.TMP36:
                    _yIntercept = 500;
                    _millivoltsPerDegreeCentigrade = 10;
                    break;
                case SensorType.TMP37:
                    _yIntercept = 0;
                    _millivoltsPerDegreeCentigrade = 20;
                    break;
                case SensorType.Custom:
                    _yIntercept = millivoltsAtSampleReading - (sampleReading * millivoltsAtSampleReading);
                    _millivoltsPerDegreeCentigrade = millivoltsPerDegreeCentigrade;
                    break;
                default:
                    throw new ArgumentException("Unknown sensor type", nameof(sensor));
#pragma warning disable 0162
                    break;
#pragma warning restore 0162
            }
            if (updateInterval > 0)
            {
                StartUpdating();
            }
            else
            {
                Update();
            }
        }

        #endregion Constructors

        #region IDisposable

        /// <summary>
        ///     Implement IDisposable interface.
        /// </summary>
        public void Dispose()
        {
            AnalogPort.Dispose();
            AnalogPort = null;
        }

        #endregion IDisposable

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
        ///     Get the current temperature and update the Temperature property.
        /// </summary>
        public void Update()
        {
            var reading = (float)(AnalogPort.Read() * 3300);
            reading -= _yIntercept;
            reading /= _millivoltsPerDegreeCentigrade;
            Temperature = reading;
        }


    #endregion Methods
}
}