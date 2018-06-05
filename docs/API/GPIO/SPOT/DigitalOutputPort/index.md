---
layout: API
title: DigitalOutputPort Class
subtitle: Digital output port wrapper for Microsoft.SPOT compatibility.
---

# Info

Wrapper class that exposes a `Microsoft.SPOT.DigitalOutput` object as an [`IDigitalOutputPort`](/API/GPIO/IDigitalOutputPort). 

Used in peripheral drivers to allow them to be constructed with either an explicit `IDigitalOutputPort` or `Microsoft.SPOT.Hardware.Cpu.Pin` interchangeably, as in the following example code taken from the [`Relay`](/API/Relays/Relay) driver class:

```csharp
public IDigitalOutputPort DigitalOut { get; protected set; }
...

public Relay(IDigitalOutputPort port, RelayType type = RelayType.NormallyOpen)
{
    // if it's normally closed, we have to invert the "on" value
    this.Type = type;
    if (this.Type == RelayType.NormallyClosed)
    {
        _onValue = false;
    }

    DigitalOut = port;
}

public Relay(H.Cpu.Pin pin, RelayType type = RelayType.NormallyOpen)
{
    // if it's normally closed, we have to invert the "on" value
    this.Type = type;
    if (this.Type == RelayType.NormallyClosed) {
        _onValue = false;
    }

    // create a digital output port shim
    DigitalOut = new GPIO.SPOT.DigitalOutputPort(pin, !_onValue);
}
```

The second constructor allows the creation of a `Relay` object from a `Microsoft.SPOT.Hardware.Cpu.Pin`, but internally creates a `DigitalOutputPort` wrapper that conforms to the `IDigitalOutputPort` interface, so that only a reference to that `IDigitalOutputPort` is used, regardless if the `Relay` is instantiated with a pin, or an `IDigitalOutputPort` directly.