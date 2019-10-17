using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft
{
    /// <summary>
    /// Decision Engine Busines Object Interface
    /// </summary>
    public interface IDecisionEngineBo
    {
        /// <summary>
        /// Execute Decision Inquiry
        /// </summary>
        /// <param name="inDto">Input Data</param>
        /// <returns>Output Data</returns>
        AuskunftDto execute(DecisionEngineInDto inDto);

        /// <summary>
        /// Execute Information Inquiry
        /// </summary>
        /// <param name="sysAuskunft">Information type requested</param>
        /// <returns>Information Data</returns>
        AuskunftDto execute(long sysAuskunft);


        /// <summary>
        /// executeWithOutSaveexecute
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        AuskunftDto executeSimulation(DecisionEngineInDto inDto);
    }
}
