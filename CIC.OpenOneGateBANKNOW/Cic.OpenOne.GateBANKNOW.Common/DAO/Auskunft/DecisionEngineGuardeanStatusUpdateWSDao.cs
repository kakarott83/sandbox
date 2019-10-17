namespace Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft
{
    using System;
    using System.Net;
    using System.Reflection;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.Text;

    using GuardeanServiceReference;

    using GuardeanStatusUpdateServiceReference;

    using OpenOne.Common.Util.Logging;

    public class DecisionEngineGuardeanStatusUpdateWSDao : IDecisionEngineGuardeanStatusUpdateWSDao
    {
        private Wkf_Status_UpdatePortTypeClient client;
        private Cic.OpenOne.Common.DTO.SoapXMLDto soapXMLDto = new Cic.OpenOne.Common.DTO.SoapXMLDto();
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public DecisionEngineGuardeanStatusUpdateWSDao()
        {
        }

        private void lazyInit()
        {
            if (client != null) return;
            try
            {
                client = new Wkf_Status_UpdatePortTypeClient();
            }
            catch (Exception e)
            {
                _log.Error("Error connecting to Guardean Decision Status Update Engine: " + e.Message, e);
            }
        }
        /// <summary>
        /// Sets the connection credentials
        /// </summary>
        /// <param name="cred"></param>
        public void setCredentials(NetworkCredential cred)
        {
            lazyInit();
            client.ClientCredentials.UserName.UserName = cred.UserName;
            client.ClientCredentials.UserName.Password = cred.Password;

        }

        public Cic.OpenOne.GateBANKNOW.Common.GuardeanStatusUpdateServiceReference.executeResponse execute(Cic.OpenOne.GateBANKNOW.Common.GuardeanStatusUpdateServiceReference.executeRequest executeRequest)
        {
            try
            {
                lazyInit();

                client.Endpoint.Behaviors.Add(new Cic.OpenOne.Common.Util.Behaviour.SoapLoggingAuskunft(ref soapXMLDto));
                using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
                {
                    var httpRequestProperty = new HttpRequestMessageProperty();
                    httpRequestProperty.Headers[System.Net.HttpRequestHeader.Authorization] = "Basic " +
                                                                                              Convert.ToBase64String(Encoding.ASCII.GetBytes(
                                                                                                  client.ClientCredentials.UserName.UserName + ":" +
                                                                                                  client.ClientCredentials.UserName.Password));

                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] =
                        httpRequestProperty;

                    _log.Info("DecisionEngineGuardeanStatusUpdate Webserviceaufruf gestartet.");
                    DateTime startTime = DateTime.Now;
                    var response = client.execute(executeRequest);
                    _log.Info("DecisionEngineGuardeanStatusUpdate Webserviceaufruf Dauer : " + (TimeSpan)(DateTime.Now - startTime));
                    
                    return response;
                }
            }
            catch (Exception ex)
            {
                _log.Error("Fehler im DecisionEngine Status Update Webserviceaufruf. ExceptionMessage: " + ex.Message);
                throw ex;
            }
        }

        public OpenOne.Common.DTO.SoapXMLDto getSoapXMLDto()
        {
            return soapXMLDto;
        }

        public void setSoapXMLDto(OpenOne.Common.DTO.SoapXMLDto soapXMLDto)
        {
            this.soapXMLDto = soapXMLDto;
        }
    }
}