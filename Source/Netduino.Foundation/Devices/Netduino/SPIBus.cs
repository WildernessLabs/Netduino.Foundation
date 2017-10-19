using System;
using Microsoft.SPOT.Hardware;

namespace Netduino.Foundation.Devices
{
    public class SPIBus : ICommunicationBus
    {
		#region Member variables / fields

		/// <summary>
		/// SPI bus object.
		/// </summary>
		SPI _spi = null;

		#endregion Member variables / fields

		#region Constructor(s)
 
		/// <summary>
		/// Default constructor for the SPIBus.
		/// </summary>
		/// <remarks>
		/// This is private to prevent the programmer using it.
		/// </remarks>
		private SPIBus()
		{
		}

		/// <summary>
		/// Create a new SPIBus object.
		/// </summary>
		/// <param name="configuration">SPI bus configuration.</param>
		public SPIBus(SPI.Configuration configuration)
		{
			_spi = new SPI(configuration);
		}

		#endregion Constructor(s)

		/// <summary>
		/// Write a single byte to the device.
		/// </summary>
		/// <param name="value">Value to be written (8-bits).</param>
		public void WriteByte(byte value)
		{
			WriteBytes(new byte[] { value });
		}

		/// <summary>
		/// Write a number of bytes to the device.
		/// </summary>
		/// <remarks>
		/// The number of bytes to be written will be determined by the length of the byte array.
		/// </remarks>
		/// <param name="values">Values to be written.</param>
		public void WriteBytes(byte[] values)
		{
			_spi.Write(values);
		}

		/// <summary>
		/// Write an unsigned short to the device.
		/// </summary>
        /// <param name="address">Address to write the first byte to.</param>
        /// <param name="value">Value to be written (16-bits).</param>
        /// <param name="order">Indicate if the data should be written as big or little endian.</param>
        public void WriteUShort(byte address, ushort value, ByteOrder order)
		{
			byte[] data = new byte[2];
			if (order == ByteOrder.LittleEndian)
			{
				data[0] = (byte)(value & 0xff);
				data[1] = (byte)((value >> 8) & 0xff);
			}
			else
			{
				data[0] = (byte)((value >> 8) & 0xff);
				data[1] = (byte)(value & 0xff);
			}
			WriteRegisters(address, data);
		}

		/// <summary>
		/// Write a number of unsigned shorts to the device.
		/// </summary>
		/// <remarks>
		/// The number of bytes to be written will be determined by the length of the byte array.
		/// </remarks>
        /// <param name="address">Address to write the first byte to.</param>
        /// <param name="values">Values to be written.</param>
        /// <param name="order">Indicate if the data should be written as big or little endian.</param>
        public void WriteUShorts(byte address, ushort[] values, ByteOrder order)
		{
			byte[] data = new byte[2 * values.Length];
			for (int index = 0; index < values.Length; index++)
			{
				if (order == ByteOrder.LittleEndian)
				{
					data[index * 2] = (byte) (values[index] & 0xff);
					data[(index * 2) + 1] = (byte) ((values[index] >> 8) & 0xff);
				}
				else
				{
					data[index * 2] = (byte) ((values[index] >> 8) & 0xff);
					data[(index * 2) + 1] = (byte) (values[index] & 0xff);
				}
			}
			WriteRegisters(address, data);
		}

        /// <summary>
        /// Write data a register in the device.
        /// </summary>
        /// <param name="address">Address of the register to write to.</param>
        /// <param name="value">Data to write into the register.</param>
        public void WriteRegister(byte address, byte value)
		{
			WriteRegisters(address, new byte[] { value });
        }

        /// <summary>
        /// Write data to one or more registers.
        /// </summary>
        /// <param name="address">Address of the first register to write to.</param>
        /// <param name="data">Data to write into the registers.</param>
        public void WriteRegisters(byte address, byte[] values)
        {
			byte[] data = new byte[values.Length + 1];
			data[0] = address;
			Array.Copy(values, 0, data, 1, values.Length);
			WriteBytes(data);
        }
        /// <summary>
		/// Write data to the device and also read some data from the device.
		/// </summary>
		/// <remarks>
		/// The number of bytes to be written and read will be determined by the length of the byte arrays.
		/// </remarks>
		/// <param name="write">Array of bytes to be written to the device.</param>
		/// <param name="length">Amount of data to read from the device.</param>
		public byte[] WriteRead(byte[] write, ushort length)
		{
			byte[] result = new byte[length];
			_spi.WriteRead(write, result);
			return (result);
		}

        /// <summary>
        /// Read a registers from the device.
        /// </summary>
        /// <param name="address">Address of the register to read.</param>
        public byte ReadRegister(byte address)
        {
			return(WriteRead(new byte[] { address }, 1)[0]);
        }

        /// <summary>
		/// Read one or more registers from the device.
		/// </summary>
		/// <param name="address">Address of the first register to read.</param>
		/// <param name="length">Number of bytes to read from the device.</param>
		public byte[] ReadRegisters(byte address, ushort length)
		{
			return (WriteRead(new byte[] { address }, length));
		}

        /// <summary>
        /// Read an usingned short from a pair of registers.
        /// </summary>
        /// <param name="address">Register address of the low byte (the high byte will follow).</param>
        /// <param name="order">Order of the bytes in the register (little endian is the default).</param>
        /// <returns>Value read from the register.</returns>
        public ushort ReadUShort(byte address, ByteOrder order = ByteOrder.LittleEndian)
        {
            throw new NotImplementedException();
        }

        /// <summary>
		/// Read the specified number of unsigned shorts starting at the register
		/// address specified.
		/// </summary>
		/// <param name="address">First register address to read from.</param>
		/// <param name="number">Number of unsigned shorts to read.</param>
		/// <param name="order">Order of the bytes (Little or Big endian)</params>
		/// <returns>Array of unsigned shorts.</returns>
        public ushort[] ReadUShorts(byte address, ushort number, ByteOrder order = ByteOrder.LittleEndian)
		{
            throw new NotImplementedException();
		}
    }
}