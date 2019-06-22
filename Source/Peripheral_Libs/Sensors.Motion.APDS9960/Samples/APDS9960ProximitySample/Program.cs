using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.Netduino;
using Netduino.Foundation.Sensors.Motion;

namespace APDS9960ProximitySample
{
    public class Program
    {
        public static void Main()
        {
            // write your code here
            var apds = new APDS9960(Cpu.Pin.GPIO_Pin10);

            apds.EnableProximity(true);
            apds.SetProximityInterruptThreshhold(0, 175);
            apds.EnableProximityInterrupt(true);




        }

    }
}
