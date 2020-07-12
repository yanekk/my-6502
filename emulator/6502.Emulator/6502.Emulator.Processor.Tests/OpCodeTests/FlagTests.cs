using _6502.Emulator.Processor.Tests.Extensions;
using FluentAssertions;
using NUnit.Framework;

namespace _6502.Emulator.Processor.Tests.OpCodeTests
{
    [TestFixture]
    internal class FlagTests : BaseTests
    {
        [Test]
        public void SEI()
        {
            HavingProcessor()
                .WithMemoryChip(0x0000, (int)OpCode.SEI, 0x2A);

            TickOnce();

            RegisterA().Should().Be(0x2A);
        }

        [Test]
        public void SED()
        {
            HavingProcessor()
                .WithMemoryChip(0x0000, (int)OpCode.SED, 0x2A);

            TickOnce();

            RegisterA().Should().Be(0x2A);
        }

        [Test]
        public void SEC()
        {
            HavingProcessor()
                .WithMemoryChip(0x0000, (int)OpCode.SEC, 0x2A);

            TickOnce();

            RegisterA().Should().Be(0x2A);
        }
    }
}
