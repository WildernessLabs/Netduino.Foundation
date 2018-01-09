---
layout: API
title: SensorFloatArgs
subtitle: Event arguments for sensors using `float` values.
---

# Info

`SensorFloatArgs` provides additional information for events that are raised by sensors that use a `float` value to describe their data. This class is used in conjunction with the `SensorFloatEventHandler ` to communicate the value from both the last raised event, and the current event.

# API

## Properties

#### `public float LastNotifiedValue { get; set; }`

The value from the last time the event was raised.

#### `public float CurrentValue { get; set; }`

The current value of the event.

## Constructors

#### `public SensorFloatEventArgs (float lastValue, float currentValue)`

Creates a new `SensorFloatEventArgs` object with the specified `lastValue` and `currentValue`.


