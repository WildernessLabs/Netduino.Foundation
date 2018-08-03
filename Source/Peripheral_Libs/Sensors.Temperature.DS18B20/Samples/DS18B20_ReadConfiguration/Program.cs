using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using SecretLabs.NETMF.Hardware;
using SecretLabs.NETMF.Hardware.NetduinoPlus;
using Netduino.Foundation.Sensors.Temperature;

namespace DS18B20_ReadConfiguration
{
    public class Program
    {
        /// <summary>
        ///     Read the configuration and ID from the DS18B20 and display the information
        ///     retrieved in the Debug Output window.
        /// </summary>
        public static void Main()
        {
            DS18B20 ds18b20 = new DS18B20(Pins.GPIO_PIN_D3, updateInterval: 0);
            Debug.Print("Device ID: 0x" + ds18b20.DeviceID.ToString("X16"));
            Debug.Print("Resolution: " + ds18b20.Resolution + " bits");
            Thread.Sleep(Timeout.Infinite);
        }
    }
}
