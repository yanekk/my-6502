using _6502.Emulator.Processor.Tests.Extensions;
using FluentAssertions;
using NUnit.Framework;

namespace _6502.Emulator.Processor.Tests.OpCodeTests
{
    [TestFixture]
    internal class SubroutineTests : BaseTests
    {
        [Test]
        public void JSR()
        {
            HavingProcessor()
                .WithMemoryChip(0x0000, (int)OpCode.JSR, 0x2A);

            TickOnce();

            RegisterA().Should().Be(0x2A);
        }

        [Test]
        public void RTS()
        {
            HavingProcessor()
                .WithMemoryChip(0x0000, (int)OpCode.RTS, 0x2A);

            TickOnce();

            RegisterA().Should().Be(0x2A);
        }

        [Test]
        public void RTI()
        {
            HavingProcessor()
                .WithMemoryChip(0x0000, (int)OpCode.RTI, 0x2A);

            TickOnce();

            RegisterA().Should().Be(0x2A);
        }
    }
}
