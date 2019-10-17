using AutoMapper;
using Cic.OpenOne.Common.Model.DdIc;
using Cic.OpenOne.Common.Util;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using CIC.Database.IC.EF6.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft
{
    /// <summary>
    /// ZEK Data Access Object
    /// </summary>
    public class ZekBatchDBDao : IZekBatchDBDao
    {
        ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        /// <summary>
        /// updateZEK für alle Zek-Sätze im Batch
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="zekDto"></param>
        /// <returns></returns>
        public int updateAllZEK(long sysAuskunft, ZekDto zekDto)
        {
            int countUpdated = 0;
            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {
                    List<ZEK> zekList = (from zek in context.ZEK
                                         join inpEC5 in context.ZEKINPEC5 on zek.SYSZEK equals inpEC5.ZEK.SYSZEK
                                         where inpEC5.AUSKUNFT.SYSAUSKUNFT == sysAuskunft
                                         select zek).ToList();
                    foreach (var zek in zekList)
                    {
                        zek.ACTIONFLAG = zekDto.actionFlag;
                        zek.RUECKMELDECODE = zekDto.rueckmeldeCode;
                        zek.RUECKMELDUNG = zekDto.rueckmeldung;
                        countUpdated++;
                    }
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    _log.Error("updateZEK failed: ", ex);
                    throw ex;
                }
            }
            return countUpdated;
        }


        /// <summary>
        /// updateZEK für alle Zek-Sätze ec7 im Batch
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="zekDto"></param>
        /// <returns></returns>
        public int updateAllZEKEC7(long sysAuskunft, ZekDto zekDto)
        {
            int countUpdated = 0;
            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {
                    List<ZEK> zekList = (from zek in context.ZEK
                                         join inpEC7 in context.ZEKINPEC7 on zek.SYSZEK equals inpEC7.ZEK.SYSZEK
                                         where inpEC7.AUSKUNFT.SYSAUSKUNFT == sysAuskunft
                                         select zek).ToList();
                    foreach (var zek in zekList)
                    {
                        zek.ACTIONFLAG = zekDto.actionFlag;
                        zek.RUECKMELDECODE = zekDto.rueckmeldeCode;
                        zek.RUECKMELDUNG = zekDto.rueckmeldung;
                        countUpdated++;
                    }
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    _log.Error("updateZEK failed: ", ex);
                    throw ex;
                }
            }
            return countUpdated;
        }





        /// <summary>
        /// Setzt das Zek.ZekActionFlag, für die Zeks in der ErrorList stehen, auf Error
        /// und die restlichen auf Processed
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="outDto"></param>
        /// <returns></returns>
        public int updateZEKErrorListCloseBatch(long sysAuskunft, ZekBatchOutDto outDto)
        {
            int countUpdated = 0;
            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {
                    List<ZEKINPEC5> inpEc5List = (from inpEC5 in context.ZEKINPEC5
                                         where inpEC5.AUSKUNFT.SYSAUSKUNFT == sysAuskunft
                                         select inpEC5).ToList();

                    ZEKBR zekBR = context.ZEKBR.Where(par => par.SYSZEKBR == outDto.batchStatusResponse.sysZekBR).Single();

                    foreach (var inpEc5 in inpEc5List)
                    {
                        
                        if (inpEc5.ZEK == null)
                            context.Entry(inpEc5).Reference(f => f.ZEK).Load();
                        var zek = inpEc5.ZEK;

                        ZekBatchInstructionErrorDto errorZek = outDto.batchStatusResponse.errorList.
                                                                Where(par => par.customerReference.Equals(zek.SYSZEK.ToString())).SingleOrDefault();
                        if (errorZek != null)
                        {
                            // Update ZEK
                            zek.ACTIONFLAG = (int)ZekActionFlag.Error;
                            zek.RUECKMELDECODE = null;
                            if (outDto.batchStatusResponse.TransactionError != null)
                            {
                                zek.RUECKMELDECODE = outDto.batchStatusResponse.TransactionError.Code.ToString();
                            }
                            zek.RUECKMELDUNG = errorZek.errorDescription;

                            // zekOutEC5 erstellen
                            ZEKOUTEC5 zekOutEC5 = new ZEKOUTEC5();
                            zekOutEC5.AUSKUNFT = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskunft).Single();
                            zekOutEC5.ZEKBR = zekBR;
                            zekOutEC5.ZEK = zek;
                            context.ZEKOUTEC5.Add(zekOutEC5);

                            // ZekCMR erstellen
                            ZEKCMR zekcrm = new ZEKCMR();
                            zekcrm.CUSTOMERREFERENCE = errorZek.customerReference;
                            zekcrm.KREDITVERTRAGID = inpEc5.KREDITVERTRAGID;
                            if (outDto.batchStatusResponse.TransactionError != null)
                            {
                                zekcrm.ERRCODE = outDto.batchStatusResponse.TransactionError.Code;
                            }
                            zekcrm.ERRTEXT = errorZek.errorDescription;
                            context.ZEKCMR.Add(zekcrm);

                            zekOutEC5.ZEKCMR = zekcrm;
                        }
                        else
                        {
                            zek.ACTIONFLAG = (int)ZekActionFlag.Processed;
                        }
                        countUpdated++;
                    }
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    _log.Error("Fehler beim Speichern von updateZEKErrorListCloseBatch: ", ex);
                    throw ex;
                }
                return countUpdated;
            }
        }

        /// <summary>
        /// Setzt das Zek.ZekActionFlag, für die Zeks in der ErrorList stehen, auf Error
        /// und die restlichen auf Processed
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="outDto"></param>
        /// <returns></returns>
        public int updateZEKErrorListUpdateBatch(long sysAuskunft, ZekBatchOutDto outDto)
        {
            int countUpdated = 0;
            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {
                    List<ZEKINPEC7> inpEC7List = (from inpEC7 in context.ZEKINPEC7
                                                  where inpEC7.AUSKUNFT.SYSAUSKUNFT == sysAuskunft
                                                  select inpEC7).ToList();

                    ZEKBR zekBR = context.ZEKBR.Where(par => par.SYSZEKBR == outDto.batchStatusResponse.sysZekBR).Single();

                    foreach (var inpEC7 in inpEC7List)
                    {
                        
                        if (inpEC7.ZEK == null)
                            context.Entry(inpEC7).Reference(f => f.ZEK).Load();
                        var zek = inpEC7.ZEK;

                        ZekBatchInstructionErrorDto errorZek = outDto.batchStatusResponse.errorList.
                                                                Where(par => par.customerReference.Equals(zek.SYSZEK.ToString())).SingleOrDefault();
                        if (errorZek != null)
                        {
                            // Update ZEK
                            zek.ACTIONFLAG = (int)ZekActionFlag.Error;
                            zek.RUECKMELDECODE = null;
                            if (outDto.batchStatusResponse.TransactionError != null)
                            {
                                zek.RUECKMELDECODE = outDto.batchStatusResponse.TransactionError.Code.ToString();
                            }
                            zek.RUECKMELDUNG = errorZek.errorDescription;

                            // zekOutEC7 erstellen
                            ZEKOUTEC7 zekOutEC7 = new ZEKOUTEC7();
                            zekOutEC7.AUSKUNFT = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskunft).Single();
                            zekOutEC7.ZEKBR = zekBR;
                            zekOutEC7.ZEK = zek;
                            context.ZEKOUTEC7.Add(zekOutEC7);

                            // ZekCMR erstellen
                            ZEKCMR zekcrm = new ZEKCMR();
                            zekcrm.CUSTOMERREFERENCE = errorZek.customerReference;
                            zekcrm.KREDITVERTRAGID = inpEC7.KREDITVERTRAGID;
                            if (outDto.batchStatusResponse.TransactionError != null)
                            {
                                zekcrm.ERRCODE = outDto.batchStatusResponse.TransactionError.Code;
                            }
                            zekcrm.ERRTEXT = errorZek.errorDescription;
                            context.ZEKCMR.Add(zekcrm);

                            zekOutEC7.ZEKCMR = zekcrm;
                        }
                        else
                        {
                            zek.ACTIONFLAG = (int)ZekActionFlag.Processed;
                        }
                        countUpdated++;
                    }
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    _log.Error("Fehler beim Speichern von updateZEKErrorListUpdateBatch: ", ex);
                    throw ex;
                }
                return countUpdated;
            }
        }

        #region ZekBatch (EC5 und EC7)

        /// <summary>
        /// Gets USERNAME and PASSWORD from AUSKUNFTCFG for ZekBatch Services
        /// </summary>
        /// <returns>IdentityDescriptor, filled with username and password</returns>
        public ZEKBatchRef.IdentityDescriptor GetIdentityDescriptor()
        {
            ZEKBatchRef.IdentityDescriptor idDescriptor = new ZEKBatchRef.IdentityDescriptor();
            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {
                    idDescriptor.name = context.AUSKUNFTCFG.Where(par => par.BEZEICHNUNG.ToLower().Equals("zekbatch")).First().USERNAME;
                    idDescriptor.password = context.AUSKUNFTCFG.Where(par => par.BEZEICHNUNG.ToLower().Equals("zekbatch")).First().KEYVALUE;
                }
                catch (Exception ex)
                {
                    _log.Error("Zugangsdaten für ZEKBatch konnten nicht geladen werden. ", ex);
                    throw ex;
                }
            }
            return idDescriptor;
        }

        /// <summary>
        /// Saves Output from Batch operations: close and update
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="outDto"></param>
        public void SaveContractsBatchOutput(long sysAuskunft, ZekBatchInstructionResponseDto outDto)
        {
            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {
                    ZEKBR zekBR = new ZEKBR();
                    zekBR.AUSKUNFT = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskunft).Single();
                    if (outDto != null)
                    {
                        if (outDto.ReturnCode != null)
                        {
                            zekBR.RETURNCODE = outDto.ReturnCode.Code;
                            zekBR.RETURNTEXT = outDto.ReturnCode.Text;
                        }
                        if (outDto.TransactionError != null)
                        {
                            zekBR.ERRCODE = outDto.TransactionError.Code;
                            zekBR.ERRTEXT = outDto.TransactionError.Text;
                        }
                        zekBR.RESPONSEDATE = DateTime.Now;
                        zekBR.RESPONSETIME = DateTimeHelper.DateTimeToClarionTime(DateTime.Now);
                        zekBR.ZEKBATCHMETHODE = outDto.zekBatchMethode;
                    }
                    context.ZEKBR.Add(zekBR);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    _log.Error("Fehler beim Speichern von SaveCloseContractsBatchOutput ", ex);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Saves Status of Batch operations (close and update)
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="outDto"></param>
        public void SaveBatchStatusOutput(long sysAuskunft, ZekBatchStatusResponseDto outDto)
        {
            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {
                    // Neuen Datensatz in ZekBR anlegen zur Historisierung der Batch-Anfragen
                    ZEKBR zekBR = new ZEKBR();
                    zekBR.AUSKUNFT = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskunft).Single();
                    zekBR.COUNTPROCESSEDWITHERROR = outDto.countProcessedError;
                    zekBR.COUNTPROCESSEDSUCCESSFULLY = outDto.countProcessedSuccesfully;
                    if (outDto.ReturnCode != null)
                    {
                        zekBR.RETURNCODE = outDto.ReturnCode.Code;
                        zekBR.RETURNTEXT = outDto.ReturnCode.Text;
                    }
                    if (outDto.TransactionError != null)
                    {
                        zekBR.ERRCODE = outDto.TransactionError.Code;
                        zekBR.ERRTEXT = outDto.TransactionError.Text;
                    }
                    zekBR.RESPONSEDATE = DateTime.Now;
                    zekBR.RESPONSETIME = DateTimeHelper.DateTimeToClarionTime(DateTime.Now);
                    zekBR.ZEKBATCHMETHODE = "batchStatus";
                    // Ein Zähler für die Status-Abfragen für eine Meldung
                    zekBR.ATTEMPTS = outDto.attempts;

                    context.ZEKBR.Add(zekBR);
                    context.SaveChanges();

                    // Die ZekOutEC5-Sätze werden nur mit dem letzten zekBR verknüpft
                    outDto.sysZekBR = zekBR.SYSZEKBR;
                }
                catch (Exception ex)
                {
                    _log.Error("Fehler beim Speichern von SaveBatchStatusOutput", ex);
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Saves Batch Vertragsabmeldung (EC5) Input
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="inDto"></param>
        public void SaveCloseContractsBatchInput(long sysAuskunft, ZekInDto inDto)
        {
            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {
                    AUSKUNFT auskunft = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskunft).Single();
                    if (inDto.BatchVertragsabmeldung != null)
                    {
                        foreach (var vertrag in inDto.BatchVertragsabmeldung)
                        {
                            
                            ZEKINPEC5 zekInpEC5 = new ZEKINPEC5();
                            zekInpEC5 = Mapper.Map<ZekBatchContractClosureInstructionDto, ZEKINPEC5>(vertrag);
                            zekInpEC5.AUSKUNFT = auskunft;
                            context.ZEKINPEC5.Add(zekInpEC5);
                        }
                    }
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    _log.Error("Fehler beim Speichern von SaveCloseContractsBatchInput ", ex);
                    throw ex;
                }
            }
        }


         /// <summary>
        /// Saves Batch Vertragsänderung (EC7) Input
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <param name="inDto"></param>
        public void SaveUpdateContractsBatchInput(long sysAuskunft, ZekInDto inDto)
        {
            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {
                    ZEKINPEC7 zekInpEC7 = new ZEKINPEC7();

                    zekInpEC7.AUSKUNFT = context.AUSKUNFT.Where(par => par.SYSAUSKUNFT == sysAuskunft).Single();
                    var AuskunftTypQuery = from AuskTyp in context.AUSKUNFTTYP
                                           join Ausk in context.AUSKUNFT on AuskTyp.SYSAUSKUNFTTYP equals Ausk.AUSKUNFTTYP.SYSAUSKUNFTTYP
                                           where Ausk.SYSAUSKUNFT == sysAuskunft
                                           select AuskTyp;
                    AUSKUNFTTYP auskunftTyp = AuskunftTypQuery.Single();
                    context.ZEKINPEC7.Add(zekInpEC7);

                    zekInpEC7.ART = (int)zekInpEC7.AUSKUNFT.AUSKUNFTTYP.SYSAUSKUNFTTYP;

                    if (inDto.BatchMutationVertragsdaten != null && inDto.BatchMutationVertragsdaten.Length > 0)
                    {
                        foreach (var mutation in inDto.BatchMutationVertragsdaten)
                        {
                            
                            zekInpEC7.CUSTOMERREFERENCE = mutation.customerReference;

                            if (auskunftTyp.BEZEICHNUNG.Equals(AuskunfttypDao.ZEKUpdateBardarlehen))
                            {
                                ZEKBDC zekbdc = new ZEKBDC();
                                zekbdc = Mapper.Map<ZekBardarlehenDescriptionDto, ZEKBDC>(mutation.bardarlehen);
                                context.ZEKBDC.Add(zekbdc);
                                zekInpEC7.ZEKBDC = zekbdc;
                            }
                            if (auskunftTyp.BEZEICHNUNG.Equals(AuskunfttypDao.ZEKUpdateFestkredit))
                            {
                                ZEKFKC zekfkc = new ZEKFKC();
                                zekfkc = Mapper.Map<ZekFestkreditDescriptionDto, ZEKFKC>(mutation.festkredit);
                                context.ZEKFKC.Add(zekfkc);
                                zekInpEC7.ZEKFKC = zekfkc;
                            }
                            if (auskunftTyp.BEZEICHNUNG.Equals(AuskunfttypDao.ZEKUpdateKontokorrentkredit))
                            {
                                ZEKKKC zekkkc = new ZEKKKC();
                                zekkkc = Mapper.Map<ZekKontokorrentkreditDescriptionDto, ZEKKKC>(mutation.kontokorrentkredit);
                                context.ZEKKKC.Add(zekkkc);
                                zekInpEC7.ZEKKKC = zekkkc;
                            }
                            if (auskunftTyp.BEZEICHNUNG.Equals(AuskunfttypDao.ZEKUpdateLeasingMietvertrag))
                            {
                                ZEKLMC zeklmc = new ZEKLMC();
                                zeklmc = Mapper.Map<ZekLeasingMietvertragDescriptionDto, ZEKLMC>(mutation.leasingMietvertrag);
                                context.ZEKLMC.Add(zeklmc);
                                zekInpEC7.ZEKLMC = zeklmc;
                            }
                            if (auskunftTyp.BEZEICHNUNG.Equals(AuskunfttypDao.ZEKUpdateTeilzahlungskredit))
                            {
                                ZEKTKC zektkc = new ZEKTKC();
                                zektkc = Mapper.Map<ZekTeilzahlungskreditDescriptionDto, ZEKTKC>(mutation.teilzahlungskredit);
                                context.ZEKTKC.Add(zektkc);
                                zekInpEC7.ZEKTKC = zektkc;
                            }
                        }
                    }

                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    _log.Error("Fehler beim Speichern von SaveUpdateContractsBatchInput ", ex);
                    throw ex;
                }
            }
        }

        #endregion



        /// <summary>
        /// Finds ZEK Input data by sysAuskunft and maps it to ZekInDto
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns>ZekInDto, filled with input for ZEK WS</returns>
        public ZekInDto FindBySysId(long sysAuskunft)
        {
            ZekInDto rval = new ZekInDto();

            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {
                    var AuskunftTypQuery = from AuskTyp in context.AUSKUNFTTYP
                                           join Ausk in context.AUSKUNFT on AuskTyp.SYSAUSKUNFTTYP equals Ausk.AUSKUNFTTYP.SYSAUSKUNFTTYP
                                           where Ausk.SYSAUSKUNFT == sysAuskunft
                                           select AuskTyp;
                    AUSKUNFTTYP auskunftTyp = AuskunftTypQuery.Single();



                    // Zek-Massen-Vertragsabmeldung (EC5)
                    if (auskunftTyp.BEZEICHNUNG == AuskunfttypDao.ZEKBatchCloseContracts)
                    {
                        var ec5Query = from zekInpEC5 in context.ZEKINPEC5
                                       join auskunft in context.AUSKUNFT on zekInpEC5.AUSKUNFT.SYSAUSKUNFT equals auskunft.SYSAUSKUNFT
                                       join zek in context.ZEK on zekInpEC5.ZEK.SYSZEK equals zek.SYSZEK
                                       where auskunft.SYSAUSKUNFT == sysAuskunft && zek.ACTIONFLAG == (int)ZekActionFlag.ReadyToProcess
                                       select zekInpEC5;

                        List<ZekBatchContractClosureInstructionDto> zekBatchCloseList = new List<ZekBatchContractClosureInstructionDto>();
                        foreach (var zekInpEC5 in ec5Query)
                        {
                            zekBatchCloseList.Add(Mapper.Map<ZEKINPEC5, ZekBatchContractClosureInstructionDto>(zekInpEC5));
                        }
                        rval.BatchVertragsabmeldung = zekBatchCloseList.ToArray();
                        return rval;
                    }



                    // Zek-Batch Mutation Vertragsdaten (EC7)
                    if (auskunftTyp.BEZEICHNUNG == AuskunfttypDao.ZEKBatchUpdateContracts)
                    {
                        var ec7Query = from zekInpEC7 in context.ZEKINPEC7
                                       join auskunft in context.AUSKUNFT on zekInpEC7.AUSKUNFT.SYSAUSKUNFT equals auskunft.SYSAUSKUNFT
                                       join zek in context.ZEK on zekInpEC7.ZEK.SYSZEK equals zek.SYSZEK
                                       where auskunft.SYSAUSKUNFT == sysAuskunft && zek.ACTIONFLAG == (int)ZekActionFlag.ReadyToProcess
                                       select zekInpEC7;

                        List<ZekBatchContractUpdateInstructionDto> zekBatchUpdateList = new List<ZekBatchContractUpdateInstructionDto>();
                        foreach (var zekInpEC7 in ec7Query)
                        {
                            ZekBatchContractUpdateInstructionDto updateDto = Mapper.Map<ZEKINPEC7, ZekBatchContractUpdateInstructionDto>(zekInpEC7);

                            if (zekInpEC7.ZEKBDC == null)
                                context.Entry(zekInpEC7).Reference(f => f.ZEKBDC).Load();
                            
                            updateDto.bardarlehen = Mapper.Map<ZEKBDC, ZekBardarlehenDescriptionDto>(zekInpEC7.ZEKBDC);
                             
                            if (zekInpEC7.ZEKFKC == null)
                                context.Entry(zekInpEC7).Reference(f => f.ZEKFKC).Load();
                            updateDto.festkredit = Mapper.Map<ZEKFKC, ZekFestkreditDescriptionDto>(zekInpEC7.ZEKFKC);
                             
                            if (zekInpEC7.ZEKKKC == null)
                                context.Entry(zekInpEC7).Reference(f => f.ZEKKKC).Load();
                            updateDto.kontokorrentkredit = Mapper.Map<ZEKKKC, ZekKontokorrentkreditDescriptionDto>(zekInpEC7.ZEKKKC);
                             
                            if (zekInpEC7.ZEKLMC == null)
                                context.Entry(zekInpEC7).Reference(f => f.ZEKLMC).Load();
                            updateDto.leasingMietvertrag = Mapper.Map<ZEKLMC, ZekLeasingMietvertragDescriptionDto>(zekInpEC7.ZEKLMC);
                             
                            if (zekInpEC7.ZEKTKC == null)
                                context.Entry(zekInpEC7).Reference(f => f.ZEKTKC).Load();
                            updateDto.teilzahlungskredit = Mapper.Map<ZEKTKC, ZekTeilzahlungskreditDescriptionDto>(zekInpEC7.ZEKTKC);

                            zekBatchUpdateList.Add(updateDto);
                        }
                        rval.BatchMutationVertragsdaten = zekBatchUpdateList.ToArray();
                        return rval;
                    }



                    return rval;
                }
                catch (Exception ex)
                {
                    _log.Error("Fehler in ZekDBDao.FindBySysId: ", ex);
                    throw ex;
                }
            }
        }



        /// <summary>
        /// Finds batchrequestId by sysAuskunft
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        public string FindBatchRequestId(long sysAuskunft)
        {
            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {
                    var AuskunftTypQuery = from AuskTyp in context.AUSKUNFTTYP
                                           join Ausk in context.AUSKUNFT on AuskTyp.SYSAUSKUNFTTYP equals Ausk.AUSKUNFTTYP.SYSAUSKUNFTTYP
                                           where Ausk.SYSAUSKUNFT == sysAuskunft
                                           select AuskTyp;
                    AUSKUNFTTYP auskunftTyp = AuskunftTypQuery.Single();



                    // Zek-Massen-Vertragsabmeldung (EC5)
                    if (auskunftTyp.BEZEICHNUNG == AuskunfttypDao.ZEKBatchCloseContracts)
                    {
                        var ec5Query = from zekInpEC5 in context.ZEKINPEC5
                                       join auskunft in context.AUSKUNFT on zekInpEC5.AUSKUNFT.SYSAUSKUNFT equals auskunft.SYSAUSKUNFT
                                       join zek in context.ZEK on zekInpEC5.ZEK.SYSZEK equals zek.SYSZEK
                                       where auskunft.SYSAUSKUNFT == sysAuskunft
                                       select zekInpEC5;

                        ZEKINPEC5 zekBatchCloseec5 = ec5Query.FirstOrDefault();
                        if (zekBatchCloseec5 != null)
                        {
                            return zekBatchCloseec5.BATCHREQUESTID;

                        }
                        else
                            return null;

                    }



                    // Zek-Batch Mutation Vertragsdaten (EC7)
                    if (auskunftTyp.BEZEICHNUNG == AuskunfttypDao.ZEKBatchUpdateContracts)
                    {
                        var ec7Query = from zekInpEC7 in context.ZEKINPEC7
                                       join auskunft in context.AUSKUNFT on zekInpEC7.AUSKUNFT.SYSAUSKUNFT equals auskunft.SYSAUSKUNFT
                                       join zek in context.ZEK on zekInpEC7.ZEK.SYSZEK equals zek.SYSZEK
                                       where auskunft.SYSAUSKUNFT == sysAuskunft
                                       select zekInpEC7;

                        ZEKINPEC7 zekBatchCloseec7 = ec7Query.FirstOrDefault();
                        if (zekBatchCloseec7 != null)
                        {
                            return zekBatchCloseec7.BATCHREQUESTID;

                        }
                        else
                            return null;
                    }



                    return null;
                }
                catch (Exception ex)
                {
                    _log.Error("Fehler in ZekDBDao.FindBySysId: ", ex);
                    throw ex;
                }
            }
        }


        /// <summary>
        /// Finds batchrequestId by sysAuskunft
        /// </summary>
        /// <param name="sysAuskunft"></param>
        /// <returns></returns>
        public int getContractsCount(long sysAuskunft)
        {
            using (DdIcExtended context = new DdIcExtended())
            {
                try
                {
                    var AuskunftTypQuery = from AuskTyp in context.AUSKUNFTTYP
                                           join Ausk in context.AUSKUNFT on AuskTyp.SYSAUSKUNFTTYP equals Ausk.AUSKUNFTTYP.SYSAUSKUNFTTYP
                                           where Ausk.SYSAUSKUNFT == sysAuskunft
                                           select AuskTyp;
                    AUSKUNFTTYP auskunftTyp = AuskunftTypQuery.Single();



                    // Zek-Massen-Vertragsabmeldung (EC5)
                    if (auskunftTyp.BEZEICHNUNG == AuskunfttypDao.ZEKBatchCloseContracts)
                    {
                        var ec5Query = from zekInpEC5 in context.ZEKINPEC5
                                       join auskunft in context.AUSKUNFT on zekInpEC5.AUSKUNFT.SYSAUSKUNFT equals auskunft.SYSAUSKUNFT
                                       join zek in context.ZEK on zekInpEC5.ZEK.SYSZEK equals zek.SYSZEK
                                       where auskunft.SYSAUSKUNFT == sysAuskunft
                                       select zekInpEC5;

                        return ec5Query.Count();
                        
                    }



                    // Zek-Batch Mutation Vertragsdaten (EC7)
                    if (auskunftTyp.BEZEICHNUNG == AuskunfttypDao.ZEKBatchUpdateContracts)
                    {
                        var ec7Query = from zekInpEC7 in context.ZEKINPEC7
                                       join auskunft in context.AUSKUNFT on zekInpEC7.AUSKUNFT.SYSAUSKUNFT equals auskunft.SYSAUSKUNFT
                                       join zek in context.ZEK on zekInpEC7.ZEK.SYSZEK equals zek.SYSZEK
                                       where auskunft.SYSAUSKUNFT == sysAuskunft
                                       select zekInpEC7;

                        return ec7Query.Count();
                    }



                    return 0;
                }
                catch (Exception ex)
                {
                    _log.Error("Fehler in ZekDBDao.getContractsCount: ", ex);
                    throw ex;
                }
            }
        }


        #region Private Methods

        private void saveZekResponseDescription(ZekResponseDescriptionDto zekResponseDescriptionDto, ZEKCMR zekcmr, DdIcExtended context)
        {
            ZEKRESDESC zekresdesc = new ZEKRESDESC();
            zekresdesc = Mapper.Map<ZekResponseDescriptionDto, ZEKRESDESC>(zekResponseDescriptionDto);
            context.ZEKRESDESC.Add(zekresdesc);
            zekresdesc.ZEKCMR = zekcmr;

            if (zekResponseDescriptionDto.FoundPerson != null)
            {
                ZEKADRDESC zekadrdesc = new ZEKADRDESC();
                zekadrdesc = Mapper.Map<ZekAddressDescriptionDto, ZEKADRDESC>(zekResponseDescriptionDto.FoundPerson);
                context.ZEKADRDESC.Add(zekadrdesc);
                zekadrdesc.ZEKRESDESC = zekresdesc;
            }

            if (zekResponseDescriptionDto.Synonyms != null)
            {
                foreach (ZekAddressDescriptionDto zekAddressDescriptionDto in zekResponseDescriptionDto.Synonyms)
                {
                    ZEKADRDESC zekadrdescsy = new ZEKADRDESC();
                    zekadrdescsy = Mapper.Map<ZekAddressDescriptionDto, ZEKADRDESC>(zekAddressDescriptionDto);
                    context.ZEKADRDESC.Add(zekadrdescsy);
                    zekadrdescsy.ZEKRESDESC = zekresdesc;
                }
            }

            if (zekResponseDescriptionDto.FoundContracts != null)
                zekresdesc.ZEKFC = saveZekFoundContracts(zekResponseDescriptionDto.FoundContracts, context);
        }


        private ZEKFC saveZekFoundContracts(ZekFoundContractsDto zekfoundContractsDto, DdIcExtended context)
        {
            if (zekfoundContractsDto != null)
            {
                ZEKFC zekfc = new ZEKFC();
                zekfc.GESAMTENGAGEMENT = (decimal?)zekfoundContractsDto.GesamtEngagement;
                context.ZEKFC.Add(zekfc);

                if (zekfoundContractsDto.AmtsinformationContracts != null)
                {
                    foreach (ZekAmtsinformationDescriptionDto zekAmtsinformationDescriptionDto in zekfoundContractsDto.AmtsinformationContracts)
                    {
                        ZEKAIC zekaic = new ZEKAIC();
                        zekaic = Mapper.Map<ZekAmtsinformationDescriptionDto, ZEKAIC>(zekAmtsinformationDescriptionDto);
                        context.ZEKAIC.Add(zekaic);
                        zekaic.ZEKFC = zekfc;
                    }
                }

                if (zekfoundContractsDto.BardarlehenContracts != null)
                {
                    foreach (ZekBardarlehenDescriptionDto zekBardarlehenDescriptionDto in zekfoundContractsDto.BardarlehenContracts)
                    {
                        ZEKBDC zekbdc = new ZEKBDC();
                        zekbdc = Mapper.Map<ZekBardarlehenDescriptionDto, ZEKBDC>(zekBardarlehenDescriptionDto);
                        context.ZEKBDC.Add(zekbdc);
                        zekbdc.ZEKFC = zekfc;
                    }
                }

                if (zekfoundContractsDto.FestkreditContracts != null)
                {
                    foreach (ZekFestkreditDescriptionDto zekFestkreditDescriptionDto in zekfoundContractsDto.FestkreditContracts)
                    {
                        ZEKFKC zekfkc = new ZEKFKC();
                        zekfkc = Mapper.Map<ZekFestkreditDescriptionDto, ZEKFKC>(zekFestkreditDescriptionDto);
                        context.ZEKFKC.Add(zekfkc);
                        zekfkc.ZEKFC = zekfc;
                    }
                }

                if (zekfoundContractsDto.GesamtEngagement != null)
                {
                }

                if (zekfoundContractsDto.KartenengagementContracts != null)
                {
                    foreach (ZekKartenengagementDescriptionDto zekKartenengagementDescriptionDto in zekfoundContractsDto.KartenengagementContracts)
                    {
                        ZEKKEC zekkec = new ZEKKEC();
                        zekkec = Mapper.Map<ZekKartenengagementDescriptionDto, ZEKKEC>(zekKartenengagementDescriptionDto);
                        context.ZEKKEC.Add(zekkec);
                        zekkec.ZEKFC = zekfc;
                    }
                }

                if (zekfoundContractsDto.KarteninformationContracts != null)
                {
                    foreach (ZekKarteninformationDescriptionDto zekKarteninformationDescriptionDto in zekfoundContractsDto.KarteninformationContracts)
                    {
                        ZEKKIC zekkic = new ZEKKIC();
                        zekkic = Mapper.Map<ZekKarteninformationDescriptionDto, ZEKKIC>(zekKarteninformationDescriptionDto);
                        context.ZEKKIC.Add(zekkic);
                        zekkic.ZEKFC = zekfc;
                    }
                }

                if (zekfoundContractsDto.KontokorrentkreditContracts != null)
                {
                    foreach (ZekKontokorrentkreditDescriptionDto zekKontokorrentkreditDescriptionDto in zekfoundContractsDto.KontokorrentkreditContracts)
                    {
                        ZEKKKC zekkkc = new ZEKKKC();
                        zekkkc = Mapper.Map<ZekKontokorrentkreditDescriptionDto, ZEKKKC>(zekKontokorrentkreditDescriptionDto);
                        context.ZEKKKC.Add(zekkkc);
                        zekkkc.ZEKFC = zekfc;
                    }
                }


                if (zekfoundContractsDto.KreditgesuchContracts != null)
                {
                    foreach (ZekKreditgesuchDescriptionDto zekKreditgesuchDescriptionDto in zekfoundContractsDto.KreditgesuchContracts)
                    {
                        ZEKKGC zekkgc = new ZEKKGC();
                        zekkgc = Mapper.Map<ZekKreditgesuchDescriptionDto, ZEKKGC>(zekKreditgesuchDescriptionDto);
                        context.ZEKKGC.Add(zekkgc);
                        zekkgc.ZEKFC = zekfc;
                    }
                }

                if (zekfoundContractsDto.LeasingMietvertragContracts != null)
                {
                    foreach (ZekLeasingMietvertragDescriptionDto zekLeasingMietvertragDescriptionDto in zekfoundContractsDto.LeasingMietvertragContracts)
                    {
                        ZEKLMC zeklmc = new ZEKLMC();
                        zeklmc = Mapper.Map<ZekLeasingMietvertragDescriptionDto, ZEKLMC>(zekLeasingMietvertragDescriptionDto);
                        context.ZEKLMC.Add(zeklmc);
                        zeklmc.ZEKFC = zekfc;
                    }
                }

                if (zekfoundContractsDto.SolidarschuldnerContracts != null)
                {
                    foreach (ZekSolidarschuldnerDescriptionDto zekSolidarschuldnerDescriptionDto in zekfoundContractsDto.SolidarschuldnerContracts)
                    {
                        ZEKSSC zekssc = new ZEKSSC();
                        zekssc = Mapper.Map<ZekSolidarschuldnerDescriptionDto, ZEKSSC>(zekSolidarschuldnerDescriptionDto);
                        context.ZEKSSC.Add(zekssc);
                        zekssc.ZEKFC = zekfc;
                    }
                }

                if (zekfoundContractsDto.TeilzahlungskreditContracts != null)
                {
                    foreach (ZekTeilzahlungskreditDescriptionDto zekTeilzahlungskreditDescriptionDto in zekfoundContractsDto.TeilzahlungskreditContracts)
                    {
                        ZEKTKC zektkc = new ZEKTKC();
                        zektkc = Mapper.Map<ZekTeilzahlungskreditDescriptionDto, ZEKTKC>(zekTeilzahlungskreditDescriptionDto);
                        context.ZEKTKC.Add(zektkc);
                        zektkc.ZEKFC = zekfc;
                    }
                }

                if (zekfoundContractsDto.TeilzahlungsvertragContracts != null)
                {
                    foreach (ZekTeilzahlungsvertragDescriptionDto zekTeilzahlungsvertragDescriptionDto in zekfoundContractsDto.TeilzahlungsvertragContracts)
                    {
                        ZEKTKC zektkc = new ZEKTKC();
                        zektkc = Mapper.Map<ZekTeilzahlungsvertragDescriptionDto, ZEKTKC>(zekTeilzahlungsvertragDescriptionDto);
                        context.ZEKTKC.Add(zektkc);
                        zektkc.ZEKFC = zekfc;
                    }
                }

                if (zekfoundContractsDto.UeberziehnungskreditContracts != null)
                {
                    foreach (ZekUeberziehungskreditDescriptionDto zekUeberziehungskreditDescriptionDto in zekfoundContractsDto.UeberziehnungskreditContracts)
                    {
                        ZEKUKC zekukc = new ZEKUKC();
                        zekukc = Mapper.Map<ZekUeberziehungskreditDescriptionDto, ZEKUKC>(zekUeberziehungskreditDescriptionDto);
                        context.ZEKUKC.Add(zekukc);
                        zekukc.ZEKFC = zekfc;
                    }
                }
                return zekfc;
            }
            return null;
        }


        private ZEKINPEC5 getZekInpEC5(long sysAuskunft, DdIcExtended context)
        {
            ZEKINPEC5 zekInpEC5 = new ZEKINPEC5();
            var ec5Query = from zekinpec5 in context.ZEKINPEC5
                           join Auskunft in context.AUSKUNFT on zekinpec5.AUSKUNFT.SYSAUSKUNFT equals Auskunft.SYSAUSKUNFT
                           where Auskunft.SYSAUSKUNFT == sysAuskunft
                           select zekinpec5;
            zekInpEC5 = ec5Query.Single();
            return zekInpEC5;
        }

        private ZEKINPEC7 getZekInpEC7(long sysAuskunft, DdIcExtended context)
        {
            ZEKINPEC7 zekInpEC7 = new ZEKINPEC7();
            var ec7Query = from zekinpec7 in context.ZEKINPEC7
                           join Auskunft in context.AUSKUNFT on zekinpec7.AUSKUNFT.SYSAUSKUNFT equals Auskunft.SYSAUSKUNFT
                           where Auskunft.SYSAUSKUNFT == sysAuskunft
                           select zekinpec7;
            zekInpEC7 = ec7Query.Single();
            return zekInpEC7;
        }

        private List<ZekRequestEntityDto> getRequestEntities(List<ZEKREQEN> zekreqenList, DdIcExtended context)
        {
            List<ZekRequestEntityDto> requestEntities = null;

            if (zekreqenList != null && zekreqenList.Count != 0)
            {
                requestEntities = new List<ZekRequestEntityDto>();
                foreach (ZEKREQEN zekreqen in zekreqenList)
                {
                    ZekRequestEntityDto zekreqenDto = Mapper.Map<ZEKREQEN, ZekRequestEntityDto>(zekreqen);

                    var addescQuery = from zekadrdesc in context.ZEKADRDESC // Selektiere alle AddrDescr
                                      where zekadrdesc.ZEKREQEN.SYSZEKREQEN == zekreqen.SYSZEKREQEN// der gesuchten sysAuskunft entsprechen.
                                      select zekadrdesc;
                    ZEKADRDESC adrdesc = addescQuery.Single();
                    zekreqenDto.AddressDescription = Mapper.Map<ZEKADRDESC, ZekAddressDescriptionDto>(adrdesc);
                    //Plausibilisierung ZEK Meldungen /BNRSIEBEN-1316
                    if (zekreqenDto.AddressDescription != null)
                    {
                        zekreqenDto.AddressDescription.FoundingDate = MyFoundingDate(zekreqenDto.AddressDescription.LegalForm, zekreqenDto.AddressDescription.FoundingDate);
                        zekreqenDto.AddressDescription.DatumWohnhaftSeit = MyWohnhaftSeit(zekreqenDto.AddressDescription.LegalForm, zekreqenDto.AddressDescription.DatumWohnhaftSeit);
                    }
                    requestEntities.Add(zekreqenDto);
                }
            }
            return requestEntities;
        }

        private ZekRequestEntityDto getRequestEntity(ZEKREQEN zekreqen, DdIcExtended context)
        {
            ZekRequestEntityDto requestEntity = Mapper.Map<ZEKREQEN, ZekRequestEntityDto>(zekreqen);
            var addescQuery = from zekadrdesc in context.ZEKADRDESC  // Selektiere alle AddrDescr
                              where zekadrdesc.ZEKREQEN.SYSZEKREQEN == zekreqen.SYSZEKREQEN // der gesuchten sysAuskunft entsprechen.
                              select zekadrdesc;
            ZEKADRDESC adrdesc = addescQuery.Single();

            requestEntity.AddressDescription = Mapper.Map<ZEKADRDESC, ZekAddressDescriptionDto>(adrdesc);
            //Plausibilisierung ZEK Meldungen /BNRSIEBEN-1316
            if (requestEntity.AddressDescription != null)
            {
                requestEntity.AddressDescription.FoundingDate = MyFoundingDate(requestEntity.AddressDescription.LegalForm, requestEntity.AddressDescription.FoundingDate);
                requestEntity.AddressDescription.DatumWohnhaftSeit = MyWohnhaftSeit(requestEntity.AddressDescription.LegalForm, requestEntity.AddressDescription.DatumWohnhaftSeit);
            }

            return requestEntity;
        }

        /// <summary>
        /// truncateErrText
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        private string MyTruncateErrText(string s)
        {
            if (s != null)
                if (s.Length > 254)
                    return s.Substring(0, 254) + "+";
                else
                    return s;
            else
                return s;
        }

        /// <summary>
        /// MyFoundingDate
        /// </summary>
        /// <param name="legalForm"></param>
        /// <param name="foundingDate"></param>
        /// <returns></returns>
        private string MyFoundingDate(int legalForm, string foundingDate)
        {
            if (legalForm <= 1)
                return null;
            return foundingDate;
        }

        /// <summary>
        /// MyWohnhaftSeit
        /// </summary>
        /// <param name="legalForm"></param>
        /// <param name="wohnhaftSeit"></param>
        /// <returns></returns>
        private string MyWohnhaftSeit(int legalForm, string wohnhaftSeit)
        {
            if (legalForm > 1)
                return null;
            return wohnhaftSeit;
        }

        #endregion Private Methods
    }
}