namespace Netduino.Foundation.Piezos
{
    interface ITuneGenerator
    {
        void PlayTone(float frequency, int duration, float volume = 1);
    }
}