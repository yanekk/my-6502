using _6502.Emulator.Processor;
using _6502.Emulator.Tests.Shared;
using FluentAssertions;
using NUnit.Framework;

namespace _6502.Emulator.Integration.Tests
{
    [TestFixture]
    public class Tests : BaseTests
    {
        [Test]
        public void IsRomLoaded()
        {
            HavingProcessor()
                .AddChip(0x0000, new RamChip())
                .AddChip(0x8000, new RomChip(".\\test.bin"))
                .Initialize();

            TickOnce();
            RegisterA().Should().Be(0xBA);

            TickOnce();
            MemoryAt(0x0001).Should().Be(0xBA);
        }
    }
}