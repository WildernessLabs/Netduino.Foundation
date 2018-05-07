---
layout: Library
title: Servo Class
subtitle: Class for controlling standard fixed rotation servos.
---

# Info

The `Servo` class inherits from [`ServoBase`](../ServoBase) and therefore implements `IServo`. It's used to control standard fixed-rotation servos.

## Creating a new `Servo`

The `Servo` constructor takes two parameters; the `PWMChannel` pin that the servo control pin is hooked up to, and a [`ServoConfig`](../ServoConfig) describing the control parameters of the servo. 

In addition to manually instantiating a `ServoConfig` class, the [`NamedServoConfigs`](NamedServoConfigs) class contains a set of known servo configurations and can be used instead:

```csharp
IServo servo = new Servo(N.PWMChannels.PWM_PIN_D9, NamedServoConfigs.BlueBirdBMS120);
```

## Rotating the Servo via `RotateTo()`
 
To rotate a fixed-range servo, you simply call the `RotateTo` method and pass the angle. For instance, the following code rotates a servo to its maximum angle:

```csharp
servo.RotateTo(servo.Config.MaximumAngle);
```

## Test Circuit

To test the servo, the following circuit can be used.

![Servo Connected to Netduino](../ServoBreadboard.png)

Note that under load, a servo can use a lot of power, and therefore it should be powered via an external power source. See the [ServoCore guide](../) for more information.

## Example

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
