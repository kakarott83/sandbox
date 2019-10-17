namespace Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft
{
    using System.Net;

    public interface IDecisionEngineGuardeanStatusUpdateWSDao
    {
        /// <summary>
        /// Sets the connection credentials
        /// </summary>
        /// <param name="cred"></param>
        void setCredentials(NetworkCredential cred);

        Cic.OpenOne.GateBANKNOW.Common.GuardeanStatusUpdateServiceReference.executeResponse execute(Cic.OpenOne.GateBANKNOW.Common.GuardeanStatusUpdateServiceReference.executeRequest executeRequest);

        OpenOne.Common.DTO.SoapXMLDto getSoapXMLDto();

        void setSoapXMLDto(OpenOne.Common.DTO.SoapXMLDto soapXMLDto);
    }
}