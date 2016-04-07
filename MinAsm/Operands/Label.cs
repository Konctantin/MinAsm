using System;
using MinAsm.Encoding;

namespace MinAsm.Operands
{
    public class Label : Operand
    {
        public Label()
            : base(DataSize.None)
        {
        }

        public override int Construct(Context context, Instruction instruction)
        {
            throw new NotImplementedException();
        }
    }
}
