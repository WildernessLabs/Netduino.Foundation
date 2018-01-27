using System.Threading;
using H = Microsoft.SPOT.Hardware;
using N = SecretLabs.NETMF.Hardware.Netduino;
using Netduino.Foundation.Sensors.HallEffect;
using Netduino.Foundation.Motors;
using Netduino.Foundation.Displays;
using Microsoft.SPOT;

namespace Netduino.Foundation.Core.Samples
{
    // http://bildr.org/2011/04/various-hall-effect-sensors/
    // 472 = 4700pf = 4.7nf capcitor
    // 104 = 100,000pf = 100nf = 0.1µf
    // 103 = 10,000 = 10nf = 0.01uf

    public class Program
    {
        public static void Main()
        {
            // instantiate an app singleton and set it to run.
            TachometerApp app = new TachometerApp();
            app.Run();

            Thread.Sleep(Timeout.Infinite);
        }
    }

    public class TachometerApp
    {
        protected HBridgeMotor _motor;
        protected LinearHallEffectTachometer _tach;
        protected SerialLCD _lcd;

        public TachometerApp()
        {
            this._motor = new HBridgeMotor(N.PWMChannels.PWM_PIN_D3, 
                N.PWMChannels.PWM_PIN_D5, N.Pins.GPIO_PIN_D4, 50);
            this._tach = new LinearHallEffectTachometer(N.Pins.GPIO_PIN_D2);
            this._lcd = new SerialLCD(new TextDisplayConfig() { Width = 16, Height = 2 });

            this._tach.RPMsChanged += RPMsChanged;
        }

        private void RPMsChanged(object sender, Sensors.SensorFloatEventArgs e)
        {
            Debug.Print("RPMs: " + e.CurrentValue);
            _lcd.WriteLine("Speed: " + e.CurrentValue.ToString("N0") + "RPMs", 0);
        }

        public void Run()
        {
            this._lcd.Clear();

            while (true)
            {

                // 75% speed
                Debug.Print("75% Speed");
                this._motor.Speed = 0.75F;

                // 3 seconds
                Thread.Sleep(3000);

                // full speed
                Debug.Print("100% Speed");
                this._motor.Speed = 1.0F;

                // 3 seconds
                Thread.Sleep(3000);

                // off
                Debug.Print("0% Speed");
                this._motor.Speed = 0.0F;

                // 3 seconds
                Thread.Sleep(3000);
            }
        }
    }

}
