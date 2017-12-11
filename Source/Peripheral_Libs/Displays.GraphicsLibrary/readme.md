# GraphicsLibrary - General graphics library for use with display hardware

The `GraphicsLibrary` class contains a general set of graphics primitives for use with hardware displays.  Supported displays should inherit from the `DisplayBase` base class.

## API

### Properties

#### `FontBase CurrentFont`

Font to be used for any text operations.  Fonts should be derived from the [`FontBase`](FontBase.md) class.

The [`Font8x8`](Font8x8.md) class contains a font for the ASCII characters 32 to 127.

### Constructors

#### `GraphicsLibrary(DisplayBase display)`

Create a new `GraphicsLibrary` object attached to the `display`.

### Methods

#### `void DrawLine(int x0, int y0, int x1, int y1, bool colored = true)`

Draw a solid line starting at (`x0`, `y0`) to (`x1`, `y1`).

If `colored` is set to `true` then the line will be drawn in the foreground color.  When `colored` is false the line will be drawn in the background color.

#### `void DrawVerticalLine(int x0, int y0, int length, bool colored = true)`

Draw a vertical line from point (`x0`, `y0`) of the specified `length`.

If `colored` is set to `true` then the line will be drawn in the foreground color.  When `colored` is false the line will be drawn in the background color.

#### `void DrawHorizontalLine(int x0, int y0, int length, bool colored = true)`

Draw a horizontal line from point (`x0`, `y0`) of the specified `length`.

If `colored` is set to `true` then the line will be drawn in the foreground color.  When `colored` is false the line will be drawn in the background color.

#### `void DrawCircle(int centerX, int centerY, int radius, bool colored = true, bool filled = false)`

Draw a circle with the centre point (`x0`, `y0`) with the specified `radius`.

If `colored` is set to `true` then the circle will be drawn in the foreground color.  When `colored` is false the circle will be drawn in the background color.

The circle will be filled when the `filled` property is set to `true`.

#### `void DrawFilledCircle(int centerX, int centerY, int radius, bool colored)`

Call the `DrawCircle` method with the `filled` parameter set to `true`.

#### `void DrawRectangle(int xLeft, int yTop, int width, int height, bool colored = true, bool filled = false)`

Draw a rectangle with the specified `width` and `height` and the top left corner on point (`xLeft`, `yLeft`).

If `colored` is set to `true` then the rectangle will be drawn in the foreground color.  When `colored` is false the rectangle will be drawn in the background color.

The rectangle will be filled when the `filled` property is set to `true`.

#### `void DrawFilledRectangle(int xLeft, int yTop, int width, int height, bool colored = true)`

Call the `DrawRectangle` method with the `filled` parameter set to `true`.

#### `void DrawText(int x, int y, int spacing, string text, bool wrap = false)`

Copy the text into the display buffer using the `CurrentFont`.  The top left corner of the text will be at the point (`x`, `y`).

*Note* The `spacing` and `wrap` parameters are not currently used.

#### `void Show()`

Where devices offer buffing, this method will copy the contents of the buffer to the display.

#### `void Clear(bool updateDisplay = false)`

Clear the internal buffers on the display.  The display will also be updated when `updateDisplay` is `true`.

#### `void DrawBitmap(int x, int y, int width, int height, byte[] bitmap, DisplayBase.BitmapMode bitmapMode)`

Put the contents of a bitmap into the display buffer.
