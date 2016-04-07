using MinAsm.Encoding;
using System;

namespace MinAsm.Operands
{
    public class Imm : Operand
    {
        long m_value;

        #region Constructors

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

        #endregion

        public override int Construct(Context context, Instruction instruction)
        {
            // todo: more variants
            instruction.Immedicate = this;
            return OperandSize;
        }

        public override string ToString() => $"0x{m_value:X}";

        public byte[] Bytes
        {
            get
            {
                switch(Size)
                {
                    case DataSize.None:  return new byte[0];
                    case DataSize.Bit8:  return BitConverter.GetBytes((byte)m_value);
                    case DataSize.Bit16: return BitConverter.GetBytes((short)m_value);
                    case DataSize.Bit32: return BitConverter.GetBytes((int)m_value);
                    case DataSize.Bit64: return BitConverter.GetBytes(m_value);
                    default:
                        throw new NotSupportedException();
                }
            }
        }

        #region Operators

        public static explicit operator byte(Imm imm)   => (byte)imm.m_value;
        public static explicit operator sbyte(Imm imm)  => (sbyte)imm.m_value;
        public static explicit operator short(Imm imm)  => (short)imm.m_value;
        public static explicit operator ushort(Imm imm) => (ushort)imm.m_value;
        public static explicit operator int(Imm imm)    => (int)imm.m_value;
        public static explicit operator uint(Imm imm)   => (uint)imm.m_value;
        public static explicit operator long(Imm imm)   => imm.m_value;

        #endregion
    }
}
