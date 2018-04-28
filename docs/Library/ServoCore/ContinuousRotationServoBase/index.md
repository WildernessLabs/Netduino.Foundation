---
layout: Library
title: ContinuousRotationServoBase
subtitle: Base class for IContinuousRotationServo implementations.
---

# Intro

`ContinuousRotationServoBase` provides a base implementation for much of the common tasks of implementing [`IContinuousRotationServo`](../IContinuousRotationServo). However, for nearly all servos, a custom class is not necessary. Instead, the recommended path is to define a custom [`ServoConfig`](../ServoConfig) or use one of the pre-defined [`NamedServoConfigs`](../NamedServoConfigs).