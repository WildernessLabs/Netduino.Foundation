// Micro Liquid Crystal Library
// http://microliquidcrystal.codeplex.com
// Appache License Version 2.0 

namespace Netduino.Foundation.Displays.MicroLiquidCrystal
{
    #region Enums

    /// <summary>
    ///     Flags used to address individual general purpose pins of the port expander.
    /// </summary>
    /// <remarks>
    ///     The Flags attribute below is commented out at the moment as this causes
    ///     compilation problems in NTEMF 4.3.
    /// </remarks>
    //[Flags]
    public enum ShifterPin : byte
    {
        None = 0x00,
        GP0 = 0x01,
        GP1 = 0x02,
        GP2 = 0x04,
        GP3 = 0x08,
        GP4 = 0x10,
        GP5 = 0x20,
        GP6 = 0x40,
        GP7 = 0x80
    }

    #endregion Enums
}