using System.Threading;
using Microsoft.SPOT;
using N = SecretLabs.NETMF.Hardware.Netduino;
using Netduino.Foundation.Sensors.Rotary;
using Netduino.Foundation.LEDs;

namespace Netduino.Foundation.Core.Samples
{
    /// <summary>
    /// Illustrates changing the brightness of an LED in response to the rotation
    /// of a rotary encoder. Rotating the encoder to the right turns the LED up,
    /// and rotating the encoder to the left, turns the LED down.
    /// </summary>
    public class RotaryEncoderSample
    {

        public static void Main()
        {
            // instantiate an app singleton and set it to run.
            App app = new App();
            app.Run();
     
            Thread.Sleep(Timeout.Infinite);
        }
    }

    /// <summary>
    /// This sample uses the App singleton pattern which is better than stuffing 
    /// everything into the static void main because it allows you to use instance
    /// members and whatnot and is much more conducive to organizing complex 
    /// logic.
    /// </summary>
    public class App
    {
        protected RotaryEncoder _rotary = null;
        protected PwmLed _led = null;
        // how much to change the brightness per rotation step. 0.05 = 20 clicks to 100%
        protected float _brightnessStepChange = 0.05F; 

        public App()
        {
            // instantiate our peripherals
            this._rotary = new RotaryEncoder(N.Pins.GPIO_PIN_D6, N.Pins.GPIO_PIN_D7);
            this._led = new PwmLed(N.PWMChannels.PWM_PIN_D11, TypicalForwardVoltage.Green);
        }

        public void Run()
        {
            // wire up events
            this._rotary.Rotated += Rotary_Rotated;
        }

        protected void Rotary_Rotated(object sender, RotaryTurnedEventArgs e)
        {
            //Debug.Print("Rotated " + ((e.Direction == RotationDirection.Clockwise) ? "clockwise." : "counter-clockwise."));

            // if clockwise, turn it up! clamp to 1, so we don't go over.
            if (e.Direction == RotationDirection.Clockwise)
            {
                if(this._led.Brightness >= 1) {
                    return;
                } else {
                    this._led.SetBrightness( (this._led.Brightness + _brightnessStepChange).Clamp(0,1) );
                }
            } else { // otherwise, turn it down. clamp to 0 so we don't go below.
                if (this._led.Brightness <= 0) {
                    return;
                } else {
                    this._led.SetBrightness( (this._led.Brightness - _brightnessStepChange).Clamp(0,1) );
                }
            }
        }
    }
}
