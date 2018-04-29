---
layout: Library
title: Servo Core
subtitle: Servo command and control framework.
---

# Info

The Netduino.Foundation ServoCore framework simplifies command and control of _servomotors_ (more commonly known as _servos_).

Servos are integrated packages that usually include a DC electric motor, torque-increasing gearing, and electronics to control the motor: 

![](Servos_Medium.jpg)

The gained their popularity as an important part of remote controlled cars, airplanes, and such, but are now also very common in robotics.

Unlike simple electric motors, they have three wires, with two wires supplying power, and the third wire accepting a [PWM](http://developer.wildernesslabs.co/Netduino/Input_Output/Digital/PWM/) control signal.

## Types of Servos

There are two types of servos; _fixed-angle_ servos, and _continuous rotation_ servos. 

### Fixed-Angle Servos

Fixed angle servos are by far the most common type of servo, and usually what people mean when they use the term servo. They can only move within a given range, most commonly between 0º and 180º. The integrated electronic control system in a servo allows them to be controlled very precisely, so that you can set them to turn to a specific angle. 

### Continuous Rotation Servos

Continuous rotation servos are sometimes known as _winch_ servos, because they were originally used as winch controls on remote control sailboats. They don't have any restriction on their angle of movement, but instead will rotate in either clockwise or counter-clockwise direction at a set speed.

## Control Signal

Both fixed-angle and continuous rotation servos are controlled with a PWM signal, and the duration of the duty cycle pulse determines their behavior. 

Every servo has a _minimum pulse duration_ and a _maximum pulse duration_ that they respond to. A control signal outside of this range will usually be ignored by the servo. The ideal standard is a minimum of `1.0ms` and a max of `2.0ms`, but in practicality, many servos have a much wider range of operation. Additionally, because the pulses vary between fractional milliseconds, µseconds (1,000 milliseconds) are usually used as the unit. So a `1.0ms` pulse is `1,000µs`.

### Fixed Angle Control

For fixed-angle servos, the minimum pulse duration will cause them to rotate to their minimum angle (nominally 0º), and the maximum pulse duration will rotate them to their maximum angle. So for instance, the following table describes the rotation of a `180º` servo that has a pulse range of `1,000µs` to `2,000µs`:

| Control Pulse | Angle   |
|---------------|---------|
| `1,000µs`     | `0º`    |
| `1,500µs`     | `90º`   |
| `2,000µs`     | `1800º` |

### Continuous Rotation Control

Continuous rotation servos still have a minimum and maximum pulse durations, but they work a little differently. At the midpoint between those extremes, a control signal will actually stop the servo, and anything less than that midpoint will cause it to rotate clockwise, and anything more than that midpoint will cause it to rotate counter-clockwise. And the distance from that midpoint controls the speed of rotation. For example, the following table describes rotation and speed of various control signals:

| Pulse Duration | Rotation Direction | Speed |
|----------------|--------------------|-------|
| `1,000µs`      | Clockwise          | 100%  |
| `1,250µs`      | Clockwise          | 50%   |
| `1,500µs`      | None               | n/a   |
| `1,750µs`      | Counter-Clockwise  | 50%   |
| `2,000µs`      | Counter-Clockwise  | 100%  |

## ServoCore API

The ServoCore API hides all this complexity and exposes easy to use methods that calculate the appropriate pulse signals based on the specified configuration of any given servo. The servo configuration is passed to the `Servo` and `ContinuousRotationServo` class during construction.

### Fixed Range `RotateTo()`
 
To rotate a fixed-range servo, you simply call the `RotateTo` method and pass the angle. For instance, the following code rotates a servo to its maximum angle:

```csharp
servo.RotateTo(servo.Config.MaximumAngle);
```
### Continuous `Rotate()`

Similarily, a continuous rotation servo's direction and speed can be set via the `Rotate` method. The following example rotates a continuous rotation servo in the clockwise direction at `50%` speed:

```csharp
servo.Rotate(Direction.Clockwise, 0.5f);
```

### Known Servo Configurations

The [`NamedServoConfigs`](NamedServoConfigs) class contains a set of known servo configurations that can be used:

```csharp
IServo servo = new Servo(N.PWMChannels.PWM_PIN_D9, NamedServoConfigs.BlueBirdBMS120);
```

This class, as with all of Netduino.Foundation, is open-source, so please feel free to send pull requests with more known configurations!

## Powering Servos

Servos can draw a lot of current, especially under load. Additionally, most common hobby servos require `6V`. For this reason, an external power supply should be used when they're used in practical applications.

For just testing however, they can be powered via the `5V` rail on the netduino as follows:

### Testing Circuit

![Servo connected to Netduino for testing](Servo_Testing_bb.svg)

### External Power Circuit

When powering with an external power source, you must connect the external `GND` to the `GND` rail on the Netduino, or the PWM control signal will not work:

![Servo connected to Netduino and external power supply](Servo_bb.svg)

In the above illustration, (4), AA batteries are used, but we usually use a power supply like the following:

![](PowerSupply_Medium.jpg)

The female plug adapter shown above can be found on [Amazon](https://amzn.to/2r7o20B).


## Cable Colors

Servo cable colors vary by manufacture, but they're always in the same order. The `GND` pin is the first pin by convention is always the darkest:

| Pin | Signal  | Futaba Colors | HiTec Colors    | JR Colors |
|-----|---------|---------------|-----------------|-----------|
| 1   | GND     | Black         | Black           | Brown     |
| 2   | VCC     | Red           | Red or Brown    | Red       |
| 3   | Control | White         | Yellow or White | Orange    |

## Code Examples

The following examples illustrate how to use the ServoCore library with fixed rotation and continuous rotation servos.

### Standard Fixed Rotation Servo

This sample app will toggle the servo between it's minimum and maximum rotation angles when the button on the Netduino is clicked.

```csharp
using Microsoft.SPOT;
using Netduino.Foundation;
using Netduino.Foundation.Sensors.Buttons;
using Netduino.Foundation.Servos;
using System.Threading;
using H = Microsoft.SPOT.Hardware;
using N = SecretLabs.NETMF.Hardware.Netduino;


namespace ServoSample
{
    public class Program
    {
        static IServo _servo = null;
        static PushButton _button = null;

        public static void Main()
        {
            _servo = new Servo(N.PWMChannels.PWM_PIN_D9, 
                               NamedServoConfigs.BlueBirdBMS120);
            _button = new PushButton((H.Cpu.Pin)0x15, 
                                      CircuitTerminationType.Floating);

            _button.Clicked += (object sender, Microsoft.SPOT.EventArgs e) =>
            {
                Debug.Print("Button Clicked");
                ToggleServo();
            };

            Thread.Sleep(Timeout.Infinite);
        }

        static void ToggleServo()
        {
            if (_servo.Angle == _servo.Config.MinimumAngle)
            {
                Debug.Print("Rotating to " + 
                            _servo.Config.MaximumAngle.ToString());
                _servo.RotateTo(_servo.Config.MaximumAngle);
            }
            else
            {
                Debug.Print("Rotating to " + 
                            _servo.Config.MinimumAngle.ToString());
                _servo.RotateTo(_servo.Config.MinimumAngle);
            }
        }
    }
}
```

### Continuous Rotation Servo

This sample app will rotate a continuous rotation servo clockwise and counter-clockwise at varying speeds when the Netduino button is clicked. Clicking the button again will stop the servo:

```csharp
using Microsoft.SPOT;
using Netduino.Foundation;
using Netduino.Foundation.Sensors.Buttons;
using Netduino.Foundation.Servos;
using System.Threading;
using H = Microsoft.SPOT.Hardware;
using N = SecretLabs.NETMF.Hardware.Netduino;


namespace ContinuousRotationServoSample

{
    public class Program
    {
        static ContinuousRotationServo _servo = null;
        static PushButton _button = null;
        static bool _running = false;

        public static void Main()
        {
            _servo = new ContinuousRotationServo(N.PWMChannels.PWM_PIN_D9, 
                           NamedServoConfigs.IdealContinuousRotationServo);
            _button = new PushButton((H.Cpu.Pin)0x15, 
                                      CircuitTerminationType.Floating);

            _button.Clicked += (object sender, Microsoft.SPOT.EventArgs e) =>
            {
                Debug.Print("Button Clicked");
                ToggleServo();
            };

            Thread.Sleep(Timeout.Infinite);
        }

        static void ToggleServo()
        {
            if (!_running)
            {
                Thread th = new Thread(() => {
                    _running = true;
                    while (_running)
                    {
                        Debug.Print("Rotating clockwise.");
                        for (float speed = 1; speed <= 10; speed++)
                        {
                            if (!_running) break;
                            _servo.Rotate(Direction.Clockwise, 
                                          (speed / 10.0f));
                            Thread.Sleep(500);
                        }

                        if (!_running) break;
                        Debug.Print("Stopping for half a sec.");
                        _servo.Stop();
                        Thread.Sleep(500);

                        Debug.Print("Rotating counter-clockwise");
                        for (float speed = 1; speed <= 10; speed++)
                        {
                            if (!_running) break;
                            _servo.Rotate(Direction.CounterClockwise, 
                                          (speed / 10.0f));
                            Thread.Sleep(500);
                        }
                    }
                });
                th.Start();
            } else {
                Debug.Print("Stopping.");
                _running = false;
                Thread.Sleep(550); // wait for the loop to break
                _servo.Stop();
            }
        }
    }
}
```

# API

## Interfaces

* **[`IServo`](/Library/ServoCore/IServo)** - The base interface for all fixed rotation servos.
* **[`IContinuousRotationServo`](/Library/ServoCore/IContinuousRotationServo)** - The base class for all continuous rotation servos.

## Base Classes

* **[`ServoBase`](/Library/ServoCore/ServoBase)** - Base class for `IServo` implementations.
* **[`ContinuousRotationServoBase`](/Library/ServoCore/ContinuousRotationServoBase)** - Base class for `IContinuousRotationServo` implementations.

## Classes

* **[`Servo`](/Library/ServoCore/Servo)** - Class for controlling standard fixed rotation servos.
* **[`ContinuousRotationServo`](/Library/ServoCore/ContinuousRotationServo)** - Class for controlling standard continuous rotation servos.
* **[`ServoConfig`](/Library/ServoCore/ServoConfig`)** - Configuration class describing the parameters of standard servos.
* **[`NamedServoConfigs`](/Library/ServoCore/NamedServoConfigs)** - A set of known servo configurations.

