using System;
using Microsoft.SPOT;
using Netduino.Foundation.GPIO;

namespace Netduino.Foundation.ICs.IOExpanders.MCP23008
{
    public class DeviceInterruptEventArgs : PortInterruptEventArgs
    {
        public byte Pin { get; set; }
    }

    public delegate void DeviceInterruptEventHandler(object sender, DeviceInterruptEventArgs e);
}
