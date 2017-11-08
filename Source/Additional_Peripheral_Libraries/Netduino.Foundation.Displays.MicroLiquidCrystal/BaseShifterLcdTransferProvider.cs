// Micro Liquid Crystal Library
// http://microliquidcrystal.codeplex.com
// Appache License Version 2.0 

using FusionWare;

namespace Netduino.Foundation.Displays.MicroLiquidCrystal
{
    /// <summary>
    /// </summary>
    public abstract class BaseShifterLcdTransferProvider : DisposableObject, ILcdTransferProvider
    {
        #region Classes / structures

        /// <summary>
        ///     Object to record the pin assignments between the shift register and
        ///     the  LCD display.
        /// </summary>
        public class ShifterSetup
        {
            public ShifterPin BL;
            public ShifterPin D4;
            public ShifterPin D5;
            public ShifterPin D6;
            public ShifterPin D7;
            public ShifterPin Enable;
            public ShifterPin RS;
            public ShifterPin RW;
        }

        #endregion Classes / structures

        #region Member variables / fields

        /// <summary>
        ///     ShfiterSetup object indicating the how the data, backlight etc lines
        ///     are connected to the shift register.
        /// </summary>
        private readonly ShifterSetup _setup;

        #endregion Member variables / fields

        #region Constructors

        /// <summary>
        ///     Constructor for the class.
        /// </summary>
        /// <param name="setup">Configuration of he pin assignmnt between the shift register and the display.</param>
        protected BaseShifterLcdTransferProvider(ShifterSetup setup)
        {
            _setup = setup;
        }

        #endregion Constructors

        #region Properties

        /// <summary>
        ///     Indicate if the driver is in four bit mode.
        /// </summary>
        public bool FourBitMode
        {
            get { return true; }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        ///     Set a data byte to the display and
        /// </summary>
        /// <param name="data">Data to be sent to the display (8-bit value).</param>
        /// <param name="mode">Indicate if this is data (true) or a command (false).</param>
        /// <param name="backlight">Indicate if the backlight should be on or off.</param>
        public void Send(byte data, bool mode, bool backlight)
        {
            var output = 0;
            if (backlight)
            {
                output |= (int) _setup.BL;
            }
            if (mode)
            {
                output |= (int) _setup.RS;
            }
            //
            //  Write the top four bits of the byte to the display.
            //
            Write4Bits(ref output, (byte) (data >> 4));
            PulseEnable(output);
            //
            // Write the bottom four bits of the byte to the display.
            //
            Write4Bits(ref output, (byte) (data & 0x0F));
            PulseEnable(output);
        }

        /// <summary>
        ///     Implement this method on derived class.
        /// </summary>
        /// <param name="output">Send a byte to the display.</param>
        protected abstract void SendByte(byte output);

        /// <summary>
        ///     Set the four data bits in the value to be written to the shift register.
        ///     The data to be written is stored in the lower four bits of the value.
        /// </summary>
        /// <param name="v">Byte that will eventually be written to the shift register.</param>
        /// <param name="data">Data byte containing the four bits to be written (value is in the lower four bits).</param>
        private void Write4Bits(ref int v, byte data)
        {
            var s = _setup;

            if ((data & 0x01) != 0)
            {
                v |= (int) s.D4;
            }
            else
            {
                v &= ~(int) s.D4;
            }
            if ((data & 0x02) != 0)
            {
                v |= (int) s.D5;
            }
            else
            {
                v &= ~(int) s.D5;
            }
            if ((data & 0x04) != 0)
            {
                v |= (int) s.D6;
            }
            else
            {
                v &= ~(int) s.D6;
            }
            if ((data & 0x08) != 0)
            {
                v |= (int) s.D7;
            }
            else
            {
                v &= ~(int) s.D7;
            }
        }

        /// <summary>
        ///     Toggler the pulse enable [in.
        /// </summary>
        /// <param name="output">Setup for this whift register (used to work out which pin is the pulse enable line).</param>
        private void PulseEnable(int output)
        {
            var enable = (int) _setup.Enable;

            output &= ~enable; // set enabled low
            SendByte((byte) output);

            output |= enable; // set enabled high
            SendByte((byte) output);

            output &= ~enable; // set enabled low
            SendByte((byte) output);
        }

        #endregion Methods
    }
}