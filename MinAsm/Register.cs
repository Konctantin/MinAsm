namespace MinAsm
{
    /// <summary>
    /// An x86-64 register.
    /// </summary>
    public class Register
    {
        /// <summary>
        /// Return register name.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Return register value.
        /// </summary>
        public byte Value { get; }

        /// <summary>
        /// Return register type <see cref="RegisterType"/>
        /// </summary>
        public RegisterType Type { get; }

        /// <summary>
        /// Returns the register size <see cref="DataSize"/>.
        /// </summary>
        public DataSize Size => (DataSize)((int)Type & 0x3F);

        /// <summary>
        /// Return full value.
        /// </summary>
        public int Full => Value | (int)Type << 5;

        /// <summary>
        ///
        /// </summary>
        public byte Value07 => (byte)(Value & 0x07);

        /// <summary>
        /// Cretae new instance of <see cref="Register"/>
        /// </summary>
        /// <param name="name">Register name.</param>
        /// <param name="value">Register value.</param>
        /// <param name="type">Register type <see cref="RegisterType"/></param>
        private Register(string name, byte value, RegisterType type)
        {
            Name  = name;
            Value = value;
            Type  = type;
        }

        /// <summary>
        ///
        /// </summary>
        public bool IsNone => Value == 0 && Type == RegisterType.None;

        /// <summary>
        /// Returns whether the register is a general purpose register.
        /// </summary>
        public bool IsGeneralPurpose => Type == RegisterType.GeneralPurpose;

        /// <summary>
        /// Returns whether the register is a control register.
        /// </summary>
        public bool IsControl => Type == RegisterType.Control;

        /// <summary>
        /// Returns whether the register is a debug register.
        /// </summary>
        public bool IsDebug => Type == RegisterType.Debug;

        /// <summary>
        /// Returns whether the register is an x87 floating-point register.
        /// </summary>
        //public bool IsFloatingPoint => Type == RegisterType.FloatingPoint;

        /// <summary>
        /// Returns whether the register is a segment register.
        /// </summary>
        public bool IsSegment => Type == RegisterType.Segment;

        /// <summary>
        /// Returns whether the register is a SIMD register.
        /// </summary>
        //public bool IsSimd => Type == RegisterType.Simd64Bit || Type == RegisterType.Simd128Bit;

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var tmp = obj as Register;
            if (tmp != null && obj is Register)
                return Value == tmp.Value && Type == tmp.Type;
            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode() => Full;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public override string ToString() => Name;

        /// <summary>
        ///
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator ==(Register a, Register b) => a?.Equals(b) == true;

        /// <summary>
        ///
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool operator !=(Register a, Register b) => a?.Equals(b) != true;

        #region Registers

        /// <summary>
        /// Empty register.
        /// </summary>
        public static Register None => new Register("<none>", 0, RegisterType.None);

        #region 8-bits

        /// <summary>
        /// The lower 8-bits of the accumulator register.
        /// </summary>
        public static Register AL => new Register(nameof(AL), 0x00, RegisterType.GeneralPurpose8Bit);

        /// <summary>
        /// The lower 8-bits of the counter register.
        /// </summary>
        public static Register CL => new Register(nameof(CL), 0x01, RegisterType.GeneralPurpose8Bit);

        /// <summary>
        /// The lower 8-bits of the data register.
        /// </summary>
        public static Register DL => new Register(nameof(DL), 0x02, RegisterType.GeneralPurpose8Bit);

        /// <summary>
        /// The lower 8-bits of the base register.
        /// </summary>
        public static Register BL => new Register(nameof(BL), 0x03, RegisterType.GeneralPurpose8Bit);

        /// <summary>
        /// The higher 8-bits of the lower 16-bits of the accumulator register.
        /// </summary>
        public static Register AH => new Register(nameof(AH), 0x04, RegisterType.GeneralPurpose8Bit);

        /// <summary>
        /// The higher 8-bits of the lower 16-bits of the counter register.
        /// </summary>
        public static Register CH => new Register(nameof(CH), 0x05, RegisterType.GeneralPurpose8Bit);

        /// <summary>
        /// The higher 8-bits of the lower 16-bits of the data register.
        /// </summary>
        public static Register DH => new Register(nameof(DH), 0x06, RegisterType.GeneralPurpose8Bit);

        /// <summary>
        /// The higher 8-bits of the lower 16-bits of the base register.
        /// </summary>
        public static Register BH => new Register(nameof(BH), 0x07, RegisterType.GeneralPurpose8Bit);

        /// <summary>
        /// The lower 8-bits of the nineth general purpose register.
        /// </summary>
        public static Register R8L => new Register(nameof(R8L), 0x08, RegisterType.GeneralPurpose8Bit);

        /// <summary>
        /// The lower 8-bits of the tenth general purpose register.
        /// </summary>
        public static Register R9L => new Register(nameof(R9L), 0x09, RegisterType.GeneralPurpose8Bit);

        /// <summary>
        /// The lower 8-bits of the eleventh general purpose register.
        /// </summary>
        public static Register R10L => new Register(nameof(R10L), 0x0A, RegisterType.GeneralPurpose8Bit);

        /// <summary>
        /// The lower 8-bits of the twelfth general purpose register.
        /// </summary>
        public static Register R11L => new Register(nameof(R11L), 0x0B, RegisterType.GeneralPurpose8Bit);

        /// <summary>
        /// The lower 8-bits of the thirteenth general purpose register.
        /// </summary>
        public static Register R12L => new Register(nameof(R12L), 0x0C, RegisterType.GeneralPurpose8Bit);

        /// <summary>
        /// The lower 8-bits of the fourteenth general purpose register.
        /// </summary>
        public static Register R13L => new Register(nameof(R13L), 0x0D, RegisterType.GeneralPurpose8Bit);

        /// <summary>
        /// The lower 8-bits of the fifteenth general purpose register.
        /// </summary>
        public static Register R14L => new Register(nameof(R14L), 0x0E, RegisterType.GeneralPurpose8Bit);

        /// <summary>
        /// The lower 8-bits of the sixteenth general purpose register.
        /// </summary>
        public static Register R15L => new Register(nameof(R15L), 0x0F, RegisterType.GeneralPurpose8Bit);

        #endregion

        #region 16-bits

        /// <summary>
        /// The lower 16-bits of the accumulator register.
        /// </summary>
        public static Register AX => new Register(nameof(AX), 0x00, RegisterType.GeneralPurpose16Bit);

        /// <summary>
        /// The lower 16-bits of the counter register.
        /// </summary>
        public static Register CX => new Register(nameof(CX), 0x01, RegisterType.GeneralPurpose16Bit);

        /// <summary>
        /// The lower 16-bits of the data register.
        /// </summary>
        public static Register DX => new Register(nameof(DX), 0x02, RegisterType.GeneralPurpose16Bit);

        /// <summary>
        /// The lower 16-bits of the base register.
        /// </summary>
        public static Register BX => new Register(nameof(BX), 0x03, RegisterType.GeneralPurpose16Bit);

        /// <summary>
        /// The lower 16-bits of the stack pointer register.
        /// </summary>
        public static Register SP => new Register(nameof(SP), 0x04, RegisterType.GeneralPurpose16Bit);

        /// <summary>
        /// The lower 16-bits of the base pointer register.
        /// </summary>
        public static Register BP => new Register(nameof(BP), 0x05, RegisterType.GeneralPurpose16Bit);

        /// <summary>
        /// The lower 16-bits of the source index register.
        /// </summary>
        public static Register SI => new Register(nameof(SI), 0x06, RegisterType.GeneralPurpose16Bit);

        /// <summary>
        /// The lower 16-bits of the destination index register.
        /// </summary>
        public static Register DI => new Register(nameof(DI), 0x07, RegisterType.GeneralPurpose16Bit);

        /// <summary>
        /// The lower 16-bits of the nineth general purpose register.
        /// </summary>
        public static Register R8W => new Register(nameof(R8W), 0x08, RegisterType.GeneralPurpose16Bit);

        /// <summary>
        /// The lower 16-bits of the tenth general purpose register.
        /// </summary>
        public static Register R9W => new Register(nameof(R9W), 0x09, RegisterType.GeneralPurpose16Bit);

        /// <summary>
        /// The lower 16-bits of the eleventh general purpose register.
        /// </summary>
        public static Register R10W => new Register(nameof(R10W), 0x0A, RegisterType.GeneralPurpose16Bit);

        /// <summary>
        /// The lower 16-bits of the twelfth general purpose register.
        /// </summary>
        public static Register R11W => new Register(nameof(R11W), 0x0B, RegisterType.GeneralPurpose16Bit);

        /// <summary>
        /// The lower 16-bits of the thirteenth general purpose register.
        /// </summary>
        public static Register R12W => new Register(nameof(R12W), 0x0C, RegisterType.GeneralPurpose16Bit);

        /// <summary>
        /// The lower 16-bits of the fourteenth general purpose register.
        /// </summary>
        public static Register R13W => new Register(nameof(R13W), 0x0D, RegisterType.GeneralPurpose16Bit);

        /// <summary>
        /// The lower 16-bits of the fifteenth general purpose register.
        /// </summary>
        public static Register R14W => new Register(nameof(R14W), 0x0E, RegisterType.GeneralPurpose16Bit);

        /// <summary>
        /// The lower 16-bits of the sixteenth general purpose register.
        /// </summary>
        public static Register R15W => new Register(nameof(R15W), 0x0F, RegisterType.GeneralPurpose16Bit);

        #endregion

        #region 32-bits

        /// <summary>
        /// The lower 32-bits of the accumulator register.
        /// </summary>
        public static Register EAX => new Register(nameof(EAX), 0x00, RegisterType.GeneralPurpose32Bit);

        /// <summary>
        /// The lower 32-bits of the counter register.
        /// </summary>
        public static Register ECX => new Register(nameof(ECX), 0x01, RegisterType.GeneralPurpose32Bit);

        /// <summary>
        /// The lower 32-bits of the data register.
        /// </summary>
        public static Register EDX => new Register(nameof(EDX), 0x02, RegisterType.GeneralPurpose32Bit);

        /// <summary>
        /// The lower 32-bits of the base register.
        /// </summary>
        public static Register EBX => new Register(nameof(EBX), 0x03, RegisterType.GeneralPurpose32Bit);

        /// <summary>
        /// The lower 32-bits of the stack pointer register.
        /// </summary>
        public static Register ESP => new Register(nameof(ESP), 0x04, RegisterType.GeneralPurpose32Bit);

        /// <summary>
        /// The lower 32-bits of the base pointer register.
        /// </summary>
        public static Register EBP => new Register(nameof(EBP), 0x05, RegisterType.GeneralPurpose32Bit);

        /// <summary>
        /// The lower 32-bits of the source index register.
        /// </summary>
        public static Register ESI => new Register(nameof(ESI), 0x06, RegisterType.GeneralPurpose32Bit);

        /// <summary>
        /// The lower 32-bits of the destination index register.
        /// </summary>
        public static Register EDI => new Register(nameof(EDI), 0x07, RegisterType.GeneralPurpose32Bit);

        /// <summary>
        /// The lower 32-bits of the nineth general purpose register.
        /// </summary>
        public static Register R8D => new Register(nameof(R8D), 0x08, RegisterType.GeneralPurpose32Bit);

        /// <summary>
        /// The lower 32-bits of the tenth general purpose register.
        /// </summary>
        public static Register R9D => new Register(nameof(R9D), 0x09, RegisterType.GeneralPurpose32Bit);

        /// <summary>
        /// The lower 32-bits of the eleventh general purpose register.
        /// </summary>
        public static Register R10D => new Register(nameof(R10D), 0x0A, RegisterType.GeneralPurpose32Bit);

        /// <summary>
        /// The lower 32-bits of the twelfth general purpose register.
        /// </summary>
        public static Register R11D => new Register(nameof(R11D), 0x0B, RegisterType.GeneralPurpose32Bit);

        /// <summary>
        /// The lower 32-bits of the thirteenth general purpose register.
        /// </summary>
        public static Register R12D => new Register(nameof(R12D), 0x0C, RegisterType.GeneralPurpose32Bit);

        /// <summary>
        /// The lower 32-bits of the fourteenth general purpose register.
        /// </summary>
        public static Register R13D => new Register(nameof(R13D), 0x0D, RegisterType.GeneralPurpose32Bit);

        /// <summary>
        /// The lower 32-bits of the fifteenth general purpose register.
        /// </summary>
        public static Register R14D => new Register(nameof(R14D), 0x0E, RegisterType.GeneralPurpose32Bit);

        /// <summary>
        /// The lower 32-bits of the sixteenth general purpose register.
        /// </summary>
        public static Register R15D => new Register(nameof(R15D), 0x0F, RegisterType.GeneralPurpose32Bit);

        #endregion

        #region 64-bits

        /// <summary>
        /// The lower 64-bits of the accumulator register.
        /// </summary>
        public static Register RAX => new Register(nameof(RAX), 0x00, RegisterType.GeneralPurpose64Bit);

        /// <summary>
        /// The lower 64-bits of the counter register.
        /// </summary>
        public static Register RCX => new Register(nameof(RCX), 0x01, RegisterType.GeneralPurpose64Bit);

        /// <summary>
        /// The lower 64-bits of the data register.
        /// </summary>
        public static Register RDX => new Register(nameof(RDX), 0x02, RegisterType.GeneralPurpose64Bit);

        /// <summary>
        /// The lower 64-bits of the base register.
        /// </summary>
        public static Register RBX => new Register(nameof(RBX), 0x03, RegisterType.GeneralPurpose64Bit);

        /// <summary>
        /// The lower 64-bits of the stack pointer register.
        /// </summary>
        public static Register RSP => new Register(nameof(RSP), 0x04, RegisterType.GeneralPurpose64Bit);

        /// <summary>
        /// The lower 64-bits of the base pointer register.
        /// </summary>
        public static Register RBP => new Register(nameof(RBP), 0x05, RegisterType.GeneralPurpose64Bit);

        /// <summary>
        /// The lower 64-bits of the source index register.
        /// </summary>
        public static Register RSI => new Register(nameof(RSI), 0x06, RegisterType.GeneralPurpose64Bit);

        /// <summary>
        /// The lower 64-bits of the destination index register.
        /// </summary>
        public static Register RDI => new Register(nameof(RDI), 0x07, RegisterType.GeneralPurpose64Bit);

        /// <summary>
        /// The lower 64-bits of the nineth general purpose register.
        /// </summary>
        public static Register R8 => new Register(nameof(R8), 0x08, RegisterType.GeneralPurpose64Bit);

        /// <summary>
        /// The lower 64-bits of the tenth general purpose register.
        /// </summary>
        public static Register R9 => new Register(nameof(R9), 0x09, RegisterType.GeneralPurpose64Bit);

        /// <summary>
        /// The lower 64-bits of the eleventh general purpose register.
        /// </summary>
        public static Register R10 => new Register(nameof(R10), 0x0A, RegisterType.GeneralPurpose64Bit);

        /// <summary>
        /// The lower 64-bits of the twelfth general purpose register.
        /// </summary>
        public static Register R11 => new Register(nameof(R11), 0x0B, RegisterType.GeneralPurpose64Bit);

        /// <summary>
        /// The lower 64-bits of the thirteenth general purpose register.
        /// </summary>
        public static Register R12 => new Register(nameof(R12), 0x0C, RegisterType.GeneralPurpose64Bit);

        /// <summary>
        /// The lower 64-bits of the fourteenth general purpose register.
        /// </summary>
        public static Register R13 => new Register(nameof(R13), 0x0D, RegisterType.GeneralPurpose64Bit);

        /// <summary>
        /// The lower 64-bits of the fifteenth general purpose register.
        /// </summary>
        public static Register R14 => new Register(nameof(R14), 0x0E, RegisterType.GeneralPurpose64Bit);

        /// <summary>
        /// The lower 64-bits of the sixteenth general purpose register.
        /// </summary>
        public static Register R15 => new Register(nameof(R15), 0x0F, RegisterType.GeneralPurpose64Bit);

        #endregion

        #region Segment

        /// <summary>
        /// The extra segment register.
        /// </summary>
        public static Register ES => new Register(nameof(ES), 0x00, RegisterType.Segment);

        /// <summary>
        /// The code segment register.
        /// </summary>
        public static Register CS => new Register(nameof(CS), 0x01, RegisterType.Segment);

        /// <summary>
        /// The stack segment register.
        /// </summary>
        public static Register SS => new Register(nameof(SS), 0x02, RegisterType.Segment);

        /// <summary>
        /// The data segment register.
        /// </summary>
        public static Register DS => new Register(nameof(DS), 0x03, RegisterType.Segment);

        /// <summary>
        /// The second extra segment register.
        /// </summary>
        public static Register FS => new Register(nameof(FS), 0x04, RegisterType.Segment);

        /// <summary>
        /// The third extra segment register.
        /// </summary>
        public static Register GS => new Register(nameof(GS), 0x05, RegisterType.Segment);


        #endregion

        #region Control

        /// <summary>
        /// The first control register.
        /// </summary>
        public static Register CR0 => new Register(nameof(CR0), 0x00, RegisterType.Control);

        /// <summary>
        /// The second control register.
        /// </summary>
        public static Register CR1 => new Register(nameof(CR1), 0x01, RegisterType.Control);

        /// <summary>
        /// The third control register.
        /// </summary>
        public static Register CR2 => new Register(nameof(CR2), 0x02, RegisterType.Control);

        /// <summary>
        /// The fourth control register.
        /// </summary>
        public static Register CR3 => new Register(nameof(CR3), 0x03, RegisterType.Control);

        /// <summary>
        /// The fifth control register.
        /// </summary>
        public static Register CR4 => new Register(nameof(CR4), 0x04, RegisterType.Control);

        /// <summary>
        /// The sixth control register.
        /// </summary>
        public static Register CR5 => new Register(nameof(CR5), 0x05, RegisterType.Control);

        /// <summary>
        /// The seventh control register.
        /// </summary>
        public static Register CR6 => new Register(nameof(CR6), 0x06, RegisterType.Control);

        /// <summary>
        /// The eighth control register.
        /// </summary>
        public static Register CR7 => new Register(nameof(CR7), 0x07, RegisterType.Control);

        /// <summary>
        /// The nineth control register.
        /// </summary>
        public static Register CR8 => new Register(nameof(CR8), 0x08, RegisterType.Control);

        /// <summary>
        /// The tenth control register.
        /// </summary>
        public static Register CR9 => new Register(nameof(CR9), 0x09, RegisterType.Control);

        /// <summary>
        /// The eleventh control register.
        /// </summary>
        public static Register CR10 => new Register(nameof(CR10), 0x0A, RegisterType.Control);

        /// <summary>
        /// The twelfth control register.
        /// </summary>
        public static Register CR11 => new Register(nameof(CR11), 0x0B, RegisterType.Control);

        /// <summary>
        /// The thirteenth control register.
        /// </summary>
        public static Register CR12 => new Register(nameof(CR12), 0x0C, RegisterType.Control);

        /// <summary>
        /// The fourteenth control register.
        /// </summary>
        public static Register CR13 => new Register(nameof(CR13), 0x0D, RegisterType.Control);

        /// <summary>
        /// The fifteenth control register.
        /// </summary>
        public static Register CR14 => new Register(nameof(CR14), 0x0E, RegisterType.Control);

        /// <summary>
        /// The sixteenth control register.
        /// </summary>
        public static Register CR15 => new Register(nameof(CR15), 0x0F, RegisterType.Control);

        #endregion

        #region Debug

        /// <summary>
        /// The first debug register.
        /// </summary>
        public static Register DR0 => new Register(nameof(DR0), 0x00, RegisterType.Debug);

        /// <summary>
        /// The second debug register.
        /// </summary>
        public static Register DR1 => new Register(nameof(DR1), 0x01, RegisterType.Debug);

        /// <summary>
        /// The third debug register.
        /// </summary>
        public static Register DR2 => new Register(nameof(DR2), 0x02, RegisterType.Debug);

        /// <summary>
        /// The fourth debug register.
        /// </summary>
        public static Register DR3 => new Register(nameof(DR3), 0x03, RegisterType.Debug);

        /// <summary>
        /// The fifth debug register.
        /// </summary>
        public static Register DR4 => new Register(nameof(DR4), 0x04, RegisterType.Debug);

        /// <summary>
        /// The sixth debug register.
        /// </summary>
        public static Register DR5 => new Register(nameof(DR5), 0x05, RegisterType.Debug);

        /// <summary>
        /// The seventh debug register.
        /// </summary>
        public static Register DR6 => new Register(nameof(DR6), 0x06, RegisterType.Debug);

        /// <summary>
        /// The eighth debug register.
        /// </summary>
        public static Register DR7 => new Register(nameof(DR7), 0x07, RegisterType.Debug);

        /// <summary>
        /// The nineth debug register.
        /// </summary>
        public static Register DR8 => new Register(nameof(DR8), 0x08, RegisterType.Debug);

        /// <summary>
        /// The tenth debug register.
        /// </summary>
        public static Register DR9 => new Register(nameof(DR9), 0x09, RegisterType.Debug);

        /// <summary>
        /// The eleventh debug register.
        /// </summary>
        public static Register DR10 => new Register(nameof(DR10), 0x0A, RegisterType.Debug);

        /// <summary>
        /// The twelfth debug register.
        /// </summary>
        public static Register DR11 => new Register(nameof(DR11), 0x0B, RegisterType.Debug);

        /// <summary>
        /// The thirteenth debug register.
        /// </summary>
        public static Register DR12 => new Register(nameof(DR12), 0x0C, RegisterType.Debug);

        /// <summary>
        /// The fourteenth debug register.
        /// </summary>
        public static Register DR13 => new Register(nameof(DR13), 0x0D, RegisterType.Debug);

        /// <summary>
        /// The fifteenth debug register.
        /// </summary>
        public static Register DR14 => new Register(nameof(DR14), 0x0E, RegisterType.Debug);

        /// <summary>
        /// The sixteenth debug register.
        /// </summary>
        public static Register DR15 => new Register(nameof(DR15), 0x0F, RegisterType.Debug);

        #endregion

        #endregion
    }
}
