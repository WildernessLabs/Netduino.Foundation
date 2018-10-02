using System;
using Microsoft.SPOT.Hardware;
using System.Threading;

namespace Netduino.Foundation.Displays
{
    /// <summary>
    ///     Provide an interface to the WaveShare ePaper color displays
    ///     104x212, 2.13inch E-Ink display HAT for Raspberry Pi, three-color, SPI interface 
    /// </summary>
    public class WaveShareColor : DisplayBase, IDisposable
    {
        #region Enums

        #endregion

        public override uint Width => 104;
        public override uint Height => 212;
        public override DisplayColorMode ColorMode => DisplayColorMode.Format2bpp;

        protected readonly byte[] blackImageBuffer;
        protected readonly byte[] colorImageBuffer;
        protected readonly byte[] spiBOneByteBuffer = new byte[1];

        protected OutputPort dataCommandPort;
        protected OutputPort resetPort;
        protected InputPort busyPort;
        protected SPI spi;

        private const bool Data = true;
        private const bool Command = false;

        int xRefreshStart, yRefreshStart, xRefreshEnd, yRefreshEnd;

        private WaveShareColor()
        {  }

        public WaveShareColor(Cpu.Pin chipSelectPin, Cpu.Pin dcPin, Cpu.Pin resetPin, Cpu.Pin busyPin, SPI.SPI_module spiModule = SPI.SPI_module.SPI1, uint speedKHz = (uint)9500)
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

          //  SetPartialWindowBlack(null, 0, 0, 50, 50);
         //   DisplayFrame();
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

        public void Refresh()
        {
            xRefreshStart = -1;
            if (xRefreshStart == -1)
            {
                DisplayFrame(blackImageBuffer, colorImageBuffer);
            }
            else
            {
                SetPartialWindow(blackImageBuffer, colorImageBuffer,
                        xRefreshStart, yRefreshStart, xRefreshEnd - xRefreshStart, yRefreshEnd - yRefreshStart); 

                  DisplayFrame();
            }

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

            SendCommand(BOOSTER_SOFT_START);
            SendData(0x17);
            SendData(0x17);
            SendData(0x17);
            SendCommand(POWER_ON);

            WaitUntilIdle();
            SendCommand(PANEL_SETTING);
            SendData(0x8F);
            SendCommand(VCOM_AND_DATA_INTERVAL_SETTING);
            SendData(0x37);
            SendCommand(RESOLUTION_SETTING);
            SendData(0x68);//width 104
            SendData(0x00);
            SendData(0xD4);//height 212
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
            while (busyPort.Read() == false)
            {
                DelayMs(50);
            }
        }

        void SetPartialWindow(byte[] bufferBlack, byte[] bufferColor, int x, int y, int width, int height)
        {
            SendCommand(PARTIAL_IN);
            SendCommand(PARTIAL_WINDOW);
            SendData(x & 0xf8);     // x should be the multiple of 8, the last 3 bit will always be ignored
            SendData(((x & 0xf8) + width - 1) | 0x07);
            SendData(y >> 8);
            SendData(y & 0xff);
            SendData((y + height - 1) >> 8);
            SendData((y + height - 1) & 0xff);
            SendData(0x01);         // Gates scan both inside and outside of the partial window. (default) 
            DelayMs(2);
            SendCommand(DATA_START_TRANSMISSION_1);
            if (bufferBlack != null)
            {
                for (int i = 0; i < width / 8 * height; i++)
                {
                    SendData(bufferBlack[i]);
                }
            }
            else
            {
                for (int i = 0; i < width / 8 * height; i++)
                {
                    SendData(0x00);
                }
            }
            DelayMs(2);
            SendCommand(DATA_START_TRANSMISSION_2);

            if (bufferColor != null)
            {
                for (int i = 0; i < width / 8 * height; i++)
                {
                    SendData(bufferColor[i]);
                }
            }
            else
            {
                for (int i = 0; i < width / 8 * height; i++)
                {
                    SendData(0x00);
                }
            }
            DelayMs(2);
            SendCommand(PARTIAL_OUT);
        }

        void SetPartialWindowBlack(byte[] bufferBlack, int x, int y, int width, int height)
        {
            SendCommand(PARTIAL_IN);
            SendCommand(PARTIAL_WINDOW);
            SendData(x & 0xf8);     // x should be the multiple of 8, the last 3 bit will always be ignored
            SendData(((x & 0xf8) + width - 1) | 0x07);
            SendData(y >> 8);
            SendData(y & 0xff);
            SendData((y + height - 1) >> 8);        
            SendData((y + height - 1) & 0xff);
            SendData(0x01);         // Gates scan both inside and outside of the partial window. (default) 
            DelayMs(2);
            SendCommand(DATA_START_TRANSMISSION_1);

            if (bufferBlack != null)
            {
                for(int i = 0; i< width / 8 * height; i++)
                {
                    SendData(bufferBlack[i]);
                }
            }
            else
            {
                for(int i = 0; i< width / 8 * height; i++)
                {
                    SendData(0x00);  
                }  
            }

            DelayMs(2);
            SendCommand(PARTIAL_OUT);  
        }

        void SetPartialWindowColor(byte[] bufferColor, int x, int y, int width, int height)
        {
            SendCommand(PARTIAL_IN);
            SendCommand(PARTIAL_WINDOW);
            SendData(x & 0xf8);
            SendData(((x & 0xf8) + width - 1) | 0x07);
            SendData(y >> 8);
            SendData(y & 0xff);
            SendData((y + height - 1) >> 8);
            SendData((y + height - 1) & 0xff);
            SendData(0x01);         // Gates scan both inside and outside of the partial window. (default) 
            DelayMs(2);
            SendCommand(DATA_START_TRANSMISSION_2);

            if (bufferColor != null)
            {
                for (int i = 0; i < width / 8 * height; i++)
                {
                    SendData(bufferColor[i]);
                }
            }
            else
            {
                for (int i = 0; i < width / 8 * height; i++)
                {
                    SendData(0x00);
                }
            }

            DelayMs(2);
            SendData(PARTIAL_OUT);
        }

        //clear the frame data from the SRAM, this doesn't update the display
        void ClearFrame()
        {
            SendCommand(DATA_START_TRANSMISSION_1);
            Thread.Sleep(2);
            
            for(int i = 0; i < Width * Height / 8; i++)
            {
                SendData(0xFF);
            }
            Thread.Sleep(2);

            SendCommand(DATA_START_TRANSMISSION_2);
            Thread.Sleep(2);
            for (int i = 0; i < Width * Height / 8; i++)
            {
                SendData(0xFF);
            }
            Thread.Sleep(2);
        }

        void DisplayFrame(byte[] blackBuffer, byte[] colorBuffer)
        {
            SendCommand(DATA_START_TRANSMISSION_1);
            Thread.Sleep(2);

            for (int i = 0; i < Width * Height / 8; i++)
            {
                SendData(blackBuffer[i]);
            }
            Thread.Sleep(2);

            SendCommand(DATA_START_TRANSMISSION_2);
            Thread.Sleep(2);
            for (int i = 0; i < Width * Height / 8; i++)
            {
                SendData(colorBuffer[i]);
            }
            Thread.Sleep(2);

            DisplayFrame();
        }

        public void DisplayFrame()
        {
            SendCommand(DISPLAY_REFRESH);
            WaitUntilIdle();
        }

        void Sleep()
        {
            SendCommand(POWER_OFF);
            WaitUntilIdle();
            SendCommand(DEEP_SLEEP);
            SendData(0xA5); //check code
        }

        public void Dispose()
        {
            spi.Dispose();
            spi = null;
            dataCommandPort = null;
            resetPort = null;
        }

        // 2.13 B (red) commands
        static byte PANEL_SETTING = 0x00;
        static byte POWER_SETTING                               = 0x01;
        static byte POWER_OFF                                   = 0x02;
        static byte POWER_OFF_SEQUENCE_SETTING                  = 0x03;
        static byte POWER_ON                                    = 0x04;
        static byte POWER_ON_MEASURE                            = 0x05;
        static byte BOOSTER_SOFT_START                          = 0x06;
        static byte DEEP_SLEEP                                  = 0x07;
        static byte DATA_START_TRANSMISSION_1                   = 0x10;
        static byte DATA_STOP                                   = 0x11;
        static byte DISPLAY_REFRESH                             = 0x12;
        static byte DATA_START_TRANSMISSION_2                   = 0x13;
        static byte VCOM_LUT                                    = 0x20;
        static byte W2W_LUT                                     = 0x21;
        static byte B2W_LUT                                     = 0x22;
        static byte W2B_LUT                                     = 0x23;
        static byte B2B_LUT                                     = 0x24;
        static byte PLL_CONTROL                                 = 0x30;
        static byte TEMPERATURE_SENSOR_CALIBRATION              = 0x40;
        static byte TEMPERATURE_SENSOR_SELECTION                = 0x41;
        static byte TEMPERATURE_SENSOR_WRITE                    = 0x42;
        static byte TEMPERATURE_SENSOR_READ                     = 0x43;
        static byte VCOM_AND_DATA_INTERVAL_SETTING              = 0x50;
        static byte LOW_POWER_DETECTION                         = 0x51;
        static byte TCON_SETTING                                = 0x60;
        static byte RESOLUTION_SETTING                          = 0x61;
        static byte GET_STATUS                                  = 0x71;
        static byte AUTO_MEASURE_VCOM                           = 0x80;
        static byte READ_VCOM_VALUE                             = 0x81;
        static byte VCM_DC_SETTING                              = 0x82;
        static byte PARTIAL_WINDOW                              = 0x90;
        static byte PARTIAL_IN                                  = 0x91;
        static byte PARTIAL_OUT                                 = 0x92;
        static byte PROGRAM_MODE                                = 0xA0;
        static byte ACTIVE_PROGRAM                              = 0xA1;
        static byte READ_OTP_DATA                               = 0xA2;
        static byte POWER_SAVING                                = 0xE3;
    }
}