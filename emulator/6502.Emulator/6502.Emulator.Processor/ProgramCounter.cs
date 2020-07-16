using _6502.Emulator.Abstract;

namespace _6502.Emulator.Processor
{
    public class ProgramCounter : IProgramCounter
    {
        ushort _current = 0;

        public ProgramCounter(ushort initialValue = 0)
        {
            _current = initialValue;
        }

        public ushort Current()
        {
            return _current;
        }

        public ushort Next()
        {
            return _current++;
        }

        public void Set(ushort address)
        {
            _current = address;
        }
    }
}
