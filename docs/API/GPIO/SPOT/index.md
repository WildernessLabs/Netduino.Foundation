---
layout: API
title: SPOT Namespace
subtitle: .NET MicroFramework compatibility classes.
---

# Info

In order to provide Unified GPIO Architecture interop with the .NET MicroFramework (NETMF) port classes, this namespace contains wrapper classes that expose the NETMF ports as `IPort` compatible classes.

These wrappers are used throughout Netduino.Foundation peripheral drivers so that they can use both the underlying .Net MicroFramework (NETMF) classes and the unified GPIO classes interchangeably. For example, consider the following code excerpts from the [`Relay`](/API/Relays/Relay) class:

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