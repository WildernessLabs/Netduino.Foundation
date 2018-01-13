---
layout: API
title: DipSwitch
subtitle: Represents an array of SPST switches. 
---

# Info

Represents a DIP-switch wired in a bus configuration, in which all switches are terminated to the same ground/common or high pin.

 * **Note: This component is untested.**

![](DIP_Switches.jpg)

# API

## Events

#### `public event ArrayEventHandler Changed`

Raised when one of the switches is switched on or off.


## Properties

#### `public ISwitch this[int i]` (Indexer)

Returns the `ISwitch` at the specified index.

## Constructors

#### `public DipSwitch(H.Cpu.Pin[] switchPins, CircuitTerminationType type)`

Creates a new `DipSwitch` connected to the specified `switchPins`, with the [`CircuitTerminationType`](/API/CircuitTerminationType) specified by the `type` parameters.

