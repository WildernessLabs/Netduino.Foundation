using System;
using Microsoft.SPOT;

namespace Netduino.Foundation
{
    public static class BitHelpers
    {

        /// <summary>
        /// Sets a single bit in a byte
        /// </summary>
        /// <param name="mask"></param>
        /// <param name="bitIndex"></param>
        /// <param name="value"></param>
        public static byte SetBit(byte mask, byte bitIndex, byte value)
        {
            return SetBit(mask, bitIndex, (value == 0) ? false : true);
        }

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
    }
}
