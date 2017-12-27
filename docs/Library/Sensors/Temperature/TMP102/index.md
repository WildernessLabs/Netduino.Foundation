---
layout: Library
title: TMP102
subtitle: Temperature sensor.
---

The TMP102 is a temperature sensor capable of reading the current temperature with an accuracy of 0.5C over the range of -25C to 85C with a total range of -40C to 125C.

## Purchasing

TMP102 sensors are available on a breakout board from the following suppliers:

* [Sparkfun TMP102 Breakout Board](https://www.sparkfun.com/products/13314)

## Hardware

TMP102 sensors can be connected to the Netduino using only four connections:

![TMP102 On Breadboard](TMP102OnBreadboard.png)

## Software

The following application reads the temperature from the TMP102 sensor every second and displays the results in the debug console:

```csharp
using System.Threading;
using Microsoft.SPOT;
using Netduino.Foundation.Sensors.Temperature;

namespace TMP102Test
{
    public class Program
    {
        public static void Main()
        {
            Debug.Print("TMP102 Test");
            var tmp102 = new TMP102();
            while (true)
            {
                Debug.Print("Temperature: " + tmp102.Temperature.ToString("f2"));
                Thread.Sleep(1000);
            }
        }
    }
}
```

## API

### Enums

#### `enum Resolution`

Allows the sensor to be configured in 12-bit `Resolution12Bits` or 13-bit `Resolution13Bits` mode.

### Constructors

#### `TMP102(byte address = 0x48, ushort speed = 100)`

Create a new `TMP102` object using the default `address` and `speed` settings for the sensor.

### Properties

#### `double Temperature`

Returns the current temperature in degrees centigrade.