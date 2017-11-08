// Micro Liquid Crystal Library
// http://microliquidcrystal.codeplex.com
// Appache License Version 2.0 

namespace Netduino.Foundation.Displays.MicroLiquidCrystal
{
    /// <summary>
    ///     Interface definition for the LCD transport providers.
    /// </summary>
    public interface ILcdTransferProvider
    {
        #region Properties

        /// <summary>
        ///     Specify if the provider works in 4-bit mode; 8-bit mode is used otherwise.
        /// </summary>
        bool FourBitMode { get; }

        #endregion Properties

        #region Methods

        /// <summary>
        ///     Send data to the LCD display.
        /// </summary>
        /// <param name="data">Byte to send to the display.</param>
        /// <param name="mode">Mode - true => data, false => command.</param>
        /// <param name="backlight">Backlight state, on (true) or off (false).</param>
        void Send(byte data, bool mode, bool backlight);

        #endregion Methods
    }
}