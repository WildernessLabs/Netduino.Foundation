using System;
using System.Text;
using System.IO.Ports;

namespace Netduino.Foundation.Core
{
    /// <summary>
    /// Provide a mechanism for reading lines of text from a SerialPort.
    /// </summary>
    public class SerialTextFile
    {
        #region Constants

        /// <summary>
        /// Default buffer size for the incoming data from the serial port.
        /// </summary>
        private const int MAXIMUM_BUFFER_SIZE = 1024;

        #endregion Constants

        #region Member variables / fields

        /// <summary>
        /// Serial port object that the 
        /// </summary>
        SerialPort _serialPort = null;

        /// <summary>
        /// Buffer to hold the incoming text from the serial port.
        /// </summary>
        private string _buffer = String.Empty;

        /// <summary>
        /// Character(s) that indicate an end of line in the text stream.
        /// </summary>
        private string _endOfLine = "\r\n";

        #endregion Member variables / fields

        #region Events and delegates

        /// <summary>
        /// Delegate for the line ready event.
        /// </summary>
        public delegate void LineReady(string line);

        /// <summary>
        /// A complete line of text has been read, send this to the event subscriber.
        /// </summary>
        public event LineReady OnLineReady = null;

        #endregion Events and delegates

        #region Constructors

        /// <summary>
        /// Default constructor for the SerialTextFile class, made private to prevent the
        /// programmer from using this method of construcing an object.
        /// </summary>
        private SerialTextFile()
        {
        }

        /// <summary>
        /// Create a new SerialTextFile and attach the instance to the specfied serial port.
        /// </summary>
        /// <param name="port">Serial port name.</param>
        /// <param name="baudRate">Baud rate.</param>
        /// <param name="parity">Parity.</param>
        /// <param name="dataBits">Data bits.</param>
        /// <param name="stopBits">Stop bits.</param>
        /// <param name="endOfLine">Text indicating the end of a line of text.</param>
        public SerialTextFile(string port, int baudRate, Parity parity, int dataBits, StopBits stopBits, string endOfLine)
        {
            _serialPort = new SerialPort(port, baudRate, parity, dataBits, stopBits);
            _endOfLine = endOfLine;
            _serialPort.DataReceived += _serialPort_DataReceived;
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Open the serial port and start processing the data from the serial port.
        /// </summary>
        public void Open()
        {
            if (!_serialPort.IsOpen)
            {
                _serialPort.Open();
            }
        }

        /// <summary>
        /// Close the serial port and stop processing data.
        /// </summary>
        public void Close()
        {
            if (_serialPort.IsOpen)
            {
                _serialPort.Close();
            }
        }

        #endregion Methods

        #region Interrupt handlers

        /// <summary>
        /// Process the data from the serial port.
        /// </summary>
        void _serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            if (e.EventType == SerialData.Chars)
            {
                lock (_buffer)
                {
                    int amount;
                    byte[] buffer;

                    buffer = new byte[MAXIMUM_BUFFER_SIZE];
                    amount = ((SerialPort) sender).Read(buffer, 0, MAXIMUM_BUFFER_SIZE);
                    if (amount > 0)
                    {
                        if ((amount + _buffer.Length) <= MAXIMUM_BUFFER_SIZE)
                        {
                            _buffer += Encoding.UTF8.GetChars(buffer);
                        }
                        else
                        {
                            throw new Exception("Buffer overflow");
                        }
                    }
                    int eolMarkerPosition = _buffer.IndexOf(_endOfLine);
                    while (eolMarkerPosition > 0)
                    {
                        string line = _buffer.Substring(0, eolMarkerPosition - 2);
                        _buffer = _buffer.Substring(eolMarkerPosition + 2);
                        eolMarkerPosition = _buffer.IndexOf(_endOfLine);
                        if (OnLineReady != null)
                        {
                            OnLineReady(line);   
                        }
                    }
                }
            }
        }

        #endregion Interrupt handlers
    }
}
