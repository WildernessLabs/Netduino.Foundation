using Microsoft.SPOT.Hardware;
using System.Threading;

namespace Netduino.Foundation.Displays
{
    public class ILI9163 : DisplayTFTSPIBase
    {
        private ILI9163() { }

        public ILI9163(Cpu.Pin chipSelectPin, Cpu.Pin dcPin, Cpu.Pin resetPin,
            uint width, uint height,
            SPI.SPI_module spiModule = SPI.SPI_module.SPI1,
            uint speedKHz = 9500) : base(chipSelectPin, dcPin, resetPin, width, height, spiModule, speedKHz)
        {
            Initialize();
        }
        
        protected override void Initialize()
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
            Write(0x05);//16 bit 565

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
    }
}