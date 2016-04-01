using System;
using MinAsm.Encoding;

namespace MinAsm.Operands
{
    public class EffectiveAddres : Operand
    {
        public Register Base { get; }
        public Register Index { get; }
        public Scale Scale { get; }

        public EffectiveAddres(Register @base)
            : this(@base, Register.None, 0)
        {
        }

        public EffectiveAddres(Register @base, Register index)
            : this(@base, index, Scale.None)
        {
        }

        public EffectiveAddres(Register @base, Register index, Scale scale)
            : base(DataSize.None)
        {
            Base  = @base;
            Index = index;
            Scale = scale;
        }

        public override int Construct(Context context, Instruction instruction)
        {
            // setOperandSize
            // EncodeDisplacement(context, instr, addressSize);
            switch (context.Architecture)
            {
                case Architectures.X86:
                    Encode32BitEffectiveAddress(instruction);
                    break;
                case Architectures.X64:
                    Encode64BitEffectiveAddress(context, instruction);
                    break;
            }
            // setOperandSize
            return 0;
        }

        /// <summary>
        /// Encodes a 32-bit effective address.
        /// </summary>
        /// <param name="instr">The <see cref="EncodedInstruction"/> encoding the operand.</param>
        void Encode32BitEffectiveAddress(Instruction instr)
        {
            instr.SetModRM();

            if (Base.IsNone && Index.IsNone)
            {

                instr.ModRM.RM  = 0x05;// R/M
                instr.ModRM.Mod = 0x00;// Mod

                // Only 32-bit displacements can be encoded without a base and index register.
                instr.DisplacementSize = DataSize.Bit32;
                if (instr.Displacement == null)
                    instr.Displacement = 0L;
            }
            else if (Base != Register.ESP && Index.IsNone)
            {
                // R/M
                instr.ModRM.RM = Base.Value07;

                // Displacement.
                if (instr.Displacement == null && Base == Register.EBP)
                {
                    // [EBP] will be represented as [EBP+disp8].
                    instr.DisplacementSize = DataSize.Bit8;
                    instr.Displacement = 0;
                }

                // Mod
                instr.ModRM.ModFromDataSize(instr.DisplacementSize);
            }
            else // [EBP+...]
            {
                // Encode the SIB byte too.
                instr.SetSib();
                instr.ModRM.RM = 0x04;// R/M

                // Displacement
                if (instr.Displacement == null && Base == Register.EBP)
                {
                    // [EBP+REG*s] will be represented as [EBP+REG*s+disp8].
                    instr.DisplacementSize = DataSize.Bit8;
                    instr.Displacement = 0;
                }

                // Mod
                instr.ModRM.ModFromDataSize(instr.DisplacementSize);

                // Base
                instr.Sib.Base = Base.Value07;
                if (Base.IsNone)
                    instr.Sib.Base = 0x05;

                // Index
                instr.Sib.Index = Index.Value07;
                if (Index.IsNone)
#warning what is this
                    instr.Sib.Index = 0x20;

                // Scale
                instr.Sib.Scale = Scale;
            }
        }

        /// <summary>
		/// Encodes a 64-bit effective address.
		/// </summary>
		/// <param name="context">The <see cref="Context"/> in which the operand is used.</param>
		/// <param name="instr">The <see cref="EncodedInstruction"/> encoding the operand.</param>
		void Encode64BitEffectiveAddress(Context context, Instruction instr)
        {
            instr.SetModRM();

            bool ripRelative = false;// this.relativeAddress ?? ((X86Architecture)context.Representation.Architecture).UseRIPRelativeAddressing;
            //bool forceRipRelative = false;// this.relativeAddress.HasValue && this.relativeAddress == true;

            if (Base.IsNone && Index.IsNone)
            {
                if (ripRelative)
                {
                    // [RIP+disp32]
                    instr.ModRM.RM  = 0x05;
                    instr.ModRM.Mod = 0x00;
                }
                else
                {
                    // [disp32]

                    instr.ModRM.RM  = 0x04;
                    instr.ModRM.Mod = 0x00;

                    instr.SetSib();
                    instr.Sib.Base  = 0x05;
                    instr.Sib.Index = 0x04;
                    instr.Sib.Scale = 0x00;
                }

                // Only 32-bit displacements can be encoded without a base and index register.
                instr.DisplacementSize = DataSize.Bit32;
                if (instr.Displacement == null)
                    instr.Displacement = 0;
            }
            else
            {
                //if (forceRipRelative)
                //    throw new AssemblerException("The effective address cannot be encoded with RIP-relative addressing.");

                if (Base != Register.RSP && Index.IsNone)
                {
                    // [REG+...]
                    instr.ModRM.RM = Base.Value07;
                }
                else
                {
                    // [REG+REG*s+...]

                    // Encode the SIB byte too.
                    instr.SetSib();

                    // R/M
                    instr.ModRM.RM = 0x04;

                    // Base
                    if (!Base.IsNone)
                        instr.Sib.Base = Base.Value07;
                    else
                        instr.Sib.Base = 0x05;

                    // Index
                    if (!Index.IsNone)
                        instr.Sib.Index = Index.Value07;
                    else
#warning what is this
                        instr.Sib.Index = 0x20;

                    // Scale
                    instr.Sib.Scale = Scale;
                }

                if (instr.Displacement == null && Base == Register.RBP)
                {
                    // [RBP] will be represented as [RBP+disp8].
                    // [RBP+REG*s] will be represented as [RBP+REG*s+disp8].
                    instr.DisplacementSize = DataSize.Bit8;
                    instr.Displacement = 0;
                }

                instr.ModRM.ModFromDataSize(instr.DisplacementSize);
            }
        }

        public override string ToString()
        {
            var str = "";
            if (!Base.IsNone)
                str += "[" + Base.Name;
            if (!Index.IsNone)
                str += "+" + Index.Name;
            if (Scale != Scale.None)
                str += "*" + (byte)Scale;
            if (!string.IsNullOrEmpty(str))
                str += "]";
            return str;
        }
    }
}
