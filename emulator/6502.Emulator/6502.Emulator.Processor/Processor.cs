using _6502.Emulator.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _6502.Emulator.Processor
{
    public class Processor6502
    {
        private readonly IProgramCounter _programCounter;
        
        private byte _registerA = 0;
        private byte _registerX = 0;
        private byte _registerY = 0;
        private byte _stackPointer = 0xFF;
        private ProcessorFlags _flagRegister = 0;

        private Stack<byte> _stack = new Stack<byte>();

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
            byte? result = null;
            switch(opcode)
            {
                case OpCode.ADC_Immediate:
                    ADC(Immediate() + Carry());
                    break;
                case OpCode.ADC_ZeroPage:
                    ADC(ZeroPage() + Carry());
                    break;
                case OpCode.ADC_ZeroPageX:
                    ADC(ZeroPageX() + Carry());
                    break;
                case OpCode.ADC_Absolute:
                    ADC(Absolute() + Carry());
                    break;
                case OpCode.ADC_AbsoluteX:
                    ADC(AbsoluteX() + Carry());
                    break;
                case OpCode.ADC_AbsoluteY:
                    ADC(AbsoluteY() + Carry());
                    break;
                case OpCode.ADC_ZeroPageIndirectX:
                    ADC(ZeroPageIndirectX() + Carry());
                    break;
                case OpCode.ADC_ZeroPageYIndirect:
                    ADC(ZeroPageYIndirect() + Carry());
                    break;

                case OpCode.AND_Immediate:
                    _registerA &= GetNextByte();
                    break;
                case OpCode.AND_ZeroPage:
                    _registerA &= ZeroPage();
                    break;
                case OpCode.AND_ZeroPageX:
                    _registerA &= ZeroPageX();
                    break;
                case OpCode.AND_Absolute:
                    _registerA &= Absolute();
                    break;
                case OpCode.AND_AbsoluteX:
                    _registerA &= AbsoluteX();
                    break;
                case OpCode.AND_AbsoluteY:
                    _registerA &= AbsoluteY();
                    break;
                case OpCode.AND_ZeroPageIndirectX:
                    _registerA &= ZeroPageIndirectX();
                    break;
                case OpCode.AND_ZeroPageYIndirect:
                    _registerA &= ZeroPageYIndirect();
                    break;

                case OpCode.ASL:
                    _registerA = (byte)(_registerA << 1);
                    break;
                case OpCode.ASL_ZeroPage:
                    SetByte(GetNextByte(), v => (byte)(v << 1));
                    break;
                case OpCode.ASL_ZeroPageX:
                    SetByte(GetNextByte(), _registerX, v => (byte)(v << 1));
                    break;
                case OpCode.ASL_Absolute:
                    SetByte(GetUShort(), v => (byte)(v << 1));
                    break;
                case OpCode.ASL_AbsoluteX:
                    SetByte(GetUShort(), _registerX, v => (byte)(v << 1));
                    break;

                case OpCode.BCC:
                    if ((_flagRegister & ProcessorFlags.Carry) == 0)
                        Branch();
                    break;
                case OpCode.BCS:
                    if ((_flagRegister & ProcessorFlags.Carry) != 0)
                        Branch();
                    break;
                case OpCode.BEQ:
                    if ((_flagRegister & ProcessorFlags.Zero) != 0)
                        Branch();
                    break;
                case OpCode.BMI:
                    if ((_flagRegister & ProcessorFlags.Negative) != 0)
                        Branch();
                    break;
                case OpCode.BNE:
                    if ((_flagRegister & ProcessorFlags.Zero) == 0)
                        Branch();
                    break;
                case OpCode.BPL:
                    if ((_flagRegister & ProcessorFlags.Negative) == 0)
                        Branch();
                    break;
                case OpCode.BVC:
                    if ((_flagRegister & ProcessorFlags.Overflow) == 0)
                        Branch();
                    break;
                case OpCode.BVS:
                    if ((_flagRegister & ProcessorFlags.Overflow) != 0)
                        Branch();
                    break;

                case OpCode.CMP_Immediate:
                    result = CMP(GetNextByte());
                    break;
                case OpCode.CMP_ZeroPage:
                    result = CMP(ZeroPage());
                    break;
                case OpCode.CMP_ZeroPageX:
                    result = CMP(ZeroPageX());
                    break;
                case OpCode.CMP_Absolute:
                    result = CMP(Absolute());
                    break;
                case OpCode.CMP_AbsoluteX:
                    result = CMP(AbsoluteX());
                    break;
                case OpCode.CMP_AbsoluteY:
                    result = CMP(AbsoluteY());
                    break;
                case OpCode.CMP_ZeroPageIndirectX:
                    result = CMP(ZeroPageIndirectX());
                    break;
                case OpCode.CMP_ZeroPageYIndirect:
                    result = CMP(ZeroPageYIndirect());
                    break;

                case OpCode.CPX_Immediate:
                    result = CPX(GetNextByte());
                    break;
                case OpCode.CPX_ZeroPage:
                    result = CPX(ZeroPage());
                    break;
                case OpCode.CPX_Absolute:
                    result = CPX(Absolute());
                    break;

                case OpCode.CPY_Immediate:
                    result = CPY(GetNextByte());
                    break;
                case OpCode.CPY_ZeroPage:
                    result = CPY(ZeroPage());
                    break;
                case OpCode.CPY_Absolute:
                    result = CPY(Absolute());
                    break;

                case OpCode.CLC:
                    _flagRegister &= ~ProcessorFlags.Carry;
                    break;
                case OpCode.CLD:
                    _flagRegister &= ~ProcessorFlags.Decimal;
                    break;
                case OpCode.CLI:
                    _flagRegister &= ~ProcessorFlags.InterruptDisable;
                    break;
                case OpCode.CLV:
                    _flagRegister &= ~ProcessorFlags.Overflow;
                    break;

                case OpCode.DEC_ZeroPage:
                    SetByte(GetNextByte(), v => --v);
                    break;
                case OpCode.DEC_ZeroPageX:
                    SetByte(GetNextByte(), _registerX, v => --v);
                    break;
                case OpCode.DEC_Absolute:
                    SetByte(GetUShort(), v => --v);
                    break;
                case OpCode.DEC_AbsoluteX:
                    SetByte(GetUShort(), _registerX, v => --v);
                    break;
                case OpCode.DEX:
                    _registerX--;
                    break;
                case OpCode.DEY:
                    _registerY--;
                    break;

                case OpCode.EOR_Immediate:
                    _registerA ^= GetNextByte();
                    break;
                case OpCode.EOR_ZeroPage:
                    _registerA ^= ZeroPage();
                    break;
                case OpCode.EOR_ZeroPageX:
                    _registerA ^= ZeroPageX();
                    break;
                case OpCode.EOR_Absolute:
                    _registerA ^= Absolute();
                    break;
                case OpCode.EOR_AbsoluteX:
                    _registerA ^= AbsoluteX();
                    break;
                case OpCode.EOR_AbsoluteY:
                    _registerA ^= AbsoluteY();
                    break;
                case OpCode.EOR_ZeroPageIndirectX:
                    _registerA ^= ZeroPageIndirectX();
                    break;
                case OpCode.EOR_ZeroPageYIndirect:
                    _registerA ^= ZeroPageYIndirect();
                    break;

                case OpCode.INC_ZeroPage:
                    SetByte(GetNextByte(), v => ++v);
                    break;
                case OpCode.INC_ZeroPageX:
                    SetByte(GetNextByte(), _registerX, v => ++v);
                    break;
                case OpCode.INC_Absolute:
                    SetByte(GetUShort(), v => ++v);
                    break;
                case OpCode.INC_AbsoluteX:
                    SetByte(GetUShort(), _registerX, v => ++v);
                    break;
                case OpCode.INX:
                    _registerX++;
                    break;
                case OpCode.INY:
                    _registerY++;
                    break;

                case OpCode.JMP_Absolute:
                    _programCounter.Set(GetUShort());
                    break;
                case OpCode.JMP_Indirect:
                    _programCounter.Set(GetUShort(GetUShort()));
                    break;

                case OpCode.LDA_Immediate:
                    _registerA = GetNextByte();
                    break;
                case OpCode.LDA_ZeroPage:
                    _registerA = ZeroPage();
                    break;
                case OpCode.LDA_ZeroPageX:
                    _registerA = ZeroPageX();
                    break;
                case OpCode.LDA_Absolute:
                    _registerA = Absolute();
                    break;
                case OpCode.LDA_AbsoluteX:
                    _registerA = AbsoluteX();
                    break;
                case OpCode.LDA_AbsoluteY:
                    _registerA = AbsoluteY();
                    break;
                case OpCode.LDA_ZeroPageIndirectX:
                    _registerA = ZeroPageIndirectX();
                    break;
                case OpCode.LDA_ZeroPageYIndirect:
                    _registerA = ZeroPageYIndirect();
                    break;

                case OpCode.LDX_Immediate:
                    _registerX = GetNextByte();
                    break;
                case OpCode.LDX_ZeroPage:
                    _registerX = ZeroPage();
                    break;
                case OpCode.LDX_ZeroPageY:
                    _registerX = GetByte(GetNextByte(), _registerY);
                    break;
                case OpCode.LDX_Absolute:
                    _registerX = Absolute();
                    break;
                case OpCode.LDX_AbsoluteY:
                    _registerX = AbsoluteY();
                    break;

                case OpCode.LDY_Immediate:
                    _registerY = GetNextByte();
                    break;
                case OpCode.LDY_ZeroPage:
                    _registerY = ZeroPage();
                    break;
                case OpCode.LDY_ZeroPageX:
                    _registerY = ZeroPageX();
                    break;
                case OpCode.LDY_Absolute:
                    _registerY = Absolute();
                    break;
                case OpCode.LDY_AbsoluteX:
                    _registerY = AbsoluteX();
                    break;

                case OpCode.LSR:
                    _registerA = (byte)(_registerA >> 1);
                    break;
                case OpCode.LSR_ZeroPage:
                    SetByte(GetNextByte(), v => (byte)(v >> 1));
                    break;
                case OpCode.LSR_ZeroPageX:
                    SetByte(GetNextByte(), _registerX, v => (byte)(v >> 1));
                    break;
                case OpCode.LSR_Absolute:
                    SetByte(GetUShort(), v => (byte)(v >> 1));
                    break;
                case OpCode.LSR_AbsoluteX:
                    SetByte(GetUShort(), _registerX, v => (byte)(v >> 1));
                    break;

                case OpCode.NOP:
                    break;

                case OpCode.ORA_Immediate:
                    _registerA |= GetNextByte();
                    break;
                case OpCode.ORA_ZeroPage:
                    _registerA |= ZeroPage();
                    break;
                case OpCode.ORA_ZeroPageX:
                    _registerA |= ZeroPageX();
                    break;
                case OpCode.ORA_Absolute:
                    _registerA |= Absolute();
                    break;
                case OpCode.ORA_AbsoluteX:
                    _registerA |= AbsoluteX();
                    break;
                case OpCode.ORA_AbsoluteY:
                    _registerA |= AbsoluteY();
                    break;
                case OpCode.ORA_ZeroPageIndirectX:
                    _registerA |= ZeroPageIndirectX();
                    break;
                case OpCode.ORA_ZeroPageYIndirect:
                    _registerA |= ZeroPageYIndirect();
                    break;

                case OpCode.PHA:
                    Push(_registerA);
                    break;

                case OpCode.PLA:
                    _registerA = Pull();
                    break;

                case OpCode.PHP:
                    Push((byte)_flagRegister);
                    break;
                case OpCode.PLP:
                    _flagRegister = (ProcessorFlags)Pull();
                    break;

                case OpCode.ROL:
                    _registerA = RotateLeft(_registerA);
                    break;
                case OpCode.ROL_ZeroPage:
                    SetByte(GetNextByte(), RotateLeft);
                    break;
                case OpCode.ROL_ZeroPageX:
                    SetByte(GetNextByte(), _registerX, RotateLeft);
                    break;
                case OpCode.ROL_Absolute:
                    SetByte(GetUShort(), RotateLeft);
                    break;
                case OpCode.ROL_AbsoluteX:
                    SetByte(GetUShort(), _registerX, RotateLeft);
                    break;

                case OpCode.ROR:
                    _registerA = RotateRight(_registerA);
                    break;
                case OpCode.ROR_ZeroPage:
                    SetByte(GetNextByte(), RotateRight);
                    break;
                case OpCode.ROR_ZeroPageX:
                    SetByte(GetNextByte(), _registerX, RotateRight);
                    break;
                case OpCode.ROR_Absolute:
                    SetByte(GetUShort(), RotateRight);
                    break;
                case OpCode.ROR_AbsoluteX:
                    SetByte(GetUShort(), _registerX, RotateRight);
                    break;

                case OpCode.SBC_Immediate:
                    _registerA -= (byte)(GetNextByte() + Borrow());
                    break;
                case OpCode.SBC_ZeroPage:
                    _registerA -= (byte)(ZeroPage() + Borrow());
                    break;
                case OpCode.SBC_ZeroPageX:
                    _registerA -= (byte)(ZeroPageX() + Borrow());
                    break;
                case OpCode.SBC_Absolute:
                    _registerA -= (byte)(Absolute() + Borrow());
                    break;
                case OpCode.SBC_AbsoluteX:
                    _registerA -= (byte)(AbsoluteX() + Borrow());
                    break;
                case OpCode.SBC_AbsoluteY:
                    _registerA -= (byte)(AbsoluteY() + Borrow());
                    break;
                case OpCode.SBC_ZeroPageIndirectX:
                    _registerA -= (byte)(ZeroPageIndirectX() + Borrow());
                    break;
                case OpCode.SBC_ZeroPageYIndirect:
                    _registerA -= (byte)(ZeroPageYIndirect() + Borrow());
                    break;

                case OpCode.SEC:
                    _flagRegister |= ProcessorFlags.Carry;
                    break;
                case OpCode.SED:
                    _flagRegister |= ProcessorFlags.Decimal;
                    break;
                case OpCode.SEI:
                    _flagRegister |= ProcessorFlags.InterruptDisable;
                    break;

                case OpCode.STA_ZeroPage:
                    SetByte(GetNextByte(), v => _registerA);
                    break;
                case OpCode.STA_ZeroPageX:
                    SetByte(GetNextByte(), _registerX, v => _registerA);
                    break;
                case OpCode.STA_Absolute:
                    SetByte(GetUShort(), v => _registerA);
                    break;
                case OpCode.STA_AbsoluteX:
                    SetByte(GetUShort(), _registerX, v => _registerA);
                    break;
                case OpCode.STA_AbsoluteY:
                    SetByte(GetUShort(), _registerY, v => _registerA);
                    break;
                case OpCode.STA_ZeroPageIndirectX:
                    SetByte(GetUShort(GetNextByte(_registerX)), v => _registerA);
                    break;
                case OpCode.STA_ZeroPageYIndirect:
                    SetByte(GetUShort(GetNextByte()), _registerY, v => _registerA);
                    break;

                case OpCode.STX_ZeroPage:
                    SetByte(GetNextByte(), v => _registerX);
                    break;
                case OpCode.STX_ZeroPageY:
                    SetByte(GetNextByte(), _registerY, v => _registerX);
                    break;
                case OpCode.STX_Absolute:
                    SetByte(GetUShort(), v => _registerX);
                    break;

                case OpCode.STY_ZeroPage:
                    SetByte(GetNextByte(), v => _registerY);
                    break;
                case OpCode.STY_ZeroPageX:
                    SetByte(GetNextByte(), _registerX, v => _registerY);
                    break;
                case OpCode.STY_Absolute:
                    SetByte(GetUShort(), v => _registerY);
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

                case OpCode.BIT_ZeroPage:
                case OpCode.BIT_Absolute:

                case OpCode.BRK:
                case OpCode.JSR:

                case OpCode.RTI:
                case OpCode.RTS:

                default:
                    throw new Exception($"Unknown opcode: {opcode}");
            }

            if (result == null)
                result = _registerA;

            _flagRegister = (result == 0)
                ? _flagRegister | ProcessorFlags.Zero
                : _flagRegister & ~ProcessorFlags.Zero;

            _flagRegister = (result & 1 << 7) != 0
                ? _flagRegister | ProcessorFlags.Negative
                : _flagRegister & ~ProcessorFlags.Negative;
        }

        private void Branch()
        {
            ushort address = (ushort)(_programCounter.Current() - (byte.MaxValue - GetNextByte()));
            _programCounter.Set(address);
        }

        private byte Carry()
        {
            return (byte)((_flagRegister & ProcessorFlags.Carry) != 0 ? 1 : 0);
        }

        private byte Borrow()
        {
            return (byte)((_flagRegister & ProcessorFlags.Carry) != 0 ? 0 : 1);
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
            var (start, chip) = FindChip(address);
            if (chip != null)
                return chip.Get((ushort)(address - start));
            return 0;
        }

        private void SetByte(ushort address, byte offset, Func<byte, byte> valueClosure)
        {
            SetByte((ushort)(address + offset), valueClosure);
        }

        private void SetByte(ushort address, Func<byte, byte> valueClosure)
        {
            var (start, chip) = FindChip(address);
            if (chip == null)
                return;
            var realAddress = (ushort)(address - start);
            var value = chip.Get(realAddress);
            chip.Set(realAddress, valueClosure(value));
        }

        private (ushort, IMemoryChip) FindChip(ushort address)
        {
            foreach (var range in _chips.Keys)
            {
                ushort start = ((ushort)range.Start.Value);
                ushort end = ((ushort)range.End.Value);

                if (start <= address && address < end)
                    return (start, _chips[range]);
            }
            return (0, null);
        }

        private byte RotateLeft(byte v)
        {
            return (byte)((byte)(v << 1) | ((byte)_flagRegister & (byte)ProcessorFlags.Carry));
        }

        private byte RotateRight(byte v)
        {
            v = (byte)(v >> 1);
            if ((_flagRegister & ProcessorFlags.Carry) != 0)
                v |= 0x80;
            return v;
        }

        private void Push(byte value)
        {
            _stack.Push(value);
            _stackPointer--;
        }

        private byte Pull()
        {
            _stackPointer++;
            return _stack.Pop();            
        }

        private byte CPX(byte mem)
        {
            return CMP(_registerX, mem);
        }

        private byte CPY(byte mem)
        {
            return CMP(_registerY, mem);
        }

        private byte CMP(byte mem)
        {
            return CMP(_registerA, mem);
        }

        private byte CMP(byte reg, byte mem)
        {
            byte result = (byte)(reg - mem);
            _flagRegister &= ~(ProcessorFlags.Carry);
            if(reg > mem)
            {
                _flagRegister |= ProcessorFlags.Carry;
                return result;
            }
            if (reg < mem)
                return result;

            _flagRegister |= ProcessorFlags.Carry;

            return result;
        }

        private void ADC(int value)
        {
            byte beforeAddition = _registerA;
            _registerA += (byte)value;
            if (beforeAddition > _registerA)
                _flagRegister |= ProcessorFlags.Carry;
        }

        private byte Immediate()
        {
            return GetNextByte();
        }

        private byte ZeroPage()
        {
            return GetByte(GetNextByte());
        }

        private byte ZeroPageX()
        {
            return GetByte(GetNextByte(), _registerX);
        }

        private byte Absolute()
        {
            return GetByte(GetUShort());
        }

        private byte AbsoluteX()
        {
            return GetByte(GetUShort(), _registerX);
        }

        private byte AbsoluteY()
        {
            return GetByte(GetUShort(), _registerY);
        }

        private byte ZeroPageIndirectX()
        {
            return GetByte(GetUShort(GetNextByte(_registerX)));
        }

        private byte ZeroPageYIndirect()
        {
            return GetByte(GetUShort(GetNextByte()), _registerY);
        }

        internal ProcessorInternalState GetInternalState()
        {
            var memory = _chips.Keys
                .SelectMany(range =>
                {
                    var chip = _chips[range];
                    var result = new List<KeyValuePair<ushort, byte>>();

                    for (var address = range.Start.Value; address < range.End.Value; address++)
                    {
                        result.Add(new KeyValuePair<ushort, byte>((ushort)address, chip.Get((ushort)(address - range.Start.Value))));
                    }
                    return result;
                })
                .ToDictionary(kv => kv.Key, kv => kv.Value);

            return new ProcessorInternalState
            {
                RegisterA = _registerA,
                RegisterX = _registerX,
                RegisterY = _registerY,
                FlagRegister = (byte)_flagRegister,
                StackPointer = _stackPointer,
                CarryFlag = (_flagRegister & ProcessorFlags.Carry) != 0,
                DecimalFlag = (_flagRegister & ProcessorFlags.Decimal) != 0,
                InterruptDisableFlag = (_flagRegister & ProcessorFlags.InterruptDisable) != 0,
                OverflowFlag = (_flagRegister & ProcessorFlags.Overflow) != 0,
                ZeroFlag = (_flagRegister & ProcessorFlags.Zero) != 0,
                NegativeFlag= (_flagRegister & ProcessorFlags.Negative) != 0,
                Memory = new MemoryInternalState(memory),
                Stack = _stack.ToArray(),
                ProgramCounter = _programCounter.Current()
            };
        }

        internal void SetInternalState(ProcessorInternalState internalState)
        {
            _registerA = internalState.RegisterA;
            _registerX = internalState.RegisterX;
            _registerY = internalState.RegisterY;
            _stackPointer = internalState.StackPointer;
            if (internalState.CarryFlag)
                _flagRegister |= ProcessorFlags.Carry;
            if (internalState.DecimalFlag)
                _flagRegister |= ProcessorFlags.Decimal;
            if (internalState.InterruptDisableFlag)
                _flagRegister |= ProcessorFlags.InterruptDisable;
            if (internalState.OverflowFlag)
                _flagRegister |= ProcessorFlags.Overflow;
            if (internalState.ZeroFlag)
                _flagRegister |= ProcessorFlags.Zero;
            if (internalState.NegativeFlag)
                _flagRegister |= ProcessorFlags.Negative;
            _stack = new Stack<byte>(internalState.Stack);
        }
    }
}
