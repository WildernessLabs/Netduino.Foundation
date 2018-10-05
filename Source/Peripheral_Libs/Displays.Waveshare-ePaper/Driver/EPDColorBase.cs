using System;
using Microsoft.SPOT.Hardware;
using System.Threading;

namespace Netduino.Foundation.Displays
{
    /// <summary>
    ///     Provide an interface to the WaveShare ePaper 2 color displays
    /// </summary>
    public abstract class EPDColorBase : SPIDisplayBase
    {
        public override DisplayColorMode ColorMode => DisplayColorMode.Format2bpp;

        protected readonly byte[] blackImageBuffer;
        protected readonly byte[] colorImageBuffer;

        protected int xRefreshStart, yRefreshStart, xRefreshEnd, yRefreshEnd;

        private EPDColorBase()
        {  }

        public EPDColorBase(Cpu.Pin chipSelectPin, Cpu.Pin dcPin, Cpu.Pin resetPin, Cpu.Pin busyPin, SPI.SPI_module spiModule = SPI.SPI_module.SPI1, uint speedKHz = (uint)9500)
        {
            dataCommandPort = new OutputPort(dcPin, false);
            resetPort = new OutputPort(resetPin, true);
            busyPort = new InputPort(busyPin, true, Port.ResistorMode.Disabled);

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

            blackImageBuffer = new byte[Width * Height / 8];
            colorImageBuffer = new byte[Width * Height / 8];

            for (int i = 0; i < blackImageBuffer.Length; i++)
            {
                blackImageBuffer[i] = 0xFF;
                colorImageBuffer[i] = 0xFF;
            }
            
            Initialize();
        }

        protected abstract void Initialize();

        protected abstract void Refresh();

        public override void Show()
        {
            Refresh();
        }

        public override void Clear(bool updateDisplay = false)
        {
            Clear(false, updateDisplay);
        }

        public void Clear(Color color, bool updateDisplay = false)
        {
            bool colored = false;
            if (color.B > 0 || color.R > 0 || color.G > 0)
                colored = true;

            Clear(colored, updateDisplay);
        }

        public void Clear(bool colored, bool updateDisplay = false)
        {
            //   ClearFrameMemory((byte)(colored ? 0 : 0xFF));
            //   DisplayFrame();

            for (int i = 0; i < blackImageBuffer.Length; i++)
            {
                blackImageBuffer[i] = colored ? (byte)0 : (byte)255;
                colorImageBuffer[i] = 255;
            }

            if (updateDisplay)
            {
                Refresh();
            }
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
            xRefreshStart = Math.Min(x, xRefreshStart);
            xRefreshEnd = Math.Max(x, xRefreshEnd);
            yRefreshStart = Math.Min(y, yRefreshStart);
            yRefreshEnd = Math.Max(y, yRefreshEnd);

            if (colored)
            {
                blackImageBuffer[(x + y * Width) / 8] &= (byte)~(0x80 >> (x % 8));
            }
            else
            {
                blackImageBuffer[(x + y * Width) / 8] |= (byte)(0x80 >> (x % 8));
            }
            colorImageBuffer[(x + y * Width) / 8] |= (byte)(0x80 >> (x % 8));
        }

        public void DrawColoredPixel(int x, int y, bool colored)
        {
            xRefreshStart = Math.Min(x, xRefreshStart);
            xRefreshEnd = Math.Max(x, xRefreshEnd);
            yRefreshStart = Math.Min(y, yRefreshStart);
            yRefreshEnd = Math.Max(y, yRefreshEnd);

            if (colored)
            {
                colorImageBuffer[(x + y * Width) / 8] &= (byte)~(0x80 >> (x % 8));
            }
            else
            {
                colorImageBuffer[(x + y * Width) / 8] |= (byte)(0x80 >> (x % 8));
            }
            blackImageBuffer[(x + y * Width) / 8] |= (byte)(0x80 >> (x % 8));
        }

        public override void DrawPixel(int x, int y, Color color)
        {
            bool colored = false;
            if (color.B == 0 && color.G == 0 && color.R > 0.5)
            {
                DrawColoredPixel(x, y, true);
            }
            else
            {
                if (color.B > 0 || color.G > 0 || color.R > 0)
                    colored = true;

                DrawPixel(x, y, colored);
            }
        }

        public void DrawPixel(int x, int y, byte r, byte g, byte b)
        {
            if (g == 0 && b == 0 && r > 127)
            {
                DrawColoredPixel(x, y, true);
            }
            else
            {
                bool colored = false;
                if (r > 0 || g > 0 || b > 0)
                    colored = true;

                DrawPixel(x, y, colored);
            }
        }
            

        // 2.13 B (red) commands
        protected static byte PANEL_SETTING                     = 0x00;
        protected static byte POWER_SETTING                     = 0x01;
        protected static byte POWER_OFF                         = 0x02;
        protected static byte POWER_OFF_SEQUENCE_SETTING        = 0x03;
        protected static byte POWER_ON                          = 0x04;
        protected static byte POWER_ON_MEASURE                  = 0x05;
        protected static byte BOOSTER_SOFT_START                = 0x06;
        protected static byte DEEP_SLEEP                        = 0x07;
        protected static byte DATA_START_TRANSMISSION_1         = 0x10;
        protected static byte DATA_STOP                         = 0x11;
        protected static byte DISPLAY_REFRESH                   = 0x12;
        protected static byte DATA_START_TRANSMISSION_2         = 0x13;
        protected static byte VCOM_LUT                          = 0x20;
        protected static byte W2W_LUT                           = 0x21;
        protected static byte B2W_LUT                           = 0x22;
        protected static byte W2B_LUT                           = 0x23;
        protected static byte B2B_LUT                           = 0x24;
        protected static byte PLL_CONTROL                       = 0x30;
        protected static byte TEMPERATURE_SENSOR_CALIBRATION    = 0x40;
        protected static byte TEMPERATURE_SENSOR_SELECTION      = 0x41;
        protected static byte TEMPERATURE_SENSOR_WRITE          = 0x42;
        protected static byte TEMPERATURE_SENSOR_READ           = 0x43;
        protected static byte VCOM_AND_DATA_INTERVAL_SETTING    = 0x50;
        protected static byte LOW_POWER_DETECTION               = 0x51;
        protected static byte TCON_SETTING                      = 0x60;
        protected static byte RESOLUTION_SETTING                = 0x61;
        protected static byte GET_STATUS                        = 0x71;
        protected static byte AUTO_MEASURE_VCOM                 = 0x80;
        protected static byte READ_VCOM_VALUE                   = 0x81;
        protected static byte VCM_DC_SETTING                    = 0x82;
        protected static byte PARTIAL_WINDOW                    = 0x90;
        protected static byte PARTIAL_IN                        = 0x91;
        protected static byte PARTIAL_OUT                       = 0x92;
        protected static byte PROGRAM_MODE                      = 0xA0;
        protected static byte ACTIVE_PROGRAM                    = 0xA1;
        protected static byte READ_OTP_DATA                     = 0xA2;
        protected static byte POWER_SAVING                      = 0xE3;
    }
}