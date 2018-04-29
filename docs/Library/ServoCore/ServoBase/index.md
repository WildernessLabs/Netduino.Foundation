---
layout: Library
title: ServoBase
subtitle: Base class for IServo implementations.
---

# Intro

`ServoBase` provides a base implementation for much of the common tasks of implementing [`IServo`](../IServo). However, for nearly all servos, a custom class is not necessary. Instead, the recommended path is to define a custom [`ServoConfig`](../ServoConfig) or use one of the pre-defined [`NamedServoConfigs`](../NamedServoConfigs).