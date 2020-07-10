using _6502.Emulator.Abstract;
using System.Linq;

namespace _6502.Emulator.Processor.Tests.TestDoubles
{
    public class TestMemoryChip : IMemoryChip
    {
        private readonly byte[] _data;

        public TestMemoryChip(params int[] data)
        {
            _data = new byte[data.Length];
            for (var i = 0; i < data.Length; i++)
            {
                _data[i] = (byte)data[i];
            }
        }

        public ushort Size => (ushort)_data.Length;

        public byte Get(ushort address)
        {
            return _data[address];
        }

        public void Set(ushort address, byte value)
        {
            _data[address] = value;
        }
    }
}
