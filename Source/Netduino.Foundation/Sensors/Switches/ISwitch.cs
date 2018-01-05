using System;
using Microsoft.SPOT;

namespace Netduino.Foundation.Sensors.Switches
{
    public interface ISwitch
    {
        event EventHandler Changed;

        bool IsOn { get; }
    }
}
