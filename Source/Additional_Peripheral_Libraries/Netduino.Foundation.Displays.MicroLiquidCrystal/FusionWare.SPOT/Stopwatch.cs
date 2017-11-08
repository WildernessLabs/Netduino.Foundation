using Microsoft.SPOT.Hardware;

namespace System.Diagnostics
{
    public class Stopwatch
    {
        private const long m_ticksPerMillisecond = TimeSpan.TicksPerMillisecond;
        private bool m_isRunning;
        private long m_startTicks;
        private long m_stopTicks;

        public long ElapsedMilliseconds
        {
            get
            {
                if ((m_startTicks != 0) && m_isRunning)
                {
                    return (Utility.GetMachineTime().Ticks - m_startTicks) / m_ticksPerMillisecond;
                }
                if ((m_startTicks != 0) && !m_isRunning)
                {
                    return (m_stopTicks - m_startTicks) / m_ticksPerMillisecond;
                }
                throw new InvalidOperationException();
            }
        }

        private Stopwatch()
        {
        }

        public static Stopwatch StartNew()
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            return stopwatch;
        }

        public void Reset()
        {
            m_startTicks = 0;
            m_stopTicks = 0;
            m_isRunning = false;
        }

        public void Start()
        {
            if ((m_startTicks != 0) && (m_stopTicks != 0))
            {
                m_startTicks = Utility.GetMachineTime().Ticks - (m_stopTicks - m_startTicks); // resume existing timer 
            }
            else
            {
                m_startTicks = Utility.GetMachineTime().Ticks; // start new timer 
            }
            m_isRunning = true;
        }

        public void Stop()
        {
            m_stopTicks = Utility.GetMachineTime().Ticks;
            m_isRunning = false;
        }
    }
}