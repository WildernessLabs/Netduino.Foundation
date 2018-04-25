using Netduino.Foundation.GPIO;

namespace Netduino.Foundation.ICs.x74595
{
    public class DigitalOutputPort : DigitalOutputPortBase
    {
        protected readonly byte _pin;
        protected readonly x74595 _x74595;

        public override bool State
        {
            get { return _state; }
            set
            {
                _x74595.WriteToPort(_pin, value);
                _state = value;
            }
        }

        public override bool InitialState
        {
            get { return _initialState; }
        }

        protected DigitalOutputPort() : base(false) { }

        internal DigitalOutputPort(x74595 x74595, byte pin, bool initialState) : base(initialState)
        {
            _x74595 = x74595;
            _pin = pin;

            if (initialState)
            {
                State = initialState;
            }
        }
    }
}
