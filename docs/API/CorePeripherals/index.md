---
layout: API
title: Core Peripherals
subtitle: Built-in peripheral drivers.
---

# About

Netduino.Foundation has two sets of peripheral drivers; [built-in ones that cover common, generic, components](/API/CorePeripherals) that are built into the Netduino.Foundation core library, and the [specialized set of third party components](/Library) which are added via their individual Nuget packages.

The built-in peripherals are as follows:

## LEDs

| Peripheral                | Description                         |
|---------------------------|-------------------------------------|
| [RgbPwmLed](/API/CorePeripherals/LEDs/RgbPwmLed)      | Pulse-Width-Modulation powered RGB LED. |

## Relays

| Peripheral                | Description                         |
|---------------------------|-------------------------------------|
| [Relay](/API/CorePeripherals/Relays/Relay)      | Electrically isolated switch. |

## Sensors

### Buttons

| Peripheral                | Description                         |
|---------------------------|-------------------------------------|
| [PushButton](/API/CorePeripherals/Sensors/Buttons/PushButton)      | Generic push button. |

### Switches

| Peripheral                | Description                         |
|---------------------------|-------------------------------------|
| [SimpleSwitch](/API/CorePeripherals/Sensors/Switches/SimpleSwitch)      | A simple single pole, single throw, switch. |