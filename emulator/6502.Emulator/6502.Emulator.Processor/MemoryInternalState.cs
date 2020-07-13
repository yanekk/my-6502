using System.Collections.Generic;

namespace _6502.Emulator.Processor
{
    internal class MemoryInternalState
    {
        private readonly Dictionary<ushort, byte> _memory;

        public MemoryInternalState(Dictionary<ushort, byte> memory)
        {
            _memory = memory;
        }

        public byte GetByte(ushort address)
        {
            return _memory[address];
        }
    }
}
