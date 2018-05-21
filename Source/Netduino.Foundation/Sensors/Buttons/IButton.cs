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
        bool State { get; }
    }
}
