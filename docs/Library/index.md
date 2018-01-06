---
layout: Library
title: Peripheral Library Reference
subtitle: Reference documentation for Netduino.Foundation peripheral library.
---


Netduino.Foundation has two sets of peripheral drivers; the ones listed in the following section, that cover common, generic, components and are built into the Netduino.Foundation core library, and the [specialized set of third party components](#external-peripherals), listed further down, which are added via their individual Nuget packages.

# Core Peripherals

## Buttons

| Peripheral                | Description                         |
|---------------------------|-------------------------------------|
| [PushButton](/API/CorePeripherals/Buttons/PushButton)      | Simple push-button. |

## LEDs

| Peripheral                | Description                         |
|---------------------------|-------------------------------------|
| [PwmLed](/API/CorePeripherals/LEDs/PwmLed)      | Pulse-Width-Modulation powered LED. |
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
| [SpstSwitch](/API/CorePeripherals/Sensors/Switches/SpstSwitch)      | A simple single-pole, single-throw (SPST), switch. |
| [DipSwitch](/API/CorePeripherals/Sensors/Switches/DipSwitch)      | A multi-pole dip switch. |

# External Peripherals

## Integrated Circuits (ICs)

| Peripheral                | Description                         |
|---------------------------|-------------------------------------|
| 74595 Shift Register      | [74595 shift register](/Library/ICs/74595) |
| AT24Cxx Family of EEPROMS | [AT24Cxx](/Library/ICs/EEPROM/AT24Cxx) Family of EEPROMs including AT24C32 |
| DS323x Real Time Clock    | [DS323x Real Time Clock modules](/Library/RTCs/DS323x) |

## Display and Graphics Drivers

| Peripheral                | Description                         |
|---------------------------|-------------------------------------|
| GraphicsLibrary | [General purpose graphics library](/Library/Displays/GraphicsLibrary) |
| `MicroLiquidCrystal` library | [LCD](/Library/Displays/MicroLiquidCrystal) using the `MicroLiquidCrystal` library. |
| Serial LCD                | [Serial LCD](/Library/Displays/SerialLCD) |
| SSD1306                   | [SSD1306 OLED Display](/Library/Displays/SSD1306).  Currently supports 128x64 and 128x32 pixel I2C displays. |

## Sensors

### Atmospheric (Temperature, Humidity, Barometer, Altitude) Sensors

| Peripheral                | Description                         |
|---------------------------|-------------------------------------|
| Analog Temperature Sensor | [Generic analog temperature sensor](/Library/Sensors/Temperature/Analog) (TMP35 / 36 / 37 / LM35)|
| BME280 Temperature, Humidity and Pressure Sensor | [BME280 combined temperature, humidity and pressure sensor](/Library/Sensors/Atmospheric/BME280) |
| BMP085                    | In development |
| DS18B20                   | In development |
| GroveTH02                 | In development |
| HIH6130 Temperature and Humidity | [HIH6130 Temperature and Humidity breakout board](/Library/Sensors/Atmospheric/HIH6130) |
| HTU21DF                   | In development |
| MPL115A2                  | [MPL115A2](/Library/Sensors/Barometric/MPL115A2) Temperature and pressure sensor |
| MPL3115A2                 | [MPL3115A2](/Library/Sensors/Barometric/MPL3115A2) Pressure and temperature sensor |
| SHT31D                    | [SHT31D](/Library/Sensors/Atmospheric/SHT31D) Temperature and humidity sensor |
| SiI7021                    | [Si7021](/Library/Sensors/Atmospheric/SI7021) Temperature and humidity sensor |
| TMP102                    | [TMP102](/Library/Sensors/Temperature/TMP102) Temperature sensor |

### Distance Sensors

| Peripheral                | Description                         |
|---------------------------|-------------------------------------|
| SharpGP2D12               | In Development |

### GPS Sensors & Libraries

| Peripheral                | Description                         |
|---------------------------|-------------------------------------|
| NMEA GPS Decoder          | [Generic GPS decoders](/Library/Sensors/GPS/NMEA) |

### Light Sensors

| Peripheral                | Description                         |
|---------------------------|-------------------------------------|
| ALS-PT19                  | [ALS-PT19](/Library/Sensors/Light/ALSPT19315C) series of light sensors |
| SI1145                    | [SI1145](/Library/Sensors/Light/SI1145) |
| TSL2561                   | [Luminosity sensor](/Library/Sensors/Light/TSL2561) |

### Motion and Orientation Sensors

| Peripheral                | Description                         |
|---------------------------|-------------------------------------|
| ADXL335 Accelerometer     | [ADXL335 Triple axis accelerometer](/Library/Sensors/Motion/ADXL335) (+/-3g, Analog output) |
| ADXL345 Accelerometer     | [ADXL345 Triple axis accelerometer](/Library/Sensors/Motion/ADXL345) (+/-16g, I2C digital output) |
| ADXL362 Accelerometer     | In Development |
| BNO055 Orientation        | [BNO055 9-Axis Orientation Sensor](/Library/Sensors/Motion/BNO055) |
| FXAS21002                 | In Development |
| FXOS8700CQ                | In Development |
| MAG3110 Magnetometer      | [MAG3110 Three axis magnetometer](/Library/Sensors/Motion/MAG3110) |
| Memsic2125                | In Development |
| MPU6050                   | In Development |
| Parallax PIR              | [Parallax PIR Rev B](/Library/Sensors/Motion/ParallaxPIR) |


## Shields

| Peripheral                | Description                         |
|---------------------------|-------------------------------------|
| Adafruit Motor Shield     | In Development |
| SparkFun Weather Shield   | In Development |

## Servos

| Peripheral                | Description                         |
|---------------------------|-------------------------------------|
| Servo Core                | [Generic Servo Library](/Library/Servos/Servo) |

