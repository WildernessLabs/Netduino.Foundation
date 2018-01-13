---
layout: API
title: IDCMotor
subtitle: Interface describing DC motors.
---

# Info

Minimum required definition for DC motor classes.

# API

## Properties

#### `float IsNeutral { get; set; }`

Whether or not the motor connected to the h-bridge is not enabled. If `IsNeutral` is `true`, then the motor controller is disabled and rotor spins freely. If `IsNeutral` is false, then the motor will be energized and the rotor will be "locked" into position. 

#### `float Speed { get; set; }`

The speed, represented by a value from `-1` to `1` of the motor. `-1` means 100% reverse, and `1` means 100% forward. 