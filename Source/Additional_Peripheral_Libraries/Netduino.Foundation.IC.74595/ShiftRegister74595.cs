using System;
using Microsoft.SPOT.Hardware;
using Netduino.Foundation.Core;

namespace Netduino.Foundation.IC
{
    /// <summary>
    /// Provide an interface to connect to a 74595 shift register.
    /// </summary>
    /// <remarks>
    /// Control the outputs from a 74595 shift register (or a chain of shift registers)
    /// using a SPI interface.
    /// </remarks>
    class ShiftRegister74595
    {
        #region Member variables / fields

        /// <summary>
        /// Array containing the bits to be output to the shift register.
        /// </summary>
        private bool[] _bits = null;

        /// <summary>
        /// Number of chips required to implement this ShiftRegister.
        /// </summary>
        private int _numberOfChips = 0;

        /// <summary>
        /// SPI interface used to communicate with the shift registers.
        /// </summary>
        private ICommunicationBus _spi = null;

        #endregion Member variables / fields

        #region Constructor(s)

        /// <summary>
        /// Default constructor.
        /// </summary>
        /// <remarks>
        /// This is private to prevent the programmer from calling it explicity.
        /// </remarks>
        private ShiftRegister74595()
        {
        }

        /// <summary>
        /// Constructor a ShiftRegister74595 object.
        /// </summary>
        /// <param name="bits">Number of bits in the shift register (should be a multiple of 8 bits).</param>
        /// <param name="spiModule">SPI Module the shift register is attached to.</param>
        /// <param name="speedKHz">Speed of the bus in kHz (default, 10 KHz).</param>
        /// <param name="latchPin">GPIO pin to use to latch the data into the shift register.</param>
        public ShiftRegister74595(int bits, SPI.SPI_module spiModule = SPI.SPI_module.SPI1, uint speedKHz = 10, Cpu.Pin latchPin = Cpu.Pin.GPIO_Pin8)
        {
            if ((bits > 0) && ((bits % 8) == 0))
            {
                _bits = new bool[bits];
                _numberOfChips = bits / 8;
                Clear();
                var config = new SPI.Configuration(SPI_mod: spiModule, ChipSelect_Port: latchPin,
                                                   ChipSelect_ActiveState: false, ChipSelect_SetupTime: 0,
                                                   ChipSelect_HoldTime: 0, Clock_IdleState: true, Clock_Edge: false,
                                                   Clock_RateKHz: speedKHz);

                SPIBus spi = new SPIBus(config);
                _spi = (ICommunicationBus)spi;
            }
            else
            {
                throw new ArgumentOutOfRangeException("ShiftRegister74595: Size must be greater than zero and a multiple of 8 bits");
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Overload the index operator to allow the user to get/set a particular 
        /// bit in the shift register.
        /// </summary>
        /// <param name="bit">Bit number to get/set.</param>
        /// <returns>Value in the specified bit.</returns>
        public bool this[int bit]
        {
            get
            {
                if ((bit >= 0) && (bit < _bits.Length))
                {
                    return (_bits[bit]);
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
        /// Clear all of the bits in the shift register.
        /// </summary>
        /// <param name="latch">If true, latch the data after the shift register is cleared (default is false)?</param>
        public void Clear(bool latch = false)
        {
            for (int index = 0; index < _bits.Length; index++)
            {
                _bits[index] = false;
            }
            if (latch)
            {
                LatchData();
            }
        }

        /// <summary>
        /// Send the data to the SPI interface.
        /// </summary>
        public void LatchData()
        {
            byte[] data = new byte[_numberOfChips];

            for (int chip = 0; chip < _numberOfChips; chip++)
            {
                data[chip] = 0;
                byte bitValue = 1;
                int offset = chip * 8;
                for (int bit = 0; bit < 8; bit++)
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
