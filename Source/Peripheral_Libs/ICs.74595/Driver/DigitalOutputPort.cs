using Netduino.Foundation.GPIO;

namespace Netduino.Foundation.ICs.ShiftRegister74595
{
    public class DigitalOutputPort : DigitalOutputPortBase
    {
        protected readonly byte _pin;
        protected readonly ShiftRegister74595 _shiftRegister74595;

        public override bool State
        {
            get { return _state; }
            set
            {
                _shiftRegister74595.WriteToPort(_pin, value);
                _state = value;
            }
        }

        public override bool InitialState
        {
            get { return _initialState; }
        }

        protected DigitalOutputPort() : base(false) { }

        internal DigitalOutputPort(ShiftRegister74595 shiftRegister74595, byte pin, bool initialState) : base(initialState)
        {
            _shiftRegister74595 = shiftRegister74595;
            _pin = pin;

            if (initialState)
            {
                State = initialState;
            }
        }
    }
}
