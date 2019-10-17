using Cic.OpenOne.Common.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Service.DTO
{
    public class getAggregationDto : oBaseDto
    {
        public Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SHS.W003.executeResponse response { get; set; }
    }
}