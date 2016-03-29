using MinAsm.Operands;

using E = MinAsm.Operands.OperandEncoding;

using I = MinAsm.Encoding.Instruction;
using O = MinAsm.Encoding.Opcode;

namespace MinAsm
{
    public abstract partial class Assembler
    {
        #region No operands opcodes
        /// <summary>
        /// No Operation.
        /// </summary>
        public void Nop() => Add(new I("NOP", 0x00, new O(0x90), 0));

        /// <summary>
        /// Call to Interrupt Procedure.
        /// </summary>
        public void Int3() => Add(new I("INT 3", 0x00, new O(0xCC), 0));

        /// <summary>
        /// Spin Loop Hint.
        /// </summary>
        public void Pause() => Add(new I("PAUSE", 0xF3, new O(0x90), 0));

        /// <summary>
        /// Return From Procedure.
        /// </summary>
        public void RenN() => Add(new I("RETN", 0x00, new O(0xC3), 0));

        /// <summary>
        /// Return From Procedure.
        /// </summary>
        public void RenF() => Add(new I("RETF", 0x00, new O(0xCB), 0));

        /// <summary>
        /// Set Carry Flag.
        /// </summary>
        public void Stc() => Add(new I("STC", 0x00, new O(0xF9), 0));

        /// <summary>
        /// Set Direction Flag.
        /// </summary>
        public void Std() => Add(new I("STD", 0x00, new O(0xFD), 0));

        /// <summary>
        /// Set Interrupt Flag.
        /// </summary>
        public void Sti() => Add(new I("STI", 0x00, new O(0xFB), 0));

        /// <summary>
        /// Clear Carry Flag.
        /// </summary>
        public void Clc() => Add(new I("CLC", 0x00, new O(0xF8), 0));

        /// <summary>
        /// Clear Direction Flag.
        /// </summary>
        public void Cld() => Add(new I("CLD", 0x00, new O(0xFC), 0));

        /// <summary>
        /// Clear Interrupt Flag. (ring 1)
        /// </summary>
        public void Cli() => Add(new I("CLI", 0x00, new O(0xFA), 0));

        /// <summary>
        /// Complement Carry Flag.
        /// </summary>
        public void Cmc() => Add(new I("CMC", 0x00, new O(0xF5), 0));

        #endregion

        /// <summary>
        /// Return From Procedure.
        /// </summary>
        /// <param name="value"></param>
        public void RetN(short value) => Add(new I("RETN", 0, new O(0xC2), 0, new Imm(value)));

        /// <summary>
        /// Return From Procedure.
        /// </summary>
        /// <param name="value"></param>
        public void RetF(short value) => Add(new I("RETF", 0, new O(0xCA), 0, new Imm(value)));

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
