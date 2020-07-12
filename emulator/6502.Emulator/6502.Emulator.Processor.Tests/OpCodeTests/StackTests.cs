using _6502.Emulator.Processor.Tests.Extensions;
using FluentAssertions;
using NUnit.Framework;

namespace _6502.Emulator.Processor.Tests.OpCodeTests
{
    [TestFixture]
    internal class StackTests : BaseTests
    {
        [Test]
        public void PLP()
        {
            HavingProcessor()
                .WithMemoryChip(0x0000, (int)OpCode.RTS, 0x2A);

            TickOnce();

            RegisterA().Should().Be(0x2A);
        }
    }
}
