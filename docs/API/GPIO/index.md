---
layout: API
title: GPIO Namespace
subtitle: General Purpose Input/Output classes and definitions
---

# Netduino.Foundation Unified GPIO Architecture

**Preview Release**

Netduino.Foundation incorporates an architecture that was designed to extend peripheral support seamlessly across expansion peripherals such as the I2C/SPI/etc. I/O expansion chips. For example, you can connect an MCP23x or 74x595 I/O expansion chip to a Netduino, and then drive a `Relay` via one of the pins on the expansion chip, just as if it were connected directly to the Netduino!

This is possible through the use of GPIO interface and base classes that define GPIO port contracts and provide base level implementation.

For instance, the following code illustrates the use of a `Relay` connected to an `MCP23x`:

```csharp
// create our MCP23008
MCP23008 mcp = new MCP23008(39); // all address pins pulled high

// create a digital output port from that mcp
DigitalOutputPort relayPort = mcp.CreateOutputPort(1, false);

// create a new relay using that digital output port
Relay relay = new Relay(relayPort);

// toggle the relay
relay.Toggle();
```

## Preview Release Version

This API is in preview release. Only the output ports are in this version (there is no `DigitalInputPort` for instance), and the API is subject to change as we validate it.

## Interfaces

* **[`IPort`](/API/GPIO/IPort)** - The base interface for all port classes.
* **[`IAnalogPort`](/API/GPIO/IAnalogPort)** - Implements [`IPort`](/API/GPIO/IPort) and is the base interface for all analog port classes.
* **[`IDigitalPort`](/API/GPIO/IDigitalPort)** - Implements [`IPort`](/API/GPIO/IPort) and is the base interface for all digital port classes.
* **[`IDigitalOutputPort`](/API/GPIO/IDigitalOutputPort)** - Implements [`IDigitalPort`](/API/GPIO/IDigitalPort) and is the interface for all digital output port classes.
* **[`IDigitalInputPort`](/API/GPIO/IDigitalInputPort)** - In Development

## Base Classes

* **[`DigitalPortBase`](/API/GPIO/DigitalPortBase)** - Provides a base implementation for much of the common tasks of classes implementing [`IDigitalPort`](/API/GPIO/IDigitalPort).
* **[`DigitalOutputPortBase`](/API/GPIO/DigitalOutputPortBase)** - Inherits from [`DigitalPortBase`](/API/GPIO/DigitalPortBase) and provides a base implementation for much of the common tasks of classes implementing [`IDigitalPort`](/API/GPIO/IDigitalPort).

## Enums

* **[`PortDirectionType`](/API/GPIO/PortDirectionType)** - Describes the signal direction (input or output) of the port; input or output.
* **[`PortSignalType`](/API/GPIO/PortSignalType)** - Describes the signal type (analog or digital) of the port; analog or digital.
