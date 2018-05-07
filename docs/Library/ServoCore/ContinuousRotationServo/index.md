---
layout: Library
title: ContinuousRotationServo Class
subtitle: Class for controlling continuous rotation servos.
---

# Info

The `ContinuousRotationServo` is used to control continuous rotation servos.

## Creating a new `ContinuousRotationServo`

The `ContinuousRotationServo ` constructor takes two parameters; the `PWMChannel` pin that the servo control pin is hooked up to, and a [`ServoConfig`](../ServoConfig) describing the control parameters of the servo. 

In addition to manually instantiating a `ServoConfig` class, the [`NamedServoConfigs`](NamedServoConfigs) class contains a set of known servo configurations and can be used instead:

```csharp
ContinuousRotationServo servo = new ContinuousRotationServo(N.PWMChannels.PWM_PIN_D9, 
                           NamedServoConfigs.IdealContinuousRotationServo);
```

## Rotating the Servo via `RotateTo()`
 
A continuous rotation servo's direction and speed can be set via the `Rotate` method. The following example rotates a continuous rotation servo in the clockwise direction at `50%` speed:

```csharp
servo.Rotate(Direction.Clockwise, 0.5f);
```

## Test Circuit

To test the servo, the following circuit can be used.

![Servo Connected to Netduino](../ServoBreadboard.png)

Note that under load, a servo can use a lot of power, and therefore it should be powered via an external power source. See the [ServoCore guide](../) for more information.

## Example

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
