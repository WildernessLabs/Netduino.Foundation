---
layout: API
title: ITemperatureSensor
subtitle: Interface describing temperature sensors
---

# Info

Minimum required definition for temperature sensors.

# API

## Events

#### `event SensorFloatEventHandler TemperatureChanged`

Raised when the temperature change crosses the specified `TemperatureChangeNotificationThreshold` value.

## Properties

#### `float Temperature { get; }`

Returns the last updated temperature value in degrees Celsius (ºC).

#### `float TemperatureChangeNotificationThreshold { get; set; }`

The minimum amount of temperature change needed to raise the `TemperatureChanged` event. Specified in degrees Celsius (ºC).

