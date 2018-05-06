---
layout: API
title: Led
subtitle: Single color LED.
---

# Info



# Example

The following example shows how to turn on and off the LED using the `IsOn` property, and uses a `StartBlink(onDuration, offDuration)` method to make the LED blink staying on for 500ms (0.5s) and off for 1000ms (1s):

## Code

```csharp
using System.Threading;
using N = SecretLabs.NETMF.Hardware.Netduino;

namespace LedSample
{
    public class Program
    {
        public static void Main()
        {
            // create a new pwm controlled LED on pin 8
            var led = new Netduino.Foundation.LEDs.Led(N.Pins.GPIO_PIN_D8);

            while(true)
            {
                led.IsOn = true;    // Led ON
                Thread.Sleep(3000); // 3 seconds
                led.IsOn = false;   // Led OFF
                Thread.Sleep(2000); // 2 seconds
                led.IsOn = true;    // Led ON
                Thread.Sleep(1000); // 1 second

                led.StartBlink(500, 1000);
                Thread.Sleep(5000); // 5 seconds
                led.Stop();
            }
        }
    }
}
```

## Circuit

![](PwmLed_bb.svg)

# API Reference

## Constructors

#### `public Led(Microsoft.SPOT.Hardware.Cpu.Pin pin)`

Creates a new Led on the specified pin connected directly to a Netduino.

#### `public Led(IDigitalOutputPort port)`

Creates a new Led on the specified pin connected through an IOExpander such as a MCP23008 or a 74HC595 shift register.

## Properties

#### `public bool IsOn { get; protected set; }`

Returns the value of the LED of whether its on or not.

#### `public IDigitalOutputPort DigitalOut { get; protected set; }`

Returns the value of the Led port its connected to.

## Methods

#### `public void StartBlink(uint onDuration = 200, uint offDuration = 200)`

Start the Blink animation which sets the brightness of the LED alternating off and on, using the durations provided.

#### `public void Stop()`

Stops the LED when its blinking and/or turns it off.