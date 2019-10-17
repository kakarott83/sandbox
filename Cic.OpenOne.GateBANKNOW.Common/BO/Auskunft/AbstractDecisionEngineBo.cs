using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft
{
    /// <summary>
    /// Abstract Class: Decision Engine Business Object
    /// </summary>
    public abstract class AbstractDecisionEngineBo : IDecisionEngineBo
    {
        /// <summary>
        /// Decision Engine Web Service Data Access Object
        /// </summary>
        protected IDecisionEngineWSDao dewsdao;

        /// <summary>
        /// Decision Engine DB Data Access Object
        /// </summary>
        protected IDecisionEngineDBDao dedao;

        /// <summary>
        /// Information Data Access Object
        /// </summary>
        protected IAuskunftDao auskunftdao;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="dewsdao">Decision Engine Web Service Data Access Object</param>
        /// <param name="dedao">Decision Engine DB Data Access Object</param>
        /// <param name="auskunftdao">Information Data Access Object</param>
        public AbstractDecisionEngineBo(IDecisionEngineWSDao dewsdao, IDecisionEngineDBDao dedao, IAuskunftDao auskunftdao)
        {
            this.dewsdao = dewsdao;
            this.dedao = dedao;
            this.auskunftdao = auskunftdao;
        }

        /// <summary>
        /// Execute Decision Engine
        /// </summary>
        /// <param name="inDto">Input Data structure</param>
        /// <returns>Output Data Structure</returns>
        public abstract AuskunftDto execute(DecisionEngineInDto inDto);
        
        /// <summary>
        /// Execute Decision Engine
        /// </summary>
        /// <param name="sysAuskunft">Information ID</param>
        /// <returns>Output Data Structure</returns>
        public abstract AuskunftDto execute(long sysAuskunft);

        /// <summary>
        /// executeWithOutSaveexecute
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        public abstract AuskunftDto executeSimulation(DecisionEngineInDto inDto);
    }
}
