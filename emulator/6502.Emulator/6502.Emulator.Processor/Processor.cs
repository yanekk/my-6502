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

        private ushort GetUShort(ushort address, byte offset)
        {
            return (ushort)(GetUShort(address) + offset);
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
            };
        }

        internal void SetInternalInfo(ProcessorInternalState internalInfo)
        {
            _registerA = internalInfo.RegisterA;
            _registerX = internalInfo.RegisterX;
            _registerY = internalInfo.RegisterY;
        }
    }
}
