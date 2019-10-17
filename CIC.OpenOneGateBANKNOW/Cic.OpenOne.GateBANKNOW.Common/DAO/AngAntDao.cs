using AutoMapper;
using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.DAO.Prisma;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Model.DdOd;
using Cic.OpenOne.Common.Model.DdOiqueue;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Model.DdOw;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.Util;
using Cic.OpenOne.Common.Util.Config;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.GateBANKNOW.Common.BO;
using Cic.OpenOne.GateBANKNOW.Common.DAO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Auskunft;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Enums;
using Cic.P000001.Common;
using CIC.Database.OD.EF6.Model;
using CIC.Database.OL.EF6.Model;
using CIC.Database.OW.EF6.Model;
using Dapper;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Globalization;
using System.Linq;
using System.Reflection;


namespace Cic.OpenOne.GateBANKNOW.Common.DAO
{
    /// <summary>
    /// Type of Zusatzaustatung
    /// </summary>
    public enum ZusatzaustatungType
    {
        /// <summary>
        /// Serienausstattung
        /// </summary> 
        Serienausstattung = 10,

        /// <summary>
        /// Sonderausstattung
        /// </summary>
        Sonderausstattung = 20,

        /// <summary>
        /// Herstellerdaten 
        /// </summary>
        Herstellerdaten = 30,

        /// <summary>
        /// Zusatzausstattung
        /// </summary>
        Zusatzausstattung = 40
    }
    class Zustaende
    {
        public string origexternzustand { get; set; }
        public string replexternzustand { get; set; }
        public long sysid { get; set; }
    }

    /// <summary>
    /// Angebot Datenklasse
    /// </summary>
    class AngebotData
    {
        /// <summary>
        /// Produkt ID
        /// </summary>
        public long sysprproduct { get; set; }
        /// <summary>
        /// Objektnutzungstyp
        /// </summary>
        public long SYSOBUSETYPE { get; set; }
    }

    
    class RatingAuflagen
    {
        public string auflageText { get; set; }
        public long rank { get; set; }
        public long antragsteller { get; set; }
    }

    /// <summary>
    /// Offer/Application Data Access Object
    /// </summary>
    public class AngAntDao : IAngAntDao
    {
        /// <summary>
        /// Logger
        /// </summary>
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public const String RW_VERL_VERFUEGBAR_WEB_DEFAULT = "select 1 from dual where vt.attribut !='saldiert' and vt.rw>7000 and vt.rlz<6 and vt.sysvart=1";

        public const String CODE_EXTIDENT_ANTRAGSID_B2C = "B2C_ANTRAGS_ID";
        private const String QUERY_VORVT = "select COUNTRENEWVAL as contractextcount,vt.sysid sysvorvt,vt.vertrag nrvorvt,antrag.sysvt sysvt,vt.rate ratevorvt from antrag left outer join antabl on antabl.sysantrag=antrag.sysid  and antrag.COUNTRENEWVAL>0 left outer join vt on vt.sysid=antabl.sysvorvt where antrag.sysid=:sysid";

        private const String EAI_ABWICKLUNG = "GET_SYSABWICKLUNG";
        // Ticket#2012070510000071 — Defect #6581 - Formalitäten werden bei KF in B2B nicht angezeigt
        // sysprchannel = 1: FahrzeugFinanzierung, sysprchannel = 2: KreditFinanzierung
        // contype = 1: Auflagen, contype = 2: Formalitäten
        // Formalitäten sollen nur bei KF mitgeliefert werden.
        // Ticket#2012080910000222 1. AS und 2. AS
        const String GETRATINGAUFLAGEN = "select distinct auflageText auflageText, antragsteller from ( select dedefcon.deftextext auflageText, dedefcon.rank rank, ratingauflage.sysperson antragsteller" +
                                        " from dedefcon, ratingauflage, rating " +
                                        " where rating.area = 'ANTRAG' and rating.sysid = :sysAntrag " +
                                        " and rating.sysrating = ratingauflage.sysrating " +
                                        " and ratingauflage.sysdedefcon = dedefcon.sysdedefcon " +
                                        " and dedefcon.contype  in  (:pcontype1, :pcontype2) " +
                                        " and ratingauflage.fullfilled in (:pfullfilled1, :pfullfilled2) " +
                                        " and not (ratingauflage.status = 1 and ratingauflage.activeflag = 0) " +
                                        " and dedefcon.sysdedefcon not in (select dedefcontxt.sysdedefcon from dedefcontxt, ctlang where dedefcontxt.sysctlang = ctlang.sysctlang and ctlang.isocode = :isoCode )" +
                                        " union all  " +
                                        " select distinct(dedefcontxt.textext) auflageText, dedefcon.rank rank, ratingauflage.sysperson antragsteller " +
                                        " from ctlang, dedefcontxt, dedefcon, ratingauflage, rating " +
                                        " where rating.area = 'ANTRAG' and rating.sysid = :sysAntrag " +
                                        " and rating.sysrating = ratingauflage.sysrating " +
                                        " and ratingauflage.sysdedefcon = dedefcon.sysdedefcon " +
                                        " and dedefcon.contype  in  (:pcontype1, :pcontype2) " +
                                        " and ratingauflage.fullfilled in (:pfullfilled1, :pfullfilled2) " +
                                        " and not (ratingauflage.status = 1 and ratingauflage.activeflag = 0) " +
                                        " and dedefcon.sysdedefcon = dedefcontxt.sysdedefcon " +
                                        " and dedefcontxt.sysctlang = ctlang.sysctlang  " +
                                        " and ctlang.isocode = :isoCode   " +
                                        " order by rank) order by antragsteller asc";



        const String GETECODEANMELDENERRCODE = "SELECT zekresdesc.retcode  " +
                                             "FROM auskunft, zekoutcode178, zekcmr, zekresdesc  " +
                                             "WHERE auskunft.sysauskunft  = zekoutcode178.sysauskunft " +
                                             "AND zekoutcode178.syszekcmr = zekcmr.syszekcmr " +
                                             "AND zekcmr.syszekcmr = zekresdesc.syszekcmr " +
                                             "AND auskunft.area           = 'ANTRAG' " +
                                             "AND auskunft.sysid          = :sysAntrag " +
                                             "AND auskunft.sysauskunfttyp = 77 " +
                                             "AND auskunft.statusnum      = 0 " +
                                             "ORDER BY auskunft.sysauskunft DESC";

        const String GETZUSTANDHISTORIE = "select zustand, alertdate zustandam from statealrt where area like 'ANTRAG' and syslease=:sysAntrag order by  sysstatealrt desc";
        const String GETZUSTANDAM = "select zustandam from antrag where sysid=:sysAntrag";

        //vc_obtyp2.id2 markenid,
        const String GETOBJECTDATAQUERY = "select vc_obtyp5.*, vc_obtyp5.id5 id, vc_obtyp1.bezeichnung fahrzeugart, vc_obtyp2.bezeichnung marke, vc_obtyp3.bezeichnung baureihe, vc_obtyp4.bezeichnung modell  from vc_obtyp1, vc_obtyp2, vc_obtyp3, vc_obtyp4, vc_obtyp5 where vc_obtyp1.id1=vc_obtyp2.id1 and vc_obtyp2.id2=vc_obtyp3.id2 and vc_obtyp3.id3=vc_obtyp4.id3 and vc_obtyp4.id4=vc_obtyp5.id4 and vc_obtyp5.id5=:id";

        const String GETREALZUSTAND = "select ctlang.sysctlang, ctlang.isocode, antrag.sysid, Zustandmap.ExterneZustand  origexternzustand,  case    when (select count (1) from cttstatedef where cttstatedef.sysctlang =ctlang.sysctlang and cttstatedef.sysstatedef = Zustandmap.ExterneZustandID) >0     then (select replacezustand from cttstatedef where cttstatedef.sysctlang =ctlang.sysctlang and cttstatedef.sysstatedef = Zustandmap.ExterneZustandID)     else Zustandmap.ExterneZustand end as ReplExterneZustand  from antrag,  ( select intstate.zustand InterneZustand, attributdef.attribut, extstate.zustand ExterneZustand, extstate.sysstatedef  ExterneZustandID from attribut, attributdef, state, statedef extstate, statedef intstate, wftable where  attribut.sysstate = state.sysstate and attribut.sysattributdef= attributdef.sysattributdef and attribut.sysstatedef= extstate.sysstatedef and state.sysstatedef=intstate.sysstatedef and state.syswftable = wftable.syswftable and wftable.syscode = 'ANTRAG' order by state.rang, attribut.rang ) Zustandmap, ctlang  where antrag.zustand=Zustandmap.InterneZustand  and antrag.attribut = Zustandmap.attribut and ctlang.flagtranslate = 1 and ctlang.isocode='de-CH' and antrag.sysid=:sysid";

        const String GETALLREALZUSTAND = "select ctlang.sysctlang, ctlang.isocode, antrag.sysid, Zustandmap.ExterneZustand  origexternzustand,  case    when (select count (1) from cttstatedef where cttstatedef.sysctlang =ctlang.sysctlang and cttstatedef.sysstatedef = Zustandmap.ExterneZustandID) >0     then (select replacezustand from cttstatedef where cttstatedef.sysctlang =ctlang.sysctlang and cttstatedef.sysstatedef = Zustandmap.ExterneZustandID)     else Zustandmap.ExterneZustand end as ReplExterneZustand  from antrag,  ( select intstate.zustand InterneZustand, attributdef.attribut, extstate.zustand ExterneZustand, extstate.sysstatedef  ExterneZustandID from attribut, attributdef, state, statedef extstate, statedef intstate, wftable where  attribut.sysstate = state.sysstate and attribut.sysattributdef= attributdef.sysattributdef and attribut.sysstatedef= extstate.sysstatedef and state.sysstatedef=intstate.sysstatedef and state.syswftable = wftable.syswftable and wftable.syscode = 'ANTRAG' order by state.rang, attribut.rang ) Zustandmap, ctlang  where antrag.zustand=Zustandmap.InterneZustand  and antrag.attribut = Zustandmap.attribut and ctlang.flagtranslate = 1 and ctlang.isocode='de-CH'";

        // 4x used
        const String SYSINTSTRCTBYPRODUCTID = "select SYSINTSTRCT from PRPRODUCT where SYSPRPRODUCT = :sysprProduct";
        const String GETANGEBOTDATA = "select sysprproduct, SYSOBUSETYPE from angkalk where syskalk = :syskalk";
        const String NAMEFROMPROVTYPEID = "select name from prprovtype where sysprprovtype= :sysprprovtype";

        const String GETFAKRA = "select count(antvs.sysantvs) from antrag, antvs, antkalk, prproduct, prclrsvset, prrsvpos " +
                                    " where antrag.sysid = antvs.sysantrag " +
                                    " and antkalk.sysantrag = antrag.sysid " +
                                    " and antkalk.sysprproduct = prproduct.sysprproduct " +
                                    " and antrag.sysid = :sysAntrag " +
                                    " and prclrsvset.sysprproduct = prproduct.sysprproduct " +
                                    " and prrsvpos.sysprrsvset = prclrsvset.sysprrsvset " +
                                    " and antrag.sysid = antvs.sysantrag " +
                                    " and antvs.sysvstyp = prrsvpos.sysvstyp " +
                                    " and prrsvpos.disabledflag = 0 ";
        const String HAENDLERABWICKLUNGSORTANGEBOT = @"SELECT *
                                        FROM (SELECT peabwo.sysperole
                                        FROM perelate, perole pevm, perole peabwo, angebot, wfuser antragsowner
                                        WHERE pevm.sysperson = decode(angebot.vertriebsweg, 'Barkredit vermittelt', angebot.sysvm, 'Fahrzeugfinanzierung vermittelt', angebot.sysvm, antragsowner.sysperson)
                                        AND perelate.sysperole2 = pevm.sysperole
                                        AND perelate.sysperole1 = peabwo.sysperole
                                        AND antragsowner.syswfuser = angebot.sysberater
                                        AND pevm.sysperson > 0 AND (perelate.relbeginndate IS NULL
                                        OR perelate.relbeginndate < to_date('01.01.1900' , 'dd.MM.yyyy')
                                        OR perelate.relbeginndate <= angebot.erfassung)
                                        AND (perelate.relenddate IS NULL
                                        OR perelate.relenddate < to_date('01.01.1900' , 'dd.MM.yyyy')
                                        OR perelate.relenddate >= angebot.erfassung)
                                        AND peabwo.sysroletype = 11 AND angebot.sysid = :sysid
                                        ORDER BY NVL(perelate.flagdefault, 0) DESC, perelate.sysperelate DESC)
                                        WHERE rownum = 1";
        const String HAENDLERABWICKLUNGSORT = "SELECT peabwo.sysperole " +
                                    " FROM perelate, perole pevm, perole peabwo  " +
            // Händler:
                                    " WHERE pevm.sysperson = :sysVM " +
                                    " AND (perelate.relbeginndate is null or perelate.relbeginndate < to_date('01.01.1900' , 'dd.MM.yyyy') OR perelate.relbeginndate <= sysdate)  " +
                                    " AND (perelate.relenddate is null or perelate.relenddate < to_date('01.01.1900' , 'dd.MM.yyyy') OR perelate.relenddate >= sysdate)  " +
                                    " AND perelate.sysperole2  = pevm.sysperole AND perelate.sysperole1 = peabwo.sysperole  " +
            // Abwicklungsort:
                                    " AND peabwo.sysroletype = 11 " +
            // falls falsch gepflegt
                                    " AND rownum = 1";

        const String GETEIGENTSEIT = "select ddlkpspos.value, ddlkpcol.code from ddlkpspos, ddlkpcol, ddlkprub " +
                                        " where ddlkpspos.sysddlkpcol = ddlkpcol.sysddlkpcol " +
                                        " and ddlkprub.sysddlkprub = ddlkpcol.sysddlkprub  " +
                                        " and ddlkprub.code = 'CASA' " +
                                        " and ddlkpcol.code in ('MONAT', 'JAHR') " +
                                        " and ddlkpspos.activeflag = 1 " +
                                        " and ddlkpspos.area = 'ANTRAG'  " +
                                        " and ddlkpspos.sysid = :sysAntrag ";

        const String GETPRCHANNEL = "select sysprchannel from antrag where sysid=:sysAntrag";
        const String QUERYFFORM = "select trim(ANTRAG.FFORM) fform from antrag where sysid= :psysid";
        const String QUERYSTRACCOUNT = " SELECT " +
                                        " CASE " +
                                        " WHEN COUNT(*) > 0 " +
                                        " THEN 1 " +
                                        " ELSE 0 " +
                                        " END AS STRAT " +
                                        " FROM roleattrib a, " +
                                        " perole p, " +
                                        " roleattribm m " +
                                        " WHERE a.sysroleattrib=m.sysroleattrib " +
                                        " AND p.sysperole      =m.sysperole " +
                                        " AND a.sysroleattrib  =22 " +
                                        " AND p.sysperson      =:p1 ";
        const String QUERYBWG = "select nvl(flagbwgarantie,0) from antrag where antrag.sysid = :psysid";
        const String QUERYSCOREBEZEICHNUNG = "select d.scorebezeichnung bezeichnung from deoutexec x, DEDETAIL d where x.SYSDEOUTEXEC=d.SYSDEOUTEXEC " +
                                             " and x.sysauskunft=(select max(sysauskunft) from auskunft where sysid=:psysid and auskunft.STATUSNUM=0 and auskunft.SYSAUSKUNFTTYP=3 and auskunft.AREA='ANTRAG')";


        // Konstanten
        const String NOTIZSYSCODE = "ANTRAG";
        const String NOTIZKAT = "Web-Pos";
        const String NOTIZKAT_ABLOESEN = "Web-Pos Ablösen";

        public const int ERFASSUNGSCLIENT_B2B = 10;
        public const int ERFASSUNGSCLIENT_MA = 20;//just for completeness
        public const int ERFASSUNGSCLIENT_ONE = 50;
        public const int ERFASSUNGSCLIENT_DMR = 60;
        public const int ERFASSUNGSCLIENT_B2C = 30;

        const String BKONTOTP_AUSZAHLUNGKREDIT = "Auszahlung Kredit";
        const String KURZTXTNOTIZ = "ePOS-now Mitteilung";

        private const int ABLTYPEIGEN = 1;
        private const String EIGENEBANK = "BANK-now AG";

        private const String QUERYSYSVG = @"SELECT SYSVG 
                                        FROM VG
                                        WHERE SYSVG =
                                            (SELECT OBTYP.SYSVGLGD
                                            FROM OBTYP
                                            WHERE OBTYP.SYSVGLGD        > 0 
                                             START WITH OBTYP.SYSOBTYP = 
                                            (SELECT SYSOBTYP FROM ANTOB WHERE SYSANTRAG=:p1 )
                                            CONNECT BY PRIOR OBTYP.SYSOBTYPP = OBTYP.SYSOBTYP AND ROWNUM   = 1)";

        private const String QUERYCLUSTER = "Select EWBBETRAG v_el_betrag, EWBPROC v_el_prozent,EWBPROF v_prof from ANTRAG where sysid=:psysid";
        private const String QUERYCLUSTERBETRAG = "Select EWBBETRAG v_el_betrag from ANTRAG where sysid=:psysid";
        private const String QUERYCLUSTERPROZENT = "Select EWBPROC v_el_prozent from ANTRAG where sysid=:psysid";
        private const String QUERYCLUSTERPROF = "Select EWBPROF v_prof from ANTRAG where sysid=:psysid";



        //private const String QUERYDDLKPPOS = "select ddlkpspos.value from ddlkpspos where ddlkpspos.area = :parea and sysid = :psysid and activeflag = 1 and sysddlkpcol in (Select sysddlkpcol from ddlkpcol where code = :pcode)";
        private const String QUERYDDLKPPOS = "select nvl(decode(ddlkpcol.type,2,KPPOS.Wert, ddlkpspos.value),ddlkpspos.value)  Wert " +
                                                "from   ddlkpcol, ddlkpspos," +
                                                "(" +
                                                "select ddlkppos.sysddlkppos, vc_ddlkppos.id, nvl(vc_ddlkppos.actualterm, ddlkppos.value) Wert  " +
                                                "from cic.vc_ddlkppos, ddlkppos " +
                                                "where vc_ddlkppos.sysddlkppos = ddlkppos.sysddlkppos " +
                                                "and ddlkppos.code= :pcode " +
                                                "and vc_ddlkppos.sysctlang= :pctlang " +
                                                ") KPPOS " +
                                                "where ddlkpspos.sysddlkpcol=ddlkpcol.sysddlkpcol " +
                                                "and ddlkpcol.code= :pcode " +
                                                "and ddlkpspos.area = :parea  " +
                                                "and ddlkpspos.sysid = :psysid " +
                                                "and KPPOS.id (+)= ddlkpspos.value ";


        const String QUERYSCOREINDEDETAIL = "select scoretotal from  dedetail, deoutexec where deoutexec.SYSDEOUTEXEC = dedetail.SYSDEOUTEXEC  " +
                                            " and deoutexec.SYSAUSKUNFT = (select max(sysauskunft) from auskunft " +
                                            " where sysauskunfttyp = 3 and statusnum = 0 and sysid = :psysid) ";

        const String QUERYDISTEXT = @"select  cttfoid.replaceterm as disclaimer from ctfoid, cttfoid, ctlang WHERE 
              ctfoid.frontid =cttfoid.frontid AND ctlang.sysctlang =cttfoid.sysctlang AND ctlang.isocode= :pisocode AND ctlang.flagtranslate=1 and ctfoid.frontid in ('DISCLAIMER_CONFIRM','DISCLAIMER_1','DISCLAIMER_2', 'DISCLAIMER_3', 'DISCLAIMER_4','DISCLAIMER_5') order by ctfoid.sysctfoid asc";

        const String QUERYDISPOPUP = @"select  cttfoid.replaceterm as disclaimer from ctfoid, cttfoid, ctlang WHERE 
                ctfoid.frontid =cttfoid.frontid AND ctlang.sysctlang =cttfoid.sysctlang AND ctlang.isocode= :pisocode AND ctlang.flagtranslate=1 and ctfoid.frontid ='DISCLAIMER_POPUP' ";

        const String QUERYDISTEXTANGEBOT = @"select cttfoid.replaceterm  as disclaimer from ctfoid, cttfoid, ctlang WHERE 
              ctfoid.frontid =cttfoid.frontid AND ctlang.sysctlang =cttfoid.sysctlang AND ctlang.isocode= :pisocode AND ctlang.flagtranslate=1 and ctfoid.frontid =:disccode order by ctfoid.sysctfoid asc";

      

        
        const String QUERYPRODUCTCODE = @"select CASE
    WHEN vttyp.code LIKE '%CASA%'
    THEN 'CASA'
    WHEN vttyp.code LIKE '%DIPLOMA%'
    THEN 'DIPLOMA'
    WHEN vttyp.code LIKE '%FLEX%'
    THEN 'FLEX'    
    WHEN vart.code ='KREDIT_CLASSIC'
    THEN 'CLASSIC'
    WHEN vart.code ='KREDIT_DISPOPLUS'
    THEN 'CARD'
  END code
 from angebot,angkalk,prproduct,vttyp,vart where prproduct.sysprproduct=angkalk.sysprproduct and prproduct.sysvart=vart.sysvart(+) and prproduct.sysvttyp=vttyp.sysvttyp(+) and ANGEBOT.SYSID = ANGKALK.SYSANGEBOT and angkalk.sysangebot=:sysangebot";

            
       
        const String QUERYDDLKPSPOS_B2C = "select ddlkpspos.sysddlkpspos sysddlkpspos, ddlkpcol.code, ddlkpspos.value from ddlkpspos,ddlkpcol,ddlkprub  where ddlkpspos.sysddlkpcol=ddlkpcol.sysddlkpcol and ddlkpcol.sysddlkprub=ddlkprub.sysddlkprub  and ddlkpspos.area=:area and ddlkprub.code=:prcode  and ddlkpspos.sysid=:sysid";
        const String QUERYUPD_DDLKPSPOS_B2C = "select  ddlkpcol.sysddlkpcol,ddlkpcol.code,ddlkprub.code rubrik from ddlkpcol,ddlkprub  where ddlkpcol.sysddlkprub=ddlkprub.sysddlkprub and ddlkprub.code in ('CASA','DIPLOMA')";


        const string QUERYPROFINLOCK = "SELECT COUNT(*) " +
                                        "FROM wftzust, " +
                                        "wftzvar " +
                                        "WHERE wftzvar.SYSWFTZUST  = wftzust.SYSWFTZUST " +
                                        "AND upper(wftzust.Gebiet) = 'ANTRAG' " +
                                        "AND wftzust.STATE         = 'AUTOCHECK' " +
                                        "AND (wftzvar.value        = 'Start' or WFTZUST.status = 0) " +
                                        "AND wftzust.syslease      = :psysid ";

        const string QUERYAUSDATUM = "select ausdatum from vt where zustand = 'aktiv' and sysantrag = :psysid";
        
        private bool isb2b = true;
        private IEaihotDao eaiHotDao;

        private enum MitantragstellerTyp : int
        {
            Solidarschuldner = 120,
            Bürge = 130,
            Partner = 800,
            Mitantragsteller = 10,//203
            Vertretungsberechtigter = 140,//234
            Bürgschaften_Garantien = 80
        }

        public AngAntDao(IEaihotDao eaiHotDao)
        {
            this.eaiHotDao = eaiHotDao;
        }

        /// <summary>
        /// setIsB2B
        /// </summary>
        /// <param name="isB2B"></param>
        public void setIsB2B(bool isB2B)
        {
            isb2b = isB2B;
        }

        /// <summary>
        /// Delivers the Auflagen for the Antrag
        /// </summary>
        /// <param name="sysid">Primaerschluessel</param>
        /// <param name="isoCode">ISO Sprachencode</param>
        /// <returns></returns>
        public String[] getAuflagen(long sysid, String isoCode)
        {

            String zustand = getAntragZustand(sysid);
            List<RatingAuflagen> auflagen = new List<RatingAuflagen>();

            bool FF;
            bool KF;

            const int pauflagen = 1;
            const int pformalitaeten = 2;
            //----------------------TODO - Auszahlungspendenz ist NIE ein Ergebnis der getAntragZustand-Funktion!!!!! es wäre in dem Fall immer "Fehlende Unterlagen"
            using (DdOlExtended context = new DdOlExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysAntrag", Value = sysid });

                long sysprchannel = context.ExecuteStoreQuery<long>(GETPRCHANNEL, parameters.ToArray()).FirstOrDefault();

                FF = sysprchannel == 1;
                KF = sysprchannel == 2;

                if ((zustand.Equals("Bewilligt") || zustand.Equals("Auszahlungsprüfung") || zustand.Equals("Auszahlungspendenz")) && FF)
                {
                    parameters.Clear();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysAntrag", Value = sysid });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "isoCode", Value = isoCode });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pcontype1", Value = pformalitaeten });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pcontype2", Value = pformalitaeten });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pfullfilled1", Value = 0 });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pfullfilled2", Value = 0 });

                    auflagen = context.ExecuteStoreQuery<RatingAuflagen>(GETRATINGAUFLAGEN, parameters.ToArray()).ToList();

                }
                else if (zustand.Equals("Auflagen offen") && FF)
                {
                    parameters.Clear();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysAntrag", Value = sysid });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "isoCode", Value = isoCode });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pcontype1", Value = pauflagen });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pcontype2", Value = pauflagen });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pfullfilled1", Value = 0 });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pfullfilled2", Value = 0 });

                    auflagen = context.ExecuteStoreQuery<RatingAuflagen>(GETRATINGAUFLAGEN, parameters.ToArray()).ToList();
                }
                else if ((zustand.Equals("Bewilligt mit Auflagen") || zustand.Equals("Bewilligt") || zustand.Equals("Auszahlungsprüfung") || zustand.Equals("Auszahlungspendenz")) && KF)
                {
                    parameters.Clear();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysAntrag", Value = sysid });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "isoCode", Value = isoCode });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pcontype1", Value = pformalitaeten });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pcontype2", Value = pauflagen });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pfullfilled1", Value = 0 });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pfullfilled2", Value = 0 });

                    auflagen = context.ExecuteStoreQuery<RatingAuflagen>(GETRATINGAUFLAGEN, parameters.ToArray()).ToList();

                }
                else//Auszahlungspendenz geht also momentan immer hier rein (weil es Fehlende Unterlagen sind), das scheint aber eh zu passen
                {
                    parameters.Clear();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysAntrag", Value = sysid });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "isoCode", Value = isoCode });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pcontype1", Value = pformalitaeten });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pcontype2", Value = pauflagen });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pfullfilled1", Value = 0 });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pfullfilled2", Value = 0 });

                    auflagen = context.ExecuteStoreQuery<RatingAuflagen>(GETRATINGAUFLAGEN, parameters.ToArray()).ToList();
                }



                List<String> retAuflagen = new List<String>();
                var as1 = (from ant in context.ANTRAG
                           where ant.SYSID == sysid
                           select ant.PERSON.SYSPERSON).FirstOrDefault();

                foreach (var auflage in auflagen)
                {
                    //Ticket#2012080910000222 1. AS und 2. AS
                    if (as1 == auflage.antragsteller)
                        retAuflagen.Add("1. AS " + auflage.auflageText);
                    else
                        if (as1 != auflage.antragsteller)
                            retAuflagen.Add("2. AS " + auflage.auflageText);
                }
                return retAuflagen.ToArray();
            }
        }

        /// <summary>
        /// Delivers the Antrag stati history
        /// </summary>
        /// <param name="sysid">Primaerschluessel</param>
        /// <param name="isoCode">ISO Sprachencode</param>
        /// <returns></returns>
        public ZustandDto[] getZustaende(long sysid, String isoCode)
        {
            using (DdOlExtended context = new DdOlExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysAntrag", Value = sysid });
                //parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "isoCode", Value = isoCode });

                List<ZustandDto> zustaende = context.ExecuteStoreQuery<ZustandDto>(GETZUSTANDHISTORIE, parameters.ToArray()).ToList();
                if (zustaende.Count == 0)
                {
                    ZustandDto zu = new ZustandDto();
                    zu.zustand = getAntragZustand(sysid);
                    parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysAntrag", Value = sysid });

                    zu.zustandAm = context.ExecuteStoreQuery<DateTime>(GETZUSTANDAM, parameters.ToArray()).FirstOrDefault();

                    zustaende.Add(zu);
                }
                return zustaende.ToArray();
            }
        }

        /// <summary>
        /// Delivers the "real"zustand composed of ZUSTAND and ATTRIBUT for a Antrag
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        public String getAntragZustand(long sysid)
        {
            using (DdOlExtended context = new DdOlExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = sysid });

                Zustaende z = context.ExecuteStoreQuery<Zustaende>(GETREALZUSTAND, parameters.ToArray()).FirstOrDefault();
                if (z == null) return "";
                return z.origexternzustand;
            }
        }

        /// <summary>
        /// Antrag Bemerkung aus DDLKPPOS
        /// </summary>
        /// <param name="sysid"></param>
        /// <param name="area"></param>
        /// <param name="code"></param>
        /// <param name="ctlang"></param>
        /// <returns></returns>
        public String getAntragBemerkung(long sysid, string area, string code, long ctlang)
        {


            using (DdOdExtended context = new DdOdExtended())
            {

                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysid", Value = sysid });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "parea", Value = area });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pcode", Value = code });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pctlang", Value = ctlang });


                string bemerkung = context.ExecuteStoreQuery<string>(QUERYDDLKPPOS, parameters.ToArray()).FirstOrDefault();
                return bemerkung;
            }

        }



        /// <summary>
        /// Zustand auslesen
        /// </summary>
        /// <param name="antraege">Antragsliste</param>
        public void fetchStates(AntragDto[] antraege)
        {
            if (antraege == null || antraege.Length == 0) return;

            System.Text.StringBuilder b = new System.Text.StringBuilder();
            for (int i = 0; i < antraege.Length; i++)
            {
                if (i > 0)
                    b.Append(",");
                b.Append(antraege[i].sysid);
            }

            using (DdOlExtended context = new DdOlExtended())
            {
                // Security check: b = resultset aus antragIds nur intern
                List<Zustaende> zustaende = context.ExecuteStoreQuery<Zustaende>(GETALLREALZUSTAND + " and antrag.sysid in (" + b.ToString() + ")", null).ToList();
                Dictionary<long, Zustaende> lookup = new Dictionary<long, Zustaende>();
                foreach (Zustaende z in zustaende)
                {
                    lookup[z.sysid] = z;
                }
                foreach (AntragDto ad in antraege)
                {
                    if (lookup.ContainsKey(ad.sysid))
                        ad.zustand = lookup[ad.sysid].origexternzustand;
                }
            }
        }

        /// <summary>
        /// setAngebotZustandAttribute
        /// </summary>
        /// <param name="sysid">sysid</param>
        /// <param name="uc">uc</param>
        public void setAngebotZustandAttribute(long sysid, String uc)
        {
            using (DdOlExtended context = new DdOlExtended())
            {
                var currentAngebot = (from Angebot in context.ANGEBOT
                                      where Angebot.SYSID == sysid
                                      select Angebot).FirstOrDefault();

                // Check if ANGEBOT was found
                if (currentAngebot != null)
                {
                    switch (uc)
                    {
                        case "AngebottoAntrag":
                            currentAngebot.ZUSTAND = Cic.OpenOne.Common.Util.EnumUtil.GetStringValue(AngebotZustand.Abgeschlossen);
                            currentAngebot.ATTRIBUT = Cic.OpenOne.Common.Util.EnumUtil.GetStringValue(AngebotAttribut.Antrageingereicht);
                            break;
                    }
                    context.SaveChanges();
                }
                else
                {
                    throw new Exception("Invalid sysid in Angebot : " + sysid);
                }
            }
        }

        /// <summary>
        /// Neues Angebot erstellen
        /// </summary>
        /// <param name="angebotInput">Angebot Eingabe</param>
        /// <param name="sysperole">User Id</param>
        /// <returns>Angebot Ausgabe</returns>
        public AngebotDto createAngebot(AngebotDto angebotInput, long sysperole)
        {
            return createAngebot(angebotInput, null, null, sysperole);
        }

        /// <summary>
        /// Creates a KREMO for the Offer
        /// </summary>
        /// <param name="angebot"></param>
        private void createOrUpdateKREMOAngebot(long sysangebot, KundeDto kunde, KundeDto mitantragsteller)
        {
            

            KREMOInDto kin = new KREMOInDto();
            kin.SysAngebot = sysangebot;
            int errcount = 0;
            try { kin.betreuungskosten = kunde.zusatzdaten[0].pkz[0].betreuungskosten.GetValueOrDefault(); } catch (Exception e) { errcount++; }
            try { kin.arbeitswegpauschale1 = kunde.zusatzdaten[0].pkz[0].arbeitswegpauschale1.GetValueOrDefault(); } catch (Exception e) { errcount++; }
            try { kin.krankenkasse = kunde.zusatzdaten[0].pkz[0].krankenkasse.GetValueOrDefault(); } catch (Exception e) { errcount++; }
            if (mitantragsteller!=null)
            try { kin.arbeitswegpauschale2 = mitantragsteller.zusatzdaten[0].pkz[0].arbeitswegpauschale1.GetValueOrDefault(); } catch (Exception e) { errcount++; }

            if (errcount > 2) return; //no data available
            DAO.CommonDaoFactory.getInstance().getKremoDao().CreateOrUpdateKREMOInDto(kin);
        }
       

        /// <summary>
        /// Updates the KREMO-Values for an ANTRAG
        /// </summary>
        /// <param name="sysantrag"></param>
        /// <param name="kunde"></param>
        /// <param name="mitantragsteller"></param>
        /// <returns>true when krankenkasse greater zero</returns>
        private bool createOrUpdateKREMOAntrag(long sysangebot, long sysantrag, KundeDto kunde, KundeDto mitantragsteller)
        {
            IKREMODBDao dao = DAO.CommonDaoFactory.getInstance().getKremoDao();
            KREMOInDto kin = dao.FindBySysAntrag(sysantrag);

            if (kin == null)
            {
                if (sysangebot > 0)
                {
                    kin = DAO.CommonDaoFactory.getInstance().getKremoDao().FindBySysAngebot(sysangebot);
                    if (kin != null)
                        kin.SysAngebot = 0;
                }
                if(kin==null)
                    kin = new KREMOInDto();

            }

            kin.SysAntrag = sysantrag;
            int errcount = 0;
            try { kin.betreuungskosten = kunde.zusatzdaten[0].pkz[0].betreuungskosten.GetValueOrDefault(); } catch (Exception e) { errcount++; }
            try { kin.arbeitswegpauschale1 = kunde.zusatzdaten[0].pkz[0].arbeitswegpauschale1.GetValueOrDefault(); } catch (Exception e) { errcount++; }
            try { kin.krankenkasse = kunde.zusatzdaten[0].pkz[0].krankenkasse.GetValueOrDefault(); } catch (Exception e) { errcount++; }
            if (mitantragsteller != null)
                try { kin.arbeitswegpauschale2 = mitantragsteller.zusatzdaten[0].pkz[0].arbeitswegpauschale1.GetValueOrDefault(); } catch (Exception e) { errcount++; }

            if (errcount > 2) return false; //no data available
            DAO.CommonDaoFactory.getInstance().getKremoDao().CreateOrUpdateKREMOInDto(kin);
            return kin.krankenkasse > 0;
        }

        /// <summary>
        /// Neues Angebot erstellen
        /// </summary>
        /// <param name="angebotInput">Angebot Eingabe</param>
        /// <param name="aktivKz"></param>
        /// <param name="endeKz"></param>
        /// <param name="sysperole">User Id</param>
        /// <returns>Angebot Ausgabe</returns>
        public AngebotDto createAngebot(AngebotDto angebotInput, int? aktivKz, int? endeKz, long sysperole)
        {
            double start = DateTime.Now.TimeOfDay.TotalMilliseconds;
            using (DdOlExtended context = new DdOlExtended())
            {
                IMapper mapper = Mapper.Instance;
                /*Cic.One.Utils.Util.Mapper.MapperConfig.getInstanceMapper("createAngebot", delegate (MapperConfigurationExpression cfg)
                {

                    cfg.CreateMap<AngAntVarDto, ANGVAR>();
                    cfg.CreateMap<AngAntProvDto, ANGPROV>();
                    cfg.CreateMap<AngAntSubvDto, ANGSUBV>();
                    cfg.CreateMap<AngAntAblDto, ANGABL>();
                });*/
                //use kunde id if given 
                if (angebotInput.kunde != null)
                    angebotInput.sysit = angebotInput.kunde.sysit;
                angebotInput.erfassung = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null);
                angebotInput.aenderung = angebotInput.erfassung;
                angebotInput.zustandAm = angebotInput.erfassung;

                int lifetime = (int)Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getQuoteDao().getQuote("Angebotgueltigbis");
                angebotInput.gueltigBis = angebotInput.erfassung.Value.Add(new TimeSpan(lifetime, 0, 0, 0));
                
                ANGEBOT angebotOutput = new ANGEBOT();

                if (angebotInput.options != null)
                {
                    angebotOutput.ANGOPTION = mapper.Map<AngAntOptionDto, ANGOPTION>(angebotInput.options);
                }

                mapper.Map<AngebotDto, ANGEBOT>(angebotInput, angebotOutput);
                _log.Debug("createAngebot A: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));
                start = (long)(DateTime.Now.TimeOfDay.TotalMilliseconds);
                // Ticket#2012090310000071
                if (angebotInput.erfassungsclient > 0)
                    // angebotInput.erfassungsclient wird nur bei copyAngebot (MAClient) im Dto übergeben
                    angebotOutput.ERFASSUNGSCLIENT = (int?)angebotInput.erfassungsclient;
                else
                    // createAngebot wird nur vom B2B aufgerufen, da wird erfassungsclient nicht mitgeschickt, MAClient verwendet Clarion
                    angebotOutput.ERFASSUNGSCLIENT = ERFASSUNGSCLIENT_B2B;
                // das Feld sysls muss gefüllt werden. Hier muss immer der Wert 1 stehen.
                angebotOutput.SYSLS = 1;
                if (angebotOutput.ERFASSUNGSCLIENT == ERFASSUNGSCLIENT_B2B || angebotOutput.ERFASSUNGSCLIENT == ERFASSUNGSCLIENT_B2C || angebotOutput.ERFASSUNGSCLIENT == ERFASSUNGSCLIENT_ONE || angebotOutput.ERFASSUNGSCLIENT == ERFASSUNGSCLIENT_DMR)
                {
                    // Ticket#2012090310000071: für den MAClient soll der Vertriebsweg einfach übernommen werden (copyAngebot)
                    if (angebotOutput.ERFASSUNGSCLIENT == ERFASSUNGSCLIENT_B2B || angebotOutput.ERFASSUNGSCLIENT == ERFASSUNGSCLIENT_B2C)
                        angebotOutput.VERTRIEBSWEG = MyGetVertriebsWeg(angebotOutput.SYSPRCHANNEL, angebotOutput.ERFASSUNGSCLIENT.Value, angebotInput.vertriebsweg );

                    // Ticket#2012090510000531
                    // antragOutput.SYSVK (Verkäufer) = PERSON von sysperole
                    angebotOutput.SYSVK = (from perole in context.PEROLE
                                           where perole.SYSPEROLE == sysperole
                                           select perole.SYSPERSON).FirstOrDefault();
                    
                    //Webfrontend übernimmt immer den VM + VK von aussen bei Fahrzeugfin (weil in Maske)
                    if (angebotInput.erfassungsclient == ERFASSUNGSCLIENT_DMR  || (angebotInput.erfassungsclient == ERFASSUNGSCLIENT_ONE && angebotInput.vertriebsweg != null && angebotInput.vertriebsweg.ToUpper().IndexOf("FAHRZ") > -1))
                    {
                        angebotOutput.SYSVM = angebotInput.sysVM;
                       // if (angebotInput.sysVK > 0)//in GUI kein Pflichtfeld, aber für Desktop notwendig!
                        angebotOutput.SYSVK = angebotInput.sysVK;
                    }
                    else
                    {
                        //antragOutput.SYSVM (Vermittler) = Person von Händler (Parent vom Verkäufer)
                        angebotOutput.SYSVM = (from parentPeRole in context.PEROLE
                                               join perole in context.PEROLE on parentPeRole.SYSPARENT equals perole.SYSPEROLE
                                               where parentPeRole.SYSPEROLE == sysperole
                                               select perole.SYSPERSON).FirstOrDefault();
                    }
                    if ((angebotInput.erfassungsclient == ERFASSUNGSCLIENT_ONE || angebotInput.erfassungsclient == ERFASSUNGSCLIENT_DMR) && angebotInput.syskd != null && angebotInput.syskd.HasValue && angebotInput.syskd.Value > 0)
                        angebotOutput.SYSKD=angebotInput.syskd.Value;

                    angebotOutput.SYSPRHGROUP = 0;
                    try
                    {
                        // Handelsgruppe wird in solveKalkulation berechnet 
                        // sie ist für jede Variante gleich, die Handelsgruppe ändert sich nicht
                        angebotOutput.SYSPRHGROUP = angebotInput.angAntVars[0].kalkulation.angAntKalkDto.sysprhgrp;
                    }
                    catch (Exception) { }

                    // Brand wird anhand der Handelsgruppe ermittelt
                    angebotOutput.SYSBRAND = (from prbrandm in context.PRBRANDM
                                           join brand in context.BRAND on prbrandm.BRAND.SYSBRAND equals brand.SYSBRAND
                                           where prbrandm.PRHGROUP.SYSPRHGROUP == angebotOutput.SYSPRHGROUP
                                           select prbrandm.SYSBRAND).FirstOrDefault();

                    // AbwicklungsOrt (PeRole) ist dem Händler zugeordnet
                    if (angebotOutput.ERFASSUNGSCLIENT == ERFASSUNGSCLIENT_B2B)
                        angebotOutput.SYSABWICKLUNG = MyGetHaendlerAbwicklungsort(angebotOutput.SYSVM);
                    
                     
                    // MwSt
                    angebotOutput.SYSMWST = context.ExecuteStoreQuery<long>("select sysmwst from lsadd where syslsadd=" + angebotOutput.SYSLS).FirstOrDefault();

                }
                else
                {
                    // Ticket#2012090510000531 : für MAClient sysBrand und sysWfUser mitkopieren
                    angebotOutput.SYSBRAND=angebotInput.sysbrand;
                    angebotOutput.SYSWFUSER = angebotInput.syswfuser;
                }
             
                _log.Debug("createAngebot B: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));
                start = (long)(DateTime.Now.TimeOfDay.TotalMilliseconds);
                angebotOutput.ZUSTAND = AngebotZustand.Neu.ToString();
                angebotOutput.ATTRIBUT = Cic.OpenOne.Common.Util.EnumUtil.GetStringValue(AngebotAttribut.NeuGueltig);
                angebotOutput.AKTIVKZ = aktivKz;
                angebotOutput.ENDEKZ = endeKz;

                
                angebotOutput.ANGEBOT1 = NkBuilderFactory.createAngebotNkBuilder().getNextNumber();

                context.ANGEBOT.Add(angebotOutput);
                context.SaveChanges();

                if (!String.IsNullOrEmpty(angebotInput.extident))
                {
                    ExtidentDto exti = new ExtidentDto();
                    exti.area = "ANGEBOT";
                    exti.extidentvalue = angebotInput.extident;
                    exti.sysarea = angebotOutput.SYSID;
                    exti.source = "" + angebotOutput.ERFASSUNGSCLIENT;
                    exti.codeextidenttyp = CODE_EXTIDENT_ANTRAGSID_B2C;
                    createOrUpdateExtident(exti);
                }

                _log.Debug("createAngebot C: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));
                start = (long)(DateTime.Now.TimeOfDay.TotalMilliseconds);
                if (angebotOutput.ERFASSUNGSCLIENT == ERFASSUNGSCLIENT_B2C)
                {
                    angebotInput.sysid = angebotOutput.SYSID;
                    //done via eaihot
                    //angebotOutput.SYSABWICKLUNG = getAbwicklungsortB2C(angebotInput,eaiHotDao);
                }
                else if (angebotOutput.ERFASSUNGSCLIENT == ERFASSUNGSCLIENT_ONE || angebotOutput.ERFASSUNGSCLIENT == ERFASSUNGSCLIENT_DMR)
                {
                    if(!angebotOutput.SYSABWICKLUNG.HasValue ||angebotOutput.SYSABWICKLUNG.Value==0)
                        angebotOutput.SYSABWICKLUNG = MyGetHaendlerAbwicklungsortOne(angebotOutput.SYSID);
                }

                SetAngebotIdInZusatzdaten(context, angebotInput.kunde, angebotOutput.SYSID);


                createOrUpdateKREMOAngebot(angebotOutput.SYSID, angebotInput.kunde, angebotInput.mitantragsteller);
                _log.Debug("createAngebot D: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));
                start = (long)(DateTime.Now.TimeOfDay.TotalMilliseconds);
                //Object--------------------------------------------------
                IPrismaDao prismaDao = PrismaDaoFactory.getInstance().getPrismaDao();
                ANGOB angob = null;
                if (angebotInput.angAntObDto != null)
                {
                    long vc_Objtyp1ID = prismaDao.getObjtyp1ID(angebotInput.angAntObDto.fzart);
                    fixAHK(angebotInput.angAntObDto, angebotInput.sysprchannel);
                    angob = new ANGOB();
                    mapper.Map<AngAntObDto, ANGOB>(angebotInput.angAntObDto, angob);
                    context.ANGOB.Add(angob);
                    
                    if (angob.BEZEICHNUNG != null && angob.BEZEICHNUNG.Length > 40)
                        angob.BEZEICHNUNG = angob.BEZEICHNUNG.Substring(0, 40);

                    //angob.ANGEBOT = angebotOutput;
                    angob.SYSANGEBOT = angebotOutput.SYSID;
                    angob.OBJEKT = NkBuilderFactory.createAngobNkBuilder().getNextNumber();
                    angob.OBART = context.OBART.Where(par => par.SYSOBART == angebotInput.angAntObDto.sysobart).FirstOrDefault();
                    angob.OBTYP = context.OBTYP.Where(par => par.SYSOBTYP == angebotInput.angAntObDto.sysobtyp).FirstOrDefault();
                    if (angob.OBTYP == null)
                        angob.OBTYP = context.OBTYP.Where(par => par.SYSOBTYP == vc_Objtyp1ID).FirstOrDefault();
                    //Brief----------------------------------------------------
                    if (angebotInput.angAntObDto.brief != null)
                    {
                        ANGOBBRIEF brief = new ANGOBBRIEF();
                        mapper.Map<AngAntObBriefDto, ANGOBBRIEF>(angebotInput.angAntObDto.brief, brief);
                        context.ANGOBBRIEF.Add(brief);
                        brief.ANGOB = angob;
                    }
                }
                _log.Debug("createAngebot E: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));
                start = (long)(DateTime.Now.TimeOfDay.TotalMilliseconds);
                
               

                //Variants--------------------------------------------------
                foreach (AngAntVarDto angantvar in angebotInput.angAntVars)
                {
                    ANGVAR angvar = new ANGVAR();
                    mapper.Map<AngAntVarDto, ANGVAR>(angantvar, angvar);
                    context.ANGVAR.Add(angvar);
                    //angvar.ANGEBOT = angebotOutput;
                    angvar.SYSANGEBOT = angebotOutput.SYSID;

                    bool isLeaseNow = false;
                    bool isDiffLeasing = false;
                    bool isDispo = false;
                    bool isClassic = false;
                    bool isCard = false;
                    String vttypCode = null;

                    if (angantvar.kalkulation != null)
                    {
                        try
                        {
                            if (angantvar.kalkulation.angAntKalkDto != null)
                            {
                                //Produkte werden anhand vart unterschieden: es gibt KREDIT, TZK, LEASE_now, CREDIT-now Dispo, DifferenzLeasing
                                CIC.Database.PRISMA.EF6.Model.VART vart = prismaDao.getVertragsart((long)angantvar.kalkulation.angAntKalkDto.sysprproduct);
								CIC.Database.PRISMA.EF6.Model.VTTYP vtt = prismaDao.getVttyp((long)angantvar.kalkulation.angAntKalkDto.sysprproduct);
                                if (vtt != null)
                                {
                                    vttypCode = vtt.CODE;
                                    if ((angvar.INANTRAG.HasValue && angvar.INANTRAG.Value > 0) || angebotInput.angAntVars.Count == 1)
                                        angebotOutput.SYSVTTYP = vtt.SYSVTTYP;
                                }
                                else if ((angvar.INANTRAG.HasValue && angvar.INANTRAG.Value > 0) || angebotInput.angAntVars.Count == 1)
                                    angebotOutput.SYSVTTYP = null;


                                if ((angvar.INANTRAG.HasValue && angvar.INANTRAG.Value > 0) || angebotInput.angAntVars.Count == 1)
                                {
                                    if (vart != null) angebotOutput.SYSVART = vart.SYSVART;
                                    angebotOutput.SYSPRPRODUCT = angantvar.kalkulation.angAntKalkDto.sysprproduct;
                                }
                                // VertragsArt ermitteln

                                isDiffLeasing = prismaDao.isDiffLeasing((long)angantvar.kalkulation.angAntKalkDto.sysprproduct); // Differenzleasing 
                                if (vart != null)
                                {
                                    String vArtCode = vart.CODE.ToLower();
                                    isLeaseNow = (vArtCode.IndexOf("leasing") > -1);                                // LEASE-now
                                    isDispo = (vArtCode.IndexOf("dispo") > -1) || (vArtCode.IndexOf("flex") > -1); // CREDIT_now Dispo
                                    isClassic = (vArtCode.IndexOf("classic") > -1);                             // CREDIT_now Classic
                                    isCard = (vArtCode.IndexOf("kredit_dispoplus") > -1);                           // CREDIT_now Card
                                }
                                //B2C Hotfix
                                angebotOutput.RATE = (decimal)angantvar.kalkulation.angAntKalkDto.initLadung;
                            }
                        }
                        catch (Exception ea) {
                            _log.Warn("Error writing back SYSPRPRODUCT/SYSVART to ANGEBOT", ea);
                        }

                        //Calculation--------------------------------------------
                        ANGKALK angkalk = new ANGKALK();

                        mapper.Map<AngAntKalkDto, ANGKALK>(angantvar.kalkulation.angAntKalkDto, angkalk);
                        context.ANGKALK.Add(angkalk);
                        
                       
                        angkalk.SYSANGEBOT = angebotOutput.SYSID;
                        
                        angkalk.SYSANGVAR = angvar.SYSANGVAR;

                        // Ticket#2012090510000531 : Felder bei CopyAngebotByID
                        if (angebotOutput.ERFASSUNGSCLIENT != ERFASSUNGSCLIENT_B2B && angebotOutput.ERFASSUNGSCLIENT != ERFASSUNGSCLIENT_B2C && angebotOutput.ERFASSUNGSCLIENT != ERFASSUNGSCLIENT_ONE && angebotOutput.ERFASSUNGSCLIENT != ERFASSUNGSCLIENT_DMR)
                        {
                            angkalk.SYSCREATE = angantvar.kalkulation.angAntKalkDto.syscreate;
                            angkalk.SYSCHANGE = angantvar.kalkulation.angAntKalkDto.syschange;
                        }

                        if ((angvar.INANTRAG.HasValue && angvar.INANTRAG.Value > 0) || angebotInput.angAntVars.Count==1)
                        {
                            angebotOutput.SYSPRPRODUCT = angantvar.kalkulation.angAntKalkDto.sysprproduct;
                        }

                        angkalk.SYSPRPRODUCT = angantvar.kalkulation.angAntKalkDto.sysprproduct;
                        angkalk.OBUSETYPE = context.OBUSETYPE.Where(par => par.SYSOBUSETYPE == angantvar.kalkulation.angAntKalkDto.sysobusetype).FirstOrDefault();

                        
                        angkalk.SYSWAEHRUNG = context.ExecuteStoreQuery<long>("select syshauswaehrung from lsadd where syslsadd=1", null).FirstOrDefault(); 

                        if (isDispo == true)
                        {
                            List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysprProduct", Value = angantvar.kalkulation.angAntKalkDto.sysprproduct });

                            angkalk.SYSINTSTRCT = context.ExecuteStoreQuery<long>(SYSINTSTRCTBYPRODUCTID, parameters.ToArray()).FirstOrDefault();
                        }

                        // Modus muss bei kredit 0, bei leasing 1 sein
                        if (isLeaseNow == true) { angkalk.MODUS = 1; } else { angkalk.MODUS = 0; }

                        // PPY (Raten pro Jahr): Konstant 12 bei bnow
                        angkalk.PPY = 12;
                        if (isClassic == true)
                        {
                            angkalk.AUSZAHLUNG = (decimal?)angantvar.kalkulation.angAntKalkDto.auszahlung;
                            angkalk.AUSZAHLUNGTYP = angantvar.kalkulation.angAntKalkDto.auszahlungTyp;
                        }

                        if (isLeaseNow == true)
                        {
                            angkalk.BGEXTERNUST = (decimal?)(angantvar.kalkulation.angAntKalkDto.bgexternbrutto - angantvar.kalkulation.angAntKalkDto.bgextern);
                            angkalk.BGINTERNUST = (decimal?)(angantvar.kalkulation.angAntKalkDto.bginternbrutto - angantvar.kalkulation.angAntKalkDto.bgintern);
                            angkalk.RATEUST = (decimal?)angantvar.kalkulation.angAntKalkDto.rateUst;
                            angkalk.AUSZAHLUNG = angkalk.BGEXTERNUST;
                        }

                        // Ticket#2012013110000051 — Defect #6104 - B2B Antrag: Anzeige Maximal- & Minimal-Kosten
                        angkalk.RAPZINSKOSTENMIN = (decimal?)(angantvar.kalkulation.angAntKalkDto.calcZinskostenMin);
                        angkalk.RAPZINSKOSTENMAX = (decimal?)(angantvar.kalkulation.angAntKalkDto.calcZinskostenMax);

                        angkalk.RAPRSVMONATMIN = (decimal?)(angantvar.kalkulation.angAntKalkDto.calcRsvmonatMin);
                        angkalk.RAPRSVMONATMAX = (decimal?)(angantvar.kalkulation.angAntKalkDto.calcRsvmonatMax);
                    }
                    context.SaveChanges();
                    _log.Debug("createAngebot F: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));
                    start = (long)(DateTime.Now.TimeOfDay.TotalMilliseconds);
                    //provisions--------------------------------------------
                    foreach (AngAntProvDto prov in angantvar.kalkulation.angAntProvDto)
                    {
                        ANGPROV angprov = new ANGPROV();
                        mapper.Map<AngAntProvDto, ANGPROV>(prov, angprov);
                        context.ANGPROV.Add(angprov);

                        
                        angprov.SYSANGVAR = angvar.SYSANGVAR;
                        angprov.SYSVT = angvar.SYSANGVAR;

                        angprov.PRPROVTYPE = context.PRPROVTYPE.Where(par => par.SYSPRPROVTYPE == prov.sysprprovtype).FirstOrDefault();
                        angprov.SYSPARTNER = prov.syspartner;
                        //angprov.PARTNER = context.PARTNER.Where(par => par.SYSPARTNER == prov.syspartner).FirstOrDefault();
                        angprov.PROVISION = (decimal?)prov.provision;
                        angprov.PROVISIONP = (decimal?)prov.provisionPro;
                        angprov.PROVISIONBRUTTO = (decimal?)prov.provisionBrutto;

                        angprov.DEFPROVISION = (decimal?)prov.defaultprovision;
                        angprov.DEFPROVISIONBRUTTO = (decimal?)prov.defaultprovisionbrutto;
                        angprov.DEFPROVISIONP = (decimal?)prov.defaultprovisionp;
                        angprov.DEFPROVISIONUST = (decimal?)prov.defaultprovisionust;

                        int index = angantvar.kalkulation.angAntProvDto.IndexOf(prov);
                        if (angantvar.kalkulation.angAntProvDtoRapMin != null && angantvar.kalkulation.angAntProvDtoRapMin.Count() > index)
                        {
                            // Provision für 5% Zinsen
                            angprov.RAPPROVISIONBRUTTOMIN = (decimal?)angantvar.kalkulation.angAntProvDtoRapMin[index].provisionBrutto;
                        }
                        if (angantvar.kalkulation.angAntProvDtoRapMax != null && angantvar.kalkulation.angAntProvDtoRapMax.Count() > index)
                        {
                            // Provision für 15% Zinsen
                            angprov.RAPPROVISIONBRUTTOMAX = (decimal?)angantvar.kalkulation.angAntProvDtoRapMax[index].provisionBrutto;
                        }

                        angprov.FLAGLOCKED = prov.flaglocked;
                        context.SaveChanges();
                        _log.Debug("createAngebot G: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));
                        start = (long)(DateTime.Now.TimeOfDay.TotalMilliseconds);
                    }
                    context.SaveChanges();
                    _log.Debug("createAngebot H: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));
                    start = (long)(DateTime.Now.TimeOfDay.TotalMilliseconds);
                    //insurances--------------------------------------------
                    foreach (AngAntVsDto vs in angantvar.kalkulation.angAntVsDto)
                    {
                        ANGVS angvs = new ANGVS();
                        mapper.Map<AngAntVsDto, ANGVS>(vs, angvs);
                        context.ANGVS.Add(angvs);

                        
                        angvs.SYSVSTYP= vs.sysvstyp;
                        angvs.SYSANGEBOT = angebotOutput.SYSID;
                        angvs.SYSANGVAR = angvar.SYSANGVAR;

                        long? sysVs = (from vstyp in context.VSTYP
                                       where vstyp.SYSVSTYP == vs.sysvstyp
                                       select vstyp.SYSVS).FirstOrDefault();
                        if (sysVs != null && sysVs.HasValue)
                        {
                            angvs.SYSVS = sysVs;
                        }
                        context.SaveChanges();
                    }
                    context.SaveChanges();
                    _log.Debug("createAngebot I: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));
                    start = (long)(DateTime.Now.TimeOfDay.TotalMilliseconds);
                    //subventions--------------------------------------------
                    foreach (AngAntSubvDto subv in angantvar.kalkulation.angAntSubvDto)
                    {
                        ANGSUBV angsub = new ANGSUBV();
                        mapper.Map<AngAntSubvDto, ANGSUBV>(subv, angsub);
                        context.ANGSUBV.Add(angsub);

                        angsub.SYSANGVAR = angvar.SYSANGVAR;

                        if (isDiffLeasing == true)
                        {
                            angsub.SUBVTYP = context.SUBVTYP.Where(par => par.SYSSUBVTYP == subv.syssubvtyp).FirstOrDefault();
                            angsub.SYSSUBVG = subv.syssubvg;
                        }
                        context.SaveChanges();
                    }
                    context.SaveChanges();
                    _log.Debug("createAngebot J: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));
                    start = (long)(DateTime.Now.TimeOfDay.TotalMilliseconds);
                    // Ablösen ---------------------------------------------
                    //Update/Insert changed ones
                    if (angantvar.kalkulation.angAntAblDto!=null)
                    {
                        foreach (AngAntAblDto abl in angantvar.kalkulation.angAntAblDto)
                        {
                            ANGABL angabl = new ANGABL();
                            context.ANGABL.Add(angabl);

                            if (String.IsNullOrEmpty(abl.bank))
                            {
                                abl.bank = EIGENEBANK;
                                abl.sysabltyp = ABLTYPEIGEN;
                            }
                            if (abl.sysabltyp == 0 && abl.bank.Equals(EIGENEBANK))
                            {
                                abl.sysabltyp = ABLTYPEIGEN;
                            }
                            if (!abl.bank.Equals(EIGENEBANK))
                            {
                                abl.sysabltyp = 2;
                            }
                            mapper.Map<AngAntAblDto, ANGABL>(abl, angabl);

                            context.SaveChanges();
                            angabl.SYSANGEBOT = angebotOutput.SYSID;
                            angabl.SYSABLTYP=abl.sysabltyp;
                        }
                    }
                    _log.Debug("createAngebot K: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));
                    start = (long)(DateTime.Now.TimeOfDay.TotalMilliseconds);
                    setCasaDiplomaSpecialFields(angebotInput, "ANGEBOT", angebotOutput.SYSID,vttypCode);
                    createOrUpdateCard(angebotOutput.SYSID, angebotInput.emboss, isCard,0);
                }

                if (angebotInput.kunde != null)
                {
                    ITRELATE itRelate = new ITRELATE();
                    context.ITRELATE.Add(itRelate);
                    //itRelate.ANGEBOT = angebotOutput;
                    itRelate.SYSANGEBOT = angebotOutput.SYSID;
                    itRelate.SYSIT1 = angebotInput.kunde.sysit;
                }
                _log.Debug("createAngebot L: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));
                start = (long)(DateTime.Now.TimeOfDay.TotalMilliseconds);
                if (angebotInput.mitantragsteller != null)
                {
                    ANGOBSICH NewANGOBSICH = new ANGOBSICH();
                    context.ANGOBSICH.Add(NewANGOBSICH);

                    NewANGOBSICH.SYSIT=angebotInput.mitantragsteller.sysit;
                    NewANGOBSICH.SYSANGEBOT = angebotOutput.SYSID;
                    if (angebotInput.mitantragsteller.syskd > 0)
                        NewANGOBSICH.SYSPERSON = angebotInput.mitantragsteller.syskd;
                    SetAngebotIdInZusatzdaten(context, angebotInput.mitantragsteller, angebotOutput.SYSID);
                    // Partner(800)/Solidarschuldner(120)/Bürge(130)
                    int crang = (int)MitantragstellerTyp.Partner;
                    if (angebotInput.mitantragsteller.zusatzdaten != null && angebotInput.mitantragsteller.zusatzdaten.Length > 0 && angebotInput.mitantragsteller.zusatzdaten[0].pkz != null)
                        if (angebotInput.mitantragsteller.zusatzdaten[0].pkz != null && angebotInput.mitantragsteller.zusatzdaten[0].pkz.Length > 0)
                            if (angebotInput.mitantragsteller.zusatzdaten[0].pkz[0].ehepartnerFlag > 1)
                                crang = angebotInput.mitantragsteller.zusatzdaten[0].pkz[0].ehepartnerFlag;
                    
                    //allow sichtyp control from outside
                    if (angebotInput.mitantragsteller.sichtyprang > 0)
                        crang = angebotInput.mitantragsteller.sichtyprang;

                    NewANGOBSICH.SICHTYP = (from typ in context.SICHTYP
                                            where typ.RANG == crang
                                            select typ).FirstOrDefault();

                    SetAngebotIdInZusatzdaten(context, angebotInput.mitantragsteller, angebotOutput.SYSID);
                }

				// syscamp (KampagnenCode)
                if (angebotInput.syscamp >= 0)
                    angebotOutput.SYSCAMP= angebotInput.syscamp;

                angebotOutput.SYSCAMPTP = angebotInput.syscamptp;
                context.SaveChanges();
                
                _log.Debug("createAngebot M: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));
                start = (long)(DateTime.Now.TimeOfDay.TotalMilliseconds);

                PEUNIHelper.ConnectNodes(context, PEUNIArea.ANGEBOT, angebotOutput.SYSID, sysperole);
                context.SaveChanges();

                if (angob != null)
                    updateAngAngObAust(angebotInput.angAntObDto.aust, angob.SYSOB);

                if (angebotInput.bemerkung != null && angebotInput.bemerkung.Length > 0)
                    if (angebotInput.erfassungsclient == ERFASSUNGSCLIENT_ONE)
                        createOrUpdateMemo(angebotOutput.SYSID, angebotInput.bemerkung, "ANGEBOT", "Bemerkung", "Allgemein", angebotOutput.SYSVK);

                    else
                        createOrUpdateMemo(angebotOutput.SYSID, angebotInput.bemerkung, "ANGEBOT", KURZTXTNOTIZ, "Allgemein", angebotOutput.SYSVK);

                _log.Debug("createAngebot N: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));
                start = (long)(DateTime.Now.TimeOfDay.TotalMilliseconds);
                

                return getAngebot(angebotOutput.SYSID);
            }
        }

        /// <summary>
        /// Get Offer from DB
        /// </summary>
        /// <param name="sysid">System ID</param>
        /// <returns>Offer Data</returns>
        public AngebotDto getAngebot(long sysid)
        {
            long start = (long)(DateTime.Now.TimeOfDay.TotalMilliseconds);

            IMapper mapper = Mapper.Instance;
            /*Cic.One.Utils.Util.Mapper.MapperConfig.getInstanceMapper("getAngebot", delegate (MapperConfigurationExpression cfg)
            {

                // HotFix #5940 : Mapping der calc-Felder 
                cfg.CreateMap<ANGKALK, AngAntKalkDto>()
                    .ForMember(dest => dest.calcRsvgesamt, opt => opt.MapFrom(src => src.RSVGESAMT))
                    .ForMember(dest => dest.calcZinskosten, opt => opt.MapFrom(src => src.ZINSKOSTEN))
                    .ForMember(dest => dest.calcRsvmonat, opt => opt.MapFrom(src => src.RSVMONAT))
                    .ForMember(dest => dest.calcRsvzins, opt => opt.MapFrom(src => src.RSVZINS))
                    .ForMember(dest => dest.calcUstzins, opt => opt.MapFrom(src => src.USTZINS));

            });*/
        

            using (DdOlExtended context = new DdOlExtended())
            {
                DbConnection con = (context.Database.Connection);
             

                _log.Debug("getAngebot A: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));
                start = (long)(DateTime.Now.TimeOfDay.TotalMilliseconds);
                AngebotDto rval = con.Query<AngebotDto>("select * from angebot where sysid=:sysid", new { sysid = sysid }).FirstOrDefault();
                if(rval==null)
                    throw new ArgumentException("Angebot does not exist: sysId = " + sysid);

                if (rval.erfassungsclient != ERFASSUNGSCLIENT_B2B && rval.erfassungsclient != ERFASSUNGSCLIENT_B2C && rval.erfassungsclient != ERFASSUNGSCLIENT_ONE && rval.erfassungsclient != ERFASSUNGSCLIENT_DMR)
                {
                    
                }
                else
                {
                    rval.sysbrand = 0;
                    rval.syswfuser = null;
                }
             
                //AngAntObDto
                if (rval.angAntObDto == null)
                {
                  
                    rval.angAntObDto = con.Query<AngAntObDto>("select * from angob where sysangebot=:sysid", new { sysid = sysid }).FirstOrDefault();
                       
                    if (rval.angAntObDto != null)
                    {
                      
                        rval.angAntObDto.brief = con.Query<AngAntObBriefDto>("select * from angobbrief where sysangobbrief=:sysid", new { sysid = sysid }).FirstOrDefault();
                      
                    }
                }
                _log.Debug("getAngebot B: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));
                start = (long)(DateTime.Now.TimeOfDay.TotalMilliseconds);
              
                rval.options = con.Query<AngAntOptionDto>("select * from angoption where sysid=:sysid", new { sysid = sysid }).FirstOrDefault();
                if (rval.options == null)
                {
                    rval.options = new AngAntOptionDto();
                }
                
                rval.angAntVars = con.Query<AngAntVarDto>("select * from angvar where sysangebot=:sysid", new { sysid = sysid }).ToList();

                _log.Debug("getAngebot C: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));
                start = (long)(DateTime.Now.TimeOfDay.TotalMilliseconds);
                if(rval.angAntVars==null)
                    rval.angAntVars = new List<AngAntVarDto>();
              
                foreach (AngAntVarDto angantvar in rval.angAntVars)
                {
                   
                    angantvar.kalkulation = new KalkulationDto();

                    //Calculation
                    /*ANGKALK angkalk = (from akalk in context.ANGKALK
                                       where akalk.ANGVAR.SYSANGVAR == angantvar.sysangvar
                                       select akalk).FirstOrDefault();
                    */
                    angantvar.kalkulation.angAntKalkDto = con.Query<AngAntKalkDto>("select angkalk.*, rsvgesamt calcrsvgesamt, zinskosten calczinskosten, rsvmonat calcrsvmona, rsvzins calcrsvzins, ustzins calcustzins, RAPZINSKOSTENMIN calczinskostenmin, RAPZINSKOSTENMAX calczinskostenmax, RAPRSVMONATMIN calcrsvmonatmin, RAPRSVMONATMAX calcrsvmonatmax  from angkalk, angvar where angvar.sysangvar = angkalk.sysangvar and angvar.sysangvar =:sysangvar", new { sysangvar = angantvar.sysangvar }).FirstOrDefault(); //mapper.Map<ANGKALK, AngAntKalkDto>(angkalk);
                    if (angantvar.kalkulation.angAntKalkDto != null)
                    {
                        _log.Debug("getAngebot C1: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));
                        // angkalk.PRPRODUCT ist hier null
                        List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syskalk", Value = angantvar.kalkulation.angAntKalkDto.syskalk });

                        AngebotData Data = context.ExecuteStoreQuery<AngebotData>(GETANGEBOTDATA, parameters.ToArray()).FirstOrDefault();
                        _log.Debug("getAngebot C2: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));
                        angantvar.kalkulation.angAntKalkDto.sysprproduct = Data.sysprproduct;

                        // Ticket#2012090510000531 : Felder bei CopyAngebotByID
                       /* if (rval.erfassungsclient != ERFASSUNGSCLIENT_B2B && rval.erfassungsclient != ERFASSUNGSCLIENT_B2C && rval.erfassungsclient != ERFASSUNGSCLIENT_ONE && rval.erfassungsclient != ERFASSUNGSCLIENT_DMR)
                        {
                            angantvar.kalkulation.angAntKalkDto.syscreate = angkalk.SYSCREATE;
                            angantvar.kalkulation.angAntKalkDto.syschange = angkalk.SYSCHANGE;
                        }*/

                        // Produkt-Bezeichnung
                        String prodName = null;
                        if (Data.sysprproduct > 0)
                            prodName = context.ExecuteStoreQuery<String>("select name from prproduct where sysprproduct="+ Data.sysprproduct).FirstOrDefault();
                        _log.Debug("getAngebot C3: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));
                        if (prodName != null)
                        {
                            angantvar.kalkulation.angAntKalkDto.prProductBezeichnung = prodName;
                            angantvar.PrProductCode = MyGetPrProductCode(Data.sysprproduct);
                        }

                        // angkalk.OBUSETYPE ist hier auch null
                        angantvar.kalkulation.angAntKalkDto.sysobusetype = Data.SYSOBUSETYPE;

                        angantvar.kalkulation.angAntKalkDto.sysangvar = angantvar.sysangvar;
                    }

                    //Provision
                    angantvar.kalkulation.angAntProvDto = con.Query<AngAntProvDto>("select * from angprov where sysvt=:sysangvar", new { sysangvar = angantvar.sysangvar }).ToList();

                 

                    //Subvention
                    angantvar.kalkulation.angAntSubvDto = con.Query<AngAntSubvDto>("select * from angsubv where sysangvar=:sysangvar", new { sysangvar = angantvar.sysangvar }).ToList();

                    _log.Debug("getAngebot C4: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));
                    //Insurances
                    angantvar.kalkulation.angAntVsDto = con.Query<AngAntVsDto>("select angvs.*,vstyp.bezeichnung vstypbezeichnung,vsart.code serviceType from angvs,vstyp,vsart where vsart.sysvsart=vstyp.sysvsart and angvs.sysvstyp=vstyp.sysvstyp and sysangvar=:sysangvar", new { sysangvar = angantvar.sysangvar }).ToList();
                    if (angantvar.kalkulation.angAntVsDto == null)
                        angantvar.kalkulation.angAntVsDto = new List<AngAntVsDto>();


                    if (angantvar.kalkulation.angAntVsDto != null && angantvar.kalkulation.angAntVsDto.Count > 0)
                    {
                        OpenOne.Common.BO.Versicherung.IInsuranceBo insbo = Cic.OpenOne.GateBANKNOW.Common.BO.BOFactory.getInstance().createInsuranceBO();
                        foreach (AngAntVsDto avs in angantvar.kalkulation.angAntVsDto)
                        {
                            insbo.updateServiceType(avs);
                        }
                    }

                    // Ablösen (B2C)---------------------------------------------
                    angantvar.kalkulation.angAntAblDto = con.Query<AngAntAblDto>("select * from angabl where sysangebot=:sysid", new { sysid = sysid }).ToList();

                    foreach (AngAntAblDto angAblDto in angantvar.kalkulation.angAntAblDto)
                    {
                        
                        // Ticket#2012081310000214 Kein Bankname wird in den Details angezeigt
                        if (String.IsNullOrEmpty(angAblDto.bank) && angAblDto.sysabltyp == ABLTYPEIGEN)
                        {
                            angAblDto.bank = EIGENEBANK;
                        }

                    }

                }
                _log.Debug("getAngebot D: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));
                start = (long)(DateTime.Now.TimeOfDay.TotalMilliseconds);
                long sysit1 = context.ExecuteStoreQuery<long>("select sysit1 from itrelate where sysangebot=" + sysid, null).FirstOrDefault();
                if (sysit1>0)
                {
                    IKundeDao kundeDao = CommonDaoFactory.getInstance().getKundeDao();
                    rval.kunde = kundeDao.getKunde((long)sysit1,sysid);

                }
                _log.Debug("getAngebot E: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));
                start = (long)(DateTime.Now.TimeOfDay.TotalMilliseconds);
                
                
                AngAntSichDto sichinfo= context.ExecuteStoreQuery<AngAntSichDto>(
                    "select sysit,sichtyp.rang syssichtyp from angobsich,sichtyp where sichtyp.syssichtyp=angobsich.syssichtyp and sysangebot=" +
                    sysid, null).FirstOrDefault();
             
                if (sichinfo != null && sichinfo.sysit>0)
                {
                    IKundeDao kundeDao = CommonDaoFactory.getInstance().getKundeDao();
                    rval.mitantragsteller = kundeDao.getKunde((long)sichinfo.sysit,sysid);
                    rval.mitantragsteller.sichtyprang = (int)sichinfo.syssichtyp;//mapped in query to rang

                }
                _log.Debug("getAngebot F: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));
                start = (long)(DateTime.Now.TimeOfDay.TotalMilliseconds);
                rval.bemerkung = MyGetMemo("Allgemein", sysid, "ANGEBOT");
                _log.Debug("getAngebot G: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));
                start = (long)(DateTime.Now.TimeOfDay.TotalMilliseconds);
                getCasaDiplomaSpecialFields(rval, "ANGEBOT", sysid);
                rval.emboss = context.ExecuteStoreQuery<String>("select emboss from card where sysangebot=" + sysid, null).FirstOrDefault();
                _log.Debug("getAngebot H: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));
                start = (long)(DateTime.Now.TimeOfDay.TotalMilliseconds);
                //Abwicklungsort / Filiale
                if (rval.sysAbwicklung > 0)
                {
                     rval.abwicklungsort = context.ExecuteStoreQuery<String>("select name from perole where sysperole="+rval.sysAbwicklung,null).FirstOrDefault();
                    
                }

                //// Eingangskanal from Kampagnencode (rh: 20170310) --> rh: 20170324: nun aus edmx verfügbar!
                //if (rval.syscamp > 0)
                //{
                //	rval.syscamptp = context.ExecuteStoreQuery<long> ("SELECT sysCampTP FROM camp WHERE sysCamp = " + rval.syscamp, null).FirstOrDefault ();
                //}
                rval.extident = context.ExecuteStoreQuery<String>("select extidentvalue from extident where codeextidenttyp='"+CODE_EXTIDENT_ANTRAGSID_B2C+"' and area='ANGEBOT' and sysarea="+sysid+" order by case when source='30' then 100 else to_number(source) end").FirstOrDefault();
                _log.Debug("getAngebot I: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));
                start = (long)(DateTime.Now.TimeOfDay.TotalMilliseconds);
                return rval;
            }
        }

        /// <summary>
        /// Angebot aktualisieren
        /// </summary>
        /// <param name="angebotInput">Angebot Eingang</param>
        /// <param name="sysperole">sysperole</param>
        /// <returns>Angebot Ausgang</returns>
        public AngebotDto updateAngebot(AngebotDto angebotInput, long sysperole)
        {
            using (DdOlExtended context = new DdOlExtended())
            {
                IMapper mapper = Mapper.Instance;/* Cic.One.Utils.Util.Mapper.MapperConfig.getInstanceMapper("updateAngebot", delegate (MapperConfigurationExpression cfg)
                {

                    cfg.CreateMap<AngAntVarDto, ANGVAR>();

                    cfg.CreateMap<AngAntProvDto, ANGPROV>();
                    cfg.CreateMap<AngAntSubvDto, ANGSUBV>();
                    cfg.CreateMap<ANGOBBRIEF, AngAntObBriefDto>();
                });*/

                ANGEBOT angebotOutput = (from ang in context.ANGEBOT
                                         where ang.SYSID == angebotInput.sysid
                                         select ang).FirstOrDefault();

                angebotInput.erfassung = angebotOutput.ERFASSUNG;
                
                //use kunde id if given 
                if (angebotInput.kunde != null)
                    angebotInput.sysit = angebotInput.kunde.sysit;
                angebotInput.aenderung = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null);
                if (angebotInput.zustand != angebotOutput.ZUSTAND)
                {
                    angebotInput.zustandAm = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null);
                }

                mapper.Map<AngebotDto, ANGEBOT>(angebotInput, angebotOutput);

                // ERFASSUNGSCLIENT soll beim Angebot wenn b2b True ist, auf 10 gesetzt werden.
                if (isb2b && angebotInput.erfassungsclient != ERFASSUNGSCLIENT_B2C && angebotInput.erfassungsclient != ERFASSUNGSCLIENT_ONE && angebotInput.erfassungsclient != ERFASSUNGSCLIENT_DMR)
                    angebotOutput.ERFASSUNGSCLIENT = ERFASSUNGSCLIENT_B2B;

                // Neue Felder--------------------------------------------
                // antragOutput.SYSVK (Verkäufer) = PERSON von sysperole
                angebotOutput.SYSVK = (from perole in context.PEROLE
                                       where perole.SYSPEROLE == sysperole
                                       select perole.SYSPERSON).FirstOrDefault();
                //Webfrontend übernimmt immer den VM von aussen bei Fahrzeugfin (weil in Maske)
                if (angebotInput.erfassungsclient == ERFASSUNGSCLIENT_DMR || (angebotInput.erfassungsclient == ERFASSUNGSCLIENT_ONE && angebotInput.vertriebsweg != null && angebotInput.vertriebsweg.ToUpper().IndexOf("FAHRZ") > -1))
                {
                    angebotOutput.SYSVM = angebotInput.sysVM;
                    //if(angebotInput.sysVK>0)//in GUI kein Pflichtfeld, aber für Desktop notwendig!
                    angebotOutput.SYSVK = angebotInput.sysVK;
                }
                else { 
                    //antragOutput.SYSVM (Vermittler) = Person von Händler (Parent vom Verkäufer)
                    angebotOutput.SYSVM = (from parentPeRole in context.PEROLE
                                           join perole in context.PEROLE on parentPeRole.SYSPARENT equals perole.SYSPEROLE
                                           where parentPeRole.SYSPEROLE == sysperole
                                           select perole.SYSPERSON).FirstOrDefault();
                }
                angebotOutput.SYSPRHGROUP = 0;
                try
                {
                    // Handelsgruppe wird in solveKalkulation berechnet 
                    // sie ist für jede Variante gleich, die Handelsgruppe ändert sich nicht
                    angebotOutput.SYSPRHGROUP = angebotInput.angAntVars[0].kalkulation.angAntKalkDto.sysprhgrp;
                }
                catch (Exception) { }

                // Brand wird anhand der Handelsgruppe ermittelt
                angebotOutput.SYSBRAND = (from prbrandm in context.PRBRANDM
                                       join brand in context.BRAND on prbrandm.BRAND.SYSBRAND equals brand.SYSBRAND
                                       where prbrandm.PRHGROUP.SYSPRHGROUP == angebotOutput.SYSPRHGROUP
                                       select prbrandm.SYSBRAND).FirstOrDefault();
                
                if (angebotOutput.ERFASSUNGSCLIENT == ERFASSUNGSCLIENT_B2C)
                {
                    angebotOutput.SYSABWICKLUNG = getAbwicklungsortB2C(angebotInput,eaiHotDao);
                }
                else if (angebotOutput.ERFASSUNGSCLIENT == ERFASSUNGSCLIENT_ONE||angebotOutput.ERFASSUNGSCLIENT == ERFASSUNGSCLIENT_DMR)
                {
                    angebotOutput.SYSABWICKLUNG = MyGetHaendlerAbwicklungsortOne(angebotInput.sysid);
                }
                else if (angebotOutput.ERFASSUNGSCLIENT != ERFASSUNGSCLIENT_B2C)// AbwicklungsOrt (PeRole) ist dem Händler zugeordnet
                {
                    angebotOutput.SYSABWICKLUNG = MyGetHaendlerAbwicklungsort(angebotOutput.SYSVM);
                }


                if ((angebotInput.erfassungsclient == ERFASSUNGSCLIENT_ONE || angebotInput.erfassungsclient == ERFASSUNGSCLIENT_DMR) && angebotInput.syskd != null && angebotInput.syskd.HasValue && angebotInput.syskd.Value > 0)
                    angebotOutput.SYSKD=angebotInput.syskd.Value;

                // das Feld sysls muss gefüllt werden. Hier muss immer der Wert 1 stehen.
                angebotOutput.SYSLS = 1;

                // MwSt
                angebotOutput.SYSMWST = context.ExecuteStoreQuery<long>("select sysmwst from lsadd where syslsadd=" + angebotOutput.SYSLS).FirstOrDefault();

                if (!String.IsNullOrEmpty(angebotInput.extident))
                {
                    ExtidentDto exti = new ExtidentDto();
                    exti.area = "ANGEBOT";
                    exti.extidentvalue = angebotInput.extident;
                    exti.sysarea = angebotOutput.SYSID;
                    exti.source = "" + angebotOutput.ERFASSUNGSCLIENT;
                    exti.codeextidenttyp = CODE_EXTIDENT_ANTRAGSID_B2C;
                    createOrUpdateExtident(exti);
                }

                SetAngebotIdInZusatzdaten(context, angebotInput.kunde, angebotOutput.SYSID);

                createOrUpdateKREMOAngebot(angebotOutput.SYSID, angebotInput.kunde, angebotInput.mitantragsteller);
                //Object
                ANGOB angob = null;
                bool deleteObaust = false;
                if (angebotInput.angAntObDto != null)
                {
                    fixAHK(angebotInput.angAntObDto, angebotInput.sysprchannel);
                    context.SaveChanges();
                    if(!context.Entry(angebotOutput).Collection(f => f.ANGOBList).IsLoaded)
                        context.Entry(angebotOutput).Collection(f => f.ANGOBList).Load();
                    
                    angob = angebotOutput.ANGOBList.FirstOrDefault();

                    if (angob != null && angob.OBTYP != null && angob.OBTYP.SYSOBTYP != angebotInput.angAntObDto.sysobtyp)
                        deleteObaust = true;

                    if (angob == null)
                    {
                        angob = new ANGOB();
                        context.ANGOB.Add(angob);
                    }
                    else
                    {
                        if (angob.OBTYP == null)
                            context.Entry(angob).Reference(f => f.OBTYP).Load();
                    }
                    

                    
                    mapper.Map<AngAntObDto, ANGOB>(angebotInput.angAntObDto, angob);
                    //angob.ANGEBOT = angebotOutput;
                    angob.SYSANGEBOT = angebotOutput.SYSID;
                    angob.OBJEKT = NkBuilderFactory.createAngobNkBuilder().getNextNumber();
                    angob.OBART = context.OBART.Where(par => par.SYSOBART == angebotInput.angAntObDto.sysobart).FirstOrDefault();
                    angob.OBTYP = context.OBTYP.Where(par => par.SYSOBTYP == angebotInput.angAntObDto.sysobtyp).FirstOrDefault();

                    //Brief
                    if (angebotInput.angAntObDto.brief != null)//brief update
                    {
                        if (angob.ANGOBBRIEF == null)
                            context.Entry(angob).Reference(f => f.ANGOBBRIEF).Load();
                        
                        ANGOBBRIEF brief = angob.ANGOBBRIEF;
                        if (brief == null)
                        {
                            brief = new ANGOBBRIEF();
                            context.ANGOBBRIEF.Add(brief);
                            angob.ANGOBBRIEF = brief;
                        }
                        mapper.Map<AngAntObBriefDto, ANGOBBRIEF>(angebotInput.angAntObDto.brief, brief);
                    }
                }

                //Pattern to update n attached Entities-------------------------------------------------

                //get current Ids
                List<long> curAngVarIds = (from inpAngVar in angebotInput.angAntVars
                                           where inpAngVar.sysangvar != 0
                                           select inpAngVar.sysangvar).ToList();

                //get entities that are no more in current ids
                List<ANGVAR> deleteAngVars = (from delAngVar in context.ANGVAR
                                              where !curAngVarIds.Contains(delAngVar.SYSANGVAR) && delAngVar.ANGEBOT.SYSID == angebotOutput.SYSID
                                              select delAngVar).ToList();

                // delete old AngVariante...
                foreach (ANGVAR angVarToDel in deleteAngVars)
                {
                    // ...aber zuerst den AngKalk-Satz, der dazu gehört
                    // für eine AngVar kann nur ein AngKalk existieren (auch wenn es in der edmx anders aussieht)
                    // Ticket#2012062510000053 :Hinzufügen einer neuen Variante erzeugt doppelte AngKalk-Sätze
                    context.SaveChanges();
                    context.Entry(angVarToDel).Collection(f => f.ANGKALKList).Load();
                    List<ANGKALK> angKalkList = angVarToDel.ANGKALKList.ToList();
                    foreach (var angKalkToDel in angKalkList)
                        context.DeleteObject(angKalkToDel);

                    context.DeleteObject(angVarToDel);
                }

                IPrismaDao prismaDao = PrismaDaoFactory.getInstance().getPrismaDao();

                // Variants
                foreach (AngAntVarDto angantvar in angebotInput.angAntVars)
                {
                    ANGVAR angvar = (from a in context.ANGVAR
                                     where a.SYSANGVAR == angantvar.sysangvar
                                     select a).FirstOrDefault();
                    if (angvar == null)     //new angvar
                    {
                        angvar = new ANGVAR();
                        //angvar.ANGEBOT = angebotOutput;
                        context.ANGVAR.Add(angvar);
                        angvar.SYSANGEBOT = angebotOutput.SYSID;
                        context.SaveChanges();//for angvar.SYSANGVAR!
                    }

                    long sysangvar = angvar.SYSANGVAR;
                    //update Fields
                    mapper.Map<AngAntVarDto, ANGVAR>(angantvar, angvar);

                    //angvar.ANGEBOT = angebotOutput;
                    angvar.SYSANGEBOT = angebotOutput.SYSID;
                    angvar.SYSANGVAR = sysangvar;

                    if (angantvar.kalkulation == null) continue;

                    bool isLeaseNow = false;
                    bool isDiffLeasing = false;
                    bool isDispo = false;
                    bool isCard = false;
                    bool isClassic = false;
                    String vttypCode = null;
                    try
                    {
                        if (angantvar.kalkulation.angAntKalkDto != null)
                        {
                            //Produkte werden anhand vart unterschieden: es gibt KREDIT, TZK, LEASE_now, CREDIT-now Dispo, DifferenzLeasing
                            CIC.Database.PRISMA.EF6.Model.VART vart = prismaDao.getVertragsart((long)angantvar.kalkulation.angAntKalkDto.sysprproduct);
							CIC.Database.PRISMA.EF6.Model.VTTYP vtt = prismaDao.getVttyp((long)angantvar.kalkulation.angAntKalkDto.sysprproduct);
                            if (vtt != null)
                            {
                                vttypCode = vtt.CODE;
                                if ((angvar.INANTRAG.HasValue && angvar.INANTRAG.Value > 0) || angebotInput.angAntVars.Count == 1)
                                {
                                    angebotOutput.SYSVTTYP = vtt.SYSVTTYP;
                                }
                            }
                            else if ((angvar.INANTRAG.HasValue && angvar.INANTRAG.Value > 0) || angebotInput.angAntVars.Count == 1)
                                angebotOutput.SYSVTTYP = null;

                            if ((angvar.INANTRAG.HasValue && angvar.INANTRAG.Value > 0) || angebotInput.angAntVars.Count == 1)
                            {
                                if (vart != null)
                                    angebotOutput.SYSVART = vart.SYSVART;
                            }
                            // VertragsArt ermitteln
                            isDiffLeasing = prismaDao.isDiffLeasing((long)angantvar.kalkulation.angAntKalkDto.sysprproduct);
                            if (vart != null)
                            {
                                String vArtCode = vart.CODE.ToLower();
                                isLeaseNow = (vArtCode.IndexOf("leasing") > -1);                                // LEASE-now
                                isDispo = (vArtCode.IndexOf("dispo") > -1) || (vArtCode.IndexOf("flex") > -1);  // CREDIT_now Dispo
                                isClassic = (vArtCode.IndexOf("classic") > -1);                             // CREDIT_now Classic
                                isCard = (vArtCode.IndexOf("kredit_dispoplus") > -1);                           // CREDIT_now Card
                            }
                        }

					}
					catch (Exception ea)
                    {
                        _log.Warn("Error writing back SYSPRPRODUCT/SYSVART to ANGEBOT", ea);
                    }

                    //Calculation
                    ANGKALK angkalk = (from a in context.ANGKALK
                                       where a.ANGVAR.SYSANGVAR == angvar.SYSANGVAR
                                       select a).FirstOrDefault();
                    if (angkalk == null)
                    {
                        angkalk = new ANGKALK();
                        
                        context.ANGKALK.Add(angkalk);
                    }
                    
                    
                    mapper.Map<AngAntKalkDto, ANGKALK>(angantvar.kalkulation.angAntKalkDto, angkalk);
                    angkalk.SYSANGVAR = angvar.SYSANGVAR;
                    angkalk.SYSANGEBOT = angebotOutput.SYSID;

                    if ((angvar.INANTRAG.HasValue && angvar.INANTRAG.Value > 0) || angebotInput.angAntVars.Count==1)
                    {
                        angebotOutput.SYSPRPRODUCT = angantvar.kalkulation.angAntKalkDto.sysprproduct;
                    }

                    angkalk.SYSPRPRODUCT =angantvar.kalkulation.angAntKalkDto.sysprproduct;
                    angkalk.OBUSETYPE = context.OBUSETYPE.Where(par => par.SYSOBUSETYPE == angantvar.kalkulation.angAntKalkDto.sysobusetype).FirstOrDefault();

                    
                    angkalk.SYSWAEHRUNG = context.ExecuteStoreQuery<long>("select syshauswaehrung from lsadd where syslsadd=1", null).FirstOrDefault(); 

                    if (isDispo == true)
                    {
                        List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysprproduct", Value = angantvar.kalkulation.angAntKalkDto.sysprproduct });

                        angkalk.SYSINTSTRCT = context.ExecuteStoreQuery<long>(SYSINTSTRCTBYPRODUCTID, parameters.ToArray()).FirstOrDefault();
                    }

                    // Modus muss bei kredit 0, bei leasing 1 sein
                    if (isLeaseNow == true) { angkalk.MODUS = 1; } else { angkalk.MODUS = 0; }

                    // PPY (Raten pro Jahr): Konstant 12 bei bnow
                    angkalk.PPY = 12;
                    if (isClassic == true)
                    {
                        angkalk.AUSZAHLUNG = (decimal?)angantvar.kalkulation.angAntKalkDto.auszahlung;
                        angkalk.AUSZAHLUNGTYP = angantvar.kalkulation.angAntKalkDto.auszahlungTyp;
                    }

                    if (isLeaseNow == true)
                    {
                        angkalk.BGEXTERNUST = (decimal?)(angantvar.kalkulation.angAntKalkDto.bgexternbrutto - angantvar.kalkulation.angAntKalkDto.bgextern);
                        angkalk.BGINTERNUST = (decimal?)(angantvar.kalkulation.angAntKalkDto.bginternbrutto - angantvar.kalkulation.angAntKalkDto.bgintern);
                        angkalk.RATEUST = (decimal?)angantvar.kalkulation.angAntKalkDto.rateUst;
                        angkalk.AUSZAHLUNG = angkalk.BGEXTERNUST;
                    }

                    // Ticket#2012013110000051 — Defect #6104 - B2B Antrag: Anzeige Maximal- & Minimal-Kosten
                    angkalk.RAPZINSKOSTENMIN = (decimal?)(angantvar.kalkulation.angAntKalkDto.calcZinskostenMin);
                    angkalk.RAPZINSKOSTENMAX = (decimal?)(angantvar.kalkulation.angAntKalkDto.calcZinskostenMax);

                    angkalk.RAPRSVMONATMIN = (decimal?)(angantvar.kalkulation.angAntKalkDto.calcRsvmonatMin);
                    angkalk.RAPRSVMONATMAX = (decimal?)(angantvar.kalkulation.angAntKalkDto.calcRsvmonatMax);

                    //provisions---------------------------------------------------------------
                    //get current Id's
                    List<long> currentIds = (from a in angantvar.kalkulation.angAntProvDto
                                             select a.sysprov).ToList();

                    //get entities that are no more in current id's
                    List<object> deleteEntities = (from a in context.ANGPROV
                                                   where !currentIds.Contains(a.SYSPROV)
                                                   && a.ANGVAR.SYSANGVAR == angvar.SYSANGVAR
                                                   select a).ToList<object>();
                    //delete old entities
                    foreach (object toDel in deleteEntities)
                        context.DeleteObject(toDel);

                    //Update/Insert changed ones
                    foreach (AngAntProvDto prov in angantvar.kalkulation.angAntProvDto)
                    {
                        ANGPROV angprov = (from a in context.ANGPROV
                                           where a.SYSPROV == prov.sysprov
                                           select a).FirstOrDefault();
                        if (angprov == null)//new angvar
                        {
                            angprov = new ANGPROV();
                            context.ANGPROV.Add(angprov);
                        }
                        mapper.Map<AngAntProvDto, ANGPROV>(prov, angprov);
                        angprov.SYSANGVAR = angvar.SYSANGVAR;
                        angprov.SYSVT = angvar.SYSANGVAR;

                        angprov.PRPROVTYPE = context.PRPROVTYPE.Where(par => par.SYSPRPROVTYPE == prov.sysprprovtype).FirstOrDefault();
                        //angprov.PARTNER = context.PARTNER.Where(par => par.SYSPARTNER == prov.syspartner).FirstOrDefault();
                        angprov.SYSPARTNER = prov.syspartner;
                        angprov.PROVISION = (decimal?)prov.provision;
                        angprov.PROVISIONP = (decimal?)prov.provisionPro;
                        angprov.PROVISIONBRUTTO = (decimal?)prov.provisionBrutto;

                        angprov.DEFPROVISION = (decimal?)prov.defaultprovision;
                        angprov.DEFPROVISIONBRUTTO = (decimal?)prov.defaultprovisionbrutto;
                        angprov.DEFPROVISIONP = (decimal?)prov.defaultprovisionp;
                        angprov.DEFPROVISIONUST = (decimal?)prov.defaultprovisionust;

                        int index = angantvar.kalkulation.angAntProvDto.IndexOf(prov);
                        if (angantvar.kalkulation.angAntProvDtoRapMin != null && angantvar.kalkulation.angAntProvDtoRapMin.Count() > index)
                        {
                            // Provision für 5% Zinsen
                            angprov.RAPPROVISIONBRUTTOMIN = (decimal?)angantvar.kalkulation.angAntProvDtoRapMin[index].provisionBrutto;
                        }
                        if (angantvar.kalkulation.angAntProvDtoRapMax != null && angantvar.kalkulation.angAntProvDtoRapMax.Count() > index)
                        {
                            // Provision für 15% Zinsen
                            angprov.RAPPROVISIONBRUTTOMAX = (decimal?)angantvar.kalkulation.angAntProvDtoRapMax[index].provisionBrutto;
                        }

                        angprov.FLAGLOCKED = prov.flaglocked;
                        context.SaveChanges();
                    }

                    //insurances
                    //get current Id's
                    currentIds = (from a in angantvar.kalkulation.angAntVsDto
                                  select a.sysangvs).ToList();

                    //get entities that are no more in current id's
                    deleteEntities = (from a in context.ANGVS
                                      where !currentIds.Contains(a.SYSANGVS)
                                      && a.ANGVAR.SYSANGVAR == angvar.SYSANGVAR
                                      select a).ToList<object>();
                    //delete old entities
                    foreach (object toDel in deleteEntities)
                        context.DeleteObject(toDel);

                    //Update/Insert changed ones
                    foreach (AngAntVsDto vs in angantvar.kalkulation.angAntVsDto)
                    {
                        ANGVS angvs = (from a in context.ANGVS
                                       where a.SYSANGVS == vs.sysangvs
                                       select a).FirstOrDefault();
                        if (angvs == null)     //new angvar
                        {
                            angvs = new ANGVS();
                            context.ANGVS.Add(angvs);
                        }
                        mapper.Map<AngAntVsDto, ANGVS>(vs, angvs);
                        angvs.SYSANGVAR = angvar.SYSANGVAR;
                        angvs.SYSVSTYP = vs.sysvstyp;
                        angvs.SYSANGEBOT = angebotOutput.SYSID;

                        long? sysVs = (from vstyp in context.VSTYP
                                          where vstyp.SYSVSTYP == vs.sysvstyp
                                          select vstyp.SYSVS).FirstOrDefault();
                        if (sysVs != null&&sysVs.HasValue)
                        {
                            angvs.SYSVS = sysVs;                            
                        }
                        context.SaveChanges();
                    }

                    //subventions--------------------------------------
                    //get current Id's
                    currentIds = (from a in angantvar.kalkulation.angAntSubvDto
                                  select a.sysangsubv).ToList();

                    //get entities that are no more in current id's
                    deleteEntities = (from a in context.ANGSUBV
                                      where !currentIds.Contains(a.SYSANGSUBV)
                                      && a.ANGVAR.SYSANGVAR == angvar.SYSANGVAR
                                      select a).ToList<object>();
                    //delete old entities
                    foreach (object toDel in deleteEntities)
                        context.DeleteObject(toDel);

                    //Update/Insert changed ones
                    foreach (AngAntSubvDto sub in angantvar.kalkulation.angAntSubvDto)
                    {
                        ANGSUBV angsubv = (from a in context.ANGSUBV
                                           where a.SYSANGSUBV == sub.sysangsubv
                                           select a).FirstOrDefault();
                        if (angsubv == null)//new angvar
                        {
                            angsubv = new ANGSUBV();
                            context.ANGSUBV.Add(angsubv);

                            angsubv.SYSANGVAR = angvar.SYSANGVAR;
                        }
                        mapper.Map<AngAntSubvDto, ANGSUBV>(sub, angsubv);

                        if (isDiffLeasing == true)
                        {
                            angsubv.SUBVTYP = context.SUBVTYP.Where(par => par.SYSSUBVTYP == sub.syssubvtyp).FirstOrDefault();
                            angsubv.SYSSUBVG = sub.syssubvg;
                        }
                        context.SaveChanges();
                    }
                    setCasaDiplomaSpecialFields(angebotInput, "ANGEBOT", angebotOutput.SYSID, vttypCode);
                    createOrUpdateCard(angebotOutput.SYSID, angebotInput.emboss, isCard,0);
                }

                // Vertriebsweg 
                if (angebotOutput.ERFASSUNGSCLIENT == ERFASSUNGSCLIENT_B2B || angebotOutput.ERFASSUNGSCLIENT == ERFASSUNGSCLIENT_B2C)
                    angebotOutput.VERTRIEBSWEG = MyGetVertriebsWeg(angebotOutput.SYSPRCHANNEL, angebotOutput.ERFASSUNGSCLIENT.Value, angebotInput.vertriebsweg);
				
				// rh: 20170327: syscamptp immer verarbeiten, --> syscamp muss ab jetzt NICHT MEHR IMMER gesetzt sein!  
	
				// syscamp (KampagnenCode)
                if (angebotInput.syscamp >= 0)
                    angebotOutput.SYSCAMP=angebotInput.syscamp;
                

				//// syscampTp: (Eingangskanal)	(rh: 20170324 Im ANG noch nicht Vorhanden!)
				if (angebotInput != null)
                {
                    angebotOutput.SYSCAMPTP = angebotInput.syscamptp;
                    
                }
				
                // Kunde
                if (angebotInput.kunde != null)
                {
                    context.SaveChanges();
                    if (!context.Entry(angebotOutput).Collection(f => f.ITRELATEList).IsLoaded)
                        context.Entry(angebotOutput).Collection(f => f.ITRELATEList).Load();
                    
                    ITRELATE itRelate = angebotOutput.ITRELATEList.FirstOrDefault();
                    if (itRelate == null)
                    {
                        itRelate = new ITRELATE();
                        context.ITRELATE.Add(itRelate);
                    }
                    //itRelate.ANGEBOT = angebotOutput;
                    itRelate.SYSANGEBOT = angebotOutput.SYSID;
                    itRelate.SYSIT1 = angebotInput.kunde.sysit;
                }

                ANGOBSICH sicherheit = (from angobsich in context.ANGOBSICH
                                        where angobsich.SYSANGEBOT == angebotInput.sysid
                                        select angobsich).FirstOrDefault();

                if (angebotInput.mitantragsteller != null)
                {

                    if (sicherheit == null) { 
                        sicherheit = new ANGOBSICH();
                       context.ANGOBSICH.Add(sicherheit);
                    }
                    
                    
                        sicherheit.SYSIT=angebotInput.mitantragsteller.sysit;
                        if (angebotInput.mitantragsteller.syskd > 0)
                            sicherheit.SYSPERSON = angebotInput.mitantragsteller.syskd;
                        sicherheit.SYSANGEBOT = angebotOutput.SYSID;
                        SetAngebotIdInZusatzdaten(context, angebotInput.mitantragsteller, angebotInput.sysid);
                        // Partner(800)/Solidarschuldner(120)/Bürge(130)
                        int crang = (int)MitantragstellerTyp.Partner;
                        if (angebotInput.mitantragsteller.zusatzdaten != null && angebotInput.mitantragsteller.zusatzdaten.Length > 0 && angebotInput.mitantragsteller.zusatzdaten[0].pkz != null)
                            if (angebotInput.mitantragsteller.zusatzdaten[0].pkz != null && angebotInput.mitantragsteller.zusatzdaten[0].pkz.Length > 0)
                                if (angebotInput.mitantragsteller.zusatzdaten[0].pkz[0].ehepartnerFlag > 1)
                                    crang = angebotInput.mitantragsteller.zusatzdaten[0].pkz[0].ehepartnerFlag;

                        //allow sichtyp control from outside
                        if (angebotInput.mitantragsteller.sichtyprang > 0)
                            crang = angebotInput.mitantragsteller.sichtyprang;

                        sicherheit.SICHTYP = (from typ in context.SICHTYP
                                              where typ.RANG == crang
                                              select typ).FirstOrDefault();
                    
                }
                else if (sicherheit != null)    //remove!
                {
                    context.DeleteObject(sicherheit);
                }




                context.SaveChanges();

                if (angob != null)
                {
                    if (deleteObaust)
                        deleteAngObAust(angob.SYSOB);
                    updateAngAngObAust(angebotInput.angAntObDto.aust, angob.SYSOB);
                }


                if (angebotInput.bemerkung != null && angebotInput.bemerkung.Length > 0)
                    if (angebotInput.erfassungsclient == ERFASSUNGSCLIENT_ONE)
                        createOrUpdateMemo(angebotOutput.SYSID, angebotInput.bemerkung, "ANGEBOT", "Bemerkung", "Allgemein", angebotOutput.SYSVK);

                    else
                        createOrUpdateMemo(angebotOutput.SYSID, angebotInput.bemerkung, "ANGEBOT", KURZTXTNOTIZ, "Allgemein", angebotOutput.SYSVK);


                return getAngebot(angebotOutput.SYSID);
            }
        }

        /// <summary>
        /// Angbeot löschen
        /// </summary>
        /// <param name="sysid">Primary Key</param>
        public void deleteAngebot(long sysid)
        {
            using (DdOlExtended context = new DdOlExtended())
            {
                ANGEBOT angebot = (from ang in context.ANGEBOT
                                   where ang.SYSID == sysid
                                   select ang).FirstOrDefault();

                // Check sight field
                if (angebot == null)
                {
                    throw new System.ApplicationException("Angebot does not exist.");
                }

                // Delete PEUNI
                string AreaName = "ANGEBOT";

                // Select PEUNI list                 
                var PEUNIQuery = from peuni in context.PEUNI
                                 where (peuni.SYSID == sysid && peuni.AREA == AreaName)
                                 select peuni;

                // Delete each PEUNI (in context)                    
                foreach (PEUNI LoopPEUNI in PEUNIQuery)
                {
                    context.DeleteObject(LoopPEUNI);
                }
                /*
                // Delete ITADRREF
                ITADRREF adrRef = context.ITADRREF.Where(par => par.SYSANGEBOT == angebot.SYSID).FirstOrDefault();
                if (adrRef != null)
                    context.DeleteObject(adrRef);
                */

                context.SaveChanges();
                // Delete ITKONTOREF
                if (!context.Entry(angebot).Collection(f => f.ITKONTOREFList).IsLoaded)
                    context.Entry(angebot).Collection(f => f.ITKONTOREFList).Load();
                
                ITKONTOREF ktoRef = angebot.ITKONTOREFList.FirstOrDefault();
                if (ktoRef != null)
                    context.DeleteObject(ktoRef);
                context.SaveChanges();
                if (!context.Entry(angebot).Collection(f => f.ANGOBList).IsLoaded)
                    context.Entry(angebot).Collection(f => f.ANGOBList).Load();
                
                ANGOB ob = angebot.ANGOBList.FirstOrDefault();
                if (ob != null)
                {
                    // Delete ANGOBBRIEF
                    
                    if (ob.ANGOBBRIEF != null)
                        context.DeleteObject(ob.ANGOBBRIEF);
                    //SELECT ANGOBINI
                    var ANGOBINIQuery = from angobini in context.ANGOBINI
                                        where angobini.SYSOBINI == ob.SYSOB
                                        select angobini;
                    //DELETE each ANGOBINI
                    foreach (ANGOBINI LoopANGOBINI in ANGOBINIQuery)
                    {
                        context.DeleteObject(LoopANGOBINI);
                    }
                    // Delete ANGOB
                    context.DeleteObject(ob);
                }

                // Delete ANGVAR, ANGKALK, ANGVS, ANGPROV, ANGSUBV
                context.SaveChanges();
                if (!context.Entry(angebot).Collection(f => f.ANGVARList).IsLoaded)
                    context.Entry(angebot).Collection(f => f.ANGVARList).Load();

                if (angebot.ANGVARList != null)
                {
                    List<ANGVAR> angvarList = angebot.ANGVARList.ToList();
                    foreach (ANGVAR angvar in angvarList)
                    {
                        //Select ANGKALK
                        ANGKALK angkalk = (from avar in context.ANGKALK
                                           where avar.ANGVAR.SYSANGVAR == angvar.SYSANGVAR
                                           select avar).FirstOrDefault();
                        if (angkalk != null)
                        {
                            context.DeleteObject(angkalk);
                        }

                        //Provision-----------------------------------------------------------

                        List<ANGPROV> angprovList = (from avar in context.ANGPROV
                                                     where avar.ANGVAR.SYSANGVAR == angvar.SYSANGVAR
                                                     select avar).ToList();
                        foreach (ANGPROV angprov in angprovList)
                        {
                            context.DeleteObject(angprov);
                        }

                        //Subvention-----------------------------------------------------------

                        List<ANGSUBV> angsubvList = (from avar in context.ANGSUBV
                                                     where avar.ANGVAR.SYSANGVAR == angvar.SYSANGVAR
                                                     select avar).ToList();
                        foreach (ANGSUBV angsubv in angsubvList)
                        {
                            context.DeleteObject(angsubv);
                        }

                        //Insurances-----------------------------------------------------------

                        List<ANGVS> angvsList = (from avar in context.ANGVS
                                                 where avar.ANGVAR.SYSANGVAR == angvar.SYSANGVAR
                                                 select avar).ToList();
                        foreach (ANGVS angvs in angvsList)
                        {
                            context.DeleteObject(angvs);
                        }
                        // Delete each ANGVAR
                        context.DeleteObject(angvar);

                    }
                }

                ANGOBSICH angobsich = (from angsich in context.ANGOBSICH
                                       where angsich.SYSANGEBOT == sysid
                                       select angsich).FirstOrDefault();
                if (angobsich != null)
                {
                    context.DeleteObject(angobsich);
                }
                // DeleteObject ANGEBOT and SaveChanges
                context.DeleteObject(angebot);

                context.SaveChanges();

            }
        }

        /// <summary>
        /// Antrag erstellen
        /// </summary>
        /// <param name="antragInput">Antrag Eingang</param>
        /// <param name="sysperole">personenrollen ID</param>
        /// <returns>Antrag Ausgang</returns>
        public AntragDto createAntrag(AntragDto antragInput, long sysperole)
        {
            return createAntrag(antragInput, null, null, sysperole);
        }

        /// <summary>
        /// Antrag erstellen
        /// </summary>
        /// <param name="antragInput">Antrag Eingang</param>
        /// <param name="aktivKz"></param>
        /// <param name="endeKz"></param>
        /// <param name="sysperole">User Id</param>
        /// <returns>Antrag Ausgang</returns>
        public AntragDto createAntrag(AntragDto antragInput, int? aktivKz, int? endeKz, long sysperole)
        {
            using (DdOlExtended context = new DdOlExtended())
            {
                
                //Mapper.CreateMap<AngAntObDto, ANTOB>().ForMember(a => a.BRIEF, opt => opt.Ignore());

                ANTRAG antragOutput = new ANTRAG();
                context.ANTRAG.Add(antragOutput);


                //use kunde id if given 
                if (antragInput.kunde != null)
                    antragInput.sysit = antragInput.kunde.sysit;
                antragInput.erfassung = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null);
                antragInput.aenderung = antragInput.erfassung;
                antragInput.zustandAm = antragInput.erfassung;

                Mapper.Map<AntragDto, ANTRAG>(antragInput, antragOutput);

                if (antragInput.options == null)
                    antragInput.options = new AngAntOptionDto();

                antragInput.options.str06 = "1";//Kremo-Werte nicht löschen!
                
                if (antragInput.options != null)
                {
                    antragOutput.ANTOPTION = Mapper.Map<AngAntOptionDto, ANTOPTION>(antragInput.options);
                }
                


                bool isLeaseNow = false;
                bool isDiffLeasing = false;
                bool isDispo = false;
                bool isClassic = false;
                bool istzk = false;             //Teilzahlungskauf ist analog Kredit zu rechnen
                bool isCredit = false;
                bool isCard = false;
                String vttypCode = null;
                try
                {
                    if (antragInput.kalkulation != null && antragInput.kalkulation.angAntKalkDto != null)
                    {
                        //Produkte werden anhand vart unterschieden: es gibt KREDIT, TZK, LEASE_now, CREDIT-now Dispo, DifferenzLeasing
                        IPrismaDao prismaDao = PrismaDaoFactory.getInstance().getPrismaDao();
                        isDiffLeasing = prismaDao.isDiffLeasing((long)antragInput.kalkulation.angAntKalkDto.sysprproduct); // Differenzleasing 

                        CIC.Database.PRISMA.EF6.Model.VART vart = prismaDao.getVertragsart((long)antragInput.kalkulation.angAntKalkDto.sysprproduct);
						CIC.Database.PRISMA.EF6.Model.VTTYP vtt = prismaDao.getVttyp((long)antragInput.kalkulation.angAntKalkDto.sysprproduct);
                        antragOutput.SYSPRPRODUCT = antragInput.kalkulation.angAntKalkDto.sysprproduct;
                        if (vtt != null)
                        {
                            vttypCode = vtt.CODE;
                            antragOutput.SYSVTTYP = vtt.SYSVTTYP;
                        }
                        else
                            antragOutput.SYSVTTYP = null;




                        // VertragsArt ermitteln
                        if (vart != null)
                        {
                            antragOutput.SYSVART = vart.SYSVART;
                            String vArtCode = vart.CODE.ToLower();
                            isLeaseNow = (vArtCode.IndexOf("leasing") > -1);                // LEASE-now
                            isDispo = (vArtCode.IndexOf("dispo") > -1) || (vArtCode.IndexOf("flex") > -1); // CREDIT_now Dispo
                            isClassic = (vArtCode.IndexOf("classic") > -1);             // CREDIT_now Classic
                            isCard = (vArtCode.IndexOf("kredit_dispoplus") > -1);           // CREDIT_now Card
                            istzk = (vArtCode.IndexOf("TZK") > -1);
                            isCredit = (vArtCode.IndexOf("KREDIT") > -1) || istzk;
                        }
                    }
				}
				catch (Exception ea)
                {
                    _log.Warn("Error writing back SYSPRPRODUCT/SYSVART to ANTRAG", ea);
                }

                // antragOutput.ERFASSUNG darf keine Uhrzeit enthalten
                antragOutput.ERFASSUNG = ((DateTime)antragOutput.ERFASSUNG).Date;
                antragOutput.ERFASSUNGZEIT = DateTimeHelper.DateTimeToClarionTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));

                // Ticket#2012080910000035, Ticket#2012081510000041 
                if (antragInput.erfassungsclient > 0)
                {
                    // antragInput.erfassungsclient wird nur bei processAngebotToAntrag im Dto übergeben
                    antragOutput.ERFASSUNGSCLIENT = (int?)antragInput.erfassungsclient;
                }
                else
                {
                    // createAntrag wird nur vom B2B aufgerufen, MAClient verwendet Clarion
                    antragOutput.ERFASSUNGSCLIENT = ERFASSUNGSCLIENT_B2B;

                }
                // Vertriebsweg
                // Ticket#2012082210000171: für den MAClient soll der Vertriebsweg einfach übernommen werden (processAngebotToAntrag)
                if (antragOutput.ERFASSUNGSCLIENT == ERFASSUNGSCLIENT_B2B || antragOutput.ERFASSUNGSCLIENT == ERFASSUNGSCLIENT_B2C)
                    antragOutput.VERTRIEBSWEG = MyGetVertriebsWeg(antragOutput.SYSPRCHANNEL, antragOutput.ERFASSUNGSCLIENT == null ? 0 : antragOutput.ERFASSUNGSCLIENT.Value, antragInput.vertriebsweg);

                if (antragInput.syskd != null && antragInput.syskd.HasValue)
                {
                    antragOutput.SYSKD = antragInput.syskd.Value;
                }

                // BNRZEHN-1265

                if (antragInput.syscamp != 0)
                {
                    antragOutput.CAMP = (from camp in context.CAMP
                                         where camp.SYSCAMP == antragInput.syscamp
                                         select camp).FirstOrDefault();
                }


                if (antragInput.syscamptp != 0)
                {
                    antragOutput.CAMPTP = (from camptp in context.CAMPTP
                                         where camptp.SYSCAMPTP == antragInput.syscamptp
                                         select camptp).FirstOrDefault();
                }

                // Ticket#2011081710000021 : Im Antrag muss zusätzlich das Feld sysls gefüllt werden. Hier muss immer der Wert 1 stehen.
                antragOutput.SYSLS = 1;

                // Ticket#2012090510000531 : die Felder sollen für MA-Client nur mitkopiert werden (ProcessAngebotToAntrag)
                if (antragOutput.ERFASSUNGSCLIENT == ERFASSUNGSCLIENT_B2B || antragOutput.ERFASSUNGSCLIENT == ERFASSUNGSCLIENT_B2C || antragOutput.ERFASSUNGSCLIENT == ERFASSUNGSCLIENT_ONE || antragOutput.ERFASSUNGSCLIENT == ERFASSUNGSCLIENT_DMR)
                {
                    //Webfrontend übernimmt immer den VM + VK von aussen bei Fahrzeugfin (weil in Maske)
                    if (antragOutput.ERFASSUNGSCLIENT == ERFASSUNGSCLIENT_DMR || ((antragOutput.ERFASSUNGSCLIENT == ERFASSUNGSCLIENT_ONE ) && antragOutput.VERTRIEBSWEG != null && antragOutput.VERTRIEBSWEG.ToUpper().IndexOf("FAHRZ") > -1))
                    {
                        
                    }
                    else
                    {
                        // antragOutput.SYSVK (Verkäufer) = PERSON von sysperole = Mitarbeiter
                        antragOutput.SYSVK = (from perole in context.PEROLE
                                              where perole.SYSPEROLE == sysperole
                                              select perole.SYSPERSON).FirstOrDefault();

                        //antragOutput.SYSVM (Vermittler) = Person von Händler (Parent vom Verkäufer)
                        // select perole.sysperson from perole where perole.sysperole = (select perole.SYSPARENT from perole where perole.SYSPEROLE = 650) ;
                        antragOutput.SYSVM = (from parentPeRole in context.PEROLE
                                              join perole in context.PEROLE on parentPeRole.SYSPARENT equals perole.SYSPEROLE
                                              where parentPeRole.SYSPEROLE == sysperole
                                              select perole.SYSPERSON).FirstOrDefault();
                    }
                    antragOutput.SYSPRHGROUP = 0;
                    try
                    {
                        // Handelsgruppe wird in solveKalkulation berechnet 
                        antragOutput.SYSPRHGROUP = antragInput.kalkulation.angAntKalkDto.sysprhgrp;
                    }
                    catch (Exception) { }

                    // Brand wird anhand der Handelsgruppe ermittelt
                    antragOutput.SYSBRAND = (from prbrandm in context.PRBRANDM
                                          join brand in context.BRAND on prbrandm.BRAND.SYSBRAND equals brand.SYSBRAND
                                          where prbrandm.PRHGROUP.SYSPRHGROUP == antragOutput.SYSPRHGROUP
                                          select prbrandm.SYSBRAND).FirstOrDefault();
                    
                    // AbwicklungsOrt (PeRole) ist dem Händler zugeordnet / BNRZEHN-1584
                    if (antragInput.sysAbwicklung > 0 &&
                        (antragOutput.ERFASSUNGSCLIENT == ERFASSUNGSCLIENT_B2C || antragOutput.ERFASSUNGSCLIENT == ERFASSUNGSCLIENT_ONE || antragOutput.ERFASSUNGSCLIENT == ERFASSUNGSCLIENT_DMR))
                        antragOutput.SYSABWICKLUNG = antragInput.sysAbwicklung;
                    else
                        antragOutput.SYSABWICKLUNG = MyGetHaendlerAbwicklungsort(antragOutput.SYSVM);

                    // MwSt
                    if (isLeaseNow == true)
                    {
                        antragOutput.SYSMWST = context.ExecuteStoreQuery<long>("select sysmwst from lsadd where syslsadd=" + antragOutput.SYSLS).FirstOrDefault();
                    }
                }

                antragOutput.ZUSTAND = AntragZustand.Neu.ToString();
                antragOutput.ATTRIBUT = AntragAttribut.Neu.ToString();
                antragOutput.AKTIVKZ = aktivKz;
                antragOutput.ENDEKZ = endeKz;

                //disable to force antragsassi to takeover it-pkz's
                //BNRDR-1418
                if (antragOutput.ERFASSUNGSCLIENT == AngAntDao.ERFASSUNGSCLIENT_ONE || antragOutput.ERFASSUNGSCLIENT == AngAntDao.ERFASSUNGSCLIENT_DMR)
                {
                    antragOutput.ZUSTAND = AntragZustand.Neu.ToString();
                    antragOutput.ATTRIBUT = AntragAttribut.Eingereicht.ToString();
                    antragInput.syskd = 0;
                }

              
                //BNRZEHN-951
                if (antragInput.antrag == null || antragInput.antrag.Trim().Equals(""))
                    antragOutput.ANTRAG1 = NkBuilderFactory.createAntragNkBuilder().getNextNumber();
                else
                    // antragInput.antrag wird nur bei processAngebotToAntrag im Dto übergeben
                    antragOutput.ANTRAG1 = antragInput.antrag;


                context.SaveChanges();


                if (!String.IsNullOrEmpty(antragInput.extident))
                {
                    ExtidentDto exti = new ExtidentDto();
                    exti.area = "ANTRAG";
                    exti.extidentvalue = antragInput.extident;
                    exti.sysarea = antragOutput.SYSID;
                    exti.source = "" + antragOutput.ERFASSUNGSCLIENT;
                    exti.codeextidenttyp = CODE_EXTIDENT_ANTRAGSID_B2C;
                    createOrUpdateExtident(exti);
                }
                long sysangebot = context.ExecuteStoreQuery<long>("select sysid from angebot where angebot='" + antragOutput.ANTRAG1 + "'", null).FirstOrDefault();

                SetAntragIdInZusatzdaten(context, antragInput.kunde, antragOutput.SYSID);
                bool hasKrankenkasse = createOrUpdateKREMOAntrag(sysangebot, antragOutput.SYSID, antragInput.kunde, antragInput.mitantragsteller);
               


                //Object---------------------------------------------
                ANTOB antob = null;
                if (antragInput.angAntObDto != null)
                {
                    fixAHK(antragInput.angAntObDto, antragInput.sysprchannel);
                    antob = new ANTOB();

                    antob.SYSOBART=antragInput.angAntObDto.sysobart;
                    antob.SYSOBTYP=antragInput.angAntObDto.sysobtyp;
                    antob.SYSNKK= antragInput.angAntObDto.sysnkk;
                    
                    antob.GEHNID = antragInput.angAntObDto.typengenehmigung;

                    context.ANTOB.Add(antob);

                    Mapper.Map<AngAntObDto, ANTOB>(antragInput.angAntObDto, antob);

                    //Brief----------------------------------------------
                    if (antragInput.angAntObDto.brief != null)
                    {
                        ANTOBBRIEF brief = new ANTOBBRIEF();
                        context.ANTOBBRIEF.Add(brief);
                        Mapper.Map<AngAntObBriefDto, ANTOBBRIEF>(antragInput.angAntObDto.brief, brief);
                        brief.AART = antragInput.angAntObDto.brief.aufbau;
                        brief.ANTOB = antob;
                        if (brief.GETRIEBE != null)
                        {
                            if (brief.GETRIEBE.Length > 20)
                            {
                                brief.GETRIEBE = brief.GETRIEBE.Substring(0, 20);
                                _log.Debug("Brief.Getriebe wurde gekürzt auf 20 Zeichen beim Antrag: " + antragOutput.SYSID);
                            }
                        }
                    }

                    antob.SYSANTRAG = antragOutput.SYSID;
                    if (antob.BEZEICHNUNG != null && antob.BEZEICHNUNG.Length > 40)
                        antob.BEZEICHNUNG = antob.BEZEICHNUNG.Substring(0, 40);

                    // in ANTOB:OBJEKT steht der Wert aus dem Nummernkreis ANTOB.....
                    // soll also analog zu ANTRAG.ANTRAG gezogen werden
                    antob.OBJEKT = NkBuilderFactory.createAntobNkBuilder().getNextNumber();
                    context.SaveChanges();


                    //BNR10 OBAUS

                    updateAngAntObAust(antragInput.angAntObDto.aust, antob.SYSOB);
                    

                }

                // Object HD ---------------------------------------------
                // Ticket JOORMANN-2011-7-27-161610
                ANTOBHD antobhd = new ANTOBHD();
                //antobhd.ANTRAG = antragOutput;
                antobhd.SYSVT = antragOutput.SYSID;
                antobhd.ANTOB = antob;                
                antobhd.SYSHD = antragOutput.SYSVM;
                antobhd.ERFASSUNG = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null);
                context.ANTOBHD.Add(antobhd);
                context.SaveChanges();


                //Calculation----------------------------------------
                if (antragInput.kalkulation != null)
                {
                    if (antragInput.kalkulation.angAntKalkDto != null)
                    {
                        IPrismaDao prismaDao = PrismaDaoFactory.getInstance().getPrismaDao();
                        ANTKALK antkalk = new ANTKALK();
                        context.ANTKALK.Add(antkalk);
                        Mapper.Map<AngAntKalkDto, ANTKALK>(antragInput.kalkulation.angAntKalkDto, antkalk);
                        
                        antkalk.SYSOB = 0;
                        if (antob != null)
                        {
                            antkalk.SYSOB = antob.SYSOB;
                        }
                        //antkalk.ANTRAG = antragOutput;
                        antkalk.SYSANTRAG = antragOutput.SYSID;
                        antkalk.SYSKALK = antragOutput.SYSID;
                        antkalk.SYSCREATE = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null);
                        antkalk.SYSCHANGE = antkalk.SYSCREATE;
                        antkalk.SYSPRPRODUCT = antragInput.kalkulation.angAntKalkDto.sysprproduct;

                        

                        antkalk.OBUSETYPE = context.OBUSETYPE.Where(par => par.SYSOBUSETYPE == antragInput.kalkulation.angAntKalkDto.sysobusetype).FirstOrDefault();

                        if (!antkalk.SYSWAEHRUNG.HasValue || antkalk.SYSWAEHRUNG.Value<1)
                            antkalk.SYSWAEHRUNG = context.ExecuteStoreQuery<long>("select syshauswaehrung from lsadd where syslsadd=1", null).FirstOrDefault(); 
                        
                        if (isDispo == true)
                        {
                            List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysprProduct", Value = antragInput.kalkulation.angAntKalkDto.sysprproduct });
                            antkalk.SYSINTSTRCT = context.ExecuteStoreQuery<long>(SYSINTSTRCTBYPRODUCTID, parameters.ToArray()).FirstOrDefault();
                        }

                        // Modus muss bei kredit 0, bei leasing 1 sein
                        if (isLeaseNow == true) { antkalk.MODUS = 1; } else { antkalk.MODUS = 0; }

                        // PPY (Raten pro Jahr): Konstant 12 bei bnow
                        antkalk.PPY = 12;
                        if (isClassic == true)
                        {
                            antkalk.AUSZAHLUNG = (decimal?)antragInput.kalkulation.angAntKalkDto.auszahlung;
                            antkalk.AUSZAHLUNGTYP = antragInput.kalkulation.angAntKalkDto.auszahlungTyp;
                        }

                        if (isLeaseNow == true)
                        {
                            antkalk.BGEXTERNUST = (decimal?)(antragInput.kalkulation.angAntKalkDto.bgexternbrutto - antragInput.kalkulation.angAntKalkDto.bgextern);
                            antkalk.BGINTERNUST = (decimal?)(antragInput.kalkulation.angAntKalkDto.bginternbrutto - antragInput.kalkulation.angAntKalkDto.bgintern);
                            antkalk.RATEUST = (decimal?)antragInput.kalkulation.angAntKalkDto.rateUst;
                            antkalk.AUSZAHLUNG = antkalk.BGEXTERNUST;
                        }
                        if (isCredit == true)
                        {
                            antkalk.BGEXTERNUST = 0;
                        }
                    }

                    //provisions-------------------------------------
                    if (antragInput.kalkulation.angAntProvDto != null)
                        foreach (AngAntProvDto prov in antragInput.kalkulation.angAntProvDto)
                        {
                            ANTPROV antprov = new ANTPROV();
                            context.ANTPROV.Add(antprov);
                            Mapper.Map<AngAntProvDto, ANTPROV>(prov, antprov);
                            // AntProv ist über AntProv.SysVT mit Antrag verbunden
                            //antprov.ANTRAG = antragOutput;
                            // in sysAntrag muss auch die Id stehen
                            antprov.SYSANTRAG = antragOutput.SYSID;
                            antprov.SYSVT = antragOutput.SYSID;
                            antprov.PRPROVTYPE = context.PRPROVTYPE.Where(par => par.SYSPRPROVTYPE == prov.sysprprovtype).FirstOrDefault();
                            //antprov.PARTNER = context.PARTNER.Where(par => par.SYSPARTNER == prov.syspartner).FirstOrDefault();
                            antprov.SYSPARTNER = prov.syspartner;
                            antprov.PROVISION = (decimal?)prov.provision;
                            antprov.PROVISIONP = (decimal?)prov.provisionPro;
                            antprov.PROVISIONBRUTTO = (decimal?)prov.provisionBrutto;

                            antprov.DEFPROVISION = (decimal?)prov.defaultprovision;
                            antprov.DEFPROVISIONBRUTTO = (decimal?)prov.defaultprovisionbrutto;
                            antprov.DEFPROVISIONP = (decimal?)prov.defaultprovisionp;
                            antprov.DEFPROVISIONUST = (decimal?)prov.defaultprovisionust;

                            int index = antragInput.kalkulation.angAntProvDto.IndexOf(prov);
                            if (antragInput.kalkulation.angAntProvDtoRapMin != null && antragInput.kalkulation.angAntProvDtoRapMin.Count() > index)
                            {
                                // Provision für 5% Zinsen
                                antprov.RAPPROVISIONBRUTTOMIN = (decimal?)antragInput.kalkulation.angAntProvDtoRapMin[index].provisionBrutto;
                            }
                            if (antragInput.kalkulation.angAntProvDtoRapMax != null && antragInput.kalkulation.angAntProvDtoRapMax.Count() > index)
                            {
                                // Provision für 15% Zinsen
                                antprov.RAPPROVISIONBRUTTOMAX = (decimal?)antragInput.kalkulation.angAntProvDtoRapMax[index].provisionBrutto;
                            }

                            antprov.FLAGLOCKED = prov.flaglocked;
                            context.SaveChanges();
                        }
                    //insurances-------------------------------------
                    if (antragInput.kalkulation.angAntVsDto != null)
                        foreach (AngAntVsDto vs in antragInput.kalkulation.angAntVsDto)
                        {
                            ANTVS antvs = new ANTVS();
                            context.ANTVS.Add(antvs);
                            Mapper.Map<AngAntVsDto, ANTVS>(vs, antvs);

                            //antvs.ANTRAG = antragOutput;
                            antvs.SYSANTRAG = antragOutput.SYSID;
                            antvs.SYSVSTYP=vs.sysvstyp;

                            antvs.SYSVS = (from vstyp in context.VSTYP
                                           where vstyp.SYSVSTYP == vs.sysvstyp
                                           select vstyp.SYSVS).FirstOrDefault();
                            context.SaveChanges();
                        }
                    //subventions------------------------------------
                    if (antragInput.kalkulation.angAntSubvDto != null)
                        foreach (AngAntSubvDto subv in antragInput.kalkulation.angAntSubvDto)
                        {
                            ANTSUBV antsubv = new ANTSUBV();
                            context.ANTSUBV.Add(antsubv);
                            Mapper.Map<AngAntSubvDto, ANTSUBV>(subv, antsubv);

                            //antsubv.ANTRAG = antragOutput;
                            antsubv.SYSANTRAG = antragOutput.SYSID;

                            if (isDiffLeasing == true)
                            {
                                antsubv.SUBVTYP = context.SUBVTYP.Where(par => par.SYSSUBVTYP == subv.syssubvtyp).FirstOrDefault();
                                antsubv.SYSSUBVG = subv.syssubvg;
                            }
                            context.SaveChanges();
                        }

                    // Ablösen ---------------------------------------------
                    //Update/Insert changed ones
                    if (antragInput.kalkulation.angAntAblDto != null)
                        foreach (AngAntAblDto abl in antragInput.kalkulation.angAntAblDto)
                        {
                            ANTABL antabl = new ANTABL();
                            context.ANTABL.Add(antabl);

                            Mapper.Map<AngAntAblDto, ANTABL>(abl, antabl);

                            antabl.SYSANTRAG = antragOutput.SYSID;
                            antabl.SYSVORVT= abl.sysvorvt;
                            antabl.SYSABLTYP= abl.sysabltyp;
                            context.SaveChanges();
                        }
                    
                }

                
                if (antragInput.mitantragsteller != null && antragInput.mitantragsteller.sysit > 0)
                {
                    createMitantragsteller(antragOutput.SYSID, antragInput.mitantragsteller, antragInput.mitantragstellerTyp);
                    SetAntragIdInZusatzdaten(context, antragInput.mitantragsteller, antragOutput.SYSID);
                }

               
                

                PEUNIHelper.ConnectNodes(context, PEUNIArea.ANTRAG, antragOutput.SYSID, sysperole);
                context.SaveChanges();


                MySaveDiffLeasingBestaetigung(antragOutput.SYSID, antragInput, antragOutput.SYSVK, isDiffLeasing);

                if (antragOutput.ERFASSUNGSCLIENT != ERFASSUNGSCLIENT_B2C && antragOutput.ERFASSUNGSCLIENT != ERFASSUNGSCLIENT_ONE && antragOutput.ERFASSUNGSCLIENT != ERFASSUNGSCLIENT_DMR)
                {
                    // Ticket#2012072310000126 — CR16425 (Ablösen): Memo-Erzeugung // BNRZW-1184 Es sollte keine ePOS Memos bei B2C Antrag erzeugt werden
                    MySaveNotizen(antragOutput.SYSID, antragInput, antragOutput.SYSVK);
                }

                setCasaDiplomaSpecialFields(antragInput, "ANTRAG", antragOutput.SYSID, vttypCode);
                

                
                
                
                antragOutput.SYSANGEBOT = sysangebot;
                context.SaveChanges();
                /*if (sysangebot > 0)
                {
                    string QUERYUPDSYSANGEBOT = "update antrag set sysangebot=" + sysangebot + " where sysid=" + antragOutput.SYSID;
                    context.ExecuteStoreCommand(QUERYUPDSYSANGEBOT, null);
                }*/
                createOrUpdateCard(sysangebot, antragInput.emboss, isCard, antragOutput.SYSID);


                return getAntrag(antragOutput.SYSID, sysperole);
            }
        }

        /// <summary>
        /// Creates a MA (Sicherheit in ANTOBSICH) for the given antragid, KundeDto (sysit has to be filled) 
        /// </summary>
        /// <param name="sysid"></param>
        /// <param name="mitantragsteller"></param>
        /// <param name="maTyp">required, default sichtyprang, can be overridden bei mitantragsteller.sichtyprang</param>
        public void createMitantragsteller(long sysid, KundeDto mitantragsteller,int maTyp)
        {
            using (DdOlExtended context = new DdOlExtended())
            {
                ANTOBSICH NewANTOBSICH = new ANTOBSICH();
                context.ANTOBSICH.Add(NewANTOBSICH);
                NewANTOBSICH.SYSIT=mitantragsteller.sysit;
                NewANTOBSICH.SYSPERSON = mitantragsteller.syskd;

                // Partner(800)/Solidarschuldner(120)/Bürge(130)
                NewANTOBSICH.RANG = MyGetMATyp(maTyp);
                //allow sichtyp control from outside
                if (mitantragsteller.sichtyprang > 0)
                    NewANTOBSICH.RANG = mitantragsteller.sichtyprang;


                NewANTOBSICH.AKTIVFLAG = 1;

                NewANTOBSICH.SYSANTRAG = sysid;
                NewANTOBSICH.SICHTYP = (from typ in context.SICHTYP
                                        where typ.RANG == NewANTOBSICH.RANG
                                        select typ).FirstOrDefault();
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Antrag kopieren
        /// </summary>
        /// <param name="inputData">Antrag Eingang</param>
        /// <param name="aktivKz"></param>
        /// <param name="endeKz"></param>
        /// <param name="sysperole">User Id</param>
        /// <param name="b2b">b2b</param>
        /// <returns>Antrag Ausgang</returns>
        public AntragDto copyAntrag(AntragDto inputData, int? aktivKz, int? endeKz, long sysperole, bool b2b)
        {
            IMapper mapper = Mapper.Instance;/* Cic.One.Utils.Util.Mapper.MapperConfig.getInstanceMapper("copyAntrag", delegate (MapperConfigurationExpression cfg)
            {

                cfg.CreateMap<OBUSETYPE, OBUSETYPE>();
                cfg.CreateMap<ANTKALKFS, ANTKALKFS>();

                cfg.CreateMap<ANTOB, ANTOB>().ForMember(dest => dest.EntityKey, opt => opt.Ignore())
                                                .ForMember(dest => dest.EntityState, opt => opt.Ignore())
                                                .ForMember(dest => dest.SYSOB, opt => opt.Ignore())
                                                .ForMember(dest => dest.OBJEKT, opt => opt.Ignore())
                                                .ForMember(dest => dest.ANTKALK, opt => opt.Ignore())

                                                .ForMember(dest => dest.ANTOBBRIEF, opt => opt.Ignore())
                                                .ForMember(dest => dest.ANTOBBRIEFReference, opt => opt.Ignore())

                                                .ForMember(dest => dest.ANTOBHDList, opt => opt.Ignore())
                                                .ForMember(dest => dest.ANTOBSICHList, opt => opt.Ignore())
                                                .ForMember(dest => dest.OBARTReference, opt => opt.Ignore())
                                                .ForMember(dest => dest.NKKReference, opt => opt.Ignore())
                                                .ForMember(dest => dest.OBKATReference, opt => opt.Ignore())
                                                .ForMember(dest => dest.OBTYPReference, opt => opt.Ignore())
                                                .ForMember(dest => dest.ANTRAGReference, opt => opt.Ignore())
                                                .ForMember(dest => dest.ANTKALKReference, opt => opt.Ignore())
                                                .ForMember(dest => dest.ANGEBOTReference, opt => opt.Ignore())
                                                .ForMember(dest => dest.ANTOBSLList, opt => opt.Ignore())
                                                .ForMember(dest => dest.ANTPROVList, opt => opt.Ignore())
                                                .ForMember(dest => dest.OBAUSTList, opt => opt.Ignore())
                                                .ForMember(dest => dest.OBPOOLList, opt => opt.Ignore())
                                                ;

                cfg.CreateMap<OBTYP, OBTYP>();
                cfg.CreateMap<ANTPROV, ANTPROV>();
                cfg.CreateMap<ANTKALK, ANTKALK>().ForMember(dest => dest.SYSKALK, opt => opt.Ignore())
                                                    .ForMember(dest => dest.ANTKALKFS, opt => opt.Ignore())
                                                    .ForMember(dest => dest.ANTOBReference, opt => opt.Ignore())
                                                    .ForMember(dest => dest.ANTRAGReference, opt => opt.Ignore())
                                                    .ForMember(dest => dest.OBTYPReference, opt => opt.Ignore())
                                                    .ForMember(dest => dest.OBUSETYPEReference, opt => opt.Ignore())

                                                    .ForMember(dest => dest.ANTRAG, opt => opt.Ignore())
                                                    .ForMember(dest => dest.ANTPROVList, opt => opt.Ignore())
                                                    .ForMember(dest => dest.ANTOBSLList, opt => opt.Ignore())
                                                    .ForMember(dest => dest.EntityKey, opt => opt.Ignore())
                                                    .ForMember(dest => dest.ANTKALKFSReference, opt => opt.Ignore())
                                                    .ForMember(dest => dest.ANTKALKVARList, opt => opt.Ignore())
                                                    .ForMember(dest => dest.EntityState, opt => opt.Ignore());

                cfg.CreateMap<ANTRAG, ANTRAG>().ForMember(dest => dest.EntityKey, opt => opt.Ignore())
                                                  .ForMember(dest => dest.EntityState, opt => opt.Ignore())
                                                  .ForMember(dest => dest.SYSID, opt => opt.Ignore())
                                                  .ForMember(dest => dest.ADMADDReference, opt => opt.Ignore())
                                                  .ForMember(dest => dest.ANTKALKList, opt => opt.Ignore())
                                                  .ForMember(dest => dest.ANTOBHDList, opt => opt.Ignore())
                                                  .ForMember(dest => dest.ANTOBSLList, opt => opt.Ignore())
                                                  .ForMember(dest => dest.ANGOBList, opt => opt.Ignore())
                                                  .ForMember(dest => dest.ANTOPTION, opt => opt.Ignore())
                                                  .ForMember(dest => dest.ANTOPTIONReference, opt => opt.Ignore())
                                                  .ForMember(dest => dest.LSADDReference, opt => opt.Ignore())
                                                  .ForMember(dest => dest.ANTPROVList, opt => opt.Ignore())
                                                  .ForMember(dest => dest.ANTSUBVList, opt => opt.Ignore())
                                                  .ForMember(dest => dest.ANTVSList, opt => opt.Ignore())
                                                  .ForMember(dest => dest.ANTZEKABLList, opt => opt.Ignore())
                                                  .ForMember(dest => dest.APPROVALList, opt => opt.Ignore())
                                                  .ForMember(dest => dest.BONITAETList, opt => opt.Ignore())
                                                  .ForMember(dest => dest.BRANDReference, opt => opt.Ignore())
                                                  .ForMember(dest => dest.GENEHM, opt => opt.Ignore())
                                                  .ForMember(dest => dest.GENEHMReference, opt => opt.Ignore())
                                                  .ForMember(dest => dest.KREMOList, opt => opt.Ignore())

                                                  .ForMember(dest => dest.ITKONTOREFList, opt => opt.Ignore())
                                                  .ForMember(dest => dest.ITRELATEList, opt => opt.Ignore())
                                                  .ForMember(dest => dest.KONTOREFList, opt => opt.Ignore())
                                                  .ForMember(dest => dest.PARTNERReference, opt => opt.Ignore())
                                                  .ForMember(dest => dest.PERSONReference, opt => opt.Ignore())
                                                  .ForMember(dest => dest.ZEKList, opt => opt.Ignore())
                                                  .ForMember(dest => dest.CAMPReference, opt => opt.Ignore())
                                                  .ForMember(dest => dest.CAMPTPReference, opt => opt.Ignore())
                                                  .ForMember(dest => dest.SLAList, opt => opt.Ignore())

                                                  // Ticket#2013012410000188 — Fehler beim Antrag kopieren
                                                  // CONTACTList ist wahrscheinlich nur für AKFCRM
                                                  .ForMember(dest => dest.CONTACTList, opt => opt.Ignore())
                                                  .ForMember(dest => dest.ANTOBList, opt => opt.Ignore())
                                                  .ForMember(dest => dest.ANTOBSICHList, opt => opt.Ignore())
                                                  .ForMember(dest => dest.CARDList, opt => opt.Ignore())
                                                  .ForMember(dest => dest.PRJOKERList, opt => opt.Ignore())
                                                  ;



                cfg.CreateMap<ANTOBSICH, ANTOBSICH>().ForMember(dest => dest.EntityKey, opt => opt.Ignore())
                                                        .ForMember(dest => dest.ANTOBReference, opt => opt.Ignore())
                                                        .ForMember(dest => dest.ANTRAGReference, opt => opt.Ignore())
                                                        .ForMember(dest => dest.ITReference, opt => opt.Ignore())
                                                        .ForMember(dest => dest.BLZReference, opt => opt.Ignore())
                                                        .ForMember(dest => dest.SICHTYPReference, opt => opt.Ignore());
            });*/
            ANTRAG antragOutput = new ANTRAG();
            ANTRAG antragInput = null;
            bool isLeaseNow = false;
            bool isDispo = false;
            bool isClassic = false;
            int? origERFASSUNGSCLIENT = null;

            IPrismaDao prismaDao = PrismaDaoFactory.getInstance().getPrismaDao();

            using (DdOlExtended context = new DdOlExtended())
            {
                antragInput = (from ant in context.ANTRAG where ant.SYSID == inputData.sysid select ant).FirstOrDefault();
                origERFASSUNGSCLIENT = antragInput.ERFASSUNGSCLIENT;
                //use kunde id if given 

                if (antragInput.PERSON == null)
                    context.Entry(antragInput).Reference(f => f.PERSON).Load();
                

                mapper.Map<ANTRAG, ANTRAG>(antragInput, antragOutput);
                if (!b2b)
                    antragOutput.ERFASSUNGSCLIENT = null;
                try
                {
					// Produkte werden anhand vart unterschieden: es gibt KREDIT, TZK, LEASE_now, CREDIT-now Dispo, DifferenzLeasing
					VART vart = (from v in context.VART where v.SYSVART == antragInput.SYSVART select v).FirstOrDefault ();

					// VertragsArt ermitteln
					String vArtCode = vart.CODE.ToLower ();
					isLeaseNow = (vArtCode.IndexOf ("leasing") > -1);				// LEASE-now
					isDispo = (vArtCode.IndexOf ("dispo") > -1) || (vArtCode.IndexOf ("flex") > -1); // CREDIT_now Dispo
					isClassic = (vArtCode.IndexOf ("classic") > -1);                // CREDIT_now Classic
				}
				catch (Exception) { }
                antragOutput.ADATUM = null;
                antragOutput.EDATUM = null;
                antragOutput.ZUSTANDAM = DateTime.Now;
                antragOutput.ZUSTAND = AntragZustand.Neu.ToString();
                antragOutput.ATTRIBUT = AntragAttribut.Neu.ToString();
                antragOutput.AKTIVKZ = aktivKz;
                antragOutput.ENDEKZ = endeKz;
                antragOutput.NOTSTOPFLAG = 0; //Ticket #6432
                // Ticket#2012061410000074 — Feld ANTRAG.ENDEAM beim Antrag Kopieren auf NULL setzen:
                antragOutput.ENDEAM = null;
                antragOutput.DATEINREICHUNG = null;
                antragOutput.DATEINREICHUNGZEIT = null;
                antragOutput.SYSBRAND=antragInput.SYSBRAND;

                // Ticket#2011081710000021 : Im Antrag muss zusätzlich das Feld sysls gefüllt werden. Hier muss immer der Wert 1 stehen.
                antragOutput.SYSLS = 1;
                antragOutput.SYSMWST = context.ExecuteStoreQuery<long>("select sysmwst from lsadd where syslsadd=" + antragOutput.SYSLS).FirstOrDefault();

                antragOutput.ANTRAG1 = NkBuilderFactory.createAntragNkBuilder().getNextNumber();
                antragOutput.FLAGFREIGABEAUSZ = 0;
                antragOutput.FLAGFREIGABEFORM = 0;

                context.ANTRAG.Add(antragOutput);

                context.SaveChanges();
            }

            ANTOB newAntOb = null;
            using (DdOlExtended context = new DdOlExtended())
            {
                antragOutput = (from ant in context.ANTRAG where ant.SYSID == antragOutput.SYSID select ant).FirstOrDefault();

                MyCopyZusatzdatenAntragID(context, inputData.sysid, antragOutput.SYSID);

                //ITKONTOREF
                ITKONTOREF ktoRefOrg = context.ITKONTOREF.Where(par => par.IT.SYSIT == antragInput.SYSIT).FirstOrDefault();
                if (ktoRefOrg != null)
                {
                    ITKONTOREF ktoRef = new ITKONTOREF();
                    ktoRef.ACTIVEFLAG = 1;
                    context.ITKONTOREF.Add(ktoRef);

                    // Verknüpfung zu IT
                    if (antragOutput.SYSIT.HasValue)
                    {
                        ktoRef.SYSIT=antragOutput.SYSIT.Value;
                    }
                    // Verknüpfung zu ANTRAG
                    ktoRef.SYSANTRAG= antragOutput.SYSID;

                    // Verknüpfung zu KONTO
                    if (inputData.sysItKonto > 0)
                    {
                        ktoRef.SYSITKONTO=inputData.sysItKonto;
                    }

                    ktoRef.KONTOTP = ktoRefOrg.KONTOTP;
                    ktoRef.BKONTOTP = ktoRefOrg.BKONTOTP;
                }

                //Object---------------------------------------------
                if (inputData.angAntObDto != null)
                {
                    newAntOb = new ANTOB();
                    context.ANTOB.Add(newAntOb);

                    ANTOB oldAntOb = (from ob in context.ANTOB where ob.SYSOB == inputData.angAntObDto.sysob select ob).FirstOrDefault();

                    mapper.Map<ANTOB, ANTOB>(oldAntOb, newAntOb);

                    newAntOb.OBTYP = context.OBTYP.Where(par => par.SYSOBTYP == inputData.angAntObDto.sysobtyp).FirstOrDefault();
                    // ANTOB_NEW.SYSOBART = ANTOB_OLD.SYSOBART
                    newAntOb.SYSOBART=oldAntOb.SYSOBART;
                    newAntOb.SYSANTRAG= antragOutput.SYSID;
                    newAntOb.SYSANTRAG = antragOutput.SYSID;

                    // in ANTOB:OBJEKT steht der Wert aus dem Nummernkreis ANTOB.....
                    // soll also analog zu ANTRAG.ANTRAG gezogen werden
                    newAntOb.OBJEKT = NkBuilderFactory.createAntobNkBuilder().getNextNumber();

                    //Brief----------------------------------------------
                    if (inputData.angAntObDto.brief != null)
                    {
                        ANTOBBRIEF brief = new ANTOBBRIEF();
                        context.ANTOBBRIEF.Add(brief);

                        mapper.Map<AngAntObBriefDto, ANTOBBRIEF>(inputData.angAntObDto.brief, brief);
                        brief.ANTOB = newAntOb;
                        if (brief.GETRIEBE != null)
                        {
                            if (brief.GETRIEBE.Length > 20)
                            {
                                brief.GETRIEBE = brief.GETRIEBE.Substring(0, 20);
                                _log.Debug("Brief.Getriebe wurde gekürzt auf 20 Zeichen beim Antrag: " + antragOutput.SYSID);
                            }
                        }
                    }

                    newAntOb.SYSANTRAG = antragOutput.SYSID;
                    newAntOb.OBJEKT = NkBuilderFactory.createAntobNkBuilder().getNextNumber();

                    context.SaveChanges();
                }

                // Object HD ---------------------------------------------
                // Ticket JOORMANN-2011-7-27-161610
                ANTOBHD antobhd = new ANTOBHD();
                //antobhd.ANTRAG = antragOutput;
                antobhd.SYSVT = antragOutput.SYSID;
                antobhd.ANTOB = newAntOb;
                antobhd.SYSHD = antragOutput.SYSVM;
                antobhd.ERFASSUNG = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null);
                context.ANTOBHD.Add(antobhd);

                context.SaveChanges();
            }

            using (DdOlExtended context = new DdOlExtended())
            {
                //Calculation----------------------------------------
                if (inputData.kalkulation != null)
                {
                    bool isDiffLeasing = false;

                    antragOutput = (from ant in context.ANTRAG where ant.SYSID == antragOutput.SYSID select ant).FirstOrDefault();
                    if (inputData.kalkulation.angAntKalkDto != null)
                    {
                        ANTKALK antkalkInput = (from ak in context.ANTKALK where ak.ANTRAG.SYSID == antragInput.SYSID select ak).FirstOrDefault();

                        double? CustZins = (double?)antkalkInput.ZINSCUST;
                        isDiffLeasing = prismaDao.isDiffLeasing((long)inputData.kalkulation.angAntKalkDto.sysprproduct); // Differenzleasing 


                        antragOutput.SYSPRPRODUCT = inputData.kalkulation.angAntKalkDto.sysprproduct;

                        ANTKALK antkalkOutput = new ANTKALK();
                        context.ANTKALK.Add(antkalkOutput);

                        mapper.Map<ANTKALK, ANTKALK>(antkalkInput, antkalkOutput);
                        mapper.Map<AngAntKalkDto, ANTKALK>(inputData.kalkulation.angAntKalkDto, antkalkOutput);
                        if (newAntOb != null)
                        {
                            antkalkOutput.SYSOB = newAntOb.SYSOB;
                        }

                        // antkalk.SYSKALK muss explizit gesetzt werden
                        antkalkOutput.SYSKALK = antragOutput.SYSID;

                        //antkalkOutput.ANTRAG = antragOutput;
                        antkalkOutput.SYSANTRAG = antragOutput.SYSID;
                        antkalkOutput.SYSPRPRODUCT = inputData.kalkulation.angAntKalkDto.sysprproduct;
                        antkalkOutput.OBUSETYPE = context.OBUSETYPE.Where(par => par.SYSOBUSETYPE == inputData.kalkulation.angAntKalkDto.sysobusetype).FirstOrDefault();

                        // Ticket#2012061410000074 : ANTKALK:SYSCREATE soll auf SYSDATE gesetzt werden
                        antkalkOutput.SYSCREATE = DateTime.Now;

                        
                         antkalkOutput.SYSWAEHRUNG = context.ExecuteStoreQuery<long>("select syshauswaehrung from lsadd where syslsadd=1", null).FirstOrDefault(); 

                        if (isDispo == true)
                        {
                            List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysprProduct", Value = inputData.kalkulation.angAntKalkDto.sysprproduct });
                            antkalkOutput.SYSINTSTRCT = context.ExecuteStoreQuery<long>(SYSINTSTRCTBYPRODUCTID, parameters.ToArray()).FirstOrDefault();
                        }

                        // Modus muss bei kredit 0, bei leasing 1 sein
                        if (isLeaseNow == true) { antkalkOutput.MODUS = 1; } else { antkalkOutput.MODUS = 0; }

                        // PPY (Raten pro Jahr): Konstant 12 bei bnow
                        antkalkOutput.PPY = 12;
                        if (isClassic == true)
                        {
                            antkalkOutput.AUSZAHLUNG = (decimal?)inputData.kalkulation.angAntKalkDto.auszahlung;
                            antkalkOutput.AUSZAHLUNGTYP = inputData.kalkulation.angAntKalkDto.auszahlungTyp;
                        }

                        if (isLeaseNow == true)
                        {
                            antkalkOutput.BGEXTERNUST = (decimal?)(inputData.kalkulation.angAntKalkDto.bgexternbrutto - inputData.kalkulation.angAntKalkDto.bgextern);
                            antkalkOutput.BGINTERNUST = (decimal?)(inputData.kalkulation.angAntKalkDto.bginternbrutto - inputData.kalkulation.angAntKalkDto.bgintern);
                            antkalkOutput.RATEUST = (decimal?)inputData.kalkulation.angAntKalkDto.rateUst;
                            antkalkOutput.AUSZAHLUNG = antkalkOutput.BGEXTERNUST;
                        }
                    }

                    //provisions-------------------------------------
                    foreach (AngAntProvDto prov in inputData.kalkulation.angAntProvDto)
                    {
                        ANTPROV antprov = new ANTPROV();
                        context.ANTPROV.Add(antprov);

                        mapper.Map<AngAntProvDto, ANTPROV>(prov, antprov);
                        // AntProv ist über AntProv.SysVT mit Antrag verbunden
                        //antprov.ANTRAG = antragOutput;
                        // in sysAntrag muss auch die Id stehen
                        antprov.SYSANTRAG = antragOutput.SYSID;
                        antprov.SYSVT = antragOutput.SYSID;
                        antprov.PRPROVTYPE = context.PRPROVTYPE.Where(par => par.SYSPRPROVTYPE == prov.sysprprovtype).FirstOrDefault();
                        //antprov.PARTNER = context.PARTNER.Where(par => par.SYSPARTNER == prov.syspartner).FirstOrDefault();
                        antprov.SYSPARTNER = prov.syspartner;
                        antprov.PROVISION = (decimal?)prov.provision;
                        antprov.PROVISIONP = (decimal?)prov.provisionPro;
                        antprov.PROVISIONBRUTTO = (decimal?)prov.provisionBrutto;

                        antprov.DEFPROVISION = (decimal?)prov.defaultprovision;
                        antprov.DEFPROVISIONBRUTTO = (decimal?)prov.defaultprovisionbrutto;
                        antprov.DEFPROVISIONP = (decimal?)prov.defaultprovisionp;
                        antprov.DEFPROVISIONUST = (decimal?)prov.defaultprovisionust;

                        int index = inputData.kalkulation.angAntProvDto.IndexOf(prov);
                        if (inputData.kalkulation.angAntProvDtoRapMin != null && inputData.kalkulation.angAntProvDtoRapMin.Count() > index)
                        {
                            // Provision für 5% Zinsen
                            antprov.RAPPROVISIONBRUTTOMIN = (decimal?)inputData.kalkulation.angAntProvDtoRapMin[index].provisionBrutto;
                        }
                        if (inputData.kalkulation.angAntProvDtoRapMax != null && inputData.kalkulation.angAntProvDtoRapMax.Count() > index)
                        {
                            // Provision für 15% Zinsen
                            antprov.RAPPROVISIONBRUTTOMAX = (decimal?)inputData.kalkulation.angAntProvDtoRapMax[index].provisionBrutto;
                        }

                        antprov.FLAGLOCKED = prov.flaglocked;
                        context.SaveChanges();
                    }
                    //insurances-------------------------------------
                    foreach (AngAntVsDto vs in inputData.kalkulation.angAntVsDto)
                    {
                        ANTVS antvs = new ANTVS();
                        context.ANTVS.Add(antvs);
                        mapper.Map<AngAntVsDto, ANTVS>(vs, antvs);

                        //antvs.ANTRAG = antragOutput;
                        antvs.SYSANTRAG = antragOutput.SYSID;
                        antvs.SYSVSTYP=vs.sysvstyp;

                        antvs.SYSVS = (from vstyp in context.VSTYP
                                       where vstyp.SYSVSTYP == vs.sysvstyp
                                       select vstyp.SYSVS).FirstOrDefault();
                        context.SaveChanges();
                    }
                    //subventions------------------------------------
                    foreach (AngAntSubvDto subv in inputData.kalkulation.angAntSubvDto)
                    {
                        ANTSUBV antsubv = new ANTSUBV();
                        context.ANTSUBV.Add(antsubv);
                        mapper.Map<AngAntSubvDto, ANTSUBV>(subv, antsubv);

                        //antsubv.ANTRAG = antragOutput;
                        antsubv.SYSANTRAG = antragOutput.SYSID;

                        if (isDiffLeasing == true)
                        {
                            antsubv.SUBVTYP = context.SUBVTYP.Where(par => par.SYSSUBVTYP == subv.syssubvtyp).FirstOrDefault();
                            antsubv.SYSSUBVG = subv.syssubvg;
                        }
                        context.SaveChanges();
                    }
                }

                context.SaveChanges();
            }

            using (DdOlExtended context = new DdOlExtended())
            {
                // Alle Sicherheiten sollen mitkopiert werden
                List<ANTOBSICH> antObSichList = (from antObSich in context.ANTOBSICH
                                                 where antObSich.SYSANTRAG == inputData.sysid
                                                 select antObSich).ToList();

                foreach (var oldAntObSich in antObSichList)
                {
                    ANTOBSICH newANTOBSICH = new ANTOBSICH();
                    context.ANTOBSICH.Add(newANTOBSICH);

                    mapper.Map(oldAntObSich, newANTOBSICH);

                    if (newAntOb != null)
                        newANTOBSICH.SYSOB = newAntOb.SYSOB;
                    newANTOBSICH.SYSSICHTYP=oldAntObSich.SYSSICHTYP;
                    newANTOBSICH.SYSANTRAG=antragOutput.SYSID;
                    newANTOBSICH.SYSANTRAG = antragOutput.SYSID;
                    newANTOBSICH.SYSPERSON = oldAntObSich.SYSPERSON ?? 0;

                    if (inputData.mitantragsteller != null)
                    {
                        newANTOBSICH.SYSIT=inputData.mitantragsteller.sysit;
                        if (inputData.mitantragsteller.syskd > 0)
                        {
                            newANTOBSICH.SYSPERSON = inputData.mitantragsteller.syskd;
                        }
                    }
                }

                // Ticket#2012010910000128 
                // in der PEUNI muss die sysperole des Verkäufers (Mitarbeiters) gespeichert werden, anstatt der des Benutzers
                long sysPeRoleForSysVK = (from perole in context.PEROLE
                                          where perole.SYSPERSON == antragOutput.SYSVK
                                          select perole.SYSPEROLE).FirstOrDefault();
                if ((antragOutput.SYSVK == null || antragOutput.SYSVK == 0) && sysPeRoleForSysVK == 0)
                {
                    // Ticket#2012050710000035 : Wenn sysvk 0 ist, wird die sysperole aus dem alten Antrag übernommen
                    // Security Check: Aufruf nur mit long
                    sysPeRoleForSysVK = context.ExecuteStoreQuery<long>("Select sysperole from peuni where area='ANTRAG' and sysid=" + antragInput.SYSID, null).FirstOrDefault();
                }

                PEUNIHelper.ConnectNodes(context, PEUNIArea.ANTRAG, antragOutput.SYSID, sysPeRoleForSysVK);

                context.SaveChanges();
            }

            // Ticket#2012013010000132 : MAClient: bei copyAntrag sollen gar keine Memos zu Anträgen kopiert werden
            if (b2b)
            {
                // Ticket#2012072310000126 — CR16425 (Ablösen): Memo-Erzeugung
                if (origERFASSUNGSCLIENT != ERFASSUNGSCLIENT_B2C && origERFASSUNGSCLIENT != ERFASSUNGSCLIENT_ONE && origERFASSUNGSCLIENT != ERFASSUNGSCLIENT_DMR)
                    MySaveNotizen(antragOutput.SYSID, inputData, antragOutput.SYSVK);
            }

            return getAntrag(antragOutput.SYSID, sysperole);
        }

        /// <summary>
        /// Opens getAntrag with person or it
        /// </summary>
        /// <param name="sysid"></param>
        /// <param name="sysperole"></param>
        /// <returns></returns>
        public AntragDto getAntrag(long sysid, long sysperole)
        {
            double start = DateTime.Now.TimeOfDay.TotalMilliseconds;
            AntragDto rval = getAntragSwitch(sysid, isb2b, sysperole);
            _log.Debug("getAntrag: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));
            return rval;
        }

        /// <summary>
        /// Get Antrag from DB
        /// </summary>
        /// <param name="sysid">Sysid</param>
        /// <param name="b2b">B2B-Flag</param>
        /// <param name="sysperole">sysperole</param>
        /// <returns>Antrag</returns>
        private AntragDto getAntragSwitch(long sysid, bool b2b, long sysperole)
        {

            IMapper mapper = Mapper.Instance;/* Cic.One.Utils.Util.Mapper.MapperConfig.getInstanceMapper("getAntragSwitch", delegate (MapperConfigurationExpression cfg) {

                cfg.CreateMap<PERSON, KundeDto>();
                cfg.CreateMap<ANTOB, AngAntObDto>()
                    .ForMember(dest => dest.brief, opt => opt.Ignore());
                cfg.CreateMap<ANTOBBRIEF, AngAntObBriefDto>();
                cfg.CreateMap<ANTKALK, AngAntKalkDto>()
                    //.ForMember(dest => dest.sysantrag, opt => opt.MapFrom(src => src.ANTRAG.SYSID))
                    .ForMember(dest => dest.calcRsvgesamt, opt => opt.MapFrom(src => src.RSVGESAMT))
                    .ForMember(dest => dest.calcZinskosten, opt => opt.MapFrom(src => src.ZINSKOSTEN))
                    .ForMember(dest => dest.calcRsvmonat, opt => opt.MapFrom(src => src.RSVMONAT))
                    .ForMember(dest => dest.calcRsvzins, opt => opt.MapFrom(src => src.RSVZINS))
                    .ForMember(dest => dest.calcUstzins, opt => opt.MapFrom(src => src.USTZINS));
            });*/
            
            using (DdOlExtended context = new DdOlExtended())
            {
                
                DbConnection con = (context.Database.Connection);
                AntragDto rval = con.Query<AntragDto>("select antrag.*,antrag.sysprproduct sysprprod from antrag where sysid=:sysid", new { sysid = sysid }).FirstOrDefault();

                //get data for extended contract,if any
                rval.options = con.Query<AngAntOptionDto>("select * from antoption where sysid=:sysid", new { sysid = sysid }).FirstOrDefault();

                rval.KNEBestaetigung = con.Query<String>("select coderelatekind from itkne where area='ANTRAG' and bezeichnung='Kontrollinhaber' and sysarea=:sysid",new { sysid }).FirstOrDefault();

                if (rval.KNEBestaetigung == null)
                    rval.KNEBestaetigung = "0";
                else if ("PARTICIPATION".Equals(rval.KNEBestaetigung))
                    rval.KNEBestaetigung = "1";
                else if ("OTHER".Equals(rval.KNEBestaetigung))
                    rval.KNEBestaetigung = "2";
                else if ("HIGHEST_DECISION_MAKER".Equals(rval.KNEBestaetigung) || "CEO".Equals(rval.KNEBestaetigung))
                    rval.KNEBestaetigung = "3";

                String vsextQuery = new EaiparDao().getEaiParFileByCode("EXT_VS_VERFUEGBAR_ANTRAG", "select 0 from dual");
                vsextQuery = vsextQuery.Replace(":sysperole", "" + sysperole);

                rval.extvscode = con.Query<String>("select ( "+vsextQuery+ ") from ANTRAG where ANTRAG.sysid=:sysid", new { sysid }).FirstOrDefault();



                //KNE read CRIFOK-Flag
                //Flag für gültige Auskunft, wenn true ist keine erneute CRIF-Anfrage notwendig
                rval.CRIFOK = 0;
                KundeDto crifcheck = con.Query<KundeDto>("select crrsid crefoid, FLAG01 feststellungsPflicht from it,itoption where it.sysit = itoption.sysit(+) and it.sysit=:sysit", new { sysit = rval.sysit }).FirstOrDefault();
                if(crifcheck!=null && crifcheck.crefoid!=null && crifcheck.feststellungsPflicht.HasValue && crifcheck.crefoid.Length>0)
                {
                    int? statusnum = con.Query<int?>("select statusnum from auskunft,auskunfttyp where auskunfttyp.sysauskunfttyp=auskunft.sysauskunfttyp and area='ANTRAG' and sysid=:sysid and auskunfttyp.bezeichnung=:atyp order by auskunft.sysauskunft desc", new { sysid = rval.sysid,atyp=CRIFBo.AUSKUNFTTYP }).FirstOrDefault();
                    if(statusnum.HasValue && statusnum.Value==0)
                    {
                        rval.CRIFOK = 1;
                    }

                }

                AntragDto tmpAntrag = con.Query<AntragDto>(QUERY_VORVT, new { sysid = sysid }).FirstOrDefault();
                rval.sysvorvt = tmpAntrag.sysvorvt;
                rval.contractextcount = tmpAntrag.contractextcount;
                rval.nrvorvt = tmpAntrag.nrvorvt;
                rval.sysvt = tmpAntrag.sysvt;
                IRounding round = RoundingFactory.createRounding();

                //get the custom values for ratevorvt,rwvorvt, zubehoervorvt same way as in AngAntBo.createExtendedContract
                if(tmpAntrag.sysvorvt>0)
                { 
                    VertragDto vorvertrag = BOFactory.getInstance().createVertragBo().getVertragForExtension(tmpAntrag.sysvorvt);
                    rval.rwvorvt = vorvertrag.rwvorvt;
                    rval.zubehoervorvt = vorvertrag.zubehoervorvt;
					if (vorvertrag.kalkulation.angAntAblDto != null && vorvertrag.kalkulation.angAntAblDto.Count > 0)
					{
						//// // rh: ALT: GET netto Rate
						// rval.ratevorvt = round.RoundCHF (vorvertrag.kalkulation.angAntAblDto[0].aktuelleRate);

						// rh 20170222: neu: // rh: GET brutto Rate: Add MWSt:
						double mwst = 0.0;
						double aktuelleRate = vorvertrag.kalkulation.angAntAblDto[0].aktuelleRate;

						mwst = BOFactory.getInstance ().createVertragBo ().getMWST (tmpAntrag.sysvorvt, DateTime.Today);	// rh: GET aktuelle MWSt
						rval.ratevorvt = round.RoundCHF (round.getGrossValue (aktuelleRate, mwst));							// rh: GET brutto Rate
					}


                }
               
                if (b2b)
                    rval.syskd = null;
               

                String fform = getFform(sysid);
                if (fform != null && (fform).Trim() != "")
                {
                    
                    // FIX BNRELF-1606
                    if (rval.contractextcount >0)
                        rval.prozesscode = "RWV";
                    else
                        rval.prozesscode = "UMSCHREIBUNG";
                        
                }
                
                //AngAntObDto

                
                //Calculation--------------------------------------------      
                ANTKALK antkalk = context.ANTKALK.Where(par => par.ANTRAG.SYSID == sysid).FirstOrDefault();
                
                rval.kalkulation = new KalkulationDto();
                rval.kalkulation.angAntKalkDto = mapper.Map<ANTKALK, AngAntKalkDto>(antkalk);
                if (antkalk != null)
                {
                    rval.kalkulation.angAntKalkDto.sysprproduct = antkalk.SYSPRPRODUCT.GetValueOrDefault();
                    rval.kalkulation.angAntKalkDto.sysobusetype = antkalk.SYSOBUSETYPE.GetValueOrDefault();
                    rval.kalkulation.angAntKalkDto.sysantrag = sysid;
                }

                //Object-------------------------------------------------
                if (rval.angAntObDto == null)
                {
                    ANTOB antob = context.ANTOB.Where(par => par.SYSANTRAG == sysid).FirstOrDefault();
                    if (antob != null)
                    {
                        
                        rval.angAntObDto = mapper.Map<ANTOB, AngAntObDto>(antob);
                        rval.angAntObDto.sysnkk = antob.SYSNKK.GetValueOrDefault();
                        rval.angAntObDto.sysobart = antob.SYSOBART.GetValueOrDefault();
                        rval.angAntObDto.sysobtyp = antob.SYSOBTYP.GetValueOrDefault();
                        rval.angAntObDto.typengenehmigung = antob.GEHNID;
                    }
                    ANTOBBRIEF antobbrief = context.ANTOBBRIEF.Where(par => par.ANTOB.SYSANTRAG == sysid).FirstOrDefault();
                    if (antobbrief != null)
                    {
                        rval.angAntObDto.brief = mapper.Map<ANTOBBRIEF, AngAntObBriefDto>(antobbrief);
                        rval.angAntObDto.brief.aufbau = antobbrief.AART;
                        //CRMGT00027719 eCode178

                        if (String.IsNullOrEmpty(antobbrief.ECODEID) && String.IsNullOrEmpty(antobbrief.ECODESTATUS))
                        {

                            List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysAntrag", Value = sysid });

                            long eCodeAnmeldenErrCode = context.ExecuteStoreQuery<long>(GETECODEANMELDENERRCODE, parameters.ToArray()).FirstOrDefault();

                            if (eCodeAnmeldenErrCode == 7)
                            {
                                rval.angAntObDto.brief.ecodeid = "";
                                rval.angAntObDto.brief.ecodestatus = "INUSE";
                            }

                        }

                        else
                        {
                            rval.angAntObDto.brief.ecodeid = antobbrief.ECODEID;
                            rval.angAntObDto.brief.ecodestatus = antobbrief.ECODESTATUS;
                            if (antobbrief.ECODESTATUS == "A")
                            {
                                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysAntrag", Value =sysid });

                                long eCodeAnmeldenReturnCode = context.ExecuteStoreQuery<long>(GETECODEANMELDENERRCODE, parameters.ToArray()).FirstOrDefault();

                                if (eCodeAnmeldenReturnCode == 56)
                                {
                                    rval.angAntObDto.brief.ecodeid = "";
                                    rval.angAntObDto.brief.ecodestatus = "RET56";
                                }
                            }
                        }
                    }
                }

                //SatzMehrKm---------------------------------------------
                if (rval.kalkulation.angAntKalkDto != null && rval.angAntObDto != null)
                {
                    rval.kalkulation.angAntKalkDto.satzmehrkm = Math.Round((rval.angAntObDto.zubehoerBrutto + rval.angAntObDto.grundBrutto) * rval.angAntObDto.satzmehrKm / 100, 2);
                }

               

                rval.kalkulation.angAntProvDto = con.Query<AngAntProvDto>("select * from antprov where sysantrag=:sysid", new { sysid = sysid }).ToList();

              
                rval.kalkulation.angAntSubvDto = con.Query<AngAntSubvDto>("select * from antsubv where sysantrag=:sysid", new { sysid = sysid }).ToList();

                //Insurances---------------------------------------------
                rval.kalkulation.angAntVsDto = con.Query<AngAntVsDto>("select * from antvs where sysantrag=:sysid and sysvstyp is not null and sysvstyp>0", new { sysid = sysid }).ToList();

                if (rval.kalkulation.angAntVsDto != null && rval.kalkulation.angAntVsDto.Count > 0)
                {
                    OpenOne.Common.BO.Versicherung.IInsuranceBo insbo = Cic.OpenOne.GateBANKNOW.Common.BO.BOFactory.getInstance().createInsuranceBO();
                    foreach (AngAntVsDto avs in rval.kalkulation.angAntVsDto)
                    {
                        insbo.updateServiceType(avs);
                    }
                }
                // Ablösen ---------------------------------------------
                List<ANTABL> antAblList = context.ANTABL.Where(par => par.SYSANTRAG == sysid).ToList<ANTABL>();
                if (antAblList.Count > 0)
                {
                    rval.kalkulation.angAntAblDto = new List<AngAntAblDto>();
                    foreach (var antAbl in antAblList)
                    {
                        AngAntAblDto antAblDto = mapper.Map<ANTABL, AngAntAblDto>(antAbl);

                        antAblDto.sysvorvt = antAbl.SYSVORVT.GetValueOrDefault();
                        antAblDto.sysabltyp = antAbl.SYSABLTYP.GetValueOrDefault();

                        // Ticket#2012081310000214 Kein Bankname wird in den Details angezeigt
                        if (String.IsNullOrEmpty(antAblDto.bank) && antAblDto.sysabltyp == ABLTYPEIGEN)
                        {
                            antAblDto.bank = EIGENEBANK;
                        }

                        rval.kalkulation.angAntAblDto.Add(antAblDto);
                    }
                }

                // 
                ANTOBSICH antobsich = (from obsich in context.ANTOBSICH
                                       where obsich.SYSANTRAG == sysid && obsich.IT != null
                                       select obsich).FirstOrDefault();
                if (antobsich != null)
                {
                    
                    if (antobsich.IT == null)
                        context.Entry(antobsich).Reference(f => f.IT).Load();
                    rval.mitantragsteller = mapper.Map<IT, KundeDto>(antobsich.IT);
                }

                PERSON person = null;
                if (!b2b)
                {
                   
                    person = (from kd in context.PERSON
                              where kd.SYSPERSON == rval.syskd
                              select kd).FirstOrDefault();
                }
                if (person != null)
                {
                    rval.kunde = mapper.Map<PERSON, KundeDto>(person);
                    rval.kunde.syskd = person.SYSPERSON;
                    if (person.KDTYP == null)
                        context.Entry(person).Reference(f => f.KDTYP).Load();
                    
                    if (person.KDTYP != null)
                    {
                        rval.kunde.syskdtyp = person.KDTYP.SYSKDTYP;
                        rval.kunde.kdtypBezeichnung = person.KDTYP.NAME;
                    }
                }
                else
                {
                    IT interessent = (from it in context.IT
                                      where it.SYSIT == rval.sysit
                                      select it).FirstOrDefault();
                    if (interessent != null)
                    {
                        rval.kunde = mapper.Map<IT, KundeDto>(interessent);
                        rval.kunde.syskdtyp = 0;
                        if (interessent.SYSKDTYP != null)
                        {
                            rval.kunde.syskdtyp = (long)interessent.SYSKDTYP;
                        }
                        rval.kunde.kdtypBezeichnung = (from kdTyp in context.KDTYP
                                                       where kdTyp.SYSKDTYP == rval.kunde.syskdtyp
                                                       select kdTyp.NAME).FirstOrDefault();
                    }

                    
                    //TODO warum hier nicht alle Kundendaten, einheitlich wie überall anstatt es von Hand zu mappen?
                    //IKundeDao kundeDao = CommonDaoFactory.getInstance().getKundeDao();
                    //getKundeViaAntragID(antrag.kunde.sysit, sysantrag);
                }

                rval.zustand = getAntragZustand(rval.sysid);
                

                AntragDto antragDtoNotizen = MyGetAntragFromNotiz(NOTIZKAT, rval.sysid);
                rval.zusammenfassung = antragDtoNotizen.zusammenfassung;
                antragDtoNotizen = MyGetAntragFromNotiz(NOTIZKAT_ABLOESEN, rval.sysid);
                rval.abloese1 = antragDtoNotizen.abloese1;
                rval.abloese2 = antragDtoNotizen.abloese2;
                rval.abloese3 = antragDtoNotizen.abloese3;

                rval.mitantragstellerTyp = (from antObSich in context.ANTOBSICH
                                            where (antObSich.RANG == (int)MitantragstellerTyp.Solidarschuldner ||
                                                    antObSich.RANG == (int)MitantragstellerTyp.Bürge ||
                                                    antObSich.RANG == (int)MitantragstellerTyp.Partner) &&
                                                  antObSich.SYSANTRAG == rval.sysid
                                            select (int)antObSich.RANG).FirstOrDefault();

                var sysprjoker = (from j in context.PRJOKER
                                  where j.ANTRAG.SYSID == sysid
                                  select j.SYSPRJOKER).FirstOrDefault();


                rval.sysprjoker = sysprjoker;


                rval.zustandBemerkung = getAntragBemerkung(sysid, "ANTRAG", "ANTRAGSZUSTAND", 1);
                rval.emboss = context.ExecuteStoreQuery<String>("select emboss from card where sysantrag=" + sysid, null).FirstOrDefault();

                getCasaDiplomaSpecialFields(rval, "ANTRAG", sysid);

                //Ausstatung
                var antobID = (from a in context.ANTOB
                               where a.SYSANTRAG == sysid
                               select a.SYSOB).FirstOrDefault();
                if (antobID > 0)
                {
                    List<ANTOBAUST> aus = (from au in context.ANTOBAUST
                                           where au.SYSANTOB == antobID
                                           select au).ToList();
                    if (aus.Count > 0)
                    {
                        List<AngAntObAustDto> ausstatung = new List<AngAntObAustDto>();

                        foreach (ANTOBAUST item in aus)
                        {
                            AngAntObAustDto angAntObAustDto = mapper.Map<ANTOBAUST, AngAntObAustDto>(item);
                            string s = (from textuni in context.TEXTUNI
                                        where textuni.AREA == "ANTOBAUST" && textuni.SYSID == item.SYSOBAUST
                                        select textuni.TEXTVALUE).FirstOrDefault();
                            if (s != null && s.Contains("\n"))
                                s = s.Replace("\n", "<BR>");
                            angAntObAustDto.freitext2 = s;
                            ausstatung.Add(angAntObAustDto);
                        }

                        rval.angAntObDto.aust = ausstatung;
                    }

                }
                rval.auszahlungsdatum = getAuszahlungsdatum(rval.sysid);
                rval.ProFinLock = getProFinLock(rval.sysid);
                //Abwicklungsort
                if (rval.sysAbwicklung > 0)
                {
                    var abwicklung = (from p in context.PEROLE
                                      where p.SYSPEROLE == rval.sysAbwicklung
                                      select p.NAME).FirstOrDefault();
                    if (abwicklung != null)
                    {
                        rval.abwicklungsort = abwicklung;
                    }
                }

                rval.extident = context.ExecuteStoreQuery<String>("select extidentvalue from extident where codeextidenttyp='" + CODE_EXTIDENT_ANTRAGSID_B2C + "' and area='ANTRAG' and sysarea=" + sysid + " order by case when source='30' then 100 else to_number(source) end").FirstOrDefault();

                return rval;
            }
        }




        /// <summary>
        /// Antrag aktualisieren
        /// </summary>
        /// <param name="antragInput">Antrag Eingang</param>
        /// <param name="sysperole">sysperole</param>
        /// <returns>Antrag Ausgang</returns>
        public AntragDto updateAntrag(AntragDto antragInput, long sysperole)
        {
            long sysob = 0;
            double start = DateTime.Now.TimeOfDay.TotalMilliseconds;
            using (DdOlExtended context = new DdOlExtended())
            {
                //Mapper.CreateMap<AngAntObDto, ANTOB>().ForAllMembers(opt => opt.Condition(srs => !srs.IsSourceValueNull));

                _log.Debug("A: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start)); start = DateTime.Now.TimeOfDay.TotalMilliseconds;

                ANTRAG antragOutput = (from ant in context.ANTRAG
                                       where ant.SYSID == antragInput.sysid
                                       select ant).FirstOrDefault();

                //use kunde id if given 
                if (antragInput.kunde != null)
                    antragInput.sysit = antragInput.kunde.sysit;
                antragInput.aenderung = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null);
                antragInput.erfassung = antragOutput.ERFASSUNG;

                if (antragInput.zustand != antragOutput.ZUSTAND)
                {
                    antragInput.zustandAm = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null);
                }
                antragInput.zustand = antragOutput.ZUSTAND;             //because we use a different state in the gui changed upon loading!
                _log.Debug("B: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start)); start = DateTime.Now.TimeOfDay.TotalMilliseconds;
                //prevent removal of this parameters when not in Input
                if (antragInput.syscamp == 0)
                    antragInput.syscamp = antragOutput.SYSCAMP.GetValueOrDefault();
                if (!antragInput.syskd.HasValue || antragInput.syskd.Value == 0)
                    antragInput.syskd = antragOutput.SYSKD.GetValueOrDefault();
                if (antragInput.syscamptp == 0)
                    antragInput.syscamptp = antragOutput.SYSCAMPTP.GetValueOrDefault();
                Mapper.Map<AntragDto, ANTRAG>(antragInput, antragOutput);

                // Ticket#2012080910000035, Ticket#2012081510000041 
                if (antragInput.erfassungsclient > 0)
                {
                    // antragInput.erfassungsclient wird nur bei processAngebotToAntrag im Dto übergeben
                    antragOutput.ERFASSUNGSCLIENT = (int?)antragInput.erfassungsclient;
                }
                else
                {
                    // createAntrag wird nur vom B2B aufgerufen, MAClient verwendet Clarion
                    antragOutput.ERFASSUNGSCLIENT = ERFASSUNGSCLIENT_B2B;
                }

                //KNE save to antoption
                ANTOPTION ao = (from antopt in context.ANTOPTION
                                where antopt.SYSID == antragInput.sysid
                                select antopt).FirstOrDefault();

                //alle beziehungen aktualisieren
                String coderelkind = "HIGHEST_DECISION_MAKER";
                if("1".Equals(antragInput.KNEBestaetigung))
                    coderelkind = "PARTICIPATION";
                if ("2".Equals(antragInput.KNEBestaetigung))
                    coderelkind = "OTHER";
                if ("3".Equals(antragInput.KNEBestaetigung))
                    coderelkind = "HIGHEST_DECISION_MAKER";

                List < Devart.Data.Oracle.OracleParameter> parski = new List<Devart.Data.Oracle.OracleParameter>();
                parski.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "code", Value = coderelkind });
                parski.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = antragInput.sysid });

                context.ExecuteStoreCommand("update itkne set coderelatekind=:code where area='ANTRAG' and bezeichnung='Kontrollinhaber' and sysarea=:sysid", parski.ToArray());
 

                //Produkte werden anhand vart unterschieden: es gibt KREDIT, TZK, LEASE_now, CREDIT-now Dispo, DifferenzLeasing
                bool isLeaseNow = false;
                bool isDiffLeasing = false;
                bool isDispo = false;
                bool isClassic = false;
                bool istzk = false;//Teilzahlungskauf ist analog Kredit zu rechnen
                bool isCredit = false;
                bool isCard = false;
                String vttypCode = null;
                try
                {
                    if (antragInput.kalkulation != null && antragInput.kalkulation.angAntKalkDto != null)
                    {
                        //Produkte werden anhand vart unterschieden: es gibt KREDIT, TZK, LEASE_now, CREDIT-now Dispo, DifferenzLeasing
                        IPrismaDao prismaDao = PrismaDaoFactory.getInstance().getPrismaDao();
                        isDiffLeasing = prismaDao.isDiffLeasing((long)antragInput.kalkulation.angAntKalkDto.sysprproduct);

                        CIC.Database.PRISMA.EF6.Model.VART vart = prismaDao.getVertragsart((long)antragInput.kalkulation.angAntKalkDto.sysprproduct);
						CIC.Database.PRISMA.EF6.Model.VTTYP vtt = prismaDao.getVttyp((long)antragInput.kalkulation.angAntKalkDto.sysprproduct);
                     

                        antragOutput.SYSPRPRODUCT = antragInput.kalkulation.angAntKalkDto.sysprproduct;
                        if (vtt != null)
                        {
                            vttypCode = vtt.CODE;
                            antragOutput.SYSVTTYP = vtt.SYSVTTYP;
                        }
                        else
                            antragOutput.SYSVTTYP = null;

                        // VertragsArt ermitteln
                        if (vart != null)
                        {
                            antragOutput.SYSVART = vart.SYSVART;

                            String vArtCode = vart.CODE.ToLower();
                            isLeaseNow = (vArtCode.IndexOf("leasing") > -1);            // LEASE-now
                            isDispo = (vArtCode.IndexOf("dispo") > -1) || (vArtCode.IndexOf("flex") > -1);  // CREDIT_now Dispo
                            isClassic = (vArtCode.IndexOf("classic") > -1);         // CREDIT_now Classic
                            isCard = (vArtCode.IndexOf("kredit_dispoplus") > -1);       // CREDIT_now Card
                            istzk = (vArtCode.IndexOf("TZK") > -1);
                            isCredit = (vArtCode.IndexOf("KREDIT") > -1) || istzk;
                        }
                    }
				}
				catch (Exception ea)
                {
                    _log.Warn("Error writing back SYSPRPRODUCT/SYSVART to ANTRAG", ea);
                }
                _log.Debug("C: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start)); start = DateTime.Now.TimeOfDay.TotalMilliseconds;
                // Neue Felder--------------------------------------------

                if (antragOutput.ERFASSUNGSCLIENT != 1)//HCE - no update of these fields
                {
                    // antragOutput.SYSVK (Verkäufer) = PERSON von sysperole = Mitarbeiter
                    antragOutput.SYSVK = (from perole in context.PEROLE
                                          where perole.SYSPEROLE == sysperole
                                          select perole.SYSPERSON).FirstOrDefault();

                    //antragOutput.SYSVM (Vermittler) = Person von Händler (Parent vom Verkäufer)
                    antragOutput.SYSVM = (from parentPeRole in context.PEROLE
                                          join perole in context.PEROLE on parentPeRole.SYSPARENT equals perole.SYSPEROLE
                                          where parentPeRole.SYSPEROLE == sysperole
                                          select perole.SYSPERSON).FirstOrDefault();
                }
                antragOutput.SYSPRHGROUP = 0;
                try
                {
                    // Handelsgruppe wird in solveKalkulation berechnet 
                    antragOutput.SYSPRHGROUP = antragInput.kalkulation.angAntKalkDto.sysprhgrp;
                }
                catch (Exception) { }
                _log.Debug("C1: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start)); start = DateTime.Now.TimeOfDay.TotalMilliseconds;
                // Brand wird anhand der Handelsgruppe ermittelt
                antragOutput.SYSBRAND = (from prbrandm in context.PRBRANDM
                                      join brand in context.BRAND on prbrandm.BRAND.SYSBRAND equals brand.SYSBRAND
                                      where prbrandm.PRHGROUP.SYSPRHGROUP == antragOutput.SYSPRHGROUP
                                      select prbrandm.SYSBRAND).FirstOrDefault();
                _log.Debug("C2: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start)); start = DateTime.Now.TimeOfDay.TotalMilliseconds;
                // AbwicklungsOrt (PeRole) ist dem Händler zugeordnet
                antragOutput.SYSABWICKLUNG = MyGetHaendlerAbwicklungsort(antragOutput.SYSVM);

                _log.Debug("C3: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start)); start = DateTime.Now.TimeOfDay.TotalMilliseconds;
                // Ticket#2011081710000021 : Im Antrag muss zusätzlich das Feld sysls gefüllt werden. Hier muss immer der Wert 1 stehen.
                antragOutput.SYSLS = 1;

                // MwSt
                if (isLeaseNow == true)
                {
                    antragOutput.SYSMWST = context.ExecuteStoreQuery<long>("select sysmwst from lsadd where syslsadd=" + antragOutput.SYSLS).FirstOrDefault();
                }
                _log.Debug("C4: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start)); start = DateTime.Now.TimeOfDay.TotalMilliseconds;

                if (!String.IsNullOrEmpty(antragInput.extident))
                {
                    ExtidentDto exti = new ExtidentDto();
                    exti.area = "ANTRAG";
                    exti.extidentvalue = antragInput.extident;
                    exti.sysarea = antragOutput.SYSID;
                    exti.source = "" + antragOutput.ERFASSUNGSCLIENT;
                    exti.codeextidenttyp = CODE_EXTIDENT_ANTRAGSID_B2C;
                    createOrUpdateExtident(exti);
                }

                long sysangebot = context.ExecuteStoreQuery<long>("select sysid from angebot where angebot='" + antragOutput.ANTRAG1 + "'", null).FirstOrDefault();
                
                SetAntragIdInZusatzdaten(context, antragInput.kunde, antragInput.sysid);
                bool hasKrankenkasse = createOrUpdateKREMOAntrag(sysangebot, antragOutput.SYSID, antragInput.kunde, antragInput.mitantragsteller);
                

                _log.Debug("D: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start)); start = DateTime.Now.TimeOfDay.TotalMilliseconds;
                //Object
                bool deleteObaust = false;
                if (antragInput.angAntObDto != null)
                {
                    fixAHK(antragInput.angAntObDto, antragInput.sysprchannel);
                    ANTOB antob = (from a in context.ANTOB
                                   where a.SYSANTRAG == antragInput.sysid
                                   select a).FirstOrDefault();
                    

                    
                    if (antob.OBTYP == null)
                        context.Entry(antob).Reference(f => f.OBTYP).Load();
                    if (antob != null && antob.OBTYP != null && antob.OBTYP.SYSOBTYP != antragInput.angAntObDto.sysobtyp)
                        deleteObaust = true;

                    if (antob == null)
                    {
                        antob = new ANTOB();
                        antob.OBJEKT = NkBuilderFactory.createAntobNkBuilder().getNextNumber();
                        context.ANTOB.Add(antob);
                        
                    }

                    //Mapper.CreateMap<AngAntObDto, ANTOB>().ForMember(dest => dest.SYSOB, opt => opt.Ignore());
                    Mapper.Map<AngAntObDto, ANTOB>(antragInput.angAntObDto, antob);
                    antob.SYSANTRAG = antragInput.sysid;
                    antob.OBART = context.OBART.Where(par => par.SYSOBART == antragInput.angAntObDto.sysobart).FirstOrDefault();
                    antob.OBTYP = context.OBTYP.Where(par => par.SYSOBTYP == antragInput.angAntObDto.sysobtyp).FirstOrDefault();
                    antob.GEHNID = antragInput.angAntObDto.typengenehmigung;
                    if (antob.BEZEICHNUNG != null && antob.BEZEICHNUNG.Length > 40)
                        antob.BEZEICHNUNG = antob.BEZEICHNUNG.Substring(0, 40);

                    CIC.Database.PRISMA.EF6.Model.VART vart = null;
                    CIC.Database.PRISMA.EF6.Model.VTTYP vtt = null;

                    IPrismaDao prismaDao = PrismaDaoFactory.getInstance().getPrismaDao();
                    if (antragInput.kalkulation != null)
                    {
                        vart = prismaDao.getVertragsart((long)antragInput.kalkulation.angAntKalkDto.sysprproduct);
                        if (vart != null)
                            antragOutput.SYSVART = vart.SYSVART;
                        vtt = prismaDao.getVttyp((long)antragInput.kalkulation.angAntKalkDto.sysprproduct);
                        if (vtt != null)
                            antragOutput.SYSVTTYP = vtt.SYSVTTYP;
                        else
                            antragOutput.SYSVTTYP = null;

                        antragOutput.SYSPRPRODUCT = antragInput.kalkulation.angAntKalkDto.sysprproduct;
                    }
                    if (antragOutput.ERFASSUNGSCLIENT == ERFASSUNGSCLIENT_B2B || antragOutput.ERFASSUNGSCLIENT == ERFASSUNGSCLIENT_B2C)
                        // Vertriebsweg 
                        antragOutput.VERTRIEBSWEG = MyGetVertriebsWeg(antragOutput.SYSPRCHANNEL, antragOutput.ERFASSUNGSCLIENT.Value, antragInput.vertriebsweg);

                    context.SaveChanges();

                    //BNR10 OBAUS
                    if (deleteObaust)
                        deleteAntObAust(antob.SYSOB);
                    updateAngAntObAust(antragInput.angAntObDto.aust, antob.SYSOB);



                    sysob = antob.SYSOB;

                    // Object HD ---------------------------------------------
                    ANTOBHD antobhd = (from obhd in context.ANTOBHD
                                       where obhd.ANTRAG.SYSID == antragInput.sysid
                                       select obhd).FirstOrDefault();
                    if (antobhd == null)
                    {
                        // Ticket JOORMANN-2011-7-27-161610
                        antobhd = new ANTOBHD();
                        context.ANTOBHD.Add(antobhd);
                    }
                    //antobhd.ANTRAG = antragOutput;
                    antobhd.SYSVT = antragOutput.SYSID;
                    antobhd.ANTOB = antob;
                    antobhd.SYSHD = antragOutput.SYSVM;
                    
                    //Brief
                    if (antragInput.angAntObDto.brief != null)//brief update
                    {

                        
                        if (antob.ANTOBBRIEF == null)
                            context.Entry(antob).Reference(f => f.ANTOBBRIEF).Load();
                        ANTOBBRIEF brief = antob.ANTOBBRIEF;
                        
                        if (brief == null)
                        {
                            brief = new ANTOBBRIEF();
                            context.ANTOBBRIEF.Add(brief);
                            antob.ANTOBBRIEF = brief;
                        }
                        Mapper.Map<AngAntObBriefDto, ANTOBBRIEF>(antragInput.angAntObDto.brief, brief);
                        brief.AART = antragInput.angAntObDto.brief.aufbau;
                        if (brief.GETRIEBE != null)
                        {
                            if (brief.GETRIEBE.Length > 20)
                            {
                                brief.GETRIEBE = brief.GETRIEBE.Substring(0, 20);
                                _log.Debug("Brief.Getriebe wurde gekürzt auf 20 Zeichen beim Antrag: " + antragOutput.SYSID);
                            }
                        }
                    }
                }
                _log.Debug("E: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start)); start = DateTime.Now.TimeOfDay.TotalMilliseconds;

                //Pattern to update n attached Entities-------------------------------------------------

                //Calculation
                if (antragInput.kalkulation != null)
                {
                    
                    ANTKALK antkalk = (from a in context.ANTKALK
                                       where a.ANTRAG.SYSID == antragOutput.SYSID
                                       select a).FirstOrDefault();
                    if (antkalk == null)
                    {
                        antkalk = new ANTKALK();
                        context.ANTKALK.Add(antkalk);
                        antkalk.SYSCREATE = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null);
                    }
                    DateTime? oldcreate = antkalk.SYSCREATE;
                    if(oldcreate==null||!oldcreate.HasValue)
                        oldcreate = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null);
                    Mapper.Map<AngAntKalkDto, ANTKALK>(antragInput.kalkulation.angAntKalkDto, antkalk);
                    //antkalk.ANTRAG = antragOutput;
                    antkalk.SYSKALK = antragOutput.SYSID;
                    antkalk.SYSANTRAG = antragOutput.SYSID;
                    antkalk.SYSCREATE = oldcreate;
                    antkalk.SYSOB = sysob;
                    antkalk.SYSCHANGE = DateTime.Now;
                    antkalk.SYSPRPRODUCT = antragInput.kalkulation.angAntKalkDto.sysprproduct;

                    
                    antkalk.OBUSETYPE = context.OBUSETYPE.Where(par => par.SYSOBUSETYPE == antragInput.kalkulation.angAntKalkDto.sysobusetype).FirstOrDefault();

                    
                    antkalk.SYSWAEHRUNG = context.ExecuteStoreQuery<long>("select syshauswaehrung from lsadd where syslsadd=1", null).FirstOrDefault(); 

                    if (isDispo == true)
                    {
                        List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysprproduct", Value = antragInput.kalkulation.angAntKalkDto.sysprproduct });

                        antkalk.SYSINTSTRCT = context.ExecuteStoreQuery<long>(SYSINTSTRCTBYPRODUCTID, parameters.ToArray()).FirstOrDefault();
                    }

                    // Modus muss bei kredit 0, bei leasing 1 sein
                    if (isLeaseNow == true) { antkalk.MODUS = 1; } else { antkalk.MODUS = 0; }

                    // PPY (Raten pro Jahr): Konstant 12 bei bnow
                    antkalk.PPY = 12;
                    if (isClassic == true)
                    {
                        antkalk.AUSZAHLUNG = (decimal?)antragInput.kalkulation.angAntKalkDto.auszahlung;
                        antkalk.AUSZAHLUNGTYP = antragInput.kalkulation.angAntKalkDto.auszahlungTyp;
                    }

                    if (isLeaseNow == true)
                    {
                        antkalk.BGEXTERNUST = (decimal?)(antragInput.kalkulation.angAntKalkDto.bgexternbrutto - antragInput.kalkulation.angAntKalkDto.bgextern);
                        antkalk.BGINTERNUST = (decimal?)(antragInput.kalkulation.angAntKalkDto.bginternbrutto - antragInput.kalkulation.angAntKalkDto.bgintern);
                        antkalk.RATEUST = (decimal?)antragInput.kalkulation.angAntKalkDto.rateUst;
                    }

                    if (isCredit == true)
                    {
                        antkalk.BGEXTERNUST = 0;
                    }

                    //provisions---------------------------------------------------------------
                    //get current Id's
                    List<long> currentIds = (from a in antragInput.kalkulation.angAntProvDto
                                             select a.sysprov).ToList();

                    _log.Debug("Saving context...");
                    //get entities that are no more in current id's
                    context.SaveChanges();
                    _log.Debug("checking list..."+antragOutput.ANTPROVList);
                    if (!context.Entry(antragOutput).Collection(f => f.ANTPROVList).IsLoaded)
                        context.Entry(antragOutput).Collection(f => f.ANTPROVList).Load();
                    _log.Debug("done");
                    List<object> deleteEntities = (from a in antragOutput.ANTPROVList
                                                   where !currentIds.Contains(a.SYSPROV)
                                                   select a).ToList<object>();
                    //delete old entities
                    foreach (object toDel in deleteEntities)
                        context.DeleteObject(toDel);

                    _log.Debug("F: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start)); start = DateTime.Now.TimeOfDay.TotalMilliseconds;
                    //Update/Insert changed ones
                    foreach (AngAntProvDto prov in antragInput.kalkulation.angAntProvDto)
                    {
                        ANTPROV antprov = (from a in context.ANTPROV
                                           where a.SYSPROV == prov.sysprov
                                           select a).FirstOrDefault();
                        if (antprov == null)// new
                        {
                            antprov = new ANTPROV();
                            context.ANTPROV.Add(antprov);
                        }
                        Mapper.Map<AngAntProvDto, ANTPROV>(prov, antprov);

                        // AntProv ist über AntProv.SysVT mit Antrag verbunden
                        //antprov.ANTRAG = antragOutput;
                        // in sysAntrag muss auch die Id stehen
                        antprov.SYSANTRAG = antragOutput.SYSID;
                        antprov.SYSVT = antragOutput.SYSID;
                        antprov.PRPROVTYPE = context.PRPROVTYPE.Where(par => par.SYSPRPROVTYPE == prov.sysprprovtype).FirstOrDefault();
                        //antprov.PARTNER = context.PARTNER.Where(par => par.SYSPARTNER == prov.syspartner).FirstOrDefault();
                        antprov.SYSPARTNER = prov.syspartner;
                        antprov.PROVISION = (decimal?)prov.provision;
                        antprov.PROVISIONP = (decimal?)prov.provisionPro;
                        antprov.PROVISIONBRUTTO = (decimal?)prov.provisionBrutto;

                        antprov.DEFPROVISION = (decimal?)prov.defaultprovision;
                        antprov.DEFPROVISIONBRUTTO = (decimal?)prov.defaultprovisionbrutto;
                        antprov.DEFPROVISIONP = (decimal?)prov.defaultprovisionp;
                        antprov.DEFPROVISIONUST = (decimal?)prov.defaultprovisionust;

                        int index = antragInput.kalkulation.angAntProvDto.IndexOf(prov);
                        if (antragInput.kalkulation.angAntProvDtoRapMin != null && antragInput.kalkulation.angAntProvDtoRapMin.Count() > index)
                        {
                            // Provision für 5% Zinsen
                            antprov.RAPPROVISIONBRUTTOMIN = (decimal?)antragInput.kalkulation.angAntProvDtoRapMin[index].provisionBrutto;
                        }
                        if (antragInput.kalkulation.angAntProvDtoRapMax != null && antragInput.kalkulation.angAntProvDtoRapMax.Count() > index)
                        {
                            // Provision für 15% Zinsen
                            antprov.RAPPROVISIONBRUTTOMAX = (decimal?)antragInput.kalkulation.angAntProvDtoRapMax[index].provisionBrutto;
                        }

                        antprov.FLAGLOCKED = prov.flaglocked;
                        context.SaveChanges();
                    }

                    //insurances----------------------------------------------------------------
                    //get current Id's
                    currentIds = (from a in antragInput.kalkulation.angAntVsDto
                                  select a.sysantvs).ToList();

                    //get entities that are no more in current id's
                    context.SaveChanges();
                    if (!context.Entry(antragOutput).Collection(f => f.ANTVSList).IsLoaded)
                        context.Entry(antragOutput).Collection(f => f.ANTVSList).Load();
                    deleteEntities = (from a in antragOutput.ANTVSList
                                      where !currentIds.Contains(a.SYSANTVS)
                                      select a).ToList<object>();
                    //delete old entities
                    foreach (object toDel in deleteEntities)
                        context.DeleteObject(toDel);
                    _log.Debug("G: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start)); start = DateTime.Now.TimeOfDay.TotalMilliseconds;
                    //Update/Insert changed ones
                    foreach (AngAntVsDto vs in antragInput.kalkulation.angAntVsDto)
                    {
                        ANTVS antvs = (from a in context.ANTVS
                                       where a.SYSANTVS == vs.sysantvs
                                       select a).FirstOrDefault();
                        if (antvs == null)// new
                        {
                            antvs = new ANTVS();
                            context.ANTVS.Add(antvs);
                        }
                        Mapper.Map<AngAntVsDto, ANTVS>(vs, antvs);

                        //antvs.ANTRAG = antragOutput;
                        antvs.SYSANTRAG = antragOutput.SYSID;
                        antvs.SYSVSTYP=vs.sysvstyp;

                        antvs.SYSVS = (from vstyp in context.VSTYP
                                       where vstyp.SYSVSTYP == vs.sysvstyp
                                       select vstyp.SYSVS).FirstOrDefault();
                        context.SaveChanges();
                    }

                    //subventions--------------------------------------
                    //get current Id's
                    currentIds = (from a in antragInput.kalkulation.angAntSubvDto
                                  select a.sysangsubv).ToList();
                    context.SaveChanges();
                    
                    //get entities that are no more in current id's                    
                    if (!context.Entry(antragOutput).Collection(f => f.ANTSUBVList).IsLoaded)
                        context.Entry(antragOutput).Collection(f => f.ANTSUBVList).Load();
                    deleteEntities = (from a in antragOutput.ANTSUBVList
                                      where !currentIds.Contains(a.SYSANTSUBV)
                                      select a).ToList<object>();
                    //delete old entities
                    foreach (object toDel in deleteEntities)
                        context.DeleteObject(toDel);
                    _log.Debug("H: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start)); start = DateTime.Now.TimeOfDay.TotalMilliseconds;
                    //Update/Insert changed ones
                    foreach (AngAntSubvDto subv in antragInput.kalkulation.angAntSubvDto)
                    {
                        ANTSUBV antsubv = (from a in context.ANTSUBV
                                           where a.SYSANTSUBV == subv.sysantsubv
                                           select a).FirstOrDefault();
                        if (antsubv == null)//new 
                        {
                            antsubv = new ANTSUBV();
                            context.ANTSUBV.Add(antsubv);
                        }
                        Mapper.Map<AngAntSubvDto, ANTSUBV>(subv, antsubv);

                        //antsubv.ANTRAG = antragOutput;
                        antsubv.SYSANTRAG = antragOutput.SYSID;

                        if (isDiffLeasing == true)
                        {
                            antsubv.SUBVTYP = context.SUBVTYP.Where(par => par.SYSSUBVTYP == subv.syssubvtyp).FirstOrDefault();
                            antsubv.SYSSUBVG = subv.syssubvg;
                        }
                        context.SaveChanges();
                    }
                    


                    // Ablösen ---------------------------------------------
                    currentIds = (from a in antragInput.kalkulation.angAntAblDto
                                  select a.sysantabl).ToList();
                    //get entities that are no more in current id's
                    currentIds.Add(-1);
                    context.ExecuteStoreCommand("delete from antabl where sysantabl in (select sysantabl from antabl where sysantrag=" + antragInput.sysid + " and sysantabl not in (" + string.Join(",", currentIds.ToArray()) + "))", null);
                    //Update/Insert changed ones
                    foreach (AngAntAblDto abl in antragInput.kalkulation.angAntAblDto)
                    {
                        ANTABL antabl = (from a in context.ANTABL
                                         where a.SYSANTABL == abl.sysantabl
                                         select a).FirstOrDefault();
                        if (antabl == null)//new 
                        {
                            antabl = new ANTABL();
                            context.ANTABL.Add(antabl);
                        }
                        Mapper.Map<AngAntAblDto, ANTABL>(abl, antabl);

                        antabl.SYSANTRAG = antragInput.sysid;
                        antabl.SYSVORVT=abl.sysvorvt;
                        antabl.SYSABLTYP=abl.sysabltyp;
                        context.SaveChanges();
                    }

                }
                _log.Debug("I: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start)); start = DateTime.Now.TimeOfDay.TotalMilliseconds;
               
                ANTOBSICH antObSicherheit = (from antsich in context.ANTOBSICH
                                             where antsich.SYSANTRAG == antragInput.sysid
                                             select antsich).FirstOrDefault();

                if (antragInput.mitantragsteller != null && antragInput.mitantragsteller.sysit > 0)
                {
                    SetAntragIdInZusatzdaten(context, antragInput.mitantragsteller, antragInput.sysid);
                    if (antObSicherheit != null)     // schon da!
                    {
                        if (antObSicherheit.SYSIT.GetValueOrDefault() != antragInput.mitantragsteller.sysit)
                            antObSicherheit.SYSIT= antragInput.mitantragsteller.sysit;
                    }
                    else
                    {
                        antObSicherheit = new ANTOBSICH();
                        context.ANTOBSICH.Add(antObSicherheit);
                        antObSicherheit.SYSIT=antragInput.mitantragsteller.sysit;
                        antObSicherheit.SYSANTRAG = antragInput.sysid;
                    }

                    // Partner(800)/Solidarschuldner(120)/Bürge(130)
                    antObSicherheit.RANG = MyGetMATyp(antragInput.mitantragstellerTyp);

                    //allow sichtyp control from outside
                    if (antragInput.mitantragsteller.sichtyprang > 0)
                        antObSicherheit.RANG = antragInput.mitantragsteller.sichtyprang;

                    antObSicherheit.AKTIVFLAG = 1;
                    antObSicherheit.SICHTYP = (from typ in context.SICHTYP
                                               where typ.RANG == antObSicherheit.RANG
                                               select typ).FirstOrDefault();
                }
                else
                {
                    // Ticket#2012071910000723 — ITPKZ-Satz des 2. AS bleibt bestehen, wenn 2. AS gelöscht wird
                    if (antObSicherheit != null)
                    {
                        long sysIt2AS =antObSicherheit.SYSIT.Value;
                        ITPKZ itpkzToDelete = (from itpkz in context.ITPKZ
                                               where itpkz.SYSANTRAG == antragOutput.SYSID && itpkz.IT != null && itpkz.IT.SYSIT == sysIt2AS
                                               select itpkz).FirstOrDefault();
                        if (itpkzToDelete != null)
                            context.DeleteObject(itpkzToDelete);

                        // Sicherheit auch löschen
                        context.DeleteObject(antObSicherheit);
                    }
                }

                _log.Debug("J: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start)); start = DateTime.Now.TimeOfDay.TotalMilliseconds;
                context.SaveChanges();
                _log.Debug("K: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start)); start = DateTime.Now.TimeOfDay.TotalMilliseconds;

                MySaveDiffLeasingBestaetigung(antragOutput.SYSID, antragInput, antragOutput.SYSVK, isDiffLeasing);

                // Ticket#2012072310000126 — CR16425 (Ablösen): Memo-Erzeugung // BNRZW-1184 Es sollte keine ePOS Memos bei B2C Antrag erzeugt werden
                if (antragOutput.ERFASSUNGSCLIENT != ERFASSUNGSCLIENT_B2C && antragOutput.ERFASSUNGSCLIENT != ERFASSUNGSCLIENT_ONE && antragOutput.ERFASSUNGSCLIENT != ERFASSUNGSCLIENT_DMR)
                    MySaveNotizen(antragOutput.SYSID, antragInput, antragOutput.SYSVK);

                _log.Debug("L: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start)); start = DateTime.Now.TimeOfDay.TotalMilliseconds;

                setCasaDiplomaSpecialFields(antragInput, "ANTRAG", antragOutput.SYSID, vttypCode);
                createOrUpdateCard(0, antragInput.emboss, isCard, antragOutput.SYSID);

                return getAntrag(antragOutput.SYSID, sysperole);
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="aust"></param>
        /// <param name="sysantob"></param>
        private void updateAngAntObAust(List<AngAntObAustDto> aust, long sysantob)
        {
            List<string> listOfIDs = new List<string>();
            try
            {
                using (DdOlExtended context = new DdOlExtended())
                {
                    
                    if (aust != null && aust.Count > 0)
                    {
                        
                        foreach (AngAntObAustDto item in aust)
                        {
                            if (item.snr == null) continue;
                            ANTOBAUST antobaust = (from au in context.ANTOBAUST
                                                   where au.SYSANTOB == sysantob && au.SNR == item.snr
                                                   select au).FirstOrDefault();
                            bool isnew = false;
                            if (antobaust == null)
                            {
                                antobaust = new ANTOBAUST();
                                isnew = true;
                                antobaust.SNR = item.snr;
                                antobaust.SYSANTOB = sysantob;
                                //BUDGETRECHNER
                                antobaust.SOURCE = item.source;
                                context.ANTOBAUST.Add(antobaust);
                                context.SaveChanges();
                            }

                            string name = ((ZusatzaustatungType)Enum.Parse(typeof(ZusatzaustatungType), (item.snr).ToString())).ToString();

                            if (isnew)
                            {
                                TEXTUNI textuni = new TEXTUNI();
                                textuni.AREA = "ANTOBAUST";
                                textuni.SYSID = antobaust.SYSOBAUST;
                                textuni.BESCHREIBUNG = name;
                                textuni.NAME = name;
                                textuni.TEXTVALUE = item.freitext2;
                                context.TEXTUNI.Add(textuni);
                                context.SaveChanges();
                                listOfIDs.Add(textuni.SYSTEXTUNI.ToString());
                            }

                            if (!isnew)
                            {

                                TEXTUNI textuni = (from tu in context.TEXTUNI
                                                   where tu.SYSID == antobaust.SYSOBAUST && tu.AREA == "ANTOBAUST"
                                                   select tu).FirstOrDefault();

                                textuni.TEXTVALUE = item.freitext2;

                                context.SaveChanges();
                                listOfIDs.Add(textuni.SYSTEXTUNI.ToString());

                            }



                        }
                    }

                }

                if (listOfIDs.Count > 0)
                    replaceBBinTextUni(listOfIDs);
            }

            catch (Exception e)
            {
                throw new Exception("Could not insert Zusatzaustatung text in TEXTUNI!", e);
            }

        }



        /// <summary>
        /// 
        /// </summary>
        /// <param name="aust"></param>
        /// <param name="sysantob"></param>
        private void updateAngAngObAust(List<AngAntObAustDto> aust, long sysangob)
        {

            List<string> listOfIDs = new List<string>();
            try
            {
                using (DdOlExtended context = new DdOlExtended())
                {
                    
                    if (aust != null && aust.Count > 0)
                    {
                        bool isnew = false;
                        foreach (AngAntObAustDto item in aust)
                        {
                            ANGOBAUST angobaust = (from au in context.ANGOBAUST
                                                   where au.SYSANGOB == sysangob && au.SNR == item.snr
                                                   select au).FirstOrDefault();
                            if (angobaust == null)
                            {
                                angobaust = new ANGOBAUST();
                                isnew = true;
                                angobaust.SNR = item.snr;
                                angobaust.SYSANGOB = sysangob;
                                //BUDGETRECHNER
                                angobaust.SOURCE = item.source;
                                context.ANGOBAUST.Add(angobaust);
                                context.SaveChanges();

                            }

                            string name = ((ZusatzaustatungType)Enum.Parse(typeof(ZusatzaustatungType), (item.snr).ToString())).ToString();

                            if (isnew)
                            {
                                TEXTUNI textuni = new TEXTUNI();
                                textuni.AREA = "ANGOBAUST";
                                textuni.SYSID = angobaust.SYSOBAUST;
                                textuni.BESCHREIBUNG = name;
                                textuni.NAME = name;
                                textuni.TEXTVALUE = item.freitext2;
                                context.TEXTUNI.Add(textuni);
                                context.SaveChanges();
                                listOfIDs.Add(textuni.SYSTEXTUNI.ToString());

                            }

                            if (!isnew)
                            {

                                TEXTUNI textuni = (from tu in context.TEXTUNI
                                                   where tu.SYSID == angobaust.SYSOBAUST && tu.AREA == "ANGOBAUST"
                                                   select tu).FirstOrDefault();

                                textuni.TEXTVALUE = item.freitext2;
                                context.SaveChanges();
                                listOfIDs.Add(textuni.SYSTEXTUNI.ToString());

                            }



                        }
                    }

                }

                if (listOfIDs.Count > 0)
                    replaceBBinTextUni(listOfIDs);
            }
            catch
            {
                throw new Exception("Could not insert Zusatzaustatung text in TEXTUNI!");
            }

        }


        /// <summary>
        /// Delete Austatung von Angebot
        /// </summary>
        /// <param name="sysAngob"></param>
        public void deleteAngObAust(long sysAngob)
        {
            try
            {
                using (DdOlExtended context = new DdOlExtended())
                {
                    
                    if (sysAngob > 0)
                    {
                        

                        List<ANGOBAUST> aust = (from au in context.ANGOBAUST
                                    where au.SYSANGOB == sysAngob
                                    select au).ToList();

                        foreach (var item in aust)
                        {
                            TEXTUNI textuni = (from tu in context.TEXTUNI
                                               where tu.SYSID == item.SYSOBAUST && tu.AREA == "ANGOBAUST"
                                               select tu).FirstOrDefault();
                            context.DeleteObject(textuni);
                            context.DeleteObject(item);
                        }
                        context.SaveChanges();
                    }
                }
            }
            catch
            {
                throw new Exception("Could not delete Zusatzaustatung text in TEXTUNI!");
            }
        }

        /// <summary>
        /// Delete Austatung von Antrag
        /// </summary>
        /// <param name="sysAntob"></param>
        public void deleteAntObAust(long sysAntob)
        {
            try
            {
                using (DdOlExtended context = new DdOlExtended())
                {
                    
                    if (sysAntob > 0)
                    {
                        

                        List<ANTOBAUST> aust = (from au in context.ANTOBAUST
                                    where au.SYSANTOB == sysAntob
                                    select au).ToList();

                        foreach (var item in aust)
                        {
                            TEXTUNI textuni = (from tu in context.TEXTUNI
                                               where tu.SYSID == item.SYSOBAUST && tu.AREA == "ANTOBAUST"
                                               select tu).FirstOrDefault();
                            context.DeleteObject(textuni);
                            context.DeleteObject(item);
                        }
                        context.SaveChanges();
                    }
                }
            }
            catch
            {
                throw new Exception("Could not delete Zusatzaustatung text in TEXTUNI!");
            }
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="systextUni"></param>
        public void replaceBBinTextUni(List<string> ids)
        {
            string idstextuni = string.Join(",", ids.ToArray());
            string QUERY = "update textuni set textvalue =  REGEXP_REPLACE(textvalue,'<BR>|<br>',CHR(10))  where systextuni in (" + idstextuni + ")";

            using (DdOlExtended context = new DdOlExtended())
            {

                context.ExecuteStoreCommand(QUERY, null);


            }
        }



        /// <summary>
        /// Antrag löschen
        /// </summary>
        /// <param name="sysid">Primary Key</param>
        public void deleteAntrag(long sysid)
        {
            using (DdOlExtended context = new DdOlExtended())
            {
                ANTRAG antrag = (from ant in context.ANTRAG
                                 where ant.SYSID == sysid
                                 select ant).FirstOrDefault();

                // Check sight field
                if (antrag != null)
                {
                    /*if (!Cic.OpenLease.Service.PEUNIHelper.IsInSightField(context, ServiceValidator.SYSPUSER, Cic.OpenLease.Service.PEUNIHelper.Areas.ANGEBOT, sysID))
                    {
                        throw new System.ApplicationException("Angebot not exists in sight field.");
                    }*/
                }
                else
                {
                    throw new System.ApplicationException("Angebot not exists.");
                }

                // Delete PEUNI
                //string AreaName = Cic.OpenLease.Model.DdOl.PEUNIHelper.Areas.ANGEBOT.ToString();
                string AreaName = "ANTRAG";

                // Select PEUNI list                 
                var PEUNIQuery = from peuni in context.PEUNI
                                 where (peuni.SYSID == sysid && peuni.AREA == AreaName)
                                 select peuni;

                // Delete each PEUNI (in context)                    
                foreach (PEUNI LoopPEUNI in PEUNIQuery)
                {
                    context.DeleteObject(LoopPEUNI);
                }
                /*
                // Delete ITADRREF
                ITADRREF adrRef = context.ITADRREF.Where(par => par.SYSANTRAG == antrag.SYSID).FirstOrDefault();
                if (adrRef != null)
                    context.DeleteObject(adrRef);

                // Delete ITKONTOREF
                if (!antrag.ITKONTOREFList.IsLoaded)
                    antrag.ITKONTOREFList.Load();
                ITKONTOREF ktoRef = antrag.ITKONTOREFList.FirstOrDefault();
                if (ktoRef != null)
                    context.DeleteObject(ktoRef);
                */
                //SELECT ANTOB
                ANTOB antob = context.ANTOB.Where(par => par.SYSANTRAG == sysid).FirstOrDefault();
                if (antob != null)
                {
                    //SELECT ANTOBINI
                    var ANTOBINIQuery = from antobini in context.ANTOBINI
                                        where antobini.SYSOBINI == antob.SYSOB
                                        select antobini;
                    //DELETE each ANTOBINI
                    foreach (ANTOBINI LoopANTOBINI in ANTOBINIQuery)
                    {
                        context.DeleteObject(LoopANTOBINI);
                    }
                    // Delete each ANTOBBRIEF     
                    
                    if (antob.ANTOBBRIEF == null)
                        context.Entry(antob).Reference(f => f.ANTOBBRIEF).Load();
                    if (antob.ANTOBBRIEF != null)
                        context.DeleteObject(antob.ANTOBBRIEF);
                    // Delete each ANTOB
                    context.DeleteObject(antob);
                }

                //Antkalk (0..1)--------------------------------------------------------------------
                ANTKALK antkalk = context.ANTKALK.Where(par => par.ANTRAG.SYSID == antrag.SYSID).FirstOrDefault();
                if (antkalk != null)
                {
                    context.DeleteObject(antkalk);
                }

                //Provision (0..n)--------------------------------------------------------------------
                List<ANTPROV> antprovList = (from avar in context.ANTPROV
                                             where avar.ANTRAG.SYSID == antrag.SYSID
                                             select avar).ToList();
                foreach (ANTPROV antprov in antprovList)
                {
                    context.DeleteObject(antprov);
                }

                //Subvention (0..n)--------------------------------------------------------------------
                List<ANTSUBV> antsubvList = (from avar in context.ANTSUBV
                                             where avar.ANTRAG.SYSID == antrag.SYSID
                                             select avar).ToList();
                foreach (ANTSUBV antsubv in antsubvList)
                {
                    context.DeleteObject(antsubv);
                }

                //Insurances (0..n)--------------------------------------------------------------------
                List<ANTVS> antvsList = (from avar in context.ANTVS
                                         where avar.ANTRAG.SYSID == antrag.SYSID
                                         select avar).ToList();
                foreach (ANTVS antvs in antvsList)
                {
                    context.DeleteObject(antvs);
                }

                ANTOBSICH antobsich = (from antsich in context.ANTOBSICH
                                       where antsich.SYSANTRAG == sysid
                                       select antsich).FirstOrDefault();
                if (antobsich != null)
                {
                    context.DeleteObject(antobsich);
                }

                // DeleteObject ANTRAG and SaveChanges
                context.DeleteObject(antrag);

                context.SaveChanges();
            }
        }

        /// <summary>
        /// Antrag einreichen und daraus einen Vertrag generieren.
        /// </summary>
        /// <param name="antragDto">Eingehender Antrag</param>
        /// <param name="syswfuser">Einreichender Benutzer</param>
        /// <param name="isocode">Sprachcode</param>
        /// <returns>Ausgehender Vertrag</returns>
        public void processAntragEinreichung(AntragDto antragDto, long syswfuser, string isocode)
        {
            VertragDto vertrag = new VertragDto();

            //zustand managed by eaihot
            using (DdOlExtended context = new DdOlExtended())
            {
                ANTRAG antrag = (from a in context.ANTRAG
                                 where a.SYSID == antragDto.sysid
                                 select a).FirstOrDefault();

                if ( !AntragZustand.Neu.ToString().Equals(antrag.ZUSTAND) )
                {
                    if (!(antrag.ZUSTAND.Equals("Bonitätsprüfung") && antrag.ATTRIBUT.Equals("Produktprüfung NOK (Vorschlag vorhanden)")))
                        throw new Exception("Antrag Status not 'Neu' or not 'Finanzierungsvorschlag'!");
                }
                antrag.ZUSTAND = AntragZustand.Neu.ToString();
                antrag.ATTRIBUT = AntragAttribut.Eingereicht.ToString();
                antrag.SYSBERATER = syswfuser;
                // antragOutput.DATEINREICHUNG darf keine Uhrzeit enthalten
                antrag.DATEINREICHUNG = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null);
                antrag.DATEINREICHUNG = ((DateTime)antrag.DATEINREICHUNG).Date;
                antrag.DATEINREICHUNGZEIT = DateTimeHelper.DateTimeToClarionTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                antrag.TESTFLAG = Convert.ToInt32(antragDto.testFlag);

                context.SaveChanges();
            }

            using (DdOwExtended context = new DdOwExtended())
            {

                long syswftable = context.ExecuteStoreQuery<long>("select syswftable from wftable where syscode='ANTRAG'", null).FirstOrDefault();
                long syswfzust = context.ExecuteStoreQuery<long>("select syswfzust from wfzust where syscode='INBOXEN'", null).FirstOrDefault();


                WFTZUST oldEntries = (from wftzust in context.WFTZUST
                                      where wftzust.WFTABLE.SYSWFTABLE == syswftable
                                      where wftzust.SYSLEASE == antragDto.sysid
                                      where wftzust.WFZUST.SYSWFZUST == syswfzust
                                      select wftzust).FirstOrDefault();

                if (oldEntries != null)
                {
                    throw new Exception("Antrag already submitted!");
                }

                DateTime currDateTime = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null);
                int currTime = DateTimeHelper.DateTimeToClarionTime(currDateTime);

                // Neuer Zustand
                WFTZUST wftZust = new WFTZUST();
                wftZust.SYSLEASE = antragDto.sysid;
                wftZust.SYSWFTABLE = syswftable;
                wftZust.SYSWFZUST = syswfzust;
                wftZust.STATE = "INBOXEN";
                wftZust.BEZEICHNUNG = "Inboxen";
                wftZust.STATUS = 0;
                wftZust.COUNTER = 1;
                wftZust.CREATEDATE = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDateNoTime(currDateTime);
                wftZust.CREATETIME = currTime;
                wftZust.CREATEBY = syswfuser;
                wftZust.CREATEWFG = "B2B";
                wftZust.GEBIET = "Antrag";

                context.WFTZUST.Add(wftZust);
                context.SaveChanges();

                // Zustandsvariable 
                WFTZVAR wftVarZustand = new WFTZVAR();
                wftVarZustand.WFTZUST = wftZust;
                if (antragDto.sysprchannel == 1U)
                    wftVarZustand.CODE = "B2B-FF";
                else
                    wftVarZustand.CODE = "B2B-KF";

                wftVarZustand.VALUE = "Neu";
                context.WFTZVAR.Add(wftVarZustand);

                // Ticket#2012070410000261 — Anpassung Inbox-Routing - Timestamp beim Antrag-Einreichen 
                WFTZVAR wftVarTimeStamp = new WFTZVAR();
                wftVarTimeStamp.WFTZUST = wftZust;
                if (antragDto.sysprchannel == 1U)
                    wftVarTimeStamp.CODE = "EINGANG_B2B-FF";
                else
                    wftVarTimeStamp.CODE = "EINGANG_B2B-KF";

                wftVarTimeStamp.VALUE = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null).ToString("dd.MM.yyyy HH:mm:ss");

                context.WFTZVAR.Add(wftVarTimeStamp);

                context.SaveChanges();

                createOrUpdateDisclaimersMemo(antragDto.sysid, syswfuser, isocode);

            }

            bool FF = antragDto.sysprchannel == 1L;        // Fahrzeugfinanzierung
            bool KF = antragDto.sysprchannel == 2L;        // Kreditfinanzierung
            bool eventtest = false;
            String testWert = AppConfig.Instance.GetEntry("BPE", "ENABLE_B2B_BPE", "", "SETUP.NET");
            eventtest = "1".Equals(testWert)||"true".Equals(testWert)||"TRUE".Equals(testWert);

            if ("ENABLE_B2B_BPE".Equals(antragDto.bemerkung))
                eventtest = true;

            if(FF)
            {
                if (eventtest) { 
                    String procCode = AppConfig.Instance.GetEntry("BPE", "PROC_FF_B2B", "", "SETUP.NET");
                    String evtCode = AppConfig.Instance.GetEntry("BPE", "EVT_FF_B2B", "evtd_WFA_FF_Sales", "SETUP.NET");
                    BPEBo.createBPEProcess(procCode, evtCode, "ANTRAG", antragDto.sysid, syswfuser);
                }
            }
            else if (KF)
            {
                if (eventtest)
                {
                    String procCode = AppConfig.Instance.GetEntry("BPE", "PROC_KF_B2B", "", "SETUP.NET");
                    String evtCode = AppConfig.Instance.GetEntry("BPE", "EVT_KF_B2B", "evtd_WFA_KF_Sales", "SETUP.NET");
                    BPEBo.createBPEProcess(procCode, evtCode, "ANTRAG", antragDto.sysid, syswfuser);
                }
            }
            
        }

       /// <summary>
       /// Ermitteln von sysvttyp aus product in Kalk
       /// </summary>
       /// <param name="sysangebot"></param>
       /// <returns></returns>
        public long getSysVttypByPrprod(long sysangebot)
        {
            const String QUERYPRPROD = "select prproduct.sysvttyp from angkalk,prproduct where prproduct.sysprproduct=angkalk.sysprproduct and sysangebot = :psysangebot";
            using (DdOlExtended context = new DdOlExtended())
            { 
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysangebot", Value = sysangebot });
                return context.ExecuteStoreQuery<long>(QUERYPRPROD, parameters.ToArray()).FirstOrDefault();
            }
        }

        /// <summary>
        /// save vttyp in ANGEBOT
        /// </summary>
        /// <param name="sysangebot"></param>
        public void updateVttypinAngebot(long sysangebot)
        {
            long sysvttyp = getSysVttypByPrprod(sysangebot);
            using (DdOlExtended context = new DdOlExtended())
                {
                    
                    ANGEBOT angebotOutput = (from ang in context.ANGEBOT
                                             where ang.SYSID == sysangebot
                                             select ang).FirstOrDefault();


                    angebotOutput.SYSVTTYP =sysvttyp;
                    context.SaveChanges();

                }
            
        }


        /// <summary>
        /// B2C Angebot einreichen
        /// </summary>
        /// <param name="angebot">Eingehendes Angebot</param>
        /// <param name="userid">Einreichender Benutzer syswfuser</param>
        /// <param name="isocode">Sprachcode</param>
        /// <returns>Ausgehender Vertrag</returns>
        public void processAngebotEinreichung(AngebotDto angebot, long userid, string isocode)
        {

            using (DdOwExtended context = new DdOwExtended())
            {
                long c = context.ExecuteStoreQuery<long>("select count(*) from wftzust,wftable,wfzust,wftzvar where wftzvar.syswftzust=wftzust.syswftzust and wftzust.syswftable=wftable.syswftable and wftable.syscode='ANGEBOT' and wfzust.syswfzust=wftzust.syswfzust and wfzust.syscode='INBOXEN_ANG' and wftzvar.value!='Upload' and wftzust.syslease=" + angebot.sysid, null).FirstOrDefault();


                if (c > 0)
                {
                    _log.Warn("processAngebotEinreichung for B2C-Angebot " + angebot.sysid + " not performed, already values in wftzust found!!!");
                    return;
                }
              


                long syswftable = context.ExecuteStoreQuery<long>("select syswftable from wftable where syscode='ANGEBOT'", null).FirstOrDefault();
                long syswfzust = context.ExecuteStoreQuery<long>("select syswfzust from wfzust where syscode='INBOXEN_ANG'", null).FirstOrDefault();

                if (angebot.kunde != null && angebot.kunde.sysctlangkorr > 0)
                    isocode = context.ExecuteStoreQuery<String>("select isocode from ctlang where sysctlang=" + angebot.kunde.sysctlangkorr, null).FirstOrDefault();
                else if (angebot.kunde != null && angebot.kunde.sysctlang > 0)
                    isocode = context.ExecuteStoreQuery<String>("select isocode from ctlang where sysctlang=" + angebot.kunde.sysctlang, null).FirstOrDefault();


                DateTime currDateTime = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null);
                int currTime = DateTimeHelper.DateTimeToClarionTime(currDateTime);
                try
                {
                    WFTZUST wftZust = (from wftzust in context.WFTZUST
                                       where wftzust.WFTABLE.SYSWFTABLE == 122
                                       where wftzust.SYSLEASE == angebot.sysid
                                       where wftzust.WFZUST.SYSWFZUST == 461
                                       select wftzust).FirstOrDefault();

                    // Neuer Zustand
                    if (wftZust == null)
                    {
                        wftZust = new WFTZUST();
                        wftZust.SYSLEASE = angebot.sysid;
                        wftZust.SYSWFTABLE=syswftable;

                        wftZust.SYSWFZUST=syswfzust;
                        wftZust.STATE = "INBOXEN";
                        wftZust.BEZEICHNUNG = "Inboxen";
                        wftZust.STATUS = 0;
                        wftZust.COUNTER = 1;
                        wftZust.CREATEDATE = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDateNoTime(currDateTime);
                        wftZust.CREATETIME = currTime;
                        wftZust.CREATEBY = userid;
                        wftZust.CREATEWFG = "B2C";
                        wftZust.GEBIET = "ANGEBOT";

                        context.WFTZUST.Add(wftZust);
                    }
                    context.SaveChanges();

                    // Zustandsvariable 
                    WFTZVAR wftVarZustand = new WFTZVAR();
                    wftVarZustand.WFTZUST = wftZust;
                    wftVarZustand.CODE = "ANG-B2C";
                    wftVarZustand.VALUE = "Neu";
                    context.WFTZVAR.Add(wftVarZustand);

                    context.SaveChanges();
                }
                catch (Exception exc)
                {
                    _log.Warn("WFTZUST not created for B2C-Angebot: " + exc.Message);
                }

                try
                {
                    createOrUpdateDisclaimersMemoAngebot(angebot.sysid, userid, isocode, false);

                    if (angebot.mitantragsteller != null && angebot.mitantragsteller.sysit > 0)
                        createOrUpdateDisclaimersMemoAngebot(angebot.sysid, userid, isocode, true);

                    if (angebot.kunde != null && angebot.kunde.bestandsKunde > 0)
                        createOrUpdateBestandskundeMemo(angebot.sysid, userid, isocode);
                }catch(Exception me)
                {
                    _log.Error("processAngebotEinreichung Memo-Update: " + me.Message, me);
                }
                bool FF = angebot.sysprchannel == 1L;        // Fahrzeugfinanzierung
                bool KF = angebot.sysprchannel == 2L;        // Kreditfinanzierung
                if(KF&&angebot.erfassungsclient== Cic.OpenOne.GateBANKNOW.Common.DAO.AngAntDao.ERFASSUNGSCLIENT_B2C)
                {
                    String testWert = AppConfig.Instance.GetEntry("BPE", "ENABLE_B2C_BPE", "", "SETUP.NET");
                    bool eventtest = "1".Equals(testWert) || "true".Equals(testWert) || "TRUE".Equals(testWert);
                    if (eventtest) { 
                        String procCode = AppConfig.Instance.GetEntry("BPE", "PROC_KF_ONLINE_B2C", "", "SETUP.NET");
                        String evtCode = AppConfig.Instance.GetEntry("BPE", "EVT_KF_ONLINE_B2C", "evtd_WFA_KF_Hauptprozess_Start", "SETUP.NET");
                        BPEBo.createBPEProcess(procCode, evtCode, "ANGEBOT", angebot.sysid, userid);
                    }
                }
            }
        }

        /// <summary>
        /// Objektdaten auslesen
        /// </summary>
        /// <param name="key">Schlüssel</param>
        /// <returns>Daten</returns>
        public AngAntObDto getObjektdaten(String key)
        {
            String[] KeyArray = new String[0];
            if (key != null)
            {
                if (key.Length > 0)
                {
                    KeyArray = key.Split(new Char[] { '>' });
                }
            }
            else
            {
                throw new Exception("Invalid key in getObjektdaten : " + key);
            }
            long sysobtyp = long.Parse(KeyArray[KeyArray.Length - 1]);

            return getObjektdaten(sysobtyp);
        }

        /// <summary>
        /// Objektdaten auslesen
        /// </summary>
        /// <param name="sysobtyp">ObjectType</param>
        /// <returns>Daten</returns>
        virtual public AngAntObDto getObjektdaten(long sysobtyp)
        {
            using (DdOlExtended ctx = new DdOlExtended())
            {
                List<object> parameters = new List<object>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = sysobtyp });

                ObViewDto ob = ctx.ExecuteStoreQuery<ObViewDto>(GETOBJECTDATAQUERY, parameters.ToArray()).FirstOrDefault();
                if (ob == null)
                {
                    throw new Exception("Keine Objektdaten vorhanden für sysobtyp = " + sysobtyp);
                }

                AngAntObDto rval = new AngAntObDto();
                rval.sysobtyp = sysobtyp;
                rval.bezeichnung = ob.bezeichnung;
                if (rval.bezeichnung != null && rval.bezeichnung.Length > 40)
                    rval.bezeichnung = rval.bezeichnung.Substring(0, 40);
                rval.ahk = 0;
                rval.ahkBrutto = 0;
                rval.ahkUst = 0;

                rval.ahk = ob.neupreisnetto;
                rval.ahkBrutto = ob.neupreisbrutto;
                rval.ahkUst = ob.neupreisbrutto - ob.neupreisnetto;

                try
                {
                    rval.baujahr = System.DateTime.ParseExact(ob.baujahr + ob.baumonat, "yyyyMM", System.Globalization.CultureInfo.InvariantCulture);
                }catch(Exception)
                {
                    try
                    {
                        rval.baujahr = System.DateTime.ParseExact(ob.baujahr, "yyyy", System.Globalization.CultureInfo.InvariantCulture);
                    }catch(Exception)
                    {

                    }
                }
                rval.baumonat = ob.baumonat;
                rval.erstzulassung = null;//new DateTime();
                rval.fabrikat = ob.modell;
                rval.grund = ob.neupreisnetto;
                rval.grundBrutto = ob.neupreisbrutto;
                rval.grundUst = ob.neupreisbrutto - ob.neupreisnetto;
                rval.hersteller = ob.marke;//rval.fabrikat;
                rval.fzart = ob.art.ToString();
                rval.typengenehmigung = ob.typengenehmigung;
                rval.schwacke = ob.schwacke;

                rval.brief = new AngAntObBriefDto();
                rval.brief.aufbau = ob.aufbau;
                double emval = 0;
                double.TryParse(ob.emission, out emval);
                rval.brief.co2emi = emval;
                rval.brief.getriebe = ob.getriebe;
                rval.brief.treibstoff = ob.treibstoff;

                // Ticket#2011092910000033 
                // rval.brief.fident = ob.SERIE;        // soll aus dem AntObBrief geholt werden, aber ohne AntragId geht das nicht

                return rval;
            }
        }

        public AngAntObDto getObjektdatenFromNkk(long sysNkk)
        {
            using (DdOlExtended ctx = new DdOlExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysnkk", Value = sysNkk });
                
                var sysObTyp = ctx.ExecuteStoreQuery<long>("select sysobtyp from ob where sysnkk=:sysnkk and nvl(ob.sysvt,0) = 0", parameters.ToArray()).FirstOrDefault();

                if (sysObTyp != 0)
                {
                    return getObjektdaten(sysObTyp);
                }
                else
                {
                    _log.Warn(string.Format("No OB found for SYSNKK = {0} using OBTYPE.", sysNkk));
                    return null;
                }
            }
        }

        public AngAntObDto getObjektdatenFromOB(long sysNkk)
        {
            
            using (DdOlExtended ctx = new DdOlExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysnkk", Value = sysNkk });

                var ob = ctx.ExecuteStoreQuery<AngAntObDto>("select * from ob where sysnkk=:sysnkk and nvl(ob.sysvt, 0) = 0", parameters.ToArray()).FirstOrDefault();
                if (ob == null)
                {
                    return null;
                }

                var obbrief = ctx.ExecuteStoreQuery<AngAntObBriefDto>(string.Format("select * from obbrief where sysobbrief = {0}", ob.sysob)).FirstOrDefault();

                ob.brief = obbrief;
                return ob;
            }
        }

        public string getVinCodeFromNkk(long sysNkk)
        {
            using (DdOlExtended ctx = new DdOlExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysnkk", Value = sysNkk });

                var fident = ctx.ExecuteStoreQuery<string>("select fident from ob, obbrief where ob.sysnkk=:sysnkk and nvl(ob.sysvt,0) = 0 and ob.sysob = obbrief.sysobbrief", parameters.ToArray()).FirstOrDefault();

                if (!string.IsNullOrEmpty(fident))
                {
                    return fident.Trim();
                }
                return null;
            }
        }

        /// <summary>
        /// getVsArtCode
        /// </summary>
        /// <param name="sysvstyp"></param>
        /// <returns></returns>
        virtual public string getVsArtCode(long sysvstyp)
        {
            using (DdOlExtended ctx = new DdOlExtended())
            {
                var query = from Vsart in ctx.VSART
                            join Vstyp in ctx.VSTYP on Vsart.SYSVSART equals Vstyp.VSART.SYSVSART
                            where Vstyp.SYSVSTYP == sysvstyp
                            select Vsart.CODE;
                string vstypcode = query.FirstOrDefault();
                if (vstypcode != null) return vstypcode.ToString();
            }
            return null;
        }

        virtual public bool istRatenabsicherung(long sysprproduct, long sysvstyp)
        {
            int flag = 0;
            try
            {
                using (DdOlExtended ctx = new DdOlExtended())
                {

                    string query = "SELECT P.DISABLEDFLAG " +
                                    " FROM PRCLRSVSET C, " +
                                    " PRPRODUCT R, " +
                                    " PRRSVSET S, " +
                                    " PRRSVPOS P, " +
                                    "  VSTYP V " +
                                    " WHERE R.SYSPRPRODUCT = C.SYSPRPRODUCT " +
                                    " AND C.SYSPRRSVSET = S.SYSPRRSVSET " +
                                    " AND S.SYSPRRSVSET = P.SYSPRRSVSET " +
                                    " AND V.SYSVSTYP = P.SYSVSTYP " +
                                    " AND R.SYSPRPRODUCT   = :psysprproduct " +
                                    " AND V.SYSVSTYP = :psysvstyp ";

                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysprproduct", Value = sysprproduct });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysvstyp", Value = sysvstyp });

                    flag = ctx.ExecuteStoreQuery<int>(query, parameters.ToArray()).FirstOrDefault();

                }
                if (flag == 1) return false;
            }
            catch (Exception)
            {
                _log.Debug("IstRatenabsicherung Methode gibt exception");
                return true;
            }
            return true;
        }

        /// <summary>
        /// getVart
        /// </summary>
        /// <param name="sysvart"></param>
        /// <returns></returns>
        virtual public VartDto getVart(long sysvart)
        {
            using ( PrismaExtended ctx = new  PrismaExtended())
            {
                return ctx.ExecuteStoreQuery<VartDto>("select * from vart where sysvart="+sysvart).FirstOrDefault();
            }
        }

        /// <summary>
        /// getAntragBezeichnungen
        /// </summary>
        /// <param name="antrag"></param>
        /// <returns></returns>
        public AntragDto getAntragBezeichnungen(AntragDto antrag)
        {
            using (DdOlExtended ctx = new DdOlExtended())
            {
                // AngAntDto --------------------------------------------------
                // Bankkunde --------------------------------------------------
                antrag.kdBezeichnung = MyGetPersonBezeichnung(antrag.syskd);

                // Kanal (FF, KF)
                var queryChannel = from BCHANNEL in ctx.BCHANNEL
                                   where BCHANNEL.SYSBCHANNEL == antrag.sysprchannel
                                   select BCHANNEL;
                BCHANNEL channel = queryChannel.FirstOrDefault();
                if (channel != null)
                {
                    antrag.prChannelBezeichnung = channel.NAME;
                }

                // Handelsgruppe --------------------------------------------------
                var queryPrhGroup = from PRHGROUP in ctx.PRHGROUP
                                    where PRHGROUP.SYSPRHGROUP == antrag.sysprhgroup
                                    select PRHGROUP;
                PRHGROUP prhGroup = queryPrhGroup.FirstOrDefault();
                if (prhGroup != null)
                {
                    antrag.prHgroupBezeichnung = prhGroup.DESCRIPTION;
                }

                // Brand  --------------------------------------------------
                var queryBrand = from BRAND in ctx.BRAND
                                 where BRAND.SYSBRAND == antrag.sysbrand
                                 select BRAND;
                BRAND brand = queryBrand.FirstOrDefault();
                if (brand != null)
                {
                    antrag.brandBezeichnung = brand.DESCRIPTION;
                }

                // Marketingaktion (Kampagnencode) --------------------------------------------------
                var queryMarktAb = from MARKTAB in ctx.MARKTAB
                                   where MARKTAB.SYSMARKTAB == antrag.sysmarktab
                                   select MARKTAB;
                MARKTAB marktAb = queryMarktAb.FirstOrDefault();
                if (marktAb != null)
                {
                    antrag.marktabBezeichnung = marktAb.TEXT;
                }

                // Erfasser --------------------------------------------------
                var queryWfUser = from WFUSER in ctx.WFUSER
                                  where WFUSER.SYSWFUSER == antrag.syswfuser
                                  select WFUSER;
                CIC.Database.OL.EF6.Model.WFUSER wfUser = queryWfUser.FirstOrDefault();
                if (wfUser != null)
                {
                    antrag.wfuserBezeichnung = wfUser.CODE;
                }

                // Änderer --------------------------------------------------
                var queryWfUserChange = from WFUSER in ctx.WFUSER
                                        where WFUSER.SYSWFUSER == antrag.syswfuserchange
                                        select WFUSER;
                wfUser = queryWfUserChange.FirstOrDefault();
                if (wfUser != null)
                {
                    antrag.wfUserChangeBezeichnung = wfUser.CODE;
                }

                // Betreuer (Antragsowner)  --------------------------------------------------
				antrag.beraterBezeichnung = getWfUserBezeichnung (antrag.sysberater);

                // Korrespondenzadresse --------------------------------------------------
                var queryKorrAdr = from ADRESSE in ctx.ADRESSE
                                   where ADRESSE.SYSADRESSE == antrag.sysKorrAdresse
                                   select ADRESSE;
                ADRESSE adresse = queryKorrAdr.FirstOrDefault();
                if (adresse != null)
                {
                    antrag.korrAdresseBezeichnung = adresse.BEZEICHNUNG;
                }

                // It Konto --------------------------------------------------
                var queryItKonto = from ITKONTO in ctx.ITKONTO
                                   where ITKONTO.SYSITKONTO == antrag.sysItKonto
                                   select ITKONTO;
                ITKONTO itKonto = queryItKonto.FirstOrDefault();
                if (itKonto != null)
                {
                    antrag.itKontoBezeichnung = itKonto.BEZEICHNUNG;
                }

                // prproduct --------------------------------------------------
                CIC.Database.PRISMA.EF6.Model.PRPRODUCT prProduct = null;
                using ( PrismaExtended ctxp = new PrismaExtended())
                {
                    if (antrag.sysprprod > 0)
                    {

                        prProduct = ctxp.PRPRODUCT.Where(par => par.SYSPRPRODUCT == antrag.sysprprod).FirstOrDefault();

                    }
                }

              
                if (prProduct != null)
                {
                    antrag.prProductCode = MyGetPrProductCode(prProduct.SYSPRPRODUCT);
                    antrag.prProductBezeichnung = prProduct.NAME;
                }
                // Security Check: Aufruf nur mit long
                antrag.zustandExtern = ctx.ExecuteStoreQuery<String>("select attribut from vt where vt.sysantrag=" + antrag.sysid + " and rownum<=1", null).FirstOrDefault();
            }
            antrag = MyGetAntragBezeichnungenKunde(antrag);
            antrag = MyGetAntragBezeichnungenObjekt(antrag);
            antrag = MyGetAntragBezeichnungenKalkulation(antrag);

            return antrag;
        }

        /// <summary>
        /// getProFinLock
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        public int getProFinLock(long sysid)
        {
            int profinlock = 0;
            using (Cic.OpenOne.Common.Model.DdOw.DdOwExtended olCtx = new DdOwExtended())
            {

                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysid", Value = sysid });

                profinlock = olCtx.ExecuteStoreQuery<int>(QUERYPROFINLOCK, parameters.ToArray()).FirstOrDefault();
                return profinlock;
            }


        }


        public DateTime? getAuszahlungsdatum(long sysid)
        {

             
            using (Cic.OpenOne.Common.Model.DdOw.DdOwExtended olCtx = new DdOwExtended())
            {

                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysid", Value = sysid });

                DateTime? ausdatum = olCtx.ExecuteStoreQuery<DateTime?>(QUERYAUSDATUM, parameters.ToArray()).FirstOrDefault();
                return ausdatum;
            }
            

        }

        /// <summary>
        /// Löscht ein Memo für den Antrag
        /// Ticket#2012080710000281 — Memo löschen, wenn Ablösen entfernt werden 
        /// </summary>
        /// <param name="kategorieBezeichnung"></param>
        /// <param name="sysLease"></param>
        public void deleteNotiz(String kategorieBezeichnung, long sysLease)
        {
            using (DdOwExtended context = new DdOwExtended())
            {
                // Es kann nur ein Memo für einen Antrag in einer Kategorie geben
                WFMMEMO wfmMemo = (from memo in context.WFMMEMO
                                   join table in context.WFTABLE on memo.SYSWFMTABLE equals table.SYSWFTABLE
                                   where table.SYSCODE == NOTIZSYSCODE
                                   where memo.SYSLEASE == sysLease && memo.WFMMKAT.BESCHREIBUNG == kategorieBezeichnung
                                   select memo).FirstOrDefault();
                if (wfmMemo != null)
                {
                    context.DeleteObject(wfmMemo);
                    context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Erstellt eine neue Notiz oder aktualisiert die existierende
        /// </summary>
        /// <param name="sysid">sysid von syscode wftable</param>
        /// <param name="notiz">notiz</param>
        /// <param name="wftableSyscode">gebiet code</param>
        /// <param name="kurzbez">kurzbez</param>
        /// <param name="kategorieBezeichnung">kategorie bez</param>
        /// <param name="sysPerson">sysperson</param>
        public static void createOrUpdateMemo(long sysid, String notizmemo, String wftableSyscode, String kurzbez, String kategorieBezeichnung, long? sysPerson)
        {
            using (DdOwExtended context = new DdOwExtended())
            {
                WFTABLE table = (from t in context.WFTABLE
                                 where t.SYSCODE == wftableSyscode
                                 select t).FirstOrDefault();

                WFMMEMO wfmmemo = (from c in context.WFMMEMO
                                   where c.SYSLEASE == sysid && c.WFMMKAT.BESCHREIBUNG == kategorieBezeichnung && c.SYSWFMTABLE == table.SYSWFTABLE
                                   select c).FirstOrDefault();

                if (wfmmemo == null)
                {
                    WFMMKAT kat = (from k in context.WFMMKAT
                                   where k.BESCHREIBUNG == kategorieBezeichnung
                                   select k).FirstOrDefault();

                    wfmmemo = new WFMMEMO();
                    wfmmemo.CREATEDATE = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDateNoTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                    wfmmemo.CREATETIME = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                    wfmmemo.CREATEUSER = getSysWfuser(sysPerson);
                    wfmmemo.SYSLEASE = sysid;
                    wfmmemo.SYSWFMTABLE = table.SYSWFTABLE;
                    wfmmemo.WFMMKAT = kat;
                    context.WFMMEMO.Add(wfmmemo);
                }
                else
                {
                    wfmmemo.EDITDATE = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDateNoTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                    wfmmemo.EDITTIME = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                    wfmmemo.EDITUSER = sysPerson;
                }
                
                wfmmemo.KURZBESCHREIBUNG = kurzbez;
                //BNRACHT-842
                if (notizmemo != null && notizmemo.Length > 4800)
                    wfmmemo.NOTIZMEMO = notizmemo.Substring(0, 4800);
                else
                    wfmmemo.NOTIZMEMO = notizmemo;

                context.SaveChanges();
            }
        }

        /// <summary>
        /// Erstellt eine neue Notiz oder aktualisiert die existierende für ANTRAG
        /// </summary>
        /// <param name="kategorieBezeichnung"></param>
        /// <param name="antragInput"></param>
        /// <param name="sysperson"></param>
        /// <returns></returns>
        public void createOrUpdateNotiz(String kategorieBezeichnung, AntragDto antragInput, long? sysperson)
        {
            using (DdOwExtended context = new DdOwExtended())
            {
                WFTABLE table = (from t in context.WFTABLE
                                 where t.SYSCODE == NOTIZSYSCODE
                                 select t).FirstOrDefault();

                WFMMEMO wfmmemo = (from c in context.WFMMEMO
                                   where c.SYSLEASE == antragInput.sysid && c.WFMMKAT.BESCHREIBUNG == kategorieBezeichnung && c.SYSWFMTABLE == table.SYSWFTABLE
                                   select c).FirstOrDefault();

                if (wfmmemo == null)
                {
                    WFMMKAT kat = (from k in context.WFMMKAT
                                   where k.BESCHREIBUNG == kategorieBezeichnung
                                   select k).FirstOrDefault();

                    wfmmemo = new WFMMEMO();
                    wfmmemo.CREATEDATE = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDateNoTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                    wfmmemo.CREATETIME = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                    wfmmemo.CREATEUSER = getSysWfuser(sysperson);
                    wfmmemo.SYSLEASE = antragInput.sysid;
                    wfmmemo.SYSWFMTABLE = table.SYSWFTABLE;
                    wfmmemo.WFMMKAT = kat;
                    context.WFMMEMO.Add(wfmmemo);
                }
                else
                {
                    wfmmemo.EDITDATE = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDateNoTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                    wfmmemo.EDITTIME = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                    wfmmemo.EDITUSER = getSysWfuser(sysperson);
                }
                wfmmemo.KURZBESCHREIBUNG = KURZTXTNOTIZ;
                //BNRACHT-842
                if (antragInput.zusammenfassung != null && antragInput.zusammenfassung.Length > 4800)
                    wfmmemo.NOTIZMEMO = antragInput.zusammenfassung.Substring(0, 4800);
                else
                    wfmmemo.NOTIZMEMO = antragInput.zusammenfassung;

                // Ticket#2012072310000126 — CR16425 (Ablösen): Memo-Erzeugung
                if (kategorieBezeichnung.Equals(NOTIZKAT_ABLOESEN))
                {
                    wfmmemo.PDEC1501 = (decimal?)antragInput.abloese1;
                    wfmmemo.PDEC1502 = (decimal?)antragInput.abloese2;
                    wfmmemo.PDEC1503 = (decimal?)antragInput.abloese3;
                }
                context.SaveChanges();
            }
        }

        public static long? getSysWfuser(long? sysperson)
        {
            if (!sysperson.HasValue) return sysperson;

            using (DdOiQueueExtended ctx = new DdOiQueueExtended())
            {
                return ctx.ExecuteStoreQuery<long>("select syswfuser from wfuser where sysperson=" + sysperson.Value).FirstOrDefault();
            }


        }
        /// <summary>
        /// copyNotizenAngebotToAntrag
        /// Ticket#2012083110000047 — Übernahme Memos in den Antrag (ProcessAngebotToAntrag)
        /// </summary>
        /// <param name="angebotSysId"></param>
        /// <param name="antragSysId"></param>
        public void copyNotizenAngebotToAntrag(long angebotSysId, long antragSysId)
        {
            List<String> cfgVarKatList = null;
            using (DdOiQueueExtended contextQueue = new DdOiQueueExtended())
            {
                cfgVarKatList = (from cfgVar in contextQueue.CFGVAR
                                 join cfgSec in contextQueue.CFGSEC on cfgVar.CFGSEC.SYSCFGSEC equals cfgSec.SYSCFGSEC
                                 join cfg in contextQueue.CFG on cfgSec.CFG.SYSCFG equals cfg.SYSCFG
                                 where cfg.CODE.ToUpper().Equals("ANGEBOT2ANTRAG") && cfgSec.CODE.ToUpper().Equals("MEMO_KATEGORIEN")
                                 select cfgVar.CODE.ToUpper()).ToList();
            }

            using (DdOwExtended context = new DdOwExtended())
            {
                WFTABLE tableANG = (from t in context.WFTABLE
                                 where t.SYSCODE == "ANGEBOT"
                                 select t).FirstOrDefault();

                WFTABLE tableANT = (from t in context.WFTABLE
                                 where t.SYSCODE == "ANTRAG"
                                 select t).FirstOrDefault();

                List<WFMMEMO> wfmMemoAngebotList = (from wfmmemo in context.WFMMEMO
                                                    where cfgVarKatList.Contains(wfmmemo.WFMMKAT.BESCHREIBUNG.ToUpper()) && wfmmemo.SYSLEASE == angebotSysId
                                                    && wfmmemo.SYSWFMTABLE == tableANG.SYSWFTABLE
                                                    select wfmmemo).ToList();
                foreach (var wfmMemoAngebot in wfmMemoAngebotList)
                {
                    // es gibt keine getKey in diesem context
                    
                    if (wfmMemoAngebot.WFMMKAT  == null)
                        context.Entry(wfmMemoAngebot).Reference(f => f.WFMMKAT).Load();

                    WFMMKAT kat = (from wfmkat in context.WFMMKAT
                                   where wfmkat.SYSWFMMKAT == wfmMemoAngebot.WFMMKAT.SYSWFMMKAT
                                   select wfmkat).FirstOrDefault();

                    

                    WFMMEMO wfmMemoAntrag = new WFMMEMO();
                    wfmMemoAntrag.CREATEDATE = wfmMemoAngebot.CREATEDATE;// Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDateNoTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                    wfmMemoAntrag.CREATETIME = wfmMemoAngebot.CREATETIME; //Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                    wfmMemoAntrag.CREATEUSER =  wfmMemoAngebot.CREATEUSER;
                    wfmMemoAntrag.SYSLEASE = antragSysId;
                    wfmMemoAntrag.SYSWFMTABLE = tableANT.SYSWFTABLE;
                    wfmMemoAntrag.WFMMKAT = kat;

                    wfmMemoAntrag.KURZBESCHREIBUNG = "Angebot-" + wfmMemoAngebot.KURZBESCHREIBUNG;
                    // WFMMEMO.KURZBESCHREIBUNG ist VARCHAR2(60): hintere Zeichen werden abgeschnitten
                    if (wfmMemoAntrag.KURZBESCHREIBUNG.Length > 60)
                        wfmMemoAntrag.KURZBESCHREIBUNG = wfmMemoAntrag.KURZBESCHREIBUNG.Remove(60);

                    if (wfmMemoAngebot.NOTIZMEMO != null)
                    {

                        //BNRACHT-842
                        if (wfmMemoAngebot.NOTIZMEMO.Length > 4800)
                            wfmMemoAntrag.NOTIZMEMO = wfmMemoAngebot.NOTIZMEMO.Substring(0, 4800);
                        else
                            wfmMemoAntrag.NOTIZMEMO = wfmMemoAngebot.NOTIZMEMO;


                    }

                    context.WFMMEMO.Add(wfmMemoAntrag);
                    context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Übernahme Dokumente vom Angebot in den Antrag
        /// </summary>
        /// <param name="angebotSysId"></param>
        /// <param name="antragSysId"></param>
        public void copyDms(long angebotSysId, long antragSysId)
        {
            using (DdOwExtended ctxOw = new DdOwExtended())
            {
                List<DMSDOCAREA> docareas = (from d in ctxOw.DMSDOCAREA
                                             where d.AREA.Equals("ANGEBOT") && d.SYSID == angebotSysId
                                             select d).ToList();

                foreach(DMSDOCAREA area in docareas)
                {
                    DMSDOCAREA dnew = new DMSDOCAREA();
                    dnew.SYSID = antragSysId;
                    dnew.RANG = area.RANG;
                    dnew.AREA = "ANTRAG";
                    dnew.SYSDMSDOC = area.SYSDMSDOC;
                    ctxOw.DMSDOCAREA.Add(dnew);
                }
                ctxOw.SaveChanges();
            }
        }


        public void createOrUpdateDisclaimersMemo(long sysid, long syswfuser, string isocode)
        {
            using (DdOwExtended context = new DdOwExtended())
            {

                int? DisclaimerTexttyp = 21;
                int? DisclaimerPopuptyp = 23;
                String text;


                WFTABLE table = (from t in context.WFTABLE
                                 where t.SYSCODE == NOTIZSYSCODE
                                 select t).FirstOrDefault();

                // Disclaimer Text
                WFMMEMO wfmmemoText = (from c in context.WFMMEMO
                                       where c.SYSLEASE == sysid && c.WFMMKAT.TYP == DisclaimerTexttyp && c.SYSWFMTABLE == table.SYSWFTABLE
                                       select c).FirstOrDefault();

                if (wfmmemoText == null)
                {
                    WFMMKAT kat = (from k in context.WFMMKAT
                                   where k.TYP == DisclaimerTexttyp
                                   select k).FirstOrDefault();


                    WFMMEMO disclaimerText = new WFMMEMO();
                    disclaimerText.CREATEDATE = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDateNoTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                    disclaimerText.CREATETIME = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                    disclaimerText.CREATEUSER = syswfuser;
                    disclaimerText.SYSLEASE = sysid;
                    disclaimerText.SYSWFMTABLE = table.SYSWFTABLE;
                    disclaimerText.SYSWFMMKAT = kat.SYSWFMMKAT;

                    disclaimerText.KURZBESCHREIBUNG = "Disclaimer Text";

                    text = getDisclaimerText(isocode);
                    /*if (text != null)
                    {
                        if (text.Length > 4800)
                            disclaimerText.NOTIZMEMO = text.Substring(0, 4800);
                        else
                            disclaimerText.NOTIZMEMO = text;


                    }*/
                    context.WFMMEMO.Add(disclaimerText);

                    //add to WFMMEMOEXT with whole text
                    context.SaveChanges();
                    WFMMEMOEXT discExt = new WFMMEMOEXT();
                    discExt.SYSWFMMEMO= disclaimerText.SYSWFMMEMO;
                    discExt.INHALT = text;
                    discExt.RANK = 1;
                    context.WFMMEMOEXT.Add(discExt);
                    
                }
                else
                {
                    wfmmemoText.EDITDATE = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDateNoTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                    wfmmemoText.EDITUSER = syswfuser;

                }


                // Disclaimer Popup
                WFMMEMO wfmmemoPopup = (from c in context.WFMMEMO
                                        where c.SYSLEASE == sysid && c.WFMMKAT.TYP == DisclaimerPopuptyp && c.SYSWFMTABLE == table.SYSWFTABLE
                                        select c).FirstOrDefault();

                if (wfmmemoPopup == null)
                {


                    WFMMKAT kat2 = (from k in context.WFMMKAT
                                    where k.TYP == DisclaimerPopuptyp
                                    select k).FirstOrDefault();

                    WFMMEMO disclaimerPopup = new WFMMEMO();
                    disclaimerPopup.CREATEDATE =
                        Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDateNoTime(
                            Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                    disclaimerPopup.CREATETIME =
                        Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(
                            Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                    disclaimerPopup.CREATEUSER = syswfuser;
                    disclaimerPopup.SYSLEASE = sysid;
                    disclaimerPopup.SYSWFMTABLE = table.SYSWFTABLE;
                    disclaimerPopup.SYSWFMMKAT = kat2.SYSWFMMKAT;

                    disclaimerPopup.KURZBESCHREIBUNG = "Disclaimer Popup";

                    text = getDisclaimerPopup(isocode);
                   /* if (text != null)
                    {
                        if (text.Length > 4800)
                            disclaimerPopup.NOTIZMEMO = text.Substring(0, 4800);
                        else
                            disclaimerPopup.NOTIZMEMO = text;


                    }*/
                    context.WFMMEMO.Add(disclaimerPopup);
                    //add to WFMMEMOEXT with whole text
                    context.SaveChanges();
                    WFMMEMOEXT discExt = new WFMMEMOEXT();
                    discExt.SYSWFMMEMO=disclaimerPopup.SYSWFMMEMO;
                    discExt.INHALT = text;
                    discExt.RANK = 1;
                    context.WFMMEMOEXT.Add(discExt);
                }
                else
                {
                    wfmmemoPopup.EDITDATE = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDateNoTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                    wfmmemoPopup.EDITUSER = syswfuser;

                }

                context.SaveChanges();

            }
        }


        private string getDisclaimerText(string isocode)
        {
            
            using (DdOdExtended odCtx = new DdOdExtended())
            {

                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pisoCode", Value = isocode });

                List<String> texts = odCtx.ExecuteStoreQuery<String>(QUERYDISTEXT, parameters.ToArray()).ToList();
                return String.Join("",texts);
            }

            
        }

        private string getDisclaimerPopup(string isocode)
        {

            
            using (DdOdExtended odCtx = new DdOdExtended())
            {

                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "pisoCode", Value = isocode });

                 List<String> texts = odCtx.ExecuteStoreQuery<String>(QUERYDISPOPUP, parameters.ToArray()).ToList();
                return String.Join("",texts);
              
            }

            
        }

        /// <summary>
        /// Creates or Updates the card-entry for a card product, referencing it to antrag and angebot and setting the emboss field
        /// </summary>
        /// <param name="sysangebot"></param>
        /// <param name="emboss"></param>
        /// <param name="isCard"></param>
        /// <param name="sysantrag"></param>
        public void createOrUpdateCard(long sysangebot, String emboss, bool isCard, long sysantrag)
        {
            using(DdOlExtended ctx = new DdOlExtended())
            {
                if(!isCard)//no more card for this offer
                {
                    ctx.ExecuteStoreCommand("delete from card where (sysangebot=" + sysangebot + " or sysantrag=" + sysantrag + ") and status is null");
                }
                else
                {
                    long syscard = ctx.ExecuteStoreQuery<long>("select syscard from card where (sysangebot=" + sysangebot + " or sysantrag=" + sysantrag + ")", null).FirstOrDefault();

                    CARD card = null;
                    if (syscard > 0)
                    {   
                        card = (from c in ctx.CARD
                                where
                                    c.SYSCARD == syscard
                                select c).FirstOrDefault();
                    }
                    else
                    { 
                        card = new CARD();
                        ctx.CARD.Add(card);
                    }
                    card.EMBOSS = emboss;
                    if (sysangebot > 0)
                        card.SYSANGEBOT= sysangebot;
                    if (sysangebot > 0)
                        card.SYSANTRAG=sysantrag;
                }

                ctx.SaveChanges();
            }
        }

        /// <summary>
        /// Erstellt Text wenn Bestandskunde
        /// </summary>
        /// <param name="sysangebot"></param>
        /// <param name="sysperson"></param>
        /// <param name="isocode"></param>
        public void createOrUpdateBestandskundeMemo(long sysangebot, long sysperson, string isocode)
        {
            using (DdOwExtended context = new DdOwExtended())
            {

                int DisclaimerTexttyp = 82;
                String text;

                WFTABLE table = (from t in context.WFTABLE
                                 where t.SYSCODE == "ANGEBOT"
                                 select t).FirstOrDefault();

                // Disclaimer Text
                WFMMEMO wfmmemoText = (from c in context.WFMMEMO
                                       where c.SYSLEASE == sysangebot && c.WFMMKAT.TYP == DisclaimerTexttyp && c.SYSWFMTABLE == table.SYSWFTABLE
                                       select c).FirstOrDefault();

                if (wfmmemoText == null)
                {
                    WFMMKAT kat = (from k in context.WFMMKAT
                                   where k.TYP == DisclaimerTexttyp
                                   select k).FirstOrDefault();


                    WFMMEMO disclaimerText = new WFMMEMO();
                    disclaimerText.CREATEDATE = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDateNoTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                    disclaimerText.CREATETIME = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                    disclaimerText.CREATEUSER = getSysWfuser(sysperson);
                    disclaimerText.SYSLEASE = sysangebot;
                    disclaimerText.SYSWFMTABLE = table.SYSWFTABLE;
                    disclaimerText.WFMMKAT = kat;

                    disclaimerText.KURZBESCHREIBUNG = "Info";

                    EaiparDao eaiParDao = new EaiparDao();
                    String newQueryString = eaiParDao.getEaiParFileByCode("B2C_KDB_QUERY", QUERYDISTEXTANGEBOT);
                    newQueryString = newQueryString.Replace(":pisocode", "'" + isocode + "'");
                    newQueryString = newQueryString.Replace(":disccode", "'B2C_BESTANDSKUNDE'");
                    using (DdOdExtended odCtx = new DdOdExtended())
                    {
                        List<String> texts = odCtx.ExecuteStoreQuery<String>(newQueryString, null).ToList();
                        text = String.Join("", texts);
                    }

                   /* if (text != null)
                    {
                        if (text.Length > 4800)
                            disclaimerText.NOTIZMEMO = text.Substring(0, 4800);
                        else
                            disclaimerText.NOTIZMEMO = text;


                    }*/
                    context.WFMMEMO.Add(disclaimerText);
                    //add to WFMMEMOEXT with whole text
                    context.SaveChanges();
                    WFMMEMOEXT discExt = new WFMMEMOEXT();
                    discExt.SYSWFMMEMO= disclaimerText.SYSWFMMEMO;
                    discExt.INHALT = text;
                    discExt.RANK = 1;
                    context.WFMMEMOEXT.Add(discExt);
                }
                else
                {
                    wfmmemoText.EDITDATE = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDateNoTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                    wfmmemoText.EDITUSER = sysperson;

                }




                context.SaveChanges();

            }
        }

        /// <summary>
        /// Erstellt AGB-Text beim Einreichen
        /// </summary>
        /// <param name="sysangebot"></param>
        /// <param name="sysperson"></param>
        /// <param name="isocode"></param>
        /// <param name="ma"></param>
        public void createOrUpdateDisclaimersMemoAngebot(long sysangebot, long syswfuser, string isocode, bool ma)
        {
            using (DdOwExtended context = new DdOwExtended())
            {

                int? DisclaimerTexttyp = 21;
                if (ma) DisclaimerTexttyp = 24;
                

                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysangebot", Value = sysangebot });
                
                String prcode = context.ExecuteStoreQuery<String>(QUERYPRODUCTCODE,parameters.ToArray()).FirstOrDefault();
                if (prcode == null) return;

                String disclaimerInhalt = getDisclaimerTextAngebot(isocode, prcode, ma);
                //BNRSZ-361 - when no disclaimer-text, do not create
                if (disclaimerInhalt == null||disclaimerInhalt.Trim().Length==0) return;

                WFTABLE table = (from t in context.WFTABLE
                                 where t.SYSCODE == "ANGEBOT"
                                 select t).FirstOrDefault();

                // Disclaimer Text
                WFMMEMO wfmmemoText = (from c in context.WFMMEMO
                                       where c.SYSLEASE == sysangebot && c.WFMMKAT.TYP == DisclaimerTexttyp && c.SYSWFMTABLE == table.SYSWFTABLE
                                       select c).FirstOrDefault();

                if (wfmmemoText == null)
                {
                    WFMMKAT kat = (from k in context.WFMMKAT
                                   where k.TYP == DisclaimerTexttyp
                                   select k).FirstOrDefault();

                    WFMMEMO disclaimerText = new WFMMEMO();
                    disclaimerText.CREATEDATE = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDateNoTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                    disclaimerText.CREATETIME = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                    disclaimerText.CREATEUSER = syswfuser;
                    disclaimerText.SYSLEASE = sysangebot;
                    disclaimerText.SYSWFMTABLE = table.SYSWFTABLE;
                    disclaimerText.WFMMKAT = kat;
                    disclaimerText.KURZBESCHREIBUNG = "Disclaimer Text";
                    context.WFMMEMO.Add(disclaimerText);
                    //add to WFMMEMOEXT with whole text
                    context.SaveChanges();
                    
                    WFMMEMOEXT discExt = new WFMMEMOEXT();
                    discExt.SYSWFMMEMO=disclaimerText.SYSWFMMEMO;
                    discExt.INHALT = disclaimerInhalt;
                    discExt.RANK = 1;
                    context.WFMMEMOEXT.Add(discExt);
                }
                else
                {
                    wfmmemoText.EDITDATE = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDateNoTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                    wfmmemoText.EDITUSER = syswfuser;

                }
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Returns the b2c product specific disclaimer text or null if not defined
        /// </summary>
        /// <param name="isocode"></param>
        /// <param name="productCode"></param>
        /// <param name="ma"></param>
        /// <returns></returns>
        private string getDisclaimerTextAngebot(string isocode, String productCode, bool ma)
        {


            
            String disccode = "DISCLAIMER_" + productCode + "_AGB_LONG";
            if (ma) disccode = "DISCLAIMER_" + productCode + "_AGB_LONG_2";
            EaiparDao eaiParDao = new EaiparDao();
            String newQueryString = eaiParDao.getEaiParFileByCode("DISCLAIMERQUERY", QUERYDISTEXTANGEBOT);
            newQueryString = newQueryString.Replace(":pisocode", "'"+isocode+"'");
            newQueryString = newQueryString.Replace(":disccode", "'"+disccode+"'");
            using (DdOdExtended odCtx = new DdOdExtended())
            {
                List<String> texts = odCtx.ExecuteStoreQuery<String>(newQueryString,null).ToList();
                if (texts == null || texts.Count == 0) return null;
                return String.Join("", texts);
            }

            
        }

     

        /// <summary>
        /// Produkt-Prüfung:
        /// Liefert diese Methode einen Wert größer 0, dann wurde eine fakultative Ratenabsicherung gewählt.
        /// </summary>
        /// <param name="sysid">Primaerschluessel</param>
        /// <returns></returns>
        public long getFakultativeRatenabsicherung(long sysid)
        {
            using (DdOlExtended context = new DdOlExtended())
            {
                long fakultativeRatenabsicherung = 0;

                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysAntrag", Value = sysid });

                fakultativeRatenabsicherung = context.ExecuteStoreQuery<long>(GETFAKRA, parameters.ToArray()).FirstOrDefault();

                return fakultativeRatenabsicherung;
            }
        }

        /// <summary>
        /// Contains the code of ddlkpcol and the value of a ddlkpspos entry
        /// </summary>
        class DdlkpValueDto
        {
            public long sysddlkpspos { get; set; }
            public string value { get; set; }
            public string code { get; set; }
        }
        /// <summary>
        /// Contains info for updating/creating a value of a ddlkpspos entrys
        /// </summary>
        class DdlkpValueUpdateDto
        {
            public String code { get; set; }
            public String rubrik { get; set; }
            public long sysddlkpcol { get; set; }

        }

        class EigentuemerSeit
        {
            public string value { get; set; }
            public string code { get; set; }
        }

        /// <summary>
        /// Sets the casa/diploma special fields
        /// </summary>
        /// <param name="angebot"></param>
        private void setCasaDiplomaSpecialFields(AngAntDto angebot, String area, long sysid, String vttypcode)
        {
            if (vttypcode == null) return;
            String vtcheck = vttypcode.ToLower();
            if (vtcheck.IndexOf("casa") < 0 && vtcheck.IndexOf("diploma") < 0)
                return;

            using (DdOdExtended context = new DdOdExtended())
            {
                
                //get all column ids for the special fields
                List<DdlkpValueUpdateDto> cols = context.ExecuteStoreQuery<DdlkpValueUpdateDto>(QUERYUPD_DDLKPSPOS_B2C, null).ToList();

                //search for pspos with code
                //if non-existent, add new with certain ddlkpcol id for code
                //if extistent, set data
                if (vtcheck.IndexOf("casa")>-1)
                { 
                    String product = "CASA";


                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "area", Value = area });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "prcode", Value = product });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = sysid });
                    List<DdlkpValueDto> vals = context.ExecuteStoreQuery<DdlkpValueDto>(QUERYDDLKPSPOS_B2C, parameters.ToArray()).ToList();
                    createOrUpdateDDLKPSPOS(context, "ORT", angebot.eigenheim_ort, product, area, sysid, cols, vals);
                    createOrUpdateDDLKPSPOS(context, "PLZ", angebot.eigenheim_plz, product, area, sysid, cols, vals);
                    createOrUpdateDDLKPSPOS(context, "HSNR", angebot.eigenheim_str_nr, product, area, sysid, cols, vals);
                    createOrUpdateDDLKPSPOS(context, "STRASSE", angebot.eigenheim_strasse, product, area, sysid, cols, vals);
                    createOrUpdateDDLKPSPOS(context, "JAHR", angebot.eigentuemer_seit_jahr, product, area, sysid, cols, vals);
                    createOrUpdateDDLKPSPOS(context, "MONAT", angebot.eigentuemer_seit_monat, product, area, sysid, cols, vals);
                    createOrUpdateDDLKPSPOS(context, "HYPOTHEK", angebot.hypothekenhoehe.HasValue ? angebot.hypothekenhoehe.Value.ToString() : "0", product, area, sysid, cols, vals);
                }
                if (vtcheck.IndexOf("diploma") > -1)
                {
                    String product = "DIPLOMA";
                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "area", Value = area });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "prcode", Value = product });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = sysid });
                    List<DdlkpValueDto> vals = context.ExecuteStoreQuery<DdlkpValueDto>(QUERYDDLKPSPOS_B2C, parameters.ToArray()).ToList();
                    createOrUpdateDDLKPSPOS(context, "SCHULE", angebot.schule_name, product, area, sysid, cols, vals);
                    createOrUpdateDDLKPSPOS(context, "ORT", angebot.schule_ort, product, area, sysid, cols, vals);
                    createOrUpdateDDLKPSPOS(context, "PLZ", angebot.schule_plz, product, area, sysid, cols, vals);
                    createOrUpdateDDLKPSPOS(context, "HSNR", angebot.schule_str_nr, product, area, sysid, cols, vals);
                    createOrUpdateDDLKPSPOS(context, "STRASSE", angebot.schule_strasse, product, area, sysid, cols, vals);
                }
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Creates or updates a ddlkpspos, finding the colcode in the cols-list and the potential pspos save-id in vals-list
        /// </summary>
        /// <param name="context"></param>
        /// <param name="code"></param>
        /// <param name="value"></param>
        /// <param name="product"></param>
        /// <param name="area"></param>
        /// <param name="sysid"></param>
        /// <param name="cols"></param>
        /// <param name="vals"></param>
        private static void createOrUpdateDDLKPSPOS(DdOdExtended context, String code, String value, String product, String area, long sysid, List<DdlkpValueUpdateDto> cols, List<DdlkpValueDto> vals)
        {
            DDLKPSPOS output = null;
            if (vals != null && vals.Count > 0)
            {
                long tmp_id = (from v in vals where v.code.Equals(code) select v.sysddlkpspos).FirstOrDefault();
                if (tmp_id > 0)
                {
                    output = (from c in context.DDLKPSPOS
                              where c.SYSDDLKPSPOS == tmp_id
                              select c).FirstOrDefault();
                    if (output != null)
                        output.VALUE = value;
                }
            }
            if (output == null)
            {
                output = new DDLKPSPOS();
                long colid = (from t in cols
                              where t.rubrik.Equals(product) && t.code.Equals(code)
                              select t.sysddlkpcol).FirstOrDefault();
                output.SYSDDLKPCOL= colid;
                output.AREA = area;
                output.SYSID = sysid;
                output.VALUE = value;
                output.ACTIVEFLAG = 1;
                context.DDLKPSPOS.Add(output);
            }

        }


        /// <summary>
        /// Reads the casa/diploma special fields for B2C
        /// </summary>
        /// <param name="angebot"></param>
        /// <param name="area"></param>
        /// <param name="sysid"></param>
        private void getCasaDiplomaSpecialFields(AngAntDto angebot, String area, long sysid)
        {
            try
            {
                using (DdOlExtended context = new DdOlExtended())
                {
                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "area", Value = area });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "prcode", Value = "CASA" });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = sysid });
                    List<DdlkpValueDto> vals = context.ExecuteStoreQuery<DdlkpValueDto>(QUERYDDLKPSPOS_B2C, parameters.ToArray()).ToList();
                    if (vals != null && vals.Count > 0)
                    {
                        angebot.eigenheim_ort = (from v in vals where v.code.Equals("ORT") select v.value).FirstOrDefault();
                        angebot.eigenheim_plz = (from v in vals where v.code.Equals("PLZ") select v.value).FirstOrDefault();
                        angebot.eigenheim_str_nr = (from v in vals where v.code.Equals("HSNR") select v.value).FirstOrDefault();
                        angebot.eigenheim_strasse = (from v in vals where v.code.Equals("STRASSE") select v.value).FirstOrDefault();
                        angebot.eigentuemer_seit_monat = (from v in vals where v.code.Equals("MONAT") select v.value).FirstOrDefault();
                        angebot.eigentuemer_seit_jahr = (from v in vals where v.code.Equals("JAHR") select v.value).FirstOrDefault();
                        String hypo = (from v in vals where v.code.Equals("HYPOTHEK") select v.value).FirstOrDefault();
                        try
                        {
                            angebot.hypothekenhoehe = Double.Parse(hypo, CultureInfo.InvariantCulture);
                        }
                        catch (Exception )
                        {
                            //invalid number, ignore
                        }
                    }
                    parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "area", Value = area });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "prcode", Value = "DIPLOMA" });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = sysid });
                    vals = context.ExecuteStoreQuery<DdlkpValueDto>(QUERYDDLKPSPOS_B2C, parameters.ToArray()).ToList();
                    if (vals != null && vals.Count > 0)
                    {
                        angebot.schule_name = (from v in vals where v.code.Equals("SCHULE") select v.value).FirstOrDefault();
                        angebot.schule_plz = (from v in vals where v.code.Equals("PLZ") select v.value).FirstOrDefault();
                        angebot.schule_str_nr = (from v in vals where v.code.Equals("HSNR") select v.value).FirstOrDefault();
                        angebot.schule_strasse = (from v in vals where v.code.Equals("STRASSE") select v.value).FirstOrDefault();
                        angebot.schule_ort = (from v in vals where v.code.Equals("ORT") select v.value).FirstOrDefault();

                    }
                }
            }
            catch (Exception e)
            {
                _log.Error("getCasaDiplomaSpecialFields", e);
            }
        }

        /// <summary>
        /// Produkt-Prüfung:
        /// Liefert das Eigentümer-Seit-Datum für den Antrag (für VTTYP = CASA)
        /// </summary>
        /// <param name="sysid">Antrag-Id</param>
        /// <returns></returns>
        public DateTime getEigentuemerSeit(long sysid)
        {
            DateTime eigentSeitDatum = DateTime.Now;

            using (DdOlExtended context = new DdOlExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysAntrag", Value = sysid });

                List<EigentuemerSeit> eigentSeitList = context.ExecuteStoreQuery<EigentuemerSeit>(GETEIGENTSEIT, parameters.ToArray()).ToList();
                if (eigentSeitList.Count > 0)
                {
                    String ddlValue = String.Empty;
                    EigentuemerSeit eigentSeit = eigentSeitList.Find(x => x.code.Equals("MONAT"));
                    if (eigentSeit != null)
                    {
                        ddlValue = eigentSeit.value;
                    }
                    int monat = 0;
                    Int32.TryParse(ddlValue, out monat);

                    ddlValue = String.Empty;
                    eigentSeit = eigentSeitList.Find(x => x.code.Equals("JAHR"));
                    if (eigentSeit != null)
                    {
                        ddlValue = eigentSeit.value;
                    }
                    int jahr = 0;
                    Int32.TryParse(ddlValue, out jahr);

                    try
                    {
                        eigentSeitDatum = new DateTime(jahr, monat, 1);
                    }
                    catch (Exception)
                    {
                        _log.Debug("CHECKANTRAG_CASA: Falsches Datum für den Parameter EigentümerSeit.");
                    }
                }
            }
            return eigentSeitDatum;
        }


        public long getPrkgroupByAntragID(long sysid)
        {
            long sysprkgroup = 0;
            const string RATINGQUERY = "select rating.sysrating from rating where area = :parea and sysid=:psysid and flag1=0";
            using (Cic.OpenOne.Common.Model.DdOw.DdOwExtended context = new Cic.OpenOne.Common.Model.DdOw.DdOwExtended())
            {

                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "parea", Value = "ANTRAG" });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysid", Value = sysid });

                long sysrating =
                    context.ExecuteStoreQuery<long>(RATINGQUERY, parameters.ToArray())
                        .FirstOrDefault();
                if (sysrating>0)
                {
                    

                    var ratingsimulvar = from rs in context.RATINGSIMUL
                                         where rs.SYSRATING == sysrating
                                         select rs.SYSID;

                    sysprkgroup = (long)ratingsimulvar.FirstOrDefault();


                }

            }

            return sysprkgroup;

        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="schwacke"></param>
        /// <returns></returns>
        public long getObtypBySchwacke(string schwacke)
        {
            long sysobtyp = 0;
            const string QUERY = "select sysobtyp from obtyp where schwacke=:schwacke";
            using (Cic.OpenOne.Common.Model.DdOw.DdOwExtended ctx = new Cic.OpenOne.Common.Model.DdOw.DdOwExtended())
            {

                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "schwacke", Value = schwacke });
                sysobtyp = ctx.ExecuteStoreQuery<long>(QUERY, parameters.ToArray()).FirstOrDefault();
            }

            return sysobtyp;

        }

        /// <summary>
        /// check if the contract is allowed to be extended CR139
        /// </summary>
        /// <returns>contract is allowed to be extended</returns>
        public bool checkRwVerlVerfuegbarWeb(long sysvt)
        {
            EaiparDao eaiParDao = new EaiparDao();
            String newQueryString = eaiParDao.getEaiParFileByCode("RW_VERL_VERFUEGBAR_WEB", RW_VERL_VERFUEGBAR_WEB_DEFAULT);
            newQueryString = "select ("+newQueryString+") isExtendible from vt where sysid="+sysvt;
            using (DdOdExtended odCtx = new DdOdExtended())
            {
                return odCtx.ExecuteStoreQuery<bool>(newQueryString, null).FirstOrDefault();   
            }
        }

        /// <summary>
        /// Updates or creates the extident-Values
        /// </summary>
        /// <param name="exti"></param>
        public void createOrUpdateExtident(ExtidentDto exti)
        {
            using (OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
            {

                DbConnection con = (ctx.Database.Connection);

                exti.sysextident = ctx.ExecuteStoreQuery<long>("select sysextident from extident where codeextidenttyp='" + CODE_EXTIDENT_ANTRAGSID_B2C + "' and area='" + exti.area + "' and sysarea=" + exti.sysarea + " and source='" + exti.source + "'").FirstOrDefault();

                if (exti.sysextident == 0)
                {
                    ctx.insertData(@"INSERT INTO CIC.EXTIDENT(CODEEXTIDENTTYP,AREA,SYSAREA,EXTIDENTVALUE,SOURCE) 
                                                        VALUES (:codeextidenttyp, :area,:sysarea,:extidentvalue,:source)", exti);
                }
                else//update
                {
                    List<Devart.Data.Oracle.OracleParameter> parameters2 = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p0", Value = exti.codeextidenttyp });
                    parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p1", Value = exti.extidentvalue });
                    parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p2", Value = exti.sysextident });
                    parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p3", Value = exti.source });
                    ctx.ExecuteStoreCommand("update extident set CODEEXTIDENTTYP=:p0,EXTIDENTVALUE=:p1, SOURCE=:p3 where sysextident=:p2", parameters2.ToArray());
                }
                ctx.SaveChanges();
            }
        }
        #region Private Methods

        /// <summary>
        /// REturns the memo for the wftable and kategorie
        /// </summary>
        /// <param name="kategorieBezeichnung"></param>
        /// <param name="syslease"></param>
        /// <param name="wftablecode"></param>
        /// <returns></returns>
        private String MyGetMemo(String kategorieBezeichnung, long syslease, String wftablecode)
        {
            AntragDto rval = new AntragDto();
            using (DdOwExtended context = new DdOwExtended())
            {
                WFTABLE table = (from t in context.WFTABLE
                                 where t.SYSCODE == wftablecode
                                 select t).FirstOrDefault();

                WFMMEMO wfmmemo = (from c in context.WFMMEMO
                                   where c.SYSLEASE == syslease && c.WFMMKAT.BESCHREIBUNG.Equals(kategorieBezeichnung) && c.SYSWFMTABLE == table.SYSWFTABLE
                                   select c).FirstOrDefault();

                if (wfmmemo != null)
                {


                    //BNRACHT-842
                    String notizmemoStr = "";
                    if (wfmmemo.NOTIZMEMO != null && wfmmemo.NOTIZMEMO.IndexOf('\0') > 0)
                    {
                        notizmemoStr = wfmmemo.NOTIZMEMO.Substring(0, wfmmemo.NOTIZMEMO.IndexOf('\0'));
                        if (notizmemoStr.Length > 4800)
                            return notizmemoStr.Substring(0, 4800);
                        return notizmemoStr;
                    }
                    notizmemoStr = wfmmemo.NOTIZMEMO;
                    if (notizmemoStr == null)
                        notizmemoStr = "";
                    if (notizmemoStr.Length > 4800)
                        return notizmemoStr.Substring(0, 4800);
                    return notizmemoStr;

                }
            }
            return null;
        }

        /// <summary>
        /// MyGetAbloesenFromNotiz
        /// </summary>
        /// <param name="kategorieBezeichnung"></param>
        /// <param name="syslease"></param>
        /// <returns></returns>
        private AntragDto MyGetAntragFromNotiz(String kategorieBezeichnung, long syslease)
        {
            AntragDto rval = new AntragDto();
            using (DdOwExtended context = new DdOwExtended())
            {
                WFTABLE table = (from t in context.WFTABLE
                                 where t.SYSCODE == NOTIZSYSCODE
                                 select t).FirstOrDefault();

                WFMMEMO wfmmemo = (from c in context.WFMMEMO
                                   where c.SYSLEASE == syslease && c.WFMMKAT.BESCHREIBUNG.Equals(kategorieBezeichnung) && c.SYSWFMTABLE == table.SYSWFTABLE
                                   select c).FirstOrDefault();

                if (wfmmemo != null)
                {


                    //BNRACHT-842
                    String notizmemoStr = "";
                    if (wfmmemo.NOTIZMEMO != null && wfmmemo.NOTIZMEMO.IndexOf('\0') > 0)
                    {
                        notizmemoStr = wfmmemo.NOTIZMEMO.Substring(0, wfmmemo.NOTIZMEMO.IndexOf('\0'));
                        if (notizmemoStr.Length > 4800)
                            rval.zusammenfassung = notizmemoStr.Substring(0, 4800);
                        else
                            rval.zusammenfassung = notizmemoStr;
                    }

                    // Ticket#2012072310000126 — CR16425 (Ablösen): Memo-Erzeugung
                    rval.abloese1 = (double)(wfmmemo.PDEC1501 ?? 0);
                    rval.abloese2 = (double)(wfmmemo.PDEC1502 ?? 0);
                    rval.abloese3 = (double)(wfmmemo.PDEC1503 ?? 0);
                }
            }
            return rval;
        }

        /// <summary>
        /// BANKNOW-57 Beteiligung Differenzleasing Memo
        /// </summary>
        /// <param name="sysId"></param>
        /// <param name="antragInput"></param>
        /// <param name="sysPerson"></param>
        /// <param name="isDiffLeasing"></param>
        private void MySaveDiffLeasingBestaetigung(long sysId, AntragDto antragInput, long? sysPerson, bool isDiffLeasing)
        {
            /*
             *  "Bestätigung Beteiligung Differenzleasing" lauten.
Die restlichen Informationen werden zum Teil bereits in den entsprechenden Feldern gespeichert. Kann für Notiz folgendes übergeben werden?
Datum: xx.xx.xxxx
Uhrzeit: xx:xx
Erfasser (ID und Name): xxxxx, xxxxxxxxxxxxxxxxx
Beteiligung Vertriebspartner (Betrag inkl. Steuer): xxxxxxxx CHF
Beteiligung BANK-now (Betrag inkl. Steuer): xxxxxxxx CHF
             * */
            String date = DateTime.Now.ToString("dd.MM.yyyy");
            String time = DateTime.Now.ToString("HH:mm");
            CultureInfo ch = new CultureInfo("de-CH");
            String vkName = "", vpName = "";
            String sumvp = "";
            String sumbn = "";
            long vpid = 0;
            String TITLE = "Bestätigung Beteiligung Differenzleasing";
            String inhalt = @"Datum: {0}
Uhrzeit: {1}
Erfasser: {2}, {3}
Beteiligung {6} (Betrag inkl. Steuer): {4} CHF
Beteiligung BANK-now (Betrag inkl. Steuer): {5} CHF";


            antragInput.sysid = sysId;
            int? kategorietyp = 20;

            using (DdOwExtended context = new DdOwExtended())
            {
                WFTABLE table = (from t in context.WFTABLE
                                 where t.SYSCODE == NOTIZSYSCODE
                                 select t).FirstOrDefault();

                WFMMEMO wfmmemo = (from c in context.WFMMEMO
                                   where c.SYSLEASE == antragInput.sysid && c.WFMMKAT.TYP  == kategorietyp && c.SYSWFMTABLE == table.SYSWFTABLE
                                   && c.KURZBESCHREIBUNG.Equals(TITLE)
                                   select c).FirstOrDefault();

                if (!isDiffLeasing)//löschen wenn kein diff-Leasing
                {
                    if (wfmmemo != null)
                    {
                        context.DeleteObject(wfmmemo);
                        context.SaveChanges();
                    }
                    return;
                }
                if (antragInput.kalkulation == null || antragInput.kalkulation.angAntSubvDto == null || antragInput.kalkulation.angAntSubvDto.Count == 0)
                {
                    _log.Warn("DiffLeasing without Subventions, no Confirmation will be saved");
                    return;
                }
                foreach (AngAntSubvDto subv in antragInput.kalkulation.angAntSubvDto)
                {
                    if (subv.syssubvg < 3)
                    {
                        sumbn = subv.betragBrutto.ToString(",0.00", ch);
                    }
                    else
                    {
                        vpid = subv.syssubvg;
                        sumvp = subv.betragBrutto.ToString(",0.00", ch);
                    }
                }


                vkName = context.ExecuteStoreQuery<String>("select name from person where sysperson=" + sysPerson, null).FirstOrDefault();
                vpName = context.ExecuteStoreQuery<String>("select name from person where sysperson=" + vpid, null).FirstOrDefault();
                if (wfmmemo == null)
                {
                    WFMMKAT kat = (from k in context.WFMMKAT
                                   where k.TYP  == kategorietyp
                                   select k).FirstOrDefault();

                    wfmmemo = new WFMMEMO();
                    wfmmemo.CREATEDATE = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDateNoTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                    wfmmemo.CREATETIME = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                    wfmmemo.CREATEUSER = getSysWfuser(sysPerson);
                    wfmmemo.SYSLEASE = antragInput.sysid;
                    wfmmemo.SYSWFMTABLE = table.SYSWFTABLE;
                    wfmmemo.WFMMKAT = kat;
                    context.WFMMEMO.Add(wfmmemo);
                }
                else
                {
                    wfmmemo.EDITDATE = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDateNoTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                    wfmmemo.EDITTIME = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                    wfmmemo.EDITUSER = getSysWfuser(sysPerson);
                }
                wfmmemo.KURZBESCHREIBUNG = TITLE;

                inhalt = String.Format(inhalt, new object[] { date, time, sysPerson, vkName, sumvp, sumbn, vpName });

                if (inhalt != null && inhalt.Length > 4800)
                    wfmmemo.NOTIZMEMO = inhalt.Substring(0, 4800);
                else
                    wfmmemo.NOTIZMEMO = inhalt;


                context.SaveChanges();
            }

        }

        /// <summary>
        /// Ticket#2012072310000126 — CR16425 (Ablösen): Memo-Erzeugung
        /// </summary>
        /// <param name="sysId"></param>
        /// <param name="antragInput"></param>
        /// <param name="sysPerson"></param>
        private void MySaveNotizen(long sysId, AntragDto antragInput, long? sysPerson)
        {
            antragInput.sysid = sysId;
            createOrUpdateNotiz(NOTIZKAT, antragInput, sysPerson);

            // Ist mindestens eines dieser drei Felder gefüllt, soll beim Speichern des Antrags ein Memo erzeugt werden.
            if (antragInput.abloese1 + antragInput.abloese2 + antragInput.abloese3 > 0)
            {
                createOrUpdateNotiz(NOTIZKAT_ABLOESEN, antragInput, sysPerson);
            }
            else
            {
                // Ticket#2012080710000281 — Memo löschen, wenn Ablösen entfernt werden
                deleteNotiz(NOTIZKAT_ABLOESEN, antragInput.sysid);
            }
        }

        /// <summary>
        /// getAntragBezeichnungen für Kalkulation
        /// </summary>
        /// <param name="antrag"></param>
        /// <returns></returns>
        private AntragDto MyGetAntragBezeichnungenKalkulation(AntragDto antrag)
        {
            CIC.Database.PRISMA.EF6.Model.PRPRODUCT prProduct = null;
            using ( PrismaExtended ctxp = new  PrismaExtended())
            {
                if (antrag.kalkulation != null)
                {
                    if (antrag.kalkulation.angAntKalkDto != null)
                    {
                        // prproduct --------------------------------------------------
                        var queryPrProduct = from PRPRODUCT in ctxp.PRPRODUCT
                                             where PRPRODUCT.SYSPRPRODUCT == antrag.kalkulation.angAntKalkDto.sysprproduct
                                             select PRPRODUCT;
                        prProduct = queryPrProduct.FirstOrDefault();
                    }
                }
            }
            using (DdOlExtended ctx = new DdOlExtended())
            {
                if (antrag.kalkulation != null)
                {
                    if (antrag.kalkulation.angAntKalkDto != null)
                    {
                        
                        if (prProduct != null)
                        {
                            antrag.kalkulation.angAntKalkDto.prProductBezeichnung = prProduct.NAME;
                        }

                        // Nutzungsart (privat, geschäftlich, demo) --------------------------------------------------
                        var queryObUseTyp = from OBUSETYPE in ctx.OBUSETYPE
                                            where OBUSETYPE.SYSOBUSETYPE == antrag.kalkulation.angAntKalkDto.sysobusetype
                                            select OBUSETYPE;
                        OBUSETYPE objektUseTyp = queryObUseTyp.FirstOrDefault();
                        if (objektUseTyp != null)
                        {
                            antrag.kalkulation.angAntKalkDto.obUseTypeBezeichnung = objektUseTyp.NAME;
                        }

                        // Waehrung --------------------------------------------------
                        var queryWaehrung = from WAEHRUNG in ctx.WAEHRUNG
                                            where WAEHRUNG.SYSWAEHRUNG == antrag.kalkulation.angAntKalkDto.syswaehrung
                                            select WAEHRUNG;
                        WAEHRUNG waehrung = queryWaehrung.FirstOrDefault();
                        if (waehrung != null)
                        {
                            antrag.kalkulation.angAntKalkDto.waehrungBezeichnung = waehrung.BEZEICHNUNG;
                        }
                    }

                    if (antrag.kalkulation.angAntVsDto != null)
                    {
                        OpenOne.Common.BO.Versicherung.IInsuranceBo insbo = Cic.OpenOne.GateBANKNOW.Common.BO.BOFactory.getInstance().createInsuranceBO();
                        // VersicherungsTyp --------------------------------------------------
                        foreach (var angAntVs in antrag.kalkulation.angAntVsDto)
                        {
                            var queryVSTyp = from VSTYP in ctx.VSTYP
                                             where VSTYP.SYSVSTYP == angAntVs.sysvstyp
                                             select VSTYP;
                            VSTYP vsTyp = queryVSTyp.FirstOrDefault();
                            if (vsTyp != null)
                            {
                                angAntVs.vsTypBezeichnung = vsTyp.BEZEICHNUNG;


                                //Ticket#2011112410000065
                                insbo.updateServiceType(angAntVs);
                            }

                            // Versicherer --------------------------------------------------
                            angAntVs.vsBezeichnung = MyGetPersonBezeichnung(angAntVs.sysvs);
                        }
                    }

                    if (antrag.kalkulation.angAntProvDto != null)
                    {
                        // Provisionsempfaenger --------------------------------------------------
                        foreach (var angAntProv in antrag.kalkulation.angAntProvDto)
                        {
                            angAntProv.partnerBezeichnung = MyGetPersonBezeichnung(angAntProv.syspartner);

                            // Provisionstyp --------------------------------------------------
                            List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysprprovtype", Value = angAntProv.sysprprovtype });

                            angAntProv.prProvTypeBezeichnung = ctx.ExecuteStoreQuery<String>(NAMEFROMPROVTYPEID, parameters.ToArray()).FirstOrDefault();
                        }
                    }

                    if (antrag.kalkulation.angAntAblDto != null)
                    {
                        foreach (var angAntAbl in antrag.kalkulation.angAntAblDto)
                        {
                            // Ablösetyp (Eigen, Fremd)  --------------------------------------------------
                            var queryAblTyp = from ABLTYP in ctx.ABLTYP
                                              where ABLTYP.SYSABLTYP == angAntAbl.sysabltyp
                                              select ABLTYP;
                            ABLTYP ablTyp = queryAblTyp.FirstOrDefault();
                            if (ablTyp != null)
                            {
                                angAntAbl.ablTypBezeichnung = ablTyp.DESCRIPTION;
                            }

                            // Abzulösender Vertrag im Eigenbestand  --------------------------------------------------
                            var queryVT = from VT in ctx.VT
                                          where VT.SYSID == angAntAbl.sysvorvt
                                          select VT;
                            VT vertrag = queryVT.FirstOrDefault();
                            if (vertrag != null)
                            {
                                angAntAbl.vorVtBezeichnung = vertrag.VERTRAG;
                            }
                        }
                    }

                    if (antrag.kalkulation.angAntSubvDto != null)
                    {
                        foreach (var angAntSubv in antrag.kalkulation.angAntSubvDto)
                        {
                            // Subventionstyp  --------------------------------------------------
                            var querySubvTyp = from SUBVTYP in ctx.SUBVTYP
                                               where SUBVTYP.SYSSUBVTYP == angAntSubv.syssubvtyp
                                               select SUBVTYP;
                            SUBVTYP subvTyp = querySubvTyp.FirstOrDefault();
                            if (subvTyp != null)
                            {
                                angAntSubv.subvTypBezeichnung = subvTyp.BEZEICHNUNG;
                            }

                            // Subventionsgeber (Händler bei Differenzleasing)  --------------------------------------------------
                            angAntSubv.subvGBezeichnung = MyGetPersonBezeichnung(angAntSubv.syssubvg);
                        }
                    }
                }
            }
            return antrag;
        }

        /// <summary>
        /// getAntragBezeichnungen für Objekt
        /// </summary>
        /// <param name="antrag"></param>
        /// <returns></returns>
        private AntragDto MyGetAntragBezeichnungenObjekt(AntragDto antrag)
        {
            using (DdOlExtended ctx = new DdOlExtended())
            {
                // Objekt --------------------------------------------------
                if (antrag.angAntObDto != null)
                {
                    var queryOB = from OB in ctx.OB
                                  where OB.SYSOB == antrag.angAntObDto.sysob
                                  select OB;
                    OB objekt = queryOB.FirstOrDefault();
                    if (objekt != null)
                    {
                        antrag.angAntObDto.obBezeichnung = objekt.BEZEICHNUNG;
                    }

                    // Objekt-Art --------------------------------------------------
                    var queryObArt = from OBART in ctx.OBART
                                     where OBART.SYSOBART == antrag.angAntObDto.sysobart
                                     select OBART;
                    OBART objektArt = queryObArt.FirstOrDefault();
                    if (objektArt != null)
                    {
                        antrag.angAntObDto.obArtBezeichnung = objektArt.DESCRIPTION;
                    }

                    // Objekt-Type --------------------------------------------------
                    var queryObTyp = from OBTYP in ctx.OBTYP
                                     where OBTYP.SYSOBTYP == antrag.angAntObDto.sysobtyp
                                     select OBTYP;
                    OBTYP objektTyp = queryObTyp.FirstOrDefault();
                    if (objektTyp != null)
                    {
                        antrag.angAntObDto.obTypBezeichnung = objektTyp.BEZEICHNUNG;
                    }
                }
            }
            return antrag;
        }

        /// <summary>
        /// getSysVglgd
        /// </summary>
        /// <param name="sysid">sysid</param>
        /// <returns></returns>
        public long? getSysVglgd(long sysid)
        {

            using (DdOlExtended ctx = new DdOlExtended())
            {


                // Objekt-Type --------------------------------------------------

                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p1", Value = sysid });

                long? sysvg = ctx.ExecuteStoreQuery<long?>(QUERYSYSVG, parameters.ToArray()).FirstOrDefault();


                return sysvg;

            }


        }

        /// <summary>
        /// getV_cluster
        /// </summary>
        /// <param name="sysid">sysid</param>
        /// <returns></returns>
        public VClusterDto getV_cluster(long sysid)
        {
            using (DdOlExtended context = new DdOlExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysid", Value = sysid });
                VClusterDto v_cluster = new VClusterDto();
                v_cluster.v_el_betrag = context.ExecuteStoreQuery<double>(QUERYCLUSTERBETRAG, parameters.ToArray()).FirstOrDefault();
                parameters.Clear();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysid", Value = sysid });
                v_cluster.v_el_prozent = context.ExecuteStoreQuery<double>(QUERYCLUSTERPROZENT, parameters.ToArray()).FirstOrDefault();
                parameters.Clear();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysid", Value = sysid });
                v_cluster.v_prof = context.ExecuteStoreQuery<double>(QUERYCLUSTERPROF, parameters.ToArray()).FirstOrDefault();
                if (v_cluster == null) return null;
                return v_cluster;
            }

        }

        public void saveClusterInAntrag(long sysid, VClusterDto vClusterDto)
        {
            try
            {
                using (DdOlExtended context = new DdOlExtended())
                {
                    ANTRAG antrag = (from ant in context.ANTRAG
                                     where ant.SYSID == sysid
                                     select ant).FirstOrDefault();
                    if (antrag != null && vClusterDto != null)
                    {
                        antrag.EWBBETRAG = (decimal?)vClusterDto.v_el_betrag;
                        antrag.EWBPROC = (decimal?)vClusterDto.v_el_prozent;
                        antrag.EWBPROF = (decimal?)vClusterDto.v_prof;
                        context.SaveChanges();

                    }

                }
            }
            catch (Exception )
            {
                _log.Error("Cannot save VClusterDto in Antrag "+sysid);


            }
        }

        /// <summary>
        /// getFform
        /// </summary>
        /// <param name="sysid">ysid</param>
        /// <returns></returns>
        public String getFform(long sysid)
        {
            using (DdOlExtended context = new DdOlExtended())
            {

                String fform = "";

                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysid", Value = sysid });
                fform = context.ExecuteStoreQuery<String>(QUERYFFORM, parameters.ToArray()).FirstOrDefault();
                return fform;

            }
        }

        /// <summary>
        /// getStraccount
        /// </summary>
        /// <param name="sysvm">sysvm</param>
        /// <returns></returns>
        public int getStraccount(long sysvm)
        {
            using (DdOlExtended context = new DdOlExtended())
            {

                int straccount = 0;

                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = ":p1", Value = sysvm });
                straccount = context.ExecuteStoreQuery<int>(QUERYSTRACCOUNT, parameters.ToArray()).FirstOrDefault();
                return straccount;

            }
        }

        /// <summary>
        /// getFlagBwgarantie
        /// </summary>
        /// <param name="sysid">sysid</param>
        /// <returns></returns>
        public int getFlagBwgarantie(long sysid)
        {
            using (DdOlExtended context = new DdOlExtended())
            {

                int bwg = 0;

                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysid", Value = sysid });
                bwg = context.ExecuteStoreQuery<int>(QUERYBWG, parameters.ToArray()).FirstOrDefault();
                return bwg;

            }
        }

        /// <summary>
        /// getScorebezeichnung
        /// </summary>
        /// <param name="sysid">sysid</param>
        /// <returns></returns>
        public String getScorebezeichnung(long sysid)
        {
            using (DdOlExtended context = new DdOlExtended())
            {

                String bezeichnung = "";

                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysid", Value = sysid });
                bezeichnung = context.ExecuteStoreQuery<String>(QUERYSCOREBEZEICHNUNG, parameters.ToArray()).FirstOrDefault();
                return bezeichnung;

            }
        }

        /// <summary>
        /// getScoreb aus DEDETAIL
        /// </summary>
        /// <param name="sysid">sysid</param>
        /// <returns></returns>
        public int getScoreInDedetailBySysantrag(long sysid)
        {
            int scoretotal = 0;
            using (DdOlExtended context = new DdOlExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "psysid", Value = sysid });
                scoretotal = context.ExecuteStoreQuery<int>(QUERYSCOREINDEDETAIL, parameters.ToArray()).FirstOrDefault();
                return scoretotal;
            }


        }

        /// <summary>
        /// Vertrag via ID auslesen
        /// </summary>
        /// <param name="sysid">Primary key</param>
        /// <param name="sysperole">sysperole</param>
        /// <returns>Daten</returns>
        public VertragDto getVertrag(long sysid, long sysperole)
        {
            using (DdOlExtended context = new DdOlExtended())
            {
                String extQuery = new EaiparDao().getEaiParFileByCode("RW_VERL_VERFUEGBAR_WEB", RW_VERL_VERFUEGBAR_WEB_DEFAULT);
                String buchwertQuery = new EaiparDao().getEaiParFileByCode(BuchwertBo.BW_PAR_FILE_CODE, BuchwertBo.BW_CALC_ALLOWED);
                String VTDETAILQUERY = "select (" + extQuery + @") isExtendible,(" + buchwertQuery + @") isBuchwertCalculationAllowed,  VT.rw, VT.ende vtende, VT.attribut zustandExtern, vt.sysantrag sysid, vt.sysid sysvt, vt.*, antobbrief.fident chassisnummer,antobbrief.stammnummer stammnummer, antobbrief.ecodestatus, antob.objektvt model,antob.fabrikat marke,antob.kennzeichen kontrollschild, antob.kennzeichen, antob.bezeichnung,vart.bezeichnung as vertragsartbezeichnung,vt.endekz vtendekz, vt.zustand vtzustand, vt.sysrwga vtsysrwga, VT.vertrag vtvertrag,  VTRUEKMAP.ZUSTAND vtruekZustand, WFZUSTMAP.SYSCODE wfzustSyscode from
                    VT,ANTRAG,Person, antob, antobbrief,vart, (select sysvt, zustand  from vtruek where sysvtruek in (select Max(sysvtruek) from vtruek group by sysvt)) vtruekMap,(select wftzust.syslease, LISTAGG(wfzust.syscode,',') within Group (order by syscode) as syscode  from wfzust, wftzust where wftzust.syswfzust = wfzust.syswfzust  group by syslease) wfzustMap 
                    where antrag.syskd=person.sysperson(+) and antrag.sysid=antob.sysantrag(+) and antob.sysob=antobbrief.SYSANTOBBRIEF(+) and VART.sysvart = antrag.sysvart and vt.sysantrag=antrag.sysid and vt.sysid = VTRUEKMAP.sysvt(+) and vt.sysid= wfzustMap.syslease(+) and vt.sysid="+sysid;


                DbConnection con = (context.Database.Connection);
                VertragDto rval = con.Query<VertragDto>(VTDETAILQUERY, null).FirstOrDefault();
                //definitive
                String buchwertDataQuery = @"Select Abloese activeOfferDate, Forderung activeOfferValue, sysvtruek activeOfferSysVtruek, syseaihfile activeOfferSysEaihfile  from Vtruek,psrpos where 
                                            sysvt = :sysvt and 
                                            Zustand = 'ERSTELLT' and vtruek.flagofferte=1 and vtruek.frist >= sysdate and vtruek.sysvtruek = Psrpos.Syslease (+) and psrpos.area (+)= 'VTRUEK' order by  sysvtruek desc";
                rval.buchwert = con.Query <BuchwertInfoDto>(buchwertDataQuery, new { sysvt=sysid}).FirstOrDefault();

                String vsextQuery = new EaiparDao().getEaiParFileByCode("EXT_VS_VERFUEGBAR_VT", "select 0 from dual");
                vsextQuery = vsextQuery.Replace(":sysperole", "" + sysperole);
                
                rval.extvscode = con.Query<String>("select ( " + vsextQuery + ") from VT where VT.sysid=:sysid", new { sysid }).FirstOrDefault();

                String vtrQuery = @"select outputparameter5 from eaihot where code = 'SOAP_HOLEBUCHWERTE_NFE' and oltable = 'VT' and sysoltable = :sysvt order by syseaihot desc";
                String sysvtr = con.Query<String>(vtrQuery, new { sysvt = sysid }).FirstOrDefault();
                try
                {
                    if (sysvtr != null && sysvtr.Length > 0)
                        rval.currentSysVtruek = long.Parse(sysvtr.Trim());
                }catch(Exception re)
                {
                    _log.Warn("Fetching currentSysVtruek not possible for sysvt=" + sysid + ": " + re.Message);
                }
                return rval;
            }
        }

        /// <summary>
        /// getAntragBezeichnungen für Kunde
        /// </summary>
        /// <param name="antrag"></param>
        /// <returns></returns>
        private AntragDto MyGetAntragBezeichnungenKunde(AntragDto antrag)
        {
            using (DdOlExtended ctx = new DdOlExtended())
            {
                // KundeTyp  --------------------------------------------------
                if (antrag.kunde != null)
                {
                    var queryKdTyp = from KDTYP in ctx.KDTYP
                                     where KDTYP.SYSKDTYP == antrag.kunde.syskdtyp
                                     select KDTYP;
                    KDTYP kdTyp = queryKdTyp.FirstOrDefault();
                    if (kdTyp != null)
                    {
                        antrag.kunde.kdtypBezeichnung = kdTyp.NAME;
                    }

                    antrag.kunde.langBezeichnung = MyGetLanguageBezeichnung(antrag.kunde.sysctlang);
                    antrag.kunde.langKorrBezeichnung = MyGetLanguageBezeichnung(antrag.kunde.sysctlangkorr);
                    antrag.kunde.landBezeichnung = MyGetLandBezeichnung(antrag.kunde.sysland);
                    antrag.kunde.land2Bezeichnung = MyGetLandBezeichnung(antrag.kunde.sysland2);
                    antrag.kunde.landNatBezeichnung = MyGetLandBezeichnung(antrag.kunde.syslandnat);
                    antrag.kunde.staatBezeichnung = MyGetStaatBezeichnung(antrag.kunde.sysstaat);
                    antrag.kunde.staat2Bezeichnung = MyGetStaatBezeichnung(antrag.kunde.sysstaat2);

                    // Branch --------------------------------------------------
                    var queryBranche = from BRANCHE in ctx.BRANCHE
                                       where BRANCHE.SYSBRANCHE == antrag.kunde.sysbranche
                                       select BRANCHE;
                    BRANCHE branche = queryBranche.FirstOrDefault();
                    if (branche != null)
                    {
                        antrag.kunde.brancheBezeichnung = branche.BEZEICHNUNG;
                    }

                    if (antrag.kunde.kontos != null)
                    {
                        // Kontos --------------------------------------------------
                        foreach (var konto in antrag.kunde.kontos)
                        {
                            var query = from KONTO in ctx.KONTO
                                        where KONTO.SYSKONTO == konto.syskonto
                                        select KONTO;
                            KONTO kontoDB = query.FirstOrDefault();
                            if (kontoDB != null)
                            {
                                konto.kontoBezeichnung = kontoDB.BEZEICHNUNG;
                            }
                        }
                    }

                    // Adressen --------------------------------------------------
                    if (antrag.kunde.adressen != null)
                    {
                        foreach (var kundenAdresse in antrag.kunde.adressen)
                        {
                            kundenAdresse.landBezeichnung = MyGetLandBezeichnung(kundenAdresse.sysland);
                            kundenAdresse.staatBezeichnung = MyGetStaatBezeichnung(kundenAdresse.sysstaat);
                            kundenAdresse.langBezeichnung = MyGetLanguageBezeichnung(kundenAdresse.sysctlang);
                        }
                    }
                }
            }
            return antrag;
        }

        private String MyGetPersonBezeichnung(long? sysId)
        {
            String bezeichnung = String.Empty;
            if (!sysId.HasValue)
                return bezeichnung;
            using (DdOlExtended ctx = new DdOlExtended())
            {
                // get description 
                var query = from PERSON in ctx.PERSON
                            where PERSON.SYSPERSON == sysId.Value
                            select PERSON;
                PERSON person = query.FirstOrDefault();
                if (person != null)
                {
                    bezeichnung = person.NAME;
                }
            }
            return bezeichnung;
        }

		/// <summary>
		/// Returns Vorname und Name from SYSWFUSER (WfUser)
		/// </summary>
		/// <param name="sysWfUser"></param>
		/// <returns></returns>
		public String getWfUserBezeichnung (long? sysWfUser)
		{
			String vorname_name = String.Empty;
            if (!sysWfUser.HasValue)
                return vorname_name;
			using (DdOlExtended ctx = new DdOlExtended ())
			{
				vorname_name = ctx.ExecuteStoreQuery<String> ("SELECT VORNAME||' '||NAME  FROM WFUSER WHERE WFUSER.SYSWFUSER = " + sysWfUser.Value).FirstOrDefault ();
			}
			return vorname_name;
		}



        private String MyGetStaatBezeichnung(long sysId)
        {
            String bezeichnung = String.Empty;
            using (DdOlExtended ctx = new DdOlExtended())
            {
                // get description 
                var query = from STAAT in ctx.STAAT
                            where STAAT.SYSSTAAT == sysId
                            select STAAT;
                STAAT staat = query.FirstOrDefault();
                if (staat != null)
                {
                    bezeichnung = staat.STAAT1;
                }
            }
            return bezeichnung;
        }

        private String MyGetLandBezeichnung(long sysId)
        {
            String bezeichnung = String.Empty;
            using (DdOlExtended ctx = new DdOlExtended())
            {
                // get description 
                var query = from LAND in ctx.LAND
                            where LAND.SYSLAND == sysId
                            select LAND;
                LAND land = query.FirstOrDefault();
                if (land != null)
                {
                    bezeichnung = land.COUNTRYNAME;
                }
            }
            return bezeichnung;
        }

        private String MyGetLanguageBezeichnung(long sysId)
        {
            String bezeichnung = String.Empty;
            using (DdOwExtended ctx = new DdOwExtended())
            {
                // get description 
                var query = from CTLANG in ctx.CTLANG
                            where CTLANG.SYSCTLANG == sysId
                            select CTLANG;
                CTLANG Lang = query.FirstOrDefault();
                if (Lang != null)
                {
                    bezeichnung = Lang.LANGUAGENAME;
                }
            }
            return bezeichnung;
        }

        public void SetAntragIdInZusatzdaten(DdOlExtended context, KundeDto kundeDto, long sysAntrag)
        {
            if (kundeDto != null && kundeDto.zusatzdaten != null)
            {
                foreach (var additionalData in kundeDto.zusatzdaten)
                {
                    if (additionalData.pkz != null)
                        foreach (var itpkzdto in additionalData.pkz)
                        {
                            ITPKZ itpkz = (from pkz in context.ITPKZ
                                           where pkz.SYSITPKZ == itpkzdto.syspkz
                                           select pkz).FirstOrDefault();
                            if (itpkz != null) { itpkz.SYSANTRAG = sysAntrag; }
                        }
                    if (additionalData.ukz != null)
                        foreach (var itukzdto in additionalData.ukz)
                        {
                            ITUKZ itukz = (from ukz in context.ITUKZ
                                           where ukz.SYSITUKZ == itukzdto.sysukz
                                           select ukz).FirstOrDefault();
                            if (itukz != null) { itukz.SYSANTRAG = sysAntrag; }
                        }
                }
            }
        }

        private long MyGetHaendlerAbwicklungsort(long? sysVM)
        {
            using (DdOlExtended context = new DdOlExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysVM", Value = sysVM });

                return context.ExecuteStoreQuery<long>(HAENDLERABWICKLUNGSORT, parameters.ToArray()).FirstOrDefault();
            }
        }
        private long MyGetHaendlerAbwicklungsortOne(long sysangebot)
        {
            using (DdOlExtended context = new DdOlExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = sysangebot });

                return context.ExecuteStoreQuery<long>(HAENDLERABWICKLUNGSORTANGEBOT, parameters.ToArray()).FirstOrDefault();
            }
        }

        /// <summary>
        /// Fetches the Abwicklungsort for an b2coffer
        /// </summary>
        /// <param name="ang"></param>
		/// <param name="eaiHotDao"></param>
        /// <returns></returns>
        public static long getAbwicklungsortB2C(AngebotDto ang, IEaihotDao eaiHotDao)
        {
            if (ang == null || ang.kunde == null)
            {
                _log.Error("Cannot call " + EAI_ABWICKLUNG + " - kunde is null");
                return 0;
            }

            using (DdOwExtended context = new DdOwExtended())
            {
                EaihotDto eaihot = new EaihotDto();

                // Set the table
                eaihot.OLTABLE = AreaConstants.Angebot.ToString().ToUpper(); ;

                // Set the area id
                eaihot.SYSOLTABLE = ang.sysid;

                // Set the event engine enabled
                eaihot.EVE = 1;
                eaihot.SUBMITDATE = DateTimeHelper.DateTimeToClarionDate(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                eaihot.SUBMITTIME = DateTimeHelper.DateTimeToClarionTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));

                // Set the document name, Angebotsnummer, Kundenname, Fahrzeug, Finanzierungsprodukt and Rate
                //      eaihot.INPUTPARAMETER1 = DateTimeHelper.DateTimeToClarionDate(inputBw.perDatum).ToString();
                eaihot.PROZESSSTATUS = (int)EaiHotStatusConstants.Pending;
                eaihot.HOSTCOMPUTER = "*";

                eaihot.CODE = EAI_ABWICKLUNG;

                eaihot.SYSEAIART = (from a in context.EAIART
                                    where a.CODE.Equals(EAI_ABWICKLUNG)
                                    select a.SYSEAIART).FirstOrDefault();


                eaihot.INPUTPARAMETER1 = ang.kunde.auslausweisCode;
                eaihot.INPUTPARAMETER2 = "" + ang.kunde.syslandnat;
                eaihot.INPUTPARAMETER3 = "" + ang.kunde.sysctlangkorr;
                eaihot.INPUTPARAMETER4 = ang.kunde.plz;
                eaihot.INPUTPARAMETER5 = "" + ang.kunde.sysland;

                EaihotDto eaihotOutput = eaiHotDao.createEaihot(eaihot);

                DateTime oldDate = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null);
                TimeSpan timeOut = new TimeSpan(0, 0, 0, 40);
                while (eaihotOutput.PROZESSSTATUS != (int)EaiHotStatusConstants.Ready && !isTimeOut(oldDate, timeOut))
                {
                    eaihotOutput = eaiHotDao.getEaihot(eaihotOutput.SYSEAIHOT);
                    System.Threading.Thread.Sleep(500);

                }
                if (eaihotOutput.PROZESSSTATUS == (int)EaiHotStatusConstants.Ready)
                {

                    if (eaihotOutput.OUTPUTPARAMETER1 != null)
                    {
                        return long.Parse(eaihotOutput.OUTPUTPARAMETER1);
                    }
                    _log.Error("Cannot call " + EAI_ABWICKLUNG + " - no result for Angebot " + ang.sysid);

                }
                else
                {
                    _log.Error("Cannot call " + EAI_ABWICKLUNG + " - timed out  for Angebot " + ang.sysid);
                }
                return 0;
            }
        }
        /// <summary>
        /// Prüfung auf Timeout
        /// </summary>
        /// <param name="oldDate">Alter Zeitstempel</param>
        /// <param name="timeOut">NeuerZeitstempel</param>
        /// <returns>Timeout true= Timeout/false = kein Timeout</returns>
        public static bool isTimeOut(DateTime oldDate, TimeSpan timeOut)
        {
            TimeSpan ts = DateTime.Now - oldDate;

            if (ts > timeOut) return true;

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysit"></param>
        /// <param name="syskd"></param>
        /// <param name="sysAntragNew"></param>
        public void createZusatzdaten4ExtendedContract(long sysit, long syskd, long sysAntragNew)
        {
            using (DdOlExtended context = new DdOlExtended())
            {
                //IT/ANGEBOT
                if (sysit > 0)
                {

                    IT it = (from i in context.IT
                             where i.SYSIT == sysit
                             select i).FirstOrDefault();


                    ITPKZ itpkz = (from ipkz in context.ITPKZ
                                   join antrag in context.ANTRAG on ipkz.SYSANTRAG equals antrag.SYSID
                                   where antrag.ATTRIBUT.Equals("Vertrag aktiviert") && ipkz.IT.SYSIT == sysit
                                   orderby antrag.ZUSTANDAM descending
                                   select ipkz).FirstOrDefault();


                    ITUKZ itukz = (from ukz in context.ITUKZ
                                   join antrag in context.ANTRAG on ukz.SYSANTRAG equals antrag.SYSID
                                   where antrag.ATTRIBUT.Equals("Vertrag aktiviert") && ukz.IT.SYSIT == sysit
                                   orderby antrag.ZUSTANDAM descending
                                   select ukz).FirstOrDefault();

                    if (itpkz != null)//found one in db, copy
                    {
                        ITPKZ itPkzRecordNew = new ITPKZ();
                        Mapper.Map<ITPKZ, ITPKZ>(itpkz, itPkzRecordNew);
                        itPkzRecordNew.SYSANTRAG = sysAntragNew;
                        itPkzRecordNew.IT = it;
                        //BNRSZ-210  Generierte BA wird bei einer RW-Verlängerung vom Vorantrag übernommen
                        itPkzRecordNew.CREBANUMMER = null;
                        itPkzRecordNew.CREBASYSAUSKUNFT = null;
                        itPkzRecordNew.CREEWKNUMMER = null;
                        itPkzRecordNew.CREEWKSYSAUSKUNFT = null;
                        
                        context.ITPKZ.Add(itPkzRecordNew);
                        context.SaveChanges();

                    }

                    if (itukz != null)
                    {
                        ITUKZ itUkzRecordNew = new ITUKZ();
                        Mapper.Map<ITUKZ, ITUKZ>(itukz, itUkzRecordNew);
                        itUkzRecordNew.SYSANTRAG = sysAntragNew;
                        itUkzRecordNew.IT = it;

                        //BNRSZ-210  Generierte BA wird bei einer RW-Verlängerung vom Vorantrag übernommen
                        itUkzRecordNew.CREBANUMMER = null;
                        itUkzRecordNew.CREBASYSAUSKUNFT = null;
                        itUkzRecordNew.CREEWKNUMMER = null;
                        itUkzRecordNew.CREEWKSYSAUSKUNFT = null;


                        long ukzsysit = itukz.SYSIT.GetValueOrDefault();
                        context.ITUKZ.Add(itUkzRecordNew);
                        context.SaveChanges();

                    }


                }

                if (syskd >0)
                {
                    PERSON person = (from p in context.PERSON
                                     where p.SYSPERSON == syskd
                                     select p).FirstOrDefault();
                    //PKZ pkz = context.ExecuteStoreQuery<PKZ>("select * from pkz where sysperson=" + syskd + " order by syspkz desc", null).FirstOrDefault();
                    PKZ pkz = (from p in context.PKZ
                               join antrag in context.ANTRAG on p.SYSANTRAG equals antrag.SYSID
                               where antrag.ATTRIBUT.Equals("Vertrag aktiviert") && p.PERSON.SYSPERSON == syskd
                               orderby antrag.ZUSTANDAM descending
                               select p).FirstOrDefault();

                    //UKZ ukz = context.ExecuteStoreQuery<UKZ>("select * from ukz where sysperson=" + syskd + " order by sysukz desc", null).FirstOrDefault();

                    UKZ ukz = (from u in context.UKZ
                               join antrag in context.ANTRAG on u.SYSANTRAG equals antrag.SYSID
                               where antrag.ATTRIBUT.Equals("Vertrag aktiviert") && u.PERSON.SYSPERSON == syskd
                               orderby antrag.ZUSTANDAM descending
                               select u).FirstOrDefault();

                    if (pkz != null)
                    {

                        PKZ pkzRecordNew = new PKZ();
                        Mapper.Map<PKZ, PKZ>(pkz, pkzRecordNew);
                        pkzRecordNew.SYSANTRAG = sysAntragNew;
                        pkzRecordNew.PERSON = person;
                        //BNRSZ-210  Generierte BA wird bei einer RW-Verlängerung vom Vorantrag übernommen
                        pkzRecordNew.CREBANUMMER = null;
                        pkzRecordNew.CREBASYSAUSKUNFT = null;
                        pkzRecordNew.CREEWKNUMMER = null;
                        pkzRecordNew.CREEWKSYSAUSKUNFT = null;
                        pkzRecordNew.CREGANUMMER = null;
                        pkzRecordNew.CREGASYSAUSKUNFT = null;

                        context.PKZ.Add(pkzRecordNew);
                        context.SaveChanges();

                    }



                    if (ukz != null)
                    {

                        UKZ ukzRecordNew = new UKZ();
                        Mapper.Map<UKZ, UKZ>(ukz, ukzRecordNew);
                        ukzRecordNew.SYSANTRAG = sysAntragNew;
                        ukzRecordNew.PERSON = person;
                        //BNRSZ-210  Generierte BA wird bei einer RW-Verlängerung vom Vorantrag übernommen
                        ukzRecordNew.CREBANUMMER = null;
                        ukzRecordNew.CREBASYSAUSKUNFT = null;
                        ukzRecordNew.CREEWKNUMMER = null;
                        ukzRecordNew.CREEWKSYSAUSKUNFT = null;

                        context.UKZ.Add(ukzRecordNew);
                        context.SaveChanges();

                    }


                }
               
                
            }


        }



        /// <summary>
        /// MyCopyZusatzdatenAntragID
        /// </summary>
        /// <param name="context"></param>
        /// <param name="sysAntragOld"></param>
        /// <param name="sysAntragNew"></param>
        private void MyCopyZusatzdatenAntragID(DdOlExtended context, long sysAntragOld, long sysAntragNew)
        {
            List<PKZ> pkzList = (from pkz in context.PKZ
                                 where pkz.SYSANTRAG == sysAntragOld
                                 select pkz).ToList();
            if (pkzList != null)
            {
                foreach (var pkzRecord in pkzList)
                {
                    PKZ pkzRecordNew = new PKZ();
                    Mapper.Map<PKZ, PKZ>(pkzRecord, pkzRecordNew);
                    pkzRecordNew.SYSANTRAG = sysAntragNew;


                    if (pkzRecord.PERSON == null)
                        context.Entry(pkzRecord).Reference(f => f.PERSON).Load();
                    
                    if (pkzRecord.PERSON != null)
                    {
                        pkzRecordNew.SYSPERSON= pkzRecord.PERSON.SYSPERSON;
                    }
                    context.PKZ.Add(pkzRecordNew);
                }
            }

            List<ITPKZ> itpkzList = (from pkz in context.ITPKZ
                                     where pkz.SYSANTRAG == sysAntragOld
                                     select pkz).ToList();
            if (itpkzList != null)
            {
                foreach (var itpkzRecord in itpkzList)
                {
                    ITPKZ itPkzRecordNew = new ITPKZ();
                    Mapper.Map<ITPKZ, ITPKZ>(itpkzRecord, itPkzRecordNew);
                    itPkzRecordNew.SYSANTRAG = sysAntragNew;

                    long pkzsysit = itpkzRecord.SYSIT.GetValueOrDefault();
                    if (pkzsysit > 0)
                    {
                        itPkzRecordNew.SYSIT = pkzsysit;
                    }
                    context.ITPKZ.Add(itPkzRecordNew);
                }
            }

            List<UKZ> ukzList = (from ukz in context.UKZ
                                 where ukz.SYSANTRAG == sysAntragOld
                                 select ukz).ToList();
            if (ukzList != null)
            {
                foreach (var ukzRecord in ukzList)
                {
                    
                    if (ukzRecord.PERSON == null)
                        context.Entry(ukzRecord).Reference(f => f.PERSON).Load();

                    UKZ ukzRecordNew = new UKZ();
                    Mapper.Map<UKZ, UKZ>(ukzRecord, ukzRecordNew);
                    ukzRecordNew.SYSANTRAG = sysAntragNew;

                    long ukzsysperson = ukzRecord.SYSPERSON.GetValueOrDefault();
                    if (ukzsysperson > 0)
                    {
                        ukzRecordNew.SYSPERSON=ukzsysperson;
                    }
                    context.UKZ.Add(ukzRecordNew);
                }
            }

            List<ITUKZ> itukzList = (from ukz in context.ITUKZ
                                     where ukz.SYSANTRAG == sysAntragOld
                                     select ukz).ToList();
            if (itukzList != null)
            {
                foreach (var itukzRecord in itukzList)
                {
                    
                    if (itukzRecord.IT == null)
                        context.Entry(itukzRecord).Reference(f => f.IT).Load();

                    ITUKZ itUkzRecordNew = new ITUKZ();
                    Mapper.Map<ITUKZ, ITUKZ>(itukzRecord, itUkzRecordNew);
                    itUkzRecordNew.SYSANTRAG = sysAntragNew;

                    long ukzsysit = itukzRecord.SYSIT.GetValueOrDefault();
                    if (ukzsysit > 0)
                    {
                        itUkzRecordNew.SYSIT= ukzsysit;
                    }
                    context.ITUKZ.Add(itUkzRecordNew);
                }
            }

            context.SaveChanges();
        }

        /// <summary>
        /// MySetAngebotIdInZusatzdaten
        /// </summary>
        /// <param name="context"></param>
        /// <param name="kundeDto"></param>
        /// <param name="sysAngebot"></param>
        private void SetAngebotIdInZusatzdaten(DdOlExtended context, KundeDto kundeDto, long sysAngebot)
        {
            if (kundeDto != null && kundeDto.zusatzdaten != null)
            {
                foreach (var additionalData in kundeDto.zusatzdaten)
                {
                    if (additionalData.pkz != null)
                        foreach (var itpkzdto in additionalData.pkz)
                        {
                            ITPKZ itpkz = (from pkz in context.ITPKZ
                                           where pkz.SYSITPKZ == itpkzdto.syspkz
                                           select pkz).FirstOrDefault();
                            if (itpkz != null) { itpkz.SYSANGEBOT = sysAngebot; }
                        }
                    if (additionalData.ukz != null)
                        foreach (var itukzdto in additionalData.ukz)
                        {
                            ITUKZ itukz = (from ukz in context.ITUKZ
                                           where ukz.SYSITUKZ == itukzdto.sysukz
                                           select ukz).FirstOrDefault();
                            if (itukz != null) { itukz.SYSANGEBOT = sysAngebot; }
                        }
                }
            }
        }

        /// <summary>
        /// BNRSZ-1591 und #2012090310000071
        /// Vertriebsweg einfach aus dem Angebot übernehmen, falls Vertriebsweg schon vorhanden ist
        /// wenn der Vertriebsweg noch unbekannt ist aus sysprChannel und erfassungsclient neu zuweisen
        /// </summary>
        /// <param name="sysPrChannel"></param>
        /// <param name="erfassungsclient"></param>
        /// <param name="vertriebsweg"></param>
        /// <returns></returns>
        private String MyGetVertriebsWeg(long? sysPrChannel, int erfassungsclient, string vertriebsweg)
        {
            if (vertriebsweg != null && vertriebsweg.Length > 0)
                return vertriebsweg; //keep original Vertriebsweg!

            // Konstanten 
            const String DDLKPPOSCODE_GESCHAEFTSART = "GESCHAEFTSART";
            const String SYSPRCHANNEL_FF = "4";
            const String SYSPRCHANNEL_KREDIT = "2";
            const String SYSPRCHANNEL_KREDIT_B2C = "1";
            const String SYSPRCHANNEL_FF_B2C = "3";

            // Der VertriebsWeg wird aus der Tabelle DDLKPPOS anhand der sysPrChannel ermittelt.
            // Wenn sysPrChannel gleich 1 ist (Fahrzeugfinanzierung), entspricht das dem Datensatz mit DDLKPPOS.ID = 4 und DDLKPPOS.Code = "GESCHAEFTSART"
            // Wenn sysPrChannel gleich 2 ist (Kreditfinanzierung), entspricht das dem Datensatz mit DDLKPPOS.ID = 2 und DDLKPPOS.Code = "GESCHAEFTSART"
            // DDLKPPOS.ID ist vom Typ Varchar
            String ddlKpPosID = String.Empty;
            switch (sysPrChannel)
            {
                case 1: ddlKpPosID = SYSPRCHANNEL_FF; break;
                case 2: ddlKpPosID = SYSPRCHANNEL_KREDIT; break;
                default: ddlKpPosID = String.Empty; break;
            }
            if (erfassungsclient == ERFASSUNGSCLIENT_B2C)
            {
                switch (sysPrChannel)
                {
                    case 1: ddlKpPosID = SYSPRCHANNEL_FF_B2C; break;
                    case 2: ddlKpPosID = SYSPRCHANNEL_KREDIT_B2C; break;
                    default: ddlKpPosID = String.Empty; break;
                }
            }

            String vertriebsWeg = String.Empty;
            try
            {
                using (Cic.OpenOne.Common.Model.DdOd.DdOdExtended oDcontext = new Cic.OpenOne.Common.Model.DdOd.DdOdExtended())
                {
                    vertriebsWeg = (from ddlKpPos in oDcontext.DDLKPPOS
                                    where ddlKpPos.ID == ddlKpPosID && ddlKpPos.CODE == DDLKPPOSCODE_GESCHAEFTSART
                                    select ddlKpPos.VALUE).FirstOrDefault();
                }
            }
            catch (Exception)
            {
                // Ignore exception, return empty string or null
            }
            return vertriebsWeg;
        }

        private String MyGetPrProductCode(long sysprProduct)
        {
            long start = (long)(DateTime.Now.TimeOfDay.TotalMilliseconds);
            IPrismaDao pDao = PrismaDaoFactory.getInstance().getPrismaDao();
            _log.Debug("MyGetPrProductCode A: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));
            start = (long)(DateTime.Now.TimeOfDay.TotalMilliseconds);
            CIC.Database.PRISMA.EF6.Model.VART vart = pDao.getVertragsart(sysprProduct);
            _log.Debug("MyGetPrProductCode B: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));
            start = (long)(DateTime.Now.TimeOfDay.TotalMilliseconds);
            if (vart == null)
                throw new Exception("Product has no VART:" + sysprProduct);

            String rval= vart.CODE + (pDao.isDiffLeasing(sysprProduct) ? "_DIFF" : "");
            _log.Debug("MyGetPrProductCode C: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));
            return rval;
        }

        /// <summary>
        /// fixAHK
        /// </summary>
        /// <param name="angAntOb"></param>
        /// <param name="sysPrChannel"></param>
        private void fixAHK(AngAntObDto angAntOb, long? sysPrChannel)
        {
            if (sysPrChannel.HasValue && sysPrChannel.Value == 1)
            {
                double ust = Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getMwStDao().getGlobalUst(1, Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                IRounding round = RoundingFactory.createRounding();
                angAntOb.ahk = round.getNetValue(angAntOb.ahkBrutto, ust);
                angAntOb.ahk = round.RoundCHF(angAntOb.ahk);
                angAntOb.ahkUst = round.RoundCHF(angAntOb.ahkBrutto - angAntOb.ahk);
            }
            else
            {
                angAntOb.ahkUst = 0;
                angAntOb.ahk = angAntOb.ahkBrutto;
            }
        }

        /// <summary>
        /// Holt den MitantragstellerTyp
        /// </summary>
        /// <param name="mitantragstellerTyp"></param>
        /// <returns></returns>
        private int MyGetMATyp(int mitantragstellerTyp)
        {
            // Default Typ ist Solidarschuldner
            int maTyp = (int)MitantragstellerTyp.Solidarschuldner;

            // Partner(800)/Solidarschuldner(120)/Bürge(130)
            int[] maTyps = Enum.GetValues(typeof(MitantragstellerTyp)).Cast<int>().ToArray();
            if (maTyps.Contains(mitantragstellerTyp))
            {
                maTyp = mitantragstellerTyp;
            }
            return maTyp;
        }

        #endregion
    }
}