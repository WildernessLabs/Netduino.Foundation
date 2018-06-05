using System;
using Microsoft.SPOT;
using H = Microsoft.SPOT.Hardware;

namespace Netduino.Foundation.GPIO.SPOT
{
    /// <summary>
    /// Represents a shim that exposes a native SPOT.Hardware.OutputPort
    /// as an IDigitalOutputPort
    /// </summary>
    public class DigitalOutputPort : DigitalOutputPortBase
    {
        protected H.OutputPort _digitalOutPort = null;

        public override bool InitialState
        {
            get {
                return _digitalOutPort.InitialState;
            }
        }

        public override bool State
        {
            get
            {
                return _state;
            }
            set
            {
                _digitalOutPort.Write(value);
                _state = value;
            }
        }

        // hidden constructors
        protected DigitalOutputPort() : base(false)
        {
            //nothing goes here
        }

        /// <summary>
        /// Creates a DigitalOutputPort from an existing SPOT.Hardware.OutputPort
        /// </summary>
        /// <param name="port"></param>
        /// <param name="initialState"></param>
        public DigitalOutputPort(H.OutputPort port, bool initialState = false) : base(initialState)
        {
            this._digitalOutPort = port;
        }

        /// <summary>
        /// Creates a new DigitalOutputPort from a pin.
        /// </summary>
        /// <param name="pin"></param>
        /// <param name="initialState"></param>
        public DigitalOutputPort(H.Cpu.Pin pin, bool initialState = false) : base(initialState)
        {
            this._digitalOutPort = new H.OutputPort(pin, initialState);
        }
    }
}
