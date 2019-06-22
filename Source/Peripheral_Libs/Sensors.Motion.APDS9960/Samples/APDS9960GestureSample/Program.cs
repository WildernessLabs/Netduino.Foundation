using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using Netduino.Foundation.Sensors.Motion;

namespace APDS9960GestureSample
{
    public class Program
    {
        public static void Main()
        {
            // write your code here
            var apds = new APDS9960(Cpu.Pin.GPIO_Pin4);

            apds.EnableProximity(true);
            apds.EnableGestures(true);


        }

    }
}
