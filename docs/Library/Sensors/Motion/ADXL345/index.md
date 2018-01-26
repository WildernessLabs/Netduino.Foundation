---
layout: Library
title: ADXL345
subtitle: Triple axis accelerometer
---

# Info

The ADXL345 is a small, low power, triple axis acceleration sensor capable of measuring up to +/-16g with a resolution of 13-bits.

## Hardware

![Sparkfun ADXL345 on Breadboard](SparkfunADXL345OnBreadboard.png)

This diagram shows how to connect the ADXL345 to the Netduino when interrupts are not required.

The `SDA` and `SCL` lines both require pull-up resistors (`4k7`) when they are not present on the breakout board.

## Purchasing

The ADXL345 is available on a small breakout board:

* [Sparkfun ADXL345 Breakout Board](https://www.sparkfun.com/products/9836)

## Software

The ADXL345 can operating in interrupt and polling mode.  Polling applications are responsible for determining when a sensor is read.  Interrupt applications will be notified when the sensor reading changes by + / - a threshold value.

### Interrupt Example

The application below demonstrates how to connect an interrupt handler to the ADXL345 sensor and display changes only when the acceleration changes in the x, y or z axis by more than the acceleration threshold (default is 5 units):

```csharp
using System.Threading;
using Microsoft.SPOT;
using Netduino.Foundation.Sensors.Motion;

namespace ADXL345InterruptSample
{
    public class Program
    {
        public static void Main()
        {
            Debug.Print("\n\n");
            Debug.Print("ADXL345 Interrupt Example.");
            Debug.Print("--------------------------");
            var adxl345 = new ADXL345();
            Debug.Print("Device ID: " + adxl345.DeviceID);
            //
            //  Attach an interrupt handler.
            //
            adxl345.AccelerationChanged += (s, e) =>
            {
                Debug.Print("X: " + e.CurrentValue.X.ToString() +
                            ", Y: " + e.CurrentValue.Y.ToString() +
                            ", Z: " + e.CurrentValue.Z.ToString());
            };
            //
            //  Interrupts are attached so power on the sensor.
            //
            adxl345.SetPowerState(false, false, true, false, ADXL345.Frequency.EightHz);
            //
            //  Put the Netduino to sleep as the interrupt handler will deal 
            //  with changes in acceleration.
            //
            Thread.Sleep(Timeout.Infinite);
        }
    }
}
```

### Polling Example

The sensor is put into polling mode by setting the `updateInterval` in the constructor to 0 milliseconds.  The following application will read the sensor and output to the debug console every 500 milliseconds:

```csharp
using Microsoft.SPOT;
using Netduino.Foundation.Sensors.Motion;
using System.Threading;

namespace ADXL345RegisterTests
{
    public class Program
    {
        public static void Main()
        {
            Debug.Print("\n\n");
            Debug.Print("ADXL345 Polling Example.");
            Debug.Print("------------------------");
            ADXL345 adxl345 = new ADXL345(updateInterval: 0);
            Debug.Print("Device ID: " + adxl345.DeviceID);
            adxl345.SetPowerState(false, false, true, false, ADXL345.Frequency.EightHz);
            while (true)
            {
                adxl345.Update();
                Debug.Print("X: " + adxl345.X.ToString() + ", Y: " + adxl345.Y.ToString() + ", Z: " + adxl345.Z.ToString());
                Thread.Sleep(500);
            }
        }
    }
}
```

## API

The ADXL345 API supports a polling mode of operation.  The general principle is to initialize the sensor and then `Read` values before extracting the measurements from the `X`, `Y` and `Z` properties.

### Enums

#### `enum Range { TwoG = 0x00, FourG = 0x01, EightG = 0x02, SixteenG = 0x03 }`

Used when changing the resolution of the sensor.

#### `enum Frequency { EightHz = 0x00, FourHz = 0x01, TwoHz = 0x02, OneHz = 0x03 }`

Used to control the frequency of sensor readings.

### Constructors

#### `ADXL345(byte address = 0x53, ushort speed = 100, ushort updateInterval = 100, double accelerationChangeNotificationThreshold = 5.0F)`

Create a new ADXL345 object on the default address.  The sensor will operate in interrupt mode with a refresh period of 100ms and a acceleration change threshold of 5.0.

### Properties

#### `double AccelerationChangeNotificationThreshold`

When operating in interrupt mode, this property defines the acceleration change threshold.  This is the size of the change in acceleration that must occur before an interrupt is generated.

#### `byte DeviceID`

Built in device ID, this should return `0xe5` for the ADXL345.

#### `X`, `Y` and `Z`

Last measurements for the x, y and z axis.  This data is only valid after a call to `Read`.

#### `OffsetX`, `OffsetY` and `OffsetZ`

The offset registers are used to compensate the values read by the sensor.  These values are automatically added to the measured values before they are written to the `X`, `Y` and `Z` registers.

### Methods

#### `void Update()`

Read the current sensor values and store the values in the `X`, `Y` and `Z` properties.

#### `void SetPowerState(bool linkActivityAndInactivity, bool autoASleep, bool measuring, bool sleep, Frequency frequency)`

Set the power state for the ADXL345.  Note that this method must be called to turn on measuring before any measurements are taken.

`linkActivityAndInactivity` - Links the inactivity and activity events when `true`.  When true and activity event must be followed by an inactivity event in order to trigger and event.

`autoSleep` - Automatically enter sleep mode if inactivity is detected.

`measuring` - Turn the sensor on when `true`.  If the sensor is turned off then the `X`, `Y` and `Z` properties will return 0.

`sleep` - Put the sensor to sleep when `true`.

`frequency` - Set the frequency of the readings.  See the `Frequency` enum.

#### `void SetDataFormat(bool selfTest, bool spiMode, bool fullResolution, bool justification, ADXL345.Range range)`

The data format register determines the way the data is presented to the `X`, `Y` and `Z` registers.

`selfTest` - Setting this value to `true` applies a test for to the sensor.

`spiMode` - When `true` this forces the SPI interface to operate in 3-wire mode.

`fullResolution` - Enter full resolution mode when `true` otherwise use the `range` setting to determine the range of the sensor.

`justification` - Set the justification to be left justified (`true`) or right justified with sign extension (`false`).

`range` - Set the sensor range to be one of +/- 2g, +/- 4g, +/-8g or +/-16g.

#### `void SetDataRate(byte dataRate, bool lowPower)`

Sets the data rate and low power mode for the sensor.  The data rate should be in the range 0-15 inclusive and the following table gives the meaning of these values:

| Data Rate | Output Data Rate | Bandwidth |
|-----------|------------------|-----------|
|     0     |      0.10 Hz     |  0.05 Hz  |
|     1     |      0.20 Hz     |  0.10 Hz  |
|     2     |      0.39 Hz     |  0.20 Hz  |
|     3     |      0.78 Hz     |  0.39 Hz  |
|     4     |      1.56 Hz     |  0.78 Hz  |
|     5     |      3.13 Hz     |  1.56 Hz  |
|     6     |      6.25 Hz     |  3.13 Hz  |
|     7     |      12.5 Hz     |  6.25 Hz  |
|     8     |      25 Hz       |  12.5 Hz  |
|     9     |      50 Hz       |  25 Hz    |
|     10    |      100 Hz      |  50 Hz    |
|     11    |      200 Hz      |  100 Hz   |
|     12    |      400 Hz      |  200 Hz   |
|     13    |      800 Hz      |  400 Hz   |
|     14    |      1600 Hz     |  800 Hz   |
|     15    |      3200 Hz     |  1600 Hz  |


`lowPower` puts the sensor in to low power mode when `true`.  In this mode the sensor is more sensitive to noise.

### Events

#### `event SensorVectorEventHandler AccelerationChanged`

This event (interrupt) is generated when the acceleration in any of the x, y or z directions exceeds the `AccelerationChangeNotificationThreshold` value.  The event will send a [`SensorVectorEventArgs`](/API/Sensors/SensorVectorEventArgs/) object containing the current and last acceleration values in [`Vector`](../../Spatial/Vector.md) objects.