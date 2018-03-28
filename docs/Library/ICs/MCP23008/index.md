---
layout: Library
title: MCP23008
subtitle: 8-bit, I2C, digital I/O expander chip.
---

# Overview

The MCP23008 chip is an 8-bit (8 port) digital I/O expander chip that uses I2C to communicate. It can be used to add additional digital input and output ports to a Netduino and can be combined with up to 8 MCP23008 chips in total, providing up to 64 additional ports.

MCP23008 is a ubiquitous chip in the hardware world and is the typical interface chip for common I2C LCD backpacks, with the [`74595`](/Library/ICs/74595/) chip being the typical interface chip for SPI LCD backpacks.

## MCP Chip Family

In addition to the MCP23008, the MCP family of chips includes; the MCP23017, which is a 16-bit version of the MCP23008, offering 16 digital ports in total, and the MCP23S08 and MCP23S017 which are SPI versions of the MCP23008 and MCP23017, respectively.

## Unified GPIO Architecture

In addition to the low-level API, the Netduino.Foundation MCP23008 driver conforms to the [unified GPIO architecture](/API/GPIO/), which enables it to be used with other Netduino.Foundation peripherals as if the ports on the MCP chip were part of the Netduino itself. For instance, you can connect an MCP23008 chip to a Netduino, and then drive a Relay via one of the pins on the expansion chip, just as if it were connected directly to a digital pin on the Netduino:

```csharp
// create our MCP23008
MCP23008 mcp = new MCP23008(39); // all address pins pulled high

// create a digital output port from that mcp
DigitalOutputPort relayPort = mcp.CreateOutputPort(1, false);

// create a new relay using that digital output port
Relay relay = new Relay(relayPort);

// toggle the relay
relay.Toggle();
```

# Hardware

[schematic of wiring it up]

## Chip Addressing

The I2C address of the chip is configurable via the address pins and is in the binary form of `0100[A2][A1][A0]`, where `A2`, `A1`, and `A0` refer to the three address pins on the chip:

![](Address_Pins.png)

For example, if all address pins were tied to ground, then the address of the chip would be `0100000` in binary, or `0x20` in hex, and `32` in decimal.

The I2C is addresses can then be as follows, where `0` represents an address pin connected to ground, and `1` represents an address pin connected to `3.3V`:

| address header | A2  | A1  | A0  | Resulting Hex Address | Resulting Decimal Address |
|----------------|-----|-----|-----|-----------------------|---------------------------|
| `0100`         | `0` | `0` | `0` | `0x20`                | `32`                      | 
| `0100`         | `0` | `0` | `1` | `0x21`                | `33`                      | 
| `0100`         | `0` | `1` | `0` | `0x22`                | `34`                      | 
| `0100`         | `0` | `1` | `1` | `0x23`                | `35`                      | 
| `0100`         | `1` | `0` | `0` | `0x24`                | `36`                      | 
| `0100`         | `1` | `0` | `1` | `0x25`                | `37`                      | 
| `0100`         | `1` | `1` | `0` | `0x26`                | `38`                      | 
| `0100`         | `1` | `1` | `1` | `0x27`                | `39`                      | 

Because there are 8 address possibilities, it's possible to put 8 MCP2308 chips on a single I2C bus.

To make this simpler, when instantiating an MCP2308 object, there is a constructor overload that takes the address pin configurations instead of an address, so that Netduino.Foundation uses the appropriate address based on the pins, instead of requiring a pre-computed address.

# API

### Constructors

#### `public MCP23008(bool pinA0, bool pinA1, bool pinA2, ushort speed = 100)`

Instantiates a new MCP23008 using the computed address based on the specified pin configurations. If a pin is tied to ground, use `false`, and if it's tied to `3.3V`, use `true` for the `pinA[x]` parameters.

#### `public MCP23008(byte address = 0x20, ushort speed = 100)`

Instantiates a new MCP23008 using the specified address and I2C bus speed.

### Methods

#### `public DigitalOutputPort CreateOutputPort(byte pin, bool initialState)`

Creates a new `DigitalOutputPort` (which implements [`IDigitalOutputPort`](/API/GPIO/IDigitalOutputPort/)) using the specified pin and initial state.

This method allows you to use a pin on the MCP2308 as if it were a digital output pin on the Netduino, via the [unified GPIO architecture](/API/GPIO/).

#### `public void SetPortDirection(byte pin, PortDirectionType direction)`

Configures the specified pin to be either an input or output port.

#### `public void WriteToPort(int pin, bool value)`

Sets a particular pin's value, either high/`3.3V` (`true`), or low/`0V` (`false`). If that pin is not in output mode, this method will first set its direction to output.

#### `public void OutputWrite(byte outputMask)`

Outputs a byte value across all of the pins by writing directly to the output latch (OLAT) register.

# Code Examples

## Simple Digital Writes

The [MCP23008_SimpleDigitalWrites](https://github.com/WildernessLabs/Netduino.Foundation/tree/MCP23008/Source/Peripheral_Libs/ICs.MCP23008/Samples/MCP2308_SimpleDigitalWrites) sample illustrates how to use the `OutputWrite` and `WriteToPort` methods to write to the output ports:

```csharp
using System;
using System.Threading;
using Microsoft.SPOT;
using Netduino.Foundation.ICs.MCP23008;

namespace MCP2308_SimpleDigitalWrites
{
    public class Program
    {
        static MCP23008 _mcp = null;

        public static void Main()
        {
            _mcp = new MCP23008(39);

            while (true)
            {
                for (int i = 0; i <= 7; i++)
                {
                    // can write a byte mask that specifies all the pin
                    // values in one byte
                    _mcp.OutputWrite((byte)(1 << i));

                    // or you can write to individual pins:
                    //for (int j = 0; j <= 7; j++) {
                    //    _mcp.WriteToPort(j, false);
                    //}
                    //_mcp.WriteToPort(i, true);

                    Debug.Print("i: " + i.ToString());
                    Thread.Sleep(250);
                }
            }
        }
    }
}

```

## Digital Output Port High-level API Sample

The [MCP23008_DigitalOutputPort](https://github.com/WildernessLabs/Netduino.Foundation/tree/MCP23008/Source/Peripheral_Libs/ICs.MCP23008/Samples/MCP23008_DigitalOutputPort) sample illustrates using the high-level unified GPIO API to write to a pin on the chip via a `DigitalOutputPort` object.

```csharp
using System;
using System.Threading;
using Microsoft.SPOT;
using Netduino.Foundation.ICs.MCP23008;

namespace MCP23008_DigitalOutputPort
{
    public class Program
    {
        static MCP23008 _mcp = null;

        public static void Main()
        {
            _mcp = new MCP23008(39);

            // create an array of ports
            DigitalOutputPort[] ports = new DigitalOutputPort[8];
            for (byte i = 0; i <= 7; i++) {
                ports[i] = _mcp.CreateOutputPort(i, false);
            }

            while (true)
            {
                // count from 0 to 7 (8 leds)
                for (int i = 0; i <= 7; i++)
                {
                    // turn on the LED that matches the count
                    for (byte j = 0; j <= 7; j++)
                    {
                        ports[j].State = (i == j);
                    }

                    Debug.Print("i: " + i.ToString());
                    Thread.Sleep(250);
                }
            }
        }
    }
}
```

## Relay Sample

The [MCP23008_RelaySample](https://github.com/WildernessLabs/Netduino.Foundation/tree/MCP23008/Source/Peripheral_Libs/ICs.MCP23008/Samples/MCP23008_RelaySample) illustrates how to use the high level unified GPIO API to combine an MCP23008 and a relay:

```csharp
using System;
using System.Threading;
using Microsoft.SPOT;
using Netduino.Foundation.GPIO;
using Netduino.Foundation.ICs.MCP23008;
using Netduino.Foundation.Relays;

namespace MCP23008_RelaySample
{
    /// <summary>
    /// Illustrates using a Netduino.Foundation.Relays.Relay object
    /// driven by an MCP23008 I2C output expander
    /// </summary>
    public class Program
    {
        public static void Main()
        {
            // create our MCP23008
            MCP23008 mcp = new MCP23008(39); // all address pins pulled high

            // create a digital output port from that mcp
            DigitalOutputPort relayPort = mcp.CreateOutputPort(1, false);

            // create a new relay using that digital output port
            Relay relay = new Relay(relayPort);

            // loop forever
            while (true) {
                // toggle the relay
                relay.Toggle();

                Debug.Print("Relay on: " + relay.IsOn.ToString());

                // wait for 5 seconds
                Thread.Sleep(5000);
            }
        }
    }
}
```