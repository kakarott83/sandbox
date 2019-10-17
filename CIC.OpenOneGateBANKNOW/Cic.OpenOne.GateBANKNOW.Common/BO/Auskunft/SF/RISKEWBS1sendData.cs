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
    class RISKEWBS1sendData : AbstractAuskunftBo<RISKEWBS1DBDto, AuskunftDto>
    {
        /// <summary>
        /// Gets AuskunftDto by SysAuskunft and calls Eurotax Webservice GetForecast
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        public override AuskunftDto doAuskunft(long sysAuskunft)
        {
            return AuskunftBoFactory.CreateDefaultRISKEWBS1Bo().sendData(sysAuskunft);
        }


        /// <summary>
        /// do Auskunft
        /// </summary>
        /// <param name="area">Area Name</param>
        /// <param name="sysId">Sys ID</param>
        /// <returns>Auskunft Dto</returns>
        public override AuskunftDto doAuskunft(string area, long sysId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// do Auskunft
        /// </summary>
        /// <param name="inDto">ZEK Input Dto</param>
        /// <returns>ZEK Output Dto</returns>
        public override AuskunftDto doAuskunft(RISKEWBS1DBDto inDto)
        {
            //return AuskunftBoFactory.CreateDefaultZekBo().closeBardarlehen(inDto);
            throw new NotImplementedException();
        }
    }
}
