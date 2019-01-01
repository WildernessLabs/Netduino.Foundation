using System;
using Microsoft.SPOT.Hardware;

namespace Netduino.Foundation.Sensors.Color
{
    public class TCS230
    {
        #region Enums
        public enum FrequencyScaling
        {
            PowerDown,
            _2Percent,
            _20Percent,
            _100Percent,
        }

        public enum ColorFilter
        {
            Red,
            Green,
            Blue,
            None,
        }
        #endregion

        #region Member variables / fields

        /// <summary>
        ///     S0 Pin.
        /// </summary>
        protected OutputPort _s0Port;

        /// <summary>
        ///     S1 Pin.
        /// </summary>
        protected OutputPort _s1Port;

        /// <summary>
        ///     S2 Pin.
        /// </summary>
        protected OutputPort _s2Port;

        /// <summary>
        ///     S3 Pin.
        /// </summary>
        protected OutputPort _s3Port;

        /// <summary>
        ///     Out Pin.
        /// </summary>
        protected InterruptPort _outPort;

        #endregion

        #region Constructors

        /// <summary>
        ///     Default constructor is private to prevent it being called.
        /// </summary>
        private TCS230()
        {
        }

        /// <summary>
        ///     Create a new HCSR04 object and hook up the interrupt handler.
        /// </summary>
        /// <param name="triggerPin"></param>
        /// <param name="echoPin"></param>
        public TCS230(Cpu.Pin s0Pin, Cpu.Pin s1Pin, Cpu.Pin s2Pin, Cpu.Pin s3Pin, Cpu.Pin outPin)
        {
            if (s0Pin == Cpu.Pin.GPIO_NONE ||
                s1Pin == Cpu.Pin.GPIO_NONE ||
                s2Pin == Cpu.Pin.GPIO_NONE || 
                s3Pin == Cpu.Pin.GPIO_NONE ||
                outPin == Cpu.Pin.GPIO_NONE)
            {
                throw new Exception("Invalid pin for the TCS230.");
            }

            _s0Port = new OutputPort(s0Pin, false);
            _s1Port = new OutputPort(s1Pin, false);
            _s2Port = new OutputPort(s2Pin, false);
            _s3Port = new OutputPort(s3Pin, false);

            _outPort = new InterruptPort(outPin, false, Port.ResistorMode.Disabled, Port.InterruptMode.InterruptEdgeBoth);

            SetOutputFrequencyScaling(FrequencyScaling._100Percent);
            SetFilter(ColorFilter.None);
        }

        #endregion

        #region Methods
        public void SetOutputFrequencyScaling(FrequencyScaling scaling)
        {
            switch(scaling)
            {
                case FrequencyScaling.PowerDown:
                    _s0Port.Write(false);
                    _s1Port.Write(false);
                    break;
                case FrequencyScaling._2Percent:
                    _s0Port.Write(false);
                    _s1Port.Write(true);
                    break;
                case FrequencyScaling._20Percent:
                    _s0Port.Write(true);
                    _s1Port.Write(false);
                    break;
                case FrequencyScaling._100Percent:
                    _s0Port.Write(true);
                    _s1Port.Write(true);
                    break;
            }
        }

        public void SetFilter(ColorFilter filter)
        {
            switch(filter)
            {
                case ColorFilter.Blue:
                    _s2Port.Write(false);
                    _s3Port.Write(true);
                    break;
                case ColorFilter.Green:
                    _s2Port.Write(true);
                    _s3Port.Write(true);
                    break;
                case ColorFilter.Red:
                    _s2Port.Write(false);
                    _s3Port.Write(false);
                    break;
                case ColorFilter.None:
                    _s2Port.Write(true);
                    _s3Port.Write(false);
                    break;
            }

                
            }


        }
        #endregion
    }
}
