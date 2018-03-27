using System;
using Microsoft.SPOT;

namespace Netduino.Foundation.GPIO
{
    public interface IDigitalOutputPort : IDigitalPort
    {
        /// <summary>
        /// Gets the port’s initial state, either low (false), or high (true), as typically configured during the port’s constructor.
        /// </summary>
        bool InitialState { get; }
    }
}
