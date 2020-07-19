﻿using _6502.Emulator.Abstract;
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
        private readonly Dictionary<OpCode, Action> _opcodes;

        public Processor6502(IClock clock, IProgramCounter programCounter)
        {
            clock.OnTick = Tick;
            _programCounter = programCounter;
            _registers = new RegisterManager();
            _alu = new Alu(_registers);
            _opcodes = new Dictionary<OpCode, Action>
            {
                {OpCode.ADC_Immediate,          () => _alu.ADC(Immediate())},
                {OpCode.ADC_ZeroPage,           () => _alu.ADC(ZeroPage())},
                {OpCode.ADC_ZeroPageX,          () => _alu.ADC(ZeroPageX())},
                {OpCode.ADC_Absolute,           () => _alu.ADC(Absolute())},
                {OpCode.ADC_AbsoluteX,          () => _alu.ADC(AbsoluteX())},
                {OpCode.ADC_AbsoluteY,          () => _alu.ADC(AbsoluteY())},
                {OpCode.ADC_ZeroPageIndirectX,  () => _alu.ADC(ZeroPageIndirectX())},
                {OpCode.ADC_ZeroPageYIndirect,  () => _alu.ADC(ZeroPageYIndirect())},

                {OpCode.AND_Immediate,          () => _alu.AND(GetNextByte())},
                {OpCode.AND_ZeroPage,           () => _alu.AND(ZeroPage())},
                {OpCode.AND_ZeroPageX,          () => _alu.AND(ZeroPageX())},
                {OpCode.AND_Absolute,           () => _alu.AND(Absolute())},
                {OpCode.AND_AbsoluteX,          () => _alu.AND(AbsoluteX())},
                {OpCode.AND_AbsoluteY,          () => _alu.AND(AbsoluteY())},
                {OpCode.AND_ZeroPageIndirectX,  () => _alu.AND(ZeroPageIndirectX())},
                {OpCode.AND_ZeroPageYIndirect,  () => _alu.AND(ZeroPageYIndirect())},

                {OpCode.ASL,                    () => _registerA = _alu.ASL(_registerA)},
                {OpCode.ASL_ZeroPage,           () => SetByte(GetNextByte(), _alu.ASL)},
                {OpCode.ASL_ZeroPageX,          () => SetByte(GetNextByte(), _registerX, _alu.ASL)},
                {OpCode.ASL_Absolute,           () => SetByte(GetUShort(), _alu.ASL)},
                {OpCode.ASL_AbsoluteX,          () => SetByte(GetUShort(), _registerX, _alu.ASL)},

                {OpCode.BCC,                    () => BranchIfNotSet(ProcessorFlags.Carry) },
                {OpCode.BCS,                    () => BranchIfSet(ProcessorFlags.Carry) },
                {OpCode.BNE,                    () => BranchIfNotSet(ProcessorFlags.Zero) },
                {OpCode.BEQ,                    () => BranchIfSet(ProcessorFlags.Zero) },
                {OpCode.BPL,                    () => BranchIfNotSet(ProcessorFlags.Negative) },
                {OpCode.BMI,                    () => BranchIfSet(ProcessorFlags.Negative) },
                {OpCode.BVC,                    () => BranchIfNotSet(ProcessorFlags.Overflow) },
                {OpCode.BVS,                    () => BranchIfSet(ProcessorFlags.Overflow) },

                {OpCode.CMP_Immediate,          () => _alu.CMP(GetNextByte())},
                {OpCode.CMP_ZeroPage,           () => _alu.CMP(ZeroPage())},
                {OpCode.CMP_ZeroPageX,          () => _alu.CMP(ZeroPageX())},
                {OpCode.CMP_Absolute,           () => _alu.CMP(Absolute())},
                {OpCode.CMP_AbsoluteX,          () => _alu.CMP(AbsoluteX())},
                {OpCode.CMP_AbsoluteY,          () => _alu.CMP(AbsoluteY())},
                {OpCode.CMP_ZeroPageIndirectX,  () => _alu.CMP(ZeroPageIndirectX())},
                {OpCode.CMP_ZeroPageYIndirect,  () => _alu.CMP(ZeroPageYIndirect())},

                {OpCode.CPX_Immediate,          () => _alu.CPX(GetNextByte())},
                {OpCode.CPX_ZeroPage,           () => _alu.CPX(ZeroPage())},
                {OpCode.CPX_Absolute,           () => _alu.CPX(Absolute())},

                {OpCode.CPY_Immediate,          () => _alu.CPY(GetNextByte())},
                {OpCode.CPY_ZeroPage,           () => _alu.CPY(ZeroPage())},
                {OpCode.CPY_Absolute,           () => _alu.CPY(Absolute())},

                {OpCode.CLC,                    () => _flagRegister &= ~ProcessorFlags.Carry},
                {OpCode.CLD,                    () => _flagRegister &= ~ProcessorFlags.Decimal},
                {OpCode.CLI,                    () => _flagRegister &= ~ProcessorFlags.InterruptDisable},
                {OpCode.CLV,                    () => _flagRegister &= ~ProcessorFlags.Overflow},

                {OpCode.DEC_ZeroPage,           () => SetByte(GetNextByte(), _alu.DEC)},
                {OpCode.DEC_ZeroPageX,          () => SetByte(GetNextByte(), _registerX, _alu.DEC)},
                {OpCode.DEC_Absolute,           () => SetByte(GetUShort(), _alu.DEC)},
                {OpCode.DEC_AbsoluteX,          () => SetByte(GetUShort(), _registerX, _alu.DEC)},

                {OpCode.DEX,                    () => _registerX = _alu.DEC(_registers.X)},
                {OpCode.DEY,                    () => _registerY = _alu.DEC(_registers.Y)},

                {OpCode.EOR_Immediate,          () => _alu.EOR(GetNextByte())},
                {OpCode.EOR_ZeroPage,           () => _alu.EOR(ZeroPage())},
                {OpCode.EOR_ZeroPageX,          () => _alu.EOR(ZeroPageX())},
                {OpCode.EOR_Absolute,           () => _alu.EOR(Absolute())},
                {OpCode.EOR_AbsoluteX,          () => _alu.EOR(AbsoluteX())},
                {OpCode.EOR_AbsoluteY,          () => _alu.EOR(AbsoluteY())},
                {OpCode.EOR_ZeroPageIndirectX,  () => _alu.EOR(ZeroPageIndirectX())},
                {OpCode.EOR_ZeroPageYIndirect,  () => _alu.EOR(ZeroPageYIndirect())},

                {OpCode.INC_ZeroPage,           () => SetByte(GetNextByte(), _alu.INC)},
                {OpCode.INC_ZeroPageX,          () => SetByte(GetNextByte(), _registerX, _alu.INC)},
                {OpCode.INC_Absolute,           () => SetByte(GetUShort(), _alu.INC)},
                {OpCode.INC_AbsoluteX,          () => SetByte(GetUShort(), _registerX, _alu.INC)},

                {OpCode.INX,                    () => _registerX = _alu.INC(_registerX)},
                {OpCode.INY,                    () => _registerY = _alu.INC(_registerY)},

                {OpCode.JMP_Absolute,           () => _programCounter.Set(GetUShort())},
                {OpCode.JMP_Indirect,           () => _programCounter.Set(GetUShort(GetUShort()))},

                {OpCode.LDA_Immediate,          () => _alu.LDA(GetNextByte())},
                {OpCode.LDA_ZeroPage,           () => _alu.LDA(ZeroPage())},
                {OpCode.LDA_ZeroPageX,          () => _alu.LDA(ZeroPageX())},
                {OpCode.LDA_Absolute,           () => _alu.LDA(Absolute())},
                {OpCode.LDA_AbsoluteX,          () => _alu.LDA(AbsoluteX())},
                {OpCode.LDA_AbsoluteY,          () => _alu.LDA(AbsoluteY())},
                {OpCode.LDA_ZeroPageIndirectX,  () => _alu.LDA(ZeroPageIndirectX())},
                {OpCode.LDA_ZeroPageYIndirect,  () => _alu.LDA(ZeroPageYIndirect())},

                {OpCode.LDX_Immediate,          () => _alu.LDX(GetNextByte())},
                {OpCode.LDX_ZeroPage,           () => _alu.LDX(ZeroPage())},
                {OpCode.LDX_ZeroPageY,          () => _alu.LDX(GetByte(GetNextByte(), _registerY))},
                {OpCode.LDX_Absolute,           () => _alu.LDX(Absolute())},
                {OpCode.LDX_AbsoluteY,          () => _alu.LDX(AbsoluteY())},
                
                {OpCode.LDY_Immediate,          () => _alu.LDY(GetNextByte())},
                {OpCode.LDY_ZeroPage,           () => _alu.LDY(ZeroPage())},
                {OpCode.LDY_ZeroPageX,          () => _alu.LDY(ZeroPageX())},
                {OpCode.LDY_Absolute,           () => _alu.LDY(Absolute())},
                {OpCode.LDY_AbsoluteX,          () => _alu.LDY(AbsoluteX())},

                {OpCode.LSR,                    () => _registerA = _alu.LSR(_registerA)},
                {OpCode.LSR_ZeroPage,           () => SetByte(GetNextByte(), _alu.LSR)},
                {OpCode.LSR_ZeroPageX,          () => SetByte(GetNextByte(), _registerX, _alu.LSR)},
                {OpCode.LSR_Absolute,           () => SetByte(GetUShort(), _alu.LSR)},
                {OpCode.LSR_AbsoluteX,          () => SetByte(GetUShort(), _registerX, _alu.LSR)},
                {OpCode.NOP,                    () => { } },
                { OpCode.ORA_Immediate,         () => _alu.ORA(GetNextByte())},
                { OpCode.ORA_ZeroPage,          () => _alu.ORA(ZeroPage())},
                { OpCode.ORA_ZeroPageX,         () => _alu.ORA(ZeroPageX())},
                { OpCode.ORA_Absolute,          () => _alu.ORA(Absolute())},
                { OpCode.ORA_AbsoluteX,         () => _alu.ORA(AbsoluteX())},
                { OpCode.ORA_AbsoluteY,         () => _alu.ORA(AbsoluteY())},
                { OpCode.ORA_ZeroPageIndirectX, () => _alu.ORA(ZeroPageIndirectX())},
                { OpCode.ORA_ZeroPageYIndirect, () => _alu.ORA(ZeroPageYIndirect())},

                { OpCode.PHA,                   () => Push(_registerA)},
                { OpCode.PLA,                   () => _registerA = Pull()},
                { OpCode.PHP,                   () => Push((byte)_flagRegister)},
                { OpCode.PLP,                   () => _flagRegister = (ProcessorFlags)Pull()},

                { OpCode.ROL,                   () => _registerA = _alu.ROL(_registerA)},
                { OpCode.ROL_ZeroPage,          () => SetByte(GetNextByte(), _alu.ROL)},
                { OpCode.ROL_ZeroPageX,         () => SetByte(GetNextByte(), _registerX, _alu.ROL)},
                { OpCode.ROL_Absolute,          () => SetByte(GetUShort(), _alu.ROL)},
                { OpCode.ROL_AbsoluteX,         () => SetByte(GetUShort(), _registerX, _alu.ROL)},

                { OpCode.ROR,                   () => _registerA = _alu.ROR(_registerA)},
                { OpCode.ROR_ZeroPage,          () => SetByte(GetNextByte(), _alu.ROR)},
                { OpCode.ROR_ZeroPageX,         () => SetByte(GetNextByte(), _registerX, _alu.ROR)},
                { OpCode.ROR_Absolute,          () => SetByte(GetUShort(), _alu.ROR)},
                { OpCode.ROR_AbsoluteX,         () => SetByte(GetUShort(), _registerX, _alu.ROR)},

                { OpCode.SBC_Immediate,         () => _alu.SBC(GetNextByte())},
                { OpCode.SBC_ZeroPage,          () => _alu.SBC(ZeroPage())},
                { OpCode.SBC_ZeroPageX,         () => _alu.SBC(ZeroPageX())},
                { OpCode.SBC_Absolute,          () => _alu.SBC(Absolute())},
                { OpCode.SBC_AbsoluteX,         () => _alu.SBC(AbsoluteX())},
                { OpCode.SBC_AbsoluteY,         () => _alu.SBC(AbsoluteY())},
                { OpCode.SBC_ZeroPageIndirectX, () => _alu.SBC(ZeroPageIndirectX())},
                { OpCode.SBC_ZeroPageYIndirect, () => _alu.SBC(ZeroPageYIndirect())},

                { OpCode.SEC,                   () => _flagRegister |= ProcessorFlags.Carry},
                { OpCode.SED,                   () => _flagRegister |= ProcessorFlags.Decimal},
                { OpCode.SEI,                   () => _flagRegister |= ProcessorFlags.InterruptDisable},

                { OpCode.STA_ZeroPage,          () => SetByte(GetNextByte(), v => _registerA)},
                { OpCode.STA_ZeroPageX,         () => SetByte(GetNextByte(), _registerX, v => _registerA)},
                { OpCode.STA_Absolute,          () => SetByte(GetUShort(), v => _registerA)},
                { OpCode.STA_AbsoluteX,         () => SetByte(GetUShort(), _registerX, v => _registerA)},
                { OpCode.STA_AbsoluteY,         () => SetByte(GetUShort(), _registerY, v => _registerA)},
                { OpCode.STA_ZeroPageIndirectX, () => SetByte(GetUShort(GetNextByte(_registerX)), v => _registerA)},
                { OpCode.STA_ZeroPageYIndirect, () => SetByte(GetUShort(GetNextByte()), _registerY, v => _registerA)},
                { OpCode.STX_ZeroPage,          () => SetByte(GetNextByte(), v => _registerX)},
                { OpCode.STX_ZeroPageY,         () => SetByte(GetNextByte(), _registerY, v => _registerX)},
                { OpCode.STX_Absolute,          () => SetByte(GetUShort(), v => _registerX)},
                { OpCode.STY_ZeroPage,          () => SetByte(GetNextByte(), v => _registerY)},
                { OpCode.STY_ZeroPageX,         () => SetByte(GetNextByte(), _registerX, v => _registerY)},
                { OpCode.STY_Absolute,          () => SetByte(GetUShort(), v => _registerY)},
                { OpCode.TAX,                   () => _alu.TAX()},
                { OpCode.TAY,                   () => _alu.TAY()},
                { OpCode.TSX,                   () => _registerX = _stackPointer},
                { OpCode.TXA,                   () => _alu.TXA()},
                { OpCode.TXS,                   () => _stackPointer = _registerX},
                { OpCode.TYA,                   () => _alu.TYA()},
            };
        }

        private void BranchIfNotSet(ProcessorFlags flag)
        {
            BranchIf((_flagRegister & flag) == 0);
        }

        private void BranchIfSet(ProcessorFlags flag)
        {
            BranchIf((_flagRegister & flag) != 0);
        }

        private void BranchIf(bool condition)
        {
            if (!condition)
                return;

            ushort address = (ushort)(_programCounter.Current() - (byte.MaxValue - GetNextByte()));
            _programCounter.Set(address);
        }

        public Processor6502 AddChip(ushort address, IMemoryChip memoryChip)
        {
            _chips[new Range(address, address + memoryChip.Size)] = memoryChip;
            return this;
        }

        private void Tick()
        {
            var opcode = (OpCode)GetNextByte();
            if(!_opcodes.ContainsKey(opcode))
                throw new Exception($"Unknown opcode: {opcode}");
            _opcodes[opcode].Invoke();
            return;
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
