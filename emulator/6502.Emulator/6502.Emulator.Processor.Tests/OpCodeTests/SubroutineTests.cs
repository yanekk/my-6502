using _6502.Emulator.Processor.Tests.Extensions;
using _6502.Emulator.Tests.Shared;
using FluentAssertions;
using NUnit.Framework;

namespace _6502.Emulator.Processor.Tests.OpCodeTests
{
    [TestFixture]
    internal class SubroutineTests : BaseTests
    {
        [Test]
        public void BRK()
        {
            HavingProcessor(0x1000)
                .WithInternalState(carryFlag: true, zeroFlag: true)
                .WithMemoryChip(0x1000, (int)OpCode.BRK)
                .WithMemoryChip(0xFFFE, 0x02, 0x20);

            TickOnce();

            ProgramCounter().Should().Be(0x2002);
            Stack().Should().StartWith(new byte[] { (byte)(ProcessorFlags.Carry | ProcessorFlags.Zero), 0x01, 0x10 });
        }

        [Test]
        public void JSR()
        {
            HavingProcessor(0x1000)
                .WithMemoryChip(0x1000, (int)OpCode.JSR, 0x02, 0x20);

            TickOnce();

            Stack().Should().StartWith(new byte[] { 0x03, 0x10 });
            ProgramCounter().Should().Be(0x2002);
        }

        [Test]
        public void RTS()
        {
            HavingProcessor(0x1000)
                .WithInternalState(stack: new byte[] { 0x02, 0x20 })
                .WithMemoryChip(0x1000, (int)OpCode.RTS);

            TickOnce();

            Stack().Should().HaveCount(0);
            ProgramCounter().Should().Be(0x2002);
        }

        [Test]
        public void RTI()
        {
            HavingProcessor(0x1000)
                .WithInternalState(stack: new byte[] { (byte)(ProcessorFlags.Carry | ProcessorFlags.Zero), 0x02, 0x20 })
                .WithMemoryChip(0x1000, (int)OpCode.RTI);

            TickOnce();

            Stack().Should().HaveCount(0);
            CarryFlag().Should().BeTrue();
            ZeroFlag().Should().BeTrue();
            ProgramCounter().Should().Be(0x2002);
        }
    }
}
