/*
Driver ported from http://wiki.sunfounder.cc/images/b/bb/LCD2004_for_Raspberry_Pi.zip
For reference: http://wiki.sunfounder.cc/index.php?title=LCD2004_Module
Brian Kim 5/5/2018
*/

using System;
using Microsoft.SPOT;
using N = SecretLabs.NETMF.Hardware.Netduino;
using H = Microsoft.SPOT.Hardware;
using System.Threading;
using Netduino.Foundation.ICs.IOExpanders.MCP23008;
using Netduino.Foundation.GPIO.SPOT;
using Netduino.Foundation.GPIO;

namespace Netduino.Foundation.Displays.LCD
{
    public class Lcd2004 : ITextDisplay
    {
        #region Member variables / fields

        private byte LCD_LINE_1 = 0x80; // # LCD RAM address for the 1st line
        private byte LCD_LINE_2 = 0xC0; // # LCD RAM address for the 2nd line
        private byte LCD_LINE_3 = 0x94; // # LCD RAM address for the 3rd line
        private byte LCD_LINE_4 = 0xD4; // # LCD RAM address for the 4th line

        private const byte LCD_SETDDRAMADDR = 0x80;
        private const byte LCD_SETCGRAMADDR = 0x40;

        protected IDigitalOutputPort LCD_E;
        protected IDigitalOutputPort LCD_RS;
        protected IDigitalOutputPort LCD_D4;
        protected IDigitalOutputPort LCD_D5;
        protected IDigitalOutputPort LCD_D6;
        protected IDigitalOutputPort LCD_D7;
        protected IDigitalOutputPort LED_ON;
        protected H.PWM LCD_V0 = null;

        private bool LCD_INSTRUCTION = false;
        private bool LCD_DATA = true;
        private static object _lock = new object();

        #endregion

        #region Properties

        public TextDisplayConfig DisplayConfig { get; protected set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor is private to prevent it being used.
        /// </summary>
        private Lcd2004() { }

        /// <summary>
        /// Creates a new Lcd2004 object connected directly to the Netduino
        /// </summary>
        /// <param name="V0">Contrast voltage</param>
        /// <param name="RS">Register Select</param>
        /// <param name="E">Enable</param>
        /// <param name="D4">Data 4</param>
        /// <param name="D5">Data 5</param>
        /// <param name="D6">Data 6</param>
        /// <param name="D7">Data 7</param>
        public Lcd2004(H.Cpu.PWMChannel V0, H.Cpu.Pin RS, H.Cpu.Pin E, H.Cpu.Pin D4, H.Cpu.Pin D5, H.Cpu.Pin D6, H.Cpu.Pin D7)
        {
            DisplayConfig = new TextDisplayConfig { Height = 4, Width = 20 };

            LCD_RS = new GPIO.SPOT.DigitalOutputPort(RS);
            LCD_E = new GPIO.SPOT.DigitalOutputPort(E);
            LCD_D4 = new GPIO.SPOT.DigitalOutputPort(D4);
            LCD_D5 = new GPIO.SPOT.DigitalOutputPort(D5);
            LCD_D6 = new GPIO.SPOT.DigitalOutputPort(D6);
            LCD_D7 = new GPIO.SPOT.DigitalOutputPort(D7);

            LCD_V0 = new H.PWM(V0, 1000, 0.5, false);
            LCD_V0.Start();

            Initialize();
        }

        /// <summary>
        /// Creates a new Lcd2004 object connected of a MCP23008
        /// </summary>
        /// <param name="V0">Contrast voltage</param>
        /// <param name="mcp">MCP23008 object</param>
        public Lcd2004(H.Cpu.PWMChannel V0, MCP23008 mcp)
        {
            DisplayConfig = new TextDisplayConfig { Height = 4, Width = 20 };

            LCD_RS = mcp.CreateOutputPort(1, false);
            LCD_E = mcp.CreateOutputPort(2, false);
            LCD_D4 = mcp.CreateOutputPort(3, false);
            LCD_D5 = mcp.CreateOutputPort(4, false);
            LCD_D6 = mcp.CreateOutputPort(5, false);
            LCD_D7 = mcp.CreateOutputPort(6, false);

            var lite = mcp.CreateOutputPort(7, true);

            LCD_V0 = new H.PWM(V0, 1000, 0.5, false);
            LCD_V0.Start();

            Initialize();
        }

        #endregion

        #region Methods

        private void Initialize()
        {
            SendByte(0x33, LCD_INSTRUCTION); // 110011 Initialise
            SendByte(0x32, LCD_INSTRUCTION); // 110010 Initialise
            SendByte(0x06, LCD_INSTRUCTION); // 000110 Cursor move direction
            SendByte(0x0C, LCD_INSTRUCTION); // 001100 Display On,Cursor Off, Blink Off
            SendByte(0x28, LCD_INSTRUCTION); // 101000 Data length, number of lines, font size
            SendByte(0x01, LCD_INSTRUCTION); // 000001 Clear display
            Thread.Sleep(5);
        }

        private void SendByte(byte value, bool mode)
        {
            lock (_lock)
            {
                LCD_RS.State = (mode);

                // high bits
                LCD_D4.State = ((value & 0x10) == 0x10);
                LCD_D5.State = ((value & 0x20) == 0x20);
                LCD_D6.State = ((value & 0x40) == 0x40);
                LCD_D7.State = ((value & 0x80) == 0x80);

                ToggleEnable();

                // low bits
                LCD_D4.State = ((value & 0x01) == 0x01);
                LCD_D5.State = ((value & 0x02) == 0x02);
                LCD_D6.State = ((value & 0x04) == 0x04);
                LCD_D7.State = ((value & 0x08) == 0x08);

                ToggleEnable();

                Thread.Sleep(5);
            }
        }

        private void ToggleEnable()
        {
            LCD_E.State = (false);
            LCD_E.State = (true);
            LCD_E.State = (false);
        }

        private byte GetLineAddress(int line)
        {
            switch (line)
            {
                case 0:
                    return LCD_LINE_1;
                case 1:
                    return LCD_LINE_2;
                case 2:
                    return LCD_LINE_3;
                case 3:
                    return LCD_LINE_4;
                default: throw new ArgumentOutOfRangeException();
            }
        }

        private void SetLineAddress(int line)
        {
            SendByte(GetLineAddress(line), LCD_INSTRUCTION);
        }

        /// <summary>
        /// Displays a string into the specific line
        /// </summary>
        /// <param name="text"></param>
        /// <param name="lineNumber"></param>
        public void WriteLine(string text, byte lineNumber)
        {
            ClearLine(lineNumber);
            SetLineAddress(lineNumber);

            var bytes = System.Text.Encoding.UTF8.GetBytes(text);
            foreach (var b in bytes)
            {
                SendByte(b, LCD_DATA);
            }
        }

        public void Write(string text)
        {
            var bytes = System.Text.Encoding.UTF8.GetBytes(text);
            foreach (var b in bytes)
            {
                SendByte(b, LCD_DATA);
            }
        }

        /// <summary>
        /// Move the cursor into an specific line and column
        /// </summary>
        /// <param name="column"></param>
        /// <param name="line"></param>
        public void SetCursorPosition(byte column, byte line)
        {
            byte lineAddress = GetLineAddress(line);
            var address = column + lineAddress;
            SendByte(((byte)(LCD_SETDDRAMADDR | address)), LCD_INSTRUCTION);
        }

        /// <summary>
        /// Clears the screen
        /// </summary>
        public void Clear()
        {
            SendByte(0x01, LCD_INSTRUCTION);
            SetCursorPosition(1, 0);
            Thread.Sleep(5);
        }

        /// <summary>
        /// Clears the specific line
        /// </summary>
        /// <param name="lineNumber"></param>
        public void ClearLine(byte lineNumber)
        {
            SetLineAddress(lineNumber);

            for(int i=0; i < DisplayConfig.Width; i++)
            {
                Write(" ");
            }
        }

        /// <summary>
        /// Adjust the screen's brightness
        /// </summary>
        /// <param name="brightness"></param>
        public void SetBrightness(float brightness = 0.75F)
        {
            Debug.Print("Set brightness not enabled");
        }

        /// <summary>
        /// Adjusts the screen's contrast
        /// </summary>
        /// <param name="contrast"></param>
        public void SetContrast(float contrast = 0.6F)
        {
            LCD_V0.Stop();
            LCD_V0.DutyCycle = contrast;
            LCD_V0.Start();
        }

        /// <summary>
        /// Saves a custom character
        /// </summary>
        /// <param name="characterMap"></param>
        /// <param name="address"></param>
        public void SaveCustomCharacter(byte[] characterMap, byte address)
        {
            address &= 0x7; // we only have 8 locations 0-7
            SendByte((byte)(LCD_SETCGRAMADDR | (address << 3)), LCD_INSTRUCTION);

            for (var i = 0; i < 8; i++)
            {
                SendByte(characterMap[i], LCD_DATA);
            }
        }

        #endregion
    }
}
