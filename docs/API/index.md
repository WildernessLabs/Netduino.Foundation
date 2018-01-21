---
layout: API
title: API Reference
subtitle: Reference documentation for Netduino.Foundation core library and peripherals.
---

# Info

Netduino.Foundation includes a core library that contains base classes, interfaces, and drivers for common, generic peripherals such as LED, buttons, etc. 

For a complete list of all peripherals, see the [Library](/Library) page.

A mostly complete reference of the API can be found on the left nav, but below is a list of some of the most important classes in the core library.

# Important Base Classes

These classes provide common base functionality for communication protocols and other high value interfaces.

| Class                                  | Description                                       |
|----------------------------------------|---------------------------------------------------|
| [I2CBus](/API/Devices/Netduino/I2CBus/)| I2C abstraction layer implementing the ICommunicationsBus interface. |
| [SoftwareSpiBus](/API/Devices/Netduino/SoftwareSpiBus/) | Software implementation of the SPI communication protocol. |
| [DisplayBase](/API/DisplayBase/) | Base class for displays using the GraphicsLibrary. |

# Common Interfaces

| Interface                              | Description                                       |
|----------------------------------------|---------------------------------------------------|
| [ICommunicationBus](/API/Devices/Netduino/ICommunicationBus/) | Interface for communicating with attached peripherals. |
| [IDCMotor](/API/Motors/IDCMotor/) | Interface for DC motors. |
| [IHumiditySensor](/API/Sensors/IHumiditySensor/) | Interface describing humidity sensors. |
| [ILightSensor](/API/Sensors/ILightSensor/) | Interface describing light sensors. |
| [ITextDisplay](/API/Displays/ITextDisplay) | Interface describing text-only displays. |
| [IPressureSensor](/API/Sensors/IPressureSensor/) | Interface describing pressure (barometric) sensors. |
| [ITemperatureSensor](/API/Sensors/ITemperatureSensor/) | Interface describing temperature sensors |

# Core Peripherals

See full peripheral list on the [Library](/Library) page.

## LEDs

| Peripheral                | Description                         |
|---------------------------|-------------------------------------|
| [PwmLed](/API/LEDs/PwmLed)            | Pulse-Width-Modulation powered LED. |
| [RgbPwmLed](/API/LEDs/RgbPwmLed)      | Pulse-Width-Modulation powered RGB LED. |

## Motors

| Peripheral                | Description                         |
|---------------------------|-------------------------------------|
| [HBridgeMotor](/API/Motors/HBridgeMotor) | Generic H-Bridge motor controller IC driver. |

## Relays

| Peripheral                | Description                         |
|---------------------------|-------------------------------------|
| [Relay](/API/Relays/Relay) | Electrically isolated switch. |

## Sensors

### Buttons

| Peripheral                | Description                         |
|---------------------------|-------------------------------------|
| [PushButton](/API/Sensors/Buttons/PushButton)       | Simple push-button. |


### Switches

| Peripheral                | Description                         |
|---------------------------|-------------------------------------|
| [SpstSwitch](/API/Sensors/Switches/SpstSwitch)      | A simple single-pole, single-throw (SPST), switch. |
| [DipSwitch](/API/Sensors/Switches/DipSwitch)        | A multi-pole dip switch. |

### Temperature

| Peripheral                | Description                         |
|---------------------------|-------------------------------------|
| [AnalogTemperature](/API/Sensors/Temperature/Analog)      | Analog temperature sensor (TMP35 / TMP36 / TMP37 / LM35) |