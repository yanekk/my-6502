using _6502.Emulator.Processor.Tests.Extensions;
using _6502.Emulator.Tests.Shared;
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
            HavingProcessor()
                .WithInternalState(a: 0x04)
                .WithMemoryChip(0x0000, (int)OpCode.LSR);

            TickOnce();

            RegisterA().Should().Be(0x02);
        }

        [Test]
        public void LSR_ZeroPage()
        {
            HavingProcessor(0x1000)
                .WithMemoryChip(0x0000, AnyByte, 0x04)
                .WithMemoryChip(0x1000, (int)OpCode.LSR_ZeroPage, 1);

            TickOnce();

            MemoryAt(0x0001).Should().Be(0x02);
        }

        [Test]
        public void LSR_ZeroPageX()
        {
            HavingProcessor(0x1000)
                .WithInternalState(x: 0x01)
                .WithMemoryChip(0x0000, AnyByte, AnyByte, 0x04)
                .WithMemoryChip(0x1000, (int)OpCode.LSR_ZeroPageX, 0x01);

            TickOnce();

            MemoryAt(0x0002).Should().Be(0x02);
        }

        [Test]
        public void LSR_Absolute()
        {
            HavingProcessor(0x1000)
                .WithMemoryChip(0x1000, (int)OpCode.LSR_Absolute, 0x01, 0x20)
                .WithMemoryChip(0x2000, AnyByte, 0x04);

            TickOnce();

            MemoryAt(0x2001).Should().Be(0x02);
        }

        [Test]
        public void LSR_AbsoluteX()
        {
            HavingProcessor(0x1000)
                .WithInternalState(x: 0x01)
                .WithMemoryChip(0x1000, (int)OpCode.LSR_AbsoluteX, 0x01, 0x20)
                .WithMemoryChip(0x2000, AnyByte, AnyByte, 0x04);

            TickOnce();

            MemoryAt(0x2002).Should().Be(0x02);
        }
    }
}
