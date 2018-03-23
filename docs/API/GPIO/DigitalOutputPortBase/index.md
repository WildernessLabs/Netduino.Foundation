---
layout: API
title: DigitalOutputPortBase
subtitle: Base class for IDigitalOutputPort implementations.
---

# Intro

`DigitalOutputPortBase` inherits from [`DigitalPortBase`](/API/GPIO/DigitalPortBase) and provides a base implementation for much of the common tasks of classes implementing [`IDigitalPort`](/API/GPIO/IDigitalPort).

The intent of `IDigitalOutputPort` is to provide an a seamless way to extend peripherals that require digital output ports such as relays, LEDs, and others, across IO expansion chips such as the MCP23x and the 74x595 family. 

## Required APIs for Implementations

Note that in addition to the requirements listed here, implementors should follow the requirements for the [`DigitalPortBase`](/API/GPIO/DigitalPortBase) base class.


## Sample Implementation

The following code comes from the [`DigitalOutputPort`](/Library/ICs/MCP23x/DigitalOutputPort) class in the MCP23x peripheral library, and exposes pins on an MCP23x chip as `IDigitalOutputPort` implementations:

```csharp
using System;
using Microsoft.SPOT;
using Netduino.Foundation.GPIO;

namespace Netduino.Foundation.ICs.MCP23008
{
    /// <summary>
    /// Convenience class for writing to pins on the MCP23008
    /// </summary>
    public class DigitalOutputPort : DigitalOutputPortBase
    {
        protected readonly int _pin;
        protected readonly MCP23008 _mcp;

        public override bool State
        {
            get { return _state; }
            set {
                _mcp.WriteToPort(_pin, value);
                _state = value;
            }
        }

        public override bool InitialState
        {
            get { return _initialState; }
        } 

        protected DigitalOutputPort() : base(false) { }

        internal DigitalOutputPort(MCP23008 mcp, int pin, bool initialState) : base(initialState)
        {
            _mcp = mcp;
            _pin = pin;

            if (initialState)
            {
                State = initialState;
            }
        }
    }
}
```

The primary function of this implementation is to be able translate the `State` property to write to a port on the MCP23x chip.