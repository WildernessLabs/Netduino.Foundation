using System;
using System.IO.Ports;
using System.Collections;
using Netduino.Foundation.Devices;
using Netduino.Foundation.Helpers;

namespace Netduino.Foundation.Sensors.GPS
{
    /// <summary>
    /// Generic NMEA GPS object.
    /// </summary>
    public class NMEA
    {
        #region Member variables / fields

        /// <summary>
        /// NMEA decoders available to the GPS.
        /// </summary>
        Hashtable _decoders = new Hashtable();

        /// <summary>
        /// GPS serial input.
        /// </summary>
        SerialTextFile _gps = null;

        #endregion Member variables / fields

        #region Constructors

        /// <summary>
        /// Default constructor for a NMEA GPS object, this is private to prevent the user from
        /// using it.
        /// </summary>
        private NMEA()
        {
        }

        /// <summary>
        /// Create a new NMEA GPS object and attach to the specified serial port.
        /// </summary>
        /// <param name="port">Serial port attached to the GPS.</param>
        /// <param name="baudRate">Baud rate.</param>
        /// <param name="parity">Parity.</param>
        /// <param name="dataBits">Number of data bits.</param>
        /// <param name="stopBits">Number of stop bits.</param>
        public NMEA(string port, int baudRate, Parity parity, int dataBits, StopBits stopBits)
        {
            _gps = new SerialTextFile(port, baudRate, parity, dataBits, stopBits, "\r\n");
            _gps.OnLineReady += _gps_OnLineReady;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Add a new NMEA decoder to the GPS.
        /// </summary>
        /// <param name="decoder">NMEA decoder.</param>
        public void AddDecoder(INMEADecoder decoder)
        {
            string key = decoder.GetPrefix();
            _decoders.Add(key, decoder);
        }

        #endregion Methods

        #region Interrupts

        /// <summary>
        /// GPS message ready for processing.
        /// </summary>
        /// <remarks>
        /// Unknown message types will be discarded.
        /// </remarks>
        /// <param name="line">GPS text for processing.</param>
        void _gps_OnLineReady(string line)
        {
            if (line.Length > 0)
            {
                int checksumLocation = line.LastIndexOf('*');
                if (checksumLocation > 0)
                {
                    string checksumDigits = line.Substring(checksumLocation + 1);
                    string actualData = line.Substring(0, checksumLocation);
                    if (Debug.Hexadecimal(Checksum.XOR(actualData.Substring(1))) == ("0x" + checksumDigits))
                    {
                        string[] elements = actualData.Split(',');
                        if (elements.Length > 0)
                        {
                            INMEADecoder decoder = (INMEADecoder)_decoders[elements[0]];
                            if (decoder != null)
                            {
                                decoder.Process(elements);
                            }
                            else
                            {
                                throw new Exception("No registered decoder for " + elements[0]);
                            }
                        }
                    }
                }
            }
        }

        #endregion Interrupts
    }
}
