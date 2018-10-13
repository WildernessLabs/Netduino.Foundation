﻿using System;
using Netduino.Foundation.Communications;

namespace Netduino.Foundation.Displays
{
    /// <summary>
    ///     Provide an interface to the SSD1306 family of OLED displays.
    /// </summary>
    public class SSD1306 : DisplayBase
    {
        #region Enums

        /// <summary>
        ///     Allow the programmer to set the scroll direction.
        /// </summary>
        public enum ScrollDirection
        {
            /// <summary>
            ///     Scroll the display to the left.
            /// </summary>
            Left,

            /// <summary>
            ///     Scroll the display to the right.
            /// </summary>
            Right,

            /// <summary>
            ///     Scroll the display from the bottom left and vertically.
            /// </summary>
            RightAndVertical,

            /// <summary>
            ///     Scroll the display from the bottom right and vertically.
            /// </summary>
            LeftAndVertical
        }

        /// <summary>
        ///     Supported display types.
        /// </summary>
        public enum DisplayType
        {
            /// <summary>
            ///     0.96 128x64 pixel display.
            /// </summary>
            OLED128x64,

            /// <summary>
            ///     0.91 128x32 pixel display.
            /// </summary>
            OLED128x32,

            /// <summary>
            ///     64x48 pixel display.
            /// </summary>
            //OLED64x48, (coming soon)
        }

        #endregion Enums

        #region Member variables / fields

        public override DisplayColorMode ColorMode => DisplayColorMode.Format1bpp;

        public override uint Width => _width;

        public override uint Height => _height;

        /// <summary>
        ///     SSD1306 display.
        /// </summary>
        private readonly ICommunicationBus _ssd1306;

        /// <summary>
        ///     Width of the display in pixels.
        /// </summary>
        private readonly uint _width;

        /// <summary>
        ///     Height of the display in pixels.
        /// </summary>
        private readonly uint _height;

        /// <summary>
        ///     Buffer holding the pixels in the display.
        /// </summary>
        private readonly byte[] _buffer;

        /// <summary>
        ///     Sequence of command bytes that must be sent to the display before
        ///     the Show method can send the data buffer.
        /// </summary>
        private readonly byte[] _showPreamble;

        /// <summary>
        ///     Sequence of bytes that should be sent to a 128x64 OLED display to setup the device.
        /// </summary>
        private readonly byte[] _oled128x64SetupSequence =
        {
            0xae, 0xd5, 0x80, 0xa8, 0x3f, 0xd3, 0x00, 0x40 | 0x0, 0x8d, 0x14, 0x20, 0x00, 0xa0 | 0x1, 0xc8, 0xda,
            0x12, 0x81, 0xcf, 0xd9, 0xf1, 0xdb, 0x40, 0xa4, 0xa6, 0xaf
        };

        /// <summary>
        ///     Sequence of bytes that should be sent to a 128x32 OLED display to setup the device.
        /// </summary>
        private readonly byte[] _oled128x32SetupSequence =
        {
            0xae, 0xd5, 0x80, 0xa8, 0x1f, 0xd3, 0x00, 0x40 | 0x0, 0x8d, 0x14, 0x20, 0x00, 0xa0 | 0x1, 0xc8,
            0xda, 0x02, 0x81, 0x8f, 0xd9, 0x1f, 0xdb, 0x40, 0xa4, 0xa6, 0xaf
        };

        #endregion Member variables / fields

        #region Properties

        /// <summary>
        ///     Backing variable for the InvertDisplay property.
        /// </summary>
        private bool _invertDisplay;

        /// <summary>
        ///     Invert the entire display (true) or return to normal mode (false).
        /// </summary>
        /// <remarks>
        ///     See section 10.1.10 in the datasheet.
        /// </remarks>
        public bool InvertDisplay
        {
            get { return _invertDisplay; }
            set
            {
                _invertDisplay = value;
                if (value)
                {
                    SendCommand(0xa7);
                }
                else
                {
                    SendCommand(0xa6);
                }
            }
        }

        /// <summary>
        ///     Backing variable for the Contrast property.
        /// </summary>
        private byte _contrast;

        /// <summary>
        ///     Get / Set the contrast of the display.
        /// </summary>
        public byte Contrast
        {
            get { return _contrast; }

            set
            {
                _contrast = value;
                SendCommands(new byte[] { 0x81, _contrast });
            }
        }

        /// <summary>
        ///     Put the display to sleep (turns the display off).
        /// </summary>
        public bool Sleep
        {
            get { return(_sleep); }
            set
            {
                _sleep = value;
                if (_sleep)
                {
                    SendCommand(0xae);
                }
                else
                {
                    SendCommand(0xaf);
                }
            }
        }

        /// <summary>
        ///     Backing variable for the Sleep property.
        /// </summary>
        private bool _sleep;

        #endregion Properties

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
        ///     This can be changed by setting the <seealso cref="IgnoreOutOfBoundsPixels" />
        ///     property to true.
        /// </remarks>
        /// <param name="address">Address of the bus on the I2C display.</param>
        /// <param name="speed">Speed of the I2C bus.</param>
        /// <param name="displayType">Type of SSD1306 display (default = 128x64 pixel display).</param>
        public SSD1306(byte address = 0x3c, ushort speed = 400, DisplayType displayType = DisplayType.OLED128x64)
        {
            var display = new I2CBus(address, speed);
            _ssd1306 = display;
            switch (displayType)
            {
                case DisplayType.OLED128x64:
                    _width = 128;
                    _height = 64;
                    SendCommands(_oled128x64SetupSequence);
                    break;
                case DisplayType.OLED128x32:
                    _width = 128;
                    _height = 32;
                    SendCommands(_oled128x32SetupSequence);
                    break;
            }
            var pages = _height / 8;
            _buffer = new byte[_width * pages];
            _showPreamble = new byte[] { 0x21, 0x00, (byte) (_width - 1), 0x22, 0x00, (byte) (pages - 1) };
            IgnoreOutOfBoundsPixels = false;
            //
            //  Finally, put the display into a known state.
            //
            InvertDisplay = false;
            Sleep = false;
            Contrast = 0xff;
            StopScrolling();
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
                Show();
        }

        public override void DrawPixel(int x, int y, Color color)
        {
            var colored = (color == Color.Black) ? false : true;

            DrawPixel(x, y, colored);
        }

        /// <summary>
        ///     Coordinates start with index 0
        /// </summary>
        /// <param name="x">Abscissa of the pixel to the set / reset.</param>
        /// <param name="y">Ordinate of the pixel to the set / reset.</param>
        /// <param name="colored">True = turn on pixel, false = turn off pixel</param>
        public override void DrawPixel(int x, int y, bool colored)
        {
            if ((x >= _width) || (y >= _height))
            {
                if (!IgnoreOutOfBoundsPixels)
                {
                    throw new ArgumentException("DisplayPixel: co-ordinates out of bounds");
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
        /// <remarks>
        ///     Currently, this method only supports copying the bitmap over the contents
        ///     of the display buffer.
        /// </remarks>
        /// <param name="x">Abscissa of the top left corner of the bitmap.</param>
        /// <param name="y">Ordinate of the top left corner of the bitmap.</param>
        /// <param name="width">Width of the bitmap in bytes.</param>
        /// <param name="height">Height of the bitmap in bytes.</param>
        /// <param name="bitmap">Bitmap to transfer</param>
        /// <param name="bitmapMode">How should the bitmap be transferred to the display?</param>
        public override void DrawBitmap(int x, int y, int width, int height, byte[] bitmap, BitmapMode bitmapMode)
        {
            if ((width * height) != bitmap.Length)
            {
                throw new ArgumentException("Width and height do not match the bitmap size.");
            }
            for (var ordinate = 0; ordinate < height; ordinate++)
            {
                for (var abscissa = 0; abscissa < width; abscissa++)
                {
                    var b = bitmap[(ordinate * width) + abscissa];
                    byte mask = 0x01;
                    for (var pixel = 0; pixel < 8; pixel++)
                    {
                        DrawPixel(x + (8 * abscissa) + pixel, y + ordinate, (b & mask) > 0);
                        mask <<= 1;
                    }
                }
            }
        }

        //needs dithering code
        public override void DrawBitmap(int x, int y, int width, int height, byte[] bitmap, Color color)
        {
            DrawBitmap(x, y, width, height, bitmap, BitmapMode.And);
        }

        /// <summary>
        ///     Start the display scrollling in the specified direction.
        /// </summary>
        /// <param name="direction">Direction that the display should scroll.</param>
        public void StartScrolling(ScrollDirection direction)
        {
            StartScrolling(direction, 0x00, 0xff);
        }

        /// <summary>
        ///     Start the display scrolling.
        /// </summary>
        /// <remarks>
        ///     In most cases setting startPage to 0x00 and endPage to 0xff will achieve an
        ///     acceptable scrolling effect.
        /// </remarks>
        /// <param name="direction">Direction that the display should scroll.</param>
        /// <param name="startPage">Start page for the scroll.</param>
        /// <param name="endPage">End oage for the scroll.</param>
        public void StartScrolling(ScrollDirection direction, byte startPage, byte endPage)
        {
            StopScrolling();
            byte[] commands;
            if ((direction == ScrollDirection.Left) || (direction == ScrollDirection.Right))
            {
                commands = new byte[] { 0x26, 0x00, startPage, 0x00, endPage, 0x00, 0xff, 0x2f };
                if (direction == ScrollDirection.Left)
                {
                    commands[0] = 0x27;
                }
            }
            else
            {
                byte scrollDirection;
                if (direction == ScrollDirection.LeftAndVertical)
                {
                    scrollDirection = 0x2a;
                }
                else
                {
                    scrollDirection = 0x29;
                }
                commands = new byte[]
                    { 0xa3, 0x00, (byte) _height, scrollDirection, 0x00, startPage, 0x00, endPage, 0x01, 0x2f };
            }
            SendCommands(commands);
        }

        /// <summary>
        ///     Turn off scrolling.
        /// </summary>
        /// <remarks>
        ///     Datasheet states that scrolling must be turned off before changing the
        ///     scroll direction in order to prevent RAM corruption.
        /// </remarks>
        public void StopScrolling()
        {
            SendCommand(0x2e);
        }
        
        #endregion Methods
    }
}