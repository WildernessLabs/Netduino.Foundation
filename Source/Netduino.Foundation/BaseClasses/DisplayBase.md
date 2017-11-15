# DisplayBase

The abstract methods in this base class should be implemented by all hardware displays wishing to use the [`GraphicsLibrary`](/Source/Additional_Peripheral_Libraries/Netduino.Foundation.Displays.GraphicsLibrary) class.

The actual implementation of the methods (buffering etc) is left to hardware driver.

## API

### Enums

#### `public enum BitmapMode`

Determine how bitmaps should be placed into the display buffer.  Possible options are:

| Option | Description                                                   |
|--------|---------------------------------------------------------------|
| `And`  | Logically AND the bitmap with the buffer contents.            |
| `Or`   | Logically OR the bitmap with the buffer contents.             |
| `XOr`  | Logically XOR the bitmap with the buffer contents.            |
| `Copy` | Copy the bytes in the bitmap over the contents of the buffer. |

### Properties

#### `public bool IgnoreOutOfBoundsPixels`

#### `public abstract void Show()`

Copy the contents of the internal buffer to the display.  This action makes the image in the internal buffer visible to the user.

#### `public abstract void Clear(bool updateDisplay = false)`

Clear the display buffer.

The display contents will be updated immediately when `updateDisplay` is set to true.  If `updateDisplay` is false then the internal buffer will be cleared without updating the image on view to the user.

#### `public abstract void DrawPixel(int x, int y, bool colored)` and `void DrawPixel(byte x, byte y, bool colored)`

Change a single pixel on the display, the location of the pixel is given by the `x` and `y` parameters.  The buffer uses a zero based coordinate system.  So for a 128x64 pixel display the coordinates should range from 0-177 and 0-63.

`colored` determines if the pixel should be turned on (`true`) or turned off (`false`).

#### `public abstract void DrawBitmap(int x, int y, int width, int height, byte[] bitmap, BitmapMode bitmapMode)`

Copy a bitmap into the display buffer.  The top left corner of the bitmap is given by the `x` and `y` parameters.  The bitmap is contained in a single dimension `byte` array in the `bitmap` parameter.

The `width` parameter gives the number of bytes per ine in the bitmap and the `height` parameter gives the number lines in the bitmap.

`bitmapMode` determines how the bitmap is copied into the display buffer.  Currently `BitmapMode.Copy` is the only mode supported by this driver.
 