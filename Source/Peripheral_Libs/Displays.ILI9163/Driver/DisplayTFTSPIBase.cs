using System;
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

        #endregion

        //these displays typicalls supports 12, 16 & 18 bit but the current driver only supports 16
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
            uint speedKHz = 9500)
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
                Clock_IdleState: false,
                Clock_Edge: true,
                Clock_RateKHz: speedKHz);

            spi = new SPI(spiConfig);

            Initialize();
        }

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

        public override void DrawPixel(int x, int y, bool colored)
        {
            SetPixel(x, y, (colored ? (ushort)(0xFFFF) : (ushort)0));
        }

        public override void DrawPixel(int x, int y, Color color)
        {
            SetPixel(x, y, Get16BitColorFromColor(color));
        }

        public void DrawPixel(int x, int y, byte r, byte g, byte b)
        {
            SetPixel(x, y, Get16BitColorFromRGB(r, g, b));
        }

        private void SetPixel(int x, int y, ushort color)
        {
            if (x < 0 || y < 0 || x >= _width || y >= _height)
                return;

            var index = ((y * _width) + x) * sizeof(ushort);

            spiBuffer[index] = (byte)(color >> 8);
            spiBuffer[++index] = (byte)(color);
        }

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
            var line = 0;
            var Half = _height / 2;
            while (++line < Half - 1)
            {
                Array.Copy(spiBuffer, 0, spiBuffer, index, 256);
                index += 256;
            }

            Array.Copy(spiBuffer, 0, spiBuffer, index, spiBuffer.Length / 2);
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