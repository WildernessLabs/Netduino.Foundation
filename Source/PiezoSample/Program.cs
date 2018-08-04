using Netduino.Foundation.Piezos;
using System.Threading;
using N = SecretLabs.NETMF.Hardware.Netduino;

namespace PiezoSample
{
    public class Program
    {
        public static void Main()
        {
            const int NUMBER_TONES = 12;
            float[] melody = new float[NUMBER_TONES] 
            {
                ToneFrequency.NOTE_C5,
                ToneFrequency.NOTE_B4,
                ToneFrequency.NOTE_G4,
                ToneFrequency.NOTE_C5,
                ToneFrequency.NOTE_B4,
                ToneFrequency.NOTE_E4,
                ToneFrequency.REST,
                ToneFrequency.NOTE_C5,
                ToneFrequency.NOTE_C4,
                ToneFrequency.NOTE_G4,
                ToneFrequency.NOTE_A4,
                ToneFrequency.NOTE_C5
            };
            int[] beats = new int[NUMBER_TONES] { 16, 16, 16, 8, 8, 16, 32, 16, 16, 16, 8, 8 };

            PiezoSpeaker piezo = new PiezoSpeaker(N.PWMChannels.PWM_PIN_D5);

            while (true)
            {
                for(int i=0; i<NUMBER_TONES; i++)
                {
                    piezo.PlayTone(melody[i], beats[i] * 100);
                }
                
                Thread.Sleep(1000);
            }
        }
    }
}