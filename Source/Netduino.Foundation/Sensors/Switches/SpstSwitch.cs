using System;
using Microsoft.SPOT;
using H = Microsoft.SPOT.Hardware;

namespace Netduino.Foundation.Sensors.Switches
{
    /// <summary>
    /// Represents a simple, on/off, Single-Pole-Single-Throw (SPST) switch that closes a circuit 
    /// to either ground/common or high. 
    /// 
    /// Use the SwitchCircuitTerminationType to specify whether the other side of the switch
    /// terminates to ground or high.
    /// </summary>
    public class SpstSwitch : ISwitch, ISensor
    {
        public event EventHandler Changed = delegate { };

        public H.InterruptPort DigitalIn { get; protected set; }

        public bool IsOn
        {
            get
            {
                return this.DigitalIn.Read();
            }
            protected set
            {
                this.Changed(this, new EventArgs());
            }
        }

        public SpstSwitch(H.Cpu.Pin pin, CircuitTerminationType type)
        {
            // if we terminate in ground, we need to pull the port high to test for circuit completion, otherwise down.
            var resistorMode = (type == CircuitTerminationType.CommonGround) ? H.Port.ResistorMode.PullUp : H.Port.ResistorMode.PullDown;

            this.DigitalIn = new H.InterruptPort(pin, true, resistorMode, Microsoft.SPOT.Hardware.Port.InterruptMode.InterruptEdgeBoth);

            this.DigitalIn.OnInterrupt += DigitalIn_OnInterrupt;
        }

        private void DigitalIn_OnInterrupt(uint data1, uint data2, DateTime time)
        {
            this.IsOn = this.DigitalIn.Read();
        }
    }
}
