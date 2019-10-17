using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Cic.OpenOne.GateBANKNOW.Common.BO;
using Cic.OpenOne.Common.BO;

namespace Cic.OpenOne.GateBANKNOW.Common.UnitTest.BOTest
{
    /// <summary>
    /// Unit Test des Rundings BO
    /// </summary>
    [TestFixture()]
    public class RoundingTest
    {
        IRounding bo;

        /// <summary>
        /// Setup des Unit Tests
        /// </summary>
        [SetUp]
        public void RoundingTestSetup()
        {
            bo = RoundingFactory.createRounding();
        }

        /// <summary>
        /// Rundungstest, aufrunden
        /// </summary>
        [Test]
        public void RoundCHFTest1()
        {
            double returnVal = bo.RoundCHF(2.237);
            Assert.AreEqual(2.25, returnVal);
        }
        /// <summary>
        /// Rundungstest, abrunden
        /// </summary>
        [Test]
        public void RoundCHFTest2()
        {
            double returnVal = bo.RoundCHF(2.222);
            Assert.AreEqual(2.20, returnVal);
        }

        /// <summary>
        /// Rundungstest für durchgeschleiften Runder, aufrunden
        /// </summary>
        [Test]
        public void RoundTest1()
        {
            double returnVal = bo.Round(2.234567, 4);
            Assert.AreEqual(2.2346, returnVal);
        }

        /// <summary>
        /// Rundungstest für durchgeschleiften Runder, abrunden
        /// </summary>
        [Test]
        public void RoundTest2()
        {
            double returnVal = bo.Round(2.23452, 4);
            Assert.AreEqual(2.2345, returnVal);
        }

        /// <summary>
        /// Nettowert errechnen
        /// </summary>
        [Test]
        public void getNetValueTest()
        {
            double returnVal = bo.getNetValue(2000.0, 7.5);
            Assert.AreEqual(1860.4651162790697, returnVal);
        }

        /// <summary>
        /// Nettowert errechnen
        /// </summary>
        [Test]
        public void getTaxValueTest()
        {
            double returnVal = bo.getTaxValue(1860.4651162790697, 7.5);
            Assert.AreEqual(139.53488372093023, returnVal);
        }

        /// <summary>
        /// Nettowert errechnen
        /// </summary>
        [Test]
        public void getGrossValueTest()
        {
            double returnVal = bo.getGrossValue(1860.4651162790697, 7.5);
            Assert.AreEqual(2000, returnVal);
        }
    }
}
