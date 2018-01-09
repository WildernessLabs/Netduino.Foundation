---
layout: API
title: CircuitTerminationType
subtitle: Describes the termination polarity of a circuit on the opposite end of a digital pin.
---

# Info

Describes whether the circuit on the opposite end of a digital pin is terminated into the common/ground or a high (3.3V) voltage source. Used to determine whether to pull the resistor wired to the switch sensor high or low to close the circuit when the switch is closed.

## API

### Enumerations

#### `public enum CircuitTerminationType`

| Name           | Description   |
|----------------|---------------|
| `CommonGround` | Termination is at `GND` voltage. |
| `High`         | Termination is at `3.3V` voltage.  |