using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.OpenOne.Common.Util.Collection;
using Cic.OpenOne.Common.Util.Config;
using Cic.One.DTO;
using Cic.One.Web.DAO;

namespace Cic.One.Web.BO.Search
{
    /// <summary>
    /// Factory for Search-Queries for certain Entity-Dtos
    /// </summary>
    public class SearchQueryInfoFactory : ISearchQueryInfoFactory
    {
      
        private static long CACHE_TIMEOUT = 2592000000;//1month
        private static CacheDictionary<Type, QueryInfoData> queryInfoCache = CacheFactory<Type, QueryInfoData>.getInstance().createCache(CACHE_TIMEOUT, CacheCategory.Data);
        private Dictionary<Type, String> hints = new Dictionary<Type, string>();

        

        public SearchQueryInfoFactory()
        {
            hints[typeof(Cic.One.DTO.AccountDto)] = AppConfig.Instance.GetEntry("HINT", "ACCOUNTS", "/*+ index(person person_accounts_ext) */", "SETUP.NET");
            hints[typeof(Cic.One.DTO.PartnerDto)] = AppConfig.Instance.GetEntry("HINT", "ACCOUNTS", "/*+ index(person person_accounts_ext) */", "SETUP.NET");
        }

        /// <summary>
        /// Fetches a QueryInfo from database configuration
        /// </summary>
        /// <param name="gviewId"></param>
        /// <returns></returns>
        public QueryInfoData getQueryInfo(String gviewId)
        {
            WfvEntry entry = DAOFactoryFactory.getInstance().getWorkflowDao().getWfvEntry(gviewId);
            if (entry == null) return null;
            if (entry.customentry == null) return null;
            if (entry.customentry.viewmeta == null) return null;
            if (entry.customentry.viewmeta.query == null) return null;
            //create sql query from configuration

            QueryInfoData qid = null;

            if (String.IsNullOrEmpty(entry.customentry.viewmeta.query.query))
                qid = new QueryInfoDataType4(entry.customentry.viewmeta.query, null, entry.customentry.searchmode);
            else
                qid = new QueryInfoDataType5(entry.customentry.viewmeta, null, entry.customentry.searchmode);
            return qid;
        }

        /// <summary>
        /// Returns the Information for Query, Search, Paging for a certain Dto-Type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual QueryInfoData getQueryInfo<T>()
        {

            Type mt = typeof(T);
            if (!queryInfoCache.ContainsKey(mt))
            {
                QueryInfoData infoData = null;

                if (typeof(T).Equals(typeof(ZinstabDto)))
                {
                    infoData = new QueryInfoDataType1("ZINSTAB", "ZINSTAB.SYSZINSTAB");
                    infoData.resultFields = "* ";
                    infoData.resultTables = "CIC.ZINSTAB ZINSTAB ";
                    infoData.searchTables = "CIC.ZINSTAB ZINSTAB ";
                    infoData.searchConditions = " 1=1 ";
                    infoData.optimized = false;

                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.AdmaddDto)))
                {
                    infoData = new QueryInfoDataType1("ADMADD", "ADMADD.SYSADMADD");
                    infoData.resultFields = "* ";
                    infoData.resultTables = "CIC.ADMADD ADMADD ";
                    infoData.searchTables = "CIC.ADMADD ADMADD ";
                    infoData.searchConditions = " 1=1 ";
                    infoData.optimized = false;

                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.KundeDto)))
                {
                    infoData = new QueryInfoDataType1("PERSON", "PERSON.SYSPERSON");
                    infoData.resultFields = "* ";
                    infoData.resultTables = "CIC.PERSON PERSON ";
                    infoData.searchTables = "CIC.PERSON PERSON ";
                    infoData.searchConditions = " 1=1 ";
                    infoData.optimized = false;

                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.ObjektDto)))
                {
                    infoData = new QueryInfoDataType1("OB", "OB.SYSOB");
                    infoData.resultFields = "KENNZEICHEN,ERSTZUL,ANZAHLSITZE,ANZAHLTUEREN,BAUJAHR,BAUMONAT,BESTELLUNG,BRIEF,FABRIKAT,OBART.NAME FAHRZEUGART,FARBEA,HAENDLER.NAME HAENDLER,HERSTELLER,obkat.name KATEGORIE,LIEFERUNG,ORDERTYPE,RN.FBETRAG+RN.FSTEUER RECHNUNGSBETRAG,RN.GBETRAG+RN.GSTEUER RECHNUNGSBETRAGEURO,RN.VALUTADATUM RECHNUNGSDATUM,RN.FAELLIGDATUM RECHNUNGSFAELLIGKEIT,RN.RECHNUNG RECHNUNGSNUMMER,SCHWACKE,SPERREZAHLUNG,SPERREFREIGABE,STANDORT,STANDORTBRIEF,OB.SYSHD,OB.SYSOB,OB.SYSOBART,OB.SYSOBKAT,OB.TYP,WAEHRUNG.CODE WAEHRUNG,OB.ZUSTAND ";
                    infoData.resultTables = "CIC.OB OB, CIC.RN RN, CIC.WAEHRUNG WAEHRUNG, CIC.OBKAT OBKAT, CIC.OBART OBART, CIC.PERSON HAENDLER  ";
                    infoData.searchTables = "CIC.OB OB, CIC.RN RN, CIC.WAEHRUNG WAEHRUNG, CIC.OBKAT OBKAT, CIC.OBART OBART, CIC.PERSON HAENDLER  ";
                    infoData.searchConditions = " ob.sysobkat=obkat.sysobkat (+) and ob.sysobart=obart.sysobart (+) and HAENDLER.sysperson=OB.SYSHD (+) AND OB.SYSOB = RN.SYSOB (+) and RN.SYSFWAEHRUNG=WAEHRUNG.SYSWAEHRUNG (+)  ";
                    infoData.optimized = false;

                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.HaendlerDto)))
                {
                    infoData = new QueryInfoDataType1("PERSON", "PERSON.SYSPERSON");
                    infoData.resultFields = "sysperson,matchcode,code,titel,name,vorname,anrede,anredecode,strasse,plz, ort,land.countryname land ";
                    infoData.resultTables = "CIC.PERSON PERSON, cic.land land ";
                    infoData.searchTables = "CIC.PERSON PERSON, cic.land land ";
                    infoData.searchConditions = " person.sysland=land.sysland and flaghd=1 ";
                    infoData.optimized = false;

                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.WaehrungDto)))
                {
                    infoData = new QueryInfoDataType1("WAEHRUNG", "WAEHRUNG.SYSWAEHRUNG");
                    infoData.resultFields = "* ";
                    infoData.resultTables = "cic.waehrung waehrung ";
                    infoData.searchTables = "cic.waehrung waehrung ";
                    infoData.searchConditions = " 1=1 ";
                    infoData.optimized = false;

                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.RahmenDto)))
                {
                    infoData = new QueryInfoDataType1("RVT", "RVT.SYSRVT");
                    infoData.resultFields = "* ";
                    infoData.resultTables = "cic.rvt rvt ";
                    infoData.searchTables = "cic.rvt rvt ";
                    infoData.searchConditions = " 1=1 ";
                    infoData.optimized = false;

                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.MycalcDto)))
                {
                    infoData = new QueryInfoDataType1("MYCALC", "MYCALC.SYSMYCALC");
                    infoData.resultFields = "* ";
                    infoData.resultTables = "cic.MYCALC MYCALC ";
                    infoData.searchTables = "cic.MYCALC MYCALC ";
                    infoData.searchConditions = " 1=1 ";
                    infoData.optimized = false;

                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.MycalcfsDto)))
                {
                    infoData = new QueryInfoDataType1("MYCALCFS", "MYCALCFS.SYSMYCALCFS");
                    infoData.resultFields = "* ";
                    infoData.resultTables = "cic.MYCALCFS MYCALCFS ";
                    infoData.searchTables = "cic.MYCALCFS MYCALCFS ";
                    infoData.searchConditions = " 1=1 ";
                    infoData.optimized = false;

                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.ItDto)))
                {
                    infoData = new QueryInfoDataType1("IT", "IT.SYSIT");
                    infoData.resultFields = "it.*";
                    infoData.resultTables = "CIC.IT IT, CIC.PARTNER partner ";
                    infoData.searchTables = "CIC.IT IT, CIC.PARTNER partner ";
                    infoData.searchConditions = " it.sysit=partner.sysit(+) ";
                    infoData.optimized = false;

                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.VorgangDto)))
                {
                    infoData = new QueryInfoDataType1("VC_VORGANG_RED", "VORGANG.SYSID");
                    infoData.resultFields = "* ";
                    infoData.resultTables = "CIC.VC_VORGANG_RED VORGANG ";
                    infoData.searchTables = "CIC.VC_VORGANG_RED VORGANG ";
                    infoData.searchConditions = " 1=1 ";
                    infoData.optimized = false;

                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.AntragzustandDto)))
                {
                    infoData = new QueryInfoDataType1("ANTRAG", "ANTRAG.SYSID");
                    infoData.resultFields = "DISTINCT ANTRAG.ZUSTAND ";
                    infoData.resultTables = "CIC.ANTRAG ANTRAG ";
                    infoData.searchTables = "CIC.ANTRAG ANTRAG ";
                    infoData.searchConditions = " ZUSTAND IS NOT NULL ";
                    infoData.optimized = false;

                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.AntragsubersichtDto)))
                {
                    infoData = new QueryInfoDataType1("ANTRAG", "ANTRAG.SYSID");
                    infoData.resultFields = "ANTRAG.SYSID, ANTRAG.ANTRAG, ANTRAG.DATANGEBOT, ANTRAG.ZUSTAND, ANTRAG.BENUTZER, ANTRAG.BGEXTERN as AHK, ANTRAG.VART, ANTRAG.OBJEKTVT, ANTRAG.SYSKD, ANTRAG.SYSADM, PERSON.Name as NAME, ADMADD.BEZEICHNUNG as BEZEICHNUNG";
                    infoData.resultTables = "CIC.ANTRAG ANTRAG, CIC.PERSON PERSON, CIC.ADMADD ADMADD  ";
                    infoData.searchTables = "CIC.ANTRAG ANTRAG, CIC.PERSON PERSON, CIC.ADMADD ADMADD  ";
                    infoData.searchConditions = " ANTRAG.ZUSTAND='AKTIV' and ANTRAG.SYSKD = PERSON.SYSPERSON and ANTRAG.SYSADM = ADMADD.SYSADMADD  ";
                    infoData.optimized = true;
                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.AntragDto)))
                {
                    infoData = new QueryInfoDataType1("ANTRAG", "ANTRAG.SYSID");
                    infoData.resultFields = "ANTRAG.SYSID, ANTRAG.ANTRAG, ANTRAG.DATANGEBOT, ANTRAG.ZUSTAND, ANTRAG.BENUTZER, ANTRAG.BGEXTERN as AHK, ANTRAG.VART, ANTRAG.OBJEKTVT, ANTRAG.SYSKD, ANTRAG.SYSADM, PERSON.Name as NAME, ANTRAG.DRUCK, ANTRAG.ATTRIBUT";
                    infoData.resultTables = "CIC.ANTRAG ANTRAG, CIC.PERSON PERSON  ";
                    infoData.searchTables = "CIC.ANTRAG ANTRAG, CIC.PERSON PERSON  ";
                    infoData.searchConditions = " ANTRAG.SYSKD = PERSON.SYSPERSON  ";
                    infoData.optimized = true;
                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.EkfantrageDto)))
                {
                    infoData = new QueryInfoDataType1("ANTRAG", "ANTRAG.SYSID");
                    infoData.resultFields = "ANTRAG.SYSID, ANTRAG.ANTRAG, ANTRAG.DATANGEBOT, ANTRAG.ZUSTAND, ANTRAG.BENUTZER, ANTRAG.BGEXTERN as AHK, ANTRAG.VART, ANTRAG.OBJEKTVT, ANTRAG.SYSKD, ANTRAG.SYSADM, PERSON.Name as NAME, ADMADD.BEZEICHNUNG as BEZEICHNUNG";
                    infoData.resultTables = "CIC.ANTRAG ANTRAG, CIC.PERSON PERSON, CIC.ADMADD ADMADD  ";
                    infoData.searchTables = "CIC.ANTRAG ANTRAG, CIC.PERSON PERSON, CIC.ADMADD ADMADD  ";
                    infoData.searchConditions = " ANTRAG.VART like '%Einkaufs%' and ANTRAG.ZUSTAND='AKTIV' and ANTRAG.SYSKD = PERSON.SYSPERSON and ANTRAG.SYSADM = ADMADD.SYSADMADD  ";
                    infoData.optimized = true;
                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.AbgerechnetevertrageDto)))
                {
                    infoData = new QueryInfoDataType1("VT", "VT.SYSID");
                    infoData.resultFields = "VT.sysID, VT.vertrag, VT.ahk, KD.name, vt.sysls, to_char(vt.rueckgabe,'dd.mm.yyyy') as rueckgabe ";
                    infoData.resultTables = "CIC.VT VT, KD ";
                    infoData.searchTables = "CIC.VT VT, KD ";
                    infoData.searchConditions = " VT.sysKD=KD.sysPERSON and VT.zustand='AKTIV' and (VT.sysls=1 or VT.sysls=2) and INSTR(VT.VArt,'Kontokorrent',1,1)=0 and INSTR(VT.VART,'Servicekonto',1,1)=0 and INSTR(VT.VART,'servicelease',1,1)=0 and INSTR(VT.VART,'Einkaufsfinanzierung',1,1)=0 and (VT.SYSVART > 0 AND VT.SYSVART < 5 OR VT.SYSVART = 600) and VT.BGEXTERN>0 and VT.LZ>0 and INSTR(VT.Vertriebsweg,'Umschreibung',1,1)=0 and (VT.SYSVART>0 AND VT.SYSVART < 5 OR VT.SYSVART = 600) and VT.BGEXTERN > 0 and VT.LZ > 0 ";
                    infoData.optimized = true;
                    // and VT.sysadm=92905 and VT.rueckgabe>='01.11.2012' and VT.rueckgabe < '01.12.2012' 
                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.AbrufscheineDto)))
                {
                    infoData = new QueryInfoDataType1("VT", "VT.SYSID");
                    infoData.resultFields = @"distinct HD.CODE as HD_CODE, HD.NAME as HD_NAME, KD.CODE as KD_CODE, KD.NAME as KD_NAME,KD.SYSPERSON as KD_SYSPERSON,VT.VERTRAG as VT_VERTRAG,VT.SYSID as VT_SYSID, 
                                            ANTRAG.ANTRAG as ANTRAG_ANTRAG,ANTRAG.SYSID as ANTRAG_SYSID,to_char(WFTZUST.CREATEDATE,'dd.mm.yyyy') as C_DATE,WFTZUST.STATUS,to_char(WFTZUST.FINISHEDDATE,'dd.mm.yyyy') as FINISHEDDATE, WFTZUST.FINISHEDDATE AS FINISHDATE,VT.ZUSTAND, 
                                            ANTRAG.BENUTZER,ANTRAG.SYSID,ANTRAG.sysADM,ADMADD.BEZEICHNUNG as ADMADD_BEZEICHNUNG,HD.SYSPERSON as HD_SYSPERSON ";
                    infoData.resultTables = "CIC.ANTRAG ANTRAG, KD, CIC.ANTOB ANTOB, CIC.ANTOBHD ANTOBHD, HD, CIC.WFTZUST WFTZUST, CIC.VT VT, CIC.ADMADD ADMADD ";
                    infoData.searchTables = "CIC.ANTRAG ANTRAG, CIC.KD KD, CIC.ANTOB ANTOB, CIC.ANTOBHD ANTOBHD, CIC.HD HD, CIC.WFTZUST WFTZUST, CIC.VT VT, CIC.ADMADD ADMADD ";
                    infoData.searchConditions = @" ANTRAG.SYSID=VT.SysANTRAG(+) AND ANTOBHD.SYSHD=HD.SYSPERSON AND ANTOBHD.SYSOB = ANTOB.SYSOB AND ANTOB.SYSVT = ANTRAG.SYSID AND KD.SYSPERSON=ANTRAG.SYSKD AND ANTRAG.sysADM = ADMADD.sysADMADD AND ANTRAG.SYSID=WFTZUST.SYSLEASE AND
                                                    WFTZUST.STATE='ABRUF_TOYOTA'  ";
                    infoData.optimized = false;
                    // and ANTRAG.sysadm=92905 
                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.MemoDto)))
                {
                    infoData = new QueryInfoDataType1("WFMMEMO", "WFMMEMO.SYSWFMMEMO");
                    infoData.resultFields = "P.NAME ||' '|| P.VORNAME EDITUSERNAME, P2.NAME ||' '|| P2.VORNAME CREATEUSERNAME, SYSWFMMEMO, SYSWFMMKAT, SYSWFMTABLE, SYSLEASE,SYSIDWFTA,KURZBESCHREIBUNG, WFMMEMO.EDITDATE,  WFMMEMO.EDITTIME EDITTIMECLA,  WFMMEMO.EDITUSER, WFMMEMO.CREATEDATE,  WFMMEMO.CREATETIME CREATETIMECLA,  WFMMEMO.CREATEUSER, STR01, STR02,DAT01, DAT02, NOTIZMEMO, INT01, INT02";
                    infoData.resultTables = "WFTABLE, WFMMEMO, CIC.wfuser P, CIC.wfuser P2";
                    infoData.searchTables = "WFTABLE, WFMMEMO, CIC.wfuser P, CIC.wfuser P2";
                    infoData.searchConditions = "edituser=p.syswfuser (+) and createuser=p2.syswfuser (+) and WFMMEMO.syswfmtable = wftable.syswftable";
                    infoData.optimized = true;
                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.HistoryDto)))
                {
                    infoData = new QueryInfoDataType1("history", "history.HISTORYID");
                    infoData.resultFields = "historyid,datum,zeit,benutzer,art,casestep,caseid,titel,kategorie,area,sysid";			// rh 20170103: ADD zeit
                    infoData.resultTables = "VC_HISTORY history";
                    infoData.searchTables = "VC_HISTORY history";
                    infoData.searchConditions = "1=1";
                    infoData.optimized = true;
                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.RechnungFaelligDto)))
                {
                    infoData = new QueryInfoDataType1("RN", "RN.SYSID");
                    infoData.resultFields = "RN.SYSID, valutadatum,gbetrag+gsteuer betrag,rechnung,text,typ,person.name";
                    infoData.resultTables = "rn,person";
                    infoData.searchTables = "rn,person";
                    infoData.searchConditions = "person.sysperson=rn.sysperson and faelligdatum<=sysdate and bezahlt=0";
                    infoData.optimized = false;
                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.TilgungDto)))
                {
                    infoData = new QueryInfoDataType1("SLPOS", "SLPOS.sysslpos");
                    infoData.resultFields = "SLPOS.sysslpos, SLPOS.VALUTA,SLPOS.BETRAG, obbrief.fident,nkonto.bezeichnung name, SLPOSTYP.name ereignis, SLPOS.tilgtage tage, SLPOS.tilgungp, SLPOS.tilgung";
                    infoData.resultTables = "CIC.SLPOS, CIC.SLPOSTYP, cic.nkk NKK, cic.sllink,  cic.sl, cic.ob, cic.obbrief, cic.nkonto";
                    infoData.searchTables = "CIC.SLPOS, CIC.SLPOSTYP, cic.nkk NKK, cic.sllink,  cic.sl, cic.ob, cic.obbrief, cic.nkonto";
                    infoData.searchConditions = "SLPOS.SYSSLPOSTYP = SLPOSTYP.SYSSLPOSTYP (+) and SLPOS.syssl=sl.syssl and sllink.syssl = sl.syssl AND sllink.gebiet = 'NKK' and nkk.sysnkk=sllink.sysid and nkk.sysnkk=ob.sysnkk (+) and ob.sysob=obbrief.sysobbrief (+) and nkonto.sysnkonto = nkk.sysnkonto";
                    infoData.optimized = false;
                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.FinanzierungDto)))
                {
                    infoData = new QueryInfoDataType1("NKK", "NKK.SYSNKK");
                    infoData.resultFields = "NKK.SYSNKK, NKK.SYSNKONTO, NKK.BEGINN, NKK.SYSNKKTYP, NKK.SYSRVT, NKK.SYSPRPRODUCT, NKK.ZUSTAND, NKK.ENDE, NKK.NOMINAL, NKK.ENDEVF, NKK.AUSZAHLUNGP, NKK.AUSZAHLUNG, NKK.AUSZAHLUNGAM, NKK.SYSPRINTSETVF, NKK.SYSPRINTSETAF, NKK.SYSPRTLGSET, NKK.SALDO,  OB.SYSOB, OB.OBJEKT OBJEKT,  OBBRIEF.FIDENT, OBBRIEF.LAUFNUMMER, RN.RECHNUNG, RN.VALUTADATUM, WAEHRUNG.CODE WAEHRUNGCODE,  PRPRODUCT.NAME PRODUCTNAME,  RVT.SYSPERSON,  PERSON.CODE PERSONCODE, PERSON.NAME PERSONNAME, NKONTO.KONTO NKONTO, NKKTYP.PPY, NKKTYP.DCC, NKKTYP.bdc, NKKTYP.SUPPRESSCAP, NKKTYP.INTFIRSTDAY, NKKTYP.INTLASTDAY, RVT.RAHMEN, NKKTYP.NAME KONTOTYP ";
                    infoData.resultTables = "CIC.NKK NKK, CIC.OB OB, CIC.OBBRIEF OBBRIEF, CIC.RN RN, CIC.NKONTO NKONTO, CIC.WAEHRUNG WAEHRUNG, CIC.NKKTYP NKKTYP, CIC.PRPRODUCT PRPRODUCT, CIC.RVT RVT, CIC.PERSON PERSON ";
                    infoData.searchTables = "CIC.NKK NKK, CIC.OB OB, CIC.OBBRIEF OBBRIEF, CIC.RN RN, CIC.NKONTO NKONTO, CIC.WAEHRUNG WAEHRUNG, CIC.NKKTYP NKKTYP, CIC.PRPRODUCT PRPRODUCT, CIC.RVT RVT, CIC.PERSON PERSON ";
                    infoData.searchConditions = "NKK.SYSNKK=OB.SYSNKK (+) AND OB.SYSOB=OBBRIEF.SYSOBBRIEF (+) AND NKK.SYSNKONTO=NKONTO.SYSNKONTO (+) AND NKK.SYSNKKTYP=NKKTYP.SYSNKKTYP (+) AND NKK.SYSPRPRODUCT=PRPRODUCT.SYSPRPRODUCT (+) AND NKK.SYSRVT=RVT.SYSRVT (+) AND RVT.SYSPERSON=PERSON.SYSPERSON (+) AND OB.SYSOB = RN.SYSOB (+) AND NKONTO.SYSWAEHRUNG=WAEHRUNG.SYSWAEHRUNG (+) AND  RN.SYSRNTYP(+) = 1 AND NKK.SYSNKONTO IN (SELECT N.SYSNKONTO FROM NKONTO N, SKONTO S WHERE N.SYSSKONTO = S.SYSSKONTO AND S.SYSLS = 1) ";
                    infoData.optimized = false;
                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.ZinsabschlDto)))
                {
                    infoData = new QueryInfoDataType1("NKKABSCHL", "NKKABSCHL.SYSNKK");
                    infoData.resultFields = "NKKABSCHL.VON VON, NKKABSCHL.BIS BIS,NKKABPOS.TEXT TEXT,NKKABSCHL.STORNO STORNO,NKKABPOS.INTRATE INRATE, NKKABPOS.BETRAG BETRAG ";
                    infoData.resultTables = "CIC.NKKABSCHL NKKABSCHL, CIC.NKKABPOS NKKABPOS, CIC.NKK NKK ";
                    infoData.searchTables = "CIC.NKKABSCHL NKKABSCHL, CIC.NKKABPOS NKKABPOS, CIC.NKK NKK "; 
                    infoData.searchConditions = "NKKABPOS.SYSNKKABSCHL = NKKABSCHL.SYSNKKABSCHL AND NKKABSCHL.SYSNKK = NKK.SYSNKK ";
                    infoData.optimized = false;
                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.ContactDto)))
                {
                    infoData = new QueryInfoDataType1("CONTACT", "CONTACT.SYSCONTACT");
                    infoData.resultFields = "CONTACT.*, CONTACTTP.NAME contactTpName,  PERSON.NAME personName, CONTACT.COMDATE  ||' '|| CONTACT.COMTIME ||' '|| CONTACT.COMDURA ENDE, CONTACT.SYSPARTNER, PARTNER.NAME PARTNERNAME, OPPO.NAME OPPONAME, PERSON.VORNAME personVorname";
                    infoData.resultTables = "CIC.CONTACT CONTACT, CONTACTTP, PERSON PERSON, PERSON PARTNER, OPPO OPPO, PTRELATE PTRELATE ";
                    infoData.searchTables = "CIC.CONTACT CONTACT, CONTACTTP, PERSON PERSON, PERSON PARTNER, OPPO OPPO, PTRELATE PTRELATE ";
                    infoData.searchConditions = " CONTACT.SYSCONTACTTP = CONTACTTP.SYSCONTACTTP(+) and CONTACT.SYSPERSON = PERSON.SYSPERSON (+) and CONTACT.SYSPARTNER = PTRELATE.SYSPTRELATE (+) and PARTNER.SYSPERSON (+)= PTRELATE.SYSPERSON2 and CONTACT.SYSOPPO = OPPO.SYSOPPO (+)";
                    infoData.permissionCondition = " and contact.syscontact in (SELECT sysid FROM peuni, perolecache WHERE peuni.area = 'CONTACT' AND peuni.sysperole = perolecache.syschild AND perolecache.sysparent = {0} and {1}) ";
                    infoData.optimized = true;

                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.KontoDto)))
                {
                    infoData = new QueryInfoDataType1("KONTO", "KONTO.SYSKONTO");
                    infoData.resultFields = "KONTO.*, BLZ.BLZ, BLZ.BIC, (select count(sysid) from rn where (sysperson = konto.sysperson or syski=konto.sysperson) and bezahlt=0 and einzug=1 and art=1 and rn.zahlsperre=0 and stornokz=0 ) readOnly ";
                    infoData.resultTables = "CIC.KONTO KONTO, CIC.BLZ ";
                    infoData.searchTables = "CIC.KONTO KONTO, CIC.BLZ ";
                    infoData.searchConditions = " KONTO.SYSBLZ = BLZ.SYSBLZ(+) ";
                    infoData.optimized = true;
                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.ItkontoDto)))
                {
                    infoData = new QueryInfoDataType1("ITKONTO", "ITKONTO.SYSITKONTO");
                    infoData.resultFields = "ITKONTO.*, BLZ.BLZ, BLZ.BIC ";
                    infoData.resultTables = "CIC.ITKONTO ITKONTO, CIC.BLZ ";
                    infoData.searchTables = "CIC.ITKONTO ITKONTO, CIC.BLZ ";
                    infoData.searchConditions = " ITKONTO.SYSBLZ = BLZ.SYSBLZ(+) ";
                    infoData.optimized = true;
                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.CampDto)))
                {
                    infoData = new QueryInfoDataType1("CAMP", "CAMP.SYSCAMP");
                    infoData.resultFields = "camp.*,case when (select count(c.syscamp) from camp p left outer join camp c on p.syscamp=c.syscampparent where p.syscamp=camp.syscamp group by p.syscamp) >0 then -1 else 0 end children, camptp.name sysCampTpBezeichnung ";
                    infoData.resultTables = "CIC.CAMP CAMP, CIC.CAMPTP CAMPTP ";
                    infoData.searchTables = "CIC.CAMP CAMP, CIC.CAMPTP CAMPTP  ";
                    infoData.searchConditions = " CAMP.SYSCAMPTP = CAMPTP.SYSCAMPTP(+) ";
                    infoData.optimized = true;

                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.AdresseDto)))
                {
                    infoData = new QueryInfoDataType1("ADRESSE", "ADRESSE.SYSADRESSE");
                    infoData.resultFields = "ADRESSE.*";
                    infoData.resultTables = "CIC.ADRESSE ADRESSE, CIC.ADRTP ADRTP ";
                    infoData.searchTables = "CIC.ADRESSE ADRESSE, CIC.ADRTP ADRTP  ";
                    infoData.searchConditions = " ADRESSE.SYSADRTP = ADRTP.SYSADRTP (+) ";
                    infoData.optimized = true;

                }  
                else if (typeof(T).Equals(typeof(Cic.One.DTO.StrasseDto)))
                {
                    infoData = new QueryInfoDataType1("STRASSE", "STRASSE.SYSSTRASSE");
                    infoData.resultFields = "STRASSE.SYSSTRASSE, STRASSE.STRBEZ STRASSE, STRASSE.SYSLAND sysland, STRASSE.PLZ plz, LAND.ISO land, LAND.COUNTRYNAME landBezeichnung, PLZ.SYSPLZ sysplz, PLZ.BEZIRK bezirk, PLZ.ORT ort, CTLANG.SYSCTLANG sysctlang, CTLANG.LANGUAGENAME ctLangBezeichnung ";
                    infoData.resultTables = "CIC.STRASSE STRASSE, CIC.PLZ PLZ, CIC.LAND LAND, CIC.CTLANG CTLANG ";
                    infoData.searchTables = "CIC.STRASSE STRASSE, CIC.PLZ PLZ, CIC.LAND LAND, CIC.CTLANG CTLANG ";
                    infoData.searchConditions = " STRASSE.PLZ = PLZ.PLZ(+) and PLZ.SYSLAND = LAND.SYSLAND and STRASSE.STRBEZSPC = CTLANG.SYSCTLANG";
                    infoData.optimized = true;

                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.PlzDto)))
                {
                    infoData = new QueryInfoDataType1("PLZ", "PLZ.SYSPLZ");
                    infoData.resultFields = "PLZ.PLZ plz, PLZ.BEZIRK bezirk, PLZ.ORT ort ";
                    infoData.resultTables = "CIC.PLZ PLZ, CIC.LAND LAND  ";
                    infoData.searchTables = "CIC.PLZ PLZ, CIC.LAND LAND  ";
                    infoData.searchConditions = " PLZ.SYSLAND = LAND.SYSLAND ";
                    infoData.optimized = true;

                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.ItadresseDto)))
                {
                    infoData = new QueryInfoDataType1("ITADRESSE", "ITADRESSE.SYSITADRESSE");
                    infoData.resultFields = "ITADRESSE.*";
                    infoData.resultTables = "CIC.ITADRESSE ITADRESSE, CIC.ADRTP ADRTP ";
                    infoData.searchTables = "CIC.ITADRESSE ITADRESSE, CIC.ADRTP ADRTP  ";
                    infoData.searchConditions = " ITADRESSE.SYSADRTP = ADRTP.SYSADRTP (+) ";
                    infoData.optimized = true;

                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.PtaskDto)))
                {
                    infoData = new QueryInfoDataType1("PTASK", "PTASK.SYSPTASK");
                    infoData.resultFields = "PTASK.* , PTASK.SYSOWNER SYSOWNEROLD,   PARTNER.NAME PARTNERNAME,  PERSON.NAME personName, WFUSER.CODE wfuserName";
                    infoData.resultTables = "CIC.PTASK PTASK, PERSON PERSON, PERSON PARTNER, PTRELATE PTRELATE, WFUSER WFUSER  ";
                    infoData.searchTables = "CIC.PTASK PTASK, PERSON PERSON, PERSON PARTNER, PTRELATE PTRELATE, WFUSER WFUSER  ";
                    infoData.searchConditions = " PTASK.SYSPERSON = PERSON.SYSPERSON (+) and PTASK.SYSPARTNER = PTRELATE.SYSPTRELATE (+) and PARTNER.SYSPERSON (+)= PTRELATE.SYSPERSON2 and PTASK.SYSOWNER (+)= WFUSER.SYSWFUSER ";
                    infoData.permissionCondition = " and ptask.sysptask in (SELECT sysid FROM peuni, perolecache WHERE area = 'PTASK' AND peuni.sysperole = perolecache.syschild AND perolecache.sysparent = {0} and {1}) ";
                    infoData.optimized = true;

                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.ApptmtDto)))
                {
                    infoData = new QueryInfoDataType1("APPTMT", "APPTMT.SYSAPPTMT");
                    infoData.resultFields = "APPTMT.*, APPTMT.SYSOWNER SYSOWNEROLD, PARTNER.NAME PARTNERNAME,  PERSON.NAME personName, WFUSER.CODE wfuserName";
                    infoData.resultTables = "CIC.APPTMT APPTMT, PERSON PERSON, PERSON PARTNER, PTRELATE PTRELATE, WFUSER WFUSER, PEUNI PEUNI, PEROLECACHE PEROLECACHE  ";
                    infoData.searchTables = "CIC.APPTMT APPTMT, PERSON PERSON, PERSON PARTNER, PTRELATE PTRELATE, WFUSER WFUSER, PEUNI PEUNI, PEROLECACHE PEROLECACHE ";
                    infoData.searchConditions = " APPTMT.SYSPERSON = PERSON.SYSPERSON (+) and APPTMT.SYSPARTNER = PTRELATE.SYSPTRELATE (+) and PARTNER.SYSPERSON (+)= PTRELATE.SYSPERSON2 and APPTMT.SYSOWNER (+)= WFUSER.SYSWFUSER " +
                                                " and APPTMT.SYSAPPTMT = PEUNI.SYSID and PEUNI.AREA = 'APPTMT' and PEUNI.SYSPEROLE = PEROLECACHE.SYSCHILD";
                    infoData.permissionCondition = "  AND PEROLECACHE.SYSPARENT = {0} and {1} ";
                    infoData.optimized = false;
                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.RecurrDto)))
                {
                    infoData = new QueryInfoDataType1("RECURR", "RECURR.SYSRECURR");
                    infoData.resultFields = "RECURR.* ";
                    infoData.resultTables = "CIC.RECURR RECURR, APPTMT APPTMT, PERSON PERSON, PERSON PARTNER, PTRELATE PTRELATE, WFUSER WFUSER, PEUNI PEUNI, PEROLECACHE PEROLECACHE  ";
                    infoData.searchTables = "CIC.RECURR RECURR, APPTMT APPTMT, PERSON PERSON, PERSON PARTNER, PTRELATE PTRELATE, WFUSER WFUSER, PEUNI PEUNI, PEROLECACHE PEROLECACHE   ";
                    infoData.searchConditions = " APPTMT.SYSAPPTMT = RECURR.SYSAPPTMT (+) and APPTMT.SYSPERSON = PERSON.SYSPERSON (+) and APPTMT.SYSPARTNER = PTRELATE.SYSPTRELATE (+) and PARTNER.SYSPERSON (+)= PTRELATE.SYSPERSON2 and APPTMT.SYSOWNER (+)= WFUSER.SYSWFUSER " +
                                                " and APPTMT.SYSAPPTMT = PEUNI.SYSID and PEUNI.AREA = 'APPTMT' and PEUNI.SYSPEROLE = PEROLECACHE.SYSCHILD"; ;
                    infoData.permissionCondition = " AND PEROLECACHE.SYSPARENT = {0} and {1} ";
                    infoData.optimized = false;
                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.ReminderDto)))
                {
                    //OpenLease query for user:
                    //SELECT REMINDER.*, BPROLE.NAMEBPROLE FROM CIC.REMINDER REMINDER, CIC.BPROLE BPROLE WHERE  REMINDER.SYSBPROLE=BPROLE.SYSBPROLE (+) AND reminder.activeflag = 1 AND (nvl(reminder.sysbpprocinst,0) = 0 OR EXISTS(SELECT 1 FROM bplistener lst WHERE lst.eventcode = reminder.eventcode AND lst.oltable = reminder.area AND lst.sysoltable = reminder.sysid AND reminder.sysbpprocinst = lst.sysbpprocinst)) AND ((reminder.syswfuser = {0} AND CIC.MDBS_TOORATIME(reminder.starttime, reminder.startdate) < SYSDATE) OR (reminder.syswfuser IS NULL AND NVL(reminder.sysbprole, 0) IN (SELECT sysbprole FROM bprole bp1, bproleusr me WHERE me.syswfuser = {0} AND me.onlyproxyflag = 0 AND me.activeflag = 1 AND me.namebprole = bp1.namebprole)) OR (reminder.syswfuser IS NOT NULL AND reminder.privateflag = 0 AND NVL(reminder.sysbprole, 0) IN (SELECT sysbprole FROM bprole bp1, bproleusr me WHERE me.syswfuser = {0} AND me.onlyproxyflag = 0 AND me.activeflag = 1 AND me.namebprole = bp1.namebprole) AND CIC.MDBS_TOORATIME(reminder.duetime, reminder.duedate) < SYSDATE) OR (EXISTS (SELECT 1 FROM table(CIC.MDBS_GetTabProxyRole({0})) proxy WHERE proxy.sysbprole = reminder.sysbprole AND proxy.syswfuser = reminder.syswfuser)) OR EXISTS (SELECT 1 FROM table(CIC.MDBS_GetTabProxyRole({0})) proxy WHERE proxy.sysbprole = reminder.sysbprole AND reminder.syswfuser IS NULL)) ORDER BY REMINDER.STARTDATE, DECODE(NVL(REMINDER.STARTTIME, 0),0,9999999,REMINDER.STARTTIME), REMINDER.PRIORITY

                    infoData = new QueryInfoDataType1("REMINDER", "REMINDER.SYSREMINDER");
                    infoData.resultFields = "REMINDER.*, BPROLE.NAMEBPROLE ";
                    infoData.resultTables = "CIC.REMINDER REMINDER, CIC.BPROLE BPROLE ";
                    infoData.searchTables = "CIC.REMINDER REMINDER, CIC.BPROLE BPROLE ";
                    infoData.searchConditions = " REMINDER.SYSBPROLE=BPROLE.SYSBPROLE (+) AND reminder.activeflag = 1 AND (nvl(reminder.sysbpprocinst,0) = 0 OR EXISTS(SELECT 1 FROM bplistener lst WHERE lst.eventcode = reminder.eventcode AND lst.oltable = reminder.area AND lst.sysoltable = reminder.sysid AND reminder.sysbpprocinst = lst.sysbpprocinst)) AND ((reminder.syswfuser = {WFUSER} AND CIC.MDBS_TOORATIME(reminder.starttime, reminder.startdate) < SYSDATE) OR (reminder.syswfuser IS NULL AND NVL(reminder.sysbprole, 0) IN (SELECT sysbprole FROM bprole bp1, bproleusr me WHERE me.syswfuser = {WFUSER} AND me.onlyproxyflag = 0 AND me.activeflag = 1 AND me.namebprole = bp1.namebprole)) OR (reminder.syswfuser IS NOT NULL AND reminder.privateflag = 0 AND NVL(reminder.sysbprole, 0) IN (SELECT sysbprole FROM bprole bp1, bproleusr me WHERE me.syswfuser = {WFUSER} AND me.onlyproxyflag = 0 AND me.activeflag = 1 AND me.namebprole = bp1.namebprole) AND CIC.MDBS_TOORATIME(reminder.duetime, reminder.duedate) < SYSDATE) OR (EXISTS (SELECT 1 FROM table(CIC.MDBS_GetTabProxyRole({WFUSER})) proxy WHERE proxy.sysbprole = reminder.sysbprole AND proxy.syswfuser = reminder.syswfuser)) OR EXISTS (SELECT 1 FROM table(CIC.MDBS_GetTabProxyRole({WFUSER})) proxy WHERE proxy.sysbprole = reminder.sysbprole AND reminder.syswfuser IS NULL))";
                    infoData.optimized = true;
                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.MailmsgDto)))
                {

                    infoData = new QueryInfoDataType1("MAILMSG", "MAILMSG.SYSMAILMSG");
                    infoData.resultFields = "MAILMSG.*,   PARTNER.NAME PARTNERNAME,  PERSON.NAME personName, (select count(1) from fileatt where mailmsg.sysmailmsg=fileatt.sysmailmsg) as attachments, WFUSER.CODE wfuserName";
                    infoData.resultTables = "CIC.MAILMSG MAILMSG, PERSON PERSON, PERSON PARTNER, PTRELATE PTRELATE, WFUSER WFUSER ";
                    infoData.searchTables = "CIC.MAILMSG MAILMSG, PERSON PERSON, PERSON PARTNER, PTRELATE PTRELATE, WFUSER WFUSER ";
                    //infoData.searchConditions = " MAILMSG.SYSPERSON = PERSON.SYSPERSON (+) and MAILMSG.SYSPARTNER = PARTNER.SYSPERSON (+)  ";
                    infoData.searchConditions = " MAILMSG.SYSPERSON = PERSON.SYSPERSON (+) and MAILMSG.SYSPARTNER = PTRELATE.SYSPTRELATE (+) and PARTNER.SYSPERSON (+)= PTRELATE.SYSPERSON2 and MAILMSG.SYSOWNER (+)= WFUSER.SYSWFUSER ";
                    infoData.permissionCondition = " and mailmsg.sysmailmsg in (SELECT sysid FROM peuni, perolecache WHERE area = 'MAILMSG' AND peuni.sysperole = perolecache.syschild AND perolecache.sysparent = {0} and {1}) ";
                    infoData.optimized = true;

                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.PrunDto)))
                {
                    infoData = new QueryInfoDataType1("PRUN", "PRUN.SYSPRUN");
                    infoData.resultFields = "* ";
                    infoData.resultTables = "CIC.PRUN PRUN ";
                    infoData.searchTables = "CIC.PRUN PRUN ";
                    infoData.searchConditions = " 1=1 ";
                    infoData.optimized = true;

                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.FileattDto)))
                {
                    infoData = new QueryInfoDataType1("FILEATT", "FILEATT.SYSFILEATT");
                    infoData.resultFields = "* ";
                    infoData.resultTables = "CIC.FILEATT FILEATT ";
                    infoData.searchTables = "CIC.FILEATT FILEATT ";
                    infoData.searchConditions = " 1=1 ";
                    infoData.optimized = true;

                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.PrproductDto)))
                {
                    infoData = new QueryInfoDataType1("PRPRODUCT", "PRPRODUCT.SYSPRPRODUCT");
                    infoData.resultFields = "* ";
                    infoData.resultTables = "CIC.PRPRODUCT PRPRODUCT ";
                    infoData.searchTables = "CIC.PRPRODUCT PRPRODUCT ";
                    infoData.searchConditions = " 1=1 ";
                    infoData.optimized = true;

                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.ItemcatDto)))
                {
                    infoData = new QueryInfoDataType1("ITEMCAT", "ITEMCAT.SYSITEMCAT");
                    infoData.resultFields = "* ";
                    infoData.resultTables = "CIC.ITEMCAT ITEMCAT ";
                    infoData.searchTables = "CIC.ITEMCAT ITEMCAT ";
                    infoData.searchConditions = " 1=1 ";
                    infoData.optimized = true;

                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.CtlangDto)))
                {
                    infoData = new QueryInfoDataType1("CTLANG", "CTLANG.SYSCTLANG");
                    infoData.resultFields = "* ";
                    infoData.resultTables = "CIC.CTLANG CTLANG ";
                    infoData.searchTables = "CIC.CTLANG CTLANG ";
                    infoData.searchConditions = " 1=1 ";
                    infoData.optimized = true;

                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.LandDto)))
                {
                    infoData = new QueryInfoDataType1("LAND", "LAND.SYSLAND");
                    infoData.resultFields = "* ";
                    infoData.resultTables = "CIC.LAND LAND ";
                    infoData.searchTables = "CIC.LAND LAND ";
                    infoData.searchConditions = " 1=1 ";
                    infoData.optimized = true;

                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.BrancheDto)))
                {
                    infoData = new QueryInfoDataType1("BRANCHE", "BRANCHE.SYSBRANCHE");
                    infoData.resultFields = "* ";
                    infoData.resultTables = "CIC.BRANCHE BRANCHE ";
                    infoData.searchTables = "CIC.BRANCHE BRANCHE ";
                    infoData.searchConditions = " 1=1 ";
                    infoData.optimized = true;

                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.AccountDto)))
                {
                    infoData = new QueryInfoDataType1("PERSON", "PERSON.SYSPERSON");
                    infoData.resultFields = "sysperson, matchcode, code,    syskdtyp,  anrede, anredecode, titel, titelCode, titel2,name, vorname, gebdatum, sysctlang, sysctlangkorr, syslandnat, einreisedatum, auslausweis, strasse, hsnr, plz, ort, sysland, sysstaat, wohnseit, strasse2, hsnr2, plz2, ort2, sysland2, sysstaat2, telefon, telefon2, handy, erreichbtel, erreichbvon, erreichbbis, email, url, sysbranche, rechtsform, rechtsformcode, gruendung, hregister, hregisterflag, revflag, zusatz, namekont, vornamekont,  fax, gebort, hregisterort, identeu, steuernr, identust, nomailingflag,privatflag, gesflag";
                    infoData.resultTables = "CIC.PERSON PERSON ";
                    infoData.searchTables = "CIC.PERSON PERSON ";
                    infoData.permissionCondition = "  AND person.sysperson=peuni.sysid and peuni.area = 'PERSON' AND peuni.sysperole = perolecache.syschild and perolecache.sysparent = {0} and {1} ";
                    infoData.permissionTables = ",peuni, perolecache";
                    infoData.searchConditions = " 1=1 ";
                    //TODO make hint configurable
                    infoData.queryStruct.partialQuery = "SELECT * FROM (SELECT rownum rnum, a.* FROM( select  " + hints[typeof(Cic.One.DTO.AccountDto)] + " {0} from {1} where {2} ) a WHERE rownum <= {3}) WHERE rnum > {4}";
                    infoData.optimized = false;

                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.WktaccountDto)))
                {
                    /*
                     *create view wkt_person_view as 
                        (SELECT IT.sysit*-1 AS wktid, it.sysit, fparkgroesse,sysperson, matchcode, code,    syskdtyp,  anrede, anredecode, titel, titelCode, titel2,trim(name) name, trim(vorname) vorname, gebdatum, sysctlang, sysctlangkorr, syslandnat, einreisedatum, auslausweis, strasse, hsnr, plz, ort, sysland, sysstaat, wohnseit, strasse2, hsnr2, plz2, ort2, sysland2, sysstaat2, telefon, telefon2, handy, erreichbtel, erreichbvon, erreichbbis, email, url, sysbranche, '' rechtsform, rechtsformcode, gruendung, hregister, hregisterflag, revflag, zusatz, namekont, vornamekont,  fax,'' as gebort,'' as hregisterort,'' as identeu, '' as steuernr, identust,0 as nomailingflag,privatflag,0 as gesflag, 0 AS FlagKd, 'GROßKUNDE' AS kundengruppe FROM IT WHERE NOT EXISTS (SELECT 1 FROM PEUNI WHERE AREA = 'IT' AND PEUNI.SYSID = IT.SYSIT))
                        union all
                        (SELECT sysperson as wktid, 0 as sysit, fparkgroesse,sysperson, matchcode, code,    syskdtyp,  anrede, anredecode, titel, titelCode, titel2,trim(name) name, trim(vorname) vorname, gebdatum, sysctlang, sysctlangkorr, syslandnat, einreisedatum, auslausweis, strasse, hsnr, plz, ort, sysland, sysstaat, wohnseit, strasse2, hsnr2, plz2, ort2, sysland2, sysstaat2, telefon, telefon2, handy, erreichbtel, erreichbvon, erreichbbis, email, url, sysbranche, rechtsform, rechtsformcode, gruendung, hregister, hregisterflag, revflag, zusatz, namekont, vornamekont,  fax, gebort, hregisterort, identeu, steuernr, identust, nomailingflag,privatflag, gesflag, 1 AS FlagKd, kundengruppe FROM PERSON WHERE PERSON.flagkd = 1 AND PERSON.aktivkz = 1)
                        ;
                        */
                    infoData = new QueryInfoDataType1("wkt_person_view", "PERSON.WKTID");
                    infoData.resultFields = "wktid, sysit, sysperson,fparkgroesse, matchcode, code,    syskdtyp,  anrede, anredecode, titel, titelCode, titel2,name, vorname, gebdatum, sysctlang, sysctlangkorr, syslandnat, einreisedatum, auslausweis, strasse, hsnr, plz, ort, sysland, sysstaat, wohnseit, strasse2, hsnr2, plz2, ort2, sysland2, sysstaat2, telefon, telefon2, handy, erreichbtel, erreichbvon, erreichbbis, email, url, sysbranche, rechtsform, rechtsformcode, gruendung, hregister, hregisterflag, revflag, zusatz, namekont, vornamekont,  fax, gebort, hregisterort, identeu, steuernr, identust, nomailingflag,privatflag, gesflag, flagkd, kundengruppe";
                    infoData.resultTables = "wkt_person_view PERSON ";
                    infoData.searchTables = "wkt_person_view PERSON ";
                    //infoData.permissionCondition = "  AND person.sysperson=peuni.sysid and peuni.area = 'PERSON' AND peuni.sysperole = perolecache.syschild and perolecache.sysparent = {0} and {1} ";
                    //infoData.permissionTables = ",peuni, perolecache";
                    infoData.searchConditions = " 1=1 ";
                    //TODO make hint configurable
                    infoData.queryStruct.partialQuery = "SELECT * FROM (SELECT rownum rnum, a.* FROM( select  {0} from {1} where {2} ) a WHERE rownum <= {3}) WHERE rnum > {4}";
                    infoData.optimized = false;

                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.WfuserDto)))
                {
                    infoData = new QueryInfoDataType1("WFUSER", "WFUSER.SYSWFUSER");
                    infoData.resultFields = "* ";
                    infoData.resultTables = "CIC.WFUSER WFUSER ";
                    infoData.searchTables = "CIC.WFUSER WFUSER ";
                    infoData.searchConditions = " 1=1 ";
                    infoData.optimized = true;

                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.OpportunityDto)))
                {
                    infoData = new QueryInfoDataType1("OPPO", "OPPO.SYSOPPO");
                    infoData.resultFields = "OPPO.*, OPPOTP.NAME oppoTpBezeichnung, PERSON.NAME personName ";
                    infoData.resultTables = "CIC.OPPO OPPO, CIC.OPPOTP OPPOTP, CIC.PERSON PERSON, CIC.PTRELATE PTRELATE ";
                    infoData.searchTables = "CIC.OPPO OPPO, CIC.OPPOTP OPPOTP, CIC.PERSON PERSON, CIC.PTRELATE PTRELATE ";
                    infoData.searchConditions = " OPPO.SYSOPPOTP = OPPOTP.SYSOPPOTP(+) and OPPO.SYSPERSON = PERSON.SYSPERSON (+) and OPPO.SYSPARTNER = PTRELATE.SYSPTRELATE (+)";
                    //infoData.permissionCondition = " and oppo.sysoppo in (SELECT sysid FROM peuni, perolecache WHERE area = 'OPPO' AND peuni.sysperole = perolecache.syschild AND perolecache.sysparent = {0} and {1}) ";
                    infoData.optimized = true;

                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.AdrtpDto)))
                {
                    infoData = new QueryInfoDataType1("ADRTP", "ADRTP.SYSADRTP");
                    infoData.resultFields = "* ";
                    infoData.resultTables = "CIC.ADRTP ADRTP ";
                    infoData.searchTables = "CIC.ADRTP ADRTP ";
                    infoData.searchConditions = " 1=1 ";
                    infoData.optimized = true;

                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.KontotpDto)))
                {
                    infoData = new QueryInfoDataType1("KONTOTP", "KONTOTP.SYSKONTOTP");
                    infoData.resultFields = "* ";
                    infoData.resultTables = "CIC.KONTOTP KONTOTP ";
                    infoData.searchTables = "CIC.KONTOTP KONTOTP ";
                    infoData.searchConditions = " 1=1 ";
                    infoData.optimized = true;

                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.BlzDto)))
                {
                    infoData = new QueryInfoDataType1("BLZ", "BLZ.SYSBLZ");
                    infoData.resultFields = "BLZ.* ";
                    infoData.resultTables = "CIC.BLZ BLZ ";
                    infoData.searchTables = "CIC.BLZ BLZ ";
                    infoData.searchConditions = " 1=1";
                    infoData.optimized = true;

                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.PtrelateDto)))
                {
                    infoData = new QueryInfoDataType1("PTRELATE", "PTRELATE.SYSPTRELATE");
                    infoData.resultFields = "PTRELATE.*,person2.NAME beziehungzu, PERSON.SYSPERSON, PERSON.NAME, PERSON.CODE, PERSON.VORNAME,  PERSON.PLZ, PERSON.ORT, PERSON.STRASSE, PERSON.TELEFON, PERSON.HSNR";
                    infoData.resultTables = "CIC.PTRELATE PTRELATE, CIC.PERSON PERSON, CIC.PERSON PERSON2 ";
                    infoData.searchTables = "CIC.PTRELATE PTRELATE, CIC.PERSON PERSON, CIC.PERSON PERSON2 ";
                    infoData.searchConditions = "PTRELATE.SYSPERSON2 = PERSON.SYSPERSON(+) and person.name is not null and person2.sysperson=ptrelate.sysperson1 ";
                    infoData.queryStruct.partialQuery = "SELECT * FROM (SELECT rownum rnum, a.* FROM( select  " + hints[typeof(Cic.One.DTO.AccountDto)] + " {0} from {1} where {2} ) a WHERE rownum <= {3}) WHERE rnum > {4}";
                    infoData.optimized = true;
                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.CrmnmDto)))
                {
                    infoData = new QueryInfoDataType1("Crmnm", "Crmnm.SYSCrmnm");
                    infoData.resultFields = "Crmnm.*, person.sysperson, matchcode, code,    syskdtyp,  anrede, anredecode, titel, titelCode, titel2,name, vorname, gebdatum, sysctlang, sysctlangkorr, syslandnat, einreisedatum, auslausweis, strasse, hsnr, plz, ort, sysland, sysstaat, wohnseit, strasse2, hsnr2, plz2, ort2, sysland2, sysstaat2, telefon, telefon2, handy, erreichbtel, erreichbvon, erreichbbis, email, url, sysbranche, rechtsform, rechtsformcode, gruendung, hregister, hregisterflag, revflag, zusatz, namekont, vornamekont,  fax, gebort, hregisterort, identeu, steuernr, identust, nomailingflag,privatflag, gesflag";
                    infoData.resultTables = "CIC.Crmnm Crmnm, CIC.PERSON PERSON ";
                    infoData.searchTables = "CIC.Crmnm Crmnm, CIC.PERSON PERSON ";
                    infoData.searchConditions = "Crmnm.sysidchild = PERSON.SYSPERSON(+) ";
                    infoData.queryStruct.partialQuery = "SELECT * FROM (SELECT rownum rnum, a.* FROM( select  " + hints[typeof(Cic.One.DTO.AccountDto)] + " {0} from {1} where {2} ) a WHERE rownum <= {3}) WHERE rnum > {4}";
                    infoData.optimized = false;
                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.PartnerDto)))
                {
                  

                    infoData = new QueryInfoDataType1("PTRELATE", "PTRELATE.SYSPTRELATE");
                    infoData.resultFields = "PTRELATE.*, person2.NAME beziehungzu,person.sysperson, person.matchcode, person.code,    person.syskdtyp,  person.anrede, person.anredecode, person.titel, person.titelCode, person.titel2,person.name, person.vorname, person.gebdatum, person.sysctlang, person.sysctlangkorr, person.syslandnat, person.einreisedatum, person.auslausweis, person.strasse, person.hsnr, person.plz, person.ort, person.sysland, person.sysstaat, person.wohnseit, person.strasse2, person.hsnr2, person.plz2, person.ort2, person.sysland2, person.sysstaat2, person.telefon, person.telefon2, person.handy, person.erreichbtel, person.erreichbvon, person.erreichbbis, person.email, person.url, person.sysbranche, person.rechtsform, person.rechtsformcode, person.gruendung, person.hregister, person.hregisterflag, person.revflag, person.zusatz, person.namekont, person.vornamekont, person.fax, person.gebort, person.hregisterort, person.identeu, person.steuernr, person.identust, person.nomailingflag,person.privatflag, person.gesflag ";
                    infoData.resultTables = " CIC.PERSON PERSON,CIC.PTRELATE PTRELATE, CIC.PERSON PERSON2 ";
                    infoData.searchTables = " CIC.PERSON PERSON,CIC.PTRELATE PTRELATE, CIC.PERSON PERSON2 ";
                    infoData.searchConditions = "PERSON.SYSPERSON=PTRELATE.SYSPERSON2(+) and   ptrelate.sysperson1=person2.sysperson(+) ";
                    infoData.queryStruct.partialQuery = "SELECT * FROM (SELECT rownum rnum, a.* FROM( select  " + hints[typeof(Cic.One.DTO.PartnerDto)] + " {0} from {1} where {2} ) a WHERE rownum <= {3}) WHERE rnum > {4}";
                    infoData.optimized = false;

                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.BeteiligterDto)))
                {
                    infoData = new QueryInfoDataType1("CRMNM", "CRMNM.SYSCRMNM");
                    infoData.resultFields = "CRMNM.*, person.sysperson, matchcode, code,    syskdtyp,  anrede, anredecode, titel, titelCode, titel2,name, vorname, gebdatum, sysctlang, sysctlangkorr, syslandnat, einreisedatum, auslausweis, strasse, hsnr, plz, ort, sysland, sysstaat, wohnseit, strasse2, hsnr2, plz2, ort2, sysland2, sysstaat2, telefon, telefon2, handy, erreichbtel, erreichbvon, erreichbbis, email, url, sysbranche, rechtsform, rechtsformcode, gruendung, hregister, hregisterflag, revflag, zusatz, namekont, vornamekont,  fax, gebort, hregisterort, identeu, steuernr, identust, nomailingflag,privatflag, gesflag ";
                    infoData.resultTables = "CIC.CRMNM CRMNM, CIC.PERSON PERSON ";
                    infoData.searchTables = "CIC.CRMNM CRMNM, CIC.PERSON PERSON ";
                    infoData.searchConditions = "CRMNM.SYSIDCHILD = PERSON.SYSPERSON(+) ";
                    infoData.queryStruct.partialQuery = "SELECT * FROM (SELECT rownum rnum, a.* FROM( select  " + hints[typeof(Cic.One.DTO.AccountDto)] + " {0} from {1} where {2} ) a WHERE rownum <= {3}) WHERE rnum > {4}";
                    infoData.optimized = false;
                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.DdlkprubDto)))
                {
                    infoData = new QueryInfoDataType1("DDLKPRUB", "DDLKPRUB.SYSDDLKPRUB");
                    infoData.resultFields = "* ";
                    infoData.resultTables = "CIC.DDLKPRUB DDLKPRUB ";
                    infoData.searchTables = "CIC.DDLKPRUB DDLKPRUB ";
                    infoData.searchConditions = " 1=1 ";
                    infoData.optimized = true;
                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.DdlkpcolDto)))
                {
                    infoData = new QueryInfoDataType1("DDLKPCOL", "DDLKPCOL.SYSDDLKPCOL");
                    infoData.resultFields = "* ";
                    infoData.resultTables = "CIC.DDLKPCOL DDLKPCOL ";
                    infoData.searchTables = "CIC.DDLKPCOL DDLKPCOL ";
                    infoData.searchConditions = " 1=1 ";
                    infoData.optimized = true;
                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.EaihotDto)))
                {
                    infoData = new QueryInfoDataType1("EAIHOT", "EAIHOT.SYSEAIHOT");
                    infoData.resultFields = "* ";
                    infoData.resultTables = "CIC.EAIHOT EAIHOT ";
                    infoData.searchTables = "CIC.EAIHOT EAIHOT ";
                    infoData.searchConditions = " 1=1 ";
                    infoData.optimized = true;
                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.DdlkpposDto)))
                {
                    infoData = new QueryInfoDataType1("DDLKPPOS", "DDLKPPOS.SYSDDLKPPOS");
                    infoData.resultFields = "* ";
                    infoData.resultTables = "CIC.DDLKPPOS DDLKPPOS ";
                    infoData.searchTables = "CIC.DDLKPPOS DDLKPPOS ";
                    infoData.searchConditions = " 1=1 ";
                    infoData.optimized = true;
                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.DdlkpsposDto)))
                {
                    infoData = new QueryInfoDataType1("DDLKPSPOS", "DDLKPSPOS.SYSDDLKPSPOS");
                    infoData.resultFields = "* ";
                    infoData.resultTables = "CIC.DDLKPSPOS DDLKPSPOS ";
                    infoData.searchTables = "CIC.DDLKPSPOS DDLKPSPOS ";
                    infoData.searchConditions = " 1=1 ";
                    infoData.optimized = true;
                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.CamptpDto)))
                {
                    infoData = new QueryInfoDataType1("CAMPTP", "CAMPTP.SYSCAMPTP");
                    infoData.resultFields = "* ";
                    infoData.resultTables = "CIC.CAMPTP CAMPTP ";
                    infoData.searchTables = "CIC.CAMPTP CAMPTP ";
                    infoData.searchConditions = " 1=1 ";
                    infoData.optimized = true;
                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.OppotpDto)))
                {
                    infoData = new QueryInfoDataType1("OPPOTP", "OPPOTP.SYSOPPOTP");
                    infoData.resultFields = "* ";
                    infoData.resultTables = "CIC.OPPOTP OPPOTP ";
                    infoData.searchTables = "CIC.OPPOTP OPPOTP ";
                    infoData.searchConditions = " 1=1 ";
                    infoData.optimized = true;
                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.CrmprDto)))
                {
                    infoData = new QueryInfoDataType1("CRMPR", "CRMPR.SYSCRMPR");
                    infoData.resultFields = "* ";
                    infoData.resultTables = "CIC.CRMPR CRMPR ";
                    infoData.searchTables = "CIC.CRMPR CRMPR ";
                    infoData.searchConditions = " 1=1 ";
                    infoData.optimized = true;
                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.ContacttpDto)))
                {
                    infoData = new QueryInfoDataType1("CONTACTTP", "CONTACTTP.SYSCONTACTTP");
                    infoData.resultFields = "* ";
                    infoData.resultTables = "CIC.CONTACTTP CONTACTTP ";
                    infoData.searchTables = "CIC.CONTACTTP CONTACTTP ";
                    infoData.searchConditions = " CONTACTTP.ACTIVEFLAG=1 ";
                    infoData.optimized = true;
                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.ItemcatmDto)))
                {
                    infoData = new QueryInfoDataType1("ITEMCATM", "ITEMCATM.SYSITEMCATM");
                    infoData.resultFields = "ITEMCATM.*, ITEMCAT.NAME, ITEMCAT.DESCRIPTION ";
                    infoData.resultTables = "CIC.ITEMCATM ITEMCATM, CIC.ITEMCAT ITEMCAT ";
                    infoData.searchTables = "CIC.ITEMCATM ITEMCATM, CIC.ITEMCAT ITEMCAT  ";
                    infoData.searchConditions = " ITEMCATM.SYSITEMCAT = ITEMCAT.SYSITEMCAT ";
                    infoData.optimized = true;
                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.PtypeDto)))
                {
                    infoData = new QueryInfoDataType1("PTYPE", "PTYPE.SYSPTYPE");
                    infoData.resultFields = "* ";
                    infoData.resultTables = "CIC.PTYPE PTYPE ";
                    infoData.searchTables = "CIC.PTYPE PTYPE ";
                    infoData.searchConditions = " 1=1 ";
                    infoData.optimized = true;
                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.ActivitiesDto)))
                {
                    infoData = new QueryInfoDataType1("vc_activities", "ACTIVITY.SYSID");
                    infoData.resultFields = "* ";
                    infoData.resultTables = "CIC.vc_activities ACTIVITY ";
                    infoData.searchTables = "CIC.vc_activities ACTIVITY ";
                    infoData.searchConditions = " 1=1 ";
                    infoData.optimized = false;
                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.OppotaskDto)))
                {
                    infoData = new QueryInfoDataType1("OPPOTASK", "OPPOTASK.SYSOPPOTASK");
                    infoData.resultFields = "* ";
                    infoData.resultTables = "CIC.OPPOTASK OPPOTASK ";
                    infoData.searchTables = "CIC.OPPOTASK OPPOTASK ";
                    infoData.searchConditions = " 1=1 ";
                    infoData.optimized = false;
                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.PrunstepDto)))
                {
                    infoData = new QueryInfoDataType1("PRUNSTEP", "PRUNSTEP.SYSPRUNSTEP");
                    infoData.resultFields = "* ";
                    infoData.resultTables = "CIC.PRUNSTEP PRUNSTEP ";
                    infoData.searchTables = "CIC.PRUNSTEP PRUNSTEP ";
                    infoData.searchConditions = " 1=1 ";
                    infoData.optimized = true;
                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.PstepDto)))
                {
                    infoData = new QueryInfoDataType1("PSTEP", "PSTEP.SYSPSTEP");
                    infoData.resultFields = "* ";
                    infoData.resultTables = "CIC.PSTEP PSTEP ";
                    infoData.searchTables = "CIC.PSTEP PSTEP ";
                    infoData.searchConditions = " 1=1 ";
                    infoData.optimized = true;
                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.PrkgroupDto)))
                {
                    infoData = new QueryInfoDataType1("PRKGROUP", "PRKGROUP.SYSPRKGROUP");
                    //infoData.resultFields = "PRKGROUP.*, PRKGROUPM.SYSPERSON ";
                    //infoData.resultTables = "CIC.PRKGROUP PRKGROUP, CIC.PRKGROUPM PRKGROUPM ";
                    //infoData.searchTables = "CIC.PRKGROUP PRKGROUP, CIC.PRKGROUPM PRKGROUPM ";
                    //infoData.searchConditions = " PRKGROUP.SYSPRKGROUP = PRKGROUPM.SYSPRKGROUP(+)  ";
                    infoData.resultFields = "PRKGROUP.* ";
                    infoData.resultTables = "CIC.PRKGROUP PRKGROUP ";
                    infoData.searchTables = "CIC.PRKGROUP PRKGROUP ";
                    infoData.searchConditions = " 1=1  ";
                    infoData.optimized = false; //IMPORTANT: optimized can not work with n-m ! will result in double results because the wrong id is used in data-query
                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.PrkgroupmDto)))
                {
                    infoData = new QueryInfoDataType1("PRKGROUPM", "PRKGROUPM.SYSPRKGROUPM");
                    infoData.resultFields = " PRKGROUP.NAME, PRKGROUP.DESCRIPTION, PRKGROUPM.*, PERSON.NAME as KUNDENNAME, PERSON.VORNAME as KUNDENVORNAME";
                    infoData.resultTables = "CIC.PRKGROUPM PRKGROUPM, CIC.PRKGROUP PRKGROUP, CIC.PERSON PERSON";
                    infoData.searchTables = "CIC.PRKGROUPM PRKGROUPM, CIC.PRKGROUP PRKGROUP, CIC.PERSON PERSON";
                    infoData.searchConditions = " PRKGROUP.SYSPRKGROUP =  PRKGROUPM.SYSPRKGROUP(+) and PERSON.SYSPERSON = PRKGROUPM.SYSPERSON";
                    infoData.optimized = false; //IMPORTANT: optimized can not work with n-m !
                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.PrkgroupzDto)))
                {
                    infoData = new QueryInfoDataType1("PRKGROUPZ", "PRKGROUPZ.SYSPRKGROUPZ");
                    infoData.resultFields = "* ";
                    infoData.resultTables = "CIC.PRKGROUPZ PRKGROUPZ ";
                    infoData.searchTables = "CIC.PRKGROUPZ PRKGROUPZ ";
                    infoData.searchConditions = " 1=1 ";
                    infoData.optimized = true;
                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.PrkgroupsDto)))
                {
                    infoData = new QueryInfoDataType1("PRKGROUPS", "PRKGROUPS.SYSPRKGROUPS");
                    infoData.resultFields = "* ";
                    infoData.resultTables = "CIC.PRKGROUPS PRKGROUPS ";
                    infoData.searchTables = "CIC.PRKGROUPS PRKGROUPS ";
                    infoData.searchConditions = " 1=1 ";
                    infoData.optimized = true;
                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.SegDto)))
                {
                    infoData = new QueryInfoDataType1("SEG", "SEG.SYSSEG");
                    infoData.resultFields = "* ";
                    infoData.resultTables = "CIC.SEG SEG ";
                    infoData.searchTables = "CIC.SEG SEG ";
                    infoData.searchConditions = " 1=1 ";
                    infoData.optimized = true;
                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.SegcDto)))
                {
                    infoData = new QueryInfoDataType1("SEGC", "SEGC.SYSSEGC");
                    infoData.resultFields = "* ";
                    infoData.resultTables = "CIC.SEGC SEGC ";
                    infoData.searchTables = "CIC.SEGC SEGC ";
                    infoData.searchConditions = " 1=1 ";

                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.StickynoteDto)))
                {
                    //This Query to Merge with Notiz
                    /*infoData = new QueryInfoDataType1("STICKYNOTE", "SYSSTICKYNOTE");
                    infoData.resultFields = "* ";
                    string from = @"(
                      SELECT SYSNOTIZ,NOTIZ,GEBIET,SYSGEBIET,SYSPERSON,SYSVT,SYSWFUSER,DATUM,ZEIT,
                      (SYSNOTIZ*-1) AS SYSSTICKYNOTE, NOTIZ AS INHALT, GEBIET AS AREA, SYSGEBIET AS SYSID, 'Notiz' BEZEICHNUNG, TO_NUMBER('') STICKYFLAG, TO_NUMBER('') PRIVATFLAG, '' CODESTICKYTYPE, TO_DATE('') SYSCRTDATE, TO_NUMBER('') SYSCRTTIME, TO_NUMBER('') SYSCRTUSER, TO_DATE('') SYSCHGDATE, TO_NUMBER('') SYSCHGTIME, TO_NUMBER('') SYSCHGUSER, 1 AS SHOWFLAG, TO_NUMBER('') ACTIVEFLAG, (case when SYSGEBIET < 0 then 1 else 0 end) DELETEFLAG
                      FROM NOTIZ
                      UNION ALL
                      SELECT (SYSSTICKYNOTE*-1) AS SYSNOTIZ,INHALT AS NOTIZ,AREA AS GEBIET,SYSID AS SYSGEBIET,TO_NUMBER('') SYSPERSON,TO_NUMBER('') SYSVT,TO_NUMBER('') SYSWFUSER,TO_DATE('') DATUM,TO_NUMBER('') ZEIT,
                      SYSSTICKYNOTE,INHALT,AREA,SYSID,BEZEICHNUNG,STICKYFLAG,PRIVATFLAG,CODESTICKYTYPE,SYSCRTDATE,SYSCRTTIME,SYSCRTUSER,SYSCHGDATE,SYSCHGTIME,SYSCHGUSER,SHOWFLAG,ACTIVEFLAG,DELETEFLAG
                      FROM STICKYNOTE
                    ) ";
                    infoData.resultTables = from;
                    infoData.searchTables = from;
                    */
                    infoData = new QueryInfoDataType1("STICKYNOTE", "STICKYNOTE.SYSSTICKYNOTE");
                    infoData.resultFields = "CIC.STICKYNOTE.*, WFUSER.NAME wfuserName ";
                    infoData.resultTables = "CIC.STICKYNOTE STICKYNOTE, CIC.WFUSER WFUSER ";
                    infoData.searchTables = "CIC.STICKYNOTE STICKYNOTE, CIC.WFUSER WFUSER ";

                    infoData.searchConditions = " stickynote.SYSCRTUSER = WFUSER.SYSWFUSER (+) ";
                    infoData.optimized = true;
                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.NotizDto)))
                {
                    //This Query to Merge with Notiz
                    /*infoData = new QueryInfoDataType1("STICKYNOTE", "SYSSTICKYNOTE");
                    infoData.resultFields = "* ";
                    string from = @"(
                      SELECT SYSNOTIZ,NOTIZ,GEBIET,SYSGEBIET,SYSPERSON,SYSVT,SYSWFUSER,DATUM,ZEIT,
                      (SYSNOTIZ*-1) AS SYSSTICKYNOTE, NOTIZ AS INHALT, GEBIET AS AREA, SYSGEBIET AS SYSID, 'Notiz' BEZEICHNUNG, TO_NUMBER('') STICKYFLAG, TO_NUMBER('') PRIVATFLAG, '' CODESTICKYTYPE, TO_DATE('') SYSCRTDATE, TO_NUMBER('') SYSCRTTIME, TO_NUMBER('') SYSCRTUSER, TO_DATE('') SYSCHGDATE, TO_NUMBER('') SYSCHGTIME, TO_NUMBER('') SYSCHGUSER, 1 AS SHOWFLAG, TO_NUMBER('') ACTIVEFLAG, (case when SYSGEBIET < 0 then 1 else 0 end) DELETEFLAG
                      FROM NOTIZ
                      UNION ALL
                      SELECT (SYSSTICKYNOTE*-1) AS SYSNOTIZ,INHALT AS NOTIZ,AREA AS GEBIET,SYSID AS SYSGEBIET,TO_NUMBER('') SYSPERSON,TO_NUMBER('') SYSVT,TO_NUMBER('') SYSWFUSER,TO_DATE('') DATUM,TO_NUMBER('') ZEIT,
                      SYSSTICKYNOTE,INHALT,AREA,SYSID,BEZEICHNUNG,STICKYFLAG,PRIVATFLAG,CODESTICKYTYPE,SYSCRTDATE,SYSCRTTIME,SYSCRTUSER,SYSCHGDATE,SYSCHGTIME,SYSCHGUSER,SHOWFLAG,ACTIVEFLAG,DELETEFLAG
                      FROM STICKYNOTE
                    ) ";
                    infoData.resultTables = from;
                    infoData.searchTables = from;
                    */
                    // select person.sysperson, person.name, notiz.* from notiz, person where notiz.sysperson = person.sysperson
                    // select wfuser.name as PERSONNAME, notiz.* from notiz, wfuser where notiz.syswfuser = wfuser.syswfuser
                    infoData = new QueryInfoDataType1("NOTIZ", "NOTIZ.SYSNOTIZ");
                    infoData.resultFields = "wfuser.name as PERSONNAME, notiz.* ";
                    infoData.resultTables = "CIC.NOTIZ NOTIZ, CIC.WFUSER WFUSER ";
                    infoData.searchTables = "CIC.NOTIZ NOTIZ, CIC.WFUSER WFUSER ";
                    infoData.searchConditions = " notiz.syswfuser = wfuser.syswfuser ";
                    infoData.optimized = true;
                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.StickytypeDto)))
                {
                    infoData = new QueryInfoDataType1("STICKYTYPE", "STICKYTYPE.SYSSTICKYTYPE");
                    infoData.resultFields = "* ";
                    infoData.resultTables = "CIC.STICKYTYPE STICKYTYPE ";
                    infoData.searchTables = "CIC.STICKYTYPE STICKYTYPE ";
                    infoData.searchConditions = " 1=1 ";
                    infoData.optimized = true;
                }
                else if (typeof(T).Equals(typeof(Cic.OpenOne.Common.DTO.RegVarDto)))
                {
                    infoData = new QueryInfoDataType1("REGVAR", "REGVAR.SYSREGVAR");
                    infoData.resultFields = "regvar.* ";
                    infoData.resultTables = "cic.regsec, cic.regvar ";
                    infoData.searchTables = "cic.regsec, cic.regvar ";
                    infoData.searchConditions = " regsec.sysregsec=regvar.sysregsec ";
                    infoData.optimized = false;
                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.WfsignatureDto)))
                {
                    infoData = new QueryInfoDataType1("WFSIGNATURE", "WFSIGNATURE.SYSWFSIGNATURE");
                    infoData.resultFields = "* ";
                    infoData.resultTables = "CIC.WFSIGNATURE WFSIGNATURE ";
                    infoData.searchTables = "CIC.WFSIGNATURE WFSIGNATURE ";
                    infoData.searchConditions = " 1=1 ";
                    infoData.optimized = true;
                }

                else if (typeof(T).Equals(typeof(Cic.One.DTO.AngvarDto)))
                {
                    infoData = new QueryInfoDataType1("ANGVAR", "ANGVAR.SYSANGVAR");
                    infoData.resultFields = "ANGVAR.SYSANGVAR, ANGVAR.SYSANGEBOT, ANGVAR.RANG, ANGVAR.BEZEICHNUNG, ANGVAR.BESCHREIBUNG ";
                    infoData.resultTables = "CIC.ANGVAR ANGVAR, CIC.ANGEBOT ";
                    infoData.searchTables = "CIC.ANGVAR ANGVAR, CIC.ANGEBOT ";
                    infoData.searchConditions = " ANGVAR.SYSANGEBOT = ANGEBOT.SYSID ";
                    infoData.optimized = true;
                }

                else if (typeof(T).Equals(typeof(Cic.One.DTO.AngobDto)))
                {
                    infoData = new QueryInfoDataType1("ANGOB", "ANGOB.SYSOB");
                    infoData.resultFields = "ANGOB.SYSOB, ANGOB.SYSVT, ANGOB.RANG, ANGOB.OBJEKT, ANGOB.BEZEICHNUNG, ANGOB.OBJEKTVT, ANGVAR.BEZEICHNUNG variante";
                    infoData.resultTables = "CIC.ANGOB ANGOB, CIC.ANGEBOT ANGEBOT, CIC.ANGVAR ANGVAR ";
                    infoData.searchTables = "CIC.ANGOB ANGOB,CIC.ANGEBOT ANGEBOT, CIC.ANGVAR ANGVAR ";
                    infoData.searchConditions = " ANGVAR.SYSANGEBOT = ANGEBOT.SYSID AND ANGOB.SYSVT = ANGVAR.SYSANGVAR ";

                    infoData.optimized = true;
                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.AngkalkDto)))
                {
                    infoData = new QueryInfoDataType1("ANGKALK", "ANGKALK.SYSKALK");
                    infoData.resultFields = "* ";
                    infoData.resultTables = "CIC.ANGKALK ANGKALK ";
                    infoData.searchTables = "CIC.ANGKALK ANGKALK ";
                    infoData.searchConditions = " 1=1 ";

                    infoData.optimized = true;
                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.AngobslDto)))
                {
                    infoData = new QueryInfoDataType1("ANGOBSL", "ANGOBSL.SYSID");
                    infoData.resultFields = "ANGOBSL.SYSID, ANGOBSL.SYSVT, ANGOBSL.RANG, ANGOBSL.BEZEICHNUNG, ANGVAR.BEZEICHNUNG variante  ";
                    infoData.resultTables = "CIC.ANGOBSL ANGOBSL, CIC.ANGEBOT ANGEBOT, CIC.ANGVAR ANGVAR ";
                    infoData.searchTables = "CIC.ANGOBSL ANGOBSL, CIC.ANGEBOT ANGEBOT, CIC.ANGVAR ANGVAR ";
                    infoData.searchConditions = " ANGVAR.SYSANGEBOT = ANGEBOT.SYSID AND ANGOBSL.SYSVT = ANGVAR.SYSANGVAR  ";
                    infoData.optimized = true;
                }

                else if (typeof(T).Equals(typeof(Cic.One.DTO.AngobslposDto)))
                {
                    infoData = new QueryInfoDataType1("ANGOBSLPOS", "ANGOBSLPOS.SYSID");
                    infoData.resultFields = "ANGOBSLPOS.SYSID, ANGOBSLPOS.SYSVTOBSL, ANGOBSLPOS.RANG, ANGOBSLPOS.ANZAHL, ANGOBSLPOS.BETRAG, ANGOBSL.BEZEICHNUNG staffel ";
                    infoData.resultTables = "CIC.ANGOBSLPOS ANGOBSLPOS, CIC.ANGEBOT ANGEBOT, CIC.ANGVAR ANGVAR, CIC.ANGOBSL ANGOBSL ";
                    infoData.searchTables = "CIC.ANGOBSLPOS ANGOBSLPOS, CIC.ANGEBOT ANGEBOT, CIC.ANGVAR ANGVAR, CIC.ANGOBSL ANGOBSL ";
                    infoData.searchConditions = " ANGVAR.SYSANGEBOT = ANGEBOT.SYSID AND ANGOBSL.SYSVT = ANGVAR.SYSANGVAR AND ANGOBSLPOS.SYSVTOBSL = ANGOBSL.SYSID ";
                    infoData.optimized = true;
                }


                else if (typeof(T).Equals(typeof(Cic.One.DTO.AntobDto)))
                {
                    infoData = new QueryInfoDataType1("ANTOB", "ANTOB.SYSOB");
                    infoData.resultFields = "ANTOB.SYSOB, ANTOB.SYSVT, ANTOB.RANG, ANTOB.OBJEKT, ANTOB.BEZEICHNUNG, ANTOB.OBJEKTVT, ANTRAG.ANTRAG antrag ";
                    infoData.resultTables = "CIC.ANTOB ANTOB, CIC.ANTRAG ANTRAG ";
                    infoData.searchTables = "CIC.ANTOB ANTOB, CIC.ANTRAG ANTRAG ";
                    infoData.searchConditions = " ANTOB.SYSVT = ANTRAG.SYSID ";
                    infoData.optimized = true;
                }




                else if (typeof(T).Equals(typeof(Cic.One.DTO.AngebotDto)))
                {
                    infoData = new QueryInfoDataType1("ANGEBOT", "ANGEBOT.SYSID");
                    infoData.resultFields = "ANGEBOT.*, KD.NAME||IT.NAME|| ', ' ||KD.VORNAME||IT.VORNAME kundeName, IT.NAME|| ', ' ||IT.VORNAME interessentName, LSADD.BEZEICHNUNG mandantName, RAHMEN.BESCHREIBUNG rahmenName ";
                    infoData.resultTables = "CIC.ANGEBOT ANGEBOT, CIC.PERSON KD, CIC.IT IT, CIC.LSADD LSADD, CIC.RVT RAHMEN ";
                    infoData.searchTables = "CIC.ANGEBOT ANGEBOT, CIC.PERSON KD, CIC.IT IT, CIC.LSADD LSADD, CIC.RVT RAHMEN ";
                    infoData.searchConditions = " ANGEBOT.SYSKD=KD.SYSPERSON(+) and ANGEBOT.SYSIT=IT.SYSIT(+) and ANGEBOT.SYSLS=LSADD.SYSLSADD(+) and ANGEBOT.SYSRVT=RAHMEN.SYSRVT(+)";
                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.VertragDto)))
                {
                    infoData = new QueryInfoDataType1("VT", "VT.SYSID");
                    infoData.resultFields = "VT.*, KD.NAME|| ', ' ||KD.VORNAME kundeName";
                    infoData.resultTables = "CIC.VT VT, CIC.PERSON KD, CIC.OB OB, CIC.KALK KALK, CIC.OBINI OBINI ";
                    infoData.searchTables = "CIC.VT VT, CIC.PERSON KD, CIC.OB OB, CIC.KALK KALK, CIC.OBINI OBINI ";
                    infoData.searchConditions = " VT.SYSKD=KD.SYSPERSON(+) and VT.SYSID=OB.SYSVT and OB.SYSOB=KALK.SYSOB and OBINI.SYSOBINI=OB.SYSOB ";
                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.RecalcDto)))
                {
                    infoData = new QueryInfoDataType1("RECALC", "RECALC.SYSRECALC");
                    infoData.resultFields = "RECALC.* ";
                    infoData.resultTables = "CIC.RECALC RECALC, CIC.VT VT, CIC.PERSON PERSON ";
                    infoData.searchTables = "CIC.RECALC RECALC, CIC.VT VT, CIC.PERSON PERSON ";
                    infoData.searchConditions = " recalc.sysvt = vt.sysid(+) and vt.syskd = person.sysperson(+) ";
                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.AntobslDto)))
                {
                    infoData = new QueryInfoDataType1("ANTOBSL", "ANTOBSL.SYSID");
                    infoData.resultFields = "ANTOBSL.SYSID, ANTOBSL.SYSVT, ANTOBSL.RANG, ANTOBSL.BEZEICHNUNG, ANTOBSL.FAELLIG, ANTRAG.ANTRAG antrag ";
                    infoData.resultTables = "CIC.ANTOBSL ANTOBSL, CIC.ANTRAG ANTRAG ";
                    infoData.searchTables = "CIC.ANTOBSL ANTOBSL, CIC.ANTRAG ANTRAG ";
                    infoData.searchConditions = " ANTOBSL.SYSVT = ANTRAG.SYSID ";
                    infoData.optimized = true;
                }

                else if (typeof(T).Equals(typeof(Cic.One.DTO.AntobslposDto)))
                {
                    infoData = new QueryInfoDataType1("ANTOBSLPOS", "ANTOBSLPOS.SYSID");
                    infoData.resultFields = "ANTOBSLPOS.SYSID, ANTOBSLPOS.SYSVTOBSL, ANTOBSLPOS.RANG, ANTOBSLPOS.ANZAHL, ANTOBSLPOS.BETRAG, ANTOBSL.BEZEICHNUNG staffel";
                    infoData.resultTables = "CIC.ANTOBSLPOS ANTOBSLPOS, CIC.ANTOBSL ANTOBSL, CIC.ANTRAG ANTRAG ";
                    infoData.searchTables = "CIC.ANTOBSLPOS ANTOBSLPOS, CIC.ANTOBSL ANTOBSL, CIC.ANTRAG ANTRAG ";
                    infoData.searchConditions = " ANTOBSL.SYSVT = ANTRAG.SYSID AND ANTOBSLPOS.SYSVTOBSL = ANTOBSL.SYSID ";
                    infoData.optimized = true;
                }

                else if (typeof(T).Equals(typeof(Cic.One.DTO.ObDto)))
                {
                    infoData = new QueryInfoDataType1("OB", "OB.SYSOB");
                    infoData.resultFields = "OB.SYSOB, OB.SYSVT, OB.RANG, OB.OBJEKT, OB.BEZEICHNUNG, OB.OBJEKTVT, VT.VERTRAG vertrag ";
                    infoData.resultTables = "CIC.OB OB, CIC.VT VT ";
                    infoData.searchTables = "CIC.OB OB, CIC.VT VT ";
                    infoData.searchConditions = " OB.SYSVT = VT.SYSID ";
                    infoData.optimized = true;
                }

                else if (typeof(T).Equals(typeof(Cic.One.DTO.VtobslDto)))
                {
                    infoData = new QueryInfoDataType1("VTOBSL", "VTOBSL.SYSID");
                    infoData.resultFields = "VTOBSL.SYSID, VTOBSL.SYSVT, VTOBSL.RANG, VTOBSL.BEZEICHNUNG, VTOBSL.FAELLIG, VT.VERTRAG vertrag";
                    infoData.resultTables = "CIC.VTOBSL VTOBSL, CIC.VT VT ";
                    infoData.searchTables = "CIC.VTOBSL VTOBSL, CIC.VT VT ";
                    infoData.searchConditions = " VTOBSL.SYSVT = VT.SYSID ";
                    infoData.optimized = true;
                }

                else if (typeof(T).Equals(typeof(Cic.One.DTO.VtobslposDto)))
                {
                    infoData = new QueryInfoDataType1("VTOBSLPOS", "VTOBSLPOS.SYSID");
                    infoData.resultFields = "VTOBSLPOS.SYSID, VTOBSLPOS.SYSVTOBSL, VTOBSLPOS.RANG, VTOBSLPOS.ANZAHL, VTOBSLPOS.BETRAG, VTOBSL.RANG staffelrang, VTOBSL.BEZEICHNUNG staffel ";
                    infoData.resultTables = "CIC.VTOBSLPOS VTOBSLPOS, CIC.VTOBSL VTOBSL, CIC.VT VT ";
                    infoData.searchTables = "CIC.VTOBSLPOS VTOBSLPOS, CIC.VTOBSL VTOBSL, CIC.VT VT ";
                    infoData.searchConditions = " VTOBSL.SYSVT = VT.SYSID AND VTOBSLPOS.SYSVTOBSL = VTOBSL.SYSID ";

                    infoData.optimized = true;
                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.ExpvalDto)))
                {
                    infoData = new QueryInfoDataType1("EXPVAL", "EXPVAL.SYSEXPVAL");
                    infoData.resultFields = "* ";
                    infoData.resultTables = "CIC.EXPVAL EXPVAL ";
                    infoData.searchTables = "CIC.EXPVAL EXPVAL ";
                    infoData.searchConditions = " 1=1 ";

                    infoData.optimized = true;
                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.ExptypDto)))
                {
                    infoData = new QueryInfoDataType1("EXPTYP", "EXPTYP.SYSEXPTYP");
                    infoData.resultFields = "* ";
                    infoData.resultTables = "CIC.EXPTYP EXPTYP ";
                    infoData.searchTables = "CIC.EXPTYP EXPTYP ";
                    infoData.searchConditions = " 1=1 ";

                    infoData.optimized = true;
                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.NkontoDto)))
                {
                    infoData = new QueryInfoDataType1("NKONTO", "NKONTO.SYSNKONTO");
                    infoData.resultFields = "* ";
                    infoData.resultTables = "cic.NKONTO NKONTO ";
                    infoData.searchTables = "cic.NKONTO NKONTO ";
                    infoData.searchConditions = " 1=1 ";
                    infoData.optimized = false;

                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.PrintsetDto)))
                {
                    infoData = new QueryInfoDataType1("PRINTSET", "PRINTSET.SYSPRINTSET");
                    infoData.resultFields = "* ";
                    infoData.resultTables = "cic.PRINTSET PRINTSET ";
                    infoData.searchTables = "cic.PRINTSET PRINTSET ";
                    infoData.searchConditions = " 1=1 ";
                    infoData.optimized = false;

                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.PrtlgsetDto)))
                {
                    infoData = new QueryInfoDataType1("PRTLGSET", "PRTLGSET.SYSPRTLGSET");
                    infoData.resultFields = "* ";
                    infoData.resultTables = "cic.PRTLGSET PRTLGSET ";
                    infoData.searchTables = "cic.PRTLGSET PRTLGSET ";
                    infoData.searchConditions = " 1=1 ";
                    infoData.optimized = false;

                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.ObkatDto)))
                {
                    infoData = new QueryInfoDataType1("OBKAT", "OBKAT.SYSOBKAT");
                    infoData.resultFields = "* ";
                    infoData.resultTables = "cic.OBKAT OBKAT ";
                    infoData.searchTables = "cic.OBKAT OBKAT ";
                    infoData.searchConditions = " 1=1 ";
                    infoData.optimized = false;

                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.VartDto)))
                {
                    infoData = new QueryInfoDataType1("VART", "VART.SYSVART");
                    infoData.resultFields = "* ";
                    infoData.resultTables = "cic.VART VART ";
                    infoData.searchTables = "cic.VARTT VART ";
                    infoData.searchConditions = " 1=1 ";
                    infoData.optimized = false;

                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.PersonDto)))
                {
                    infoData = new QueryInfoDataType1("PERSON", "PERSON.SYSPERSON");
                    infoData.resultFields = "* ";
                    infoData.resultTables = "CIC.PERSON PERSON ";
                    infoData.searchTables = "CIC.PERSON PERSON ";

                    infoData.searchConditions = " 1=1 ";
  
                    infoData.optimized = false;

                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.KalkDto)))
                {
                    infoData = new QueryInfoDataType1("KALK", "KALK.SYSKALK");
                    infoData.resultFields = "* ";
                    infoData.resultTables = "CIC.KALK KALK ";
                    infoData.searchTables = "CIC.KALK KALK ";

                    infoData.searchConditions = " 1=1 ";
  
                    infoData.optimized = false;

                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.FahrzeugbriefDto)))
                {
                    infoData = new QueryInfoDataType1("OBBRIEF", "OBBRIEF.SYSOBBRIEF");
                    infoData.resultFields = "* ";
                    infoData.resultTables = "CIC.OBBRIEF OBBRIEF ";
                    infoData.searchTables = "CIC.OBBRIEF OBBRIEF ";

                    infoData.searchConditions = " 1=1 ";
  
                    infoData.optimized = false;

                }
     
                else if (typeof(T).Equals(typeof(Cic.One.DTO.AngobbriefDto)))
                {
                    infoData = new QueryInfoDataType1("ANGOBBRIEF", "ANGOBBRIEF.SYSANGOBBRIEF");
                    infoData.resultFields = "* ";
                    infoData.resultTables = "CIC.ANGOBBRIEF ANGOBBRIEF ";
                    infoData.searchTables = "CIC.ANGOBBRIEF ANGOBBRIEF ";

                    infoData.searchConditions = " 1=1 ";
  
                    infoData.optimized = false;

                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.RechnungDto)))
                {
                    infoData = new QueryInfoDataType1("RN", "RN.SYSID");
                    infoData.resultFields = "* ";
                    infoData.resultTables = "CIC.RN RN ";
                    infoData.searchTables = "CIC.RN RN ";

                    infoData.searchConditions = " 1=1 ";
  
                    infoData.optimized = false;

                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.BrandDto)))
                {
                    infoData = new QueryInfoDataType1("BRAND", "BRAND.SYSBRAND");
                    infoData.resultFields = "* ";
                    infoData.resultTables = "CIC.BRAND BRAND ";
                    infoData.searchTables = "CIC.BRAND BRAND ";

                    infoData.searchConditions = " 1=1 ";
  
                    infoData.optimized = false;

                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.VertriebskanalDto)))
                {
                    infoData = new QueryInfoDataType1("BCHANNEL", "BCHANNEL.SYSBCHANNEL");
                    infoData.resultFields = "* ";
                    infoData.resultTables = "CIC.BCHANNEL BCHANNEL ";
                    infoData.searchTables = "CIC.BCHANNEL BCHANNEL ";

                    infoData.searchConditions = " 1=1 ";
  
                    infoData.optimized = false;

                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.HandelsgruppeDto)))
                {
                    infoData = new QueryInfoDataType1("PRHGROUP", "PRHGROUP.SYSPRHGROUP");
                    infoData.resultFields = "* ";
                    infoData.resultTables = "CIC.PRHGROUP PRHGROUP ";
                    infoData.searchTables = "CIC.PRHGROUP PRHGROUP ";

                    infoData.searchConditions = " 1=1 ";
  
                    infoData.optimized = false;

                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.RollentypDto)))
                {
                    infoData = new QueryInfoDataType1("ROLETYPE", "ROLETYPE.SYSROLETYPE");
                    infoData.resultFields = "* ";
                    infoData.resultTables = "CIC.ROLETYPE ROLETYPE ";
                    infoData.searchTables = "CIC.ROLETYPE ROLETYPE ";

                    infoData.searchConditions = " 1=1 ";
  
                    infoData.optimized = false;

                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.RolleDto)))
                {
                    infoData = new QueryInfoDataType1("PEROLE", "PEROLE.SYSPEROLE");
                    infoData.resultFields = "* ";
                    infoData.resultTables = "CIC.PEROLE PEROLE ";
                    infoData.searchTables = "CIC.PEROLE PEROLE ";

                    infoData.searchConditions = " 1=1 ";
  
                    infoData.optimized = false;

                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.StaffeltypDto)))
                {
                    infoData = new QueryInfoDataType1("SLTYP", "SLTYP.SYSSLTYP");
                    infoData.resultFields = "* ";
                    infoData.resultTables = "CIC.SLTYP SLTYP ";
                    infoData.searchTables = "CIC.SLTYP SLTYP ";

                    infoData.searchConditions = " 1=1 ";
  
                    infoData.optimized = false;

                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.StaffelpositionstypDto)))
                {
                    infoData = new QueryInfoDataType1("SLPOSTYP", "SLPOSTYP.SYSSLPOSTYP");
                    infoData.resultFields = "* ";
                    infoData.resultTables = "CIC.SLPOSTYP SLPOSTYP ";
                    infoData.searchTables = "CIC.SLPOSTYP SLPOSTYP ";

                    infoData.searchConditions = " 1=1 ";
  
                    infoData.optimized = false;

                
                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.BesuchsberichtDto)))
                {
                    infoData = new QueryInfoDataType1("FILEATT", "FILEATT.SYSFILEATT");
                    infoData.resultFields = "CONTENT,sysfileatt,area,sysid ";
                    infoData.resultTables = "CIC.FILEATT FILEATT ";
                    infoData.searchTables = "CIC.FILEATT FILEATT ";

                    infoData.searchConditions = " 1=1 ";

                    infoData.optimized = false;

                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.InboxDto)))
                {
                    infoData = new QueryInfoDataType1("vc_bplistener", "bplistener.SYSID");
                    infoData.resultFields = "bplistener.*,r.duedate faellig, bplistener.sysvm syshaendler";
                    infoData.resultTables = "vc_bplistener bplistener, (select reminder.* from reminder where ((reminder.syswfuser = {WFUSER}) OR (reminder.syswfuser IS NULL AND CIC.MDBS_TOORATIME(reminder.starttime, reminder.startdate) < SYSDATE AND NVL(reminder.sysbprole, 0) IN (SELECT sysbprole FROM bprole bp1, bproleusr me WHERE me.syswfuser = {WFUSER} AND me.onlyproxyflag = 0 AND me.activeflag = 1 AND me.namebprole = bp1.namebprole)) OR (reminder.syswfuser IS NOT NULL AND CIC.MDBS_TOORATIME(reminder.duetime, reminder.duedate) < SYSDATE AND reminder.privateflag = 0 AND NVL(reminder.sysbprole, 0) IN (SELECT sysbprole FROM bprole bp1, bproleusr me WHERE me.syswfuser = {WFUSER} AND me.onlyproxyflag = 0 AND me.activeflag = 1 AND me.namebprole = bp1.namebprole)) OR EXISTS (SELECT 1 FROM table(CIC.MDBS_GetTabProxyRole({WFUSER})) proxy, bproleproxy, bproleusr WHERE bproleproxy.sysbproleproxy = proxy.sysbproleproxy and bproleproxy.sysbproleusr = bproleusr.sysbproleusr AND proxy.sysbprole = reminder.sysbprole AND bproleusr.onlyproxyflag = 0 AND proxy.syswfuser = reminder.syswfuser) OR EXISTS (SELECT 1 FROM table(CIC.MDBS_GetTabProxyRole({WFUSER})) proxy, bproleproxy, bproleusr WHERE bproleproxy.sysbproleproxy = proxy.sysbproleproxy and bproleproxy.sysbproleusr = bproleusr.sysbproleusr AND proxy.sysbprole = reminder.sysbprole AND bproleusr.onlyproxyflag = 0 AND reminder.syswfuser IS NULL))) r";
                    infoData.searchTables = "vc_bplistener bplistener, (select reminder.* from reminder where ((reminder.syswfuser = {WFUSER}) OR (reminder.syswfuser IS NULL AND CIC.MDBS_TOORATIME(reminder.starttime, reminder.startdate) < SYSDATE AND NVL(reminder.sysbprole, 0) IN (SELECT sysbprole FROM bprole bp1, bproleusr me WHERE me.syswfuser = {WFUSER} AND me.onlyproxyflag = 0 AND me.activeflag = 1 AND me.namebprole = bp1.namebprole)) OR (reminder.syswfuser IS NOT NULL AND CIC.MDBS_TOORATIME(reminder.duetime, reminder.duedate) < SYSDATE AND reminder.privateflag = 0 AND NVL(reminder.sysbprole, 0) IN (SELECT sysbprole FROM bprole bp1, bproleusr me WHERE me.syswfuser = {WFUSER} AND me.onlyproxyflag = 0 AND me.activeflag = 1 AND me.namebprole = bp1.namebprole)) OR EXISTS (SELECT 1 FROM table(CIC.MDBS_GetTabProxyRole({WFUSER})) proxy, bproleproxy, bproleusr WHERE bproleproxy.sysbproleproxy = proxy.sysbproleproxy and bproleproxy.sysbproleusr = bproleusr.sysbproleusr AND proxy.sysbprole = reminder.sysbprole AND bproleusr.onlyproxyflag = 0 AND proxy.syswfuser = reminder.syswfuser) OR EXISTS (SELECT 1 FROM table(CIC.MDBS_GetTabProxyRole({WFUSER})) proxy, bproleproxy, bproleusr WHERE bproleproxy.sysbproleproxy = proxy.sysbproleproxy and bproleproxy.sysbproleusr = bproleusr.sysbproleusr AND proxy.sysbprole = reminder.sysbprole AND bproleusr.onlyproxyflag = 0 AND reminder.syswfuser IS NULL))) r";

                    infoData.searchConditions = " bplistener.oltable=r.area(+) and bplistener.sysid=r.sysid(+)  ";

                    infoData.optimized = false;

                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.ProcessDto)))
                {
                    infoData = new QueryInfoDataType1("bplistener", "bplistener.sysbplistener");
                    infoData.resultFields = "bplistener.*, wfuser.name benutzer, bplistener.eventcode process, bplistener.namebplane casestep, bprole.NAMEBPROLE namebprole";
                    infoData.resultTables = "bplistener, bpprocinst, bpprocdef, bpmnode,wfuser,bprole ";
                    infoData.searchTables = "bplistener, bpprocinst, bpprocdef, bpmnode,wfuser,bprole ";

                    infoData.searchConditions = " bplistener.sysbprole=bprole.sysbprole(+) and bplistener.sysbpprocinst = bpprocinst.sysbpprocinst AND bplistener.sysbpprocdef = bpprocdef.sysbpprocdef and bplistener.syswfuser=wfuser.syswfuser AND bplistener.sysbpmnode = bpmnode.sysbpmnode(+) AND bplistener.isusertask = 1 AND NVL(bplistener.execstatus, 0) = 0  ";

                    infoData.optimized = false;

                }
                else if (typeof(T).Equals(typeof(Cic.One.DTO.ZekDto)))
                {
                    infoData = new QueryInfoDataType1("ZEK", "ZEK.SYSZEK");
                    infoData.resultFields = "* ";
                    infoData.resultTables = "CIC.ZEK ZEK ";
                    infoData.searchTables = "CIC.ZEK ZEK ";
                    infoData.searchConditions = " 1=1 ";
                }
                else
                    throw new Exception("Type not supported for Search: " + typeof(T));


                queryInfoCache[mt] = infoData;
            }


            return queryInfoCache[mt];
        }

        public QueryInfoData getQueryInfo(String entityTable, String entityField)
        {
            return new QueryInfoDataType1(entityTable, entityField);
        }
    }
}
