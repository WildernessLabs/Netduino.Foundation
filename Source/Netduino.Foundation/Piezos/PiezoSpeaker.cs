using Microsoft.SPOT.Hardware;
using System.Threading;

namespace Netduino.Foundation.Piezos
{
    public class PiezoSpeaker : ITuneGenerator
    {
        private PWM _pwm;
        private bool _isPlaying = false;

        public PiezoSpeaker(Cpu.PWMChannel pwmChannel)
        {
            _pwm = new PWM(pwmChannel, 100, 0, false);
        }

        /// <summary>
        /// Play a frequency for a specified duration
        /// </summary>
        /// <param name="frequency">The frequency in hertz of the tone to be played</param>
        /// <param name="duration">How long the note is played in milliseconds</param>
        /// <param name="volume">The volume to play the note [0..1] </param>
        public void PlayTone(float frequency, int duration, float volume = 1)
        {
            if (!_isPlaying)
            {
                _isPlaying = true;

                if (frequency > 0)
                {
                    var period = (uint)(1000000 / frequency);

                    _pwm.Period = period;
                    _pwm.Duration = period / 2;
                    _pwm.DutyCycle = Map(volume, 0, 1, 0, 0.025f);
                    _pwm.Start();
                }

                Thread.Sleep(duration);

                _pwm.Stop();

                _isPlaying = false;
            }
        }

        float Map(float value, float fromLow, float fromHigh, float toLow, float toHigh)
        {
            return (((toHigh - toLow) * (value - fromLow)) / (fromHigh - fromLow)) - toLow;
        }
    }
}