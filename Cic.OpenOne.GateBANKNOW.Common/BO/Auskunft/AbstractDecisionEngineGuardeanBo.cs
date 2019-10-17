using Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft
{
    using SHS.W006;

    public abstract class AbstractDecisionEngineGuardeanBo : IDecisionEngineGuardeanBo
    {
        /// <summary>
        /// Decision Engine Web Service Data Access Object
        /// </summary>
        protected readonly IDecisionEngineGuardeanWSDao dewsdao;

        /// <summary>
        /// Decision Engine Status Update Web Service Data Access Object
        /// </summary>
        protected readonly DecisionEngineGuardeanStatusUpdateWSDao desuwsdao;

        /// <summary>
        /// Decision Engine DB Data Access Object
        /// </summary>
        protected readonly IDecisionEngineGuardeanDBDao dedao;

        /// <summary>
        /// Information Data Access Object
        /// </summary>
        protected readonly IAuskunftDao auskunftdao;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dewsdao">Decision Engine Web Service Data Access Object</param>
        /// <param name="desuwsdao">Decision Engine Status Update Web Service Data Access Object</param>
        /// <param name="dedao">Decision Engine DB Data Access Object</param>
        /// <param name="auskunftdao">Information Data Access Object</param>
        public AbstractDecisionEngineGuardeanBo(IDecisionEngineGuardeanWSDao dewsdao, DecisionEngineGuardeanStatusUpdateWSDao desuwsdao, IDecisionEngineGuardeanDBDao dedao, IAuskunftDao auskunftdao)
        {
            this.dewsdao = dewsdao;
            this.desuwsdao = desuwsdao;
            this.dedao = dedao;
            this.auskunftdao = auskunftdao;
        }

        /// <summary>
        /// Execute Decision Engine
        /// </summary>
        /// <param name="inDto">Input Data structure</param>
        /// <returns>Output Data Structure</returns>
        public abstract AuskunftDto execute(DecisionEngineGuardeanInDto inDto);
        
        /// <summary>
        /// Execute Decision Engine
        /// </summary>
        /// <param name="sysAuskunft">Information ID</param>
        /// <returns>Output Data Structure</returns>
        public abstract AuskunftDto execute(long sysAuskunft);

        /// <summary>
        /// Executes a status update to an application
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        public abstract AuskunftDto executeStatusUpdate(DecisionEngineGuardeanInDto inDto);


        /// <summary>
        /// Returns the Liability Chain for the Guardean decision process
        /// INT 6
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public abstract executeResponse getLiabilityChain(executeRequest req);

        /// <summary>
        /// Sets the customer check result from the Guardean decision process
        /// INT 7
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public abstract SHS.W007.executeResponse setCustomerCheckResult(SHS.W007.executeRequest req);

        /// <summary>
        /// Executes a status update to an application
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        public abstract AuskunftDto executeStatusUpdate(long sysAuskunft);

        /// <summary>
        /// Updates the Auskunft with the Guardean Result
        /// </summary>
        /// <param name="resp"></param>
        public abstract void setCreditDecisionResult(Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SHS.W002.executeRequest resp);


        /// <summary>
        /// Deliver Aggregation-Information for the Guardean decision process 
        /// INT3
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public abstract Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SHS.W003.executeResponse getAggregation(Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.SHS.W003.executeRequest req);
    }
}