using System;
using Netduino.Foundation.Devices;

namespace Netduino.Foundation.Displays
{
    /// <summary>
    /// </summary>
    public class SSD1306 : DisplayBase
    {
        #region Member variables / fields

        /// <summary>
        ///     SSD1306 display.
        /// </summary>
        private readonly ICommunicationBus _ssd1306;

        /// <summary>
        ///     Width of the display in pixels.
        /// </summary>
        private readonly int _width;

        /// <summary>
        ///     Height of the display in pixels.
        /// </summary>
        private readonly int _height;

        /// <summary>
        ///     Number of pages in the display.
        /// </summary>
        private readonly int _pages;

        /// <summary>
        ///     Buffer holding the pixels in the display.
        /// </summary>
        private readonly byte[] _buffer;

        /// <summary>
        ///     Sequence of cammnd bytes that must be set to the display before
        ///     the Show method can send the data buffer.
        /// </summary>
        private readonly byte[] _showPreamble;

        #endregion Member variables / fields

        #region Constructors

        /// <summary>
        ///     Default constructor is private to prevent it being used.
        /// </summary>
        private SSD1306()
        {
        }

        /// <summary>
        ///     Create a new SSD1306 object using the default parameters for
        /// </summary>
        /// <remarks>
        ///     Note that by default, any pixels out of bounds will throw and exception.
        ///     This can be changed by setting the <seealso cref="IgnoreOutOfBoundsPixels"/>
        ///     property to true.
        /// </remarks>
        /// <param name="address">Address of the bus on the I2C display.</param>
        /// <param name="speed">Speed of the I2C bus.</param>
        /// <param name="width">Width of the display (default to 128 pixels).</param>
        /// <param name="height">Height of the display (default to 64 pixels).</param>
        public SSD1306(byte address = 0x3c, ushort speed = 400, ushort width = 128, ushort height = 64)
        {
            var display = new I2CBus(address, speed);
            _ssd1306 = display;
            _width = width;
            _height = height;
            _pages = _height / 8;
            _buffer = new byte[width * _pages];
            IgnoreOutofBoundsPixels = false;
            //
            //  Now setup the display.
            //
            SendCommands(new byte[]
            {
                0xae, 0xd5, 0x80, 0xa8, 0x3f, 0xd3, 0x00, 0x40 | 0x0, 0x8d, 0x14, 0x20, 0x00, 0xa0 | 0x1, 0xc8, 0xda,
                0x12, 0x81, 0xcf, 0xd9, 0xf1, 0xdb, 0x40, 0xa4, 0xa6, 0xaf
            });
            _showPreamble = new byte[] { 0x21, 0x00, (byte) (_width - 1), 0x22, 0x00, (byte) (_pages - 1) };
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        ///     Send a command to the display.
        /// </summary>
        /// <param name="command">Command byte to send to the display.</param>
        private void SendCommand(byte command)
        {
            _ssd1306.WriteBytes(new byte[] { 0x00, command });
        }

        /// <summary>
        ///     Send a sequence of commands to the display.
        /// </summary>
        /// <param name="commands">List of commands to send.</param>
        private void SendCommands(byte[] commands)
        {
            var data = new byte[commands.Length + 1];
            data[0] = 0x00;
            Array.Copy(commands, 0, data, 1, commands.Length);
            _ssd1306.WriteBytes(data);
        }

        /// <summary>
        ///     Send the internal pixel buffer to display.
        /// </summary>
        public override void Show()
        {
            SendCommands(_showPreamble);
            //
            //  Send the buffer page by page.
            //
            const int PAGE_SIZE = 16;
            var data = new byte[PAGE_SIZE + 1];
            data[0] = 0x40;
            for (ushort index = 0; index < _buffer.Length; index += PAGE_SIZE)
            {
                Array.Copy(_buffer, index, data, 1, PAGE_SIZE);
                SendCommand(0x40);
                _ssd1306.WriteBytes(data);
            }
        }

        /// <summary>
        ///     Clear the display buffer.
        /// </summary>
        /// <param name="updateDisplay">Immediately update the display when true.</param>
        public override void Clear(bool updateDisplay = false)
        {
            Array.Clear(_buffer, 0, _buffer.Length);
            if (updateDisplay)
            {
                Show();
            }
        }

        /// <summary>
        ///     Coordinates start with index 0
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="colored">True = turn on pixel, false = turn off pixel</param>
        public override void DrawPixel(int x, int y, bool colored)
        {
            DrawPixel((byte) x, (byte) y, colored);
        }

        /// <summary>
        ///     Coordinates start with index 0
        /// </summary>
        /// <param name="x">Abscissa of the pixel to the set / reset.</param>
        /// <param name="y">Ordinate of the pixel to the set / reset.</param>
        /// <param name="colored">True = turn on pixel, false = turn off pixel</param>
        public override void DrawPixel(byte x, byte y, bool colored)
        {
            if ((x >= _width) || (y >= _height))
            {
                if (!IgnoreOutofBoundsPixels)
                {
                    throw new ArgumentException("DisplayPixel: co-ordinates oout of bounds");
                }
                //
                //  If we get here then we have a problem but the application wants the
                //  pixels to be thrown away if out of bounds of the display.
                //
                return;
            }
            var index = ((y / 8) * _width) + x;
            if (colored)
            {
                _buffer[index] = (byte) (_buffer[index] | (byte) (1 << (y % 8)));
            }
            else
            {
                _buffer[index] = (byte) (_buffer[index] & ~(byte) (1 << (y % 8)));
            }
        }

        /// <summary>
        ///     Copy a bitmap to the display.
        /// </summary>
        /// <param name="x">Abscissa of the top left corner of the bitmap.</param>
        /// <param name="y">Ordinate of the top left corner of the bitmap.</param>
        /// <param name="width">Width of the bitmap in bytes.</param>
        /// <param name="height">Height of the bitmap in bytes.</param>
        /// <param name="bitmap">Bitmap to transfer</param>
        /// <param name="bitmapMode">How should the bitmap be transferred to the display?</param>
        public override void DrawBitmap(int x, int y, int width, int height, byte[] bitmap, BitmapMode bitmapMode)
        {
            if ((width * height != bitmap.Length))
            {
                throw new ArgumentException("Width and height do not match the bitmap size.");
            }
            for (int ordinate = 0; ordinate < height; ordinate++)
            {
                for (int abscissa = 0; abscissa < width; abscissa++)
                {
                    byte b = bitmap[(ordinate * width) + abscissa];
                    byte mask = 0x01;
                    for (int pixel = 0; pixel < 8; pixel++)
                    {
                        DrawPixel(x + (8 * abscissa) + pixel, y + ordinate, ((b & mask) > 0));
                        mask <<= 1;
                    }
                }
            }
        }

        #endregion Methods
    }
}