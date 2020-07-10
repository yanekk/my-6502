using _6502.Emulator.Abstract;
using System.IO;

namespace _6502.Emulator.Processor
{
    public class RomChip : IMemoryChip
    {
        public ushort Size => _size;

        private const ushort _size = 32768;
        private readonly byte[] _memory = new byte[_size];

        public RomChip(string filePath) : this(File.ReadAllBytes(filePath))
        {

        }

        public RomChip(byte[] data)
        {
            data.CopyTo(_memory, 0);
        }

        public byte Get(ushort address)
        {
            return _memory[address];
        }

        public void Set(ushort address, byte value)
        {
        }
    }
}
