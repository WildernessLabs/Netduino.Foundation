using System;
using Microsoft.SPOT;
using Netduino.Foundation.Sensors.Rotary;
using System.Threading;

namespace Netduino.Foundation.Displays.TextDisplayMenu.InputTypes
{
    public class Time : TimeBase
    {
        public Time() : base(TimeMode.HH_MM)
        {
        }
    }
}
