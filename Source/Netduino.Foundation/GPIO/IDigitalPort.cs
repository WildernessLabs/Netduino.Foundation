using System;
using Microsoft.SPOT;

namespace Netduino.Foundation.GPIO
{
    public interface IDigitalPort : IPort
    {
        //TODO: should this be a property? or should we force a method for write(value)?
        bool State { get; set; }
    }
}
