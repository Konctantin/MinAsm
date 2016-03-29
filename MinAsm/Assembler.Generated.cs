using MinAsm.Operands;

using E = MinAsm.Operands.OperandEncoding;
using P = MinAsm.Encoding.Prefix;

using I = MinAsm.Encoding.Instruction;
using O = MinAsm.Encoding.Opcode;

namespace MinAsm
{
    public abstract partial class Assembler
    {
        /// <summary>
        /// No Operation.
        /// </summary>
        public void Nop() => Add(new I("NOP", P.None, new O(0x90), E.Default, 0));

        #region Mov

        public void Mov(Register dst, int value) => Add(new I("MOV", P.None, new O(0xC4), E.ModRm, 0,
            new Operand[] {
                new RegisterOperand(dst),
                new Imm(value)
            }));

        public void Mov(Register dst, long value) => Add(new I("MOV", P.RexW, new O(0xB8), E.ModRm, 0,
            new Operand[] {
                new RegisterOperand(dst),
                new Imm(value)
            }));

        #endregion
    }
}
