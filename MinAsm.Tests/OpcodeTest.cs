using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MinAsm.Encoding;

namespace MinAsm.Encoding.Tests
{
    /// <summary>Этот класс содержит параметризованные модульные тесты для Opcode</summary>
    [TestClass]
    [PexClass(typeof(Opcode))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    public partial class OpcodeTest
    {
        /// <summary>Тестовая заглушка для .ctor(Byte[])</summary>
        [PexMethod]
        public Opcode ConstructorTest(byte[] bytes)
        {
            Opcode target = new Opcode(bytes);
            return target;
            // TODO: добавление проверочных утверждений в метод OpcodeTest.ConstructorTest(Byte[])
        }
    }
}
