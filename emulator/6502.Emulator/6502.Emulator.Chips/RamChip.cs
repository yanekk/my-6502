using _6502.Emulator.Abstract;

namespace _6502.Emulator.Processor
{
    public class RamChip : IMemoryChip
    {
        public ushort Size => _size;

        private const ushort _size = 32768;
        private readonly byte[] _memory = new byte[_size];

        public byte Get(ushort address)
        {
            return _memory[address];
        }

        public void Set(ushort address, byte value)
        {
            _memory[address] = value;
        }
    }
}
