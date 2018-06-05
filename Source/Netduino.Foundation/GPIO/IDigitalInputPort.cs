using System;
using Microsoft.SPOT;

namespace Netduino.Foundation.GPIO
{
    public interface IDigitalInputPort : IDigitalPort
    {
        event PortInterruptEventHandler Changed;

        bool InterruptEnabled { get; }

        bool Value { get; }

        
    }
}
