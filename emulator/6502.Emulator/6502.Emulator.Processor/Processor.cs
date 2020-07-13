using _6502.Emulator.Abstract;
using System;
using System.Collections.Generic;

namespace _6502.Emulator.Processor
{
    public class Processor6502
    {
        private readonly IProgramCounter _programCounter;
        
        private byte _registerA = 0;
        private byte _registerX = 0;
        private byte _registerY = 0;
        private byte _stackPointer = 0;

        private bool _carryFlag = false;
        private bool _decimalFlag = false;
        private bool _interruptDisableFlag = false;
        private bool _overflowFlag = false;

        private Dictionary<Range, IMemoryChip> _chips = new Dictionary<Range, IMemoryChip>();

        public Processor6502(IClock clock, IProgramCounter programCounter)
        {
            clock.OnTick = Tick;
            _programCounter = programCounter;
        }

        public Processor6502 AddChip(ushort address, IMemoryChip memoryChip)
        {
            _chips[new Range(address, address + memoryChip.Size)] = memoryChip;
            return this;
        }

        private void Tick()
        {
            var opcode = (OpCode)GetNextByte();
            switch(opcode)
            {
                case OpCode.AND_Immediate:
                    _registerA &= GetNextByte();
                    break;
                case OpCode.AND_ZeroPage:
                    _registerA &= GetByte(GetNextByte());
                    break;
                case OpCode.AND_ZeroPageX:
                    _registerA &= GetByte(GetNextByte(), _registerX);
                    break;
                case OpCode.AND_Absolute:
                    _registerA &= GetByte(GetUShort());
                    break;
                case OpCode.AND_AbsoluteX:
                    _registerA &= GetByte(GetUShort(), _registerX);
                    break;
                case OpCode.AND_AbsoluteY:
                    _registerA &= GetByte(GetUShort(), _registerY);
                    break;
                case OpCode.AND_ZeroPageIndirectX:
                    _registerA &= GetByte(GetUShort(GetNextByte(_registerX)));
                    break;
                case OpCode.AND_ZeroPageYIndirect:
                    _registerA &= GetByte(GetUShort(GetNextByte()), _registerY);
                    break;

                case OpCode.CLC:
                    _carryFlag = false;
                    break;
                case OpCode.CLD:
                    _decimalFlag = false;
                    break;
                case OpCode.CLI:
                    _interruptDisableFlag = false;
                    break;
                case OpCode.CLV:
                    _overflowFlag = false;
                    break;

                case OpCode.EOR_Immediate:
                    _registerA ^= GetNextByte();
                    break;
                case OpCode.EOR_ZeroPage:
                    _registerA ^= GetByte(GetNextByte());
                    break;
                case OpCode.EOR_ZeroPageX:
                    _registerA ^= GetByte(GetNextByte(), _registerX);
                    break;
                case OpCode.EOR_Absolute:
                    _registerA ^= GetByte(GetUShort());
                    break;
                case OpCode.EOR_AbsoluteX:
                    _registerA ^= GetByte(GetUShort(), _registerX);
                    break;
                case OpCode.EOR_AbsoluteY:
                    _registerA ^= GetByte(GetUShort(), _registerY);
                    break;
                case OpCode.EOR_ZeroPageIndirectX:
                    _registerA ^= GetByte(GetUShort(GetNextByte(_registerX)));
                    break;
                case OpCode.EOR_ZeroPageYIndirect:
                    _registerA ^= GetByte(GetUShort(GetNextByte()), _registerY);
                    break;

                case OpCode.LDA_Immediate:
                    _registerA = GetNextByte();
                    break;
                case OpCode.LDA_ZeroPage:
                    _registerA = GetByte(GetNextByte());
                    break;
                case OpCode.LDA_ZeroPageX:
                    _registerA = GetByte(GetNextByte(), _registerX);
                    break;
                case OpCode.LDA_Absolute:
                    _registerA = GetByte(GetUShort());
                    break;
                case OpCode.LDA_AbsoluteX:
                    _registerA = GetByte(GetUShort(), _registerX);
                    break;
                case OpCode.LDA_AbsoluteY:
                    _registerA = GetByte(GetUShort(), _registerY);
                    break;
                case OpCode.LDA_ZeroPageIndirectX:
                    _registerA = GetByte(GetUShort(GetNextByte(_registerX)));
                    break;
                case OpCode.LDA_ZeroPageYIndirect:
                    _registerA = GetByte(GetUShort(GetNextByte()), _registerY);
                    break;

                case OpCode.LDX_Immediate:
                    _registerX = GetNextByte();
                    break;
                case OpCode.LDX_ZeroPage:
                    _registerX = GetByte(GetNextByte());
                    break;
                case OpCode.LDX_ZeroPageY:
                    _registerX = GetByte(GetNextByte(), _registerY);
                    break;
                case OpCode.LDX_Absolute:
                    _registerX = GetByte(GetUShort());
                    break;
                case OpCode.LDX_AbsoluteY:
                    _registerX = GetByte(GetUShort(), _registerY);
                    break;

                case OpCode.LDY_Immediate:
                    _registerY = GetNextByte();
                    break;
                case OpCode.LDY_ZeroPage:
                    _registerY = GetByte(GetNextByte());
                    break;
                case OpCode.LDY_ZeroPageX:
                    _registerY = GetByte(GetNextByte(), _registerX);
                    break;
                case OpCode.LDY_Absolute:
                    _registerY = GetByte(GetUShort());
                    break;
                case OpCode.LDY_AbsoluteX:
                    _registerY = GetByte(GetUShort(), _registerX);
                    break;

                case OpCode.ORA_Immediate:
                    _registerA |= GetNextByte();
                    break;
                case OpCode.ORA_ZeroPage:
                    _registerA |= GetByte(GetNextByte());
                    break;
                case OpCode.ORA_ZeroPageX:
                    _registerA |= GetByte(GetNextByte(), _registerX);
                    break;
                case OpCode.ORA_Absolute:
                    _registerA |= GetByte(GetUShort());
                    break;
                case OpCode.ORA_AbsoluteX:
                    _registerA |= GetByte(GetUShort(), _registerX);
                    break;
                case OpCode.ORA_AbsoluteY:
                    _registerA |= GetByte(GetUShort(), _registerY);
                    break;
                case OpCode.ORA_ZeroPageIndirectX:
                    _registerA |= GetByte(GetUShort(GetNextByte(_registerX)));
                    break;
                case OpCode.ORA_ZeroPageYIndirect:
                    _registerA |= GetByte(GetUShort(GetNextByte()), _registerY);
                    break;

                case OpCode.TAX:
                    _registerX = _registerA;
                    break;
                case OpCode.TAY:
                    _registerY = _registerA;
                    break;
                case OpCode.TSX:
                    _registerX = _stackPointer;
                    break;
                case OpCode.TXA:
                    _registerA = _registerX;
                    break;
                case OpCode.TXS:
                    _stackPointer = _registerX;
                    break;
                case OpCode.TYA:
                    _registerA = _registerY;
                    break;

                case OpCode.SEC:
                    _carryFlag = true;
                    break;
                case OpCode.SED:
                    _decimalFlag = true;
                    break;
                case OpCode.SEI:
                    _interruptDisableFlag = true;
                    break;

                case OpCode.BRK:
                case OpCode.JSR:

                case OpCode.ADC_Immediate:
                case OpCode.ADC_ZeroPage:
                case OpCode.ADC_ZeroPageX:
                case OpCode.ADC_Absolute:
                case OpCode.ADC_AbsoluteX:
                case OpCode.ADC_AbsoluteY:
                case OpCode.ADC_ZeroPageIndirectX:
                case OpCode.ADC_ZeroPageYIndirect:

                case OpCode.ASL:
                case OpCode.ASL_ZeroPage:
                case OpCode.ASL_ZeroPageX:
                case OpCode.ASL_Absolute:
                case OpCode.ASL_AbsoluteX:

                case OpCode.BCC:
                case OpCode.BCS:
                case OpCode.BEQ:
                case OpCode.BMI:
                case OpCode.BNE:
                case OpCode.BPL:
                case OpCode.BVC:
                case OpCode.BVS:

                case OpCode.BIT_ZeroPage:
                case OpCode.BIT_Absolute:

                case OpCode.CMP_Immediate:
                case OpCode.CMP_ZeroPage:
                case OpCode.CMP_ZeroPageX:
                case OpCode.CMP_Absolute:
                case OpCode.CMP_AbsoluteX:
                case OpCode.CMP_AbsoluteY:
                case OpCode.CMP_ZeroPageIndirectX:
                case OpCode.CMP_ZeroPageYIndirect:

                case OpCode.CPX_Immediate:
                case OpCode.CPX_ZeroPage:
                case OpCode.CPX_Absolute:

                case OpCode.CPY_Immediate:
                case OpCode.CPY_ZeroPage:
                case OpCode.CPY_Absolute:

                case OpCode.DEC_ZeroPage:
                case OpCode.DEC_ZeroPageX:
                case OpCode.DEC_Absolute:
                case OpCode.DEC_AbsoluteX:

                case OpCode.DEX:
                case OpCode.DEY:

                case OpCode.INC_ZeroPage:
                case OpCode.INC_ZeroPageX:
                case OpCode.INC_Absolute:
                case OpCode.INC_AbsoluteX:

                case OpCode.INX:
                case OpCode.INY:

                case OpCode.JMP_Absolute:
                case OpCode.JMP_Indirect:

                case OpCode.LSR:
                case OpCode.LSR_ZeroPage:
                case OpCode.LSR_ZeroPageX:
                case OpCode.LSR_Absolute:
                case OpCode.LSR_AbsoluteX:

                case OpCode.NOP:

                case OpCode.PHA:
                case OpCode.PLA:

                case OpCode.PHP:
                case OpCode.PLP:

                case OpCode.ROL:
                case OpCode.ROL_ZeroPage:
                case OpCode.ROL_ZeroPageX:
                case OpCode.ROL_Absolute:
                case OpCode.ROL_AbsoluteX:

                case OpCode.ROR:
                case OpCode.ROR_ZeroPage:
                case OpCode.ROR_ZeroPageX:
                case OpCode.ROR_Absolute:
                case OpCode.ROR_AbsoluteX:

                case OpCode.RTI:
                case OpCode.RTS:

                case OpCode.SBC_Immediate:
                case OpCode.SBC_ZeroPage:
                case OpCode.SBC_ZeroPageX:
                case OpCode.SBC_Absolute:
                case OpCode.SBC_AbsoluteX:
                case OpCode.SBC_AbsoluteY:
                case OpCode.SBC_ZeroPageIndirectX:
                case OpCode.SBC_ZeroPageYIndirect:

                case OpCode.STA_ZeroPage:
                case OpCode.STA_ZeroPageX:
                case OpCode.STA_Absolute:
                case OpCode.STA_AbsoluteX:
                case OpCode.STA_AbsoluteY:
                case OpCode.STA_ZeroPageIndirectX:
                case OpCode.STA_ZeroPageYIndirect:

                case OpCode.STX_ZeroPage:
                case OpCode.STX_ZeroPageY:
                case OpCode.STX_Absolute:

                case OpCode.STY_ZeroPage:
                case OpCode.STY_ZeroPageX:
                case OpCode.STY_Absolute:
                default:
                    throw new Exception($"Unknown opcode: {opcode}");
            }
        }

        private byte GetByte(ushort address, byte offset)
        {
            return GetByte((ushort)(address + offset));
        }

        private ushort GetUShort()
        {
            return (ushort)(GetNextByte() | GetNextByte() << 8);
        }

        private ushort GetUShort(ushort address)
        {
            return (ushort)(GetByte(address) | (GetByte(address, 1)) << 8);
        }

        private byte GetNextByte()
        {
            return GetByte(_programCounter.Next());
        }

        private byte GetNextByte(byte offset)
        {
            return (byte)(GetNextByte() + offset);
        }

        private byte GetByte(ushort address)
        {
            foreach(var range in _chips.Keys)
            {
                ushort start = ((ushort)range.Start.Value);
                ushort end = ((ushort)range.End.Value);

                if (start <= address && address < end)
                    return _chips[range].Get((ushort)(address - start));
            }
            return 0;
        }

        private void SetByte(ushort address, byte value)
        {

        }

        internal ProcessorInternalState GetInternalState()
        {
            return new ProcessorInternalState
            {
                RegisterA = _registerA,
                RegisterX = _registerX,
                RegisterY = _registerY,
                StackPointer = _stackPointer,
                CarryFlag = _carryFlag,
                DecimalFlag = _decimalFlag,
                InterruptDisableFlag = _interruptDisableFlag,
                OverflowFlag = _overflowFlag
            };
        }

        internal void SetInternalState(ProcessorInternalState internalState)
        {
            _registerA = internalState.RegisterA;
            _registerX = internalState.RegisterX;
            _registerY = internalState.RegisterY;
            _stackPointer = internalState.StackPointer;
            _carryFlag = internalState.CarryFlag;
            _decimalFlag = internalState.DecimalFlag;
            _interruptDisableFlag = internalState.InterruptDisableFlag;
            _overflowFlag = internalState.OverflowFlag;
        }
    }
}
