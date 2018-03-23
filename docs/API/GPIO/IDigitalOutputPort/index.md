---
layout: API
title: IDigitalOutputPort
subtitle: Interface contract for GPIO digital output ports
---

# Info

`IDigitalOutputPort` implements [`IDigitalPort`](/API/GPIO/IDigitalPort) and is the interface for all digital output port classes.

# API

### Properties

#### `bool InitialState { get; }`

Gets the port's initial state, either low (`false`), or high (`true`), as typically configured during the port's constructor. 