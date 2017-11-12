using System;

namespace Netduino.Foundation.Displays
{
    /// <summary>
    ///     Provide high level graphics functions
    /// </summary>
    public class GraphicsLibrary
    {
        #region Member variables / fields

        /// <summary>
        /// </summary>
        private readonly IDisplay _display;

        #endregion Member variables / fields

        #region Constructors

        /// <summary>
        /// </summary>
        /// <param name="display"></param>
        public GraphicsLibrary(IDisplay display)
        {
            _display = display;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        ///     Draw a line using Bresenhams line drawing algorithm.
        /// </summary>
        /// <remarks>
        ///     Bresenhams line drawing algoritm:
        ///     https://en.wikipedia.org/wiki/Bresenham's_line_algorithm
        ///     C# Implementation:
        ///     https://en.wikipedia.org/wiki/Bresenham's_line_algorithm
        /// </remarks>
        /// <param name="x0">Abscissa of the starting point of the line.</param>
        /// <param name="y0">Ordinate of the starting point of the line</param>
        /// <param name="x1">Abscissa of the end point of the line.</param>
        /// <param name="y1">Ordinate of the end point of the line</param>
        /// <param name="colored">Turn the pixel on (true) or off (false).</param>
        public void DrawLine(int x0, int y0, int x1, int y1, bool colored = true)
        {
            var steep = Math.Abs(y1 - y0) > Math.Abs(x1 - x0);
            if (steep)
            {
                int t;
                t = x0; // swap x0 and y0
                x0 = y0;
                y0 = t;
                t = x1; // swap x1 and y1
                x1 = y1;
                y1 = t;
            }
            if (x0 > x1)
            {
                int t;
                t = x0; // swap x0 and x1
                x0 = x1;
                x1 = t;
                t = y0; // swap y0 and y1
                y0 = y1;
                y1 = t;
            }
            var dx = x1 - x0;
            var dy = Math.Abs(y1 - y0);
            var error = dx / 2;
            var ystep = y0 < y1 ? 1 : -1;
            var y = y0;
            for (var x = x0; x <= x1; x++)
            {
                _display.DrawPixel(steep ? y : x, steep ? x : y, colored);
                error = error - dy;
                if (error < 0)
                {
                    y += ystep;
                    error += dx;
                }
            }
        }

        /// <summary>
        ///     Draw a horizontal line.
        /// </summary>
        /// <param name="x0">Abscissa of the starting point of the line.</param>
        /// <param name="y0">Ordinate of the starting point of the line.</param>
        /// <param name="length">Length of the line to draw.</param>
        /// <param name="colored">Turn the pixel on (true) or off (false).</param>
        public void DrawHorizontalLine(int x0, int y0, int length, bool colored = true)
        {
            for (var x = x0; (x - x0) < length; x++)
            {
                _display.DrawPixel(x, y0, colored);
            }
        }

        /// <summary>
        ///     Draw a vertical line.
        /// </summary>
        /// <param name="x0">Abscissa of the starting point of the line.</param>
        /// <param name="y0">Ordinate of the starting point of the line.</param>
        /// <param name="length">Length of the line to draw.</param>
        /// <param name="colored">Show the line when (true) or off (false).</param>
        public void DrawVerticalLine(int x0, int y0, int length, bool colored = true)
        {
            for (var y = y0; (y - y0) < length; y++)
            {
                _display.DrawPixel(x0, y, colored);
            }
        }

        /// <summary>
        ///     Draw a dircle.
        /// </summary>
        /// <remarks>
        ///     This algorithm draws the circle by splitting the full circle into eight
        ///     segments.
        ///     This method uses the Midpoint algorithm:
        ///     https://en.wikipedia.org/wiki/Midpoint_circle_algorithm
        ///     A C# implementation can be found here:
        ///     https://rosettacode.org/wiki/Bitmap/Midpoint_circle_algorithm#C.23
        /// </remarks>
        /// <param name="centerX">Abscissa of the centre point of the circle.</param>
        /// <param name="centerY">Ordinate of the centre point of the circle.</param>
        /// <param name="radius">Radius of the circle.</param>
        /// <param name="colored">Show the circle when true, </param>
        /// <param name="filled">Draw a filled circle?</param>
        public void DrawCircle(int centerX, int centerY, int radius, bool colored = true, bool filled = false)
        {
            var d = (5 - (radius * 4)) / 4;
            var x = 0;
            var y = radius;
            while (x <= y)
            {
                if (filled)
                {
                    DrawLine(centerX + x, centerY + y, centerX - x, centerY + y, colored);
                    DrawLine(centerX + x, centerY - y, centerX - x, centerY - y, colored);
                    DrawLine(centerX - y, centerY + x, centerX + y, centerY + x, colored);
                    DrawLine(centerX - y, centerY - x, centerX + y, centerY - x, colored);
                }
                else
                {
                    _display.DrawPixel(centerX + x, centerY + y, colored);
                    _display.DrawPixel(centerX + y, centerY + x, colored);
                    _display.DrawPixel(centerX - y, centerY + x, colored);
                    _display.DrawPixel(centerX - x, centerY + y, colored);
                    _display.DrawPixel(centerX - x, centerY - y, colored);
                    _display.DrawPixel(centerX - y, centerY - x, colored);
                    _display.DrawPixel(centerX + x, centerY - y, colored);
                    _display.DrawPixel(centerX + y, centerY - x, colored);
                }
                if (d < 0)
                {
                    d += (2 * x) + 1;
                }
                else
                {
                    d += (2 * (x - y)) + 1;
                    y--;
                }
                x++;
            }
        }

        /// <summary>
        ///     Draw a filled dircle.
        /// </summary>
        /// <param name="centerX">Abscissa of the centre point of the circle.</param>
        /// <param name="centerY">Ordinate of the centre point of the circle.</param>
        /// <param name="radius">Radius of the circle.</param>
        /// <param name="colored">Show the circle when true, </param>
        public void DrawFilledCircle(int centerX, int centerY, int radius, bool colored)
        {
            DrawCircle(centerX, centerY, radius, colored, true);
        }

        /// <summary>
        ///     Draw a rectangle.
        /// </summary>
        /// <param name="xLeft">Abscissa of the top left corner.</param>
        /// <param name="yTop">Ordinate of the top left corner.</param>
        /// <param name="width">Width of the rectangle.</param>
        /// <param name="height">Height of the rectangle.</param>
        /// <param name="colored">Draw the pixel (true) or turn the pixel off (false).</param>
        /// <param name="filled">Fill the rectangle (true) or draw the outline (false, default).</param>
        public void DrawRectangle(int xLeft, int yTop, int width, int height, bool colored = true, bool filled = false)
        {
            width--;
            height--;
            if (filled)
            {
                for (var i = 0; i <= height; i++)
                {
                    DrawLine(xLeft, yTop + i, xLeft + width, yTop + i, colored);
                }
            }
            else
            {
                DrawLine(xLeft, yTop, xLeft + width, yTop, colored);
                DrawLine(xLeft + width, yTop, xLeft + width, yTop + height, colored);
                DrawLine(xLeft + width, yTop + height, xLeft, yTop + height, colored);
                DrawLine(xLeft, yTop, xLeft, yTop + height, colored);
            }
        }

        /// <summary>
        ///     Draw a filled rectangle.
        /// </summary>
        /// <param name="xLeft">Abscissa of the top left corner.</param>
        /// <param name="yTop">Ordinate of the top left corner.</param>
        /// <param name="width">Width of the rectangle.</param>
        /// <param name="height">Height of the rectangle.</param>
        /// <param name="colored">Draw the pixel (true) or turn the pixel off (false).</param>
        public void DrawFilledRectangle(int xLeft, int yTop, int width, int height, bool colored = true)
        {
            DrawRectangle(xLeft, yTop, width, height, colored, true);
        }

        #endregion Methods

        #region IDisplay

        /// <summary>
        ///     Show the changes on the display.
        /// </summary>
        public void Show()
        {
            _display.Show();
        }

        /// <summary>
        ///     Clear the display.
        /// </summary>
        /// <param name="updateDisplay">Update the display immediately when true.</param>
        public void Clear(bool updateDisplay = false)
        {
            _display.Clear(updateDisplay);
        }

        #endregion IDisplay
    }
}