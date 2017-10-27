using Netduino.Foundation.Devices;
using System;

namespace Netduino.Foundation.Sensors.Motion
{
    public class MAG3110
    {
        #region Enums

        /// <summary>
        /// Register addresses in the sensor.
        /// </summary>
        enum Registers { DRStatus = 0x00, XMSB = 0x01, XLSB = 0x02, YMSB = 0x03, YLSB = 0x04, ZMSB = 0x05, ZLSB = 0x06,
                         WhoAmI = 0x07, SystemMode = 0x08, XOffsetMSB = 0x09, XOffsetLSB = 0x0a, YOffsetMSB = 0x0b,
                         YOffsetLSB = 0x0c, ZOffsetMSB = 0x0d, ZOffsetLSB = 0x0e, Temperature = 0x0f, Control1 = 0x10,
                         Control2 = 0x11 }

        #endregion Enums

        #region Member variables / fields

        /// <summary>
        /// MAG3110 object.
        /// </summary>
        private I2CBus _mag3110 = null;

        #endregion Member variables / fields

        #region Properties

        /// <summary>
        /// Reading from the X axis.
        /// </summary>
        /// <remarks>
        /// Data in this property is only current after a call to Read.
        /// </remarks>
        public short X { get; private set; }

        /// <summary>
        /// Reading from the Y axis.
        /// </summary>
        /// <remarks>
        /// Data in this property is only current after a call to Read.
        /// </remarks>
        public short Y { get; private set; }

        /// <summary>
        /// Reading from the Z axis.
        /// </summary>
        /// <remarks>
        /// Data in this property is only current after a call to Read.
        /// </remarks>
        public short Z { get; private set; }

        /// <summary>
        /// Temperature of the sensor die.
        /// </summary>
        public sbyte Temperature
        {
            get
            {
                return((sbyte) _mag3110.ReadRegister((byte) Registers.Temperature));
            }
        }

        /// <summary>
        /// Change or get the standby status of the sensor.
        /// </summary>
        public bool Standby
        {
            get
            {
                byte controlRegister = _mag3110.ReadRegister((byte) Registers.Control1);
                return ((controlRegister & 0x03) == 0);
            }
            set
            {
                byte controlRegister = _mag3110.ReadRegister((byte) Registers.Control1);
                if (value)
                {
                    controlRegister &= 0xfc;    // ~0x03
                }
                else
                {
                    controlRegister |= 0x01;
                }
                _mag3110.WriteRegister((byte) Registers.Control1, controlRegister);
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor is private to prevent the developer from calling it.
        /// </summary>
        private MAG3110()
        {
        }

        /// <summary>
        /// Create a new MAG3110 object using the default parameters for the component.
        /// </summary>
        /// <param name="address">Address of the MAG3110 (default = 0x0e).</param>
        /// <param name="speed">Speed of the I2C bus (default = 400 KHz).</param>
        public MAG3110(byte address = 0x0e, ushort speed = 400)
        {
            _mag3110 = new I2CBus(address, speed);
            byte deviceID = _mag3110.ReadRegister((byte) Registers.WhoAmI);
            if (deviceID != 0xc4)
            {
                throw new Exception("Unknown device ID, " + deviceID.ToString() + " retruend, 0xc4 expected");
            }
            Reset();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Reset the sensor.
        /// </summary>
        public void Reset()
        {
            Standby = true;
            _mag3110.WriteRegister((byte) Registers.Control1, 0x00);
            _mag3110.WriteRegister((byte) Registers.Control2, 0x80);
            _mag3110.WriteRegisters((byte) Registers.XOffsetMSB, new byte[] { 0, 0, 0, 0, 0, 0 });
        }

        /// <summary>
        /// Force the sensor to make a reading and update the relevant properties.
        /// </summary>
        public void Read()
        {
            byte controlRegister = _mag3110.ReadRegister((byte) Registers.Control1);
            controlRegister |= 0x02;
            _mag3110.WriteRegister((byte) Registers.Control1, controlRegister);
            byte[] data = _mag3110.ReadRegisters((byte) Registers.XMSB, 6);
            X = (short) ((data[0] << 8) | data[1]);
            Y = (short) ((data[2] << 8) | data[3]);
            Z = (short) ((data[4] << 8) | data[5]);
        }

        #endregion
    }
}
