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

        #region Events, properties & delegates
        public static NetworkInterface CurrentNetworkInterface { get; private set; } = null;

        public delegate void NetworkConnectedDelegate(object sender, EventArgs e);
        public static event NetworkConnectedDelegate NetworkConnected;

        #endregion
		
        #region Methods

        /// <summary>
        ///     Attempt to start the network interface(s).
        /// </summary>
        /// <param name="uri">Web address to check if required.  If this is null then the check will not be performed.</param>
        /// <returns>True of the network started correctly, false otherwise.</returns>
        public static void InitializeNetwork(string uri = null)
        {
            if (Microsoft.SPOT.Hardware.SystemInfo.SystemID.SKU == 3)
            {
                Debug.Print("Wireless tests run only on Device");
                return; //ToDo - is this needed? 
            }

            _interfaces = NetworkInterface.GetAllNetworkInterfaces();

            ListNetworkInterfaces();

            var th = new Thread(() => CheckNetworkInterfacesForConnection(uri));
            th.Start();
        }

        private static bool CheckNetworkInterfacesForConnection (string uri)
        {
            foreach (var networkInterface in _interfaces)
            {
                if (UpdateIPAddressFromDHCP(networkInterface))
                {
                    if (uri == null)
                    {
                        CurrentNetworkInterface = networkInterface;
                        return true;
                    }

                    int retryCount = 0;

                    while (retryCount < 3)
                    {
                        try
                        {
                            MakeWebRequest(uri);

                            CurrentNetworkInterface = networkInterface;
                            return true;
                        }
                        catch (Exception e)
                        {
                            Debug.Print(e.Message);
                            retryCount++;
                        }
                    }
                }
                return false;
            }
            //  If we get here then none of the network interface have started correctly
            return false;
        }

        private static void MakeWebRequest(string uri)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(uri);
            httpWebRequest.Method = "GET";

            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();

            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                var result = streamReader.ReadToEnd();
                Debug.Print("Response from " + uri + ":\n" + result);
            }
        }

        /// <summary>
        /// Checks the IP Address.
        /// </summary>
        /// <returns><c>true</c> if IP Address was assigned, <c>false</c> otherwise.</returns>
        /// <param name="networkInterface">Net.</param>
        private static bool UpdateIPAddressFromDHCP(NetworkInterface networkInterface)
        {
            int timeout = 10000; // timeout, in milliseconds to wait for an IP

            // check to see if the IP address is empty (0.0.0.0). IPAddress.Any is 0.0.0.0.
            if (networkInterface.IPAddress == IPAddress.Any.ToString())
            {
                Debug.Print("No IP Address");

                if (networkInterface.IsDhcpEnabled)
                {
                    Debug.Print("DHCP is enabled, attempting to get an IP Address");

                    // ask for an IP address from DHCP [note this is a static, not sure which network interface it would act on]
                    int sleepInterval = 10;
                    int maxIntervalCount = timeout / sleepInterval;
                    int count = 0;

                    while (IPAddress.GetDefaultLocalAddress() == IPAddress.Any && count < maxIntervalCount)
                    {
                        Debug.Print("Sleep while obtaining an IP");
                        Thread.Sleep(sleepInterval);
                        count++;
                    };

                    // if we got here, we either timed out or got an address, so let's find out.
                    if (networkInterface.IPAddress == IPAddress.Any.ToString())
                    {
                        Debug.Print("Failed to get an IP Address in the alotted time.");
                        return false;
                    }

                    Debug.Print("Got an IP Address: " + networkInterface.IPAddress.ToString());

                    if (NetworkConnected != null)
                        NetworkConnected.Invoke(null, EventArgs.Empty);
                    return true;
                }
                else
                {
                    Debug.Print("DHCP is not enabled and no IP address is configured.");
                    return false;
                }
            }
            else
            {
                Debug.Print("Already have an IP Address: " + networkInterface.IPAddress.ToString());

                if (NetworkConnected != null)
                    NetworkConnected.Invoke(null, EventArgs.Empty);

                return true;
            }
        }


        /// <summary>
        ///     Display (on Debug console) the network interface information for all of the interfaces.
        /// </summary>
        public static void ListNetworkInterfaces()
        {
            foreach (var networkInterface in _interfaces)
            {
                ListNetworkInfo((networkInterface));
            }
        }

        /// <summary>
        ///     Display the netowrk information for a a specified network interface on the Debug console.
        /// </summary>
        /// <param name="networkInterface">Network interface to decode and display the information for.</param>
        private static void ListNetworkInfo(NetworkInterface networkInterface)
        {
            try
            {
                switch (networkInterface.NetworkInterfaceType)
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
                Debug.Print("MAC Address: " + DebugInformation.Hexadecimal(networkInterface.PhysicalAddress));
                Debug.Print("DHCP enabled: " + networkInterface.IsDhcpEnabled.ToString());
                Debug.Print("Dynamic DNS enabled: " + networkInterface.IsDynamicDnsEnabled.ToString());
                Debug.Print("IP Address: " + networkInterface.IPAddress.ToString());
                Debug.Print("Subnet Mask: " + networkInterface.SubnetMask.ToString());
                Debug.Print("Gateway: " + networkInterface.GatewayAddress.ToString());

                if (networkInterface is Wireless80211)
                {
                    var wifi = networkInterface as Wireless80211;
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
