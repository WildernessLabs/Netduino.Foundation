---
layout: API
title: IdealPIDController
subtitle: Proportional, Integral, Derivative (PID) controller using the ideal (textbook) calculation.
---

# Info

PID is the quintessential industrial control algorithm. It is a mathematical tool for efficiently affecting change in a system to get it to a particular target state, and keeping it there, harmoniously.

It’s the algorithm that keeps drones balanced in the air, your car at the right speed when cruise control is on, and ships on the right heading with variable winds. It’s also the algorithm that can efficiently heat a cup of coffee to the perfect temperature and keep it there

`IdealPIDController` implements the _ideal_ or textbook standard version of the PID algorithm as defined by many textbooks and [Wikipedia](https://en.wikipedia.org/wiki/PID_controller):

![](Ideal_PID_Block_Diagram.svg)

Despite it's name, the "ideal" PID controller is actually less common and more specialized in its use than the [`StandardPIDController`](/API/Controllers/PID/StandardPIDController/), which is a slight modification on the ideal algorithm.

For a detailed discussion and explanation of the PID algorithm, see the Wilderness Labs [PID Guide](http://developer.wildernesslabs.co/Hardware/Reference/Algorithms/Proportional_Integral_Derivative/).

# API

## Properties

#### float ProportionalComponent { get; set; }

The _gain_ value to use when calculating the proportional corrective action. A good value to start with when tuning is `1.0`.

#### float IntegralComponent { get; set; }

The _gain_ value to use when calculating the integral corrective action. The integral correction is based on a time unit of error/seconds. A good value to start with when tuning is `0.1`.

#### float DerivativeComponent { get; set; }

The _gain_ value to use when calculating the derivative corrective action. Derivative should only be used when the `ActualInput` value has very little noise, as the derivative uses the slope of change to calculate the corrective action. A good value to start with when tuning is `0.001`.

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