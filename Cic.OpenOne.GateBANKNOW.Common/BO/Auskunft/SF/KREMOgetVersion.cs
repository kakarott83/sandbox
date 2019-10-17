using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF
{
    /// <summary>
    /// Kremo getVersion ServiceFacade
    /// </summary>
    public class KREMOgetVersion : AbstractAuskunftBo<KREMOInDto, KREMOOutDto>
    {
        /// <summary>
        /// overwritten AbstractAuskunftBo method to get input values by InDto and call KREMOWebservice CallKREMOgetVersion() 
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        public override KREMOOutDto doAuskunft(KREMOInDto inDto)
        {
            return AuskunftBoFactory.CreateDefaultKREMOBo().getVersion(inDto);
        }

        /// <summary>
        /// overwritten IAuskunftBo method to get input values by a filled AUSKUNFT set and call KREMOWebservice CallKREMOgetVersion() 
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        public override AuskunftDto doAuskunft(long sysAuskunft)
        {
            return AuskunftBoFactory.CreateDefaultKREMOBo().getVersion(sysAuskunft);
        }

        /// <summary>
        /// overwritten IAuskunftBo method to get input values from database by Area and SYSId and call KREMOWebservice CallKREMOgetVersion() 
        /// </summary>
        /// <param name="area"></param>
        /// <param name="sysId"></param>
        /// <returns></returns>
        public override AuskunftDto doAuskunft(string area, long sysId)
        {
            throw new NotImplementedException();
        }
    }
}
