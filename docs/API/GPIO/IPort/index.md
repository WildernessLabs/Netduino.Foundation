---
layout: API
title: IPort
subtitle: Interface contract for GPIO ports
---

# Info

`IPort` is the base interface for all port classes.

# API

### Properties

#### `PortDirectionType DirectionType { get; }`

A [`PortDirectionType`](/API/GPIO/PortDirectionType) indicating the direction, whether input or output, that the port is configured for.

#### `PortSignalType SignalType { get; }`

A [`PortSignalType`](/API/GPIO/PortSignalType) indicating what kind of signal, whether Analog or Digital, that the port is configured for.