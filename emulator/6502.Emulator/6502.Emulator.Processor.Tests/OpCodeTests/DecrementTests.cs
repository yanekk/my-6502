using _6502.Emulator.Processor.Tests.Extensions;
using _6502.Emulator.Tests.Shared;
using FluentAssertions;
using NUnit.Framework;

namespace _6502.Emulator.Processor.Tests.OpCodeTests
{
    [TestFixture]
    internal class DecrementTests : BaseTests
    {
        [Test]
        public void DEC_ZeroPage()
        {
            HavingProcessor(0x1000)
                .WithMemoryChip(0x0000, AnyByte, 0x03)
                .WithMemoryChip(0x1000, (int)OpCode.DEC_ZeroPage, 1);

            TickOnce();

            MemoryAt(0x0001).Should().Be(0x02);
        }

        [Test]
        public void DEC_ZeroPageX()
        {
            HavingProcessor(0x1000)
                .WithInternalState(x: 0x01)
                .WithMemoryChip(0x0000, AnyByte, AnyByte, 0x03)
                .WithMemoryChip(0x1000, (int)OpCode.DEC_ZeroPageX, 0x01);

            TickOnce();

            MemoryAt(0x0002).Should().Be(0x02);
        }

        [Test]
        public void DEC_Absolute()
        {
            HavingProcessor(0x1000)
                .WithMemoryChip(0x1000, (int)OpCode.DEC_Absolute, 0x01, 0x20)
                .WithMemoryChip(0x2000, AnyByte, 0x03);

            TickOnce();

            MemoryAt(0x2001).Should().Be(0x02);
        }

        [Test]
        public void DEC_AbsoluteX()
        {
            HavingProcessor(0x1000)
                .WithInternalState(x: 0x01)
                .WithMemoryChip(0x1000, (int)OpCode.DEC_AbsoluteX, 0x01, 0x20)
                .WithMemoryChip(0x2000, AnyByte, AnyByte, 0x03);

            TickOnce();

            MemoryAt(0x2002).Should().Be(0x02);
        }

        [Test]
        public void DEX()
        {
            HavingProcessor(0x1000)
                .WithInternalState(x: 0x03)
                .WithMemoryChip(0x1000, (int)OpCode.DEX);

            TickOnce();

            RegisterX().Should().Be(0x02);
        }

        [Test]
        public void DEY()
        {
            HavingProcessor(0x1000)
                .WithInternalState(y: 0x03)
                .WithMemoryChip(0x1000, (int)OpCode.DEY); ;

            TickOnce();

            RegisterY().Should().Be(0x02);
        }
    }
}
