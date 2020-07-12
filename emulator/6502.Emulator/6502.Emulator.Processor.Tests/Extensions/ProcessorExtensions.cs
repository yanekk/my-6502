using _6502.Emulator.Processor.Tests.TestDoubles;

namespace _6502.Emulator.Processor.Tests.Extensions
{
    public static class ProcessorExtensions
    {
        public static Processor6502 WithMemoryChip(this Processor6502 processor, params int[] data)
        {
            return processor
                .AddChip(0x0000, new TestMemoryChip(data));
        }

        public static Processor6502 WithMemoryChip(this Processor6502 processor, ushort address, params int[] data)
        {
            return processor
                .AddChip(address, new TestMemoryChip(data));
        }

        public static Processor6502 WithInternalState(this Processor6502 processor, 
            byte a = 0, byte x = 0, byte y = 0, byte stackPointer = 0,
            bool carryFlag = false, bool decimalFlag = false, bool interruptDisableFlag = false, bool overflowFlag = false)
        {
            processor.SetInternalState(new ProcessorInternalState
            {
                RegisterA = a,
                RegisterX = x,
                RegisterY = y,
                StackPointer = stackPointer,
                CarryFlag = carryFlag,
                DecimalFlag = decimalFlag,
                InterruptDisableFlag = interruptDisableFlag,
                OverflowFlag = overflowFlag
            });
            return processor;
        }
    }
}
