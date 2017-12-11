# SHT31D - Temperature and Humidity Sensor

The SHT31D is a temperature and humidity sensor with a built in I2C interface.  The sensor has a typical accuracy of +/- 2% relative humidity and +/- 0.3C.

## Hardware

![Adafruit SHT31D on Breadboard](SHT31DOnBreadboard.png)

The SHT31D breakout board from Adafruit is supplied with pull-up resistors installed on the `SCL` and `SDA` lines.

The `ADR` line is tied low giving and I2C address of 0x44.  This address line can also be tied high and in this case the I2C address is 0x45.

## Purchasing

The SHT31D temperature and humidity is available on a breakout board from Adafruit:

* [SHT31D Temperature and Humidity Sensor](https://www.adafruit.com/product/2857)

## Software

```csharp
using Microsoft.SPOT;
using Netduino.Foundation.Sensors.Barometric;
using System.Threading;

namespace SHT31DTest
{
    public class Program
    {
        public static void Main()
        {
            SHT31D sht31d = new SHT31D();

            Debug.Print("SHT31D Temperature / Humidity Test");
            while (true)
            {
                sht31d.Read();
                Debug.Print("Temperature: " + sht31d.Temperature.ToString("f2") + ", Humidity: " + sht31d.Humidity.ToString("f2"));
                Thread.Sleep(1000);
            }
        }
    }
}
```

## API

### Constructor

#### `SHT31D(byte address = 0x44, ushort speed = 100)`

Create a new SHT31D temperature and humidity sensor object.  The address defaults to 0x44 and the speed of the I2C bus to 100 Khz.

### Properties

#### `float Temperature`

Last temperature reading made when the `Read` method was called.

#### `float Humidity`

Last humidity reading made when the `Read` method was called.

### Methods

#### `void Read()`

The `Read` method forces a temperature and humidity reading from the SHT31D temperature and humidity sensor.  The reading is made using high repeatability mode.