using System;
using System.Reflection;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.Util.Behaviour;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft
{
    /// <summary>
    /// KREMO Web Service Data Access Object Inteface
    /// </summary>
    public class KREMOWSDao : IKREMOWSDao
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        KREMORef.ServiceSoapClient Client;
        private Cic.OpenOne.Common.DTO.SoapXMLDto soapXMLDto = new Cic.OpenOne.Common.DTO.SoapXMLDto();
      
        /// <summary>
        /// Calls KREMOWebService method CallKREMOgetVersion()
        /// </summary>
        /// <returns></returns>
        public string CallKremoGetVersion() 
        {
            Client = new KREMORef.ServiceSoapClient();

            Client.Endpoint.Behaviors.Add(new Cic.OpenOne.Common.Util.Behaviour.SoapLoggingAuskunft(ref soapXMLDto)); 

            _log.Info("KremoGetVersion Webserviceaufruf gestartet.");
            DateTime startTime = DateTime.Now;
            string response = Client.CallKREMOgetVersion();
            _log.Info("KremoGetVersion Webserviceaufruf Dauer : " + (TimeSpan)(DateTime.Now - startTime));


            return response;
        }

        /// <summary>
        /// Calls KREMOWebService method CallKREMObyValues()
        /// </summary>
        /// <param name="in_Value"></param>
        /// <param name="out_Value"></param>
        /// <returns></returns>
        public long CallKremoByValues(ref KREMORef.ArrayOfDouble in_Value, ref KREMORef.ArrayOfDouble out_Value)
        {
            Client = new KREMORef.ServiceSoapClient();

            Client.Endpoint.Behaviors.Add(new Cic.OpenOne.Common.Util.Behaviour.SoapLoggingAuskunft(ref soapXMLDto));  
            try
            {
                _log.Info("KremoByValues Webserviceaufruf gestartet.");
                DateTime startTime = DateTime.Now;
                long response = Client.CallKREMObyValues(ref in_Value, ref out_Value);
                _log.Info("KremoByValues Webserviceaufruf Dauer : " + (TimeSpan)(DateTime.Now - startTime));


                return response;
            }
            catch (Exception ex)
            {
                _log.Error("Fehler im KREMO-Webserviceaufruf. Exception Message: ", ex);
                throw ex;
            }
        }

      

        /// <summary>
        /// getSoapXMLDto
        /// </summary>
        /// <returns></returns>
        public Cic.OpenOne.Common.DTO.SoapXMLDto getSoapXMLDto()
        {
            return this.soapXMLDto;
        }

        /// <summary>
        /// setSoapXMLDto
        /// </summary>
        /// <param name="soapXMLDto"></param>
        public void setSoapXMLDto(Cic.OpenOne.Common.DTO.SoapXMLDto soapXMLDto)
        {
            this.soapXMLDto = soapXMLDto;
        }

    }
}
