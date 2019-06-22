using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;
using Netduino.Foundation.Sensors.Motion;

namespace APDS9960ColorSensorSample
{
    public class Program
    {
        public static void Main()
        {
            // write your code here
            //https://github.com/adafruit/Adafruit_APDS9960/tree/master/examples/color_sensor
            var apds = new APDS9960(Cpu.Pin.GPIO_Pin10);
            apds.EnableColorSensor(true);

        }

    }
}
