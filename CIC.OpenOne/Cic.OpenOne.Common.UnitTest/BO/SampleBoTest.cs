using Cic.OpenOne.Common.DAO;
using System;
using NUnit.Framework;
using Cic.OpenOne.Common.BO;
using NMock2;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.Common.UnitTest.BO
{
    
    
    /// <summary>
    ///Dies ist eine Testklasse für "SampleDaoTest" und soll
    ///alle SampleDaoTest Komponententests enthalten.
    ///</summary>
    [TestFixture]
    public class SampleBoTest
    {


      
        /// <summary>
        ///Ein Test für "sampleMethod"
        ///</summary>
        [Test]
        public void sampleMethodTest()
        {
            Mockery mocks = new Mockery();

            
            ISampleDao dao = mocks.NewMock<ISampleDao>();

            Expect.Once.On(dao).Method("sampleMethod").WithAnyArguments().Will(Throw.Exception(new NotImplementedException())); 
            //Expect.Once.On(dao).Method("sampleMethod").WithAnyArguments().Will(  Return.Value("meinTest"));

            SampleBo target = new SampleBo(dao);

            iSampleDto iParam = new iSampleDto();
            iParam.SampleText = "TEST";
          
            oSampleDto actual;
            
            try
            {
                actual = target.sampleMethod(iParam);
                Assert.Fail("Expected Exception");
                
            }
            catch (NotImplementedException )
            {
                //Expected to throw exception
            }
        }
    }
}
