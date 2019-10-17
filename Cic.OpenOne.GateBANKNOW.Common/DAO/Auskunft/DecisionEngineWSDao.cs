using System;
using Cic.OpenOne.Common.Util.Logging;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft
{
    /// <summary>
    /// Decision Engine Web Service Data Access Object
    /// </summary>
    public class DecisionEngineWSDao : IDecisionEngineWSDao
    {
        DAO.Auskunft.DecisionEngineRef.S1PublicClient Client = new DecisionEngineRef.S1PublicClient();
        private Cic.OpenOne.Common.DTO.SoapXMLDto soapXMLDto = new Cic.OpenOne.Common.DTO.SoapXMLDto();


        /// <summary>
        /// Execute Decision Engine
        /// </summary>
        /// <param name="request">Request</param>
        /// <param name="_log">Logger</param>
        /// <returns>Response</returns>
        public DecisionEngineRef.StrategyOneResponse execute(string request, ILog _log)
        {
            try
            {



                Client.Endpoint.Behaviors.Add(new Cic.OpenOne.Common.Util.Behaviour.SoapLoggingAuskunft(ref soapXMLDto));

                _log.Info("DecisionEngine Webserviceaufruf gestartet.");
                DateTime startTime = DateTime.Now;
                DecisionEngineRef.StrategyOneResponse response = Client.execute(request);
                _log.Info("DecisionEngine Webserviceaufruf Dauer : " + (TimeSpan)(DateTime.Now - startTime));



                return response;
            }
            catch (Exception ex)
            {
                _log.Error("Fehler im DecisionEngine Webserviceaufruf. ExceptionMessage: " + ex.Message);
                throw ex;
            }
        }

        #region Get/Set Methods

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
        /// <param name="soapXMLDto">soapXMLDto</param>
        public void setSoapXMLDto(Cic.OpenOne.Common.DTO.SoapXMLDto soapXMLDto)
        {
            this.soapXMLDto = soapXMLDto;
        }


        #endregion Get/Set Methods
    }
}