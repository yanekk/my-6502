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
                case OpCode.LDAi:
                    _registerA = GetNextByte();
                    break;
                case OpCode.LDAzp:
                    _registerA = GetByte(GetNextByte());
                    break;
                case OpCode.LDAa:
                    _registerA = GetByte((ushort)(GetNextByte() | GetNextByte() << 8));
                    break;

                case OpCode.LDXi:
                    _registerX = GetNextByte();
                    break;
                case OpCode.LDXzp:
                    _registerX = GetByte(GetNextByte());
                    break;
                case OpCode.LDXa:
                    _registerX = GetByte((ushort)(GetNextByte() | GetNextByte() << 8));
                    break;

                case OpCode.LDYi:
                    _registerY = GetNextByte();
                    break;
                case OpCode.LDYzp:
                    _registerY = GetByte(GetNextByte());
                    break;
                case OpCode.LDYa:
                    _registerY = GetByte((ushort)(GetNextByte() | GetNextByte() << 8));
                    break;

                default:
                    throw new Exception($"Unknown opcode: {opcode}");
            }
        }

        private byte GetNextByte()
        {
            return GetByte(_programCounter.Next());
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

        internal ProcessorInternalInfo GetInternalInfo()
        {
            return new ProcessorInternalInfo
            {
                RegisterA = _registerA,
                RegisterX = _registerX,
                RegisterY = _registerY,
            };
        }

        internal void SetInternalInfo(ProcessorInternalInfo internalInfo)
        {
            _registerA = internalInfo.RegisterA;
            _registerX = internalInfo.RegisterX;
            _registerY = internalInfo.RegisterY;
        }
    }
}
