namespace Netduino.Foundation.Displays
{
    /// <summary>
    ///     Define the interface for the display object.
    /// </summary>
    public abstract class DisplayBase
    {
        /// <summary>
        /// 
        /// </summary>
        public enum BitmapMode
        {
            And,
            Or,
            XOr,
            Copy
        };

        /// <summary>
        ///     Indicate of the hardware driver should ignore out of bounds pixels
        ///     or if the driver should generate an exception.
        /// </summary>
        public bool IgnoreOutofBoundsPixels { get; set; }

        /// <summary>
        ///     Transfer the contents of the buffer to the display.
        /// </summary>
        public abstract void Show();

        /// <summary>
        ///     Clear the display.
        /// </summary>
        /// <param name="updateDisplay">Update the dipslay once the buffer has been cleared when true.</param>
        public abstract void Clear(bool updateDisplay = false);

        public abstract void DrawPixel(int x, int y, Color color);

        /// <summary>
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="colored"></param>
        public abstract void DrawPixel(int x, int y, bool colored);

        /// <summary>
        ///     Copy a bitmap to the display.
        /// </summary>
        /// <param name="x">Abscissa of the top left corner of the bitmap.</param>
        /// <param name="y">Ordinate of the top left corner of the bitmap.</param>
        /// <param name="width">Width of the bitmap.</param>
        /// <param name="height">Height of the bitmap.</param>
        /// <param name="bitmap">Bitmap to transfer</param>
        /// <param name="bitmapMode">How should the bitmap be transferred to the display?</param>
        public abstract void DrawBitmap(int x, int y, int width, int height, byte[] bitmap, BitmapMode bitmapMode);

        public abstract void DrawBitmap(int x, int y, int width, int height, byte[] bitmap, Color color);
    }
}