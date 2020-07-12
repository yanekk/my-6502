using _6502.Emulator.Processor.Tests.Extensions;
using FluentAssertions;
using NUnit.Framework;

namespace _6502.Emulator.Processor.Tests.OpCodeTests
{
    [TestFixture]
    internal class CompareTests : BaseTests
    {
        [Test]
        public void AND_Immediate()
        {
            HavingProcessor()
                .WithMemoryChip(0x0000, (int)OpCode.AND_Immediate, 0x2A);

            TickOnce();

            RegisterA().Should().Be(0x2A);
        }

        [Test]
        public void AND_ZeroPage()
        {
            HavingProcessor(0x1000)
                .WithMemoryChip(0x0000, AnyByte, 0x2A)
                .WithMemoryChip(0x1000, (int)OpCode.AND_ZeroPage, 1);

            TickOnce();

            RegisterA().Should().Be(0x2A);
        }

        [Test]
        public void AND_ZeroPageX()
        {
            HavingProcessor(0x1000)
                .WithInternalState(x: 0x01)
                .WithMemoryChip(0x0000, AnyByte, AnyByte, 0x6C)
                .WithMemoryChip(0x1000, (int)OpCode.AND_ZeroPageX, 0x01);

            TickOnce();

            RegisterA().Should().Be(0x6C);
        }

        [Test]
        public void AND_Absolute()
        {
            HavingProcessor(0x1000)
                .WithMemoryChip(0x1000, (int)OpCode.AND_Absolute, 0x01, 0x20)
                .WithMemoryChip(0x2000, AnyByte, 0x2A);

            TickOnce();

            RegisterA().Should().Be(0x2A);
        }

        [Test]
        public void AND_AbsoluteX()
        {
            HavingProcessor(0x1000)
                .WithInternalState(x: 0x01)
                .WithMemoryChip(0x1000, (int)OpCode.AND_AbsoluteX, 0x01, 0x20)
                .WithMemoryChip(0x2000, AnyByte, AnyByte, 0x2A);

            TickOnce();

            RegisterA().Should().Be(0x2A);
        }

        [Test]
        public void AND_AbsoluteY()
        {
            HavingProcessor(0x1000)
                .WithInternalState(y: 0x01)
                .WithMemoryChip(0x1000, (int)OpCode.AND_AbsoluteY, 0x01, 0x20)
                .WithMemoryChip(0x2000, AnyByte, AnyByte, 0x2A);

            TickOnce();

            RegisterA().Should().Be(0x2A);
        }

        [Test]
        public void AND_IndirectX()
        {
            HavingProcessor(0x1000)
                .WithInternalState(x: 0x01)
                .WithMemoryChip(0x0000, AnyByte, AnyByte, 0x01, 0x20)
                .WithMemoryChip(0x1000, (int)OpCode.AND_ZeroPageIndirectX, 0x01)
                .WithMemoryChip(0x2000, AnyByte, 0x2A);

            TickOnce();

            RegisterA().Should().Be(0x2A);
        }

        [Test]
        public void AND_IndirectY()
        {
            HavingProcessor(0x1000)
                .WithInternalState(y: 0x01)
                .WithMemoryChip(0x0000, AnyByte, 0x01, 0x20)
                .WithMemoryChip(0x1000, (int)OpCode.AND_ZeroPageYIndirect, 0x01)
                .WithMemoryChip(0x2000, AnyByte, AnyByte, 0x2A);

            TickOnce();

            RegisterA().Should().Be(0x2A);
        }
    }
}
