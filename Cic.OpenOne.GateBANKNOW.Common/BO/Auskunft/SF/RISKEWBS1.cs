using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF
{
    /// <summary>
    /// S1 RISKEWBS1 ServiceFacade
    /// </summary>
    class RISKEWBS1 : AbstractAuskunftBo<RISKEWBS1InDto, AuskunftDto>
    {
        /// <summary>
        /// Gets AuskunftDto by SysAuskunft and calls Eurotax Webservice GetForecast
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        public override AuskunftDto doAuskunft(long sysAuskunft)
        {
            return AuskunftBoFactory.CreateDefaultRISKEWBS1Bo.RISKEWBS1(sysAuskunft);
        }
    }
}
