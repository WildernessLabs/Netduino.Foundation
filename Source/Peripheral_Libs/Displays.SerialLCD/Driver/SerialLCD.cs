using Netduino.Foundation.Displays;
using System;
using System.IO.Ports;
using System.Text;
using System.Threading;

namespace Netduino.Foundation.Displays
{
    /// <summary>
    ///     Encapsulate the functionality required to control the Sparkfun serial LCD display.
    /// </summary>
    public class SerialLCD : IDisposable
    {
        #region Properties

        /// <summary>
        ///     Display configuration (width and height).
        /// </summary>
        public TextDisplayConfig DisplayConfig { get; private set; }

        #endregion

        #region Enums

        /// <summary>
        ///     Describe the cursor style to be displayed.
        /// </summary>
        public enum CursorStyle
        {
            UnderlineOn = 0x0e,
            UnderlineOff = 0x0c,
            BlinkingBoxOn = 0x0d,
            BlinkingBoxOff = 0x0c
        }

        /// <summary>
        ///     Display power state.
        /// </summary>
        public enum DisplayPowerState
        {
            On = 0x0c,
            Off = 0x08
        }

        /// <summary>
        ///     Describe the number of lines and characters on the display.
        /// </summary>
        public enum LCDDimensions
        {
            Characters20Wide = 0x03,
            Characters16Wide = 0x04,
            Lines4 = 0x05,
            Lines2 = 0x06
        }

        /// <summary>
        ///     Possible baud rates for the display.
        /// </summary>
        public enum LCDBaudRate
        {
            Baud2400 = 11,
            Baud4800,
            Baud9600,
            Baud14400,
            Baud19200,
            Baud38400
        }

        /// <summary>
        ///     Direction to move the cursor or the display.
        /// </summary>
        public enum Direction
        {
            Left,
            Right
        }

        #endregion Enums

        #region Constants

        /// <summary>
        ///     Byte used to prefix the extended PCD display commands.
        /// </summary>
        private const byte EXTENDED_LCD_COMMAND_CHARACTER = 0xfe;

        /// <summary>
        ///     Byte used to prefix the interface commands.
        /// </summary>
        private const byte CONFIGURATION_COMMAND_CHARACTER = 0x7c;

        #endregion Constants

        #region Member variables / fields

        /// <summary>
        ///     Comp port being used to communicate with the display.
        /// </summary>
        private readonly SerialPort _comPort;

        /// <summary>
        ///     Track if Dispose has been called.
        /// </summary>
        private bool _disposed;

        #endregion Member variable / fields

        #region Constructors

        /// <summary>
        ///     Make the default constructor private to prevent it being called.
        /// </summary>
        private SerialLCD()
        {
        }

        /// <summary>
        ///     Create a new SerialLCD object.
        /// </summary>
        /// <param name="config">TextDisplayConfig object defining the LCD dimension (null will default to 16x2).</param>
        /// <param name="port">Com port the display is connected to.</param>
        /// <param name="baudRate">Baud rate to use (default = 9600).</param>
        /// <param name="parity">Parity to use (deafult is None).</param>
        /// <param name="dataBits">Number of data bits (default is 8 data bits).</param>
        /// <param name="stopBits">Number of stop bits (default is one stop bit).</param>
        public SerialLCD(TextDisplayConfig config = null,  string port = "COM1", int baudRate = 9600,
            Parity parity = Parity.None, int dataBits = 8, StopBits stopBits = StopBits.One)
        {
            if (config == null)
            {
                // assume a 16x2 LCD.
                DisplayConfig = new TextDisplayConfig() { Height = 2, Width = 16 };
            } 
            else
            {
                DisplayConfig = config;
            }

            // close the com if open
            if ((_comPort != null) && _comPort.IsOpen)
            {
                _comPort.Close();
            }
            // reopen
            _comPort = new SerialPort(port, baudRate, parity, dataBits, stopBits);
            _comPort.Open();

            // configure the LCD controller for the appropriate screen size
            byte lines = 0;
            byte characters = 0;
            switch (DisplayConfig.Width)
            {
                case 16:
                    characters = (byte)LCDDimensions.Characters16Wide;
                    break;
                case 20:
                    characters = (byte)LCDDimensions.Characters20Wide;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(config.Width), "Display width should be 16 or 20.");
            }
            switch (DisplayConfig.Height)
            {
                case 2:
                    lines = (byte)LCDDimensions.Lines2;
                    break;
                case 4:
                    lines = (byte)LCDDimensions.Lines4;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(config.Height), "Display height should be 2 or 4 lines.");
            }
            Send(new[] { CONFIGURATION_COMMAND_CHARACTER, characters, CONFIGURATION_COMMAND_CHARACTER, lines });
            Thread.Sleep(10);
        }

        #endregion Constructors

        #region Implement IDisposable

        /// <summary>
        ///     Implement IDisposable.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            //
            // Call to GC.SupressFinalize will take this object off the finalization queue 
            // and prevent multiple executions.
            GC.SuppressFinalize(this);
        }

        /// <summary>
        ///     Initiate object disposal.
        /// </summary>
        /// <param name="disposing">
        ///     Flag used to determine if the method is being called by the runtime (false) or programmatically
        ///     (true).
        /// </param>
        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    _comPort.Close();
                    _comPort.Dispose();
                }
                _disposed = true; // Done - prevent accidental or intentional Dispose calls.
            }
        }

        #endregion Implement IDisposable

        #region Methods

        /// <summary>
        ///     Write the buffer of data to the COM port (i.e. the display).
        /// </summary>
        /// <param name="buffer">Bytes of data to be sent to the display.</param>
        private void Send(byte[] buffer)
        {
            _comPort.Write(buffer, 0, buffer.Length);
        }

        /// <summary>
        ///     Toggle the splash screen.
        /// </summary>
        public void ToggleSplashScreen()
        {
            Send(new byte[] { CONFIGURATION_COMMAND_CHARACTER, 9 });
        }

        /// <summary>
        ///     Set up the splash screen.
        /// </summary>
        public void SetSplashScreen(string line1, string line2)
        {
            WriteLine(line1, 0);
            WriteLine(line2, 1);
            Send(new byte[] { CONFIGURATION_COMMAND_CHARACTER, 10 });
        }

        /// <summary>
        ///     Change the baud rate of the display using the command interface.
        /// </summary>
        public void SetBaudRate(LCDBaudRate baudRate)
        {
            Send(new[] { CONFIGURATION_COMMAND_CHARACTER, (byte) baudRate });
        }

        /// <summary>
        ///     Clear the display.
        /// </summary>
        public void Clear()
        {
            byte[] buffer = { EXTENDED_LCD_COMMAND_CHARACTER, 0x01 };
            Send(buffer);
        }

        /// <summary>
        ///     Set the cursor position on the display to the specified column and line.
        /// </summary>
        /// <param name="column">Column on the display to move the cursor to (0-15 or 0-19).</param>
        /// <param name="line">Line on the display to move the cursor to (0-3).</param>
        public void SetCursorPosition(byte column, byte line)
        {
            if (column >= DisplayConfig.Width)
            {
                throw new ArgumentOutOfRangeException(nameof(column), "Column exceeds with width of the display.");
            }
            if (line >= DisplayConfig.Height)
            {
                throw new ArgumentOutOfRangeException(nameof(line), "Line exceeds the height of the display.");
            }
            //
            //  The following calculations are taken from the data on page
            //  3 of the data sheet.
            //
            var absoluteCharacterPosition = column;
            switch (line)
            {
                case 0:
                    break;
                case 1:
                    absoluteCharacterPosition += 64;
                    break;
                case 2:
                    absoluteCharacterPosition += (byte) DisplayConfig.Height;
                    break;
                case 3:
                    absoluteCharacterPosition += DisplayConfig.Width == 16 ? (byte) 80 : (byte) 84;
                    break;
            }
            Send(new[] { EXTENDED_LCD_COMMAND_CHARACTER, (byte) (0x80 + (absoluteCharacterPosition & 0xff)) });
        }

        /// <summary>
        ///     Move the cursor either right or left on the display.
        /// </summary>
        /// <param name="direction">Direction to move the cursor, left or right.</param>
        public void MoveCursor(Direction direction)
        {
            if (direction == Direction.Left)
            {
                Send(new byte[] { EXTENDED_LCD_COMMAND_CHARACTER, 0x10 });
            }
            else
            {
                Send(new byte[] { EXTENDED_LCD_COMMAND_CHARACTER, 0x14 });
            }
        }

        /// <summary>
        ///     Scroll the contents of the display one character in the specified direction.
        /// </summary>
        /// <param name="direction">Direction to scroll the display, left or right.</param>
        public void ScrollDisplay(Direction direction)
        {
            if (direction == Direction.Left)
            {
                Send(new byte[] { EXTENDED_LCD_COMMAND_CHARACTER, 0x18 });
            }
            else
            {
                Send(new byte[] { EXTENDED_LCD_COMMAND_CHARACTER, 0x1c });
            }
        }

        /// <summary>
        ///     Set the cursor style to underline or block.  The cursor can also be blinking or solid.
        /// </summary>
        /// <param name="style">New cursor style (Block/Underline, Blinking/Solid).</param>
        public void SetCursorStyle(CursorStyle style)
        {
            Send(new[] { EXTENDED_LCD_COMMAND_CHARACTER, (byte) style });
        }

        /// <summary>
        ///     Display the text at the current cursor position.
        /// </summary>
        /// <param name="text">Text to display.</param>
        [Obsolete("Use .Write methods instead.")]
        public void DisplayText(string text)
        {
            Write(text);
        }

        /// <summary>
        ///     Display the text at the current cursor position.
        /// </summary>
        /// <param name="text">Text to display.</param>
        public void Write(string text)
        {
            Send(Encoding.UTF8.GetBytes(text));
        }

        /// <summary>
        ///     Writes the specified text to the specified line.
        /// </summary>
        /// <param name="lineNumber">Line to write the text on (0-3).</param>
        /// <param name="text">Text to display.</param>
        public void WriteLine(string text, byte lineNumber)
        {
            if (text.Length > DisplayConfig.Width)
            {
                throw new Exception("number characters must be <= columns");
            }

            // clear the line
            ClearLine(lineNumber);

            // write the line
            this.SetCursorPosition(0, lineNumber);
            this.Write(text);
        }

        /// <summary>
        ///     Clears the specified line by writing a string of empty characters
        ///     to it.
        /// </summary>
        /// <param name="lineNumber">Line to clear (0-3)</param>
        public void ClearLine(byte lineNumber)
        {
            // clear the line
            this.SetCursorPosition(0, lineNumber);
            char[] clearChars = new char[DisplayConfig.Width];
            for (int i = 0; i < DisplayConfig.Width; i++)
            {
                clearChars[i] = ' ';
            }
            string clearString = new string(clearChars);
            this.Write(clearString);
        }

        /// <summary>
        ///     Turn the display on or off.
        /// </summary>
        /// <param name="state">New power state for the display.</param>
        public void SetDisplayVisualState(DisplayPowerState state)
        {
            Send(new[] {EXTENDED_LCD_COMMAND_CHARACTER, (byte) state});
        }

        #endregion Methods
    }
}