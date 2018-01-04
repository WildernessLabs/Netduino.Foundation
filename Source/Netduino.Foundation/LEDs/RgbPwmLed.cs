using System;
using Microsoft.SPOT;
using H = Microsoft.SPOT.Hardware;
using N = SecretLabs.NETMF.Hardware.Netduino;

namespace Netduino.Foundation.LEDs
{
    /// <summary>
    /// Represents a Pulse-Width-Modulation (PWM) controlled RGB LED. Controlling an RGB LED with 
    /// PWM allows for more colors to be expressed than if it were simply controlled with normal
    /// digital outputs which provide only binary control at each pin. As such, a PWM controlled 
    /// RGB LED can express millions of colors, as opposed to the 6? colors that can be expressed
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

        protected double dutyCycleMax = .3; // RGB Led doesn't seem to get much brighter than at 30%

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

            // start our PWMs. Note, we might want to move this elsewhere, 
            // depending on the desired behavior.
            this.RedPwm.Start();
            this.GreenPwm.Start();
            this.BluePwm.Start();
        }

        public void SetHsvColor(double hue, double saturation, double brightness)
        {
            double red;
            double green;
            double blue;

            // convert to RGB
            Converters.HsvToRgb(hue, saturation, brightness, out red, out green, out blue);

            SetRgbColor(red, green, blue);
        }

        public void SetRgbColor(double red, double green, double blue)
        {
            RedPwm.DutyCycle = (red * dutyCycleMax);
            GreenPwm.DutyCycle = (green * dutyCycleMax);
            BluePwm.DutyCycle = (blue * dutyCycleMax);
        }
    }
}
