---
layout: GettingStarted
title: Getting Started
subtitle: Working with the unified GPIO Architecture.
---

# Netduino.Foundation Unified GPIO Architecture

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

For more information, check out the [GPIO namespace API documentation](/API/GPIO).


## [Next - Check out the Peripheral Library!](/Library)