using System;
using MinAsm.Operands;

namespace MinAsm.Encoding
{
    /// <summary>
    /// Describes a single operand.
    /// </summary>
    public class OperandDescriptor : IEquatable<OperandDescriptor>
    {
        /// <summary>
        /// Gets the operand type of the operand <see cref="OperandType"/>.
        /// </summary>
        public OperandType Type { get; }

        /// <summary>
        /// Gets the register type of the operand <see cref="MinAsm.RegisterType"/>.
        /// </summary>
        public RegisterType RegisterType { get; }

        /// <summary>
        /// Gets the size of the operand <see cref="DataSize"/>.
        /// </summary>
        public DataSize Size { get; }

        /// <summary>
        /// Gets the encoding of the operand <see cref="OperandEncoding"/>.
        /// </summary>
        public OperandEncoding Encoding { get; }

        /// <summary>
        /// Gets the fixed register of the operand.
        /// </summary>
        public Register FixedRegister { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperandDescriptor"/> class.
        /// </summary>
        /// <param name="fixedRegister">The fixed <see cref="Register"/> value of the operand.</param>
        public OperandDescriptor(Register fixedRegister)
            : this(OperandType.RegisterOperand, OperandEncoding.Default, fixedRegister.Type, fixedRegister.Size, fixedRegister)
        {
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="OperandDescriptor"/> class.
        /// </summary>
        /// <param name="type">The type of operand.</param>
        /// <param name="encoding">The operand encoding.</param>
        /// <param name="size">The size of the operand.</param>
        public OperandDescriptor(OperandType type, OperandEncoding encoding, DataSize size)
            : this(type, encoding, RegisterType.None, size, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperandDescriptor"/> class.
        /// </summary>
        /// <param name="type">The type of operand.</param>
        /// <param name="encoding">The operand encoding.</param>
        /// <param name="regType">A bitwise combination of valid types of the register.</param>
        public OperandDescriptor(OperandType type, OperandEncoding encoding, RegisterType regType)
            : this(type, encoding, regType, DataSize.None, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperandDescriptor"/> class.
        /// </summary>
        /// <param name="type">The type of operand.</param>
        /// <param name="encoding">The operand encoding.</param>
        /// <param name="regType"></param>
        /// <param name="size">The size of the operand.</param>
        public OperandDescriptor(OperandType type, OperandEncoding encoding, RegisterType regType, DataSize size)
            : this(type, encoding, regType, size, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OperandDescriptor"/> class.
        /// </summary>
        /// <param name="type">The type of operand.</param>
        /// <param name="encoding">The operand encoding.</param>
        /// <param name="regType">A bitwise combination of valid types of the register.</param>
        /// <param name="size">The size of the operand.</param>
        /// <param name="fixedReg">The fixed <see cref="Register"/> value of the operand.</param>
        public OperandDescriptor(OperandType type, OperandEncoding encoding, RegisterType regType, DataSize size, Register fixedReg)
        {
            Type          = type;
            RegisterType  = regType;
            Encoding      = encoding;
            FixedRegister = fixedReg;
            Size          = size;
        }

        public bool Equals(OperandDescriptor other)
        {
            if (other == null)
                return false;

            throw new NotImplementedException();
        }
    }
}