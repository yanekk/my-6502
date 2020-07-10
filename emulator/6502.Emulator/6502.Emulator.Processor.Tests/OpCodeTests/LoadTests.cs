using _6502.Emulator.Processor.Tests.TestDoubles;
using NUnit.Framework;

namespace _6502.Emulator.Processor.Tests
{
    [TestFixture]
    public class LoadTests
    {
        private TestClock _testClock;

        [SetUp]
        public void SetupClock()
        {
            _testClock = new TestClock();
        }

        [Test]
        public void LDA_Immediate()
        {
            // arrange
            var expectedRegisterValue = 42;
            var processor = CreateProcessor()
                .AddChip(0x0000, new TestMemoryChip((int)OpCode.LDAi, expectedRegisterValue));

            // act
            _testClock.TickOnce();

            // assert
            Assert.AreEqual(expectedRegisterValue, processor.GetInternalInfo().RegisterA);
        }

        [Test]
        public void LDA_ZeroPage()
        {
            // arrange
            var expectedRegisterValue = 42;
            var processor = CreateProcessor(0x1000)
                .AddChip(0x0000, new TestMemoryChip(25, expectedRegisterValue))
                .AddChip(0x1000, new TestMemoryChip((int)OpCode.LDAzp, 1));

            // act
            _testClock.TickOnce();

            // assert
            Assert.AreEqual(expectedRegisterValue, processor.GetInternalInfo().RegisterA);
        }

        [Test]
        public void LDA_Absolute()
        {
            // arrange
            var expectedRegisterValue = 42;
            var processor = CreateProcessor(0x1000)
                .AddChip(0x1000, new TestMemoryChip((int)OpCode.LDAa, 0x01, 0x20))
                .AddChip(0x2000, new TestMemoryChip(25, expectedRegisterValue));

            // act
            _testClock.TickOnce();

            // assert
            Assert.AreEqual(expectedRegisterValue, processor.GetInternalInfo().RegisterA);
        }

        [Test]
        public void LDX_Immediate()
        {
            // arrange
            var expectedRegisterValue = 42;
            var processor = CreateProcessor()
                .AddChip(0x0000, new TestMemoryChip((int)OpCode.LDXi, expectedRegisterValue));

            // act
            _testClock.TickOnce();

            // assert
            Assert.AreEqual(expectedRegisterValue, processor.GetInternalInfo().RegisterX);
        }

        [Test]
        public void LDX_ZeroPage()
        {
            // arrange
            var expectedRegisterValue = 42;
            var processor = CreateProcessor(0x1000)
                .AddChip(0x0000, new TestMemoryChip(25, expectedRegisterValue))
                .AddChip(0x1000, new TestMemoryChip((int)OpCode.LDXzp, 1));

            // act
            _testClock.TickOnce();

            // assert
            Assert.AreEqual(expectedRegisterValue, processor.GetInternalInfo().RegisterX);
        }

        [Test]
        public void LDX_Absolute()
        {
            // arrange
            var expectedRegisterValue = 42;
            var processor = CreateProcessor(0x1000)
                .AddChip(0x1000, new TestMemoryChip((int)OpCode.LDXa, 0x01, 0x20))
                .AddChip(0x2000, new TestMemoryChip(25, expectedRegisterValue));

            // act
            _testClock.TickOnce();

            // assert
            Assert.AreEqual(expectedRegisterValue, processor.GetInternalInfo().RegisterX);
        }

        [Test]
        public void LDY_Immediate()
        {
            // arrange
            var expectedRegisterValue = 42;
            var processor = CreateProcessor()
                .AddChip(0x0000, new TestMemoryChip((int)OpCode.LDYi, expectedRegisterValue));

            // act
            _testClock.TickOnce();

            // assert
            Assert.AreEqual(expectedRegisterValue, processor.GetInternalInfo().RegisterY);
        }

        [Test]
        public void LDY_ZeroPage()
        {
            // arrange
            var expectedRegisterValue = 42;
            var processor = CreateProcessor(0x1000)
                .AddChip(0x0000, new TestMemoryChip(25, expectedRegisterValue))
                .AddChip(0x1000, new TestMemoryChip((int)OpCode.LDYzp, 1));

            // act
            _testClock.TickOnce();

            // assert
            Assert.AreEqual(expectedRegisterValue, processor.GetInternalInfo().RegisterY);
        }

        [Test]
        public void LDY_Absolute()
        {
            // arrange
            var expectedRegisterValue = 42;
            var processor = CreateProcessor(0x1000)
                .AddChip(0x1000, new TestMemoryChip((int)OpCode.LDYa, 0x01, 0x20))
                .AddChip(0x2000, new TestMemoryChip(25, expectedRegisterValue));

            // act
            _testClock.TickOnce();

            // assert
            Assert.AreEqual(expectedRegisterValue, processor.GetInternalInfo().RegisterY);
        }

        private Processor6502 CreateProcessor()
        {
            return new Processor6502(_testClock, new ProgramCounter());
        }

        private Processor6502 CreateProcessor(ushort initialProgramCounterValue)
        {
            return new Processor6502(_testClock, new ProgramCounter(initialProgramCounterValue));
        }
    }
}
