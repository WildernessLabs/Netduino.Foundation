// Micro Liquid Crystal Library
// http://microliquidcrystal.codeplex.com
// Appache License Version 2.0 

using FusionWare.SPOT.Hardware;

namespace Netduino.Foundation.Displays.MicroLiquidCrystal
{
    /// <summary>
    ///     MCP230008 output expander chip object.
    /// </summary>
    public class MCP23008Expander : I2CDeviceDriver
    {
        #region Enums

        /// <summary>
        ///     Indicate if a pin is an input or output.
        /// </summary>
        public enum PinMode : byte
        {
            Output = 0,
            Input = 1
        }

        #endregion Enums

        #region Classes / structures.

        /// <summary>
        ///     Class holding the constants that define the registers in the MCP230008
        /// </summary>
        public static class Register
        {
            /// <summary>
            ///     I/O direction
            /// </summary>
            public const byte IODIR = 0x00;

            /// <summary>
            ///     Input polarity
            /// </summary>
            public const byte IPOL = 0x01;

            /// <summary>
            ///     Interrupt-on-change control
            /// </summary>
            public const byte GPINTEN = 0x02;

            /// <summary>
            ///     Default compare for interrupt-on-change
            /// </summary>
            public const byte DEFVAL = 0x03;

            /// <summary>
            ///     Interrupt control
            /// </summary>
            public const byte INTCON = 0x04;

            /// <summary>
            ///     Configuration
            /// </summary>
            public const byte IOCON = 0x05;

            /// <summary>
            ///     Pullup resistor configuration.
            /// </summary>
            public const byte GPPU = 0x06;

            /// <summary>
            ///     Interrupt flag
            /// </summary>
            public const byte INTF = 0x07;

            /// <summary>
            ///     Interrupt capture
            /// </summary>
            public const byte INTCAP = 0x08; //(readonly)

            /// <summary>
            ///     Input Port
            /// </summary>
            public const byte GPIO = 0x09;

            /// <summary>
            ///     Output latches
            /// </summary>
            public const byte OLAT = 0x0A;
        }

        #endregion Classes / structures.

        #region Constants

        /// <summary>
        ///     Address mask to be combined with the address specified in the constructor.
        /// </summary>
        public const byte AddressMask = 0x20;

        /// <summary>
        ///     Clock speed for the I2C bus.  Possible values are 100 KHz, 400 KHz and 1.7 MHz.
        /// </summary>
        private const int ClockRateKhz = 100;

        #endregion Constants

        #region Constructors

        /// <summary>
        ///     Construct a new MCP230008Expander object on the I2C bus with the default address (0x20).
        /// </summary>
        /// <param name="bus">Fusionware I2CBus connected to the MCP230008.</param>
        public MCP23008Expander(I2CBus bus) : this(bus, 0)
        {
        }

        /// <summary>
        ///     Construct a new MCP230008Expander object on the I2C bus with the specified address.
        /// </summary>
        /// <param name="bus">Fusionware I2CBus connected to the MCP230008.</param>
        /// <param name="address">Address of the MCP230008 on the I2C bus.</param>
        public MCP23008Expander(I2CBus bus, ushort address)
            : base(bus, (ushort) (AddressMask | address), ClockRateKhz)
        {
            Reset();
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        ///     Reset to the default state.
        /// </summary>
        public void Reset()
        {
            var buffer = new byte[11];
            //
            //  Start with address of first register.
            //
            buffer[0] = Register.IODIR;
            //
            //  Set all pins as inputs.
            //
            buffer[1] = 0xFF;
            //
            //  Set all other registers to default.
            //
            for (var i = 2; i < 11; i++)
            {
                buffer[i] = 0;
            }

            Write(buffer);
        }

        /// <summary>
        ///     Set (or reset) a bit in a byte.
        /// </summary>
        /// <param name="v">Current value of the byte.</param>
        /// <param name="p">Pin to change (this is actually a mask).</param>
        /// <param name="d">Value to set or reset in the pin / bit.</param>
        public static void SetRegBit(ref byte v, ShifterPin p, bool d)
        {
            if (d)
            {
                v = (byte) (v | (byte) p);
            }
            else
            {
                v = (byte) (v & ~(byte) p);
            }
        }

        /// <summary>
        ///     Set the mode for the specified pin (input or output).
        /// </summary>
        /// <param name="pin">Pin to set the new mode.</param>
        /// <param name="mode">New mode (input or output).</param>
        public void SetPinMode(ShifterPin pin, PinMode mode)
        {
            if (pin == ShifterPin.None)
            {
                return;
            }
            var iodir = ReadReg8(Register.IODIR);
            SetRegBit(ref iodir, pin, mode == PinMode.Input);
            WriteReg8(Register.IODIR, iodir);
        }

        /// <summary>
        ///     Configure the polarity of the GPIO port bits.
        /// </summary>
        /// <param name="pin"></param>
        /// <param name="inverted">
        ///     If true, the corresponding GPIO register bit will reflect
        ///     then inverted value on the pin.
        /// </param>
        public void InputPolarity(ShifterPin pin, bool inverted)
        {
            if (pin == ShifterPin.None)
            {
                return;
            }
            var ipol = ReadReg8(Register.IPOL);
            SetRegBit(ref ipol, pin, inverted);
            WriteReg8(Register.IPOL, ipol);
        }

        /// <summary>
        ///     Control the pull-up resistors for the port pins.
        /// </summary>
        /// <param name="pin"></param>
        /// <param name="pullup">
        ///     If set to true and the corresponding pin is configured as an input,
        ///     the corresponding port pin is internally pulled up with a 100k resistor.
        /// </param>
        public void PullUp(ShifterPin pin, bool pullup)
        {
            if (pin == ShifterPin.None)
            {
                return;
            }

            var gppu = ReadReg8(Register.GPPU);
            SetRegBit(ref gppu, pin, pullup);
            WriteReg8(Register.GPPU, gppu);
        }

        /// <summary>
        ///     Read a value from a pin on the MCP230008.
        /// </summary>
        /// <param name="pin">Pin to read the currrent value.</param>
        /// <returns>Value (true or false) representing the current digital state of the pin.</returns>
        public bool DigitalRead(ShifterPin pin)
        {
            if (pin == ShifterPin.None)
            {
                return false;
            }

            var gpio = ReadReg8(Register.GPIO);
            return (gpio & (byte) pin) != 0;
        }

        /// <summary>
        ///     Modify value of output latch pin.
        /// </summary>
        /// <param name="pin">Pin to write the value to.</param>
        /// <param name="value">Value to write to the pin.</param>
        public void DigitalWrite(ShifterPin pin, bool value)
        {
            if (pin == ShifterPin.None)
            {
                return;
            }

            var gpio = ReadReg8(Register.OLAT);
            SetRegBit(ref gpio, pin, value);
            WriteReg8(Register.OLAT, gpio);
        }

        /// <summary>
        ///     Read the GPIO port from the MCP230008.
        /// </summary>
        /// <returns>8-bit value read from the GPIO port.</returns>
        public byte ReadPort()
        {
            return ReadReg8(Register.GPIO);
        }

        /// <summary>
        ///     Output the 8-bit value to latches in the MCP230008.
        /// </summary>
        /// <param name="value">Value to write to the MCP230008.</param>
        public void Output(byte value)
        {
            WriteReg8(Register.OLAT, value);
        }

        #endregion Methods
    }
}