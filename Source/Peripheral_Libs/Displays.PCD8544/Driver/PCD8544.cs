using System;
using Microsoft.SPOT.Hardware;

namespace Netduino.Foundation.Displays
{
    public class PCD8544 : DisplayBase
    {
        public override DisplayColorMode ColorMode => DisplayColorMode.Format1bpp;

        public override uint Height => 48;

        public override uint Width => 84;

        public bool InvertDisplay
        {
            get { return _invertDisplay; }
            set { Invert(value); }
        }
        protected bool _invertDisplay = false;

        protected OutputPort dataCommandPort;
        protected OutputPort resetPort;
        protected SPI spi;

        protected byte[] spiBuffer;

        public PCD8544(Cpu.Pin chipSelectPin, Cpu.Pin dcPin, Cpu.Pin resetPin,
            SPI.SPI_module spiModule = SPI.SPI_module.SPI1,
            uint speedKHz = 4000)
        {
            spiBuffer = new byte[Width * Height / 8];

            dataCommandPort = new OutputPort(dcPin, true);
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

        private void Initialize()
        {
            resetPort.Write(false);
            resetPort.Write(true);

            dataCommandPort.Write(false);

            // spi.Write(new byte[] { 0x21, 0xBF, 0x04, 0x14, 0x0C, 0x20, 0x0C });

            spi.Write(new byte[]
            {
                0x21, // LCD Extended Commands.
                0xBF, // Set LCD Vop (Contrast). //0xB0 for 5V, 0XB1 for 3.3v, 0XBF if screen too dark
                0x04, // Set Temp coefficient. //0x04
                0x14, // LCD bias mode 1:48. //0x13 or 0X14
                0x0D, // LCD in normal mode. 0x0d for inverse
                0x20, // We must send 0x20 before modifying the display control mode
                0x0C // Set display control, normal mode. 0x0D for inverse, 0x0C for normal
            });

            dataCommandPort.Write(true);

            Clear();
            Show();
        }
        
        public override void Clear(bool updateDisplay = false)
        {
            spiBuffer = new byte[Width * Height / 8];

            dataCommandPort.Write(false);
            spi.Write(new byte[] { 0x80, 0x40 });
            dataCommandPort.Write(true);
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
        ///     Coordinates start with index 0
        /// </summary>
        /// <param name="x">Abscissa of the pixel to the set / reset.</param>
        /// <param name="y">Ordinate of the pixel to the set / reset.</param>
        /// <param name="colored">True = turn on pixel, false = turn off pixel</param>
        public override void DrawPixel(int x, int y, bool colored)
        {
            if (x < 0 || x >= 84 || y < 0 || y >= 48)
                return; // out of the range! return true to indicate failure.

            ushort index = (ushort)((x % 84) + (int)(y * 0.125) * 84);

            byte bitMask = (byte)(1 << (y % 8));

            if (colored)
                spiBuffer[index] |= bitMask;
            else
                spiBuffer[index] &= (byte)~bitMask;
        }

        public override void DrawPixel(int x, int y, Color color)
        {
            var colored = (color == Color.Black) ? false : true;

            DrawPixel(x, y, colored);
        }

        public override void Show()
        {
            spi.Write(spiBuffer);
        }

        private void Invert(bool inverse)
        {
            _invertDisplay = inverse;
            dataCommandPort.Write(false);
            spi.Write(inverse ? new byte[] { 0x0D } : new byte[] { 0x0C });
            dataCommandPort.Write(true);
        }
    }
}