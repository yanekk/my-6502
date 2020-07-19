using _6502.Emulator.Processor.Tests.Extensions;
using _6502.Emulator.Tests.Shared;
using FluentAssertions;
using NUnit.Framework;

namespace _6502.Emulator.Processor.Tests.OpCodeTests
{
    [TestFixture]
    internal class RotateRightTests : BaseTests
    {
        [TestCase(true, 0x9E)]
        [TestCase(false, 0x1E)]
        public void ROR(bool carryFlag, byte expected)
        {
            HavingProcessor()
                .WithInternalState(a: 0x3C, carryFlag: carryFlag)
                .WithMemoryChip(0x0000, (int)OpCode.ROR);

            TickOnce();

            RegisterA().Should().Be(expected);
        }

        [TestCase(true, 0x9E)]
        [TestCase(false, 0x1E)]
        public void ROR_ZeroPage(bool carryFlag, byte expected)
        {
            HavingProcessor(0x1000)
                .WithInternalState(carryFlag: carryFlag)
                .WithMemoryChip(0x0000, AnyByte, 0x3C)
                .WithMemoryChip(0x1000, (int)OpCode.ROR_ZeroPage, 1);

            TickOnce();

            MemoryAt(0x0001).Should().Be(expected);
        }

        [TestCase(true, 0x9E)]
        [TestCase(false, 0x1E)]
        public void ROR_ZeroPageX(bool carryFlag, byte expected)
        {
            HavingProcessor(0x1000)
                .WithInternalState(carryFlag: carryFlag)
                .WithInternalState(x: 0x01)
                .WithMemoryChip(0x0000, AnyByte, AnyByte, 0x3C)
                .WithMemoryChip(0x1000, (int)OpCode.ROR_ZeroPageX, 0x01);

            TickOnce();

            MemoryAt(0x0002).Should().Be(expected);
        }

        [TestCase(true, 0x9E)]
        [TestCase(false, 0x1E)]
        public void ROR_Absolute(bool carryFlag, byte expected)
        {
            HavingProcessor(0x1000)
                .WithInternalState(carryFlag: carryFlag)
                .WithMemoryChip(0x1000, (int)OpCode.ROR_Absolute, 0x01, 0x20)
                .WithMemoryChip(0x2000, AnyByte, 0x3C);

            TickOnce();

            MemoryAt(0x2001).Should().Be(expected);
        }

        [TestCase(true, 0x9E)]
        [TestCase(false, 0x1E)]
        public void ROR_AbsoluteX(bool carryFlag, byte expected)
        {
            HavingProcessor(0x1000)
                .WithInternalState(x: 0x01, carryFlag: carryFlag)
                .WithMemoryChip(0x1000, (int)OpCode.ROR_AbsoluteX, 0x01, 0x20)
                .WithMemoryChip(0x2000, AnyByte, AnyByte, 0x3C);

            TickOnce();

            MemoryAt(0x2002).Should().Be(expected);
        }
    }
}
