using _6502.Emulator.Processor.Tests.Extensions;
using _6502.Emulator.Tests.Shared;
using FluentAssertions;
using NUnit.Framework;

namespace _6502.Emulator.Processor.Tests.OpCodeTests
{
    [TestFixture]
    internal class RotateLeftTests : BaseTests
    {
        [TestCase(true, 0x79)]
        [TestCase(false, 0x78)]
        public void ROL(bool carryFlag, byte expected)
        {
            HavingProcessor()
                .WithInternalState(a: 0x3C, carryFlag: carryFlag)
                .WithMemoryChip(0x0000, (int)OpCode.ROL);

            TickOnce();

            RegisterA().Should().Be(expected);
        }

        [TestCase(true, 0x79)]
        [TestCase(false, 0x78)]
        public void ROL_ZeroPage(bool carryFlag, byte expected)
        {
            HavingProcessor(0x1000)
                .WithInternalState(carryFlag: carryFlag)
                .WithMemoryChip(0x0000, AnyByte, 0x3C)
                .WithMemoryChip(0x1000, (int)OpCode.ROL_ZeroPage, 1);

            TickOnce();

            MemoryAt(0x0001).Should().Be(expected);
        }

        [TestCase(true, 0x79)]
        [TestCase(false, 0x78)]
        public void ROL_ZeroPageX(bool carryFlag, byte expected)
        {
            HavingProcessor(0x1000)
                .WithInternalState(carryFlag: carryFlag)
                .WithInternalState(x: 0x01)
                .WithMemoryChip(0x0000, AnyByte, AnyByte, 0x3C)
                .WithMemoryChip(0x1000, (int)OpCode.ROL_ZeroPageX, 0x01);

            TickOnce();

            MemoryAt(0x0002).Should().Be(expected);
        }

        [TestCase(true, 0x79)]
        [TestCase(false, 0x78)]
        public void ROL_Absolute(bool carryFlag, byte expected)
        {
            HavingProcessor(0x1000)
                .WithInternalState(carryFlag: carryFlag)
                .WithMemoryChip(0x1000, (int)OpCode.ROL_Absolute, 0x01, 0x20)
                .WithMemoryChip(0x2000, AnyByte, 0x3C);

            TickOnce();

            MemoryAt(0x2001).Should().Be(expected);
        }

        [TestCase(true, 0x79)]
        [TestCase(false, 0x78)]
        public void ROL_AbsoluteX(bool carryFlag, byte expected)
        {
            HavingProcessor(0x1000)
                .WithInternalState(x: 0x01, carryFlag: carryFlag)
                .WithMemoryChip(0x1000, (int)OpCode.ROL_AbsoluteX, 0x01, 0x20)
                .WithMemoryChip(0x2000, AnyByte, AnyByte, 0x3C);

            TickOnce();

            MemoryAt(0x2002).Should().Be(expected);
        }
    }
}
