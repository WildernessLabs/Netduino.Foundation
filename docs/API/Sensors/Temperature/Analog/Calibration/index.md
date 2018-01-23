---
layout: API
title: Analog Temperature Sensor Calibration
subtitle: Calibration object for linear analog temperature sensors.
---

The [`AnalogTemperature` driver](index.md) can be used with any sensor that has a linear voltage response.  The driver already works with a number of sensors, the `Calibration` object allows this to be extended to work with new sensors.

These sensors exhibit a linear change in the analog voltage for each degree centigrade.  This is often presented in the datasheet as follows:

![](../AnalogSensorLinearResponse.png)

The `AnalogTemperature` driver will work with any sensor of this type given the correct parameters.

## Theory of Operation

The linear response of the supported sensors conforms to the standard equation for a straight line:

```
y = m * x + c
```

Three values are required from the data sheet:

* Sample reading
* Sensor output (in millivolts) at the sample reading
* Millivolts change in the voltage per 1 &deg;C change in temperature

These values map onto the above equation as follows:

| Value From the Data Sheet           | Symbol in the Equation |
|-------------------------------------|------------------------|
| Sample reading                      |           x            |
| Sensor output at the sample reading |           y            |
| Sensor output change per 1 &deg;C   |           m            |

c remains the only value that is unknown.  Rearranging the equation gives the following:

```
c = y - (m * x)
```

Substituting the values from the data sheet gives:

```
c = (sensor output at sample reading) - (sensor output change per 1 &deg;C) * (sample reading).
```

Now that `c` is known, the `AnalogTemperature` sensor class can be used with any sensor exhibiting a linear response.

## API

### Constructor

#### `Calibration()`

Create a new `Calibration` object with the default settings for the `TMP35`, `LM35` or `TMP45` sensors.

#### `Calibration(int sampleReading, int millivoltsAtSampleReading, int millivoltsPerDegreeCentigrade)`

Create a new `Calibration` object with the specified settings.

### Properties

#### `public int SampleReading { get; private set; } = 25`

Sample temperature as specified in the sensor data sheet.  The default value is set to 25 &deg;C.

#### `public int MillivoltsAtSampleReading { get; private set; } = 250`

Sensor output (in millivolts) when the sensor is at the specified temperature (i.e. the `SampleReading`).  The default value is 250 millivolts.

#### `public int MillivoltsPerDegreeCentigrade { get; private set; } = 10`

Change in sensor output (in millivolts) for each 1 &deg;C change in temperature.