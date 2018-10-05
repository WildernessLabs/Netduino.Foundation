using System;
using Microsoft.SPOT;
using System.Threading;
using Microsoft.SPOT.Hardware;

namespace Netduino.Foundation.Displays
{
    //Samsung S6D02A1 controller
    public class S6D02A1 : DisplayTFTSPIBase
    {
        private S6D02A1() { }

        public S6D02A1(Cpu.Pin chipSelectPin, Cpu.Pin dcPin, Cpu.Pin resetPin,
            uint width, uint height,
            SPI.SPI_module spiModule = SPI.SPI_module.SPI1,
            uint speedKHz = 9500) : base(chipSelectPin, dcPin, resetPin, width, height, spiModule, speedKHz)
        { }

        protected override void Initialize()
        {
            resetPort.Write(true);
            Thread.Sleep(50);
            resetPort.Write(false);
            Thread.Sleep(50);
            resetPort.Write(true);
            Thread.Sleep(50);

            SendCommand(0xf0, new byte[] { 0x5a, 0x5a });             // Excommand2
            SendCommand(0xfc, new byte[] { 0x5a, 0x5a });             // Excommand3
            SendCommand(0x26, new byte[] { 0x01 });                   // Gamma set
            SendCommand(0xfa, new byte[] { 0x02, 0x1f, 0x00, 0x10, 0x22, 0x30, 0x38, 0x3A, 0x3A, 0x3A, 0x3A, 0x3A, 0x3d, 0x02, 0x01 });   // Positive gamma control
            SendCommand(0xfb, new byte[] { 0x21, 0x00, 0x02, 0x04, 0x07, 0x0a, 0x0b, 0x0c, 0x0c, 0x16, 0x1e, 0x30, 0x3f, 0x01, 0x02 });   // Negative gamma control
            SendCommand(0xfd, new byte[] { 0x00, 0x00, 0x00, 0x17, 0x10, 0x00, 0x01, 0x01, 0x00, 0x1f, 0x1f });                           // Analog parameter control
            SendCommand(0xf4, new byte[] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x3f, 0x3f, 0x07, 0x00, 0x3C, 0x36, 0x00, 0x3C, 0x36, 0x00 });   // Power control
            SendCommand(0xf5, new byte[] { 0x00, 0x70, 0x66, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x6d, 0x66, 0x06 });               // VCOM control
            SendCommand(0xf6, new byte[] { 0x02, 0x00, 0x3f, 0x00, 0x00, 0x00, 0x02, 0x00, 0x06, 0x01, 0x00 });                           // Source control
            SendCommand(0xf2, new byte[] { 0x00, 0x01, 0x03, 0x08, 0x08, 0x04, 0x00, 0x00, 0x00, 0x00, 0x00, 0x01, 0x00, 0x00, 0x04, 0x08, 0x08 });   //Display control
            SendCommand(0xf8, new byte[] { 0x11 });                   // Gate control
            SendCommand(0xf7, new byte[] { 0xc8, 0x20, 0x00, 0x00 });  // Interface control
            SendCommand(0xf3, new byte[] { 0x00, 0x00 });              // Power sequence control
            SendCommand(0x11, null);                 // Wake
            Thread.Sleep(150);
            SendCommand(0xf3, new byte[] { 0x00, 0x01 });  // Power sequence control
            Thread.Sleep(50);
            SendCommand(0xf3, new byte[] { 0x00, 0x03 });  // Power sequence control
            Thread.Sleep(50);
            SendCommand(0xf3, new byte[] { 0x00, 0x07 });  // Power sequence control
            Thread.Sleep(50);
            SendCommand(0xf3, new byte[] { 0x00, 0x0f });  // Power sequence control
            Thread.Sleep(150);
            SendCommand(0xf4, new byte[] { 0x00, 0x04, 0x00, 0x00, 0x00, 0x3f, 0x3f, 0x07, 0x00, 0x3C, 0x36, 0x00, 0x3C, 0x36, 0x00 });    // Power control
            Thread.Sleep(50);
            SendCommand(0xf3, new byte[] { 0x00, 0x1f });   // Power sequence control
            Thread.Sleep(50);
            SendCommand(0xf3, new byte[] { 0x00, 0x7f });   // Power sequence control
            Thread.Sleep(50);
            SendCommand(0xf3, new byte[] { 0x00, 0xff });   // Power sequence control
            Thread.Sleep(50);
            SendCommand(0xfd, new byte[] { 0x00, 0x00, 0x00, 0x17, 0x10, 0x00, 0x00, 0x01, 0x00, 0x16, 0x16 });                           // Analog parameter control
            SendCommand(0xf4, new byte[] { 0x00, 0x09, 0x00, 0x00, 0x00, 0x3f, 0x3f, 0x07, 0x00, 0x3C, 0x36, 0x00, 0x3C, 0x36, 0x00 });   // Power control
            SendCommand(0x36, new byte[] { 0xC8 }); // Memory access data control
            SendCommand(0x35, new byte[] { 0x00 }); // Tearing effect line on
            SendCommand(0x3a, new byte[] { 0x05 }); // Interface pixel control
            Thread.Sleep(150);
            SendCommand(0x29, null);                // Display on
            SendCommand(0x2c, null);				// Memory write

            SetAddressWindow(0, 0, (byte)(_width - 1), (byte)(_height - 1));

            dataCommandPort.Write(Data);
        }

        private void SetAddressWindow(byte x0, byte y0, byte x1, byte y1)
        {
            dataCommandPort.Write(Command);
            Write((byte)LcdCommand.CASET);  // column addr set
            dataCommandPort.Write(Data);
            Write(0x0);
            Write(x0);   // XSTART 
            Write(0x0);
            Write(x1);   // XEND

            dataCommandPort.Write(Command);
            Write((byte)LcdCommand.RASET);  // row addr set
            dataCommandPort.Write(Data);
            Write(0x0);
            Write(y0);    // YSTART
            Write(0x0);
            Write(y1);    // YEND

            dataCommandPort.Write(Command);
            Write((byte)LcdCommand.RAMWR);  // write to RAM */
        }

        void SendCommand(byte command, byte[] data)
        {
            dataCommandPort.Write(Command);
            Write(command);

            if(data != null)
            {
                dataCommandPort.Write(Data);
                for (int i = 0; i < data.Length; i++)
                    Write(data[i]);
            }
        }
    }
}
