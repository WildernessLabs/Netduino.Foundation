using System;
using Microsoft.SPOT;
using Netduino.Foundation.Audio.Radio;

namespace TEA5767Sample
{
    public class Program
    {
        public static void Main()
        {
            Debug.Print("TEA5767 Sample");

            var radio = new TEA5767();

            radio.SelectFrequency(95.3f);
        }
    }
}
