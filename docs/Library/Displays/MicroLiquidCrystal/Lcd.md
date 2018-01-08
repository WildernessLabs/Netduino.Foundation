---
layout: Library
title: LCD Class
---

# API

### Properties

#### `bool ShowCursor`

Turn the cursor on (`true`) or off (`false`).

#### `bool BlinkCursor`

Display a blinking cursor (`true`) or static cursor(`false`).

#### `bool Visible`

Make the contents of the LCD visible (`true`) or turn the display off (`false`).  Make this display contents visible restores the previous contents and cursor conditions.

#### `bool Backlight`

Turn the backlight on (`true`) or off (`false`).

#### `Encoding Encoding`

Encoding used for the characters on the display.  the default encoding is `UTF8`.

### Constructors

#### `Lcd(ILcdTransferProvider provider)`

Create a new `Lcd` object configured to use the specified transfer provider.

### Methods

#### `Begin(columns, lines)`

Setup the display with the specified number of `columns` and `lines`.  By default the text will scroll from left to right when scrolling is enabled.

For one line displays, the font height will be set to standard height.

#### `void Begin(byte columns, byte lines, bool leftToRight, bool dotSize)`

Setup the display with the specified number of `columns` and `lines`.

When scrolling is enabled, the text will scroll left to right when `leftToRight` is `true` or right to left when `leftToRight` is `false`.

For single line displays it is possible (where supported) to use a 10 pixel heigh font.  This is indicated by the `dotSize` parameter.

#### `void Clear()`

Clear the contents of the display.

#### `void Home()`

Move the cursor to the upper left corner of the LCD.

#### `void SetCursorPosition(column, line)`

Move the cursor to the specified `column` and `row`.  Note that both of these values are 0 indexed.

#### `void ScrollDisplayLeft()` and `void ScrollDisplayRight()`

Scroll the display one position to the left or right.

#### `void MoveCursor(bool right)`

Move the cursor one position left (when `right` is `false`) or right.

#### `void Write(string text)`

Convert the string using the preset encoding (see the `Encoding` property) to s stream of characters and send the characters to the LCD display.

#### `void Write(byte[] buffer, int offset, int count)`

Write a `count` bytes from the `buffer` to the LCD display starting at the specified `index`.

#### `void WriteByte(byte data)`

Write a single byte to the display at the current cursor position.

#### `void SendCommand(byte command)`

Send the specified `command` byte to the LCD display.

#### `void CreateChar(int location, byte[] charmap, int offset)`

Create a customs character at `location` in the custom character buffer.  `location` should be in the range 0-7.

The pixel data is stored in the `charmap` at the specified `offset`.

#### `void CreateChar(int location, byte[] charmap)`

Create a customs character at `location` in the custom character buffer.  `location` should be in the range 0-7.

The pixel data is stored in the `charmap`.