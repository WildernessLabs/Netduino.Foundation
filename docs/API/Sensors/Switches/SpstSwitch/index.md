---
layout: API
title: SpstSwitch
subtitle: A simple, single pole, single throw switch sensor.
---

# Info

Represents a simple, on/off, Single-Pole-Single-Throw (SPST) switch that closes a circuit to either ground/common or high:

![](SPST_Switch.jpg)

Use the [`CircuitTerminationType`](/API/CircuitTerminationType) to specify whether the other side of the switch terminates to ground or high.

# API

## Events

#### `public event EventHandler Changed`

Raised when the switch circuit is opened or closed.

## Properties

#### `public bool IsOn`

Describes whether or not the switch circuit is closed/connected (`IsOn = true`), or open (`IsOn = false`).

## Constructors

#### `public SpstSwitch(H.Cpu.Pin pin, CircuitTerminationType type)`

Instantiates a new `SpstSwitch` object connected to the specified digital `pin`, and with the specified `CircuitTerminationType` in the `type` parameter.