using System;
using System.Text;
using Microsoft.SPOT;

namespace Netduino.Foundation.Core
{
    public class Helpers
    {
        /// <summary>
        /// Convert a byte into the hex representation of the value.
        /// </summary>
        /// <param name="b">Value to convert.</param>
        /// <returns>two hexadecimal digits representing the byte.</returns>
        public static string HexadecimalDigits(byte b)
        {
            char[] digits = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'a', 'b', 'c', 'd', 'e', 'f' };
            return("" + digits[b >> 4] + digits[b & 0xf]);
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
            return ("0x" + HexadecimalDigits((byte) ((us >> 8) & 0xff))) + HexadecimalDigits((byte) (us & 0xff));
        }

        /// <summary>
        /// Dump the array of bytes to the debug output in hexadeciaml.
        /// </summary>
        /// <param name="startAddress">Starting address of the register.</param>
        /// <param name="registers">Byte array of the register contents.</param>
        public static void DisplayRegisters(byte startAddress, byte[] registers)
        {
            byte start = startAddress;
            start &= 0xf0;
            string line = string.Empty;
            Debug.Print("       0    1    2    3    4    5    6    7    8    9    a    b    c    d    e    f");
            for (byte index = start; index < startAddress + registers.Length; index++)
            {
                if ((index % 16) == 0)
                {
                    if (line != string.Empty)
                    {
                        Debug.Print(line);
                    }
                    line = Hexadecimal(index) + ": ";
                }
                if (index >= startAddress)
                {
                    line += Hexadecimal(registers[index - startAddress]) + " ";
                }
                else
                {
                    line += "     ";
                }
            }
            if (line != string.Empty)
            {
                Debug.Print(line);
            }
        }

        /// <summary>
        /// Calculate a checksum for the string by XORing the bytes in the string.
        /// </summary>
        /// <param name="data">String to calculate the checksum for.</param>
        /// <returns>XOR checksum for the sting.</returns>
        public static byte XORChecksum(string data)
        {
            return XORChecksum(Encoding.UTF8.GetBytes(data));
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
            return(checksum);
        }
    }
}
