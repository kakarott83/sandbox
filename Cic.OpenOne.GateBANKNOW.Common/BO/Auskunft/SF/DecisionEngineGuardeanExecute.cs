using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SF
{
    using SHS.W006;

    public class DecisionEngineGuardeanExecute : AbstractAuskunftBo<DecisionEngineGuardeanInDto, AuskunftDto>
    {
        /// <summary>
        /// Gets input values by a filled AUSKUNFT entity and calls DecisionEngine Webservice
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        public override AuskunftDto doAuskunft(long sysAuskunft)
        {
            return AuskunftBoFactory.CreateDefaultDecisionEngineGuardeanBo().execute(sysAuskunft);
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
            return AuskunftBoFactory.CreateDefaultDecisionEngineGuardeanBo().execute(inDto);
        }

        /// <summary>
        /// Updates the Auskunft with the Guardean Result
        /// </summary>
        /// <param name="resp"></param>
        public void setCreditDecisionResult(Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SHS.W002.executeRequest resp)
        {
            AuskunftBoFactory.CreateDefaultDecisionEngineGuardeanBo().setCreditDecisionResult(resp);
        }

        /// <summary>
        /// Deliver Aggregation-Information for the Guardean decision process 
        /// INT3
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SHS.W003.executeResponse getAggregation(Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SHS.W003.executeRequest req)
        {
            return AuskunftBoFactory.CreateDefaultDecisionEngineGuardeanBo().getAggregation(req);
        }

        public executeResponse getLiabilityChain(executeRequest req)
        {
            return AuskunftBoFactory.CreateDefaultDecisionEngineGuardeanBo().getLiabilityChain(req);
        }

        public SHS.W007.executeResponse setCustomerCheckResult(SHS.W007.executeRequest req)
        {
            return AuskunftBoFactory.CreateDefaultDecisionEngineGuardeanBo().setCustomerCheckResult(req);
        }
    }
}