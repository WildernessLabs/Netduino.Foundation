---
layout: API
title: StandardPIDController
subtitle: Proportional, Integral, Derivative (PID) controller using the a common calculation.
---

# Info

PID is the quintessential industrial control algorithm. It is a mathematical tool for efficiently affecting change in a system to get it to a particular target state, and keeping it there, harmoniously.

It’s the algorithm that keeps drones balanced in the air, your car at the right speed when cruise control is on, and ships on the right heading with variable winds. It’s also the algorithm that can efficiently heat a cup of coffee to the perfect temperature and keep it there.

`StandardPIDController` implements the most common version of the PID algorithm, which tends to be more generally applicable than the _ideal_ calculation. The standard version of PID is discussed on the [Wikipedia PID article](https://en.wikipedia.org/wiki/PID_controller#Ideal_versus_standard_PID_form). 

The following block diagram describes the calculation using the standard algorithm:

![](Standard_PID_Block_Diagram.svg)

In the standard form, step 3 produces a single error value that uses both the integral to account for past errors and the derivative which can predict future error based on rate of change. This single error correction is then scaled (multiplied) by the proportional error constant.

For a detailed discussion and explanation of the PID algorithm, see the Wilderness Labs [PID Guide](http://developer.wildernesslabs.co/Hardware/Reference/Algorithms/Proportional_Integral_Derivative/).

# API

## Properties

#### float ProportionalComponent { get; set; }

The _gain_ value to use when calculating the proportional corrective action. A good value to start with when tuning is `1.0`.

#### float IntegralComponent { get; set; }

In the standard PID algorithm, the derivative gain is specified in [`repeats per minute`](http://developer.wildernesslabs.co/Hardware/Reference/Algorithms/Proportional_Integral_Derivative/#integral-and-derivative-gain-components-in-relation-to-time). A good value to start with when tuning is `0.1`.

#### float DerivativeComponent { get; set; }

In the standard PID algorithm, the derivative gain is specified in [`repeats per minute`](http://developer.wildernesslabs.co/Hardware/Reference/Algorithms/Proportional_Integral_Derivative/#integral-and-derivative-gain-components-in-relation-to-time). As with the ideal form, the derivative should only be used when the `ActualInput` value has very little noise, as the derivative uses the slope of change to calculate the corrective action. A good value to start with when tuning is `0.001`.

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


# Example

The [Food Dehydrator 3000](https://github.com/WildernessLabs/Netduino_Samples/tree/master/Netduino.Foundation/FoodDehydrator3000) sample app illustrates basic standard PID controller usage. Specifically, the [`DehydratorController`](https://github.com/WildernessLabs/Netduino_Samples/blob/master/Netduino.Foundation/FoodDehydrator3000/FoodDehydrator3000/DehydratorController.cs) class utilizes Netduino.Foundation's `StandardPIDController` to bring the dehydrator up to a specified temperature and uses an [Analog Temperature Sensor](/API/Sensors/Temperature/Analog/) to provide feedback.

Additionally, the dehydrator uses a [PWM Signal](http://developer.wildernesslabs.co/Netduino/Input_Output/Digital/PWM/) to modulate the power of the heater element. 

The salient usage is described below.

#### `DehydratorController` Constructor:

In the constructor, the PID controller is instantiated and configured. In the case of the dehydrator, only the `ProportionalComponent` and `IntegralComponent` are used to calculate the control output (`DerivativeComponent` is set to `0`, effectively removing it from the control). This is because the derivative calculation is based on the rate of change, and it requires a very smooth sensor reading, but the temp sensor reading is fairly noisy. However, it doesn't matter, as this still provides a very efficient control. 

Additionally, the control output is clamped via the `OutputMin` and `OutputMax` properties between `0.0` and `1.0`, which translates to `0%` to `100%` duty cycle of the PWM that controls the heater element. If the controller were used in an system that kept a boat on a heading, or a car between lines, then the clamp might be between something like `-0.5` and `0.5`, in which a negative value meant a left heading, and a positive value meant a right heading.

```csharp
public DehydratorController(AnalogTemperature tempSensor, SoftPwm heater, Relay fan, SerialLCD display)
{
    _tempSensor = tempSensor;
    _heaterRelayPwm = heater;
    _fanRelay = fan;
    _display = display;

    _pidController = new StandardPidController();
    _pidController.ProportionalComponent = .5f; // proportional
    _pidController.IntegralComponent = .55f; // integral time minutes
    _pidController.DerivativeComponent = 0f; // derivative time in minutes
    _pidController.OutputMin = 0.0f; // 0% power minimum
    _pidController.OutputMax = 1.0f; // 100% power max
    _pidController.OutputTuningInformation = true;

}
```

#### `TurnOn` Method

When the dehydrator is turned on, the `TurnOn` method is called, which sets the temperature and running time, and then calls the `StartRegulatingTemperatureThread` method which is responsible for the bulk of the control work.

```csharp
public void TurnOn(int temp, TimeSpan runningTime)
{
    // set our state vars
    TargetTemperature = (float)temp;
    this._runningTimeLeft = runningTime;
    this._running = true;

    // keeping fan off, to get temp to rise.
    this._fanRelay.IsOn = true;
            
    // TEMP - to be replaced with PID stuff
    this._heaterRelayPwm.Frequency = 1.0f / 5.0f; // 5 seconds to start (later we can slow down)
    // on start, if we're under temp, turn on the heat to start.
    float duty = (_tempSensor.Temperature < TargetTemperature) ? 1.0f : 0.0f;
    this._heaterRelayPwm.DutyCycle = duty;
    this._heaterRelayPwm.Start();

    // start our temp regulation thread. might want to change this to notify.
    StartRegulatingTemperatureThread();
}
```

#### `StartRegulatingTemperatureThread` Method

This method starts a new thread which is actually responsible for reading the temperature input from the sensor and calling the PID controller's `CalculateControlOutput` method to determine the amount of power to provide to the heating element in order to bring the dehydrator up to the target temperature:

```csharp
protected void StartRegulatingTemperatureThread()
{
    _tempControlThread = new Thread(() => {

        // reset our integral history
        _pidController.ResetIntegrator();

        while (this._running) {

            // set our input and target on the PID calculator
            _pidController.ActualInput = _tempSensor.Temperature;
            _pidController.TargetInput = this.TargetTemperature;

            // get the appropriate power level 
            var powerLevel = _pidController.CalculateControlOutput();

            // set our PWM appropriately
            this._heaterRelayPwm.DutyCycle = powerLevel;

            // sleep for a while. 
            Thread.Sleep(_powerUpdateInterval);
        }
    });
    _tempControlThread.Start();
}
```