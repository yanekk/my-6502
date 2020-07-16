namespace _6502.Emulator.Processor
{
    internal class ProcessorInternalState
    {
        public byte RegisterA { get; set; }
        public byte RegisterX { get; set; }
        public byte RegisterY { get; set; }
        public byte StackPointer { get; set; }
        public bool DecimalFlag { get; set; }
        public bool CarryFlag { get; set; }
        public bool InterruptDisableFlag { get; set; }
        public bool OverflowFlag { get; set; }
        public bool ZeroFlag { get; set; }
        public bool NegativeFlag { get; set; }
        public MemoryInternalState Memory { get; set; }
        public byte[] Stack { get; set; }
        public byte FlagRegister { get; set; }
        public ushort ProgramCounter { get; set; }
    }
}
