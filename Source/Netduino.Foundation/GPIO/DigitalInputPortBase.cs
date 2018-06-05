using System;
using Microsoft.SPOT;

namespace Netduino.Foundation.GPIO
{
    public abstract class DigitalInputPortBase
    {
        public event PortInterruptEventHandler Changed = delegate { };

        public bool InterruptEnabled
        {
            get { return _interruptEnabled; }
        } protected bool _interruptEnabled;

        public abstract bool Value { get; }
    
        protected DigitalInputPortBase(bool interruptEnabled = false)
        {
            _interruptEnabled = interruptEnabled;
        }

        protected void RaiseChanged(bool value)
        {
            this.Changed(this, new PortInterruptEventArgs() { ValueAtInterrupt = value });
        }
    }
}
