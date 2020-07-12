using _6502.Emulator.Processor.Tests.Extensions;
using FluentAssertions;
using NUnit.Framework;

namespace _6502.Emulator.Processor.Tests.OpCodeTests
{
    [TestFixture]
    internal class AddWithCarryTests : BaseTests
    {
        [Test]
        public void ADC_Immediate()
        {
            HavingProcessor()
                .WithMemoryChip(0x0000, (int)OpCode.ADC_Immediate, 0x2A);

            TickOnce();

            RegisterA().Should().Be(0x2A);
        }

        [Test]
        public void ADC_ZeroPage()
        {
            HavingProcessor(0x1000)
                .WithMemoryChip(0x0000, AnyByte, 0x2A)
                .WithMemoryChip(0x1000, (int)OpCode.ADC_ZeroPage, 1);

            TickOnce();

            RegisterA().Should().Be(0x2A);
        }

        [Test]
        public void ADC_ZeroPageX()
        {
            HavingProcessor(0x1000)
                .WithInternalState(x: 0x01)
                .WithMemoryChip(0x0000, AnyByte, AnyByte, 0x6C)
                .WithMemoryChip(0x1000, (int)OpCode.ADC_ZeroPageX, 0x01);

            TickOnce();

            RegisterA().Should().Be(0x6C);
        }

        [Test]
        public void ADC_Absolute()
        {
            HavingProcessor(0x1000)
                .WithMemoryChip(0x1000, (int)OpCode.ADC_Absolute, 0x01, 0x20)
                .WithMemoryChip(0x2000, AnyByte, 0x2A);

            TickOnce();

            RegisterA().Should().Be(0x2A);
        }

        [Test]
        public void ADC_AbsoluteX()
        {
            HavingProcessor(0x1000)
                .WithInternalState(x: 0x01)
                .WithMemoryChip(0x1000, (int)OpCode.ADC_AbsoluteX, 0x01, 0x20)
                .WithMemoryChip(0x2000, AnyByte, AnyByte, 0x2A);

            TickOnce();

            RegisterA().Should().Be(0x2A);
        }

        [Test]
        public void ADC_AbsoluteY()
        {
            HavingProcessor(0x1000)
                .WithInternalState(y: 0x01)
                .WithMemoryChip(0x1000, (int)OpCode.ADC_AbsoluteY, 0x01, 0x20)
                .WithMemoryChip(0x2000, AnyByte, AnyByte, 0x2A);

            TickOnce();

            RegisterA().Should().Be(0x2A);
        }

        [Test]
        public void ADC_IndirectX()
        {
            HavingProcessor(0x1000)
                .WithInternalState(x: 0x01)
                .WithMemoryChip(0x0000, AnyByte, AnyByte, 0x01, 0x20)
                .WithMemoryChip(0x1000, (int)OpCode.ADC_ZeroPageIndirectX, 0x01)
                .WithMemoryChip(0x2000, AnyByte, 0x2A);

            TickOnce();

            RegisterA().Should().Be(0x2A);
        }

        [Test]
        public void ADC_IndirectY()
        {
            HavingProcessor(0x1000)
                .WithInternalState(y: 0x01)
                .WithMemoryChip(0x0000, AnyByte, 0x01, 0x20)
                .WithMemoryChip(0x1000, (int)OpCode.ADC_ZeroPageYIndirect, 0x01)
                .WithMemoryChip(0x2000, AnyByte, AnyByte, 0x2A);

            TickOnce();

            RegisterA().Should().Be(0x2A);
        }
    }
}
