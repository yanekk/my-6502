using _6502.Emulator.Processor.Tests.Extensions;
using FluentAssertions;
using NUnit.Framework;

namespace _6502.Emulator.Processor.Tests.OpCodeTests
{
    [TestFixture]
    internal class CompareTests : BaseTests
    {
        [TestCase(0x2A, 0x2B, true, false, false)]
        [TestCase(0x2A, 0x2A, false, true, true)]
        [TestCase(0x2B, 0x2A, false, false, true)]
        public void CMP_Immediate(byte a, byte m, bool negativeFlag, bool zeroFlag, bool carryFlag)
        {
            HavingProcessor()
                .WithInternalState(a: a)
                .WithMemoryChip(0x0000, (int)OpCode.CMP_Immediate, m);

            TickOnce();

            NegativeFlag().Should().Be(negativeFlag);
            ZeroFlag().Should().Be(zeroFlag);
            CarryFlag().Should().Be(carryFlag);
        }

        [TestCase(0x2A, 0x2B, true, false, false)]
        [TestCase(0x2A, 0x2A, false, true, true)]
        [TestCase(0x2B, 0x2A, false, false, true)]
        public void CMP_ZeroPage(byte a, byte m, bool negativeFlag, bool zeroFlag, bool carryFlag)
        {
            HavingProcessor(0x1000)
                .WithInternalState(a: a)
                .WithMemoryChip(0x0000, AnyByte, m)
                .WithMemoryChip(0x1000, (int)OpCode.CMP_ZeroPage, 1);

            TickOnce();

            NegativeFlag().Should().Be(negativeFlag);
            ZeroFlag().Should().Be(zeroFlag);
            CarryFlag().Should().Be(carryFlag);
        }

        [TestCase(0x2A, 0x2B, true, false, false)]
        [TestCase(0x2A, 0x2A, false, true, true)]
        [TestCase(0x2B, 0x2A, false, false, true)]
        public void CMP_ZeroPageX(byte a, byte m, bool negativeFlag, bool zeroFlag, bool carryFlag)
        {
            HavingProcessor(0x1000)
                .WithInternalState(a: a, x: 0x01)
                .WithMemoryChip(0x0000, AnyByte, AnyByte, m)
                .WithMemoryChip(0x1000, (int)OpCode.CMP_ZeroPageX, 0x01);

            TickOnce();

            NegativeFlag().Should().Be(negativeFlag);
            ZeroFlag().Should().Be(zeroFlag);
            CarryFlag().Should().Be(carryFlag);
        }

        [TestCase(0x2A, 0x2B, true, false, false)]
        [TestCase(0x2A, 0x2A, false, true, true)]
        [TestCase(0x2B, 0x2A, false, false, true)]
        public void CMP_Absolute(byte a, byte m, bool negativeFlag, bool zeroFlag, bool carryFlag)
        {
            HavingProcessor(0x1000)
                .WithInternalState(a: a)
                .WithMemoryChip(0x1000, (int)OpCode.CMP_Absolute, 0x01, 0x20)
                .WithMemoryChip(0x2000, AnyByte, m);

            TickOnce();

            NegativeFlag().Should().Be(negativeFlag);
            ZeroFlag().Should().Be(zeroFlag);
            CarryFlag().Should().Be(carryFlag);
        }

        [TestCase(0x2A, 0x2B, true, false, false)]
        [TestCase(0x2A, 0x2A, false, true, true)]
        [TestCase(0x2B, 0x2A, false, false, true)]
        public void CMP_AbsoluteX(byte a, byte m, bool negativeFlag, bool zeroFlag, bool carryFlag)
        {
            HavingProcessor(0x1000)
                .WithInternalState(a: a, x: 0x01)
                .WithMemoryChip(0x1000, (int)OpCode.CMP_AbsoluteX, 0x01, 0x20)
                .WithMemoryChip(0x2000, AnyByte, AnyByte, m);

            TickOnce();

            NegativeFlag().Should().Be(negativeFlag);
            ZeroFlag().Should().Be(zeroFlag);
            CarryFlag().Should().Be(carryFlag);
        }

        [TestCase(0x2A, 0x2B, true, false, false)]
        [TestCase(0x2A, 0x2A, false, true, true)]
        [TestCase(0x2B, 0x2A, false, false, true)]
        public void CMP_AbsoluteY(byte a, byte m, bool negativeFlag, bool zeroFlag, bool carryFlag)
        {
            HavingProcessor(0x1000)
                .WithInternalState(a: a, y: 0x01)
                .WithMemoryChip(0x1000, (int)OpCode.CMP_AbsoluteY, 0x01, 0x20)
                .WithMemoryChip(0x2000, AnyByte, AnyByte, m);

            TickOnce();

            NegativeFlag().Should().Be(negativeFlag);
            ZeroFlag().Should().Be(zeroFlag);
            CarryFlag().Should().Be(carryFlag);
        }

        [TestCase(0x2A, 0x2B, true, false, false)]
        [TestCase(0x2A, 0x2A, false, true, true)]
        [TestCase(0x2B, 0x2A, false, false, true)]
        public void CMP_IndirectX(byte a, byte m, bool negativeFlag, bool zeroFlag, bool carryFlag)
        {
            HavingProcessor(0x1000)
                .WithInternalState(a: a, x: 0x01)
                .WithMemoryChip(0x0000, AnyByte, AnyByte, 0x01, 0x20)
                .WithMemoryChip(0x1000, (int)OpCode.CMP_ZeroPageIndirectX, 0x01)
                .WithMemoryChip(0x2000, AnyByte, m);

            TickOnce();

            NegativeFlag().Should().Be(negativeFlag);
            ZeroFlag().Should().Be(zeroFlag);
            CarryFlag().Should().Be(carryFlag);
        }

        [TestCase(0x2A, 0x2B, true, false, false)]
        [TestCase(0x2A, 0x2A, false, true, true)]
        [TestCase(0x2B, 0x2A, false, false, true)]
        public void CMP_IndirectY(byte a, byte m, bool negativeFlag, bool zeroFlag, bool carryFlag)
        {
            HavingProcessor(0x1000)
                .WithInternalState(a: a, y: 0x01)
                .WithMemoryChip(0x0000, AnyByte, 0x01, 0x20)
                .WithMemoryChip(0x1000, (int)OpCode.CMP_ZeroPageYIndirect, 0x01)
                .WithMemoryChip(0x2000, AnyByte, AnyByte, m);

            TickOnce();

            NegativeFlag().Should().Be(negativeFlag);
            ZeroFlag().Should().Be(zeroFlag);
            CarryFlag().Should().Be(carryFlag);
        }

        [TestCase(0x2A, 0x2B, true, false, false)]
        [TestCase(0x2A, 0x2A, false, true, true)]
        [TestCase(0x2B, 0x2A, false, false, true)]
        public void CPX_Immediate(byte x, byte m, bool negativeFlag, bool zeroFlag, bool carryFlag)
        {
            HavingProcessor()
                .WithInternalState(x: x)
                .WithMemoryChip(0x0000, (int)OpCode.CPX_Immediate, m);

            TickOnce();

            NegativeFlag().Should().Be(negativeFlag);
            ZeroFlag().Should().Be(zeroFlag);
            CarryFlag().Should().Be(carryFlag);
        }

        [TestCase(0x2A, 0x2B, true, false, false)]
        [TestCase(0x2A, 0x2A, false, true, true)]
        [TestCase(0x2B, 0x2A, false, false, true)]
        public void CPX_ZeroPage(byte x, byte m, bool negativeFlag, bool zeroFlag, bool carryFlag)
        {
            HavingProcessor(0x1000)
                .WithInternalState(x: x)
                .WithMemoryChip(0x0000, AnyByte, m)
                .WithMemoryChip(0x1000, (int)OpCode.CPX_ZeroPage, 1);

            TickOnce();

            NegativeFlag().Should().Be(negativeFlag);
            ZeroFlag().Should().Be(zeroFlag);
            CarryFlag().Should().Be(carryFlag);
        }

        [TestCase(0x2A, 0x2B, true, false, false)]
        [TestCase(0x2A, 0x2A, false, true, true)]
        [TestCase(0x2B, 0x2A, false, false, true)]
        public void CPX_Absolute(byte x, byte m, bool negativeFlag, bool zeroFlag, bool carryFlag)
        {
            HavingProcessor(0x1000)
                .WithInternalState(x: x)
                .WithMemoryChip(0x1000, (int)OpCode.CPX_Absolute, 0x01, 0x20)
                .WithMemoryChip(0x2000, AnyByte, m);

            TickOnce();

            NegativeFlag().Should().Be(negativeFlag);
            ZeroFlag().Should().Be(zeroFlag);
            CarryFlag().Should().Be(carryFlag);
        }

        [TestCase(0x2A, 0x2B, true, false, false)]
        [TestCase(0x2A, 0x2A, false, true, true)]
        [TestCase(0x2B, 0x2A, false, false, true)]
        public void CPY_Immediate(byte y, byte m, bool negativeFlag, bool zeroFlag, bool carryFlag)
        {
            HavingProcessor()
                .WithInternalState(y: y)
                .WithMemoryChip(0x0000, (int)OpCode.CPY_Immediate, m);

            TickOnce();

            NegativeFlag().Should().Be(negativeFlag);
            ZeroFlag().Should().Be(zeroFlag);
            CarryFlag().Should().Be(carryFlag);
        }

        [TestCase(0x2A, 0x2B, true, false, false)]
        [TestCase(0x2A, 0x2A, false, true, true)]
        [TestCase(0x2B, 0x2A, false, false, true)]
        public void CPY_ZeroPage(byte y, byte m, bool negativeFlag, bool zeroFlag, bool carryFlag)
        {
            HavingProcessor(0x1000)
                .WithInternalState(y: y)
                .WithMemoryChip(0x0000, AnyByte, m)
                .WithMemoryChip(0x1000, (int)OpCode.CPY_ZeroPage, 1);

            TickOnce();

            NegativeFlag().Should().Be(negativeFlag);
            ZeroFlag().Should().Be(zeroFlag);
            CarryFlag().Should().Be(carryFlag);
        }

        [TestCase(0x2A, 0x2B, true, false, false)]
        [TestCase(0x2A, 0x2A, false, true, true)]
        [TestCase(0x2B, 0x2A, false, false, true)]
        public void CPY_Absolute(byte y, byte m, bool negativeFlag, bool zeroFlag, bool carryFlag)
        {
            HavingProcessor(0x1000)
                .WithInternalState(y: y)
                .WithMemoryChip(0x1000, (int)OpCode.CPY_Absolute, 0x01, 0x20)
                .WithMemoryChip(0x2000, AnyByte, m);

            TickOnce();

            NegativeFlag().Should().Be(negativeFlag);
            ZeroFlag().Should().Be(zeroFlag);
            CarryFlag().Should().Be(carryFlag);
        }
    }
}
