using _6502.Emulator.Processor.Tests.Extensions;
using _6502.Emulator.Tests.Shared;
using FluentAssertions;
using NUnit.Framework;

namespace _6502.Emulator.Processor.Tests.OpCodeTests
{
    [TestFixture]
    internal class LoadTests : BaseTests
    {
        [Test]
        public void LDA_Immediate()
        {
            HavingProcessor()
                .WithMemoryChip(0x0000, (int)OpCode.LDA_Immediate, 0x2A);

            TickOnce();

            RegisterA().Should().Be(0x2A);
        }

        [Test]
        public void LDA_ZeroPage()
        {
            HavingProcessor(0x1000)
                .WithMemoryChip(0x0000, AnyByte, 0x2A)
                .WithMemoryChip(0x1000, (int)OpCode.LDA_ZeroPage, 1);

            TickOnce();

            RegisterA().Should().Be(0x2A);
        }

        [Test]
        public void LDA_ZeroPageX()
        {
            HavingProcessor(0x1000)
                .WithInternalState(x: 0x01)
                .WithMemoryChip(0x0000, AnyByte, AnyByte, 0x6C)
                .WithMemoryChip(0x1000, (int)OpCode.LDA_ZeroPageX, 0x01);

            TickOnce();

            RegisterA().Should().Be(0x6C);
        }

        [Test]
        public void LDA_Absolute()
        {
            HavingProcessor(0x1000)
                .WithMemoryChip(0x1000, (int)OpCode.LDA_Absolute, 0x01, 0x20)
                .WithMemoryChip(0x2000, AnyByte, 0x2A);

            TickOnce();

            RegisterA().Should().Be(0x2A);
        }

        [Test]
        public void LDA_AbsoluteX()
        {
            HavingProcessor(0x1000)
                .WithInternalState(x: 0x01)
                .WithMemoryChip(0x1000, (int)OpCode.LDA_AbsoluteX, 0x01, 0x20)
                .WithMemoryChip(0x2000, AnyByte, AnyByte, 0x2A);

            TickOnce();

            RegisterA().Should().Be(0x2A);
        }

        [Test]
        public void LDA_AbsoluteY()
        {
            HavingProcessor(0x1000)
                .WithInternalState(y: 0x01)
                .WithMemoryChip(0x1000, (int)OpCode.LDA_AbsoluteY, 0x01, 0x20)
                .WithMemoryChip(0x2000, AnyByte, AnyByte, 0x2A);

            TickOnce();

            RegisterA().Should().Be(0x2A);
        }

        [Test]
        public void LDA_IndirectX()
        {
            HavingProcessor(0x1000)
                .WithInternalState(x: 0x01)
                .WithMemoryChip(0x0000, AnyByte, AnyByte, 0x01, 0x20)
                .WithMemoryChip(0x1000, (int)OpCode.LDA_ZeroPageIndirectX, 0x01)
                .WithMemoryChip(0x2000, AnyByte, 0x2A);

            TickOnce();

            RegisterA().Should().Be(0x2A);
        }

        [Test]
        public void LDA_IndirectY()
        {
            HavingProcessor(0x1000)
                .WithInternalState(y: 0x01)
                .WithMemoryChip(0x0000, AnyByte, 0x01, 0x20)
                .WithMemoryChip(0x1000, (int)OpCode.LDA_ZeroPageYIndirect, 0x01)
                .WithMemoryChip(0x2000, AnyByte, AnyByte, 0x2A);

            TickOnce();

            RegisterA().Should().Be(0x2A);
        }

        [Test]
        public void LDX_Immediate()
        {
            HavingProcessor()
                .WithMemoryChip(0x0000, (int)OpCode.LDX_Immediate, 0x2A);

            TickOnce();

            RegisterX().Should().Be(0x2A);
        }

        [Test]
        public void LDX_ZeroPage()
        {
            HavingProcessor(0x1000)
                .WithMemoryChip(0x0000, AnyByte, 0x2A)
                .WithMemoryChip(0x1000, (int)OpCode.LDX_ZeroPage, 1);

            TickOnce();

            RegisterX().Should().Be(0x2A);
        }

        [Test]
        public void LDX_ZeroPageY()
        {
            HavingProcessor(0x1000)
                .WithInternalState(y: 0x01)
                .WithMemoryChip(0x0000, AnyByte, AnyByte, 0x6C)
                .WithMemoryChip(0x1000, (int)OpCode.LDX_ZeroPageY, 0x01);

            TickOnce();

            RegisterX().Should().Be(0x6C);
        }

        [Test]
        public void LDX_AbsoluteY()
        {
            HavingProcessor(0x1000)
                .WithInternalState(y: 0x01)
                .WithMemoryChip(0x1000, (int)OpCode.LDX_AbsoluteY, 0x01, 0x20)
                .WithMemoryChip(0x2000, AnyByte, AnyByte, 0x2A);

            TickOnce();

            RegisterX().Should().Be(0x2A);
        }

        [Test]
        public void LDX_Absolute()
        {
            HavingProcessor(0x1000)
                .WithMemoryChip(0x1000, (int)OpCode.LDX_Absolute, 0x01, 0x20)
                .WithMemoryChip(0x2000, AnyByte, 0x2A);

            TickOnce();

            RegisterX().Should().Be(0x2A);
        }

        [Test]
        public void LDY_Immediate()
        {
            HavingProcessor()
                .WithMemoryChip(0x0000, (int)OpCode.LDY_Immediate, 0x2A);

            TickOnce();

            RegisterY().Should().Be(0x2A);
        }

        [Test]
        public void LDY_ZeroPage()
        {
            HavingProcessor(0x1000)
                .WithMemoryChip(0x0000, AnyByte, 0x2A)
                .WithMemoryChip(0x1000, (int)OpCode.LDY_ZeroPage, 1);

            TickOnce();

            RegisterY().Should().Be(0x2A);
        }

        [Test]
        public void LDY_ZeroPageX()
        {
            HavingProcessor(0x1000)
                .WithInternalState(x: 0x01)
                .WithMemoryChip(0x0000, AnyByte, AnyByte, 0x6C)
                .WithMemoryChip(0x1000, (int)OpCode.LDY_ZeroPageX, 0x01);

            TickOnce();

            RegisterY().Should().Be(0x6C);
        }

        [Test]
        public void LDY_Absolute()
        {
            HavingProcessor(0x1000)
                .WithMemoryChip(0x1000, (int)OpCode.LDY_Absolute, 0x01, 0x20)
                .WithMemoryChip(0x2000, AnyByte, 0x2A);

            TickOnce();

            RegisterY().Should().Be(0x2A);
        }

        [Test]
        public void LDY_AbsoluteX()
        {
            HavingProcessor(0x1000)
                .WithInternalState(x: 0x01)
                .WithMemoryChip(0x1000, (int)OpCode.LDY_AbsoluteX, 0x01, 0x20)
                .WithMemoryChip(0x2000, AnyByte, AnyByte, 0x2A);

            TickOnce();

            RegisterY().Should().Be(0x2A);
        }
    }
}
