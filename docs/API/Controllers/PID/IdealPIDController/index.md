---
layout: API
title: IdealPIDController
subtitle: Proportional, Integral, Derivative (PID) controller using the ideal (textbook) calculation.
---

# Info

Describes the required API for a [Proportional, Integral, Derivative (PID)](https://en.wikipedia.org/wiki/PID_controller) controller. 

For a detailed discussion and explanation of the PID algorithm, see the Wilderness Labs [PID Guide](http://developer.wildernesslabs.co/Hardware/Reference/Algorithms/Proportional_Integral_Derivative/).

# API

## Properties

#### float ProportionalComponent { get; set; }

The value to use when calculating the proportional corrective action.

#### float IntegralComponent { get; set; }

The value to use when calculating the integral corrective action.

#### float DerivativeComponent { get; set; }

The value to use when calculating the derivative corrective action.

#### float OutputMax { get; set; }

The maximum allowable control output value. The control output is clamped to this value after being calculated via the `CalculateControlOutput` method.

#### float OutputMin { get; set; }

The minimum allowable control output value. The control output is clamped to this value after being calculated via the `CalculateControlOutput` method.

#### bool OutputTuningInformation { get; set; }

Whether or not to print the calculation information to the output console in an comma-delimited form. Useful for  pasting into a spreadsheet to graph the system control  performance when tuning the PID controller corrective action gains.

Output format is:

```
[output descriptor],[target],[input],[proportional action],[integral action],[derivative action],[calculated control output]
```

#### float ActualInput { get; set; }

Represents the _Process Variable_ (PV), or the actual signal reading of the system in its current state. For example, when heating a cup of coffee to `75º`, if the temp sensor says the coffee is currently at `40º`, the `40º` is the actual input value.

#### float TargetInput { get; set; }

Represents the _set point_ (SP), or the reference target signal to achieve. For example, when heating a cup of coffee to `75º`, `75º` is the target input value.

## Methods

#### void ResetIntegrator()

Resets the integrator error history. 

#### float CalculateControlOutput()

Calculates the control output based on the difference (error) between the `ActualInput` and `TargetInput`, using the supplied `ProportionalComponent`, `IntegralComponent`, and `DerivativeComponent`.