using System;
using System.Threading;
using Microsoft.SPOT;
using H = Microsoft.SPOT.Hardware;

namespace Netduino.Foundation.Generators
{
    /// <summary>
    /// A Pulse Width Modulation Generator that can
    /// generates waveforms in software. The maximum
    /// Frequency is about 100 Hz.
    /// </summary>
	public class SoftPwm
    {
        public H.OutputPort OutputPort { get; private set; }

        public float DutyCycle {
            get { return _dutyCycle; }
            set {
                _dutyCycle = value;
                _onTimeMilliseconds = CalculateOnTimeMillis();
                _offTimeMilliseconds = CalculateOffTimeMillis();
                //Debug.Print("OnTime: " + _onTimeMilliseconds.ToString() + ", OffTime: " + _offTimeMilliseconds.ToString());
            }
        } protected float _dutyCycle;
        public float Frequency {
            get { return _frequency; }
            set {
                if (Frequency <= 0) {
                    throw new Exception("Frequency must be > 0.");
                }
                _frequency = value;
                _onTimeMilliseconds = CalculateOnTimeMillis();
                _offTimeMilliseconds = CalculateOffTimeMillis();
                //Debug.Print("OnTime: " + _onTimeMilliseconds.ToString() + ", OffTime: " + _offTimeMilliseconds.ToString());
            }
        } protected float _frequency = 1.0f; // in the case it doesn't get set before dutycycle, initialize to 1

        protected Thread _th = null;
        protected int _onTimeMilliseconds = 0;
        protected int _offTimeMilliseconds = 0;
        protected bool _running = false;

        public SoftPwm(H.Cpu.Pin outputPin, float dutyCycle = 0.5f, float frequency = 1.0f)
        {
            OutputPort = new H.OutputPort(outputPin, false);
            DutyCycle = dutyCycle;
            Frequency = frequency;
        }

        public void Start()
        {
            this._running = true;

            // create a new thread that actually writes the pwm to the output port
            _th = new Thread(() => { 
                while (this._running)
                {
                    OutputPort.Write(true);
                    Thread.Sleep(_onTimeMilliseconds);
                    OutputPort.Write(false);
                    Thread.Sleep(_offTimeMilliseconds);
                }
            });
            _th.Start();
        }

        public void Stop()
        {
            // setting this will wrap up the thread
            this._running = false;

            // need to make sure the port is off, otherwise it can get
            // stuck in an ON state.
            OutputPort.Write(false);
        }

        /// <summary>
        /// Calculates the on time in milliseconds
        /// </summary>
        protected int CalculateOnTimeMillis()
        {
            var dc = DutyCycle;
            // clamp
            if (dc < 0) dc = 0;
            if (dc > 1) dc = 1;
            // on time  = 
            return (int)(dc / Frequency * 1000);
        }

        protected int CalculateOffTimeMillis()
        {
            var dc = DutyCycle;
            // clamp
            if (dc < 0) dc = 0;
            if (dc > 1) dc = 1;
            // off time = 
            return (int)(((1 - dc) / Frequency) * 1000);
        }
    }
}
