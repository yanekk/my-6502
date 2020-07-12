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
                .WithMemoryChip(0x0000, (int)OpCode.JMP_Absolute, 0x2A);

            TickOnce();

            RegisterA().Should().Be(0x2A);
        }

        [Test]
        public void JMP_Indirect()
        {
            HavingProcessor()
                .WithMemoryChip(0x0000, (int)OpCode.JMP_Indirect, 0x2A);

            TickOnce();

            RegisterA().Should().Be(0x2A);
        }

        [Test]
        public void BVS()
        {
            HavingProcessor()
                .WithMemoryChip(0x0000, (int)OpCode.BVS, 0x2A);

            TickOnce();

            RegisterA().Should().Be(0x2A);
        }

        [Test]
        public void BVC()
        {
            HavingProcessor()
                .WithMemoryChip(0x0000, (int)OpCode.BVC, 0x2A);

            TickOnce();

            RegisterA().Should().Be(0x2A);
        }

        [Test]
        public void BPL()
        {
            HavingProcessor()
                .WithMemoryChip(0x0000, (int)OpCode.BPL, 0x2A);

            TickOnce();

            RegisterA().Should().Be(0x2A);
        }

        [Test]
        public void BNE()
        {
            HavingProcessor()
                .WithMemoryChip(0x0000, (int)OpCode.BNE, 0x2A);

            TickOnce();

            RegisterA().Should().Be(0x2A);
        }

        [Test]
        public void BMI()
        {
            HavingProcessor()
                .WithMemoryChip(0x0000, (int)OpCode.BMI, 0x2A);

            TickOnce();

            RegisterA().Should().Be(0x2A);
        }

        [Test]
        public void BEQ()
        {
            HavingProcessor()
                .WithMemoryChip(0x0000, (int)OpCode.BEQ, 0x2A);

            TickOnce();

            RegisterA().Should().Be(0x2A);
        }

        [Test]
        public void BCS()
        {
            HavingProcessor()
                .WithMemoryChip(0x0000, (int)OpCode.BCS, 0x2A);

            TickOnce();

            RegisterA().Should().Be(0x2A);
        }

        [Test]
        public void BCC()
        {
            HavingProcessor()
                .WithMemoryChip(0x0000, (int)OpCode.BCC, 0x2A);

            TickOnce();

            RegisterA().Should().Be(0x2A);
        }
    }
}
