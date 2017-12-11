# BME280 Temperature, Pressure and Humidity Sensor

The BME280 is a combined temperature, pressure and humidity sensor.

## Purchasing

The BME280 sensor is available as a breakout board from the following suppliers:

* [Sparkfun BME280](https://www.sparkfun.com/products/13676)
* [Adafruit BME280](https://www.adafruit.com/product/2652)

## Hardware

The BME280 can be connected using I2C or SPI.  Only 4 wires are required when using I2C:

* 3.3V
* Ground
* SDA
* SCL

![BME280 on Breadboard](BME280OnBreadboard.png)

It should be noted that the Sparkfun board is supplied with pull-up resistors enabled by default.  The Adafruit board does not have any pull-up resistors onboard.  It is therefore necessary to add two pull-up resistors (`4.7 K` should be adequate for a single device) between 3.3V and SDA and 3.3V and SCL.

## Software

The following creates an instance of the `BME280` class using the I2C interface.  The temperature, pressure and humidity are read every second and the readings displayed using the debugger.

```csharp
using Microsoft.SPOT;
using Netduino.Foundation.Sensors.Barometric;
using System.Threading;

namespace BME280Test
{
    public class Program
    {
        public static void Main()
        {
            BME280 sensor = new BME280();

            string message;
            while (true)
            {
                sensor.Read();
                message = "Temperature: " + sensor.Temperature.ToString("F1") + " C\n";
                message += "Humidity: " + sensor.Humidity.ToString("F1") + " %\n";
                message += "Pressure: " + (sensor.Pressure / 100).ToString("F0") + " hPa\n\n";
                Debug.Print(message);
                Thread.Sleep(1000);
            }
        }
    }
}
```

## API

### Enums

#### `Oversample`

Possible oversample rates (`Skip, 1, 2, 4, 8, or 16).

#### `Modes`

Operating modes, `Sleep`, `Forced` or `Normal`.

#### `StandbyDuration`

Possible values for the inactive duration when running in `Normal` mode.  Possible durations range from 0.5 ms to 20 ms.

#### `FilterCoefficient`

Possible values for the filter coefficient.

### Constructor

#### `BME280(byte address = 0x77, ushort speed = 100)`

The simplest constructor creates a new `BME280` object configured to use I2C with the default address of `0x77` and a default speed of `100` KHz.

Default values are set for the sensor properties:

* Mode is set to normal.
* Oversampling setting for temperature, pressure and humidity set to x1
* Filter is turned off.
* Standby period is set to 0.5 milliseconds.

### Properties

#### `float Temperature`

Temperature in &deg;C.

#### `float Pressure`

Air pressure in Pascals.

#### `float Humidity`

Relative humidity as a percentage.

#### `TemperatureOverSampling`

Set the temperature oversampling rate.

#### `PressureOversampling`

Set the pressure oversampling rate.

#### `HumidityOverSampling`

Set the humidity oversampling rate.

#### `Mode`

Set the sensor operating mode.

#### `Standby`

Set the inactive duration in normal mode.

#### `Filter`

Set the time constant for the IIR filter.

#### `Measuring`

Indicate if the sensor is making measurements.

#### `UpdatingMemory`

Indicates if the sensor is copying data into memory.

### Methods

#### `void Reset()`

Resets the sensor and reads the compensation data.

#### `void Read()`

Read the current temperature, pressure and humidity.

This method must be called before accessing the `Temperature`, `Pressure` or `Humidity` properties.

#### `void UpdateConfiguration()`

This method should be called when one or more of the following properties are modified:

* `TemperatureOverSampling`
* `PressureOversampling`
* `HumidityOverSampling`
* `Mode`
* `Standby`
* `Filter`

Setting any of the above properties without calling `UpdateConfiguration` will not change the operation of the sensor.  Calling `UpdateConfiguration` will cause the properties in the sensor to be updated and so the method of operation of the sensor will also change.