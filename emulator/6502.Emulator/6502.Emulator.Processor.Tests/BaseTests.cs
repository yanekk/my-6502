using _6502.Emulator.Processor.Tests.TestDoubles;
using NUnit.Framework;
using System;

namespace _6502.Emulator.Processor.Tests
{
    internal abstract class BaseTests
    {
        private TestClock _testClock;
        private Processor6502 _processor;

        protected byte AnyByte => (byte)new Random().Next(0, 255);

        [SetUp]
        public void SetUp()
        {
            _testClock = new TestClock();
        }

        [TearDown]
        public void TearDown()
        {
            _processor = null;
            _testClock = null;
        }

        protected Processor6502 HavingProcessor()
        {
            _processor = new Processor6502(_testClock, new ProgramCounter());
            return _processor;
        }

        protected Processor6502 HavingProcessor(ushort initialProgramCounterValue)
        {
            _processor = new Processor6502(_testClock, new ProgramCounter(initialProgramCounterValue));
            return _processor;
        }

        protected void TickOnce()
        {
            _testClock.TickOnce();
        }

        protected byte RegisterA()
        {
            return _processor.GetInternalState().RegisterA;
        }

        protected byte RegisterX()
        {
            return _processor.GetInternalState().RegisterX;
        }

        protected byte RegisterY()
        {
            return _processor.GetInternalState().RegisterY;
        }
    }
}
