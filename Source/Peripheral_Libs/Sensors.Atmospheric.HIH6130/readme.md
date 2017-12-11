# HIH6130 - Temperature and Humidity Sensor

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

The following application polls the HIH6130 sensor once every second and displays the temperature and humidity in the debug output:

```csharp
using Microsoft.SPOT;
using Netduino.Foundation.Sensors.Barometric;
using System.Threading;

namespace HIH6130Test
{
    public class Program
    {
        public static void Main()
        {
            HIH6130 hih6130 = new HIH6130();
            while (true)
            {
                hih6130.Read();
                Debug.Print("Temperature: " + hih6130.Temperature.ToString("f2") + ", Humidity: " + hih6130.Humidity.ToString("f2"));
                Thread.Sleep(1000);
            }
        }
    }
}
```

## API

### Constructor

#### `HIH6130(byte address = 0x27, ushort speed = 100)`

Create a new HIH6130 object with the default address and I2C bus speed.

### Properties

#### `float Temperature`

Retrieve the last read temperature.  The property is only valid after a call to `Read`.

#### `float Humidity`

Retrieve the last read humidity.  This property is only valid following a call to `Read`.

### Methods

#### `void Read()`

Force the temperature and humidity reading.  This will set the `Humidity` and `Temperature` properties with the current temperature and humidity.

This method will generate an exception if the status bits returned by the sensor are not zero.