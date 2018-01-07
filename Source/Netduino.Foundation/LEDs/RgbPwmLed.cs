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

        protected Thread _animationThread = null;

        public Color Color
        {
            get { return this._color; }
        } protected Color _color = new Color(0, 0, 0);
        

        public RgbPwmLed(H.Cpu.PWMChannel redPin, H.Cpu.PWMChannel greenPin, H.Cpu.PWMChannel bluePin, bool isCommonCathode = true)
        //public RgbLed(H.PWM redPin, H.PWM greenPin, H.PWM bluePin, bool isCommonCathode = true)
        {
            this.IsCommonCathode = isCommonCathode;
            this.RedPin = redPin;
            this.GreenPin = greenPin;
            this.BluePin = bluePin;

            this.RedPwm = new Microsoft.SPOT.Hardware.PWM(this.RedPin, 100, 0, false);
            this.GreenPwm = new Microsoft.SPOT.Hardware.PWM(this.GreenPin, 100, 0, false);
            this.BluePwm = new Microsoft.SPOT.Hardware.PWM(this.BluePin, 100, 0, false);
        }

        //public void SetHsvColor(double hue, double saturation, double brightness)
        //{
        //    double red;
        //    double green;
        //    double blue;

        //    // convert to RGB
        //    Converters.HsvToRgb(hue, saturation, brightness, out red, out green, out blue);

        //    SetRgbColor(red, green, blue);
        //}

        //public void SetRgbColor(double red, double green, double blue)
        //{
        //    RedPwm.DutyCycle = (red * dutyCycleMax);
        //    GreenPwm.DutyCycle = (green * dutyCycleMax);
        //    BluePwm.DutyCycle = (blue * dutyCycleMax);
        //}

        public void SetColor(Color color)
        {
            this._color = color;

            // set the color based on the RGB values
            RedPwm.DutyCycle = (this._color.R * dutyCycleMax);
            GreenPwm.DutyCycle = (this._color.G * dutyCycleMax);
            BluePwm.DutyCycle = (this._color.B * dutyCycleMax);

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
            if (colors.Count != durations.Length) {
                throw new Exception("colors and durations arrays must be same length.");
            }

            // stop any existing animations
            this.Stop();
            this._animationThread = new Thread(() => {
                while (loop)
                {
                    for (int i = 0; i < colors.Count; i++)
                    {
                        this.SetColor((Color)colors[i]);
                        Thread.Sleep(durations[i]);
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
        //public void StartPulse(int pulseDuration = 600, float highBrightness = 1, float lowBrightness = 0.15F)
        //{
        //    if (highBrightness > 1 || highBrightness <= 0)
        //    {
        //        throw new ArgumentOutOfRangeException("highBrightness", "highBrightness must be > 0 and <= 1");
        //    }
        //    if (lowBrightness >= 1 || lowBrightness < 0)
        //    {
        //        throw new ArgumentOutOfRangeException("lowBrightness", "lowBrightness must be >= 0 and < 1");
        //    }
        //    if (lowBrightness >= highBrightness)
        //    {
        //        throw new Exception("lowBrightness must be less than highbrightness");
        //    }

        //    // precalculate the colors to keep the loop tight
        //    float brightness = lowBrightness;
        //    int intervalTime = 60; // 60 miliseconds is probably the fastest update we want to do, given that threads are given 20 miliseconds by default. 
        //    int steps = pulseDuration / intervalTime;
        //    float brightnessIncrement = (highBrightness - lowBrightness) / steps;

        //    // array of colors we'll walk up and down
        //    float brightnessStep;
        //    Color[] colorsAscending = new Color[steps];
        //    for (int i = 0; i < steps; i++)
        //    {
        //        brightnessStep = lowBrightness + (brightnessIncrement * i);
        //        colorsAscending[i] = Color.FromHsba(this._color.Hue, this._color.Saturation, brightnessStep);
        //    }

        //    // stop any existing animations
        //    this.Stop();
        //    this._animationThread = new Thread(() => {
        //        // pulse the LED by taking the brightness from low to high and back again.

        //        while (true)
        //        {
        //            // walk up
        //            for (int i = 0; i < colorsAscending.Length; i++)
        //            {
        //                this.SetColor(colorsAscending[i]);
        //                // go to sleep, my friend.
        //                Thread.Sleep(intervalTime);
        //            }
        //            // walk down (start at penultimate to not repeat, and finish at 1
        //            for (int i = (colorsAscending.Length - 2); i > 0; i--)
        //            {
        //                this.SetColor(colorsAscending[i]);
        //                // go to sleep, my friend.
        //                Thread.Sleep(intervalTime);
        //            }
        //        }
        //    });
        //    this._animationThread.Start();
        //}

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
