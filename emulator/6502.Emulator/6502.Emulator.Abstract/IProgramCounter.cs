namespace _6502.Emulator.Abstract
{
    public interface IProgramCounter
    {
        void Set(ushort address);
        ushort Next();
        ushort Current();
    }
}
