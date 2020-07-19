using _6502.Emulator.Processor.Tests.Extensions;
using _6502.Emulator.Tests.Shared;
using FluentAssertions;
using NUnit.Framework;

namespace _6502.Emulator.Processor.Tests.OpCodeTests
{
    [TestFixture]
    internal class BitTests : BaseTests
    {
        [Test]
        public void BIT_ZeroPage()
        {
            HavingProcessor()
                .WithMemoryChip(0x0000, (int)OpCode.BIT_ZeroPage, 0x2A);

            TickOnce();

            RegisterA().Should().Be(0x2A);
        }

        [Test]
        public void BIT_Absolute()
        {
            HavingProcessor()
                .WithMemoryChip(0x0000, (int)OpCode.BIT_Absolute, 0x2A);

            TickOnce();

            RegisterA().Should().Be(0x2A);
        }
    }
}
