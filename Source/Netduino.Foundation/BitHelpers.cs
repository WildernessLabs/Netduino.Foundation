using System;
using Microsoft.SPOT;

namespace Netduino.Foundation
{
    public static class BitHelpers
    {

        /// <summary>
        /// Returns a new byte mask based on the input mask, with a single 
        /// bit set. To the passed in value.
        /// </summary>
        /// <param name="mask">The original byte mask value.</param>
        /// <param name="bitIndex">The index of the bit to set.</param>
        /// <param name="value">The value to set the bit. Should be 0 or 1.</param>
        public static byte SetBit(byte mask, byte bitIndex, byte value)
        {
            return SetBit(mask, bitIndex, (value == 0) ? false : true);
        }

        /// <summary>
        /// Returns a new byte mask based on the input mask, with a single 
        /// bit set. To the passed in value.
        /// </summary>
        /// <param name="mask">The original byte mask value.</param>
        /// <param name="bitIndex">The index of the bit to set.</param>
        /// <param name="value">The value to set the bit. true for 1, false for 0.</param>
        /// <returns></returns>
        public static byte SetBit(byte mask, byte bitIndex, bool value)
        {
            byte newMask = mask;

            if (value)
            {
                newMask |= (byte)(1 << bitIndex);
            }
            else
            {
                newMask &= (byte)~(1 << bitIndex); // tricky to zero
            }

            return newMask;
        }

        /// <summary>
        /// Returns the value of the bask at the given ordinal.
        /// </summary>
        /// <param name="mask"></param>
        /// <param name="bitIndex"></param>
        /// <returns></returns>
        public static bool GetBitValue(byte mask, byte bitIndex)
        {
            
            return (((mask & (byte)(1 << bitIndex)) != 0) ? true : false);
        }
    }
}
