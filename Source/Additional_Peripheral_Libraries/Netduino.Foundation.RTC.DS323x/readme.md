# DS323x - Real Time Clock with Battery Backup

The DS323x ICs offer a low cost accurate real time clock with a temperature compensation crystal oscillator.  This range of chips offers the following functionality:

* Temperature compensation
* Battery backup
* I2C (DS3231) and SPI (DS3234) interfaces.
* Two programmable alarms
* 32.768 KHz square wave output

## Purchasing

A variety of modules are available including low cost modules with integrated EEPROM:

* [DS3231 with integrated EEPROM](https://www.amazon.com/s/ref=nb_sb_noss?url=search-alias%3Daps&field-keywords=ds3231)
* [Sparkfun DS3234 Breakout board](https://www.sparkfun.com/products/10160)

## Hardware

The DS3231 real time clock module (see image below) requires only four (for simple timekeeping) or five (for alarms) connections

![DS3231 Real Time Clock Module](DS3231RTCOnBreadboard.png)

| Netduino Pin | Sensor Pin        | Wire Color |
|--------------|-------------------|------------|
| 3.3V         | V<sub>cc</sub>    | Red        |
| GND          | GND               | Black      |
| SC           | SCK               | Blue       |
| SD           | SDA               | White      |
| SQW          | Digital Interrupt | Orange     |

The 32K pin outputs the 32,768 Hz clock signal from the module.  This signal is only available when power is supplied by V<sub>cc</sub>, it is not available when the module is on battery power.

The orange wire is only required if the alarms are being used to interrupt the Netduino.

## Software

The following application sets an alarm to trigger at when the current second is equal to 15.  The interrupt routine displays the time and then clears the interrupt flag:

```csharp
using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;
using Netduino.Foundation.RTC;

namespace DS3231Test
{
    public class Program
    {
        public static void Main()
        {
            DS3231 rtc = new DS3231(0x68, 100, Pins.GPIO_PIN_D8);
            rtc.ClearInterrupt(DS323x.Alarm.BothAlarmsRaised);
            rtc.SetAlarm(DS323x.Alarm.Alarm1Raised, new DateTime(2017, 10, 29, 9, 43, 15), DS323x.AlarmType.WhenSecondsMatch);
            rtc.OnAlarm1Raised += rtc_OnAlarm1Raised;
            Thread.Sleep(Timeout.Infinite);
        }

        static void rtc_OnAlarm1Raised(object sender)
        {
            DS3231 rtc = (DS3231) sender;
            Debug.Print("Alarm 1 has been activated: " + rtc.CurrentDateTime.ToString());
            rtc.ClearInterrupt(DS323x.Alarm.Alarm1Raised);
        }
    }
}
```

Connect the interrupt pin of the DS3231 real time clock to digital input pin 8 on the Netduino.

## API

## Enums

#### `Alarm`

Type of alarm event that has been raised or the type of alarm that should be raised:

* `Alarm1Raised`
* `Alarm2Raised`
* `BothAlarmsRaised`

#### `AlarmType`

The two alarms have a number of possible options that may be configured with the `SetAlarm` method:

*Alarm 1*

* `OncePerSecond` - Event raised every second
* `WhenSecondsMatch` - Event raised with the seconds in the current time matches the seconds in the alarm 1 time.
* `WhenMinutesSecondsMatch` - Event raised when both the seconds and the minutes in the current time match the time stored in the alarm 1 time.
* `WhenHoursMinutesSecondsMatch` - Event raised when the hours, minutes and seconds in the current time match the time stored in the alarm 1 time.
* `WhenDateHoursMinutesSecondsMatch` - Event raised when the current date and time match the time stored in the alarm 1 time.
* `WhenDayHoursMinutesSecondsMatch` - Event raised when the day, hour, minute and second in the current time match the time in atored in alarm 1.

*Alarm 2*

* `OncePerMinute` - Event is raised every minute.
* `WhenMinutesMatch` - Event is raised when the minutes in the current time match the minutes in alarm 2.
* `WhenHoursMinutesMatch` - Event raised when the hours and minutes in the current time match those stored in alarm 2.
* `WhenDateHoursMinutesMatch` - Event raised when the date, hours and minutes of the current time match the those stored in alarm 2.
* `WhenDayHoursMinutesMatch` - Event raised when the day, hours, minutes match those stored in alarm 2.

### Constructors

### Properties

#### `DateTime CurrentDateTime`

Get or set the current time.

#### `Temperature`

Temperature of the sensor in degrees centigrade.

### Methods

#### `SetAlarm(Alarm alarm, DateTime time, AlarmType type)`

Set one of the two alarms where `alarm` indicates which alarm should be set and `type` determines which event will generate the alarm.  The `time` parameter provides the date / time information for the specified alarm.

#### `EnableDisableAlarm(Alarm alarm, bool enable)`

Enable or disable the specified alarm.

#### `ClearInterrupt(Alarm alarm)`

Clear the interrupt for the specified alarm.  This must be called in the alarm event handlers prior to exit.

### Events

#### `OnAlarm1Raised(object sender)` and `OnAlarm2Raised(object sender)`

Raised when the appropriate alarm is triggered.  If both alarms are triggered at the same time then both events will be triggered.

It is important that the event handler clears the event by calling the `ClearInterrupts` method before exiting.  If the interrupts are not cleared then future events will not be triggered.