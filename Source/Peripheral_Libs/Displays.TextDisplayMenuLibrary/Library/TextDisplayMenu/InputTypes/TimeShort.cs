using System;
using Microsoft.SPOT;

namespace Netduino.Foundation.Displays.TextDisplayMenu.InputTypes
{
    public class TimeShort : TimeBase
    {
        public TimeShort() : base(TimeMode.MM_SS)
        {
        }
    }
}
