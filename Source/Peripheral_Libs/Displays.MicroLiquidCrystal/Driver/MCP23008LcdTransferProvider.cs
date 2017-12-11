// Micro Liquid Crystal Library
// http://microliquidcrystal.codeplex.com
// Appache License Version 2.0 

using System.Threading;
using FusionWare.SPOT.Hardware;

namespace Netduino.Foundation.Displays.MicroLiquidCrystal
{
    /// <summary>
    ///     MCP230008 LCD transport provider.  This class allows a LCD to be controlled
    ///     by a microcontroller using a MCP230008 output expander.
    /// </summary>
    public class MCP23008LcdTransferProvider : BaseShifterLcdTransferProvider
    {
        #region IDisposable interface

        /// <summary>
        ///     Implement the Dispose method in the IDisposable interface.
        /// </summary>
        /// <param name="disposing">True if disposing.</param>
        protected override void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    _expander.Dispose();
                }

                IsDisposed = true;
            }
        }

        #endregion IDisposable interface

        #region Methods

        /// <summary>
        ///     Output a byte to the MCP230008.
        /// </summary>
        /// <param name="output">Byte to write to the MCP230008.</param>
        protected override void SendByte(byte output)
        {
            _expander.Output(output);
        }

        #endregion Methods

        #region Member varialbes / fileds

        /// <summary>
        ///     MCP230008 output expander object.
        /// </summary>
        private readonly MCP23008Expander _expander;

        /// <summary>
        ///     This setup corresponds to connections on Adafruit's LCD backpack
        ///     http://www.ladyada.net/products/i2cspilcdbackpack/
        /// </summary>
        public static ShifterSetup DefaultSetup
        {
            get
            {
                return new ShifterSetup
                {
                    RW = ShifterPin.None, // not used
                    RS = ShifterPin.GP1,
                    Enable = ShifterPin.GP2,
                    D4 = ShifterPin.GP3,
                    D5 = ShifterPin.GP4,
                    D6 = ShifterPin.GP5,
                    D7 = ShifterPin.GP6,
                    BL = ShifterPin.GP7
                };
            }
        }

        #endregion Member variables / fields

        #region Constructors

        /// <summary>
        ///     Create a new MSP230008 LCD transport provider using the I2C bus.
        /// </summary>
        /// <param name="bus">FusionWare I2CBus object connected to the LCD and the microcontroller.</param>
        public MCP23008LcdTransferProvider(I2CBus bus) : this(bus, 0, DefaultSetup)
        {
        }

        /// <summary>
        ///     Create a new MSP230008 LCD transport provider using the I2C bus.
        /// </summary>
        /// <param name="bus">FusionWare I2CBus object connected to the LCD and the microcontroller.</param>
        /// <param name="address">Address of the MCP230008 on the I2C bus.</param>
        /// <param name="setup">ShifterSetup object defining the connections between the MCP230008 and the LCD.</param>
        public MCP23008LcdTransferProvider(I2CBus bus, ushort address, ShifterSetup setup)
            : base(setup)
        {
            _expander = new MCP23008Expander(bus, address);
            Thread.Sleep(10); // make sure bus initializes

            _expander.SetPinMode(setup.Enable, MCP23008Expander.PinMode.Output);
            _expander.SetPinMode(setup.RS, MCP23008Expander.PinMode.Output);
            _expander.SetPinMode(setup.D4, MCP23008Expander.PinMode.Output);
            _expander.SetPinMode(setup.D5, MCP23008Expander.PinMode.Output);
            _expander.SetPinMode(setup.D6, MCP23008Expander.PinMode.Output);
            _expander.SetPinMode(setup.D7, MCP23008Expander.PinMode.Output);

            if (setup.RW != ShifterPin.None)
            {
                _expander.SetPinMode(setup.RW, MCP23008Expander.PinMode.Output);
            }

            if (setup.BL != ShifterPin.None)
            {
                _expander.SetPinMode(setup.BL, MCP23008Expander.PinMode.Output);
            }
        }

        #endregion Constructors
    }
}