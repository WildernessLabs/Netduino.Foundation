using System;
using Microsoft.SPOT;
using H = Microsoft.SPOT.Hardware;

namespace Netduino.Foundation.Sensors.Rotary
{
    public class RotaryEncoder : IRotaryEncoder
    {
        public event RotaryTurnedEventHandler Rotated = delegate { };

        public H.InterruptPort APhasePin {
            get { return this._aPhasePin; }
        } readonly H.InterruptPort _aPhasePin;
        public H.InterruptPort BPhasePin {
            get { return this._bPhasePin; }
        } readonly H.InterruptPort _bPhasePin;

        // whether or not we're processing the gray code (encoding of rotational information)
        protected bool _processing = false;

        // we need two sets of gray code results to determine direction of rotation
        protected TwoBitGrayCode[] _results = new TwoBitGrayCode[2];

        public RotaryEncoder(H.Cpu.Pin aPhasePin, H.Cpu.Pin bPhasePin)
        {
            this._aPhasePin = new H.InterruptPort(aPhasePin, true, H.Port.ResistorMode.PullUp, H.Port.InterruptMode.InterruptEdgeBoth);
            this._bPhasePin = new H.InterruptPort(bPhasePin, true, H.Port.ResistorMode.PullUp, H.Port.InterruptMode.InterruptEdgeBoth);

            // both events go to the same event handler because we need to read both
            // pins to determine current orientation
            this._aPhasePin.OnInterrupt += PhasePin_OnInterrupt;
            this._bPhasePin.OnInterrupt += PhasePin_OnInterrupt;
        }

        protected void PhasePin_OnInterrupt(uint data1, uint data2, DateTime time)
        {
            //Debug.Print((!_processing ? "1st result: " : "2nd result: ") + "A{" + (this.APhasePin.Read() ? "1" : "0") + "}, " + "B{" + (this.BPhasePin.Read() ? "1" : "0") + "}");

            // the first time through (not processing) store the result in array slot 0.
            // second time through (is processing) store the result in array slot 2.
            _results[_processing ? 1 : 0].APhase = this.APhasePin.Read();
            _results[_processing ? 1 : 0].BPhase = this.BPhasePin.Read();

            // if this is the second result that we're reading, we should now have 
            // enough information to know which way it's turning, so process the
            // gray code
            if (_processing) {
                this.ProcessRotationResults();
            }

            // toggle our processing flag
            this._processing = !this._processing;
        }

        protected void ProcessRotationResults()
        {
            // if there hasn't been any change, then it's a garbage reading. so toss it out.
            if (_results[0].APhase == _results[1].APhase 
                && 
                _results[0].BPhase == _results[1].BPhase) {
                //Debug.Print("Garbage");
                return;
            }

            // start by reading the a phase pin. if it's High
            if (_results[0].APhase)
            {
                // if b phase went down, then it spun counter-clockwise
                if (!_results[1].BPhase) {
                    OnRaiseRotationEvent(RotationDirection.CounterClockwise);
                } else {
                    OnRaiseRotationEvent(RotationDirection.Clockwise);
                }
            }
            // if a phase is low
            else
            {
                // if b phase went up, then it spun counter-clockwise
                if (_results[1].BPhase) {
                    OnRaiseRotationEvent(RotationDirection.CounterClockwise);
                } else {
                    OnRaiseRotationEvent(RotationDirection.Clockwise);
                }
            }
        }


        protected void OnRaiseRotationEvent(RotationDirection direction)
        {
            this.Rotated(this, new RotaryTurnedEventArgs(direction));
        }
    }
}
