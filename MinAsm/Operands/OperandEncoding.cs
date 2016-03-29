namespace MinAsm.Operands
{
    /// <summary>
    /// Specifies how the <see cref="Operand"/> gets encoded.
    /// </summary>
    public enum OperandEncoding
    {
        /// <summary>
        /// The default encoding of the register. This is used when the operand is part of a 'reg' operand, but encoded in the ModR/M byte.
        /// </summary>
        /// <remarks>
        Default,

        /// <summary>
        /// Add the register value to the opcode. This is used when the operand is part of a 'reg' operand, but encoded in the last opcode byte.
        /// </summary>
        AddToOpcode,

        /// <summary>
        /// Reg/mem encoding. This is used when the operand is part of a 'reg/mem' operand, encoded in the ModR/M byte.
        /// </summary>
        ModRm,

        /// <summary>
        /// The operand is not encoded. This is used for operands which are implicitly part of the instruction.
        /// </summary>
        Ignore,
    }
}
