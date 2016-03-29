using MinAsm.Encoding;

namespace MinAsm.Operands
{
    public abstract class Operand
    {
        public virtual DataSize PerfSize { get; set; }
        public virtual DataSize Size => PerfSize;
        public virtual int OperandSize => (int)Size >> 3;

        public Operand(DataSize dataSize)
        {
            PerfSize = dataSize;
        }

        public abstract int Construct(Context context, Instruction instruction);
    }
}
