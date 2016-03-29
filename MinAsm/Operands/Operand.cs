using MinAsm.Encoding;

namespace MinAsm.Operands
{
    public abstract class Operand
    {
        public DataSize PerfSize { get; set; }
        public virtual DataSize Size => PerfSize;
        public virtual int OperandSize => (int)Size >> 3;

        public Operand(DataSize dataSize)
        {
            PerfSize = dataSize;
        }

        public abstract int Construct(Context context, Instruction instruction);

        public override string ToString() => $"Size: 0x{((int)PerfSize >> 3):X}";
    }
}
