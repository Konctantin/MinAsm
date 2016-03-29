using System.Collections.Generic;
using MinAsm.Operands;

namespace MinAsm.Encoding
{
    public class Instruction
    {
        public string Mnemonic { get; }

        public Prefix Prefix { get; }

        public Opcode Opcode { get; }

        public OperandEncoding Encoding { get; } = OperandEncoding.Default;

        public byte FixedReg;

        public ModRM ModRM { get; private set; }

        public Sib Sib { get; private set; }

        public byte OpcodeReg { get; set; }

        public List<Operand> Oparands { get; } = new List<Operand>();

        public long? Displacement { get; internal set; }
        public DataSize DisplacementSize { get; internal set; }

        public Imm Immedicate { get; set; }

        public Instruction(string mnemonic, Prefix prefix, Opcode opcode, OperandEncoding encoding, byte fixedReg, params Operand[] operands)
        {
            Mnemonic = mnemonic;
            Prefix   = prefix;
            Opcode   = opcode;
            Encoding = encoding;
            FixedReg = fixedReg;
            Oparands = new List<Operand>(operands);
        }

        internal void SetModRM()
        {
            if (ModRM == null)
            {
                ModRM = new ModRM();
                ModRM.Reg = FixedReg;
            }
        }

        internal void SetSib()
        {
            if (Sib == null)
                Sib = new Sib();
        }

        public void Construct(Context context)
        {
            foreach (var operand in Oparands)
            {
                if (operand == null)
                    continue;
                operand.Construct(context, this);
            }

            // prefix
            // opcode
            // modRM
            // sib
            // Displ
            // Imm
        }


        public override string ToString() => Mnemonic + " " + string.Join(", ", Oparands);
    }
}
