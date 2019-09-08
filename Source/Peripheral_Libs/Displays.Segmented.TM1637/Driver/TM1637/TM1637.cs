using System;
using Microsoft.SPOT;
using Netduino.Foundation.Communications;

namespace Netduino.Foundation.Displays.Segmented
{
    public class TM1637
    {
        #region Fields / Properties

        /// <summary>
        ///     TM1637 radio.
        /// </summary>
        private readonly ICommunicationBus _I2CBus;

        private byte _brightness;

        static byte SEG_A = 1;
        static byte SEG_B = 2;
        static byte SEG_C = 4;
        static byte SEG_D = 8;
        static byte SEG_E = 16;
        static byte SEG_F = 32;
        static byte SEG_G = 64;
        static byte SEG_DP = 128;

        static byte TM1637_I2C_COMM1 = 0x40;
        static byte TM1637_I2C_COMM2 = 0xC0;
        static byte TM1637_I2C_COMM3 = 0x80;

        static int[] digitToSegment = {
         // XGFEDCBA
           63,  // 0
           6,   // 1
           91,  // 2
           79,  // 3
           102, // 4
           109, // 5
           125, // 6
           7,   // 7
           127, // 8
           111, // 9
           119, // A
           124, // b
           57,  // C
           94,  // d
           121, // E
           113, // F
           64,  // hyphen/minus
        };

        #endregion

        #region Constructors

        private TM1637 () {}

                /// <summary>
        ///     Create a new TEA5767 object using the default parameters
        /// </summary>
        /// <param name="address">Address of the bus on the I2C display.</param>
        /// <param name="speed">Speed of the I2C bus.</param>
        public TM1637(byte address = 0x60, ushort speed = 400)
        {
            _I2CBus = new I2CBus(address, speed);

            InitTM1637();
        }

        #endregion

        #region Methods

        void InitTM1637()
        {

        }

        public void SetBrightness(byte brightness, bool backlightOn)
        {
            _brightness = (byte)((brightness & 0x7) | (backlightOn ? 8 : 0));
        }

        void SetSegments (byte[] segments, int length = 0, int position = 0)
        {
            if (length == 0)
                length = segments.Length;

            // Write COMM1
            start();
            writeByte(TM1637_I2C_COMM1);
            stop();

            // Write COMM2 + first digit address
            start();
            writeByte(TM1637_I2C_COMM2 + (position & 0x03));

            // Write the data bytes
            for (byte k = 0; k < length; k++)
                writeByte(segments[k]);

            stop();

            // Write COMM3 + brightness
            start();
            writeByte(TM1637_I2C_COMM3 + (_brightness & 0x0f));
            stop();
        }

        public void Clear ()
        {
            var data = new byte[] { 0, 0, 0, 0 };
            SetSegments(segments);
        }

        #endregion
    }
}
