using System;
using Microsoft.SPOT;
using Netduino.Foundation.Communications;

namespace Netduino.Foundation.Sensors.Motion
{
    public class APDS9960
    {
        #region Member variables / fields

        /// <summary>
        ///     Communication bus used to communicate with the sensor.
        /// </summary>
        private readonly ICommunicationBus _apds9960;

        #endregion

        #region enums
        /** I2C Registers */
        enum Registers
        {
            APDS9960_RAM = 0x00,
            ENABLE = 0x80,
            ATIME = 0x81,
            WTIME = 0x83,
            AILTIL = 0x84,
            AILTH = 0x85,
            AIHTL = 0x86,
            AIHTH = 0x87,
            PILT = 0x89,
            PIHT = 0x8B,
            PERS = 0x8C,
            CONFIG1 = 0x8D,
            PPULSE = 0x8E,
            CONTROL = 0x8F,
            CONFIG2 = 0x90,
            ID = 0x92,
            STATUS = 0x93,
            CDATAL = 0x94,
            CDATAH = 0x95,
            RDATAL = 0x96,
            RDATAH = 0x97,
            GDATAL = 0x98,
            GDATAH = 0x99,
            BDATAL = 0x9A,
            BDATAH = 0x9B,
            PDATA = 0x9C,
            POFFSET_UR = 0x9D,
            POFFSET_DL = 0x9E,
            CONFIG3 = 0x9F,
            GPENTH = 0xA0,
            GEXTH = 0xA1,
            GCONF1 = 0xA2,
            GCONF2 = 0xA3,
            GOFFSET_U = 0xA4,
            GOFFSET_D = 0xA5,
            GOFFSET_L = 0xA7,
            GOFFSET_R = 0xA9,
            GPULSE = 0xA6,
            GCONF3 = 0xAA,
            GCONF4 = 0xAB,
            GFLVL = 0xAE,
            GSTATUS = 0xAF,
            IFORCE = 0xE4,
            PICLEAR = 0xE5,
            CICLEAR = 0xE6,
            AICLEAR = 0xE7,
            GFIFO_U = 0xFC,
            GFIFO_D = 0xFD,
            GFIFO_L = 0xFE,
            GFIFO_R = 0xFF,
        };

        /** ADC gain settings */
        enum ADCGain
        {
            GAIN_1X = 0x00,  /**< No gain */
            GAIN_4X = 0x01,  /**< 2x gain */
            GAIN_16X = 0x02, /**< 16x gain */
            GAIN_64X = 0x03  /**< 64x gain */
        }

        /** Proximity gain settings */
        enum ProximityGain
        {
            PGAIN_1X = 0x00, /**< 1x gain */
            PGAIN_2X = 0x04, /**< 2x gain */
            PGAIN_4X = 0x08, /**< 4x gain */
            PGAIN_8X = 0x0C  /**< 8x gain */
        }

        /** Pulse length settings */
        enum PulseLength
        {
            PL_4US = 0x00,  /**< 4uS */
            PL_8US = 0x40,  /**< 8uS */
            PL_16US = 0x80, /**< 16uS */
            PL_32US = 0xC0  /**< 32uS */
        }

        /** LED drive settings */
        enum LedDrive
        {
            DRIVE_100MA = 0x00, /**< 100mA */
            DRIVE_50MA = 0x40,  /**< 50mA */
            DRIVE_25MA = 0x80,  /**< 25mA */
            DRIVE_12MA = 0xC0   /**< 12.5mA */
        }

        /** LED boost settings */
        enum LedBoost
        {
            BOOST_100PCNT = 0x00, /**< 100% */
            BOOST_150PCNT = 0x10, /**< 150% */
            BOOST_200PCNT = 0x20, /**< 200% */
            BOOST_300PCNT = 0x30  /**< 300% */
        }

        /** Dimensions */
        enum Dimensions
        {
            ALL = 0x00,        // All dimensions
            UP_DOWN = 0x01,    // Up/Down dimensions
            LEFT_RIGHT = 0x02, // Left/Right dimensions
        };

        /** FIFO Interrupts */
        enum FifoInterurrupt
        {
            FIFO_1 = 0x00,  // Generate interrupt after 1 dataset in FIFO
            FIFO_4 = 0x01,  // Generate interrupt after 2 datasets in FIFO
            FIFO_8 = 0x02,  // Generate interrupt after 3 datasets in FIFO
            FIFO_16 = 0x03, // Generate interrupt after 4 datasets in FIFO
        };

        /** Gesture Gain */
        enum GestureGain
        {
            GGAIN_1 = 0x00, // Gain 1x
            GGAIN_2 = 0x01, // Gain 2x
            GGAIN_4 = 0x02, // Gain 4x
            GGAIN_8 = 0x03, // Gain 8x
        };

        /** Pulse Lenghts */
        enum GesturePulseLength
        {
            GPL_4US = 0x00,  // Pulse 4us
            GPL_8US = 0x01,  // Pulse 8us
            GPL_16US = 0x02, // Pulse 16us
            GPL_32US = 0x03, // Pulse 32us
        };

        static int APDS9960_UP = 0x01;    /**< Gesture Up */
        static int APDS9960_DOWN = 0x02;  /**< Gesture Down */
        static int APDS9960_LEFT = 0x03;  /**< Gesture Left */
        static int APDS9960_RIGHT = 0x04; /**< Gesture Right */


        #endregion

        #region Constructors

        /// <summary>
        ///     Make the default constructor private so that it cannot be used.
        /// </summary>
        private APDS9960()
        {
        }

        /// <summary>
        ///     Create a new instance of the APDS9960 communicating over the I2C interface.
        /// </summary>
        /// <param name="address">Address of the I2C sensor</param>
        /// <param name="speed">Speed of the I2C bus in KHz</param>
        public APDS9960(byte address = 0x39, ushort speed = 100)
        {
            var device = new I2CBus(address, speed);
            _apds9960 = device;
        }

        #endregion Constructors
    }
}
