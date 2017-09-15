//
//  This software is distributed under the terms of the Apache licence agreement.
//
//  Apache License Version 2.0, January 2004
//
//  The full text of the Apache 2.0 licence agreement can be found here:
//  <http://www.apache.org/licenses/>
//
using Microsoft.SPOT.Hardware;

namespace Netduino.Foundation.Sensors.Temperature.Analog
{
    /// <summary>
    /// Provide the ability to read the temperature from the following sensors:
    ///     - TMP35 / 36 / 37
    ///     - LM35 / 45
    /// </summary>
    public class AnalogTemperatureSensor
    {
        /// <summary>
        /// Millivots per degree centigrade for the sensor attached to the analog port.
        /// </summary>
        private int _millivoltsPerDegreeCentigrade;

        /// <summary>
        /// Minimum sensor reading in degrees centegrade.
        /// </summary>
        private int _minimumReading;

        /// <summary>
        /// Number of millivots at the specified minimum reading.
        /// </summary>
        private int _millvoltsAtMinimumReading;

        /// <summary>
        /// Type of temperarure sensor.
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

        /// <summary>
        /// Default constructor, private to prevent this being used.
        /// </summary>
        private AnalogTemperatureSensor()
        {
        }

        /// <summary>
        /// New instance of the AnalogTemperatureSensor class.
        /// </summary>
        /// <param name="port">Analog port the temperature sensor is connected to.</param>
        /// <param name="sensor">Type of sensor attached to the analog port.</param>
        /// <param name="minimumReading">Minimum sensor reading in degrees centigrade (Optional)</param>
        /// <param name="millivoltsAtMinimumReading">Number of milliviolts representing the minimum reading (Optional)</param>
        /// <param name="milliVoltsPerDegreeCentigrade">Number of millivolts pre degree centigrade (Optional)</param>
        public AnalogTemperatureSensor(AnalogInput port, SensorType sensor, int minimumReading = 10, 
                                       int millivoltsAtMinimumReading = 100, int milliVoltsPerDegreeCentigrade = 10)
        {
            AnalogPort = port;
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
                    _millivoltsPerDegreeCentigrade = milliVoltsPerDegreeCentigrade;
                    break;
            }
        }
    }
}
