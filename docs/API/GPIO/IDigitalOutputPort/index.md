---
layout: API
title: IDigitalOutputPort
subtitle: Interface contract for GPIO digital output ports
---

# Info

`IDigitalOutputPort` implements [`IDigitalPort`](/API/GPIO/IDigitalPort) and is the interface for all digital output port classes.

The intent of `IDigitalOutputPort` is to provide an a seamless way to extend peripherals that require digital output ports such as relays, LEDs, and others, across IO expansion chips such as the MCP23x and the 74x595 family. 

We've created a [`DigitalOutputPortBase`](/API/GPIO/DigitalOutputPortBase) class to help reduce the implementation burden of `IDigitalOutputPort`.

# API

### Properties

#### `bool InitialState { get; }`

Gets the port's initial state, either low (`false`), or high (`true`), as typically configured during the port's constructor. 