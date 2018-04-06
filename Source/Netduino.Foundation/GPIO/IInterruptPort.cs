using System;
using Microsoft.SPOT;

namespace Netduino.Foundation.GPIO
{
    // TODO: consider getting rid of this and integrate into IDigitalInputPort
    public interface IInterruptPort
    {
        event PortInterruptEventHandler InterruptTriggered;
    }
}
