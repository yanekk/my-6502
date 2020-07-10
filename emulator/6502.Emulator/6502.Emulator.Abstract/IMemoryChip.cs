namespace _6502.Emulator.Abstract
{
    public interface IMemoryChip
    {
        ushort Size { get; }

        byte Get(ushort address);
        void Set(ushort address, byte value);
    }
}
