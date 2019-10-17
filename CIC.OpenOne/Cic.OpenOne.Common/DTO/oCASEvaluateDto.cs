using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.DTO;

namespace Cic.OpenOne.Common.DTO
{
    public class oCASEvaluateDto : oBaseDto
    {
        public CASEvaluateResult[] evaluationResults {get;set;}
    }
}