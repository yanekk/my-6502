using _6502.Emulator.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _6502.Emulator.Processor
{
    public class Processor6502
    {
        private readonly IProgramCounter _programCounter;
        private readonly Alu _alu;

        private byte _stackPointer = 0xFF;
        private RegisterManager _registers;

        private byte _registerA { get => _registers.A; set => _registers.A = value; }
        private byte _registerX { get => _registers.X; set => _registers.X = value; }
        private byte _registerY { get => _registers.Y; set => _registers.Y = value; }
        private ProcessorFlags _flagRegister { get => _registers.Status; set => _registers.Status = value; }

        private Stack<byte> _stack = new Stack<byte>();

        private Dictionary<Range, IMemoryChip> _chips = new Dictionary<Range, IMemoryChip>();

        public Processor6502(IClock clock, IProgramCounter programCounter)
        {
            clock.OnTick = Tick;
            _programCounter = programCounter;
            _registers = new RegisterManager();
            _alu = new Alu(_registers);
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
                case OpCode.ADC_Immediate:
                    _alu.ADC(Immediate());
                    break;
                case OpCode.ADC_ZeroPage:
                    _alu.ADC(ZeroPage());
                    break;
                case OpCode.ADC_ZeroPageX:
                    _alu.ADC(ZeroPageX());
                    break;
                case OpCode.ADC_Absolute:
                    _alu.ADC(Absolute());
                    break;
                case OpCode.ADC_AbsoluteX:
                    _alu.ADC(AbsoluteX());
                    break;
                case OpCode.ADC_AbsoluteY:
                    _alu.ADC(AbsoluteY());
                    break;
                case OpCode.ADC_ZeroPageIndirectX:
                    _alu.ADC(ZeroPageIndirectX());
                    break;
                case OpCode.ADC_ZeroPageYIndirect:
                    _alu.ADC(ZeroPageYIndirect());
                    break;

                case OpCode.AND_Immediate:
                    _alu.AND(GetNextByte());
                    break;
                case OpCode.AND_ZeroPage:
                    _alu.AND(ZeroPage());
                    break;
                case OpCode.AND_ZeroPageX:
                    _alu.AND(ZeroPageX());
                    break;
                case OpCode.AND_Absolute:
                    _alu.AND(Absolute());
                    break;
                case OpCode.AND_AbsoluteX:
                    _alu.AND(AbsoluteX());
                    break;
                case OpCode.AND_AbsoluteY:
                    _alu.AND(AbsoluteY());
                    break;
                case OpCode.AND_ZeroPageIndirectX:
                    _alu.AND(ZeroPageIndirectX());
                    break;
                case OpCode.AND_ZeroPageYIndirect:
                    _alu.AND(ZeroPageYIndirect());
                    break;

                case OpCode.ASL:
                    _registerA = _alu.ASL(_registerA);
                    break;
                case OpCode.ASL_ZeroPage:
                    SetByte(GetNextByte(), _alu.ASL);
                    break;
                case OpCode.ASL_ZeroPageX:
                    SetByte(GetNextByte(), _registerX, _alu.ASL);
                    break;
                case OpCode.ASL_Absolute:
                    SetByte(GetUShort(), _alu.ASL);
                    break;
                case OpCode.ASL_AbsoluteX:
                    SetByte(GetUShort(), _registerX, _alu.ASL);
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
                    _alu.CMP(GetNextByte());
                    break;
                case OpCode.CMP_ZeroPage:
                    _alu.CMP(ZeroPage());
                    break;
                case OpCode.CMP_ZeroPageX:
                    _alu.CMP(ZeroPageX());
                    break;
                case OpCode.CMP_Absolute:
                    _alu.CMP(Absolute());
                    break;
                case OpCode.CMP_AbsoluteX:
                    _alu.CMP(AbsoluteX());
                    break;
                case OpCode.CMP_AbsoluteY:
                    _alu.CMP(AbsoluteY());
                    break;
                case OpCode.CMP_ZeroPageIndirectX:
                    _alu.CMP(ZeroPageIndirectX());
                    break;
                case OpCode.CMP_ZeroPageYIndirect:
                    _alu.CMP(ZeroPageYIndirect());
                    break;

                case OpCode.CPX_Immediate:
                    _alu.CPX(GetNextByte());
                    break;
                case OpCode.CPX_ZeroPage:
                    _alu.CPX(ZeroPage());
                    break;
                case OpCode.CPX_Absolute:
                    _alu.CPX(Absolute());
                    break;

                case OpCode.CPY_Immediate:
                    _alu.CPY(GetNextByte());
                    break;
                case OpCode.CPY_ZeroPage:
                    _alu.CPY(ZeroPage());
                    break;
                case OpCode.CPY_Absolute:
                    _alu.CPY(Absolute());
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
                    SetByte(GetNextByte(), _alu.DEC);
                    break;
                case OpCode.DEC_ZeroPageX:
                    SetByte(GetNextByte(), _registerX, _alu.DEC);
                    break;
                case OpCode.DEC_Absolute:
                    SetByte(GetUShort(), _alu.DEC);
                    break;
                case OpCode.DEC_AbsoluteX:
                    SetByte(GetUShort(), _registerX, _alu.DEC);
                    break;
                case OpCode.DEX:
                    _registerX = _alu.DEC(_registers.X);
                    break;
                case OpCode.DEY:
                    _registerY = _alu.DEC(_registers.Y);
                    break;

                case OpCode.EOR_Immediate:
                    _alu.EOR(GetNextByte());
                    break;
                case OpCode.EOR_ZeroPage:
                    _alu.EOR(ZeroPage());
                    break;
                case OpCode.EOR_ZeroPageX:
                    _alu.EOR(ZeroPageX());
                    break;
                case OpCode.EOR_Absolute:
                    _alu.EOR(Absolute());
                    break;
                case OpCode.EOR_AbsoluteX:
                    _alu.EOR(AbsoluteX());
                    break;
                case OpCode.EOR_AbsoluteY:
                    _alu.EOR(AbsoluteY());
                    break;
                case OpCode.EOR_ZeroPageIndirectX:
                    _alu.EOR(ZeroPageIndirectX());
                    break;
                case OpCode.EOR_ZeroPageYIndirect:
                    _alu.EOR(ZeroPageYIndirect());
                    break;

                case OpCode.INC_ZeroPage:
                    SetByte(GetNextByte(), _alu.INC);
                    break;
                case OpCode.INC_ZeroPageX:
                    SetByte(GetNextByte(), _registerX, _alu.INC);
                    break;
                case OpCode.INC_Absolute:
                    SetByte(GetUShort(), _alu.INC);
                    break;
                case OpCode.INC_AbsoluteX:
                    SetByte(GetUShort(), _registerX, _alu.INC);
                    break;
                case OpCode.INX:
                    _registerX = _alu.INC(_registerX);
                    break;
                case OpCode.INY:
                    _registerY = _alu.INC(_registerY);
                    break;

                case OpCode.JMP_Absolute:
                    _programCounter.Set(GetUShort());
                    break;
                case OpCode.JMP_Indirect:
                    _programCounter.Set(GetUShort(GetUShort()));
                    break;

                case OpCode.LDA_Immediate:
                    _alu.LDA(GetNextByte());
                    break;
                case OpCode.LDA_ZeroPage:
                    _alu.LDA(ZeroPage());
                    break;
                case OpCode.LDA_ZeroPageX:
                    _alu.LDA(ZeroPageX());
                    break;
                case OpCode.LDA_Absolute:
                    _alu.LDA(Absolute());
                    break;
                case OpCode.LDA_AbsoluteX:
                    _alu.LDA(AbsoluteX());
                    break;
                case OpCode.LDA_AbsoluteY:
                    _alu.LDA(AbsoluteY());
                    break;
                case OpCode.LDA_ZeroPageIndirectX:
                    _alu.LDA(ZeroPageIndirectX());
                    break;
                case OpCode.LDA_ZeroPageYIndirect:
                    _alu.LDA(ZeroPageYIndirect());
                    break;

                case OpCode.LDX_Immediate:
                    _alu.LDX(GetNextByte());
                    break;
                case OpCode.LDX_ZeroPage:
                    _alu.LDX(ZeroPage());
                    break;
                case OpCode.LDX_ZeroPageY:
                    _alu.LDX(GetByte(GetNextByte(), _registerY));
                    break;
                case OpCode.LDX_Absolute:
                    _alu.LDX(Absolute());
                    break;
                case OpCode.LDX_AbsoluteY:
                    _alu.LDX(AbsoluteY());
                    break;

                case OpCode.LDY_Immediate:
                    _alu.LDY(GetNextByte());
                    break;
                case OpCode.LDY_ZeroPage:
                    _alu.LDY(ZeroPage());
                    break;
                case OpCode.LDY_ZeroPageX:
                    _alu.LDY(ZeroPageX());
                    break;
                case OpCode.LDY_Absolute:
                    _alu.LDY(Absolute());
                    break;
                case OpCode.LDY_AbsoluteX:
                    _alu.LDY(AbsoluteX());
                    break;

                case OpCode.LSR:
                    _registerA = _alu.LSR(_registerA);
                    break;
                case OpCode.LSR_ZeroPage:
                    SetByte(GetNextByte(), _alu.LSR);
                    break;
                case OpCode.LSR_ZeroPageX:
                    SetByte(GetNextByte(), _registerX, _alu.LSR);
                    break;
                case OpCode.LSR_Absolute:
                    SetByte(GetUShort(), _alu.LSR);
                    break;
                case OpCode.LSR_AbsoluteX:
                    SetByte(GetUShort(), _registerX, _alu.LSR);
                    break;

                case OpCode.NOP:
                    break;

                case OpCode.ORA_Immediate:
                    _alu.ORA(GetNextByte());
                    break;
                case OpCode.ORA_ZeroPage:
                    _alu.ORA(ZeroPage());
                    break;
                case OpCode.ORA_ZeroPageX:
                    _alu.ORA(ZeroPageX());
                    break;
                case OpCode.ORA_Absolute:
                    _alu.ORA(Absolute());
                    break;
                case OpCode.ORA_AbsoluteX:
                    _alu.ORA(AbsoluteX());
                    break;
                case OpCode.ORA_AbsoluteY:
                    _alu.ORA(AbsoluteY());
                    break;
                case OpCode.ORA_ZeroPageIndirectX:
                    _alu.ORA(ZeroPageIndirectX());
                    break;
                case OpCode.ORA_ZeroPageYIndirect:
                    _alu.ORA(ZeroPageYIndirect());
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
                    _registerA = _alu.ROL(_registerA);
                    break;
                case OpCode.ROL_ZeroPage:
                    SetByte(GetNextByte(), _alu.ROL);
                    break;
                case OpCode.ROL_ZeroPageX:
                    SetByte(GetNextByte(), _registerX, _alu.ROL);
                    break;
                case OpCode.ROL_Absolute:
                    SetByte(GetUShort(), _alu.ROL);
                    break;
                case OpCode.ROL_AbsoluteX:
                    SetByte(GetUShort(), _registerX, _alu.ROL);
                    break;

                case OpCode.ROR:
                    _registerA = _alu.ROR(_registerA);
                    break;
                case OpCode.ROR_ZeroPage:
                    SetByte(GetNextByte(), _alu.ROR);
                    break;
                case OpCode.ROR_ZeroPageX:
                    SetByte(GetNextByte(), _registerX, _alu.ROR);
                    break;
                case OpCode.ROR_Absolute:
                    SetByte(GetUShort(), _alu.ROR);
                    break;
                case OpCode.ROR_AbsoluteX:
                    SetByte(GetUShort(), _registerX, _alu.ROR);
                    break;

                case OpCode.SBC_Immediate:
                    _alu.SBC(GetNextByte());
                    break;
                case OpCode.SBC_ZeroPage:
                    _alu.SBC(ZeroPage());
                    break;
                case OpCode.SBC_ZeroPageX:
                    _alu.SBC(ZeroPageX());
                    break;
                case OpCode.SBC_Absolute:
                    _alu.SBC(Absolute());
                    break;
                case OpCode.SBC_AbsoluteX:
                    _alu.SBC(AbsoluteX());
                    break;
                case OpCode.SBC_AbsoluteY:
                    _alu.SBC(AbsoluteY());
                    break;
                case OpCode.SBC_ZeroPageIndirectX:
                    _alu.SBC(ZeroPageIndirectX());
                    break;
                case OpCode.SBC_ZeroPageYIndirect:
                    _alu.SBC(ZeroPageYIndirect());
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
                    _alu.TAX();
                    break;
                case OpCode.TAY:
                    _alu.TAY();
                    break;
                case OpCode.TSX:
                    _registerX = _stackPointer;
                    break;
                case OpCode.TXA:
                    _alu.TXA();
                    break;
                case OpCode.TXS:
                    _stackPointer = _registerX;
                    break;
                case OpCode.TYA:
                    _alu.TYA();
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
        }

        private void Branch()
        {
            ushort address = (ushort)(_programCounter.Current() - (byte.MaxValue - GetNextByte()));
            _programCounter.Set(address);
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
