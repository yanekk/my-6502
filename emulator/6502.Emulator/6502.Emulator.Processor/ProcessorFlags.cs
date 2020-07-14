using System;

namespace _6502.Emulator.Processor
{
    [Flags]
    internal enum ProcessorFlags : byte
    {
        Carry = 1,
        Zero = 2,
        InterruptDisable = 4,
        Decimal = 8,
        NoEffect = 16,
        Overflow = 32,
        Negative = 64
    }
}
