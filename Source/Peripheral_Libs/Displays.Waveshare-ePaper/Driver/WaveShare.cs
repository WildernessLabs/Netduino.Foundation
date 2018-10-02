using System;
using Microsoft.SPOT.Hardware;
using System.Threading;

namespace Netduino.Foundation.Displays
{
    /// <summary>
    ///     Provide an interface to the WaveShare ePaper monochrome displays
    /// </summary>
    public class WaveShare : DisplayBase, IDisposable
    {
        #region Enums

        #endregion

        public override uint Width => 200;
        public override uint Height => 200;
        public override DisplayColorMode ColorMode => DisplayColorMode.Format1bpp;

        protected readonly byte[] imageBuffer;
        protected readonly byte[] spiBOneByteBuffer = new byte[1];

        protected OutputPort dataCommandPort;
        protected OutputPort resetPort;
        protected InputPort busyPort;
        protected SPI spi;

        private const bool Data = true;
        private const bool Command = false;

        int xRefreshStart, yRefreshStart, xRefreshEnd, yRefreshEnd;

        private WaveShare()//1.54" display
        {

        }

        public WaveShare(Cpu.Pin chipSelectPin, Cpu.Pin dcPin, Cpu.Pin resetPin, Cpu.Pin busyPin, SPI.SPI_module spiModule = SPI.SPI_module.SPI1, uint speedKHz = (uint)9500)
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

            imageBuffer = new byte[Width * Height / 8];

            for(int x = 0; x < Width; x++)
            {
                for(int y = 0; y < Height; y++)
                {
                    DrawPixel(x, y, false);
                }
            }

            Initialize();
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

            for (int i = 0; i < imageBuffer.Length; i++)
                imageBuffer[i] = colored ? (byte)0 : (byte)255;

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
                        if((b & mask) > 0)
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
                imageBuffer[(x + y * Width) / 8] &= (byte)~(0x80 >> (x % 8));
            }
            else
            {
                imageBuffer[(x + y * Width) / 8] |= (byte)(0x80 >> (x % 8));
            }
        }

        public override void DrawPixel(int x, int y, Color color)
        {
            bool colored = false;
            if (color.B > 0 || color.G > 0 || color.R > 0)
                colored = true;

            DrawPixel(x, y, colored);
        }

        public void DrawPixel(int x, int y, byte r, byte g, byte b)
        {
            bool colored = false;
            if (r > 0 || g > 0 || b > 0)
                colored = true;

            DrawPixel(x, y, colored);
        }

        public void Refresh()
        {
            if (xRefreshStart == -1)
                SetFrameMemory(imageBuffer);
            else
                SetFrameMemory(imageBuffer, xRefreshStart, yRefreshStart, xRefreshEnd - xRefreshStart, yRefreshEnd - yRefreshStart);

            DisplayFrame();

            xRefreshStart = yRefreshStart = xRefreshEnd = yRefreshEnd = -1;
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
            Reset();

            SendCommand(DRIVER_OUTPUT_CONTROL);
            SendData(199);
            SendData(199 >> 8);
            SendData(0x00);                     // GD = 0; SM = 0; TB = 0;

            SendCommand(BOOSTER_SOFT_START_CONTROL);
            SendData(0xD7);
            SendData(0xD6);
            SendData(0x9D);

            SendCommand(WRITE_VCOM_REGISTER);
            SendData(0xA8);                     // VCOM 7C

            SendCommand(SET_DUMMY_LINE_PERIOD);
            SendData(0x1A);                     // 4 dummy lines per gate

            SendCommand(SET_GATE_TIME);
            SendData(0x08);                     // 2us per line

            SendCommand(DATA_ENTRY_MODE_SETTING);
            SendData(0x03);                     // X increment; Y increment

            SendData(LUT_Full_Update);
        }

        void Reset()
        {
            resetPort.Write(false);
            DelayMs(200);
            resetPort.Write(true);
            DelayMs(200);
        }

        public void DelayMs(int millseconds)
        {
            Thread.Sleep(millseconds);
        }

        void SendCommand(byte command)
        {
            dataCommandPort.Write(Command);
            Write(command);
        }

        void SendData(int data)
        {
            SendData((byte)data);
        }

        void SendData(byte data)
        {
            dataCommandPort.Write(Data);
            Write(data);
        }

        void SendData(byte[] data)
        {
            dataCommandPort.Write(Data);
            spi.Write(data);
        }

        void WaitUntilIdle()
        {
            while (busyPort.Read() == true)
            {
                DelayMs(50);
            }
        }

        public void SetFrameMemory(byte[] image_buffer,
                                int x,
                                int y,
                                int image_width,
                                int image_height)
        {
            int x_end;
            int y_end;

            if (image_buffer == null ||
                x < 0 || image_width < 0 ||
                y < 0 || image_height < 0)
            {
                return;
            }

              /* x point must be the multiple of 8 or the last 3 bits will be ignored */
            x &= 0xF8;
            image_width &= 0xF8;
            if (x + image_width >= Width)
                x_end = (int)Width - 1;
            else
                x_end = x + image_width - 1;

            if (y + image_height >= Height)
                y_end = (int)Height - 1;
            else
                y_end = y + image_height - 1;

            SetMemoryArea(x, y, x_end, y_end);
            SetMemoryPointer(x, y);
            SendCommand(WRITE_RAM);
            /* send the image data */
            for (int j = 0; j < y_end - y + 1; j++)
            {
                for (int i = 0; i < (x_end - x + 1) / 8; i++)
                {
                    SendData(image_buffer[i + j * (image_width / 8)]);
                }
            }
        }

        public void SetFrameMemory(byte[] image_buffer)
        {
            SetMemoryArea(0, 0, (int)Width - 1, (int)Height - 1);
            SetMemoryPointer(0, 0);
            SendCommand(WRITE_RAM);
            /* send the image data */
            for (int i = 0; i < Width / 8 * Height; i++)
            {
                SendData(image_buffer[i]);
            }
        }

        public void ClearFrameMemory(byte color)
        {
            SetMemoryArea(0, 0, (int)Width - 1, (int)Height - 1);
            SetMemoryPointer(0, 0);
            SendCommand(WRITE_RAM);
            /* send the color data */
            for (int i = 0; i < Width / 8 * Height; i++)
            {
                SendData(color);
            }
        }

        public void DisplayFrame()
        {
            SendCommand(DISPLAY_UPDATE_CONTROL_2);
            SendData(0xC4);
            SendCommand(MASTER_ACTIVATION);
            SendCommand(TERMINATE_FRAME_READ_WRITE);
            WaitUntilIdle();
        }

        void SetMemoryArea(int x_start, int y_start, int x_end, int y_end)
        {
            SendCommand(SET_RAM_X_ADDRESS_START_END_POSITION);
            /* x point must be the multiple of 8 or the last 3 bits will be ignored */
            SendData((x_start >> 3) & 0xFF);
            SendData((x_end >> 3) & 0xFF);
            SendCommand(SET_RAM_Y_ADDRESS_START_END_POSITION);
            SendData(y_start & 0xFF);
            SendData((y_start >> 8) & 0xFF);
            SendData(y_end & 0xFF);
            SendData((y_end >> 8) & 0xFF);
        }

        void SetMemoryPointer(int x, int y)
        {
            SendCommand(SET_RAM_X_ADDRESS_COUNTER);
            /* x point must be the multiple of 8 or the last 3 bits will be ignored */
            SendData((x >> 3) & 0xFF);
            SendCommand(SET_RAM_Y_ADDRESS_COUNTER);
            SendData(y & 0xFF);
            SendData((y >> 8) & 0xFF);
            WaitUntilIdle();
        }

        void Sleep()
        {
            SendCommand(DEEP_SLEEP_MODE);
            WaitUntilIdle();
        }

        public void Dispose()
        {
            spi.Dispose();
            spi = null;
            dataCommandPort = null;
            resetPort = null;
        }

        // EPD1IN54 commands
        static byte DRIVER_OUTPUT_CONTROL = 0x01;
        static byte BOOSTER_SOFT_START_CONTROL = 0x0C;
        static byte GATE_SCAN_START_POSITION = 0x0F;
        static byte DEEP_SLEEP_MODE = 0x10;
        static byte DATA_ENTRY_MODE_SETTING = 0x11;
        static byte SW_RESET = 0x12;
        static byte TEMPERATURE_SENSOR_CONTROL = 0x1A;
        static byte MASTER_ACTIVATION = 0x20;
        static byte DISPLAY_UPDATE_CONTROL_1 = 0x21;
        static byte DISPLAY_UPDATE_CONTROL_2 = 0x22;
        static byte WRITE_RAM = 0x24;
        static byte WRITE_VCOM_REGISTER = 0x2C;
        static byte WRITE_LUT_REGISTER = 0x32;
        static byte SET_DUMMY_LINE_PERIOD = 0x3A;
        static byte SET_GATE_TIME = 0x3B;
        static byte BORDER_WAVEFORM_CONTROL = 0x3C;
        static byte SET_RAM_X_ADDRESS_START_END_POSITION = 0x44;
        static byte SET_RAM_Y_ADDRESS_START_END_POSITION = 0x45;
        static byte SET_RAM_X_ADDRESS_COUNTER = 0x4E;
        static byte SET_RAM_Y_ADDRESS_COUNTER = 0x4F;
        static byte TERMINATE_FRAME_READ_WRITE = 0xFF;

        public static readonly byte[] LUT_Full_Update =
        {
            0x02, 0x02, 0x01, 0x11, 0x12, 0x12, 0x22, 0x22,
            0x66, 0x69, 0x69, 0x59, 0x58, 0x99, 0x99, 0x88,
            0x00, 0x00, 0x00, 0x00, 0xF8, 0xB4, 0x13, 0x51,
            0x35, 0x51, 0x51, 0x19, 0x01, 0x00
        };

        public static readonly byte[] LUT_Partial_Update =
        {
            0x10, 0x18, 0x18, 0x08, 0x18, 0x18, 0x08, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
            0x00, 0x00, 0x00, 0x00, 0x13, 0x14, 0x44, 0x12,
            0x00, 0x00, 0x00, 0x00, 0x00, 0x00
        };
    }
}