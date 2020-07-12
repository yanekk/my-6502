using _6502.Emulator.Processor.Tests.Extensions;
using FluentAssertions;
using NUnit.Framework;

namespace _6502.Emulator.Processor.Tests.OpCodeTests
{
    [TestFixture]
    internal class ShiftRightTests : BaseTests
    {
        [Test]
        public void LSR()
        {
            HavingProcessor(0x1000)
                .WithMemoryChip(0x0000, AnyByte, 0x2A)
                .WithMemoryChip(0x1000, (int)OpCode.LSR, 1);

            TickOnce();

            RegisterA().Should().Be(0x2A);
        }

        [Test]
        public void LSR_ZeroPage()
        {
            HavingProcessor(0x1000)
                .WithMemoryChip(0x0000, AnyByte, 0x2A)
                .WithMemoryChip(0x1000, (int)OpCode.LSR_ZeroPage, 1);

            TickOnce();

            RegisterA().Should().Be(0x2A);
        }

        [Test]
        public void LSR_ZeroPageX()
        {
            HavingProcessor(0x1000)
                .WithInternalState(x: 0x01)
                .WithMemoryChip(0x0000, AnyByte, AnyByte, 0x6C)
                .WithMemoryChip(0x1000, (int)OpCode.LSR_ZeroPageX, 0x01);

            TickOnce();

            RegisterA().Should().Be(0x6C);
        }

        [Test]
        public void LSR_Absolute()
        {
            HavingProcessor(0x1000)
                .WithMemoryChip(0x1000, (int)OpCode.LSR_Absolute, 0x01, 0x20)
                .WithMemoryChip(0x2000, AnyByte, 0x2A);

            TickOnce();

            RegisterA().Should().Be(0x2A);
        }

        [Test]
        public void LSR_AbsoluteX()
        {
            HavingProcessor(0x1000)
                .WithInternalState(x: 0x01)
                .WithMemoryChip(0x1000, (int)OpCode.LSR_AbsoluteX, 0x01, 0x20)
                .WithMemoryChip(0x2000, AnyByte, AnyByte, 0x2A);

            TickOnce();

            RegisterA().Should().Be(0x2A);
        }
    }
}
