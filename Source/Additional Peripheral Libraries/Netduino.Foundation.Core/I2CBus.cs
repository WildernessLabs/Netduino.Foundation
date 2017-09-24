using System;
using Microsoft.SPOT.Hardware;

namespace Netduino.Foundation.Core
{
    public class I2CBus
    {
		/// <summary>
		/// I2C bus used to communicate with a device (sensor etc.).
		/// </summary>
		private I2CDevice _device;

		/// <summary>
		/// Default constructor for the I2CBus class.  This is private to prevent the
		/// developer from calling it.
		/// </summary>
        private I2CBus()
        {
        }

		/// <summary>
		/// Initializes a new instance of the <see cref="T:Netduino.Foundation.Core.I2CBus"/> class.
		/// </summary>
		/// <param name="address">Address of the device.</param>
		/// <param name="speed">Bus speed in kHz.</param>
		public I2CBus(byte address, ushort speed)
		{
			_device = new I2CDevice(new I2CDevice.Configuration(address, speed));
		}

		/// <summary>
		/// Write a single byte to the device.
		/// </summary>
		/// <param name="value">Value to be written (8-bits).</param>
		public void WriteByte(byte value)
		{
			byte[] data = { value };
			WriteBytes(data);
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
			I2CDevice.I2CTransaction[] transaction =
			{
				I2CDevice.CreateWriteTransaction(values)
			};
			int retryCount = 0;
			while (_device.Execute(transaction, 100) != 2)
			{
				if (retryCount > 3)
				{
					throw new Exception("WriteBytes: Retry count exceeded.");
				}
				retryCount++;
			}
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
		public byte[] WriteRead(byte[] write, ushort length)
		{
			byte[] read = new byte[length];
			I2CDevice.I2CTransaction[] transaction =
			{
				I2CDevice.CreateWriteTransaction(write),
				I2CDevice.CreateReadTransaction(read)
			};
			int bytesTransferred = 0;
			int retryCount = 0;
			while (bytesTransferred != (write.Length + read.Length))
			{
				if (retryCount > 3)
				{
					throw new Exception("WriteRead: Retry count exceeded.");
				}
				retryCount++;
				bytesTransferred = _device.Execute(transaction, 100);
			}
			return (read);
		}

		/// <summary>
		/// Read one or more registers from the device.
		/// </summary>
		/// <param name="address">Address of the first register to read.</param>
		/// <param name="length">Number of bytes to read from the device.</param>
		public byte[] ReadRegisters(byte address, ushort length)
		{
			byte[] registerAddress = { address };
			return (WriteRead(registerAddress, length));
		}

		/// <summary>
		/// Write data into a single register.
		/// </summary>
		/// <param name="address">Address of the register to write to.</param>
		/// <param name="value">Value to write into the register.</param>
		public void WriteRegister(byte address, byte value)
		{
			byte[] data = new byte[] { address, value };
			WriteBytes(data);
		}

		/// <summary>
		/// Write data to one or more registers.
		/// </summary>
		/// <remarks>
		/// This method assumes that the register address is written first followed by the data to be
		/// written into the first register followed by the data for subsequent registers.
		/// </remarks>
		/// <param name="address">Address of the first register to write to.</param>
		/// <param name="data">Data to write into the registers.</param>
		public void WriteRegisters(byte address, byte[] data)
		{
			byte[] registerAndData = new byte[data.Length + 1];
			registerAndData[0] = address;
			Array.Copy(data, 0, registerAndData, 1, data.Length);
			WriteBytes(registerAndData);
		}
    }
}
