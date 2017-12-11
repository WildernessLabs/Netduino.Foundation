using Microsoft.SPOT.Hardware;

namespace Netduino.Foundation.Sensors.Motion
{
    /// <summary>
    ///     Provide an interface for the ADXL335 triple axis accelerometer.
    /// </summary>
    public class ADXL335
    {
        #region Structures

        /// <summary>
        ///     Structure to hold the X, Y & Z readings.
        /// </summary>
        /// <remarks>
        ///     This can be used to hold either the raw sensor data or the G forces
        ///     depending upon which method is called.
        /// </remarks>
        public struct Readings
        {
            public double X;
            public double Y;
            public double Z;
        }

        #endregion Structures

        #region Member variables / fields

        /// <summary>
        ///     Analog input channel connected to the x axis.
        /// </summary>
        private readonly AnalogInput _x;

        /// <summary>
        ///     Analog input channel connected to the x axis.
        /// </summary>
        private readonly AnalogInput _y;

        /// <summary>
        ///     Analog input channel connected to the x axis.
        /// </summary>
        private readonly AnalogInput _z;

        /// <summary>
        ///     Voltage that represents 0g.  This is the supply voltage / 2.
        /// </summary>
        private double _zeroGVoltage;

        #endregion Member variables / fields

        #region Properties

        /// <summary>
        ///     Volts per G for the X axis.
        /// </summary>
        public double XVoltsPerG { get; set; }

        /// <summary>
        ///     Volts per G for the X axis.
        /// </summary>
        public double YVoltsPerG { get; set; }

        /// <summary>
        ///     Volts per G for the X axis.
        /// </summary>
        public double ZVoltsPerG { get; set; }

        /// <summary>
        ///     Power supply voltage applied to the sensor.  This will be set (in the constructor)
        ///     to 3.3V by default.
        /// </summary>
        private double _supplyVoltage;

        public double SupplyVoltage
        {
            get { return _supplyVoltage; }
            set
            {
                _supplyVoltage = value;
                _zeroGVoltage = value / 2;
            }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        ///     Make the default constructor private so that the developer cannot access it.
        /// </summary>
        private ADXL335()
        {
        }

        /// <summary>
        ///     Create anew ADXL335 sensor object.
        /// </summary>
        /// <param name="x">Analog pin connected to the X axis output from the ADXL335 sensor.</param>
        /// <param name="y">Analog pin connected to the Y axis output from the ADXL335 sensor.</param>
        /// <param name="z">Analog pin connected to the Z axis output from the ADXL335 sensor.</param>
        public ADXL335(Cpu.AnalogChannel x, Cpu.AnalogChannel y, Cpu.AnalogChannel z)
        {
            _x = new AnalogInput(x);
            _y = new AnalogInput(y);
            _z = new AnalogInput(z);
            //
            //  Now set the default calibration data.
            //
            XVoltsPerG = 0.325;
            YVoltsPerG = 0.325;
            ZVoltsPerG = 0.550;
            SupplyVoltage = 3.3;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        ///     Read the sensor output and convert the sensor readings into acceleration values.
        /// </summary>
        /// <returns><see cref="Readings" /> structure containing the acceleration in g.</returns>
        public Readings GetAcceleration()
        {
            var data = new Readings();

            data.X = ((_x.Read() * SupplyVoltage) - _zeroGVoltage) / XVoltsPerG;
            data.Y = ((_y.Read() * SupplyVoltage) - _zeroGVoltage) / YVoltsPerG;
            data.Z = ((_z.Read() * SupplyVoltage) - _zeroGVoltage) / ZVoltsPerG;
            return data;
        }

        /// <summary>
        ///     Get the raw analog input values from the sensor.
        /// </summary>
        /// <returns><see cref="Readings" /> structure containing the raw sensor data from the analog pins.</returns>
        public Readings GetRawSensorData()
        {
            return new Readings { X = _x.Read(), Y = _y.Read(), Z = _z.Read() };
        }

        #endregion Methods
    }
}