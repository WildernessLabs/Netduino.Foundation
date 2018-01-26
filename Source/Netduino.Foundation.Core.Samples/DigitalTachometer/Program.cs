using System.Threading;
using H = Microsoft.SPOT.Hardware;
using N = SecretLabs.NETMF.Hardware.Netduino;
using Netduino.Foundation.Sensors.HallEffect;
using Netduino.Foundation.Motors;
using Microsoft.SPOT;

namespace Netduino.Foundation.Core.Samples
{
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

        public TachometerApp()
        {
            this._motor = new HBridgeMotor(N.PWMChannels.PWM_PIN_D3, 
                N.PWMChannels.PWM_PIN_D5, N.Pins.GPIO_PIN_D4);
            this._tach = new LinearHallEffectTachometer(N.Pins.GPIO_PIN_D1);
            this._tach.RPMsChanged += RPMsChanged;
        }

        private void RPMsChanged(object sender, Sensors.SensorFloatEventArgs e)
        {
            Debug.Print("RPMs: " + e.CurrentValue);
        }

        public void Run()
        {
            while (true)
            {

                // 75% speed
                this._motor.Speed = 0.75F;

                // 3 seconds
                Thread.Sleep(3000);

                // full speed
                this._motor.Speed = 1.0F;

                // 3 seconds
                Thread.Sleep(3000);

                // off
                this._motor.Speed = 0.0F;

                // 3 seconds
                Thread.Sleep(3000);
            }
        }
    }

}
