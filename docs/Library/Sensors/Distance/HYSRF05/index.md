---
layout: Library
title: HYSRF05
subtitle: Distance Sensor.
---

# HYSRF05

The HYSRF05 ultrasonic sensor uses sonar to determine distance to an object like bats do. It offers excellent non-contact range detection with high accuracy and stable readings in an easy-to-use package.

## Purchasing

The HYSRF05 sensor is available from Sparkfun:

* [Ultrasonic Sensor - HC-SR04](https://www.sparkfun.com/products/13959)

## Hardware

The HYSRF05 sensor has 5 connections, but you will only need to connect: power, ground, echo and trigger:

![HYSRF05 and Netduino](ParallaxPIROnBreadboard.png)

## Software

The following application creates a `HYSRF05` object, invokes `MeasureDistanceSensor` every second and attaches interrupt handlers to the `DistanceDetected` event that its triggered when the sensor picks up a rebound signal:

```csharp
using Microsoft.SPOT;
using System.Threading;
using Netduino.Foundation.Sensors.Distance;
using SecretLabs.NETMF.Hardware.Netduino;

namespace HYSRF05Test
{
    public class Program
    {
        public static void Main()
        {
            var  _HYSRF05 = new HYSRF05(Pins.GPIO_PIN_D12, Pins.GPIO_PIN_D11);
            _HYSRF05.DistanceDetected += OnDistanceDetected;

            while (true)
            {
                _HYSRF05.MeasureDistance();
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

#### `HYSRF05(Cpu.Pin triggerPin, Cpu.Pin echoPin)`

Create a new `HYSRF05` object with the sensor connected to the trigger `OutputPort` and echo `interruptPort`.

### Properties

#### `public float CurrentDistance { get; private set; }`

Returns the distance after calling MeasureDistance method

### Methods

#### `public void MeasureDistance()`

Makes the HYSRF05 sensor to send a ultrasonic distance, and if detects a rebound signal, it will fire the `DistanceDetected` event.

### Events

#### `public event DistanceDetectedEventHandler DistanceDetected`

This is raised when the sensor picks up an obstacle within its maximum distance range.