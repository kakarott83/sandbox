using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF
{
    /// <summary>
    /// DecisionEngine ServiceFacade, implements AbstractAuskunftBo 
    /// </summary>
    public class DecisionEngineExecute : AbstractAuskunftBo<DecisionEngineInDto, AuskunftDto>
    {
        /// <summary>
        /// Gets input values by a filled AUSKUNFT entity and calls DecisionEngine Webservice
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        public override AuskunftDto doAuskunft(long sysAuskunft)
        {
            return AuskunftBoFactory.CreateDefaultDecisionEngineBo().execute(sysAuskunft);
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
        public override AuskunftDto doAuskunft(DecisionEngineInDto inDto)
        {
            return AuskunftBoFactory.CreateDefaultDecisionEngineBo().execute(inDto);
        }
    }
}
