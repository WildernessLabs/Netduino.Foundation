using System;
using Microsoft.SPOT;

namespace Netduino.Foundation.GPIO
{
    public interface IPort
    {
        PortDirectionType DirectionType { get; }

        PortSignalType SignalType { get; }
    }
}
