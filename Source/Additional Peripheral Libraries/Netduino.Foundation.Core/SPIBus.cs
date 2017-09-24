using System;
namespace Netduino.Foundation.Core
{
    public class SPIBus
    {
		public SPIBus()
		{
		}

		/// <summary>
		/// Write a single byte to the device.
		/// </summary>
		/// <param name="value">Value to be written (8-bits).</param>
		public void WriteByte(byte value)
		{
			throw new NotImplementedException();
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
			throw new NotImplementedException();
		}

		/// <summary>
		/// Write an unsigned short to the device.
		/// </summary>
		/// <param name="value">Value to be written (16-bits).</param>
		public void WriteUShort(ushort value)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Write a number of unsigned shorts to the device.
		/// </summary>
		/// <remarks>
		/// The number of bytes to be written will be determined by the length of the byte array.
		/// </remarks>
		/// <param name="values">Values to be written.</param>
		public void WriteUShorts(ushort[] values)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Write data to the device and also read some data from the device.
		/// </summary>
		/// <remarks>
		/// The number of bytes to be written and read will be determined by the length of the byte arrays.
		/// </remarks>
		/// <param name="write">Array of bytes to be written to the device.</param>
		/// <param name="length">Amount of data to read from the device.</param>
		public void WriteRead(byte[] write, ushort length)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Read one or more registers from the device.
		/// </summary>
		/// <param name="address">Address of the first register to read.</param>
		/// <param name="length">Number of bytes to read from the device.</param>
		public byte[] ReadRegisters(byte address, ushort length)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Write data to one or more registers.
		/// </summary>
		/// <param name="address">Address of the first register to write to.</param>
		/// <param name="data">Data to write into the registers.</param>
		public void WriteRegisters(byte address, byte[] data)
		{
			throw new NotImplementedException();
		}
    }
}
