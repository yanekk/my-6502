using _6502.Emulator.Abstract;
using System;

namespace _6502.Emulator.Processor.Tests.TestDoubles
{
    public class TestClock : IClock
    {
        public Action OnTick { private get; set; }

        public TestClock()
        {

        }

        public void TickOnce()
        {
            Tick(1);
        }

        public void Tick(int count)
        {
            for (var i = 0; i < count; i++)
            {
                OnTick();
            }
        }
    }
}
