using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Model.DdOd;
using Cic.OpenOne.Common.Util.Logging;
using Devart.Data.Oracle;
using Cic.OpenOne.Common.Model.DdOw;

namespace Cic.OpenOne.Common.DAO
{
    /// <summary>
    /// UebersetzungsDao
    /// </summary>
    public class TranslateDao : ITranslateDao
    {
        private static string QUERYDDLKPPOSDNULL = "SELECT vc_ddlkppos.TOOLTIP, vc_ddlkppos.ACTUALTERM, vc_ddlkppos.ORIGTERM, vc_ddlkppos.CODE, vc_ddlkppos.SYSDDLKPPOS, vc_ddlkppos.ID FROM cic.vc_ddlkppos vc_ddlkppos, ctlang WHERE vc_ddlkppos.code = :code AND vc_ddlkppos.sysctlang =ctlang.sysctlang AND ctlang.isocode =:isocode AND vc_ddlkppos.domainid IS NULL ORDER BY vc_ddlkppos.rank";

        private static string QUERYDDLKPPOSDNOTNULL = "SELECT vc_ddlkppos.TOOLTIP, vc_ddlkppos.ACTUALTERM, vc_ddlkppos.ORIGTERM, vc_ddlkppos.CODE, vc_ddlkppos.SYSDDLKPPOS, vc_ddlkppos.ID FROM cic.vc_ddlkppos vc_ddlkppos, ctlang WHERE vc_ddlkppos.code = :code  AND vc_ddlkppos.sysctlang =ctlang.sysctlang and ctlang.isocode=:isocode and vc_ddlkppos.domainid=:domainid order by vc_ddlkppos.rank";

        private static string QUERYCTLUT = "SELECT ctlut.AREA Area, lang.ISOCODE IsoCode, ctlut.SYSID sysID, ctlut.ORIGTERM1 OrigTerm, ctlut.ACTUALTERM1 Name, ctlut.ACTUALTERM2 Description FROM cic.vc_ctlut ctlut, ctlang lang WHERE ctlut.AREA = :area  AND ctlut.sysctlang = lang.sysctlang and lang.isocode=:isocode order by ctlut.sysid";

        private static string QUERYCTLUTMULTIPLE = "SELECT ctlut.AREA Area, lang.ISOCODE IsoCode, ctlut.SYSID sysID, ctlut.ORIGTERM1 OrigTerm, ctlut.ACTUALTERM1 Name, ctlut.ACTUALTERM2 Description, ctlut.ACTUALTERM3 Bezeichnung FROM cic.vc_ctlut ctlut, ctlang lang WHERE ctlut.AREA IN ({0})  AND ctlut.sysctlang = lang.sysctlang and lang.isocode=:isocode order by ctlut.sysid";

        //replaceblob currently not fetched. Note: a blob will be read by a proxy upon first access of the byte-array!
        private static string STATICFIELDS = "SELECT ctfoid.frontid, ctfoid.typ, ctfoid.verbaldescription MASTER, cttfoid.replaceterm TRANSLATION, ctlang.isocode FROM ctfoid, cttfoid, ctlang WHERE ctfoid.frontid =cttfoid.frontid AND ctlang.sysctlang =cttfoid.sysctlang AND ctlang.flagtranslate=1 ORDER BY ctfoid.frontid";
        private static string STATICFIELDS2 = "select distinct * from (select nvl(ctglbl.CODE,ctglbl.origterm) as frontid,ctglbl.CTRLTYPE as typ,ctglbl.ORIGTERM as MASTER,cttglbl.REPLACETERM as TRANSLATION,ctlang.isocode as isocode from CTGLBL,CTTGLBL,CTLANG where CTGLBL.SYSCTGLBL = CTTGLBL.SYSCTGLBL and CTTGLBL.SYSCTLANG=CTLANG.SYSCTLANG union select nvl(ctglbl.CODE,replace(ctglbl.ORIGTERM,':','')) as frontid,ctglbl.CTRLTYPE as typ,replace(ctglbl.ORIGTERM,':','') as MASTER,replace(cttglbl.REPLACETERM,':','') as TRANSLATION,ctlang.isocode as isocode from CTGLBL,CTTGLBL,CTLANG where CTGLBL.SYSCTGLBL = CTTGLBL.SYSCTGLBL and CTTGLBL.SYSCTLANG=CTLANG.SYSCTLANG and ctglbl.ORIGTERM like '%:' union select nvl(ctglbl.CODE,replace(replace(ctglbl.ORIGTERM,':',''),'&','')) as frontid,ctglbl.CTRLTYPE as typ,replace(replace(ctglbl.ORIGTERM,':',''),'&','') as MASTER,replace(replace(cttglbl.REPLACETERM,':',''),'&','') as TRANSLATION,ctlang.isocode as isocode from CTGLBL,CTTGLBL,CTLANG where CTGLBL.SYSCTGLBL = CTTGLBL.SYSCTGLBL and CTTGLBL.SYSCTLANG=CTLANG.SYSCTLANG and ctglbl.ORIGTERM like '%:' union SELECT ctfoid.frontid, ctfoid.typ, ctfoid.verbaldescription MASTER, cttfoid.replaceterm TRANSLATION, ctlang.isocode FROM ctfoid, cttfoid, ctlang WHERE ctfoid.frontid =cttfoid.frontid AND ctlang.sysctlang =cttfoid.sysctlang AND ctlang.flagtranslate=1 UNION select concat(CTTLOCL.winname, concat('_', CTTLOCL.frontid)) as frontid, null as typ, CTTLOCL.frontid as master, CTTLOCL.replaceterm as translation, CTLANG.isocode as isocode   from CTTLOCL, ctlang WHERE CTTLOCL.SYSCTLANG=CTLANG.SYSCTLANG AND cttlocl.replaceterm IS NOT NULL) order by MASTER";

        private static string MESSAGETRANSLATIONQUERY = "SELECT ctmessage.MESSAGECODE, ctmessage.MessageTyp, cttmessage.replacemessagetext replacementmessagetext, " +
                                                        "       cttmessage.replacemessagetitle replacementmessagetitle, ctlang.isocode  " +
                                                        " FROM ctmessage, cttmessage, ctlang " +
                                                        " WHERE ctmessage.messagecode = cttmessage.messagecode AND cttmessage.sysctlang = ctlang.sysctlang AND " +
                                                        "       ctmessage.MessageCode = :code AND ctmessage.messagearea = 'SERVICES'";

        private static string QUERYSTATICFIELDS = "Select PARAMFILE from eaipar where CODE = :pcode";


        /// <summary>
        /// The logger
        /// </summary>
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private String isoCode;
        private String Area;

        /// <summary>
        /// Read the entire Translation List 
        /// </summary>
        /// <returns>array of DropListDtos</returns>
        virtual public List<CTLUT_Data> readoutTranslationList(String Area, String isoCode)
        {
            this.isoCode = isoCode;
            this.Area = Area;
            List<CTLUT_Data> rval = new List<CTLUT_Data>();
            bool bMultiple = false;
            using (DdOdExtended odCtx = new DdOdExtended())
            {
                String query = QUERYCTLUT;
                if (Area.Contains(','))
                {
                    query = String.Format(QUERYCTLUTMULTIPLE, Area);
                    bMultiple = true;
                }
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                if (!bMultiple)
                {
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "area", Value = Area });
                }
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "isocode", Value = isoCode });

                List<CTLUT_Data> values = odCtx.ExecuteStoreQuery<CTLUT_Data>(query, parameters.ToArray()).ToList();

                foreach (CTLUT_Data Position in values)
                {
                    rval.Add(Position);
                }
            }
            return rval;
        }

        /// <summary>
        /// Retrieve an Entry by it's Original Name
        /// </summary>
        /// <param name="OrigTerm">Original Name</param>
        /// <param name="Area">Übersetzungsbereich</param>
        /// <param name="translations">Übersetzungsliste</param>
        /// <returns>Data Entry</returns>
        virtual public CTLUT_Data RetrieveEntry(string OrigTerm, string Area, List<CTLUT_Data> translations)
        {
            foreach (CTLUT_Data Position in translations)
            {
                if (Position.OrigTerm == OrigTerm)
                {
                    if (Area != null && Area.Length > 0)
                    {
                        if (Area.CompareTo(Position.Area) == 0)
                        {
                            return Position;
                        }
                    }
                    else
                    {
                        return Position;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Retrieve and Entry by it's SysID.
        /// </summary>
        /// <param name="ID">Sys ID</param>
        /// <param name="Area">Übersetzungsbereich</param>
        /// <param name="translations">Übersetzungsliste</param>
        /// <returns>Data Entry</returns>
        virtual public CTLUT_Data RetrieveEntry(long ID, string Area, List<CTLUT_Data> translations)
        {
            foreach (CTLUT_Data Position in translations)
            {
                if (Position.sysID == ID)
                {
                    if (Area != null && Area.Length > 0)
                    {
                        if (Area.CompareTo(Position.Area) == 0)
                        {
                            return Position;
                        }
                    }
                    else
                    {
                        return Position;
                    }
                }
            }
            return null;
        }

        /// <summary>
        /// Übersetzen einer Liste von Datenzugriffsobjekten
        /// </summary>
        /// <param name="List">Eingangsliste</param>
        /// <param name="Area">Übersetzungsbereich</param>
        /// <param name="translations">Übersetzungsliste</param>
        /// <returns>Ausgangsliste</returns>
        public DropListDto[] TranslateList(DropListDto[] List, string Area, List<CTLUT_Data> translations)
        {
            foreach (DropListDto ListEntry in List)
            {
                CTLUT_Data Entry = RetrieveEntry(ListEntry.sysID, Area, translations);
                if (Entry != null)
                {
                    ListEntry.bezeichnung = Entry.Name;
                    ListEntry.beschreibung = Entry.Description;
                }
            }
            return List;
        }

        /// <summary>
        /// Get List of static Translation Entries
        /// </summary>
        /// <returns>Translation List</returns>
        virtual public List<TranslationDto> GetStaticList()
        {
            Dictionary<String, TranslationDto> TranslationList = new Dictionary<String, TranslationDto>();
            String newQueryString = "";
            //flagtranslate BNRZEHN-1747
            using (DdOwExtended eaiParContext = new DdOwExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> param = new List<Devart.Data.Oracle.OracleParameter>();
                param.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pcode", Value = "STATICFIELDSQUERY" });
                newQueryString = eaiParContext.ExecuteStoreQuery<String>(QUERYSTATICFIELDS, param.ToArray()).FirstOrDefault();
            }

            using (DdOdExtended odCtx = new DdOdExtended())
            {
                String query = STATICFIELDS;
                if (newQueryString != null && newQueryString.Trim() != "")
                    query = newQueryString;

                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();

                List<StaticTranslateDto> TranslationData = odCtx.ExecuteStoreQuery<StaticTranslateDto>(query, parameters.ToArray()).ToList();

                foreach (StaticTranslateDto Position in TranslationData)
                {
                    if (TranslationList.Keys.Contains(Position.frontid) == false)
                    {
                        TranslationDto NewEntry = new TranslationDto();
                        NewEntry.translations = new List<TranslationValue>();
                        NewEntry.frontId = Position.frontid;
                        NewEntry.master = Position.master;
                        NewEntry.typ = Position.typ;
                        NewEntry.translations.Add(new TranslationValue());
                        NewEntry.translations[0].frontId = Position.frontid;
                        NewEntry.translations[0].isoCode = Position.isocode;
                        NewEntry.translations[0].translation = Position.translation;
                        NewEntry.translations[0].longTranslation = Position.replaceblob;
                        TranslationList.Add(Position.frontid, NewEntry);
                    }
                    else
                    {
                        TranslationDto Item = TranslationList[Position.frontid];
                        TranslationValue NewEntry = new TranslationValue();
                        NewEntry.frontId = Position.frontid;
                        NewEntry.isoCode = Position.isocode;
                        NewEntry.translation = Position.translation;
                        NewEntry.longTranslation = Position.replaceblob;
                        Item.translations.Add(NewEntry);
                    }
                }
            }
            return TranslationList.Values.ToList<TranslationDto>();
        }

        /// <summary>
        /// Get List of static Translation Entries for OneWeb
        /// </summary>
        /// <returns>Translation List</returns>
        virtual public List<TranslationDto> GetStaticList2()
        {
            Dictionary<String, TranslationDto> TranslationList = new Dictionary<String, TranslationDto>();

            using (DdOdExtended odCtx = new DdOdExtended())
            {
                String query = STATICFIELDS2;
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();

                List<StaticTranslateDto> TranslationData = odCtx.ExecuteStoreQuery<StaticTranslateDto>(query, parameters.ToArray()).ToList();

                foreach (StaticTranslateDto Position in TranslationData)
                {
                    if (Position.frontid == null) continue;
                    if (TranslationList.Keys.Contains(Position.frontid) == false)
                    {
                        TranslationDto NewEntry = new TranslationDto();
                        NewEntry.translations = new List<TranslationValue>();
                        NewEntry.frontId = Position.frontid;
                        NewEntry.master = Position.master;
                        NewEntry.typ = Position.typ;
                        NewEntry.translations.Add(new TranslationValue());
                        NewEntry.translations[0].frontId = Position.frontid;
                        NewEntry.translations[0].isoCode = Position.isocode;
                        NewEntry.translations[0].translation = Position.translation;
                        NewEntry.translations[0].longTranslation = Position.replaceblob;
                        TranslationList.Add(Position.frontid, NewEntry);
                    }
                    else
                    {
                        TranslationDto Item = TranslationList[Position.frontid];
                        TranslationValue NewEntry = new TranslationValue();
                        NewEntry.frontId = Position.frontid;
                        NewEntry.isoCode = Position.isocode;
                        NewEntry.translation = Position.translation;
                        NewEntry.longTranslation = Position.replaceblob;
                        Item.translations.Add(NewEntry);
                    }
                }
            }
            return TranslationList.Values.ToList<TranslationDto>();
        }

        /// <summary>
        /// finds a dictionary entry in DDLKPPOS by its CODE
        /// </summary>
        /// <param name="code">the CODE of the dictionary entries</param>
        /// <param name="isoCode">the isoCode of the language</param>
        /// <param name="domainId">the domainId</param>
        /// <returns>array of DropListDtos</returns>
        virtual public DropListDto[] findByDDLKPPOSCode(DDLKPPOSType code, String isoCode, String domainId)
        {
            List<DropListDto> dropListDtoList = new List<DropListDto>();

            using (DdOdExtended odCtx = new DdOdExtended())
            {
                String query = QUERYDDLKPPOSDNULL;
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "code", Value = code.ToString() });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "isocode", Value = isoCode });
                if (domainId != null)
                {
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "domainid", Value = domainId });
                    query = QUERYDDLKPPOSDNOTNULL;
                }
                List<DDLKPPOSData> values = odCtx.ExecuteStoreQuery<DDLKPPOSData>(query, parameters.ToArray()).ToList();

                foreach (DDLKPPOSData ddlkppos in values)
                {
                    String term = (ddlkppos.ACTUALTERM != null && ddlkppos.ACTUALTERM.Length > 0) ? ddlkppos.ACTUALTERM : ddlkppos.ORIGTERM;

                    dropListDtoList.Add(
                        new DropListDto()
                        {
                            sysID = (long)ddlkppos.SYSDDLKPPOS,
                            code = ddlkppos.ID,
                            beschreibung = ddlkppos.TOOLTIP,
                            bezeichnung = term
                        });
                }
            }
            return dropListDtoList.ToArray();
        }

        /// <summary>
        /// finds a dictionary entry in DDLKPPOS by its CODE, domainid assumed to be null
        /// </summary>
        /// <param name="code">the CODE of the dictionary entries</param>
        /// <param name="isoCode">the isoCode of the language</param>
        /// <returns>array of DropListDtos</returns>
        virtual public DropListDto[] findByDDLKPPOSCode(DDLKPPOSType code, String isoCode)
        {
            return findByDDLKPPOSCode(code, isoCode, null);
        }

        /// <summary>
        /// Get List of static Translation Entries
        /// </summary>
        /// <returns>Translation List</returns>
        virtual public List<MessageTranslateDto> readoutMessagetranslation(String MessageCode, String isoCode)
        {
            List<MessageTranslateDto> TranslationList = new List<MessageTranslateDto>();
            using (DdOdExtended odCtx = new DdOdExtended())
            {
                String query = MESSAGETRANSLATIONQUERY;
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                OracleParameter item = new OracleParameter("code", MessageCode);
                parameters.Add(item);

                List<MessageTranslateDto> TranslationData = odCtx.ExecuteStoreQuery<MessageTranslateDto>(query, parameters.ToArray()).ToList();

                foreach (MessageTranslateDto Position in TranslationData)
                {
                    if (Position.IsoCode != null && Position.IsoCode.CompareTo(isoCode) == 0)
                    {
                        TranslationList.Add(Position);
                    }
                }
            }
            return TranslationList;
        }
    }
}