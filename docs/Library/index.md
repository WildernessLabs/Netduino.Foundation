---
layout: Library
title: Peripheral Library Reference
subtitle: Reference documentation for Netduino.Foundation peripheral library.
---

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
| `MicroLiquidCrystal` library | [LCD](/Source/Peripheral_Libs/Displays.MicroLiquidCrystal) using the `MicroLiquidCrystal` library. |
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
| ALS-PT19                  | [ALS-PT19](/Source/Peripheral_Libs/Sensors.Light.ALSPT19315C) series of light sensors |
| SI1145                    | In Development |
| TSL2561                   | [Luminosity sensor](/Source/Peripheral_Libs/Sensors.Light.TSL2561) |

### Motion and Orientation Sensors

| Peripheral                | Description                         |
|---------------------------|-------------------------------------|
| ADXL335 Accelerometer     | [ADXL335 Triple axis accelerometer](/Source/Peripheral_Libs/Sensors.Motion.ADXL335) (+/-3g, Analog output) |
| ADXL345 Accelerometer     | [ADXL345 Triple axis accelerometer](/Source/Peripheral_Libs/Sensors.Motion.ADXL345) (+/-16g, I2C digital output) |
| ADXL362 Accelerometer     | In Development |
| BNO055 Orientation        | [BNO055 9-Axis Orientation Sensor](/Source/Peripheral_Libs/Sensors.Motion.BNO055) |
| FXAS21002                 | In Development |
| FXOS8700CQ                | In Development |
| MAG3110 Magnetometer      | [MAG3110 Three axis magnetometer](/Source/Peripheral_Libs/Sensors.Motion.MAG3110) |
| Memsic2125                | In Development |
| MPU6050                   | In Development |
| Parallax PIR              | [Parallax PIR Rev B](/Source/Peripheral_Libs/Sensors.Motion.ParallaxPIR) |


## Shields

| Peripheral                | Description                         |
|---------------------------|-------------------------------------|
| Adafruit Motor Shield     | In Development |
| SparkFun Weather Shield   | In Development |

## Servos

| Peripheral                | Description                         |
|---------------------------|-------------------------------------|
| Servo Core                  | [Generic Servo Library](/Source/Peripheral_Libs/Servos.Servo) |

