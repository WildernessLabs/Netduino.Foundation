# GpioLcdTransferProvider

Use upto 11 GPIO pins on a microcontroller to control a LCD display.  This class implements the `ILcdTransferProvider` interface.

## API

### Properties

#### `bool FourBitMode { get; private set; }`

Determine if the chip is in 4 or 8 bit mode.

### Constructors

#### `GpioLcdTransferProvider(Cpu.Pin rs, Cpu.Pin rw, Cpu.Pin enable, Cpu.Pin d4, Cpu.Pin d5, Cpu.Pin d6, Cpu.Pin d7)`

Create a new `GpioLcdTransferProvider` object.

* `rs` - GPIO pin that is connected to the RS (register select) pin on the LCD
* `rw` - CPU pin that is connected to the RW (Read/Write) pin on the LCD (optional).
* `enable` GPIO pin is connected to the enable pin on the LCD.
* `d4` - `d7` - GPIO pin on the microcontroller connected to digital pins 0-7 pin on the LCD display.

#### `GpioLcdTransferProvider(Cpu.Pin rs, Cpu.Pin enable, Cpu.Pin d0, Cpu.Pin d1, Cpu.Pin d2, Cpu.Pin d3, Cpu.Pin d4, Cpu.Pin d5, Cpu.Pin d6, Cpu.Pin d7)`

Create a new `GpioLcdTransferProvider` object.

* `rs` - GPIO pin that is connected to the RS (register select) pin on the LCD
* `enable` GPIO pin is connected to the enable pin on the LCD.
* `d0` - `d7` - GPIO pin on the microcontroller connected to digital pins 0-7 pin on the LCD display.

#### `GpioLcdTransferProvider(Cpu.Pin rs, Cpu.Pin enable, Cpu.Pin d0, Cpu.Pin d1, Cpu.Pin d2, Cpu.Pin d3, Cpu.Pin d4, Cpu.Pin d5, Cpu.Pin d6, Cpu.Pin d7)`

Create a new `GpioLcdTransferProvider` object.

* `rs` - GPIO pin that is connected to the RS (register select) pin on the LCD
* `rw` - CPU pin that is connected to the RW (Read/Write) pin on the LCD (optional).
* `enable` GPIO pin is connected to the enable pin on the LCD.
* `d0` - `d7` - GPIO pin on the microcontroller connected to digital pins 0-7 pin on the LCD display.

### Methods

#### `void Send(byte value, bool mode, bool backlight)`

Send a single byte of data to the display.

`mode` should be set to `true` if the byte is a data byte or `false` for a data byte.

`backlight` determines if the backlight on the display should be turned on or off.