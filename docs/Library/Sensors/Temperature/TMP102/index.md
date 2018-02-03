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

The TMP102 temperature sensor can operate in interrupt or polling mode.

### Interrupt Mode

The example below will check the temperature every second.  An interrupt will be raised if the difference in temperature between the last reported reading and the current reading is greater than + / - 0.1 &deg;C.

```csharp
using System.Threading;
using Microsoft.SPOT;
using Netduino.Foundation.Sensors.Temperature;

namespace TMP102InterruptSample
{
    /// <summary>
    ///     Illustrate how to read the TMP102 using interrupts.
    /// </summary>
    public class Program
    {
        public static void Main()
        {
            Debug.Print("TMP102 Interrupt Sample");
            //
            //  Create a new TMP102 object, check the temperature every
            //  1s and report any changes grater than +/- 0.1C.
            //
            var tmp102 = new TMP102(updateInterval: 1000, temperatureChangeNotificationThreshold: 0.1F);
            //
            //  Hook up the interrupt handler.
            //
            tmp102.TemperatureChanged += (s, e) =>
            {
                Debug.Print("Temperature: " + e.CurrentValue.ToString("f2"));
            };
            //
            //  Now put the main program to sleep as the interrupt handler
            //  above deals with processing the sensor data.
            //
            Thread.Sleep(Timeout.Infinite);
        }
    }
}
```

### Polling Mode

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

This API supports a polling and interrupt method of reading the sensor.  In polling mode, the `Update` method forces the sensor to take a new reading and then record the current temperature in the `Temperature` property.

### Constants

#### `const ushort MinimumPollingPeriod = 100`

Minimum value for the `updateInterval` property in the constructor.  This represents the minimum number of milliseconds between sensor samples when operating in interrupt mode.

### Enums

#### `enum Resolution`

Allows the sensor to be configured in 12-bit `Resolution12Bits` or 13-bit `Resolution13Bits` mode.

### Constructors

#### `TMP102(byte address = 0x48, ushort speed = 100, ushort updateInterval = MinimumPollingPeriod, float temperatureChangeNotificationThreshold = 0.001F`

Create a new `TMP102` object using the default settings for the sensor.

### Properties

#### `float Temperature`

Retrieve the last read temperature.  In polling mode, the `Temperature` property is only valid after a call to `Update`.  In interrupt mode the `Temperature` property will be updated periodically.

#### `public float TemperatureChangeNotificationThreshold { get; set; } = 0.001F`

Threshold for the `TemperatureChanged` event.  Differences between the last notified value and the current value which exceed + / - `TemperateChangedNotificationThreshold` will generate an interrupt.

### Methods

#### `void Update()`

Force the sensor to take a reading and record the readings in the `Pressure` and `Temperature` properties.

### Events

#### `event SensorFloatEventHandler TemperatureChanged`

A `TemperatureChanged` event is raised when the difference between the current and last temperature readings exceed +/- `temperatureChangeNotificationThreshold`.  The event will return the last reading and the current reading (see [`SensorFloatEventArgs`](/API/Sensors/SensorFloatEventArgs)).