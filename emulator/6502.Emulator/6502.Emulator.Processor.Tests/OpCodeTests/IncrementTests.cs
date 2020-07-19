using _6502.Emulator.Processor.Tests.Extensions;
using _6502.Emulator.Tests.Shared;
using FluentAssertions;
using NUnit.Framework;

namespace _6502.Emulator.Processor.Tests.OpCodeTests
{
    [TestFixture]
    internal class IncrementTests : BaseTests
    {
        [Test]
        public void INC_ZeroPage()
        {
            HavingProcessor(0x1000)
                .WithMemoryChip(0x0000, AnyByte, 0x03)
                .WithMemoryChip(0x1000, (int)OpCode.INC_ZeroPage, 1);

            TickOnce();

            MemoryAt(0x0001).Should().Be(0x04);
        }

        [Test]
        public void INC_ZeroPageX()
        {
            HavingProcessor(0x1000)
                .WithInternalState(x: 0x01)
                .WithMemoryChip(0x0000, AnyByte, AnyByte, 0x03)
                .WithMemoryChip(0x1000, (int)OpCode.INC_ZeroPageX, 0x01);

            TickOnce();

            MemoryAt(0x0002).Should().Be(0x04);
        }

        [Test]
        public void INC_Absolute()
        {
            HavingProcessor(0x1000)
                .WithMemoryChip(0x1000, (int)OpCode.INC_Absolute, 0x01, 0x20)
                .WithMemoryChip(0x2000, AnyByte, 0x03);

            TickOnce();

            MemoryAt(0x2001).Should().Be(0x04);
        }

        [Test]
        public void INC_AbsoluteX()
        {
            HavingProcessor(0x1000)
                .WithInternalState(x: 0x01)
                .WithMemoryChip(0x1000, (int)OpCode.INC_AbsoluteX, 0x01, 0x20)
                .WithMemoryChip(0x2000, AnyByte, AnyByte, 0x03);

            TickOnce();

            MemoryAt(0x2002).Should().Be(0x04);
        }

        [Test]
        public void INX()
        {
            HavingProcessor(0x1000)
                .WithInternalState(x: 0x03)
                .WithMemoryChip(0x1000, (int)OpCode.INX);

            TickOnce();

            RegisterX().Should().Be(0x04);
        }

        [Test]
        public void INY()
        {
            HavingProcessor(0x1000)
                .WithInternalState(y: 0x03)
                .WithMemoryChip(0x1000, (int)OpCode.INY); ;

            TickOnce();

            RegisterY().Should().Be(0x04);
        }
    }
}
