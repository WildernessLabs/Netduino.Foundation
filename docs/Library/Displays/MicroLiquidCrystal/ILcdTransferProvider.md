---
layout: Library
title: ILcdTransferProvider
---

## API

### `bool FourBitMode`

Indicate if the display is in 4-bit or 8-bit mode.

### `void Send(byte data, bool mode, bool backlight)`

Send the specified `byte` to the display.  `mode` determines if the `data` byte is a command or a data byte.

`backlight` indicates if the backlight on the display should be turned on or not.