using System.Threading;
using System.Net;
using System.IO;
using Microsoft.SPOT;
using Microsoft.SPOT.Net.NetworkInformation;
using System;
using Netduino.Foundation.Helpers;

namespace Netduino.Foundation.Network
{
    public static class Initializer
    {
        #region Member varialbles / fields

        /// <summary>
        ///     Array of interfaces on this board.
        /// </summary>
        private static NetworkInterface[] _interfaces;

        #endregion Member variables / fields


        #region Methods

        /// <summary>
        ///     Attempt to start the network interface(s).
        /// </summary>
        /// <param name="uri">Web address to check if required.  If this is null then the check will not be performed.</param>
        /// <returns>True of the network started correctly, false otherwise.</returns>
        public static bool InitializeNetwork(string uri = null)
        {
            if (Microsoft.SPOT.Hardware.SystemInfo.SystemID.SKU == 3)
            {
                Debug.Print("Wireless tests run only on Device");
                return false;
            }
            _interfaces = NetworkInterface.GetAllNetworkInterfaces();
            ListNetworkInterfaces();
            foreach (var net in _interfaces)
            {
                if (CheckIPAddress(net))
                {
                    if (uri != null)
                    {
                        int retryCount = 0;
                        while (retryCount < 3)
                        {
                            try
                            {
                                var httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
                                httpWebRequest.Method = "GET";

                                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                                {
                                    var result = streamReader.ReadToEnd();
                                    Debug.Print("Response from " + uri + ":\n" + result);
                                }
                                return (true);
                            }
                            catch (Exception e)
                            {
                                Debug.Print(e.Message);
                                retryCount++;
                            }
                        }
                    }
                    else
                    {
                        return (true);
                    }
                }
                return (false);
            }
            //
            //  If we get here then none of the network interface have started correctly.
            //
            return false;
        }

        /// <summary>
        /// Checks the IPA ddress.
        /// </summary>
        /// <returns><c>true</c>, if IPA ddress was checked, <c>false</c> otherwise.</returns>
        /// <param name="net">Net.</param>
        private static bool CheckIPAddress(NetworkInterface net)
        {
            int timeout = 10000; // timeout, in milliseconds to wait for an IP. 10,000 = 10 seconds

            // check to see if the IP address is empty (0.0.0.0). IPAddress.Any is 0.0.0.0.
            if (net.IPAddress == IPAddress.Any.ToString())
            {
                Debug.Print("No IP Address");

                if (net.IsDhcpEnabled)
                {
                    Debug.Print("DHCP is enabled, attempting to get an IP Address");

                    // ask for an IP address from DHCP [note this is a static, not sure which network interface it would act on]
                    int sleepInterval = 10;
                    int maxIntervalCount = timeout / sleepInterval;
                    int count = 0;
                    while (IPAddress.GetDefaultLocalAddress() == IPAddress.Any && count < maxIntervalCount)
                    {
                        Debug.Print("Sleep while obtaining an IP");
                        Thread.Sleep(10);
                        count++;
                    };

                    // if we got here, we either timed out or got an address, so let's find out.
                    if (net.IPAddress == IPAddress.Any.ToString())
                    {
                        Debug.Print("Failed to get an IP Address in the alotted time.");
                        return false;
                    }

                    Debug.Print("Got IP Address: " + net.IPAddress.ToString());
                    return true;

                    //NOTE: this does not work, even though it's on the actual network device. [shrug]
                    // try to renew the DHCP lease and get a new IP Address
                    //net.RenewDhcpLease ();
                    //while (net.IPAddress == "0.0.0.0") {
                    //    Thread.Sleep (10);
                    //}
                }
                else
                {
                    Debug.Print("DHCP is not enabled, and no IP address is configured.");
                    return false;
                }
            }
            else
            {
                Debug.Print("Already had IP Address: " + net.IPAddress.ToString());
                return true;
            }
        }


        /// <summary>
        ///     Display (on Debug console) the network interface information for all of the interfaces.
        /// </summary>
        public static void ListNetworkInterfaces()
        {
            foreach (var net in _interfaces)
            {
                ListNetworkInfo((net));
            }
        }

        /// <summary>
        ///     Display the netowrk information for a a specified network interface on the Debug console.
        /// </summary>
        /// <param name="net">Network interface to decode and display the information for.</param>
        private static void ListNetworkInfo(NetworkInterface net)
        {
            try
            {
                switch (net.NetworkInterfaceType)
                {
                    case (NetworkInterfaceType.Ethernet):
                        Debug.Print("Found Ethernet Interface");
                        break;
                    case (NetworkInterfaceType.Wireless80211):
                        Debug.Print("Found 802.11 WiFi Interface");
                        break;
                    case (NetworkInterfaceType.Unknown):
                        Debug.Print("Found Unknown Interface");
                        break;
                }
                Debug.Print("MAC Address: " + DebugInformation.Hexadecimal(net.PhysicalAddress));
                Debug.Print("DHCP enabled: " + net.IsDhcpEnabled.ToString());
                Debug.Print("Dynamic DNS enabled: " + net.IsDynamicDnsEnabled.ToString());
                Debug.Print("IP Address: " + net.IPAddress.ToString());
                Debug.Print("Subnet Mask: " + net.SubnetMask.ToString());
                Debug.Print("Gateway: " + net.GatewayAddress.ToString());
                if (net is Wireless80211)
                {
                    var wifi = net as Wireless80211;
                    Debug.Print("SSID:" + wifi.Ssid.ToString());
                }
            }
            catch (Exception e)
            {
                Debug.Print("ListNetworkInfo exception:  " + e.Message);
            }
        }

        #endregion Methods
    }
}
