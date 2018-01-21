---
layout: API
title: RotaryTurnedEventArgs
subtitle: Event arguments for rotary encoder rotational events.
---

# Info

`RotaryTurnedEventArgs` provides rotational information from a `RotaryTurned` event on an [`IRotaryEncoder`](/API/Sensors/Rotary/IRotaryEncoder) and describes whether the rotation is in the clockwise or counterclockwise direction.

# API

## Properties

#### `public RotationDirection Direction { get; set; }`

The direction of rotation.