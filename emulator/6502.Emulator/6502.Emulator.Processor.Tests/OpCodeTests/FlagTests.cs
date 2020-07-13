using _6502.Emulator.Processor.Tests.Extensions;
using FluentAssertions;
using NUnit.Framework;

namespace _6502.Emulator.Processor.Tests.OpCodeTests
{
    [TestFixture]
    internal class FlagTests : BaseTests
    {
        [Test]
        public void CLV()
        {
            HavingProcessor()
                .WithInternalState(overflowFlag: true)
                .WithMemoryChip(0x0000, (int)OpCode.CLV);

            TickOnce();

            OverflowFlag().Should().BeFalse();
        }

        [Test]
        public void SEI()
        {
            HavingProcessor()
                .WithInternalState(interruptDisableFlag: false)
                .WithMemoryChip(0x0000, (int)OpCode.SEI);

            TickOnce();

            InterrupDisableFlag().Should().BeTrue();
        }

        [Test]
        public void CLI()
        {
            HavingProcessor()
                .WithInternalState(interruptDisableFlag: true)
                .WithMemoryChip(0x0000, (int)OpCode.CLI);

            TickOnce();

            InterrupDisableFlag().Should().BeFalse();
        }

        [Test]
        public void SED()
        {
            HavingProcessor()
                .WithInternalState(decimalFlag: false)
                .WithMemoryChip(0x0000, (int)OpCode.SED);

            TickOnce();

            DecimalFlag().Should().BeTrue();
        }

        [Test]
        public void CLD()
        {
            HavingProcessor()
                .WithInternalState(decimalFlag: true)
                .WithMemoryChip(0x0000, (int)OpCode.CLD);

            TickOnce();

            DecimalFlag().Should().BeFalse();
        }

        [Test]
        public void SEC()
        {
            HavingProcessor()
                .WithInternalState(carryFlag: false)
                .WithMemoryChip(0x0000, (int)OpCode.SEC);

            TickOnce();

            CarryFlag().Should().BeTrue();
        }

        [Test]
        public void CLC()
        {
            HavingProcessor()
                .WithInternalState(carryFlag: true)
                .WithMemoryChip(0x0000, (int)OpCode.CLC);

            TickOnce();
            CarryFlag().Should().BeFalse();
        }
    }
}
