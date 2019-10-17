using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;
using AutoMapper;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKBatchRef;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft.ZEKRef;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft
{
    /// <summary>
    /// ZekBo implements AbstractZekBo
    /// </summary>
    public class ZekBatchBo : AbstractZekBatchBo
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        DAO.Auskunft.ZEKBatchRef.IdentityDescriptor idDescBatch;

        
        ZekBatchOutDto outDto;

        /// <summary>
        /// Konstruktor, IZEKBatchWSDao, IZEKBatchDBDao, IAuskunfDao werden initialisert
        /// </summary>
        /// <param name="zekBatchWSDao"></param>
        /// <param name="zekBatchDBDao"></param>
        /// <param name="auskunftDao"></param>
        public ZekBatchBo(IZekBatchWSDao zekBatchWSDao, IZekBatchDBDao zekBatchDBDao, IAuskunftDao auskunftDao)
            : base(zekBatchWSDao, zekBatchDBDao, auskunftDao)
        {
        }

        #region ZekBatch (EC5 und EC7)

        /// <summary>
        /// Startet einen Thread für Massen-Vertragsabmeldung (EC5)
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        public override AuskunftDto closeContractsBatch(long sysAuskunft)
        {
            long code = (long)AuskunftErrorCode.ErrorCIC;
            string auskunftHostcomputer = auskunftDao.getAuskunftHostcomputer(sysAuskunft);
            string localHostcomputer = System.Net.Dns.GetHostName().ToString();

            if (auskunftHostcomputer == null || auskunftHostcomputer.Equals(""))
            {
                auskunftDao.setAuskunftHostcomputer(sysAuskunft);
                auskunftHostcomputer = auskunftDao.getAuskunftHostcomputer(sysAuskunft);

            }

            // Get AuskunftDto
            AuskunftDto auskunftDto = auskunftDao.FindBySysId(sysAuskunft);
            try
            {
                // Fill InDto
                ZekInDto inDto = zekBatchDBDao.FindBySysId(auskunftDto.sysAuskunft);

                if (auskunftDto.Fehlercode.Equals(((long)AuskunftErrorCode.BatchRequestSent).ToString()))
                {
                    if (auskunftHostcomputer == localHostcomputer)
                    {
                        // Create the thread object. 
                        Worker workerObject = Worker.Instance;
                        workerObject.doThread(auskunftDao, zekBatchWSDao, zekBatchDBDao);
                    }
                }

                else
                {
                    if (inDto.BatchVertragsabmeldung.Length == 0)
                    {
                        // Auskunft.FehlerCode = -2
                        code = (long)AuskunftErrorCode.ErrorCIC;
                        auskunftDto.Fehlercode = code.ToString();
                        auskunftDto.Status = "Keine ZEK-Verträge mit Status 1 ('ReadyToProcess') gefunden.";
                        auskunftDao.UpdateAuskunftDtoAuskunft(auskunftDto, code);
                        throw new ApplicationException(auskunftDto.Status);
                    }

                    auskunftDto.ZekInDto = inDto;

                    zekBatchWSDao.MySetSysAuskunft(sysAuskunft);

                    // WS-Aufruf-Daten sammeln
                    idDescBatch = zekBatchDBDao.GetIdentityDescriptor();
                    List<ContractClosureInstruction> contracts = MyMapFromDtoToContractClosureInstruction(inDto.BatchVertragsabmeldung);

                    // String batchRequestId = MyGetNewBulkIdentifier();
                    String batchRequestId = inDto.BatchVertragsabmeldung[0].batchRequestId;

                    // AuskunftErrorCode.ErrorZEK = -1
                    code = (long)AuskunftErrorCode.ErrorService;

                    // Send request away
                    zekBatchWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));
                    BatchInstructionResponse batchInstrResponse = zekBatchWSDao.closeContractsBatch(idDescBatch, batchRequestId, contracts.ToArray());

                    // AuskunftErrorCode.ErrorCIC = -2
                    code = (long)AuskunftErrorCode.ErrorCIC;

                    // Zek.actionFlag = 2 (InProcess) für alle Zek-Sätze im Batch
                    zekBatchDBDao.updateAllZEK(sysAuskunft, new ZekDto { actionFlag = (int)ZekActionFlag.InProcess });

                    outDto = new ZekBatchOutDto();

                    // Speichern der Felder aus der BatchInstructionResponse in die Tabelle ZekBR (ReturnCode und ggf. TransactionError)
                    outDto.batchInstructionResponse = Mapper.Map<DAO.Auskunft.ZEKBatchRef.BatchInstructionResponse, ZekBatchInstructionResponseDto>(batchInstrResponse);

                    outDto.batchInstructionResponse.zekBatchMethode = "closeContractsBatch";
                    zekBatchDBDao.SaveContractsBatchOutput(sysAuskunft, outDto.batchInstructionResponse);

                    if (outDto.batchInstructionResponse.TransactionError != null)
                    {
                        // Update Auskunft
                        code = (long)outDto.batchInstructionResponse.TransactionError.Code;
                        auskunftDto.Fehlercode = code.ToString();
                        auskunftDto.Status = outDto.batchInstructionResponse.TransactionError.Text;
                        auskunftDao.UpdateAuskunftDtoAuskunft(auskunftDto, code);

                        // Zek.actionFlag zurück auf 1 (readyToProcess) für alle Zek-Sätze im Batch
                        zekBatchDBDao.updateAllZEK(sysAuskunft, new ZekDto { actionFlag = (int)ZekActionFlag.ReadyToProcess });

                        // ENDE
                    }
                    else
                    {
                        // Auskunft.FehlerCode = -5
                        code = (long)AuskunftErrorCode.BatchRequestSent;
                        auskunftDto.Fehlercode = code.ToString();
                        auskunftDto.Status = "Batch fertig für BatchStatus.";
                        auskunftDao.UpdateAuskunftDtoAuskunft(auskunftDto, code);

                        // Create the thread object. 
                        Worker workerObject = Worker.Instance;
                        workerObject.doThread(auskunftDao, zekBatchWSDao, zekBatchDBDao);
                    }
                }
            }
            catch (Exception ex)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in closeContractsBatch(inDto) !");
                throw new ApplicationException("Unexpected Exception in closeContractsBatch(inDto) !", ex);
            }
            return auskunftDto;
        }

        /// <summary>
        /// Startet einen Thread für Massen-Mutation Vertragsdaten (EC7) 
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        public override AuskunftDto updateContractsBatch(long sysAuskunft)
        {
            long code = (long)AuskunftErrorCode.ErrorCIC;
            string auskunftHostcomputer = auskunftDao.getAuskunftHostcomputer(sysAuskunft);
            string localHostcomputer = System.Net.Dns.GetHostName().ToString();

            if (auskunftHostcomputer == null || auskunftHostcomputer.Equals(""))
            {
                auskunftDao.setAuskunftHostcomputer(sysAuskunft);
                auskunftHostcomputer = auskunftDao.getAuskunftHostcomputer(sysAuskunft);

            }

            // Get AuskunftDto
            AuskunftDto auskunftDto = auskunftDao.FindBySysId(sysAuskunft);
            try
            {
                // Fill InDto
                ZekInDto inDto = zekBatchDBDao.FindBySysId(auskunftDto.sysAuskunft);

                if (auskunftDto.Fehlercode.Equals(((long)AuskunftErrorCode.BatchRequestSent).ToString()))
                {
                    if (auskunftHostcomputer == localHostcomputer)
                    {
                        // Create the thread object. 
                        Worker workerObject = Worker.Instance;
                        workerObject.doThread(auskunftDao, zekBatchWSDao, zekBatchDBDao);
                    }
                }
                else
                {
                    if (inDto.BatchMutationVertragsdaten.Length == 0)
                    {
                        // Auskunft.FehlerCode = -2
                        code = (long)AuskunftErrorCode.ErrorCIC;
                        auskunftDto.Fehlercode = code.ToString();
                        auskunftDto.Status = "Keine ZEK-Verträge mit Status 1 ('ReadyToProcess') gefunden.";
                        auskunftDao.UpdateAuskunftDtoAuskunft(auskunftDto, code);
                        throw new ApplicationException(auskunftDto.Status);
                    }

                    auskunftDto.ZekInDto = inDto;

                    zekBatchWSDao.MySetSysAuskunft(sysAuskunft);

                    // WS-Aufruf-Daten sammeln
                    idDescBatch = zekBatchDBDao.GetIdentityDescriptor();
                    List<ContractUpdateInstruction> contracts = MyMapFromDtoToContractUpdateInstruction(inDto.BatchMutationVertragsdaten);

                    // String batchRequestId = MyGetNewBulkIdentifier();
                    String batchRequestId = inDto.BatchMutationVertragsdaten[0].batchRequestId;

                    // AuskunftErrorCode.ErrorZEK = -1
                    code = (long)AuskunftErrorCode.ErrorService;

                    // Send request away
                    zekBatchWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(sysAuskunft));
                    BatchInstructionResponse batchInstrResponse = zekBatchWSDao.updateContractsBatch(idDescBatch, batchRequestId, contracts.ToArray());

                    // AuskunftErrorCode.ErrorCIC = -2
                    code = (long)AuskunftErrorCode.ErrorCIC;

                    // Zek.actionFlag = 2 (InProcess) für alle Zek-Sätze im Batch
                    zekBatchDBDao.updateAllZEKEC7(sysAuskunft, new ZekDto { actionFlag = (int)ZekActionFlag.InProcess });

                    outDto = new ZekBatchOutDto();

                    // Speichern der Felder aus der BatchInstructionResponse in die Tabelle ZekBR (ReturnCode und ggf. TransactionError)
                    outDto.batchInstructionResponse = Mapper.Map<DAO.Auskunft.ZEKBatchRef.BatchInstructionResponse, ZekBatchInstructionResponseDto>(batchInstrResponse);

                    outDto.batchInstructionResponse.zekBatchMethode = "updateContractsBatch";
                    zekBatchDBDao.SaveContractsBatchOutput(sysAuskunft, outDto.batchInstructionResponse);

                    if (outDto.batchInstructionResponse.TransactionError != null)
                    {
                        // Update Auskunft
                        code = (long)outDto.batchInstructionResponse.TransactionError.Code;
                        auskunftDto.Fehlercode = code.ToString();
                        auskunftDto.Status = outDto.batchInstructionResponse.TransactionError.Text;
                        auskunftDao.UpdateAuskunftDtoAuskunft(auskunftDto, code);

                        // Zek.actionFlag zurück auf 1 (readyToProcess) für alle Zek-Sätze im Batch
                        zekBatchDBDao.updateAllZEKEC7(sysAuskunft, new ZekDto { actionFlag = (int)ZekActionFlag.ReadyToProcess });

                        // ENDE
                    }
                    else
                    {

                        // Auskunft.FehlerCode = -5
                        code = (long)AuskunftErrorCode.BatchRequestSent;
                        auskunftDto.Fehlercode = code.ToString();
                        auskunftDto.Status = "Batch fertig für BatchStatus.";
                        auskunftDao.UpdateAuskunftDtoAuskunft(auskunftDto, code);

                        // Create the thread object. 
                        Worker workerObject = Worker.Instance;
                        workerObject.doThread(auskunftDao, zekBatchWSDao, zekBatchDBDao);

                    }
                }
            }
            catch (Exception ex)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in updateContractsBatch(inDto) !");
                throw new ApplicationException("Unexpected Exception in updateContractsBatch(inDto) !", ex);
            }
            return auskunftDto;
        }


        /// <summary>
        /// Saves Auskunft and Zek-Input, sends closeContractsBatch request (EC5) away and saves response
        /// Massen-Vertragsabmeldung (EC5)
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        public override AuskunftDto closeContractsBatch(ZekInDto inDto)
        {
            long code = (long)AuskunftErrorCode.ErrorCIC;

            // Save Auskunft
            long sysAuskunft = auskunftDao.SaveAuskunft(AuskunfttypDao.ZEKBatchCloseContracts);
            try
            {
                // Save Massen-Vertragsabmeldung Input
                zekBatchDBDao.SaveCloseContractsBatchInput(sysAuskunft, inDto);

                AuskunftDto auskunftDto = null;

                auskunftDto = closeContractsBatch(sysAuskunft);

                return auskunftDto;
            }
            catch (Exception ex)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in closeContractsBatch(inDto) !");
                throw new ApplicationException("Unexpected Exception in closeContractsBatch(inDto) !", ex);
            }
        }


        /// <summary>
        /// Saves Auskunft and Zek-Input, sends updateContractsBatch request (EC7) away and saves response
        /// Mutation Vertragsdaten (EC7) 
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns></returns>
        public override AuskunftDto updateContractsBatch(ZekInDto inDto)
        {
            long code = (long)AuskunftErrorCode.ErrorCIC;

            // Save Auskunft
            long sysAuskunft = auskunftDao.SaveAuskunft(AuskunfttypDao.ZEKBatchUpdateContracts);
            try
            {
                // Save Massen-Änderungen der Vertragsdaten Input
                zekBatchDBDao.SaveUpdateContractsBatchInput(sysAuskunft, inDto);

                AuskunftDto auskunftDto = null;

                auskunftDto = updateContractsBatch(sysAuskunft);

                return auskunftDto;
            }
            catch (Exception ex)
            {
                auskunftDao.UpdateAuskunft(sysAuskunft, code);
                _log.Error("Exception in updateContractsBatch(inDto) !");
                throw new ApplicationException("Unexpected Exception in updateContractsBatch(inDto) !", ex);
            }
        }

        #endregion


        #region MyMethods

        // ZEK Batch Services

        /// <summary>
        /// Maps ZekBatchContractUpdateInstructionDto to ContractClosureInstruction Array
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns>List of ContractUpdateInstruction</returns>
        private List<ContractUpdateInstruction> MyMapFromDtoToContractUpdateInstruction(ZekBatchContractUpdateInstructionDto[] inDto)
        {
            List<ContractUpdateInstruction> contracts = new List<ContractUpdateInstruction>();
            if (inDto != null)
            {
                foreach (var mutation in inDto)
                {
                    ContractUpdateInstruction entity = Mapper.Map<ZekBatchContractUpdateInstructionDto, ContractUpdateInstruction>(mutation);
                    contracts.Add(entity);
                }
            }
            return contracts;
        }

        /// <summary>
        /// Maps List of ZekBatchContractClosureInstructionDto to ContractClosureInstruction Array
        /// </summary>
        /// <param name="inDto"></param>
        /// <returns>List of ContractClosureInstruction</returns>
        private List<ContractClosureInstruction> MyMapFromDtoToContractClosureInstruction(ZekBatchContractClosureInstructionDto[] inDto)
        {
            List<ContractClosureInstruction> contracts = new List<ContractClosureInstruction>();
            if (inDto != null)
            {
                foreach (var vertragsabmeldung in inDto)
                {
                    ContractClosureInstruction entity = Mapper.Map<ZekBatchContractClosureInstructionDto, ContractClosureInstruction>(vertragsabmeldung);
                    contracts.Add(entity);
                }
            }

            return contracts;
        }

        #endregion
    }


    //public class Worker
    //{
    //    // Volatile is used as hint to the compiler that this data
    //    // member will be accessed by multiple threads.
    //    private volatile bool _shouldStop;

    //    // This method will be called when the thread is started.
    //    public void DoWork()
    //    {
    //        while (!_shouldStop)
    //        {
    //            Console.WriteLine("worker thread: working...");
    //        }
    //        Console.WriteLine("worker thread: terminating gracefully.");
    //    }

    //    public void RequestStop()
    //    {
    //        _shouldStop = true;
    //    }
    //}


    /// <summary>
    /// Async ZEK State checker
    /// </summary>
    public class Worker
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private const int BATCHSTATUSINTERVALDEFAULT = 5;
        private const int BATCHSERRORCLIENTDEFAULT = 5;
        private const string CFGCONFIG = "SETUP";
        private const string CFGSECTION = "REEXECUTE_CMS";
        private const string CFGVARIABLE = "STATUSINTERVAL";
        private const string CFGVARIABLE2 = "ZEKERRORCLIENT";

        private volatile bool _shouldStop = false;
        private bool initial = false;
        Thread workerThread = null;

        private static Worker instance;

        /// <summary>
        /// Worker Constructor
        /// </summary>
        private Worker() { }

        /// <summary>
        /// Instance
        /// </summary>
        public static Worker Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Worker();
                    instance.initial = true;
                }
                else
                {
                    instance.initial = false;
                }

                return instance;
            }
        }

        /// <summary>
        /// doThread
        /// </summary>
        /// <param name="auskunftDao"></param>
        /// <param name="zekBatchWSDao"></param>
        /// <param name="zekBatchDBDao"></param>
        public void doThread(IAuskunftDao auskunftDao, IZekBatchWSDao zekBatchWSDao, IZekBatchDBDao zekBatchDBDao)
        {
            int batchStatusIntervalInitial = MyGetBatchStatusIntervalParameter();
            if (instance.initial || instance._shouldStop)
            {
                workerThread = new Thread(() => { getBatchStatusThread(auskunftDao, zekBatchWSDao, zekBatchDBDao); });
                workerThread.Start();
                // sec. warten das erste mal
                Thread.Sleep(10000);
                instance.initial = false;
            }
        }

        /// <summary>
        /// getBatchStatusThread
        /// </summary>
        /// <param name="auskunftDao"></param>
        /// <param name="zekBatchWSDao"></param>
        /// <param name="zekBatchDBDao"></param>
        public void getBatchStatusThread(IAuskunftDao auskunftDao, IZekBatchWSDao zekBatchWSDao, IZekBatchDBDao zekBatchDBDao)
        {
            instance._shouldStop = false;
            int zekListErrorClientAnzahl = 0;
            bool zekErrorClient = false;
            try
            {
                while (!_shouldStop)
                {
                    int zekErrorClientConfigParam = MyGetBatchzekErrorClientParameter();
                    // Gibt es alte Auskunftsätze mit FehlerCode = -5 ?
                    List<AuskunftDto> unprocessedAuskunfte = auskunftDao.getUnprocessedBatchAuskunfte();
                    if ((unprocessedAuskunfte.Count == 0) || zekListErrorClientAnzahl > zekErrorClientConfigParam )
                    {
                        instance._shouldStop = true;
                    }
                    else
                    {
                        foreach (var auskunftUnprocessedDto in unprocessedAuskunfte)
                        {
                            getBatchStatus(auskunftUnprocessedDto, auskunftDao, zekBatchWSDao, zekBatchDBDao, ref zekErrorClient);
                        }
                        if (zekErrorClient)
                        {
                            zekListErrorClientAnzahl++;
                        }
                        else
                        {
                            zekListErrorClientAnzahl = 0;
                        }
                        int batchStatusInterval = MyGetBatchStatusIntervalParameter();
                        Thread.Sleep(batchStatusInterval);
                    }
                }
            }    // while
            catch (Exception ex)
            {
                _log.Error("Exception in getBatchStatusThread(auskunftId): " + ex.Message);
            }
        }

        /// <summary>
        /// RequestStophmm
        /// </summary>
        public void RequestStop()
        {
            _shouldStop = true;
        }

        /// <summary>
        /// Holt den BatchStatusInterval-Parameter aus der Konfiguration
        /// Default ist 5 Minuten
        /// </summary>
        /// <returns></returns>
        private int MyGetBatchStatusIntervalParameter()
        {
            String paramDelay = Cic.OpenOne.Common.Util.Config.AppConfig.Instance.GetEntry(CFGSECTION, CFGVARIABLE, BATCHSTATUSINTERVALDEFAULT.ToString(), CFGCONFIG);

            int batchStatusDelay = 0;
            Int32.TryParse(paramDelay, out batchStatusDelay);

            // Minuten zu Millisekunden 
            batchStatusDelay = batchStatusDelay * 60 * 1000;

            return batchStatusDelay;
        }
        /// <summary>
        /// Holt den errorClient-Parameter aus der Konfiguration, anzahl die mögliche aufrufe ohne Antwort von zekClient
        /// Default ist 5 Minuten
        /// </summary>
        /// <returns></returns>
        private int MyGetBatchzekErrorClientParameter()
        {
            String param = Cic.OpenOne.Common.Util.Config.AppConfig.Instance.GetEntry(CFGSECTION, CFGVARIABLE2, BATCHSERRORCLIENTDEFAULT.ToString(), CFGCONFIG);

            int errorClient = 0;
            Int32.TryParse(param, out errorClient);
            return errorClient;
        }

        /// <summary>
        /// getBatchStatus for ZEK
        /// </summary>
        /// <param name="auskunftUnprocessedDto"></param>
        /// <param name="auskunftDao"></param>
        /// <param name="zekBatchWSDao"></param>
        /// <param name="zekBatchDBDao"></param>
        /// <param name="ZekErrorClient"></param>
        public void getBatchStatus(AuskunftDto auskunftUnprocessedDto, IAuskunftDao auskunftDao, IZekBatchWSDao zekBatchWSDao, IZekBatchDBDao zekBatchDBDao, ref bool ZekErrorClient)
        {
            long code = (long)AuskunftErrorCode.ErrorCIC;
            int batchStatusAttempts = 0;

            ZekBatchOutDto outDto = new ZekBatchOutDto();

            string batchRequestId = zekBatchDBDao.FindBatchRequestId(auskunftUnprocessedDto.sysAuskunft);
            int countcontracts = zekBatchDBDao.getContractsCount(auskunftUnprocessedDto.sysAuskunft);
            string auskunfttyp = auskunftDao.GetAuskunfttypBezeichng(auskunftUnprocessedDto.sysAuskunft);

            // Wurde das Batch vor 4 Stunden oder früher geschickt ?
            if ((DateTime.Now - auskunftUnprocessedDto.Anfragedatum).Hours >= 4)
            {
                // Auskunft.FehlerCode = -2
                code = (long)AuskunftErrorCode.ErrorCIC;
                auskunftUnprocessedDto.Fehlercode = code.ToString();
                auskunftUnprocessedDto.Status = "Das Batch wurde vor 4 Stunden oder früher geschickt.";
                auskunftDao.UpdateAuskunftDtoAuskunft(auskunftUnprocessedDto, code);

                // Zek.actionFlag zurück auf 1 (readyToProcess) für alle Zek-Sätze im Batch
                if (auskunfttyp.Equals(AuskunfttypDao.ZEKBatchUpdateContracts))
                {
                    zekBatchDBDao.updateAllZEKEC7(auskunftUnprocessedDto.sysAuskunft, new ZekDto { actionFlag = (int)ZekActionFlag.ReadyToProcess });
                }
                if (auskunfttyp.Equals(AuskunfttypDao.ZEKBatchCloseContracts))
                {
                    zekBatchDBDao.updateAllZEK(auskunftUnprocessedDto.sysAuskunft, new ZekDto { actionFlag = (int)ZekActionFlag.ReadyToProcess });
                }
            }
            else
            {
                BatchStatusResponse batchStatusResponse = null;
                string exceptionText = "";
                zekBatchWSDao.MySetSysAuskunft(auskunftUnprocessedDto.sysAuskunft);
                try
                {
                    zekBatchWSDao.setSoapXMLDto(auskunftDao.getEntitySoapLog(auskunftUnprocessedDto.sysAuskunft));
                    batchStatusResponse = zekBatchWSDao.batchStatus(zekBatchDBDao.GetIdentityDescriptor(), batchRequestId);
                }
                catch (Exception ex)
                {
                    _log.Error("Exception in getBatchStatusThread(auskunftId): " + ex.Message);
                    exceptionText = ex.Message;

                    
                }
                if (batchStatusResponse == null)
                {
                    ZekBatchStatusResponseDto zekBatchStatusResponseDto = new ZekBatchStatusResponseDto();
                    ZekReturnCodeDto returnCodeCICIntern = new ZekReturnCodeDto();
                    ZekErrorClient = true;
                    returnCodeCICIntern.Text = exceptionText;
                    returnCodeCICIntern.Code = 999;
                    zekBatchStatusResponseDto.ReturnCode = returnCodeCICIntern;
                    outDto.batchStatusResponse = zekBatchStatusResponseDto;
                    zekBatchDBDao.SaveBatchStatusOutput(auskunftUnprocessedDto.sysAuskunft, outDto.batchStatusResponse);
                    outDto.batchStatusResponse.attempts = ++batchStatusAttempts;

                }
                else
                {
                outDto.batchStatusResponse = Mapper.Map<BatchStatusResponse, ZekBatchStatusResponseDto>(batchStatusResponse);
                outDto.batchStatusResponse.attempts = ++batchStatusAttempts;

                // Speichern der Felder aus der BatchStatusResponse in dieTabelle ZekBR (ReturnCode und ggf. TransactionError)
                // (Neuen Datensatz in ZekBR anlegen zur Historisierung der Batch-Anfragen)
                zekBatchDBDao.SaveBatchStatusOutput(auskunftUnprocessedDto.sysAuskunft, outDto.batchStatusResponse);

                if (batchStatusResponse.transactionError != null)
                {
                    // Fehler bei zekBatchWSDao.batchStatus
                    auskunftUnprocessedDto.Fehlercode = batchStatusResponse.transactionError.code.ToString();
                    auskunftUnprocessedDto.Status = "Transaction Error in batchStatus.";
                    auskunftDao.UpdateAuskunftDtoAuskunft(auskunftUnprocessedDto, code);

                    // Zek.actionFlag zurück auf 1 (readyToProcess) für alle Zek-Sätze im Batch
                    if (auskunfttyp.Equals(AuskunfttypDao.ZEKBatchUpdateContracts))
                    {
                        zekBatchDBDao.updateAllZEKEC7(auskunftUnprocessedDto.sysAuskunft, new ZekDto { actionFlag = (int)ZekActionFlag.ReadyToProcess });
                    }
                    if (auskunfttyp.Equals(AuskunfttypDao.ZEKBatchCloseContracts))
                    {
                        zekBatchDBDao.updateAllZEK(auskunftUnprocessedDto.sysAuskunft, new ZekDto { actionFlag = (int)ZekActionFlag.ReadyToProcess });
                    }

                    // ENDE
                }
                else
                {
                    switch (batchStatusResponse.returnCode.code)
                    {
                        case (int)ZekBatchStatus.Pending:       // 31
                            // Loop: Wurde das Batch vor 4 Stunden oder früher geschickt ? (ANFRAGEUHRZEIT - Now > 4 ?)
                            break;
                        case (int)ZekBatchStatus.Processed:       // 32

                            // Auskunft erfolgreich

                            if (batchStatusResponse.errorList != null && batchStatusResponse.errorList.Length > 0)
                            {
                                if (batchStatusResponse.countProcessedSuccesfully + batchStatusResponse.countProcessedError == countcontracts)
                                {
                                    if (auskunfttyp.Equals(AuskunfttypDao.ZEKBatchUpdateContracts))
                                    {
                                        zekBatchDBDao.updateZEKErrorListUpdateBatch(auskunftUnprocessedDto.sysAuskunft, outDto);
                                    }
                                    if (auskunfttyp.Equals(AuskunfttypDao.ZEKBatchCloseContracts))
                                    {
                                        zekBatchDBDao.updateZEKErrorListCloseBatch(auskunftUnprocessedDto.sysAuskunft, outDto);
                                    }
                                    // Cannot send to ZEK due to internal problems
                                }
                                else
                                {
                                    // code = -2
                                    code = (long)AuskunftErrorCode.ErrorCIC;
                                    auskunftUnprocessedDto.Fehlercode = code.ToString();
                                    auskunftUnprocessedDto.Status = "Anzahl der Verträge stimmt nicht.";
                                    auskunftDao.UpdateAuskunftDtoAuskunft(auskunftUnprocessedDto, code);

                                    // Zek.actionFlag zurück auf 1 (readyToProcess) für alle Zek-Sätze im Batch
                                    if (auskunfttyp.Equals(AuskunfttypDao.ZEKBatchUpdateContracts))
                                    {
                                        zekBatchDBDao.updateAllZEKEC7(auskunftUnprocessedDto.sysAuskunft, new ZekDto { actionFlag = (int)ZekActionFlag.ReadyToProcess });
                                    }
                                    if (auskunfttyp.Equals(AuskunfttypDao.ZEKBatchCloseContracts))
                                    {
                                        zekBatchDBDao.updateAllZEK(auskunftUnprocessedDto.sysAuskunft, new ZekDto { actionFlag = (int)ZekActionFlag.ReadyToProcess });
                                    }

                                    break;
                                    // ENDE
                                }
                            }
                            else
                            {
                                // batchStatusResponse.errorList ist leer:
                                // Zek.actionFlag auf 3 (Processed) für alle Zek-Sätze im Batch
                                if (auskunfttyp.Equals(AuskunfttypDao.ZEKBatchUpdateContracts))
                                {
                                    zekBatchDBDao.updateAllZEKEC7(auskunftUnprocessedDto.sysAuskunft, new ZekDto { actionFlag = (int)ZekActionFlag.Processed });
                                }
                                if (auskunfttyp.Equals(AuskunfttypDao.ZEKBatchCloseContracts))
                                {
                                    zekBatchDBDao.updateAllZEK(auskunftUnprocessedDto.sysAuskunft, new ZekDto { actionFlag = (int)ZekActionFlag.Processed });
                                }

                                // Wurde ein Polling-Thread gestartet ?
                                // if (workerThread.IsAlive)
                                {
                                    // Ja: Ende ; 
                                    // _shouldStop = true;
                                    // ENDE
                                }
                                // else
                                // Nein: Loop: Gibt es alte Auskunftsätze mit FehlerCode = -5 ?
                            }

                            // code = 0
                            code = (long)AuskunftErrorCode.NoError;
                            auskunftUnprocessedDto.Fehlercode = code.ToString();
                            auskunftUnprocessedDto.Status = String.Empty;
                            String zusatztext = "OK = "+batchStatusResponse.countProcessedSuccesfully+"/ NOK = " + batchStatusResponse.countProcessedError;
                            auskunftDao.UpdateAuskunftDtoAuskunft(auskunftUnprocessedDto, code,zusatztext);


                            // ENDE

                            break;
                    }
                }
                }
            }
        }
    }
}