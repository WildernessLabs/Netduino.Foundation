using System;
using Microsoft.SPOT;

namespace Netduino.Foundation.GPIO
{
    interface IDigitalOutputPort : IDigitalPort
    {
        bool InitialState { get; }
    }
}
