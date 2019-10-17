using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.GateBANKNOW.Common.GuardeanServiceReference;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Reflection;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceModel.Security;
using System.Text;
using System.Web;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft
{
    public class DecisionEngineGuardeanWSDao : IDecisionEngineGuardeanWSDao
    {
        private Wkf_Control_InterfacePortTypeClient client; 
        private Cic.OpenOne.Common.DTO.SoapXMLDto soapXMLDto = new Cic.OpenOne.Common.DTO.SoapXMLDto();
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public DecisionEngineGuardeanWSDao()
        {
        }

        private void lazyInit()
        {
            if (client != null) return;
            try
            {
                client = new Wkf_Control_InterfacePortTypeClient();
            }
            catch (Exception e)
            {
                _log.Error("Error connecting to Guardean Decision Engine: " + e.Message, e);
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

        public Cic.OpenOne.GateBANKNOW.Common.GuardeanServiceReference.executeResponse execute(Cic.OpenOne.GateBANKNOW.Common.GuardeanServiceReference.executeRequest executeRequest)
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
                    
                    _log.Info("DecisionEngineGuardean Webserviceaufruf gestartet.");
                    DateTime startTime = DateTime.Now;
                    Cic.OpenOne.GateBANKNOW.Common.GuardeanServiceReference.executeResponse response = client.execute(executeRequest);
                    _log.Info("DecisionEngineGuardean Webserviceaufruf Dauer : " + (TimeSpan)(DateTime.Now - startTime));


                    return response;
                }
            }
            catch (Exception ex)
            {
                _log.Error("Fehler im DecisionEngine Webserviceaufruf. ExceptionMessage: " + ex.Message);
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