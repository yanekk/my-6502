namespace _6502.Emulator.Processor
{
    internal class RegisterManager
    {
        public byte A { get; set; }
        public byte X { get; set; }
        public byte Y { get; set; }
        public ProcessorFlags Status { get; set; }
    }
}
