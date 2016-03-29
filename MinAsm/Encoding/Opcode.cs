using System;

namespace MinAsm.Encoding
{
    public class Opcode
    {
        public byte[] Bytes { get; }

        public Opcode(params byte[] bytes)
        {
            Bytes = bytes;
        }

        /// <summary>
        /// Modifyed last byte.
        /// </summary>
        /// <param name="value"></param>
        public void Add(byte value)
        {
            if (Bytes.Length == 0)
                throw new IndexOutOfRangeException();

            if (value > 15)
                throw new ArgumentOutOfRangeException(nameof(value));

            Bytes[Bytes.Length - 1] |= (byte)(value & 0x7);
        }
    }
}
