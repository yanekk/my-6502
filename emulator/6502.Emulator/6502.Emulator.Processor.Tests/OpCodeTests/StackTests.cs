using _6502.Emulator.Processor.Tests.Extensions;
using FluentAssertions;
using NUnit.Framework;

namespace _6502.Emulator.Processor.Tests.OpCodeTests
{
    [TestFixture]
    internal class StackTests : BaseTests
    {
        [Test]
        public void PLP()
        {
            byte flags = (byte)(ProcessorFlags.Carry | ProcessorFlags.Decimal | ProcessorFlags.InterruptDisable | ProcessorFlags.Overflow);
            HavingProcessor()
                .WithInternalState(stack: new byte[] { flags })
                .WithMemoryChip(0x0000, (int)OpCode.PLP, 0x2A);

            TickOnce();

            FlagRegister().Should().Be(flags);
        }

        [Test]
        public void PLA()
        {
            HavingProcessor()
                .WithInternalState(stack: new byte[] { 0x2A })
                .WithMemoryChip(0x0000, (int)OpCode.PLA);

            TickOnce();

            RegisterA().Should().Be(0x2A);
        }

        [Test]
        public void PHP()
        {
            HavingProcessor()
                .WithInternalState(carryFlag: true, decimalFlag: true, interruptDisableFlag: true, overflowFlag: true)
                .WithMemoryChip(0x0000, (int)OpCode.PHP, 0x2A);

            TickOnce();

            Stack().Should().StartWith((byte)(ProcessorFlags.Carry | ProcessorFlags.Decimal | ProcessorFlags.InterruptDisable | ProcessorFlags.Overflow));       
        }

        [Test]
        public void PHA()
        {
            HavingProcessor()
                .WithInternalState(a: 0x2A)
                .WithMemoryChip(0x0000, (int)OpCode.PHA, 0x2A);

            TickOnce();

            Stack().Should().StartWith(0x2A);
        }
    }
}
