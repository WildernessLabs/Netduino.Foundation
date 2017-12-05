using System;
using MSH = Microsoft.SPOT.Hardware;

namespace Netduino.Foundation.Devices
{
    /// <summary>
    ///     Implement a software version of the SPI communication protocol.
    /// </summary>
    public class SoftwareSPIBus : ICommunicationBus
    {
        #region Member variables / fields

        /// <summary>
        ///     MOSI output port.
        /// </summary>
        private MSH.OutputPort _mosi;

        /// <summary>
        ///     MISO Input port.
        /// </summary>
        private MSH.InputPort _miso;

        /// <summary>
        ///     Clock output port.
        /// </summary>
        private MSH.OutputPort _clock;

        /// <summary>
        ///     Chip select port.
        /// </summary>
        private MSH.OutputPort _chipSelect;

        /// <summary>
        ///     Boolean representation of the clock polarity.
        /// </summary>
        private readonly bool _polarity;

        /// <summary>
        ///     Boolean representation of the clock phase.
        /// </summary>
        private readonly bool _phase;

        #endregion Member variables / fields

        #region Constructors

        /// <summary>
        ///     Default constructor (private to prevent it from being used).
        /// </summary>
        private SoftwareSPIBus()
        {
        }

        /// <summary>
        ///     Create a new SoftwareSPIBus object using the specified pins.
        /// </summary>
        /// <param name="mosi">MOSI pin.</param>
        /// <param name="miso">MISO pin</param>
        /// <param name="clock">Clock pin.</param>
        /// <param name="chipSelect">Chip select pin.</param>
        /// <param name="cpha">Clock phase (0 or 1, default is 0).</param>
        /// <param name="cpol">Clock polarity (0 or 1, default is 0).</param>
        public SoftwareSPIBus(MSH.Cpu.Pin mosi, MSH.Cpu.Pin miso, MSH.Cpu.Pin clock, MSH.Cpu.Pin chipSelect, byte cpha = 0, byte cpol = 0)
        {
            _phase = (cpha == 1);
            _polarity = (cpol == 1);
            _mosi = new MSH.OutputPort(mosi, false);
            _miso = new MSH.InputPort(miso, false, MSH.Port.ResistorMode.Disabled);
            _clock = new MSH.OutputPort(clock, _polarity);
            _chipSelect = new MSH.OutputPort(chipSelect, true);
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        ///     Write a single byte to the device.
        /// </summary>
        /// <param name="value">Value to be written (8-bits).</param>
        public void WriteByte(byte value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Write a number of bytes to the device.
        /// </summary>
        /// <remarks>
        ///     The number of bytes to be written will be determined by the length of the byte array.
        /// </remarks>
        /// <param name="values">Values to be written.</param>
        public void WriteBytes(byte[] values)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Write data a register in the device.
        /// </summary>
        /// <param name="register">Address of the register to write to.</param>
        /// <param name="value">Data to write into the register.</param>
        public void WriteRegister(byte register, byte value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Write data to one or more registers.
        /// </summary>
        /// <param name="address">Address of the first register to write to.</param>
        /// <param name="data">Data to write into the registers.</param>
        public void WriteRegisters(byte address, byte[] data)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Write an unsigned short to the device.
        /// </summary>
        /// <param name="address">Address to write the first byte to.</param>
        /// <param name="value">Value to be written (16-bits).</param>
        /// <param name="order">Indicate if the data should be written as big or little endian.</param>
        public void WriteUShort(byte address, ushort value, ByteOrder order)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Write a number of unsigned shorts to the device.
        /// </summary>
        /// <remarks>
        ///     The number of bytes to be written will be determined by the length of the byte array.
        /// </remarks>
        /// <param name="address">Address to write the first byte to.</param>
        /// <param name="values">Values to be written.</param>
        /// <param name="order">Indicate if the data should be written as big or little endian.</param>
        public void WriteUShorts(byte address, ushort[] values, ByteOrder order)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Write data to the device and also read some data from the device.
        /// </summary>
        /// <remarks>
        ///     The number of bytes to be written and read will be determined by the length of the byte arrays.
        /// </remarks>
        /// <param name="write">Array of bytes to be written to the device.</param>
        /// <param name="length">Amount of data to read from the device.</param>
        public byte[] WriteRead(byte[] write, ushort length)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Read the specified number of bytes from the I2C device.
        /// </summary>
        /// <returns>The bytes.</returns>
        /// <param name="numberOfBytes">Number of bytes.</param>
        public byte[] ReadBytes(ushort numberOfBytes)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Read a registers from the device.
        /// </summary>
        /// <param name="address">Address of the register to read.</param>
        public byte ReadRegister(byte address)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Read one or more registers from the device.
        /// </summary>
        /// <param name="address">Address of the first register to read.</param>
        /// <param name="length">Number of bytes to read from the device.</param>
        public byte[] ReadRegisters(byte address, ushort length)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Read an unsigned short from a pair of registers.
        /// </summary>
        /// <param name="address">Register address of the low byte (the high byte will follow).</param>
        /// <param name="order">Order of the bytes in the register (little endian is the default).</param>
        /// <returns>Value read from the register.</returns>
        public ushort ReadUShort(byte address, ByteOrder order = ByteOrder.LittleEndian)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        ///     Read the specified number of unsigned shorts starting at the register
        ///     address specified.
        /// </summary>
        /// <param name="address">First register address to read from.</param>
        /// <param name="number">Number of unsigned shorts to read.</param>
        /// <param name="order">Order of the bytes (Little or Big endian)</param>
        /// <returns>Array of unsigned shorts.</returns>
        public ushort[] ReadUShorts(byte address, ushort number, ByteOrder order = ByteOrder.LittleEndian)
        {
            throw new NotImplementedException();
        }

        #endregion Methods
    }
}