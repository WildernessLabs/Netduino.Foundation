# Micro Liquid Crystal Display

## Description

The μLiquidCrystal is .NET Micro Framework library for popular LCD displays with HD44780 compatible controller.
 
The library closely matches functionality implemented in the Arduino Liquid Crystal Library. It supports various methods of connecting the display to the microcontroller:
Directly to GPIO pins (see GpioLcdTransferProvider )
Indirectly via shift registers (see Shifter74Hc595LcdTransferProvider)
other methods can be added in future versions
Please see my blog for introduction to the library:
http://geekswithblogs.net/kobush/archive/2010/09/05/netmf_liquid_crystal.aspx
 
Hardware
The library was developed and tested using Netduino, but it should work with any other .NET Micro Framework board.
 
Eric D. Burdo figured out how to use this library with Arduino LCD Shield from Seedstudio Electronic Brick Starter Kit. The connections were a little tricky but he shows the proper pin assignment for the GpioLiquidCrystalTransferProvider in his blog post.
24/09/2010 - Pete Brown demonstrates how to use the library with 4x20 LCD.
 
Authors
Developed by Szymon Kobalczyk based on the Arduino Liquid Crystal Library. Includes portions of code from implementation by Pavel Bánský.
 
## License

The library is released as open source and governed by Apache License.

Last edited Sep 24, 2010 at 1:22 PM by kobush, version 16