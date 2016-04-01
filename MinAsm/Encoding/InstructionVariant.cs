using MinAsm.Operands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinAsm.Encoding
{
    /// <summary>
    /// A single variant of an opcode. Most opcodes have multiple possible variants.
    /// </summary>
    public class InstructionVariant
    {
        /// <summary>
        /// Gets or sets the opcode bytes emitted for this opcode variant.
        /// </summary>
        public Opcode Opcode { get; }

        /// <summary>
        /// Gets or sets whether this opcode variant requires REX prefix.
        /// </summary>
        public bool RexB { get; }

        /// <summary>
        /// Gets or sets the fixed value of the REG part of the ModR/M byte.
        /// </summary>
        public byte RegField { get; }

        /// <summary>
        /// Gets or sets the operand size for this instruction variant.
        /// </summary>
        public DataSize Size { get; }

        /// <summary>
        /// Gets a collection of operand descriptors.
        /// </summary>
        public IEnumerable<OperandDescriptor> OperandDescriptors { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InstructionVariant"/> class.
        /// </summary>
        /// <param name="opcode">An array of bytes representing the opcode bytes for this instruction variant.</param>
        /// <param name="rex"></param>
        /// <param name="regField"></param>
        /// <param name="size"></param>
        /// <param name="operands"></param>
        public InstructionVariant(Opcode opcode, bool rex, byte regField, DataSize size, params OperandDescriptor[] operands)
        {
            Opcode = opcode;
            RexB   = rex;
            Size   = size;
            OperandDescriptors = new List<OperandDescriptor>(operands);
        }

        public void Construct()
        {
        }
    }
}