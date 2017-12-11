# BaseShifterLcdTransferProvider

This is the base class for the shift register transfer provider.  It implements the `ILcdTransferProvider` interface.

## Classes and Structures

#### `ShifterSetup` (member of the `BaseShifterLcdTransferProvider` class)

The members of the `ShifterSetup` class allow the mapping of the functions on the LCD display pins to shift register pins.

`D4`, `D5`, `D6` and `D7` map to the four data pins connected to the LCD interface.

`BL` maps to the backlight pin on the LCD interface.

`RS` (Register Select) maps to the command or data selection pin on the LCD interface.

`RW` Read-write selection pin.

`Enable` line on the LCD interface.

## Constructor

### `BaseShifterLcdTransferProvider(ShifterSetup setup)`

Create a new `BaseShifterLcdTransferProvider` with the specified `ShiftSetup` GPIO mapping.

### `bool FourBitMode`

Determine if the LCD is in 4 or 8 bit mode.

### `void Send(byte data, bool mode, bool backlight)`

Send the specified `byte` to the display.  `mode` determines if the `data` byte is a command or a data byte.

`backlight` indicates if the backlight on the display should be turned on or not.

### `protected abstract void SendByte(byte output)`

Send a single byte to the LCD display.

This method should be implemented in the derived class.