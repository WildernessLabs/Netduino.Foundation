---
layout: Examples
title: HIH6130 + Serial LCD Temperature and Humidity
subtitle: Displays temp and humidity from the HIH6130 sensor on a serial LCD.
---

# Info

This example uses the notification pattern to listen for changed events on an [HIH6130](/Library/Sensors/Atmospheric/HIH6130) I2C sensor and displays the temperature and humidity on a [serial LCD](/Library/Displays/SerialLCD) screen.

## Circuit

### HIH6130

The HIH6130 requires four connections between the Netduino and the breakout board.

| Netduino Pin | Sensor Pin     | Wire Color |
|--------------|----------------|------------|
| 3.3V         | V<sub>cc</sub> | Red        |
| GND          | GND            | Black      |
| SC           | SCK            | Blue       |
| SD           | SDA            | White      |

As shown in the following photo:

![HIH6130 on Breadboard](/Library/Sensors/Atmospheric/HIH6130/HIH6130OnBreadboard.png)

### Serial LCD

The serial LCD display needs three connections to make it work:

| Netduino Pin | Sensor Pin     | Wire Color |
|--------------|----------------|------------|
| 3.3V         | 5V             | Red        |
| GND          | GND            | Black      |
| 1            | RX             | Blue       |

As shown in the following illustration:

![Netduino Connected to SerialLCD](/Library/Displays/SerialLCD/SerialLCD.png)

**Note:** Netduino pin 1 corresponds to the COM1 TX pin, but the serial LCD RX pin can be place in any of the COM TX pins, including D3, D8, and SCL. However, in this case the HIH6130 sensor is using the SCL netduino pin because that's part of the I2C pins. For more information on changing the COM port that the serial LCD is connected to, see the HIH6130 [constructor documentation](/Library/Displays/SerialLCD/#constructor).

## Code

```csharp
using System.Threading;
using Microsoft.SPOT;
using H = Microsoft.SPOT.Hardware;
using N = SecretLabs.NETMF.Hardware.Netduino;
using Netduino.Foundation.Sensors;
using Netduino.Foundation.Displays;
using F = Netduino.Foundation;

namespace TempDisplay_6130_SerialLCD
{
    /// <summary>
    /// Pulls the temperature and humidity from an HIH6130 sensor and 
    /// displays them on a serial LCD.
    /// 
    /// See http://netduino.foundation/Library/Sensors/Atmospheric/HIH6130/
    /// for how to wire up the HIH6130.
    /// 
    /// See http://netduino.foundation/Library/Displays/SerialLCD/ for how
    /// to wire up the Serial LCD.
    /// </summary>
    public class Program
    {
        public static void Main()
        {
            // initialize our network
            App app = new App();
            app.Run();

            Thread.Sleep(Timeout.Infinite);
        }

        public class App
        {
            F.Sensors.Atmospheric.HIH6130 _hih = null;
            F.Displays.SerialLCD _lcd = null;

            public App()
            {
                _hih = new F.Sensors.Atmospheric.HIH6130();
                TextDisplayConfig displayConfig = new TextDisplayConfig() 
                   { Width = 16, Height = 2 };
                this._lcd = new F.Displays.SerialLCD(displayConfig);
            }

            public void Run()
            {
                // wire up our events
                _hih.TemperatureChanged += 
                    (object sender, SensorFloatEventArgs e) => {
                    DisplayTemperature(e.CurrentValue);
                };
                _hih.HumidityChanged += (object sender, SensorFloatEventArgs e) 
                    => {
                    DisplayHumidity(e.CurrentValue);
                };

                // clear our screen
                _lcd.Clear();

                // display the initial stuff
                DisplayTemperature(_hih.Temperature);
                DisplayHumidity(_hih.Humidity);
            }

            public void DisplayTemperature(float value)
            {
                char degree = System.Convert.ToChar(223);
                string temp = value.ToString("N2");
                string text = ("Temp: " + temp + degree + "C");
                _lcd.WriteLine(text, 0);
            }

            public void DisplayHumidity(float value)
            {
                string text = ("Humidity: " + value.ToString("N2") + "%");
                _lcd.WriteLine(text, 1);
            }

        }
    }
}
```
