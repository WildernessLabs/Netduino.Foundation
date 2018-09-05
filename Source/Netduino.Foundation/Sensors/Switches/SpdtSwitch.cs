using System;
using Microsoft.SPOT;
using H = Microsoft.SPOT.Hardware;

namespace Netduino.Foundation.Sensors.Switches
{
    /// <summary>
    /// Represents a simple, two position, Single-Pole-Dual-Throw (SPDT) switch that closes a circuit 
    /// to either ground/common or high depending on position.
    /// </summary>
    public class SpdtSwitch : ISwitch, ISensor
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

        public SpdtSwitch(H.Cpu.Pin pin)
        {
            H.Port.ResistorMode resistorMode = H.Port.ResistorMode.Disabled;

            this.DigitalIn = new H.InterruptPort(pin, true, resistorMode, H.Port.InterruptMode.InterruptEdgeBoth);

            this.DigitalIn.OnInterrupt += DigitalIn_OnInterrupt;
        }

        private void DigitalIn_OnInterrupt(uint data1, uint data2, DateTime time)
        {
            this.IsOn = this.DigitalIn.Read();
        }
    }
}