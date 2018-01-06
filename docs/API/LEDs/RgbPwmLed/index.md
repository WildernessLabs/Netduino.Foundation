---
layout: API
title: RgbPwmLed
subtitle: Pulse-Width-Modulation (PWM) powered RGB LED.
---

# Info

## Sourcing



## Example



### Code

```csharp
using System;
using Microsoft.SPOT;
using H = Microsoft.SPOT.Hardware;
using N = SecretLabs.NETMF.Hardware.Netduino;
using System.Threading;

namespace Netduino.Foundation.Core.Samples
{
    public class Program
    {
        public static void Main()
        {
            // create a new pwm controlled RGB LED on pins Red = 9, Green = 10, and Blue = 11.
            var rgbPwmLed = new Netduino.Foundation.LEDs.RgbPwmLed(N.PWMChannels.PWM_PIN_D9, N.PWMChannels.PWM_PIN_D10, N.PWMChannels.PWM_PIN_D11);

            // run forever
            while (true)
            {

                // loop through the entire hue spectrum (360 degrees)
                for (int i = 0; i < 360; i++)
                {
                    // set the color of the RGB
                    rgbPwmLed.SetHsvColor(i, 1, 1);

                    // for a fun, fast rotation through the hue spectrum:
                    //Thread.Sleep (1);
                    // for a gentle walk through the forest of colors;
                    Thread.Sleep(18);
                }
            }
        }
    }
}

```

### Circuit

![](RgbPwmLed_bb.svg)


# API

## Constructors

## Methods