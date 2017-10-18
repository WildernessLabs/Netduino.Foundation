using System;
using System.Text;
using Microsoft.SPOT;

namespace Netduino.Foundation.Helpers
{
    public class Checksum
    {
        /// <summary>
        /// Calculate a checksum for the string by XORing the bytes in the string.
        /// </summary>
        /// <param name="data">String to calculate the checksum for.</param>
        /// <returns>XOR checksum for the sting.</returns>
        public static byte XOR(string data)
        {
            return(XOR(Encoding.UTF8.GetBytes(data)));
        }

        /// <summary>
        /// Generate a checksum by XORing all of the data in the array.
        /// </summary>
        /// <param name="data">Data to calculate the checksum for.</param>
        /// <returns>XOR Checksum of the array of bytes.</returns>
        public static byte XOR(byte[] data)
        {
            byte checksum = 0;
            for (int index = 0; index < data.Length; index++)
            {
                checksum ^= data[index];
            }
            return(checksum);
        }
    }
}
