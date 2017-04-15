using System;
using Microsoft.SPOT;

namespace Netduino.Foundation.Generators
{
    public class Constant : Block
    {
        public OutputPort Output { get; private set; }

        public Constant (double value)
        {
            Output = AddOutput ("Output", Units.Scalar, value);
        }
    }
}
