using System;
using Cic.OpenOne.GateBANKNOW.Common.BO;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using NUnit.Framework;
using NUnit.Mocks;
using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.DAO;

namespace Cic.OpenOne.GateBANKNOW.Common.UnitTest.BOTest
{
    /// <summary>
    /// MwStBoTest class
    /// </summary>
    [TestFixture()]
    public class MwStBoTest
    {
        /// <summary>
        /// Mehrwertsteuer BO
        /// </summary>
        MwStBo bo;

        /// <summary>
        /// Mehrwertsteuer DAO Mock
        /// </summary>
        DynamicMock MwstDaoMock;

        DateTime Date = DateTime.Now;
        /// <summary>
        /// Setup of the test
        /// </summary>
        [SetUp]
        public void MwStBoTestInit()
        {
            MwstDaoMock = new DynamicMock(typeof(IMwStDao));
            bo = new MwStBo((IMwStDao)MwstDaoMock.MockInstance);
        }

        /// <summary>
        /// Auslesen der Mehrwertsteuer
        /// </summary>
        [Test]
        public void getMehrwertSteuerTest1()
        {
            double MwSt = 8;

            MwstDaoMock.ExpectAndReturn("getMehrwertSteuer", MwSt, 3, Date);
            double mwStRet = bo.getMehrwertSteuer(1, 3, Date);

            Assert.AreEqual(8, mwStRet);
        }

        /// <summary>
        /// Auslesen der Mehrwertsteuer
        /// </summary>
        [Test]
        public void getMehrwertSteuerTest2()
        {
            double MwSt = 9;

            MwstDaoMock.ExpectAndReturn("getGlobalUst", MwSt, 1, Date);
            double mwStRet = bo.getMehrwertSteuer(1, 3, Date);

            Assert.AreEqual(9, mwStRet);
        }
    }
}