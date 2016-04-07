using System;
using System.Linq;
using MinAsm.Operands;
using MinAsm.Encoding;

namespace MinAsm
{
    class Program
    {
        static void Main(string[] args)
        {
            var asm = new Assembler64();

            // mov
            asm.Mov(Register.RCX, 0x4455667777);
            asm.Mov(Register.RAX, 0xDDDDDD);
            asm.Mov(Register.RCX, new EffectiveAddres(Register.RDX));
            asm.Mov(Register.RCX, Register.RDX);

            //cmp
            asm.Cmp(Register.RCX, 0xdddddd);

            // simple instructions
            asm.Pause();
            asm.Nop();
            asm.RetN();
            asm.RetN(0x2255);

            Console.WriteLine(string.Join("\n", asm));

            Console.WriteLine("***  SIB  ***");

            Console.WriteLine((Sib)2);
            Console.WriteLine((Sib)0x55);

            Console.WriteLine("***  ModRM  ***");

            Console.WriteLine((ModRM)2);
            Console.WriteLine((ModRM)0x55);

            Console.ReadLine();
        }
    }
}
