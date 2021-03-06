﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MinAsm
{
    /// <summary>
    /// Specifies a type of register.
    /// </summary>
    [Flags]
    public enum RegisterType
    {
        /// <summary>
        /// No register type.
        /// </summary>
        None = 0,
        /// <summary>
        /// A general purpose register.
        /// </summary>
        GeneralPurpose = GeneralPurpose8Bit | GeneralPurpose16Bit | GeneralPurpose32Bit | GeneralPurpose64Bit,
        /// <summary>
        /// An 8-bit general purpose register.
        /// </summary>
        GeneralPurpose8Bit = 0x100 | DataSize.Bit8,
        /// <summary>
        /// A 16-bit general purpose register.
        /// </summary>
        GeneralPurpose16Bit = 0x100 | DataSize.Bit16,
        /// <summary>
        /// A 32-bit general purpose register.
        /// </summary>
        GeneralPurpose32Bit = 0x100 | DataSize.Bit32,
        /// <summary>
        /// A 64-bit general purpose register.
        /// </summary>
        GeneralPurpose64Bit = 0x100 | DataSize.Bit64,
        /// <summary>
        /// A 64-bit MMX register.
        /// </summary>
        Simd64Bit = 0x400 | DataSize.Bit64,
        /// <summary>
        /// A 16-bit segment register.
        /// </summary>
        Segment = 0x1000 | DataSize.Bit16,
        /// <summary>
        /// A 32-bit control register.
        /// </summary>
        Control = 0x2000 | DataSize.Bit32,
        /// <summary>
        /// A 32-bit debug register.
        /// </summary>
        Debug = 0x4000 | DataSize.Bit32,
    }
}
