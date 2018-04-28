---
layout: Library
title: Servo Core
subtitle: Library for controlling generic servo motors based on the Arduino servo library.
---

## Purchasing

This example uses a generic servo, these are available from a number of suppliers including [Adafruit](www.adafruit.com) and [Sparkfun](www.sparkfun.com).

* [Servo Motor](https://www.sparkfun.com/categories/245)

## Hardware

![Servo Connected to Netduino](ServoBreadboard.png)

Note the presence of a current limiting resistor in the above circuit.

## Software

The following application sweeps the servo from 0 to 180 degrees and then back again.  This is repeated continuously:

```csharp
using System.Threading;
using SecretLabs.NETMF.Hardware.NetduinoPlus;
using Netduino.Foundation.Servos;

namespace ServoTest
{
    public class Program
    {
        public static void Main()
        {
            Servo servo = new Servo(PWMChannels.PWM_PIN_D9, 1000, 2000);
            while (true)
            {
                for (int angle = 0; angle <= 180; angle++)
                {
                    servo.Angle = angle;
                    Thread.Sleep(40);
                }
                for (int angle = 179; angle > 0; angle--)
                {
                    servo.Angle = angle;
                    Thread.Sleep(40);
                }
            }
         }
    }
}
```

## API

### Properties

#### `Angle`

Get or set the servo angle where the angle is specified in degrees and should be between 0 and 180 inclusive.

#### `Attached`

Indicate if the servo has a `PWM` pin associated with it.

### Constructors

#### `Servo(Cpu.PWMChannel pin, int minimum = 544, int maximum = 2400)`

The default constructor creates a new 'Servo' object with the `MinimumPulseWidth` set to 544 and the `MaximumPulseWidth` set to 2400.

In this case the `Servo` object is not connected to a `PWM` pin.

```csharp
Servo(pin, minimum, maximum)
```

Create a new `Servo` object and connect it to the specified `PWM` pin.  The `MinimumPulseWidth` and `MaximumPulseWidth` are set to the specified values.

### Methods

#### `void Attach(pin, minimum, maximum)`

Attach the `Servo` object to a `PWM` pin

#### `void Write(angle)`

This is equivalent to setting the `Angle` property.

#### `double Read()`

This is the equivalent to reading the `Angle` property.

#### `void Detach()`

If the `Servo` is attached to a pin then this method will stop the `PWM` pulses and release the `PWM` pin being used to control the servo.

#### `void Stop()`

Turn off the `PWM` pulses being used to control the servo.

#### `void WriteMicroseconds(microseconds)`

This method is not implemented and will throw an exception if called.