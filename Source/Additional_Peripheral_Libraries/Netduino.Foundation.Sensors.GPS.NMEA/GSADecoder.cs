using System;
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
            if (OnActiveSatelitesReceived != null)
            {
                ActiveSatellites satelites = new ActiveSatellites();
                switch (data[1].ToLower())
                {
                    case "a":
                        satelites.SateliteSelection = ActiveSateliteSelection.Automatic;
                        break;
                    case "m":
                        satelites.SateliteSelection = ActiveSateliteSelection.Manual;
                        break;
                    default:
                        satelites.SateliteSelection = ActiveSateliteSelection.Unknown;
                        break;
                }
                satelites.Demensions = (DimensionalFixType) int.Parse(data[2]);
                int sateliteCount = 0;
                for (int index = 3; index < 15; index++)
                {
                    if ((data[index] != null) && (data[index] != ""))
                    {
                        sateliteCount++;
                    }
                }
                if (sateliteCount > 0)
                {
                    satelites.SatelitesUsedForFix = new string[sateliteCount];
                    int currentSatelite = 0;
                    for (int index = 3; index < 15; index++)
                    {
                        if ((data[index] != null) && (data[index] != ""))
                        {
                            satelites.SatelitesUsedForFix[currentSatelite] = data[index];
                            currentSatelite++;
                        }
                    }
                }
                else
                {
                    satelites.SatelitesUsedForFix = null;
                }
                satelites.DilutionOfPrecision = double.Parse(data[15]);
                satelites.HorizontalDilutionOfPrecision = double.Parse(data[16]);
                satelites.VerticalDilutionOfPrecision = double.Parse(data[17]);
                OnActiveSatelitesReceived(this, satelites);
            }
        }

        #endregion NMEADecoder methods & properties
    }
}
