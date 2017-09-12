# Todo



## Drivers

### Output (what to call this? should it include LED stuff?)

#### LEDs

#### LCDs

#### Piezo (core lib)

### Input/HID (what to call this?)

#### Joystick

#### Keypad

 * VKEY
   * [SparkFun](https://www.sparkfun.com/products/12080)
 * Waveshare AD (Analog)
   * [Amazon](http://www.amazon.com/Waveshare-Accessory-buttons-controlled-keyboard/dp/B00KM6UXVS)
 * MPR121
   * [SparkFun (Breakout)](https://www.sparkfun.com/products/9695)
   * [SparkFun (Keypad)](https://www.sparkfun.com/products/12017)
   * [Adafruit (Breakout)](https://www.adafruit.com/products/1982)
   * [Adafruit (Shield)](https://www.adafruit.com/products/2024)
 * MPR121QR2
  * [SparkFun](https://www.sparkfun.com/products/12013)
 * Grove QTouch (AT42QT1070)
  * [Seeed Studio](http://www.seeedstudio.com/depot/Grove-Touch-Sensor-p-747.html)
 * 12 Button Keypad (requires I2C backpack) [Note: may not be true for Netduino]
  * [Adafruit](https://www.adafruit.com/products/1824)
  * [SparkFun](https://www.sparkfun.com/products/8653)

### Sensors

#### Accelerometers

* **Analog**
  * ADXL335
    * [SparkFun](https://www.sparkfun.com/products/9269)
    * [Adafruit](https://www.adafruit.com/product/163)
  * ADXL362
    * [SparkFun](https://www.sparkfun.com/products/11446)
  * LIS344AL

* **Digital**
  * MPU6050 (I2C IMU)
    * [Invensense](https://www.invensense.com/products/motion-tracking/6-axis/mpu-6050/)
    * [SparkFun](https://www.sparkfun.com/products/11028)
  * ADXL345 (I2C)
    * [Adafruit](https://www.adafruit.com/product/1231)
    * [SparkFun](https://www.sparkfun.com/products/9836)

#### Altimeters

 * BMP180
   * [Adafruit](https://www.adafruit.com/products/1603)
   * [SparkFun](https://www.sparkfun.com/products/11824)
   * [Grove](http://www.seeedstudio.com/depot/Grove-Barometer-Sensor-BMP180-p-1840.html)
 * BMP280
   * [Adafruit](https://www.adafruit.com/products/2651)
 * BME280
   * [Adafruit](https://www.adafruit.com/products/2652)
   * [SparkFun](https://www.sparkfun.com/products/13676)
 * MPL3115A2
   * [Adafruit](https://www.adafruit.com/products/1893)
   * [SparkFun](https://www.sparkfun.com/products/11084)
      * [SparkFun Weather Shield](https://www.sparkfun.com/products/12081)
      * [SparkFun Photon Weather Shield](https://www.sparkfun.com/products/13630)
 * MS5611
   * [Amazon](http://www.amazon.com/MS5611-High-resolution-Atmospheric-Pressure-Module/dp/B00F4P6LKE)

#### Barometers

Note: should we just combine altimeters with barometers? It includes all the ones above, and adds these two:

 * MPL115A2
   * [Adafruit](https://www.adafruit.com/products/992)
   * [SparkFun](https://www.sparkfun.com/products/9721)

#### Compasses

 * HMC5883L
    * [Triple-axis Magnetometer (Compass) Board - HMC5883L](https://www.adafruit.com/products/1746)
    * [SparkFun Triple Axis Magnetometer Breakout - HMC5883L](https://www.sparkfun.com/products/10530)
 * BNO055
    * [Adafruit 9-DOF Absolute Orientation IMU Fusion Breakout - BNO055](https://www.adafruit.com/product/2472)
 * MAG3110
    * [SparkFun Triple Axis Magnetometer Breakout - MAG3110](https://www.sparkfun.com/products/12670)

#### GPS

 * [MediaTek MT3339](http://www.mediatek.com/en/products/connectivity/gps/mt3339)
   * [66-Channel LS20031 GPS Receiver Module](https://www.pololu.com/product/2138)
   * [G-Top LadyBird 1 (PA6H)](http://www.gtop-tech.com/en/product/LadyBird-1-PA6H/MT3339_GPS_Module_04.html)
   * [Adafruit Ultimate GPS Breakout](https://www.adafruit.com/products/746)
 * [U-blox NEO-6M](https://www.u-blox.com/en/product/neo-6-series)
 * [Crius Neo-6 v3.1](http://www.ebay.com/itm/like/281357870575?ul_noapp=true&chn=ps&lpid=82)
 * [GP-20U7 (56 Channel)](https://www.sparkfun.com/products/13740?utm_source=j5)

#### Gyroscopes

 * MPU6050 (I2C) - See Accelerometer above. Maybe combine?
 * BNO055
   * [Adafruit](https://www.adafruit.com/products/2472)
   * [Atmel](http://www.atmel.com/tools/ATBNO055-XPRO.aspx)
   * [Tindie](https://www.tindie.com/products/onehorse/bno-055-9-axis-motion-sensor-with-hardware-sensor-fusion/)

#### Hygrometers (Humidity Sensors)

 * BME280 (See barometer, above)
 * HTU21D
   * [Adafruit](https://www.adafruit.com/products/1899)
   * [Sparkfun](https://www.sparkfun.com/products/12064)
 * HIH6130
   * [Sparkfun](https://www.sparkfun.com/products/11295)
 * TH02
   * [Grove](http://www.seeedstudio.com/depot/Grove-TemperatureHumidity-Sensor-HighAccuracy-Mini-p-1921.html)
 * SI7020
   * [Tessel Climate](http://www.seeedstudio.com/depot/Tessel-Climate-Module-p-2225.html)
 * SI7021
   * [Sparkfun](https://www.sparkfun.com/products/13763)
 * SHT31D
   * [Adafruit](https://www.adafruit.com/products/2857)
 * DHT11 (Via I2C Backpack)
   * [Google](https://www.google.com/search?q=DHT11)
 * DHT21 (Via I2C Backpack)
   * [Google](https://www.google.com/search?q=DHT21)
 * DHT22 (Via I2C Backpack)
   * [Google](https://www.google.com/search?q=DHT22)

#### InertialMeasurement

 * MPU6050
   * [Invensense](http://www.invensense.com/products/motion-tracking/6-axis/mpu-6050/)
   * [SparkFun](https://www.sparkfun.com/products/11028)
 * BNO055
   * [Adafruit](https://www.adafruit.com/products/2472)
   * [Atmel](http://www.atmel.com/tools/ATBNO055-XPRO.aspx)
   * [Tindie](https://www.tindie.com/products/onehorse/bno-055-9-axis-motion-sensor-with-hardware-sensor-fusion/)
   
#### Light

 * Photoreistors
   * [SparkFun Mini Photocell](https://www.sparkfun.com/products/9088)
 * TSL2561
   * [Adafruit](https://www.adafruit.com/product/439)
   * [SparkFun](https://www.sparkfun.com/products/12055)
 * [Lego EV3 Color & Light Sensor](http://shop.lego.com/en-US/EV3-Color-Sensor-45506)
 * [Lego NXT Color Sensor](http://shop.lego.com/en-US/Light-Sensor-9844)

#### Motion

 * [Passive Infra-Red by Parallax](http://www.parallax.com/tabid/768/productid/83/default.aspx)
 * [Generic Passive Infra-Red, HC-SR501](http://www.amazon.com/HC-SR501-Sensor-Module-Pyroelectric-Infrared/dp/B007XQRKD4)
 * [OSEPP IR Proximity Sensor](http://osepp.com/products/sensors-arduino-compatible/osepp-ir-proximity-sensor-module/) (Note: despite its name, this is not a proximity sensor that outputs a measured distance to an obstruction.)
 * Sharp IR Motion Detection
   * [GP2Y0D810Z0F](https://www.pololu.com/product/1134)
   * [GP2Y0D815Z0F](https://www.pololu.com/product/1133)

#### Proximity

#### Temperature

### Motors

#### Servo (core lib)

#### Stepper (core lib)

### Multipurpose

(breakout boards, shields, etc.)

### Servo

### Switches

#### Buttons

#### Relays
