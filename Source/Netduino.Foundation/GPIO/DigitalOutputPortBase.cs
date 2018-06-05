using System;
using Microsoft.SPOT;

namespace Netduino.Foundation.GPIO
{
    /// <summary>
    /// DigitalOutputPortBase inherits from DigitalPortBase and provides a 
    /// base implementation for much of the common tasks of classes implementing 
    /// IDigitalPort.
    /// </summary>
    public abstract class DigitalOutputPortBase : DigitalPortBase, IDigitalOutputPort
    {
        /// <summary>
        /// The InitialState property is backed by the readonly bool 
        /// _initialState member and should be during the constructor.
        /// </summary>
        public abstract bool InitialState { get; }
        protected readonly bool _initialState;

        protected DigitalOutputPortBase(bool initialState) : base(PortDirectionType.Output)
        {
            _initialState = initialState;
            _state = initialState;
        }
    }
}
