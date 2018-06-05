using System;
using Microsoft.SPOT;

namespace Netduino.Foundation.GPIO
{
    public interface IDigitalPort : IPort
    {
        /// <summary>
        /// Gets or sets the port state, either high (true), or low (false).
        /// </summary>
        bool State { get; set; }
    }
}
