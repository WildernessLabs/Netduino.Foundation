---
layout: Library
title: Shifter74Hc595LcdTransferProvider
---

# API

This class is derived from `BaseShifterLcdTransferProvider` class.

### Enums

#### `enum BitOrder`

Allow the specification of the bit order for the data transfer.

### Properties

#### `ShifterSetup DefaultSetup`

Default setup for the shift register pin mapping.

### Constructors

#### `Shifter74Hc595LcdTransferProvider(SPI.SPI_module spiBus, Cpu.Pin latchPin, BitOrder bitOrder)`

Create a new shift register (say 74595) on the specified SPI bus (`spiBus`)

* `spiBus` is the SPI bus to use for communication with the shift register.
* `latchPin` Pin to be used to latch the data into the shift register.
* `bitOrder` Order in which the data is transmitted to the shift register (MSB or LSB).

#### `Shifter74Hc595LcdTransferProvider(SPI.SPI_module spiBus, Cpu.Pin latchPin)`

Create a new shift register (say 74595) on the specified SPI bus.

* `spiBus` is the SPI bus to use for communication with the shift register.
* `latchPin` Pin to be used to latch the data into the shift register.

This constructor uses the default pin mapping between the shift register and the LCD display and the bit order defaults to MSB.

#### `Shifter74Hc595LcdTransferProvider(SPI.SPI_module spiBus, Cpu.Pin latchPin, BitOrder bitOrder, ShifterSetup setup)`

Create a new shift register (say 74595) on the specified SPI bus.

* `spiBus` is the SPI bus to use for communication with the shift register.
* `latchPin` Pin to be used to latch the data into the shift register.
* `bitOrder` Order in which the data is transmitted to the shift register (MSB or LSB).
* `setup` Shift register pin mapping between the shift register and the LCD control pins.

### Methods

#### `void SendByte(byte output)`

Send a single byte to the LCD display.