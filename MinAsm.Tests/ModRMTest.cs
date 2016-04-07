// <copyright file="ModRMTest.cs">Copyright ©  2016</copyright>
using System;
using Microsoft.Pex.Framework;
using Microsoft.Pex.Framework.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MinAsm.Encoding.Tests
{
    /// <summary>Этот класс содержит параметризованные модульные тесты для ModRM</summary>
    [PexClass(typeof(ModRM))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(InvalidOperationException))]
    [PexAllowedExceptionFromTypeUnderTest(typeof(ArgumentException), AcceptExceptionSubtypes = true)]
    [TestClass]
    public partial class ModRMTest
    {
        [TestMethod]
        public void TypeCastByteToModRmMethod()
        {
            Assert.AreEqual(new ModRM(0x55), (ModRM)0x55);
        }

        [TestMethod]
        public void TypeCastModRMToByteMethod()
        {
            Assert.AreEqual((byte)new ModRM(0x55), 0x55);
        }

        [TestMethod]
        public void PropertyRMTestMethod()
        {
            Assert.AreEqual<byte>(new ModRM(0x55).RM, 5);
        }

        [TestMethod]
        public void PropertyModTestMethod()
        {
            Assert.AreEqual<byte>((byte)new ModRM(0x55).Mod, 1);
        }

        [TestMethod]
        public void PropertyRegTestMethod()
        {
            Assert.AreEqual<byte>(new ModRM(0x55).Reg, 2);
        }
    }
}
