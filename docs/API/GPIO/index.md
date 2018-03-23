---
layout: API
title: GPIO Namespace
subtitle: General Purpose Input/Output classes and definitions
---

# Netduino.Foundation Unified GPIO Architecture

Netduino.Foundation incorporates an architecture that was designed to extend peripheral support seamlessly across expansion peripherals such as the I2C/SPI/etc. I/O expansion chips. For example, you can connect an MCP23x or 74x595 I/O expansion chip to a Netduino, and then drive a `Relay` via one of the pins on the expansion chip, just as if it were connected directly to the Netduino!

[illustration]

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

## Interfaces

* **[`IPort`](/API/GPIO/IPort)** - blah
* **[`IAnalogPort`](/API/GPIO/IAnalogPort)** - blah
* **[`IDigitalPort`](/API/GPIO/IDigitalPort)** - blah
* **[`IDigitalOutputPort`](/API/GPIO/IDigitalOutputPort)** - blah
* **[`IDigitalInputPort`](/API/GPIO/IDigitalInputPort)** - blah

## Base Classes

* **[`DigitalPortBase`](/API/GPIO/DigitalPortBase)** - blah
* **[`DigitalOutputPortBase`](/API/GPIO/DigitalOutputPortBase)** - blah

## Enums

* **[`PortDirectionType`](/API/GPIO/PortDirectionType)** - blah
* **[`PortSignalType`](/API/GPIO/PortSignalType)** - blah
