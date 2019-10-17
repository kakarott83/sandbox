using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.TestUI.DTOS;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.Attribute4UI;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.EurotaxRef;
using Cic.OpenOne.GateBANKNOW.Common.DTO;

namespace Cic.OpenOne.GateBANKNOW.TestUI.BO
{
    class TestBO
    {

        public List<TestOutDto> doAuskunft(TestDto inDto)
        {  TestOutDto test = new TestOutDto();
        test.CandidateList = new List<DVAddressMatchDto>();
            DVAddressMatchDto m = new DVAddressMatchDto();
            test.CandidateList.Add(m);
            List<TestOutDto> testlist = new List<TestOutDto>();
            testlist.Add(test);
            return testlist;
        }
        public TestDto doInput(TestDto inDto)
        {
         
           
            return inDto;
        }

    }
}
