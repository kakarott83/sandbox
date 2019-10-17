using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.Common.Model.DdOw;
using Cic.OpenOne.Common.Util;
using AutoMapper;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Util.Config;
using Cic.OpenOne.GateBANKNOW.Common.BO;
using Cic.OpenOne.Common.Util.Logging;
using System.Reflection;
using CIC.Database.OW.EF6.Model;

namespace Cic.OpenOne.GateBANKNOW.Common.DAO
{
    public class FileDao : IFileDao
    {


        const string QUERYGETSYSDMSDOCAREA = "Select * from DMSDOCAREA where sysdmsdocarea = :parea and sysid = :sysid";
        const string QUERYGETDMSDOC = "Select * from DMSDOC where sysdmsdoc = :psysdmsdoc";
        const string QUERYUPDATEDMSDOCAREA = "Update DMSDOCAREA set area = :parea where sysdmsdocarea = :psysdmsdocarea";


        const string QUERYGETOHNEIHNHALT = "Select DMSDOCAREA.AREA area, DMSDOCAREA.SYSID sysId, DMSDOCAREA.SYSDMSDOC sysFile, length(DMSDOC.INHALT) fileSize, dmsdoc.sysdmsdoc, DMSDOC.DATEINAME filename, case when DMSDOC.ungueltigflag > 0 then 0 else 1 end activFlag, DMSDOC.UNGUELTIGFLAG,  DMSDOC.BEMERKUNG description, DMSDOC.GEDRUCKTVON syscrtuser, DMSDOC.GEDRUCKTAM sysCrtDate,  DMSDOC.GEDRUCKTUM sysCrtTime " +
                                          " from DMSDOCAREA, DMSDOC  where dmsdocarea.sysdmsdoc = dmsdoc.sysdmsdoc and dmsdocarea.sysdmsdoc = :psysdmsdoc";


        const string QUERYGETSYSIDANTRAG = "Select sysid from antrag where sysangebot = :psysid";
        const string QUERYGETSYSIDANGEBOT = "Select sysid from angebot where sysantrag = :psysid";
        private ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);


        /// <summary>
        /// createOrUpdateFile
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public FileDto createOrUpdateFile(FileDto file)
        {
            long? sysprchannel;
            using (DdOlExtended context = new DdOlExtended())
            {
                var id = (from a in context.ANTRAG
                          where a.SYSID == file.sysId
                          select a.SYSPRCHANNEL).FirstOrDefault();
                sysprchannel = id;
            }

            FileDto fileoutput = new FileDto();
            long sysdmsdoc = 0;
            bool newFile = false;
            using (DdOwExtended ctxOw = new DdOwExtended())
            {
                try
                {
                    DMSDOC dmsdoc = null;
                    DMSDOCAREA docarea = null;


                    if (file.sysFile == 0 && file.sysId > 0 && file.area != "")
                    {
                        file.sysCrtTime = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                        file.sysCrtDate = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDateNoTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                        file.activFlag = true;
                        dmsdoc = new DMSDOC();
                        dmsdoc = Mapper.Map<FileDto, DMSDOC>(file, dmsdoc);
                        dmsdoc.UNGUELTIGFLAG = 0;
                        dmsdoc.GEDRUCKTVON = file.syscrtuser.ToString();
                        dmsdoc.SEARCHTERMS = "FORMALITAETEN";
                        ctxOw.DMSDOC.Add(dmsdoc);
                        newFile = true;
                        docarea = new DMSDOCAREA();
                        docarea = Mapper.Map<FileDto, DMSDOCAREA>(file, docarea);
                        docarea.DMSDOC = dmsdoc;
                        docarea.RANG = 0;
                        ctxOw.DMSDOCAREA.Add(docarea);

                        ctxOw.SaveChanges();
                        sysdmsdoc = dmsdoc.SYSDMSDOC;

                    }
                    else
                    {
                        if (file.sysFile > 0)
                        {

                            List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysid", Value = file.sysId });
                            dmsdoc = ctxOw.ExecuteStoreQuery<DMSDOC>(QUERYGETDMSDOC, parameters.ToArray()).FirstOrDefault();


                            if (dmsdoc != null)
                            {
                                Mapper.Map<FileDto, DMSDOC>(file, dmsdoc);
                                ctxOw.SaveChanges();
                                sysdmsdoc = dmsdoc.SYSDMSDOC;
                            }

                            else throw new Exception("File with id " + dmsdoc.SYSDMSDOC + " not found!");
                        }
                        else throw new Exception("Area or Sysid not found");
                    }

                    long sysAngebot = getSysidAngebot(file.sysId);
                    CreateDmsAkte(ctxOw, file.syswfuser, sysAngebot, file.sysId ?? 0, dmsdoc);
                }
                catch (ArgumentException ex)
                {
                    throw new ArgumentException("Could not create or update upload", ex);
                }


            }

            using (DdOwExtended context = new DdOwExtended())
            {


                DateTime currDateTime = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null);
                int currTime = DateTimeHelper.DateTimeToClarionTime(currDateTime);

                try
                {
                    WFTZUST wftZust = (from wftzust in context.WFTZUST
                                       where wftzust.WFTABLE.SYSWFTABLE == 117
                                       where wftzust.SYSLEASE == file.sysId
                                       where wftzust.WFZUST.SYSWFZUST == 258
                                       select wftzust).FirstOrDefault();

                    if (wftZust == null)
                    {

                        long syswftable = context.ExecuteStoreQuery<long>("select syswftable from wftable where syscode='ANTRAG'", null).FirstOrDefault();
                        long syswfzust = context.ExecuteStoreQuery<long>("select syswfzust from wfzust where syscode='INBOXEN'", null).FirstOrDefault();

                        // Neuer Zustand
                        wftZust = new WFTZUST();
                        wftZust.SYSLEASE = file.sysId;
                        wftZust.SYSWFTABLE = syswftable;
                        wftZust.SYSWFZUST = syswfzust;
                        wftZust.STATE = "INBOXEN";
                        wftZust.BEZEICHNUNG = "Inboxen";
                        wftZust.STATUS = 0;
                        wftZust.COUNTER = 1;
                        wftZust.CREATEDATE = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDateNoTime(currDateTime);
                        wftZust.CREATETIME = currTime;
                        wftZust.CREATEBY = file.syscrtuser;
                        wftZust.CREATEWFG = "B2B";
                        wftZust.GEBIET = "Antrag";

                        context.WFTZUST.Add(wftZust);
                        context.SaveChanges();
                    }

                    string code = "";
                    if (sysprchannel == 1U)
                        code = "B2B-FF";
                    else
                        code = "B2B-KF";

                    WFTZVAR wftVarZustand = (from wftvar in context.WFTZVAR
                                             where wftvar.WFTZUST.SYSWFTZUST == wftZust.SYSWFTZUST && (wftvar.CODE == code)
                                             select wftvar).FirstOrDefault();



                    if (wftVarZustand == null)
                    {

                        // Zustandsvariable 
                        wftVarZustand = new WFTZVAR();
                        wftVarZustand.WFTZUST = wftZust;
                        if (sysprchannel == 1U)
                            wftVarZustand.CODE = "B2B-FF";
                        else
                            wftVarZustand.CODE = "B2B-KF";

                        wftVarZustand.VALUE = "Upload";
                        context.WFTZVAR.Add(wftVarZustand);

                        // Ticket#2012070410000261 — Anpassung Inbox-Routing - Timestamp beim Antrag-Einreichen 
                        WFTZVAR wftVarTimeStamp = new WFTZVAR();
                        wftVarTimeStamp.WFTZUST = wftZust;
                        if (sysprchannel == 1U)
                            wftVarTimeStamp.CODE = "EINGANG_B2B-FF";
                        else
                            wftVarTimeStamp.CODE = "EINGANG_B2B-KF";

                        wftVarTimeStamp.VALUE = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null).ToString("dd.MM.yyyy HH:mm:ss");

                        context.WFTZVAR.Add(wftVarTimeStamp);

                        context.SaveChanges();

                    }
                    else
                        if (wftVarZustand.VALUE != "Neu" && wftVarZustand.VALUE != "Upload")
                        {
                            wftVarZustand.VALUE = "Upload";
                            context.SaveChanges();
                        }


                }
                catch (ArgumentException ex)
                {
                    throw new ArgumentException("Could not find or update WFTZVAR ", ex);
                }



            }

            String testWert = AppConfig.Instance.GetEntry("BPE", "ENABLE_B2B_BPE_Upload", "", "SETUP.NET");
            bool eventtestDMS = "1".Equals(testWert) || "true".Equals(testWert) || "TRUE".Equals(testWert);
            if (newFile && eventtestDMS)
            {
                String procCode = AppConfig.Instance.GetEntry("BPE", "PROC_B2B_DOC_Eingang", "tprc_WFA_B2B_DOC_Eingang", "SETUP.NET");
                String evtCode = AppConfig.Instance.GetEntry("BPE", "EVT_B2B_DOC_Eingang", " evtd_WFA_B2B_DOC_Eingang", "SETUP.NET");
                BPEBo.createBPEProcess(procCode, evtCode, "DMSDOC", sysdmsdoc, file.syswfuser);
            }
            

            return getFileOhneInhalt(sysdmsdoc);

        }

        /// <summary>
        /// createOrUpdateFileAngebot for B2C
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public FileDto createOrUpdateFileAngebot(FileDto file)
        {
          
            FileDto fileoutput = new FileDto();
            long sysdmsdoc = 0;
            long sysprchannel = 0;
            bool newFile = false;
            long sysidAntrag = 0;
            if (file.area.ToUpper() == "ANGEBOT" && file.sysId > 0)
            {
                sysidAntrag = getSysidAntrag(file.sysId);
            }
            
            using (DdOwExtended ctxOw = new DdOwExtended())
            {
                sysprchannel = ctxOw.ExecuteStoreQuery<long>("select sysprchannel from antrag where sysid=" + sysidAntrag, null).FirstOrDefault();
                try
                {
                    DMSDOC dmsdoc = null;
                    DMSDOCAREA docarea = null;

                    if (file.sysFile == 0 && file.sysId > 0 && file.area != "")
                    {
                        file.sysCrtTime = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                        file.sysCrtDate = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDateNoTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                        file.activFlag = true;
                        dmsdoc = new DMSDOC();
                        newFile = true;
                        dmsdoc = Mapper.Map<FileDto, DMSDOC>(file, dmsdoc);
                        dmsdoc.UNGUELTIGFLAG = 0;
                        dmsdoc.GEDRUCKTVON = file.syscrtuser.ToString();
                        dmsdoc.SEARCHTERMS = "FORMALITAETEN";
                        ctxOw.DMSDOC.Add(dmsdoc);

                        docarea = new DMSDOCAREA();
                        docarea = Mapper.Map<FileDto, DMSDOCAREA>(file, docarea);
                        docarea.DMSDOC = dmsdoc;
                        docarea.RANG = 0;
                        ctxOw.DMSDOCAREA.Add(docarea);

                        ctxOw.SaveChanges();
                        sysdmsdoc = dmsdoc.SYSDMSDOC;

                        if (sysidAntrag > 0)
                        {
                            docarea = new DMSDOCAREA();
                            docarea.AREA = "ANTRAG";
                            docarea.SYSID = sysidAntrag;
                            docarea.DMSDOC = dmsdoc;
                            docarea.RANG = 0;
                            ctxOw.DMSDOCAREA.Add(docarea);
                            ctxOw.SaveChanges();
                        }

                    }
                    else
                    {
                        if (file.sysFile > 0)
                        {

                            List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysdmsdoc", Value = file.sysFile });
                            dmsdoc = ctxOw.ExecuteStoreQuery<DMSDOC>(QUERYGETDMSDOC, parameters.ToArray()).FirstOrDefault();


                            if (dmsdoc != null)
                            {
                                Mapper.Map<FileDto, DMSDOC>(file, dmsdoc);
                                ctxOw.SaveChanges();
                                sysdmsdoc = dmsdoc.SYSDMSDOC;
                            }

                            else throw new Exception("File with id " + dmsdoc.SYSDMSDOC + " not found!");
                        }
                        else throw new Exception("Area or Sysid not found");
                    }

                    CreateDmsAkte(ctxOw, file.syswfuser, file.sysId ?? 0, sysidAntrag, dmsdoc);
                }
                catch (ArgumentException ex)
                {
                    throw new ArgumentException("Could not create or update upload", ex);
                }


            }

            using (DdOwExtended context = new DdOwExtended())
            {


                DateTime currDateTime = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null);
                int currTime = DateTimeHelper.DateTimeToClarionTime(currDateTime);

                try
                {
                    WFTZUST wftZust = (from wftzust in context.WFTZUST
                                       where wftzust.WFTABLE.SYSWFTABLE == 122
                                       where wftzust.SYSLEASE == file.sysId
                                       where wftzust.WFZUST.SYSWFZUST == 461
                                       select wftzust).FirstOrDefault();

                    if (wftZust == null)
                    {

                        long syswftable = context.ExecuteStoreQuery<long>("select syswftable from wftable where syscode='ANGEBOT'", null).FirstOrDefault();
                        long syswfzust = context.ExecuteStoreQuery<long>("select syswfzust from wfzust where syscode='INBOXEN_ANG'", null).FirstOrDefault();

                        // Neuer Zustand
                        wftZust = new WFTZUST();
                        wftZust.SYSLEASE = file.sysId;
                        wftZust.SYSWFTABLE = syswftable;
                        wftZust.SYSWFZUST = syswfzust;
                        wftZust.STATE = "INBOXEN";
                        wftZust.BEZEICHNUNG = "Inboxen";
                        wftZust.STATUS = 0;
                        wftZust.COUNTER = 1;
                        wftZust.CREATEDATE = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDateNoTime(currDateTime);
                        wftZust.CREATETIME = currTime;
                        wftZust.CREATEBY = file.syscrtuser;
                        wftZust.CREATEWFG = "B2C";
                        wftZust.GEBIET = "Angebot";

                        context.WFTZUST.Add(wftZust);
                        context.SaveChanges();


                    }

                    string code = "ANG-B2C";
                    
                    WFTZVAR wftVarZustand = (from wftvar in context.WFTZVAR
                                             where wftvar.WFTZUST.SYSWFTZUST == wftZust.SYSWFTZUST && (wftvar.CODE == code)
                                             select wftvar).FirstOrDefault();



                    if (wftVarZustand == null)
                    {

                        // Zustandsvariable 
                        wftVarZustand = new WFTZVAR();
                        wftVarZustand.WFTZUST = wftZust;
                        wftVarZustand.CODE = code;

                        wftVarZustand.VALUE = "Upload";
                        context.WFTZVAR.Add(wftVarZustand);

                        context.SaveChanges();

                    }
                    else if (wftVarZustand.VALUE != "Neu" && wftVarZustand.VALUE != "Upload")
                    {
                        wftVarZustand.VALUE = "Upload";
                        context.SaveChanges();
                    }


                }
                catch (ArgumentException ex)
                {
                    throw new ArgumentException("Could not find or update WFTZVAR ", ex);
                }



            }
            String testWert = AppConfig.Instance.GetEntry("BPE", "ENABLE_B2C_BPE", "", "SETUP.NET");
            bool eventtestBPEAngebot = "1".Equals(testWert) || "true".Equals(testWert) || "TRUE".Equals(testWert);
            
            bool FF = sysprchannel == 1L;        // Fahrzeugfinanzierung
            bool KF = sysprchannel == 2L;        // Kreditfinanzierung
            if (KF)
            {
                if (eventtestBPEAngebot)
                {
                    String procCode = AppConfig.Instance.GetEntry("BPE", "PROC_KF_UPLOAD_B2C", "", "SETUP.NET");
                    String evtCode = AppConfig.Instance.GetEntry("BPE", "EVT_KF_UPLOAD_B2C", "evtd_WFA_KF_UploadB2C_Start", "SETUP.NET");
                    BPEBo.createBPEProcess(procCode, evtCode, "ANGEBOT", (long)file.sysId, file.syswfuser);
                }
            }
            else if (FF)
            {
                if (eventtestBPEAngebot)
                {
                    String procCode = AppConfig.Instance.GetEntry("BPE", "PROC_FF_UPLOAD_B2C", "", "SETUP.NET");
                    String evtCode = AppConfig.Instance.GetEntry("BPE", "EVT_FF_UPLOAD_B2C", "evtd_WFA_FF_UploadB2C_Start", "SETUP.NET");
                    BPEBo.createBPEProcess(procCode, evtCode, "ANGEBOT", (long)file.sysId, file.syswfuser);
                }
            }

            testWert = AppConfig.Instance.GetEntry("BPE", "ENABLE_B2C_BPE_Upload", "", "SETUP.NET");
            bool eventtestDMS = "1".Equals(testWert) || "true".Equals(testWert) || "TRUE".Equals(testWert);
            if (newFile && eventtestDMS)
            {
                String procCode = AppConfig.Instance.GetEntry("BPE", "PROC_B2C_DOC_Eingang", "tprc_WFA_B2C_DOC_Eingang", "SETUP.NET");
                String evtCode = AppConfig.Instance.GetEntry("BPE", "EVT_B2C_DOC_Eingang", "evtd_WFA_B2C_DOC_Eingang", "SETUP.NET");
                BPEBo.createBPEProcess(procCode, evtCode, "DMSDOC", sysdmsdoc, file.syswfuser);
            }
            _log.Info("Uploaded Angebot-File to id " + sysdmsdoc + " BPE Angebot: " + eventtestBPEAngebot + " BPE DMSDoc: " + eventtestDMS);
            return getFileOhneInhalt(sysdmsdoc);

        }

        /// <summary>
        /// BANKNOW-259: Legt eine DMSAKTE an und erzeugt einen EAIHOT Satz zur Verarbeitung
        /// </summary>
        /// <param name="ctxOw"></param>
        /// <param name="file"></param>
        /// <param name="sysidAntrag"></param>
        /// <param name="dmsdoc"></param>
        private void CreateDmsAkte(DdOwExtended ctxOw, long syswfuser, long sysAngebot, long sysAntrag, DMSDOC dmsdoc)
        {
            string active = AppConfig.Instance.GetEntry("DMR", "ACTIVE", "0", "SETUP");
            if (active != "1")
            {
                return;
            }

            // Removed, see BNRVZ-1427

            var userCode = "";
            var wfuser = ctxOw.WFUSER.FirstOrDefault(a => a.SYSWFUSER == syswfuser);
            if (wfuser != null)
            {
                userCode = wfuser.CODE;
            }

            DMSAKTE dmsakte = new DMSAKTE()
            {
                AREA = "DMSDOC",
                SYSID = dmsdoc.SYSDMSDOC,
                SYSANTRAG = sysAntrag,
                SYSANGEBOT = sysAngebot,
                CREDATE = DateTime.Now,
                CRETIME = DateTimeHelper.DateTimeToClarionTime(DateTime.Now),
                CREUSR = userCode,
                METHOD = "DMR"
            };
            ctxOw.DMSAKTE.Add(dmsakte);
            ctxOw.SaveChanges();

            // Create EAIBos call for createorupdate DMR
            var eaihotBo = BOFactory.getInstance().createEaihotBo();
            eaihotBo.createEAIBosCall("CREATEORUPDATEDMR", "DMSAKTE", dmsakte.SYSDMSAKTE, syswfuser, null, null, null);
        }

        /// <summary>
        /// getFile
        /// </summary>
        /// <param name="sysFile"></param>
        /// <returns></returns>
        public FileDto getFile(long sysFile)
        {

            FileDto rval = null;

            using (DdOwExtended owCtx = new DdOwExtended())
            {

                DMSDOC fileOut = (from e in owCtx.DMSDOC
                                  where e.SYSDMSDOC == sysFile
                                  select e).FirstOrDefault();



                if (fileOut != null)
                {

                    rval = Mapper.Map<DMSDOC, FileDto>(fileOut);
                    rval.sysFile = fileOut.SYSDMSDOC;
                    rval.fileSize = fileOut.INHALT.Length;
                    if (rval.fileName.Contains('.'))
                    {
                        string[] t = rval.fileName.Split(new Char[] { '.' });
                        rval.format = t.Last();
                    }
                    rval.activFlag = fileOut.UNGUELTIGFLAG == 0 ? true : false;
                    rval.syscrtuser = long.Parse(fileOut.GEDRUCKTVON);

                }

                DMSDOCAREA docarea = (from e in owCtx.DMSDOCAREA
                                      where e.DMSDOC.SYSDMSDOC == sysFile
                                      select e).FirstOrDefault();
                if (docarea != null)
                {
                    rval.area = docarea.AREA;
                    rval.sysId = docarea.SYSID;
                }

            }
            return rval;
        }


        /// <summary>
        /// getFileOhneInhalt
        /// </summary>
        /// <param name="sysFile"></param>
        /// <returns></returns>
        public FileDto getFileOhneInhalt(long sysFile)
        {

            FileDto rval = null;

            using (DdOwExtended owCtx = new DdOwExtended())
            {

                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysdmsdoc", Value = sysFile });
                rval = owCtx.ExecuteStoreQuery<FileDto>(QUERYGETOHNEIHNHALT, parameters.ToArray()).FirstOrDefault();
                if (rval != null && rval.fileName.Contains('.'))
                {
                    string[] t = rval.fileName.Split(new Char[] { '.' });
                    rval.format = t.Last();
                }

            }
            return rval;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysAngebotID"></param>
        /// <returns></returns>
        public long getSysidAntrag(long? sysidAngebot)
        {
            using (DdOlExtended ctx = new DdOlExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysid", Value = sysidAngebot });
                return ctx.ExecuteStoreQuery<long>(QUERYGETSYSIDANTRAG, parameters.ToArray()).FirstOrDefault();
            }
        }

        public long getSysidAngebot(long? sysidAntrag)
        {
            using (DdOlExtended ctx = new DdOlExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysid", Value = sysidAntrag });
                return ctx.ExecuteStoreQuery<long>(QUERYGETSYSIDANGEBOT, parameters.ToArray()).FirstOrDefault();
            }
        }

    }
}
