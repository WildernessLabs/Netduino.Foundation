using System;

namespace Netduino.Foundation
{
	public class OutputPort : Port
	{
//		readonly TUnits units;

		public OutputPort (Block block, string name, Units units, double initialValue)
			: base (block, name, units, initialValue)
		{
//			units = new TUnits ();
		}
	}
}

