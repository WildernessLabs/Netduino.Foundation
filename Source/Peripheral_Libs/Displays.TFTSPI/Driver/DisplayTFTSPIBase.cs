using System;
using System.Threading;
using Microsoft.SPOT.Hardware;

namespace Netduino.Foundation.Displays
{
    public abstract class DisplayTFTSPIBase : DisplayBase, IDisposable
    {
        #region Enums
        public enum LcdCommand
        {
            CASET = 0x2A,
            RASET = 0x2B,
            RAMWR = 0x2C
        };

        public enum Rotation
        {
            Normal, //zero
            Rotate_90, //in degrees
            Rotate_180,
            Rotate_270,
        }

        #endregion

        //these displays typically support 12, 16 & 18 bit but the current driver only supports 16
        public override DisplayColorMode ColorMode => DisplayColorMode.Format16bppRgb565;
        public override uint Width => _width;
        public override uint Height => _height;

        protected OutputPort dataCommandPort;
        protected OutputPort resetPort;
        protected SPI spi;

        protected readonly byte[] spiBuffer;
        protected readonly byte[] spiBOneByteBuffer = new byte[1];

        protected uint _width;
        protected uint _height;

        protected const bool Data = true;
        protected const bool Command = false;

        protected abstract void Initialize();

        internal DisplayTFTSPIBase()
        {
        }

        public DisplayTFTSPIBase(Cpu.Pin chipSelectPin, Cpu.Pin dcPin, Cpu.Pin resetPin,
            uint width, uint height,
            SPI.SPI_module spiModule = SPI.SPI_module.SPI1,
            uint speedKHz = 9500, bool idleClockState = false)
        {
            _width = width;
            _height = height;

            spiBuffer = new byte[_width * _height * sizeof(ushort)];

            dataCommandPort = new OutputPort(dcPin, false);
            resetPort = new OutputPort(resetPin, true);

            var spiConfig = new SPI.Configuration(
                SPI_mod: spiModule,
                ChipSelect_Port: chipSelectPin,
                ChipSelect_ActiveState: false,
                ChipSelect_SetupTime: 0,
                ChipSelect_HoldTime: 0,
                Clock_IdleState: idleClockState,
                Clock_Edge: true,
                Clock_RateKHz: speedKHz);
            
            spi = new SPI(spiConfig);
        }

        /// <summary>
        ///     Clear the display.
        /// </summary>
        /// <param name="updateDisplay">Update the dipslay once the buffer has been cleared when true.</param>
        public override void Clear(bool updateDisplay = false)
        {
            Clear(0, updateDisplay);
        }

        public void Clear(Color color, bool updateDisplay = false)
        {
            Clear(Get16BitColorFromColor(color), updateDisplay);
        }

        protected void Clear(ushort color, bool updateDisplay = false)
        {
            ClearScreen(color);

            if (updateDisplay)
                Refresh();
        }

        /// <summary>
        ///     Display a 1-bit bitmap
        /// 
        ///     This method simply calls a similar method in the display hardware.
        /// </summary>
        /// <param name="x">Abscissa of the top left corner of the bitmap.</param>
        /// <param name="y">Ordinate of the top left corner of the bitmap.</param>
        /// <param name="width">Width of the bitmap in bytes.</param>
        /// <param name="height">Height of the bitmap in bytes.</param>
        /// <param name="bitmap">Bitmap to display.</param>
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

        /// <summary>
        ///     Display a 1-bit bitmap
        /// 
        ///     This method simply calls a similar method in the display hardware.
        /// </summary>
        /// <param name="x">Abscissa of the top left corner of the bitmap.</param>
        /// <param name="y">Ordinate of the top left corner of the bitmap.</param>
        /// <param name="width">Width of the bitmap in bytes.</param>
        /// <param name="height">Height of the bitmap in bytes.</param>
        /// <param name="bitmap">Bitmap to display.</param>
        /// <param name="color">The color of the bitmap.</param>
        public override void DrawBitmap(int x, int y, int width, int height, byte[] bitmap, Color color)
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
                        if ((b & mask) > 0)
                            DrawPixel(x + (8 * abscissa) + pixel, y + ordinate, color);
                        mask <<= 1;
                    }
                }
            }
        }

        /// <summary>
        ///     Draw a single pixel 
        /// </summary>
        /// <param name="x">x location </param>
        /// <param name="y">y location</param>
        /// <param name="colored">Turn the pixel on (true) or off (false).</param>
        public override void DrawPixel(int x, int y, bool colored)
        {
            SetPixel(x, y, (colored ? (ushort)(0xFFFF) : (ushort)0));
        }

        /// <summary>
        ///     Draw a single pixel 
        /// </summary>
        /// <param name="x">x location </param>
        /// <param name="y">y location</param>
        /// <param name="color">Color of pixel.</param>
        public override void DrawPixel(int x, int y, Color color)
        {
            SetPixel(x, y, Get16BitColorFromColor(color));
        }

        /// <summary>
        ///     Draw a single pixel 
        /// </summary>
        /// <param name="x">x location </param>
        /// <param name="y">y location</param>
        /// <param name="r">8 bit red value</param>
        /// <param name="g">8 bit green value</param>
        /// <param name="b">8 bit blue value</param>
        public void DrawPixel(int x, int y, byte r, byte g, byte b)
        {
            SetPixel(x, y, Get16BitColorFromRGB(r, g, b));
        }

        /// <summary>
        ///     Draw a single pixel 
        /// </summary>
        /// <param name="x">x location </param>
        /// <param name="y">y location</param>
        /// <param name="color">16bpp (565) encoded color value</param>
        private void SetPixel(int x, int y, ushort color)
        {
            if (x < 0 || y < 0 || x >= _width || y >= _height)
                return;

            var index = ((y * _width) + x) * sizeof(ushort);

            spiBuffer[index] = (byte)(color >> 8);
            spiBuffer[++index] = (byte)(color);
        }

        /// <summary>
        ///     Draw the display buffer to screen
        /// </summary>
        public void Refresh()
        {
            spi.Write(spiBuffer);
        }

        private ushort Get16BitColorFromRGB(byte red, byte green, byte blue)
        {
            red >>= 3;
            green >>= 2;
            blue >>= 3;

            red &= 0x1F;
            ushort value = red;
            value <<= 6;
            green &= 0x3F;
            value |= green;
            value <<= 5;
            blue &= 0x1F;
            value |= blue;
            return value;
        }

        private ushort Get16BitColorFromColor(Color color)
        {
            //this seems heavy
            byte red = (byte)(color.R * 255.0);
            byte green = (byte)(color.G * 255.0);
            byte blue = (byte)(color.B * 255.0);

            return Get16BitColorFromRGB(red, green, blue);
        }

        public override void Show()
        {
            Refresh();
        }

        protected void Write(byte value)
        {
            spiBOneByteBuffer[0] = value;
            spi.Write(spiBOneByteBuffer);
        }

        protected void Write(byte[] data)
        {
            spi.Write(data);
        }

        protected void DelayMs(int millseconds)
        {
            Thread.Sleep(millseconds);
        }

        protected void SendCommand(byte command)
        {
            dataCommandPort.Write(Command);
            Write(command);
        }

        protected void SendData(int data)
        {
            SendData((byte)data);
        }

        protected void SendData(byte data)
        {
            dataCommandPort.Write(Data);
            Write(data);
        }

        protected void SendData(byte[] data)
        {
            dataCommandPort.Write(Data);
            spi.Write(data);
        }

        /// <summary>
        ///     Directly sets the display to a 16bpp color value
        /// </summary>
        /// <param name="color">16bpp color value (565)</param>
        public void ClearScreen(ushort color = 0)
        {
            var high = (byte)(color >> 8);
            var low = (byte)(color);

            var index = 0;
            spiBuffer[index++] = high;
            spiBuffer[index++] = low;
            spiBuffer[index++] = high;
            spiBuffer[index++] = low;
            spiBuffer[index++] = high;
            spiBuffer[index++] = low;
            spiBuffer[index++] = high;
            spiBuffer[index++] = low;
            spiBuffer[index++] = high;
            spiBuffer[index++] = low;
            spiBuffer[index++] = high;
            spiBuffer[index++] = low;
            spiBuffer[index++] = high;
            spiBuffer[index++] = low;
            spiBuffer[index++] = high;
            spiBuffer[index++] = low;

            Array.Copy(spiBuffer, 0, spiBuffer, 16, 16);
            Array.Copy(spiBuffer, 0, spiBuffer, 32, 32);
            Array.Copy(spiBuffer, 0, spiBuffer, 64, 64);
            Array.Copy(spiBuffer, 0, spiBuffer, 128, 128);
            Array.Copy(spiBuffer, 0, spiBuffer, 256, 256);

            index = 512;
            var Half = _height / 2;

            while(index < spiBuffer.Length - 256)
            {
                Array.Copy(spiBuffer, 0, spiBuffer, index, 256);
                index += 256;
            }

            while (index < spiBuffer.Length)
            {
                spiBuffer[index++] = high;
                spiBuffer[index++] = low;
            }
        }

        public void ClearWithoutFullScreenBuffer(ushort color)
        {
            var buffer = new ushort[_width];

            for (int x = 0; x < _width; x++)
            {
                buffer[x] = color;
            }

            for (int y = 0; y < _height; y++)
            {
                spi.Write(buffer);
            }
        }

        public void Dispose()
        {
            spi.Dispose();
            spi = null;
            dataCommandPort = null;
            resetPort = null;
        }
    }
}