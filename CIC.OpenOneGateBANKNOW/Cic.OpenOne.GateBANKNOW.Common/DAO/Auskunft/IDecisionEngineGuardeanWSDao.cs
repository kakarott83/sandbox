using Cic.OpenOne.Common.Util.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.ServiceModel.Security;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft
{
    public interface IDecisionEngineGuardeanWSDao
    {
        /// <summary>
        /// execute Decision Engine via Web Service Access
        /// </summary>
        /// <param name="executeRequest">Request Data</param>
        /// <returns>response Data</returns>
        Cic.OpenOne.GateBANKNOW.Common.GuardeanServiceReference.executeResponse execute(Cic.OpenOne.GateBANKNOW.Common.GuardeanServiceReference.executeRequest executeRequest);

        /// <summary>
        /// getSoapXMLDto
        /// </summary>
        Cic.OpenOne.Common.DTO.SoapXMLDto getSoapXMLDto();

        /// <summary>
        /// setSoapXMLDto
        /// </summary>
        /// <param name="soapXMLDto"></param>
        void setSoapXMLDto(Cic.OpenOne.Common.DTO.SoapXMLDto soapXMLDto);

        /// <summary>
        /// Sets the connection credentials
        /// </summary>
        /// <param name="cred"></param>
        void setCredentials(NetworkCredential cred);
    }
}