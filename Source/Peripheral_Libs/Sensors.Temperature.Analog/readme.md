# Analog Temperature Sensor

The analog temperature sensor driver can be used with any sensor that has a linear response.  This includes the following sensors:

* TMP35
* TMP36
* TMP37
* LM35

These sensors are characterised by a linear change in the analog voltage for each degree centigrade.  This is often presented in the datasheet as follows:

![Linear Analog Sensor](AnalogSensorLinearResponse.png)

This driver can work with any sensor of this type.

## Purchasing

TMP36 sensors can be purchased from a number of suppliers including:

* [Adafruit](https://www.adafruit.com/product/165)
* [Sparkfun](https://www.sparkfun.com/products/10988)

## Hardware

Consider the TMP36, this sensor requires only three connections; power, ground and the analog output:

![TMP36 Connected to Netduino](TMP36.png)

## Software

The following application reads the temperature every five seconds and prints the temperature reading on the debug console:

```csharp
using System.Threading;
using Microsoft.SPOT;
using SecretLabs.NETMF.Hardware.NetduinoPlus;
using Netduino.Foundation.Sensors.Temperature.Analog;

namespace NFAnalogTemperatureSensorTest
{
    public class Program
    {
        public static void Main()
        {
            Debug.Print("** Read TMP36 **");
            var tmp36 = new AnalogTemperatureSensor(AnalogChannels.ANALOG_PIN_A0, AnalogTemperatureSensor.SensorType.TMP36);
            while (true)
            {
                Debug.Print("Reading: " + tmp36.Temperature.ToString("f2"));
                Thread.Sleep(5000);
            }
        }
    }
}
```

## API

### Enums

#### `SensorType`

A number of temperature sensors are predefined and support natively by the `AnalogTemperatureSensor` class.  The natively supported sensors can be found in the `SensorType` enum:

```csharp
enum SensorType { Custom, TMP35, TMP36, TMP37, LM35, LM45, LM50 };
```

### Constructor

#### `AnalogTemperatureSensor(Cpu.AnalogChannel analogPin, SensorType sensor, int sampleReading = 25, int millivoltsAtSampleReading = 250, int millivoltsPerDegreeCentigrade = 10)`

The `AnalogTemperatureSensor` can be constructed in a number of ways:

* Using one of the built in sensor types (TMP35, TMP36 etc.)
* User defined analog sensor

##### Built-in Temperature Sensors

A new instance of a supported sensor can be constructed  by supplying the sensor type and the analog pin that the sensor is connected to:

```csharp
var tmp36 = new AnalogTemperatureSensor(AnalogChannels.ANALOG_PIN_A0, AnalogTemperatureSensor.SensorType.TMP36);
```

##### User Defined Temperature Sensor

A new sensor can be supported using the `SensorType.Custom` sensor type.  When this type of sensor is used, three additional pieces of information are required:

* Millivolts change per degree centigrade 
* Temperature of a known sensor output
* Reading for the specified temperature above

For a new sensor, the above could be:

* 10 Millivolts per degree centigrade
* 250 millivolts output at 25C

The constructor for this sensor when connected to pin `A0` would be:

```csharp
var newSensor = new AnalogTemperatureSensor(AnalogChannels.ANALOG_PIN_A0, 25, 250, 10);
```

### Properties

#### Temperature Property

The `Temperature` property takes a reading from the sensor and returns the result in degrees centigrade.

Example:

```csharp
currentTemperature = analogTemperatureSensor.Temperature;
```