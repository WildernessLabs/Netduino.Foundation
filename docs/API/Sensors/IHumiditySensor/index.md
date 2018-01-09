---
layout: API
title: IHumiditySensor
subtitle: Interface describing humidity sensors
---

# Info

Minimum required definition for humidity sensors.

# API

## Events

#### `event SensorFloatEventHandler HumidityChanged`

Raised when the humidity change crosses the specified `HumidityChangeNotificationThreshold` value.

## Properties

#### `float Humidity { get; }`

Returns the last updated humidity value.

* **TODO** what is the default value type? millibars?

#### `float HumidityChangeNotificationThreshold { get; set; }`

The minimum amount of humidity change needed to raise the `HumidityChanged` event.

* **TODO** What is the default value type?

