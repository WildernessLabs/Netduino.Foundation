---
layout: Library
title: PwmLed
subtitle: Simple LED that's current-limited via Pulse-Width-Modulation (PWM).
---

# Info

Represents an LED whose voltage is limited by the duty-cycle of a PWM signal.

# API Reference

## Constructors

### `public PwmLed(H.Cpu.PWMChannel pin, float forwardVoltage)`

Creates a new PwmLed on the specified PWM pin and limited to the appropriate  voltage based on the passed `forwardVoltage`. Typical LED forward voltages can be found in the [`TypicalForwardVoltage`](../TypicalForwardVoltage/) class.

## Properties

## Methods