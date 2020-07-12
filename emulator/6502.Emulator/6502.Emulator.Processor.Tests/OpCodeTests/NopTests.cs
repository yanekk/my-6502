using _6502.Emulator.Processor.Tests.Extensions;
using FluentAssertions;
using NUnit.Framework;

namespace _6502.Emulator.Processor.Tests.OpCodeTests
{
    internal class NopTests : BaseTests
    {
        [Test]
        public void NOP()
        {
            HavingProcessor()
                .WithMemoryChip(0x0000, (int)OpCode.NOP, 0x2A);

            TickOnce();

            RegisterA().Should().Be(0x2A);
        }
    }
}
