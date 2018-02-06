using System;
using System.Net;
using System.Net.Sockets;

namespace Netduino.Foundation
{
    public static class NetworkTime
    {
        public static DateTime GetNetworkTime(int timeZone = 0, string ntpServer = "pool.ntp.org")
        {
            var ntpData = new byte[48];
            ntpData[0] = 0x1B; //LeapIndicator = 0 (no warning), VersionNum = 3 (IPv4 only), Mode = 3 (Client Mode)
            //
            //    Lookup the server and grab the time data.
            //
            var addresses = Dns.GetHostEntry(ntpServer).AddressList;
            var ipEndPoint = new IPEndPoint(addresses[0], 123);
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.Connect(ipEndPoint);
            socket.Send(ntpData);
            socket.Receive(ntpData);
            socket.Close();
            //
            //    Decode the time data to work out the current date and time.
            //
            ulong intPart = (ulong) ntpData[40] << 24 | (ulong) ntpData[41] << 16 | (ulong) ntpData[42] << 8 |
                            (ulong) ntpData[43];
            ulong fractPart = (ulong) ntpData[44] << 24 | (ulong) ntpData[45] << 16 | (ulong) ntpData[46] << 8 |
                                (ulong) ntpData[47];
            var milliseconds = (intPart * 1000) + ((fractPart * 1000) / 0x100000000L);
            milliseconds += ((ulong) timeZone * 3600000);
            var networkDateTime = (new DateTime(1900, 1, 1)).AddMilliseconds((long) milliseconds);
            return networkDateTime;
        }
    }
}