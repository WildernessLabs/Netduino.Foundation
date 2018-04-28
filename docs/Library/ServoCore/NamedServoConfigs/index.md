---
layout: Library
title: NamedServoConfigs
subtitle: A set of known servo configurations.
---

# Info

The operating parameters of a servo is described by the [`ServoConfig`](../ServoConfig) class. The `NamedServoConfigs` contains a number of these configurations for known servos.

## Example

When instantiating a new [`Servo`](../Servo) or [`ContinuousRotationServo`](../ContinuousRotationServo), the constructor takes a `ServoConfig`. However, instead of manually creating a new configuration, a known configuration can be used from the `NamedServoConfigs` class.

For example, the following code instantiates a new `Servo` class that can command a `BlueBirdBMS120` servo:

```csharp
IServo servo = new Servo(N.PWMChannels.PWM_PIN_D9, NamedServoConfigs.BlueBirdBMS120);
```

Named configs can also be used with a `ContinuousRotationServo` as well:

```csharp
ContinuousRotationServo servo = new ContinuousRotationServo(N.PWMChannels.PWM_PIN_D9, NamedServoConfigs.IdealContinuousRotationServo);
```

# Known Servo Configurations

| Configuration Name | Minimum Angle | Maximum Angle | Minimum Pulse Width | Maximum Pulse Width |
|--------------------|---------------|---------------|---------------------|---------------------|
| `Ideal180Servo`    | `0º`          | `180º`        | `1,000µs`           | `2,000µs`           |
| `Ideal270Servo`    | `0º`          | `270º`        | `1,000µs`           | `2,000µs`           |
| `IdealContinuousRotationServo` | `-1` | `-1`       | `1,000µs`           | `2,000µs`           |
| `BlueBirdBMS120`   | `0º`          | `120º`        | `900µs`             | `2,100µs`           |
| `HiTecStandard`    | `0º`          | `180º`        | `900µs`             | `2,100µs`           |

# Open-Source!

This class, as with all of Netduino.Foundation, is open-source, so please feel free to send pull requests with more known configurations!
