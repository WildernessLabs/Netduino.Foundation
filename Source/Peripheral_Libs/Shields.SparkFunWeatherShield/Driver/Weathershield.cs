using System;
using Microsoft.SPOT;
using Netduino.Foundation.Sensors.Atmospheric;
using Netduino.Foundation.Sensors.Barometric;

namespace Netduino.Foundation.Sensors.Weather
{
    public class Weathershield
    {
        #region Member variables / fields

        /// <summary>
        ///     SI7021 humidity and temperature sensor.
        /// </summary>
        private SI7021 _humidityAndTemperature = new SI7021();

        /// <summary>
        ///     MPL3115A2 pressure sensor.
        /// </summary>
        private MPL3115A2 _pressure = new MPL3115A2();

        #endregion Member variables / fields
    }
}
