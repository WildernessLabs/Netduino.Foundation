---
layout: Library
title: GGA Decoder
subtitle: Positional GPS information.
---

`GGADecoder` intercepts and decodes the positional information from the GPS unit.

## `OnPositionReceived(object sender, GPSLocation location)`

Generated when location messages are received from the GPS unit.  The current location information is passed to the application in a `GPSLocation` structure:

```csharp
/// <summary>
/// Hold the location taken from a GPS reading.
/// </summary>
public struct GPSLocation
{
    /// <summary>
    /// Time that the reading was taken.  The date component is fixed for each reading.
    /// </summary>
    public DateTime ReadingTime;

    /// <summary>
    /// Latitude of the reading.
    /// </summary>
    public DegreeMinutePosition Latitude;

    /// <summary>
    /// Longitude of the reading.
    /// </summary>
    public DegreeMinutePosition Longitude;

    /// <summary>
    /// Quality of the fix.
    /// </summary>
    public FixType FixQuality;

    /// <summary>
    /// Number of satellites used to generate the positional information.
    /// </summary>
    public int NumberOfSatellites;

    /// <summary>
    /// Horizontal dilution of position (HDOP).
    /// </summary>
    public double HorizontalDilutionOfPrecision;

    /// <summary>
    /// Altitude above mean sea level (m).
    public double Altitude;
}
```