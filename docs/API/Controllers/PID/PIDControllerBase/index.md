---
layout: API
title: PIDControllerBase
subtitle: Base class providing shared PID controller functionality.
---

# Info

The `PIDControllerBase` can be used as a base class when implementing a custom [Proportional, Integral, Derivative (PID)](https://en.wikipedia.org/wiki/PID_controller) controller, and contains a considerable amount of shared implementation that conforms to the [`IPIDController` Interface](/API/Controllers/PID/IPIDController). For example implementations that utilize the `PIDControllerBase` class, see the [`IdealPIDController`](https://github.com/WildernessLabs/Netduino.Foundation/blob/master/Source/Netduino.Foundation/Controllers/PID/IdealPidController.cs) or [`StandardPIDController`](https://github.com/WildernessLabs/Netduino.Foundation/blob/master/Source/Netduino.Foundation/Controllers/PID/StandardPidController.cs) implementations.

For a detailed discussion and explanation of the PID algorithm, see the Wilderness Labs [PID Guide](http://developer.wildernesslabs.co/Hardware/Reference/Algorithms/Proportional_Integral_Derivative/).

The `PIDControllerBase` implements a number of properties and methods that simplify the task of creating a PID controller with a custom algorithm. Simply inherit from `PIDControllerBase` and implement the `CalculateControlOutput` method with your custom algorithm.

The source for `PIDControllerBase` can be found [here](https://github.com/WildernessLabs/Netduino.Foundation/blob/master/Source/Netduino.Foundation/Controllers/PID/PidControllerBase.cs), and may be helpful in implementing controllers that derive from it.

# API

The API of `PIDControllerBase` conforms to [`IPIDController` Interface](/API/Controllers/PID/IPIDController), so only the API marked as `abstract` or `virtual`, and therefore up to the derivative implementation is documented here.

## Abstract and Virtual Methods

#### public abstract float CalculateControlOutput()

This method should be implemented by any derivative classes and should calculate the control output based on the difference (error) between the `ActualInput` and `TargetInput`, using the supplied `ProportionalComponent`, `IntegralComponent`, and `DerivativeComponent`.