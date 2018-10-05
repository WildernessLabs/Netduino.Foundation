using System;
using Microsoft.SPOT;

namespace TFTSPISample
{
    public class NFBitmap
    {
        public static byte[] Get8bppGreyScale(byte[] bitmap24bbp)
        {
            int offset = 14 + bitmap24bbp[14];
            int width = bitmap24bbp[18];
            int height = bitmap24bbp[22];

            var dataLength = (bitmap24bbp.Length - offset) / 3;
            var greyScale = new byte[dataLength];

            for (int i = 0; i < dataLength; i++)
            {
                greyScale[i] = (byte)(bitmap24bbp[3 * i + offset] * 7 / 100 +
                                      bitmap24bbp[3 * i + 1 + offset] * 72 / 100 +
                                      bitmap24bbp[3 * i + 2 + offset] * 21 / 100);
            }
            return greyScale;
        }

        //flattens a 8bpp greyscale byte array to 1 bit precision
        static public byte[] Dither8bppto1bpp(byte[] imageData, int width, int height, bool dither)
        {
            byte oldValue, newValue;
            int quantError;
            int index;

            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    index = i + j * width;
                    oldValue = imageData[index];
                    newValue = Get1bppValueFrom8bppGreyscale(oldValue);
                    quantError = oldValue - newValue;
                    imageData[index] = newValue;

                    if (dither == false || i == 0 || i == width - 1 || j == height - 1)
                        continue;

                    imageData[index + 1] = AddQuantError(imageData[index + 1], quantError, 7);
                    imageData[index - 1 + width] = AddQuantError(imageData[index - 1 + width], quantError, 3);
                    imageData[index + width] = AddQuantError(imageData[index + width], quantError, 5);
                    imageData[index + 1 + width] = AddQuantError(imageData[index + 1 + width], quantError, 1);
                }
            }

            return imageData;
        }

        //Add quantiziation error to a single byte for dithering
        static byte AddQuantError(byte value, int quantError, int quantFactor)
        {
            double temp = quantError * quantFactor / 16f;
            temp += value;

            if (temp > 255)
                return 255;

            return (byte)temp;
        }

        //reduce an 8bit value to 1bit 
        static byte Get1bppValueFrom8bppGreyscale(byte data)
        {
            if (data > 127)
                return 255;
            return 0;
        }
    }
}