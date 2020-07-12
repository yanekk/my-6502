namespace _6502.Emulator.Processor
{
    public enum OpCode
    {
        BRK  = 0x00,
        JSR = 0x10,

        ADC_Immediate = 0x69,
        ADC_ZeroPage = 0x65,
        ADC_ZeroPageX = 0x75,
        ADC_Absolute = 0x6D,
        ADC_AbsoluteX = 0x7D,
        ADC_AbsoluteY = 0x79,
        ADC_ZeroPageIndirectX = 0x61,
        ADC_ZeroPageYIndirect = 0x71,

        AND_Immediate = 0x29,
        AND_ZeroPage = 0x25,
        AND_ZeroPageX = 0x35,
        AND_Absolute = 0x2D,
        AND_AbsoluteX = 0x3D,
        AND_AbsoluteY = 0x39,
        AND_ZeroPageIndirectX = 0x21,
        AND_ZeroPageYIndirect = 0x31,

        ASL_Accumulator = 0x0A,
        ASL_ZeroPage = 0x06,
        ASL_ZeroPageX = 0x16,
        ASL_Absolute = 0x0E,
        ASL_AbsoluteX = 0x1E,

        BCC = 0x90,
        BCS = 0xB0,
        BEQ = 0xF0,
        BMI = 0x30,
        BNE = 0xD0,
        BPL = 0x10,
        BVC = 0x50,
        BVS = 0x70,

        BIT_ZeroPage = 0x24,
        BIT_Absolute = 0x2C,

        CLC = 0x18,
        CLD = 0xD8,
        CLI = 0x58,
        CLV = 0xB8,

        CMP_Immediate = 0xC9,
        CMP_ZeroPage = 0xC5,
        CMP_ZeroPageX = 0xD5,
        CMP_Absolute = 0xCD,
        CMP_AbsoluteX = 0xDD,
        CMP_AbsoluteY = 0xD9,
        CMP_ZeroPageIndirectX = 0xC1,
        CMP_ZeroPageYIndirect = 0xD1,

        CPX_Immediate = 0xE0,
        CPX_ZeroPage = 0xE4,
        CPX_Absolute = 0xEC,

        CPY_Immediate = 0xC0,
        CPY_ZeroPage = 0xC4,
        CPY_Absolute = 0xCC,

        DEC_ZeroPage = 0xC6,
        DEC_ZeroPageX = 0xD6,
        DEC_Absolute = 0xCE,
        DEC_AbsoluteX = 0xDE,

        DEX = 0xCA,
        DEY = 0x88,

        EOR_Immediate = 0x49,
        EOR_ZeroPage = 0x45,
        EOR_ZeroPageX = 0x55,
        EOR_Absolute = 0x4D,
        EOR_AbsoluteX = 0x5D,
        EOR_AbsoluteY = 0x59,
        EOR_ZeroPageIndirectX = 0x41,
        EOR_ZeroPageYIndirect = 0x51,

        INC_ZeroPage = 0xE6,
        INC_ZeroPageX = 0xF6,
        INC_Absolute = 0xEE,
        INC_AbsoluteX = 0xFE,

        INX = 0xE8,
        INY = 0xC8,

        JMP_Absolute = 0x4C,
        JMP_Indirect = 0x6C,

        LDA_Immediate = 0xA9,
        LDA_ZeroPage = 0xA5,
        LDA_ZeroPageX = 0xB5,
        LDA_Absolute = 0xAD,
        LDA_AbsoluteX = 0xBD,
        LDA_AbsoluteY = 0xB9,
        LDA_ZeroPageIndirectX = 0xA1,
        LDA_ZeroPageYIndirect = 0xB1,

        LDX_Immediate = 0xA2,
        LDX_ZeroPage = 0xA6,
        LDX_ZeroPageY = 0xB6,
        LDX_Absolute = 0xAE,
        LDX_AbsoluteY = 0xBE,

        LDY_Immediate = 0xA0,
        LDY_ZeroPage = 0xA4,
        LDY_ZeroPageX = 0xB4,
        LDY_Absolute = 0xAC,
        LDY_AbsoluteX = 0xBC,

        LSR_Accumulator = 0x4A,
        LSR_ZeroPage = 0x46,
        LSR_ZeroPageX = 0x56,
        LSR_Absolute = 0x4E,
        LSR_AbsoluteX = 0x5E,

        NOP = 0xEA,

        ORA_Immediate = 0x09,
        ORA_ZeroPage = 0x05,
        ORA_ZeroPageX = 0x15,
        ORA_Absolute = 0x0D,
        ORA_AbsoluteX = 0x1D,
        ORA_AbsoluteY = 0x19,
        ORA_ZeroPageIndirectX = 0x01,
        ORA_ZeroPageYIndirect = 0x11,

        PHA = 0x48,
        PLA = 0x68,

        PHP = 0x08,
        PLP = 0x28,

        ROL_Accumulator = 0x2A,
        ROL_ZeroPage = 0x26,
        ROL_ZeroPageX = 0x36,
        ROL_Absolute = 0x2E,
        ROL_AbsoluteX = 0x3E,

        ROR_Accumulator = 0x6A,
        ROR_ZeroPage = 0x66,
        ROR_ZeroPageX = 0x76,
        ROR_Absolute = 0x6E,
        ROR_AbsoluteX = 0x7E,

        RTI = 0x40,
        RTS = 0x60,

        SBC_Immediate = 0xE9,
        SBC_ZeroPage = 0xE5,
        SBC_ZeroPageX = 0xF5,
        SBC_Absolute = 0xED,
        SBC_AbsoluteX = 0xFD,
        SBC_AbsoluteY = 0xF9,
        SBC_ZeroPageIndirectX = 0xE1,
        SBC_ZeroPageYIndirect = 0xF1,

        SEC = 0x38,
        SED = 0xF8,
        SEI = 0x78,

        STA_ZeroPage = 0x85,
        STA_ZeroPageX = 0x95,
        STA_Absolute = 0x8D,
        STA_AbsoluteX = 0x9D,
        STA_AbsoluteY = 0x99,
        STA_ZeroPageIndirectX = 0x81,
        STA_ZeroPageYIndirect = 0x91,

        STX_ZeroPage = 0x86,
        STX_ZeroPageY = 0x96,
        STX_Absolute = 0x8E,

        STY_ZeroPage = 0x84,
        STY_ZeroPageY = 0x94,
        STY_Absolute = 0x8C,

        TAX = 0xAA,
        TAY = 0xA8,
        TSX = 0xBA,
        TXA = 0x8A,
        TXS = 0x9A,
        TYA = 0x98,
    }
}
