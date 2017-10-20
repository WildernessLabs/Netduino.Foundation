using System;

namespace Netduino.Foundation
{
    /// <summary>
    /// Provide a mechanism to convert from on type (typically string) to
    /// another .NET type.
    /// 
    /// This class provide methods that are available in .NET not in NETMF, for
    /// example double.TryParse is available, but int.TryParse is not.
    /// </summary>
    public class Converters
    {
        public Converters()
        {
        }

        /// <summary>
        /// Parse a string and return the integer representation of the string or the
        /// default value.
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
            return ((int) result);
        }

        /// <summary>
        /// Parse a string and return the double representation of the string or the
        /// default value.
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
        	return (result);
        }

    }
}
