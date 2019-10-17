using Cic.OpenOne.Common.DAO;
using System;
using NUnit.Framework;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.Common.UnitTest.DAO
{
    
    
    /// <summary>
    ///Dies ist eine Testklasse für "SampleDaoTest" und soll
    ///alle SampleDaoTest Komponententests enthalten.
    ///</summary>
    [TestFixture]
    public class SampleDaoTest
    {


      
        /// <summary>
        ///Ein Test für "sampleMethod"
        ///</summary>
        [Test]
        public void sampleMethodTest()
        {
            SampleDao target = new SampleDao();
            iSampleDto iParam = new iSampleDto();
            iParam.SampleText = "TEST";
            
            oSampleDto actual;
            
            try
            {
                actual = target.sampleMethod(iParam);

                Assert.AreEqual(iParam.SampleText, actual.SampleText);
                //Assert.Inconclusive("Überprüfen Sie die Richtigkeit dieser Testmethode.");
            }
            catch (NotImplementedException )
            {
                Assert.Fail("Expected");
            }
        }
    }
}
