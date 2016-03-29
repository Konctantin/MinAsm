using System;

namespace MinAsm
{
    /// <summary>
    /// Specifies the size of the a data unit.
    /// </summary>
    [Flags]
    public enum DataSize
    {
        /// <summary>
        /// No data size.
        /// </summary>
        None   = 0,
        /// <summary>
        /// An 8-bit data unit.
        /// </summary>
        Bit8   = 8   >> 3,
        /// <summary>
        /// A 16-bit data unit.
        /// </summary>
        Bit16  = 16  >> 3,
        /// <summary>
        /// A 32-bit data unit.
        /// </summary>
        Bit32  = 32  >> 3,
        /// <summary>
        /// A 64-bit data unit.
        /// </summary>
        Bit64  = 64  >> 3,
    }
}