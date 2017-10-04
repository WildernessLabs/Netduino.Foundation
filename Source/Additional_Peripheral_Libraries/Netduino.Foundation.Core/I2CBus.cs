using System;
using Microsoft.SPOT.Hardware;

namespace Netduino.Foundation.Core
{
    public class I2CBus : ICommunicationBus
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
        /// <param name="address">Address to write the first byte to.</param>
        /// <param name="value">Value to be written (16-bits).</param>
        /// <param name="order">Indicate if the data should be written as big or little endian.</param>
        public void WriteUShort(byte address, ushort value, ByteOrder order)
		{
            byte[] data = new byte[2];
            if (order == ByteOrder.LittleEndian)
            {
                data[0] = (byte) (value & 0xff);
                data[1] = (byte) ((value >> 8) & 0xff);
            }
            else
            {
                data[0] = (byte) ((value >> 8) & 0xff);
                data[1] = (byte) (value & 0xff);
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
                    data[2 * index] = (byte) (values[index] & 0xff);
                    data[(2 * index) + 1] = (byte) ((values[index] >> 8) & 0xff);
                }
                else
                {
                    data[2 * index] = (byte) ((values[index] >> 8) & 0xff);
                    data[(2 * index) + 1] = (byte) (values[index] & 0xff);
                }
            }
            WriteRegisters(address, data);
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

        /// <summary>
        /// Read a register from the device.
        /// </summary>
        /// <param name="address">Address of the register to read.</param>
        public byte ReadRegister(byte address)
        {
            byte[] registerAddress = { address };
            byte[] result = WriteRead(registerAddress, 1);
            return (result[0]);
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
        /// Read an usingned short from a pair of registers.
        /// </summary>
        /// <param name="address">Register address of the low byte (the high byte will follow).</param>
        /// <param name="order">Order of the bytes in the register (little endian is the default).</param>
        /// <returns>Value read from the register.</returns>
        public ushort ReadUShort(byte address, ByteOrder order = ByteOrder.LittleEndian)
        {
            byte[] data = ReadRegisters(address, 2);
            ushort result = 0;
            if (order == ByteOrder.LittleEndian)
            {
                result = (ushort) ((data[1] << 8) + data[0]);
            }
            else
            {
                result = (ushort) ((data[0] << 8) + data[1]);
            }
            return (result);
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
			byte[] data = ReadRegisters(address, (ushort) ((2 * number) & 0xffff));
			ushort[] result = new ushort[number];
			for (int index = 0; index < number; index++)
			{
				if (order == ByteOrder.LittleEndian)
				{
					result[index] = (ushort) ((data[(2 * index) + 1] << 8) + data[2 * index]);
				}
				else
				{
					result[index] = (ushort) ((data[2 * index] << 8) + data[(2 * index) + 1]);
				}
			}
			return(result);
		}
    }
}