using MinAsm.Encoding;

namespace MinAsm.Operands
{
    public class Imm : Operand
    {
        long m_value;

        public Imm()
            : this(0L, DataSize.None)
        {
        }

        public Imm(byte value)
            : this(value, DataSize.Bit8)
        {
        }

        public Imm(sbyte value)
            : this(value, DataSize.Bit8)
        {
        }

        public Imm(short value)
            : this(value, DataSize.Bit16)
        {
        }

        public Imm(ushort value)
            : this(value, DataSize.Bit16)
        {
        }

        public Imm(int value)
            : this(value, DataSize.Bit32)
        {
        }

        public Imm(uint value)
            : this(value, DataSize.Bit32)
        {
        }

        public Imm(long value, DataSize size = DataSize.Bit64)
            : base(size)
        {
            m_value = value;
        }

        public override int Construct(Context context, Instruction instruction)
        {
            return OperandSize;
        }

        public override string ToString() => $"0x{m_value:X}";
    }
}
