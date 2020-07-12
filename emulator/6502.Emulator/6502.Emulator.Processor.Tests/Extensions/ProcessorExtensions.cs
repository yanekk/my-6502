using _6502.Emulator.Processor.Tests.TestDoubles;

namespace _6502.Emulator.Processor.Tests.Extensions
{
    public static class ProcessorExtensions
    {
        public static Processor6502 WithMemoryChip(this Processor6502 processor, ushort address, params int[] data)
        {
            return processor
                .AddChip(address, new TestMemoryChip(data));
        }

        public static Processor6502 WithInternalState(this Processor6502 processor, byte a = 0, byte x = 0, byte y = 0)
        {
            processor.SetInternalInfo(new ProcessorInternalState
            {
                RegisterA = a,
                RegisterX = x,
                RegisterY = y,
            });
            return processor;
        }
    }
}
