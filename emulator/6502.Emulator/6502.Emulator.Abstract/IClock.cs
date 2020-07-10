using System;

namespace _6502.Emulator.Abstract
{
    public interface IClock 
    {
        Action OnTick { set; }
    }
}
