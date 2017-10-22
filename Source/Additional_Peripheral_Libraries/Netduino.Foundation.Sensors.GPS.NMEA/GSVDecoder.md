# GSV Decoder

`GSADecoder` intercepts the satellites in view messages from the GPS unit.

## `OnSatellitesInViewReceived(object sender, Satellite[] satellites)`

Generated when the list of satellites in view is received from the GPS unit.

```csharp
/// <summary>
/// Satellite information to use in the GSV (Satellites in View) decoder.
/// </summary>
public struct Satellite
{
    public string ID;

    public int Elevation;

    public int Azimuth;

    public int SignalTolNoiseRatio;
}
```