using _6502.Emulator.Processor.Tests.Extensions;
using _6502.Emulator.Tests.Shared;
using FluentAssertions;
using NUnit.Framework;

namespace _6502.Emulator.Processor.Tests.OpCodeTests
{
    [TestFixture]
    internal class OrTests : BaseTests
    {
        [Test]
        public void ORA_Immediate()
        {
            HavingProcessor()
                .WithInternalState(a: 0xF0)
                .WithMemoryChip(0x0000, (int)OpCode.ORA_Immediate, 0x0F);

            TickOnce();

            RegisterA().Should().Be(0xFF);
        }

        [Test]
        public void ORA_ZeroPage()
        {
            HavingProcessor(0x1000)
                .WithInternalState(a: 0xF0)
                .WithMemoryChip(0x0000, AnyByte, 0x0F)
                .WithMemoryChip(0x1000, (int)OpCode.ORA_ZeroPage, 1);

            TickOnce();

            RegisterA().Should().Be(0xFF);
        }

        [Test]
        public void ORA_ZeroPageX()
        {
            HavingProcessor(0x1000)
                .WithInternalState(x: 0x01, a:0xF0)
                .WithMemoryChip(0x0000, AnyByte, AnyByte, 0x0F)
                .WithMemoryChip(0x1000, (int)OpCode.ORA_ZeroPageX, 0x01);

            TickOnce();

            RegisterA().Should().Be(0xFF);
        }

        [Test]
        public void ORA_Absolute()
        {
            HavingProcessor(0x1000)
                .WithInternalState(a: 0xF0)
                .WithMemoryChip(0x1000, (int)OpCode.ORA_Absolute, 0x01, 0x20)
                .WithMemoryChip(0x2000, AnyByte, 0x0F);

            TickOnce();

            RegisterA().Should().Be(0xFF);
        }

        [Test]
        public void ORA_AbsoluteX()
        {
            HavingProcessor(0x1000)
                .WithInternalState(x: 0x01, a: 0xF0)
                .WithMemoryChip(0x1000, (int)OpCode.ORA_AbsoluteX, 0x01, 0x20)
                .WithMemoryChip(0x2000, AnyByte, AnyByte, 0x0F);

            TickOnce();

            RegisterA().Should().Be(0xFF);
        }

        [Test]
        public void ORA_AbsoluteY()
        {
            HavingProcessor(0x1000)
                .WithInternalState(y: 0x01, a: 0xF0)
                .WithMemoryChip(0x1000, (int)OpCode.ORA_AbsoluteY, 0x01, 0x20)
                .WithMemoryChip(0x2000, AnyByte, AnyByte, 0x0F);

            TickOnce();

            RegisterA().Should().Be(0xFF);
        }

        [Test]
        public void ORA_IndirectX()
        {
            HavingProcessor(0x1000)
                .WithInternalState(x: 0x01, a: 0xF0)
                .WithMemoryChip(0x0000, AnyByte, AnyByte, 0x01, 0x20)
                .WithMemoryChip(0x1000, (int)OpCode.ORA_ZeroPageIndirectX, 0x01)
                .WithMemoryChip(0x2000, AnyByte, 0x0F);

            TickOnce();

            RegisterA().Should().Be(0xFF);
        }

        [Test]
        public void ORA_IndirectY()
        {
            HavingProcessor(0x1000)
                .WithInternalState(y: 0x01, a: 0xF0)
                .WithMemoryChip(0x0000, AnyByte, 0x01, 0x20)
                .WithMemoryChip(0x1000, (int)OpCode.ORA_ZeroPageYIndirect, 0x01)
                .WithMemoryChip(0x2000, AnyByte, AnyByte, 0x0F);

            TickOnce();

            RegisterA().Should().Be(0xFF);
        }
    }
}
