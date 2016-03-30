using System.Collections.Generic;
using System.Linq;
using MinAsm.Operands;

namespace MinAsm.Encoding
{
    public class Instruction
    {
        public string Mnemonic { get; }

        public byte Prefix { get; private set; }

        public Opcode Opcode { get; protected set; }

        public byte FixedReg;

        public ModRM ModRM { get; private set; }

        public Sib Sib { get; private set; }

        public byte OpcodeReg { get; set; }

        public List<Operand> Oparands { get; } = new List<Operand>();

        public long? Displacement { get; internal set; }
        public DataSize DisplacementSize { get; internal set; }

        public Imm Immedicate { get; set; }

        // public Imm ExtraImmedicate { get; set; }

        public Instruction(string mnemonic, byte prefix, Opcode opcode, byte fixedReg, params Operand[] operands)
        {
            Mnemonic = mnemonic;
            Prefix   = prefix;
            Opcode   = opcode;
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

            // modRM
            // sib

            // Displ
            // Imm
            // ExtraImm
        }

        public int Size
        {
            get
            {
                int size = 0;
                if (Prefix != 0)
                    ++size;
                size += Opcode.Size;
                if (ModRM != null)
                    ++size;
                if (Sib != null)
                    ++size;
                if (Immedicate != null)
                    size += Immedicate.OperandSize;
                return size;
            }
        }

        public IEnumerable<byte> Bytes
        {
            get
            {
                if (Prefix != 0)
                    yield return Prefix;

                foreach (var b in Opcode)
                    yield return b;

                if (ModRM != null)
                    yield return (byte)ModRM;

                if (Sib != null)
                    yield return (byte)Sib;

                if (Immedicate != null)
                    foreach (var b in Immedicate.Bytes)
                        yield return b;

                yield break;
            }
        }


        public override string ToString() => (Mnemonic + " " + string.Join(", ", Oparands))
            .PadRight(40, ' ')
            + string.Join(" ", Bytes.Select(n => n.ToString("X2")))
            ;
    }
}
