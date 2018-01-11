---
layout: API
title: IPressureSensor
subtitle: Interface describing pressure (barometric) sensors
---

# Info

Minimum required definition for pressure sensors.

# API

## Events

#### `event SensorFloatEventHandler PressureChanged`

Raised when the pressure change crosses the specified `PressureChangeNotificationThreshold` value.

## Properties

#### `float Pressure { get; }`

Returns the last updated pressure value.

* **TODO** what is the default value type? millibars?

#### `float PressureChangeNotificationThreshold { get; set; }`

The minimum amount of pressure change needed to raise the `PressureChanged` event.

* **TODO** What is the default value type?

