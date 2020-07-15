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

        protected byte StackPointer()
        {
            return _processor.GetInternalState().StackPointer;
        }

        protected byte MemoryAt(ushort address)
        {
            return _processor.GetInternalState().Memory.GetByte(address);
        }

        protected byte FlagRegister()
        {
            return _processor.GetInternalState().FlagRegister;
        }

        protected bool CarryFlag()
        {
            return _processor.GetInternalState().CarryFlag;
        }

        protected bool DecimalFlag()
        {
            return _processor.GetInternalState().DecimalFlag;
        }

        protected bool InterrupDisableFlag()
        {
            return _processor.GetInternalState().InterruptDisableFlag;
        }

        protected bool OverflowFlag()
        {
            return _processor.GetInternalState().OverflowFlag;
        }

        protected byte[] Stack()
        {
            return _processor.GetInternalState().Stack;
        }
    }
}
