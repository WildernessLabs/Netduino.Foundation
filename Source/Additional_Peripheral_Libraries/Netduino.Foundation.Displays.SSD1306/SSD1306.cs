using System;
using Netduino.Foundation.Devices;

namespace Netduino.Foundation.Displays
{
    /// <summary>
    /// </summary>
    public class SSD1306 : IDisplay
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
        /// <param name="address">Address of the bus on the I2C display.</param>
        /// <param name="speed">Speed of the I2C bus.</param>
        /// <param name="width">Width of the display (default to 128 pixels).</param>
        /// <param name="height">Height of the display (default to 64 pixels).</param>
        public SSD1306(byte address = 60, ushort speed = 400, ushort width = 128, ushort height = 64)
        {
            var display = new I2CBus(address, speed);
            _ssd1306 = display;
            _width = width;
            _height = height;
            _pages = _height / 8;
            _buffer = new byte[width * _pages];
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
        ///     Send a sequence of commands to the display one by one.
        /// </summary>
        /// <param name="commands">List of commands to send.</param>
        private void SendCommands(byte[] commands)
        {
            var command = new byte[2];
            command[0] = 0x00;
            for (var index = 0; index < commands.Length; index++)
            {
                command[1] = commands[index];
                _ssd1306.WriteBytes(command);
            }
        }

        /// <summary>
        ///     Make the display ready for use.
        /// </summary>
        /// <remarks>
        ///     Currently only 128 x 64 bit SSD1306 OLED displays are supported.
        /// </remarks>
        public void Setup()
        {
        }

        /// <summary>
        ///     Send buffer to display
        /// </summary>
        public void Show()
        {
            SendCommands(_showPreamble);
            //
            //  Send the buffer page by page.
            //
            var data = new byte[17];
            data[0] = 0x40;
            for (ushort outer = 0; outer < _buffer.Length; outer += 16)
            {
                for (ushort inner = 0; inner < 16; inner++)
                {
                    data[inner + 1] = _buffer[outer + inner];
                }
                SendCommand(0x40);
                _ssd1306.WriteBytes(data);
            }
        }

        /// <summary>
        ///     Clear the display buffer.
        /// </summary>
        /// <param name="updateDisplay">Immediately update the display when true.</param>
        public void Clear(bool updateDisplay = false)
        {
            for (var index = 0; index < _buffer.Length; index++)
            {
                _buffer[index] = 0;
            }
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
        public void DrawPixel(int x, int y, bool colored)
        {
            DrawPixel((byte) x, (byte) y, colored);
        }

        /// <summary>
        ///     Coordinates start with index 0
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="colored">True = turn on pixel, false = turn off pixel</param>
        public void DrawPixel(byte x, byte y, bool colored)
        {
            if ((x >= _width) || (y >= _height))
            {
                throw new Exception("DisplayPixel: co-ordinates oout of bounds");
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

        #endregion Methods
    }
}