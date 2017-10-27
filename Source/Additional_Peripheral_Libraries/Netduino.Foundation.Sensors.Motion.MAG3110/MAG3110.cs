using Microsoft.SPOT.Hardware;
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

        #region Structures

        /// <summary>
        /// Sensor readings to be passed back when an interrupt is generated.
        /// </summary>
        public struct SensorReading
        {
            public short X;
            public short Y;
            public short Z;
        }

        #endregion Structures

        #region Delegates and events

        /// <summary>
        /// Delegate for the OnDataReceived event.
        /// </summary>
        /// <param name="sensorReading">Sensor readings from the MAG3110.</param>
        public delegate void ReadingComplete(SensorReading sensorReading);

        /// <summary>
        /// Generated when the sensor indicates that data is ready for processing.
        /// </summary>
        public event ReadingComplete OnReadingComplete = null;

        #endregion Delegates and events

        #region Member variables / fields

        /// <summary>
        /// MAG3110 object.
        /// </summary>
        private I2CBus _mag3110 = null;

        /// <summary>
        /// Interrupt port used to detect then end of a conversion.
        /// </summary>
        private InterruptPort _interruptPort = null;

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

        /// <summary>
        /// Indicate if there is any data ready for reading (x, y or z).
        /// </summary>
        /// <remarks>
        /// See section 5.1.1 of the datasheet.
        /// </remarks>
        public bool DataReady
        {
            get
            {
               return((_mag3110.ReadRegister((byte) Registers.DRStatus) & 0x08) > 0);
            }
        }

        /// <summary>
        /// Enable or disable interrupts.
        /// </summary>
        /// <remarks>
        /// Interrupts can be triggered when a conversion completes (see section 4.2.5 
        /// of the datasheet).  The interrupts are tied to the ZYXDR bit in the DR Status
        /// register.
        /// </remarks>
        private bool _interruptsEnabled = false;
        public bool InterruptsEnabled
        {
            get
            {
                return (_interruptsEnabled);
            }
            set
            {
                Standby = true;
                byte cr2 = _mag3110.ReadRegister((byte) Registers.Control2);
                if (value)
                {
                    cr2 |= 0x80;
                }
                else
                {
                    cr2 &= 0x7f;
                }
                _mag3110.WriteRegister((byte) Registers.Control2, cr2);
                _interruptsEnabled = value;
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
        /// <param name="interruptPin">Interrupt pin used to detect end of conversions.</param>
        public MAG3110(byte address = 0x0e, ushort speed = 400, Cpu.Pin interruptPin = Cpu.Pin.GPIO_NONE)
        {
            _mag3110 = new I2CBus(address, speed);
            byte deviceID = _mag3110.ReadRegister((byte) Registers.WhoAmI);
            if (deviceID != 0xc4)
            {
                throw new Exception("Unknown device ID, " + deviceID.ToString() + " retruend, 0xc4 expected");
            }
            if (interruptPin != Cpu.Pin.GPIO_NONE)
            {
                _interruptPort = new InterruptPort(interruptPin, false, Microsoft.SPOT.Hardware.Port.ResistorMode.Disabled, 
                                                   Microsoft.SPOT.Hardware.Port.InterruptMode.InterruptEdgeHigh);
                _interruptPort.OnInterrupt += _interruptPort_OnInterrupt;
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
        /// Force the sensor to make a reading and update the relevanyt properties.
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

        #region Interrupt handlers

        /// <summary>
        /// Interrupt from the MAG3110 conversion complete interrupt.
        /// </summary>
        void _interruptPort_OnInterrupt(uint data1, uint data2, DateTime time)
        {
            if (OnReadingComplete != null)
            {
                Read();
                SensorReading readings = new SensorReading();
                readings.X = X;
                readings.Y = Y;
                readings.Z = Z;
                OnReadingComplete(readings);
            }
        }

        #endregion Interrupt handlers
    }
}
