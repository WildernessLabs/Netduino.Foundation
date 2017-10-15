using System;

namespace Netduino.Foundation.Sensors.GPS
{
    /// <summary>
    /// Methods that must be implemented for any NMEA decoder.
    /// </summary>
    public interface INMEADecoder
    {
        /// <summary>
        /// Get the prefix for the decoder.
        /// </summary>
        /// <remarks>
        /// The lines of text from the GPS start with text such as $GPGGA, $GPGLL, $GPGSA etc.  The prefix
        /// is the start of the line (i.e. $GPCGA).
        /// </remarks>
        /// <returns>The prefix for the decoder.</returns>
        string GetPrefix();

        /// <summary>
        /// Get the friendly (human readable) name for the decoder.
        /// </summary>
        /// <returns>Friendly name for the decoder.</returns>
        string GetName();

        /// <summary>
        /// Process the message from the GPS.
        /// </summary>
        /// <param name="elements">String array of the elements of the message.</param>
        void Process(string[] elements);
    }
}
