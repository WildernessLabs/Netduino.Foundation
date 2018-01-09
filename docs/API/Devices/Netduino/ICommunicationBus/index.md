---
layout: API
title: ICommunicationsBus
subtitle: Interface for communicating with attached peripherals.
---

# Info

Define a standard interface for communicating with an attached
peripheral.

# API

## Methods

#### `void WriteByte(byte value)`

Write a single byte to the device.

#### `void WriteBytes(byte[] values)`

Write a number of bytes to the device.

#### `void WriteUShort(byte address, ushort value, ByteOrder order)`

Write an unsigned short to the device.

#### `void WriteUShort(byte address, ushort value, ByteOrder order)`

Write an unsigned short to the device.

#### `void WriteUShorts(byte address, ushort[] values, ByteOrder order)`

Write a number of unsigned shorts to the device.

#### `void WriteRegister(byte address, byte value)`

Write to a data register in the device.

#### `void WriteRegisters(byte address, byte[] data)`

Write data to one or more registers.

#### `byte[] WriteRead(byte[] write, ushort length)`

Write data to the device and also read some data from the device.

#### `byte[] ReadBytes(ushort numberOfBytes)`

Read the specified number of bytes from the I2C device.

#### `byte ReadRegister(byte address)`

Read registers from the device.

#### `byte[] ReadRegisters(byte address, ushort length)`

Read one or more registers from the device.

#### `ushort ReadUShort(byte address, ByteOrder order)`

Read an unsigned short from a pair of registers.

#### `ushort[] ReadUShorts(byte address, ushort number, ByteOrder order = ByteOrder.LittleEndian)`

Read the specified number of unsigned shorts starting at the register address specified.