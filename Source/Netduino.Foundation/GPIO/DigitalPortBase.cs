using System;
using Microsoft.SPOT;

namespace Netduino.Foundation.GPIO
{
    public abstract class DigitalPortBase : IDigitalPort
    {
        public PortDirectionType DirectionType
        {
            get { return _direction; }
        } protected readonly PortDirectionType _direction;

        public PortSignalType SignalType { get { return PortSignalType.Digital; } }

        /// <summary>
        /// API question: do we use a property to write, or do we
        /// use a Write(bool) method?
        /// </summary>
        public bool State {
            get { return _state; }
            set { _state = value; }
        } protected bool _state = false;

        /// <summary>
        /// what to do here?
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public virtual bool Write(bool value)
        {
            bool success = true;

            // try to write


            if (success)
            {
                _state = value;
            }

            return success;
        }
    }
}
