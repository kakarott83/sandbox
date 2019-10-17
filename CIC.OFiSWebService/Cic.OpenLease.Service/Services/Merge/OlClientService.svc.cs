// OWNER MK, 22-07-2009
namespace Cic.OpenLease.Service.Services.Merge.OpenLease
{
    #region Using
    using Cic.OpenLease.ServiceAccess;
    using Cic.OpenLease.ServiceAccess.DdOw;
    using Cic.OpenLease.ServiceAccess.Merge.OlClient;
    using Cic.OpenOne.Common.Model.DdOl;
    using Cic.OpenOne.Common.Model.DdOw;
    using Cic.OpenOne.Common.Util.Collection;
    using Cic.OpenOne.Common.Util.Config;
    using Cic.OpenOne.Common.Util.Logging;
    using Cic.OpenOne.GateBANKNOW.Common.BO;
    using Dapper;
    using Devart.Data.Oracle;
    using System;
    using System.Collections.Generic;
    using System.Data.Common;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    /*using DMSDOC = Model.DdOw.DMSDOC;
using DMSDOCAREA = Model.DdOw.DMSDOCAREA;
using WFTX = Model.DdOw.WFTX;
*/
    #endregion

    [System.CLSCompliant(true)]
    [System.ServiceModel.ServiceBehavior(Namespace = "http://cic-software.de/Cic.OpenLease.Service.Services.Merge.OpenLease")]
    [Cic.OpenOne.Common.Util.Behaviour.SOAPMessageInspection]
    public class OlClientService : IOlClientService
    {
        #region Private variables
        private static readonly ILog _Log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private const string CnstDocumentsCfgCode = "DOKUMENTE";
        private const long UPL_DOC_ID = 8999;
        #endregion

        #region IOlClientService Members
      /*  Cic.OpenLease.ServiceAccess.Merge.OlClient.HotOutputDto Cic.OpenLease.ServiceAccess.Merge.OlClient.IOlClientService.Execute(Cic.OpenLease.ServiceAccess.Merge.OlClient.HotInputDto hotInputDto)
        {
            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateExecute();

            try
            {
              //  Cic.OpenLease.Client.Executor Executor;

                //Executor = new Cic.OpenLease.Client.Executor();

                Cic.OpenLease.ServiceAccess.Merge.OlClient.HotOutputDto HotOutputDto;

                HotOutputDto = new Cic.OpenLease.ServiceAccess.Merge.OlClient.HotOutputDto();

               // HotOutputDto.EAIHOT = Executor.Execute(hotInputDto.EAIHOT);

                return HotOutputDto;
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.OlClientExecuteFailed, exception);

                // Log the exception
                _Log.Error(TopLevelException.Message, TopLevelException);

                // Throw the exception
                throw TopLevelException;
            }
        }*/

        /// <summary>
        /// HCBE Deliver Documents list
        /// </summary>
        /// <param name="sysAreaId"></param>
        /// <returns></returns>
        public DocumentShortDto[] DeliverDocumentsListNew(long sysAreaId)
        {
            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateView();


            try
            {

                IPrintAngAntBo bo = BOFactory.getInstance().createPrintAngAntBo();
                String subArea = "ANGEBOT";
                using (DdOlExtended Context = new DdOlExtended())
                {
                    if (!Cic.OpenLease.Service.PEUNIHelper.IsInSightField(Context, ServiceValidator.SYSPUSER, Cic.OpenLease.Service.PEUNIHelper.Areas.ANGEBOT, sysAreaId))
                        throw new Exception("No Permission to ANGEBOT");
                }
                Cic.OpenOne.GateBANKNOW.Common.DTO.DokumenteDto[] commonDto = bo.listAvailableDokumente(Cic.OpenOne.GateBANKNOW.Common.DTO.Enums.EaiHotOltable.Angebot, sysAreaId,
                                                                                ServiceValidator.SYSPUSER, "de-DE", subArea,true,ServiceValidator.SysPEROLE);

                List<DocumentShortDto> Documents = new List<DocumentShortDto>();

                foreach (Cic.OpenOne.GateBANKNOW.Common.DTO.DokumenteDto d in commonDto)
                {
                    DocumentShortDto dto = new DocumentShortDto();
                    dto.Area = AreaConstants.Angebot;
                    dto.Name = "" + d.sysEaiquo;
                    dto.Description = d.Bezeichnung;
                    dto.Druck = d.Druck;//default auswahl
                    dto.Section = "Kunde";
                    if (d.MAExemplar > 0)
                        dto.Section = "Darlehensnehmer 2";
                    else if (d.BGExemplar > 0)
                        dto.Section = "Bürge";
                    else if (d.BnowMitarbeiterExemplar > 0)
                        dto.Section = "Bank";
                    else if (d.VertriebspatnerExemplar > 0)
                        dto.Section = "Händler";
                    Documents.Add(dto);
                }

                // Return the documents list as an array
                return Documents.ToArray();
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverDocumentsListFailed, exception);

                // Log the exception
                _Log.Error(TopLevelException.Message, TopLevelException);

                // Throw the exception
                throw TopLevelException;
            }
        }
        public DocumentShortDto[] DeliverDocumentsList(AreaConstants area)
        {
            return DeliverDocumentsListNew(8192);
        }

        /// <summary>
        /// Returns all uploaded documents for the offer
        /// </summary>
        /// <param name="sysangebot"></param>
        /// <returns></returns>
        public DocumentListDto[] DeliverUploadedDocuments(long sysangebot)
        {
            try
            {
                if (sysangebot == 0) return new DocumentListDto[0];

                ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, MethodBase.GetCurrentMethod().Name);
                using (DdOlExtended Context = new DdOlExtended())
                {
                    if (!Cic.OpenLease.Service.PEUNIHelper.IsInSightField(Context, ServiceValidator.SYSPUSER, Cic.OpenLease.Service.PEUNIHelper.Areas.ANGEBOT, sysangebot))
                        throw new Exception("No Permission to ANGEBOT");
                }
                String Query = "select sysdmsdoc,dateiname filename, bemerkung remark, searchterms from dmsdoc where sysdmsdoc in (select distinct dmsdoc.sysdmsdoc from dmsdoc,dmsdocarea where dmsdocarea.sysdmsdoc=dmsdoc.sysdmsdoc and syswftx="+ UPL_DOC_ID + " and ((dmsdocarea.area='ANGEBOT' and dmsdocarea.sysid=:sysangebot) or (dmsdocarea.area='ANTRAG' and dmsdocarea.sysid=:sysantrag)))";
                
                using (DdOwExtended owCtx = new DdOwExtended())
                {

                   

                    long sysantrag = owCtx.ExecuteStoreQuery<long>("select sysid from antrag where sysangebot="+sysangebot,null).FirstOrDefault();
                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysangebot", Value = sysangebot });
                    if (sysantrag == 0)
                        sysantrag = -1;
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysantrag", Value = sysantrag });

                    return owCtx.ExecuteStoreQuery<DocumentListDto>(Query, parameters.ToArray()).ToArray();
                }
            }catch(Exception ex)
            {
                _Log.Error("Error fetching documents", ex);
                return null;
            }
        }

        /// <summary>
        /// Uploads a document to dmsdoc/dmsdocarea
        /// </summary>
        /// <param name="doc"></param>
        public String uploadDocument(DocumentUploadDto doc)
        {
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateView();
            if (doc == null) return "NO_DATA_TO_UPLOAD";
            if (doc.filedata == null ||doc.filedata.Length==0) return "NO_FILEDATA_TO_UPLOAD";
            
            try
            {
                using (DdOwExtended owCtx = new DdOwExtended())
                {
                    long wftxid = UPL_DOC_ID;
                    String titel = owCtx.ExecuteStoreQuery<String>("select doctitel from wftx where syswftx=" + wftxid, null).FirstOrDefault();

                    CIC.Database.OW.EF6.Model.DMSDOC dbdoc = new CIC.Database.OW.EF6.Model.DMSDOC();
                    owCtx.DMSDOC.Add(dbdoc);
                    dbdoc.BEMERKUNG = doc.remark;
                    dbdoc.SEARCHTERMS = doc.searchterms;
                    dbdoc.SYSWFTX= wftxid;
                    dbdoc.INHALT = doc.filedata;
                    dbdoc.DATEINAME = doc.filename;
                    dbdoc.NAME = titel;
                    owCtx.SaveChanges();

                    CIC.Database.OW.EF6.Model.DMSDOCAREA docarea = new CIC.Database.OW.EF6.Model.DMSDOCAREA();
                    docarea.DMSDOC = dbdoc;
                    docarea.SYSID = doc.sysid;
                    docarea.AREA = "ANGEBOT";
                    docarea.RANG = 1;
                    owCtx.DMSDOCAREA.Add(docarea);

                    long sysantrag = owCtx.ExecuteStoreQuery<long>("select sysantrag from angebot where sysid=" + doc.sysid, null).FirstOrDefault();
                    if (sysantrag > 0)//falls schon mit antrag verknüpft, dokument dort mitverknüpfen
                    {
                        docarea = new CIC.Database.OW.EF6.Model.DMSDOCAREA();
                        docarea.DMSDOC = dbdoc;

                        docarea.SYSID = sysantrag;
                        docarea.AREA = "ANTRAG";
                        docarea.RANG = 1;
                        owCtx.DMSDOCAREA.Add(docarea);
                    }

                    owCtx.SaveChanges();
                    
                    return "SUCCESS";
                }
            }catch(Exception e)
            {
                _Log.Error("Error uploading document",e);
                return "ERROR_" + e.Message;
            }
        }

        public long PrintDocument(DocumentShortDto document, long sysAreaId)
        {
            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateView();
            EaiParameter EaiParameter = new EaiParameter();

            try
            {
                

                // Create a context
                using (DdOlExtended Context = new DdOlExtended())
                {
                    if (!Cic.OpenLease.Service.PEUNIHelper.IsInSightField(Context, ServiceValidator.SYSPUSER, Cic.OpenLease.Service.PEUNIHelper.Areas.ANGEBOT, sysAreaId))
                        throw new Exception("No Permission to ANGEBOT");

                    // Validate the status if area is Angebot
                    /*if ((document.Area == Cic.OpenLease.ServiceAccess.AreaConstants.Angebot || document.Area == Cic.OpenLease.ServiceAccess.AreaConstants.AngebotEinreichen) && !ZustandHelper.VerifyAngebotStatus(sysAreaId, Context, AngebotZustand.Kalkuliert, AngebotZustand.Gedruckt, AngebotZustand.BONITAETSPRUEFUNG, AngebotZustand.Genehmigt, AngebotZustand.Antraganlegen))
                    {
                        String zustand = (from Angebot in Context.ANGEBOT
                                          where Angebot.SYSID == sysAreaId
                                          select Angebot.ZUSTAND).FirstOrDefault();

                        // Throw an exception
                        _Log.Error("PrintDocument Exception: Angebot Status invalid for print: " + zustand + " not in AngebotZustand.Kalkuliert, AngebotZustand.Gedruckt, AngebotZustand.BONITAETSPRUEFUNG,AngebotZustand.Genehmigt, AngebotZustand.Antraganlegen");
                        throw new Exception("Invalid angebot status for PrintDocument().");
                    }*/
                    IPrintAngAntBo bo = BOFactory.getInstance().createPrintAngAntBo();
                    String subArea = "ANGEBOT";

                    Cic.OpenOne.GateBANKNOW.Common.DTO.DokumenteDto[] commonDto = bo.listAvailableDokumente(Cic.OpenOne.GateBANKNOW.Common.DTO.Enums.EaiHotOltable.Angebot, sysAreaId,
                                                                                    ServiceValidator.SYSPUSER, "de-DE", subArea, ServiceValidator.SysPEROLE);
                    foreach (Cic.OpenOne.GateBANKNOW.Common.DTO.DokumenteDto d in commonDto)
                    {
                        String f = d.Bezeichnung;
                    }
                    long l = this.printCheckedDokumente(commonDto, sysAreaId, Cic.OpenOne.GateBANKNOW.Common.DTO.Enums.EaiHotOltable.Angebot, ServiceValidator.SYSPUSER, false);
                    
                    /*
                     * if (document.Area == AreaConstants.Angebot || document.Area == AreaConstants.AngebotEinreichen)
                      {

                          document.Area = AreaConstants.Angebot;
                          /*   Mappings fro ANGEBOT:
                           *      EaiHot.INPUTPARAMETER1 = DocumentName;
                                  EaiHot.INPUTPARAMETER4 = Angebotsnummer;
                                  EaiHot.INPUTPARAMETER5 = Kundenname;
                                  EaiHot.OUTPUTPARAMETER3 = Fahrzeug;
                                  EaiHot.OUTPUTPARAMETER4 = Finanzierungsprodukt;
                                  EaiHot.OUTPUTPARAMETER5 = Rate;
                           * 

                          DocInfoDto docInfo = Context.ExecuteStoreQuery<DocInfoDto>("select angebot.sysid, prproduct.name productName, angob.fabrikat, it.vorname, it.name, angkalk.ratebrutto, angebot.angebot from angebot, angkalk,angob,prproduct,it where angkalk.sysangebot=angebot.sysid and angob.sysob = angkalk.sysob and prproduct.sysprproduct=angebot.sysprproduct and it.sysit = angebot.sysit and Angebot.SYSID =" + sysAreaId, null).FirstOrDefault();

                          EaiParameter.INPUTPARAMETER1 = document.Name;
                          EaiParameter.INPUTPARAMETER4 = docInfo.angebot;
                          if (docInfo.ratebrutto.HasValue)
                          {
                              EaiParameter.OUTPUTPARAMETER5 = docInfo.ratebrutto.ToString();
                          }
                          EaiParameter.INPUTPARAMETER5 = docInfo.vorname + " " + docInfo.name;
                          EaiParameter.OUTPUTPARAMETER4 = docInfo.productName;
                          EaiParameter.OUTPUTPARAMETER3 = docInfo.fabrikat;
                      }
                      if (document.Area == AreaConstants.Ruecknahmeprotokoll||document.Area==AreaConstants.Vertragsuebersicht)
                      {
                          document.Area = AreaConstants.VT;
                          EaiParameter.INPUTPARAMETER1 = document.Name;
                      }*/
                }
                /*
                SysEaiHot = DocumentsHelper.PrintQueue.Add(document, sysAreaId, ServiceValidator.SYSWFUSER, ServiceValidator.ISOLanguageCode, EaiParameter);

                */
                using (DdOlExtended Context = new DdOlExtended())
                {
                    if ((document.Area == AreaConstants.Angebot || document.Area == AreaConstants.AngebotEinreichen) && ZustandHelper.VerifyAngebotStatus(sysAreaId, Context, AngebotZustand.Neu, AngebotZustand.Kalkuliert))
                    {

                        if (!ZustandHelper.VerifyAngebotStatus(sysAreaId, Context, AngebotZustand.Eingereicht, AngebotZustand.Wiedereingereicht, AngebotZustand.Genehmigt))
                        {
                            // Change the status
                            ZustandHelper.SetAngebotStatus(null,sysAreaId, Context, AngebotZustand.Gedruckt);
                        }
                    }
                }

                // Return SysEaiHot
                return 0;// SysEaiHot;
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.PrintDocumentFailed, exception);

                // Log the exception
                _Log.Error(TopLevelException.Message, exception);

                // Throw the exception
                throw TopLevelException;
            }
        }

        /// <summary>
        /// HCEB Druck der ausgewählten Dokumente
        /// </summary>
        /// <param name="document"></param>
        /// <param name="sysAreaId"></param>
        /// <returns></returns>
        public long PrintDocumentNew(DocumentShortDto[] document, long sysAreaId)
        {
            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateView();
            EaiParameter EaiParameter = new EaiParameter();

            try
            {
                long SysEaiHot;

                // Create a context
                using (DdOlExtended Context = new DdOlExtended())
                {
                    
                    if (!Cic.OpenLease.Service.PEUNIHelper.IsInSightField(Context, ServiceValidator.SYSPUSER, Cic.OpenLease.Service.PEUNIHelper.Areas.ANGEBOT, sysAreaId))
                        throw new Exception("No Permission to ANGEBOT");

                  
                    IPrintAngAntBo bo = BOFactory.getInstance().createPrintAngAntBo();
                    
                    List<Cic.OpenOne.GateBANKNOW.Common.DTO.DokumenteDto> commonDtos = new List<Cic.OpenOne.GateBANKNOW.Common.DTO.DokumenteDto>();
                    
                    foreach(DocumentShortDto ds in document)
                    {
                        if (ds.Name == null || ds.Name.Length < 1||ds.Name.Equals("null")) continue;
                        
                        Cic.OpenOne.GateBANKNOW.Common.DTO.DokumenteDto d = new OpenOne.GateBANKNOW.Common.DTO.DokumenteDto();
                        d.Druck = 1;
                        d.sysEaiquo = long.Parse(ds.Name);
                        d.DokumentenID = Context.ExecuteStoreQuery<int>("select F01 from eaiqou where syseaiqou=" + ds.Name, null).FirstOrDefault();
                        commonDtos.Add(d);
                    }
                    SysEaiHot = this.printCheckedDokumente(commonDtos.ToArray(), sysAreaId, Cic.OpenOne.GateBANKNOW.Common.DTO.Enums.EaiHotOltable.Angebot, ServiceValidator.SYSPUSER, false);
                    
                  
                }
                
                // Return SysEaiHot
                return SysEaiHot;// SysEaiHot;
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.PrintDocumentFailed, exception);

                // Log the exception
                _Log.Error(TopLevelException.Message, exception);

                // Throw the exception
                throw TopLevelException;
            }
        }


                
            

               /// <summary>
        /// Drucken ausgewähler Dokumente
        /// </summary>
        /// <param name="dokumente">Dokumentenliste</param>
        /// <param name="sysid">Primärschlüssel</param>
        /// <param name="type">Typ</param>
        /// <param name="sysperson">Person ID</param>
        /// <param name="eCodeEintragund">eCodeEintragund</param>
        /// <returns>Binaerdaten</returns>
        public long printCheckedDokumente(Cic.OpenOne.GateBANKNOW.Common.DTO.DokumenteDto[] dokumente, long sysid, Cic.OpenOne.GateBANKNOW.Common.DTO.Enums.EaiHotOltable type, long sysperson, bool eCodeEintragund)
        {
            
            Cic.OpenOne.GateBANKNOW.Common.DAO.IEaihotDao  eaihotDao = Cic.OpenOne.GateBANKNOW.Common.DAO.CommonDaoFactory.getInstance().getEaihotDao();
            CIC.Database.OW.EF6.Model.EAIART eaiArt = eaihotDao.getEaiArt("DRUCKAUFTRAG_FE");
            string oltable = "ANGEBOT";

            Cic.OpenOne.Common.DTO.EaihotDto eaihot = null;
            if (eaiArt != null)
            {
                eaihot = new Cic.OpenOne.Common.DTO.EaihotDto()
                {
                    CODE = "DRUCKAUFTRAG_FE",
                    OLTABLE = oltable,
                    SYSOLTABLE = sysid,
                    SYSEAIART = eaiArt.SYSEAIART,
                    SUBMITDATE = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDate(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null)),
                    SUBMITTIME = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null)),
                    EVE = 0,
                    PROZESSSTATUS = (int)EaiHotStatusConstants.Pending,
                    INPUTPARAMETER1 = sysperson.ToString(),
                    HOSTCOMPUTER = "*",
                    INPUTPARAMETER2 = eCodeEintragund.ToString()
                };
                eaihot = eaihotDao.createEaihot(eaihot);


                foreach (Cic.OpenOne.GateBANKNOW.Common.DTO.DokumenteDto dokument in dokumente)
                {
                    eaihotDao.createEaiqin(new Cic.OpenOne.GateBANKNOW.Common.DTO.EaiqinDto()
                    {
                        sysEaihot = eaihot.SYSEAIHOT,
                        F20 = dokument.sysEaiquo.ToString(),
                        F01 = dokument.DokumentenID.ToString(),
                        F02 = "1",
                        F03 = dokument.KundenExemplar.ToString(),
                        F04 = dokument.VertriebspatnerExemplar.ToString(),
                        F05 = dokument.BnowMitarbeiterExemplar.ToString(),
                        F06 = dokument.Druck.ToString(),
                    });
                }
                eaihot.EVE = 1;
                eaihot = eaihotDao.updateEaihot(eaihot);

                DateTime oldDate = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null);
                TimeSpan timeOut = new TimeSpan(0, 0, 1, 0);
                while (eaihot.PROZESSSTATUS != (int)EaiHotStatusConstants.Ready && !isTimeOut(oldDate, timeOut))
                {
                    eaihot = eaihotDao.getEaihot(eaihot.SYSEAIHOT);
                    System.Threading.Thread.Sleep(500);
                }
                if (eaihot.PROZESSSTATUS == (int)EaiHotStatusConstants.Ready)
                {
                    return eaihot.SYSEAIHOT;
               
                }
                else
                {
                    // Throw an exception
                    throw new ApplicationException("Could not get dokumentfile (timeout).");
                }
            }
            return 0;
        }

          /// <summary>
        /// Prüfung auf Timeout
        /// </summary>
        /// <param name="oldDate">Alter Zeitstempel</param>
        /// <param name="timeOut">NeuerZeitstempel</param>
        /// <returns>Timeout true= Timeout/false = kein Timeout</returns>
        public static bool isTimeOut(DateTime oldDate, TimeSpan timeOut)
        {
            TimeSpan ts = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null) - oldDate;

            if (ts > timeOut) return true;

            return false;
        }
        public DocumentDto DeliverPrintedDocument(long sysEaiHot)
        {
            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateView();

            try
            {
                // Return the document
                return DocumentsHelper.GetDocument(sysEaiHot, ServiceValidator.SYSPUSER);
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverPrintedDocumentFailed, exception);

                // Log the exception
                _Log.Error(TopLevelException.Message, TopLevelException);

                // Throw the exception
                throw TopLevelException;
            }
        }

        public DocumentDto[] DeliverPrintedDocuments(AreaConstants area, long sysAreaId)
        {
            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateView();

            try
            {
                if (area == AreaConstants.AngebotEinreichen)
                    area = AreaConstants.Angebot;

                // Return the documents
                return DocumentsHelper.GetDocuments(area, sysAreaId);
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.DeliverPrintedDocumentsFailed, exception);

                // Log the exception
                _Log.Error(TopLevelException.Message, TopLevelException);

                // Throw the exception
                throw TopLevelException;
            }
        }

        public List<EAIHotDto> Search(EAIHotDto searchData, Cic.OpenLease.ServiceAccess.SearchParameters searchParameters, EAIHotSortData[] sortColumns)
        {
            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateView();

            try
            {

                // Get the config entries
               /* List<CFGVAR> ConfigEntries = ConfigHelper.GetConfigEntries(CnstDocumentsCfgCode, AreaConstants.Angebot.ToString().ToUpper());
                Dictionary<string, string> dictBez = new Dictionary<string, string>();
                // Iterate through all congif entries
                foreach (var LoopConfigEntry in ConfigEntries)
                {
                    dictBez[LoopConfigEntry.CODE] = LoopConfigEntry.BEZEICHNUNG;
                }
                ConfigEntries = ConfigHelper.GetConfigEntries(CnstDocumentsCfgCode, AreaConstants.AngebotEinreichen.ToString().ToUpper());
                foreach (var LoopConfigEntry in ConfigEntries)
                {
                    dictBez[LoopConfigEntry.CODE] = LoopConfigEntry.BEZEICHNUNG;
                }*/
                using (DdOwExtended ctx = new DdOwExtended())
                {
                    String query = MyCreateQuery(ctx, searchData, ServiceValidator.SysPEROLE, ServiceValidator.SYSPUSER, false, sortColumns);
                    query = "SELECT * FROM (SELECT rownum rnum, a.* FROM(" + query + ") a WHERE rownum <= " + (searchParameters.Skip + searchParameters.Top) + ") WHERE rnum > " + searchParameters.Skip;


                    Task<List<EAIHotDto>> taskResult = SearchWithTimeout<EAIHotDto>(query, MyDeliverQueryParameters(searchData));
                    Task.FromResult(taskResult);

                    if (taskResult.Exception != null)
                        throw taskResult.Exception;

                    return taskResult.Result.ToList();

                    //List<EAIHotDto> rval = ctx.ExecuteStoreQuery<EAIHotDto>(query, MyDeliverQueryParameters(searchData)).ToList<EAIHotDto>();
                   /* foreach (EAIHotDto dto in rval)
                    {
                        if (dictBez.ContainsKey(dto.OUTPUTPARAMETER1))
                            dto.Description = dictBez[dto.OUTPUTPARAMETER1];
                        else dto.Description = "";
                    }
                    */


                    //return rval;
                }
            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.EAIHotSearchFailed, exception);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.EAIHotSearchFailed, exception);

                // Throw the exception
                throw TopLevelException;
            }

        }
   
        /// <summary>
        /// Returns the count for the search of documents
        /// </summary>
        /// <param name="searchData"></param>
        /// <returns></returns>
        public int SearchCount(EAIHotDto searchData)
        {
            // Validate
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, System.Reflection.MethodBase.GetCurrentMethod().Name);


            try
            {
                using (DdOwExtended ctx = new DdOwExtended())
                {
                    Task<List<long>> taskResult = SearchWithTimeout<long>(MyCreateQuery(ctx, searchData, ServiceValidator.SysPEROLE, ServiceValidator.SYSPUSER, true, null), MyDeliverQueryParameters(searchData));
                    Task.FromResult(taskResult);

                    if (taskResult.Exception != null)
                        throw taskResult.Exception;

                    return (int)taskResult.Result.FirstOrDefault();
                }
                /*using (DdOwExtended ctx = new DdOwExtended())
                {
                    return (int)ctx.ExecuteStoreQuery<long>(MyCreateQuery(ctx, searchData, ServiceValidator.SysPEROLE, ServiceValidator.SYSPUSER, true, null), MyDeliverQueryParameters(searchData)).FirstOrDefault<long>();
                }*/


            }
            catch (Exception exception)
            {
                // Create an exception
                Exception TopLevelException = new ServiceException(Cic.OpenLease.ServiceAccess.ServiceCodes.EAIHotSearchFailed, exception);

                // Log the exception
                _Log.Error(Cic.OpenLease.ServiceAccess.ServiceCodes.EAIHotSearchFailed, exception);

                // Throw the exception
                return 0;
            }

        }

        async Task<List<T>> SearchWithTimeout<T>(string query, object[] param)
        {
            CancellationTokenSource tokenSource = new CancellationTokenSource();
            tokenSource.CancelAfter(TimeSpan.FromSeconds(20));
            using (DdOlExtended ctx = new DdOlExtended())
            {
                try
                {


                    DbConnection con = (ctx.Database.Connection);
                    long start = (long)(DateTime.Now.TimeOfDay.TotalMilliseconds);
                    if (param != null && param.Length == 0)
                        param = null;
                    var dbArgs = new DynamicParameters();
                    if (param != null)
                        foreach (OracleParameter pair in param) dbArgs.Add(pair.ParameterName, pair.Value);
                    

                    List<T> rval = (await con.QueryAsync<T>(
                        new CommandDefinition(query, dbArgs, cancellationToken: tokenSource.Token)
                    )).ToList();

                    //List<R> rval = con.Query<R>(query, dbArgs).ToList();

                    long duration = (long)(DateTime.Now.TimeOfDay.TotalMilliseconds - start);
                    if (duration > 2000 )
                    {
                        _Log.Warn("Long Query(" + duration + "ms): " + query + " params: " + (param != null ? param.AsList().ToString() : ""));
                    }
                    return rval;
                }
                catch (Exception ex)
                {
                    _Log.Warn("Search with query failed: " + query + " " + ex.Message);
                   
                    throw;
                }
            }
        }
        #endregion

        private static CacheDictionary<String, int> countCache = CacheFactory<String, int>.getInstance().createCache(1000 * 60 * 10);

        private string MyCreateQuery(DdOwExtended context, EAIHotDto searchData, long? sysPEROLE, long sysPUSER, bool count, EAIHotSortData[] sortColumns)
        {
            System.Text.StringBuilder QueryBuilder = new System.Text.StringBuilder();
            QueryBuilder.Append("SELECT ");

            if (count)
            {
                QueryBuilder.Append("COUNT(*) ");
            }
            else
            {
                QueryBuilder.Append(" FILEFLAGOUT, CIC.CIC_SYS.to_oradate(FINISHDATE) FINISHDATE, INPUTPARAMETER1, INPUTPARAMETER2, INPUTPARAMETER3, angebot.angebot INPUTPARAMETER4, it.vorname||' '||it.name INPUTPARAMETER5, wftx.doctitel description, OUTPUTPARAMETER2, angebot.objektvt OUTPUTPARAMETER3, prproduct.NAME OUTPUTPARAMETER4, angkalk.RATEBRUTTO OUTPUTPARAMETER5, PROZESSSTATUS, EAIHOT.SYSEAIHOT, SYSOLTABLE  ");
            }
            QueryBuilder.Append(" FROM   CIC.EAIHFILE,CIC.EAIHOT left outer join angebot on (angebot.sysid=eaihot.sysoltable and oltable='ANGEBOT') left outer join wftx on wftx.syswftx=eaihot.outputparameter1, it, prproduct,angkalk WHERE CIC.EAIHFILE.SYSEAIHOT=CIC.EAIHOT.SYSEAIHOT and 1 = 1 and prproduct.sysprproduct=angebot.sysprproduct and it.sysit=angebot.sysit and angkalk.sysangebot=angebot.sysid ");


            int status = 0;
            if (searchData.PROZESSSTATUS == DocumentStatusConstants.Working)
                status = 1;
            else if (searchData.PROZESSSTATUS == DocumentStatusConstants.Ready)
                status = 2;

            if (searchData.SYSEAIHOT.HasValue) QueryBuilder.Append("AND SYSEAIHOT = :SYSEAIHOT ");
            if (searchData.OUTPUTPARAMETER1 != null) QueryBuilder.Append(" AND UPPER(OUTPUTPARAMETER1) like '%'||UPPER(:OUTPUTPARAMETER1)||'%' ");
            if (searchData.OUTPUTPARAMETER2 != null) QueryBuilder.Append(" AND UPPER(OUTPUTPARAMETER2) like '%'||UPPER(:OUTPUTPARAMETER2)||'%' ");
            if (searchData.OUTPUTPARAMETER3 != null) QueryBuilder.Append(" AND UPPER(angebot.objektvt) like '%'||UPPER(:OUTPUTPARAMETER3)||'%' ");
            if (searchData.OUTPUTPARAMETER4 != null) QueryBuilder.Append(" AND UPPER(prproduct.NAME) like '%'||UPPER(:OUTPUTPARAMETER4)||'%' ");
            if (searchData.OUTPUTPARAMETER5 != null) QueryBuilder.Append(" AND UPPER(OUTPUTPARAMETER5) like '%'||UPPER(:OUTPUTPARAMETER5)||'%' ");
            if (searchData.INPUTPARAMETER1 != null) QueryBuilder.Append(" AND UPPER(INPUTPARAMETER1) like '%'||UPPER(:INPUTPARAMETER1)||'%' ");
            if (searchData.INPUTPARAMETER2 != null) QueryBuilder.Append(" AND UPPER(INPUTPARAMETER2) like '%'||UPPER(:INPUTPARAMETER2)||'%' ");
            if (searchData.INPUTPARAMETER3 != null) QueryBuilder.Append(" AND UPPER(INPUTPARAMETER3) like '%'||UPPER(:INPUTPARAMETER3)||'%' ");
            if (searchData.INPUTPARAMETER4 != null) QueryBuilder.Append(" AND UPPER(angebot.angebot) like '%'||UPPER(:INPUTPARAMETER4)||'%' ");
            if (searchData.INPUTPARAMETER5 != null) QueryBuilder.Append(" AND UPPER(CONCAT(CONCAT(CONCAT(CONCAT(CONCAT(CONCAT(it.vorname,' '),it.name),' '),it.zusatz),it.name),it.vorname)) like '%'||UPPER(:INPUTPARAMETER5)||'%' ");

            if (searchData.VERKAEUFERNAME != null) QueryBuilder.Append(" AND SYSBERATADDB IN(SELECT SYSPERSON FROM CIC.PERSON WHERE (UPPER(CONCAT(CONCAT(CONCAT(CONCAT(CONCAT(CONCAT(vorname,' '),name),' '),zusatz),name),vorname)) LIKE '%'||UPPER(:VERKAEUFERNAME)||'%' )) ");

            //if (searchData.VERKAEUFERNAME != null) QueryBuilder.Append(" AND (UPPER(CONCAT(CONCAT(CONCAT(CONCAT(CONCAT(CONCAT(vorname,' '),name),' '),zusatz),name),vorname)) LIKE UPPER(:VERKAEUFERNAME)) ");
            
            if (searchData.ANGEBOTVON != null) QueryBuilder.Append(" AND to_char(DATANGEBOT,'yyyy-mm-dd') >= :ANGEBOTVON AND DATANGEBOT IS NOT NULL ");
            if (searchData.ANGEBOTBIS != null) QueryBuilder.Append(" AND to_char(DATANGEBOT,'yyyy-mm-dd') <= :ANGEBOTBIS AND DATANGEBOT IS NOT NULL ");
            
            if (searchData.FINISHDATE != null) QueryBuilder.Append(" AND CIC.CIC_SYS.to_oradate(FINISHDATE)=:FINISHDATE  ");
            //gelesene bzw. ungelesene:
            if (searchData.FILEFLAGOUT.HasValue)
            {

                if (searchData.FILEFLAGOUT == 0)//ungelesene
                    QueryBuilder.Append(" and nvl(fileflagout,0)=0 ");

                else//gelesene
                    QueryBuilder.Append(" and FileFlagOut=" + searchData.FILEFLAGOUT + " ");
            }
            else
            {
                QueryBuilder.Append(" and nvl(fileflagout,0)<2 ");
            }

            QueryBuilder.Append(" AND EAIHOT.PROZESSSTATUS=" + status + " ");
            QueryBuilder.Append(" AND EAIHOT.CODE = 'DRUCKAUFTRAG_FE' ");


            string area = "";
            if (searchData.area == AreaConstants.Alle)
            {
                QueryBuilder.Append("  AND OLTABLE = 'ANGEBOT' ");
                // Sight fields narrowing
                if (sysPEROLE.HasValue)
                {
                    QueryBuilder.Append(" AND sysoltable in (SELECT sysid FROM peuni, perolecache WHERE area = 'ANGEBOT' AND peuni.sysperole = perolecache.syschild AND perolecache.sysparent = "+sysPEROLE.Value+") ");
                    //QueryBuilder.Append(" AND (SYSOLTABLE IN (select * from TABLE(cic.CIC_PEROLE_UTILS.GetTabIDFromPEUNI(" + sysPUSER + ", 'ANGEBOT',sysdate))) )");// OR SYSOLTABLE IN (select * from TABLE(cic.CIC_PEROLE_UTILS.GetTabIDFromPEUNI(" + sysPUSER + ", 'ANTRAG'))) OR SYSOLTABLE IN (select * from TABLE(cic.CIC_PEROLE_UTILS.GetTabIDFromPEUNI(" + sysPUSER + ", 'IT'))))");
                }
            }
            else
            {
                if (searchData.area == AreaConstants.Angebot || searchData.area == AreaConstants.AngebotEinreichen)
                {
                    area = "ANGEBOT";
                }
                else if (searchData.area == AreaConstants.Antrag)
                {
                    area = "ANTRAG";
                }
                else if (searchData.area == AreaConstants.It)
                {
                    area = "IT";
                }
                else if (searchData.area == AreaConstants.Ruecknahmeprotokoll ||searchData.area==AreaConstants.Vertragsuebersicht)
                {
                    area = "VT";
                }
                QueryBuilder.Append("  AND OLTABLE='" + area + "' ");

                // Sight fields narrowing
                if (sysPEROLE.HasValue)
                {
                    // Narrow
                    if (area.Length > 0)
                        QueryBuilder.Append(" AND SYSOLTABLE IN (select * from TABLE(cic.CIC_PEROLE_UTILS.GetTabIDFromPEUNI(" + sysPUSER + ", '" + area + "',sysdate))) ");
                }
            }

            //Get filter
            string Filter;
            
            
            Filter = AppConfig.getValueFromDb("B2B", "FILTERS", "EAIHOT", string.Empty);

            if (Filter != null && Filter.Length > 0)
            {
                QueryBuilder.Append("AND " + Filter);
            }


            if (sortColumns != null && sortColumns.Length > 0)
            {
                int i = 0;
                QueryBuilder.Append(" ORDER BY ");
                // Order
                foreach (EAIHotSortData sortCol in sortColumns)
                {
                    QueryBuilder.Append(sortCol.SortDataConstant.ToString() + " " + sortCol.SortOrderConstant.ToString());
                    if (i != sortColumns.Length - 1)
                    {
                        QueryBuilder.Append(", ");
                    }
                    i++;
                }
            }
            else
            {
                // Default Order
                if (!count)
                    QueryBuilder.Append(" ORDER BY SYSOLTABLE, FILEFLAGOUT, INPUTPARAMETER4");
            }
            _Log.Debug("Query: " + QueryBuilder.ToString());
            return QueryBuilder.ToString();
        }

        private object[] MyDeliverQueryParameters(EAIHotDto searchData)
        {

            System.Collections.Generic.List<Devart.Data.Oracle.OracleParameter> ParametersList = new List<Devart.Data.Oracle.OracleParameter>();
            if (searchData.SYSEAIHOT.HasValue) ParametersList.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "SYSEAIHOT", Value = searchData.SYSEAIHOT });
            if (searchData.OUTPUTPARAMETER1 != null) ParametersList.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "OUTPUTPARAMETER1", Value = searchData.OUTPUTPARAMETER1 });
            if (searchData.OUTPUTPARAMETER2 != null) ParametersList.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "OUTPUTPARAMETER2", Value = searchData.OUTPUTPARAMETER2 });
            if (searchData.OUTPUTPARAMETER3 != null) ParametersList.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "OUTPUTPARAMETER3", Value = searchData.OUTPUTPARAMETER3 });
            if (searchData.OUTPUTPARAMETER4 != null) ParametersList.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "OUTPUTPARAMETER4", Value = searchData.OUTPUTPARAMETER4 });
            if (searchData.OUTPUTPARAMETER5 != null) ParametersList.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "OUTPUTPARAMETER5", Value = searchData.OUTPUTPARAMETER5 });
            if (searchData.INPUTPARAMETER1 != null) ParametersList.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "INPUTPARAMETER1", Value = searchData.INPUTPARAMETER1 });
            if (searchData.INPUTPARAMETER2 != null) ParametersList.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "INPUTPARAMETER2", Value = searchData.INPUTPARAMETER2 });
            if (searchData.INPUTPARAMETER3 != null) ParametersList.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "INPUTPARAMETER3", Value = searchData.INPUTPARAMETER3 });
            if (searchData.INPUTPARAMETER4 != null) ParametersList.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "INPUTPARAMETER4", Value = searchData.INPUTPARAMETER4 });
            if (searchData.INPUTPARAMETER5 != null) ParametersList.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "INPUTPARAMETER5", Value = searchData.INPUTPARAMETER5 });
            //if (searchData.PROZESSSTATUS != null) ParametersList.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "PROZESSSTATUS", Value = searchData.PROZESSSTATUS });
            if (searchData.FINISHDATE != null) ParametersList.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "FINISHDATE", Value = searchData.FINISHDATE });

            if (searchData.VERKAEUFERNAME != null) ParametersList.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "VERKAEUFERNAME", Value = searchData.VERKAEUFERNAME });
            
            if (searchData.ANGEBOTVON != null) ParametersList.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "ANGEBOTVON", Value = searchData.ANGEBOTVON.Value.Date.ToString("yyyy-MM-dd") });
            if (searchData.ANGEBOTBIS != null) ParametersList.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "ANGEBOTBIS", Value = searchData.ANGEBOTBIS.Value.Date.ToString("yyyy-MM-dd") });

            return ParametersList.ToArray();
        }

       
        // Returns the current activity state for the current user
       
       /* public ActivityInfo getActivityInfos()
        {
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateExecute();
            ActivityInfo rval = new ActivityInfo();
            rval.ACTIVITYCOUNT = 0;

            rval.EOTCOUNT = 0;
            rval.CAMPCOUNT = 0;
            using (Cic.OpenLease.Model.DdOw.OwExtendedEntities Context = new Cic.OpenLease.Model.DdOw.OwExtendedEntities())
            {
                rval.ACTIVITYCOUNT = getDueActivities();

                rval.EOTCOUNT = Context.ExecuteStoreQuery<int>("select count(*) from CIC.OPPO OPPO left outer join camp on  camp.syscamp=oppo.syscamp left outer join wfuser wf on wf.syswfuser=oppo.syswfuser , vt, person kd, person vk, person hd, ob,obini,gebiete_v gebiete, lsadd,iam,iamtype where  kd.sysperson=vt.syskd and vk.sysperson=vt.sysberataddb and hd.sysperson=vt.sysvpfil and vt.sysid=oppo.sysid and oppo.area='VT' and ob.sysvt=vt.sysid and obini.sysobini=ob.sysob and gebiete.sysperson=vt.sysvpfil and lsadd.syslsadd=vt.sysls and oppo.sysiam=iam.sysiam and iamtype.sysiamtype = iam.sysiamtype and vt.sysid in (SELECT sysid FROM peuni, perolecache WHERE area = 'VT' AND peuni.sysperole = perolecache.syschild AND perolecache.sysparent = " + ServiceValidator.SysPEROLE + ") and iamtype.code='EOT'", null).FirstOrDefault<int>();
                rval.CAMPCOUNT = Context.ExecuteStoreQuery<int>("select count(*) from CIC.OPPO OPPO left outer join camp on  camp.syscamp=oppo.syscamp left outer join wfuser wf on wf.syswfuser=oppo.syswfuser , vt, person kd, person vk, person hd, ob,obini,gebiete_v gebiete, lsadd,iam,iamtype where  kd.sysperson=vt.syskd and vk.sysperson=vt.sysberataddb and hd.sysperson=vt.sysvpfil and vt.sysid=oppo.sysid and oppo.area='VT' and ob.sysvt=vt.sysid and obini.sysobini=ob.sysob and gebiete.sysperson=vt.sysvpfil and lsadd.syslsadd=vt.sysls and oppo.sysiam=iam.sysiam and iamtype.sysiamtype = iam.sysiamtype and vt.sysid in (SELECT sysid FROM peuni, perolecache WHERE area = 'VT' AND peuni.sysperole = perolecache.syschild AND perolecache.sysparent = " + ServiceValidator.SysPEROLE + ") and iamtype.code='CAMP' and camp.status>1 and 3>camp.status and (camp.privateFlag=0 or (camp.privateFlag=1 and not(letterflag=1))) and (camp.hoflag=0 or ((kd.privatflag=1 and oppo.status>0) or (kd.privatflag!=1)))", null).FirstOrDefault<int>();
            }
            return rval;
        }*/

        /// <summary>
        /// Returns the amount of due activities for the current user
        /// </summary>
        /// <returns></returns>
        public int getDueActivities()
        {
            ServiceValidator ServiceValidator = new ServiceValidator(this.GetType().Name, MethodBase.GetCurrentMethod().Name);
            ServiceValidator.ValidateExecute();
            return 0;
            /*
            using (Cic.OpenLease.Model.DdOw.OwExtendedEntities Context = new Cic.OpenLease.Model.DdOw.OwExtendedEntities())
            {
                if (!countCache.ContainsKey("DUE"+ServiceValidator.SysPEROLE))
                {
                    int due = Context.ExecuteStoreQuery<int>(@"select count(*) from vt,OPPOTASK left outer join oppo on oppo.sysoppo=oppotask.sysoppo left outer join camp on  camp.syscamp=oppo.syscamp,gebiete_v gebiete,iam,iamtype, CIC.PERSON KD where 
 vt.sysid=OPPOTASK.sysid and OPPOTASK.area='VT'  and gebiete.sysperson=vt.sysvpfil and oppo.sysoppo=oppotask.sysoppo and oppo.sysiam=iam.sysiam and iamtype.sysiamtype = iam.sysiamtype and kd.sysperson=vt.syskd 
and ((oppotask.duedate is null or oppotask.duedate<=sysdate)  or oppotask.changedflag=1) and 2>oppotask.status and ((iamtype.code='CAMP' and camp.status>1 and 3>camp.status and (camp.privateFlag=0 or (camp.privateFlag=1 and not(letterflag=1))) and (camp.hoflag=0 or ((kd.privatflag=1 and oppo.status>0) or (kd.privatflag!=1)))   ) or (iamtype.code='EOT')) and vt.sysid in (SELECT sysid FROM peuni, perolecache WHERE area = 'VT' AND peuni.sysperole = perolecache.syschild AND perolecache.sysparent =  " + ServiceValidator.SysPEROLE + ")", null).FirstOrDefault<int>();

                    countCache["DUE" + ServiceValidator.SysPEROLE] = due;
                }
                return countCache["DUE" + ServiceValidator.SysPEROLE];
            }*/
        }



    }


    class DocInfoDto
    {
        public String productName { get; set; }
        public String fabrikat { get; set; }
        public String vorname { get; set; }
        public String name { get; set; }
        public String angebot { get; set; }
        public decimal? ratebrutto { get; set; }
    }
}
