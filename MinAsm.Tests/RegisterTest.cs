using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MinAsm;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MinAsm.Tests
{
    [TestClass]
    public class RegisterTest
    {
        [TestMethod]
        public void TestRegisterFullValues()
        {
            Assert.AreEqual(Register.EAX.Full, 8320);
        }
    }
}
