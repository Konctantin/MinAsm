using System;

namespace MinAsm
{
    class Program
    {
        static void Main(string[] args)
        {
            var asm = new Assembler32();
            asm.Mov(Register.RAX, 0x44556677);
            asm.Mov(Register.RAX, 0x44556677L);
            asm.Nop();
            Console.ReadLine();
        }
    }
}
