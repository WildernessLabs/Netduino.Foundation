using Microsoft.SPOT.Hardware;
using System.Threading;

namespace Netduino.Foundation.Displays
{
    public abstract class SPIDisplayBase : DisplayBase
    {
        protected readonly byte[] spiBOneByteBuffer = new byte[1];

        protected OutputPort dataCommandPort;
        protected OutputPort resetPort;
        protected InputPort busyPort;
        protected SPI spi;

        protected const bool Data = true;
        protected const bool Command = false;

        protected void Write(byte value)
        {
            spiBOneByteBuffer[0] = value;
            spi.Write(spiBOneByteBuffer);
        }

        protected void Reset()
        {
            resetPort.Write(false);
            DelayMs(200);
            resetPort.Write(true);
            DelayMs(200);
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

        protected virtual void WaitUntilIdle()
        {
            while (busyPort.Read() == false)
            {
                DelayMs(50);
            }
        }
    }
}