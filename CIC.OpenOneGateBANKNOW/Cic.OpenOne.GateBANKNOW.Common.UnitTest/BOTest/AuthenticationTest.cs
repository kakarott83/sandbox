using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Mocks;
using NUnit.Framework;
using Cic.OpenOne.GateBANKNOW.Common.BO;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using AutoMapper;
using Cic.OpenOne.Common.Util.Security;

namespace Cic.OpenOne.GateBANKNOW.Common.UnitTest.BOTest
{
    /// <summary>
    /// Test for Authentication
    /// </summary>
    [TestFixture()]
    public class AuthenticationTest
    {
     

        /// <summary>
        /// Blackboxtest 
        /// ToDo:
        /// Ausgeschaltet, weil autenticate einen DB Zugriff im BO hat. 
        /// Was effektiv einen automatisierten Unit Test mit Mocks auf dem Hudson unmöglich macht.
        /// </summary>
        [Test]
        public void loginTest()
        {
            /*
            oExtendedUserDto rval = new oExtendedUserDto();
            AuthenticationBo authBo = new AuthenticationBo();
            authBo.authenticate("AS","nelenele", MembershipProvider.USER_TYPE_PUSER, ref rval);

            Assert.IsNotEmpty(rval.rolemap);
             */
        }
    }
}
