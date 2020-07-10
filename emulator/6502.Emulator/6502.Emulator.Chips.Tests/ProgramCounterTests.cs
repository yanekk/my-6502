using _6502.Emulator.Processor;
using NUnit.Framework;

namespace _6502.Emulator.Processor.Tests
{
    [TestFixture]
    public class ProgramCounterTests
    {
        [Test]
        public void IsFirstZero()
        {
            var pc = new ProgramCounter();
            Assert.AreEqual(0, pc.Next());
        }

        [Test]
        public void IsSecondOne()
        {
            var pc = new ProgramCounter();
            pc.Next();

            Assert.AreEqual(1, pc.Next());
        }

        [Test]
        public void IsSetReturned()
        {
            var pc = new ProgramCounter();
            pc.Set(15);
            Assert.AreEqual(15, pc.Next());
        }

        [Test]
        public void IsSetIncremented()
        {
            var pc = new ProgramCounter();
            pc.Set(15);
            pc.Next();
            Assert.AreEqual(16, pc.Next());
        }
    }
}
