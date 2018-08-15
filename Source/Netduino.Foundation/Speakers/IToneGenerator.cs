namespace Netduino.Foundation.Audio
{
    interface IToneGenerator
    {
        void PlayTone(float frequency, int duration = 0);
        void StopTone();
    }
}