using MinAsm.Operands;

using E = MinAsm.Operands.OperandEncoding;

using I = MinAsm.Encoding.Instruction;
using O = MinAsm.Encoding.Opcode;

namespace MinAsm
{
    public class Assembler64 : Assembler
    {
        public Assembler64(long offset = 0L)
            : base(Architectures.X64, offset)
        {
        }

        // todo: insert instructions

        #region Mov

        public void Mov(Register dst, int value) => Add(new I("MOV", 0x48, new O(0xC7), 0,
                new RegisterOperand(dst, E.ModRm),
                new Imm(value)
            ));

        public void Mov(Register dst, long value) => Add(new I("MOV", 0x48, new O(0xB8), 0,
                new RegisterOperand(dst, E.AddToOpcode),
                new Imm(value)
            ));

        public void Mov(Register dst, Register src) => Add(new I("MOV", 0x48, new O(0x89), 0,
                new RegisterOperand(dst, E.ModRm),
                new RegisterOperand(src, E.Default)
            ));

        public void Mov(Register dst, EffectiveAddres src) => Add(new I("MOV", 0x48, new O(0x8B), 0,
                new RegisterOperand(dst, E.Default),
                src // E.ModRm
            ));

        #endregion

        #region CMP

        /// <summary>
        /// Compare imm32 sign-extended to 64-bits.
        /// </summary>
        /// <param name="dst"></param>
        /// <param name="value"></param>
        public void Cmp(Register dst, int value) => Add(new I("CMP", 0x48, new O(0x81), 7,
            new RegisterOperand(dst, E.ModRm),
            new Imm(value)
            ));

        #endregion
    }
}
