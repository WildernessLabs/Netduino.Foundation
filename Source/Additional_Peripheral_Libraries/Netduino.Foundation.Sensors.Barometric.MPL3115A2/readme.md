# MPL3115A2 - Pressure and Temperature Sensor

The MPL3115A2 is a barometric pressure sensor capable of reading both temperature and temperature compensated pressure reading.  This sensor includes the following features:

* I2C digital interface
* 24-bit ADC
* Altitude and pressure measurements
* Temperature sensor

## Purchasing

The MPL3115A2 is available on breakout boards and a weather shield:

* [Adafruit MPL3115A2 Breakout Board](https://www.adafruit.com/product/1893)
* [Sparkfun MPL3115A2 Breakout Board](https://www.sparkfun.com/products/11084)
* [Sparkfun Weather Shield](https://www.sparkfun.com/products/13956)

## Hardware

MPL3115A2 configured for polling more data reads:

![MPL3115A2 on Breadboard in Polling Mode](MPL3115A2OnBreadboard.png)

## Software

The following application reads the temperature and pressure every second and displays the result on the debug console:

```csharp
using System.Threading;
using Microsoft.SPOT;
using Netduino.Foundation.Sensors.Barometric;

namespace MPL3115A2Test
{
    public class Program
    {
        public static void Main()
        {
            Debug.Print("MPL3115A2 Test");
            var mpl3115a2 = new MPL3115A2();
            while (true)
            {
                mpl3115a2.Read();
                Debug.Print("Temperature: " + mpl3115a2.Temperature.ToString("f2") + ", Pressure: " + mpl3115a2.Pressure.ToString("f2"));
                Thread.Sleep(1000);
            }
        }
    }
}
```

## API

### Constructors

#### `MPL3115A2(byte address = 0x60, ushort speed = 400)`

Create a new `MPL3115A2` object with the default `I2C` address and bus speed.

### Properties

#### `double Temperature`

Temperature in degrees centigrade from the last time the `Read` method was called.

#### `double Pressure`

Pressure in Pascals from the last time the `Read` method was called.

#### `bool Standby`

Get or set the standby status of the sensor.

Note that calling the `Read` method will take the sensor out of standby mode.

### Methods

#### `void Read()`

Read the current temperature and pressure from the sensor and set the `Temperature` and `Pressure` properties.

#### `void Reset()`

Reset the sensor.