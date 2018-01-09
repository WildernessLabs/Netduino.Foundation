---
layout: API
title: SoftwareSPIBus
subtitle: Software implementation of the SPI communication protcol.
---

# Info

The `SoftwareSPIBus` provides a software implementation of the SPI serial communication protocol.


# API

## Constructors

#### `public SoftwareSPIBus(MSH.Cpu.Pin mosi, MSH.Cpu.Pin miso, MSH.Cpu.Pin clock, MSH.Cpu.Pin chipSelect, byte cpha = 0, byte cpol = 0)`

Instantiates a new `SoftwareSPIBus` object using the specified pins.

## Method

#### `public void WriteByte(byte value)`

**Not yet implemented**

Write a single byte to the device.

#### `public void WriteBytes(byte[] values)`

**Not yet implemented**

Write a number of bytes to the device.

#### `public void WriteRegister(byte register, byte value)`

**Not yet implemented**

Write data to a register in the device.

#### `public void WriteRegisters(byte address, byte[] data)`

**Not yet implemented**

Write data to one or more registers.

#### `public void WriteUShort(byte address, ushort value, ByteOrder order)`

**Not yet implemented**

Write an unsigned short to the device.

#### `public void WriteUShorts(byte address, ushort[] values, ByteOrder order)`

**Not yet implemented**

Write a number of unsigned shorts to the device.

#### `public byte WriteRead(byte value)`

Write and read a single byte.

#### `public byte[] WriteRead(byte[] write, ushort length)`

Write data to the device and also read some data from the device.

#### `public byte[] ReadBytes(ushort numberOfBytes)`

**Not yet implemented**

Read the specified number of bytes from the SPI device.

#### `public byte ReadRegister(byte address)`

**Not yet implemented**

Read a register from the device.

#### `public byte[] ReadRegisters(byte address, ushort length)`

**Not yet implemented**

Read one or more registers from the device.

#### `public ushort ReadUShort(byte address, ByteOrder order = ByteOrder.LittleEndian)`

**Not yet implemented**

Read an unsigned short from a pair of registers.

#### `public ushort[] ReadUShorts(byte address, ushort number, ByteOrder order = ByteOrder.LittleEndian)`

**Not yet implemented**

Read the specified number of unsigned shorts starting at the register address specified.




