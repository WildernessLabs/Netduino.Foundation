using System;
using Microsoft.SPOT.Hardware;
using Netduino.Foundation.Devices;

namespace Netduino.Foundation.ICs
{
    /// <summary>
    ///     Provide an interface to connect to a 74595 shift register.
    /// </summary>
    /// <remarks>
    ///     Control the outputs from a 74595 shift register (or a chain of shift registers)
    ///     using a SPI interface.
    /// </remarks>
    public class ShiftRegister74595
    {
        #region Member variables / fields

        /// <summary>
        ///     Array containing the bits to be output to the shift register.
        /// </summary>
        private readonly bool[] _bits;

        /// <summary>
        ///     Number of chips required to implement this ShiftRegister.
        /// </summary>
        private readonly int _numberOfChips;

        /// <summary>
        ///     SPI interface used to communicate with the shift registers.
        /// </summary>
        private readonly ICommunicationBus _spi;

        #endregion Member variables / fields

        #region Constructor(s)

        /// <summary>
        ///     Default constructor.
        /// </summary>
        /// <remarks>
        ///     This is private to prevent the programmer from calling it explicitly.
        /// </remarks>
        private ShiftRegister74595()
        {
        }

        /// <summary>
        ///     Constructor a ShiftRegister74595 object.
        /// </summary>
        /// <param name="bits">Number of bits in the shift register (should be a multiple of 8 bits).</param>
        /// <param name="config">SPI Configuration object.</param>
        public ShiftRegister74595(int bits, SPI.Configuration config)
        {
            if ((bits > 0) && ((bits % 8) == 0))
            {
                _bits = new bool[bits];
                _numberOfChips = bits / 8;
                Clear();
                _spi = new SPIBus(config);
            }
            else
            {
                throw new ArgumentOutOfRangeException(
                    "ShiftRegister74595: Size must be greater than zero and a multiple of 8 bits");
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Overload the index operator to allow the user to get/set a particular
        ///     bit in the shift register.
        /// </summary>
        /// <param name="bit">Bit number to get/set.</param>
        /// <returns>Value in the specified bit.</returns>
        public bool this[int bit]
        {
            get
            {
                if ((bit >= 0) && (bit < _bits.Length))
                {
                    return _bits[bit];
                }
                throw new IndexOutOfRangeException("ShiftRegister74595: Bit index out of range.");
            }
            set
            {
                if ((bit >= 0) && (bit < _bits.Length))
                {
                    _bits[bit] = value;
                }
                else
                {
                    throw new IndexOutOfRangeException("ShiftRegister74595: Bit index out of range.");
                }
            }
        }

        /// <summary>
        ///     Clear all of the bits in the shift register.
        /// </summary>
        /// <param name="latch">If true, latch the data after the shift register is cleared (default is false)?</param>
        public void Clear(bool latch = false)
        {
            for (var index = 0; index < _bits.Length; index++)
            {
                _bits[index] = false;
            }
            if (latch)
            {
                LatchData();
            }
        }

        /// <summary>
        ///     Send the data to the SPI interface.
        /// </summary>
        public void LatchData()
        {
            var data = new byte[_numberOfChips];

            for (var chip = 0; chip < _numberOfChips; chip++)
            {
                data[chip] = 0;
                byte bitValue = 1;
                var offset = chip * 8;
                for (var bit = 0; bit < 8; bit++)
                {
                    if (_bits[offset + bit])
                    {
                        data[chip] |= bitValue;
                    }
                    bitValue <<= 1;
                }
            }
            _spi.WriteBytes(data);
        }

        #endregion
    }
}