using System;
using Microsoft.SPOT;

namespace Netduino.Foundation.Sensors.Switches
{
    interface ISwitch
    {
        event EventHandler Changed;

        bool IsOn { get; }
    }
}
