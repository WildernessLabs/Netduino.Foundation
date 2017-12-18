using System;
using System.Threading;
using Microsoft.SPOT;
using Netduino.Foundation.RTCs;
using SecretLabs.NETMF.Hardware.NetduinoPlus;

namespace DS3231Test
{
    public class Program
    {
        public static void Main()
        {
            var rtc = new DS3231(0x68, 100, Pins.GPIO_PIN_D8);
            //
            //  Set the current time.
            //
            rtc.CurrentDateTime = new DateTime(2017, 11, 11, 14, 16, 0);
            Debug.Print("Current time: " + rtc.CurrentDateTime.ToString("dd MMM yyyy HH:mm:ss"));
            Debug.Print("Temperature: " + rtc.Temperature.ToString("f2"));
            rtc.ClearInterrupt(DS323x.Alarm.BothAlarmsRaised);
            rtc.DisplayRegisters();
            rtc.SetAlarm(DS323x.Alarm.Alarm1Raised, new DateTime(2017, 10, 29, 9, 43, 15),
                         DS323x.AlarmType.WhenSecondsMatch);
            rtc.OnAlarm1Raised += rtc_OnAlarm1Raised;
            rtc.DisplayRegisters();
            Thread.Sleep(Timeout.Infinite);
        }

        /// <summary>
        /// Alarm has been raised.
        /// </summary>
        private static void rtc_OnAlarm1Raised(object sender)
        {
            var rtc = (DS3231) sender;
            Debug.Print("Alarm 1 has been activated: " + rtc.CurrentDateTime.ToString(("dd MMM yyyy HH:mm:ss")));
            rtc.ClearInterrupt(DS323x.Alarm.Alarm1Raised);
        }
    }
}