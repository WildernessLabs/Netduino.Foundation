using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;
using Netduino.Foundation.Sensors.Atmospheric;

namespace HTU21D_FTest
{
    public class Program
    {
        public static void Main()
        {
            HTU21DF htu21d = new HTU21DF();
        }
    }
}
