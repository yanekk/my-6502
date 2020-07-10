using System;
using System.Diagnostics;
using System.Threading;
using _6502.Emulator.Abstract;

namespace _6502.Emulator.Processor
{
    public class ClockChip : IClock
    {
        bool _stopTimer = true;
        private Thread _threadTimer;

        public Action OnTick { private get; set; }

        public void Start()
        {
            _stopTimer = false;

            ThreadStart threadStart = delegate ()
            {
                NotificationTimer(ref _stopTimer);
            };

            _threadTimer = new Thread(threadStart)
            {
                Priority = ThreadPriority.Highest
            };
            _threadTimer.Start();
        }

        public void Stop()
        {
            _stopTimer = true;
        }

        void NotificationTimer(ref bool stopTimer)
        {
            int ticksPerMicrosecond = (int)(Stopwatch.Frequency / 1_000_000D * 2.3);
            var spinWait = new SpinWait();
            while (!stopTimer)
            {
                spinWait.SpinOnce();
                if (spinWait.Count == ticksPerMicrosecond)
                {
                    OnTick();
                    spinWait.Reset();
                }
            }
        }
    }
}
