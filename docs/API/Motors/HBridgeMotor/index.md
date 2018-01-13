---
layout: API
title: HBridgeMotor
subtitle: Generic h-bridge motor controller driver.
---

# Info

An [h-bridge](https://www.wikiwand.com/en/H_bridge) motor controller enables a control signal to drive a much larger load in either polarity, allowing the Netduino to drive DC motors in forward or reverse from an external power supply. Using [pulse-width-modulation (PWM)](http://developer.wildernesslabs.co/Netduino/Input_Output/Digital/PWM/) as the control signal extends this control by allowing not just forward or reverse, but variable speeds in either direction.

## Sourcing

This generic driver works with standard h-bridges ICs such as the Texas Instruments [L2N93E](https://octopart.com/search?q=L293NE) or [SN754410](https://octopart.com/search?q=SN754410) chips. However, it should also work with any [standard h-bridge controller](https://www.amazon.com/s/ref=nb_sb_noss_1?url=search-alias%3Daps&field-keywords=h+bridge).

# Example

The following example uses a dual h-bridge chip to control two motors. Both the L293NE and NS754410 are dual h-bridge chips that can drive two motors in both forward and reverse. This is a common configuration for wheeled robots. Forward and reverse motion is obtained by setting both the motors to have the same forward or reverse speed, and turning is accomplished by setting them to different speeds.

## Sample Circuit

![](DualMotorHBridge_bb.svg)

## Code

The following code creates an h-bridge controller with the PWM controllers on pins 3 and 5, and the enable pin on pin 4. It then sets the motor speed to 100% forward, stops the motor for half a second, and then sets the motor speed to 100% reverse. 

```csharp
using System.Threading;
using H = Microsoft.SPOT.Hardware;
using N = SecretLabs.NETMF.Hardware.Netduino;
using Netduino.Foundation.Motors;

namespace HBridgeMotor_Sample
{
    public class Program
    {
        public static void Main()
        {
            var motor1 = new HBridgeMotor(N.PWMChannels.PWM_PIN_D3, N.PWMChannels.PWM_PIN_D5, N.Pins.GPIO_PIN_D4);
            var motor2 = new HBridgeMotor(N.PWMChannels.PWM_PIN_D6, N.PWMChannels.PWM_PIN_D10, N.Pins.GPIO_PIN_D7);

            while (true)
            {
                // set the speed on both motors to 100% forward
                motor1.Speed = 1f;
                motor2.Speed = 1f;
                Thread.Sleep(1000);
                motor1.Speed = 0f;
                motor2.Speed = 0f;
                Thread.Sleep(500);
                // 100% reverse
                motor1.Speed = -1f;
                motor2.Speed = -1f;
                Thread.Sleep(1000);
            }
        }
    }
}
```

# API

## Properties

#### `float IsNeutral { get; set; }`

Whether or not the motor connected to the h-bridge is not enabled. If `IsNeutral` is `true`, then the motor controller is disabled and rotor spins freely. If `IsNeutral` is false, then the motor will be energized and the rotor will be "locked" into position. 

#### `public float MotorCalibrationMultiplier { get; set; } = 1;`



#### `float Speed { get; set; }`

The speed, represented by a value from `-1` to `1` of the motor. `-1` means 100% reverse, and `1` means 100% forward. 


## Constructors

#### `public HBridgeMotor(H.Cpu.PWMChannel a1Pin, H.Cpu.PWMChannel a2Pin, H.Cpu.Pin enablePin)`

Instantiates a new `HBridgeMotor` on the specified pins. 

##### Parameters

 * `a1Pin` - The PWM channel pin that creates the forward rotation signal.
 * `a2Pin` - The PWM channel pin that creates the reverse rotation signal.
 * `enablePin` - A digital IO pin that enables and disables the h-bridge, which controls `IsNeutral`.