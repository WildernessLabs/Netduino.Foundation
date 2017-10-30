using Netduino.Foundation.Devices;
using System.Threading;

namespace Netduino.Foundation.Sensors.Barometric
{
    public class GroveTH02
    {
        #region Constants

        /// <summary>
        /// Start measurement bit in the configuration register.
        /// </summary>
        private const byte START_MEASUREMENT = 0x01;

        /// <summary>
        /// Measure temperature bit in the configuration register.
        /// </summary>
        private const byte MEASURE_TEMPERATURE = 0x10;

        /// <summary>
        /// Heater control bit in the configuration register.
        /// </summary>
        private const byte HEATER_ON = 0x02;

        /// <summary>
        /// Mask used to turn the heater off.
        /// </summary>
        private const byte HEATER_MASK = 0xfd;

        #endregion

        #region Enums

        /// <summary>
        /// Register addresses in the Grove TH02 sensor.
        /// </summary>
        private enum Registers { Status = 0x00, DataHigh = 0x01, DataLow = 0x02, Config = 0x04, ID = 0x11 }

        #endregion Enums

        #region Member variables / fields

        /// <summary>
        /// MAG3110 object.
        /// </summary>
        private ICommunicationBus _groveTH02 = null;

        #endregion Member variables / fields

        #region Properties

        /// <summary>
        /// Humidity reading from the last call to Read.
        /// </summary>
        public float Humidity { get; private set; }

        /// <summary>
        /// Temperature reading from last call to Read.
        /// </summary>
        public float Temperature { get; private set; }

        /// <summary>
        /// Get / set the heater status.
        /// </summary>
        public bool HeaterOn
        {
            get
            {
                return ((_groveTH02.ReadRegister((byte) Registers.Config) & HEATER_ON) > 0);
            }
            set
            {
                byte config = _groveTH02.ReadRegister((byte)Registers.Config);
                if (value)
                {
                    config |= HEATER_ON;                    
                }
                else
                {
                    config &= HEATER_MASK;
                }
                _groveTH02.WriteRegister((byte)Registers.Config, config);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor is private to prevent the developer from calling it.
        /// </summary>
        private GroveTH02()
        {
        }

        /// <summary>
        /// Create a new GroveTH02 object using the default parameters for the component.
        /// </summary>
        /// <param name="address">Address of the Grove TH02 (default = 0x4-).</param>
        /// <param name="speed">Speed of the I2C bus (default = 100 KHz).</param>
        public GroveTH02(byte address = 0x40, ushort speed = 100)
        {
            I2CBus device = new I2CBus(address, speed);
            _groveTH02 = (ICommunicationBus) device;
            Read();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Force the sensor to make a reading and update the relevant properties.
        /// </summary>
        public void Read()
        {
            int temp = 0;
            //
            //  Get the humidity first.
            //
            _groveTH02.WriteRegister((byte)Registers.Config, START_MEASUREMENT);
            //
            //  Maximum conversion time should be 40ms but loop just in case 
            /// it takes longer.
            //
            Thread.Sleep(40);
            while ((_groveTH02.ReadRegister((byte)Registers.Status) & 0x01) > 0) ;
            byte[] data = _groveTH02.ReadRegisters((byte)Registers.DataHigh, 2);
            temp = data[0] << 8;
            temp |= data[1];
            temp >>= 4;
            Humidity = (((float) temp) / 16) - 24;
            //
            //  Now get the temperature.
            //
            _groveTH02.WriteRegister((byte)Registers.Config, START_MEASUREMENT | MEASURE_TEMPERATURE);
            Thread.Sleep(40);
            while ((_groveTH02.ReadRegister((byte)Registers.Status) & 0x01) > 0) ;
            data = _groveTH02.ReadRegisters((byte)Registers.DataHigh, 2);
            temp = data[0] << 8;
            temp |= data[1];
            temp >>= 2;
            Temperature = (((float) temp) / 32) - 50;
        }

        #endregion
    }
}
