# 74595 Shift Register

Shift registers offer the ability to increase the number of outputs on a microcontroller by using I2C or SPI interfaces.  In the case of the 74xx595 series of shift registers, the SPI interface is used to output a series of bits that are then latched to the output pins of the chip.

This class allows the Netduino to control the output pins on a 74HCT595 shift register using the SPI interface.

Note that when using this chip care should be taken to make sure that the total output load of the chip does not exceed the current and thermal dissipation properties for the specific shift register being used.

## Hardware

The board below shows the Netduino connected to a shift register and 8 LEDs.  The binary value on the output pins of the shift register will be presented on the LEDs:

![Shift Register and LEDs on Breadboard](ShiftRegisterAndLEDsOnBreadboard.png)

## Software

The application below uses a `ShiftRegister74595` object to cycle through the bits on the shift register and light the appropriate LED.

```csharp
using System.Threading;
using Microsoft.SPOT.Hardware;
using Netduino.Foundation.IC;
using SecretLabs.NETMF.Hardware.NetduinoPlus;

namespace ShiftRegisterTest
{
    public class Program
    {
        public static void Main()
        {
            var config = new SPI.Configuration(SPI_mod: SPI_Devices.SPI1,
                                               ChipSelect_Port: Pins.GPIO_PIN_D8,
                                               ChipSelect_ActiveState: false,
                                               ChipSelect_SetupTime: 0,
                                               ChipSelect_HoldTime: 0,
                                               Clock_IdleState: true,
                                               Clock_Edge: true,
                                               Clock_RateKHz: 10);
            var shiftRegister = new ShiftRegister74595(8, config);
            while (true)
            {
                shiftRegister.Clear(true);
                for (byte index = 0; index <= 7; index++)
                {
                    shiftRegister[index] = true;
                    shiftRegister.LatchData();
                    Thread.Sleep(500);
                    shiftRegister[index] = false;
                }
            }
        }
    }
}
```

## API

### Constructors

#### `ShiftRegister74595(int bits, SPI.Configuration config)`

Create a new `ShiftRegister74595` object using the parameters in the `SPI.Configuration` object.  The `SPI.Configuration` object in the sample application above is correct for a 74xx595.

### Methods

#### `bool this[int bit]`

The index operator is overloaded to allow simple access to each of the bits in the shift register as follows:

```csharp
shiftRegister[index] = true;
```

#### `void Clear(bool latch = false)`

Clear all of the bits in the shift register.  The optional `latch` parameter can be used to force the bits onto the output from the shift register.

#### `void LatchData()`

Latch the data onto the shift register outputs.