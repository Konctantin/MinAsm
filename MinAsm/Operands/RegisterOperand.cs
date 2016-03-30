using MinAsm.Encoding;

namespace MinAsm.Operands
{
    public class RegisterOperand : Operand
    {
        public Register Register { get; }

        public override DataSize Size => Register.Size;

        public OperandEncoding Encoding { get; }

        public RegisterOperand(Register register, OperandEncoding encoding = OperandEncoding.Default)
            : base(register.Size)
        {
            Register = register;
            Encoding = encoding;
        }

        public override int Construct(Context context, Instruction instruction)
        {
            switch (Encoding)
            {
                case OperandEncoding.Default:
                    instruction.SetModRM();
                    instruction.ModRM.Reg = Register.Value;
                    break;
                case OperandEncoding.AddToOpcode:
                    instruction.Opcode.Add(Register.Value);
                    break;
                case OperandEncoding.ModRm:
                    instruction.SetModRM();
                    instruction.ModRM.Mod = 0x03;
                    instruction.ModRM.RM  = Register.Value;
                    instruction.ModRM.Reg = instruction.FixedReg;
                    break;
                case OperandEncoding.Ignore:
                    // The operand is ignored.
                    break;
            }

            // Set the operand size to the size of the register.
            return 0;
        }

        public override string ToString() => Register.Name;
    }
}
