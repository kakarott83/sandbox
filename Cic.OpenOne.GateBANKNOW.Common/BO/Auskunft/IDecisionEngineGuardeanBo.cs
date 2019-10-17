using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft
{
    using SHS.W006;

    public interface IDecisionEngineGuardeanBo
    {
        /// <summary>
        /// Execute Decision Inquiry
        /// </summary>
        /// <param name="inDto">Input Data</param>
        /// <returns>Output Data</returns>
        AuskunftDto execute(DecisionEngineGuardeanInDto inDto);

        /// <summary>
        /// Execute Information Inquiry
        /// </summary>
        /// <param name="sysAuskunft">Information type requested</param>
        /// <returns>Information Data</returns>
        AuskunftDto execute(long sysAuskunft);

        /// <summary>
        /// Updates the Auskunft with the Guardean Result
        /// INT2
        /// </summary>
        /// <param name="resp"></param>
        void setCreditDecisionResult(Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SHS.W002.executeRequest resp);

        /// <summary>
        /// Deliver Aggregation-Information for the Guardean decision process 
        /// INT3
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SHS.W003.executeResponse getAggregation(Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SHS.W003.executeRequest req);

        /// <summary>
        /// Returns the Liability Chain for the Guardean decision process
        /// INT 6
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        executeResponse getLiabilityChain(executeRequest req);

        /// <summary>
        /// Sets the customer check result from the Guardean decision process
        /// INT 7
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        SHS.W007.executeResponse setCustomerCheckResult(SHS.W007.executeRequest req);

        /// <summary>
        /// Executes a status update to an application
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        AuskunftDto executeStatusUpdate(long sysAuskunft);

        /// <summary>
        /// Executes a status update to an application
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        AuskunftDto executeStatusUpdate(DecisionEngineGuardeanInDto inDto);


    }
}