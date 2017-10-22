namespace Netduino.Foundation.Sensors.GPS
{
    public class GSADecoder : NMEADecoder
    {
        #region Delegates and events

        /// <summary>
        /// Delegate for the GSA data received event.
        /// </summary>
        /// <param name="activeSatellites">Active satellites.</param>
        /// <param name="sender">Reference to the object generating the event.</param>
        public delegate void ActiveSatellitesReceived(object sender, ActiveSatellites activeSatellites);

        /// <summary>
        /// Event raised when valid GSA data is received.
        /// </summary>
        public event ActiveSatellitesReceived OnActiveSatellitesReceived = null;

        #endregion Delegates and events

        #region Constructors

        /// <summary>
        /// Default constructor.
        /// </summary>
        public GSADecoder()
        {
        }

        #endregion Constructors
        
        #region INMEADecoder methods & properties

        /// <summary>
        /// Prefix for the GSA decoder.
        /// </summary>
        public override string Prefix { get { return ("$GPGSA"); } }

        /// <summary>
        /// Friendly name for the GSA messages.
        /// </summary>
        public override string Name { get { return ("GSA - DOP and number of active satellites."); } }

        /// <summary>
        /// Process the data from a GSA message.
        /// </summary>
        /// <param name="data">String array of the message components for a GSA message.</param>
        public override void Process(string[] data)
        {
            if (OnActiveSatellitesReceived != null)
            {
                ActiveSatellites Satellites = new ActiveSatellites();
                switch (data[1].ToLower())
                {
                    case "a":
                        Satellites.SatelliteSelection = ActiveSatelliteSelection.Automatic;
                        break;
                    case "m":
                        Satellites.SatelliteSelection = ActiveSatelliteSelection.Manual;
                        break;
                    default:
                        Satellites.SatelliteSelection = ActiveSatelliteSelection.Unknown;
                        break;
                }
                Satellites.Demensions = (DimensionalFixType) int.Parse(data[2]);
                int SatelliteCount = 0;
                for (int index = 3; index < 15; index++)
                {
                    if ((data[index] != null) && (data[index] != ""))
                    {
                        SatelliteCount++;
                    }
                }
                if (SatelliteCount > 0)
                {
                    Satellites.SatellitesUsedForFix = new string[SatelliteCount];
                    int currentSatellite = 0;
                    for (int index = 3; index < 15; index++)
                    {
                        if ((data[index] != null) && (data[index] != ""))
                        {
                            Satellites.SatellitesUsedForFix[currentSatellite] = data[index];
                            currentSatellite++;
                        }
                    }
                }
                else
                {
                    Satellites.SatellitesUsedForFix = null;
                }
                Satellites.DilutionOfPrecision = double.Parse(data[15]);
                Satellites.HorizontalDilutionOfPrecision = double.Parse(data[16]);
                Satellites.VerticalDilutionOfPrecision = double.Parse(data[17]);
                OnActiveSatellitesReceived(this, Satellites);
            }
        }

        #endregion NMEADecoder methods & properties
    }
}
