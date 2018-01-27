---
layout: Library
title: EulerAngles
subtitle: Euler Angle Reading
---

# Info

Euler angles describe the orientation of an object in 3D space with respect to a co-ordinate system.  Euler angles had the disadvantage that they suffer from gimbal lock as the angles approach + / - 90 &deg;.

# API

## Constructor

### `EulerAngles (double heading, double roll, double pitch)`

Create a new `EulerAngles` object with the specified `heading`, `roll` and `yaw`.

## Properties

### `double Heading`

Current heading reading from a sensor.

### `double Roll`

Current roll reading from a sensor.

### `double Pitch`

Current pitch reading from the sensor.