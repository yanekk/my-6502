using _6502.Emulator.Processor.Tests.Extensions;
using FluentAssertions;
using NUnit.Framework;

namespace _6502.Emulator.Processor.Tests.OpCodeTests
{
    [TestFixture]
    internal class ExclusiveOrTests : BaseTests
    {
        [Test]
        public void EOR_Immediate()
        {
            HavingProcessor()
                .WithMemoryChip(0x0000, (int)OpCode.EOR_Immediate, 0x2A);

            TickOnce();

            RegisterA().Should().Be(0x2A);
        }

        [Test]
        public void EOR_ZeroPage()
        {
            HavingProcessor(0x1000)
                .WithMemoryChip(0x0000, AnyByte, 0x2A)
                .WithMemoryChip(0x1000, (int)OpCode.EOR_ZeroPage, 1);

            TickOnce();

            RegisterA().Should().Be(0x2A);
        }

        [Test]
        public void EOR_ZeroPageX()
        {
            HavingProcessor(0x1000)
                .WithInternalState(x: 0x01)
                .WithMemoryChip(0x0000, AnyByte, AnyByte, 0x6C)
                .WithMemoryChip(0x1000, (int)OpCode.EOR_ZeroPageX, 0x01);

            TickOnce();

            RegisterA().Should().Be(0x6C);
        }

        [Test]
        public void EOR_Absolute()
        {
            HavingProcessor(0x1000)
                .WithMemoryChip(0x1000, (int)OpCode.EOR_Absolute, 0x01, 0x20)
                .WithMemoryChip(0x2000, AnyByte, 0x2A);

            TickOnce();

            RegisterA().Should().Be(0x2A);
        }

        [Test]
        public void EOR_AbsoluteX()
        {
            HavingProcessor(0x1000)
                .WithInternalState(x: 0x01)
                .WithMemoryChip(0x1000, (int)OpCode.EOR_AbsoluteX, 0x01, 0x20)
                .WithMemoryChip(0x2000, AnyByte, AnyByte, 0x2A);

            TickOnce();

            RegisterA().Should().Be(0x2A);
        }

        [Test]
        public void EOR_AbsoluteY()
        {
            HavingProcessor(0x1000)
                .WithInternalState(y: 0x01)
                .WithMemoryChip(0x1000, (int)OpCode.EOR_AbsoluteY, 0x01, 0x20)
                .WithMemoryChip(0x2000, AnyByte, AnyByte, 0x2A);

            TickOnce();

            RegisterA().Should().Be(0x2A);
        }

        [Test]
        public void EOR_IndirectX()
        {
            HavingProcessor(0x1000)
                .WithInternalState(x: 0x01)
                .WithMemoryChip(0x0000, AnyByte, AnyByte, 0x01, 0x20)
                .WithMemoryChip(0x1000, (int)OpCode.EOR_ZeroPageIndirectX, 0x01)
                .WithMemoryChip(0x2000, AnyByte, 0x2A);

            TickOnce();

            RegisterA().Should().Be(0x2A);
        }

        [Test]
        public void EOR_IndirectY()
        {
            HavingProcessor(0x1000)
                .WithInternalState(y: 0x01)
                .WithMemoryChip(0x0000, AnyByte, 0x01, 0x20)
                .WithMemoryChip(0x1000, (int)OpCode.EOR_ZeroPageYIndirect, 0x01)
                .WithMemoryChip(0x2000, AnyByte, AnyByte, 0x2A);

            TickOnce();

            RegisterA().Should().Be(0x2A);
        }
    }
}
