---
layout: API
title: ILightSensor
subtitle: Interface describing light sensors
---

# Info

Minimum required definition for light sensors.

# API

## Events

#### `event SensorFloatEventHandler LightLevelChanged`

Raised when the pressure change crosses the specified `LightLevelChangeNotificationThreshold` value.

## Properties

#### `float LightLevel { get; }`

Returns the last updated luminosity value.

* **TODO** what is the default value type?

#### `float LightLevelChangeNotificationThreshold { get; set; }`

The minimum amount of luminosity change needed to raise the `LightLevelChanged` event.

* **TODO** What is the default value type?

