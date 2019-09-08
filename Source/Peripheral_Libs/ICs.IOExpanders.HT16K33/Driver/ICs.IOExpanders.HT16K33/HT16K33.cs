using System;
using Microsoft.SPOT;
using Netduino.Foundation.Communications;

namespace ICs.IOExpanders
{
    //128 LED driver
    //39 key input
    public class HT16K33
    {
        #region Enums
        public enum BlinkRate : byte
        {
            Off = 0,
            Fast = 2, //2hz
            Medium = 4, //1hz
            Slow = 8, //0.5hz
        }

        public enum Brightness : byte
        {
            _0,
            _1, 
            _2, 
            _3, 
            _4, 
            _5, 
            _6,
            _7,
            _8,
            _9,
            _10,
            _11,
            _12,
            _13,
            _14,
            _15,
            Off = 0,
            Low = 4,
            Medium = 8,
            High = 12,
            Maximum = 15,
        }

        #endregion Enums

        #region Member variables / fields
        /// <summary>
        ///     HT16K33 LED driver and key scan
        /// </summary>
        private readonly ICommunicationBus _I2CBus;

        //display buffer for 16x8 LEDs
        private byte[] displayBuffer = new byte[16];
        //key buffer for 39 keys
        private byte[] keyBuffer = new byte[6];

        readonly byte HT16K33_DSP = 128;
        readonly byte HT16K33_SS = 32; // System setup register
        readonly byte HT16K33_KDAP = 64; // Key Address Data Pointer
        readonly byte HT16K33_IFAP = 96; // Read INT flag status
        readonly byte HT16K33_DIM = 0xE0; // Set brightness / dimmer
        readonly byte HT16K33_DDAP = 0; //display address pointer

        #endregion Member variables / fields


        #region Constructors

        /// <summary>
        ///     Default HT16K33 constructor is private to prevent it from being used.
        /// </summary>
        private HT16K33() { }

        /// <summary>
        ///     Create a new HT16K33 object using the default parameters
        /// </summary>
        /// <param name="address">Address of the bus on the I2C display.</param>
        /// <param name="speed">Speed of the I2C bus.</param>
        public HT16K33(byte address = 0x70, ushort speed = 100)
        {
            _I2CBus = new I2CBus(address, speed);

            InitHT16K33();
        }

        #endregion Constructors

        void InitHT16K33()
        {
            //   SetIsAwake(true);
            //   SetDisplayOn(true);
            //    SetBlinkRate(BlinkRate.Off);

            _I2CBus.WriteByte(0x21);

            _I2CBus.WriteByte(0x81);

            SetBrightness(Brightness.Maximum);
            ClearDisplay(); 
        }

        void SetIsAwake(bool awake)
        {
            byte value = (byte)(HT16K33_SS | (byte)(awake ? 1 : 0));

            _I2CBus.WriteByte(value);
        }

        void SetDisplayOn(bool isOn)
        {
            byte value = (byte)(HT16K33_DSP | (byte)(isOn ? 1 : 0));

            _I2CBus.WriteByte(value);
        }

        public void SetBlinkRate(BlinkRate blinkRate)
        {
            byte value = (byte)(HT16K33_DSP | (byte)blinkRate);

            _I2CBus.WriteByte(value);
        }

        public void SetBrightness(Brightness brightness)
        {
            byte value = (byte)(HT16K33_DIM | (byte)brightness);

            _I2CBus.WriteByte(value);
        }

        public void ClearDisplay ()
        {
            for (int i = 0; i < displayBuffer.Length; i++)
                displayBuffer[i] = 0;

            UpdateDisplay();
        }

        public void UpdateDisplay ()
        {
            _I2CBus.WriteRegisters(0x0, displayBuffer);
        }

        public void ToggleLed(byte ledIndex, bool ledOn)
        {
            if (ledIndex > 127)
                throw new IndexOutOfRangeException("LED Index must be between 0 and 127");


            var index = ledIndex / 8;
            

            if (ledOn)
            {
                displayBuffer[index] = (byte)(displayBuffer[index] | (byte)(1 << (ledIndex % 8)));
            }
            else
            {
                displayBuffer[index] = (byte)(displayBuffer[index] & ~(byte)(1 << (ledIndex % 8)));
            }
        }

        public bool IsLedOn(int ledIndex)
        {
            //need to do some bit math here
            return false;
        }
    }
}
