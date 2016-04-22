using System;

namespace MinAsm.Encoding
{
    /// <summary>
    /// The base-plus-index and scale-plus-index forms of 32-bit addressing require the SIB byte.
    /// </summary>
    public sealed class Sib
    {
        byte m_b = 0,
             m_i = 0;
        Scale m_s = Scale.None;

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        public Sib(byte value = 0) : this(
                (byte)((value     ) & 7),
                (byte)((value >> 3) & 7),
                (byte)((value >> 6) & 3))
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="b">The base field specifies the register number of the base register.</param>
        /// <param name="i">The index field specifies the register number of the index register.</param>
        /// <param name="s">The scale field specifies the scale factor.</param>
        public Sib(byte b, byte i, byte s)
        {
            if (b > 7)
                throw new ArgumentOutOfRangeException(nameof(b));

            if (i > 7)
                throw new ArgumentOutOfRangeException(nameof(i));

            if (s > 3)
                throw new ArgumentOutOfRangeException(nameof(s));

            m_b = b;
            m_i = i;
            m_s = (Scale)s;
        }

        /// <summary>
        /// The base field specifies the register number of the base register.
        /// </summary>
        public byte Base
        {
            get { return m_b; }
            set
            {
                if (value > 7)
                    throw new ArgumentOutOfRangeException(nameof(value));

                m_b = value;
            }
        }

        /// <summary>
        /// The index field specifies the register number of the index register.
        /// </summary>
        public byte Index
        {
            get { return m_i; }
            set
            {
                if (value > 7)
                    throw new ArgumentOutOfRangeException(nameof(value));

                m_i = value;
            }
        }

        /// <summary>
        /// The scale field specifies the scale factor.
        /// </summary>
        public Scale Scale
        {
            get { return m_s; }
            set
            {
                if ((byte)value > 3)
                    throw new ArgumentOutOfRangeException(nameof(value));

                m_s = value;
            }
        }

        /// <summary>
        /// REX.B
        /// </summary>
        public byte RexB => (byte)((m_b & 0x08) >> 3);

        /// <summary>
        /// REX.X
        /// </summary>
        public byte RexX => (byte)((m_i & 0x08) >> 2);

        /// <summary>
        ///
        /// </summary>
        byte Raw => (byte)(((byte)m_s << 6) | ((m_i & 7) << 3) | (m_b & 7));

        /// <summary>
        ///
        /// </summary>
        public bool IsEmpty => m_s == 0 && m_i == 0 && m_b == 0;

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj != null && obj is Sib)
                return (obj as Sib).Raw == Raw;
            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => Raw;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override string ToString() => $"Base: {Base:X2} Index: {Index:X2} Scale: {(byte)Scale:X2}";

        /// <summary>
        ///
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(Sib a, Sib b) => a?.Raw == b?.Raw;

        /// <summary>
        ///
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(Sib a, Sib b) => a?.Raw != b?.Raw;

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        public static explicit operator byte(Sib value) => value.Raw;

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        public static explicit operator Sib(byte value) => new Sib(value);
    }
}
