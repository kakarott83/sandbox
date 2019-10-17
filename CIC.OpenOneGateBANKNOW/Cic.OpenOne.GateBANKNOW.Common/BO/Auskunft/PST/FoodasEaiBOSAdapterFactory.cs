using AutoMapper;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Model.DdOw;
using Cic.OpenOne.Common.Util;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using CIC.Database.OW.EF6.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.PST
{
    /// <summary>
    /// Factory for Adapting the PS-Team Foodas WS to the CALL_BOS EAIHOT mechanism
    /// </summary>
    public class FoodasEaiBOSAdapterFactory : IEaiBOSAdapterFactory
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public IEaiBOSAdapter getEaiBOSAdapter(String method)
        {
            switch (method)
            {
                case ("PS_GRUNDDATEN"):
                    return new GrunddatenAdapter();
                case ("PS_GRUNDDATEN_MASS"):
                    return new GrunddatenMassAdapter();
                case ("PS_BEAUFTRAGUNG"):
                    return new BeauftratungAdapter();
                case ("PS_EVENT"):
                    return new EventAdapter();
                case ("PS_DOKUMENT"):
                    return new PSDokumentAdapter();
            }
            return null;
        }

        /// <summary>
        /// Adapter for calling the beauftragung (for kfz-sending) at the ps-team WS
        /// </summary>
        private class PSDokumentAdapter : IEaiBOSAdapter
        {
            public void processEaiHot(IEaihotDao dao, EaihotDto eaihot)
            {
                try
                {
                    bool retail = false;
                    if (eaihot.INPUTPARAMETER2 != null && "1".Equals(eaihot.INPUTPARAMETER2))
                    {
                        retail = true;
                    }
                    Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.PST.FoodasBo fbo = new Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.PST.FoodasBo();
                    Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.FoodasGetDokumentOutDto outputdata = fbo.getDokument(eaihot.INPUTPARAMETER1, retail);


                    if (outputdata.hasError)
                        fbo.logError(eaihot.SYSEAIHOT);

                    eaihot.OUTPUTPARAMETER1 = outputdata.AKt_Briefnummer;
                    eaihot.OUTPUTPARAMETER2 = outputdata.hasError ? "1" : "0";
                    eaihot.OUTPUTPARAMETER3 = stripString(outputdata.errorCode);
                    eaihot.OUTPUTPARAMETER4 = stripString(outputdata.errorMessage);
                    eaihot.OUTPUTPARAMETER5 = outputdata.errorRecordID;
                    dao.updateEaihot(eaihot);
                    if (outputdata.data != null && outputdata.data.Length > 0 && !outputdata.hasError)
                    {
                        EaihotDto eaihotInput = new EaihotDto();
                        eaihotInput.CODE = "PS_DOKUMENT";
                        eaihotInput.PROZESSSTATUS = 0;
                        eaihotInput.HOSTCOMPUTER = "*";
                        eaihotInput.EVE = 0;
                        eaihotInput.INPUTPARAMETER1 = outputdata.FahrzeugIdent;
                        eaihotInput.INPUTPARAMETER2 = outputdata.Dokument_typ;
                        eaihotInput.INPUTPARAMETER3 = outputdata.Akt_Bewegung_typ_id;
                        eaihotInput.INPUTPARAMETER4 = outputdata.Psbarcode;
                        eaihotInput.SUBMITDATE = DateTimeHelper.DateTimeToClarionDate(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                        eaihotInput.SUBMITTIME = DateTimeHelper.DateTimeToClarionTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                        EAIART art = dao.getEaiArt("PS_DOK_FROM_PROVIDER");
                        if (art == null)
                        {
                            _log.Error("EAIART PS_DOC_FROM_PROVIDER NOT FOUND, cannot proceed with PS_TEAM GETDOKUMENT");

                        }
                        eaihotInput.SYSEAIART = art.SYSEAIART;
                        eaihotInput = dao.createEaihot(eaihotInput);

                        using (DdOwExtended ctxOw = new DdOwExtended())
                        {
                            DMSDOC dmsdoc = new DMSDOC();
                            dmsdoc.INHALT = outputdata.data;
                            dmsdoc.UNGUELTIGFLAG = 0;
                            dmsdoc.DATEINAME = "ZLB.pdf";
                            dmsdoc.BEMERKUNG = "ZLB II von PS-TEAM";
                            dmsdoc.SEARCHTERMS = "ZB2";
                            //TODO Some configurable constant for this document template id!
                            dmsdoc.SYSWFTX=9051;
                            ctxOw.DMSDOC.Add(dmsdoc);

                            DMSDOCAREA docarea = new DMSDOCAREA();
                            docarea.AREA = "OB";
                            docarea.SYSID = eaihot.SYSOLTABLE;
                            docarea.DMSDOC = dmsdoc;
                            docarea.RANG = 0;
                            ctxOw.DMSDOCAREA.Add(docarea);

                            ctxOw.SaveChanges();
                        }


                        //activate now
                        dao.activateEaihot(eaihotInput, 1);
 
                    }

                }
                catch (Exception e)
                {
                    eaihot.OUTPUTPARAMETER2 = "1";
                    eaihot.OUTPUTPARAMETER3 = "WS-Failure";
                    eaihot.OUTPUTPARAMETER4 = stripString(e.Message);
                    _log.Error("Error in IEaiBOSAdapter for PS_BEAUFTRAGUNG", e);
                    dao.updateEaihot(eaihot);
                }
            }
        }

        /// <summary>
        /// Adapter for calling the beauftragung (for kfz-sending) at the ps-team WS
        /// </summary>
        private class BeauftratungAdapter : IEaiBOSAdapter
        {
            public void processEaiHot(IEaihotDao dao, EaihotDto eaihot)
            {
                try
                {
                    List<EaiqinDto> queue = dao.listEaiqinForEaihot(eaihot.SYSEAIHOT);
                    bool retail = false;
                    if (eaihot.INPUTPARAMETER2 != null && "1".Equals(eaihot.INPUTPARAMETER2))
                    {
                        retail = true;
                    }
                    bool tempendg = false;
                    if (eaihot.INPUTPARAMETER3 != null && "1".Equals(eaihot.INPUTPARAMETER3))
                    {
                        tempendg = true;
                    }
                    Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.FoodasBeauftragungInDto inputdata = FoodasEaiBOSAdapterFactory.getBeauftragungDto(queue);
                    Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.PST.FoodasBo fbo = new Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.PST.FoodasBo();
                    Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.FoodasBeauftragungOutDto outputdata = fbo.beauftragungSpeichern(inputdata, retail, tempendg);

                    if (outputdata.hasError)
                        fbo.logError(eaihot.SYSEAIHOT);
                    eaihot.OUTPUTPARAMETER1 = outputdata.auftragsnummer;
                    eaihot.OUTPUTPARAMETER2 = outputdata.hasError ? "1" : "0";
                    eaihot.OUTPUTPARAMETER3 = stripString(outputdata.errorCode);
                    eaihot.OUTPUTPARAMETER4 = stripString(outputdata.errorMessage);
                    eaihot.OUTPUTPARAMETER5 = stripString(outputdata.errorRecordID);
                     
                    dao.updateEaihot(eaihot);
                }
                catch (Exception e)
                {
                    eaihot.OUTPUTPARAMETER2 = "1";
                    eaihot.OUTPUTPARAMETER3 = "WS-Failure";
                    eaihot.OUTPUTPARAMETER4 = stripString(e.Message);
                    _log.Error("Error in IEaiBOSAdapter forPS_DOKUMENT", e);
                    dao.updateEaihot(eaihot);
                }
            }
        }
        private static String stripString(String s)
        {
            if (s == null)
            {
                return s;
            }
            if (s.Length > 255)
            {
                return s.Substring(0, 255);
            }
            return s;
        }
        /// <summary>
        /// Adapter for calling the psteam grunddaten (avis) update by eaihot
        /// </summary>
        private class GrunddatenAdapter : IEaiBOSAdapter
        {
            public void processEaiHot(IEaihotDao dao, EaihotDto eaihot)
            {
                try
                {
                    List<EaiqinDto> queue = dao.listEaiqinForEaihot(eaihot.SYSEAIHOT);

                    bool retail = false;
                    if (eaihot.INPUTPARAMETER2 != null && "1".Equals(eaihot.INPUTPARAMETER2))
                    {
                        retail = true;
                    }

                    Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.FoodasGrunddatenInDto inputdata = FoodasEaiBOSAdapterFactory.getGrunddatenDto(queue);
                    Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.PST.FoodasBo fbo = new Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.PST.FoodasBo();
                    Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.FoodasGrunddatenOutDto outputdata = fbo.grunddatenImportSpeichern(inputdata, retail);
                    eaihot.OUTPUTPARAMETER1 = outputdata.vorgangsnummer;
                    eaihot.OUTPUTPARAMETER2 = outputdata.hasError ? "1" : "0";
                    eaihot.OUTPUTPARAMETER3 = stripString(outputdata.errorCode);
                    eaihot.OUTPUTPARAMETER4 = stripString(outputdata.errorMessage);
                    eaihot.OUTPUTPARAMETER5 = outputdata.errorRecordID;
                    if (outputdata.hasError)
                        fbo.logError(eaihot.SYSEAIHOT);
                    dao.updateEaihot(eaihot);
                }
                catch (Exception e)
                {
                    eaihot.OUTPUTPARAMETER2 = "1";
                    eaihot.OUTPUTPARAMETER3 = "WS-Failure";
                    eaihot.OUTPUTPARAMETER4 = stripString(e.Message);
                    _log.Error("Error in IEaiBOSAdapter for PS_GRUNDDATEN", e);
                    dao.updateEaihot(eaihot);
                }
            }
        }

        /// <summary>
        /// Adapter for calling the psteam grunddaten (avis) update by eaihot - Massdistribution
        /// </summary>
        private class GrunddatenMassAdapter : IEaiBOSAdapter
        {
            public void processEaiHot(IEaihotDao dao, EaihotDto eaihot)
            {
                try
                {
                    List<EaiqinDto> queue = dao.listEaiqinForEaihot(eaihot.SYSEAIHOT);

                    bool retail = false;
                    if (eaihot.INPUTPARAMETER2 != null && "1".Equals(eaihot.INPUTPARAMETER2))
                    {
                        retail = true;
                    }
                    int anz = int.Parse(eaihot.INPUTPARAMETER3);
                    List<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.FoodasGrunddatenInDto> inputdataList = FoodasEaiBOSAdapterFactory.getGrunddatenDtos(queue,anz);
                    Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.PST.FoodasBo fbo = new Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.PST.FoodasBo();
                    List<Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.FoodasGrunddatenOutDto> outputdataList = fbo.grunddatenImportSpeichernMass(inputdataList, retail);

                    
                    bool hadError = false;
                    List<EaiqoutDto> outputs = new List<EaiqoutDto>();
                    foreach(Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.FoodasGrunddatenOutDto outputdata in outputdataList)
                    {
                        if (outputdata.hasError)
                            hadError = true;

                        EaiqoutDto eaiqout = new EaiqoutDto();
                        eaiqout.sysEaihot = eaihot.SYSEAIHOT;
                        eaiqout.F01 = outputdata.RecordID;
                        eaiqout.F02 = outputdata.vorgangsnummer;
                        eaiqout.F03 = outputdata.hasError ? "1" : "0";
                        eaiqout.F04 = stripString(outputdata.errorCode);
                        eaiqout.F05 = stripString(outputdata.errorMessage);
                        eaiqout.F06 = outputdata.errorRecordID;
                        outputs.Add(eaiqout);
                    }
                    if(hadError)
                        fbo.logError(eaihot.SYSEAIHOT);
                    dao.createEaiqout(outputs);
                    dao.updateEaihot(eaihot);
                    
                }
                catch (Exception e)
                {
                    eaihot.OUTPUTPARAMETER2 = "1";
                    eaihot.OUTPUTPARAMETER3 = "WS-Failure";
                    eaihot.OUTPUTPARAMETER4 = stripString(e.Message);
                    _log.Error("Error in IEaiBOSAdapter for PS_GRUNDDATEN", e);
                    dao.updateEaihot(eaihot);
                }
            }
        }
        public static void processEventData(byte[] filedata)
        {
            try
                {
                    
                    Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.FoodasEventOutDto outputdata = new Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.PST.FoodasBo().processEvents(filedata);
                    
                    //for every event create an eaihot
                    if (outputdata.events != null && outputdata.events.Count > 0)
                    {
                        IEaihotDao dao = Cic.OpenOne.GateBANKNOW.Common.DAO.CommonDaoFactory.getInstance().getEaihotDao();
                        foreach (Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.FoodasEventOutDataDto data in outputdata.events)
                        {
                            EaihotDto eaihotInput = new EaihotDto();
                            eaihotInput.CODE = "PS_LAGERORT";
                            eaihotInput.PROZESSSTATUS = 0;
                            eaihotInput.HOSTCOMPUTER = "*";
                            eaihotInput.EVE = 0;
                           /* if(data.AKt_Briefnummer==null||data.AKt_Briefnummer.Length<5)
                            {
                                _log.Warn("Event not used for PS_LAGERORT Briefnummer empty: Bewegung_Auftrag_ID " + data.Bewegung_Auftrag_ID + "/Bewegung_typ_ID " + data.Bewegung_typ_ID + "/EVT_ID " + data.EVT_ID + "/Fahrzeug_ID " + data.Fahrzeug_ID + "/FahrzeugIdent " + data.FahrzeugIdent);
                                continue;
                            }*/
                            eaihotInput.INPUTPARAMETER1 = data.AKt_Briefnummer;
                            EAIART art = dao.getEaiArt("PS_FROM_PROVIDER");
                            if (art == null)
                            {
                                _log.Error("EAIART PS_FROM_PROVIDER NOT FOUND, cannot proceed with PS_TEAM Events");
                                break;
                            }
                            eaihotInput.SYSEAIART = art.SYSEAIART;
                            eaihotInput.SUBMITDATE = DateTimeHelper.DateTimeToClarionDate(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                            eaihotInput.SUBMITTIME = DateTimeHelper.DateTimeToClarionTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                            eaihotInput = dao.createEaihot(eaihotInput);

                            List<EaiqinDto> queueQin = FoodasEaiBOSAdapterFactory.getQeueInFromEvents(eaihotInput.SYSEAIHOT, data);
                            dao.createEaiqin(queueQin);

                            //activate now
                            dao.activateEaihot(eaihotInput, 1);
                        }
                        _log.Debug("Processed "+outputdata.events.Count+" events from file data");
                    }
                    _log.Debug("Processed 0 events from file data");
                }
                catch (Exception e)
                {
                    _log.Error("Error processing Event-Data for PS_EVENT", e);
                }
        }
        /// <summary>
        /// Adapter to process ps-team events called from a job
        /// events will be added as eaihot and eaiqin for every kfzbrief-change
        /// The errors from the ps-team ws call will be stored in the original eaihot outputparameter 2-5
        /// </summary>
        private class EventAdapter : IEaiBOSAdapter
        {
            public void processEaiHot(IEaihotDao dao, EaihotDto eaihot)
            {
                try
                {
                    //eaiart PS_FROM_PROVIDER
                    //code   = PS_LAGERORT
                    //Inputparameter1=kfzbrief
                    bool retail = false;
                    if (eaihot.INPUTPARAMETER2 != null && "1".Equals(eaihot.INPUTPARAMETER2))
                    {
                        retail = true;
                    }
                    List<EaiqinDto> queue = dao.listEaiqinForEaihot(eaihot.SYSEAIHOT);
                    Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.PST.FoodasBo fbo = new Cic.OpenOne.GateBANKNOW.Common.BO.Auskunft.PST.FoodasBo();
                    Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.FoodasEventOutDto outputdata = fbo.getEvents(retail);
                    if (outputdata.hasError)
                        fbo.logError(eaihot.SYSEAIHOT);
                    eaihot.OUTPUTPARAMETER2 = outputdata.hasError ? "1" : "0";
                    eaihot.OUTPUTPARAMETER3 = stripString(outputdata.errorCode);
                    eaihot.OUTPUTPARAMETER4 = stripString(outputdata.errorMessage);
                    eaihot.OUTPUTPARAMETER5 = outputdata.errorRecordID;
                    dao.updateEaihot(eaihot);

                    //for every event create an eaihot
                    if (outputdata.events != null && outputdata.events.Count > 0)
                    {
                        foreach (Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.FoodasEventOutDataDto data in outputdata.events)
                        {
                            EaihotDto eaihotInput = new EaihotDto();
                            eaihotInput.CODE = "PS_LAGERORT";
                            eaihotInput.PROZESSSTATUS = 0;
                            eaihotInput.HOSTCOMPUTER = "*";
                            eaihotInput.EVE = 0;
                           /* if(data.AKt_Briefnummer==null||data.AKt_Briefnummer.Length<5)
                            {
                                _log.Warn("Event not used for PS_LAGERORT Briefnummer empty: Bewegung_Auftrag_ID " + data.Bewegung_Auftrag_ID + "/Bewegung_typ_ID " + data.Bewegung_typ_ID + "/EVT_ID " + data.EVT_ID + "/Fahrzeug_ID " + data.Fahrzeug_ID + "/FahrzeugIdent " + data.FahrzeugIdent);
                                continue;
                            }*/
                            eaihotInput.INPUTPARAMETER1 = data.AKt_Briefnummer;
                            EAIART art = dao.getEaiArt("PS_FROM_PROVIDER");
                            if (art == null)
                            {
                                _log.Error("EAIART PS_FROM_PROVIDER NOT FOUND, cannot proceed with PS_TEAM Events");
                                break;
                            }
                            eaihotInput.SYSEAIART = art.SYSEAIART;
                            eaihotInput = dao.createEaihot(eaihotInput);

                            List<EaiqinDto> queueQin = FoodasEaiBOSAdapterFactory.getQeueInFromEvents(eaihotInput.SYSEAIHOT, data);
                            dao.createEaiqin(queueQin);

                            //activate now
                            dao.activateEaihot(eaihotInput, 1);
                        }
                    }
                }
                catch (Exception e)
                {
                    eaihot.OUTPUTPARAMETER2 = "1";
                    eaihot.OUTPUTPARAMETER3 = "WS-Failure";
                    eaihot.OUTPUTPARAMETER4 = stripString(e.Message);
                    _log.Error("Error in IEaiBOSAdapter for PS_EVENT", e);
                    dao.updateEaihot(eaihot);
                }

            }
        }

        /// <summary>
        /// builds a beauftratung Dto from the eaiqin queue
        /// expects fieldname in F01 and value in F02
        /// </summary>
        /// <param name="queue"></param>
        /// <returns></returns>
        private static FoodasBeauftragungInDto getBeauftragungDto(List<EaiqinDto> queue)
        {
            FoodasBeauftragungInDto rval = new FoodasBeauftragungInDto();

            var props = typeof(FoodasBeauftragungInDto).GetProperties();

            foreach (var prop in props)
            {
                String value = (from t in queue
                                where t.F02.Equals(prop.Name)
                                select t.F03).FirstOrDefault();
                prop.SetValue(rval, value);

            }
            return rval;
        }

        /// <summary>
        /// builds a grunddaten Dto from the eaiqin queue
        /// expects fieldname in F01 and value in F02
        /// </summary>
        /// <param name="queue"></param>
        /// <returns></returns>
        private static FoodasGrunddatenInDto getGrunddatenDto(List<EaiqinDto> queue)
        {
            FoodasGrunddatenInDto rval = new FoodasGrunddatenInDto();

            var props = typeof(FoodasGrunddatenInDto).GetProperties();

            foreach (var prop in props)
            {
                String value = (from t in queue
                                where t.F02.Equals(prop.Name)
                                select t.F03).FirstOrDefault();
                prop.SetValue(rval, value);

            }
            return rval;
        }
        /// <summary>
        /// builds a list of grunddaten Dto from the eaiqin queue
        /// expects fieldname in F01 and value in F02
        /// <![CDATA[
        /// the Fieldname must be <propertyname of FoodasGrunddatenInDto>_<number, beginning from 0>
        /// ]]>
        /// </summary>
        /// <param name="queue"></param>
        /// <returns></returns>
        private static List<FoodasGrunddatenInDto> getGrunddatenDtos(List<EaiqinDto> queue, int anz)
        {
            List<FoodasGrunddatenInDto> rvalList = new List<FoodasGrunddatenInDto>();

            var props = typeof(FoodasGrunddatenInDto).GetProperties();

            for (int i = 0; i < anz; i++)
            {
                FoodasGrunddatenInDto rval = new FoodasGrunddatenInDto();
                foreach (var prop in props)
                {
                    String value = (from t in queue
                                    where t.F02.Equals(prop.Name+"_"+(i+1))
                                    select t.F03).FirstOrDefault();
                    prop.SetValue(rval, value);

                }
                rvalList.Add(rval);
            }
            return rvalList;
        }

        /// <summary>
        /// create a eaiqin
        /// </summary>
        /// <param name="syseaihot"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        private static List<EaiqinDto> getQeueInFromEvents(long syseaihot, Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.FoodasEventOutDataDto data)
        {
            List<EaiqinDto> rval = new List<EaiqinDto>();

            var props = typeof(Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft.FoodasEventOutDataDto).GetProperties();

            foreach (var prop in props)
            {
                String value = (String)prop.GetValue(data);
                EaiqinDto qout = new EaiqinDto();
                qout.F01 = "0";
                qout.F02 = prop.Name;
                qout.F03 = value;
                qout.sysEaihot = syseaihot;
                rval.Add(qout);
            }
            return rval;
        }
    }
}
