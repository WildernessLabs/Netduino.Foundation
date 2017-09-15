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

namespace Netduino.Foundation.Sensors.Temperature.Analog
{
    /// <summary>
    /// Provide the ability to read the temperature from the following sensors:
    ///     - TMP35 / 36 / 37
    ///     - LM35 / 45
    /// </summary>
    public class AnalogTemperatureSensor : IDisposable
    {
        #region Private member variables (fields)

        /// <summary>
        /// Millivolts per degree centigrade for the sensor attached to the analog port.
        /// </summary>
        private int _millivoltsPerDegreeCentigrade;

        /// <summary>
        /// Minimum sensor reading in degrees centigrade.
        /// </summary>
        private int _minimumReading;

        /// <summary>
        /// Number of millivolts at the specified minimum reading.
        /// </summary>
        private int _millvoltsAtMinimumReading;

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
        public float Temperature
        {
            get
            {
                //
                //  Get the sensor reading in millivolts.
                //
                float milliVoltReading = (float) (AnalogPort.ReadRaw() * (3.3 / 1023)) * 1000;
                //
                //  Now calculate the actual temperature.
                //
                float reading = milliVoltReading - _millvoltsAtMinimumReading;
                reading /= _millivoltsPerDegreeCentigrade;
                reading += _minimumReading;
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
        /// <param name="minimumReading">Minimum sensor reading in degrees centigrade (Optional)</param>
        /// <param name="millivoltsAtMinimumReading">Number of millivolts representing the minimum reading (Optional)</param>
        /// <param name="millivoltsPerDegreeCentigrade">Number of millivolts pre degree centigrade (Optional)</param>
        public AnalogTemperatureSensor(Cpu.AnalogChannel analogPin, SensorType sensor, int minimumReading = 10, 
                                       int millivoltsAtMinimumReading = 100, int millivoltsPerDegreeCentigrade = 10)
        {
            AnalogPort = new AnalogInput(analogPin);
            switch (sensor)
            {
                case SensorType.TMP35:
                case SensorType.LM35:
                case SensorType.LM45:
                    _minimumReading = 10;
                    _millvoltsAtMinimumReading = 100;
                    _millivoltsPerDegreeCentigrade = 10;
                    break;
                case SensorType.LM50:
                case SensorType.TMP36:
                    _minimumReading = -40;
                    _millvoltsAtMinimumReading = 100;
                    _millivoltsPerDegreeCentigrade = 10;
                    break;
                case SensorType.TMP37:
                    _minimumReading = 5;
                    _millvoltsAtMinimumReading = 100;
                    _millivoltsPerDegreeCentigrade = 20;
                    break;
                case SensorType.Custom:
                    _minimumReading = minimumReading;
                    _millvoltsAtMinimumReading = millivoltsAtMinimumReading;
                    _millivoltsPerDegreeCentigrade = millivoltsPerDegreeCentigrade;
                    break;
                default:
                    throw new ArgumentException("Unknown sensor type", nameof(sensor));
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
