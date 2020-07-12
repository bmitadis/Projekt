using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjektHR.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjektHR.Models.Tests
{
    [TestClass()]
    public class PracownikTests
    {
        [TestMethod()]
        public void PeselValidationTestNeg()
        {
            if (Pracownik.WalidacjaPesel("12345678912"))
                Assert.Fail();
        }
        [TestMethod()]
        public void PeselValidationTestNeg2()
        {
            if (Pracownik.WalidacjaPesel("123456789123456789"))
                Assert.Fail();
        }
        [TestMethod()]
        public void PeselValidationTestNeg3()
        {
            if (Pracownik.WalidacjaPesel("teutyurtyutyuyiuuyi"))
                Assert.Fail();
        }
        [TestMethod()]
        public void PeselValidationTestNeg4()
        {
            if (Pracownik.WalidacjaPesel("123"))
                Assert.Fail();
        }
        [TestMethod()]
        public void PeselValidationTestNeg5()
        {
            if (Pracownik.WalidacjaPesel(""))
                Assert.Fail();
        }

        [TestMethod()]
        public void PeselValidationTestPos()
        {
            if (!Pracownik.WalidacjaPesel("88110512118"))
                Assert.Fail();
        }
        [TestMethod()]
        public void PeselValidationTestPos2()
        {
            if (!Pracownik.WalidacjaPesel("89062714364"))
                Assert.Fail();
        }

    }
}