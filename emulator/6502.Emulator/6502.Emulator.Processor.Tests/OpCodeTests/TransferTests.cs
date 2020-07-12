using _6502.Emulator.Processor.Tests.Extensions;
using FluentAssertions;
using NUnit.Framework;

namespace _6502.Emulator.Processor.Tests.OpCodeTests
{
    internal class TransferTests : BaseTests
    {
        [Test]
        public void TAX()
        {
            HavingProcessor()
                .WithMemoryChip(0x0000, (int)OpCode.TAX, 0x2A);

            TickOnce();

            RegisterA().Should().Be(0x2A);
        }

        [Test]
        public void TAY()
        {
            HavingProcessor()
                .WithMemoryChip(0x0000, (int)OpCode.TAY, 0x2A);

            TickOnce();

            RegisterA().Should().Be(0x2A);
        }

        [Test]
        public void TSX()
        {
            HavingProcessor()
                .WithMemoryChip(0x0000, (int)OpCode.TSX, 0x2A);

            TickOnce();

            RegisterA().Should().Be(0x2A);
        }

        [Test]
        public void TXA()
        {
            HavingProcessor()
                .WithMemoryChip(0x0000, (int)OpCode.TXA, 0x2A);

            TickOnce();

            RegisterA().Should().Be(0x2A);
        }

        [Test]
        public void TXS()
        {
            HavingProcessor()
                .WithMemoryChip(0x0000, (int)OpCode.TXS, 0x2A);

            TickOnce();

            RegisterA().Should().Be(0x2A);
        }

        [Test]
        public void TYA()
        {
            HavingProcessor()
                .WithMemoryChip(0x0000, (int)OpCode.TYA, 0x2A);

            TickOnce();

            RegisterA().Should().Be(0x2A);
        }
    }
}
