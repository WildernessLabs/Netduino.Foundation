using System;
using Microsoft.SPOT;

namespace Netduino.Foundation
{
    /// <summary>
    /// Whether the circuit is terminated into the common/ground or a high (3.3V) voltage
    /// source. Used to determine whether to pull the resistor wired to the switch
    /// sensor high or low to close the circuit when the switch is closed.
    /// </summary>
    public enum CircuitTerminationType
    {
        CommonGround,
        High
    }
}
