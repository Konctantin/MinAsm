using System.Collections.Generic;
using MinAsm.Operands;

namespace MinAsm.Encoding
{
    public class Instruction
    {
        public string Mnemonic { get; }

        public byte Prefix { get; private set; }

        public Opcode Opcode { get; protected set; }

        public OperandEncoding Encoding { get; }

        public byte FixedReg;

        public ModRM ModRM { get; private set; }

        public Sib Sib { get; private set; }

        public byte OpcodeReg { get; set; }

        public List<Operand> Oparands { get; } = new List<Operand>();

        public long? Displacement { get; internal set; }
        public DataSize DisplacementSize { get; internal set; }

        public Imm Immedicate { get; set; }

        // public Imm ExtraImmedicate { get; set; }

        public Instruction(string mnemonic, byte prefix, Opcode opcode, OperandEncoding encoding, byte fixedReg, params Operand[] operands)
        {
            Mnemonic = mnemonic;
            Prefix   = prefix;
            Opcode   = opcode;
            Encoding = encoding;
            FixedReg = fixedReg;
            Oparands = new List<Operand>(operands);
        }

        /// <summary>
        /// Emits the REX-prefix, if used.
        /// </summary>
        void ConstructPrefix()
        {
            if (Prefix > 0)
            {
                Prefix |= 0x08;

                if (ModRM != null && Sib != null)
                {
                    Prefix |= Sib.RexB;
                    Prefix |= Sib.RexX;
                    Prefix |= ModRM.RexR;
                }
                else if (ModRM != null)
                {
                    Prefix |= ModRM.RexB;
                    Prefix |= ModRM.RexR;
                }
                else
                {
                    // No ModR/M or SIB bytes, but a reg-value anyway.
                    Prefix |= (byte)((OpcodeReg & 0x08) >> 3);     // REX.B
                }
            }
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

            ConstructPrefix();

            if (Encoding == OperandEncoding.AddToOpcode)
                Opcode.Add(FixedReg);

            // modRM
            // sib

            // Displ
            // Imm
            // ExtraImm
        }


        public override string ToString() => Mnemonic + " " + string.Join(", ", Oparands);
    }
}
