using System;
using Microsoft.SPOT;

namespace Netduino.Foundation.Displays.TextDisplayMenu.InputTypes
{
    public class TimeDetailed : Time
    {
        public TimeDetailed()
        {
            this._timeMode = TimeMode.HH_MM_SS;
        }
    }
}
