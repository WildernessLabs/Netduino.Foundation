---
layout: API
title: SensorVectorArgs
subtitle: Event arguments for sensors using `Vector` values.
---

# Info

`SensorVectorEventArgs` provides additional information for events that are raised by sensors that use a `Vector` value to describe their data. This class is used in conjunction with the `SensorVectorEventHandler` to communicate the value from both the last raised event, and the current event.

# API

## Properties

#### `public Vector LastNotifiedValue { get; set; }`

The acceleration from the last time the event was raised.

#### `public Vector CurrentValue { get; set; }`

The current acceleration registered by the sensor.

## Constructors

#### `public SensorVectorEventArgs(Vector lastValue, Vector currentValue)`

Creates a new `SensorVectorEventArgs` object with the specified `lastValue` and `currentValue`.