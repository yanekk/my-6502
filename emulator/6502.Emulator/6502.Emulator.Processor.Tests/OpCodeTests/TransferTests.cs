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
                .WithInternalState(a: 0x2A)
                .WithMemoryChip(0x0000, (int)OpCode.TAX);

            TickOnce();

            RegisterX().Should().Be(0x2A);
        }

        [Test]
        public void TAY()
        {
            HavingProcessor()
                .WithInternalState(a: 0x2A)
                .WithMemoryChip(0x0000, (int)OpCode.TAY);

            TickOnce();

            RegisterY().Should().Be(0x2A);
        }

        [Test]
        public void TSX()
        {
            HavingProcessor()
                .WithInternalState(stackPointer: 0x2A)
                .WithMemoryChip(0x0000, (int)OpCode.TSX);

            TickOnce();

            RegisterX().Should().Be(0x2A);
        }

        [Test]
        public void TXA()
        {
            HavingProcessor()
                .WithInternalState(x: 0x2A)
                .WithMemoryChip(0x0000, (int)OpCode.TXA, 0x2A);

            TickOnce();

            RegisterA().Should().Be(0x2A);
        }

        [Test]
        public void TXS()
        {
            HavingProcessor()
                .WithInternalState(x: 0x2A)
                .WithMemoryChip(0x0000, (int)OpCode.TXS, 0x2A);

            TickOnce();

            StackPointer().Should().Be(0x2A);
        }

        [Test]
        public void TYA()
        {
            HavingProcessor()
                .WithInternalState(y: 0x2A)
                .WithMemoryChip(0x0000, (int)OpCode.TYA, 0x2A);

            TickOnce();

            RegisterA().Should().Be(0x2A);
        }
    }
}
