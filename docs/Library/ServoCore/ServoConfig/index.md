---
layout: Library
title: ServoConfig
subtitle: Configuration class describing the parameters of standard servos.
---

# Info

The `ServoConfig` class is used to describe the operating parameters of any given servo. It is most often used when instantiating a new [`Servo`](../Servo) object that can control a servo with that particular configuration.

There are a number of known servo configurations in the [`NamedServoConfigs`](../NamedServoConfigs) class.

# API

### Properties

#### `public int MinimumAngle { get; private set; } = 0`

The minimum angle of rotation that a servo has. Almost always `0º`.


#### `public int MaximumAngle { get; private set; } = 180`

The maximum angle of rotation that a servo has. Most commonly `180º`, though servos exist that have maximum angles of `160º`, `270º`, and others.

If the servo allows continuous rotation, `-1` should be used.

#### `public int MinimumPulseDuration { get; private set; } = 1000`

The minimum pulse duration (in µ seconds) that the servo responds to. The minimum pulse duration is used to set the angle of the servo to its minimum angle.

The "ideal" standard is `1,000`, however, many servos have a lower minimum pulse. Refer to your servos documentation to find this value.

#### `public int MaximumPulseDuration { get; private set; } = 2000`

The maximum pulse duration (in µ seconds) that the servo responds to. The maximum pulse duration is used to set the angle of the servo to its maximum angle.

The "ideal" standard is `2,000`, however, many servos have a higher maximum pulse. Refer to your servos documentation to find this value.

#### `public int Frequency { get; private set; } = 50`

The frequency, in hz, that the servo uses. Almost always `50hz`.