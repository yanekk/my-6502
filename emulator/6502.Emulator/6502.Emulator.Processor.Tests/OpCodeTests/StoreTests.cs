using _6502.Emulator.Processor.Tests.Extensions;
using FluentAssertions;
using NUnit.Framework;

namespace _6502.Emulator.Processor.Tests.OpCodeTests
{
    [TestFixture]
    internal class StoreTests : BaseTests
    {
        [Test]
        public void STA_ZeroPage()
        {
            HavingProcessor(0x1000)
                .WithInternalState(a: 0x2A)
                .WithMemoryChip(0x0000, AnyByte, 0x00)
                .WithMemoryChip(0x1000, (int)OpCode.STA_ZeroPage, 1);

            TickOnce();

            MemoryAt(0x0001).Should().Be(0x2A);
        }

        [Test]
        public void STA_ZeroPageX()
        {
            HavingProcessor(0x1000)
                .WithInternalState(x: 0x01, a: 0x2A)
                .WithMemoryChip(0x0000, AnyByte, AnyByte, 0x00)
                .WithMemoryChip(0x1000, (int)OpCode.STA_ZeroPageX, 0x01);

            TickOnce();

            MemoryAt(0x0002).Should().Be(0x2A);
        }

        [Test]
        public void STA_Absolute()
        {
            HavingProcessor(0x1000)
                .WithInternalState(a: 0x2A)
                .WithMemoryChip(0x1000, (int)OpCode.STA_Absolute, 0x01, 0x20)
                .WithMemoryChip(0x2000, AnyByte, 0x00);

            TickOnce();

            MemoryAt(0x2001).Should().Be(0x2A);
        }

        [Test]
        public void STA_AbsoluteX()
        {
            HavingProcessor(0x1000)
                .WithInternalState(x: 0x01, a: 0x2A)
                .WithMemoryChip(0x1000, (int)OpCode.STA_AbsoluteX, 0x01, 0x20)
                .WithMemoryChip(0x2000, AnyByte, AnyByte, 0x00);

            TickOnce();

            MemoryAt(0x2002).Should().Be(0x2A);
        }

        [Test]
        public void STA_AbsoluteY()
        {
            HavingProcessor(0x1000)
                .WithInternalState(y: 0x01, a: 0x2A)
                .WithMemoryChip(0x1000, (int)OpCode.STA_AbsoluteY, 0x01, 0x20)
                .WithMemoryChip(0x2000, AnyByte, AnyByte, 0x00);

            TickOnce();

            MemoryAt(0x2002).Should().Be(0x2A);
        }

        [Test]
        public void STA_IndirectX()
        {
            HavingProcessor(0x1000)
                .WithInternalState(x: 0x01, a: 0x2A)
                .WithMemoryChip(0x0000, AnyByte, AnyByte, 0x01, 0x20)
                .WithMemoryChip(0x1000, (int)OpCode.STA_ZeroPageIndirectX, 0x01)
                .WithMemoryChip(0x2000, AnyByte, 0x00);

            TickOnce();

            MemoryAt(0x2001).Should().Be(0x2A);
        }

        [Test]
        public void STA_IndirectY()
        {
            HavingProcessor(0x1000)
                .WithInternalState(y: 0x01, a: 0x2A)
                .WithMemoryChip(0x0000, AnyByte, 0x01, 0x20)
                .WithMemoryChip(0x1000, (int)OpCode.STA_ZeroPageYIndirect, 0x01)
                .WithMemoryChip(0x2000, AnyByte, AnyByte, 0x00);

            TickOnce();

            MemoryAt(0x2002).Should().Be(0x2A);
        }

        [Test]
        public void STX_ZeroPage()
        {
            HavingProcessor(0x1000)
                .WithInternalState(x: 0x2A)
                .WithMemoryChip(0x0000, AnyByte, 0x00)
                .WithMemoryChip(0x1000, (int)OpCode.STX_ZeroPage, 1);

            TickOnce();

            MemoryAt(0x0001).Should().Be(0x2A);
        }

        [Test]
        public void STX_ZeroPageY()
        {
            HavingProcessor(0x1000)
                .WithInternalState(y: 0x01, x: 0x2A)
                .WithMemoryChip(0x0000, AnyByte, AnyByte, 0x00)
                .WithMemoryChip(0x1000, (int)OpCode.STX_ZeroPageY, 0x01);

            TickOnce();

            MemoryAt(0x0002).Should().Be(0x2A);
        }

        [Test]
        public void STX_Absolute()
        {
            HavingProcessor(0x1000)
                .WithInternalState(x: 0x2A)
                .WithMemoryChip(0x1000, (int)OpCode.STX_Absolute, 0x01, 0x20)
                .WithMemoryChip(0x2000, AnyByte, 0x00);

            TickOnce();

            MemoryAt(0x2001).Should().Be(0x2A);
        }

        [Test]
        public void STY_ZeroPage()
        {
            HavingProcessor(0x1000)
                .WithInternalState(y: 0x2A)
                .WithMemoryChip(0x0000, AnyByte, 0x00)
                .WithMemoryChip(0x1000, (int)OpCode.STY_ZeroPage, 1);

            TickOnce();

            MemoryAt(0x0001).Should().Be(0x2A);
        }

        [Test]
        public void STY_ZeroPageX()
        {
            HavingProcessor(0x1000)
                .WithInternalState(x: 0x01, y: 0x2A)
                .WithMemoryChip(0x0000, AnyByte, AnyByte, 0x00)
                .WithMemoryChip(0x1000, (int)OpCode.STY_ZeroPageX, 0x01);

            TickOnce();

            MemoryAt(0x0002).Should().Be(0x2A);
        }

        [Test]
        public void STY_Absolute()
        {
            HavingProcessor(0x1000)
                .WithInternalState(y: 0x2A)
                .WithMemoryChip(0x1000, (int)OpCode.STY_Absolute, 0x01, 0x20)
                .WithMemoryChip(0x2000, AnyByte, 0x00);

            TickOnce();

            MemoryAt(0x2001).Should().Be(0x2A);
        }
    }
}
