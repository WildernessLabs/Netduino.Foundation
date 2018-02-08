---
layout: API
title: TypicalForwardVoltage
subtitle: Forward voltages for typical LEDs.
---

# Info

Defines typical forward voltages for LEDs. Useful when creating a [PwmLed](/API/LEDs/PwmLed) or [RgbPwmLed](/API/LEDs/RgbPwmLed) to limit the maximum PWM duty cycle to an average voltage that won't burn out the LED.

## Constants

#### public const float Red = 1.8F

Common red LEDs usually have around 1.8V forward voltage.

#### public const float Green = 2.1F

Common green LEDs usually have around 2.1V forward voltage.

#### public const float Blue = 3.5F

Common blue LEDs usually have around 3.5V forward voltage.

#### public const float Yellow = 3.5F

Common yellow LEDs usually have around 3.5V forward voltage.

#### public const float White = 2.15F

Common white LEDs usually have around 2.15V forward voltage.

#### public const float ResistorLimited = 0.0F

Use this if your LED is already voltage limited via a resistor. This will tell Netduino.Foundation not to reduce the PWM duty cycle.