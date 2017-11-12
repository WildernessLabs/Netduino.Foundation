namespace Netduino.Foundation.Displays
{
    /// <summary>
    /// Define the interface for the display object.
    /// </summary>
    public interface IDisplay
    {
        /// <summary>
        /// Transfer the contents of the buffer to the display.
        /// </summary>
        void Show();

        /// <summary>
        /// Clear the display.
        /// </summary>
        /// <param name="updateDisplay">Update the dipslay once the buffer has been cleared when true.</param>
        void Clear(bool updateDisplay = false);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="colored"></param>
        void DrawPixel(byte x, byte y, bool colored);

        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="colored"></param>
        void DrawPixel(int x, int y, bool colored);
    }
}