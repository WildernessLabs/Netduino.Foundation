using System;

namespace Netduino.Foundation.Sensors.Motion
{
	public interface IAccelerometer
	{
		OutputPort XAcceleration { get; }
		OutputPort YAcceleration { get; }
		OutputPort ZAcceleration { get; }
	}
}
