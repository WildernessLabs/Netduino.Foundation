using System;
using System.Text;
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
        //SerialTextFile _gps = null;

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
//            _gps = new SerialTextFile(port, baudRate, parity, dataBits, stopBits, "\r\n");
//            _gps.OnLineReceived += _gps_OnLineReceived;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Add a new NMEA decoder to the GPS.
        /// </summary>
        /// <param name="decoder">NMEA decoder.</param>
        public void AddDecoder(NMEADecoder decoder)
        {
            if (_decoders.Contains(decoder.Prefix))
            {
                throw new Exception(decoder.Prefix + " already registered.");
            }
            _decoders.Add(decoder.Prefix, decoder);
        }

        /// <summary>
        /// Convert a byte into the hex representation of the value.
        /// </summary>
        /// <param name="b">Value to convert.</param>
        /// <returns>two hexadecimal digits representing the byte.</returns>
        public static string HexadecimalDigits(byte b)
        {
        	char[] digits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };
        	return ("" + digits[b >> 4] + digits[b & 0xf]);
        }

        /// <summary>
        /// Convert a byte into hexadecimal including the "0x" prefix.
        /// </summary>
        /// <param name="b">Value to convert.</param>
        /// <returns>Hexadecimal string including the 0x prefix.</returns>
        public static string Hexadecimal(byte b)
        {
        	return ("0x" + HexadecimalDigits(b));
        }

        /// <summary>
        /// Convert an unsigned short into hexadecimal.
        /// </summary>
        /// <param name="us">Unsigned short value to convert.</param>
        /// <returns>Hexadecimal reporesentation of the unsigned short.</returns>
        public static string Hexadecimal(ushort us)
        {
        	return ("0x" + HexadecimalDigits((byte)((us >> 8) & 0xff))) + HexadecimalDigits((byte)(us & 0xff));
        }

        /// <summary>
        /// Calculate a checksum for the string by XORing the bytes in the string.
        /// </summary>
        /// <param name="data">String to calculate the checksum for.</param>
        /// <returns>XOR checksum for the sting.</returns>
        public static byte XORChecksum(string data)
        {
        	return (XORChecksum(Encoding.UTF8.GetBytes(data)));
        }

        /// <summary>
        /// Generate a checksum by XORing all of the data in the array.
        /// </summary>
        /// <param name="data">Data to calculate the checksum for.</param>
        /// <returns>XOR Checksum of the array of bytes.</returns>
        public static byte XORChecksum(byte[] data)
        {
        	byte checksum = 0;
        	for (int index = 0; index < data.Length; index++)
        	{
        		checksum ^= data[index];
        	}
        	return (checksum);
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
        void _gps_OnLineReceived(string line)
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
                            NMEADecoder decoder = (NMEADecoder)_decoders[elements[0]];
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
