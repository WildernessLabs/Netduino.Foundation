using System;
using Microsoft.SPOT.Hardware;
using Netduino.Foundation.Communications;

namespace Netduino.Foundation.ICs.IOExpanders.x74595
{
    /// <summary>
    ///     Provide an interface to connect to a 74595 shift register.
    /// </summary>
    /// <remarks>
    ///     Control the outputs from a 74595 shift register (or a chain of shift registers)
    ///     using a SPI interface.
    /// </remarks>
    public class x74595
    {
        #region Constants

        /// <summary>
        ///     Error message for a pin number out of range exception.
        /// </summary>
        private const string EM_PIN_RANGE_MESSAGE = "x74595: Pin number is out of range.";

        /// <summary>
        ///     Error message for the when the input data length (byte array) does not match
        ///     the number of chips in the shift register chain.
        /// </summary>
        private const string EM_LENGTH_MISMATCH = "x74595: input data length does not match shift register length.";

        #endregion

        #region Member variables / fields

        /// <summary>
        ///     Array containing the byte values for each of the 74595 chips.
        /// </summary>
        private readonly byte[] _chips;

        /// <summary>
        ///     Number of pins available based upon the number of chips in the chain.
        /// </summary>
        private readonly byte _numberOfPins = 0;

        /// <summary>
        ///     SPI interface used to communicate with the shift registers.
        /// </summary>
        private readonly ICommunicationBus _spi;

        #endregion Member variables / fields

        #region Constructor(s)

        /// <summary>
        ///     Default constructor.
        /// </summary>
        /// <remarks>
        ///     This is private to prevent the programmer from calling it explicitly.
        /// </remarks>
        private x74595()
        {
        }

        /// <summary>
        ///     Constructor a x74595 object.
        /// </summary>
        /// <param name="pins">Number of pins in the shift register (should be a multiple of 8 pins).</param>
        /// <param name="config">SPI Configuration object.</param>
        public x74595(byte pins, SPI.Configuration config)
        {
            if ((pins > 0) && ((pins % 8) == 0))
            {
                _numberOfPins = pins;
                _chips = new byte[pins / 8];
                _spi = new SPIBus(config);
                Clear();
            }
            else
            {
                throw new ArgumentOutOfRangeException(nameof(pins), "x74595: Size must be greater than zero and a multiple of 8 pins");
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     Overload the index operator to allow the user to get/set a particular
        ///     pin in the shift register.
        /// </summary>
        /// <param name="pin">Bit number to get/set.</param>
        /// <returns>Value in the specified pin.</returns>
        public bool this[int pin]
        {
            get
            {
                if ((pin >= 0) && (pin < _numberOfPins))
                {
                    int chip = _chips.Length - (pin / 8) - 1;
                    int bit = pin % 8;
                    return ((_chips[chip] & bit) == 1);
                }
                throw new IndexOutOfRangeException(EM_PIN_RANGE_MESSAGE);
            }
            set
            {
                //
                //  This check is effectively done twice as WriteToPort also checks the
                //  pin number.  However, this check is slightly different as the input
                //  type is an integer and not a byte and so it needs to be done here
                //  as well.
                //
                if ((pin >= 0) && (pin < _numberOfPins))
                {
                    WriteToPort((byte) (pin & 0xff), value);
                }
                else
                {
                    throw new IndexOutOfRangeException(EM_PIN_RANGE_MESSAGE);
                }
            }
        }

        /// <summary>
        /// Creates a new DigitalOutputPort using the specified pin and initial state.
        /// </summary>
        /// <param name="pin">The pin number to create the port on.</param>
        /// <param name="initialState">Whether the pin is initially high or low.</param>
        /// <returns></returns>
        public DigitalOutputPort CreateOutputPort(byte pin, bool initialState)
        {
            if (IsValidPin(pin))
            {
                // create the convenience class
                return new DigitalOutputPort(this, pin, initialState);
            }

            throw new IndexOutOfRangeException(EM_PIN_RANGE_MESSAGE);
        }

        /// <summary>
        ///     Sets a particular pin's value. 
        /// </summary>
        /// <param name="pin">The pin to write to.</param>
        /// <param name="value">The value to write. True for high, false for low.</param>
        public void WriteToPort(byte pin, bool value)
        {
            if (IsValidPin(pin))
            {
                //
                //  Convert the pin number to a chip and bit combination in
                //  little endian form.
                //
                int chip = _chips.Length - (pin / 8) - 1;
                int bit = pin % 8;
                if (value)
                {
                    _chips[chip] |= (byte) (1 << bit);
                }
                else
                {
                    _chips[chip] &= (byte) (~(1 << bit));
                }
                LatchData();
            }
            else
            {
                throw new IndexOutOfRangeException(EM_PIN_RANGE_MESSAGE);
            }
        }

        /// <summary>
        ///     Outputs a byte value across all of the pins by writing directly 
        ///     to the shift register.
        /// </summary>
        /// <remarks>
        ///     Calling this method will result in an exception being thrown if
        ///     the shift register chain contains more than one shift register.
        /// </remarks>
        /// <param name="value">Byte value to be written to the chain of shift registers.</param>
        public void WriteToPorts(byte value)
        {
            WriteToPorts(new byte[] { value });
        }

        /// <summary>
        ///     Write a number of bytes to the shift register.
        /// </summary>
        /// <remarks>
        ///     Note that the number of bytes being written must match the number
        ///     of bytes used to implement the shift register.
        /// </remarks>
        /// <param name="values">Array of byte values to be written.</param>
        public void WriteToPorts(byte[] values)
        {
            if (values.Length != _chips.Length)
            {
                throw new ArgumentOutOfRangeException(nameof(values), EM_LENGTH_MISMATCH);
            }

            for (var index = 0; index < values.Length; index++)
            {
                _chips[index] = values[index];
            }
            LatchData();
        }

        /// <summary>
        ///     Clear all of the pins in the shift register.
        /// </summary>
        /// <param name="latch">If true, latch the data after the shift register is cleared (default is false).</param>
        public void Clear(bool latch = false)
        {
            for (var index = 0; index < _chips.Length; index++)
            {
                _chips[index] = 0;
            }

            if (latch)
            {
                LatchData();
            }
        }

        /// <summary>
        ///     Send the data to the SPI interface.
        /// </summary>
        protected void LatchData()
        {
            _spi.WriteBytes(_chips);
        }

        /// <summary>
        ///     Check if the specified pin is valid.
        /// </summary>
        /// <param name="pin">Pin number</param>
        /// <returns>True if the pin number is valid, false if it is invalid.</returns>
        protected bool IsValidPin(byte pin)
        {
            return (pin <= _numberOfPins);
        }

        #endregion
    }
}