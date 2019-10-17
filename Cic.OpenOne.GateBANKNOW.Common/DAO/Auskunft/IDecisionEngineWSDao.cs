using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.Common.Util.Logging;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft
{
    /// <summary>
    /// Decision Engine Web Service Data Access Object
    /// </summary>
    public interface IDecisionEngineWSDao
    {
        /// <summary>
        /// execute Decision Engine via Web Service Access
        /// </summary>
        /// <param name="request">Request Data</param>
        /// <param name="_log">Logger</param>
        /// <returns>response Data</returns>
        DAO.Auskunft.DecisionEngineRef.StrategyOneResponse execute(string request, ILog _log);
       

        /// <summary>
        /// getSoapXMLDto
        /// </summary>
        Cic.OpenOne.Common.DTO.SoapXMLDto getSoapXMLDto();

        /// <summary>
        /// setSoapXMLDto
        /// </summary>
        /// <param name="soapXMLDto"></param>
        void setSoapXMLDto(Cic.OpenOne.Common.DTO.SoapXMLDto soapXMLDto);
    }
}
