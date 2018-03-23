using System;
using Microsoft.SPOT;

namespace Netduino.Foundation.GPIO
{
    public abstract class DigitalOutputPortBase : IDigitalOutputPort
    {
        public PortDirectionType DirectionType
        {
            get { return PortDirectionType.Output; }
        }
        public PortSignalType SignalType { get { return PortSignalType.Digital; } }

        public abstract bool InitialState { get; }
        protected readonly bool _initialState;

        /// <summary>
        /// API question: do we use a property to write, or do we
        /// use a Write(bool) method?
        /// </summary>
        public abstract bool State { get; set; }
        protected bool _state = false;

        protected DigitalOutputPortBase(bool initialState)
        {
            _initialState = initialState;
            _state = initialState;
        }
    }
}
