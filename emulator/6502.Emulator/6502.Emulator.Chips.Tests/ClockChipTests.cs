using NUnit.Framework;
using System.Diagnostics;
using System.Threading;

namespace _6502.Emulator.Processor.Tests
{
    [TestFixture]
    public class ClockChipTests
    {
        [Test]
        public void ClockIsTicking()
        {
            var ticks = 0;
            var autoEvent = new AutoResetEvent(false);
            var stopWatch = new Stopwatch();

            var clock = new ClockChip
            {
                OnTick = () =>
                {
                    ticks++;
                    if (ticks == 1_000_000)
                        autoEvent.Set();
                }
            };
            stopWatch.Start();
            
            clock.Start();
            autoEvent.WaitOne();
            clock.Stop();
            
            stopWatch.Stop();
            Assert.That(stopWatch.ElapsedMilliseconds, Is.EqualTo(1000).Within(50));
        }
    }
}
