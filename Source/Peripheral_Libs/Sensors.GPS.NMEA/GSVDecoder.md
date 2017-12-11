# GSV Decoder

`GSADecoder` intercepts the satellites in view messages from the GPS unit.

## `OnSatellitesInViewReceived(object sender, Satellite[] satellites)`

Generated when the list of satellites in view is received from the GPS unit.

```csharp
    /// <summary>
    ///     Satellite information to use in the GSV (Satellites in View) decoder.
    /// </summary>
    public struct Satellite
    {
        /// <summary>
        ///     Satellite ID.
        /// </summary>
        public string ID;

        /// <summary>
        ///     Angle of elevation.
        /// </summary>
        public int Elevation;

        /// <summary>
        ///     Satellite azimuth.
        /// </summary>
        public int Azimuth;

        /// <summary>
        ///     Signal to noise ratio of the signal.
        /// </summary>
        public int SignalTolNoiseRatio;
    }
```