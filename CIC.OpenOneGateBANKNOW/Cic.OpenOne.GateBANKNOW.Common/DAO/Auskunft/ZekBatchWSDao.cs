using System;
using System.Reflection;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Util.Logging;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft
{
    /// <summary>
    /// ZEK Web service Data Access Object
    /// </summary>
    public class ZekBatchWSDao : IZekBatchWSDao
    {
        ZEKBatchRef.ZEKBatchServiceClient ClientBatch;

        ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private long sysAuskunft;

        private Cic.OpenOne.Common.DTO.SoapXMLDto soapXMLDto = new Cic.OpenOne.Common.DTO.SoapXMLDto();

        #region ZekBatch (EC5 und EC7)

        /// <summary>
        /// Vertragsabmeldung (EC5)
        /// Calls ZEK Batch Webservice closeContractsBatch
        /// </summary>
        /// <param name="idDesc"></param>
        /// <param name="batchRequestId"></param>
        /// <param name="contracts"></param>
        /// <returns>BatchInstructionResponse</returns>
        public ZEKBatchRef.BatchInstructionResponse closeContractsBatch(ZEKBatchRef.IdentityDescriptor idDesc,
                                                                        String batchRequestId,
                                                                        ZEKBatchRef.ContractClosureInstruction[] contracts)
        {
            try
            {
                ClientBatch = new ZEKBatchRef.ZEKBatchServiceClient();

                // DebugFlag holen
                // Wenn das Flag SoapLoggingAuskunftEnabled auf False gesetzt ist, dann werden die XMLs nur für die 
                // AuskuftTypen in die LOGDUMP-Tabelle geschrieben, für die das Feld AuskunftTyp.LOGDUMPFLAG auf True gesetzt ist.
                ClientBatch.Endpoint.Behaviors.Add(new Cic.OpenOne.Common.Util.Behaviour.SoapLoggingAuskunft(ref soapXMLDto));

                _log.Info("ZEK-Massen-Vertragsabmeldung (closeContractsBatch) Webserviceaufruf gestartet.");
                DateTime startTime = DateTime.Now;

                ZEKBatchRef.BatchInstructionResponse response = ClientBatch.closeContractsBatch(idDesc, batchRequestId, contracts);

                _log.Info("ZEK-Massen-Vertragsabmeldung (closeContractsBatch) Webserviceaufruf Dauer : " + (TimeSpan)(DateTime.Now - startTime));

                return response;
            }
            catch (Exception ex)
            {
                _log.Error("Fehler im ZEK-Massen-Vertragsabmeldung Serviceaufruf ", ex);

                throw ex;
            }
        }

        /// <summary>
        /// Mutation Vertragsdaten (EC7) 
        /// Calls ZEK Batch Webservice updateContractsBatch
        /// </summary>
        /// <param name="idDesc"></param>
        /// <param name="batchRequestId"></param>
        /// <param name="contracts"></param>
        /// <returns>BatchInstructionResponse</returns>
        public ZEKBatchRef.BatchInstructionResponse updateContractsBatch(ZEKBatchRef.IdentityDescriptor idDesc,
                                                                        String batchRequestId,
                                                                        ZEKBatchRef.ContractUpdateInstruction[] contracts)
        {
            try
            {
                ClientBatch = new ZEKBatchRef.ZEKBatchServiceClient();

                // DebugFlag holen
                // Wenn das Flag SoapLoggingAuskunftEnabled auf False gesetzt ist, dann werden die XMLs nur für die 
                // AuskuftTypen in die LOGDUMP-Tabelle geschrieben, für die das Feld AuskunftTyp.LOGDUMPFLAG auf True gesetzt ist.
                ClientBatch.Endpoint.Behaviors.Add(new Cic.OpenOne.Common.Util.Behaviour.SoapLoggingAuskunft(ref soapXMLDto));

                _log.Info("ZEK-Mutation Vertragsdaten (updateContractsBatch) Webserviceaufruf gestartet.");
                DateTime startTime = DateTime.Now;

                ZEKBatchRef.BatchInstructionResponse response = ClientBatch.updateContractsBatch(idDesc, batchRequestId, contracts);


                _log.Info("ZEK-Mutation Vertragsdaten (updateContractsBatch) Webserviceaufruf Dauer : " + (TimeSpan)(DateTime.Now - startTime));


                return response;
            }
            catch (Exception ex)
            {
                _log.Error("Fehler im ZEK-Mutation Vertragsdaten Serviceaufruf ", ex);

                throw ex;
            }
        }

        /// <summary>
        /// Get batch Status
        /// Calls ZEK Batch Webservice batchStatus
        /// </summary>
        /// <param name="idDesc"></param>
        /// <param name="batchRequestId"></param>
        /// <returns>BatchStatusResponse</returns>
        public ZEKBatchRef.BatchStatusResponse batchStatus(ZEKBatchRef.IdentityDescriptor idDesc, String batchRequestId)
        {
            try
            {
                ClientBatch = new ZEKBatchRef.ZEKBatchServiceClient();
                // DebugFlag holen
                // Wenn das Flag SoapLoggingAuskunftEnabled auf False gesetzt ist, dann werden die XMLs nur für die 
                // AuskuftTypen in die LOGDUMP-Tabelle geschrieben, für die das Feld AuskunftTyp.LOGDUMPFLAG auf True gesetzt ist.
                ClientBatch.Endpoint.Behaviors.Add(new Cic.OpenOne.Common.Util.Behaviour.SoapLoggingAuskunft(ref soapXMLDto));

                _log.Info("ZEK-BatchStatus Webserviceaufruf gestartet.");
                DateTime startTime = DateTime.Now;

                ZEKBatchRef.BatchStatusResponse response = ClientBatch.batchStatus(idDesc, batchRequestId);

                _log.Info("ZEK-BatchStatus Webserviceaufruf Dauer : " + (TimeSpan)(DateTime.Now - startTime));


                return response;
            }
            catch (Exception ex)
            {
                _log.Error("Fehler im BatchStatus Serviceaufruf ", ex);
                Cic.OpenOne.Common.Util.Logging.LogUtil.addWFLog("ZEKBATCHSTATUS", "SYSAUSKUNF " + sysAuskunft + " " + ex.Message,2);
                throw ex;
            }
        }

        #endregion



        #region Set/Get Methods

        /// <summary>
        /// MySetSysAuskunft
        /// </summary>
        /// <param name="sysid"></param>
        public void MySetSysAuskunft(long sysid)
        {
            this.sysAuskunft = sysid;
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

        #endregion Set/Get Methods
    }
}