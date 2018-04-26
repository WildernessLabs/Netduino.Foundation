using System;
using Microsoft.SPOT;

namespace Netduino.Foundation.Displays.TextDisplayMenu.InputTypes
{
    public class TimeShort : Time
    {
        public TimeShort()
        {
            this._timeMode = TimeMode.MM_SS;
        }
    }
}
