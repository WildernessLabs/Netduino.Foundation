using System;
using Netduino.Foundation.Sensors;
using Microsoft.SPOT.Hardware;
using S = SecretLabs.NETMF.Hardware;
using System.Threading;

namespace Sensors.Moisture
{
    public class FC28 : IMoistureSensor
    {
        S.AnalogInput _analogPort;
        OutputPort _digitalPort;

        public float Moisture { get; private set; }

        public FC28(Cpu.Pin analogPort, Cpu.Pin digitalPort)
        {
            _analogPort = new S.AnalogInput(analogPort);
            _digitalPort = new OutputPort(digitalPort, false);
        }

        public float Read()
        {
            int sample;

            _digitalPort.Write(true);
            Thread.Sleep(5);
            sample = _analogPort.Read();
            _digitalPort.Write(false);

            Moisture = 100 - Map(sample, 0, 1023, 0, 100);
            return Moisture;
        }

        protected float Map(float value, float fromLow, float fromHigh, float toLow, float toHigh)
        {
            return (((toHigh - toLow) * (value - fromLow)) / (fromHigh - fromLow)) - toLow;
        }
    }
}
