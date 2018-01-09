---
layout: API
title: Color
subtitle: Represents an RGB or HSB color.
---

# Info

Represents a color defined by either its red, green, blue, and alpha components, or its hue, saturation, and brightness values.

# API

## Properties

#### `public double A { public get; readonly set; }`

Returns the alpha component of the color.

#### `public double R { public get; readonly set; }`

Returns the red component of the color.

#### `public double G { public get; readonly set; }`

Returns the green component of the color.

#### `public double B { public get; readonly set; }`

Returns the blue component of the color.

#### `public double Hue { public get; readonly set; }`

Returns the hue value of the color. Multiply by 360 to get the hue in degrees.


#### `public double Saturation { public get; readonly set; }`

Returns the saturation value of the color.


#### `public double Brightness { public get; readonly set; }`

Returns the brightness value of the color.


## Constructors

#### `public Color(double r, double g, double b, double a)`

Creates a new `Color` object with the specified red, green, blue, and alpha values.

#### `public Color(double r, double g, double b)`

Creates a new `Color` object with the specified red, green, and blue values.

### Static Constructors

#### `public static Color FromHex(string hex)`

Returns a new color from the passed in rgb hex value. For instance a value of `#990000` would create a shade of red.

#### `public static Color FromRgba(int r, int g, int b, int a)`

Returns a new color from the passed in red (`r`), green (`g`), blue (`b`), and alpha (`a`) components. Values should be from `0` to `255`, inclusive.

#### `public static Color FromRgb(int r, int g, int b)`

Returns a new color from the passed in red (`r`), green (`g`), and blue (`b`) components. Values should be from `0` to `255`, inclusive.

#### `public static Color FromRgba(double r, double g, double b, double a)`

Returns a new color from the passed in red (`r`), green (`g`), blue (`b`), and alpha (`a`) components. Values should be from `0` to `1`, inclusive.

#### `public static Color FromRgb(double r, double g, double b)`

Returns a new color from the passed in red (`r`), green (`g`), and blue (`b`) components. Values should be from `0` to `1`, inclusive.



## Methods

#### `public Color MultiplyAlpha(double alpha)`

Multiplies the current color alpha value by the passed in value.

#### `public Color AddBrightness(double delta)`

Adds the specified brightness change to the current color.

### Static Methods

#### `static void ConvertToRgb(double hue, double saturation, double brightness, Mode mode, out double r, out double g, out double b)`

Converts the specified color, described by the passed in `hue`, `saturation`, and `brightness` values into the equivalent color specified by the red (`r`), green (`g`), and blue (`b`) out parameters.

#### `static void ConvertToHsb(double r, double g, double b, Mode mode, out double h, out double s, out double l)`

Converts the specified color, described by the passed in red (`r`), green (`g`), and blue (`b`) values into the equivalent color specified by the hue (`h`), saturation (`s`), and brightness/luminosity (`l`) out parameters.


