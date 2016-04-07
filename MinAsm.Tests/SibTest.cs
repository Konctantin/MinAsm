using System;
using MinAsm;
using MinAsm.Encoding;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MinAsm.Encoding.Tests
{
    [TestClass]
    public class SibTest
    {
        [TestMethod]
        public void TypeCastByteToSibMethod()
        {
            Assert.AreEqual(new Sib(0x55), (Sib)0x55);
        }

        [TestMethod]
        public void TypeCastSibToByteMethod()
        {
            Assert.AreEqual((byte)new Sib(0x55), 0x55);
        }

        [TestMethod]
        public void PropertyBaseTestMethod()
        {
            Assert.AreEqual<byte>(new Sib(0x55).Base, 5);
        }

        [TestMethod]
        public void PropertyScaleTestMethod()
        {
            Assert.AreEqual<byte>((byte)new Sib(0x55).Scale, 1);
        }

        [TestMethod]
        public void PropertyIndexTestMethod()
        {
            Assert.AreEqual<byte>(new Sib(0x55).Index, 2);
        }
    }
}
