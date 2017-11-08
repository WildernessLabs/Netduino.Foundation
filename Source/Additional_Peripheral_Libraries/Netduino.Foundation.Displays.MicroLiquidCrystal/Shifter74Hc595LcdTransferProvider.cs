// Micro Liquid Crystal Library
// http://microliquidcrystal.codeplex.com
// Appache License Version 2.0 

using Microsoft.SPOT.Hardware;

namespace Netduino.Foundation.Displays.MicroLiquidCrystal
{
    /// <summary>
    /// 
    /// </summary>
    public class Shifter74Hc595LcdTransferProvider : BaseShifterLcdTransferProvider
    {
        #region Enums

        /// <summary>
        ///     Define the bit order.
        /// </summary>
        public enum BitOrder
        {
            MSBFirst,
            LSBFirst
        }

        #endregion Enums

        #region Member variables / fields

        /// <summary>
        ///     Bit order for the data transmission.
        /// </summary>
        private readonly BitOrder _bitOrder;

        /// <summary>
        ///     Pin connected to the latch port on the 74595 shift register.
        /// </summary>
        private readonly OutputPort _latchPort;

        /// <summary>
        ///     SPI object used to communicate with the 74595 shift register.
        /// </summary>
        private readonly SPI _spi;

        /// <summary>
        ///     Buffer holding the data to transmit to the shift register.
        /// </summary>
        private readonly byte[] _writeBuf = new byte[1];

        #endregion Member variables / fields

        #region Properties

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>
        ///     By default bytes are sent in the following order.
        ///         +--------- 0x80 d7
        ///         |+-------- 0x40 d6
        ///         ||+------- 0x20 d5
        ///         |||+------ 0x10 d4
        ///         |||| +---- 0x08 enable  
        ///         |||| |+--- 0x04 rw  
        ///         |||| ||+-- 0x02 rs  
        ///         |||| |||+- 0x01 backlight
        ///         7654 3210
        /// </remarks>
        public static ShifterSetup DefaultSetup
        {
            get
            {
                return new ShifterSetup
                {
                    BL = ShifterPin.GP0,
                    RS = ShifterPin.GP1,
                    RW = ShifterPin.GP2,
                    Enable = ShifterPin.GP3,
                    D4 = ShifterPin.GP4,
                    D5 = ShifterPin.GP5,
                    D6 = ShifterPin.GP6,
                    D7 = ShifterPin.GP7
                };
            }
        }

        #endregion Properties

        #region Constructors

        /// <summary>
        ///     Create a new shift register (say 74595) on the specified SPI bus.
        /// </summary>
        /// <param name="spiBus">SPI module connected to the shift register.</param>
        /// <param name="latchPin">Microcontroller pin connected to the latch pin on the shift register.</param>
        /// <param name="bitOrder">Bit order of the data to be transmitted to the shift register.</param>
        public Shifter74Hc595LcdTransferProvider(SPI.SPI_module spiBus, Cpu.Pin latchPin, BitOrder bitOrder)
            : this(spiBus, latchPin, bitOrder, DefaultSetup)
        {
        }

        /// <summary>
        ///     Create a new shift register (say 74595) on the specified SPI bus.
        /// </summary>
        /// <param name="spiBus">SPI module connected to the shift register.</param>
        /// <param name="latchPin">Microcontroller pin connected to the latch pin on the shift register.</param>
        public Shifter74Hc595LcdTransferProvider(SPI.SPI_module spiBus, Cpu.Pin latchPin)
            : this(spiBus, latchPin, BitOrder.MSBFirst)
        {
        }

        /// <summary>
        ///     Create a new shift register (say 74595) on the specified SPI bus.
        /// </summary>
        /// <param name="spiBus">SPI module connected to the shift register.</param>
        /// <param name="latchPin">Microcontroller pin connected to the latch pin on the shift register.</param>
        /// <param name="bitOrder">Bit order of the data to be transmitted to the shift register.</param>
        /// <param name="setup">ShifterSetup object defining the order of the pins on the shift register and the pins on the LCD.</param>
        public Shifter74Hc595LcdTransferProvider(SPI.SPI_module spiBus, Cpu.Pin latchPin, BitOrder bitOrder,
            ShifterSetup setup)
            : base(setup)
        {
            _bitOrder = bitOrder;

            var spiConfig = new SPI.Configuration(
                Cpu.Pin.GPIO_NONE, //latchPin,
                false, // active state
                0, // setup time
                0, // hold time 
                false, // clock idle state
                true, // clock edge
                1000, // clock rate
                spiBus);

            _spi = new SPI(spiConfig);
            _latchPort = new OutputPort(latchPin, true);
        }

        #endregion Constructors

        #region IDisposable interface

        /// <summary>
        ///     Dispose of the resources allocated to this object.
        /// </summary>
        /// <param name="disposing">Dispose of the resources if true.</param>
        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    _spi.Dispose();
                    _latchPort.Dispose();
                }

                IsDisposed = true;
            }
        }

        #endregion IDisposable interface

        #region Methods

        /// <summary>
        ///     Send a byte to the shift register.
        /// </summary>
        /// <param name="output">Value to output to the shift register.</param>
        protected override void SendByte(byte output)
        {
            if (_bitOrder == BitOrder.LSBFirst)
            {
                output = ReverseBits(output);
            }

            _writeBuf[0] = output;

            _latchPort.Write(false);
            _spi.Write(_writeBuf);
            _latchPort.Write(true);
        }

        /// <summary>
        ///     Reverse the bits in the parameter.
        /// </summary>
        /// <param name="v">Byte value to be reversed.</param>
        /// <returns>Byte containing the bit in the parameter in reverse order.</returns>
        private static byte ReverseBits(byte v)
        {
            var r = v; // r will be reversed bits of v; first get LSB of v
            var s = 8 - 1; // extra shift needed at end

            for (v >>= 1; v != 0; v >>= 1)
            {
                r <<= 1;
                r |= (byte) (v & 1);
                s--;
            }
            r <<= s; // shift when v's highest bits are zero
            return r;
        }

        #endregion Methods
    }
}