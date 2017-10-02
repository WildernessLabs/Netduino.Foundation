//
//  This software is distributed under the terms of the Apache licence agreement.
//
//  Apache License Version 2.0, January 2004
//
//  The full text of the Apache 2.0 licence agreement can be found here:
//  <http://www.apache.org/licenses/>
//
using System;
using Microsoft.SPOT.Hardware;
using Microsoft.SPOT;
using SecretLabs.NETMF.Hardware;

namespace Netduino.Foundation.Sensors.Temperature.Analog
{
    /// <summary>
    /// Provide the ability to read the temperature from the following sensors:
    ///     - TMP35 / 36 / 37
    ///     - LM35 / 45
    /// </summary>
    /// <remarks>
    /// <i>AnalogTemperatureSensor</i> provides a method of reading the temperature from
    /// linear analog temperature sensors.  There are a number of these sensors available
    /// including the commonly available TMP and LM series.
    /// 
    /// Sensors of this type obey the following equation:
    /// 
    ///     y = mx + c
    ///     
    /// where y is the reading in millivolts, m is the gradient (number of millivolts per
    /// degree centigrade and C is the point where the line would intercept the y axis.
    ///
    /// The <i>SensorType</i> enum defines the list of sensors with default settings in the 
    /// library.  Unsupported sensors that use the same linear algorithm can be constructed
    /// by setting the sensor type to <i>SensorType.Custom</i> and providing the settings for 
    /// the linear calculations.
    ///
    /// The default sensors have the following settings:
    ///
    /// Sensor              Millvolts at 25C    Millivolts per degree C
    /// TMP35, LM35, LM45       250                     10
    /// TMP36, LM50             750                     10
    /// TMP37                   500                     20
    /// </remarks>
    public class AnalogTemperatureSensor : IDisposable
    {
        #region Private member variables (fields)

        /// <summary>
        /// Millivolts per degree centigrade for the sensor attached to the analog port.
        /// </summary>
        /// <remarks>
        /// This will be the gradient of the 
        /// </remarks>
        private int _millivoltsPerDegreeCentigrade;

        /// <summary>
        /// Point where the line y = mx +c would intercept the y-axis.
        /// </summary>
        private float _yIntercept;

        #endregion Private member variables (fields)

        #region Properties

        /// <summary>
        /// Type of temperature sensor.
        /// </summary>
        public enum SensorType { Custom, TMP35, TMP36, TMP37, LM35, LM45, LM50 };

        /// <summary>
        /// Analog port that the temperature sensor is attached to.
        /// </summary>
        /// <value>Analog port connected to the temperature sensor.</value>
        private AnalogInput AnalogPort { get; set; }

        /// <summary>
        /// Temperature in degrees centigrade.
        /// </summary>
        /// <remarks>
        /// The temperature is given by the following calculation:
        ///
        /// temperature = (reading in millivolts - yIntercept) / millivolts per degree centigrade
        /// </remarks>
        public float Temperature
        {
            get
            {
                float reading = (float) (AnalogPort.Read() * 3300);
                reading -= _yIntercept;
                reading /= _millivoltsPerDegreeCentigrade;
                return (reading);
            }
        }

        #endregion Properties

        #region Constructor(s)

        /// <summary>
        /// Default constructor, private to prevent this being used.
        /// </summary>
        private AnalogTemperatureSensor()
        {
        }

        /// <summary>
        /// New instance of the AnalogTemperatureSensor class.
        /// </summary>
        /// <param name="analogPin">Analog pin the temperature sensor is connected to.</param>
        /// <param name="sensor">Type of sensor attached to the analog port.</param>
        /// <param name="sampleReading">Sample sensor reading in degrees centigrade (Optional)</param>
        /// <param name="millivoltsAtSampleReading">Number of millivolts representing the sample reading (Optional)</param>
        /// <param name="millivoltsPerDegreeCentigrade">Number of millivolts pre degree centigrade (Optional)</param>
        public AnalogTemperatureSensor(Cpu.AnalogChannel analogPin, SensorType sensor, int sampleReading = 25, 
                                       int millivoltsAtSampleReading = 250, int millivoltsPerDegreeCentigrade = 10)
        {
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
                    throw new ArgumentException("Unknown sensor type", "sensor");
#pragma warning disable 0162
                    break;
#pragma warning restore 0162
            }
        }

        #endregion Constructors

        #region IDisposable

        /// <summary>
        /// Implement IDisposable interface.
        /// </summary>
        public void Dispose()
        {
            AnalogPort.Dispose();
            AnalogPort = null;
        }

        #endregion IDisposable
    }
}
