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

This application will create a new ADXL345 object, display the device ID and then present the sensor readings once per second:

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
            Debug.Print("ADXL345 Register Test Application.");
            Debug.Print("----------------------------------");
            ADXL345 adxl345 = new ADXL345();
            Debug.Print("Device ID: " + adxl345.DeviceID);
            adxl345.SetPowerState(false, false, true, false, ADXL345.Frequency.EightHz);
            while (true)
            {
                adxl345.Read();
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

#### `ADXL345(byte address = 0x53, ushort speed = 100)`

Create a new ADXL345 object on the default address.

### Properties

#### `byte DeviceID`

Built in device ID, this should return `oxe5` for the ADXL345.

#### `X`, `Y` and `Z`

Last measurements for the x, y and z axis.  This data is only valid after a call to `Read`.

#### `OffsetX`, `OffsetY` and `OffsetZ`

The offset registers are used to compensate the values read by the sensor.  These values are automatically added to the measured values before they are written to the `X`, `Y` and `Z` registers.

### Methods

#### `void Read()`

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