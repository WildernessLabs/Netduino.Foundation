using System;
using System.Collections;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using Netduino.Foundation;
using System.Threading;
using SecretLabs.NETMF.Hardware.Netduino;

namespace Netduino.Foundation.Communications
{
    public sealed class OneWireBus
    {
        public class Devices
        {
            public Cpu.Pin Pin { get; set; }

            private OutputPort Port { get; set; }

            public OneWire DeviceBus { get; set; }

            public ArrayList DeviceIDs { get; set; }

            /// <summary>
            ///     Default constructor is private to prevent it being invoked.
            /// </summary>
            private Devices()
            {
            }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="pin"></param>
            public Devices(Cpu.Pin pin)
            {
                Pin = pin;
                Port = new OutputPort(pin, false);
                DeviceBus = new OneWire(Port);
                DeviceIDs = new ArrayList();
                ScanForDevices();
            }

            /// <summary>
            /// 
            /// </summary>
            private void ScanForDevices()
            {
                ArrayList deviceList;
                lock (DeviceBus)
                {
                    DeviceBus.TouchReset();
                    deviceList = DeviceBus.FindAllDevices();
                }

                for (var device = 0; device < deviceList.Count; device++)
                {
                    UInt64 deviceID = 0;
                    byte[] deviceIDArray = ((byte[])deviceList[device]);
                    for (var index = 0; index < 8; index++)
                    {
                        int places = 8 * index;
                        byte value = deviceIDArray[index];
                        deviceID |= ((UInt64)value) << places;
                    }

                    DeviceIDs.Add(deviceID);
                }
            }
        };

        private static readonly OneWireBus oneWireBus = new OneWireBus();

        private static readonly ArrayList _devices = new ArrayList();

        public static OneWireBus Instance { get { return oneWireBus; } }

        static OneWireBus()
        {
        }

        /// <summary>
        ///     Default constructor is private to prevent it being invoked.
        /// </summary>
        private OneWireBus()
        {
        }

        public static Devices Add(Cpu.Pin pin)
        {
            Devices result = null;
            foreach (Devices device in _devices)
            {
                if (device.Pin == pin)
                {
                    result = device;
                }
            }

            if (result == null)
            {
                result = new Devices(pin);
                _devices.Add(result);
            }
            return (result);
        }
    }
}
