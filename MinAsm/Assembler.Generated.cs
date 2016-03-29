using MinAsm.Operands;

using E = MinAsm.Operands.OperandEncoding;

using I = MinAsm.Encoding.Instruction;
using O = MinAsm.Encoding.Opcode;

namespace MinAsm
{
    public abstract partial class Assembler
    {
        /// <summary>
        /// No Operation.
        /// </summary>
        public void Nop() => Add(new I("NOP", 0x00, new O(0x90), 0));

        #region Mov

        public void Mov(Register dst, int value) => Add(new I("MOV", 0x00, new O(0xC4), 0,
            new Operand[] {
                new RegisterOperand(dst, E.ModRm),
                new Imm(value)
            }));

        public void Mov(Register dst, long value) => Add(new I("MOV", 0x48, new O(0xB8), 0,
            new Operand[] {
                new RegisterOperand(dst, E.ModRm),
                new Imm(value)
            }));

        #endregion
    }
}
