using MinAsm.Encoding;

namespace MinAsm.Operands
{
    public abstract class Operand
    {
        public virtual DataSize Size { get; protected set; }
        public virtual int OperandSize => (int)Size >> 3;

        public Operand(DataSize dataSize)
        {
            Size = dataSize;
        }

        public abstract int Construct(Context context, Instruction instruction);

        public override string ToString() => $"Size: 0x{OperandSize:X}";
    }
}
