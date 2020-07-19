using _6502.Emulator.Processor.Tests.Extensions;
using _6502.Emulator.Tests.Shared;
using FluentAssertions;
using NUnit.Framework;

namespace _6502.Emulator.Processor.Tests.OpCodeTests
{
    [TestFixture]
    internal class ShiftLeftTests : BaseTests
    {
        [Test]
        public void ASL()
        {
            HavingProcessor()
                .WithInternalState(a:0x01)
                .WithMemoryChip(0x0000, (int)OpCode.ASL);

            TickOnce();

            RegisterA().Should().Be(0x02);
        }

        [Test]
        public void ASL_ZeroPage()
        {
            HavingProcessor(0x1000)
                .WithMemoryChip(0x0000, AnyByte, 0x01)
                .WithMemoryChip(0x1000, (int)OpCode.ASL_ZeroPage, 1);

            TickOnce();

            MemoryAt(0x0001).Should().Be(0x02);
        }

        [Test]
        public void ASL_ZeroPageX()
        {
            HavingProcessor(0x1000)
                .WithInternalState(x: 0x01)
                .WithMemoryChip(0x0000, AnyByte, AnyByte, 0x01)
                .WithMemoryChip(0x1000, (int)OpCode.ASL_ZeroPageX, 0x01);

            TickOnce();

            MemoryAt(0x0002).Should().Be(0x02);
        }

        [Test]
        public void ASL_Absolute()
        {
            HavingProcessor(0x1000)
                .WithMemoryChip(0x1000, (int)OpCode.ASL_Absolute, 0x01, 0x20)
                .WithMemoryChip(0x2000, AnyByte, 0x01);

            TickOnce();

            MemoryAt(0x2001).Should().Be(0x02);
        }

        [Test]
        public void ASL_AbsoluteX()
        {
            HavingProcessor(0x1000)
                .WithInternalState(x: 0x01)
                .WithMemoryChip(0x1000, (int)OpCode.ASL_AbsoluteX, 0x01, 0x20)
                .WithMemoryChip(0x2000, AnyByte, AnyByte, 0x01);

            TickOnce();

            MemoryAt(0x2002).Should().Be(0x02);
        }
    }
}
