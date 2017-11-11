using System;

namespace Netduino.Foundation
{
    /// <summary>
    ///     Provide a mechanism to convert from on type (typically string) to
    ///     another .NET type.
    ///     This class provide methods that are available in .NET not in NETMF, for
    ///     example double.TryParse is available, but int.TryParse is not.
    /// </summary>
    public class Converters
    {
        #region Methods

        /// <summary>
        ///     Parse a string and return the integer representation of the string or the
        ///     default value.
        /// </summary>
        /// <param name="value">String containing the value to be converted.</param>
        /// <param name="defaultValue">Default value in the case where the string cannot be parsed.</param>
        /// <returns>Integer representation of the string or the default value.</returns>
        public static int Integer(string value, int defaultValue = 0)
        {
            double result;

            if (!double.TryParse(value, out result))
            {
                result = defaultValue;
            }
            return (int) result;
        }

        /// <summary>
        ///     Parse a string and return the double representation of the string or the
        ///     default value.
        /// </summary>
        /// <param name="value">String containing the value to be converted.</param>
        /// <param name="defaultValue">Default value in the case where the string cannot be parsed.</param>
        /// <returns>Double representation of the string or the default value.</returns>
        public static double Double(string value, double defaultValue = 0.0)
        {
            double result;

            if (!double.TryParse(value, out result))
            {
                result = defaultValue;
            }
            return result;
        }

        /// <summary>
        ///     Convert a BCD value in a byte into a decimal representation.
        /// </summary>
        /// <param name="bcd">BCD value to decode.</param>
        /// <returns>Decimal version of the BCD value.</returns>
        public static byte BCDToByte(byte bcd)
        {
            var result = bcd & 0x0f;
            result += (bcd >> 4) * 10;
            return (byte) (result & 0x0f);
        }

        /// <summary>
        ///     Convert a byte to BCD.
        /// </summary>
        /// <returns>BCD encoded version of the byte value.</returns>
        /// <param name="v">Byte value to encode.</param>
        public static byte ByteToBCD(byte v)
        {
            if (v > 99)
            {
                throw new ArgumentException("v", "Value to encode should be in the range 0-99 inclusive.");
            }
            var result = (v % 10) << 4;
            result += v / 10;
            return (byte) (result & 0xff);
        }

        #endregion Methods
    }
}