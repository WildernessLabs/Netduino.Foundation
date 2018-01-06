---
layout: GettingStarted
title: Getting Started
subtitle: Getting up and running with Netduino.Foundation
---

# Hello, World!

1. [Configure your Netduino development environment](http://developer.wildernesslabs.co/Netduino/Getting_Started/).
2. Create a new .NET MicroFramework console application.
3. Add a reference to the `Netduino.Foundation` Nuget package.
4. Plug the longer leg (cathode) of a green LED into pin `11` and the other leg into `GND`:
![](PwmLed_bb.svg)
5. Add the following code to the `program.cs` file in your application:
 
```csharp
using N = SecretLabs.NETMF.Hardware.Netduino;
using System.Threading;

namespace Netduino.Foundation.Core.Samples
{
    public class HelloWorldProgram
    {
        public static void Main()
        {
            // create a new LED on pin 11
            var pwmLed = new LEDs.PwmLed(N.PWMChannels.PWM_PIN_D11, 
                LEDs.TypicalForwardVoltage.Green);

            // pulse the LED
            pwmLed.StartPulse();

            // keep the app running
            while (true) { Thread.Sleep(1000); }

        }
    }
}
```

6. Deploy and run the program!