---
layout: Library
title: HCSR04
subtitle: Distance Sensor.
---

# HCSR04

The HC-SR04 ultrasonic sensor uses sonar to determine distance to an object like bats do. It offers excellent non-contact range detection with high accuracy and stable readings in an easy-to-use package.

## Purchasing

The HCSR04 sensor is available from Sparkfun:

* [Ultrasonic Sensor - HC-SR04](https://www.sparkfun.com/products/13959)

## Hardware

The HCSR04 sensor requires only four connections: power, ground, echo and trigger:

![HCSR04 and Netduino](ParallaxPIROnBreadboard.png)

## Software

The following application creates a `HCSR04` object, invokes `MeasureDistanceSensor` every second and attaches interrupt handlers to the `DistanceDetected` event that its triggered when the sensor picks up a rebound signal:

```csharp
using Microsoft.SPOT;
using System.Threading;
using Netduino.Foundation.Sensors.Distance;
using SecretLabs.NETMF.Hardware.Netduino;

namespace HCSR04Test
{
    public class Program
    {
        public static void Main()
        {
            var  _HCSR04 = new HCSR04(Pins.GPIO_PIN_D12, Pins.GPIO_PIN_D11);
            _HCSR04.DistanceDetected += OnDistanceDetected;

            while (true)
            {
                _HCSR04.MeasureDistance();
                Thread.Sleep(1000);
            }
        }

        private static void OnDistanceDetected(object sender, DistanceEventArgs e) 
        {
            Debug.Print(e.Distance.ToString());
        }
    }
}
```

## API

### Constructors

#### `HCSR04(Cpu.Pin triggerPin, Cpu.Pin echoPin)`

Create a new `HCSR04` object with the sensor connected to the trigger `OutputPort` and echo `interruptPort`.

### Properties

#### `public float CurrentDistance { get; private set; }`

Returns the distance after calling MeasureDistance method

### Methods

#### `public void MeasureDistance()`

Makes the HCSR04 sensor to send a ultrasonic distance, and if detects a rebound signal, it will fire the `DistanceDetected` event.

### Events

#### `public event DistanceDetectedEventHandler DistanceDetected`

This is raised when the sensor picks up an obstacle within its maximum distance range.