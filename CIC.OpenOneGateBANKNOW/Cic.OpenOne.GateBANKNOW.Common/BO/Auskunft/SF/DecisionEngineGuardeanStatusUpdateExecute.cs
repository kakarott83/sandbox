using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF
{
    using DTO.Auskunft;

    class DecisionEngineGuardeanStatusUpdateExecute : AbstractAuskunftBo<DecisionEngineGuardeanInDto, AuskunftDto>
    {
        /// <summary>
        /// Gets input values by a filled AUSKUNFT entity and calls DecisionEngine Webservice
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        public override AuskunftDto doAuskunft(long sysAuskunft)
        {
            return AuskunftBoFactory.CreateDefaultDecisionEngineGuardeanBo().executeStatusUpdate(sysAuskunft);
        }
        /// <summary>
        /// Gets input values from database by Area and sysId and calls DecisionEngine Webservice
        /// </summary>
        /// <param name="area"></param>
        /// <param name="sysId"></param>
        /// <returns></returns>
        public override AuskunftDto doAuskunft(string area, long sysId)
        {
            throw new NotImplementedException();
        }
        /// <summary>
        /// Gets input values by inDto and calls DecisionEngine Webservice 
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        public override AuskunftDto doAuskunft(DecisionEngineGuardeanInDto inDto)
        {
            return AuskunftBoFactory.CreateDefaultDecisionEngineGuardeanBo().executeStatusUpdate(inDto);
        }
    }
}
