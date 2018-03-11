using System;
using Microsoft.SPOT;

namespace Netduino.Foundation.GPIO
{
    public interface IDigitalPort
    {
        bool State { get; set; }
    }
}
