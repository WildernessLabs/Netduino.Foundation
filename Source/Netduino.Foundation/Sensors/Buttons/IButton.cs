using System;

#if MF_FRAMEWORK_VERSION_V4_3
using Microsoft.SPOT;
#endif

namespace Netduino.Foundation.Sensors.Buttons
{
	public interface IButton
	{
        event EventHandler PressStarted;
        event EventHandler PressEnded;
        event EventHandler Clicked;
        /// <summary>
        /// Returns the current raw state of the switch. If the switch 
        /// is pressed (connected), returns true, otherwise false.
        /// </summary>
       bool State { get; }
    }
}
