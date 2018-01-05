using System;
using Microsoft.SPOT;
using H = Microsoft.SPOT.Hardware;

namespace Netduino.Foundation.Sensors.Switches
{
    public class SimpleSwitch : ISwitch, ISensor
    {
        public event EventHandler Changed = delegate { };

        public H.InputPort DigitalIn { get; protected set; }

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

        public SimpleSwitch(H.Cpu.Pin pin)
        {
            this.DigitalIn = new H.InputPort(pin, false, Microsoft.SPOT.Hardware.Port.ResistorMode.Disabled);
            this.DigitalIn.EnableInterrupt();

            this.DigitalIn.OnInterrupt += DigitalIn_OnInterrupt;

        }

        private void DigitalIn_OnInterrupt(uint data1, uint data2, DateTime time)
        {
            this.IsOn = this.DigitalIn.Read();
        }
    }
}
