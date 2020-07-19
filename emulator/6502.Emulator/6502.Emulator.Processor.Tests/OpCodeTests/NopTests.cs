using _6502.Emulator.Processor.Tests.Extensions;
using _6502.Emulator.Tests.Shared;
using NUnit.Framework;

namespace _6502.Emulator.Processor.Tests.OpCodeTests
{
    internal class NopTests : BaseTests
    {
        [Test]
        public void NOP()
        {
            HavingProcessor()
                .WithMemoryChip(0x0000, (int)OpCode.NOP);

            Assert.DoesNotThrow(() => { TickOnce(); });
        }
    }
}
