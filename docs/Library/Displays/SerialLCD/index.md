---
layout: Library
title: Serial LCD
subtitle: Serial LCD backpacks provide a convenient way to display data on a 16x2 or 20x4 LCD display.
---


## Purchasing

This driver supports the [Sparkfun Serial Backpack](https://www.sparkfun.com/products/258).  This can also be purchased as a [kit](https://www.sparkfun.com/products/10097) with both the serial backpack and the LCD.

## Hardware

The Serial LCD requires only three connections; power, ground and serial data.  The `T`<sub>`x`</sub> pin on the Netduino should be connected to the `R`<sub>`x`</sub> pin on the serial backpack:

![Netduino Connected to SerialLCD](SerialLCD.png)

**Note:** This display uses a COM port on the Netduino.  This means that both the `T`<sub>`x`</sub> and the `R`<sub>`x`</sub> pins are  allocated to the COM port even though the R<sub>x</sub> pin is not used for communication. By default, it uses COM1, which means that both pin 1 and 2 are reserved. Attempting to use a reserved pin for another use will result in a runtime error.

## Software

The application below outputs <i>Hello, world</i> on the LCD:

```csharp
using System.Threading;
using Netduino.Foundation.Displays;

namespace SerialLCDTest
{
    public class Program
    {
        public static void Main()
        {
            var display = new SerialLCD();
            //
            //  Clear the display ready for the test.
            //
            display.Clear();
            display.SetCursorStyle(SerialLCD.CursorStyle.BlinkingBoxOff);
            display.SetCursorStyle(SerialLCD.CursorStyle.UnderlineOff);
            //
            //  Display some text on the bottom row of a 16x2 LCD.
            //
            display.SetCursorPosition(2, 1);
            display.DisplayText("Hello, world");
            Thread.Sleep(1000);
       }
    }
}
```

## API

### Enums

#### `CursorStyle`

Defines the two possible cursor styles (line and box) along with an indication of the state of the cursor (on or off).

#### `DisplayPowerState`

Possible display power states (on or off).

#### `LCDDimensions`

Describe the number of characters and lines on the LCD display.

#### `LCDBaudRate`

Possible baud rates for the display (2400, 4800, 9600, 14400, 19200 or 38400).

#### `Direction`

Define the possible directions for scrolling the display and also text direction.

### Constructor

#### `SerialLCD(TextDisplayConfig config = null, string port = "COM1", int baudRate = 9600, Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One)`

This can be used with the display as supplied by creating a new object with the defaults:

```csharp
var lcd = new SerialLCD();
```

The properties for a particular display can be set by supplying values for the parameters in the constructor.

### Methods

#### `void Clear()`

The `Clear` method will clear the display.

#### `void ClearLine(byte lineNumber)`

Clears the specified line by writing a string of empty characters to it.

#### `void MoveCursor(Direction direction)`

`MoveCursor` moves the cursor left or right on the LCD.

#### `public void SaveCustomCharacter(byte[] characterMap, byte address)`

Saves a custom character to one of 8 slots in the CGRAM. Custom characters are defined as character maps and can be created using online graphical tools such as the one found at: [maxpromer.github.io/LCD-Character-Creator/](http://maxpromer.github.io/LCD-Character-Creator/).

##### Parameters

| Parameter | Description  |
|-----------|--------------|
| `characterMap` | A byte array defining the character. For a list of sample characters, see [this](https://www.quinapalus.com/hd44780udg.html). |
| `address` | The slot to put the character in. Can be `0`-`7`. <br/><br/>**Note:** due to .Net's underlying string implementation, slot 0 is unusable unless you decode the string yourself and use the `Write(byte[] chars)` method. Otherwise,  when the string is created, .Net treats the `0` character as a string terminator. |

The following code illustrates saving 4 custom characters and then displaying them on the screen.

```csharp
using System.Threading;
using System.Text;
using Microsoft.SPOT;
using H = Microsoft.SPOT.Hardware;
using N = SecretLabs.NETMF.Hardware.Netduino;
using Netduino.Foundation.Displays;

namespace Netduino.Foundation.Samples
{
    public class Program
    {
        public static void Main()
        {
            SerialLCD _display = new SerialLCD();

            // define our character maps.
            // see http://maxpromer.github.io/LCD-Character-Creator/ for 
            // a GUI character maker
            byte[] happyFace = { 0x0, 0x0, 0xa, 0x0, 0x11, 0xe, 0x0, 0x0 };
            byte[] sadFace   = { 0x0, 0x0, 0xa, 0x0, 0xe, 0x11, 0x0, 0x0 };
            byte[] rocket    = { 0x4, 0xa, 0xa, 0xa, 0x11, 0x15, 0xa, 0x0 };
            byte[] heart     = { 0x0, 0xa, 0x1f, 0x1f, 0xe, 0x4, 0x0, 0x0 };

            // save the custom characters
            _display.SaveCustomCharacter(happyFace, 1);
            _display.SaveCustomCharacter(sadFace, 2);
            _display.SaveCustomCharacter(rocket, 3);
            _display.SaveCustomCharacter(heart, 4);

            _display.Clear();
            _display.SetBrightness();

            // create our string, using the addresses of the characters
            // casted to char.
            StringBuilder s = new StringBuilder();
            s.Append("1:" + (char)1 + " ");
            s.Append("2:" + (char)2 + " ");
            s.Append("3:" + (char)3 + " ");
            s.Append("4:" + (char)4 + " ");
            _display.WriteLine(s.ToString(), 0);

            Thread.Sleep(Timeout.Infinite);
        }
    }
}
```

The output of this code looks like:

![](Custom_Characters_Displayed.jpg)

#### `void ScrollDisplay(Direction direction)`

`ScrollDisplay` moves the contents of the display left or right.

It is possible to scroll the contents of the display all the way to the left (or right) so that the contents are no longer visible.

Writing to the display when the scrolled fully left or right will change the contents of the display memory but not the actual display.  It is necessary to scroll the display in the reverse direction in order to view the updated contents.

#### `void SetBaudRate(LCDBaudRate baudRate)`

`SetBaudRate` will change the baud rate of the display.  A new `SerialLCD` object will need to be created once the baud rate is changed as the serial port will still be using the old baud rate.

#### `public void SetBrightness(float brightness = 0.75f)`

Sets the backlight brightness of the LCD. Valid values are `0` through `1`. Sleeps for 125 milliseconds after setting to let the display settle. `0` turns the backlight off, `1` sets it to 100%, and `0.5` sets it to 50%.

The following code creates a new LCD display and sets the brightness to 100%:

```csharp
SerialLCD _display = new SerialLCD();
// set to 100% brightness
_display.SetBrightness(1);
```

#### `void SetCursorPosition(byte column, byte line)`

This method will set the cursor position on the LCD display.  The cursor locations are zero based.

Example:

```csharp
SetCursorPosition(0, 0);
```

This moves the cursor to the top left corner of a 16x2 LCD.

#### `void SetCursorStyle(CursorStyle style)`

Two cursors are available, box or underline.

#### `void SetDisplayVisualState(DisplayPowerState state)`

Turn the display on or off.

#### `void SetSplashScreen(string line1, string line2)`

A splash screen is displayed when the LCD is first powered up.  This can be changed or turned off.

`SetSplashScreen` will change the splash screen.

Example:

```csharp
SetSplashScreen("Weather Station", "Version 1.0");
```

This code will set the splash screen to:

```
Weather Station
Version 1.0
```

#### `void ToggleSplashScreen()`

`ToggleSplashScreen` will turn the splash screen on or off depending upon the current setting.

#### `void Write(string text)`

Write the text to the current cursor position in the display memory.

#### `public void Write(byte[] chars)`

Displays the characters at the current cursor position. Unlike the `string text` overload, this assumes the text has already been encoded to characters. Can be useful in sending pre-encoded characters, or accessing custom a custom character saved in the 0 slot.

#### `void WriteLine(string text, byte lineNumber)`

Write the text to the specified line on the display.
