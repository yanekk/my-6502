namespace _6502.Emulator.Processor
{
    public enum OpCode
    {
        BRK  = 0x00,

        LDAi = 0xA9,
        LDAzp = 0xA5,
        LDAa = 0xAD,

        LDXi = 0xA2,
        LDXzp = 0xA6,
        LDXa = 0xAE,

        LDYi = 0xA0,
        LDYzp = 0xA4,
        LDYa = 0xAC,
    }
}
