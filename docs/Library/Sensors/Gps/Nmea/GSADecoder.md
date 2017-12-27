---
layout: Library
title: GSA Decoder
subtitle: Active satellite messages.
---
# GSA Decoder

`GSADecoder` intercepts the active satellites messages from the GPS unit.

## `OnActiveSatellitesReceived(object sender, ActiveSatellites activeSatellites)`

Generated when the list of active satellites is received from the GPS unit.

```csharp
public struct ActiveSatellites
{
    /// <summary>
    /// Dimensional fix type (No fix, 2D or 3D?)
    /// </summary>
    public DimensionalFixType Dimensions;

    /// <summary>
    /// Satellite selection type (Automatic or manual).
    /// </summary>
    public ActiveSatelliteSelection SatelliteSelection;

    /// <summary>
    /// PRNs of the satellites used in the fix.
    /// </summary>
    public string[] SatellitesUsedForFix;
    
    /// <summary>
    /// Dilution of precision for the reading.
    /// </summary>
    public double DilutionOfPrecision;

    /// <summary>
    /// Horizontal dilution of precision for the reading.
    /// </summary>
    public double HorizontalDilutionOfPrecision;

    /// <summary>
    /// Vertical dilution of precision for the reading.
    /// </summary>
    public double VerticalDilutionOfPrecision;
}
```
