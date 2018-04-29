---
layout: Library
title: IServo
subtitle: Interface contract for fixed rotation servos.
---

# Info

`IServo` is the base interface for all servos that can rotate only a fixed amount. Fixed rotation servos are by far the most common servos.

# API

### Properties

#### `int Angle { get; }`

Gets the current angle. Returns `-1` if the angle is unknown.

#### `ServoConfig Config { get; }`

Gets the [`ServoConfig`](../ServoConfig) object describing the servo.

### Methods
    
#### `void RotateTo(int angle)`

Rotates the servo to a given angle.
        