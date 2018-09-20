using System;
using Netduino.Foundation.Communications;
using Microsoft.SPOT.Hardware;
using System.Threading;

namespace Netduino.Foundation.Displays
{
    /// <summary>
    ///     Provide an interface to the SSD1306 family of OLED displays.
    /// </summary>
    public class ILI9163 : DisplayBase, IDisposable
    {
        #region Enums
        public enum LcdCommand
        {
            CASET = 0x2A,
            RASET = 0x2B,
            RAMWR = 0x2C
        };

      /*  public enum Colors
        {   
            Black = 0x0000,//black
            Blue = 0x001F, (1F = 31 or 6 bit ... makes sense)
            Red = 0xF800,
            Green = 0x07E0,
            Cyan = 0x07FF, 
            Magenta = 0xF81F,
            Yellow = 0xFFE0,
            White = 0xFFFF //white
        }*/
        #endregion
        public bool AutoRefreshScreen { get; set; }

        public const byte Width = 128;
        public const byte Height = 160;

        public readonly byte[] spiBuffer = new byte[Width * Height * sizeof(ushort)];

        protected readonly byte[] spiBOneByteBuffer = new byte[1];
        protected OutputPort dataCommandPort;
        protected OutputPort resetPort;
        protected SPI spi;

        private const bool Data = true;
        private const bool Command = false;

        private ILI9163()
        {

        }

        public ILI9163(Cpu.Pin chipSelectPin, Cpu.Pin dcPin, Cpu.Pin resetPin, SPI.SPI_module spiModule = SPI.SPI_module.SPI1, uint speedKHz = (uint)9500)
        {
            AutoRefreshScreen = false;

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
                        if((b & mask) > 0)
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
            if (AutoRefreshScreen)
            {
                Refresh();
            }
        }

        public void DrawPixel(int x, int y, byte r, byte g, byte b)
        {
            SetPixel(x, y, Get16BitColorFromRGB(r, g, b));
            if (AutoRefreshScreen)
            {
                Refresh();
            }
        }

        private void SetPixel(int x, int y, ushort color)
        {
            if ((x < 0) || (x >= Width) || (y < 0) || (y >= Height)) return;
            var index = ((y * Width) + x) * sizeof(ushort);
            spiBuffer[index] = (byte)(color >> 8);
            spiBuffer[++index] = (byte)(color);
        }

        public void Refresh()
        {
            spi.Write(spiBuffer);
        }

        private ushort Get16BitColorFromRGB(int red, int green, int blue)
        {
            red >>= 3;
            green >>= 2;
            blue >>= 3;

            red &= 0x1F;
            ushort value = (ushort)red;
            value <<= 6;
            green &= 0x3F;
            value |= (byte)green;
            value <<= 5;
            blue &= 0x1F;
            value |= (byte)blue;
            return value;
        }

        private ushort Get16BitColorFromColor(Color color)
        {
            //this seems heavy
            int red = (int)(color.R * 255.0);
            int green = (int)(color.G * 255.0);
            int  blue = (int)(color.B * 255.0);

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

        private void Initialize()
        {
            resetPort.Write(true);
            Thread.Sleep(50);
            resetPort.Write(false);
            Thread.Sleep(50);
            resetPort.Write(true);
            Thread.Sleep(50);

            dataCommandPort.Write(Command); //software reset
            Write(0x01);

            dataCommandPort.Write(Command); //exit sleep mode
            Write(0x11);

            dataCommandPort.Write(Command); //set pixel format
            Write(0x3A);
            dataCommandPort.Write(Data);
            Write(0x05);

            dataCommandPort.Write(Command);
            Write(0x26);
            dataCommandPort.Write(Data);
            Write(0x04);

            dataCommandPort.Write(Command);
            Write(0x36);
            dataCommandPort.Write(Data);
            Write(0x00); //set to RGB

            dataCommandPort.Write(Command);
            Write(0xF2);
            dataCommandPort.Write(Data);
            Write(0x01);

            dataCommandPort.Write(Command);
            Write(0xE0);
            dataCommandPort.Write(Data);
            Write(0x04);
            Write(0x3F);
            Write(0x25);
            Write(0x1C);
            Write(0x1E);
            Write(0x20);
            Write(0x12);
            Write(0x2A);
            Write(0x90);
            Write(0x24);
            Write(0x11);
            Write(0x00);
            Write(0x00);
            Write(0x00);
            Write(0x00);
            Write(0x00); // Positive Gamma

            dataCommandPort.Write(Command);
            Write(0xE1);
            dataCommandPort.Write(Data);
            Write(0x20);
            Write(0x20);
            Write(0x20);
            Write(0x20);
            Write(0x05);
            Write(0x00);
            Write(0x15);
            Write(0xA7);
            Write(0x3D);
            Write(0x18);
            Write(0x25);
            Write(0x2A);
            Write(0x2B);
            Write(0x2B);
            Write(0x3A); // Negative Gamma

            dataCommandPort.Write(Command);
            Write(0xB1);
            dataCommandPort.Write(Data);
            Write(0x08);
            Write(0x08); // Frame rate control 1

            dataCommandPort.Write(Command);
            Write(0xB4);
            dataCommandPort.Write(Data);
            Write(0x07);      // Display inversion

            dataCommandPort.Write(Command);
            Write(0xC0);
            dataCommandPort.Write(Data);
            Write(0x0A);
            Write(0x02); // Power control 1

            dataCommandPort.Write(Command);
            Write(0xC1);
            dataCommandPort.Write(Data);
            Write(0x02);       // Power control 2

            dataCommandPort.Write(Command);
            Write(0xC5);
            dataCommandPort.Write(Data);
            Write(0x50);
            Write(0x5B); // Vcom control 1

            dataCommandPort.Write(Command);
            Write(0xC7);
            dataCommandPort.Write(Data);
            Write(0x40);       // Vcom offset

            dataCommandPort.Write(Command);
            Write(0x2A);
            dataCommandPort.Write(Data);
            Write(0x00);
            Write(0x00);
            Write(0x00);
            Write(0x7F);
            Thread.Sleep(250); // Set column address

            dataCommandPort.Write(Command);
            Write(0x2B);
            dataCommandPort.Write(Data);
            Write(0x00);
            Write(0x00);
            Write(0x00);
            Write(0x9F);           // Set page address

            dataCommandPort.Write(Command);
            Write(0x36);
            dataCommandPort.Write(Data);
            Write(0xC0);       // Set address mode

            dataCommandPort.Write(Command);
            Write(0x29);           // Set display on
            Thread.Sleep(10);

            SetAddressWindow(0, 0, Width - 1, Height - 1);

            dataCommandPort.Write(Data);
        }

        private void SetAddressWindow(byte x0, byte y0, byte x1, byte y1)
        {
            dataCommandPort.Write(Command);
            Write((byte)LcdCommand.CASET);  // column addr set
            dataCommandPort.Write(Data);
            Write(0x00);
            Write((byte)(x0));   // XSTART 
            Write(0x00);
            Write((byte)(x1));   // XEND

            dataCommandPort.Write(Command);
            Write((byte)LcdCommand.RASET);  // row addr set
            dataCommandPort.Write(Data);
            Write(0x00);
            Write((byte)(y0));    // YSTART
            Write(0x00);
            Write((byte)(y1));    // YEND

            dataCommandPort.Write(Command);
            Write((byte)LcdCommand.RAMWR);  // write to RAM */
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
            var Half = Height / 2;
            while (++line < Half - 1)
            {
                Array.Copy(spiBuffer, 0, spiBuffer, index, 256);
                index += 256;
            }

            Array.Copy(spiBuffer, 0, spiBuffer, index, spiBuffer.Length / 2);

            if (AutoRefreshScreen)
            {
                Refresh();
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