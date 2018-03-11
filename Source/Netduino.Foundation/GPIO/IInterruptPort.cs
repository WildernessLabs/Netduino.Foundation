using System;
using Microsoft.SPOT;

namespace Netduino.Foundation.GPIO
{
    public interface IInterruptPort
    {
        event PortInterruptEventHandler InterruptTriggered;
    }
}
