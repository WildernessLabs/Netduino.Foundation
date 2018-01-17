---
layout: Library
title: Peripheral Library Reference
subtitle: Reference documentation for Netduino.Foundation peripheral library.
---


Netduino.Foundation has two sets of peripheral drivers; the ones listed in the following section, that cover common, generic, components and are built into the Netduino.Foundation core library, and the [specialized set of third party components](#external-peripherals), listed further down, which are added via their individual Nuget packages.

# Core Peripherals

## LEDs

| Peripheral                | Description                         |
|---------------------------|-------------------------------------|
| [PwmLed](/API/LEDs/PwmLed)            | Pulse-Width-Modulation powered LED. |
| [RgbPwmLed](/API/LEDs/RgbPwmLed)      | Pulse-Width-Modulation powered RGB LED. |

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

# External Peripherals

## Integrated Circuits (ICs)

| Peripheral                | Description                         |
|---------------------------|-------------------------------------|
| [74595 Shift Register](/Library/ICs/74595)          | 74595 shift register for digital output expansion. |
| [AT24Cxx EEPROMS](/Library/ICs/EEPROM/AT24Cxx)      | AT24Cxx Family of EEPROMs including AT24C32. |
| [DS323x RTCs](/Library/RTCs/DS323x)                 | Real Time Clock modules. |

## Display and Graphics Drivers

| Peripheral                | Description                         |
|---------------------------|-------------------------------------|
| [GraphicsLibrary](/Library/Displays/GraphicsLibrary) | General purpose graphics library. |
| [MicroLiquidCrystal library](/Library/Displays/MicroLiquidCrystal) | I2C/SPI LCD Library. |
| [Serial LCD](/Library/Displays/SerialLCD)            | SparkFun serial LCD backpack driver.|
| [SSD1306](/Library/Displays/SSD1306)                 | SSD1306 OLED Display.  Currently supports 128x64 and 128x32 pixel I2C displays. |

## Sensors

### Atmospheric (Temperature, Humidity, Barometer, Altitude) Sensors

| Peripheral                | Description                         |
|---------------------------|-------------------------------------|
| [BME280](/Library/Sensors/Atmospheric/BME280)      | Combined I2C/SPI temperature, humidity, and pressure sensor. |
| BMP085                    | In development |
| DS18B20                   | In development |
| GroveTH02                 | In development |
| [HIH6130](/Library/Sensors/Atmospheric/HIH6130)    | Combined I2C temperature and humidity sensor. |
| HTU21DF                   | In development |
| [MPL115A2](/Library/Sensors/Barometric/MPL115A2)   | Combined I2C temperature and pressure sensor. |
| [MPL3115A2](/Library/Sensors/Barometric/MPL3115A2) | Combined I2C pressure and temperature sensor. |
| [SHT31D](/Library/Sensors/Atmospheric/SHT31D)      | Combined I2C temperature and humidity sensor. |
| [SiI7021](/Library/Sensors/Atmospheric/SI7021)     | Combined I2C temperature and humidity sensor. |
| [TMP102](/Library/Sensors/Temperature/TMP102)      | I2C temperature sensor. |

### Distance Sensors

| Peripheral                | Description                         |
|---------------------------|-------------------------------------|
| SharpGP2D12               | In Development |

### GPS Sensors & Libraries

| Peripheral                | Description                         |
|---------------------------|-------------------------------------|
| [NMEA GPS Decoder](/Library/Sensors/GPS/NMEA)      | Generic GPS sentence decoder library. |

### Light Sensors

| Peripheral                | Description                         |
|---------------------------|-------------------------------------|
| [ALS-PT19](/Library/Sensors/Light/ALSPT19315C)     | Analog light sensor. |
| [SI1145](/Library/Sensors/Light/SI1145)            | I2C infrared, ultraviolet, and ambient light sensor. |
| [TSL2561](/Library/Sensors/Light/TSL2561)          | I2C infrared-compensated light sensor. |

### Motion and Orientation Sensors

| Peripheral                | Description                         |
|---------------------------|-------------------------------------|
| [ADXL335](/Library/Sensors/Motion/ADXL335)         | Analog triple axis, +/-3g accelerometer. |
| [ADXL345](/Library/Sensors/Motion/ADXL345)         | I2C triple axis accelerometer, +/-16g accelerometer. |
| ADXL362 Accelerometer     | In Development |
| [BNO055](/Library/Sensors/Motion/BNO055)           | I2C 9-Axis absolute orientation sensor. |
| FXAS21002                 | In Development |
| FXOS8700CQ                | In Development |
| [MAG3110](/Library/Sensors/Motion/MAG3110)         | I2C three axis magnetometer. |
| Memsic2125                | In Development |
| MPU6050                   | In Development |
| [Parallax PIR](/Library/Sensors/Motion/ParallaxPIR)| Parallax PIR Rev B digital motion detector. |


## Shields

| Peripheral                | Description                         |
|---------------------------|-------------------------------------|
| Adafruit Motor Shield     | In Development |
| SparkFun Weather Shield   | In Development |

## Servos

| Peripheral                | Description                         |
|---------------------------|-------------------------------------|
| [ServoCore](/Library/Servos/Servo)                | Generic servo library. |

