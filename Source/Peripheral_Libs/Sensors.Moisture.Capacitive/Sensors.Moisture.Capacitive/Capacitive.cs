using Netduino.Foundation.Sensors;
using Microsoft.SPOT.Hardware;
using S = SecretLabs.NETMF.Hardware;

namespace Sensors.Moisture
{
    public class Capacitive : IMoistureSensor
    {
        S.AnalogInput _analogPort;
        public float Moisture
        {
            get
            {
                return Map(_analogPort.Read(), 0, 1023, 0, 100);
            }
        }

        public Capacitive(Cpu.Pin analogPort)
        {
            _analogPort = new S.AnalogInput(analogPort);
        }

        protected float Map(float value, float fromLow, float fromHigh, float toLow, float toHigh)
        {
            return (((toHigh - toLow) * (value - fromLow)) / (fromHigh - fromLow)) - toLow;
        }
    }
}