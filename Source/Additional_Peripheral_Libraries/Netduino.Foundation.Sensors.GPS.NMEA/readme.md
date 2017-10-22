# NMEA GPS - Generic GPS Decoder

`NMEA` provides a library of routines to read the output from a GPS receiver and translate the output into a series of messages for the application.  A number of decoders are provided for the application developer:

* [GGA - 3D Fix](GGLDecoder.md)
* [GLL - Location Information](GLLDecoder.md)
* [GSA - Active satellites](GSADecoder.md)
* [GSV - Satellites in view](GSVDecoder.md)
* [RMC - Recommended Minimum](RMCDecoder.md)
* [VTG - Course over ground](VTGDecoder.md)

## Hardware

The library is compatible with NMEA GPS such as the following:

* [GP-20U7](https://www.sparkfun.com/products/13740)
* [Crius Neo-9](https://www.amazon.com/Crius-U-blox-Multiwii-Pixhawk-Controller/dp/B00KTYRZC8)

Three connections are required as a minimum:

| GPS Connection | Netduino Connection |
|------------------|------------|
| Power            | 3.3V or 5V |
| GND              | Ground     |
| T<sub>x</sub>    | R<sub>x</sub> on one of the COM ports. |

A 5V power connection is acceptable as the digital pins on the Netduino are 5V tolerant.

## Software

The following application creates a new `NMEA` GPS object and attaches the object to `COM1`.  A location (GLL) decoder is then attached to the GPS and messages are processed by the `ggaDecoder_OnPositionReceived` method:

```csharp
using Microsoft.SPOT;
using Netduino.Foundation.Sensors.GPS;
using System;
using System.Threading;

namespace NMEAExample
{
    public class Program
    {
        public static void Main()
        {
            NMEA gps = new NMEA("COM1", 9600, System.IO.Ports.Parity.None, 8, System.IO.Ports.StopBits.One);
            //
            GGADecoder ggaDecoder = new GGADecoder();
            ggaDecoder.OnPositionReceived += ggaDecoder_OnPositionReceived;
            gps.AddDecoder(ggaDecoder);
            //
            gps.Open();
            Thread.Sleep(Timeout.Infinite);
        }

        static string DecodeDMPostion(DegreeMinutePosition dmp)
        {
            string position = dmp.Degrees.ToString("f2") + "d " + dmp.Minutes.ToString("f2") + "m ";
            switch (dmp.Direction)
            {
                case DirectionIndicator.East:
                    position += "E";
                    break;
                case DirectionIndicator.West:
                    position += "W";
                    break;
                case DirectionIndicator.North:
                    position += "N";
                    break;
                case DirectionIndicator.South:
                    position += "S";
                    break;
                case DirectionIndicator.Unknown:
                    position += "Unknown";
                    break;
            }
            return (position);
        }

        static void ggaDecoder_OnPositionReceived(object sender, GPSLocation location)
        {
            Debug.Print("Location information received.");
            Debug.Print("Time of reading: " + location.ReadingTime);
            Debug.Print("Latitude: " +  DecodeDMPostion(location.Latitude));
            Debug.Print("Longitude: " + DecodeDMPostion(location.Longitude));
            Debug.Print("Altitude: " + location.Altitude);
            Debug.Print("Number of satellites: " + location.NumberOfSatellites);
            Debug.Print("Fix quality: " + location.FixQuality);
            Debug.Print("HDOP: " + location.HorizontalDilutionOfPrecision.ToString("f2"));
            Debug.Print("*********************************************\n");
        }
    }
}

```

Decoding follows the same pattern for all of the decoders:

* Create a new decoder
* Provide a message handler for the decoder.
* Associate the decoder with the `NMEA` object

## API

Much of the work of decoding the NMEA messages is handled by the decoders.  The `NMEA` object provides a mechanism for receiving lines of text from the `COM` port on the Netduino, decoding the messages and sending the data to the application.

### Constructors

#### `public NMEA(string port, int baudRate, Parity parity, int dataBits, StopBits stopBits)`

Create a new `NMEA` object and attach the object to the specified `COM` port with the specified communication parameters.

Note that this does not start to process the text from the GPS unit, a call to `Open` is required following construction of the `NMEA` class.

### Methods

#### `Open()`

Open the connection to the GPS unit.

#### `Close()`

Close the connection to the GPS unit.

#### `AddDecoder(NMEACecoder decoder)`

Add a new decoder to the list of registered decoders.
