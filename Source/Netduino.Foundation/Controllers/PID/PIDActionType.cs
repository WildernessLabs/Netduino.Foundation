using System;
using Microsoft.SPOT;

namespace Netduino.Foundation.Controllers.PID
{
    [Flags]
    public enum PIDActionType
    {
        Proportional,
        Integral,
        Derivative
    }
}
