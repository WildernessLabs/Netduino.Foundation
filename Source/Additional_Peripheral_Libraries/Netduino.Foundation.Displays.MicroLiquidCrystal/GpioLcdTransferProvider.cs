// Micro Liquid Crystal Library
// http://microliquidcrystal.codeplex.com
// Appache License Version 2.0 

using System;
using Microsoft.SPOT.Hardware;

namespace Netduino.Foundation.Displays.MicroLiquidCrystal
{
    /// <summary>
    ///     GPIO LCD Transport Provider.
    ///     This class uses up to 11 GPIO pins to connect to the LCD display.
    /// </summary>
    public class GpioLcdTransferProvider : ILcdTransferProvider, IDisposable
    {
        #region Properties

        /// <summary>
        ///     True if the interface is operating in four bit mode.
        /// </summary>
        public bool FourBitMode { get; private set; }

        #endregion Properties

        #region Member variables / fields

        /// <summary>
        ///     GPIO ports used for the data lines connecting the microcontroller
        ///     to the LCD
        /// </summary>
        private readonly OutputPort[] _dataPorts;

        /// <summary>
        ///     GPIO port connected to the enable pin on the LCD.
        /// </summary>
        private readonly OutputPort _enablePort;

        /// <summary>
        ///     GPIO port connected to the Register Select pin on the LCD.
        /// </summary>
        private readonly OutputPort _rsPort;

        /// <summary>
        ///     GPIO pin connected to the Read/Write pin on the LCD.
        /// </summary>
        private readonly OutputPort _rwPort;

        /// <summary>
        ///     Indicate if this object has been disposed.
        /// </summary>
        private bool _disposed;

        #endregion Member variables / fields

        #region Constructors and Destructors

        /// <summary>
        ///     Construct a new GPIO LCD Transfer Objects using RS / Enable / D4, D5, D6 and D7.
        /// </summary>
        /// <param name="rs">GPIO pin on the microcontroller connected to the Register Select pin on the LCD display.</param>
        /// <param name="enable">GPIO pin on the microcontroller connected to the Enable pin on the LCD display.</param>
        /// <param name="d4">GPIO pin on the microcontroller connected to digital pin 4 pin on the LCD display.</param>
        /// <param name="d5">GPIO pin on the microcontroller connected to digital pin 5 pin on the LCD display.</param>
        /// <param name="d6">GPIO pin on the microcontroller connected to digital pin 6 pin on the LCD display.</param>
        /// <param name="d7">GPIO pin on the microcontroller connected to digital pin 7 pin on the LCD display.</param>
        /// </param>
        public GpioLcdTransferProvider(Cpu.Pin rs, Cpu.Pin enable, Cpu.Pin d4, Cpu.Pin d5, Cpu.Pin d6, Cpu.Pin d7)
            : this(true, rs, Cpu.Pin.GPIO_NONE, enable, Cpu.Pin.GPIO_NONE, Cpu.Pin.GPIO_NONE, Cpu.Pin.GPIO_NONE,
                   Cpu.Pin.GPIO_NONE, d4, d5, d6, d7)
        {
        }

        /// <summary>
        ///     Construct a new GPIO LCD Transfer Objects using RS / RW / Enable / D4, D5, D6 and D7.
        /// </summary>
        /// <param name="rs">GPIO pin on the microcontroller connected to the Register Select pin on the LCD display.</param>
        /// <param name="rw">GPIO pin on the microcontroller connected to the Read/Write pin on the LCD display.</param>
        /// <param name="enable">GPIO pin on the microcontroller connected to the Enable pin on the LCD display.</param>
        /// <param name="d4">GPIO pin on the microcontroller connected to digital pin 4 pin on the LCD display.</param>
        /// <param name="d5">GPIO pin on the microcontroller connected to digital pin 5 pin on the LCD display.</param>
        /// <param name="d6">GPIO pin on the microcontroller connected to digital pin 6 pin on the LCD display.</param>
        /// <param name="d7">GPIO pin on the microcontroller connected to digital pin 7 pin on the LCD display.</param>
        /// </param>
        public GpioLcdTransferProvider(Cpu.Pin rs, Cpu.Pin rw, Cpu.Pin enable, Cpu.Pin d4, Cpu.Pin d5, Cpu.Pin d6,
            Cpu.Pin d7)
            : this(true, rs, rw, enable, Cpu.Pin.GPIO_NONE, Cpu.Pin.GPIO_NONE, Cpu.Pin.GPIO_NONE, Cpu.Pin.GPIO_NONE, d4,
                   d5, d6, d7)
        {
        }

        /// <summary>
        ///     Construct a new GPIO LCD Transfer Objects using RS / Enable / D0, D1, D2, D3, D4, D5, D6 and D7.
        /// </summary>
        /// <param name="rs">GPIO pin on the microcontroller connected to the Register Select pin on the LCD display.</param>
        /// <param name="enable">GPIO pin on the microcontroller connected to the Enable pin on the LCD display.</param>
        /// <param name="d0">GPIO pin on the microcontroller connected to digital pin 0 pin on the LCD display.</param>
        /// <param name="d1">GPIO pin on the microcontroller connected to digital pin 1 pin on the LCD display.</param>
        /// <param name="d2">GPIO pin on the microcontroller connected to digital pin 2 pin on the LCD display.</param>
        /// <param name="d3">GPIO pin on the microcontroller connected to digital pin 3 pin on the LCD display.</param>
        /// <param name="d4">GPIO pin on the microcontroller connected to digital pin 4 pin on the LCD display.</param>
        /// <param name="d5">GPIO pin on the microcontroller connected to digital pin 5 pin on the LCD display.</param>
        /// <param name="d6">GPIO pin on the microcontroller connected to digital pin 6 pin on the LCD display.</param>
        /// <param name="d7">GPIO pin on the microcontroller connected to digital pin 7 pin on the LCD display.</param>
        /// </param>
        public GpioLcdTransferProvider(Cpu.Pin rs, Cpu.Pin enable, Cpu.Pin d0, Cpu.Pin d1, Cpu.Pin d2, Cpu.Pin d3,
            Cpu.Pin d4, Cpu.Pin d5, Cpu.Pin d6, Cpu.Pin d7)
            : this(false, rs, Cpu.Pin.GPIO_NONE, enable, d0, d1, d2, d3, d4, d5, d6, d7)
        {
        }

        /// <summary>
        ///     Construct a new GPIO LCD Transfer Objects using RS / RW / Enable / D0, D1, D2, D3, D4, D5, D6 and D7.
        /// </summary>
        /// <param name="rs">GPIO pin on the microcontroller connected to the Register Select pin on the LCD display.</param>
        /// <param name="rw">GPIO pin on the microcontroller connected to the Read/Write pin on the LCD display.</param>
        /// <param name="enable">GPIO pin on the microcontroller connected to the Enable pin on the LCD display.</param>
        /// <param name="d0">GPIO pin on the microcontroller connected to digital pin 0 pin on the LCD display.</param>
        /// <param name="d1">GPIO pin on the microcontroller connected to digital pin 1 pin on the LCD display.</param>
        /// <param name="d2">GPIO pin on the microcontroller connected to digital pin 2 pin on the LCD display.</param>
        /// <param name="d3">GPIO pin on the microcontroller connected to digital pin 3 pin on the LCD display.</param>
        /// <param name="d4">GPIO pin on the microcontroller connected to digital pin 4 pin on the LCD display.</param>
        /// <param name="d5">GPIO pin on the microcontroller connected to digital pin 5 pin on the LCD display.</param>
        /// <param name="d6">GPIO pin on the microcontroller connected to digital pin 6 pin on the LCD display.</param>
        /// <param name="d7">GPIO pin on the microcontroller connected to digital pin 7 pin on the LCD display.</param>
        /// </param>
        public GpioLcdTransferProvider(Cpu.Pin rs, Cpu.Pin rw, Cpu.Pin enable, Cpu.Pin d0, Cpu.Pin d1, Cpu.Pin d2,
            Cpu.Pin d3, Cpu.Pin d4, Cpu.Pin d5, Cpu.Pin d6, Cpu.Pin d7)
            : this(false, rs, rw, enable, d0, d1, d2, d3, d4, d5, d6, d7)
        {
        }

        /// <summary>
        ///     Creates a variable of type LiquidCrystal. The display can be controlled using 4 or 8 data lines. If the former,
        ///     omit the pin numbers for d0 to d3 and leave those lines unconnected. The RW pin can be tied to ground instead of
        ///     connected to a pin on the Arduino; if so, omit it from this function's parameters.
        /// </summary>
        /// <param name="fourBitMode">Indicate if four bit mode be used.</param>
        /// <param name="rs">The number of the CPU pin that is connected to the RS (register select) pin on the LCD.</param>
        /// <param name="rw">The number of the CPU pin that is connected to the RW (Read/Write) pin on the LCD (optional).</param>
        /// <param name="enable">the number of the CPU pin that is connected to the enable pin on the LCD.</param>
        /// <param name="d0">GPIO pin on the microcontroller connected to digital pin 0 pin on the LCD display.</param>
        /// <param name="d1">GPIO pin on the microcontroller connected to digital pin 1 pin on the LCD display.</param>
        /// <param name="d2">GPIO pin on the microcontroller connected to digital pin 2 pin on the LCD display.</param>
        /// <param name="d3">GPIO pin on the microcontroller connected to digital pin 3 pin on the LCD display.</param>
        /// <param name="d4">GPIO pin on the microcontroller connected to digital pin 4 pin on the LCD display.</param>
        /// <param name="d5">GPIO pin on the microcontroller connected to digital pin 5 pin on the LCD display.</param>
        /// <param name="d6">GPIO pin on the microcontroller connected to digital pin 6 pin on the LCD display.</param>
        /// <param name="d7">GPIO pin on the microcontroller connected to digital pin 7 pin on the LCD display.</param>
        /// </param>
        public GpioLcdTransferProvider(bool fourBitMode, Cpu.Pin rs, Cpu.Pin rw, Cpu.Pin enable,
            Cpu.Pin d0, Cpu.Pin d1, Cpu.Pin d2, Cpu.Pin d3,
            Cpu.Pin d4, Cpu.Pin d5, Cpu.Pin d6, Cpu.Pin d7)
        {
            FourBitMode = fourBitMode;

            if (rs == Cpu.Pin.GPIO_NONE)
            {
                throw new ArgumentException("rs");
            }
            _rsPort = new OutputPort(rs, false);
            //
            //  We can save 1 pin by not using RW. Indicate by passing Cpu.Pin.GPIO_NONE 
            //  instead of a pin number.
            //
            if (rw != Cpu.Pin.GPIO_NONE) // (RW is optional)
            {
                _rwPort = new OutputPort(rw, false);
            }

            if (enable == Cpu.Pin.GPIO_NONE)
            {
                throw new ArgumentException("enable");
            }
            _enablePort = new OutputPort(enable, false);

            var dataPins = new[] { d0, d1, d2, d3, d4, d5, d6, d7 };
            _dataPorts = new OutputPort[8];
            for (var i = 0; i < 8; i++)
            {
                if (dataPins[i] != Cpu.Pin.GPIO_NONE)
                {
                    _dataPorts[i] = new OutputPort(dataPins[i], false);
                }
            }
        }

        /// <summary>
        ///     Dispose of the GPIO LCD Tranfer object.
        /// </summary>
        ~GpioLcdTransferProvider()
        {
            Dispose(false);
        }

        #endregion Constructors and Destructors

        #region IDisposable interface methods

        /// <summary>
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
        }

        /// <summary>
        ///     Dispose of the GPIO LCD Tranfer object.
        /// </summary>
        /// <param name="disposing">Indicate if the GC should be called.</param>
        private void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _rsPort.Dispose();
                _rwPort.Dispose();
                _enablePort.Dispose();

                for (var i = 0; i < 8; i++)
                {
                    if (_dataPorts[i] != null)
                    {
                        _dataPorts[i].Dispose();
                    }
                }
                _disposed = true;
            }

            if (disposing)
            {
                GC.SuppressFinalize(this);
            }
        }

        #endregion IDisposable interface methods

        #region Methods

        /// <summary>
        ///     Write either command or data, with automatic 4/8-bit selection
        /// </summary>
        /// <param name="value">value to write</param>
        /// <param name="mode">Mode for RS (register select) pin.</param>
        /// <param name="backlight">Backlight state.</param>
        public void Send(byte value, bool mode, bool backlight)
        {
            if (_disposed)
            {
                throw new ObjectDisposedException();
            }

            //  TODO: set backlight

            _rsPort.Write(mode);
            //
            //  If there is a RW pin, set it low to Write.
            //
            if (_rwPort != null)
            {
                _rwPort.Write(false);
            }

            if (!FourBitMode)
            {
                Write8Bits(value);
            }
            else
            {
                Write4Bits((byte) (value >> 4));
                Write4Bits(value);
            }
        }

        /// <summary>
        ///     Write the byte as a single 8 bit value.
        /// </summary>
        /// <param name="value">Value to write to the GPIO pins.</param>
        private void Write8Bits(byte value)
        {
            for (var i = 0; i < 8; i++)
            {
                _dataPorts[i].Write(((value >> i) & 0x01) == 0x01);
            }

            PulseEnable();
        }

        /// <summary>
        ///     Write the byte in two four bit chunks.
        /// </summary>
        /// <param name="value">Value to the write to the GPIO ports</param>
        private void Write4Bits(byte value)
        {
            for (var i = 0; i < 4; i++)
            {
                _dataPorts[4 + i].Write(((value >> i) & 0x01) == 0x01);
            }

            PulseEnable();
        }

        /// <summary>
        ///     Toggle the pulse enable pin.
        /// </summary>
        private void PulseEnable()
        {
            _enablePort.Write(false);
            _enablePort.Write(true); // enable pulse must be >450ns
            _enablePort.Write(false); // commands need > 37us to settle
        }

        #endregion Methods
    }
}