using System;
using Microsoft.SPOT.Hardware;
using Netduino.Foundation.Devices;

namespace Netduino.Foundation.RTC
{
    class DS323x
    {
        #region Constants

        /// <summary>
        /// Number of registers that hold the date and time information.
        /// </summary>
        private const int DATE_TIME_REGISTERS_SIZE = 0x07;

        /// <summary>
        /// Bit mask to turn Alarm1 on.
        /// </summary>
        private const byte ALARM1_ENABLE = 0x01;

        /// <summary>
        /// Bit mask to turn Alarm1 off.
        /// </summary>
        private const byte ALARM1_DISABLE = 0xfe;

        /// <summary>
        /// Bit mask to turn Alarm2 on.
        /// </summary>
        private const byte ALARM2_ENABLE = 0x02;

        /// <summary>
        /// Bit mask to turn Alarm2 off.
        /// </summary>
        private const byte ALARM2_DISABLE = 0xfd;

        /// <summary>
        /// Interrupt flag for Alarm1.
        /// </summary>
        private const byte ALARM1_INTERRUPT_FLAG = 0x01;

        /// <summary>
        /// Bit mask to clear the Alarm1 interrupt.
        /// </summary>
        private const byte ALARM1_INTERRUPT_OFF = 0xfe;

        /// <summary>
        /// Interrupt flag for the Alarm2 interrupt.
        /// </summary>
        private const byte ALARM2_INTERRUPT_FLAG = 0x02;

        /// <summary>
        /// Bit mask to clear the Alarm2 interrupt.
        /// </summary>
        private const byte ALARM2_INTERRUPT_OFF = 0xfd;

        #endregion Constants

        #region Enums

        /// <summary>
        /// Register addresses in the sensor.
        /// </summary>
        protected enum Registers
        {
            Seconds = 0x00, Minutes = 0x01, Hours = 0x02, Day = 0x03, Date = 0x04,
            Month = 0x05, Year = 0x06, Alarm1Seconds = 0x07, Alarm1Minutes = 0x08,
            Alarm1Hours = 0x09, Alarm1DayDate = 0x0a, Alarm2Minutes = 0x0b,
            Alarm2Hours = 0x0c, Alarm2DayDate = 0x0d, Control = 0x0e,
            ControlStatus = 0x0f, AgingOffset = 0x10, TemperatureMSB = 0x11,
            TemperatureLSB = 0x12
        }

        /// <summary>
        /// Possible values for the alarm that can be set or alarm that has been raised.
        /// </summary>
        public enum Alarm
        {
            Alarm1Raised, Alarm2Raised, BothAlarmsRaised, Unknown
        };

        /// <summary>
        /// Registers bits in the control register.
        /// </summary>
        enum ControlRegisterBits { A1IE = 0x01, A2IE = 0x02, INTCON = 0x04, RS1 = 0x08, 
                                   RS2 = 0x10, Conv = 0x20, BBSQW = 0x40, NotEOSC = 0x80 };

        /// <summary>
        /// Register bits in the control / status register.
        /// </summary>
        enum StatusRegisterBits { A1F = 0x02, A2F = 0x02, BSY = 0x04, EN32Khz = 0x08, Crate0 = 0x10, 
                                  Crate1 = 0x20, BB32kHz = 0x40, OSF = 0x80 };

        /// <summary>
        /// Possible frequency for the square wave output.
        /// </summary>
        public enum RateSelect { OneHz = 0, OnekHz = 1, FourkHz = 2, EightkHz = 3 };

        /// <summary>
        /// Determine which alarm should be raised.
        /// </summary>
        public enum AlarmType
        {
            //
            //  Alarm 1 options.
            //
            OncePerSecond, WhenSecondsMatch, WhenMinutesSecondsMatch,  WhenHoursMinutesSecondsMatch,
            WhenDateHoursMinutesSecondsMatch, WhenDayHoursMinutesSecondsMatch,
            //
            //  Alarm 2 options.
            //
            OncePerMinute, WhenMinutesMatch, WhenHoursMinutesMatch, WhenDateHoursMinutesMatch,
            WhenDayHoursMinutesMatch
        };

        #endregion Enums

        #region Member variables / fields

        /// <summary>
        /// DS323x Real Time Clock object.
        /// </summary>
        protected ICommunicationBus _ds323x = null;

        /// <summary>
        /// Interrupt port attached to the DS323x RTC module.
        /// </summary>
        protected InterruptPort _interruptPort = null;

        #endregion Member variables / fields

        #region Delegate and events

        /// <summary>
        /// Delegate for the alarm events.
        /// </summary>
        public delegate void AlarmRaised(object sender);

        /// <summary>
        /// Event raised when Alarm1 is triggered.
        /// </summary>
        public event AlarmRaised OnAlarm1Raised = null;

        /// <summary>
        /// Event raised when Alarm2 is triggered.
        /// </summary>
        public event AlarmRaised OnAlarm2Raised = null;

        #endregion Delegate and events

        #region Properties

        /// <summary>
        /// Get / Set the current date and time.
        /// </summary>
        public DateTime CurrentDateTime
        {
            get
            {
                byte[] data = _ds323x.ReadRegisters((byte) Registers.Seconds, DATE_TIME_REGISTERS_SIZE);
                return (DecodeDateTimeRegisters(data));
            }
            set
            {
                _ds323x.WriteRegisters((byte) Registers.Seconds, EncodeDateTimeRegisters(value));
            }
        }

        /// <summary>
        /// Get the current die temperature.
        /// </summary>
        public double Temperature
        {
            get
            {
                byte[] data = _ds323x.ReadRegisters((byte) Registers.TemperatureMSB, 2);
                ushort temperature = (ushort) ((data[0] << 2) | (data[1] >> 6));
                return (temperature * 0.25);
            }
        }

        /// <summary>
        /// Control register.
        /// </summary>
        /// <remarks>
        /// Control register contains the following bit (in sequence b7 - b0):
        /// 
        /// EOSC - BBSQW - CONV - RS1 - RS2 - INTCN - A2IE - A1IE
        /// </remarks>
        public byte ControlRegister
        {
            get
            {
                return (_ds323x.ReadRegister((byte) Registers.Control));
            }
            set
            {
                _ds323x.WriteRegister((byte) Registers.Control, value);
            }
        }

        /// <summary>
        /// Control and status register.
        /// </summary>
        /// <remarks>
        /// Control and status register contains the following bit (in sequence b7 - b0):
        /// 
        /// OSF - 0 - 0 - 0 - EN32KHZ - BSY - A2F - A1F
        /// </remarks>
        public byte ControlStatusRegister
        {
            get
            {
                return (_ds323x.ReadRegister((byte) Registers.ControlStatus));
            }
            set
            {
                _ds323x.WriteRegister((byte) Registers.ControlStatus, value);
            }
        }

        /// <summary>
        /// Determine which alarm has been raised.
        /// </summary>
        public Alarm WhichAlarm
        {
            get
            {
                byte controlStatusRegister;
                Alarm result = Alarm.Unknown;

                controlStatusRegister = ControlStatusRegister;
                if (((controlStatusRegister & 0x01) != 0) && ((controlStatusRegister & 0x02) != 0))
                {
                    result = Alarm.BothAlarmsRaised;
                }
                if ((controlStatusRegister & 0x01) != 0)
                {
                    result = Alarm.Alarm1Raised;
                }
                if ((controlStatusRegister & 0x02) != 0)
                {
                    result = Alarm.Alarm2Raised;
                }
                return(result);
            }
        }

        /// <summary>
        /// Setup the interrupts.
        /// </summary>
        Cpu.Pin _interruptPin = Cpu.Pin.GPIO_NONE;
        protected Cpu.Pin InterruptPin
        {
            set
            {
                if (_interruptPin != Cpu.Pin.GPIO_NONE)
                {
                    throw new Exception("Cannot change interrupt pin.");
                }
                _interruptPin = value;
                _interruptPort = new InterruptPort(value, false, Microsoft.SPOT.Hardware.Port.ResistorMode.Disabled, Microsoft.SPOT.Hardware.Port.InterruptMode.InterruptEdgeLow);
                _interruptPort.OnInterrupt += _interruptPort_OnInterrupt;
            }
        }

        #endregion Properties

        #region Methods

        /// <summary>
        /// Decode the register contents and create a DateTime version of the
        /// register contents.
        /// </summary>
        /// <param name="data">Register contents.</param>
        /// <returns>DateTime object version of the data.</returns>
        protected DateTime DecodeDateTimeRegisters(byte[] data)
        {
            byte seconds = Converters.BCDToByte(data[0]);
            byte minutes = Converters.BCDToByte(data[1]);
            byte hour = 0;
            if ((data[2] & 0x40) != 0)
            {
                hour = Converters.BCDToByte((byte) (data[2] & 0x1f));
                if ((data[2] & 0x20) != 0)
                {
                    hour += 12;
                }
            }
            else
            {
                hour = Converters.BCDToByte((byte)( data[2] & 0x3f));
            }
            byte wday = data[3];
            byte day = Converters.BCDToByte(data[4]);
            byte month = Converters.BCDToByte((byte) (data[5] & 0x7f));
            ushort year = (ushort) (1900 + Converters.BCDToByte(data[6]));
            if ((data[5] & 0x80) != 0)
            {
                year += 100;
            }
            return (new DateTime(year, month, day, hour, minutes, seconds));
        }

        /// <summary>
        /// Encode the a DateTime object into the format used by the DS323x chips.
        /// </summary>
        /// <param name="dt">DateTime object to encode.</param>
        /// <returns>Bytes to send to the DS323x chip.</returns>
        protected byte[] EncodeDateTimeRegisters(DateTime dt)
        {
            byte[] data = new byte[7];

            data[0] = Converters.ByteToBCD((byte) dt.Second);
            data[1] = Converters.ByteToBCD((byte) dt.Minute);
            data[2] = Converters.ByteToBCD((byte) dt.Hour);
            data[3] = (byte) dt.DayOfWeek;
            data[4] = Converters.ByteToBCD((byte) dt.Day);
            data[5] = Converters.ByteToBCD((byte) dt.Month);
            if (dt.Year > 1999)
            {
                data[5] |= 0x80;
                data[6] = Converters.ByteToBCD((byte) ((dt.Year - 2000) & 0xff));
            }
            else
            {
                data[6] = Converters.ByteToBCD((byte) ((dt.Year - 1900) & 0xff));
            }
            return (data);
        }

        /// <summary>
        /// Convert the day of the week to a byte.
        /// </summary>
        /// <param name="day">Day of the week</param>
        /// <returns>Byte representation of the day of the week (Sunday = 1).</returns>
        public byte DayOfWeekToByte(DayOfWeek day)
        {
            byte result = 1;
            switch (day)
            {
                case DayOfWeek.Sunday:
                    result = 1;
                    break;
                case DayOfWeek.Monday:
                    result = 2;
                    break;
                case DayOfWeek.Tuesday:
                    result = 3;
                    break;
                case DayOfWeek.Wednesday:
                    result = 4;
                    break;
                case DayOfWeek.Thursday:
                    result = 5;
                    break;
                case DayOfWeek.Friday:
                    result = 6;
                    break;
                case DayOfWeek.Saturday:
                    result = 7;
                    break;
            }
            return (result);
        }

        /// <summary>
        /// Set one of the two alarms on the DS323x module.
        /// </summary>
        /// <param name="alarm">Define the alarm to be set.</param>
        /// <param name="time">Date and time for the alarm.</param>
        /// <param name="type">Type of alarm to set.</param>
        public void SetAlarm(Alarm alarm, DateTime time, AlarmType type)
        {
            byte[] data = null;
            Registers register = Registers.Alarm1Seconds;
            int element = 0;

            if (alarm == Alarm.Alarm1Raised)
            {
                data = new byte[5];
                element = 1;
                data[0] = Converters.ByteToBCD((byte) (time.Second & 0xff));
            }
            else
            {
                data = new byte[4];
                register = Registers.Alarm2Minutes;
            }
            data[element++] = Converters.ByteToBCD((byte) (time.Minute & 0xff));
            data[element++] = Converters.ByteToBCD((byte) (time.Hour & 0xff));
            if ((type == AlarmType.WhenDayHoursMinutesMatch) || (type == AlarmType.WhenDayHoursMinutesSecondsMatch))
            {
                data[element] = (byte) (DayOfWeekToByte(time.DayOfWeek) | 0x40);
            }
            else
            {
                data[element] = Converters.ByteToBCD((byte) (time.Day & 0xff));
            }
            switch (type)
            {
                //
                //  Alarm 1 interrupts.
                //
                case AlarmType.OncePerSecond:
                    data[0] |= 0x80;
                    data[1] |= 0x80;
                    data[2] |= 0x80;
                    data[3] |= 0x80;
                    break;
                case AlarmType.WhenSecondsMatch:
                    data[1] |= 0x80;
                    data[2] |= 0x80;
                    data[3] |= 0x80;
                    break;
                case AlarmType.WhenMinutesSecondsMatch:
                    data[2] |= 0x80;
                    data[3] |= 0x80;
                    break;
                case AlarmType.WhenHoursMinutesSecondsMatch:
                    data[3] |= 0x80;
                    break;
                case AlarmType.WhenDateHoursMinutesSecondsMatch:
                    break;
                case AlarmType.WhenDayHoursMinutesSecondsMatch:
                    data[3] |= 0x40;
                    break;
                //
                //  Alarm 2 interupts.
                //
                case AlarmType.OncePerMinute:
                    data[0] |= 0x80;
                    data[1] |= 0x80;
                    data[2] |= 0x80;
                    break;
                case AlarmType.WhenMinutesMatch:
                    data[1] |= 0x80;
                    data[2] |= 0x80;
                    break;
                case AlarmType.WhenHoursMinutesMatch:
                    data[2] |= 0x80;
                    break;
                case AlarmType.WhenDateHoursMinutesMatch:
                    break;
                case AlarmType.WhenDayHoursMinutesMatch:
                    data[2] |= 0x40;
                    break;
            }
            _ds323x.WriteRegisters((byte) register, data);
            //
            //  Turn the relevant alarm on.
            //
            byte controlRegister = ControlRegister;
            byte bits = (byte) ControlRegisterBits.A1IE;
            if (alarm == Alarm.Alarm2Raised)
            {
                bits = (byte) ControlRegisterBits.A2IE;
            }
            controlRegister |= ((byte) ControlRegisterBits.INTCON);
            controlRegister |= bits;
            ControlRegister = controlRegister;
        }

        /// <summary>
        /// Enable or disable the specified alarm.
        /// </summary>
        /// <param name="alarm">Alarm to enable / disable.</param>
        /// <param name="enable">Alarm state, true = on, false = off.</param>
        public void EnableDisableAlarm(Alarm alarm, bool enable)
        {
            byte controlRegister;

            controlRegister = ControlRegister;
            if (alarm == Alarm.Alarm1Raised)
            {
                if (enable)
                {
                    controlRegister |= ALARM1_ENABLE;
                }
                else
                {
                    controlRegister &= ALARM1_DISABLE;
                }
            }
            else
            {
                if (enable)
                {
                    controlRegister |= ALARM2_ENABLE;
                }
                else
                {
                    controlRegister &= ALARM2_DISABLE;
                }
            }
            ControlRegister = controlRegister;
        }

        /// <summary>
        /// Clear the alarm interrupt flag for the specified alarm.
        /// </summary>
        /// <param name="alarm">Alarm to clear.</param>
        public void ClearInterrupt(Alarm alarm)
        {
            byte controlStatusRegister = ControlStatusRegister;
            switch (alarm)
            {
                case Alarm.Alarm1Raised:
                    controlStatusRegister &= ALARM1_INTERRUPT_OFF;
                    break;
                case Alarm.Alarm2Raised:
                    controlStatusRegister &= ALARM2_INTERRUPT_OFF;
                    break;
                case Alarm.BothAlarmsRaised:
                    controlStatusRegister &= ALARM1_INTERRUPT_OFF;
                    controlStatusRegister &= ALARM2_INTERRUPT_OFF;
                    break;
            }
            ControlStatusRegister = controlStatusRegister;
        }

        /// <summary>
        /// Display the registers.
        /// </summary>
        public void DisplayRegisters()
        {
            byte[] data = _ds323x.ReadRegisters((byte) 0, 0x12);
            Helpers.DebugInformation.DisplayRegisters(0, data);
        }

        #endregion

        #region Interrupt handler

        /// <summary>
        /// Alarm interrupt has been raised, work out which one and raise the necessary event.
        /// </summary>
        void _interruptPort_OnInterrupt(uint data1, uint data2, DateTime time)
        {
            if ((OnAlarm1Raised != null) || (OnAlarm2Raised != null))
            {
                Alarm alarm = WhichAlarm;
                if (((alarm == Alarm.Alarm1Raised) || (alarm == Alarm.BothAlarmsRaised)) && (OnAlarm1Raised != null))
                {
                    OnAlarm1Raised(this);
                }
                if (((alarm == Alarm.Alarm2Raised) || (alarm == Alarm.BothAlarmsRaised)) && (OnAlarm2Raised != null))
                {
                    OnAlarm2Raised(this);
                }
            }
        }

        #endregion Interrupt handlers.
    }
}
