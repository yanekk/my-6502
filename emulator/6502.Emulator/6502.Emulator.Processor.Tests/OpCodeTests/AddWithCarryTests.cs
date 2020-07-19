using _6502.Emulator.Processor.Tests.Extensions;
using _6502.Emulator.Tests.Shared;
using FluentAssertions;
using NUnit.Framework;

namespace _6502.Emulator.Processor.Tests.OpCodeTests
{
    [TestFixture]
    internal class AddWithCarryTests : BaseTests
    {
        [TestCase(0x01, 0xFF, true, true, false)]
        [TestCase(0x01, 0xFE, false, false, true)]
        public void ADC_Flags(byte a, byte m, bool zeroFlag, bool carryFlag, bool negativeFlag)
        {
            HavingProcessor()
                .WithInternalState(a: a)
                .WithMemoryChip(0x0000, (int)OpCode.ADC_Immediate, m);

            TickOnce();

            CarryFlag().Should().Be(carryFlag);
            ZeroFlag().Should().Be(zeroFlag);
            NegativeFlag().Should().Be(negativeFlag);
        }

        [TestCase(false, 0x14)]
        [TestCase(true, 0x15)]
        public void ADC_Immediate(bool carryFlag, byte result)
        {
            HavingProcessor()
                .WithInternalState(a: 0x0A, carryFlag: carryFlag)
                .WithMemoryChip(0x0000, (int)OpCode.ADC_Immediate, 0x0A);

            TickOnce();

            RegisterA().Should().Be(result);
        }

        [Test]
        public void ADC_ZeroPage()
        {
            HavingProcessor(0x1000)
                .WithInternalState(a: 0x0A)
                .WithMemoryChip(0x0000, AnyByte, 0x0A)
                .WithMemoryChip(0x1000, (int)OpCode.ADC_ZeroPage, 1);

            TickOnce();

            RegisterA().Should().Be(0x14);
        }

        [Test]
        public void ADC_ZeroPageX()
        {
            HavingProcessor(0x1000)
                .WithInternalState(x: 0x01, a: 0x0A)
                .WithMemoryChip(0x0000, AnyByte, AnyByte, 0x0A)
                .WithMemoryChip(0x1000, (int)OpCode.ADC_ZeroPageX, 0x01);

            TickOnce();

            RegisterA().Should().Be(0x14);
        }

        [Test]
        public void ADC_Absolute()
        {
            HavingProcessor(0x1000)
                .WithInternalState(a: 0x0A)
                .WithMemoryChip(0x1000, (int)OpCode.ADC_Absolute, 0x01, 0x20)
                .WithMemoryChip(0x2000, AnyByte, 0x0A);

            TickOnce();

            RegisterA().Should().Be(0x14);
        }

        [Test]
        public void ADC_AbsoluteX()
        {
            HavingProcessor(0x1000)
                .WithInternalState(x: 0x01, a: 0x0A)
                .WithMemoryChip(0x1000, (int)OpCode.ADC_AbsoluteX, 0x01, 0x20)
                .WithMemoryChip(0x2000, AnyByte, AnyByte, 0x0A);

            TickOnce();

            RegisterA().Should().Be(0x14);
        }

        [Test]
        public void ADC_AbsoluteY()
        {
            HavingProcessor(0x1000)
                .WithInternalState(y: 0x01, a: 0x0A)
                .WithMemoryChip(0x1000, (int)OpCode.ADC_AbsoluteY, 0x01, 0x20)
                .WithMemoryChip(0x2000, AnyByte, AnyByte, 0x0A);

            TickOnce();

            RegisterA().Should().Be(0x14);
        }

        [Test]
        public void ADC_IndirectX()
        {
            HavingProcessor(0x1000)
                .WithInternalState(x: 0x01, a: 0x0A)
                .WithMemoryChip(0x0000, AnyByte, AnyByte, 0x01, 0x20)
                .WithMemoryChip(0x1000, (int)OpCode.ADC_ZeroPageIndirectX, 0x01)
                .WithMemoryChip(0x2000, AnyByte, 0x0A);

            TickOnce();

            RegisterA().Should().Be(0x14);
        }

        [Test]
        public void ADC_IndirectY()
        {
            HavingProcessor(0x1000)
                .WithInternalState(y: 0x01, a: 0x0A)
                .WithMemoryChip(0x0000, AnyByte, 0x01, 0x20)
                .WithMemoryChip(0x1000, (int)OpCode.ADC_ZeroPageYIndirect, 0x01)
                .WithMemoryChip(0x2000, AnyByte, AnyByte, 0x0A);

            TickOnce();

            RegisterA().Should().Be(0x14);
        }
    }
}
