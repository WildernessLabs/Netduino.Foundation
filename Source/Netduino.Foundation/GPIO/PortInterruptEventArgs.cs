using System;
using Microsoft.SPOT;

namespace Netduino.Foundation.GPIO
{
    public class PortInterruptEventArgs : EventArgs
    {
        public bool ValueAtInterrupt { get; set; }

    }

    public delegate void PortInterruptEventHandler(object sender, PortInterruptEventArgs e);
}
