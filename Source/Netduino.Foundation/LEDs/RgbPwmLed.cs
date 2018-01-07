using System;
using Microsoft.SPOT;
using H = Microsoft.SPOT.Hardware;
using N = SecretLabs.NETMF.Hardware.Netduino;
using System.Threading;

namespace Netduino.Foundation.LEDs
{
    /// <summary>
    /// Represents a Pulse-Width-Modulation (PWM) controlled RGB LED. Controlling an RGB LED with 
    /// PWM allows for more colors to be expressed than if it were simply controlled with normal
    /// digital outputs which provide only binary control at each pin. As such, a PWM controlled 
    /// RGB LED can express millions of colors, as opposed to the 8 colors that can be expressed
    /// via binary digital output.
    /// </summary>
    public class RgbPwmLed
    {
        public bool IsCommonCathode { get; protected set; }
        public H.Cpu.PWMChannel RedPin { get; protected set; }
        public H.PWM RedPwm { get; protected set; }
        public H.Cpu.PWMChannel GreenPin { get; protected set; }
        public H.PWM BluePwm { get; protected set; }
        public H.Cpu.PWMChannel BluePin { get; protected set; }
        public H.PWM GreenPwm { get; protected set; }


        // TODO: this should be based on voltage drop so it can be used with or without resistors.
        protected double dutyCycleMax = .3; // RGB Led doesn't seem to get much brighter than at 30%

        protected float _maximumRedPwmDuty = 1;
        protected float _maximumGreenPwmDuty = 1;
        protected float _maximumBluePwmDuty = 1;
        public float RedForwardVoltage { get; protected set; }
        public float GreenForwardVoltage { get; protected set; }
        public float BlueForwardVoltage { get; protected set; }

        protected Thread _animationThread = null;

        public Color Color
        {
            get { return this._color; }
        } protected Color _color = new Color(0, 0, 0);
        
        
        /// <summary>
        /// TODO: in the case of isCommonCathode = false, invert the stuff.
        /// 
        /// Implementation notes: Architecturally, it would be much cleaner to construct this class
        /// as three PwmLeds. Then each one's implementation would be self-contained. However, that
        /// would require three additional threads during ON; one contained by each PwmLed. For this
        /// reason, I'm basically duplicating the functionality for all three in here. 
        /// </summary>
        /// <param name="redPin"></param>
        /// <param name="greenPin"></param>
        /// <param name="bluePin"></param>
        /// <param name="isCommonCathode"></param>
        public RgbPwmLed(
            H.Cpu.PWMChannel redPin, H.Cpu.PWMChannel greenPin, H.Cpu.PWMChannel bluePin,
            float redLedForwardVoltage = TypicalForwardVoltage.ResistorLimited, 
            float greenLedForwardVoltage = TypicalForwardVoltage.ResistorLimited, 
            float blueLedForwardVoltage = TypicalForwardVoltage.ResistorLimited,
            bool isCommonCathode = true)
        {
            // validate and persist forward voltages
            if (redLedForwardVoltage < 0 || redLedForwardVoltage > 3.3F) {
                throw new ArgumentOutOfRangeException("redLedForwardVoltage", "error, forward voltage must be between 0, and 3.3");
            } this.RedForwardVoltage = redLedForwardVoltage;
            if (greenLedForwardVoltage < 0 || greenLedForwardVoltage > 3.3F) {
                throw new ArgumentOutOfRangeException("greenLedForwardVoltage", "error, forward voltage must be between 0, and 3.3");
            } this.GreenForwardVoltage = greenLedForwardVoltage;
            if (blueLedForwardVoltage < 0 || blueLedForwardVoltage > 3.3F) {
                throw new ArgumentOutOfRangeException("blueLedForwardVoltage", "error, forward voltage must be between 0, and 3.3");
            } this.BlueForwardVoltage = blueLedForwardVoltage;
            // calculate and set maximum PWM duty cycles
            this._maximumRedPwmDuty = Helpers.CalculateMaximumDutyCycle(RedForwardVoltage);
            this._maximumGreenPwmDuty = Helpers.CalculateMaximumDutyCycle(GreenForwardVoltage);
            this._maximumBluePwmDuty = Helpers.CalculateMaximumDutyCycle(BlueForwardVoltage);

            this.IsCommonCathode = isCommonCathode;
            this.RedPin = redPin;
            this.GreenPin = greenPin;
            this.BluePin = bluePin;

            this.RedPwm = new Microsoft.SPOT.Hardware.PWM(this.RedPin, 100, 0, false);
            this.GreenPwm = new Microsoft.SPOT.Hardware.PWM(this.GreenPin, 100, 0, false);
            this.BluePwm = new Microsoft.SPOT.Hardware.PWM(this.BluePin, 100, 0, false);
        }


        public void SetColor(Color color)
        {
            this._color = color;

            // set the color based on the RGB values
            RedPwm.DutyCycle = (this._color.R * _maximumRedPwmDuty);
            GreenPwm.DutyCycle = (this._color.G * _maximumGreenPwmDuty);
            BluePwm.DutyCycle = (this._color.B * _maximumBluePwmDuty);

            // start our PWMs.
            this.RedPwm.Start();
            this.GreenPwm.Start();
            this.BluePwm.Start();
        }

        // HACK/TODO: this is the signature i want, but it's broken until 4.4. (https://github.com/NETMF/netmf-interpreter/issues/87)
        // using arraylist for now
        //public void StartRunningColors(Color[] colors, int[] durations, bool loop)
        public void StartRunningColors(System.Collections.ArrayList colors, int[] durations, bool loop = true)
        {
            if (durations.Length != 1 && colors.Count != durations.Length)
            {
                throw new Exception("durations must either have a count of 1, if they're all the same, or colors and durations arrays must be same length.");
            }

            // stop any existing animations
            this.Stop();
            this._animationThread = new Thread(() => {
                while (loop)
                {
                    for (int i = 0; i < colors.Count; i++)
                    {
                        this.SetColor((Color)colors[i]);
                        // if all the same, use [0], otherwise individuals
                        Thread.Sleep((durations.Length == 1) ? durations[0] : durations[i]);
                    }
                }
            });
            this._animationThread.Start();
        }

        public void StartAlternatingColors(Color colorOne, Color colorTwo, int colorOneDuration, int colorTwoDuration)
        {
            System.Collections.ArrayList foo = new System.Collections.ArrayList{ colorOne, colorTwo };

            this.StartRunningColors(new System.Collections.ArrayList { colorOne, colorTwo }, new int[] { colorOneDuration, colorTwoDuration });
        }

        /// <summary>
        /// Start the Blink animation which sets the brightness of the LED alternating between a low and high brightness setting, using the durations provided.
        /// </summary>
        public void StartBlink(Color color, int highDuration = 200, int lowDuration = 200, float highBrightness = 1, float lowBrightness = 0)
        {
            if (highBrightness > 1 || highBrightness <= 0)
            {
                throw new ArgumentOutOfRangeException("onBrightness", "onBrightness must be > 0 and <= 1");
            }
            if (lowBrightness >= 1 || lowBrightness < 0)
            {
                throw new ArgumentOutOfRangeException("offBrightness", "lowBrightness must be >= 0 and < 1");
            }
            if (lowBrightness >= highBrightness)
            {
                throw new Exception("offBrightness must be less than onBrightness");
            }

            // pre-calculate colors
            var highColor = Color.FromHsba(color.Hue, color.Saturation, highBrightness);
            var lowColor = Color.FromHsba(color.Hue, color.Saturation, lowBrightness);

            this.StartRunningColors(new System.Collections.ArrayList { highColor, lowColor }, new int[] { highDuration, lowDuration });
            //this.StartAlternate(highColor, lowColor, highDuration, lowDuration);
        }

        /// <summary>
        /// Start the Pulse animation which gradually alternates the brightness of the LED between a low and high brightness setting, using the durations provided.
        /// </summary>
        public void StartPulse(Color color, int pulseDuration = 600, float highBrightness = 1, float lowBrightness = 0.15F)
        {
            if (highBrightness > 1 || highBrightness <= 0)
            {
                throw new ArgumentOutOfRangeException("highBrightness", "highBrightness must be > 0 and <= 1");
            }
            if (lowBrightness >= 1 || lowBrightness < 0)
            {
                throw new ArgumentOutOfRangeException("lowBrightness", "lowBrightness must be >= 0 and < 1");
            }
            if (lowBrightness >= highBrightness)
            {
                throw new Exception("lowBrightness must be less than highbrightness");
            }

            // precalculate the colors to keep the loop tight
            float brightness = lowBrightness;
            int intervalTime = 60; // 60 miliseconds is probably the fastest update we want to do, given that threads are given 20 miliseconds by default. 
            int steps = pulseDuration / intervalTime;
            float brightnessIncrement = (highBrightness - lowBrightness) / steps;

            // array of colors we'll walk up and down
            float brightnessStep;
            System.Collections.ArrayList colors = new System.Collections.ArrayList();
            Color[] colorsAscending = new Color[steps];

            // walk up
            for (int i = 0; i < steps; i++)
            {
                brightnessStep = lowBrightness + (brightnessIncrement * i);
                //colorsAscending[i] = Color.FromHsba(this._color.Hue, this._color.Saturation, brightnessStep);
                colors.Add(Color.FromHsba(color.Hue, color.Saturation, brightnessStep));
            } // walk down (start at penultimate to not repeat, and finish at 1
            for (int i = (steps - 2); i > 0; i--)
            {
                brightnessStep = lowBrightness + (brightnessIncrement * i);
                colors.Add(Color.FromHsba(color.Hue, color.Saturation, brightnessStep));
            }

            this.StartRunningColors(colors, new int[] { intervalTime });
        }

        /// <summary>
        /// Stops any running animations.
        /// </summary>
        public void Stop()
        {
            if (this._animationThread != null)
            {
                this._animationThread.Abort();
            }
        }

    }
}
