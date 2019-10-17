namespace Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft
{
    using System.Net;

    using Cic.OpenOne.GateBANKNOW.Common.SchufaSiml2AuskunfteiWorkflow;

    public interface ISchufaWSDao
    {

        /// <summary>
        /// execute Schufa Service via Web Service Access
        /// </summary>
        /// <param name="executeRequest">Request Data</param>
        /// <returns>response Data</returns>
        executeResponse execute(executeRequest input);
        
        /// <summary>
        /// Sets the connection credentials
        /// </summary>
        /// <param name="cred"></param>
        void setCredentials(NetworkCredential cred);

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
