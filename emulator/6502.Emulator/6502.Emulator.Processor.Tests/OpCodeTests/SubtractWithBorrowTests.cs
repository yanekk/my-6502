using _6502.Emulator.Processor.Tests.Extensions;
using _6502.Emulator.Tests.Shared;
using FluentAssertions;
using NUnit.Framework;

namespace _6502.Emulator.Processor.Tests.OpCodeTests
{
    [TestFixture]
    internal class SubtractWithBorrowTests : BaseTests
    {
        [TestCase(true, 0x16)]
        [TestCase(false, 0x15)]
        public void SBC_Immediate(bool carryFlag, byte result)
        {
            HavingProcessor()
                .WithInternalState(a: 0x20, carryFlag: carryFlag)
                .WithMemoryChip(0x0000, (int)OpCode.SBC_Immediate, 0x0A);

            TickOnce();

            RegisterA().Should().Be(result);
        }

        [Test]
        public void SBC_ZeroPage()
        {
            HavingProcessor(0x1000)
                .WithInternalState(a: 0x20)
                .WithMemoryChip(0x0000, AnyByte, 0x0A)
                .WithMemoryChip(0x1000, (int)OpCode.SBC_ZeroPage, 1);

            TickOnce();

            RegisterA().Should().Be(0x15);
        }

        [Test]
        public void SBC_ZeroPageX()
        {
            HavingProcessor(0x1000)
                .WithInternalState(x: 0x01, a: 0x20)
                .WithMemoryChip(0x0000, AnyByte, AnyByte, 0x0A)
                .WithMemoryChip(0x1000, (int)OpCode.SBC_ZeroPageX, 0x01);

            TickOnce();

            RegisterA().Should().Be(0x15);
        }

        [Test]
        public void SBC_Absolute()
        {
            HavingProcessor(0x1000)
                .WithInternalState(a: 0x20)
                .WithMemoryChip(0x1000, (int)OpCode.SBC_Absolute, 0x01, 0x20)
                .WithMemoryChip(0x2000, AnyByte, 0x0A);

            TickOnce();

            RegisterA().Should().Be(0x15);
        }

        [Test]
        public void SBC_AbsoluteX()
        {
            HavingProcessor(0x1000)
                .WithInternalState(x: 0x01, a: 0x20)
                .WithMemoryChip(0x1000, (int)OpCode.SBC_AbsoluteX, 0x01, 0x20)
                .WithMemoryChip(0x2000, AnyByte, AnyByte, 0x0A);

            TickOnce();

            RegisterA().Should().Be(0x15);
        }

        [Test]
        public void SBC_AbsoluteY()
        {
            HavingProcessor(0x1000)
                .WithInternalState(y: 0x01, a: 0x20)
                .WithMemoryChip(0x1000, (int)OpCode.SBC_AbsoluteY, 0x01, 0x20)
                .WithMemoryChip(0x2000, AnyByte, AnyByte, 0x0A);

            TickOnce();

            RegisterA().Should().Be(0x15);
        }

        [Test]
        public void SBC_IndirectX()
        {
            HavingProcessor(0x1000)
                .WithInternalState(x: 0x01, a: 0x20)
                .WithMemoryChip(0x0000, AnyByte, AnyByte, 0x01, 0x20)
                .WithMemoryChip(0x1000, (int)OpCode.SBC_ZeroPageIndirectX, 0x01)
                .WithMemoryChip(0x2000, AnyByte, 0x0A);

            TickOnce();

            RegisterA().Should().Be(0x15);
        }

        [Test]
        public void SBC_IndirectY()
        {
            HavingProcessor(0x1000)
                .WithInternalState(y: 0x01, a: 0x20)
                .WithMemoryChip(0x0000, AnyByte, 0x01, 0x20)
                .WithMemoryChip(0x1000, (int)OpCode.SBC_ZeroPageYIndirect, 0x01)
                .WithMemoryChip(0x2000, AnyByte, AnyByte, 0x0A);

            TickOnce();

            RegisterA().Should().Be(0x15);
        }
    }
}
