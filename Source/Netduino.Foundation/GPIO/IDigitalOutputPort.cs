using System;
using Microsoft.SPOT;

namespace Netduino.Foundation.GPIO
{
    public interface IDigitalOutputPort : IDigitalPort
    {
        bool InitialState { get; }
    }
}
