using System;
using Microsoft.SPOT;

namespace Netduino.Foundation.GPIO
{
    /// <summary>
    /// DigitalPortBase provides a base implementation for much of the 
    /// common tasks of classes implementing IDigitalPort.
    /// </summary>
    public abstract class DigitalPortBase : IDigitalPort
    {
        /// <summary>
        /// The PortDirectionType property is backed by the readonly _direction member. 
        /// This member must be set during the constructor and describes whether the 
        /// port in an input or output port.
        /// </summary>
        public PortDirectionType DirectionType
        {
            get { return _direction; }
        } protected readonly PortDirectionType _direction;

        /// <summary>
        /// The PortSignalType property returns PortSignalType.Digital.
        /// </summary>
        public PortSignalType SignalType { get { return PortSignalType.Digital; } }

        /// <summary>
        /// Gets or sets the port state, either high (true), or low (false).
        /// </summary>
        public virtual bool State {
            get { return _state; }
            set { _state = value; }
        } protected bool _state = false;

        protected DigitalPortBase(PortDirectionType direction)
        {
            _direction = direction;
        }
    }
}
