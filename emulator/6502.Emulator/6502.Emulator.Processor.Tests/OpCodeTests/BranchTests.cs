using _6502.Emulator.Processor.Tests.Extensions;
using FluentAssertions;
using NUnit.Framework;

namespace _6502.Emulator.Processor.Tests.OpCodeTests
{
    [TestFixture]
    internal class BranchTests : BaseTests
    {
        [Test]
        public void JMP_Absolute()
        {
            HavingProcessor()
                .WithMemoryChip(0x0000, (int)OpCode.JMP_Absolute, 0x05, 0x20);

            TickOnce();
            ProgramCounter().Should().Be(0x2005);
        }

        [Test]
        public void JMP_Indirect()
        {
            HavingProcessor()
                .WithMemoryChip(0x0000, (int)OpCode.JMP_Indirect, 0x00, 0x20)
                .WithMemoryChip(0x2000, 0x40, 0x50);

            TickOnce();
            ProgramCounter().Should().Be(0x5040);
        }

        [TestCase(true, 0x0001)]
        [TestCase(false, 0x0003)]
        public void BVS(bool overflowFlag, int programCounter)
        {
            HavingProcessor()
                .WithInternalState(overflowFlag: overflowFlag)
                .WithMemoryChip(0x0000, (int)OpCode.NOP, (int)OpCode.NOP, (int)OpCode.BVS, 0xFD, (int)OpCode.NOP);

            Tick(3);

            ProgramCounter().Should().Be((ushort)programCounter);
        }

        [TestCase(false, 0x0001)]
        [TestCase(true, 0x0003)]
        public void BVC(bool overflowFlag, int programCounter)
        {
            HavingProcessor()
                .WithInternalState(overflowFlag: overflowFlag)
                .WithMemoryChip(0x0000, (int)OpCode.NOP, (int)OpCode.NOP, (int)OpCode.BVC, 0xFD, (int)OpCode.NOP);

            Tick(3);

            ProgramCounter().Should().Be((ushort)programCounter);
        }

        [TestCase(false, 0x0001)]
        [TestCase(true, 0x0003)]
        public void BPL(bool negativeFlag, int programCounter)
        {
            HavingProcessor()
                .WithInternalState(negativeFlag: negativeFlag)
                .WithMemoryChip(0x0000, (int)OpCode.NOP, (int)OpCode.NOP, (int)OpCode.BPL, 0xFD, (int)OpCode.NOP);

            Tick(3);

            ProgramCounter().Should().Be((ushort)programCounter);
        }

        [TestCase(false, 0x0001)]
        [TestCase(true, 0x0003)]
        public void BNE(bool zeroFlag, int programCounter)
        {
            HavingProcessor()
                .WithInternalState(zeroFlag: zeroFlag)
                .WithMemoryChip(0x0000, (int)OpCode.NOP, (int)OpCode.NOP, (int)OpCode.BNE, 0xFD, (int)OpCode.NOP);

            Tick(3);

            ProgramCounter().Should().Be((ushort)programCounter);
        }

        [TestCase(true, 0x0001)]
        [TestCase(false, 0x0003)]
        public void BMI(bool negativeFlag, int programCounter)
        {
            HavingProcessor()
                .WithInternalState(negativeFlag: negativeFlag)
                .WithMemoryChip(0x0000, (int)OpCode.NOP, (int)OpCode.NOP, (int)OpCode.BMI, 0xFD, (int)OpCode.NOP);

            Tick(3);

            ProgramCounter().Should().Be((ushort)programCounter);
        }

        [TestCase(true, 0x0001)]
        [TestCase(false, 0x0003)]
        public void BEQ(bool zeroFlag, int programCounter)
        {
            HavingProcessor()
                .WithInternalState(zeroFlag: zeroFlag)
                .WithMemoryChip(0x0000, (int)OpCode.NOP, (int)OpCode.NOP, (int)OpCode.BEQ, 0xFD, (int)OpCode.NOP);

            Tick(3);

            ProgramCounter().Should().Be((ushort)programCounter);
        }

        [TestCase(true, 0x0001)]
        [TestCase(false, 0x0003)]
        public void BCS(bool carryFlag, int programCounter)
        {
            HavingProcessor()
                .WithInternalState(carryFlag: carryFlag)
                .WithMemoryChip(0x0000, (int)OpCode.NOP, (int)OpCode.NOP, (int)OpCode.BCS, 0xFD, (int)OpCode.NOP);

            Tick(3);

            ProgramCounter().Should().Be((ushort)programCounter);
        }

        [TestCase(false, 0x0001)]
        [TestCase(true, 0x0003)]
        public void BCC(bool carryFlag, int programCounter)
        {
            HavingProcessor()
                .WithInternalState(carryFlag: carryFlag)
                .WithMemoryChip(0x0000, (int)OpCode.NOP, (int)OpCode.NOP, (int)OpCode.BCC, 0xFD, (int)OpCode.NOP);

            Tick(3);

            ProgramCounter().Should().Be((ushort)programCounter);
        }
    }
}
