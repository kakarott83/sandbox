namespace Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft
{
    using System;
    using System.Net;
    using System.Reflection;
    using System.ServiceModel;
    using System.ServiceModel.Channels;
    using System.Text;

    using Cic.OpenOne.Common.DTO;
    using Cic.OpenOne.Common.Util.Behaviour;
    using Cic.OpenOne.Common.Util.Logging;
    using Cic.OpenOne.GateBANKNOW.Common.SchufaSiml2AuskunfteiWorkflow;

    public class SchufaWSDao : ISchufaWSDao
    {
        private SchufaSiml2AuskunfteiWorkflowPortTypeClient client; 
        private SoapXMLDto soapXMLDto = new SoapXMLDto()
        {
            logDumpFlag = true
        };

        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public SchufaWSDao()
        {
           
        }
        private void lazyInit()
        {
            if (client != null) return;
            try
            {
                client = new SchufaSiml2AuskunfteiWorkflowPortTypeClient();
            }
            catch (Exception e)
            {
                _log.Error("Error connecting to Schufa Engine: " + e.Message, e);
                throw e;
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

        public executeResponse execute(executeRequest executeRequest)
        {
            try
            {
                lazyInit();

                client.Endpoint.Behaviors.Add(new SoapLoggingAuskunft(ref soapXMLDto));
                using (OperationContextScope scope = new OperationContextScope(client.InnerChannel))
                {
                    var httpRequestProperty = new HttpRequestMessageProperty();
                    httpRequestProperty.Headers[HttpRequestHeader.Authorization] = "Basic " +
                      Convert.ToBase64String(Encoding.ASCII.GetBytes(client.ClientCredentials.UserName.UserName + ":" + client.ClientCredentials.UserName.Password));

                    OperationContext.Current.OutgoingMessageProperties[HttpRequestMessageProperty.Name] = httpRequestProperty;

                    _log.Info("Schufa Webserviceaufruf gestartet.");
                    DateTime startTime = DateTime.Now;
                    executeResponse response = client.execute(executeRequest);
                    _log.Info("Schufa Webserviceaufruf Dauer : " + (TimeSpan)(DateTime.Now - startTime));
                    
                    if(response == null)
                        throw new Exception("The webservice did not return an expected message.");

                    return response;
                }
            }
            catch (Exception ex)
            {
                _log.Error("Fehler im Schufa Webserviceaufruf. ExceptionMessage: " + ex.Message);
                throw ex;
            }
        }
        
        public SoapXMLDto getSoapXMLDto()
        {
            return soapXMLDto;
        }

        public void setSoapXMLDto(SoapXMLDto soapXMLDto)
        {
            this.soapXMLDto = soapXMLDto;
        }
    }
}