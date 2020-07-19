using System;

namespace _6502.Emulator.Processor
{
    internal class Alu
    {
        private readonly RegisterManager _registers;

        public Alu(RegisterManager registers)
        {
            _registers = registers;
        }

        public void ADC(byte value)
        {
            byte carry = (byte)((_registers.Status & ProcessorFlags.Carry) != 0 ? 1 : 0);
            byte beforeAddition = _registers.A;
            _registers.A += (byte)(value + carry);
            if (beforeAddition > _registers.A)
                _registers.Status |= ProcessorFlags.Carry;
            SetFlags(_registers.A);
        }

        public void SBC(byte value)
        {
            byte borrow = (byte)((_registers.Status & ProcessorFlags.Carry) != 0 ? 0 : 1);
            _registers.A -= (byte)(value + borrow);
            SetFlags(_registers.A);
        }

        public void ORA(byte value)
        {
            _registers.A |= value;
            SetFlags(_registers.A);
        }

        public void AND(byte value)
        {
            _registers.A &= value;
            SetFlags(_registers.A);
        }

        public byte CPX(byte mem)
        {
            return CMP(_registers.X, mem);
        }

        public byte CPY(byte mem)
        {
            return CMP(_registers.Y, mem);
        }

        public byte CMP(byte mem)
        {
            return CMP(_registers.A, mem);
        }

        private byte CMP(byte reg, byte mem)
        {
            byte result = (byte)(reg - mem);
            _registers.Status &= ~(ProcessorFlags.Carry);
            if (reg > mem)
            {
                _registers.Status |= ProcessorFlags.Carry;
                return SetFlags(result);
            }
            if (reg < mem)
                return SetFlags(result);

            _registers.Status |= ProcessorFlags.Carry;
            return SetFlags(result);
        }

        public void EOR(byte value)
        {
            _registers.A ^= value;
            SetFlags(_registers.A);
        }

        public void LDA(byte value)
        {
            _registers.A = value;
            SetFlags(_registers.A);
        }

        public void LDX(byte value)
        {
            _registers.X = value;
            SetFlags(_registers.X);
        }

        public void LDY(byte value)
        {
            _registers.Y = value;
            SetFlags(_registers.Y);
        }

        public byte ROL(byte value)
        {
            return SetFlags((byte)((value << 1) | (int)(_registers.Status & ProcessorFlags.Carry)));
        }

        public byte ROR(byte value)
        {
            value = (byte)(value >> 1);
            if ((_registers.Status & ProcessorFlags.Carry) != 0)
                value |= 0x80;
            return SetFlags(value);
        }

        public byte LSR(byte value)
        {
            return SetFlags((byte)(value >> 1));
        }

        public byte ASL(byte value)
        {
            return SetFlags((byte)(value << 1));
        }

        public byte INC(byte value)
        {
            return SetFlags(++value);
        }

        public byte DEC(byte value)
        {
            return SetFlags(--value);
        }

        internal void TAX()
        {
            _registers.X = _registers.A;
            SetFlags(_registers.X);
        }

        internal void TAY()
        {
            _registers.Y = _registers.A;
            SetFlags(_registers.Y);
        }

        internal void TXA()
        {
            _registers.A = _registers.X;
            SetFlags(_registers.A);
        }

        internal void TYA()
        {
            _registers.A = _registers.Y;
            SetFlags(_registers.A);
        }

        private byte SetFlags(byte result)
        {
            _registers.Status = (result == 0)
                ? _registers.Status | ProcessorFlags.Zero
                : _registers.Status & ~ProcessorFlags.Zero;

            _registers.Status = (result & 1 << 7) != 0
                ? _registers.Status | ProcessorFlags.Negative
                : _registers.Status & ~ProcessorFlags.Negative;
            return result;
        }
    }
}
