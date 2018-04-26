using System;
using Microsoft.SPOT;

namespace Netduino.Foundation.Displays.TextDisplayMenu.InputTypes
{
    public class Temperature : NumericBase
    {
        public Temperature() : base(100, -10, 2)
        {
        }
    }
}
