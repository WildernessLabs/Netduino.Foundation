using Netduino.Foundation.Piezos;
using System.Threading;
using N = SecretLabs.NETMF.Hardware.Netduino;

namespace PiezoSample
{
    public class Program
    {
        public static void Main()
        {
            const int NUMBER_OF_NOTES = 16;

            float[] melody = new float[NUMBER_OF_NOTES] 
            {
                NoteFrequencies.NOTE_A3,
                NoteFrequencies.NOTE_B3,
                NoteFrequencies.NOTE_CS4,
                NoteFrequencies.NOTE_D4,
                NoteFrequencies.NOTE_E4,
                NoteFrequencies.NOTE_FS4,
                NoteFrequencies.NOTE_GS4,
                NoteFrequencies.NOTE_A4,
                NoteFrequencies.NOTE_A4,
                NoteFrequencies.NOTE_GS4,
                NoteFrequencies.NOTE_FS4,
                NoteFrequencies.NOTE_E4,
                NoteFrequencies.NOTE_D4,
                NoteFrequencies.NOTE_CS4,
                NoteFrequencies.NOTE_B3,
                NoteFrequencies.NOTE_A3,
            };

            var piezo = new PiezoSpeaker(N.PWMChannels.PWM_PIN_D5);

            while (true)
            {
                for(int i = 0; i < NUMBER_OF_NOTES; i++)
                {
                    //PlayTone with a duration in synchronous
                    piezo.PlayTone(melody[i], 600);
                    Thread.Sleep(50);
                }
                
                Thread.Sleep(1000);

                //PlayTone without a duration will return immediately and play the tone
                piezo.PlayTone(NoteFrequencies.NOTE_A4);
                Thread.Sleep(2000);

                //call StopTone to end a tone started without a duration
                piezo.StopTone();

                Thread.Sleep(2000);
            }
        }
    }
}