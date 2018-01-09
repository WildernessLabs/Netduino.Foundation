---
layout: API
title: I2CBus
subtitle: I2C abstraction layer implementing the ICommunicationsBus interface.
---

# Info

The I2CBus provides an implementation to communicate with I2C devices, compatible with the [ICommunicationsBus](/API/Devices/Netduino/ICommunicationsBus) interface.

# API

## Constructors

#### `public I2CBus(byte address, ushort speed, ushort transactionTimeout = 100)`

Instantiates a new instance of an I2C device at the specified `address` and `speed`.

## Methods

#### `public void WriteByte(byte value)`

Writes a single byte to the device.

#### `public void WriteBytes(byte[] values)`

Writes a number of bytes to the device.

#### `public void WriteUShort(byte address, ushort value, ByteOrder order)`

Writes an unsigned short to the device.

#### `public void WriteUShorts(byte address, ushort[] values, ByteOrder order)`

Writes a number of unsigned shorts to the device.

#### `public byte[] WriteRead(byte[] write, ushort length)`

Writes data to the device and also reads some data from the device.

#### `public void WriteRegister(byte address, byte value)`

Writes data into a single register.

#### `public void WriteRegisters(byte address, byte[] data)`

Write data to one or more registers. This method assumes that the register address is written first followed by the data to be written into the first register followed by the data for subsequent registers.

#### `public byte[] ReadBytes(ushort numberOfBytes)`

Read the specified number of bytes from the I2C device.

#### `public byte ReadRegister(byte address)`

Read a register from the device.

#### `public byte[] ReadRegisters(byte address, ushort length)`

Read one or more registers from the device.

#### `public ushort ReadUShort(byte address, ByteOrder order = ByteOrder.LittleEndian)`

Read an unsigned short from a pair of registers.

#### `public ushort[] ReadUShorts(byte address, ushort number, ByteOrder order = ByteOrder.LittleEndian)`

Read the specified number of unsigned shorts starting at the register address specified.



