using System;

namespace MinAsm.Encoding
{
    /// <summary>
    /// Certain encodings of the ModR/M byte require a second addressing byte.
    /// </summary>
    public class ModRM
    {
        byte m_rm  = 0,
             m_reg = 0,
             m_mod = 0;

        public ModRM(byte value = 0) : this(
                (byte)((value     ) & 7),
                (byte)((value >> 3) & 7),
                (byte)((value >> 6) & 3))
        {
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="rm">The r/m field can specify a register as an operand or it can be combined with the mod field to encode an addressing mode.
        /// Sometimes, certain combinations of the mod field and the r/m field is used to express opcode information for some instructions.</param>
        /// <param name="reg">The reg/opcode field specifies either a register number or three more bits of opcode information.
        /// The purpose of the reg/opcode field is specified in the primary opcode.</param>
        /// <param name="mod">The mod field combines with the r/m field to form 32 possible values: eight registers and 24 addressing modes.</param>
        public ModRM(byte rm, byte reg, byte mod)
        {
            if (rm > 7)
                throw new ArgumentOutOfRangeException(nameof(rm));

            if (reg > 7)
                throw new ArgumentOutOfRangeException(nameof(reg));

            if (mod > 3)
                throw new ArgumentOutOfRangeException(nameof(mod));

            m_rm  = rm;
            m_reg = reg;
            m_mod = mod;
        }

        /// <summary>
        /// The r/m field can specify a register as an operand or it can be combined with the mod field to encode an addressing mode.
        /// Sometimes, certain combinations of the mod field and the r/m field is used to express opcode information for some instructions.
        /// </summary>
        public byte RM
        {
            get { return m_rm; }
            set
            {
                if (value > 7)
                    throw new ArgumentOutOfRangeException(nameof(value));

                m_rm = value;
            }
        }

        /// <summary>
        /// The reg/opcode field specifies either a register number or three more bits of opcode information.
        /// The purpose of the reg/opcode field is specified in the primary opcode.
        /// </summary>
        public byte Reg
        {
            get { return m_reg; }
            set
            {
                if (value > 7)
                    throw new ArgumentOutOfRangeException(nameof(value));

                m_reg = value;
            }
        }

        /// <summary>
        /// The mod field combines with the r/m field to form 32 possible values: eight registers and 24 addressing modes.
        /// </summary>
        public byte Mod
        {
            get { return m_mod; }
            set
            {
                if (value > 3)
                    throw new ArgumentOutOfRangeException(nameof(value));

                m_mod = value;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="size"></param>
        public void ModFromDataSize(DataSize size)
        {
            switch (size)
            {
                case DataSize.None:  Mod = 0x00; break;
                case DataSize.Bit8:  Mod = 0x01; break;
                case DataSize.Bit16: Mod = 0x02; break;
                case DataSize.Bit32: Mod = 0x02; break;
            }
        }

        /// <summary>
        /// REX.R
        /// </summary>
        public byte RexR => (byte)((m_reg & 0x08) >> 1);

        /// <summary>
        /// REX.B
        /// </summary>
        public byte RexB => (byte)((m_rm & 0x08) >> 3);

        /// <summary>
        ///
        /// </summary>
        byte Raw => (byte)((m_mod << 6) | ((m_reg & 7) << 3) | (m_rm & 7));

        /// <summary>
        ///
        /// </summary>
        public bool IsEmpty => m_mod == 0 && m_reg == 0 && m_rm == 0;

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            if (obj != null && obj is ModRM)
                return (obj as ModRM).Raw == Raw;
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
        public override string ToString() => $"RM: {RM:X2} Reg/Opcode: {Reg:X2} Mod: {Mod:X2}";

        /// <summary>
        ///
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(ModRM a, ModRM b) => a?.Raw == b?.Raw;

        /// <summary>
        ///
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(ModRM a, ModRM b) => a?.Raw != b?.Raw;

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        public static explicit operator byte(ModRM value) => value.Raw;

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        public static explicit operator ModRM(byte value) => new ModRM(value);
    }
}
