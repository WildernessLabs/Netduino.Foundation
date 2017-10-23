# Si7021 - Humidity and Temperature Sensor

The Si7021 is a small temperature and humidity sensor with an I2C interface.

## Purchasing

The Si7021 is available on a breakout board from the the following suppliers:

* [Adafruit Si7021 Breakout Board](https://www.adafruit.com/product/3251)
* [Sparkfun Si7021 Breakout Board](https://www.sparkfun.com/products/13763)

## Hardware

![Si7021 Connected to Netduino](Si7021OnBreadboard.png)

Both the Sparkfun and Adafruit boards have pull-up resistors already installed on the breakout boards and so these are not required to use the sensor.

## Software

The following application reads a number of static properties from the sensor (`SerialNumber` etc.).  The current temperature and humidity are then read once a second and displayed through the debugger interface:

```csharp
using System;
using Microsoft.SPOT;
using System.Threading;
using Netduino.Foundation.Sensors.Barometric;

namespace Si7021Test
{
    public class Program
    {
        public static void Main()
        {
            Debug.Print("SI7021 Test");
            SI7021 _si7021 = new SI7021();
            Debug.Print("Serial number: " + _si7021.SerialNumber.ToString());
            Debug.Print("Firmware revision: " + _si7021.FirmwareRevision.ToString());
            Debug.Print("Sensor type: " + _si7021.SensorType.ToString());
            Debug.Print("Current resolution: " + _si7021.Resolution.ToString());
            while (true)
            {
                _si7021.Read();
                Debug.Print("Temperature: " + _si7021.Temperature.ToString("f2") + ", Humidity: " + _si7021.Humidity.ToString("f2"));
                Thread.Sleep(1000);
            }
        }
    }
}
```

## API

### Enums

#### `DeviceType`

Possible types of sensor.

| Code              | Description        |
|-------------------|--------------------|
| Unknown           | Unknown sensor     |
| Si7013            | Si7013             |
| Si7020            | Si7020             |
| Si7021            | Si7021             |
| EngineeringSample | Engineering sample |

### Constructors

#### `SI7021(byte address = 0x40, ushort speed = 100)`

Create a new SI7021 object.

### Properties

#### `Humidity`

Humidity reading from the last call to `Read`.

#### `Temperature`

Temperature reading from the last call to `Read`.

#### `SerialNumber`

The serial number from the sensor.

#### `SensorType`

Type of sensor attached to the Netduino.

#### `FirmwareRevision`

Firmware revision number as read from the sensor.

#### `Resolution`

Resolution of the sensor:

| Sensor Code | Temperature Resolution | Humidity Resolution |
|-------------|------------------------|---------------------|
| 0           | 14 bit                 | 14 bit              |
| 1           | 12 bit                 | 8 bit               |
| 2           | 13 bit                 | 10 bit              |
| 3           | 11 bit                 | 11 bit              |

### Methods

#### `Read`

Read the current humidity and temperature from the sensor.

#### `Reset`

Perform a soft reset and then read the humidity and temperature from the sensor.

#### `Heater`

Turn the heater on or off.