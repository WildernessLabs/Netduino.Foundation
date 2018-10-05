using System;
using Microsoft.SPOT;

namespace ILI9163Sample
{
    public class Dither
    {
        static public byte[] Dither8bppto1bpp(byte[] imageData, int width, int height)
        {
            byte oldValue, newValue;
            int quantError;
            int index;

            //assume 8bpp greyscale
            for (int j = 0; j < height; j++)
            {
                for (int i = 0; i < width; i++)
                {
                    index = i + j * width;
                    oldValue = imageData[index];
                    newValue = Get1bppValue(oldValue);
                    quantError = oldValue - newValue;
                    imageData[index] = newValue;

                    if (i == 0 || i == width - 1 || j == height - 1)
                        continue;

                    imageData[index + 1] = GetCorrectedValue(imageData[index + 1], quantError, 7);
                    imageData[index - 1 + width] = GetCorrectedValue(imageData[index - 1 + width], quantError, 3);
                    imageData[index + width] = GetCorrectedValue(imageData[index + width], quantError, 5);
                    imageData[index + 1 + width] = GetCorrectedValue(imageData[index + 1 + width], quantError, 1);
                }
            }

            return imageData;
        }

        static byte GetCorrectedValue(byte value, int quantError, int quantFactor)
        {
            double temp = quantError * quantFactor / 16f;
            temp += value;

            if (temp > 255)
                return 255;

            return (byte)temp;
        }

        static byte Get1bppValue(byte data)
        {
            if (data > 127)
                return 255;
            return 0;
        }
    }
}
