---
layout: API
title: DigitalPortBase
subtitle: Base class for IDigitalPort implementations.
---

# Intro

`DigitalPortBase` provides a base implementation for much of the common tasks of classes implementing [`IDigitalPort`](/API/GPIO/IDigitalPort).

## Required APIs for Implementations

### Properties

#### `public PortDirectionType { get; }`

The `PortDirectionType` property is backed by the `readonly _direction` member. This member must be set during the constructor and describes whether the port in an input or output port.

## Other API

### Properties

#### `public PortSignalType SignalType`

The `PortSignalType` property returns `PortSignalType.Digital`.
