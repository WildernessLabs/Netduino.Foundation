Waveshare Wiki: https://www.waveshare.com/wiki/2.9inch_e-Paper_Module
Arduino driver sound code: https://github.com/soonuse/epd-library-arduino

Display Naming convention:
Waveshare names their displays by size - so the 1.54" e-Paper display is simply known as the 1.54" e-Paper display and the control board is the HAT.

The drivers follow a similar convention, the 1.54" driver is named EDP1i54 (E Paper Display 1 inch .54).

Letters in Name:
Color displays are noted by a B or C. B = Red, C = Yellow.
At the time of writing, WaveShare has a 2.13D display, the D has a lower resolution than the regular 2.13" display.

Selecting a driver:
Select the driver based on the size or diagonal measurement of your display. Most displays show the size in inches on both the the front and back of the driver board (HAT).

If your display includes red, use the B varient of the display, if it includes yellow, use C.

What if I can't find a driver?
Not all displays are currently supported, you can reference existing drivers and compare to the published Arduino drivers to create your own. Don't forget to sumbit a pull-request! :)

What if I'm using an e-Paper display from another manufacturer?
Displays from other manufactures appear to be using the same driver control boards and will likely work. However these haven't been tested yet. Try it and let us know!

