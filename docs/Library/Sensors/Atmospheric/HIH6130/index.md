---
layout: Library
title: HIH6130
subtitle: I2C Temperature and Humidity Sensor
---

The HIH6130 sensor allows the reading of the relative humidity and temperature providing the data over an I2C interface.

## Purchasing

The HIH6130 sensor is available on a breakout board from Sparkfun.

* [Sparkfun HIH6130 Breakout Board](https://www.sparkfun.com/products/11295)

## Hardware
The HIH6130 requires only four connections between the Netduino and the breakout board.

| Netduino Pin | Sensor Pin     | Wire Color |
|--------------|----------------|------------|
| 3.3V         | V<sub>cc</sub> | Red        |
| GND          | GND            | Black      |
| SC           | SCK            | Blue       |
| SD           | SDA            | White      |

![HIH6130 on Breadboard](HIH6130OnBreadboard.png)

## Software

The HIH6130 can operate is a polling or interrupt mode.  The default operation is interrupt mode.

### Interrupt Mode

The following sample demonstrates how to put the HIH6130 sensor into interrupt mode.  The sensor will generate an interrupt every 100 ms (default setting for the polling period).

```csharp
using Microsoft.SPOT;
using Netduino.Foundation.Sensors.Atmospheric;
using System.Threading;

namespace HIH6130InterruptSample
{
    public class Program
    {
        public static void Main()
        {
            //
            //  Create a new HIH6130 and set the temperature change threshold to half a degree.
            //
            HIH6130 hih6130 = new HIH6130(temperatureChangeNotificationThreshold: 0.5F);
            //
            //  Hook up the temperature interrupt handler.
            //
            hih6130.TemperatureChanged += (s, e) =>
            {
                Debug.Print("Temperature changed: " + e.CurrentValue.ToString("f2"));
            };
            //
            //  Hook up the humidity interrupt handler.
            //
            hih6130.HumidityChanged += (s, e) =>
            {
                Debug.Print("Humidity changed: " + e.CurrentValue.ToString("f2"));
            };
            //
            //  Now put the main application to sleep.  The temperature changes will be dealt
            //  with by the event handler above.
            //
            Thread.Sleep(Timeout.Infinite);
        }
    }
}
```

### Polling Mode

The following application polls the HIH6130 sensor once every second and displays the temperature and humidity in the debug output.  The sensor is put into polling mode by setting the `updateInterval` to 0 in the constructor.

```csharp
using Microsoft.SPOT;
using Netduino.Foundation.Sensors.Atmospheric;
using System.Threading;

namespace HIH6130PollingSample
{
    /// <summary>
    ///     This application illustrates how to use the HIH6310 sensor in polling mode.
    /// </summary>
    public class Program
    {
        public static void Main()
        {
            //
            //  Create a new HIH6130 in polling mode.
            //
            HIH6130 hih6130 = new HIH6130(updateInterval: 0);
            //
            //  Loop continuously updating the sensor readings and displaying
            //  them on the debug console.
            //
            while (true)
            {
                hih6130.Update();
                Debug.Print("Temperature: " + hih6130.Temperature.ToString("f2") +
                            "C, Humidity: " + hih6130.Humidity.ToString("f2") + "%");
                Thread.Sleep(1000);
            }
        }
    }
}
```

## API

### Constants

#### `const ushort MinimumPollingPeriod = 100`

Minimum value for the `updateInterval` property in the constructor.  This represents the minimum number of milliseconds between sensor samples when operating in interrupt mode.

### Constructor

#### `public HIH6130(byte address = 0x27, ushort speed = 100, ushort updateInterval = MinimumPollingPeriod, float humidityChangeNotificationThreshold = 0.001F, float temperatureChangeNotificationThreshold = 0.001F`

Create a new HIH6130 object with the default address and I2C bus speed.

In interrupt mode, the `updateInterval` defines the number of milliseconds between samples.  By default, this is set to the `MinimumPollingPeriod` and this places the sensor in interrupt mode.  Setting the `updateInterval` to 0 milliseconds places the sensor in polling mode.

`humidityChangeNotificationThreshold` and `temperatureChangeNotificationThreshold` define the thresholds for the interrupts (events).  Any changes in the temperature and humidity readings that exceed the respective thresholds will generate the appropriate event.

### Properties

#### `float Temperature`

Retrieve the last read temperature.  The property is only valid after a call to `Read`.

#### `public float TemperatureChangeNotificationThreshold { get; set; } = 0.001F`

Threshold for the `TemperatureChanged` event.  Differences between the last notified value and the current value which exceed + / - `TemperateChangedNotificationThreshold` will generate an interrupt.

#### `float Humidity`

Retrieve the last read humidity.  This property is only valid following a call to `Read`.

#### `public float HumidityChangeNotificationThreshold { get; set; } = 0.001F`

Threshold for the `HumidityChanged` event.  Differences between the last notified value and the current value which exceed + / - `HumidityChangedNotificationThreshold` will generate an interrupt.

### Methods

#### `void Update()`

Force the temperature and humidity reading.  This will set the `Humidity` and `Temperature` properties with the current temperature and humidity.

This method will generate an exception if the status bits returned by the sensor are not zero.

### Events

#### `event SensorFloatEventHandler TemperatureChanged`

A `TemperatureChanged` event is raised when the difference between the current and last temperature readings exceed +/- `temperatureChangeNotificationThreshold`.  The event will return the last reading and the current reading (see [`SensorFloatEventArgs`](/API/Sensors/SensorFloatEventArgs)).

#### `event SensorFloatEventHandler HumidityChanged`

A `HumidityChanged` event is raised when the difference between the current and last humidity readings exceed +/- `humidityChangeNotificationThreshold`.  The event will return the last reading and the current reading (see [`SensorFloatEventArgs`](/API/Sensors/SensorFloatEventArgs)).