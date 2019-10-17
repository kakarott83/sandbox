
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.EntityClient;
using System.Data.Objects;
using System.Globalization;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

using AutoMapper;
using Cic.One.DTO;
using Cic.One.DTO.BN;
using Cic.One.Web.BO.Search;
using Cic.OpenOne.Common.BO.Search;
using Cic.OpenOne.Common.Model.DdOd;
using Cic.OpenOne.Common.Model.DdOw;
using Cic.OpenOne.Common.Util.Collection;
using Cic.OpenOne.Common.Util.Logging;
using Cic.OpenOne.Common.Util.Security;
using Devart.Data.Oracle;
using Cic.One.Web.BO;
using Cic.One.Workflow.BO;
using Dapper;
using Cic.OpenOne.Common.BO;
using System.ServiceModel;
using CIC.Bas.Framework.OpenLease.Subscriptions;
using Cic.OpenOne.Common.Util;
using Cic.OpenOne.Common.Util.Exceptions;
using Cic.OpenOne.Common.Util.Config;
using CIC.Database.OL.EF4.Model;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Model.DdCt;

namespace Cic.One.Web.DAO
{
    public class EntityDao : IEntityDao
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
       

        #region Queries
        private static String QUERY_SALES = @"select (select name from wfuser where syswfuser=ra.syscrtuser) createUser,(select name from wfuser where syswfuser=ra.syschguser) finishUser, ps.loadcondition pnlcmd,d.doctype,deeplnkcode deeplnk,ra.SYSCHGUSER , ra.SYSCHGDATE  ,ra.SYSCHGTIME    ,ra.SYSCRTUSER ,ra.SYSCRTDATE ,ra.SYSCRTTIME ,s.sysprunstep,ra.SYSRATINGAUFLAGE,d.name NAME,
                                ps.description pp,
                                decode(ra.sysperson,antrag.syskd,'1','2') OWNER,d.CONTYPE ART,s.flagok,s.flagnok flagnokstep,ra.fullfilled
                                from 
                                 dedefcon d
                                ,ratingauflage ra
                                ,rating rt
                                ,prunstep s
                                ,antrag
                                ,pstep ps,deeplnkrel
                                where rt.sysrating = ra.sysrating                                
                                and d.externcode=deeplnkrel.externcode(+)
                                and ra.sysdedefcon = d.sysdedefcon
                                and  ra.sysratingauflage=s.syscode(+)
                                and rt.sysid = antrag.sysid
                                and ra.ACTIVEFLAG = 1
                                and rt.flag1 = 0
                                and s.syspstep=ps.syspstep(+) 
                                and antrag.sysid =:sysid  order by decode(ra.sysperson,antrag.syskd,'1','2'),d.contype,d.name";
        private static String QUERY_PAYMENTS = @"select ps.art psart,ps.syspstep,(select name from wfuser where syswfuser=ra.syscrtuser) createUser,(select name from wfuser where syswfuser=ra.syschguser) finishUser, ps.loadcondition pnlcmd,d.doctype,deeplnkcode deeplnk,ra.SYSCHGUSER , ra.SYSCHGDATE  ,ra.SYSCHGTIME    ,ra.SYSCRTUSER ,ra.SYSCRTDATE ,ra.SYSCRTTIME ,s.sysprunstep,p.sysprunsteppos,ra.SYSRATINGAUFLAGE,d.name NAME,
                                ps.description pp,
                                perror.CODE,perror.DESCRIPTION,
                                decode(ra.sysperson,antrag.syskd,'1','2') OWNER,d.CONTYPE ART,s.flagok,s.flagnok flagnokstep, p.flagnok,ra.fullfilled
                                from 
                                 dedefcon d
                                ,ratingauflage ra
                                ,rating rt
                                ,prunstep s
                                ,prunsteppos p
                                ,antrag
                                ,perror,pstep ps,antoption ao,deeplnkrel
                                where rt.sysrating = ra.sysrating
                                and d.externcode=deeplnkrel.externcode(+)
                                and ao.sysid=antrag.sysid
                                and ra.sysdedefcon = d.sysdedefcon
                                and ra.sysratingauflage=s.syscode(+)
                                and  s.SYSPRUNSTEP= p.SYSPRUNSTEP(+) 
                                and rt.sysid = antrag.sysid
                                and ra.ACTIVEFLAG = 1
                                and rt.flag1 = 0
                                and p.PERRORCODE=perror.code(+)
                                and ps.syspstep (+) =s.syspstep                 
                                and antrag.sysid =:sysid order by decode(ra.sysperson,antrag.syskd,'1','2'),d.contype,d.name,perror.code";

        private static String QUERY_PERSONPKZACTIVE = @"select pkz.*,kredrate kredtrate,anzahlvollstr anzvollstr,pkz.syspkz syspkz from pkz where syspkz=(
                                        SELECT nvl(max(syspkz),0)
                                         FROM pkz,
                                         antrag
                                         WHERE pkz.sysantrag =antrag.sysid
                                         AND (antrag.zustand='Bonitätsprüfung'
                                         OR antrag.zustand = 'Risikoprüfung'
                                         OR antrag.zustand   = 'Nachbearbeitung'
                                         OR antrag.attribut  = 'Vertrag aktiviert')
                                         AND pkz.sysperson   =:sysperson)";
        private static String QUERY_PERSONPKZACTIVELAST = @"select pkz.*,kredrate kredtrate,anzahlvollstr anzvollstr,pkz.syspkz syspkz from pkz where syspkz=(
                                        SELECT nvl(max(syspkz),0)
                                         FROM pkz
                                         WHERE pkz.sysperson   =:sysperson)";
        private static String QUERY_PERSONPKZACTIVEANTRAG = @"select pkz.*,kredrate kredtrate,anzahlvollstr anzvollstr,pkz.syspkz syspkz from pkz where pkz.sysperson   =:sysperson and pkz.sysantrag=:sysantrag";

        private static String QUERY_PERSONUKZACTIVE = @"select ukz.*,obligoeigen obliboeigen,ukz.sysukz sysukz from ukz where sysukz=(
                        SELECT nvl(max(sysukz),0)
                         FROM ukz,
                         antrag
                         WHERE ukz.sysantrag =antrag.sysid
                         AND (antrag.zustand='Bonitätsprüfung'
                         OR antrag.zustand = 'Risikoprüfung'
                         OR antrag.zustand   = 'Nachbearbeitung'
                         OR antrag.attribut  = 'Vertrag aktiviert')
                         AND ukz.sysperson  =:sysperson)";

        private static String QUERY_PERSONUKZACTIVELAST = @"select ukz.*,obligoeigen obliboeigen,ukz.sysukz sysukz from ukz where sysukz=(
                        SELECT nvl(max(sysukz),0)
                         FROM ukz
                         WHERE ukz.sysperson  =:sysperson)";
        private static String QUERY_PERSONUKZACTIVEANTRAG = @"select ukz.*,obligoeigen obliboeigen,ukz.sysukz sysukz from ukz where sysperson=:sysperson and sysantrag=:sysantrag";


        private static String QUERYWFEXEC = "select * from wfexec where syswfexec=:syswfexec";
        private static String QUERYSLPOSTYP = "select * from slpostyp where sysslpostyp=:sysslpostyp";
        private static String QUERYSLTYP = "select * from sltyp where syssltyp=:syssltyp";
        private static String QUERYPEROLE = "select * from perole where sysperole=:sysperole";
        private static String QUERYROLETYPE = "select * from roletype where sysroletype=:sysroletype";
        private static String QUERYPRHGROUP = "select * from prhgroup where sysprhgroup=:sysprhgroup";
        private static String QUERYBCHANNEL = "select * from bchannel where sysbchannel=:sysbchannel";
        private static String QUERYBRAND = "select * from brand where sysbrand=:sysbrand";
        private static String QUERYRN = "select * from rn where sysid=:sysid";
        private static String QUERYANGOBBRIEF = "select * from angobbrief where sysangobbrief=:sysangobbrief";
        private static String QUERYSLPOS_ZAHLUNGSPLAN = "select * from slpos where sysslpos=:sysslpos";
        private static String QUERYOBBRIEF = "select * from obbrief where sysobbrief=:sysobbrief";
        private static String QUERYKALK = "select * from kalk where syskalk=:syskalk";
        private static String QUERYPERSON = "select * from person where sysperson=:sysperson";

        private static String QUERYEXPDISPS = @"select exptyp.sysexptyp,exprange.sysexprange,expval.sysexpval, expval.val, expval.crtdate, expval.crttime from exptyp, expval 
left outer join exprange on exprange.sysexprange in (SELECT DISTINCT
       FIRST_VALUE(sysexprange)  OVER (ORDER BY rang asc)
FROM   exprange where exprange.sysexptyp=expval.sysexptyp and exprange.minval<=expval.val and exprange.maxval>expval.val) and  exprange.sysexptyp=expval.sysexptyp 
where exptyp.sysexptyp=expval.sysexptyp and expval.area=:area and expval.sysid=:sysid";

        private static String QUERYEXPDISP = @"select exptyp.sysexptyp,exprange.sysexprange,expval.sysexpval, expval.val, expval.crtdate, expval.crttime from exptyp, expval 
left outer join exprange on exprange.sysexprange in (SELECT DISTINCT
       FIRST_VALUE(sysexprange)  OVER (ORDER BY rang asc)
FROM   exprange where exprange.sysexptyp=expval.sysexptyp and exprange.minval<=expval.val and exprange.maxval>expval.val) and  exprange.sysexptyp=expval.sysexptyp 
where exptyp.sysexptyp=expval.sysexptyp and expval.area=:area and expval.sysid=:sysid and exptyp.sysexptyp=:sysexptyp";

        private static String QUERYEXPDISPDEF = @"select exprange.output,expval.area,expval.sysid areaid,exptyp.sysexptyp,exprange.sysexprange,expval.sysexpval, expval.val, expval.crtdate, expval.crttime from exptyp, expval 
left outer join exprange on exprange.sysexprange in (SELECT DISTINCT
       FIRST_VALUE(sysexprange)  OVER (ORDER BY rang asc)
FROM   exprange where exprange.sysexptyp=expval.sysexptyp and exprange.minval<=expval.val and exprange.maxval>expval.val) and  exprange.sysexptyp=expval.sysexptyp 
where exptyp.sysexptyp=expval.sysexptyp and expval.area=:area and areadefaultflag=1 and expval.sysid in ";


        private static String QUERYEXPDEFCALCIDS = @"select case when (crtdate +daysvalid)<sysdate then 1 else 0 end as expired, expval.sysid,exptyp.sysexptyp,expression,sysexpval,archivflag from exptyp,expval
                where aktivkz=1 and exptyp.sysexptyp=expval.sysexptyp and method=1 and areadefaultflag=1 and exptyp.area=:area and expval.sysid in ";


        private static String QUERYEXPCALCIDS = @"select  exptyp.sysexptyp, expression,sysexpval,archivflag from exptyp, expval
where aktivkz=1 and exptyp.sysexptyp=expval.sysexptyp(+) and visibilityflag=1 and (crtdate +daysvalid)<sysdate  and method=1 and exptyp.area=:area and expval.sysid=:sysid";

        private static String QUERYEXPCALCID = @"select  exptyp.method, exptyp.sysexptyp, expression,sysexpval,archivflag from exptyp left outer join expval on  exptyp.sysexptyp=expval.sysexptyp and  expval.sysid=:sysid where exptyp.sysexptyp=:sysexptyp and exptyp.area=:area";


        private static String QUERYEXPTYPES = "select exptyp.* from exptyp where area=:area";
        private static String QUERYEXPTYPE = "select exptyp.* from exptyp where area=:area and areadefaultflag=:defaultflag";

        private static String QUERYEXPRANGES = "select exprange.* from exptyp, exprange where exptyp.sysexptyp=exprange.sysexptyp and area=:area";

        private static String QUERYRECALC = "select * from cic.recalc where sysrecalc=:sysrecalc";
        private static String QUERYRECALCRV = "select count(*) from vtobsich where Trim(bezeichnung) like '%RNV%' and sysvt = :sysvt";
        private static String QUERYRECALCSV = "select count(*) from vtobsl where sysvt = :sysvt and rang = 5000 and inaktiv<>1";
        private static String QUERYRECALCTIRE = "select count(*) from vtobsl where sysvt = :sysvt and rang in (5010, 5020, 5040) and inaktiv<>1";
        private static String QUERYRECALCANAB = "select count(*) from vtobsl where sysvt = :sysvt  and rang = 5060 and inaktiv<>1";
        private static String QUERYRECALCSO = "select count(*) from vtobsl where sysvt = :sysvt and rang = 5090 and inaktiv<>1";
        private static String QUERYRECALCFUEL = "select count(*) from vtobsl where sysvt = :sysvt and rang = 6000 and inaktiv<>1";
        private static String QUERYRECALCEW = "select count(*) from vtobsl where sysvt = :sysvt and rang = 5080 and inaktiv<>1";
        private static String QUERYRECALCDEPOT = "select depot from kalk where syskalk = (select sysob from ob where sysvt = :sysvt)";

        private static String QUERYRECALCZINS = "select vtobsl.variabel from vtobsl where vtobsl.sysvt = :sysvt and vtobsl.syssltyp in (select syssltyp from sltyp where renditeflag = 1) and vtobsl.inaktiv=0";
        private static String QUERYRECALCREIFEN = "select vtobsl.variabel from vtobsl where vtobsl.sysvt = :sysvt and vtobsl.rang = 5010   and vtobsl.inaktiv=0";
        private static String QUERYRECALCSERVICE = "select vtobsl.variabel from vtobsl where vtobsl.sysvt = :sysvt and vtobsl.rang = 5000  and vtobsl.inaktiv=0";

        private static String QUERYRECALCSYSRVT = "select angebot.sysrvt from angebot where angebot.sysls = 3 and (select vpvertrag from vt where sysvt = 0 start with sysid = :sysvt connect by prior sysvt = sysid) like angebot||' %'  order by 1 desc";

        private static String QUERYMYCALC = "select * from cic.mycalc where sysmycalc=:sysmycalc";
        private static String QUERYMYCALCAUST = "select * from cic.mycalcaust where sysmycalc=:sysmycalc";
        private static String QUERYMYCALCFS = "select * from cic.mycalcfs where sysmycalcfs=:sysmycalcfs";
        private static String QUERYOBTYP = "SELECT a.sysobtyp ID5,  a.sysobtypp ID4,  a.bezeichnung BEZEICHNUNG,  a.aufbau AUFBAU,  a.getriebe GETRIEBE,  a.treibstoff TREIBSTOFF,  a.co2emi EMISSION,  a.baumonat BAUMONAT,  a.baujahr BAUJAHR,  a.typengenehmigung,  a.art,  a.schwacke,  a.neupreisbrutto,  a.neupreisnetto,  a.leistung,  a.baubismonat,  a.baubisjahr, a.bgn FROM  (SELECT 5,   o.bgn, o.sysobtyp,    o.sysobtypp,    o.bezeichnung bezeichnung,    CASE      WHEN ft.sysfztyp>0      THEN ft.aufbau      ELSE f.aufbau    END Aufbau,    CASE      WHEN ft.sysfztyp>0      THEN ft.gart      ELSE f.getriebe    END Getriebe,    CASE      WHEN ft.sysfztyp>0      THEN        CASE          WHEN ft.treibstoff = 0          THEN 'Diesel'          WHEN ft.treibstoff = 1          THEN 'Benzin'          ELSE 'Andere'        END      ELSE f.treibstoff    END Treibstoff,    CASE      WHEN ft.sysfztyp>0      THEN ft.co2emi      ELSE f.co2emi    END co2emi,    CASE      WHEN ft.sysfztyp>0      THEN TO_CHAR(ft.gebautvon, 'MM')      ELSE f.BauMonat    END BauMonat,    CASE      WHEN ft.sysfztyp>0      THEN TO_CHAR(ft.gebautvon, 'yyyy')      ELSE f.BauJahr    END BauJahr,    CASE      WHEN ft.sysfztyp>0      THEN ft.typ      ELSE f.Typengenehmigung    END Typengenehmigung,    o.art,    o.schwacke,    CASE      WHEN ft.sysfztyp>0      THEN ft.grund      ELSE f.neupreisbrutto    END Neupreisbrutto,    CASE      WHEN ft.sysfztyp>0      THEN ft.grund      ELSE f.neupreisnetto    END Neupreisnetto,    CASE      WHEN ft.sysfztyp>0     THEN ft.leistung      ELSE f.leistung    END Leistung,    CASE      WHEN ft.sysfztyp>0      THEN TO_CHAR(ft.gebautbis, 'MM')      ELSE TO_CHAR(f.gebautbis, 'MM')    END Baubismonat,    CASE      WHEN o.noextid = 1      THEN TO_CHAR(ft.gebautbis, 'yyyy')      ELSE TO_CHAR(f.gebautbis, 'yyyy')    END Baubisjahr  FROM cic.obtyp o,    cic.obtypfzadd f,    cic.fztyp ft  WHERE o.flagaktiv = 1  AND o.sysobtyp    = f.sysobtyp(+)  AND o.sysfztyp    = ft.sysfztyp(+)  AND o.sysobtyp    =:sysobtyp  ) a";

        private static String QUERYOBJEKT = "select TYPFZ,KENNZEICHEN,ERSTZUL,ANZAHLSITZE,ANZAHLTUEREN,BAUJAHR,BAUMONAT,NKK.AUSZAHLUNGP BELEIHUNGSWERT,BESTELLUNG,BRIEF,FABRIKAT,OBART.NAME FAHRZEUGART,FARBEA,NKK.KONTO FINANZIERUNG,HAENDLER.NAME HAENDLER,HERSTELLER,obkat.name KATEGORIE,LIEFERUNG,ORDERTYPE,RN.FBETRAG+RN.FSTEUER RECHNUNGSBETRAG,RN.GBETRAG+RN.GSTEUER RECHNUNGSBETRAGEURO,RN.VALUTADATUM RECHNUNGSDATUM,RN.FAELLIGDATUM RECHNUNGSFAELLIGKEIT,RN.RECHNUNG RECHNUNGSNUMMER,SCHWACKE,SPERREZAHLUNG,SPERREFREIGABE,STANDORT,STANDORTBRIEF,OB.SYSHD,NKK.SYSNKK,OB.SYSOB,OB.SYSOBART,OB.SYSOBKAT,OB.TYP,WAEHRUNG.CODE WAEHRUNG,OB.ZUSTAND from CIC.NKK NKK, CIC.OB OB, CIC.RN RN, CIC.WAEHRUNG WAEHRUNG, CIC.OBKAT OBKAT, CIC.OBART OBART, CIC.PERSON HAENDLER where NKK.SYSNKK=OB.SYSNKK and ob.sysobkat=obkat.sysobkat (+) and ob.sysobart=obart.sysobart (+) and HAENDLER.sysperson=OB.SYSHD (+) AND OB.SYSOB = RN.SYSOB (+) and RN.SYSFWAEHRUNG=WAEHRUNG.SYSWAEHRUNG (+) and OB.sysob=:sysob";
        //private static String QUERYBRIEF = "select SYSOBBRIEF,TREIBSTOFF,GETRIEBE,MOTOR,ZULGEW,REIFV,REIFMUH,FELGV,FELGMUH,KW,TANK,VERBRAUCHGESAMT,CO2EMI,NOX,DPF,FIDENT,MOTORNUMMER,IMPCODE from obbrief where sysobbrief=:sysob";
        private static String QUERYRAHMEN = "select *  from cic.rvt rvt where rvt.sysrvt=:sysrvt";
        private static String QUERYRAHMENPOS = "select rvtpos.*,rvtvs.sysvstyp from rvtpos,rvtvs where rvtpos.sysrvt=:sysrvt and rvtpos.sysrvtpos=rvtvs.sysrvtpos(+)";
        private static String QUERYHAENDLER = "select sysperson,matchcode,code,titel,name,vorname,strasse,anrede,anredecode,strasse,plz, ort,telefon, land.countryname land  from cic.person person, cic.land land where person.sysland=land.sysland and flaghd=1 and sysperson=:sysperson";
        private static String QUERYKUNDE = "select *  from cic.person person where sysperson=:sysperson";
        private static String QUERYLOGDUMP = "select *  from cic.logdump logdump where syslogdump=:syslogdump";
        private static String QUERYOPP = "select *  from cic.oppo opp where sysoppo=:sysoppo";
        private static String QUERYOPPOTASK = "select oppotask.*,case when crt.syswfuser is null then 'Automatisch generiert' else trim(crt.name)||' '||trim(crt.vorname) end crtuserName,(select iamtype.code from iamtype, oppo, iam where oppo.sysiam=iam.sysiam and iam.sysiamtype=iamtype.sysiamtype and oppo.sysoppo=oppotask.sysoppo) oppoiamcode  from cic.Oppotask Oppotask left outer join wfuser crt on crt.syswfuser=oppotask.syscrtuser where sysOppotask=:sysOppotask";
        private static String QUERYCONTACT = "select CONTACT.*, PERSON.NAME personName, PERSON.VORNAME personVorname from cic.CONTACT CONTACT, cic.PERSON PERSON where CONTACT.SYSPERSON = PERSON.SYSPERSON (+) AND syscontact=:syscontact";
        private static String QUERYKONTO = "select *  from cic.konto konto where syskonto=:syskonto";
        private static String QUERYNKONTO = "select * from cic.nkonto where sysnkonto=:sysnkonto";
        private static String QUERYITKONTO = "select *  from cic.itkonto itkonto where sysitkonto=:sysitkonto";
        private static String QUERYCAMP = "select *  from cic.camp camp where syscamp=:syscamp";
        private static String QUERYWFUSER = "select *  from cic.wfuser wfuser where syswfuser=:syswfuser";

        private static String QUERYADRESSE = "select * from cic.adresse adresse where sysadresse=:sysadresse";


        private static String QUERYFINANZIERUNG = "select klinie.bezeichnung kreditlinie,prproduct.name PRODUCTNAME,nkk.*,RVT.SYSPERSON,  PERSON.CODE PERSONCODE, PERSON.NAME PERSONNAME from cic.nkk nkk,cic.rvt,cic.person,cic.prproduct,cic.klinie where sysnkk=:sysnkk and NKK.SYSRVT=RVT.SYSRVT (+) and NKK.SYSklinie=klinie.sysklinie (+) AND RVT.SYSPERSON=PERSON.SYSPERSON (+) and NKK.sysprproduct=prproduct.sysprproduct (+)";

        private static String QUERYITADRESSE = "select * from cic.itadresse itadresse where sysitadresse=:sysitadresse";
        private static String QUERYPTASK = "select * from cic.ptask ptask where sysptask=:sysptask";
        private static String QUERYAPPTMT = "select * from cic.apptmt apptmt where sysapptmt=:sysapptmt";
        private static String QUERYREMINDER = "select r.*,(select param1 from bpcasestep where param3=r.sysreminder) syswfmmemo from cic.reminder r where r.sysreminder=:sysreminder";
        private static String QUERYMAILMSG = "select * from cic.mailmsg mailmsg where sysmailmsg=:sysmailmsg";
        private static String QUERYMEMO = @"  select P.NAME ||' '|| P.VORNAME EDITUSERNAME, P2.NAME ||' '|| P2.VORNAME CREATEUSERNAME, WFMMEMO.SYSWFMMEMO, SYSWFMMKAT, SYSWFMTABLE, SYSLEASE,SYSIDWFTA,KURZBESCHREIBUNG, WFMMEMO.EDITDATE,  WFMMEMO.EDITTIME EDITTIMECLA,  WFMMEMO.EDITUSER, WFMMEMO.CREATEDATE,  WFMMEMO.CREATETIME CREATETIMECLA,  WFMMEMO.CREATEUSER, STR01, STR02,DAT01, DAT02,
                                              CASE WHEN WFMMEMOEXT.INHALT is not null THEN WFMMEMOEXT.INHALT ELSE WFMMEMO.NOTIZMEMO END  NOTIZMEMO from 
                                              WFTABLE, WFMMEMO, CIC.wfuser P, CIC.wfuser P2, wfmmemoext where edituser=p.syswfuser (+) and createuser=p2.syswfuser (+) and WFMMEMO.syswfmtable = wftable.syswftable and wfmmemo.syswfmmemo=:syswfmmemo and wfmmemoext.syswfmmemo(+)= wfmmemo.syswfmmemo ";
        private static String QUERYPRUN = "select * from cic.prun prun where sysprun=:sysprun";
        private static String QUERYPRUNART = "select * from cic.prunart prunart where sysprunart=:sysprunart";
        private static String QUERYPRUNARTACTIVE = "select * from cic.prunart prunart where activeflag=1 and  (validuntil is null or validuntil>=sysdate  or validuntil=to_date('01.01.0111' , 'dd.MM.yyyy')) and (validfrom is null or validfrom<=sysdate  or validfrom=to_date('01.01.0111' , 'dd.MM.yyyy'))";
        private static String QUERYFILEATT = "select * from cic.fileatt fileatt where sysfileatt=:sysfileatt";
        private static String QUERYFILEATTENTITY = "select * from cic.fileatt fileatt where sysid=:sysid and UPPER(area)=:area and activeflag = 1 and typcode='icon' order by sysfileatt desc";

        private static String QUERYDMSDOC = "select * from cic.dmsdoc dmsdoc where sysdmsdoc=:sysdmsdoc";
        private static String QUERYDMSDOCENTITY = "select * from cic.dmsdoc dmsdoc, cic.dmsdocarea dmsdocarea where dmsdocarea.sysdmsdoc=dmsdoc.sysdmsdoc and dmsdocarea.sysid=:sysid and dmsdocarea.area=:area order by name asc";

        private static String QUERYPRPRODUCT = "select * from cic.prproduct prproduct where sysprproduct=:sysprproduct";
        private static String QUERYITEMCAT = "select * from cic.itemcat itemcat where sysitemcat=:sysitemcat";
        private static String QUERYCTLANG = "select * from cic.ctlang ctlang where sysctlang=:sysctlang";
        private static String QUERYLAND = "select * from cic.land land where sysland=:sysland";
        private static String QUERYBRANCHE = "select * from cic.branche branche where sysbranche=:sysbranche";
        private static String QUERYACCOUNT = "select case when perole.sysroletype=3 then (select nvl(max(1),0) HAT_ID_BERECHTIGUNG from roleattribm, roleattrib, perole vk where roleattribm.SYSROLEATTRIB = roleattrib.SYSROLEATTRIB and vk.sysperole=roleattribm.SYSPEROLE and vk.sysperson = sysperson and roleattrib.SYSROLEATTRIB=11) when perole.sysroletype=2 then (select nvl(max(1),0) HAT_ID_BERECHTIGUNG from roleattribm, roleattrib, perole vk, perole hd where roleattribm.SYSROLEATTRIB = roleattrib.SYSROLEATTRIB and vk.sysperole=roleattribm.SYSPEROLE and vk.sysparent = hd.sysperole and hd.sysperson = person.sysperson and roleattrib.SYSROLEATTRIB=11) else 0 end idberechtigung, (select max(inactiveflag) from perole where sysperson=person.sysperson) inactiveflag, person.locked,PEOPTION.OPTION16 fax2, PEOPTION.OPTION7 handy2, PEOPTION.OPTION3 email2, werbecode, werbecodegrund,mitarbeiterflag,ausschluss,infomailflag,infomail2flag,infosmsflag,sysrisikokl,sysreferenz,ptelefon,person.sysadmadd, person.sysperson, matchcode, code,    syskdtyp,  anrede, anredecode, titel, titelCode, titel2,trim(person.name) name, trim(vorname) vorname, gebdatum, sysctlang, sysctlangkorr, syslandnat, einreisedatum, auslausweis,auslausweiscode,auslausweisGueltig, trim(strasse) strasse, hsnr, plz, trim(ort) ort, sysland, sysstaat, wohnseit, strasse2, hsnr2, plz2, ort2, sysland2, sysstaat2, telefon, telefon2, handy, erreichbtel, erreichbvon, erreichbbis, email, url, sysbranche, rechtsform, rechtsformcode, gruendung, hregister, hregisterflag, revflag, zusatz, namekont, vornamekont,  fax, gebort, hregisterort, identeu, steuernr, identust, nomailingflag,privatflag, gesflag, mitarbeiterflag,crefoid  from cic.person person, cic.peoption peoption, cic.perole perole where person.sysperson=:sysaccount and person.sysperson=peoption.sysid(+) and person.sysperson=perole.sysperson(+)";
            //"select (select nvl(max(1),0) HAT_ID_BERECHTIGUNG from roleattribm, roleattrib, perole vk, perole hd where roleattribm.SYSROLEATTRIB = roleattrib.SYSROLEATTRIB and vk.sysperole=roleattribm.SYSPEROLE and vk.sysparent = hd.sysperole and hd.sysperson = person.sysperson and roleattrib.SYSROLEATTRIB=11) idberechtigung,(select max(inactiveflag) from perole where sysperson=person.sysperson) inactiveflag, locked,PEOPTION.OPTION16 fax2, PEOPTION.OPTION7 handy2, PEOPTION.OPTION3 email2, werbecode, werbecodegrund,mitarbeiterflag,ausschluss,infomailflag,infomail2flag,infosmsflag,sysrisikokl,sysreferenz,ptelefon,person.sysadmadd, person.sysperson, matchcode, code,    syskdtyp,  anrede, anredecode, titel, titelCode, titel2,trim(name) name, trim(vorname) vorname, gebdatum, sysctlang, sysctlangkorr, syslandnat, einreisedatum, auslausweis, trim(strasse) strasse, hsnr, plz, trim(ort) ort, sysland, sysstaat, wohnseit, strasse2, hsnr2, plz2, ort2, sysland2, sysstaat2, telefon, telefon2, handy, erreichbtel, erreichbvon, erreichbbis, email, url, sysbranche, rechtsform, rechtsformcode, gruendung, hregister, hregisterflag, revflag, zusatz, namekont, vornamekont,  fax, gebort, hregisterort, identeu, steuernr, identust, nomailingflag,privatflag, gesflag, mitarbeiterflag  from cic.person person, cic.peoption peoption where sysperson=:sysaccount and person.sysperson=peoption.sysid(+)";
        private static String QUERYKDKONT = "select adresse.anrede panrede, adresse.name plastname, adresse.vorname pfirstname, adresse.strasse pstrasse, adresse.hsnr phsnr,adresse.plz pplz, adresse.ort port, adresse.sysland psysland, adresse.sysstaat psysstaat from adresse where adresse.sysperson=:syskd and adresse.rang(+)=2";
        private static String QUERYSTAAT = "select sysstaat from staat where sysland=:sysland and sysstaat=:sysstaat";
        private static String QUERYWKTACCOUNTIT = "select fparkgroesse,IT.sysit*-1 AS wktid, it.sysit, sysperson, matchcode, code,    syskdtyp,  anrede, anredecode, titel, titelCode, titel2,name, vorname, gebdatum, sysctlang, sysctlangkorr, syslandnat, einreisedatum, auslausweis, strasse, hsnr, plz, ort, sysland, sysstaat, wohnseit, strasse2, hsnr2, plz2, ort2, sysland2, sysstaat2, telefon, telefon2, handy, erreichbtel, erreichbvon, erreichbbis, email, url, sysbranche, '' rechtsform, rechtsformcode, gruendung, hregister, hregisterflag, revflag, zusatz, namekont, vornamekont,  fax,'' as gebort,'' as hregisterort,'' as identeu, '' as steuernr, identust,0 as nomailingflag,privatflag,0 as gesflag, 0 AS FlagKd, 'GROßKUNDE' AS kundengruppe  from cic.it where it.sysit=:sysaccount";
        private static String QUERYWKTACCOUNTPERSON = "select ptelefon,fparkgroesse,sysperson as wktid, 0 as sysit, sysperson, matchcode, code,    syskdtyp,  anrede, anredecode, titel, titelCode, titel2,name, vorname, gebdatum, sysctlang, sysctlangkorr, syslandnat, einreisedatum, auslausweis, strasse, hsnr, plz, ort, sysland, sysstaat, wohnseit, strasse2, hsnr2, plz2, ort2, sysland2, sysstaat2, telefon, telefon2, handy, erreichbtel, erreichbvon, erreichbbis, email, url, sysbranche, rechtsform, rechtsformcode, gruendung, hregister, hregisterflag, revflag, zusatz, namekont, vornamekont,  fax, gebort, hregisterort, identeu, steuernr, identust, nomailingflag,privatflag, gesflag, 1 AS FlagKd, kundengruppe from cic.person where person.sysperson=:sysaccount";
        private static String QUERYWKTACCOUNTOPTS = "select int01 dauerrechnung,int02 monatsrechnungart from  peoption where sysid=:sysaccount";
        private static String QUERYWKTITOPTS = "select int01 dauerrechnung,int02 monatsrechnungart from  itoption where sysit=:sysit";
        private static String QUERYBOMITARBEITER = "select admadd.bezeichnung from admadd,kne where sysunter=:sysadmadd and admadd.sysadmadd=kne.sysober and admadd.aktivkz=1";
        private static String QUERYIT = "select * from cic.it it where sysit=:sysit";
        private static String QUERYPARTNER = "select PTRELATE.*, person.sysperson, matchcode, code,    syskdtyp,  anrede, anredecode, titel, titelCode, titel2,name, vorname, gebdatum, sysctlang, sysctlangkorr, syslandnat, einreisedatum, auslausweis, strasse, hsnr, plz, ort, sysland, sysstaat, wohnseit, strasse2, hsnr2, plz2, ort2, sysland2, sysstaat2, telefon, telefon2, handy, erreichbtel, erreichbvon, erreichbbis, email, url, sysbranche, rechtsform, rechtsformcode, gruendung, hregister, hregisterflag, revflag, zusatz, namekont, vornamekont,  fax, gebort, hregisterort, identeu, steuernr, identust, nomailingflag,privatflag, gesflag  from cic.person person, cic.ptrelate ptrelate where ptrelate.sysptrelate=:syspartner and ptrelate.sysperson2=person.sysperson";
        private static String QUERYPARTNER2 = "select person.sysperson*-1 sysptrelate, person.sysperson*-1 sysperson2, person.sysperson, matchcode, code,    syskdtyp,  anrede, anredecode, titel, titelCode, titel2,name, vorname, gebdatum, sysctlang, sysctlangkorr, syslandnat, einreisedatum, auslausweis, strasse, hsnr, plz, ort, sysland, sysstaat, wohnseit, strasse2, hsnr2, plz2, ort2, sysland2, sysstaat2, telefon, telefon2, handy, erreichbtel, erreichbvon, erreichbbis, email, url, sysbranche, rechtsform, rechtsformcode, gruendung, hregister, hregisterflag, revflag, zusatz, namekont, vornamekont,  fax, gebort, hregisterort, identeu, steuernr, identust, nomailingflag,privatflag, gesflag  from cic.person person where sysperson=:syspartner";

        private static String QUERYBETEILIGTER = "select CRMNM.*, person.sysperson, matchcode, code,    syskdtyp,  anrede, anredecode, titel, titelCode, titel2,name, vorname, gebdatum, sysctlang, sysctlangkorr, syslandnat, einreisedatum, auslausweis, strasse, hsnr, plz, ort, sysland, sysstaat, wohnseit, strasse2, hsnr2, plz2, ort2, sysland2, sysstaat2, telefon, telefon2, handy, erreichbtel, erreichbvon, erreichbbis, email, url, sysbranche, rechtsform, rechtsformcode, gruendung, hregister, hregisterflag, revflag, zusatz, namekont, vornamekont,  fax, gebort, hregisterort, identeu, steuernr, identust, nomailingflag,privatflag, gesflag  from cic.person person, cic.CRMNM CRMNM where CRMNM.syscrmnm=:syscrmnm and CRMNM.childarea='PERSON' and CRMNM.SYSIDCHILD=person.sysperson";
        private static String QUERYBETEILIGTER2 = "select person.sysperson, matchcode, code,    syskdtyp,  anrede, anredecode, titel, titelCode, titel2,name, vorname, gebdatum, sysctlang, sysctlangkorr, syslandnat, einreisedatum, auslausweis, strasse, hsnr, plz, ort, sysland, sysstaat, wohnseit, strasse2, hsnr2, plz2, ort2, sysland2, sysstaat2, telefon, telefon2, handy, erreichbtel, erreichbvon, erreichbbis, email, url, sysbranche, rechtsform, rechtsformcode, gruendung, hregister, hregisterflag, revflag, zusatz, namekont, vornamekont,  fax, gebort, hregisterort, identeu, steuernr, identust, nomailingflag,privatflag, gesflag  from cic.person person where sysperson=:sysperson";

        private static String QUERYPUSER = @"
			select konto.kontonr kontonummer,
            trim((select nvl(cttstatedef.REPLACEZUSTAND,STATEDEF.ZUSTAND) from cttstatedef, STATEDEF, ctlang where STATEDEF.SYSSTATEDEF = cttstatedef.SYSSTATEDEF(+) and ctlang.sysctlang = cttstatedef.sysctlang(+)
              and STATEDEF.ZUSTAND = decode(nvl(puser.syspuser,0),0,'ePos inaktiv',nvl(ma.zustand,decode(ma.inactiveflag,0,'Aktiv','Inaktiv'))) and ctlang.isocode=:isocode)
              || ' ' ||
              (    select nvl(CTTATTRIBUTDEF.REPLACEATTRIBUT,ATTRIBUTDEF.ATTRIBUT) from CTTATTRIBUTDEF, ATTRIBUTDEF, ctlang where ATTRIBUTDEF.SYSATTRIBUTDEF = CTTATTRIBUTDEF.SYSATTRIBUTDEF(+) and ctlang.sysctlang = CTTATTRIBUTDEF.sysctlang(+)
              and ATTRIBUTDEF.ATTRIBUT = ma.attribut and ctlang.isocode=:isocode)
              ) status, 
			konto.iban, konto.kontoinhaber, konto.sysblz, person.anredecode anredecode, person.anrede anrede, person.syslandnat, person.gebdatum, person.sysctlang, person.sysctlangkorr,person.telefon,person.fax,peoption.option7 mobile,
			person.strasse,person.hsnr,person.ort,person.plz,person.sysland,person.sysstaat,peoption.option3 email,peoption.str03 extuserid,
			(select max(rolle.id) FROM cic.vc_ddlkppos rolle, roleattrib,roleattribm WHERE roleattribm.sysperole=perole.sysperole and roleattribm.sysroleattrib=roleattrib.sysroleattrib AND rolle.code = 'ROLLENAUSPRAEGUNGEN' and rolle.domainid = 'ROLLE' AND rolle.sysctlang=1 and rolle.id=to_char(roleattrib.sysroleattrib)) AS rolle,
				(select count(*) from RGR, RGM where rgr.sysrgr = rgm.sysrgr and rgr.name in ('EPOS_ADMIN') and rgm.syswfuser = wfuser.syswfuser and not exists (select 1 from wftzvar v, wftzust z where z.syswftzust=v.syswftzust and z.syswftable=2 and z.syslease = wfuser.sysperson and  z.state = 'RECHTE_FREIGABE' and v.code = to_char(rgm.sysrgr) and v.value = '0' )) adminflag,
					(select count(*) from RGR, RGM where rgr.sysrgr = rgm.sysrgr and rgr.name in ('EPOS_VERTRAGSABSCHLUSS') and rgm.syswfuser = wfuser.syswfuser and not exists (select 1 from wftzvar v, wftzust z where z.syswftzust=v.syswftzust and z.syswftable=2 and z.syslease = wfuser.sysperson and z.state = 'RECHTE_FREIGABE' and v.code = to_char(rgm.sysrgr) and v.value = '0' )) vtflag, 
					person.ahvnummer ahv, person.quellsteuerpflicht steuerflag, puser.externeid, wfuser.sysperson, puser.syspuser, wfuser.syswfuser, roletype.name rolle, roletype.typ role, 
					/* rh: war wfuser.name , wfuser.vorname, */
					person.name, person.vorname, 
					perole.validfrom, perole.validuntil, perole.inaktivgrund, perole.sysperole, perole.sysparent, perole.zustand, perole.attribut, roletype.*, 
					puser.* 
					from puser, perole, roletype, wfuser, person, konto, peoption, perole ma
					where wfuser.sysperson = perole.sysperson(+)
					and person.sysperson = perole.sysperson
					and peoption.sysid(+) = person.sysperson
					and roletype.sysroletype = perole.sysroletype
					and wfuser.sysperson = puser.sysperson(+)
					and konto.sysperson(+) = person.sysperson
					and konto.rang(+) = 37
                    and wfuser.sysperson=ma.sysperson 
					and wfuser.syswfuser = :syswfuser";

        private static String QUERYSTRASSE = "select *  from cic.strasse strasse where sysstrasse=:sysstrasse";
        private static String QUERYADRTP = "select *  from cic.adrtp adrtp where sysadrtp =:sysadrtp";
        private static String QUERYKONTOTP = "select *  from cic.kontotp kontotp where syskontotp=:syskontotp";
        private static String QUERYBLZ = "select *  from cic.blz blz where sysblz=:sysblz";
        private static String QUERYPTRELATE = "select *  from cic.ptrelate ptrelate where syspartner=:sysptrelate";
        private static String QUERYCRMNM = "select *  from cic.crmnm crmnm where sysidparent=:syscrmnm";

        private static String QUERYDDLKPRUB = "select *  from cic.ddlkprub ddlkprub where sysddlkprub=:sysddlkprub";
        private static String QUERYDDLKPCOL = "select *  from cic.ddlkpcol ddlkpcol where sysddlkpcol=:sysddlkppos";
        private static String QUERYDDLKPPOS = "select *  from cic.ddlkppos ddlkppos where sysddlkppos=:sysddlkppos";
        private static String QUERYDDLKPSPOS = "select *  from cic.ddlkpspos ddlkpspos where sysddlkpspos=:sysddlkpspos";
        private static String QUERYCAMPTP = "select *  from cic.camptp camptp where syscamptp=:syscamptp";
        private static String QUERYZINSTAB = "select *  from cic.zinstab zinstab where syszinstab=:syszinstab";
        private static String QUERYOPPOTP = "select *  from cic.oppotp oppotp where sysoppotp=:sysoppotp";
        private static String QUERYCRMPR = "select *  from cic.crmpr crmpr where syscrmpr=:syscrmpr";
        private static String QUERYCONTACTTP = "select *  from cic.contacttp contacttp where syscontacttp=:syscontacttp";
        private static String QUERYITEMCATM = "select *  from cic.itemcatm itemcatm  where sysitemcatm =:sysitemcatm";
        private static String QUERYRECURR = "select *  from cic.recurr recurr  where sysrecurr =:sysrecurr";
        private static String QUERYPTYPEPTASK = "select ptask.sysptask from ptask,pchecker where pchecker.syswfuser=:syswfuser and pchecker.sysptask=ptask.sysptask and sysptype=:sysptype";
        private static String QUERYPTYPE = "select ptype.*,pcontent.*  from cic.ptype ptype, cic.pcontent pcontent,cic.ctlang  where ptype.sysptype =:sysptype and ptype.sysptype=pcontent.sysptype and ctlang.sysctlang=pcontent.sysctlang and ctlang.sysctlang=NVL((SELECT MIN(ctlang.sysctlang) FROM cic.ctlang ctlang, pcontent pc WHERE ctlang.sysctlang=pc.sysctlang AND pc.sysptype=ptype.sysptype AND ctlang.isocode = :isocode), (SELECT MIN(pc.sysctlang) FROM pcontent pc WHERE pc.sysptype=ptype.sysptype ))";
        //private static String QUERYPSUBJECTS = "select name||' '||vorname from person,psubject, ptask where ptask.sysptype=:sysptype and psubject.sysptask=ptask.sysptask and psubject.area='PERSON' and psubject.syslease=person.sysperson";
        private static String QUERYPCHECKER = "select name||' '||vorname from wfuser,pchecker, ptask where ptask.sysptype=:sysptype and pchecker.sysptask=ptask.sysptask and pchecker.syswfuser=wfuser.syswfuser";
        private static String QUERYPSUBJECTSLIST = @"select (select max(rolle.id) from cic.vc_ddlkppos rolle, roleattrib,roleattribm,perole where perole.sysperson= person.sysperson and roleattribm.sysperole=perole.sysperole and roleattribm.sysroleattrib=roleattrib.sysroleattrib
              and rolle.code='ROLLENAUSPRAEGUNGEN' and rolle.domainid='ROLLE' and rolle.sysctlang=1 and rolle.id=to_char(roleattrib.sysroleattrib)) as rolle, psubject.*,person.name, person.vorname from person,psubject, ptask,pchecker where pchecker.syswfuser=:syswfuser and pchecker.sysptask=ptask.sysptask and ptask.sysptype=:sysptype and psubject.sysptask=ptask.sysptask and psubject.area='PERSON' and psubject.syslease=person.sysperson";
        private static String QUERYPRUNSTEP = "select *  from cic.prunstep prunstep  where sysprunstep =:sysprunstep";
        private static String QUERYPSTEP = "select *  from cic.pstep pstep  where syspstep =:syspstep";
        private static String QUERYPRKGROUP = "select *  from cic.prkgroup prkgroup  where sysprkgroup =:sysprkgroup";
        private static String QUERYPRKGROUPM = "select *  from cic.prkgroupm prkgroupm  where sysprkgroupm =:sysprkgroupm";
        private static String QUERYPRKGROUPZ = "select *  from cic.prkgroupz prkgroupz  where sysprkgroupz =:sysprkgroupz";
        private static String QUERYPRKGROUPS = "select *  from cic.prkgroups prkgroups  where sysprkgroups =:sysprkgroups";
        private static String QUERYSEG = "select *  from cic.seg seg  where sysseg =:sysseg";
        private static String QUERYSEGC = "select *  from cic.segc segc  where syssegc =:syssegc";
        private static String QUERYDDLKPSPOSAREA = "select * from CIC.DDLKPSPOS DDLKPSPOS where area=:area and sysid=:sysid";
        
        private static String QUERYDDLKPRUBSCODEAREA = "select *  from cic.ddlkprub ddlkprub where code=:code and area=:area order by ddlkprub.rank ";
        private static String QUERYDDLKPRUBSAREA = "select *  from cic.ddlkprub ddlkprub where area=:area order by ddlkprub.rank ";
        private static String QUERYDDLKPRUBSCODE = "select *  from cic.ddlkprub ddlkprub where code=:code order by ddlkprub.rank ";
        private static String QUERYDDLKPRUBS = "select *  from cic.ddlkprub ddlkprub order by ddlkprub.rank ";

        private static String QUERYDDLKPCOLS = "select *  from cic.ddlkpcol ddlkpcol where sysddlkprub is not null order by ddlkpcol.rank ";
        private static String QUERYDDLKPPOSCOL = "select *  from cic.ddlkppos ddlkppos where sysddlkpcol is not null order by sysddlkpcol asc";
        private static String QUERYSTICKYNOTE = "select * from cic.stickynote where sysstickynote =:sysstickynote";
        private static String QUERYSTICKYTYPE = "select * from cic.stickytype where sysstickytype =:sysstickytype";
        private static String QUERYWFSIGNATURE = "select * from cic.wfsignature where syswfsignature =:syswfsignature";
        private static String QUERY_CHECK_PRKGROUP = "SELECT SYSPRKGROUPM from cic.PRKGROUPM where sysperson=:sysperson and sysprkgroup=:sysprkgroup";
        //ANGEBOT
        //private static String QUERYANGEBOT = "select cic.angebot.sysid, cic.angebot.sysls, cic.angebot.angebot, cic.angebot.sysls, cic.angebot.syswaehrung, cic.angebot.vart, cic.angebot.konstellation, cic.angebot.vertriebsweg, cic.angebot.Fform, cic.angebot.beginn, cic.angebot.uebernahme, cic.angebot.bgextern," +
        //" angebot.lz, angebot.dataktiv, angebot.sz, angebot.ppy, angebot.ende, angebot.rueckgabe, angebot.rw, angebot.zustand, angebot.ok, angebot.aktivkz, angebot.locked, angebot.endekz, angebot.endeam, cic.angebot.sysadm, cic.antrag.sysvpfil from angebot where angebot.sysid = :sysangebot";
        private static String QUERYANGVAR = "select cic.angvar.sysangvar, cic.angvar.sysangebot, cic.angvar.rang, cic.angvar.bezeichnung, cic.angvar.beschreibung from cic.angvar where cic.angvar.sysangebot = :sysangebot";
        private static String QUERYANGOBINI = "select * from cic.angobini where sysobini=:sysobini";
        private static String QUERYANGOBAUST = "select * from cic.angobaust where sysangob=:sysangob";
        private static String QUERYANGOBAUSTID = "select sysobaust from cic.angobaust where sysangob=:sysangob";
        private static String QUERYANTOBAUSTID = "select sysobaust from cic.antobaust where sysantob=:sysantob";
        private static String QUERYOBAUSTID = "select sysobaust from cic.obaust where sysob=:sysob";
        private static String QUERYANGEBOTOPTS = "select * from  angoption where sysid=:sysid";
        private static String QUERYANTRAGOPTS = "select * from  antoption where sysid=:sysid";
        private static String QUERYANGEBOTMYCALCS = "SELECT count(*) from mycalc where sysangebot=:sysangebot";
        private static String QUERYMYCALCAUSTID = "select sysobaust from cic.mycalcaust where sysmycalc=:sysmycalc";


        private static String QUERYANGOBANGEBOT = "select * from cic.angob where cic.angob.sysvt = :sysangvar";
        private static String QUERYANGOB = "select * from cic.angob where sysob = :sysangob";
        private static String QUERYANTOB = "select * from cic.antob where sysantrag = :sysantrag";
        private static String QUERYANTKONTO = "select konto.* from konto,kontoref,antrag where konto.sysperson=kontoref.sysperson and kontoref.sysperson=antrag.syskd and konto.syskonto=kontoref.syskonto and kontoref.sysantrag=antrag.sysid and antrag.sysid=:sysantrag";
        private static String QUERYANTOB2 = "select * from cic.antob where sysvt = :sysantrag";

        private static String QUERYANTOBAUST = "select * from cic.antobaust where sysantob=:sysantob";
        private static String QUERYANTKALK = "select antkalk.* from antkalk where antkalk.syskalk =:sysantkalk";

        private static String QUERYOB = "select * from cic.ob where cic.ob.sysob = :sysob";
        private static String QUERYOBAUST = "select * from cic.obaust where sysob=:sysob";

        private static String QUERYANGKALK = "select angkalk.* from angkalk where angkalk.syskalk =:sysangkalk";
        private static String QUERYSLPOS = "select slpos.*, sllink.syssllink, sl.syssltyp from slpos,sllink,sl where sl.syssl=slpos.syssl and slpos.syssl=sllink.syssl and sllink.sysid=:sysangkalk and sllink.gebiet='ANGKALK' order by syssltyp,valuta";
        private static String QUERYSLPOSANT = "select slpos.*, sllink.syssllink, sl.syssltyp from slpos,sllink,sl where sl.syssl=slpos.syssl and slpos.syssl=sllink.syssl and sllink.sysid=:sysantkalk and sllink.gebiet='ANTKALK' order by syssltyp,valuta";


        private static String QUERYANGOBSL = "select cic.angobsl.sysid, cic.angobsl.sysvt, cic.angobsl.rang, cic.angobsl.bezeichnung from cic.angobsl where cic.angobsl.sysvt = :sysangvar";
        private static String QUERYANGOBSLPOS = "select cic.angobslpos.sysid, cic.angobslpos.sysvtobsl, cic.angobslpos.rang, cic.angobslpos.anzahl, cic.angobslpos.betrag from cic.angobslpos where cic.angobslpos.sysvtobsl = :sysvtobsl";
        private static String QUERYLSADD = "select cic.lsadd.syslsadd, cic.lsadd.bezeichnung, cic.lsadd.mandant from cic.lsadd where cic.lsadd.syslsadd = :syslsadd";
        private static String QUERYWAEHRUNG = "select cic.waehrung.syswaehrung, cic.waehrung.code,  cic.waehrung.bezeichnung  from cic.waehrung where cic.waehrung.syswaehrung = :syswaehrung";
        private static String QUERYVPFILADDANGEBOT = "select * from cic.vpfiladd where cic.vpfiladd.sysvpfiladd in (Select cic.angebot.sysvpfil from cic.angebot where cic.angebot.sysid = :sysid)";
        //ANTRAG
        //private static String QUERYANTRAG = "select cic.antrag.sysid, cic.antrag.antrag, cic.antrag.sysls, cic.antrag.syswaehrung, cic.antrag.sysvart, cic.antrag.vart, cic.antrag.konstellation, cic.antrag.vertriebsweg, cic.antrag.fform, cic.antrag.beginn, cic.antrag.uebernahme, cic.antrag.bgextern, cic.antrag.lz, cic.antrag.dataktiv, cic.antrag.sz, cic.antrag.ppy, cic.antrag.ende, cic.antrag.rueckgabe, cic.antrag.rw, cic.antrag.zustand, cic.antrag.ok, antrag.aktivkz, cic.antrag.locked, cic.antrag.endekz, cic.antrag.endeam, cic.antrag.sysadm, cic.antrag.sysvpfil from cic.antrag where cic.antrag.sysid= :sysantrag";
        //private static String QUERYANTOB = "select cic.antob.sysvt, cic.antob.rang, cic.antob.objekt, cic.antob.bezeichnung, cic.antob.objektvt from cic.antob where cic.antob.sysvt = :sysangvar";
        //private static String QUERYANTOBSL = "select cic.antobsl.sysid, cic.antobsl.sysvt, cic.antobsl.rang, cic.antobsl.bezeichnung, cic.antobsl.faellig from cic.antobsl where cic.antobsl.sysvt = :sysangvar";
        //private static String QUERYANTOBSLPOS = "select cic.antobslpos.sysid, cic.antobslpos.sysvtobsl, cic.antobslpos.rang, cic.antobslpos.anzahl, cic.antobslpos.betrag from cic.antobslpos where cic.antobslpos.sysvtobsl = :sysvtobsl";
        private static String QUERYVPFILADDANTRAG = "select * from cic.vpfiladd where cic.vpfiladd.sysvpfiladd in (Select cic.antrag.sysvpfil from cic.antrag where cic.antrag.sysid = :sysid)";
        //VERTRAG
        //private static String QUERYVERTRAG = "select cic.vt.sysid, cic.vt.vertrag, cic.vt.sysls, cic.vt.syswaehrung, cic.vt.sysvart, cic.vt.vart, cic.vt.konstellation, cic.vt.vertriebsweg, cic.vt.fform, cic.vt.beginn, cic.vt.uebernahme, cic.vt.bgextern, cic.vt.lz, cic.vt.dataktiv, cic.vt.sz, cic.vt.ppy, cic.vt.ende, cic.vt.rueckgabe, cic.vt.rw, cic.vt.zustand, cic.vt.ok, cic.vt.aktivkz, cic.vt.locked, cic.vt.endekz, cic.vt.endekz, cic.vt.endeam, cic.vt.sysadm, cic.vt.sysvpfil from cic.vt where cic.vt.sysid = :sysvt";
        private static String QUERYOBVT = "select * from cic.ob where cic.ob.sysvt = :sysvt";
        private static String QUERYVTOBSL = "select cic.vtobsl.sysid, cic.vtobsl.sysvt, cic.vtobsl.rang, cic.vtobsl.bezeichnung, cic.vtobsl.faellig from cic.vtobsl where cic.vtobsl.sysvt = :sysvt";
        private static String QUERYVTOBSLPOS = "select cic.vtobslpos.sysid, cic.vtobslpos.sysvtobsl, cic.vtobslpos.rang, cic.vtobslpos.anzahl, cic.vtobslpos.betrag from cic.vtobslpos where cic.vtobslpos.sysvtobsl = :sysvtobsl order by rang";
        private static String QUERYVPFILADDVERTRAG = "select * from cic.vpfiladd where cic.vpfiladd.sysvpfiladd in (Select cic.vt.sysvpfil from cic.vt where cic.vt.sysid = :sysid)";
        private static String QUERYVTDEPOT = "Select depot from kalk where sysob = :sysob order by syskalk desc";
        //ADMADD
        private static String QUERYADMADD = "select * from cic.admadd where cic.admadd.sysadmadd = :sysadmadd";

        private static String QUERYPRINTSET = "select * from cic.printset where sysprintset=:sysprintset";
        private static String QUERYPRTLGSET = "select * from cic.prtlgset where sysprtlgset=:sysprtlgset";
        private static String QUERYOBKAT = "select * from cic.obkat where sysobkat=:sysobkat";
        private static String QUERYVART = "select * from cic.vart where sysvart=:sysvart";
        private static String QUERYVARTS = "select * from cic.vart";
        private static String QUERYEAIHFILE = "select * from cic.eaihfile where syseaihot=:syseaihot";
        private static String QUERYOBINI = "select * from obini where sysobini=:sysobini";
        //private static String QUERYZEK = "select * from zek where syszek=:syszek";
        private static String QUERYPROCESS = "select bplistener.sysbplistener, ANTRAG.ERFASSUNG datum, antrag.syskd, WFUSER.name benutzer, WFUSER.syswfuser syswfuser, bplistener.eventcode process, bplistener.namebplane casestep from CIC.bplistener, CIC.ANTRAG ANTRAG, CIC.VART VART, CIC.ANTOB ANTOB, CIC.PERSON VM, CIC.PERSON VK, CIC.ANTKALK ANTKALK, CIC.PRPRODUCT PRPRODUCT, CIC.WFUSER WFUSER, CIC.IT IT, CIC.VT VT where bplistener.sysbplistener=:sysprocess and bplistener.sysoltable=vt.sysid and bplistener.oltable='VT' and not exists ( select * from bpproccasevar where bpproccasevar.namebpvartype = 'DESKTOP_AUSBLENDEN' and bpproccasevar.namebpvarvalue = '1' and bplistener.sysbpprocinst = bpproccasevar.sysbpprocinst ) and bplistener.eventcode not in ('evtd_VI_Strategie_Update') and ( VART.SYSVART (+) = PRPRODUCT.SysVART AND ANTKALK.SysPRPRODUCT = PRPRODUCT.SysPRPRODUCT (+) AND ANTKALK.SysANTRAG(+) = ANTRAG.SysID ) AND (ANTOB.SysANTRAG(+) = ANTRAG.SysID) AND (ANTRAG.SysVM = VM.SysPERSON(+)) AND (ANTRAG.SysVK = VK.SysPERSON(+))  AND (ANTRAG.SysWFUSER = WFUSER.SysWFUSER(+)) AND (ANTRAG.SysIT = IT.SysIT(+)) and vt.sysantrag=antrag.sysid";
        #endregion


        private static String QUERYKDEXT = @"SELECT person.sysperson, person.ausschluss, mahn.*,(SELECT EINKNETTO + CASE WHEN MONATSLOHNXTDFLAG = 1 THEN (EINKNETTO - ZULAGEKIND - ZULAGEAUSBILDUNG - ZULAGESONST) / 12 ELSE 0 END + (JBONUSNETTO/12) + NEBEINKNETTO + ZEINKNETTO 
from pkz where syspkz = (select max(syspkz) from pkz where sysperson=person.sysperson)) einknettoberech,(select miete from pkz where syspkz = (select max(syspkz) from pkz where sysperson=person.sysperson))  miete, aufst.aufstockstopbis, eng.*, kremo.budget1, peoption.ulon02 risikoklasse, 
(select listagg(perkam.name,',') within group (order by perkam.name asc) from perelate, perole,perkam,roletype where roletype.sysroletype=perkam.sysroletype and roletype.code='KAM' and perole.sysperole in (select perole.sysperole from perole, roletype where (perole.validuntil is null or perole.validuntil>=sysdate  or perole.validuntil=to_date('01.01.0111' , 'dd.MM.yyyy')) and (perole.validfrom is null or perole.validfrom<=sysdate  or perole.validfrom=to_date('01.01.0111' , 'dd.MM.yyyy')) and  perole.sysroletype=roletype.sysroletype and perole.sysperole in (select sysperole from perole connect by PRIOR sysparent = sysperole start with perole.sysperson=person.sysperson) and roletype.typ=6) and perole.sysperole=perelate.sysperole2 and perkam.sysperole=perelate.sysperole1 
and (perelate.relbeginndate is null or perelate.relbeginndate<=sysdate or perelate.relbeginndate=to_date('01.01.0111', 'dd.MM.yyyy')) and (perelate.relenddate is null or perelate.relenddate>=sysdate or perelate.relenddate=to_date('01.01.0111', 'dd.MM.yyyy')) )  as KAM
,(select listagg(perpop.name,',') within group (order by perpop.name asc) from perelate, perole,perpop,roletype where roletype.sysroletype=perpop.sysroletype and roletype.code='AWO' and perole.sysperole in (select perole.sysperole from perole, roletype where (perole.validuntil is null or perole.validuntil>=sysdate  or perole.validuntil=to_date('01.01.0111' , 'dd.MM.yyyy')) and (perole.validfrom is null or perole.validfrom<=sysdate  or perole.validfrom=to_date('01.01.0111' , 'dd.MM.yyyy')) and  perole.sysroletype=roletype.sysroletype and perole.sysperole in (select sysperole from perole connect by PRIOR sysparent = sysperole start with perole.sysperson=person.sysperson) and roletype.typ=6) and perole.sysperole=perelate.sysperole2 and perpop.sysperole=perelate.sysperole1
and (perelate.relbeginndate is null or perelate.relbeginndate<=sysdate or perelate.relbeginndate=to_date('01.01.0111', 'dd.MM.yyyy')) and (perelate.relenddate is null or perelate.relenddate>=sysdate or perelate.relenddate=to_date('01.01.0111', 'dd.MM.yyyy')) ) as abwicklungsort

FROM person,
                                                  (SELECT vt.syskd,
                                                    MAX(mstufe) mstufe,
                                                    SUM(mzaehler1) mzaehler1,
                                                    SUM(mzaehler2) mzaehler2,
                                                    SUM(mzaehler3) mzaehler3,
                                                    MAX(mdatum1) mdatum1,
                                                    MAX(mdatum2) mdatum2,
                                                    MAX(mdatum3) mdatum3
                                                  FROM vtmahn,
                                                    vt
                                                  WHERE sysvtmahn =vt.sysid
                                                  GROUP BY vt.syskd
                                                  ) mahn, peoption,
                                                  (SELECT syskd,
                                                    MAX(aufstockstopbis) aufstockstopbis
                                                  FROM antrag
                                                  GROUP BY syskd
                                                  ) aufst,
                                                  (SELECT vt.syskd,
                                                    SUM(fivkznm.sbsaldos - fivkznm.sbsaldoh) AS engagement
                                                  FROM penkonto,
                                                    fivkznm,
                                                    vt
                                                  WHERE fivkznm.sysskonto                   = penkonto.sysnkonto
                                                  AND penkonto.rang                         = 10000
                                                  AND fivkznm.sysperiod                     = EXTRACT (YEAR FROM sysdate) - 2005
                                                  AND fivkznm.periode                       = EXTRACT (MONTH FROM sysdate)
                                                  AND (fivkznm.sbsaldos - fivkznm.sbsaldoh <> 0)
                                                  AND penkonto.sysvt                        = vt.sysid (+)
                                                  GROUP BY vt.syskd
                                                  ) eng,                                                  
                                                  kremo
                                                WHERE person.sysperson=:syskd
                                                AND person.sysperson  =mahn.syskd (+)
                                                AND person.sysperson  =peoption.SYSID (+)
                                                AND person.sysperson  =aufst.syskd (+)
                                                AND person.sysperson  =eng.syskd (+)
                                                AND person.sysperson  = kremo.sysperson (+)";

        private static String QUERYREGELN = @"SELECT DISTINCT externcode code,
  name text
FROM dedefrul,
  derul,
  dedetail,
  deoutexec
WHERE deoutexec.sysdeoutexec =dedetail.sysdeoutexec
AND dedetail.sysdedetail   =derul.sysdedetail
AND derul.sysdedefrul      =dedefrul.sysdedefrul
AND deoutexec.sysauskunft in
(SELECT auskunft.sysauskunft
  FROM auskunft,
    deenvinp,
    deinpexec
  WHERE auskunft.statusnum           = 0
  AND auskunft.area                  = 'ANTRAG'
  AND auskunft.sysauskunfttyp        = 3
  AND auskunft.sysauskunft           =deinpexec.sysauskunft
  AND deenvinp.sysdeinpexec          =deinpexec.sysdeinpexec
  AND auskunft.sysid                 =:sysAntrag
  )";
        private static String QUERYAUFLAGEN2 = @"select ratingauflage.sysratingauflage,dedefcon.name auflagetext,ratingauflage.fullfilled,ratingauflage.fullfilleddate, ratingauflage.syschgdate, ratingauflage.syschgtime, wfuser.vorname||' '||wfuser.name erledigtvon from dedefcon,ratingauflage,rating,wfuser where rating.area='ANTRAG' and rating.sysrating=ratingauflage.sysrating and ratingauflage.syschguser=wfuser.syswfuser(+) and 
                            dedefcon.sysdedefcon=ratingauflage.sysdedefcon and rating.sysid=:sysid";
        private static String QUERYAUFLAGEN = @"select case when ctype = 1 then '(A)' else '(F)' end||' '|| auflageText auflageText , antragsteller owner, fullfilled, fullfilleddate,erledigtvon, syschgdate, syschgtime from (
                                            select dedefcon.deftextext auflageText, dedefcon.rank rank,case when ratingauflage.sysperson =antrag.syskd then '1. AS' else '2. AS' end  antragsteller ,fullfilled, fullfilleddate ,wfuser.vorname||' '||wfuser.name erledigtvon,
                                            ratingauflage.syschgdate,ratingauflage.syschgtime ,dedefcon.contype ctype
                                          from dedefcon, ratingauflage, rating, wfuser,antrag
                                          where rating.area = 'ANTRAG' 
                                            and rating.sysid=antrag.sysid and rating.flag1=0
                                          and rating.sysid = :sysAntrag    
                                          and rating.sysrating = ratingauflage.sysrating    
                                          and ratingauflage.sysdedefcon = dedefcon.sysdedefcon    
                                          and dedefcon.contype  in  (1,2)                                             
                                          and not (ratingauflage.status = 1 and ratingauflage.activeflag = 0)    
                                          and ratingauflage.syschguser=wfuser.syswfuser(+)
                                          and dedefcon.sysdedefcon not in (select dedefcontxt.sysdedefcon from dedefcontxt, ctlang where dedefcontxt.sysctlang = ctlang.sysctlang and ctlang.isocode = :isoCode )   
                                          union all     
                                          select distinct(dedefcontxt.textext) auflageText, dedefcon.rank rank, case when ratingauflage.sysperson =antrag.syskd then '1. AS' else '2. AS' end  owner, fullfilled, fullfilleddate   ,wfuser.vorname||' '||wfuser.name erledigtvon,
                                          ratingauflage.syschgdate,ratingauflage.syschgtime,dedefcon.contype ctype
                                          from ctlang, dedefcontxt, dedefcon, ratingauflage, rating,wfuser  ,antrag
                                          where rating.area = 'ANTRAG' 
                                          and rating.sysid=antrag.sysid and rating.flag1=0
                                          and rating.sysid = :sysAntrag    
                                          and rating.sysrating = ratingauflage.sysrating    
                                          and ratingauflage.sysdedefcon = dedefcon.sysdedefcon    
                                          and dedefcon.contype  in  (1,2)                                           
                                          and not (ratingauflage.status = 1 and ratingauflage.activeflag = 0)    
                                          and dedefcon.sysdedefcon = dedefcontxt.sysdedefcon    
                                          and dedefcontxt.sysctlang = ctlang.sysctlang     
                                          and ctlang.isocode = :isoCode  
                                          and ratingauflage.syschguser=wfuser.syswfuser(+)
                                          order by rank) order by antragsteller, ctype asc";
        private static String QUERYCLARIFICATION = "select int05 erledigt,ulong04 sysbprole,wfmmemo.syswfmmemo,wfmmemo.syswfmtable,wfmmemo.syslease,wfmmemo.syswfmmkat,wfmmemo.createuser,wfmmemo.edituser,wfmmemo.sysbpprocinst,wfmmemo.kurzbeschreibung,wfmmemo.notizmemo,wfmmemo.createdate,wfmmemo.createtime createtimecla,wfuser.vorname||' '||wfuser.NAME username from wfmmemo,wfuser where wfuser.syswfuser=createuser and syswfmmemo=:syswfmmemo";
		private static String QUERYLETTERSALUTATION = 
							@"SELECT MAX (DECODE (DOMAINID, 'LANG', ACTUALTERM, '')) ANREDE
								FROM vc_ddlkppos
								WHERE CODE      = 'ANREDEN'
								  AND SYSCTLANG = :SYSCTLANG
								  AND ID        = :ANREDECODE
								GROUP BY ID";
		private static String QUERYTITLE =
							@"SELECT MAX (DECODE (DOMAINID, 'KURZ', ACTUALTERM, '')) TITEL
								FROM vc_ddlkppos
								WHERE CODE      = 'ANREDEN'
								  AND SYSCTLANG = :SYSCTLANG
								  AND ID        = :ANREDECODE
								GROUP BY ID";


		/*  DUMMY: SLA Detail Liste 
		private static String QUERYSLA = 
									@"SELECT   0 Aktiv,
										0 Pause,
										'Time to conditional Yes' Metrik ,
										'Warnung' Status,
										to_date ('23.02.2017','dd.mm.yyyy') StatusDate,
										344567 StatusTime ,
										NULL NextStatus,
										to_date ('01.01.0111','dd.mm.yyyy') NestStatusDate,
										0 NextStatusTime
									  FROM dual
									UNION ALL
									SELECT   1 Aktiv,
										1 Pause,
										'Time to Cash' Metrik ,
										'Warnung' Status,
										to_date ('23.02.2017','dd.mm.yyyy') StatusDate,
										347567 StatusTime ,
										'Ziel überschritten' NextStatus,
										to_date ('23.02.2017','dd.mm.yyyy') NestStatusDate,
										357567 NextStatusTime
									  FROM dual";
		 */
		/*  ORIGINAL: SLA Detail Liste */
		private static String QUERYSLA = @"
			SELECT SLA.NameIntern ServiceLevel, SLA.ACTIVEFLAG Aktiv, SLA.PauseFlag Pause, SLAMETRIC.DESCRIPTION Metrik , TRANSLATE_STATUS.ACTUALTERM Status, SLA.StatusDate,
				SLA.StatusTime, TRANSLATE_NEXTSTATUS.ACTUALTERM NextStatus, SLA.NextStatusDate, SLA.NextStatusTime
			FROM SLA, SLAMETRIC ,
				(SELECT * FROM CIC.VC_DDLKPPOS WHERE code = 'SLA_STATUS' AND sysctlang = (SELECT sysctlang FROM cic.ctlang WHERE ISOCODE = :ISOCODE)) TRANSLATE_STATUS ,
				(SELECT * FROM CIC.VC_DDLKPPOS WHERE code = 'SLA_STATUS' AND sysctlang = (SELECT sysctlang FROM cic.ctlang WHERE ISOCODE = :ISOCODE)) TRANSLATE_NEXTSTATUS
			WHERE SLAMETRIC.SYSSLAMETRIC           = SLA.SYSSLAMETRIC
				AND TRANSLATE_STATUS.ORIGTERM(+)     = SLA.STATUS
				AND TRANSLATE_NEXTSTATUS.ORIGTERM(+) = SLA.NEXTSTATUS
				AND (SLA.SYSANTRAG                   = :SYSID
				OR SLA.SYSANGEBOT                    = :SYSID)
			ORDER BY SLA.SYSANGEBOT DESC NULLS last,
				SLA.SYSANTRAG DESC NULLS last,
				SLAMETRIC.SYSSLAMETRIC,
				SLA.SYSID DESC NULLS last,
				SLA.SYSSLA DESC";

//		/*  ECHT-Daten TESTER OHNE ÜBERSETZUNG: SLA Detail Liste */ 
//		private static String QUERYSLA = @"
//			SELECT SLA.ACTIVEFLAG Aktiv, SLA.PauseFlag Pause, 
//				Name Metrik, Status, SLA.StatusDate, SLA.StatusTime ,
//				NextStatus, SLA.NextStatusDate, SLA.NextStatusTime
//			FROM SLA
//			WHERE (SLA.SYSANTRAG  = 32072
//				OR SLA.SYSANGEBOT = 32072)
//			ORDER BY SLA.SYSANGEBOT DESC NULLS last,
//				SLA.SYSANTRAG DESC NULLS last,
//				SLA.SYSID DESC NULLS last,
//				SLA.SYSSLA DESC";

		private static String QUERYPREADFLAG = @"
			SELECT * FROM PREAD
			WHERE area = 'BPLISTENER' 
				AND SYSID = :sysid";

        protected long sysPerole = 0;
        protected long sysWfuser = 0;
        protected String isoCode = null;

        private static long CACHE_TIMEOUT = 1000 * 60 * 60 * 12;//12H
        private static CacheDictionary<long, List<DdlkpposDto>> DdlkpposCache = CacheFactory<long, List<DdlkpposDto>>.getInstance().createCache(CACHE_TIMEOUT, CacheCategory.Data);
        private static CacheDictionary<long, List<DdlkpcolDto>> DdlkpcolCache = CacheFactory<long, List<DdlkpcolDto>>.getInstance().createCache(CACHE_TIMEOUT, CacheCategory.Data);
        private static CacheDictionary<String, List<DdlkprubDto>> DdlkprubCache = CacheFactory<String, List<DdlkprubDto>>.getInstance().createCache(CACHE_TIMEOUT, CacheCategory.Data);

        private static CacheDictionary<String, List<VartDto>> vartCache = CacheFactory<String, List<VartDto>>.getInstance().createCache(CACHE_TIMEOUT, CacheCategory.Data);

        public EntityDao()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysWfuser"></param>
        public void setSysWfuser(long sysWfuser)
        {
            this.sysWfuser = sysWfuser;
        }

        public long getSysWfuser()
        {
            return this.sysWfuser;
        }

        public void setSysPerole(long sysPerole)
        {
            this.sysPerole = sysPerole;
        }

        public long getSysPerole()
        {
            return this.sysPerole;
        }

        public void setISOLanguage(String isoCode)
        {
            this.isoCode = isoCode;
        }

        public String getISOLanguage()
        {
            return this.isoCode;
        }

        /// <summary>
        /// Returns the queryable result list for the given sql
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public IEnumerable<long> getSQLResults(String sql)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                return ctx.ExecuteStoreQuery<long>(sql, null).ToList<long>();
            }
        }

        #region CREATEORUPDATE


        /// <summary>
        /// updates/creates Gview
        /// </summary>
        /// <param name="gview"></param>
        /// <returns></returns>
        public GviewDto createOrUpdateGview(GviewDto gview)
        {


            if (gview == null) return gview;
            GviewDto rval = new GviewDto();

            using (DdCtExtended ctx = new DdCtExtended())
            {
                //Algorithm:
                //A fetch all schema information (column alias, table, length, type)
                //B iterate all conf tables, fkeyless
                //  create or update table, remember pkey for table
                //  -> these tables are done!
                //C iterate all conf tables with fkeys
                //  create or update table, assigned with fkeys, remember pkey for table
                //    if one fkey not yet done, mark table as update again for fkey
                //D iterate all conf tables with fkeys and marked as update again
                //  create or update table, assigned ONLY with fkeys

                //A:
                Dictionary<String, SchemaInfoDto> tabInfo = SchemaInfoDto.getSchemaInfos(ctx, gview.gviewId);
                List<SchemaInfoDto> tables = tabInfo.Values.ToList();

                //B:
                List<SchemaInfoDto> noFkey = (from t in tables
                                              where t.hasFkeys == false
                                              select t).ToList();
                foreach (SchemaInfoDto si in noFkey)
                {
                    si.createOrUpdate(ctx, gview, tabInfo);
                }
                //C:
                List<SchemaInfoDto> withFkey = (from t in tables
                                                where t.hasFkeys == true
                                                select t).ToList();
                foreach (SchemaInfoDto si in withFkey)
                {
                    si.createOrUpdate(ctx, gview, tabInfo);
                }
                //D:
                withFkey = (from t in tables
                            where t.hasFkeys == true && t.updateAgain == true
                            select t).ToList();
                foreach (SchemaInfoDto si in withFkey)
                {
                    si.createOrUpdate(ctx, gview, tabInfo);
                }


            }
            return getGviewDetails(gview.sysId, gview.gviewId,null);
        }

        /// <summary>
        /// updates/creates Staffelpositionstyp
        /// </summary>
        /// <param name="staffelpositionstyp"></param>
        /// <returns></returns>
        public StaffelpositionstypDto createOrUpdateStaffelpositionstyp(StaffelpositionstypDto staffelpositionstyp)
        {
            SLPOSTYP staffelpositionstypOutput = null;
            using (DdOlExtended ctx = new DdOlExtended())
            {
                if (staffelpositionstyp.sysslpostyp == 0)
                {
                    staffelpositionstypOutput = new SLPOSTYP();
                    staffelpositionstypOutput = Mapper.Map<StaffelpositionstypDto, SLPOSTYP>(staffelpositionstyp, staffelpositionstypOutput);
                    staffelpositionstypOutput.SYSSLPOSTYP = 0;

                    ctx.AddToSLPOSTYP(staffelpositionstypOutput);


                }
                else
                {
                    staffelpositionstypOutput = (from p in ctx.SLPOSTYP
                                                 where p.SYSSLPOSTYP == staffelpositionstyp.sysslpostyp
                                                 select p).FirstOrDefault();
                    if (staffelpositionstypOutput != null)
                        staffelpositionstypOutput = Mapper.Map<StaffelpositionstypDto, SLPOSTYP>(staffelpositionstyp, staffelpositionstypOutput);
                    else throw new Exception("Staffelpositionstyp with id " + staffelpositionstyp.sysslpostyp + " not found!");
                }

                ctx.SaveChanges();
                SearchCache.entityChanged("SLPOSTYP");
            }
            return Mapper.Map<SLPOSTYP, StaffelpositionstypDto>(staffelpositionstypOutput);
        }

        /// <summary>
        /// updates/creates Staffeltyp
        /// </summary>
        /// <param name="staffeltyp"></param>
        /// <returns></returns>
        public StaffeltypDto createOrUpdateStaffeltyp(StaffeltypDto staffeltyp)
        {
            SLTYP staffeltypOutput = null;
            using (DdOlExtended ctx = new DdOlExtended())
            {
                if (staffeltyp.syssltyp == 0)
                {
                    staffeltypOutput = new SLTYP();
                    staffeltypOutput = Mapper.Map<StaffeltypDto, SLTYP>(staffeltyp, staffeltypOutput);
                    staffeltypOutput.SYSSLTYP = 0;

                    ctx.AddToSLTYP(staffeltypOutput);


                }
                else
                {
                    staffeltypOutput = (from p in ctx.SLTYP
                                        where p.SYSSLTYP == staffeltyp.syssltyp
                                        select p).FirstOrDefault();
                    if (staffeltypOutput != null)
                        staffeltypOutput = Mapper.Map<StaffeltypDto, SLTYP>(staffeltyp, staffeltypOutput);
                    else throw new Exception("Staffeltyp with id " + staffeltyp.syssltyp + " not found!");
                }

                ctx.SaveChanges();
                SearchCache.entityChanged("SLTYP");
            }
            return Mapper.Map<SLTYP, StaffeltypDto>(staffeltypOutput);
        }

        /// <summary>
        /// updates/creates Rolle
        /// </summary>
        /// <param name="rolle"></param>
        /// <returns></returns>
        public RolleDto createOrUpdateRolle(RolleDto rolle)
        {
            PEROLE rolleOutput = null;
            using (DdOlExtended ctx = new DdOlExtended())
            {
                if (rolle.sysperole == 0)
                {
                    rolleOutput = new PEROLE();
                    rolleOutput = Mapper.Map<RolleDto, PEROLE>(rolle, rolleOutput);
                    rolleOutput.SYSPEROLE = 0;

                    ctx.AddToPEROLE(rolleOutput);


                }
                else
                {
                    rolleOutput = (from p in ctx.PEROLE
                                   where p.SYSPEROLE == rolle.sysperole
                                   select p).FirstOrDefault();
                    if (rolleOutput != null)
                        rolleOutput = Mapper.Map<RolleDto, PEROLE>(rolle, rolleOutput);
                    else throw new Exception("Rolle with id " + rolle.sysperole + " not found!");
                }

                ctx.SaveChanges();
                SearchCache.entityChanged("PEROLE");
            }
            return Mapper.Map<PEROLE, RolleDto>(rolleOutput);
        }

        /// <summary>
        /// updates/creates Rollentyp
        /// </summary>
        /// <param name="rollentyp"></param>
        /// <returns></returns>
        public RollentypDto createOrUpdateRollentyp(RollentypDto rollentyp)
        {
            ROLETYPE rollentypOutput = null;
            using (DdOlExtended ctx = new DdOlExtended())
            {
                if (rollentyp.sysroletype == 0)
                {
                    rollentypOutput = new ROLETYPE();
                    rollentypOutput = Mapper.Map<RollentypDto, ROLETYPE>(rollentyp, rollentypOutput);
                    rollentypOutput.SYSROLETYPE = 0;

                    ctx.AddToROLETYPE(rollentypOutput);


                }
                else
                {
                    rollentypOutput = (from p in ctx.ROLETYPE
                                       where p.SYSROLETYPE == rollentyp.sysroletype
                                       select p).FirstOrDefault();
                    if (rollentypOutput != null)
                        rollentypOutput = Mapper.Map<RollentypDto, ROLETYPE>(rollentyp, rollentypOutput);
                    else throw new Exception("Rollentyp with id " + rollentyp.sysroletype + " not found!");
                }

                ctx.SaveChanges();
                SearchCache.entityChanged("ROLETYPE");
            }
            return Mapper.Map<ROLETYPE, RollentypDto>(rollentypOutput);
        }

        /// <summary>
        /// updates/creates Handelsgruppe
        /// </summary>
        /// <param name="handelsgruppe"></param>
        /// <returns></returns>
        public HandelsgruppeDto createOrUpdateHandelsgruppe(HandelsgruppeDto handelsgruppe)
        {
            PRHGROUP handelsgruppeOutput = null;
            using (DdOlExtended ctx = new DdOlExtended())
            {
                if (handelsgruppe.sysprhgroup == 0)
                {
                    handelsgruppeOutput = new PRHGROUP();
                    handelsgruppeOutput = Mapper.Map<HandelsgruppeDto, PRHGROUP>(handelsgruppe, handelsgruppeOutput);
                    handelsgruppeOutput.SYSPRHGROUP = 0;

                    ctx.AddToPRHGROUP(handelsgruppeOutput);


                }
                else
                {
                    handelsgruppeOutput = (from p in ctx.PRHGROUP
                                           where p.SYSPRHGROUP == handelsgruppe.sysprhgroup
                                           select p).FirstOrDefault();
                    if (handelsgruppeOutput != null)
                        handelsgruppeOutput = Mapper.Map<HandelsgruppeDto, PRHGROUP>(handelsgruppe, handelsgruppeOutput);
                    else throw new Exception("Handelsgruppe with id " + handelsgruppe.sysprhgroup + " not found!");
                }

                ctx.SaveChanges();
                SearchCache.entityChanged("PRHGROUP");
            }
            return Mapper.Map<PRHGROUP, HandelsgruppeDto>(handelsgruppeOutput);
        }

        /// <summary>
        /// updates/creates Vertriebskanal
        /// </summary>
        /// <param name="vertriebskanal"></param>
        /// <returns></returns>
        public VertriebskanalDto createOrUpdateVertriebskanal(VertriebskanalDto vertriebskanal)
        {
            BCHANNEL vertriebskanalOutput = null;
            using (DdOlExtended ctx = new DdOlExtended())
            {
                if (vertriebskanal.sysbchannel == 0)
                {
                    vertriebskanalOutput = new BCHANNEL();
                    vertriebskanalOutput = Mapper.Map<VertriebskanalDto, BCHANNEL>(vertriebskanal, vertriebskanalOutput);
                    vertriebskanalOutput.SYSBCHANNEL = 0;

                    ctx.AddToBCHANNEL(vertriebskanalOutput);


                }
                else
                {
                    vertriebskanalOutput = (from p in ctx.BCHANNEL
                                            where p.SYSBCHANNEL == vertriebskanal.sysbchannel
                                            select p).FirstOrDefault();
                    if (vertriebskanalOutput != null)
                        vertriebskanalOutput = Mapper.Map<VertriebskanalDto, BCHANNEL>(vertriebskanal, vertriebskanalOutput);
                    else throw new Exception("Vertriebskanal with id " + vertriebskanal.sysbchannel + " not found!");
                }

                ctx.SaveChanges();
                SearchCache.entityChanged("BCHANNEL");
            }
            return Mapper.Map<BCHANNEL, VertriebskanalDto>(vertriebskanalOutput);
        }

        /// <summary>
        /// updates/creates Brand
        /// </summary>
        /// <param name="brand"></param>
        /// <returns></returns>
        public BrandDto createOrUpdateBrand(BrandDto brand)
        {
            BRAND brandOutput = null;
            using (DdOlExtended ctx = new DdOlExtended())
            {
                if (brand.sysbrand == 0)
                {
                    brandOutput = new BRAND();
                    brandOutput = Mapper.Map<BrandDto, BRAND>(brand, brandOutput);
                    brandOutput.SYSBRAND = 0;

                    ctx.AddToBRAND(brandOutput);


                }
                else
                {
                    brandOutput = (from p in ctx.BRAND
                                   where p.SYSBRAND == brand.sysbrand
                                   select p).FirstOrDefault();
                    if (brandOutput != null)
                        brandOutput = Mapper.Map<BrandDto, BRAND>(brand, brandOutput);
                    else throw new Exception("Brand with id " + brand.sysbrand + " not found!");
                }

                ctx.SaveChanges();
                SearchCache.entityChanged("BRAND");
            }
            return Mapper.Map<BRAND, BrandDto>(brandOutput);
        }

        /// <summary>
        /// updates/creates Rechnung
        /// </summary>
        /// <param name="rechnung"></param>
        /// <returns></returns>
        public RechnungDto createOrUpdateRechnung(RechnungDto rechnung)
        {
            RN rechnungOutput = null;
            using (DdOlExtended ctx = new DdOlExtended())
            {
                if (rechnung.sysid == 0)
                {
                    rechnungOutput = new RN();
                    rechnungOutput = Mapper.Map<RechnungDto, RN>(rechnung, rechnungOutput);
                    rechnungOutput.SYSID = 0;

                    ctx.AddToRN(rechnungOutput);


                }
                else
                {
                    rechnungOutput = (from p in ctx.RN
                                      where p.SYSID == rechnung.sysid
                                      select p).FirstOrDefault();
                    if (rechnungOutput != null)
                        rechnungOutput = Mapper.Map<RechnungDto, RN>(rechnung, rechnungOutput);
                    else throw new Exception("Rechnung with id " + rechnung.sysid + " not found!");
                }

                ctx.SaveChanges();
                SearchCache.entityChanged("RN");
            }
            return Mapper.Map<RN, RechnungDto>(rechnungOutput);
        }

        /// <summary>
        /// updates/creates Angobbrief
        /// </summary>
        /// <param name="angobbrief"></param>
        /// <returns></returns>
        public AngobbriefDto createOrUpdateAngobbrief(AngobbriefDto angobbrief)
        {
            ANGOBBRIEF angobbriefOutput = null;
            using (DdOlExtended ctx = new DdOlExtended())
            {
                if (angobbrief.sysangobbrief == 0)
                {
                    angobbriefOutput = new ANGOBBRIEF();
                    angobbriefOutput = Mapper.Map<AngobbriefDto, ANGOBBRIEF>(angobbrief, angobbriefOutput);
                    angobbriefOutput.SYSANGOBBRIEF = 0;

                    ctx.AddToANGOBBRIEF(angobbriefOutput);


                }
                else
                {
                    angobbriefOutput = (from p in ctx.ANGOBBRIEF
                                        where p.SYSANGOBBRIEF == angobbrief.sysangobbrief
                                        select p).FirstOrDefault();
                    if (angobbriefOutput != null)
                        angobbriefOutput = Mapper.Map<AngobbriefDto, ANGOBBRIEF>(angobbrief, angobbriefOutput);
                    else throw new Exception("Angobbrief with id " + angobbrief.sysangobbrief + " not found!");
                }

                ctx.SaveChanges();
                SearchCache.entityChanged("ANGOBBRIEF");
            }
            return Mapper.Map<ANGOBBRIEF, AngobbriefDto>(angobbriefOutput);
        }

        /// <summary>
        /// updates/creates Zahlungsplan
        /// </summary>
        /// <param name="zahlungsplan"></param>
        /// <returns></returns>
        public ZahlungsplanDto createOrUpdateZahlungsplan(ZahlungsplanDto zahlungsplan)
        {
            SLPOS zahlungsplanOutput = null;
            using (DdOlExtended ctx = new DdOlExtended())
            {
                if (zahlungsplan.sysslpos == 0)
                {
                    zahlungsplanOutput = new SLPOS();
                    zahlungsplanOutput = Mapper.Map<ZahlungsplanDto, SLPOS>(zahlungsplan, zahlungsplanOutput);
                    zahlungsplanOutput.SYSSLPOS = 0;

                    ctx.AddToSLPOS(zahlungsplanOutput);


                }
                else
                {
                    zahlungsplanOutput = (from p in ctx.SLPOS
                                          where p.SYSSLPOS == zahlungsplan.sysslpos
                                          select p).FirstOrDefault();
                    if (zahlungsplanOutput != null)
                        zahlungsplanOutput = Mapper.Map<ZahlungsplanDto, SLPOS>(zahlungsplan, zahlungsplanOutput);
                    else throw new Exception("Zahlungsplan with id " + zahlungsplan.sysslpos + " not found!");
                }

                ctx.SaveChanges();
                SearchCache.entityChanged("SLPOS");
            }
            return Mapper.Map<SLPOS, ZahlungsplanDto>(zahlungsplanOutput);
        }

		/// <summary>
		/// updates/creates Kreditlinie
		/// </summary>
		/// <param name="kreditlinie"></param>
		/// <returns></returns>
		public KreditlinieDto createOrUpdateKreditlinie (KreditlinieDto kreditlinie)
		{
			KLINIE kreditlinieOutput = null;
			using (DdOlExtended ctx = new DdOlExtended ())
			{
				if (kreditlinie.sysklinie == 0)
				{
					kreditlinieOutput = new KLINIE ();
					kreditlinieOutput = Mapper.Map<KreditlinieDto, KLINIE> (kreditlinie, kreditlinieOutput);
					kreditlinieOutput.SYSKLINIE = 0;

					ctx.AddToKLINIE (kreditlinieOutput);
				}
				else
				{
					kreditlinieOutput = (from p in ctx.KLINIE
										 where p.SYSKLINIE == kreditlinie.sysklinie
										  select p).FirstOrDefault ();
					if (kreditlinieOutput != null)
						kreditlinieOutput = Mapper.Map<KreditlinieDto, KLINIE> (kreditlinie, kreditlinieOutput);
					else
						throw new Exception ("Kreditlinie with id " + kreditlinie.sysklinie + " not found!");
				}

				ctx.SaveChanges ();
				SearchCache.entityChanged ("KLINIE");
			}
			return Mapper.Map<KLINIE, KreditlinieDto> (kreditlinieOutput);
		}


        /// <summary>
        /// updates/creates Fahrzeugbrief
        /// </summary>
        /// <param name="fahrzeugbrief"></param>
        /// <returns></returns>
        public FahrzeugbriefDto createOrUpdateFahrzeugbrief(FahrzeugbriefDto fahrzeugbrief)
        {
            OBBRIEF fahrzeugbriefOutput = null;
            using (DdOlExtended ctx = new DdOlExtended())
            {
                if (fahrzeugbrief.sysobbrief == 0)
                {
                    fahrzeugbriefOutput = new OBBRIEF();
                    fahrzeugbriefOutput = Mapper.Map<FahrzeugbriefDto, OBBRIEF>(fahrzeugbrief, fahrzeugbriefOutput);
                    fahrzeugbriefOutput.SYSOBBRIEF = 0;

                    ctx.AddToOBBRIEF(fahrzeugbriefOutput);


                }
                else
                {
                    fahrzeugbriefOutput = (from p in ctx.OBBRIEF
                                           where p.SYSOBBRIEF == fahrzeugbrief.sysobbrief
                                           select p).FirstOrDefault();
                    if (fahrzeugbriefOutput != null)
                        fahrzeugbriefOutput = Mapper.Map<FahrzeugbriefDto, OBBRIEF>(fahrzeugbrief, fahrzeugbriefOutput);
                    else throw new Exception("Fahrzeugbrief with id " + fahrzeugbrief.sysobbrief + " not found!");
                }

                ctx.SaveChanges();
                SearchCache.entityChanged("OBBRIEF");
            }
            return Mapper.Map<OBBRIEF, FahrzeugbriefDto>(fahrzeugbriefOutput);
        }

        /// <summary>
        /// updates/creates Kalk
        /// </summary>
        /// <param name="kalk"></param>
        /// <returns></returns>
        public KalkDto createOrUpdateKalk(KalkDto kalk)
        {
            KALK kalkOutput = null;
            using (DdOlExtended ctx = new DdOlExtended())
            {
                if (kalk.syskalk == 0)
                {
                    kalkOutput = new KALK();
                    kalkOutput = Mapper.Map<KalkDto, KALK>(kalk, kalkOutput);
                    kalkOutput.SYSKALK = 0;

                    ctx.AddToKALK(kalkOutput);


                }
                else
                {
                    kalkOutput = (from p in ctx.KALK
                                  where p.SYSKALK == kalk.syskalk
                                  select p).FirstOrDefault();
                    if (kalkOutput != null)
                        kalkOutput = Mapper.Map<KalkDto, KALK>(kalk, kalkOutput);
                    else throw new Exception("Kalk with id " + kalk.syskalk + " not found!");
                }

                ctx.SaveChanges();
                SearchCache.entityChanged("KALK");
            }
            return Mapper.Map<KALK, KalkDto>(kalkOutput);
        }

        /// <summary>
        /// updates/creates Person
        /// </summary>
        /// <param name="person"></param>
        /// <returns></returns>
        public PersonDto createOrUpdatePerson(PersonDto person)
        {
            PERSON personOutput = null;
            using (DdOlExtended ctx = new DdOlExtended())
            {
                if (person.sysperson == 0)
                {
                    personOutput = new PERSON();
                    personOutput = Mapper.Map<PersonDto, PERSON>(person, personOutput);
                    personOutput.SYSPERSON = 0;

                    ctx.AddToPERSON(personOutput);


                }
                else
                {
                    personOutput = (from p in ctx.PERSON
                                    where p.SYSPERSON == person.sysperson
                                    select p).FirstOrDefault();
                    if (personOutput != null)
                        personOutput = Mapper.Map<PersonDto, PERSON>(person, personOutput);
                    else throw new Exception("Person with id " + person.sysperson + " not found!");
                }

                ctx.SaveChanges();
                SearchCache.entityChanged("PERSON");
            }
            return Mapper.Map<PERSON, PersonDto>(personOutput);
        }

        /// <summary>
        /// updates/creates expvalar
        /// </summary>
        /// <param name="expval"></param>
        /// <returns></returns>
        public ExpvalDto createOrUpdateExpvalar(ExpvalDto expval)
        {
            using (DdOlExtended ctx = new DdOlExtended())
            {
                if (expval.sysexpval == 0)
                {

                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "val", Value = expval.val });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "dt", Value = expval.crtdate });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "tm", Value = expval.crttime });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysexptyp", Value = expval.sysexptyp });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "area", Value = expval.area });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = expval.sysid });
                    ctx.ExecuteStoreCommand("insert into expvalar (sysexptyp,area,sysid, val,crtdate,crttime) values(:sysexptyp,:area,:sysid,:val,:dt,:tm)", parameters.ToArray());
                }
                else
                {

                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "val", Value = expval.val });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "dt", Value = expval.crtdate });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "tm", Value = expval.crttime });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = expval.sysexpval });
                    ctx.ExecuteStoreCommand("update expvalar set val=:val, crtdate=:dt, crttime=:tm where sysexpval=:id", parameters.ToArray());
                }

                ctx.SaveChanges();
                List<Devart.Data.Oracle.OracleParameter> pars = new List<Devart.Data.Oracle.OracleParameter>();
                pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "area", Value = expval.area });
                pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = expval.sysid });
                return ctx.ExecuteStoreQuery<ExpvalDto>("select * from expvalar where area=:area and sysid=:sysid", pars.ToArray()).FirstOrDefault();
            }
        }
        /// <summary>
        /// updates/creates expval
        /// </summary>
        /// <param name="expval"></param>
        /// <returns></returns>
        public ExpvalDto createOrUpdateExpval(ExpvalDto expval)
        {



            EXPVAL rval = null;

            using (DdOlExtended ctx = new DdOlExtended())
            {
                if (expval.sysexpval == 0)
                {
                    rval = new EXPVAL();
                    rval = Mapper.Map<ExpvalDto, EXPVAL>(expval, rval);
                    rval.SYSEXPVAL = 0;

                    ctx.AddToEXPVAL(rval);
                }
                else
                {
                    rval = (from p in ctx.EXPVAL
                            where p.SYSEXPVAL == expval.sysexpval
                            select p).FirstOrDefault();
                    if (rval != null)
                        rval = Mapper.Map<ExpvalDto, EXPVAL>(expval, rval);
                    else throw new Exception("expval with id " + expval.sysexpval + " not found!");
                }
                rval.SYSEXPTYP = expval.sysexptyp;

                ctx.SaveChanges();
            }
            return Mapper.Map<EXPVAL, ExpvalDto>(rval);


        }

        /// <summary>
        /// updates/creates eaihot
        /// </summary>
        /// <param name="eaihot"></param>
        /// <returns></returns>
        public EaihotDto createOrUpdateEaihot(EaihotDto eaihot)
        {

            EAIHOT rval = null;

            using (DdOwExtended ctx = new DdOwExtended())
            {
                if (eaihot.syseaihot == 0)
                {
                    rval = new EAIHOT();
                    rval = Mapper.Map<EaihotDto, EAIHOT>(eaihot, rval);
                    rval.SYSEAIHOT = 0;

                    ctx.AddToEAIHOT(rval);
                }
                else
                {
                    rval = (from p in ctx.EAIHOT
                            where p.SYSEAIHOT == eaihot.syseaihot
                            select p).FirstOrDefault();
                    if (rval != null)
                        rval = Mapper.Map<EaihotDto, EAIHOT>(eaihot, rval);
                    else throw new Exception("eaihot with id " + eaihot.syseaihot + " not found!");
                }
                if (eaihot.syseaiart != null && eaihot.syseaiart.HasValue && eaihot.syseaiart.Value > 0)
                    rval.EAIARTReference.EntityKey = ctx.getEntityKey(typeof(EAIART), eaihot.syseaiart.Value);
                else
                {
                    var CurrentEaiArt = (from EaiArt in ctx.EAIART
                                         where EaiArt.CODE.ToUpper().Equals(eaihot.code.ToUpper())
                                         select EaiArt).FirstOrDefault();

                    rval.EAIART = CurrentEaiArt;
                }
                
                ctx.SaveChanges();
            }
            return Mapper.Map<EAIHOT, EaihotDto>(rval);
        }

		/// <summary>
		/// updates/creates Pread datarow (gelesen/ungelesen) (rh 20170515)
		/// </summary>
		/// <param name="pread"></param>
		/// <returns></returns>
		public PreadDto createOrUpdatePread (PreadDto pread)
		{
			//using (DdOlExtended ctx = new DdOlExtended())
			//{
			//	if (preadflag.getEntityId() == 0)
			//	{
			//		ctx.ExecuteStoreCommand("INSERT INTO PREAD (area,sysid,flagread) VALUES('BPLISTENER'," + preadflag.sysID + "," + preadflag.preadFlag + ")", null);
			//	}
			//	else
			//	{
			//		ctx.ExecuteStoreCommand("UPDATE PREAD SET flagread=" + preadflag.preadFlag  + " WHERE SYSID=" + preadflag.getEntityId(), null);
			//	}
			//}

			//return getPreadFlagDetails (preadflag.sysID);

			using (DdOlExtended ctx = new DdOlExtended ())
			{
				// ZUERST schaun, ob diese sysID schon vorhanden ist:
				List<Devart.Data.Oracle.OracleParameter> parsPre = new List<Devart.Data.Oracle.OracleParameter> ();
				parsPre.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = pread.sysID });
				////////PreadDto objResult = ctx.ExecuteStoreQuery<PreadDto> ("SELECT * FROM PREAD WHERE sysid=:sysid", parsPre.ToArray ()).FirstOrDefault ();
				long? objResult = ctx.ExecuteStoreQuery<long?> ("SELECT 1 FROM PREAD WHERE sysid=:sysid", parsPre.ToArray ()).FirstOrDefault ();

				if (objResult == null)
				{
					List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter> ();
					parameters.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = pread.sysID });
					parameters.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = "readflag", Value = pread.preadFlag });
					ctx.ExecuteStoreCommand ("INSERT INTO PREAD (area,sysid,flagread) VALUES('BPLISTENER',:sysid,:readflag)", parameters.ToArray ());
				}
				else
				{
					List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter> ();
					parameters.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = "readflag", Value = pread.preadFlag });
					parameters.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = pread.sysID });
					ctx.ExecuteStoreCommand ("UPDATE PREAD SET flagread=:readflag WHERE SYSID=:sysid", parameters.ToArray ());
				}

				ctx.SaveChanges ();
				
				List<Devart.Data.Oracle.OracleParameter> pars = new List<Devart.Data.Oracle.OracleParameter> ();
				pars.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = pread.sysID });
				return ctx.ExecuteStoreQuery<PreadDto> ("SELECT * FROM PREAD WHERE sysid=:sysid", pars.ToArray ()).FirstOrDefault ();
			}


			//PREAD rval = null;

			//using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new Cic.OpenOne.Common.Model.Prisma.PrismaExtended ())
			//// using (DdOlExtended ctx = new DdOlExtended())
			//{
			//	if (rval.SYSID == 0)
			//	{
			//		rval = new PREAD ();
			//		rval = Mapper.Map<PreadDto, PREAD> (pread, rval);
			//		rval.SYSID = 0;

			//		ctx.AddToPREAD (rval);

			//	}
			//	else
			//	{
			//		// rval = (from PREAD in ctx.PREAD	where rval.SYSID == pread.entityId select PREAD).FirstOrDefault ();

			//		if (rval != null)
			//			rval = Mapper.Map<PreadDto, PREAD> (pread, rval);
			//		else
			//			throw new Exception ("PREAD with id " + pread.entityId + " not found!");
			//	}

			//	ctx.SaveChanges ();
			//	SearchCache.entityChanged ("PREAD");

			//}

			//return Mapper.Map<PREAD, PreadDto> (rval);
 		}


        /// updates/creates Finanzierung
        /// </summary>
        /// <param name="finanzierung"></param>
        /// <returns></returns>
        public FinanzierungDto createOrUpdateFinanzierung(FinanzierungDto finanzierung, int saveMode)
        {
            if(saveMode==0)
            {
                NKK finanzierungOutput = null;
                using (DdOlExtended ctx = new DdOlExtended())
                {
                    if (finanzierung.SYSOB == 0)
                    {
                        finanzierungOutput = new NKK();
                        finanzierungOutput = Mapper.Map<FinanzierungDto, NKK>(finanzierung, finanzierungOutput);
                        finanzierungOutput.SYSNKK = 0;

                        ctx.AddToNKK(finanzierungOutput);
                        SearchCache.entityChanged("NKK");

                    }
                    else
                    {
                        finanzierungOutput = (from p in ctx.NKK
                                            where p.SYSNKK == finanzierung.sysnkk
                                            select p).FirstOrDefault();
                        if (finanzierungOutput != null)
                            finanzierungOutput = Mapper.Map<FinanzierungDto, NKK>(finanzierung, finanzierungOutput);
                        else throw new Exception("finanzierung with id " + finanzierung.sysnkk + " not found!");
                    }

                    ctx.SaveChanges();
                }
                return Mapper.Map<NKK, FinanzierungDto>(finanzierungOutput);
            }
            if (saveMode == 2)//HCE
            {
                try
                {

                    EAIHOT eaihotInput = new EAIHOT();
                    eaihotInput.CODE = "HEK_FO_ORDERTYPE_CHANGE";
                    eaihotInput.OLTABLE = "NKK";
                    eaihotInput.SYSOLTABLE = finanzierung.sysnkk;
                    eaihotInput.PROZESSSTATUS = 0;
                    eaihotInput.HOSTCOMPUTER = "*";
                    eaihotInput.CLIENTART = 0;
                    eaihotInput.EVE = 0;
                    eaihotInput.SUBMITDATE = DateTimeHelper.DateTimeToClarionDate(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                    eaihotInput.SUBMITTIME = DateTimeHelper.DateTimeToClarionTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                    using (DdOwExtended owCtx = new DdOwExtended())
                    {
                        owCtx.AddToEAIHOT(eaihotInput);
                        eaihotInput.EAIART = (from EaiArt in owCtx.EAIART
                                              where EaiArt.CODE == eaihotInput.CODE
                                              select EaiArt).FirstOrDefault();

                        owCtx.SaveChanges();
                        long syseaihot = eaihotInput.SYSEAIHOT;



                        List<EAIQIN> eaiqins = new List<EAIQIN>();
                        eaiqins.Add(createEaiQin(owCtx.getEntityKey(typeof(EAIHOT), syseaihot), "system.syshd", "" + sysWfuser));

                        eaiqins.Add(createEaiQin(owCtx.getEntityKey(typeof(EAIHOT), syseaihot), "customeraccount.sysnkk", "" + finanzierung.sysnkk));
                        eaiqins.Add(createEaiQin(owCtx.getEntityKey(typeof(EAIHOT), syseaihot), "system.changereason", finanzierung.versandgrund));
                        eaiqins.Add(createEaiQin(owCtx.getEntityKey(typeof(EAIHOT), syseaihot), "system.shipmenttype", finanzierung.versandart));
                        eaiqins.Add(createEaiQin(owCtx.getEntityKey(typeof(EAIHOT), syseaihot), "system.sysperson", "" + finanzierung.sysversandperson));

                        foreach (EAIQIN eaiqinInput in eaiqins)
                        {
                            owCtx.AddToEAIQIN(eaiqinInput);
                        }

                        eaihotInput.EVE = 1;
                        owCtx.SaveChanges();

                        DateTime oldDate = DateTime.Now;
                        TimeSpan timeOut = new TimeSpan(0, 0, 1, 0);
                        EaihotDto eaihotOutput = getEaihot(eaihotInput.SYSEAIHOT);

                        while (eaihotOutput.prozessstatus != (int)EaiHotStatusConstants.Ready && !isTimeOut(oldDate, timeOut))
                        {
                            eaihotOutput = getEaihot(eaihotInput.SYSEAIHOT);
                            System.Threading.Thread.Sleep(500);
                        }
                        if (eaihotOutput.prozessstatus == (int)EaiHotStatusConstants.Ready)
                        {
                            finanzierung.bezeichnung = eaihotOutput.outputparameter1;//Statusmeldung für GUI-USER
                        }
                        else
                        {
                            //TIMEOUT EAI, könnte aber später noch verarbeitet werden!
                            finanzierung.bezeichnung = "Die Beauftragung kann aufgrund eines technischen Fehlers nicht durchgeführt werden. Bitte nehmen Sie Kontakt mit der IT-Serviceline auf.";
                        }
                        //falls eine Meldung kommt diese über eine ServiceBaseException zurückliefern
                        if (finanzierung.bezeichnung != null && finanzierung.bezeichnung.Length > 0)
                        {
                            throw new ServiceBaseException("FIN-0001",finanzierung.bezeichnung,Cic.OpenOne.Common.DTO.MessageType.Warn);
                        }
                    }

                }
                catch (Exception e)
                {
                    _log.Info("Processing NKK data delivered: "+e.Message,e);
                    finanzierung.bezeichnung = e.Message;
                    //throw e;

                }
            }
         /*              DateTime oldDate = Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null);
                        TimeSpan timeOut = new TimeSpan(0, 0, 1, 0);
                        EaihotDto eaihotOutput = getEaihot(eaihotInput.SYSEAIHOT);

                        while (eaihotOutput.prozessstatus != (int)EaiHotStatusConstants.Ready && !isTimeOut(oldDate, timeOut))
                        {
                            eaihotOutput = getEaihot(eaihotInput.SYSEAIHOT);
                            System.Threading.Thread.Sleep(500);
                        }

                    }

                }
                catch (Exception e)
                {
                    _log.Error("Error processing NKK data ", e);

                }
            }*/
            return finanzierung;
        }


        /// updates/creates RechnungFaellig
        /// </summary>
        /// <param name="rechnungFaellig"></param>
        /// <returns></returns>
        public RechnungFaelligDto createOrUpdateRechnungFaellig(RechnungFaelligDto rechnungFaellig)
        {

            RN rnOutput = null;
            using (DdOlExtended ctx = new DdOlExtended())
            {
                if (rechnungFaellig.SYSID == 0)
                {
                    rnOutput = new RN();
                    rnOutput = Mapper.Map<RechnungFaelligDto, RN>(rechnungFaellig, rnOutput);
                    rnOutput.SYSRN = 0;

                    ctx.AddToRN(rnOutput);


                }
                else
                {
                    rnOutput = (from rn in ctx.RN
                                where rn.SYSRN == rechnungFaellig.SYSID
                                select rn).FirstOrDefault();
                    if (rnOutput != null)
                        rnOutput = Mapper.Map<RechnungFaelligDto, RN>(rechnungFaellig, rnOutput);
                    else throw new Exception("RN with id " + rechnungFaellig.SYSID + " not found!");
                }

                ctx.SaveChanges();
                SearchCache.entityChanged("RN");
            }
            return Mapper.Map<RN, RechnungFaelligDto>(rnOutput);
        }

        /// updates/creates Tilgung
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public TilgungDto createOrUpdateTilgung(TilgungDto tilgung)
        {

            SLPOS tilgungOutput = null;
            using (DdOlExtended ctx = new DdOlExtended())
            {
                if (tilgung.SYSSLPOS == 0)
                {
                    tilgungOutput = new SLPOS();
                    tilgungOutput = Mapper.Map<TilgungDto, SLPOS>(tilgung, tilgungOutput);
                    tilgungOutput.TILGUNG = 0;

                    ctx.AddToSLPOS(tilgungOutput);


                }
                else
                {
                    tilgungOutput = (from slpos in ctx.SLPOS
                                     where slpos.SYSSLPOS == tilgung.SYSSLPOS
                                     select slpos).FirstOrDefault();
                    if (tilgungOutput != null)
                        tilgungOutput = Mapper.Map<TilgungDto, SLPOS>(tilgung, tilgungOutput);
                    else throw new Exception("SLPOS with id " + tilgung.SYSSLPOS + " not found!");
                }

                ctx.SaveChanges();
                SearchCache.entityChanged("SLPOS");
            }
            return Mapper.Map<SLPOS, TilgungDto>(tilgungOutput);
        }

        /// updates/creates Objekt
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public ObDto createOrUpdateOb(ObDto objekt)
        {
            ObDto rval;
            OB objektOutput = null;
            using (DdOlExtended ctx = new DdOlExtended())
            {
                if (objekt.sysOb == 0)
                {
                    objektOutput = new OB();
                    objektOutput = Mapper.Map<ObDto, OB>(objekt, objektOutput);
                    objektOutput.SYSOB = 0;

                    ctx.AddToOB(objektOutput);


                }
                else
                {
                    objektOutput = (from p in ctx.OB
                                    where p.SYSOB == objekt.sysOb
                                    select p).FirstOrDefault();
                    if (objektOutput != null)
                        objektOutput = Mapper.Map<ObjektDto, OB>(objekt, objektOutput);
                    else throw new Exception("Objekt with id " + objekt.sysOb + " not found!");
                }

                ctx.SaveChanges();
                SearchCache.entityChanged("OB");
                rval = Mapper.Map<OB, ObDto>(objektOutput);


                if (objekt.ausstattungen != null)
                {
                    rval.ausstattungen = new List<ObjektAustDto>();
                    //search for all current entries for this angebot
                    List<Devart.Data.Oracle.OracleParameter> austpar = new List<Devart.Data.Oracle.OracleParameter>();
                    austpar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysob", Value = rval.sysOb });
                    List<long> allAust = ctx.ExecuteStoreQuery<long>(QUERYOBAUSTID, austpar.ToArray()).ToList();
                    //remember really saved austs
                    List<long> savedAust = new List<long>();
                    foreach (ObjektAustDto aust in objekt.ausstattungen)
                    {
                        aust.sysob = rval.sysOb;
                        ObjektAustDto updAust = createOrUpdateObaust(aust);
                        savedAust.Add(updAust.sysobaust);
                        rval.ausstattungen.Add(updAust);
                    }
                    //delete items no more present 
                    List<long> delAust = allAust.Except(savedAust).ToList();
                    if (delAust.Count > 0)
                    {
                        String ids = String.Join(",", delAust);
                        ctx.ExecuteStoreCommand("delete from obaust where sysobaust in (" + ids + ")", null);
                    }

                }

            }

            return rval;
        }

        private EAIQIN createEaiQin(System.Data.EntityKey syseaihot, string value, string data)
        {
            EAIQIN eaiqinInput = new EAIQIN();
            eaiqinInput.EAIHOTReference.EntityKey = syseaihot;
            eaiqinInput.F01 = value;
            eaiqinInput.F02 = data;
            return eaiqinInput;


        }

        /// updates/creates Objekt
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public ObDto createOrUpdateHEKOb(ObDto objekt)
        {
            try
            {
                String vin = objekt.serie.ToUpper();

                EAIHOT eaihotInput = new EAIHOT();
                eaihotInput.CODE = "HEK_FO_USED_CAR_FIN";
                eaihotInput.OLTABLE = "SYSTEM";
                eaihotInput.SYSOLTABLE = 0;
                eaihotInput.PROZESSSTATUS = 0;
                eaihotInput.HOSTCOMPUTER = "*";
                eaihotInput.CLIENTART = 0;
                eaihotInput.EVE = 0;
                eaihotInput.SUBMITDATE = DateTimeHelper.DateTimeToClarionDate(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                eaihotInput.SUBMITTIME = DateTimeHelper.DateTimeToClarionTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                using (DdOwExtended owCtx = new DdOwExtended())
                {

                     List<Devart.Data.Oracle.OracleParameter> pars = new List<Devart.Data.Oracle.OracleParameter>();
                    pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "bez", Value = vin });

                    long available = owCtx.ExecuteStoreQuery<long>("select count(*) from nkk where zustand in ('gebucht','aktiv','angefragt') and bezeichnung=:bez", pars.ToArray()).FirstOrDefault();
                    if(available>0)
                    {
                        throw new Exception("Es ist bereits ein Fahrzeug mit der eingegebenen FIN vorhanden. Eine Beantragung ist daher nicht möglich. Bitte ändern Sie die FIN um den Vorgang fortzusetzen");
                    }
                    owCtx.AddToEAIHOT(eaihotInput);
                    eaihotInput.EAIART = (from EaiArt in owCtx.EAIART
                                          where EaiArt.CODE == eaihotInput.CODE
                                          select EaiArt).FirstOrDefault();

                    owCtx.SaveChanges();
                    long syseaihot = eaihotInput.SYSEAIHOT;



                    List<EAIQIN> eaiqins = new List<EAIQIN>();
                    eaiqins.Add(createEaiQin(owCtx.getEntityKey(typeof(EAIHOT), syseaihot), "system.syshd",""+sysWfuser));

                    eaiqins.Add(createEaiQin(owCtx.getEntityKey(typeof(EAIHOT), syseaihot), "car.schwacke", objekt.schwacke));
                    eaiqins.Add(createEaiQin(owCtx.getEntityKey(typeof(EAIHOT), syseaihot), "car.hersteller", objekt.hersteller));
                    eaiqins.Add(createEaiQin(owCtx.getEntityKey(typeof(EAIHOT), syseaihot), "car.fabrikat", objekt.modell));
                    eaiqins.Add(createEaiQin(owCtx.getEntityKey(typeof(EAIHOT), syseaihot), "car.typ", objekt.fabrikat));

                    String tmpVar = "0000";
                    if (objekt.baujahr.HasValue)
                        tmpVar = String.Format("{0:yyyy}", objekt.baujahr.Value);
                    if (long.Parse(tmpVar) > 2100)
                        tmpVar = "0000";
                    eaiqins.Add(createEaiQin(owCtx.getEntityKey(typeof(EAIHOT), syseaihot), "car.baujahr", tmpVar));
                    eaiqins.Add(createEaiQin(owCtx.getEntityKey(typeof(EAIHOT), syseaihot), "car.vin", vin));

                    tmpVar = "";
                    if (objekt.erstzul.HasValue)
                        tmpVar = String.Format("{0:yyyy-MM-dd}", objekt.erstzul.Value);
                    eaiqins.Add(createEaiQin(owCtx.getEntityKey(typeof(EAIHOT), syseaihot), "car.erstzul", tmpVar));

                    eaiqins.Add(createEaiQin(owCtx.getEntityKey(typeof(EAIHOT), syseaihot), "car.kmstand", objekt.ubnahmekm.ToString(CultureInfo.InvariantCulture)));

                    tmpVar = "";
                    if (objekt.rechnung.HasValue)
                        tmpVar = String.Format("{0:yyyy-MM-dd}", objekt.rechnung.Value);
                    eaiqins.Add(createEaiQin(owCtx.getEntityKey(typeof(EAIHOT), syseaihot), "invoice.rndatum", tmpVar));

                    
                    eaiqins.Add(createEaiQin(owCtx.getEntityKey(typeof(EAIHOT), syseaihot), "invoice.rnbetrag", objekt.grund.ToString(CultureInfo.InvariantCulture)));
                    eaiqins.Add(createEaiQin(owCtx.getEntityKey(typeof(EAIHOT), syseaihot), "car.fzart", objekt.fzart));
                    eaiqins.Add(createEaiQin(owCtx.getEntityKey(typeof(EAIHOT), syseaihot), "car.fztyp", objekt.typ));
                    eaiqins.Add(createEaiQin(owCtx.getEntityKey(typeof(EAIHOT), syseaihot), "car.vk", objekt.ahk.ToString(CultureInfo.InvariantCulture)));
                    eaiqins.Add(createEaiQin(owCtx.getEntityKey(typeof(EAIHOT), syseaihot), "car.kw", objekt.kw.ToString(CultureInfo.InvariantCulture)));
                    eaiqins.Add(createEaiQin(owCtx.getEntityKey(typeof(EAIHOT), syseaihot), "car.ps",  objekt.ps.ToString(CultureInfo.InvariantCulture)));

                    tmpVar = "";
                    if (objekt.zustandam.HasValue)
                        tmpVar = String.Format("{0:yyyy-MM-dd}", objekt.zustandam.Value);
                    eaiqins.Add(createEaiQin(owCtx.getEntityKey(typeof(EAIHOT), syseaihot), "car.deregistration.stillegung", tmpVar));

                    eaiqins.Add(createEaiQin(owCtx.getEntityKey(typeof(EAIHOT), syseaihot), "car.anzahl", objekt.anzahl.ToString(CultureInfo.InvariantCulture)));
                    eaiqins.Add(createEaiQin(owCtx.getEntityKey(typeof(EAIHOT), syseaihot), "car.brief", objekt.standortbrief));
                    eaiqins.Add(createEaiQin(owCtx.getEntityKey(typeof(EAIHOT), syseaihot), "car.damage.wert", objekt.sonzub.ToString(CultureInfo.InvariantCulture)));
                    eaiqins.Add(createEaiQin(owCtx.getEntityKey(typeof(EAIHOT), syseaihot), "car.specification", objekt.gehnid));
                    eaiqins.Add(createEaiQin(owCtx.getEntityKey(typeof(EAIHOT), syseaihot), "car.erinklmwst", objekt.importer.ToString(CultureInfo.InvariantCulture)));

                    eaiqins.Add(createEaiQin(owCtx.getEntityKey(typeof(EAIHOT), syseaihot), "car.syshd", getSysWfuser().ToString()));
                    eaiqins.Add(createEaiQin(owCtx.getEntityKey(typeof(EAIHOT), syseaihot), "car.schwackerw", objekt.rw.ToString(CultureInfo.InvariantCulture)));

                    int r=1;
                    foreach (ObjektAustDto aust in objekt.ausstattungen)
                    {
                        eaiqins.Add(createEaiQin(owCtx.getEntityKey(typeof(EAIHOT), syseaihot), "car.extras.rang_"+r, ""+r));
                        eaiqins.Add(createEaiQin(owCtx.getEntityKey(typeof(EAIHOT), syseaihot), "car.extras.beschreibung_"+r, aust.beschreibung));
                        eaiqins.Add(createEaiQin(owCtx.getEntityKey(typeof(EAIHOT), syseaihot), "car.extras.preis_"+r, aust.betrag.ToString(CultureInfo.InvariantCulture)));
                        r++;
                    }
                    
                    foreach (EAIQIN eaiqinInput in eaiqins)
                    {
                        owCtx.AddToEAIQIN(eaiqinInput);
                    }

                    eaihotInput.EVE = 1;
                    owCtx.SaveChanges();

                    DateTime oldDate = DateTime.Now;
                    TimeSpan timeOut = new TimeSpan(0, 0, 1, 0);
                    EaihotDto eaihotOutput = getEaihot(eaihotInput.SYSEAIHOT);

                    while (eaihotOutput.prozessstatus != (int)EaiHotStatusConstants.Ready && !isTimeOut(oldDate, timeOut))
                    {
                        eaihotOutput = getEaihot(eaihotInput.SYSEAIHOT);
                        System.Threading.Thread.Sleep(500);
                    }
                    if (eaihotOutput.prozessstatus == (int)EaiHotStatusConstants.Ready)
                    {
                        objekt.source = eaihotOutput.outputparameter2;
                        objekt.source2 = eaihotOutput.outputparameter3;
                    }
                    else
                    {
                        objekt.source = "TIMEOUT";
                    }
                    if(objekt.source!=null && objekt.source.Length>0)
                    {
                        throw new Exception(objekt.source);
                    }
                }
                
            }
            catch (Exception e)
            {
                _log.Info("Processing HEK data delivered "+e.Message,e);
                objekt.source = e.Message;
                throw e;

            }
            return objekt;
        }


        /// <summary>
        /// Bestehendes Eaihot holen
        /// </summary>
        /// <param name="syseaihot">Primary Key</param>
        /// <returns>Eaihot Ausgang</returns>
        public EaihotDto getEaihot(long syseaihot)
        {
            EaihotDto rval = null;
            using (DdOwExtended owCtx = new DdOwExtended())
            {
                DbConnection con = (owCtx.Connection as EntityConnection).StoreConnection;
                rval = con.Query<EaihotDto>("select * from eaihot where syseaihot=" + syseaihot).FirstOrDefault();
            }
            return rval;
        }

        /// <summary>
        /// Returns the Eaihot WITH File-Data
        /// </summary>
        /// <param name="syseaihot"></param>
        /// <returns></returns>
        public EaihotDto getEaihotDetails(long syseaihot)
        {
            using (DdOwExtended ctx = new DdOwExtended())
            {
                DbConnection con = (ctx.Connection as EntityConnection).StoreConnection;
                EaihotDto rval = con.Query<EaihotDto>("select * from eaihot where syseaihot=" + syseaihot).FirstOrDefault();

                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syseaihot", Value = syseaihot });

                rval.files = ctx.ExecuteStoreQuery<EaihfileDto>(QUERYEAIHFILE, parameters.ToArray()).ToList();

                return rval;

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
        /// updates/creates Recalc
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public RecalcDto createOrUpdateRecalc(RecalcDto recalc)
        {
            SearchCache.entityChanged("RECALC");
            return recalc;          
        }

        /// <summary>
        /// updates/creates Mycalc
        /// </summary>
        /// <param name="mycalc"></param>
        /// <returns></returns>
        public MycalcDto createOrUpdateMycalc(MycalcDto mycalc)
        {
            MYCALC rval = null;

            using (DdOlExtended ctx = new DdOlExtended())
            {
                if (mycalc.sysmycalc == 0)
                {
                    rval = new MYCALC();
                    rval = Mapper.Map<MycalcDto, MYCALC>(mycalc, rval);
                    rval.SYSMYCALC = 0;
                    ctx.AddToMYCALC(rval);
                }
                else
                {
                    rval = (from p in ctx.MYCALC
                            where p.SYSMYCALC == mycalc.sysmycalc
                            select p).FirstOrDefault();
                    if (rval != null)
                        rval = Mapper.Map<MycalcDto, MYCALC>(mycalc, rval);
                    else throw new Exception("Mycalc with id " + mycalc.sysmycalc + " not found!");
                }

                rval.ZINSINT = (decimal)mycalc.zins;
                rval.ZINSEFF = mycalc.zins;
                if (mycalc.syskalktyp != 0)
                {
                    
                    KALKTYP kt = (from p in ctx.KALKTYP
                                  where p.SYSKALKTYP == mycalc.syskalktyp
                                  select p).FirstOrDefault();
                    rval.MODUS = kt.MODUS;
                    rval.PPY = kt.PPY;
                    if (!rval.PPY.HasValue || rval.PPY.Value < 1)
                        rval.PPY = 12;
                }
               
                ctx.SaveChanges();


                rval.BEZEICHNUNG = "K" + rval.SYSMYCALC;
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "val", Value = mycalc.zins10 });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = rval.SYSMYCALC });
                ctx.ExecuteStoreCommand("update mycalc set zins10=:val where sysmycalc=:id", parameters.ToArray());



                MycalcDto returnvalue = Mapper.Map<MYCALC, MycalcDto>(rval);
                returnvalue.zins10 = mycalc.zins10;

                if (mycalc.serviceKalkulation != null)
                {
                    mycalc.serviceKalkulation.sysmycalcfs = returnvalue.sysmycalc;//this has the same pkey!
                    returnvalue.serviceKalkulation = createOrUpdateMycalcfs(mycalc.serviceKalkulation);
                }
                returnvalue.equipment = new List<MycalcaustDto>();
                returnvalue.sysangebot = mycalc.sysangebot;

                if (mycalc.equipment != null)
                {
                    //search for all current entries for this angebot
                    List<Devart.Data.Oracle.OracleParameter> austpar = new List<Devart.Data.Oracle.OracleParameter>();
                    austpar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysmycalc", Value = rval.SYSMYCALC });
                    List<long> allAust = ctx.ExecuteStoreQuery<long>(QUERYMYCALCAUSTID, austpar.ToArray()).ToList();
                    //remember really saved austs
                    List<long> savedAust = new List<long>();
                    foreach (MycalcaustDto aust in mycalc.equipment)
                    {
                        aust.sysmycalc = returnvalue.sysmycalc;//link to main entity
                        MycalcaustDto updAust = createOrUpdateMycalcaust(aust);
                        savedAust.Add(updAust.sysobaust);
                        returnvalue.equipment.Add(updAust);
                    }
                    //delete items no more present 
                    List<long> delAust = allAust.Except(savedAust).ToList();
                    if (delAust.Count > 0)
                    {
                        String ids = String.Join(",", delAust);
                        ctx.ExecuteStoreCommand("delete from mycalcaust where sysobaust in (" + ids + ")", null);
                    }

                }


                ctx.SaveChanges();
                SearchCache.entityChanged("MYCALC");
                return returnvalue;
            }

        }

        /// <summary>
        /// updates/creates Mycalcfs
        /// </summary>
        /// <param name="mycalcfs"></param>
        /// <returns></returns>
        public MycalcfsDto createOrUpdateMycalcfs(MycalcfsDto mycalcfs)
        {
            MYCALCFS rval = null;

            using (DdOlExtended ctx = new DdOlExtended())
            {
                rval = (from p in ctx.MYCALCFS
                        where p.SYSMYCALCFS == mycalcfs.sysmycalcfs
                        select p).FirstOrDefault();

                if (rval == null)
                {
                    rval = new MYCALCFS();
                    rval = Mapper.Map<MycalcfsDto, MYCALCFS>(mycalcfs, rval);
                    
                    ctx.AddToMYCALCFS(rval);
                }
                else
                {
                    rval = Mapper.Map<MycalcfsDto, MYCALCFS>(mycalcfs, rval);
                }


                ctx.SaveChanges();
            }
            return Mapper.Map<MYCALCFS, MycalcfsDto>(rval);
        }

        /// <summary>
        /// updates/creates Mycalcaust
        /// </summary>
        /// <param name="mycalcaust"></param>
        /// <returns></returns>
        public MycalcaustDto createOrUpdateMycalcaust(MycalcaustDto mycalcaust)
        {
            MYCALCAUST rval = null;

            using (DdOlExtended ctx = new DdOlExtended())
            {
                if (mycalcaust.sysobaust == 0)
                {
                    rval = new MYCALCAUST();
                    rval = Mapper.Map<MycalcaustDto, MYCALCAUST>(mycalcaust, rval);
                    rval.SYSOBAUST = 0;

                    ctx.AddToMYCALCAUST(rval);
                }
                else
                {
                    rval = (from p in ctx.MYCALCAUST
                            where p.SYSOBAUST == mycalcaust.sysobaust
                            select p).FirstOrDefault();
                    if (rval != null)
                        rval = Mapper.Map<MycalcaustDto, MYCALCAUST>(mycalcaust, rval);
                    else throw new Exception("Mycalcaust with id " + mycalcaust.sysobaust + " not found!");
                }

                ctx.SaveChanges();
            }
            return Mapper.Map<MYCALCAUST, MycalcaustDto>(rval);
        }

        /// updates/creates Rahmen
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public RahmenDto createOrUpdateRahmen(RahmenDto rahmen)
        {

            RVT rahmenOutput = null;
            using (DdOlExtended ctx = new DdOlExtended())
            {
                if (rahmen.sysrvt == 0)
                {
                    rahmenOutput = new RVT();
                    rahmenOutput = Mapper.Map<RahmenDto, RVT>(rahmen, rahmenOutput);
                    rahmenOutput.SYSRVT = 0;
                    
                    ctx.AddToRVT(rahmenOutput);



                }
                else
                {
                    rahmenOutput = (from p in ctx.RVT
                                    where p.SYSRVT == rahmen.sysrvt
                                    select p).FirstOrDefault();
                    if (rahmenOutput != null)
                        rahmenOutput = Mapper.Map<RahmenDto, RVT>(rahmen, rahmenOutput);
                    else throw new Exception("Rahmen with id " + rahmen.sysrvt + " not found!");
                }

                ctx.SaveChanges();
                rahmenOutput.RAHMEN = "R" + rahmenOutput.SYSRVT;
                if (rahmen.sysperson == 0 && rahmen.sysit == 0)
                    rahmenOutput.RAHMEN = "T" + rahmenOutput.SYSRVT;

                RahmenDto rval = Mapper.Map<RVT, RahmenDto>(rahmenOutput);
                rval.positionen = new List<RvtPosDto>();
                if (rahmen.positionen != null)
                    foreach (RvtPosDto rpos in rahmen.positionen)
                    {
                        rpos.sysrvt = rval.sysrvt;//link to main entity
                        rval.positionen.Add(createOrUpdateRahmenPos(rpos));
                    }
                ctx.SaveChanges();
                SearchCache.entityChanged("RVT");
                return rval;
            }


        }

        /// <summary>
        /// creates/updates one rahmenpos
        /// </summary>
        /// <param name="rvtpos"></param>
        /// <returns></returns>
        public RvtPosDto createOrUpdateRahmenPos(RvtPosDto rvtpos)
        {

            RVTPOS rahmenOutput = null;
            using (DdOlExtended ctx = new DdOlExtended())
            {
                if (rvtpos.sysrvtpos == 0)//create
                {
                    rahmenOutput = new RVTPOS();
                    rahmenOutput = Mapper.Map<RvtPosDto, RVTPOS>(rvtpos, rahmenOutput);
                    rahmenOutput.SYSRVTPOS = 0;
                    
                    ctx.AddToRVTPOS(rahmenOutput);
                }
                else if (rvtpos.sysrvtpos < 0)//delete with id <0
                {
                    rahmenOutput = (from p in ctx.RVTPOS
                                    where p.SYSRVTPOS == -1 * rvtpos.sysrvtpos
                                    select p).FirstOrDefault();
                    if (rahmenOutput != null)
                        ctx.DeleteObject(rahmenOutput);

                }
                else//update
                {
                    rahmenOutput = (from p in ctx.RVTPOS
                                    where p.SYSRVTPOS == rvtpos.sysrvtpos
                                    select p).FirstOrDefault();
                    if (rahmenOutput != null)
                        rahmenOutput = Mapper.Map<RvtPosDto, RVTPOS>(rvtpos, rahmenOutput);

                    else throw new Exception("Rahmenpos with id " + rvtpos.sysrvt + " not found!");
                }


                ctx.SaveChanges();

                if (rvtpos.sysabrregel > 0 && rahmenOutput.SYSRVTPOS > 0)
                {
                    //TODO Map EDMX
                    ctx.ExecuteStoreCommand("update RVTPOS set sysabrregel=" + rvtpos.sysabrregel + " where sysrvtpos=" + rahmenOutput.SYSRVTPOS, null);
                }
                if (rahmenOutput != null)
                    rvtpos.sysrvtpos = rahmenOutput.SYSRVTPOS;
                if (rvtpos.sysvstyp > 0)
                {
                    createOrUpdateRvtvs(rvtpos);
                }

            }
            RvtPosDto rval = Mapper.Map<RVTPOS, RvtPosDto>(rahmenOutput);
            rval.sysvstyp = rvtpos.sysvstyp;

            //rval.sysrvt = rvtpos.sysrvt;
            //rval.sysfstyp = rvtpos.sysfstyp;
            return rval;
        }

        /// <summary>
        /// creates/updates one sysvstypid on rahmenpos
        /// </summary>
        /// <param name="rvtpos"></param>
        /// <returns></returns>
        public void createOrUpdateRvtvs(RvtPosDto rvtpos)
        {

            RVTVS rahmenOutput = null;
            using (DdOlExtended ctx = new DdOlExtended())
            {
                long sysrvtvs = ctx.ExecuteStoreQuery<long>("select sysrvtvs from rvtvs where sysrvtpos=" + rvtpos.sysrvtpos, null).FirstOrDefault();

                if (sysrvtvs == 0)
                {
                    rahmenOutput = new RVTVS();
                    
                    ctx.AddToRVTVS(rahmenOutput);
                }
                else
                {
                    rahmenOutput = (from p in ctx.RVTVS
                                    where p.SYSRVTVS == sysrvtvs
                                    select p).FirstOrDefault();
                    
                }
                rahmenOutput.SYSRVTPOS = rvtpos.sysrvtpos;
                rahmenOutput.SYSVSTYP = rvtpos.sysvstyp;

                ctx.SaveChanges();
            }

        }

        /// updates/creates Haendler
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public HaendlerDto createOrUpdateHaendler(HaendlerDto haendler)
        {

            PERSON haendlerOutput = null;
            using (DdOlExtended ctx = new DdOlExtended())
            {
                if (haendler.SYSPERSON == 0)
                {
                    haendlerOutput = new PERSON();
                    haendlerOutput = Mapper.Map<HaendlerDto, PERSON>(haendler, haendlerOutput);
                    haendlerOutput.SYSPERSON = 0;

                    ctx.AddToPERSON(haendlerOutput);


                }
                else
                {
                    haendlerOutput = (from p in ctx.PERSON
                                      where p.SYSPERSON == haendler.SYSPERSON
                                      select p).FirstOrDefault();
                    if (haendlerOutput != null)
                        haendlerOutput = Mapper.Map<HaendlerDto, PERSON>(haendler, haendlerOutput);
                    else throw new Exception("Haendler with id " + haendler.SYSPERSON + " not found!");
                }

                ctx.SaveChanges();
                SearchCache.entityChanged("PERSON");
            }
            return Mapper.Map<PERSON, HaendlerDto>(haendlerOutput);
        }
        /// <summary>
        /// updates/creates Kunde
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public KundeDto createOrUpdateKunde(KundeDto kunde)
        {

            PERSON kundeOutput = null;
            using (DdOlExtended ctx = new DdOlExtended())
            {
                if (kunde.sysperson == 0)
                {
                    kundeOutput = new PERSON();
                    kundeOutput = Mapper.Map<KundeDto, PERSON>(kunde, kundeOutput);
                    kundeOutput.SYSPERSON = 0;

                    ctx.AddToPERSON(kundeOutput);


                }
                else
                {
                    kundeOutput = (from p in ctx.PERSON
                                   where p.SYSPERSON == kunde.sysperson
                                   select p).FirstOrDefault();
                    if (kundeOutput != null)
                        kundeOutput = Mapper.Map<KundeDto, PERSON>(kunde, kundeOutput);
                    else throw new Exception("Kunde with id " + kunde.sysperson + " not found!");
                }

                ctx.SaveChanges();
                SearchCache.entityChanged("PERSON");
            }
            return Mapper.Map<PERSON, KundeDto>(kundeOutput);
        }

        /// <summary>
        /// updates/creates It
        /// </summary>
        /// <param name="sysit"></param>
        /// <returns></returns>
        public ItDto createOrUpdateIt(ItDto it)
        {

            IT itOutput = null;
            using (DdOlExtended ctx = new DdOlExtended())
            {
                if (it.sysit == 0)
                {
                    itOutput = new IT();
                    itOutput = Mapper.Map<ItDto, IT>(it, itOutput);
                    itOutput.SYSIT = 0;
                    ctx.AddToIT(itOutput);


                }
                else
                {
                    itOutput = (from p in ctx.IT
                                where p.SYSIT == it.sysit
                                select p).FirstOrDefault();
                    if (itOutput != null)
                        itOutput = Mapper.Map<ItDto, IT>(it, itOutput);
                    else throw new Exception("It with id " + it.sysit + " not found!");
                }
                
                ctx.SaveChanges();
                SearchCache.entityChanged("IT");
            }
            return Mapper.Map<IT, ItDto>(itOutput);
        }

        /// <summary>
        /// updates/creates Angkalk
        /// </summary>
        /// <param name="syskalk"></param>
        /// <returns></returns>
        public AngkalkDto createOrUpdateAngkalk(AngkalkDto angkalk)
        {

            ANGKALK angkalkOutput = null;

            using (DdOlExtended ctx = new DdOlExtended())
            {
                //ensure usage of only one angkalk for the angebot
                /*if (angkalk.syskalk == 0 && angkalk.sysangebot>0)
                {
                    angkalk.syskalk = ctx.ExecuteStoreQuery<long>("select syskalk from angkalk where sysangebot=" + angkalk.sysangebot, null).FirstOrDefault();
                }*/

                if (angkalk.syskalk == 0)
                {
                    angkalkOutput = new ANGKALK();
                    angkalkOutput = Mapper.Map<AngkalkDto, ANGKALK>(angkalk, angkalkOutput);
                    angkalkOutput.SYSKALK = 0;

                    ctx.AddToANGKALK(angkalkOutput);
                }
                else
                {
                    angkalkOutput = (from p in ctx.ANGKALK
                                     where p.SYSKALK == angkalk.syskalk
                                     select p).FirstOrDefault();
                    if (angkalkOutput != null)
                        angkalkOutput = Mapper.Map<AngkalkDto, ANGKALK>(angkalk, angkalkOutput);
                    else throw new Exception("Angkalk with id " + angkalk.syskalk + " not found!");
                }
                if (angkalk.sysprproduct > 0)
                {
                    angkalkOutput.SYSPRPRODUCT= angkalk.sysprproduct;
                }

                ctx.SaveChanges();
                AngkalkDto rval = Mapper.Map<ANGKALK, AngkalkDto>(angkalkOutput);
                rval.syskalk = angkalkOutput.SYSKALK;

                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysangkalk", Value = angkalkOutput.SYSKALK });
                List<SlDto> orgZp = ctx.ExecuteStoreQuery<SlDto>(QUERYSLPOS, parameters.ToArray()).ToList();
                //delete all slpos
                //delete all sl
                //delete all sllink
                if (orgZp.Count > 0)
                {
                    long[] delIds = (from z in orgZp
                                     select z.syssl).ToArray();

                    ctx.ExecuteStoreCommand("delete from slpos where syssl in (" + String.Join(",", delIds) + ")", null);
                    ctx.ExecuteStoreCommand("delete from sl where syssl in (" + String.Join(",", delIds) + ")", null);
                    ctx.ExecuteStoreCommand("delete from sllink where syssl in (" + String.Join(",", delIds) + ")", null);
                }
                if (angkalk.zahlplan != null && angkalk.zahlplan.Count > 0)
                {
                    rval.zahlplan = new List<SlDto>();

                    foreach (SlDto zp in angkalk.zahlplan)
                    {

                        if (zp.syssllink == 0)//create sllink if not yet there
                        {
                            SL sl = new SL();
                            sl.SYSSLTYP = zp.syssltyp;//must be set by gui
                            
                            ctx.AddToSL(sl);
                            ctx.SaveChanges();

                            SLLINK sllink = new SLLINK();
                            sllink.GEBIET = "ANGKALK";
                            sllink.SYSID = angkalkOutput.SYSKALK;
                            sllink.SYSSL = sl.SYSSL;
                            
                            ctx.AddToSLLINK(sllink);
                            ctx.SaveChanges();


                            zp.syssllink = sllink.SYSSLLINK;
                            zp.syssl = sl.SYSSL;
                        }

                        rval.zahlplan.Add(createOrUpdateSl(ctx, zp));
                    }


                }

                ctx.SaveChanges();
                //TODO Map EDMX
                angkalk.bezeichnung = "K" + rval.syskalk;
                List<Devart.Data.Oracle.OracleParameter> upar = new List<Devart.Data.Oracle.OracleParameter>();
                upar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syskalk", Value = rval.syskalk });
                upar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "valuta", Value = angkalk.valutaa });
                upar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "bez", Value = angkalk.bezeichnung });
                ctx.ExecuteStoreCommand("update ANGKALK set valutaa=:valuta, bezeichnung=:bez where syskalk=:syskalk", upar.ToArray());
                rval.valutaa = angkalk.valutaa;
                rval.bezeichnung = angkalk.bezeichnung;
                SearchCache.entityChanged("ANGKALK");

                return rval;
            }

        }

        /// <summary>
        /// updates/creates Antkalk
        /// </summary>
        /// <param name="syskalk"></param>
        /// <returns></returns>
        public AntkalkDto createOrUpdateAntkalk(AntkalkDto antkalk)
        {

            ANTKALK angkalkOutput = null;

            using (DdOlExtended ctx = new DdOlExtended())
            {
                if (antkalk.syskalk == 0)
                {
                    angkalkOutput = new ANTKALK();
                    angkalkOutput = Mapper.Map<AntkalkDto, ANTKALK>(antkalk, angkalkOutput);
                    angkalkOutput.SYSKALK = 0;

                    ctx.AddToANTKALK(angkalkOutput);


                }
                else
                {

                    angkalkOutput = (from p in ctx.ANTKALK
                                     where p.SYSKALK == antkalk.syskalk
                                     select p).FirstOrDefault();
                    if (angkalkOutput != null)
                        angkalkOutput = Mapper.Map<AntkalkDto, ANTKALK>(antkalk, angkalkOutput);
                    else if(antkalk.syskalk>0)
                    {
                        angkalkOutput = new ANTKALK();
                        angkalkOutput = Mapper.Map<AntkalkDto, ANTKALK>(antkalk, angkalkOutput);
                        angkalkOutput.SYSKALK = antkalk.syskalk;

                        ctx.AddToANTKALK(angkalkOutput);
                    } else 
                        throw new Exception("Antkalk with id " + antkalk.syskalk + " not found!");
                }
                if (antkalk.sysprproduct > 0)
                {
                    angkalkOutput.SYSPRPRODUCT= antkalk.sysprproduct;
                }

                ctx.SaveChanges();
                AntkalkDto rval = Mapper.Map<ANTKALK, AntkalkDto>(angkalkOutput);
                rval.syskalk = angkalkOutput.SYSKALK;

                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysantkalk", Value = angkalkOutput.SYSKALK });
                List<SlDto> orgZp = ctx.ExecuteStoreQuery<SlDto>(QUERYSLPOSANT, parameters.ToArray()).ToList();
                //delete all slpos
                //delete all sl
                //delete all sllink
                if (orgZp.Count > 0)
                {
                    long[] delIds = (from z in orgZp
                                     select z.syssl).ToArray();

                    ctx.ExecuteStoreCommand("delete from slpos where syssl in (" + String.Join(",", delIds) + ")", null);
                    ctx.ExecuteStoreCommand("delete from sl where syssl in (" + String.Join(",", delIds) + ")", null);
                    ctx.ExecuteStoreCommand("delete from sllink where syssl in (" + String.Join(",", delIds) + ")", null);
                }
                if (antkalk.zahlplan != null && antkalk.zahlplan.Count > 0)
                {
                    rval.zahlplan = new List<SlDto>();

                    foreach (SlDto zp in antkalk.zahlplan)
                    {

                        if (zp.syssllink == 0)//create sllink if not yet there
                        {
                            SL sl = new SL();
                            sl.SYSSLTYP = zp.syssltyp;
                            ctx.AddToSL(sl);
                            ctx.SaveChanges();

                            SLLINK sllink = new SLLINK();
                            sllink.GEBIET = "ANTKALK";
                            sllink.SYSID = angkalkOutput.SYSKALK;
                            sllink.SYSSL = sl.SYSSL;
                            ctx.AddToSLLINK(sllink);
                            ctx.SaveChanges();


                            zp.syssllink = sllink.SYSSLLINK;
                            zp.syssl = sl.SYSSL;
                        }

                        rval.zahlplan.Add(createOrUpdateSl(ctx, zp));
                    }


                }

                ctx.SaveChanges();
                //TODO Map EDMX
                //antkalk.bezeichnung = "K" + rval.syskalk;
                List<Devart.Data.Oracle.OracleParameter> upar = new List<Devart.Data.Oracle.OracleParameter>();
                upar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syskalk", Value = rval.syskalk });
                upar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "valuta", Value = antkalk.valutaa });
                upar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "bez", Value = antkalk.bezeichnung });
                ctx.ExecuteStoreCommand("update ANTKALK set valutaa=:valuta, bezeichnung=:bez where syskalk=:syskalk", upar.ToArray());
                rval.valutaa = antkalk.valutaa;
                rval.bezeichnung = antkalk.bezeichnung;
                SearchCache.entityChanged("ANTKALK");

                return rval;
            }

        }

        /// <summary>
        /// updates/creates Zahlplanpos
        /// for faster loops has to be commited outside this method
        /// </summary>
        /// <param name="sl"></param>
        /// <returns></returns>
        public SlDto createOrUpdateSl(DdOlExtended ctx, SlDto sl)
        {
            SLPOS rval = null;

            // using (DdOlExtended ctx = new DdOlExtended())
            {
                if (sl.sysslpos == 0)
                {
                    rval = new SLPOS();
                    rval = Mapper.Map<SlDto, SLPOS>(sl, rval);
                    rval.SYSSLPOS = 0;

                    ctx.AddToSLPOS(rval);
                }
                else
                {
                    rval = (from p in ctx.SLPOS
                            where p.SYSSLPOS == sl.sysslpos
                            select p).FirstOrDefault();
                    if (rval != null)
                        rval = Mapper.Map<SlDto, SLPOS>(sl, rval);
                    else throw new Exception("sl with id " + sl.sysslpos + " not found!");
                }
                
                rval.SYSSL = sl.syssl;
                
                // ctx.SaveChanges();
            }
            SlDto rv = Mapper.Map<SLPOS, SlDto>(rval);
            rv.syssltyp = sl.syssltyp;
            return rv;
        }

        /// <summary>
        /// updates/creates Angob
        /// </summary>
        /// <param name="sysangob"></param>
        /// <returns></returns>
        public AngobDto createOrUpdateAngob(AngobDto angob)
        {

            ANGOB angobOutput = null;
            using (DdOlExtended ctx = new DdOlExtended())
            {
                if (angob.fabrikat != null && angob.fabrikat.Length > 60)
                    angob.fabrikat = angob.fabrikat.Substring(0, 60);
                if (angob.typ != null && angob.typ.Length > 60)
                    angob.typ = angob.typ.Substring(0, 60);
                if (angob.eek != null && angob.eek.Length > 10)
                    angob.eek = angob.eek.Substring(0, 10);
                if (angob.sysOb == 0)
                {
                    angobOutput = new ANGOB();
                    angobOutput = Mapper.Map<AngobDto, ANGOB>(angob, angobOutput);
                    angobOutput.SYSOB = 0;

                    ctx.AddToANGOB(angobOutput);


                }
                else
                {
                    angobOutput = (from p in ctx.ANGOB
                                   where p.SYSOB == angob.sysOb
                                   select p).FirstOrDefault();
                    if (angobOutput != null)
                        angobOutput = Mapper.Map<AngobDto, ANGOB>(angob, angobOutput);
                    else throw new Exception("Angob with id " + angob.sysOb + " not found!");
                }

                

                if (angob.zusatzdaten != null && angob.zusatzdaten.erstzul != null)
                {
                    angob.baujahr = angob.zusatzdaten.erstzul;
                    angobOutput.BAUJAHR = angob.baujahr;
                }
                ctx.SaveChanges();

                //TODO REMOVE WHEN MAPPED
                List<Devart.Data.Oracle.OracleParameter> upar = new List<Devart.Data.Oracle.OracleParameter>();
                upar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysob", Value = angobOutput.SYSOB });
                upar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "typfz", Value = angob.typfz });
                ctx.ExecuteStoreCommand("update ANGOB set typfz=:typfz where sysob=:sysob", upar.ToArray());

                AngobDto rval = Mapper.Map<ANGOB, AngobDto>(angobOutput);
                //TODO REMOVE WHEN MAPPED
                rval.typfz = angob.typfz;

                if (angob.ausstattungen != null)
                {
                    rval.ausstattungen = new List<ObjektAustDto>();
                    //search for all current entries for this angebot
                    List<Devart.Data.Oracle.OracleParameter> austpar = new List<Devart.Data.Oracle.OracleParameter>();
                    austpar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysangob", Value = rval.sysOb });
                    List<long> allAust = ctx.ExecuteStoreQuery<long>(QUERYANGOBAUSTID, austpar.ToArray()).ToList();
                    //remember really saved austs
                    List<long> savedAust = new List<long>();
                    foreach (ObjektAustDto aust in angob.ausstattungen)
                    {
                        aust.sysangob = rval.sysOb;
                        ObjektAustDto updAust = createOrUpdateAngobaust(aust);
                        savedAust.Add(updAust.sysobaust);
                        rval.ausstattungen.Add(updAust);
                    }
                    //delete items no more present 
                    List<long> delAust = allAust.Except(savedAust).ToList();
                    if (delAust.Count > 0)
                    {
                        String ids = String.Join(",", delAust);
                        ctx.ExecuteStoreCommand("delete from angobaust where sysobaust in (" + ids + ")", null);
                    }

                }
                if (angob.zusatzdaten != null)
                {
                    angob.zusatzdaten.sysobini = rval.sysOb;
                    rval.zusatzdaten = createOrUpdateAngobini(angob.zusatzdaten);
                }
                SearchCache.entityChanged("ANGOB");
                return rval;
            }

        }

        /// <summary>
        /// updates/creates Antob
        /// </summary>
        /// <param name="sysantob"></param>
        /// <returns></returns>
        public AntobDto createOrUpdateAntob(AntobDto antob)
        {

            ANTOB antobOutput = null;
            using (DdOlExtended ctx = new DdOlExtended())
            {
                if (antob.fabrikat != null && antob.fabrikat.Length > 60)
                    antob.fabrikat = antob.fabrikat.Substring(0, 60);
                if (antob.typ != null && antob.typ.Length > 60)
                    antob.typ = antob.typ.Substring(0, 60);
                if (antob.eek != null && antob.eek.Length > 10)
                    antob.eek = antob.eek.Substring(0, 10);
                if (antob.sysOb == 0)
                {
                    antobOutput = new ANTOB();
                    antobOutput = Mapper.Map<AntobDto, ANTOB>(antob, antobOutput);
                    antobOutput.SYSOB = 0;

                    ctx.AddToANTOB(antobOutput);


                }
                else
                {
                    antobOutput = (from p in ctx.ANTOB
                                   where p.SYSOB == antob.sysOb
                                   select p).FirstOrDefault();
                    if (antobOutput != null)
                        antobOutput = Mapper.Map<AntobDto, ANTOB>(antob, antobOutput);
                    else throw new Exception("Antob with id " + antob.sysOb + " not found!");
                }

                

                antobOutput.OBJEKT = null;
                ctx.SaveChanges();

                //TODO REMOVE WHEN MAPPED
                /*  List<Devart.Data.Oracle.OracleParameter> upar = new List<Devart.Data.Oracle.OracleParameter>();
                  upar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysob", Value = antobOutput.SYSOB });
                  upar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "typfz", Value = antob.typfz });
                  ctx.ExecuteStoreCommand("update ANTOB set typfz=:typfz where sysob=:sysob", upar.ToArray());
                  */
                AntobDto rval = Mapper.Map<ANTOB, AntobDto>(antobOutput);
                //TODO REMOVE WHEN MAPPED
                rval.typfz = antob.typfz;

                if (antob.ausstattungen != null)
                {
                    rval.ausstattungen = new List<ObjektAustDto>();
                    //search for all current entries for this angebot
                    List<Devart.Data.Oracle.OracleParameter> austpar = new List<Devart.Data.Oracle.OracleParameter>();
                    austpar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysantob", Value = rval.sysOb });
                    List<long> allAust = ctx.ExecuteStoreQuery<long>(QUERYANTOBAUSTID, austpar.ToArray()).ToList();
                    //remember really saved austs
                    List<long> savedAust = new List<long>();
                    foreach (ObjektAustDto aust in antob.ausstattungen)
                    {
                        aust.sysangob = rval.sysOb;
                        ObjektAustDto updAust = createOrUpdateAntobaust(aust);
                        savedAust.Add(updAust.sysobaust);
                        rval.ausstattungen.Add(updAust);
                    }
                    //delete items no more present 
                    List<long> delAust = allAust.Except(savedAust).ToList();
                    if (delAust.Count > 0)
                    {
                        String ids = String.Join(",", delAust);
                        ctx.ExecuteStoreCommand("delete from antobaust where sysobaust in (" + ids + ")", null);
                    }

                }


                SearchCache.entityChanged("ANTOB");
                return rval;
            }

        }

        /// <summary>
        /// updates/creates Angobaust
        /// </summary>
        /// <param name="angobaust"></param>
        /// <returns></returns>
        public ObjektAustDto createOrUpdateAngobaust(ObjektAustDto angobaust)
        {

            ANGOBAUST angobOutput = null;
            using (DdOlExtended ctx = new DdOlExtended())
            {
                if (angobaust.sysobaust == 0)
                {
                    angobOutput = new ANGOBAUST();
                    angobOutput = Mapper.Map<ObjektAustDto, ANGOBAUST>(angobaust, angobOutput);
                    angobOutput.SYSOBAUST = 0;

                    ctx.AddToANGOBAUST(angobOutput);


                }
                else
                {
                    angobOutput = (from p in ctx.ANGOBAUST
                                   where p.SYSOBAUST == angobaust.sysobaust
                                   select p).FirstOrDefault();
                    if (angobOutput != null)
                        angobOutput = Mapper.Map<ObjektAustDto, ANGOBAUST>(angobaust, angobOutput);
                    else throw new Exception("AngobAustDto with id " + angobaust.sysobaust + " not found!");
                }

                ctx.SaveChanges();
                SearchCache.entityChanged("ANGOBAUST");
            }
            return Mapper.Map<ANGOBAUST, ObjektAustDto>(angobOutput);
        }

        /// <summary>
        /// updates/creates Antobaust
        /// </summary>
        /// <param name="antobaust"></param>
        /// <returns></returns>
        public ObjektAustDto createOrUpdateAntobaust(ObjektAustDto antobaust)
        {

            ANTOBAUST antobOutput = null;
            using (DdOlExtended ctx = new DdOlExtended())
            {
                if (antobaust.sysobaust == 0)
                {
                    antobOutput = new ANTOBAUST();
                    antobOutput = Mapper.Map<ObjektAustDto, ANTOBAUST>(antobaust, antobOutput);
                    antobOutput.SYSOBAUST = 0;

                    ctx.AddToANTOBAUST(antobOutput);


                }
                else
                {
                    antobOutput = (from p in ctx.ANTOBAUST
                                   where p.SYSOBAUST == antobaust.sysobaust
                                   select p).FirstOrDefault();
                    if (antobOutput != null)
                        antobOutput = Mapper.Map<ObjektAustDto, ANTOBAUST>(antobaust, antobOutput);
                    else throw new Exception("AntobAustDto with id " + antobaust.sysobaust + " not found!");
                }

                ctx.SaveChanges();
                SearchCache.entityChanged("ANTOBAUST");
            }
            return Mapper.Map<ANTOBAUST, ObjektAustDto>(antobOutput);
        }

        /// <summary>
        /// updates/creates Obaust
        /// </summary>
        /// <param name="obaust"></param>
        /// <returns></returns>
        public ObjektAustDto createOrUpdateObaust(ObjektAustDto obaust)
        {

            OBAUST obOutput = null;
            using (DdOlExtended ctx = new DdOlExtended())
            {
                if (obaust.sysobaust == 0)
                {
                    obOutput = new OBAUST();
                    obOutput = Mapper.Map<ObjektAustDto, OBAUST>(obaust, obOutput);
                    obOutput.SYSOBAUST = 0;
                    


                    ctx.AddToOBAUST(obOutput);


                }
                else
                {
                    obOutput = (from p in ctx.OBAUST
                                where p.SYSOBAUST == obaust.sysobaust
                                select p).FirstOrDefault();
                    if (obOutput != null)
                        obOutput = Mapper.Map<ObjektAustDto, OBAUST>(obaust, obOutput);
                    else throw new Exception("ObAustDto with id " + obaust.sysobaust + " not found!");
                }

                ctx.SaveChanges();
                SearchCache.entityChanged("OBAUST");
            }
            return Mapper.Map<OBAUST, ObjektAustDto>(obOutput);
        }

        /// <summary>
        /// updates/creates Angobini / Zusatzdaten for Object
        /// </summary>
        /// <param name="angobini"></param>
        /// <returns></returns>
        public AngobIniDto createOrUpdateAngobini(AngobIniDto angobini)
        {

            ANGOBINI angobOutput = null;
            using (DdOlExtended ctx = new DdOlExtended())
            {
                if (angobini.farbe_a != null && angobini.farbe_a.Length > 20)
                    angobini.farbe_a = angobini.farbe_a.Substring(0, 20);
                if (angobini.farbe_i != null && angobini.farbe_i.Length > 20)
                    angobini.farbe_i = angobini.farbe_i.Substring(0, 20);

                if (angobini.sysobini > 0)
                {
                    angobOutput = (from p in ctx.ANGOBINI
                                   where p.SYSOBINI == angobini.sysobini
                                   select p).FirstOrDefault();
                    if (angobOutput != null)
                        angobOutput = Mapper.Map<AngobIniDto, ANGOBINI>(angobini, angobOutput);
                }
                if (angobOutput == null)
                {
                    angobOutput = new ANGOBINI();
                    angobOutput = Mapper.Map<AngobIniDto, ANGOBINI>(angobini, angobOutput);
                    angobOutput.SYSOBINI = angobini.sysobini;
                    ctx.AddToANGOBINI(angobOutput);

                }



                ctx.SaveChanges();
                SearchCache.entityChanged("ANGOBINI");
            }
            return Mapper.Map<ANGOBINI, AngobIniDto>(angobOutput);
        }

        /// <summary>
        /// updates/creates AngobslDto
        /// </summary>
        /// <param name="sysangob"></param>
        /// <returns></returns>
        public AngobslDto createOrUpdateAngobsl(AngobslDto angob)
        {

            ANGOBSL angobOutput = null;
            using (DdOlExtended ctx = new DdOlExtended())
            {
                if (angob.sysid == 0)
                {
                    angobOutput = new ANGOBSL();
                    angobOutput = Mapper.Map<AngobslDto, ANGOBSL>(angob, angobOutput);
                    angobOutput.SYSID = 0;
                    ctx.AddToANGOBSL(angobOutput);


                }
                else
                {
                    angobOutput = (from p in ctx.ANGOBSL
                                   where p.SYSID == angob.sysid
                                   select p).FirstOrDefault();
                    if (angobOutput != null)
                        angobOutput = Mapper.Map<AngobslDto, ANGOBSL>(angob, angobOutput);
                    else throw new Exception("AngobslDto with id " + angob.sysid + " not found!");
                }

                ctx.SaveChanges();
                SearchCache.entityChanged("ANGOBSL");
            }
            return Mapper.Map<ANGOBSL, AngobslDto>(angobOutput);
        }

        /// <summary>
        /// updates/creates Angvar
        /// </summary>
        /// <param name="sysangvar"></param>
        /// <returns></returns>
        public AngvarDto createOrUpdateAngvar(AngvarDto angvar)
        {

            ANGVAR angvarOutput = null;
            using (DdOlExtended ctx = new DdOlExtended())
            {
                if (angvar.sysAngvar == 0)
                {
                    angvarOutput = new ANGVAR();
                    angvarOutput = Mapper.Map<AngvarDto, ANGVAR>(angvar, angvarOutput);
                    angvarOutput.SYSANGVAR = 0;
                    

                    ctx.AddToANGVAR(angvarOutput);


                }
                else
                {
                    angvarOutput = (from p in ctx.ANGVAR
                                    where p.SYSANGVAR == angvar.sysAngvar
                                    select p).FirstOrDefault();
                    if (angvarOutput != null)
                        angvarOutput = Mapper.Map<AngvarDto, ANGVAR>(angvar, angvarOutput);
                    else throw new Exception("Angvar with id " + angvar.sysAngvar + " not found!");
                }

                ctx.SaveChanges();
                SearchCache.entityChanged("ANGVAR");
                AngvarDto rval = Mapper.Map<ANGVAR, AngvarDto>(angvarOutput);
                rval.angobList = new List<AngobDto>();
                rval.angobslList = new List<AngobslDto>();
                if (angvar.angobList != null)
                    foreach (AngobDto ob in angvar.angobList)
                    {
                        ob.sysVT = rval.sysAngvar;//sysangvar and is linked via sysvt in angob!
                        rval.angobList.Add(createOrUpdateAngob(ob));
                    }
                if (angvar.angobslList != null)
                    foreach (AngobslDto ob in angvar.angobslList)
                    {
                        ob.sysvt = rval.sysAngvar; //angobsl.sysvt = sysangvar
                        rval.angobslList.Add(createOrUpdateAngobsl(ob));
                    }
                return rval;

            }

        }

        /// <summary>
        /// updates/creates Angebot
        /// </summary>
        /// <param name="sysangebot"></param>
        /// <returns></returns>
        public AngebotDto createOrUpdateAngebot(AngebotDto angebot)
        {

            ANGEBOT angebotOutput = null;
            using (DdOlExtended ctx = new DdOlExtended())
            {
                if (angebot.sysID == 0)
                {
                    angebotOutput = new ANGEBOT();
                    angebotOutput = Mapper.Map<AngebotDto, ANGEBOT>(angebot, angebotOutput);
                    angebotOutput.SYSANGEBOT = 0;

                    ctx.AddToANGEBOT(angebotOutput);
                    ctx.SaveChanges();
                    angebotOutput.ANGEBOT1 = "A" + angebotOutput.SYSID;
                    if (angebot.varianten != null && angebot.varianten.Count > 0 && angebot.varianten[0].angobList != null && angebot.varianten[0].angobList.Count > 0)
                    {
                        angebot.varianten[0].angobList[0].bezeichnung = angebotOutput.ANGEBOT1;
                    }

                }
                else
                {
                    angebotOutput = (from p in ctx.ANGEBOT
                                     where p.SYSID == angebot.sysID
                                     select p).FirstOrDefault();
                    if (angebotOutput != null)
                        angebotOutput = Mapper.Map<AngebotDto, ANGEBOT>(angebot, angebotOutput);
                    else throw new Exception("Angebot with id " + angebot.sysID + " not found!");
                }

                if (angebot.SysPerson != 0 || angebot.sysKd != 0)
                {
                    if (angebot.SysPerson != 0)
                        angebotOutput.SYSKD = angebot.SysPerson;
                    if (angebot.sysKd != 0)
                        angebotOutput.SYSKD = angebot.sysKd;
                }

                ANGOPTION popt = (from p in ctx.ANGOPTION
                                  where p.SYSID == angebotOutput.SYSID
                                  select p).FirstOrDefault();
                if (popt == null)
                {
                    popt = new ANGOPTION();
                    popt.SYSID =  angebotOutput.SYSID;
                    ctx.AddToANGOPTION(popt);
                    angebotOutput.ANGOPTION = Mapper.Map<AngAntOptionDto, ANGOPTION>(angebot.options);
                }
                else
                {
                    angebot.options.sysid = angebotOutput.SYSID;
                }
                popt = Mapper.Map<AngAntOptionDto, ANGOPTION>(angebot.options, popt);
                //popt.SYSID = angebotOutput.SYSID;
                ctx.SaveChanges();

                
                SearchCache.entityChanged("ANGEBOT");
                AngebotDto rval = Mapper.Map<ANGEBOT, AngebotDto>(angebotOutput);
                rval.options = Mapper.Map<ANGOPTION, AngAntOptionDto>(popt);
                rval.varianten = new List<AngvarDto>();
                if (angebot.varianten != null)
                    foreach (AngvarDto var in angebot.varianten)
                    {
                        if (var.angobList != null && var.angobList.Count > 0)
                            var.angobList[0].bezdeutsch = popt.OPTION1;

                        var.sysAngebot = rval.sysID; //link to angebot
                        rval.varianten.Add(createOrUpdateAngvar(var));
                    }

                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = angebotOutput.SYSID });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "beschrdeutsch", Value = angebot.beschrdeutsch });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "extratingcode", Value = angebot.extratingcode });

                ctx.ExecuteStoreCommand("update angebot set extratingcode=:extratingcode, beschrdeutsch=:beschrdeutsch where sysid=:sysid", parameters.ToArray());
                rval.extratingcode = angebot.extratingcode;
                rval.beschrdeutsch = angebot.beschrdeutsch;

                //Produkt Info
                rval.produkt = getProduktInfoAngebotDetails(angebotOutput.SYSID );

                return rval;
            }

        }

        /// <summary>
        /// updates/creates Antrag
        /// </summary>
        /// <param name="antrag"></param>
        /// <returns></returns>
        public AntragDto createOrUpdateAntrag(AntragDto antrag)
        {

            ANTRAG antragOutput = null;
            using (DdOlExtended ctx = new DdOlExtended())
            {
                if (antrag.sysID == 0)
                {
                    antragOutput = new ANTRAG();
                    antragOutput = Mapper.Map<AntragDto, ANTRAG>(antrag, antragOutput);
                    antragOutput.SYSID = 0;
                    NkBuilder nkantrag = new NkBuilder( "ANTRAG", "B2B");
                    antragOutput.ANTRAG1 = nkantrag.getNextNumber();
                    
                    ctx.AddToANTRAG(antragOutput);
                    ctx.SaveChanges();
                    //antragOutput.ANTRAG1 = "A" + antragOutput.SYSID;


                }
                else
                {
                    antragOutput = (from p in ctx.ANTRAG
                                    where p.SYSID == antrag.sysID
                                    select p).FirstOrDefault();
                    if (antragOutput != null)
                        antragOutput = Mapper.Map<AntragDto, ANTRAG>(antrag, antragOutput);
                    else throw new Exception("Antrag with id " + antrag.sysID + " not found!");
                }

                if (antrag.sysKd != 0)
                {
                    
                        antragOutput.SYSKD = antrag.sysKd;
                }

                ANTOPTION popt = (from p in ctx.ANTOPTION
                                  where p.SYSID == antrag.sysID
                                  select p).FirstOrDefault(); 
                if (popt == null)
                {
                    popt = new ANTOPTION();
                    popt.SYSID = antragOutput.SYSID;
                    ctx.AddToANTOPTION(popt);
                }
                
                popt = Mapper.Map<AngAntOptionDto, ANTOPTION>(antrag.options, popt);
                ctx.SaveChanges();
                //TODO EDMX Missing
                List<Devart.Data.Oracle.OracleParameter> pars = new List<Devart.Data.Oracle.OracleParameter>();
                pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysantrag", Value =  antragOutput.SYSID });
                pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "code", Value = antrag.extratingcode});
                pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "benutzer", Value = antrag.benutzer });
                pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "beschr", Value = antrag.beschrdeutsch });
                ctx.ExecuteStoreCommand("update antrag set benutzer=:benutzer,extratingcode=:code,beschrdeutsch=:beschr where sysantrag=:sysantrag",pars.ToArray());

                DbConnection con = (ctx.Connection as EntityConnection).StoreConnection;

                if(antrag.konto!=null)
                {
                    if (antrag.konto.syskonto == null && antrag.konto.sysblz>0)//neues Konto anlegen, ändern hier nicht zulassen
                    {
                        antrag.konto = createOrUpdateKonto(antrag.konto);
                    }
                    long syskontoref = con.Query<long>("select kontoref.syskontoref from kontoref,antrag where activeflag=1 and kontoref.sysperson=antrag.syskd and kontoref.sysantrag=antrag.sysid and antrag.sysid=:sysantrag and kontoref.syskonto=:syskonto", new { sysantrag = antragOutput.SYSID, syskonto = antrag.konto.syskonto }).FirstOrDefault();
                    //andere konten auf diesen Antrag inaktivieren
                    ctx.ExecuteStoreCommand("update kontoref set activeflag=0, validuntil=sysdate where syskontoref!="+syskontoref+" and sysantrag=" + antragOutput.SYSID);
                    //long otherkontoref = con.Query<long>("select kontoref.syskontoref from kontoref where kontoref.sysantrag=antrag.sysid and antrag.sysid=:sysantrag", new { sysantrag = rval.sysID }).FirstOrDefault();
                    if(syskontoref==0)//noch keine Antragsverknüpfung, neu anlegen
                    {
                        KONTOREF ktoref = new KONTOREF();
                        ktoref.ACTIVEFLAG = 1;
                        ktoref.SYSANTRAG = antragOutput.SYSID;
                        ktoref.SYSKONTO = antrag.konto.syskonto;
                        ktoref.SYSPERSON = antrag.sysKd;
                        ktoref.VALIDFROM=DateTime.Now;

                        ctx.AddToKONTOREF(ktoref);
                        ctx.SaveChanges();
                    }

                }
                


                SearchCache.entityChanged("ANTRAG");
                AntragDto rval = Mapper.Map<ANTRAG, AntragDto>(antragOutput);
                //rval.options = Mapper.Map<ANTOPTION, AntoptionDto>(popt);


                antrag.antob.sysVT = rval.sysID;//sysantrag and is linked via sysvt in angob!
                antrag.antob.sysAntrag = rval.sysID;//this is the correct link!
                antrag.antob.objekt = antragOutput.ANTRAG1;
                rval.antob = createOrUpdateAntob(antrag.antob);


                //Produkt Info
               
                rval.produkt = getProduktInfoAntragDetails(ctx,rval.sysID);
                rval.konto = con.Query<KontoDto>(QUERYANTKONTO, new { sysantrag = rval.sysID }).FirstOrDefault();

                //currently allow update for given auflagen (no insert!)
                if(antrag.auflagen!=null && antrag.auflagen.Count>0)
                {
                    foreach(RatingAuflageDto ra in antrag.auflagen)
                    {
                        if (ra.sysratingauflage > 0 && ra.fullfilleddate != null && ra.fullfilleddate.HasValue)
                        {
                             pars = new List<Devart.Data.Oracle.OracleParameter>();
                            pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysratingauflage", Value = ra.sysratingauflage });
                            pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "ff", Value = ra.fullfilled });
                            pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "dt", Value = ra.fullfilleddate });
                            pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "us", Value = sysWfuser });
                            pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "t", Value = DateTimeHelper.DateTimeToClarionTime(DateTime.Now) });

                            ctx.ExecuteStoreCommand("update ratingauflage set syschguser=:us, syschgdate=sysdate, syschgtime=:t, fullfilled=:ff, fullfilleddate=:dt where sysratingauflage=:sysratingauflage", pars.ToArray());
                        }
                    }
                }

                rval.auflagen = con.Query<RatingAuflageDto>(QUERYAUFLAGEN2, new { sysid = rval.sysID }).ToList();
                return rval;
            }

        }

        /// <summary>
        /// updates/creates Contact
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public ContactDto createOrUpdateContact(ContactDto contact)
        {

            CONTACT contactOutput = null;
            ContactDto contactDto = null;
            using (DdOlExtended ctx = new DdOlExtended())
            {
                if (contact.sysContact == 0)
                {
                    contactOutput = new CONTACT();
                    contactOutput = Mapper.Map<ContactDto, CONTACT>(contact, contactOutput);
                    
                    contactOutput.SYSCONTACT = 0;
                    ctx.AddToCONTACT(contactOutput);



                }
                else
                {
                    //die beziehung ist bereits in ptrelate abgespeichert worden
                    /* if (contact.sysPtrelate < 0)
                         contact.sysPartner = -1 * contact.sysPtrelate;
                     else
                     {
                         long? syspartner = (from c in ctx.PTRELATE where c.SYSPTRELATE == contact.sysPtrelate select c.SYSPERSON2).FirstOrDefault();
                         contact.sysPartner = syspartner.HasValue ? syspartner.Value : 0;
                     }*/
                    contactOutput = (from c in ctx.CONTACT
                                     where c.SYSCONTACT == contact.sysContact
                                     select c).FirstOrDefault();
                    if (contactOutput != null)
                    {
                        contactOutput = Mapper.Map<ContactDto, CONTACT>(contact, contactOutput);


                        
                        
                        

                    }
                    else throw new Exception("Contact with id " + contact.sysContact + " not found!");
                }

                ctx.SaveChanges();
                SearchCache.entityChanged("CONTACT");
                contactDto = Mapper.Map<CONTACT, ContactDto>(contactOutput);
                // contactDto.sysPtrelate = contact.sysPtrelate;
                bool isPeuni = createOrUpdatePeuni("CONTACT", getSysPerole(), contactOutput.SYSCONTACT);
            }


            return contactDto;
        }

        /// <summary>
        /// updates/creates Adresse
        /// </summary>
        /// <param name="adresse"></param>
        /// <returns></returns>
        public AdresseDto createOrUpdateAdresse(AdresseDto adresse)
        {

            ADRESSE adresseOutput = null;
            using (DdOlExtended ctx = new DdOlExtended())
            {
                if (adresse.sysAdresse == 0)
                {
                    adresseOutput = new ADRESSE();

                    adresseOutput = Mapper.Map<AdresseDto, ADRESSE>(adresse, adresseOutput);
                    adresseOutput.SYSADRESSE = 0;


                    int maxrang = ctx.ExecuteStoreQuery<int>("select max(rang) from adresse where sysperson=" + adresse.sysPerson).FirstOrDefault();
                    maxrang++;
                    adresseOutput.RANG = maxrang;

                    ctx.AddToADRESSE(adresseOutput);

                }
                else
                {
                    adresseOutput = (from c in ctx.ADRESSE
                                     where c.SYSADRESSE == adresse.sysAdresse
                                     select c).FirstOrDefault();
                    if (adresseOutput != null)
                        adresseOutput = Mapper.Map<AdresseDto, ADRESSE>(adresse, adresseOutput);
                    else throw new Exception("adresse with id " + adresse.sysAdresse + " not found!");
                }

                ctx.SaveChanges();
                SearchCache.entityChanged("ADRESSE");
            }
            return Mapper.Map<ADRESSE, AdresseDto>(adresseOutput);
        }

        /// <summary>
        /// updates/creates partner for wktaccount
        /// </summary>
        /// <param name="itadresse"></param>
        /// <returns></returns>
        public void createOrUpdateWktPartner(WktaccountDto wktAccount)
        {

            PARTNER partner = null;
            using (DdOlExtended ctx = new DdOlExtended())
            {
                if (wktAccount.sysit > 0)
                {
                    List<Devart.Data.Oracle.OracleParameter> pars = new List<Devart.Data.Oracle.OracleParameter>();
                    pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = wktAccount.sysit });
                    long syspartner = ctx.ExecuteStoreQuery<long>("select syspartner from partner where sysit=:sysit", pars.ToArray()).FirstOrDefault();
                    if (syspartner == 0)
                    {
                        partner = new PARTNER();
                        ctx.AddToPARTNER(partner);
                    }
                    else
                    {
                        partner = (from c in ctx.PARTNER
                                   where c.SYSPARTNER == syspartner
                                   select c).FirstOrDefault();
                    }
                    partner.SYSIT = wktAccount.sysit;

                }
                else if (wktAccount.sysperson > 0)
                {
                    List<Devart.Data.Oracle.OracleParameter> pars = new List<Devart.Data.Oracle.OracleParameter>();
                    pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperson", Value = wktAccount.sysperson });
                    long syspartner = ctx.ExecuteStoreQuery<long>("select syspartner from partner where sysperson=:sysperson", pars.ToArray()).FirstOrDefault();
                    if (syspartner == 0)
                    {
                        partner = new PARTNER();
                        ctx.AddToPARTNER(partner);
                    }
                    else
                    {
                        partner = (from c in ctx.PARTNER
                                   where c.SYSPARTNER == syspartner
                                   select c).FirstOrDefault();
                    }
                    partner.SYSPERSON = wktAccount.sysperson;
                }
                //TODO automapper 2.0
                //partner = Mapper.Map<WktaccountDto, PARTNER>(wktAccount, partner);
                if (partner != null)
                {
                    partner.LASTNAME = wktAccount.plastname;
                    partner.FIRSTNAME = wktAccount.pfirstname;
                    partner.ANREDE = wktAccount.panrede;
                    partner.TITEL = wktAccount.ptitel;
                    partner.TELEFON = wktAccount.ptelefon;
                    partner.MOBIL = wktAccount.pmobil;
                    partner.EMAIL = wktAccount.pemail;
                    partner.FAX = wktAccount.pfax;
                }
                ctx.SaveChanges();
            }

        }

        /// <summary>
        /// updates/creates Itadresse
        /// </summary>
        /// <param name="itadresse"></param>
        /// <returns></returns>
        public ItadresseDto createOrUpdateItadresse(ItadresseDto itadresse)
        {

            ITADRESSE itadresseOutput = null;
            using (DdOlExtended ctx = new DdOlExtended())
            {
                if (itadresse.sysItadresse == 0)
                {
                    itadresseOutput = new ITADRESSE();

                    itadresseOutput = Mapper.Map<ItadresseDto, ITADRESSE>(itadresse, itadresseOutput);
                    itadresseOutput.SYSITADRESSE = 0;

                    
                    ctx.AddToITADRESSE(itadresseOutput);

                }
                else
                {
                    itadresseOutput = (from c in ctx.ITADRESSE
                                       where c.SYSITADRESSE == itadresse.sysItadresse
                                       select c).FirstOrDefault();
                    if (itadresseOutput != null)
                        itadresseOutput = Mapper.Map<ItadresseDto, ITADRESSE>(itadresse, itadresseOutput);
                    else throw new Exception("itadresse with id " + itadresse.sysItadresse + " not found!");
                }

                ctx.SaveChanges();
                SearchCache.entityChanged("ITADRESSE");
            }
            return Mapper.Map<ITADRESSE, ItadresseDto>(itadresseOutput);
        }

        /// <summary>
        /// updates/creates prunart data - but only validuntil and art
        /// </summary>
        /// <param name="prunart"></param>
        /// <returns></returns>
        public PrunartDto createOrUpdatePrunart(PrunartDto prunart)
        {
            using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended pe = new Cic.OpenOne.Common.Model.Prisma.PrismaExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> pars = new List<Devart.Data.Oracle.OracleParameter>();
                pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "vuntil", Value = prunart.validUntilNew });
                pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "art", Value = prunart.artNew });
                //pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysprunart", Value = prunart.sysprunart });
                pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysptype", Value = prunart.sysptype });
                pe.ExecuteStoreCommand("UPDATE PRUNART set activeflag=0");
                if(prunart.artNew!=null && prunart.artNew.Length>0)
                    pe.ExecuteStoreCommand("INSERT INTO PRUNART(VALIDFROM,VALIDUNTIL,ACTIVEFLAG,ART,SYSPTYPE) values (SYSDATE,:vuntil,1,:art,:sysptype)", pars.ToArray());

                long sysprunart = pe.ExecuteStoreQuery<long>("select sysprunart from prunart where activeflag=1",null).FirstOrDefault();

                return getPrunartDetails(sysprunart);
            }
        }

        /// <summary>
        /// updates/creates Checklist data
        /// </summary>
        /// <param name="chklist"></param>
        /// <returns></returns>
        public ChklistDto createOrUpdateChklist(ChklistDto chklist)
        {
            long clatime = DateTimeHelper.DateTimeToClarionTime(DateTime.Now);
            using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended pe = new Cic.OpenOne.Common.Model.Prisma.PrismaExtended())
            {

                if (chklist.salesFlag > 0)
                {
                    List<Devart.Data.Oracle.OracleParameter> pars = new List<Devart.Data.Oracle.OracleParameter>();
                    pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "edatum", Value = chklist.receiveDate });
                    pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "udatum", Value = chklist.vtDate });
                    pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = chklist.sysid });

                    pe.ExecuteStoreCommand("UPDATE ANTRAG SET EDATUM=:edatum, udatum=:udatum where sysid=:sysid", pars.ToArray());

                    if (chklist.vtDate != null)
                    {
                        pars = new List<Devart.Data.Oracle.OracleParameter>();
                        int? clatimevt = DateTimeHelper.DateTimeToClarionTime(chklist.vtDate);
                        pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p", Value = clatimevt });
                        pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = chklist.sysid });

                        pe.ExecuteStoreCommand("UPDATE ANTOPTION set ulon05=:p where sysid=:sysid", pars.ToArray());
                    }
                }
                if(chklist.artNew!=null)
                {
                    List<Devart.Data.Oracle.OracleParameter> pars = new List<Devart.Data.Oracle.OracleParameter>();
                    pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p1", Value = chklist.artNew });
                    pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = chklist.sysid });

                    pe.ExecuteStoreCommand("UPDATE ANTOPTION SET STR07=:p1 where sysid=:sysid", pars.ToArray());
                }
                if (chklist.geldfluss.HasValue)
                {
                    List<Devart.Data.Oracle.OracleParameter> pars = new List<Devart.Data.Oracle.OracleParameter>();
                    pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p1", Value = chklist.geldfluss.Value });
                    pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p2", Value = getSysWfuser() });
                    pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = chklist.sysid });

                    pe.ExecuteStoreCommand("UPDATE ANTRAG SET FLAGFREIGABEAUSZ=:p1,FREIGABEAUSZ=sysdate,SYSWFUSERAUSZ=:p2 where sysid=:sysid", pars.ToArray());
                }
                if (chklist.schlusskontrolle.HasValue)
                {
                    List<Devart.Data.Oracle.OracleParameter> pars = new List<Devart.Data.Oracle.OracleParameter>();
                    pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p1", Value = chklist.schlusskontrolle.Value });
                    pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p2", Value = getSysWfuser() });
                    pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = chklist.sysid });

                    pe.ExecuteStoreCommand("UPDATE ANTRAG SET FLAGFREIGABEFORM=:p1,SYSWFUSERFORM=:p2 where sysid=:sysid", pars.ToArray());
                }
                if (chklist.salesFlag > 0)
                {
                    int? clatimevt = DateTimeHelper.DateTimeToClarionTime(DateTime.Now);
                    List<Devart.Data.Oracle.OracleParameter> pars = new List<Devart.Data.Oracle.OracleParameter>();
                    pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p1", Value = chklist.pruefung });
                    pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p2", Value = getSysWfuser() });
                    pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p3", Value = DateTime.Now });
                    pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p4", Value = clatimevt });
                    pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = chklist.sysid });

                    pe.ExecuteStoreCommand("UPDATE ANTRAG SET FLAGSK=:p1,SYSSKUSER=:p2,CHGDATESK=:p3,CHGTIMESK=:p4 where sysid=:sysid", pars.ToArray());
                }


                if(chklist.rows!=null && chklist.rows.Count>0)
                foreach (ChklistEntryDto row in chklist.rows)
                {
                    if (chklist.salesFlag > 0)//SALES SIEHT/SCHREIBT in prunstep.flagok, wenn prunstep leer dann in RATINGAUFLAGE.fulfilled
                    {
                        if (row.sysprunstep > 0)//flagnok, flagok,SYSCHGUSEROK,CHGDATEOK,CHGTIMEOK,SYSCHGUSERNOK,CHGDATENOK,CHGTIMENOK  (user=syswfuser)
                        {

                            pe.ExecuteStoreCommand("UPDATE PRUNSTEP set FLAGOK=" + row.flagok + ", CHGDATEOK=SYSDATE, CHGTIMEOK=" + clatime + ", SYSCHGUSEROK=" + getSysWfuser() + " WHERE SYSPRUNSTEP=" + row.sysprunstep, null);
                            //wenn alle runstep der auflage ok, dann auflage auf fulfilled
                            if (row.flagok > 0)
                            {
                                long nokcount = pe.ExecuteStoreQuery<long>("select count(*) from prunstep where prunstep.syscode=" + row.sysratingauflage + " and activeflag=1 and (flagok=0)", null).FirstOrDefault();
                                if (nokcount == 0)
                                {
                                    pe.ExecuteStoreCommand("UPDATE RATINGAUFLAGE set FULLFILLED=1, FULLFILLEDDATE=SYSDATE, SYSCHGUSER=" + getSysWfuser() + ", SYSCHGDATE=SYSDATE, SYSCHGTIME=" + clatime + " WHERE SYSRATINGAUFLAGE=" + row.sysratingauflage, null);
                                }
                            }
                            else//ratingauflage fullfilled zurücksetzen
                            {
                                pe.ExecuteStoreCommand("UPDATE RATINGAUFLAGE set FULLFILLED=0, FULLFILLEDDATE=SYSDATE, SYSCHGUSER=" + getSysWfuser() + ", SYSCHGDATE=SYSDATE, SYSCHGTIME=" + clatime + " WHERE SYSRATINGAUFLAGE=" + row.sysratingauflage, null);
                            }
                        }
                        else//Ratingauflage fulfilled
                        {
                            pe.ExecuteStoreCommand("UPDATE RATINGAUFLAGE set FULLFILLED=" + row.flagok + ", FULLFILLEDDATE=SYSDATE, SYSCHGUSER=" + getSysWfuser() + ", SYSCHGDATE=SYSDATE, SYSCHGTIME=" + clatime + " WHERE SYSRATINGAUFLAGE=" + row.sysratingauflage, null);
                        }

                    }
                    else
                    {//Payments
                        if (row.sysprunsteppos > 0)//flagnok, flagok,SYSCHGUSEROK,CHGDATEOK,CHGTIMEOK,SYSCHGUSERNOK,CHGDATENOK,CHGTIMENOK  (user=syswfuser)
                        {
                            pe.ExecuteStoreCommand("UPDATE PRUNSTEPPOS set FLAGNOK=" + row.flagnok + ", SYSCHGUSER=" + getSysWfuser() + ", CHGDATE=SYSDATE, CHGTIMEOK=" + clatime + " WHERE SYSPRUNSTEPPOS=" + row.sysprunsteppos, null);

                            if (row.flagnokstep.HasValue)//wenn ein runstep auf nok, den dazugehörigen runstep auf nok setzen
                            {
                                pe.ExecuteStoreCommand("UPDATE PRUNSTEP set FLAGNOK=" + row.flagnokstep + ",FLAGOK=0, CHGDATENOK=SYSDATE, CHGTIMENOK=" + clatime + ", SYSCHGUSERNOK=" + getSysWfuser() + " WHERE SYSPRUNSTEP=" + row.sysprunstep, null);
                            }
                            if (row.flagnok > 0)//wenn ein runsteppos auf nok, den dazugehörigen runstep auf nok setzen
                            {
                                pe.ExecuteStoreCommand("UPDATE PRUNSTEP set FLAGNOK=1,FLAGOK=0, CHGDATENOK=SYSDATE, CHGTIMENOK=" + clatime + ", SYSCHGUSERNOK=" + getSysWfuser() + " WHERE SYSPRUNSTEP=" + row.sysprunstep, null);
                                // 
                            }
                            else
                            {

                                long nokcount = pe.ExecuteStoreQuery<long>("select count(*) from prunsteppos,prunstep where prunstep.sysprunstep=prunsteppos.sysprunstep and prunstep.syscode=" + row.sysratingauflage + " and activeflag=1 and (prunsteppos.flagnok=1)", null).FirstOrDefault();
                                if (nokcount == 0)
                                {
                                    pe.ExecuteStoreCommand("UPDATE PRUNSTEP set FLAGNOK=0, CHGDATENOK=SYSDATE, CHGTIMENOK=" + clatime + ", SYSCHGUSERNOK=" + getSysWfuser() + " WHERE SYSPRUNSTEP=" + row.sysprunstep, null);

                                }
                            }
                        }
                    }

                }
                pe.SaveChanges();
            }
            igetChecklistDetailDto iget = new igetChecklistDetailDto();

            iget.sysid = chklist.sysid;
            iget.salesFlag = chklist.salesFlag;
            return getChklistDetails(iget);
        }

        /// <summary>
        /// updates/creates Camp
        /// </summary>
        /// <param name="Camp"></param>
        /// <returns></returns>
        public CampDto createOrUpdateCamp(CampDto camp)
        {

            CAMP campOutput = null;
            using (DdOlExtended ctx = new DdOlExtended())
            {
                if (camp.sysCamp == 0)
                {
                    campOutput = new CAMP();
                    campOutput = Mapper.Map<CampDto, CAMP>(camp, campOutput);
                    campOutput.SYSCAMP = 0;
                    campOutput.SYSCAMPPARENT = 0;

                    
                    ctx.AddToCAMP(campOutput);

                    ctx.SaveChanges();
                    campOutput.SYSCAMPPARENT = 0;
                }
                else
                {
                    campOutput = (from c in ctx.CAMP
                                  where c.SYSCAMP == camp.sysCamp
                                  select c).FirstOrDefault();
                    if (campOutput != null)
                        campOutput = Mapper.Map<CampDto, CAMP>(camp, campOutput);
                    else throw new Exception("Camp with id " + camp.sysCamp + " not found!");

                    if (campOutput.SYSCAMPPARENT == null)
                        campOutput.SYSCAMPPARENT = 0;

                   
                }
                SearchCache.entityChanged("CAMP");
                SearchCache.entityChanged("OPPO");
                ctx.SaveChanges();
            }
            return Mapper.Map<CAMP, CampDto>(campOutput);
        }

        /// <summary>
        /// updates/creates Oppo
        /// </summary>
        /// <param name="oppo"></param>
        /// <returns></returns>
        virtual public OpportunityDto createOrUpdateOppo(OpportunityDto oppo)
        {

            OPPO oppoOutput = null;
            OpportunityDto oppoDto = null;
            using (DdOlExtended ctx = new DdOlExtended())
            {
                if (oppo.sysOppo == 0)
                {
                    oppoOutput = new OPPO();
                    oppoOutput = Mapper.Map<OpportunityDto, OPPO>(oppo, oppoOutput);
                    
                    oppoOutput.SYSOPPO = 0;
                    ctx.AddToOPPO(oppoOutput);

                }
                else
                {
                    oppoOutput = (from c in ctx.OPPO
                                  where c.SYSOPPO == oppo.sysOppo
                                  select c).FirstOrDefault();
                    if (oppoOutput != null)
                    {
                        oppoOutput = Mapper.Map<OpportunityDto, OPPO>(oppo, oppoOutput);

                        
                    }
                    else throw new Exception("Oppo with id " + oppo.sysOppo + " not found!");

                }

                ctx.SaveChanges();
                SearchCache.entityChanged("OPPO");
                oppoDto = Mapper.Map<OPPO, OpportunityDto>(oppoOutput);
                bool isPeuni = createOrUpdatePeuni("OPPO", getSysPerole(), oppoOutput.SYSOPPO);
            }


            return oppoDto;
        }


        /// <summary>
        /// updates/creates Oppotask
        /// </summary>
        /// <param name="oppotask"></param>
        /// <returns></returns>
        public OppotaskDto createOrUpdateOppotask(OppotaskDto oppotask)
        {


            OPPOTASK oppotaskOutput = null;
            OppotaskDto oppotaskDto = null;
            using (DdOlExtended ctx = new DdOlExtended())
            {

                double daydiff = (oppotask.duedate - DateTime.Now).TotalDays;
                if (daydiff > 2)
                    oppotask.phase = 0;
                else if (daydiff <= 2 && daydiff >= 0)
                    oppotask.phase = 1;
                else oppotask.phase = 2;

                if (oppotask.sysOppotask == 0)
                {
                    oppotaskOutput = new OPPOTASK();
                    oppotaskOutput = Mapper.Map<OppotaskDto, OPPOTASK>(oppotask, oppotaskOutput);

                    
                    if (oppotask.sysoppo != 0)
                    {
                        
                        if (oppotask.area == null)
                        {
                            oppotaskOutput.AREA = ctx.ExecuteStoreQuery<String>("select area from oppo where sysoppo=" + oppotask.sysoppo, null).FirstOrDefault();
                        }
                    }

                    oppotaskOutput.SYSOPPOTASK = 0;
                    oppotaskOutput.CRTDATE = DateTime.Now;
                    oppotaskOutput.CRTTIME = (long)Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(oppotaskOutput.CRTDATE.Value);


                    ctx.AddToOPPOTASK(oppotaskOutput);

                }
                else
                {
                    oppotaskOutput = (from c in ctx.OPPOTASK
                                      where c.SYSOPPOTASK == oppotask.sysOppotask
                                      select c).FirstOrDefault();
                    if (oppotaskOutput != null)
                    {
                        oppotaskOutput = Mapper.Map<OppotaskDto, OPPOTASK>(oppotask, oppotaskOutput);
                        
                    }
                    else throw new Exception("OPPOTASK with id " + oppotask.sysOppotask + " not found!");

                }

                if (oppotask.sysoppo > 0)
                {
                    int status = ctx.ExecuteStoreQuery<int>("select status from oppo where sysoppo=" + oppotask.sysoppo, null).FirstOrDefault();
                    ////-	In Bearbeitung (sobald eine Aktivität einen anderen Status als „Aktivität erstellt“ hat
                    if (status < 1 && oppotask.status > 0)
                    {
                        ctx.ExecuteStoreCommand("update oppo set status=1 where sysoppo=" + oppotask.sysoppo, null);
                    }
                }


                ctx.SaveChanges();
                SearchCache.entityChanged("OPPOTASK");
                SearchCache.entityChanged("OPPO");
                oppotaskDto = Mapper.Map<OPPOTASK, OppotaskDto>(oppotaskOutput);
                oppotaskDto.crtuserName = ctx.ExecuteStoreQuery<String>("select trim(crt.name)||' '||trim(crt.vorname) from wfuser crt where syswfuser=" + oppotask.syscrtuser, null).FirstOrDefault();
                oppotaskDto.oppoiamcode = ctx.ExecuteStoreQuery<String>("select iamtype.code from iamtype, oppo, iam where oppo.sysiam=iam.sysiam and iam.sysiamtype=iamtype.sysiamtype and oppo.sysoppo=" + oppotaskDto.sysoppo, null).FirstOrDefault();
                //bool isPeuni = createOrUpdatePeuni("OPPOTASK", getSysPerole(), oppotaskOutput.SYSOPPOTASK);

                if ("VT".Equals(oppotaskDto.area) && oppotaskDto.sysid > 0)
                {
                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();

                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = oppotaskDto.sysid });
                    OppotaskDto addInfos = ctx.ExecuteStoreQuery<OppotaskDto>("select  angob.configid sa3angebot, angebot.angebot, angebot.sysid sysangebot from angebot, vt,angob where angob.sysob = angebot.sysid and angebot.sysid=vt.sysangebot and vt.sysid=:sysid", parameters.ToArray()).FirstOrDefault();
                    if (addInfos != null)
                    {
                        oppotaskDto.angebot = addInfos.angebot;
                        oppotaskDto.sa3angebot = addInfos.sa3angebot;
                        oppotaskDto.sysangebot = addInfos.sysangebot;
                    }
                }
            }



            return oppotaskDto;
        }

        /// <summary>
        /// updates/creates Wkt Account
        /// </summary>
        /// <param name="wktaccount"></param>
        /// <returns></returns>
        public WktaccountDto createOrUpdateWktAccount(WktaccountDto wktaccount)
        {
            long syswkt = 0;
            int dauerrechnung = 0;
            if (wktaccount.monatsrechnung == 1)
                dauerrechnung = 2;
            if (wktaccount.dauerrechnung == 1)
                dauerrechnung = 1;

            if (wktaccount.wktid < 0 || wktaccount.flagkd < 1)
            {
                wktaccount.sysit = wktaccount.wktid * -1;
                ItDto it = createOrUpdateIt(Mapper.Map<WktaccountDto, ItDto>(wktaccount));
                syswkt = it.sysit * -1;
                wktaccount.sysit = it.sysit;
                wktaccount.wktid = syswkt;

                using (DdOlExtended ctx = new DdOlExtended())
                {
                    ITOPTION popt = ctx.ExecuteStoreQuery<ITOPTION>("SELECT * from itoption where sysit=" + it.sysit, null).FirstOrDefault();

                    if (popt == null)
                    {
                        ctx.ExecuteStoreCommand("insert into ITOPTION(sysit,int01,int02) values(" + it.sysit + "," + dauerrechnung + "," + wktaccount.monatsrechnungart + ")", null);

                    }
                    else
                    {
                        ctx.ExecuteStoreCommand("update ITOPTION set int01=" + dauerrechnung + ", int02=" + wktaccount.monatsrechnungart + " where sysit=" + it.sysit, null);
                    }

                }
            }
            else
            {
                wktaccount.sysperson = wktaccount.wktid;
                /*AccountDto acc = createOrUpdateAccount(Mapper.Map<WktaccountDto, AccountDto>(wktaccount));
                syswkt = acc.entityId;
                wktaccount.sysperson = acc.entityId;
                wktaccount.wktid = syswkt;
                using (DdOlExtended ctx = new DdOlExtended())
                {
                    PEOPTION popt = ctx.ExecuteStoreQuery<PEOPTION>("SELECT * from peoption where sysid=" + syswkt, null).FirstOrDefault();

                    if (popt == null)
                    {
                        ctx.ExecuteStoreCommand("insert into PEOPTION(sysid,int01,int02) values(" + syswkt + "," + dauerrechnung + "," + wktaccount.monatsrechnungart + ")", null);
                        
                    }
                    else
                    {
                        ctx.ExecuteStoreCommand("update PEOPTION set int01=" + dauerrechnung + ", int02=" + wktaccount.monatsrechnungart + " where sysid=" + syswkt, null);
                    }

                }*/
                syswkt = wktaccount.sysperson;

            }
            createOrUpdateWktPartner(wktaccount);

            return getWktAccountDetails(syswkt);
        }

        /// <summary>
        /// updates/creates Oppo
        /// </summary>
        /// <param name="oppo"></param>
        /// <returns></returns>
        public AccountDto createOrUpdateAccount(AccountDto account)
        {
            if("IT".Equals(account.area))
            {
                ItDto updIt = createOrUpdateIt(Mapper.Map<AccountDto,ItDto>(account));
                AccountDto rval2 = Mapper.Map<ItDto, AccountDto>(updIt);
                rval2.area = "IT";
                rval2.sysid = updIt.sysit;
                return rval2;
            }
           

            PERSON personOutput = null;
            AccountDto accountDto = null;
            using (DdOlExtended ctx = new DdOlExtended())
            {
                if (account.sysperson == 0)
                {
                    personOutput = new PERSON();
                    personOutput = Mapper.Map<AccountDto, PERSON>(account, personOutput);

                    personOutput.SYSPERSON = 0;
                    ctx.AddToPERSON(personOutput);


                }
                else
                {
                    personOutput = (from p in ctx.PERSON
                                    where p.SYSPERSON == account.sysperson
                                    select p).FirstOrDefault();


                    if (personOutput != null)
                    {

                        long? oldref = personOutput.SYSREFERENZ;
                        personOutput = Mapper.Map<AccountDto, PERSON>(account, personOutput);
                        personOutput.SYSREFERENZ = oldref;//TODO sysreferenz nullable - Fix uk_reference problem as the new reference might never be different for a available person

                    }

                    else throw new Exception("Account with id " + account.sysperson + " not found!");
                }

                if (personOutput != null)
                {
                    if (account.waehrung != 0)
                    {
                        personOutput.SYSWAEHRUNG = (long)account.waehrung;

                    }
                    if (account.sysland != 0)
                        personOutput.SYSLAND = account.sysland;
                    if (account.sysadmadd != 0)
                        personOutput.SYSADMADD = account.sysadmadd;
                }
                ctx.SaveChanges();
                SearchCache.entityChanged("PERSON");
                long sysPrkgroup = account.sysPrkgroup;
                accountDto = Mapper.Map<PERSON, AccountDto>(personOutput);
                bool isPeuni = createOrUpdatePeuni("PERSON", getSysPerole(), personOutput.SYSPERSON);

                if (sysPrkgroup > 0)
                {
                    createOrUpdatePrkgroupm(new PrkgroupmDto[]
                        {
                            new PrkgroupmDto(){ addToPerson = 1, sysPrkgroup = sysPrkgroup, sysPerson = accountDto.sysperson  }
                        });
                }
            }

            return accountDto;
        }




        /// <summary>
        /// updates/creates PartnerDto
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public PartnerDto createOrUpdatePartner(PartnerDto account)
        {

            PERSON personOutput = null;
            PartnerDto accountDto = null;
            using (DdOlExtended ctx = new DdOlExtended())
            {
                if (account.sysperson == 0)//neuer partner
                {
                    personOutput = new PERSON();
                    personOutput = Mapper.Map<PartnerDto, PERSON>(account, personOutput);
                    if (account.waehrung != 0)
                    {
                        personOutput.SYSWAEHRUNG= (long)account.waehrung;

                    }
                    

                    personOutput.SYSPERSON = 0;
                    ctx.AddToPERSON(personOutput);

                }
                else//vorhandener Partner
                {
                    personOutput = (from p in ctx.PERSON
                                    where p.SYSPERSON == account.sysperson
                                    select p).FirstOrDefault();


                    if (personOutput != null)
                    {
                        long? oldref = personOutput.SYSREFERENZ;
                       
                        
                        personOutput = Mapper.Map<PartnerDto, PERSON>(account, personOutput);
                        personOutput.SYSREFERENZ = oldref;//TODO sysreferenz nullable - Fix uk_reference problem as the new reference might never be different for a available person

                        if (account.waehrung != 0)
                        {
                            personOutput.SYSWAEHRUNG =(long)account.waehrung;

                        }
                        
                    }

                    else throw new Exception("Account with id " + account.sysperson + " not found!");
                }

                ctx.SaveChanges();

                accountDto = Mapper.Map<PERSON, PartnerDto>(personOutput);
                PTRELATE pr = null;
                if (account.sysperson1 > 0)
                {

                    if (account.sysptrelate > 0)
                    {
                        pr = (from p in ctx.PTRELATE
                              where p.SYSPTRELATE == account.sysptrelate
                              select p).FirstOrDefault();
                    }
                    else
                    {
                        pr = (from p in ctx.PTRELATE
                              where p.SYSPERSON2 == accountDto.sysperson && p.SYSPERSON1 == account.sysperson1 && p.TYPCODE == account.typCode && p.ADDITIONALINFO == account.additionalInfo
                              select p).FirstOrDefault();
                    }
                    if (pr == null)
                    {
                        pr = new PTRELATE();
                        pr.SYSPERSON2 = accountDto.sysperson;
                        pr.SYSPERSON1 = account.sysperson1;

                        pr.TYPCODE = account.typCode;
                        pr.FUNCCODE = account.funcCode;
                        pr.ADDITIONALINFO = account.additionalInfo;
                        pr.RELBEGINNDATE = account.relbeginndate;
                        pr.RELENDDATE = account.relenddate;
                        pr.ACTIVEFLAG = account.activeFlag;

                        ctx.AddToPTRELATE(pr);

                    }
                    if (pr != null)
                    {
                        pr.TYPCODE = account.typCode;
                        pr.FUNCCODE = account.funcCode;
                        pr.ADDITIONALINFO = account.additionalInfo;
                        pr.RELBEGINNDATE = account.relbeginndate;
                        pr.RELENDDATE = account.relenddate;
                        pr.ACTIVEFLAG = account.activeFlag;
                    }

                }
                ctx.SaveChanges();
                SearchCache.entityChanged("PERSON");
                if (pr != null)
                {
                    accountDto = Mapper.Map<PTRELATE, PartnerDto>(pr, accountDto);
                    SearchCache.entityChanged("CONTACT");
                    SearchCache.entityChanged("PTRELATE");
                    SearchCache.entityChanged("PTASK");
                    SearchCache.entityChanged("APPTMT");
                    SearchCache.entityChanged("PTRELATE");
                    SearchCache.entityChanged("PARTNER");

                }
            }

            return accountDto;
        }

        /// <summary>
        /// updates/creates BeteiligterDto
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public BeteiligterDto createOrUpdateBeteiligter(BeteiligterDto account)
        {

            PERSON personOutput = null;
            BeteiligterDto accountDto = null;
            using (DdOlExtended ctx = new DdOlExtended())
            {
                if (account.sysperson == 0)//neuer beteiligter
                {
                    personOutput = new PERSON();
                    personOutput = Mapper.Map<BeteiligterDto, PERSON>(account, personOutput);
                    if (account.waehrung != 0)
                    {
                        personOutput.SYSWAEHRUNG=(long)account.waehrung;

                    }
                    

                    personOutput.SYSPERSON = 0;
                    ctx.AddToPERSON(personOutput);

                }
                else//vorhandener Beteiligter
                {
                    personOutput = (from p in ctx.PERSON
                                    where p.SYSPERSON == account.sysperson
                                    select p).FirstOrDefault();


                    if (personOutput != null)
                    {

                        personOutput = Mapper.Map<BeteiligterDto, PERSON>(account, personOutput);

                        if (account.waehrung != 0)
                        {
                            personOutput.SYSWAEHRUNG = (long)account.waehrung;

                        }
                       
                    }

                    else throw new Exception("Account with id " + account.sysperson + " not found!");
                }

                ctx.SaveChanges();
                accountDto = Mapper.Map<PERSON, BeteiligterDto>(personOutput);


                CRMNM nm = null;
                if (account.sysidparent > 0)
                {
                    nm = (from p in ctx.CRMNM
                          where p.SYSCRMNM == account.syscrmnm
                          select p).FirstOrDefault();

                    if (nm == null)
                        nm = (from p in ctx.CRMNM
                              where p.PARENTAREA.Equals("OPPO")
                              && p.CHILDAREA.Equals("PERSON")
                              && p.SYSIDPARENT == account.sysidparent
                              && p.SYSIDCHILD == account.sysperson
                              select p).FirstOrDefault();

                    if (nm == null)
                    {
                        nm = new CRMNM();
                        nm.SYSIDPARENT = account.sysidparent;
                        nm.PARENTAREA = "OPPO";
                        nm.SYSIDCHILD = account.sysperson;
                        nm.CHILDAREA = "PERSON";
                        nm.TYPCODE = account.typCode;
                        nm.ADDITIONALINFO = account.additionalInfo;
                        nm.ACTIVEFLAG = 1;
                        ctx.AddToCRMNM(nm);


                    }
                    else
                    {
                        nm.TYPCODE = account.typCode;
                        nm.ADDITIONALINFO = account.additionalInfo;
                        nm.ACTIVEFLAG = account.activeFlag;

                    }

                }
                ctx.SaveChanges();
                SearchCache.entityChanged("PERSON");
                SearchCache.entityChanged("CRMNM");

                if (nm != null)
                {

                    accountDto = Mapper.Map<CRMNM, BeteiligterDto>(nm, accountDto);
                }




            }

            return accountDto;
        }

        /// <summary>
        /// updates/creates Konto
        /// </summary>
        /// <param name="oppo"></param>
        /// <returns></returns>
        public KontoDto createOrUpdateKonto(KontoDto konto)
        {
            KONTO kontoOutput = null;
            KontoDto kontoDto = null;
            using (DdOlExtended ctx = new DdOlExtended())
            {
                if (konto.syskonto == 0)
                {
                    kontoOutput = new KONTO();
                    kontoOutput = Mapper.Map<KontoDto, KONTO>(konto, kontoOutput);
                    
                  
                    int nextrang = 20;

                    if(konto.syskontoTp==0)
                    {
                        nextrang = ctx.ExecuteStoreQuery<int>("select nvl(max(rang),19)+1 from konto where sysperson=" + konto.sysperson).FirstOrDefault();
                    }
                    else
                    {
                        int baserang = ctx.ExecuteStoreQuery<int>("select rang from kontotp where syskontotp=" + konto.syskontoTp).FirstOrDefault();
                        nextrang = ctx.ExecuteStoreQuery<int>("select nvl(max(rang),"+(baserang-1)+")+1 from konto where syskontotp=" + konto.syskontoTp+" and sysperson=" + konto.sysperson).FirstOrDefault();
                    }
                    

                    kontoOutput.RANG = nextrang;

                    kontoOutput.SYSKONTO = 0;
                    ctx.AddToKONTO(kontoOutput);

                }
                else
                {
                    kontoOutput = (from c in ctx.KONTO
                                   where c.SYSKONTO == konto.syskonto
                                   select c).FirstOrDefault();
                    if (kontoOutput != null)
                    {
                        if (konto.readOnly > 0) //KontoOutput auch checken. (Im Moment ist readonly nur im Dto, aber noch nicht in der Edmx 27.6.2013)
                            throw new Exception("Änderung nicht möglich, es liegen unbezahlte bereits zur Überweisung freigegebene ERs vor.");
                        kontoOutput = Mapper.Map<KontoDto, KONTO>(konto, kontoOutput);

                  

                    }
                    else throw new Exception("konto with id " + konto.syskonto + " not found!");
                }

                ctx.SaveChanges();
                SearchCache.entityChanged("KONTO");
                kontoDto = Mapper.Map<KONTO, KontoDto>(kontoOutput);
            }


            return kontoDto;
        }

        /// <summary>
        /// updates/creates Itkonto
        /// </summary>
        /// <param name="oppo"></param>
        /// <returns></returns>
        public ItkontoDto createOrUpdateItkonto(ItkontoDto itkonto)
        {
            ITKONTO itkontoOutput = null;
            ItkontoDto itkontoDto = null;
            using (DdOlExtended ctx = new DdOlExtended())
            {
                if (itkonto.sysitkonto == 0)
                {
                    itkontoOutput = new ITKONTO();
                    itkontoOutput = Mapper.Map<ItkontoDto, ITKONTO>(itkonto, itkontoOutput);

                    itkontoOutput.SYSITKONTO = 0;
                    ctx.AddToITKONTO(itkontoOutput);

                }
                else
                {
                    itkontoOutput = (from c in ctx.ITKONTO
                                     where c.SYSITKONTO == itkonto.sysitkonto
                                     select c).FirstOrDefault();
                    if (itkontoOutput != null)
                    {
                        if (itkonto.readOnly > 0) //ItkontoOutput auch checken. (Im Moment ist readonly nur im Dto, aber noch nicht in der Edmx 27.6.2013)
                            throw new Exception("Änderung nicht möglich, es liegen unbezahlte bereits zur Überweisung freigegebene ERs vor.");
                        itkontoOutput = Mapper.Map<ItkontoDto, ITKONTO>(itkonto, itkontoOutput);

                    }
                    else throw new Exception("itkonto with id " + itkonto.sysitkonto + " not found!");
                }

                ctx.SaveChanges();
                SearchCache.entityChanged("ITKONTO");
                itkontoDto = Mapper.Map<ITKONTO, ItkontoDto>(itkontoOutput);
            }


            return itkontoDto;
        }

        /// <summary>
        /// updates/creates PartnerRelation
        /// currently never called
        /// </summary>
        /// <param name="ptrelate"></param>
        /// <returns></returns>
        public PtrelateDto[] createOrUpdatePtrelate(PtrelateDto[] ptrelate)
        {
            return ptrelate;
            /*
            PTRELATE ptrelateOutput = null;
            using (DdOlExtended ctx = new DdOlExtended())
            {
                foreach (PtrelateDto dto in ptrelate)
                {
                    if (dto.addToPerson == 0)
                    {
                        if (dto.sysPtrelate > 0)
                        {
                            ptrelateOutput = (from p in ctx.PTRELATE
                                              where p.SYSPTRELATE == dto.sysPtrelate
                                              select p).FirstOrDefault();
                            ctx.DeleteObject(ptrelateOutput);
                        }
                    }
                    else if (dto.sysPtrelate == 0)
                    {
                        ptrelateOutput = new PTRELATE();
                        ptrelateOutput = Mapper.Map<PtrelateDto, PTRELATE>(dto, ptrelateOutput);
                        ptrelateOutput.SYSPTRELATE = 0;

                        ctx.AddToPTRELATE(ptrelateOutput);
                        SearchCache.entityChanged("PTRELATE");
                    }
                    else
                    {
                        ptrelateOutput = (from p in ctx.PTRELATE
                                          where p.SYSPTRELATE == dto.sysPtrelate
                                          select p).FirstOrDefault();
                        if (ptrelateOutput != null)
                            ptrelateOutput = Mapper.Map<PtrelateDto, PTRELATE>(dto, ptrelateOutput);
                        else throw new Exception("Ptrelate with id " + dto.sysPtrelate + " not found!");
                    }
                }
                ctx.SaveChanges();
            }
            return null;// Mapper.Map<PTRELATE, PtrelateDto>(ptrelateOutput);
             * */
        }


        /// <summary>
        /// updates/creates PartnerRelation
        /// currently never called
        /// </summary>
        /// <param name="crmnm"></param>
        /// <returns></returns>
        public CrmnmDto[] createOrUpdateCrmnm(CrmnmDto[] crmnm)
        {

            return crmnm;// Mapper.Map<CRMNM, CrmnmDto>(crmnmOutput);
        }


        /// <summary>
        /// updates/creates CrmProdukte
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public CrmprDto createOrUpdateCrmProdukte(CrmprDto crmpr)
        {
            CRMPR crmprOutput = null;

            using (DdOlExtended ctx = new DdOlExtended())
            {
                if (crmpr.sysCrmPr == 0)
                {
                    crmprOutput = new CRMPR();
                    crmprOutput = Mapper.Map<CrmprDto, CRMPR>(crmpr, crmprOutput);

                    if (crmpr.sysPrproduct != 0)
                        crmprOutput.SYSPRPRODUCT=crmpr.sysPrproduct;

                    crmprOutput.SYSCRMPR = 0;
                    ctx.AddToCRMPR(crmprOutput);

                }
                else
                {
                    crmprOutput = (from p in ctx.CRMPR
                                   where p.SYSCRMPR == crmpr.sysCrmPr
                                   select p).FirstOrDefault();
                    if (crmprOutput != null)
                    {
                        crmprOutput = Mapper.Map<CrmprDto, CRMPR>(crmpr, crmprOutput);

                        if (crmpr.sysPrproduct != 0)
                            crmprOutput.SYSPRPRODUCT=crmpr.sysPrproduct;
                    }
                    else throw new Exception("Crmpr with id " + crmpr.sysCrmPr + " not found!");
                }

                ctx.SaveChanges();
                SearchCache.entityChanged("CRMPR");
            }
            return Mapper.Map<CRMPR, CrmprDto>(crmprOutput);
        }

        /// <summary>
        /// updates/creates Kategorie
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public ItemcatDto createOrUpdateItemcat(ItemcatDto itemcat)
        {
            ITEMCAT itemcatOutput = null;
            //using (DdOlExtended ctx = new DdOlExtended())
            using (DdOwExtended ctxOw = new DdOwExtended())
            {
                if (itemcat.sysItemCat == 0)
                {
                    itemcatOutput = new ITEMCAT();
                    itemcatOutput = Mapper.Map<ItemcatDto, ITEMCAT>(itemcat, itemcatOutput);
                    itemcatOutput.SYSITEMCAT = 0;
                    ctxOw.AddToITEMCAT(itemcatOutput);

                }
                else
                {
                    itemcatOutput = (from p in ctxOw.ITEMCAT
                                     where p.SYSITEMCAT == itemcat.sysItemCat
                                     select p).FirstOrDefault();
                    if (itemcatOutput != null)
                        itemcatOutput = Mapper.Map<ItemcatDto, ITEMCAT>(itemcat, itemcatOutput);
                    else throw new Exception("Itemcat with id " + itemcat.sysItemCat + " not found!");
                }

                ctxOw.SaveChanges();
                SearchCache.entityChanged("ITEMCAT");
            }
            return Mapper.Map<ITEMCAT, ItemcatDto>(itemcatOutput);
        }


        /// <summary>
        /// updates/creates ItemKategorien
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public ItemcatmDto createOrUpdateItemcatm(ItemcatmDto itemcatm)
        {
            ITEMCATM itemcatmOutput = null;
            ItemcatmDto itemcatmDto = null;
            //using (DdOlExtended ctx = new DdOlExtended())
            using (DdOwExtended ctxOw = new DdOwExtended())
            {
                if (itemcatm.sysItemCatm == 0)
                {

                    ITEMCATM result = (from item in ctxOw.ITEMCATM
                                       where item.SYSID == itemcatm.sysid && item.AREA == itemcatm.area && item.ITEMCAT.SYSITEMCAT == itemcatm.sysItemCat
                                       select item)
                                      .FirstOrDefault();

                    if (result == null)
                    {
                        itemcatmOutput = new ITEMCATM();
                        itemcatmOutput = Mapper.Map<ItemcatmDto, ITEMCATM>(itemcatm, itemcatmOutput);
                        
                        itemcatmOutput.SYSITEMCATM = 0;
                        ctxOw.AddToITEMCATM(itemcatmOutput);

                    }
                }
                else
                {
                    itemcatmOutput = (from p in ctxOw.ITEMCATM
                                      where p.SYSITEMCATM == itemcatm.sysItemCatm
                                      select p).FirstOrDefault();
                    if (itemcatmOutput != null)
                    {
                        itemcatmOutput = Mapper.Map<ItemcatmDto, ITEMCATM>(itemcatm, itemcatmOutput);

                       

                    }
                    else throw new Exception("Itemcatm with id " + itemcatm.sysItemCatm + " not found!");
                }

                ctxOw.SaveChanges();
                SearchCache.entityChanged("ITEMCATM");
                itemcatmDto = Mapper.Map<ITEMCATM, ItemcatmDto>(itemcatmOutput);
            }
            return itemcatmDto;
        }

        /// <summary>
        /// updates/creates Dmsdoc
        /// </summary>
        /// <param name="dmsdoc"></param>
        /// <returns></returns>
        public DmsdocDto createOrUpdateDmsdoc(DmsdocDto dmsdoc)
        {
            DMSDOC dmsdocOutput = null;
            DmsdocDto dmsdocDto = null;

            using (DdOwExtended ctxOw = new DdOwExtended())
            {
                if (dmsdoc.sysdmsdoc == 0)
                {
                    dmsdocOutput = new DMSDOC();
                    dmsdocOutput = Mapper.Map<DmsdocDto, DMSDOC>(dmsdoc, dmsdocOutput);
                    dmsdocOutput.SYSDMSDOC = 0;

                    ctxOw.AddToDMSDOC(dmsdocOutput);

                    if(dmsdoc.area!=null&& dmsdoc.area.Length>0)
                    {
                        ctxOw.SaveChanges();
                        DMSDOCAREA area = new DMSDOCAREA();
                        area.AREA = dmsdoc.area;
                        area.SYSID = dmsdoc.sysid;
                        ctxOw.AddToDMSDOCAREA(area);
                        area.DMSDOCReference.EntityKey = ctxOw.getEntityKey(typeof(DMSDOC), dmsdocOutput.SYSDMSDOC);
                    }
                    
                }
                else
                {
                    dmsdocOutput = (from p in ctxOw.DMSDOC
                                    where p.SYSDMSDOC == dmsdoc.sysdmsdoc
                                    select p).FirstOrDefault();
                    if (dmsdocOutput != null)
                    {
                        dmsdocOutput = Mapper.Map<DmsdocDto, DMSDOC>(dmsdoc, dmsdocOutput);
                    }
                    else throw new Exception("DDMSDoc with id " + dmsdoc.sysdmsdoc + " not found!");
                }

                ctxOw.SaveChanges();
                String name = dmsdoc.name;
                //TODO remove after edmx
                List<Devart.Data.Oracle.OracleParameter> parameters =
                               new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "name", Value =name });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysdmsdoc", Value = dmsdocOutput.SYSDMSDOC });
                ctxOw.ExecuteStoreCommand("update dmsdoc set name=:name where sysdmsdoc=:sysdmsdoc", parameters.ToArray());

                SearchCache.entityChanged("DMSDOC");
                dmsdocDto = Mapper.Map<DMSDOC, DmsdocDto>(dmsdocOutput);
                dmsdocDto.name = name;
            }
            return dmsdocDto;
        }


        /// <summary>
        /// updates/creates Attachement
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public FileattDto createOrUpdateFileatt(FileattDto fileatt)
        {
            FILEATT fileattOutput = null;
            FileattDto fileattDto = null;
            //using (DdOlExtended ctx = new DdOlExtended())
            using (DdOwExtended ctxOw = new DdOwExtended())
            {
                if (fileatt.sysFileAtt == 0)
                {
                    fileattOutput = new FILEATT();
                    fileattOutput = Mapper.Map<FileattDto, FILEATT>(fileatt, fileattOutput);
                    fileattOutput.SYSFILEATT = 0;

                    if (fileatt.sysApptmt != 0)
                    {
                        fileattOutput.APPTMTReference.EntityKey = ctxOw.getEntityKey(typeof(APPTMT), fileatt.sysApptmt);
                    }
                    if (fileatt.sysMailMsg != 0)
                    {
                        fileattOutput.MAILMSGReference.EntityKey = ctxOw.getEntityKey(typeof(MAILMSG), fileatt.sysMailMsg);
                    }
                    if (fileatt.sysPtask != 0)
                    {
                        fileattOutput.PTASKReference.EntityKey = ctxOw.getEntityKey(typeof(PTASK), fileatt.sysPtask);
                    }

                    ctxOw.AddToFILEATT(fileattOutput);

                }
                else
                {
                    fileattOutput = (from p in ctxOw.FILEATT
                                     where p.SYSFILEATT == fileatt.sysFileAtt
                                     select p).FirstOrDefault();
                    if (fileattOutput != null)
                    {
                        fileattOutput = Mapper.Map<FileattDto, FILEATT>(fileatt, fileattOutput);
                        if (fileatt.sysApptmt != 0)
                        {
                            fileattOutput.APPTMTReference.EntityKey = ctxOw.getEntityKey(typeof(APPTMT), fileatt.sysApptmt);
                        }
                        if (fileatt.sysMailMsg != 0)
                        {
                            fileattOutput.MAILMSGReference.EntityKey = ctxOw.getEntityKey(typeof(MAILMSG), fileatt.sysMailMsg);
                        }
                        if (fileatt.sysPtask != 0)
                        {
                            fileattOutput.PTASKReference.EntityKey = ctxOw.getEntityKey(typeof(PTASK), fileatt.sysPtask);
                        }
                    }
                    else throw new Exception("Fileatt with id " + fileatt.sysFileAtt + " not found!");
                }

                ctxOw.SaveChanges();
                SearchCache.entityChanged("FILEATT");
                fileattDto = Mapper.Map<FILEATT, FileattDto>(fileattOutput);
            }
            return fileattDto;
        }

        /// <summary>
        /// updates/creates Reminder
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public ReminderDto createOrUpdateReminder(ReminderDto reminder)
        {
            REMINDER reminderOutput = null;
            ReminderDto reminderDto = null;
            //using (DdOlExtended ctx = new DdOlExtended())
            using (DdOwExtended ctxOw = new DdOwExtended())
            {
                if (reminder.sysReminder == 0)
                {
                    reminderOutput = new REMINDER();
                    reminderOutput = Mapper.Map<ReminderDto, REMINDER>(reminder, reminderOutput);
                    reminderOutput.SYSREMINDER = 0;

                    if (reminder.sysApptmt != 0)
                    {
                        reminderOutput.APPTMTReference.EntityKey = ctxOw.getEntityKey(typeof(APPTMT), reminder.sysApptmt);
                    }
                    if (reminder.sysMailMsg != 0)
                    {
                        reminderOutput.MAILMSGReference.EntityKey = ctxOw.getEntityKey(typeof(MAILMSG), reminder.sysMailMsg);
                    }
                    if (reminder.sysPtask != 0)
                    {
                        reminderOutput.PTASKReference.EntityKey = ctxOw.getEntityKey(typeof(PTASK), reminder.sysPtask);
                    }

                    ctxOw.AddToREMINDER(reminderOutput);

                }
                else
                {
                    reminderOutput = (from p in ctxOw.REMINDER
                                      where p.SYSREMINDER == reminder.sysReminder
                                      select p).FirstOrDefault();
                    if (reminderOutput != null)
                    {
                        reminderOutput = Mapper.Map<ReminderDto, REMINDER>(reminder, reminderOutput);

                        if (reminder.sysApptmt != 0)
                        {
                            reminderOutput.APPTMTReference.EntityKey = ctxOw.getEntityKey(typeof(APPTMT), reminder.sysApptmt);
                        }
                        if (reminder.sysMailMsg != 0)
                        {
                            reminderOutput.MAILMSGReference.EntityKey = ctxOw.getEntityKey(typeof(MAILMSG), reminder.sysMailMsg);
                        }
                        if (reminder.sysPtask != 0)
                        {
                            reminderOutput.PTASKReference.EntityKey = ctxOw.getEntityKey(typeof(PTASK), reminder.sysPtask);
                        }
                    }
                    else throw new Exception("Reminder with id " + reminder.sysReminder + " not found!");
                }

                ctxOw.SaveChanges();
                SearchCache.entityChanged("REMINDER");
                reminderDto = Mapper.Map<REMINDER, ReminderDto>(reminderOutput);
            }
            return reminderDto;
        }

        /// <summary>
        /// updates/creates Recurrence
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public RecurrDto createOrUpdateRecurr(RecurrDto recurr)
        {
            RECURR recurrOutput = null;
            RecurrDto recurrDto = null;
            //using (DdOlExtended ctx = new DdOlExtended())
            using (DdOwExtended ctxOw = new DdOwExtended())
            {

                RECURR result = (from rec in ctxOw.RECURR
                                 where (recurr.sysApptmt != 0 && rec.APPTMT.SYSAPPTMT == recurr.sysApptmt) || (recurr.sysPtask != 0 && rec.PTASK.SYSPTASK == recurr.sysPtask)
                                 select rec).FirstOrDefault();

                if (result != null)
                {
                    recurr.sysRecurr = result.SYSRECURR;
                }
                if (recurr.sysRecurr == 0)
                {


                    recurrOutput = new RECURR();
                    recurrOutput = Mapper.Map<RecurrDto, RECURR>(recurr, recurrOutput);
                    recurrOutput.SYSRECURR = 0;

                    if (recurr.sysApptmt != 0)
                    {
                        recurrOutput.APPTMTReference.EntityKey = ctxOw.getEntityKey(typeof(APPTMT), recurr.sysApptmt);
                    }
                    if (recurr.sysPtask != 0)
                    {
                        recurrOutput.PTASKReference.EntityKey = ctxOw.getEntityKey(typeof(PTASK), recurr.sysPtask);
                    }

                    ctxOw.AddToRECURR(recurrOutput);

                }
                else
                {
                    recurrOutput = (from p in ctxOw.RECURR
                                    where p.SYSRECURR == recurr.sysRecurr
                                    select p).FirstOrDefault();
                    if (recurrOutput != null)
                    {
                        recurrOutput = Mapper.Map<RecurrDto, RECURR>(recurr, recurrOutput);
                        if (recurr.sysApptmt != 0)
                        {
                            recurrOutput.APPTMTReference.EntityKey = ctxOw.getEntityKey(typeof(APPTMT), recurr.sysApptmt);
                        }
                        if (recurr.sysPtask != 0)
                        {
                            recurrOutput.PTASKReference.EntityKey = ctxOw.getEntityKey(typeof(PTASK), recurr.sysPtask);
                        }


                    }
                    else throw new Exception("Recurr with id " + recurr.sysRecurr + " not found!");
                }

                ctxOw.SaveChanges();
                SearchCache.entityChanged("RECURR");
                recurrDto = Mapper.Map<RECURR, RecurrDto>(recurrOutput);
            }
            return recurrDto;
        }

        /// <summary>
        /// updates/creates Checklist
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public PrunDto createOrUpdatePrun(PrunDto prun)
        {
            PRUN prunOutput = null;
            PrunDto prunDto = null;
            //using (DdOlExtended ctx = new DdOlExtended())
            using (DdOwExtended ctxOw = new DdOwExtended())
            {
                if (prun.sysPrun == 0)
                {
                    prunOutput = new PRUN();
                    prunOutput = Mapper.Map<PrunDto, PRUN>(prun, prunOutput);
                    prunOutput.SYSPRUN = 0;

                    if (prun.sysPtype != 0)
                    {
                        prunOutput.PTYPEReference.EntityKey = ctxOw.getEntityKey(typeof(PTYPE), prun.sysPtype);
                    }

                    ctxOw.AddToPRUN (prunOutput);

                }
                else
                {
                    prunOutput = (from p in ctxOw.PRUN
                                  where p.SYSPRUN == prun.sysPrun
                                  select p).FirstOrDefault();
                    if (prunOutput != null)
                    {
                        prunOutput = Mapper.Map<PrunDto, PRUN>(prun, prunOutput);
                        if (prun.sysPtype != 0)
                        {
                            prunOutput.PTYPEReference.EntityKey = ctxOw.getEntityKey(typeof(PTYPE), prun.sysPtype);
                        }

                    }
                    else throw new Exception("Prun with id " + prun.sysPrun + " not found!");
                }

                ctxOw.SaveChanges();
                SearchCache.entityChanged("PRUN");
                prunDto = Mapper.Map<PRUN, PrunDto>(prunOutput);

            }
            return prunDto;
        }

        /// <summary>
        /// updates/creates Checklisttype
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public PtypeDto createOrUpdatePtype(PtypeDto ptype)
        {
            PTYPE ptypeOutput = null;
            //using (DdOlExtended ctx = new DdOlExtended())
            using (DdOwExtended ctxOw = new DdOwExtended())
            {
                if (ptype.sysPtype == 0)
                {
                    ptypeOutput = new PTYPE();
                    ptypeOutput = Mapper.Map<PtypeDto, PTYPE>(ptype, ptypeOutput);
                    ptypeOutput.SYSPTYPE = 0;

                    ctxOw.AddToPTYPE(ptypeOutput);

                }
                else
                {
                    ptypeOutput = (from p in ctxOw.PTYPE
                                   where p.SYSPTYPE == ptype.sysPtype
                                   select p).FirstOrDefault();
                    if(ptype.checkresult==null && ptype.sysptask==0)
                    { 
                        if (ptypeOutput != null)
                            ptypeOutput = Mapper.Map<PtypeDto, PTYPE>(ptype, ptypeOutput);
                        else throw new Exception("Ptype with id " + ptype.sysPtype + " not found!");
                    }
                    long syspchecker = ctxOw.ExecuteStoreQuery<long>("select syspchecker from pchecker,ptask where ptask.sysptype="+ptype.sysPtype+" and ptask.sysptask=pchecker.sysptask and pchecker.syswfuser="+this.sysWfuser,null).FirstOrDefault();
                    if(syspchecker>0)
                    {
                        int cflag = ctxOw.ExecuteStoreQuery<int>("select completeflag from ptask where sysptask=" + ptype.sysptask, null).FirstOrDefault();
                        //ptype.art=20-> sammelaufgabe Wenn ein zugewiesener Prüfer PCHECKER:SYSWFUSER Bestätigt / Ablehnt PTASK:COMPLETEFLAG auf 1 setzen... Falls ein anderer Prüfer die gleiche Prüfaufgabe offen
                        //hat, wird seine Änderung nicht übernommen (PTASK:COMPLETEFLAG bereits auf 1, keine Änderung zulassen).
                        //PTYPE:ART=10: PTASK:COMPLETEFLAG auf 1 setzen wenn der letzte Prüfer Bestätigt / Abgelehnt hat.
                        

                        if (ptype.art == 10 || (ptype.art == 20 && cflag == 0))//Nur die erste Antwort (Bestätigung / Ablehnung) eines Prüfers sollen übernommen werden. Die Angaben der anderen Prüfer werden ignoriert.
                        {
                            int time = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(DateTime.Now);
                            List<Devart.Data.Oracle.OracleParameter> parameters =
                                new List<Devart.Data.Oracle.OracleParameter>();
                            parameters.Add(new Devart.Data.Oracle.OracleParameter
                            {
                                ParameterName = "result",
                                Value = ptype.checkresult
                            });
                            parameters.Add(new Devart.Data.Oracle.OracleParameter {ParameterName = "time", Value = time});
                            parameters.Add(new Devart.Data.Oracle.OracleParameter
                            {
                                ParameterName = "syspchecker",
                                Value = syspchecker
                            });
                            ctxOw.ExecuteStoreCommand(
                                "update pchecker set result=:result, resultdate=trunc(sysdate), resulttime=:time where syspchecker=:syspchecker",
                                parameters.ToArray());
                            ctxOw.SaveChanges();
                        }

                        //anzahl prüfer die nicht beantwortet haben
                        long checknok = ctxOw.ExecuteStoreQuery<long>("select count(*) from pchecker,ptask where pchecker.result is null and ptask.sysptype=" + ptype.sysPtype + " and ptask.sysptask=pchecker.sysptask and ptask.sysptask="+ ptype.sysptask, null).FirstOrDefault();


                        if ((ptype.art == 20 && cflag == 0) || (ptype.art == 10 && checknok == 0)) //no more results pending
                        {
                            List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                            parameters.Add(new Devart.Data.Oracle.OracleParameter {ParameterName = "syswfuser", Value = this.sysWfuser});
                            parameters.Add(new Devart.Data.Oracle.OracleParameter {ParameterName = "sysptask", Value = ptype.sysptask});
                            ctxOw.ExecuteStoreCommand("update ptask set completeuser=:syswfuser, completeflag=1 where sysptask=:sysptask", parameters.ToArray());
                        }
                        if ((ptype.art == 20 && cflag == 0) || (ptype.art == 10))//Bei jedem Prüfer (Bestätigung/Ablehnung) muss ein EAIHOT-Satz angelegt werden. 
                        {
                            //beim Erledigen der Prüfaufgaben einen EAIHOT-Satz anlegen
                            EaihotDto eaihot = new EaihotDto();
                            eaihot.syseaiart = 147;
                            eaihot.oltable = "PCHECKER";
                            eaihot.sysoltable = syspchecker;
                            eaihot.code = "PRUEFAUFGABE_CHECKED";
                            eaihot.syswfuser = this.sysWfuser;
                            eaihot.inputparameter1 = ptype.checkresult;
                            eaihot.eve = 1;
                            eaihot.clientart = 0;
                            eaihot.prozessstatus = 0;
                            eaihot.hostcomputer = "*";

                            EAIHOT rval = new EAIHOT();
                            rval = Mapper.Map<EaihotDto, EAIHOT>(eaihot, rval);
                            rval.SYSEAIHOT = 0;
                            rval.EAIARTReference.EntityKey = ctxOw.getEntityKey(typeof(EAIART), eaihot.syseaiart.Value);
                            ctxOw.AddToEAIHOT(rval);
                            ctxOw.SaveChanges();

                        }
                    }

                    
                }

                ctxOw.SaveChanges();
                SearchCache.entityChanged("PTYPE");
            }
            return getPtypeDetails(ptypeOutput.SYSPTYPE);
            //return Mapper.Map<PTYPE, PtypeDto>(ptypeOutput);
        }

        /// <summary>
        /// updates/creates Check
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public PrunstepDto createOrUpdatePrunstep(PrunstepDto prunstep)
        {
            PRUNSTEP prunstepOutput = null;
            PrunstepDto prunstepDto = null;
            //using (DdOlExtended ctx = new DdOlExtended())
            using (DdOwExtended ctxOw = new DdOwExtended())
            {
                if (prunstep.sysPrunStep == 0)
                {
                    prunstepOutput = new PRUNSTEP();
                    prunstepOutput = Mapper.Map<PrunstepDto, PRUNSTEP>(prunstep, prunstepOutput);
                    prunstepOutput.SYSPRUNSTEP = 0;

                    if (prunstep.sysPstep != 0)
                    {
                        prunstepOutput.PSTEPReference.EntityKey = ctxOw.getEntityKey(typeof(PSTEP), prunstep.sysPstep);
                    }
                    if (prunstep.sysPrun != 0)
                    {
                        prunstepOutput.PRUNReference.EntityKey = ctxOw.getEntityKey(typeof(PRUN), prunstep.sysPrun);
                    }

                    ctxOw.AddToPRUNSTEP(prunstepOutput);

                }
                else
                {
                    prunstepOutput = (from p in ctxOw.PRUNSTEP
                                      where p.SYSPRUNSTEP == prunstep.sysPrunStep
                                      select p).FirstOrDefault();
                    if (prunstepOutput != null)
                    {
                        prunstepOutput = Mapper.Map<PrunstepDto, PRUNSTEP>(prunstep, prunstepOutput);

                        if (prunstep.sysPstep != 0)
                        {
                            prunstepOutput.PSTEPReference.EntityKey = ctxOw.getEntityKey(typeof(PSTEP), prunstep.sysPstep);
                        }
                        if (prunstep.sysPrun != 0)
                        {
                            prunstepOutput.PRUNReference.EntityKey = ctxOw.getEntityKey(typeof(PRUN), prunstep.sysPrun);
                        }
                    }
                    else throw new Exception("Prunstep with id " + prunstep.sysPrunStep + " not found!");
                }

                ctxOw.SaveChanges();
                SearchCache.entityChanged("PRUNSTEP");
                prunstepDto = Mapper.Map<PRUNSTEP, PrunstepDto>(prunstepOutput);
            }
            return prunstepDto;
        }

        /// <summary>
        /// updates/creates Checktype
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public PstepDto createOrUpdatePstep(PstepDto pstep)
        {
            PSTEP pstepOutput = null;
            PstepDto pstepDto = null;
            //using (DdOlExtended ctx = new DdOlExtended())
            using (DdOwExtended ctxOw = new DdOwExtended())
            {
                if (pstep.sysPstep == 0)
                {
                    pstepOutput = new PSTEP();
                    pstepOutput = Mapper.Map<PstepDto, PSTEP>(pstep, pstepOutput);
                    pstepOutput.SYSPSTEP = 0;

                    if (pstep.sysPtype != 0)
                    {
                        pstepOutput.PTYPEReference.EntityKey = ctxOw.getEntityKey(typeof(PTYPE), pstep.sysPstep);
                    }

                    ctxOw.AddToPSTEP(pstepOutput);

                }
                else
                {
                    pstepOutput = (from p in ctxOw.PSTEP
                                   where p.SYSPSTEP == pstep.sysPstep
                                   select p).FirstOrDefault();
                    if (pstepOutput != null)
                    {
                        pstepOutput = Mapper.Map<PstepDto, PSTEP>(pstep, pstepOutput);
                        if (pstep.sysPtype != 0)
                        {
                            pstepOutput.PTYPEReference.EntityKey = ctxOw.getEntityKey(typeof(PTYPE), pstep.sysPstep);
                        }

                    }
                    else throw new Exception("Pstep with id " + pstep.sysPstep + " not found!");
                }

                ctxOw.SaveChanges();
                SearchCache.entityChanged("PSTEP");
                pstepDto = Mapper.Map<PSTEP, PstepDto>(pstepOutput);
            }
            return pstepDto;
        }

        /// <summary>
        /// updates/creates Segment
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public SegDto createOrUpdateSeg(SegDto seg)
        {
            SEG segOutput = null;
            using (DdOlExtended ctx = new DdOlExtended())
            {
                if (seg.sysSeg == 0)
                {
                    segOutput = new SEG();
                    segOutput = Mapper.Map<SegDto, SEG>(seg, segOutput);
                    segOutput.SYSSEG = 0;

                    ctx.AddToSEG(segOutput);

                }
                else
                {
                    segOutput = (from p in ctx.SEG
                                 where p.SYSSEG == seg.sysSeg
                                 select p).FirstOrDefault();
                    if (segOutput != null)
                        segOutput = Mapper.Map<SegDto, SEG>(seg, segOutput);
                    else throw new Exception("Seg with id " + seg.sysSeg + " not found!");
                }

                ctx.SaveChanges();
                SearchCache.entityChanged("SEG");
            }
            return Mapper.Map<SEG, SegDto>(segOutput);
        }

        /// <summary>
        /// updates/creates SegmentKampagne
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public SegcDto createOrUpdateSegc(SegcDto segc)
        {
            SEGC segcOutput = null;
            using (DdOlExtended ctx = new DdOlExtended())
            {
                if (segc.sysSeg == 0)
                {
                    segcOutput = new SEGC();
                    segcOutput = Mapper.Map<SegcDto, SEGC>(segc, segcOutput);


                    

                    segcOutput.SYSSEGC = 0;
                    ctx.AddToSEGC(segcOutput);

                }
                else
                {
                    segcOutput = (from p in ctx.SEGC
                                  where p.SYSSEGC == segc.sysSegc
                                  select p).FirstOrDefault();
                    if (segcOutput != null)
                    {
                        segcOutput = Mapper.Map<SegcDto, SEGC>(segc, segcOutput);

                       
                    }
                    else throw new Exception("Segc with id " + segc.sysSegc + " not found!");
                }

                ctx.SaveChanges();
                SearchCache.entityChanged("SEGC");
            }
            return Mapper.Map<SEGC, SegcDto>(segcOutput);
        }


        /// <summary>
        /// updates/creates Prkgroup
        /// </summary>
        /// <param name="prkgroup"></param>
        /// <returns></returns>
        public PrkgroupDto createOrUpdatePrkgroup(PrkgroupDto prkgroup)
        {
            PRKGROUP prkgroupOutput = null;
            using (DdOlExtended ctx = new DdOlExtended())
            {
                if (prkgroup.sysPrkgroup == 0)
                {
                    prkgroupOutput = new PRKGROUP();
                    prkgroupOutput = Mapper.Map<PrkgroupDto, PRKGROUP>(prkgroup, prkgroupOutput);
                    prkgroupOutput.SYSPRKGROUP = 0;

                    ctx.AddToPRKGROUP(prkgroupOutput);

                }
                else
                {
                    prkgroupOutput = (from p in ctx.PRKGROUP
                                      where p.SYSPRKGROUP == prkgroup.sysPrkgroup
                                      select p).FirstOrDefault();
                    if (prkgroupOutput != null)
                        prkgroupOutput = Mapper.Map<PrkgroupDto, PRKGROUP>(prkgroup, prkgroupOutput);
                    else throw new Exception("Prkgroup with id " + prkgroup.sysPrkgroup + " not found!");
                }

                ctx.SaveChanges();
                SearchCache.entityChanged("PRKGROUP");
            }
            return Mapper.Map<PRKGROUP, PrkgroupDto>(prkgroupOutput);
        }

        /// <summary>
        /// updates/creates ddlkppos
        /// </summary>
        /// <param name="ddlkppos"></param>
        /// <returns></returns>
        public DdlkpsposDto[] createOrUpdateDdlkpspos(DdlkpsposDto[] ddlkpspos)
        {

            if (ddlkpspos == null || ddlkpspos.Length == 0) return null;
            List<DdlkpsposDto> endresult = new List<DdlkpsposDto>();
            List<DDLKPSPOS> result = new List<DDLKPSPOS>();
            using (DdOdExtended ctx = new DdOdExtended())
            {
                foreach (DdlkpsposDto pos in ddlkpspos)
                {
                    if (pos.sysddlkpspos > 0 && (pos.value == null || "".Equals(pos.value)))
                    {
                        //Soll aus der DB entfernt werden
                        DDLKPSPOS output = (from p in ctx.DDLKPSPOS
                                            where p.SYSDDLKPSPOS == pos.sysddlkpspos
                                            select p).FirstOrDefault();
                        if (output != null)
                            ctx.DeleteObject(output);

                        pos.sysddlkpspos = 0;
                        endresult.Add(pos);
                    }
                    else if (pos.sysddlkpspos == 0 && (pos.value == null || "".Equals(pos.value)))
                    {
                        //Existiert nicht in der DB und hat keinen Wert
                        //Do nothing
                    }
                    else if (pos.sysddlkpspos <= 0)
                    {
                        //Existiert nicht in der DB
                        result.Add(CreateDdlkpspos(pos, ctx));
                    }
                    else
                    {
                        //Existiert in der DB und wird geupdatet
                        DDLKPSPOS output = (from p in ctx.DDLKPSPOS
                                            where p.SYSDDLKPSPOS == pos.sysddlkpspos
                                            select p).FirstOrDefault();
                        if (output != null)
                        {
                            output = Mapper.Map<DdlkpsposDto, DDLKPSPOS>(pos, output);

                            if (pos.sysddlkpcol != 0)
                            {
                                output.DDLKPCOLReference.EntityKey = ctx.getEntityKey(typeof(DDLKPCOL), pos.sysddlkpcol);
                            }
                            if (pos.sysddlkppos != 0)
                            {
                                output.DDLKPPOSReference.EntityKey = ctx.getEntityKey(typeof(DDLKPPOS), pos.sysddlkppos);
                            }
                            result.Add(output);
                        }
                        else
                            //Falls es doch noch nicht in der DB war, wird es hinzugefügt
                            result.Add(CreateDdlkpspos(pos, ctx));
                        //throw new Exception("DDLKPSPOS with id " + pos.sysddlkpspos + " not found!");
                    }

                }
                ctx.SaveChanges();
            }
            endresult.AddRange(Mapper.Map<List<DDLKPSPOS>, List<DdlkpsposDto>>(result));
            SearchCache.entityChanged("DDLKPSPOS");
            SearchCache.entityChanged("DDLKPPOS");
            SearchCache.entityChanged("DDLKPCOL");
            SearchCache.entityChanged("DDLKPRUB");

            return endresult.ToArray();
        }

        private DDLKPSPOS CreateDdlkpspos(DdlkpsposDto pos, DdOdExtended ctx)
        {
            DDLKPSPOS output = new DDLKPSPOS();

            output = Mapper.Map<DdlkpsposDto, DDLKPSPOS>(pos, output);
            output.SYSDDLKPSPOS = 0;

            if (pos.sysddlkpcol != 0)
            {
                output.DDLKPCOLReference.EntityKey = ctx.getEntityKey(typeof(DDLKPCOL), pos.sysddlkpcol);
            }
            if (pos.sysddlkppos != 0)
            {
                output.DDLKPPOSReference.EntityKey = ctx.getEntityKey(typeof(DDLKPPOS), pos.sysddlkppos);
            }

            ctx.AddToDDLKPSPOS(output);
            SearchCache.entityChanged("DDLKPSPOS");
            return output;
        }


        /// <summary>
        /// updates/creates Prkgroup
        /// </summary>
        /// <param name="prkgroup"></param>
        /// <returns></returns>
        public PrkgroupmDto createOrUpdatePrkgroupm(PrkgroupmDto[] prkgroups)
        {
            PRKGROUPM prkgroupOutput = null;
            if (prkgroups == null || prkgroups.Length == 0) return null;
            using (DdOlExtended ctx = new DdOlExtended())
            {
                foreach (PrkgroupmDto prkgroup in prkgroups)
                {
                    if (prkgroup.addToPerson == 1)//hinzufügen
                    {
                        if (prkgroup.sysPrkgroupm <= 0)//look if in db already
                        {
                            List<Devart.Data.Oracle.OracleParameter> pars = new List<Devart.Data.Oracle.OracleParameter>();
                            pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperson", Value = prkgroup.sysPerson });
                            pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysprkgroup", Value = prkgroup.sysPrkgroup });
                            prkgroup.sysPrkgroupm = ctx.ExecuteStoreQuery<long>(QUERY_CHECK_PRKGROUP, pars.ToArray()).FirstOrDefault();
                        }
                        if (prkgroup.sysPrkgroupm <= 0)
                        {




                            prkgroupOutput = new PRKGROUPM();
                            prkgroupOutput = Mapper.Map<PrkgroupmDto, PRKGROUPM>(prkgroup, prkgroupOutput);
                            prkgroupOutput.SYSPRKGROUPM = 0;
                            

                            ctx.AddToPRKGROUPM(prkgroupOutput);

                        }
                        else
                        {
                            prkgroupOutput = (from p in ctx.PRKGROUPM
                                              where p.SYSPRKGROUPM == prkgroup.sysPrkgroupm
                                              select p).FirstOrDefault();
                            if (prkgroupOutput != null)
                                prkgroupOutput = Mapper.Map<PrkgroupmDto, PRKGROUPM>(prkgroup, prkgroupOutput);
                            else throw new Exception("Prkgroupm with id " + prkgroup.sysPrkgroupm + " not found!");
                        }
                    }
                    else
                    {
                        if (prkgroup.sysPrkgroupm < 0) prkgroup.sysPrkgroupm *= -1;
                        if (prkgroup.sysPrkgroupm > 0)
                        {
                            prkgroupOutput = (from p in ctx.PRKGROUPM
                                              where p.SYSPRKGROUPM == prkgroup.sysPrkgroupm
                                              select p).FirstOrDefault();
                            if (prkgroupOutput != null)
                                ctx.DeleteObject(prkgroupOutput);

                        }
                    }
                }
                ctx.SaveChanges();
                SearchCache.entityChanged("PRKGROUPM");
                SearchCache.entityChanged("PRKGROUP");
            }
            return null;// Mapper.Map<PRKGROUPM, PrkgroupmDto>(prkgroupOutput);
        }


        /// <summary>
        /// updates/creates Stickynote
        /// </summary>
        /// <param name="stickynotes"></param>
        /// <returns></returns>
        public StickynoteDto[] createOrUpdateStickynotes(StickynoteDto[] stickynotes)
        {
            STICKYNOTE stickynoteOutput = null;
            NOTIZ notizOutput = null;

            List<StickynoteDto> rval = new List<StickynoteDto>();

            using (DdOlExtended ctxOl = new DdOlExtended())
            using (DdOwExtended ctxOw = new DdOwExtended())
            {
                foreach (StickynoteDto stickynote in stickynotes)
                {
                    if (stickynote.sysStickynote == 0)
                    {
                        stickynoteOutput = new STICKYNOTE();
                        stickynoteOutput = Mapper.Map<StickynoteDto, STICKYNOTE>(stickynote, stickynoteOutput);
                        stickynoteOutput.SYSSTICKYNOTE = 0;
                        stickynoteOutput.STICKYFLAG = 0;
                        stickynoteOutput.PRIVATFLAG = 0;
                        stickynoteOutput.DELETEFLAG = 0;
                        stickynoteOutput.ACTIVEFLAG = 1;
                        stickynoteOutput.SHOWFLAG = 1;
                        stickynoteOutput.SYSCRTDATE = DateTime.Now;
                        stickynoteOutput.SYSCRTTIME = (long)Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(stickynoteOutput.SYSCRTDATE.Value);
                        if (stickynote.area != null)
                            stickynoteOutput.AREA = stickynote.area;//.ToUpper();

                        ctxOw.AddToSTICKYNOTE(stickynoteOutput);


                    }
                    else
                    {
                        if (stickynote.SYSNOTIZ > 0)
                        {
                            notizOutput = (from p in ctxOl.NOTIZ
                                           where p.SYSNOTIZ == stickynote.SYSNOTIZ
                                           select p).FirstOrDefault();
                            if (notizOutput != null)
                            {
                                //notizOutput = Mapper.Map<StickynoteDto, NOTIZ>(stickynote, notizOutput);
                                notizOutput.GEBIET = stickynote.GEBIET;
                                notizOutput.NOTIZ1 = stickynote.inhalt;
                                notizOutput.SYSGEBIET = stickynote.SYSGEBIET;
                                notizOutput.SYSNOTIZ = stickynote.SYSNOTIZ;
                                notizOutput.SYSPERSON = stickynote.SYSPERSON;
                                notizOutput.SYSVT = stickynote.SYSVT;
                                notizOutput.SYSWFUSER = stickynote.SYSWFUSER;
                                notizOutput.DATUM = stickynote.DATUM;
                                notizOutput.ZEIT = (long)Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(notizOutput.DATUM.Value);
                                if (stickynote.GEBIET != null)
                                    notizOutput.GEBIET = stickynote.GEBIET;//.ToUpper();
                            }
                            else throw new Exception("Notiz with id " + stickynote.SYSNOTIZ + " not found!");
                            //notizOutput.DATUM = DateTime.Now;
                            //notizOutput.ZEIT = (long)Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(notizOutput.ZEIT.Value);
                        }
                        else
                        {
                            stickynoteOutput = (from p in ctxOw.STICKYNOTE
                                                where p.SYSSTICKYNOTE == stickynote.sysStickynote
                                                select p).FirstOrDefault();
                            if (stickynoteOutput != null)
                                stickynoteOutput = Mapper.Map<StickynoteDto, STICKYNOTE>(stickynote, stickynoteOutput);
                            else throw new Exception("Stickynote with id " + stickynote.sysStickynote + " not found!");
                            stickynoteOutput.SYSCHGDATE = DateTime.Now;
                            stickynoteOutput.SYSCHGTIME = (long)Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(stickynoteOutput.SYSCHGDATE.Value);
                            if (stickynote.area != null)
                                stickynoteOutput.AREA = stickynote.area;//.ToUpper();
                        }
                    }

                    if (stickynote.SYSNOTIZ > 0)
                    {
                        ctxOl.SaveChanges();
                        rval.Add(Mapper.Map<NOTIZ, StickynoteDto>(notizOutput));
                    }
                    else
                    {
                        ctxOw.SaveChanges();
                        StickynoteDto o = Mapper.Map<STICKYNOTE, StickynoteDto>(stickynoteOutput);
                        if (o.sysCrtUser > 0)
                            o.wfuserName = ctxOw.ExecuteStoreQuery<String>("select  WFUSER.NAME wfuserName from wfuser where syswfuser=" + o.sysCrtUser, null).FirstOrDefault();
                        rval.Add(o);
                    }


                }
                SearchCache.entityChanged("STICKYNOTE");
            }
            return rval.ToArray();
        }


        /// <summary>
        /// updates/creates Stickytype
        /// </summary>
        /// <param name="prkgroup"></param>
        /// <returns></returns>
        public StickytypeDto createOrUpdateStickytype(StickytypeDto stickytype)
        {
            STICKYTYPE stickytypeOutput = null;
            using (DdOwExtended ctxOw = new DdOwExtended())
            {
                if (stickytype.sysStickytype == 0)
                {
                    stickytypeOutput = new STICKYTYPE();
                    stickytypeOutput = Mapper.Map<StickytypeDto, STICKYTYPE>(stickytype, stickytypeOutput);
                    stickytypeOutput.SYSSTICKYTYPE = 0;

                    ctxOw.AddToSTICKYTYPE(stickytypeOutput);

                }
                else
                {
                    stickytypeOutput = (from p in ctxOw.STICKYTYPE
                                        where p.SYSSTICKYTYPE == stickytype.sysStickytype
                                        select p).FirstOrDefault();
                    if (stickytypeOutput != null)
                        stickytypeOutput = Mapper.Map<StickytypeDto, STICKYTYPE>(stickytype, stickytypeOutput);
                    else throw new Exception("Stickynote with id " + stickytype.sysStickytype + " not found!");
                }

                ctxOw.SaveChanges();
                SearchCache.entityChanged("STICKYTYPE");
            }
            return Mapper.Map<STICKYTYPE, StickytypeDto>(stickytypeOutput);
        }


        public bool createOrUpdatePeuni(String area, long sysperole, long sysid, bool forceChangePerole = false)
        {
            PEUNI peuni = null;
            using (DdOlExtended ctxOl = new DdOlExtended())
            {
                peuni = (from p in ctxOl.PEUNI
                         where p.AREA == area && p.SYSID == sysid
                         select p).FirstOrDefault();
                if (peuni == null)
                {
                    peuni = new PEUNI();
                    peuni.SYSPEROLE=sysperole;
                    peuni.AREA = area;
                    peuni.SYSID = sysid;
                    ctxOl.AddToPEUNI(peuni);
                    ctxOl.SaveChanges();
                }
                
                else if (forceChangePerole)
                {
                    //Falls ein Ownerwechsel passiert
                    peuni.SYSPEROLE = sysperole;
                    ctxOl.SaveChanges();
                }
                else
                {
                    return false;
                }

            }
            return true;

        }


        /// <summary>
        /// updates/creates besuchsbericht
        /// </summary>
        /// <param name="besuchsbericht"></param>
        /// <returns></returns>
        public BesuchsberichtDto createOrUpdateBesuchsbericht(BesuchsberichtDto besuchsbericht)
        {
            FileattDto fileatt = Mapper.Map(besuchsbericht, new FileattDto());
            var result = createOrUpdateFileatt(fileatt);
            return Mapper.Map<FileattDto, BesuchsberichtDto>(result);
        }

        /// <summary>
        /// updates/creates Wfsignature
        /// </summary>
        /// <param name="wfsignatureDto"></param>
        /// <param name="sysWfUser"></param>
        /// <returns></returns>
        public WfsignatureDto createOrUpdateWfsignature(WfsignatureDto wfsignatureDto)
        {

            throw new NotImplementedException("Currently not Allowed");
        }

        /// <summary>
        /// updates/creates ZEK
        /// </summary>
        /// <param name="zek"></param>
        /// <returns></returns>
        public ZekDto createOrUpdateZek(ZekDto zek)
        {
            //do nothing, we save zek request data in service

            ZEK zekOutput = null;
            /*
            using (DdOlExtended ctx = new DdOlExtended())
            {
                if (zek.syszek == 0)
                {
                    zekOutput = new ZEK();
                    zekOutput = Mapper.Map<ZekDto, ZEK>(zek, zekOutput);
                    zekOutput.SYSZEK = 0;
                    ctx.AddToZEK(zekOutput);
                }
                else
                {
                    zekOutput = (from p in ctx.ZEK
                                 where p.SYSZEK == zek.syszek
                                 select p).FirstOrDefault();
                    if (zekOutput != null)
                        zekOutput = Mapper.Map<ZekDto, ZEK>(zek, zekOutput);
                    else throw new Exception("ZEK request with id " + zek.syszek + " not found!");
                }

                ctx.SaveChanges();
            }
            */
            return Mapper.Map<ZEK, ZekDto>(zekOutput);
        }


        #region Mail

        /// <summary>
        /// updates/creates Mailmsg
        /// </summary>
        /// <param name="oppo"></param>
        /// <returns></returns>
        public MailmsgDto createOrUpdateMailmsg(MailmsgDto mailmsg)
        {
            MAILMSG mailmsgOutput = null;
            using (DdOwExtended ctx = new DdOwExtended())
            {
                if (mailmsg.sysMailmsg == 0)//New MailMsg
                {
                    mailmsg.sysOwner = new CredentialContext().getMembershipInfo().sysWFUSER;
                    ContactDto contact = new ContactDto();
                    contact.sysPerson = mailmsg.sysPerson;
                    contact.sysOwner = mailmsg.sysOwner;
                    contact.way = 3;//EMail
                    contact.sysContactTp = 1;
                    contact.reason = "Mailing";
                    contact.reasonCode = "3";
                    contact.direction = 1;//Eingehend
                    contact = createOrUpdateContact(contact);
                    mailmsgOutput = new MAILMSG();
                    mailmsgOutput = Mapper.Map<MailmsgDto, MAILMSG>(mailmsg, mailmsgOutput);
                    mailmsgOutput.SYSMAILMSG = 0;
                    mailmsgOutput.SYSCONTACT = contact.sysContact;

                    ctx.AddToMAILMSG(mailmsgOutput);


                }
                else
                {
                    mailmsgOutput = (from p in ctx.MAILMSG
                                     where p.SYSMAILMSG == mailmsg.sysMailmsg
                                     select p).FirstOrDefault();
                    if (mailmsgOutput != null)
                        mailmsgOutput = Mapper.Map<MailmsgDto, MAILMSG>(mailmsg, mailmsgOutput);
                    else throw new Exception("Mailmsg with id " + mailmsg.sysMailmsg + " not found!");
                }

                ctx.SaveChanges();
                SearchCache.entityChanged("MAILMSG");
                bool isPeuni = createOrUpdatePeuni("MAILMSG", getSysPerole(), mailmsgOutput.SYSMAILMSG);
            }
            return Mapper.Map<MAILMSG, MailmsgDto>(mailmsgOutput);
        }



        /// <summary>
        /// updates/creates Appointment
        /// </summary>
        /// <param name="konto"></param>
        /// <returns></returns>
        public ApptmtDto createOrUpdateApptmt(ApptmtDto apptmt)
        {
            APPTMT apptmtOutput = null;

            if (apptmt.sysOwner == 0)
                apptmt.sysOwner = new CredentialContext().getMembershipInfo().sysWFUSER;

            using (DdOwExtended ctx = new DdOwExtended())
            {
                if (apptmt.sysApptmt == 0)
                {
                    apptmtOutput = new APPTMT();
                    apptmtOutput = Mapper.Map<ApptmtDto, APPTMT>(apptmt, apptmtOutput);
                    apptmtOutput.SYSAPPTMT = 0;

                    ctx.AddToAPPTMT(apptmtOutput);

                }
                else
                {
                    apptmtOutput = (from p in ctx.APPTMT
                                    where p.SYSAPPTMT == apptmt.sysApptmt
                                    select p).FirstOrDefault();

                    if (apptmtOutput != null)
                    {
                        if (apptmt.sysOwner != apptmtOutput.SYSOWNER)
                        {
                            //SearchCache.entityChanged("APPTMT");
                        }
                        apptmtOutput = Mapper.Map<ApptmtDto, APPTMT>(apptmt, apptmtOutput);
                    }
                    else throw new Exception("Apptmt with id " + apptmt.sysApptmt + " not found!");
                }

                ctx.SaveChanges();
                SearchCache.entityChanged("APPTMT");
                bool isPeuni = createOrUpdatePeuni("APPTMT", getSysPerole(), apptmtOutput.SYSAPPTMT, true);
            }
            return Mapper.Map<APPTMT, ApptmtDto>(apptmtOutput);
        }

        /// <summary>
        /// updates/creates Task
        /// </summary>
        /// <param name="konto"></param>
        /// <returns></returns>
        public PtaskDto createOrUpdatePtask(PtaskDto ptask)
        {
            PTASK ptaskOutput = null;
            if (ptask.sysOwner == 0)
                ptask.sysOwner = new CredentialContext().getMembershipInfo().sysWFUSER;

            using (DdOwExtended ctxOw = new DdOwExtended())
            {
                if (ptask.sysPtask == 0)
                {
                    ptaskOutput = new PTASK();
                    ptaskOutput = Mapper.Map<PtaskDto, PTASK>(ptask, ptaskOutput);
                    ptaskOutput.SYSPTASK = 0;

                    if (ptask.sysPtype != 0)
                    {
                        ptaskOutput.PTYPEReference.EntityKey = ctxOw.getEntityKey(typeof(PTASK), ptask.sysPtask);
                    }

                    ctxOw.AddToPTASK(ptaskOutput);

                }
                else
                {
                    ptaskOutput = (from p in ctxOw.PTASK
                                   where p.SYSPTASK == ptask.sysPtask
                                   select p).FirstOrDefault();
                    if (ptaskOutput != null)
                    {
                        if (ptask.sysOwner != ptaskOutput.SYSOWNER)
                        {
                            //SearchCache.entityChanged("PTASK");
                        }
                        ptaskOutput = Mapper.Map<PtaskDto, PTASK>(ptask, ptaskOutput);
                        if (ptask.sysPtype != 0)
                        {
                            ptaskOutput.PTYPEReference.EntityKey = ctxOw.getEntityKey(typeof(PTYPE), ptask.sysPtype);

                        }
                    }
                    else throw new Exception("Ptask with id " + ptask.sysPtask + " not found!");

                }
                ctxOw.SaveChanges();
                //TODO remove after edmx
                ctxOw.ExecuteStoreCommand("update PTASK set sysptasktyp=" + ptask.sysPtasktyp + " where sysptask=" + ptaskOutput.SYSPTASK, null);
                SearchCache.entityChanged("PTASK");
                bool isPeuni = createOrUpdatePeuni("PTASK", getSysPerole(), ptaskOutput.SYSPTASK, true);
            }
            PtaskDto rval = Mapper.Map<PTASK, PtaskDto>(ptaskOutput);
            //TODO remove after edmx
            rval.sysPtasktyp = ptask.sysPtasktyp;
            return rval;

        }


		/// <summary>
		/// Get a substring of the first N characters.
		/// </summary>
		public static string Truncate (string source, int length)
		{
			if (source != null && source.Length > length)
			{
				source = source.Substring (0, length);
			}
			return source;
		}

        /// <summary>
        /// Creates or updates a puser
        /// </summary>
        /// <param name="puser"></param>
        /// <returns></returns>
        public PuserDto createOrUpdatePuser(PuserDto puser)
        {
            String eaitype = "UPDATE";
            using (DdOlExtended ctx = new DdOlExtended())
            {
                try
                {
                    ctx.Connection.Open();//needs to be opened first to keep the session id!!!
                    long sysperolenew = 0, vkperole = 0;
                    try
                    {
                        String uid = System.Environment.UserName;// ServiceSecurityContext.Current.PrimaryIdentity.Name;
                        Devart.Data.Oracle.OracleParameter ctype = new Devart.Data.Oracle.OracleParameter { Value = 1, ParameterName = "pClientType", Direction = System.Data.ParameterDirection.Input, DbType = System.Data.DbType.Decimal };
                        Devart.Data.Oracle.OracleParameter pmach = new Devart.Data.Oracle.OracleParameter { Value = System.Environment.MachineName, ParameterName = "pMachine", Direction = System.Data.ParameterDirection.Input, DbType = System.Data.DbType.String };
                        Devart.Data.Oracle.OracleParameter posuser = new Devart.Data.Oracle.OracleParameter { Value = uid, ParameterName = "pOSUser", Direction = System.Data.ParameterDirection.Input, DbType = System.Data.DbType.String };
                        Devart.Data.Oracle.OracleParameter pprog = new Devart.Data.Oracle.OracleParameter { Value = "GateBANKNOW", ParameterName = "pProgram", Direction = System.Data.ParameterDirection.Input, DbType = System.Data.DbType.String };
                        Devart.Data.Oracle.OracleParameter puserid = new Devart.Data.Oracle.OracleParameter { Value = "", ParameterName = "pPortalUserID", Direction = System.Data.ParameterDirection.Input, DbType = System.Data.DbType.String };
                        Devart.Data.Oracle.OracleParameter pwfuser = new Devart.Data.Oracle.OracleParameter { Value = this.sysWfuser, ParameterName = "pSysWFUSER", Direction = System.Data.ParameterDirection.Input, DbType = System.Data.DbType.Decimal };

                        DbParameter[] prodpars = { ctype, pmach, posuser, pprog, puserid, pwfuser };
                        ctx.ExecuteProcedure("CIC.cic_sys.set_session_context", prodpars);

                    }
                    catch (Exception pe)
                    {
                        _log.Warn("cic_sys.set_session_context failed", pe);
                    }
                  

                    if (puser.sysperson == 0)
                    {
                        Devart.Data.Oracle.OracleParameter prolepar = new Devart.Data.Oracle.OracleParameter { Value = "PEROLE", ParameterName = "pNEArea", Direction = System.Data.ParameterDirection.Input, DbType = System.Data.DbType.String };
                        Devart.Data.Oracle.OracleParameter prolerval = new Devart.Data.Oracle.OracleParameter { Direction = System.Data.ParameterDirection.ReturnValue, OracleDbType = Devart.Data.Oracle.OracleDbType.Number };
                        object rval = ctx.ExecuteFunction("cic_sys.set_NE_Seq", new DbParameter[] { prolepar, prolerval });
                        sysperolenew = Convert.ToInt64(rval);
                    }
                    else
                    {
                        vkperole = ctx.ExecuteStoreQuery<long>("select sysperole from perole where perole.sysperson=" + puser.sysperson, null).FirstOrDefault();
                        if (vkperole == 0)
                        {
                            Devart.Data.Oracle.OracleParameter prolepar = new Devart.Data.Oracle.OracleParameter { Value = "PEROLE", ParameterName = "pNEArea", Direction = System.Data.ParameterDirection.Input, DbType = System.Data.DbType.String };
                            Devart.Data.Oracle.OracleParameter prolerval = new Devart.Data.Oracle.OracleParameter { Direction = System.Data.ParameterDirection.ReturnValue, OracleDbType = Devart.Data.Oracle.OracleDbType.Number };
                            object rval = ctx.ExecuteFunction("cic_sys.set_NE_Seq", new DbParameter[] { prolepar, prolerval });
                            sysperolenew = Convert.ToInt64(rval);
                        }
                        else
                        {
                            Devart.Data.Oracle.OracleParameter prolepar = new Devart.Data.Oracle.OracleParameter { Value = "PEROLE", ParameterName = "pNEArea", Direction = System.Data.ParameterDirection.Input, DbType = System.Data.DbType.String };
                            Devart.Data.Oracle.OracleParameter prolerval = new Devart.Data.Oracle.OracleParameter { Value = vkperole, ParameterName = "pNESysArea", Direction = System.Data.ParameterDirection.Input, OracleDbType = Devart.Data.Oracle.OracleDbType.Number };

                            ctx.ExecuteProcedure("CIC.cic_sys.set_NE", new DbParameter[] { prolepar, prolerval });
                        }
                    }

                    //Find users HD Role for the parent
                    long hdPerole = PeRoleUtil.FindRootPEROLEByRoleType(ctx, this.sysPerole, (long)RoleTypeTyp.HAENDLER);
                    bool deactivate = !String.IsNullOrEmpty(puser.inaktivgrund) && puser.validuntil != null;
                    if (!deactivate && (puser.adminflag == 0 && puser.syswfuser > 0 && puser.sysperole > 0))//nicht-neuer user, adminflag wird auf 0 gesetzt
                    {
                        //TODO validate if another admin for hdperole available, if not return;
                        //TODO validate if another admin for hdperole available, if not return;
                        List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "akt_SysPerole", Value = puser.sysperole });
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "NewValidBis", Value = DateTime.Now.AddDays(1) });
                        long admincount = ctx.ExecuteStoreQuery<long>("SELECT COUNT(rgm.sysrgm) FROM rgm, rgr, wfuser, perole ma WHERE rgr.name ='EPOS_ADMIN' AND rgr.sysrgr = rgm.sysrgr AND rgm.syswfuser = wfuser.syswfuser AND wfuser.sysperson = ma.sysperson AND ma.sysperole IN (SELECT perole.sysperole FROM perole, perole akt WHERE (perole.validuntil = TO_DATE ('01.01.0111','dd.mm.yyyy') OR perole.validuntil >= :NewValidBis  OR perole.validuntil IS NULL) AND (perole.validfrom = TO_DATE ('01.01.0111','dd.mm.yyyy') OR perole.validfrom <= :NewValidBis  OR perole.validfrom IS NULL) AND perole.sysperson > 0 AND perole.INACTIVEFLAG = 0 AND perole.sysperole <> akt.sysperole AND perole.sysparent = akt.sysparent AND akt.sysperole = :akt_SysPerole)", parameters.ToArray()).FirstOrDefault();
                        if (admincount == 0)
                            throw new Exception("Es muß mindestens ein Benutzer mit Admin-Recht für diesen Händler erhalten bleiben! Deaktivierung nicht möglich!");

                       /* long admincount = ctx.ExecuteStoreQuery<long>("select count(*) from perole p, RGR, RGM,  wfuser where rgr.sysrgr = rgm.sysrgr and rgr.name in ('EPOS_ADMIN') and rgm.syswfuser = wfuser.syswfuser and wfuser.sysperson =p.sysperson and p.sysparent =" + hdPerole, null).FirstOrDefault();
                        if (admincount == 0)
                            throw new Exception("Es muß mindestens ein Benutzer mit Admin-Recht für diesen Händler erhalten bleiben!");*/
                    }
                    if (deactivate)//deaktivieren wurde gedrückt
                    {
                        //TODO validate if another admin for hdperole available, if not return;
                        List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "akt_SysPerole", Value = puser.sysperole });
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "NewValidBis", Value = puser.validuntil.Value.AddDays(1) });
                        long admincount = ctx.ExecuteStoreQuery<long>("SELECT COUNT(rgm.sysrgm) FROM rgm, rgr, wfuser, perole ma WHERE rgr.name ='EPOS_ADMIN' AND rgr.sysrgr = rgm.sysrgr AND rgm.syswfuser = wfuser.syswfuser AND wfuser.sysperson = ma.sysperson AND ma.sysperole IN (SELECT perole.sysperole FROM perole, perole akt WHERE (perole.validuntil = TO_DATE ('01.01.0111','dd.mm.yyyy') OR perole.validuntil >= :NewValidBis  OR perole.validuntil IS NULL) AND (perole.validfrom = TO_DATE ('01.01.0111','dd.mm.yyyy') OR perole.validfrom <= :NewValidBis  OR perole.validfrom IS NULL) AND perole.sysperson > 0 AND perole.INACTIVEFLAG = 0 AND perole.sysperole <> akt.sysperole AND perole.sysparent = akt.sysparent AND akt.sysperole = :akt_SysPerole)", parameters.ToArray()).FirstOrDefault();
                        if (admincount == 0)
                            throw new Exception("Zum Deaktivierungsdatum des Mitarbeiters existiert in Ihrem Unternehmen kein weiterer Benutzer mit Adminrechten. Deaktivierung nicht möglich!");
                    }
                    //wenn syswfuser==leer
                    //person anlegen, wfuser anlegen, puser anlegen, perole anlegen, alle verknüpfen
                    //PERSON
                    int ahvcode = 99;
                    if (puser.ahv != null && puser.ahv.Length > 0)
                        ahvcode = 1;

                    if (puser.sysperson == 0)
                    {
                        PERSON pers = new PERSON();
                        pers = Mapper.Map<PuserDto, PERSON>(puser, pers);
                        pers.SYSLAND= puser.sysland;
                        pers.QUELLSTEUERPFLICHT = puser.steuerflag;
                        // pers.ANREDE = null;
                        pers.ANREDECODE = puser.anredecode;
						pers.EMAIL = puser.email;

                        pers.AHVNUMMER = puser.ahv;
                        pers.AHVCODE = ahvcode;
                        pers.FAX = puser.fax;
                        pers.TITELCODE = pers.ANREDECODE;

						// GET anrede (letter salutation)	(rh 20170301)
						List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter> ();
						parameters.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = "SYSCTLANG", Value = pers.SYSCTLANG });
						parameters.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = "ANREDECODE", Value = pers.ANREDECODE });
						string letterSalutation = ctx.ExecuteStoreQuery<string> (QUERYLETTERSALUTATION, parameters.ToArray ()).FirstOrDefault ();
						pers.ANREDE = letterSalutation;

						List<Devart.Data.Oracle.OracleParameter> parameters2 = new List<Devart.Data.Oracle.OracleParameter> ();
						parameters2.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = "SYSCTLANG", Value = pers.SYSCTLANG });
						parameters2.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = "ANREDECODE", Value = pers.ANREDECODE });
						string title = ctx.ExecuteStoreQuery<string> (QUERYTITLE, parameters2.ToArray ()).FirstOrDefault ();
						pers.TITEL = title;

						// rh alt 20170117: pers.SYSCTLANG = puser.sysctlangkorr;
						// rh: neu 20170117
						pers.SYSCTLANGKORR = puser.sysctlangkorr;
						pers.SYSCTLANG = puser.sysctlang;

						pers.SYSSTAAT= puser.sysstaat;

						pers.VORNAME = Truncate (pers.VORNAME, 60);		// VOR-NAME in PERSON: Limit 60 (rh 20170320)

                        ctx.AddToPERSON(pers);
                        ctx.SaveChanges();
                        puser.sysperson = pers.SYSPERSON;

                        PEOPTION peopt = new PEOPTION();
                        peopt.OPTION7 = puser.mobile;
                        peopt.OPTION3 = puser.email;
                        peopt.STR03 = puser.extuserid;
                        peopt.SYSID = pers.SYSPERSON;
                        ctx.AddToPEOPTION(peopt);
                        ctx.SaveChanges();
                        //korradresse von händler übernehmen (syswfuser/sysperole vom bo)
                        String korradr = @"insert into adresse(sysperson,rang,name,zusatz,vorname,anrede,anredecode,strasse,plz,ort,land,sysland,gueltigab,gueltigbis,hsnr,sysstaat,titel,email,sysctlang,email2,telefon,fax,handy,url)
                                             select  " + puser.sysperson + ",2,adresse.name,zusatz,vorname,anrede,anredecode,strasse,plz,ort,land,sysland,gueltigab,gueltigbis,hsnr,sysstaat,titel,email,sysctlang,email2,telefon,fax,handy,url from adresse,perole where adresse.sysperson=perole.sysperson and perole.sysroletype=2 and adresse.rang=2 and perole.sysperole=" + hdPerole;
                        ctx.ExecuteStoreCommand(korradr, null);
                    }
                    else
                    {
                        PERSON pers = (from p in ctx.PERSON
                                       where p.SYSPERSON == puser.sysperson
                                       select p).FirstOrDefault();
                        String etemp = pers.EMAIL;
                        String atemp = pers.ANREDE;
                       
						pers = Mapper.Map<PuserDto, PERSON>(puser, pers);
                        pers.SYSLAND= puser.sysland;
                        pers.QUELLSTEUERPFLICHT = puser.steuerflag;
						// pers.EMAIL = etemp;					
						pers.EMAIL = puser.email;
						// pers.ANREDE = atemp;					// rh 20170228: Resets any changes
						// rh 20170301: SET any changes to Anrede (and letter salutation)
						pers.ANREDECODE = puser.anredecode;
						pers.TITELCODE = puser.anredecode;

						// GET anrede (letter salutation) AND TITLE	(rh 20170301)
						List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter> ();
						parameters.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = "SYSCTLANG", Value = pers.SYSCTLANG });
						parameters.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = "ANREDECODE", Value = pers.ANREDECODE });
						string letterSalutation = ctx.ExecuteStoreQuery<string> (QUERYLETTERSALUTATION, parameters.ToArray ()).FirstOrDefault ();
						pers.ANREDE = letterSalutation;

						List<Devart.Data.Oracle.OracleParameter> parameters2 = new List<Devart.Data.Oracle.OracleParameter> ();
						parameters2.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = "SYSCTLANG", Value = pers.SYSCTLANG });
						parameters2.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = "ANREDECODE", Value = pers.ANREDECODE });
						string title = ctx.ExecuteStoreQuery<string> (QUERYTITLE, parameters2.ToArray ()).FirstOrDefault ();
						pers.TITEL = title;	

						pers.FAX = puser.fax;
                        pers.AHVNUMMER = puser.ahv;
                        pers.AHVCODE = ahvcode;
						
						// rh alt 20170117: pers.SYSCTLANG = puser.sysctlangkorr;
						// rh: neu 20170117
						pers.SYSCTLANGKORR = puser.sysctlangkorr;
						pers.SYSCTLANG = puser.sysctlang;

						pers.SYSSTAAT= puser.sysstaat;

						pers.VORNAME = Truncate (pers.VORNAME, 60);		// VOR-NAME in PERSON: Limit 60 (rh 20170320)

                        PEOPTION peopt = (from p in ctx.PEOPTION
                                          where p.SYSID == puser.sysperson
                                          select p).FirstOrDefault();
                        if (peopt == null)
                        {
                            peopt = new PEOPTION();

                            peopt.SYSID = puser.sysperson;
                            ctx.AddToPEOPTION(peopt);
                            ctx.SaveChanges();
                        }
                        peopt.OPTION7 = puser.mobile;
                        peopt.OPTION3 = puser.email;
                        peopt.STR03 = puser.extuserid;
                        ctx.SaveChanges();
                    }
                    String wfuserCode = null;
                    //WFUSER
                    if (puser.syswfuser == 0)
                    {
                        NkBuilder nkwfuser = new NkBuilder("WFUSER", "B2B");
                        wfuserCode = nkwfuser.getNextNumber();
                        CIC.Database.OL.EF4.Model.WFUSER wf = new CIC.Database.OL.EF4.Model.WFUSER();
                        wf = Mapper.Map<PuserDto, CIC.Database.OL.EF4.Model.WFUSER>(puser, wf);
                        wf.CODE = wfuserCode;

						wf.NAME = Truncate (wf.NAME, 30);			// NAME in WFUSER: Limit 30		(rh 20170320)
						wf.VORNAME = Truncate (wf.VORNAME, 30);		// VOR-NAME in WFUSER: Limit 30 (rh 20170320)
                        ctx.AddToWFUSER(wf);
                        ctx.SaveChanges();
                        puser.syswfuser = wf.SYSWFUSER;

                        //korradresse von händler übernehmen (syswfuser/sysperole vom bo)
                        String wfvalidity = @"insert into cic.vausradd(sysvausradd,disabled) values("
                            + puser.syswfuser + ",1)";
                        ctx.ExecuteStoreCommand(wfvalidity, null);
                    }
                    else
                    {
                        CIC.Database.OL.EF4.Model.WFUSER wf = (from p in ctx.WFUSER
                                                                   where p.SYSWFUSER == puser.syswfuser
                                                                   select p).FirstOrDefault();
                        wfuserCode = wf.CODE;

						wf = Mapper.Map<PuserDto, CIC.Database.OL.EF4.Model.WFUSER> (puser, wf);
						
						wf.NAME = Truncate (wf.NAME, 30);			// NAME in WFUSER: Limit 30		(rh 20170320)
						wf.VORNAME = Truncate (wf.VORNAME, 30);		// VOR-NAME in WFUSER: Limit 30 (rh 20170320)
						
						ctx.SaveChanges ();
                    }

                    bool doagain = false;
                    String aktZustand = null, aktAttribut = null;
                    //PEROLE
                    if (vkperole == 0)
                    {
                        long roletype = ctx.ExecuteStoreQuery<long>("select sysroletype from roletype where (flagintern is null or flagintern=0) and typ=" + ((int)RoleTypeTyp.VERKAEUFER), null).FirstOrDefault();

                     

                        List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperole", Value = sysperolenew });
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "name", Value = puser.name + " " + puser.vorname });
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperson", Value = puser.sysperson });
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysparent", Value = hdPerole });
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "validfrom", Value = puser.validfrom });
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "validuntil", Value = puser.validuntil });
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysroletype", Value = roletype });
                        //Beim Einfügen des neuen Mitarbeiters wird Zustand/Attribut=Aktivierung/pendent vergeben.
                        ctx.ExecuteStoreCommand(@"insert into perole (sysperole,name,sysperson,sightfieldlevel,sysparent,validfrom,validuntil,inactiveflag,attribut,zustand,sysroletype,zustandam) 
                                                values(:sysperole,:name,:sysperson,1,:sysparent,:validfrom,:validuntil,1,'pendent','Aktivierung',:sysroletype,sysdate)", parameters.ToArray());
                        ctx.SaveChanges();
                        eaitype = "INSERT";
                        puser.sysperole = sysperolenew;
                        vkperole = puser.sysperole;
                        doagain = true;
                    }
                    else
                    {
                        CIC.Database.OL.EF4.Model.PEROLE pr = (from p in ctx.PEROLE
                                                                   where p.SYSPEROLE == vkperole
                                                                   select p).FirstOrDefault();
                        pr.VALIDFROM = puser.validfrom;
                        pr.VALIDUNTIL = puser.validuntil;
                        aktZustand = pr.ZUSTAND;
                        aktAttribut = pr.ATTRIBUT;
                        if (!String.IsNullOrEmpty(puser.inaktivgrund) && puser.validuntil != null)
                        {
                            pr.ZUSTAND = "Deaktivierung";
                            pr.ZUSTANDAM = DateTime.Now;
                            pr.ATTRIBUT = "pendent";
                            pr.INAKTIVGRUND = puser.inaktivgrund;
                            eaitype = "SPERRE";
                        }
                        else if (!"pendent".Equals(pr.ATTRIBUT))//Falls Attribut "pendent" vorliegt, wird keine Änderung des Zustands/Attributs benötigt
                        {
                            if ("Aktivierung".Equals(pr.ZUSTAND) && "abgelehnt".Equals(pr.ATTRIBUT))
                            {
                                pr.ATTRIBUT = "pendent";
                            }
                            else
                            {
                                pr.ZUSTAND = "Änderung";
                                pr.ZUSTANDAM = DateTime.Now;
                                pr.ATTRIBUT = "pendent";
                            }
                        }
                        ctx.SaveChanges();
                    }

					puser.name = Truncate (puser.name, 60);				// NAME in puser: Limit 60 (rh 20170320)
					puser.vorname = Truncate (puser.vorname, 60);		// VOR-NAME in puser: Limit 60 (rh 20170320)
					
					//PUSER
                    if (puser.syspuser == 0)
                    {
                        CIC.Database.OL.EF4.Model.PUSER pu = new CIC.Database.OL.EF4.Model.PUSER();
                        pu = Mapper.Map<PuserDto, CIC.Database.OL.EF4.Model.PUSER>(puser, pu);
                        pu.DISABLED = 1;
                        pu.DISABLEDREASON = "Neuanlage aus ePOS";
                        pu.EXTERNEID = wfuserCode;
                        pu.SYSPERSON=puser.sysperson;
                        if (puser.sysperole > 0)
                            pu.SYSDEFAULTPEROLE = puser.sysperole;
                        ctx.AddToPUSER(pu);
                        ctx.SaveChanges();
                        puser.syspuser = pu.SYSPUSER;

                    }
                    else
                    {
                        CIC.Database.OL.EF4.Model.PUSER pu = (from p in ctx.PUSER
                                                                  where p.SYSPUSER == puser.syspuser
                                                                  select p).FirstOrDefault();
                        pu.SYSPERSON= puser.sysperson;
                        if (puser.sysperole > 0)
                            pu.SYSDEFAULTPEROLE = puser.sysperole;
                        pu = Mapper.Map<PuserDto, CIC.Database.OL.EF4.Model.PUSER>(puser, pu);
                    }




                    //kont
                    if (puser.sysblz == 0 && puser.blz!=null && puser.blz.Length>0)
                        if (puser.blz != null) puser.sysblz = long.Parse(puser.blz);
                    if ((puser.iban != null && puser.iban.Length > 0) || puser.sysblz>0 || (puser.kontoinhaber!=null&&puser.kontoinhaber.Length>0))
                    {
                        long syskonto = ctx.ExecuteStoreQuery<long>("select syskonto from konto where sysperson=" + puser.sysperson + " and rang=37").FirstOrDefault();
                        if (syskonto == 0)
                        {
                            KONTO kto = new KONTO();
                            kto.SYSPERSON=puser.sysperson;
                            kto.IBAN = puser.iban;
                            kto.KONTONR = puser.kontonummer;
                            kto.KONTOINHABER = puser.kontoinhaber;
                            
                            kto.SYSBLZ = puser.sysblz;
                            kto.RANG = 37;
                            ctx.AddToKONTO(kto);
                            ctx.SaveChanges();
                        }
                        else
                        {
                            KONTO kto = (from p in ctx.KONTO
                                         where p.SYSKONTO == syskonto
                                         select p).FirstOrDefault();
                            kto.IBAN = puser.iban;
                            kto.SYSBLZ = puser.sysblz;
                            kto.KONTONR = puser.kontonummer;
                            kto.KONTOINHABER = puser.kontoinhaber;
                            ctx.SaveChanges();
                        }
                    }

                    //admin
                    updateRgr(ctx,"EPOS_ADMIN", puser.syswfuser, puser.adminflag);
                    //vtflag
                    updateRgr(ctx,"EPOS_VERTRAGSABSCHLUSS", puser.syswfuser, puser.vtflag);

                    //provisionsanteil an alle provisionen hängen - über eaihot


                    //rolle
                    if (puser.rolle != null && puser.rolle.Length > 0)
                    {
                        long roleid = long.Parse(puser.rolle);
                        long hasRolleId = ctx.ExecuteStoreQuery<long>(@"select roleattribm.sysroleattribm from cic.vc_ddlkppos rolle, roleattrib,roleattribm where roleattribm.sysperole=" + puser.sysperole + @" and roleattribm.sysroleattrib=roleattrib.sysroleattrib
                                    and rolle.code='ROLLENAUSPRAEGUNGEN' and rolle.domainid='ROLLE' and rolle.id=to_char(roleattrib.sysroleattrib) and rolle.id=" + roleid, null).FirstOrDefault();

                        if (hasRolleId == 0)//nicht vorhanden, anlegen oder aktualisieren
                        {
                            long otherRoleId = ctx.ExecuteStoreQuery<long>(@"select roleattribm.sysroleattribm from cic.vc_ddlkppos rolle, roleattrib,roleattribm where roleattribm.sysperole=" + puser.sysperole + @" and roleattribm.sysroleattrib=roleattrib.sysroleattrib
                                    and rolle.code='ROLLENAUSPRAEGUNGEN' and rolle.domainid='ROLLE' and rolle.id=to_char(roleattrib.sysroleattrib)", null).FirstOrDefault();
                            ROLEATTRIBM attm = null;
                            
                            if(otherRoleId>0)
                                attm = (from f in ctx.ROLEATTRIBM
                                        where f.SYSROLEATTRIBM == otherRoleId
                                                      select f).FirstOrDefault();
                            else//keine bisher vorhanden, neu anlegen
                            {
                                attm = new ROLEATTRIBM();
                                ctx.AddToROLEATTRIBM(attm);
                            }
                           
                           
                            attm.SYSPEROLE= puser.sysperole;
                            attm.SYSROLEATTRIB= long.Parse(puser.rolle);
                            
                            ctx.SaveChanges();
                        }

                    }


                    ctx.SaveChanges();
                    
                   
                  

                    if (doagain)
                    {
                     

                        Devart.Data.Oracle.OracleParameter prolepar = new Devart.Data.Oracle.OracleParameter { Value = "PEROLE", ParameterName = "pNEArea", Direction = System.Data.ParameterDirection.Input, DbType = System.Data.DbType.String };
                        Devart.Data.Oracle.OracleParameter prolerval = new Devart.Data.Oracle.OracleParameter { Value = vkperole, ParameterName = "pNESysArea", Direction = System.Data.ParameterDirection.Input, OracleDbType = Devart.Data.Oracle.OracleDbType.Number };

                       
                        ctx.ExecuteStoreCommand("update perole set INACTIVEFLAG=0 where sysperole=" + vkperole, null);

                        try
                        {
                            Devart.Data.Oracle.OracleParameter ctype = new Devart.Data.Oracle.OracleParameter { Value = vkperole, ParameterName = "pSysPEROLE", Direction = System.Data.ParameterDirection.Input, DbType = System.Data.DbType.Decimal };
                            DbParameter[] prodpars = { ctype };
                            ctx.ExecuteProcedure("CIC.CIC_PEROLE_UTILS.RefreshPEROLECACHEAfterInsert", prodpars);
                        }
                        catch (Exception pe)
                        {
                            _log.Warn("CIC_PEROLE_UTILS.RefreshPEROLECACHEAfterInsert failed", pe);
                        }
                    }
                  

                    addEaihot("VP_ONBOARDING", "PEROLE", vkperole, eaitype, puser.provision.HasValue ? puser.provision.Value.ToString() : null);

                }
                finally
                {
                    ctx.ExecuteProcedure("CIC.cic_sys.reset_NE", new DbParameter[] { });
                }

            }
            
            return puser;
        }

        /// <summary>
        /// Sets or deletes a rgr (rechtezuweisung for the given wfuser)
        /// </summary>
        /// <param name="rgr"></param>
        /// <param name="syswfuser"></param>
        /// <param name="flag"></param>
        private void updateRgr(DdOlExtended ctx, String rgr, long syswfuser, int flag)
        {
          
                long adminid = ctx.ExecuteStoreQuery<long>("select sysrgr from rgr where name='" + rgr + "'", null).FirstOrDefault();
                long isadmin = ctx.ExecuteStoreQuery<long>("select sysrgm from perole p, RGR, RGM,  wfuser where rgr.sysrgr = rgm.sysrgr and rgr.name = '" + rgr + "' and rgm.syswfuser = wfuser.syswfuser and wfuser.sysperson =p.sysperson and wfuser.syswfuser=" + syswfuser, null).FirstOrDefault();
                if (flag > 0 && isadmin == 0)
                {
                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syswfuser", Value = syswfuser });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysrgr", Value = adminid });
                    ctx.ExecuteStoreCommand(@"insert into rgm (syswfuser,sysrgr) 
                                                values(:syswfuser,:sysrgr)", parameters.ToArray());
                    ctx.SaveChanges();

                }
                else if (isadmin > 0 && flag == 0)
                {
                    //delete rgm
					try
					{
						ctx.ExecuteStoreCommand ("delete from rgm where sysrgm=" + isadmin, null);
					}
					catch (OracleException oraEx)
					{
						//—————————————————————————————————————————————————————————————————————————————
						// rh 20170117: 
						// due to ticket BNRDR-2247: "SOLL: Die WEB VLM darf die Exception ORA-20012 
						// generell nicht an Benutzer weiter geben."
						//—————————————————————————————————————————————————————————————————————————————
						// OracleException (0x80004005): 
						// message: ORA-20012: NE Delete ORA-06512: at "CIC.RGM_NE_DEL"
						//—————————————————————————————————————————————————————————————————————————————
						bool skipThat = oraEx.Message.StartsWith ("ORA-20012");
						if (skipThat)
						{
							// skip this
							bool bAllesGut = true;
						}
						else
						{
							throw;
						}
					}
                }
            
        }
        /// <summary>
        /// Adds an eaiart for given code and area/id
        /// </summary>
        /// <param name="code"></param>
        /// <param name="area"></param>
        /// <param name="sysid"></param>
        private void addEaihot(String code, String area, long sysid, String input1, String input2)
        {
            EAIHOT eaihotInput = new EAIHOT();
            eaihotInput.CODE = code;
            eaihotInput.OLTABLE = area;
            eaihotInput.SYSOLTABLE = sysid;
            eaihotInput.PROZESSSTATUS = (int)EaiHotStatusConstants.Pending;
            eaihotInput.SYSWFUSER = sysWfuser;
            eaihotInput.EVE = 1;
            eaihotInput.HOSTCOMPUTER = "*";
            eaihotInput.CLIENTART = 1;
            if (input1 != null)
                eaihotInput.INPUTPARAMETER1 = input1;
            if (input2 != null)
                eaihotInput.INPUTPARAMETER2 = input2;

            DateTime d = DateTime.Now;
            eaihotInput.SUBMITDATE = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDate(d);
            eaihotInput.SUBMITTIME = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(d);

            using (DdOwExtended owCtx = new DdOwExtended())
            {


                eaihotInput.EAIART = (from EaiArt in owCtx.EAIART
                                      where EaiArt.CODE.Equals(code)
                                      select EaiArt).FirstOrDefault();

                owCtx.AddToEAIHOT(eaihotInput);
                owCtx.SaveChanges();
            }
        }

        /// <summary>
        /// updates/creates clarification
        /// </summary>
        /// <param name="clarification"></param>
        /// <returns></returns>
        public ClarificationDto createOrUpdateClarification(ClarificationDto clarification)
        {
            using (DdOwExtended context = new DdOwExtended())
            {
                WFMMEMO wfmmemo = (from c in context.WFMMEMO
                                   where c.SYSWFMMEMO == clarification.syswfmmemo
                                   select c).FirstOrDefault();
                bool create = false;
                if (wfmmemo == null)
                {
                    WFMMKAT kat = (from k in context.WFMMKAT
                                   where k.SYSWFMMKAT == clarification.syswfmmkat
                                   select k).FirstOrDefault();

                    wfmmemo = Mapper.Map<ClarificationDto, WFMMEMO>(clarification);
                    wfmmemo.CREATEDATE = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDateNoTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                    wfmmemo.CREATETIME = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                    wfmmemo.CREATEUSER = getSysWfuser();
                    wfmmemo.EDITUSER = clarification.edituser;// createuser;
                    wfmmemo.WFMMKAT = kat;
                    context.AddToWFMMEMO(wfmmemo);
                    create = true;
                }
                bool newMainQuestion = false;
                if (clarification.olarea != null)//wenn hauptgebiet übergeben wird, (z.b. ANGEBOT oder ANTRAG), dann hier die syswfmtable ermitteln -> Hauptfrage wird erstellt
                {
                    List<Devart.Data.Oracle.OracleParameter> parameters2 = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syscode", Value =clarification.olarea });
                    wfmmemo.SYSWFMTABLE = context.ExecuteStoreQuery<long>("select syswftable from wftable where syscode=:syscode", parameters2.ToArray()).FirstOrDefault();
                    newMainQuestion = true;
                }
                if (clarification.kurzbeschreibung!=null && clarification.kurzbeschreibung.Length > 80)
                    clarification.kurzbeschreibung = clarification.kurzbeschreibung.Substring(0, 80);

                wfmmemo.KURZBESCHREIBUNG = clarification.kurzbeschreibung;

                String inhalt = clarification.notizmemo;
                if (inhalt == null)
                {
                    wfmmemo.NOTIZMEMO = null;
                    inhalt = "";
                }
                else
                {
                    inhalt = Regex.Replace(inhalt, @"\r\n|\n\r|\n|\r", "\r\n");
                    if (inhalt.Length > 4800)
                        inhalt = inhalt.Substring(0, 4800);
                    wfmmemo.NOTIZMEMO = inhalt;

                }
                wfmmemo.INT05 = clarification.erledigt;
                wfmmemo.ULONG04 = clarification.sysbprole;
                context.SaveChanges();

                List<Devart.Data.Oracle.OracleParameter> ipar = new List<Devart.Data.Oracle.OracleParameter>();
                ipar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syswfmmemo", Value = wfmmemo.SYSWFMMEMO });
                ClarificationDto mainArea = context.ExecuteStoreQuery<ClarificationDto>("select syswfmmemo,wftable.syscode olarea, wfmmemo.syslease syslease,wfmmemo.createuser createuser from wfmmemo,wftable where wftable.syswftable=wfmmemo.SYSWFMTABLE start with syswfmmemo=:syswfmmemo connect by prior syslease=syswfmmemo and syswfmmkat=174 order by level desc", ipar.ToArray()).FirstOrDefault();
                

                if (!clarification.reminderDate.HasValue)
                {
                    double tage = context.ExecuteStoreQuery<double>("select prozent tage from quote,quotedat where quotedat.sysquote=quote.sysquote and bezeichnung='FAELLIG_NFP_ALLG' and gueltigab<=sysdate order by gueltigab desc", null).FirstOrDefault();
                    clarification.reminderDate = DateTime.Now.AddDays((int)tage);
                }
               /* if (clarification.erledigt < 1 && clarification.olarea == null)//Antwort, also den zugehörigen listener+reminder, die bprole holen, die wird für die antwort benötigt
                {

                    ipar = new List<Devart.Data.Oracle.OracleParameter>();
                    ipar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syswfuser", Value = sysWfuser });
                    ipar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "area", Value = mainArea.olarea });
                    ipar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = mainArea.syslease });
                    ipar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "param1", Value = clarification.syslease });
                    String remforThisAnswer = context.ExecuteStoreQuery<String>("select param3 as sysreminder from bpcasestep where param1=:param1 and stepbyuser=:syswfuser and oltable=:area and sysoltable=:sysid and param2='ABKLSTART'", ipar.ToArray()).FirstOrDefault();
                    clarification.sysbprole = context.ExecuteStoreQuery<long>("select sysbprole from bplistener where evaluatecode='WFA_Dummy_Abklaerung' and oltable='REMINDER' and sysoltable=" + remforThisAnswer, null).FirstOrDefault();
                }*/
                bool answerOrForward = clarification.erledigt < 1 && clarification.olarea == null;

                long sysreminder = 0, createUserReminder = 0;
                if (create && (newMainQuestion||clarification.isForward>0))
                {

                    //jeder reminder ist über oltable/sysoltable mit bplistener verknüpft
                    //jeder reminder ist über param3 mit einem bpcasestep verknüpft, der casestep ist über area/sysid mit der hauptarea verknüpft und mit param1 an das memo
                    REMINDER r = new REMINDER();//Reminder für Frageempfänger, der die Frage beantworten soll
                    context.AddToREMINDER(r);
                    r.SYSID = mainArea.syslease;//Reminder verknüpft mit hauptgebiet
                    r.AREA = mainArea.olarea;
                    r.DUEDATE = clarification.reminderDate;
                    r.DUETIME = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(r.DUEDATE);
                    r.STARTDATE = r.DUEDATE;
                    r.STARTTIME = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(r.STARTDATE);
                    r.TITLE = clarification.kurzbeschreibung;
                    if (inhalt.Length > 255)
                        inhalt = inhalt.Substring(0, 255);
                    r.DESCRIPTION = inhalt;
                    r.ACTIVEFLAG = 1;
                    if (clarification.edituser>0)
                        r.WFUSERReference.EntityKey = context.getEntityKey(typeof(Cic.OpenOne.Common.Model.DdOw.WFUSER), clarification.edituser);
                    if(clarification.sysbprole>0)
                        r.BPROLEReference.EntityKey = context.getEntityKey(typeof(Cic.OpenOne.Common.Model.DdOw.BPROLE), clarification.sysbprole);
                    r.CREATEUSER = clarification.createuser;
                    r.CREATEDATE = new DateTime();
                    r.CREATETIME = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(r.CREATEDATE);

                    REMINDER r2 = new REMINDER();//Reminder für Fragesteller
                    if (newMainQuestion)
                    {
                        context.AddToREMINDER(r2);
                        r2.SYSID = mainArea.syslease;//Reminder verknüpft mit hauptgebiet
                        r2.AREA = mainArea.olarea;
                        r2.DUEDATE = clarification.reminderDate;
                        r2.DUETIME = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(r2.DUEDATE);
                        r2.STARTDATE = r2.DUEDATE;
                        r2.STARTTIME = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(r2.STARTDATE);
                        r2.TITLE = clarification.kurzbeschreibung;
                        if (inhalt.Length > 255)
                            inhalt = inhalt.Substring(0, 255);
                        r2.DESCRIPTION = inhalt;
                        r2.ACTIVEFLAG = 1;
                        r2.WFUSERReference.EntityKey = context.getEntityKey(typeof(Cic.OpenOne.Common.Model.DdOw.WFUSER), getSysWfuser());
                        r2.CREATEUSER = getSysWfuser();
                        r2.CREATEDATE = new DateTime();
                        r2.CREATETIME = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(r2.CREATEDATE);
                    }
                    context.SaveChanges();
                    sysreminder = r.SYSREMINDER;
                    createUserReminder = r2.SYSREMINDER;


                    //Invoke BPE Process for the listener of the reminder:
                    CIC.Bas.Modules.OpenLeaseCommon.Evaluate.Compatibility.BusinessProcessing.Subscriptions.Dto.LookupVariableDto[] vars = new CIC.Bas.Modules.OpenLeaseCommon.Evaluate.Compatibility.BusinessProcessing.Subscriptions.Dto.LookupVariableDto[1];
                    vars[0] = new CIC.Bas.Modules.OpenLeaseCommon.Evaluate.Compatibility.BusinessProcessing.Subscriptions.Dto.LookupVariableDto();
                    vars[0].VariableName = "WEB_VARS";
                    vars[0].LookupVariableName = "SYSBPROLE";
                    vars[0].Value = ""+clarification.sysbprole;

                    long procUser = clarification.edituser;
                    if (clarification.edituser == 0)//es wurde kein User gewählt - darf nicht sein, sonst hat der ersteller 2 listener!
                        procUser = getSysWfuser();
                    long sysbplistener = BOS.launchWF("tprc_WFA_Abklaerung", "evtd_WFA_Abklaerung_Start", "REMINDER", r.SYSREMINDER, procUser, vars);
                    

                    String namebplane = context.ExecuteStoreQuery<String>("select namebprole from bprole where sysbprole=" + clarification.sysbprole, null).FirstOrDefault();
                    List<Devart.Data.Oracle.OracleParameter> iparr = new List<Devart.Data.Oracle.OracleParameter>();
                    iparr.Add(new Devart.Data.Oracle.OracleParameter {ParameterName = "sysbplistener", Value = sysbplistener});
                    iparr.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "namebplane", Value = namebplane });
                    context.ExecuteStoreCommand("update bplistener set namebplane=:namebplane, evaluatecode='WFA_Dummy_Abklaerung' where sysbplistener=:sysbplistener", iparr.ToArray());

                    if(newMainQuestion)//es wurde ein User gewählt, auch für den Anlegeuser einen BPLISTENER anlegen
                    {
                        sysbplistener = BOS.launchWF("tprc_WFA_Abklaerung_quest", "evtd_WFA_Abklaerung_quest", "REMINDER", createUserReminder, getSysWfuser(), vars);
                        List<Devart.Data.Oracle.OracleParameter> iparr2 = new List<Devart.Data.Oracle.OracleParameter>();
                        iparr2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysbplistener", Value = sysbplistener });
                        iparr2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "namebplane", Value = namebplane });
                        context.ExecuteStoreCommand("update bplistener set namebplane=:namebplane, evaluatecode='WFA_Dummy_Abklaerung' where sysbplistener=:sysbplistener", iparr2.ToArray());
                    }
                
                    context.SaveChanges();

                
                    ipar = new List<Devart.Data.Oracle.OracleParameter>();
                    ipar.Add(new Devart.Data.Oracle.OracleParameter {ParameterName = "syswfuser", Value = sysWfuser});
                    ipar.Add(new Devart.Data.Oracle.OracleParameter {ParameterName = "time", Value = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(DateTime.Now)});
                    ipar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "area", Value = mainArea.olarea });
                    ipar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = mainArea.syslease });
                    ipar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "param1", Value = wfmmemo.SYSWFMMEMO }); //jede frage/antwort wird am bpcasestep hinterlegt, der casestep wiederum verlinkt mit dem reminder
                    
                    ipar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "param3", Value = sysreminder });
                    ipar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "param4", Value = createUserReminder });
                    context.ExecuteStoreCommand("insert into bpcasestep(param1,param2,param3,param4,steptext,stepbyuser,stepdate,steptime,historyflag,oltable,sysoltable,mastercase) values(:param1,'ABKLSTART',:param3,:param4,'Abklärung gestartet',:syswfuser,sysdate,:time,1,:area,:sysid,'WF_Abwicklung')", ipar.ToArray());

                    
                }
                ipar = new List<Devart.Data.Oracle.OracleParameter>();
                ipar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "area", Value = mainArea.olarea });
                ipar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = mainArea.syslease });
                ipar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "param1", Value = wfmmemo.SYSWFMMEMO });
                //gebiet-casestep ermitteln
                long sysbpcasestep = context.ExecuteStoreQuery<long>("select sysbpcasestep from bpcasestep where param2='ABKLSTART' and  oltable=:area and sysoltable=:sysid and param1=:param1", ipar.ToArray()).FirstOrDefault();

                if (create && clarification.olarea != null && newMainQuestion) //Frage direkt am Hauptgebiet (angebot/antrag), link erzeugen
                {
                    ipar = new List<Devart.Data.Oracle.OracleParameter>();
                    ipar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "txt", Value = "Abklärung gestartet" });
                    ipar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "casestep", Value = sysbpcasestep});
                    ipar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "area", Value = "WFMMEMO" });
                    ipar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = wfmmemo.SYSWFMMEMO });
                    context.ExecuteStoreCommand("insert into bpcasesteplnk(description,oltable,sysoltable,sysbpcasestep) values(:txt,:area,:sysid,:casestep)", ipar.ToArray());
                }

                //neue Reminder id von oben ermitteln
               /* ipar = new List<Devart.Data.Oracle.OracleParameter>();
                ipar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "area", Value = mainArea.olarea });
                ipar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = mainArea.syslease });
                ipar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "param1", Value = wfmmemo.SYSWFMMEMO });
                String sysreminderStr = context.ExecuteStoreQuery<String>("select param3 from bpcasestep where oltable=:area and sysoltable=:sysid and param1=:param1", ipar.ToArray()).FirstOrDefault();
                */

                
                if (clarification.erledigt > 0)// && !String.IsNullOrEmpty(sysreminderStr))//Frage wurde erledigt
                {

                    ipar = new List<Devart.Data.Oracle.OracleParameter>();
                    ipar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syswfuser", Value = sysWfuser });
                    ipar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "time", Value = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(DateTime.Now) });
                    ipar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "area", Value = mainArea.olarea });
                    ipar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = mainArea.syslease });
                    ipar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "param1", Value = wfmmemo.SYSWFMMEMO });
                    
                    context.ExecuteStoreCommand("insert into bpcasestep(param1,param2,steptext,stepbyuser,stepdate,steptime,historyflag,oltable,sysoltable,mastercase,closeflag) values(:param1,'ABKLEND','Abklärung erledigt',:syswfuser,sysdate,:time,1,:area,:sysid,'WF_Abwicklung',1)", ipar.ToArray());
                    
                    ipar = new List<Devart.Data.Oracle.OracleParameter>();
                    ipar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "area", Value = mainArea.olarea });
                    ipar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = mainArea.syslease });
                    ipar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "param1", Value = wfmmemo.SYSWFMMEMO });
                    //end-casestep von oben ermitteln
                    long sysbpcasestepEnd = context.ExecuteStoreQuery<long>("select sysbpcasestep from bpcasestep where param2='ABKLEND' and  oltable=:area and sysoltable=:sysid and param1=:param1", ipar.ToArray()).FirstOrDefault();

                    long availmemolink = context.ExecuteStoreQuery<long>("select sysbpcasesteplnk from bpcasesteplnk where oltable='WFMMEMO' and sysoltable=" + wfmmemo.SYSWFMMEMO, null).FirstOrDefault();
                    if (availmemolink>0)//Hauptfrage wurde ohne Antwort erledigt, es gibt schon einen link auf das MEMO
                    {
                        //link auf end-casestep updaten, laut #BNRDR-3187 darf nur ein link auf ein memo bestehen
                        ipar = new List<Devart.Data.Oracle.OracleParameter>();
                        ipar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "casestep", Value = sysbpcasestepEnd });
                        ipar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = availmemolink });

                        context.ExecuteStoreCommand("update bpcasesteplnk set sysbpcasestep=:casestep where sysbpcasesteplnk=:sysid", ipar.ToArray());
                    }
                    else
                    {
                        //link auf end-casestep anlegen
                        ipar = new List<Devart.Data.Oracle.OracleParameter>();
                        ipar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "txt", Value = "Abklärung erledigt" });
                        ipar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "casestep", Value = sysbpcasestepEnd });
                        ipar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "area", Value = "WFMMEMO" });
                        ipar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = wfmmemo.SYSWFMMEMO });
                        context.ExecuteStoreCommand("insert into bpcasesteplnk(description,oltable,sysoltable,sysbpcasestep) values(:txt,:area,:sysid,:casestep)", ipar.ToArray());
                    }
                    context.ExecuteStoreCommand("update bpcasestep set closeflag=1 where sysbpcasestep=" + sysbpcasestep, null);

                    //Reminder auf diese Frage schließen (für Frageempfänger)
                    ipar = new List<Devart.Data.Oracle.OracleParameter>();
                    ipar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "area", Value = mainArea.olarea });
                    ipar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = mainArea.syslease });
                    ipar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "param1", Value = wfmmemo.SYSWFMMEMO });
                    //and stepbyuser=:syswfuser
                    String remforThisAnswer = context.ExecuteStoreQuery<String>("select param3 as sysreminder from bpcasestep where param1=:param1  and oltable=:area and sysoltable=:sysid and param2='ABKLSTART'", ipar.ToArray()).FirstOrDefault();

                    //Reminder für Fragesteller schließen
                    ipar = new List<Devart.Data.Oracle.OracleParameter>();
                    //ipar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syswfuser", Value = sysWfuser });
                    ipar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "area", Value = mainArea.olarea });
                    ipar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = mainArea.syslease });
                    ipar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "param1", Value = mainArea.syswfmmemo });//häng an hauptfrage
                    String remforQuestion = context.ExecuteStoreQuery<String>("select param4 as sysreminder from bpcasestep where param1=:param1  and oltable=:area and sysoltable=:sysid and param2='ABKLSTART'", ipar.ToArray()).FirstOrDefault();

                    long listenerForThisAnswer = 0;
                    if (remforThisAnswer != null)
                    {
                        listenerForThisAnswer = context.ExecuteStoreQuery<long>("select sysbplistener from bplistener where evaluatecode='WFA_Dummy_Abklaerung' and oltable='REMINDER' and sysoltable=" + remforThisAnswer, null).FirstOrDefault();
                        context.ExecuteStoreCommand("update reminder set activeflag=0 where sysreminder=" + remforThisAnswer, null);
                    }

                    long listenerForQuestion = 0;
                    if (remforQuestion != null)
                    {
                        listenerForQuestion = context.ExecuteStoreQuery<long>("select sysbplistener from bplistener where evaluatecode='WFA_Dummy_Abklaerung' and oltable='REMINDER' and sysoltable=" + remforQuestion, null).FirstOrDefault();
                        context.ExecuteStoreCommand("update reminder set activeflag=0 where sysreminder=" + remforQuestion, null);
                    }


                    //alle Reminder schließen
                   /* ipar = new List<Devart.Data.Oracle.OracleParameter>();
                    ipar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "mainArea", Value = mainArea.olarea });
                    ipar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syslease", Value = mainArea.syslease });
                    context.ExecuteStoreCommand("update reminder set activeflag=0 where sysreminder in (select reminder.sysreminder from bplistener,reminder where oltable='REMINDER' and bplistener.sysoltable=reminder.sysreminder and bplistener.evaluatecode='WFA_Dummy_Abklaerung' and reminder.area=:mainArea and reminder.sysid=:syslease)", ipar.ToArray());
                    */
                    
                    
                    /*ipar = new List<Devart.Data.Oracle.OracleParameter>();
                    ipar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "mainArea", Value = mainArea.olarea });
                    ipar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syslease", Value = mainArea.syslease });
                    context.ExecuteStoreCommand("delete from bplistener where sysbplistener in (select bplistener.sysbplistener from bplistener,reminder where oltable='REMINDER' and bplistener.sysoltable=reminder.sysreminder and bplistener.evaluatecode='WFA_Dummy_Abklaerung' and reminder.area=:mainArea and reminder.sysid=:syslease)", ipar.ToArray());*/

                    //todo: remove
                   // context.ExecuteStoreCommand("delete from bplistener where sysbplistener=" + listenerForThisAnswer, null);


                    CIC.Bas.Modules.OpenLeaseCommon.Evaluate.Compatibility.BusinessProcessing.Subscriptions.Dto.LookupVariableDto[] vars = new CIC.Bas.Modules.OpenLeaseCommon.Evaluate.Compatibility.BusinessProcessing.Subscriptions.Dto.LookupVariableDto[1];
                    vars[0] = new CIC.Bas.Modules.OpenLeaseCommon.Evaluate.Compatibility.BusinessProcessing.Subscriptions.Dto.LookupVariableDto();
                    vars[0].VariableName = "WEB_VARS";
                    vars[0].LookupVariableName = "CMD";
                    vars[0].Value = "evtd_WFA_Abklaerung_Ende";
                    if (listenerForThisAnswer>0)
                        BOS.sendEvent(listenerForThisAnswer, getSysWfuser(), vars);

                    vars = new CIC.Bas.Modules.OpenLeaseCommon.Evaluate.Compatibility.BusinessProcessing.Subscriptions.Dto.LookupVariableDto[1];
                    vars[0] = new CIC.Bas.Modules.OpenLeaseCommon.Evaluate.Compatibility.BusinessProcessing.Subscriptions.Dto.LookupVariableDto();
                    vars[0].VariableName = "WEB_VARS";
                    vars[0].LookupVariableName = "CMD";
                    vars[0].Value = "evtd_WFA_Abklaerung_quest_ende";
                    if (listenerForQuestion > 0)
                    {
                        context.ExecuteStoreCommand("update bplistener set eventcode='evtd_WFA_Abklaerung_quest' where sysbplistener=" + listenerForQuestion, null);
                        BOS.sendEvent(listenerForQuestion, getSysWfuser(), vars);
                    }
                    
                }
                else if (answerOrForward)//Antwort oder Weiterleitung, also den zugehörigen listener+reminder schliessen
                {
                    //Reminder auf diese Frage schließen
                    ipar = new List<Devart.Data.Oracle.OracleParameter>();
                    //ipar.Add(new Devart.Data.Oracle.OracleParameter {ParameterName = "syswfuser", Value = sysWfuser});
                    ipar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "area", Value = mainArea.olarea });
                    ipar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = mainArea.syslease });
                    ipar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "param1", Value = clarification.syslease });
                    //and stepbyuser=:syswfuser
                    String remforThisAnswer = context.ExecuteStoreQuery<String>("select param3 as sysreminder from bpcasestep where param1=:param1  and oltable=:area and sysoltable=:sysid and param2='ABKLSTART'",ipar.ToArray()).FirstOrDefault();

                    //Listener umhängen?!
                    //context.ExecuteStoreCommand("update bplistener set syswfuser="+mainArea.createuser+" where  evaluatecode='WFA_Dummy_Abklaerung' and oltable='REMINDER' and sysoltable=" + remforThisAnswer,null);
                    
                    //Listener (nicht vom ersteller) beenden
                    //Erstelleruser hat seinen eigenen listener! syswfuser!="+mainArea.createuser+" and
                    if (remforThisAnswer != null)
                    {
                        long listenerForThisAnswer = context.ExecuteStoreQuery<long>("select sysbplistener from bplistener where  evaluatecode='WFA_Dummy_Abklaerung' and oltable='REMINDER' and sysoltable=" + remforThisAnswer, null).FirstOrDefault();
                        context.ExecuteStoreCommand("update reminder set activeflag=0 where sysreminder=" + remforThisAnswer, null);

                        //Invoke BPE Process for the listener of the reminder:
                        CIC.Bas.Modules.OpenLeaseCommon.Evaluate.Compatibility.BusinessProcessing.Subscriptions.Dto.LookupVariableDto[] vars = new CIC.Bas.Modules.OpenLeaseCommon.Evaluate.Compatibility.BusinessProcessing.Subscriptions.Dto.LookupVariableDto[1];
                        vars[0] = new CIC.Bas.Modules.OpenLeaseCommon.Evaluate.Compatibility.BusinessProcessing.Subscriptions.Dto.LookupVariableDto();
                        vars[0].VariableName = "WEB_VARS";
                        vars[0].LookupVariableName = "CMD";
                        vars[0].Value = "evtd_WFA_Abklaerung_Ende";
                        BOS.sendEvent(listenerForThisAnswer, getSysWfuser(), vars);
                    }


                    //Original Prozess eines weiterschalten weil Frage beantwortet wurde: Abklärung gestartet -> Abklärung beantwortet
                    //BNRVZ-931
                    ipar = new List<Devart.Data.Oracle.OracleParameter>();
                    ipar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "area", Value = mainArea.olarea });
                    ipar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = mainArea.syslease });
                    ipar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "param1", Value = mainArea.syswfmmemo });//häng an hauptfrage
                    String remforQuestion = context.ExecuteStoreQuery<String>("select param4 as sysreminder from bpcasestep where param1=:param1  and oltable=:area and sysoltable=:sysid and param2='ABKLSTART'", ipar.ToArray()).FirstOrDefault();
                    if (remforQuestion != null)
                    {
                        long listenerForQuestion = context.ExecuteStoreQuery<long>("select sysbplistener from bplistener where evaluatecode='WFA_Dummy_Abklaerung' and oltable='REMINDER' and sysoltable=" + remforQuestion, null).FirstOrDefault();
                        /*CIC.Bas.Modules.OpenLeaseCommon.Evaluate.Compatibility.BusinessProcessing.Subscriptions.Dto.LookupVariableDto[] vars = new CIC.Bas.Modules.OpenLeaseCommon.Evaluate.Compatibility.BusinessProcessing.Subscriptions.Dto.LookupVariableDto[1];
                        vars[0] = new CIC.Bas.Modules.OpenLeaseCommon.Evaluate.Compatibility.BusinessProcessing.Subscriptions.Dto.LookupVariableDto();
                        vars[0].VariableName = "WEB_VARS";
                        vars[0].LookupVariableName = "CMD";
                        vars[0].Value = "evtd_WFA_Abklaerung_quest_answered";
                        if (listenerForQuestion > 0)
                            BOS.sendEvent(listenerForQuestion, getSysWfuser(), vars);*/
                        if (listenerForQuestion > 0)
                            context.ExecuteStoreCommand("update bplistener set eventcode='evtd_WFA_Abklaerung_quest_answered' where sysbplistener=" + listenerForQuestion, null);

                    }
                }
                




                
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syswfmmemo", Value = wfmmemo.SYSWFMMEMO });
                ClarificationDto rval = context.ExecuteStoreQuery<ClarificationDto>(QUERYCLARIFICATION, parameters.ToArray()).FirstOrDefault();
                rval.level = clarification.level;
                return rval;
            }
        }

        /// <summary>
        /// Erstellt eine neue Notiz oder aktualisiert die existierende
        /// </summary>
        /// <param name="memo">sysid, notiz, wftableSyscode, kurzbez, kategorie, ...</param>
        /// <param name="refTable">optional: name of table to be referenced by memo.syswfmtable</param>
        public MemoDto createOrUpdateMemo(MemoDto memo, String refTable = null)
        {
            using (DdOwExtended context = new DdOwExtended())
            {
                if (refTable != null && refTable.Length > 0 && memo.SYSWFMTABLE == 0)
                {
                    WFTABLE table = (from t in context.WFTABLE
                                     where t.SYSCODE == refTable
                                     select t).FirstOrDefault();

                    if (table != null)
                        memo.SYSWFMTABLE = table.SYSWFTABLE;
                }


                WFMMEMO wfmmemo = (from c in context.WFMMEMO
                                   where c.SYSWFMMEMO == memo.SYSWFMMEMO 
                                   select c).FirstOrDefault();
                
                
               
                if (wfmmemo == null)
                {
                    WFMMKAT kat = (from k in context.WFMMKAT
                                   where k.SYSWFMMKAT == memo.SYSWFMMKAT
                                   select k).FirstOrDefault();

                    wfmmemo = Mapper.Map<MemoDto, WFMMEMO>(memo);
                    wfmmemo.CREATEDATE = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDateNoTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                    wfmmemo.CREATETIME = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                    wfmmemo.CREATEUSER = getSysWfuser();
                    wfmmemo.WFMMKAT = kat;
                    context.AddToWFMMEMO(wfmmemo);
                }
                else
                {
                    wfmmemo.EDITDATE = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionDateNoTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                    wfmmemo.EDITTIME = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(Cic.OpenOne.Common.Util.Config.CfgDate.verifyPerDate(null));
                    wfmmemo.EDITUSER = getSysWfuser();
                    wfmmemo.DAT01 = memo.DAT01;
                    wfmmemo.DAT02 = memo.DAT02;
                    wfmmemo.STR01 = memo.STR01;
                    wfmmemo.STR02 = memo.STR02;
                    wfmmemo.INT01 = memo.INT01;
                    wfmmemo.INT02 = memo.INT02;
                }
                wfmmemo.KURZBESCHREIBUNG = memo.KURZBESCHREIBUNG;
                String inhalt = memo.NOTIZMEMO;
                if (inhalt == null)
                {
                    wfmmemo.NOTIZMEMO = null;
                }
                else
                {
                    inhalt = Regex.Replace(inhalt, @"\r\n|\n\r|\n|\r", "\r\n"); 
                    if (inhalt.Length > 4800)
                        inhalt = inhalt.Substring(0, 4800);
                    wfmmemo.NOTIZMEMO = inhalt;

                }

                context.SaveChanges();


                return Mapper.Map<WFMMEMO, MemoDto>(wfmmemo);
            }
        }


        #endregion

        #endregion


        #region GET

        /// <summary>
        /// Returns all Gview Details
        /// 
        /// called when 
        ///   * a gview-list row was selected to fetch this rows' data
        ///   * user opens a breadcrumb coming from Entity X, fills the gvview sysid with X.entityId
        ///   
        /// GVIEW processing:
        /// a) read the wfc config
        /// b) fill fields from workflow-queue (F01=id, F05 = value etc), if gview runs in bpe context, F10-F20 can be instant-replaced with ${object.P01-P10} Notation
        /// c) preprocess all ${...} in label and string-value attribute
        /// d) evaluate by cas, if cw: found
        /// </summary>
        /// <param name="sysid"></param>
        /// <param name="gviewId"></param>
        /// <returns></returns>
        public GviewDto getGviewDetails(long sysid, String gviewId, WorkflowContext extctx)
        {
            GviewDto rval = new GviewDto();


            //read gview detail xml config and fech the viewfield definitions 
            WfvEntry entry = DAOFactoryFactory.getInstance().getWorkflowDao().getWfvEntry(gviewId);
            if (entry == null) return null;
            rval.fields = entry.customentry.viewmeta.fields;
            rval.area = entry.customentry.viewmeta.area;
            rval.gviewId = gviewId;//id for fetching the config upon saving
            rval.pkeys = new List<Pkey>();
            rval.sysId = sysid;

            foreach (Viewfield vf in rval.fields)//read all configured fields from the result
            {
                vf.value = new ViewValue();
            }

            WorkflowContext wctx = new WorkflowContext();
            wctx.area = rval.area;
            if (wctx.area == null || wctx.area.Length == 0)
                wctx.area = "SYSTEM";
            wctx.areaid = ""+rval.sysId;
            wctx.sysWFUSER = this.sysWfuser;
            wctx.inputfields = rval.fields;
            if(extctx!=null)
                wctx.entities = extctx.entities;
            
            //replace the ${object} notation
            BPEWorkflowService.EvaluateViewFieldsData( wctx);
            //evaluate cas if necessary
            BPEWorkflowService.EvaluateViewFields(Cic.One.Web.BO.BOFactoryFactory.getInstance().getCASBo(), wctx);

            //dont load if no id given
            if (sysid == 0) return rval;

            using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new Cic.OpenOne.Common.Model.Prisma.PrismaExtended())
            {

                Dictionary<String, SchemaInfoDto> tabInfo = null;
                List<SchemaInfoDto> tables=null;

                DbConnection con = (ctx.Connection as EntityConnection).StoreConnection;

                try
                {
                    //create sql query from configuration
                    QueryInfoData qid = null;
                    if (String.IsNullOrEmpty(entry.customentry.viewmeta.query.query))
                    {
                        tabInfo = SchemaInfoDto.getSchemaInfos(ctx, gviewId);
                        tables = tabInfo.Values.ToList();
                        qid = new QueryInfoDataType4(entry.customentry.viewmeta.query, SchemaInfoDto.getPrimaryKeyQueryPrefix(tabInfo), entry.customentry.searchmode);
                        qid.addAdditionalSearchConditions(" and " + qid.entityField + "=" + sysid);
                    }
                    else
                    {
                        qid = new QueryInfoDataType5(entry.customentry.viewmeta, null, entry.customentry.searchmode);
                        if(entry.customentry.viewmeta.query.pkey!=null)
                            qid.addAdditionalSearchConditions(" and " + entry.customentry.viewmeta.query.pkey + "=" + sysid);
                    }
                    

                    con.Open();
                    DbCommand cmd = con.CreateCommand();
                    cmd.CommandText = qid.getCompleteQuery();
                    DbDataReader reader = cmd.ExecuteReader();
                    Dictionary<String, int> fieldMap = new Dictionary<string, int>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        fieldMap[reader.GetName(i)] = i;
                    }
                    
                    if (reader.Read())
                    {
                        //rval.sysId = Convert.ToInt64(reader.GetValue(fieldMap["I_UKEY"]));
                        foreach (Viewfield vf in rval.fields)//read all configured fields from the result
                        {
                            if (fieldMap.ContainsKey(vf.attr.field.ToUpper()))
                            {
                                vf.fillFromDataReader(reader, fieldMap[vf.attr.field.ToUpper()]);
                            }
                        }
                        //get all primary key values
                        if(tables!=null)
                        foreach (SchemaInfoDto si in tables)
                        {
                            long pkey = Convert.ToInt64(reader.GetValue(fieldMap[si.getPrimaryKeyQueryPrefix()]));
                            rval.pkeys.Add(new Pkey(pkey, si.baseTableName));
                        }
                    }
                    

                    /*
                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = sysid });
                    GviewDto rval = ctx.ExecuteStoreQuery<GviewDto>(query, parameters.ToArray()).FirstOrDefault();
                    if (rval != null)
                    {
                        rval.fields = entry.viewmeta.fields;
                        foreach(Viewfield f in rval.fields)
                        {
                            f.fillFromGViewDto(rval);
                        }
                    }
                    */

                }
                catch (Exception ex)
                {
                    _log.Error("Error reading generic detail ", ex);
                }
                finally
                {
                    con.Close();
                }
            }
            return rval;
        }

        /// <summary>
        /// Returns all Staffelpositionstyp Details
        /// </summary>
        /// <param name="sysslpostyp"></param>
        /// <returns></returns>
        public StaffelpositionstypDto getStaffelpositionstypDetails(long sysslpostyp)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysslpostyp", Value = sysslpostyp });
                StaffelpositionstypDto rval = ctx.ExecuteStoreQuery<StaffelpositionstypDto>(QUERYSLPOSTYP, parameters.ToArray()).FirstOrDefault();

                return rval;
            }
        }

        /// <summary>
        /// Returns all Staffeltyp Details
        /// </summary>
        /// <param name="syssltyp"></param>
        /// <returns></returns>
        public StaffeltypDto getStaffeltypDetails(long syssltyp)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syssltyp", Value = syssltyp });
                StaffeltypDto rval = ctx.ExecuteStoreQuery<StaffeltypDto>(QUERYSLTYP, parameters.ToArray()).FirstOrDefault();

                return rval;
            }
        }

        /// <summary>
        /// Returns all Rolle Details
        /// </summary>
        /// <param name="sysperole"></param>
        /// <returns></returns>
        public RolleDto getRolleDetails(long sysperole)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperole", Value = sysperole });
                RolleDto rval = ctx.ExecuteStoreQuery<RolleDto>(QUERYPEROLE, parameters.ToArray()).FirstOrDefault();

                return rval;
            }
        }

        /// <summary>
        /// Returns all Rollentyp Details
        /// </summary>
        /// <param name="sysroletype"></param>
        /// <returns></returns>
        public RollentypDto getRollentypDetails(long sysroletype)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysroletype", Value = sysroletype });
                RollentypDto rval = ctx.ExecuteStoreQuery<RollentypDto>(QUERYROLETYPE, parameters.ToArray()).FirstOrDefault();

                return rval;
            }
        }

        /// <summary>
        /// Returns all Handelsgruppe Details
        /// </summary>
        /// <param name="sysprhgroup"></param>
        /// <returns></returns>
        public HandelsgruppeDto getHandelsgruppeDetails(long sysprhgroup)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysprhgroup", Value = sysprhgroup });
                HandelsgruppeDto rval = ctx.ExecuteStoreQuery<HandelsgruppeDto>(QUERYPRHGROUP, parameters.ToArray()).FirstOrDefault();

                return rval;
            }
        }

        /// <summary>
        /// Returns all Vertriebskanal Details
        /// </summary>
        /// <param name="sysbchannel"></param>
        /// <returns></returns>
        public VertriebskanalDto getVertriebskanalDetails(long sysbchannel)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysbchannel", Value = sysbchannel });
                VertriebskanalDto rval = ctx.ExecuteStoreQuery<VertriebskanalDto>(QUERYBCHANNEL, parameters.ToArray()).FirstOrDefault();

                return rval;
            }
        }

        /// <summary>
        /// Returns all Brand Details
        /// </summary>
        /// <param name="sysbrand"></param>
        /// <returns></returns>
        public BrandDto getBrandDetails(long sysbrand)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysbrand", Value = sysbrand });
                BrandDto rval = ctx.ExecuteStoreQuery<BrandDto>(QUERYBRAND, parameters.ToArray()).FirstOrDefault();

                return rval;
            }
        }

        /// <summary>
        /// Returns all Rechnung Details
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        public RechnungDto getRechnungDetails(long sysid)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = sysid });
                RechnungDto rval = ctx.ExecuteStoreQuery<RechnungDto>(QUERYRN, parameters.ToArray()).FirstOrDefault();

                return rval;
            }
        }

        /// <summary>
        /// Returns all Angobbrief Details
        /// </summary>
        /// <param name="sysangobbrief"></param>
        /// <returns></returns>
        public AngobbriefDto getAngobbriefDetails(long sysangobbrief)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysangobbrief", Value = sysangobbrief });
                AngobbriefDto rval = ctx.ExecuteStoreQuery<AngobbriefDto>(QUERYANGOBBRIEF, parameters.ToArray()).FirstOrDefault();

                return rval;
            }
        }

        /// <summary>
        /// Returns all Zahlungsplan Details
        /// </summary>
        /// <param name="sysslpos"></param>
        /// <returns></returns>
        public ZahlungsplanDto getZahlungsplanDetails(long sysslpos)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysslpos", Value = sysslpos });
                ZahlungsplanDto rval = ctx.ExecuteStoreQuery<ZahlungsplanDto>(QUERYSLPOS_ZAHLUNGSPLAN, parameters.ToArray()).FirstOrDefault();

                return rval;
            }
        }

        /// <summary>
        /// Returns all Fahrzeugbrief Details
        /// </summary>
        /// <param name="sysobbrief"></param>
        /// <returns></returns>
        public FahrzeugbriefDto getFahrzeugbriefDetails(long sysobbrief)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysobbrief", Value = sysobbrief });
                FahrzeugbriefDto rval = ctx.ExecuteStoreQuery<FahrzeugbriefDto>(QUERYOBBRIEF, parameters.ToArray()).FirstOrDefault();

                return rval;
            }
        }

        /// <summary>
        /// Returns all Kalk Details
        /// </summary>
        /// <param name="syskalk"></param>
        /// <returns></returns>
        public KalkDto getKalkDetails(long syskalk)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syskalk", Value = syskalk });
                KalkDto rval = ctx.ExecuteStoreQuery<KalkDto>(QUERYKALK, parameters.ToArray()).FirstOrDefault();

                return rval;
            }
        }

        /// <summary>
        /// Returns all Person Details
        /// </summary>
        /// <param name="sysperson"></param>
        /// <returns></returns>
        public PersonDto getPersonDetails(long sysperson)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperson", Value = sysperson });
                PersonDto rval = ctx.ExecuteStoreQuery<PersonDto>(QUERYPERSON, parameters.ToArray()).FirstOrDefault();

                return rval;
            }
        }

        /// <summary>
        /// Returns all Updateinfos for the area and id
        /// (all indicators for one area)
        /// </summary>
        /// <param name="area"></param>
        /// <param name="areaid"></param>
        /// <returns></returns>
        public List<ExpUpdDto> getExpUpdates(String area, long areaid)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "area", Value = area });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = areaid });
                return ctx.ExecuteStoreQuery<ExpUpdDto>(QUERYEXPCALCIDS, parameters.ToArray()).ToList();
            }
        }

        /// <summary>
        /// Returns one Updateinfo for the area and id and sysexptyp, independent of mode/age of the value
        /// (all indicators for one area)
        /// </summary>
        /// <param name="area"></param>
        /// <param name="areaid"></param>
        /// <param name="areaid"></param>
        /// <returns></returns>
        public List<ExpUpdDto> getExpUpdate(String area, long areaid, long sysexptyp)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "area", Value = area });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = areaid });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysexptyp", Value = sysexptyp });
                return ctx.ExecuteStoreQuery<ExpUpdDto>(QUERYEXPCALCID, parameters.ToArray()).ToList();
            }
        }

        /// <summary>
        /// Returns all Updateinfos for the area and ids
        /// (one indicator for all items of this area)
        /// </summary>
        /// <param name="area"></param>
        /// <param name="areaids"></param>
        /// <returns></returns>
        public List<ExpUpdDto> getExpDefUpdates(String area, long[] areaids)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "area", Value = area });
                return ctx.ExecuteStoreQuery<ExpUpdDto>(QUERYEXPDEFCALCIDS + " ( " + String.Join(",", areaids) + ")", parameters.ToArray()).ToList();
            }
        }

        /// <summary>
        /// returns all indicators gui values for the area
        /// </summary>
        /// <param name="area"></param>
        /// <param name="areaid"></param>
        /// <returns></returns>
        public List<ExpdispDto> getExpdisps(String area, long areaid)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "area", Value = area });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = areaid });
                return ctx.ExecuteStoreQuery<ExpdispDto>(QUERYEXPDISPS, parameters.ToArray()).ToList();
            }
        }

        /// <summary>
        /// returns one indicators gui value for the area
        /// </summary>
        /// <param name="area"></param>
        /// <param name="areaid"></param>
        /// <param name="sysexptyp"></param>
        /// <returns></returns>
        public List<ExpdispDto> getExpdisp(String area, long areaid, long sysexptyp)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "area", Value = area });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = areaid });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysexptyp", Value = sysexptyp });
                return ctx.ExecuteStoreQuery<ExpdispDto>(QUERYEXPDISP, parameters.ToArray()).ToList();
            }
        }

        /// <summary>
        /// returns all default indicators
        /// </summary>
        /// <param name="area"></param>
        /// <param name="areaid"></param>
        /// <returns></returns>
        public List<ExpdefDto> getExpdef(String area, long[] areaid)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "area", Value = area });
                return ctx.ExecuteStoreQuery<ExpdefDto>(QUERYEXPDISPDEF + "(" + String.Join(",", areaid) + ")", parameters.ToArray()).ToList();
            }
        }


        /// <summary>
        /// returns the default indicator for the area
        /// </summary>
        /// <param name="area"></param>
        /// <param name="defaultflag"></param>
        /// <returns></returns>
        public ExptypDto getExptype(String area, int defaultflag)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "area", Value = area });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "defaultflag", Value = defaultflag });
                return ctx.ExecuteStoreQuery<ExptypDto>(QUERYEXPTYPE, parameters.ToArray()).FirstOrDefault();
            }
        }
        /// <summary>
        /// returns all indicators for the area
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public List<ExptypDto> getExptypes(String area)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "area", Value = area });
                return ctx.ExecuteStoreQuery<ExptypDto>(QUERYEXPTYPES, parameters.ToArray()).ToList();
            }
        }


        /// <summary>
        /// returns all indicator ranges for the area
        /// </summary>
        /// <param name="area"></param>
        /// <returns></returns>
        public List<ExprangeDto> getExpranges(String area)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "area", Value = area });
                return ctx.ExecuteStoreQuery<ExprangeDto>(QUERYEXPRANGES, parameters.ToArray()).ToList();

            }
        }

        /// <summary>
        /// Liefert die SysPerole für einen Wfuser. 
        /// Achtung: Falls die lokale Variable schon die Perole beinhaltet, wird diese zurückgeliefert.
        /// </summary>
        /// <param name="sysWfuser">Wfuser welcher gesucht wird</param>
        /// <returns>Perole von dem User</returns>
        public long getSysPerole(long sysWfuser)
        {
            if (this.sysPerole != 0)
                return getSysPerole();

            using (Cic.OpenOne.Common.Model.DdOw.DdOwExtended ctx = new Cic.OpenOne.Common.Model.DdOw.DdOwExtended())
            {
                MembershipProvider prov = new MembershipProvider(ctx);

                var sysPerson = (from wfuser in ctx.WFUSER
                                 where wfuser.SYSWFUSER == sysWfuser
                                 select wfuser.SYSPERSON).FirstOrDefault();

                if (sysPerson.HasValue)
                {
                    Cic.OpenOne.Common.DTO.PeroleDto perole = prov.getUserRoles(sysPerson.Value).FirstOrDefault();
                    if (perole != null)
                    {
                        this.sysPerole = perole.SYSPEROLE;
                        return perole.SYSPEROLE;
                    }
                }
            }
            return 0;
        }

        /// <summary>
        /// returns select items for a rub entry
        /// </summary>
        /// <param name="sysddlkpcol"></param>
        /// <returns></returns>
        public List<DdlkpposDto> getDdlkppos(long sysddlkpcol)
        {
            if (!DdlkpposCache.ContainsKey(sysddlkpcol))
            {
                DdlkpposCache.Clear();
                using (DdCtExtended ctx = new DdCtExtended())
                {
                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    //parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysddlkpcol", Value = sysddlkpcol });
                    List<DdlkpposDto> pos = ctx.ExecuteStoreQuery<DdlkpposDto>(QUERYDDLKPPOSCOL, parameters.ToArray()).ToList();
                    List<DdlkpposDto> clist = null;
                    long lastsyskey = -1;
                    foreach (DdlkpposDto p in pos)
                    {
                        if (p.sysddlkpcol != lastsyskey)
                        {
                            lastsyskey = p.sysddlkpcol;
                            if (!DdlkpposCache.ContainsKey(lastsyskey))
                            {
                                clist = new List<DdlkpposDto>();
                                DdlkpposCache[lastsyskey] = clist;
                            }
                            else
                                clist = DdlkpposCache[lastsyskey];

                        }
                        clist.Add(p);
                    }

                }
            }
            return DdlkpposCache[sysddlkpcol];
        }

        /// <summary>
        /// returns the rub entries
        /// </summary>
        /// <param name="sysddlkprub"></param>
        /// <returns></returns>
        public List<DdlkpcolDto> getDdlkpcols(long sysddlkprub)
        {
            if (!DdlkpcolCache.ContainsKey(sysddlkprub))
            {
                using (DdCtExtended ctx = new DdCtExtended())
                {
                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    //parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysddlkprub", Value = sysddlkprub });
                    List<DdlkpcolDto> pos = ctx.ExecuteStoreQuery<DdlkpcolDto>(QUERYDDLKPCOLS, parameters.ToArray()).ToList();
                    List<DdlkpcolDto> clist = null;
                    long lastsyskey = -1;
                    foreach (DdlkpcolDto p in pos)
                    {
                        if (p.sysddlkprub != lastsyskey)
                        {
                            lastsyskey = p.sysddlkprub;
                            if (!DdlkpcolCache.ContainsKey(lastsyskey))
                            {
                                clist = new List<DdlkpcolDto>();
                                DdlkpcolCache[lastsyskey] = clist;
                            }
                            else
                                clist = DdlkpcolCache[lastsyskey];

                        }
                        clist.Add(p);
                    }

                }
            }
            List<DdlkpcolDto> rval = new List<DdlkpcolDto>();
            List<DdlkpcolDto> cachedcol = DdlkpcolCache[sysddlkprub];
            foreach (DdlkpcolDto c in cachedcol)
            {
                rval.Add(new DdlkpcolDto(c));
            }
            return rval;

        }

        /// <summary>
        /// returns a list of rubs for the area
        /// </summary>
        /// <param name="crmarea"></param>
        /// <returns></returns>
        public List<DdlkprubDto> getDdlkprubs(String code, String area)
        {
            String key = code + "_" + area;
            if (!DdlkprubCache.ContainsKey(key))
            {
                using (DdCtExtended ctx = new DdCtExtended())
                {
                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                      
                    String query = QUERYDDLKPRUBS;
                    if(code!=null && area!=null){
                        query = QUERYDDLKPRUBSCODEAREA;
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "area", Value = area });
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "code", Value = code });
                    }
                    else if(code!=null && area==null){
                        query = QUERYDDLKPRUBSCODE;
                        
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "code", Value = code });
                    }
                    else if(code==null && area!=null){
                        query = QUERYDDLKPRUBSAREA;
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "area", Value = area });
                    }
                    
                   

                    List<DdlkprubDto> pos = ctx.ExecuteStoreQuery<DdlkprubDto>(query, parameters.ToArray()).ToList();
                    DdlkprubCache[key] = pos;
                    /*
                    List<DdlkprubDto> clist = null;
                    String lastsyskey = null;
                    foreach (DdlkprubDto p in pos)
                    {
                        if (p.code == null) continue;
                        if (p.area == null) continue;
                        //if (!p.code.Equals(lastsyskey))
                        {
                            lastsyskey = p.code+"_"+p.area;
                            if (!DdlkprubCache.ContainsKey(lastsyskey))
                            {
                                clist = new List<DdlkprubDto>();
                                DdlkprubCache[lastsyskey] = clist;
                            }
                            else
                                clist = DdlkprubCache[lastsyskey];

                        }
                        clist.Add(p);
                    }*/

                }
            }
            return DdlkprubCache[key];



        }

        /// <summary>
        /// returns a list of Ddlkpspos (rub-values for a certain entity)
        /// </summary>
        /// <param name="area"></param>
        /// <param name="areaid"></param>
        /// <returns></returns>
        public List<DdlkpsposDto> getDdlkpspos(String area, long areaid)
        {
            if (area == null || areaid == 0) return null;

            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "area", Value = area });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = areaid });


                return ctx.ExecuteStoreQuery<DdlkpsposDto>(QUERYDDLKPSPOSAREA, parameters.ToArray()).ToList();

            }
        }

        /// <summary>
        /// Returns all ObjektDetails
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public virtual ObjektDto getObjektDetails(long sysob)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysob", Value = sysob });

                ObjektDto rval = ctx.ExecuteStoreQuery<ObjektDto>(QUERYOBJEKT, parameters.ToArray()).FirstOrDefault();

                parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysob", Value = sysob });
                // rval.BRIEF = ctx.ExecuteStoreQuery<BriefDto>(QUERYBRIEF, parameters.ToArray()).FirstOrDefault();
                return rval;
            }
        }

        /// <summary>
        /// returns all Obtyp Details from mat views vc_obtyp1-5
        /// </summary>
        /// <param name="sysobtyp"></param>
        /// <returns></returns>
        public virtual ObtypDto getObtypDetails(long sysobtyp)
        {

            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysobtyp", Value = sysobtyp });

                ObtypDto rval = ctx.ExecuteStoreQuery<ObtypDto>(QUERYOBTYP, parameters.ToArray()).FirstOrDefault();

                if (rval == null)
                {
                    rval = new ObtypDto();
                    rval.bezeichnung = "Objekt nicht gefunden";
                }

                // if (rval == null)
                {


                    parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysobtyp", Value = sysobtyp });
                    ObtypDto tmpval = ctx.ExecuteStoreQuery<ObtypDto>("select vc_obtyp5.art fzart, vc_obtyp5.neupreisbrutto, vc_obtyp5.leistung, vc_obtyp5.neupreisnetto, vc_obtyp5.baujahr, vc_obtyp5.bezeichnung modell, vc_obtyp3.bezeichnung fabrikat, vc_obtyp4.bezeichnung baureihe, vc_obtyp2.bezeichnung hersteller, vc_obtyp2.bezeichnung marke from vc_obtyp2,vc_obtyp3,vc_obtyp4,vc_obtyp5 where vc_obtyp2.id2=vc_obtyp3.id2 and vc_obtyp3.id3=vc_obtyp4.id3 and vc_obtyp4.id4=vc_obtyp5.id4 and vc_obtyp5.id5=:sysobtyp", parameters.ToArray()).FirstOrDefault();
                    if (tmpval != null)
                    {
                        rval.marke = tmpval.marke;
                        rval.modell = tmpval.modell;
                        rval.fzart = tmpval.fzart;
                    }

                    /* parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysobtyp", Value = sysobtyp });
                    rval.bezeichnung = ctx.ExecuteStoreQuery<String>("select beschreibung from obtyp where sysobtyp=:sysobtyp", parameters.ToArray()).FirstOrDefault();
                    */

                }

                /* Cic.OpenLease.Service.Services.DdOl.VehicleDao vd = new OpenLease.Service.Services.DdOl.VehicleDao();
                 Cic.OpenLease.ServiceAccess.DdOl.BmwTechnicalDataDto dataDto = new OpenLease.ServiceAccess.DdOl.BmwTechnicalDataDto();
                 vd.deliverBmwTechnicalDataExtendedFromObTyp(sysobtyp, 1, dataDto);
                 */

                if (rval.neupreisnetto == 0)
                {
                    parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysobtyp", Value = sysobtyp });
                    ObtypDto tmpval = ctx.ExecuteStoreQuery<ObtypDto>("select fztyp.typ typ,obtyp.bezeichnung,obtyp.beschreibung, obtyp.schwacke, obtyp.art fzart, obtyp.bezeichnung, obtyp.grund neupreisnetto, obtyp.schwacke, obtyp.bgn,obtyp.fabrikat marke from obtyp left outer join fztyp on obtyp.sysfztyp=fztyp.sysfztyp  where obtyp.sysobtyp=:sysobtyp", parameters.ToArray()).FirstOrDefault();
                    if (tmpval != null)
                    {
                        rval.fzart = tmpval.fzart;
                        rval.marke = tmpval.marke;
                        rval.modell = tmpval.bezeichnung;
                        rval.bezeichnung = tmpval.typ;
                        rval.beschreibung = tmpval.beschreibung;
                        rval.schwacke = tmpval.schwacke;
                        rval.neupreisnetto = tmpval.neupreisnetto;
                        rval.bgn = tmpval.bgn;
                        rval.baujahr = 2015;
                        rval.baumonat = 1;

                    }
                }

                rval.sysobtyp = sysobtyp;
                return rval;
            }
        }

        /// <summary>
        /// Returns all Obkat Details
        /// </summary>
        /// <param name="sysobkat"></param>
        /// <returns></returns>
        public ObkatDto getObkatDetails(long sysobkat)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysobkat", Value = sysobkat });
                ObkatDto rval = ctx.ExecuteStoreQuery<ObkatDto>(QUERYOBKAT, parameters.ToArray()).FirstOrDefault();
                return rval;
            }
        }

        /// <summary>
        /// Returns all Vart Details
        /// </summary>
        /// <param name="sysvart"></param>
        /// <returns></returns>
        public VartDto getVartDetails(long sysvart)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysvart", Value = sysvart });
                VartDto rval = ctx.ExecuteStoreQuery<VartDto>(QUERYVART, parameters.ToArray()).FirstOrDefault();
                return rval;
            }
        }

        /// <summary>
        /// Returns all Vart Details
        /// </summary>
        /// <param name="sysvart"></param>
        /// <returns></returns>
        public List<VartDto> getVarten()
        {
            if(!vartCache.ContainsKey("VART"))
            {
                using (DdCtExtended ctx = new DdCtExtended())
                {
                    vartCache["VART"]= ctx.ExecuteStoreQuery<VartDto>(QUERYVARTS,null ).ToList();
                }

            }
            return vartCache["VART"];
        }

        /// <summary>
        /// Returns all Recalc Details
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public RecalcDto getRecalcDetails(long sysrecalc)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysrecalc", Value = sysrecalc });

                RecalcDto rval = ctx.ExecuteStoreQuery<RecalcDto>(QUERYRECALC, parameters.ToArray()).FirstOrDefault();

                parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysvt", Value = rval.sysvt });
                rval.rnvflag = ctx.ExecuteStoreQuery<int>(QUERYRECALCRV, parameters.ToArray()).FirstOrDefault();
                parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysvt", Value = rval.sysvt });
                rval.anabflag = ctx.ExecuteStoreQuery<int>(QUERYRECALCANAB, parameters.ToArray()).FirstOrDefault();
                parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysvt", Value = rval.sysvt });
                rval.fuelflag = ctx.ExecuteStoreQuery<int>(QUERYRECALCFUEL, parameters.ToArray()).FirstOrDefault();
                parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysvt", Value = rval.sysvt });
                rval.tireflag = ctx.ExecuteStoreQuery<int>(QUERYRECALCTIRE, parameters.ToArray()).FirstOrDefault();
                parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysvt", Value = rval.sysvt });
                rval.svflag = ctx.ExecuteStoreQuery<int>(QUERYRECALCSV, parameters.ToArray()).FirstOrDefault();
                parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysvt", Value = rval.sysvt });
                rval.soflag = ctx.ExecuteStoreQuery<int>(QUERYRECALCSO, parameters.ToArray()).FirstOrDefault();
                parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysvt", Value = rval.sysvt });
                rval.ewflag = ctx.ExecuteStoreQuery<int>(QUERYRECALCEW, parameters.ToArray()).FirstOrDefault();
                parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysvt", Value = rval.sysvt });
                rval.depot = ctx.ExecuteStoreQuery<double>(QUERYRECALCDEPOT, parameters.ToArray()).FirstOrDefault();


                parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysvt", Value = rval.sysvt });
                rval.zinsvariabel = ctx.ExecuteStoreQuery<int>(QUERYRECALCZINS, parameters.ToArray()).FirstOrDefault();
                parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysvt", Value = rval.sysvt });
                rval.servicevariabel = ctx.ExecuteStoreQuery<int>(QUERYRECALCSERVICE, parameters.ToArray()).FirstOrDefault();
                parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysvt", Value = rval.sysvt });
                rval.reifenvariabel = ctx.ExecuteStoreQuery<int>(QUERYRECALCREIFEN, parameters.ToArray()).FirstOrDefault();
                parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysvt", Value = rval.sysvt });
                rval.sysrvt = ctx.ExecuteStoreQuery<long>(QUERYRECALCSYSRVT, parameters.ToArray()).FirstOrDefault();

                return rval;
            }
        }

        /// <summary>
        /// Returns all Mycalc Details
        /// </summary>
        /// <param name="sysmycalc"></param>
        /// <returns></returns>
        public MycalcDto getMycalcDetails(long sysmycalc)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysmycalc", Value = sysmycalc });

                MycalcDto rval = ctx.ExecuteStoreQuery<MycalcDto>(QUERYMYCALC, parameters.ToArray()).FirstOrDefault();

                if (rval.sysobtyp == 0)//not filled, search for it
                {
                    rval.sysobtyp = ctx.ExecuteStoreQuery<long>("select angob.sysobtyp from angob,angvar where angob.sysvt=angvar.sysangvar and angvar.sysangebot=" + rval.sysangebot, null).FirstOrDefault();
                }

                parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysmycalc", Value = sysmycalc });
                rval.equipment = ctx.ExecuteStoreQuery<MycalcaustDto>(QUERYMYCALCAUST, parameters.ToArray()).ToList();

                parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysmycalcfs", Value = sysmycalc });
                rval.serviceKalkulation = ctx.ExecuteStoreQuery<MycalcfsDto>(QUERYMYCALCFS, parameters.ToArray()).FirstOrDefault();
                return rval;
            }
        }

        /// <summary>
        /// Returns all RahmenDetails
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public RahmenDto getRahmenDetails(long sysrvt)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysrvt", Value = sysrvt });

                RahmenDto rval = ctx.ExecuteStoreQuery<RahmenDto>(QUERYRAHMEN, parameters.ToArray()).FirstOrDefault();
                if (rval == null) return new RahmenDto();
                parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysrvt", Value = sysrvt });
                rval.positionen = ctx.ExecuteStoreQuery<RvtPosDto>(QUERYRAHMENPOS, parameters.ToArray()).ToList();

                return rval;
            }
        }

        /// <summary>
        /// Returns all Haendler Details
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        public HaendlerDto getHaendlerDetails(long sysperson)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperson", Value = sysperson });

                HaendlerDto rval = ctx.ExecuteStoreQuery<HaendlerDto>(QUERYHAENDLER, parameters.ToArray()).FirstOrDefault();


                return rval;
            }
        }

        /// <summary>
        /// loads customer details from db
        /// </summary>
        /// <param name="sysperson"></param>
        /// <returns></returns>
        public KundeDto getKundeDetails(long sysperson)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperson", Value = sysperson });
                KundeDto rval = ctx.ExecuteStoreQuery<KundeDto>(QUERYKUNDE, parameters.ToArray()).FirstOrDefault();
                //ggf Mappen wenn über EDMX gelesen wird:
                //rval = Mapper.Map<PERSON, KundeDto>(queryResult, rval);
                return rval;
            }
        }

        /// <summary>
        /// loads logdump details from DB
        /// </summary>
        /// <param name="logdump"></param>
        /// <returns></returns>
        public LogDumpDto getLogDumpDetails(long logdump)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List <OracleParameter> parameters = new List <OracleParameter>();
                parameters.Add(new OracleParameter { ParameterName = "syslogdump", Value = logdump });
                LogDumpDto rval = ctx.ExecuteStoreQuery <LogDumpDto> (QUERYLOGDUMP, parameters.ToArray()).FirstOrDefault();
                //ggf Mappen wenn über EDMX gelesen wird:
                //rval = Mapper.Map<LOGDUMP, LogDumpDto>(queryResult, rval);
                return rval;
            }
        }

        /// <summary>
        /// loads it details from db
        /// </summary>
        /// <param name="sysit"></param>
        /// <returns></returns>
        public ItDto getItDetails(long sysit)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                DbConnection con = (ctx.Connection as EntityConnection).StoreConnection;
                //List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                //parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = sysit });
                ItDto rval = con.Query<ItDto>(QUERYIT, new { sysit=sysit}).FirstOrDefault();
                //ggf Mappen wenn über EDMX gelesen wird:
                //rval = Mapper.Map<IT, ItDto>(queryResult, rval);
                return rval;
            }
        }

        /// <summary>
        /// loads angkalk details from db
        /// </summary>
        /// <param name="sysangkalk"></param>
        /// <returns></returns>
        public AngkalkDto getAngkalkDetails(long sysangkalk)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysangkalk", Value = sysangkalk });
                AngkalkDto rval = ctx.ExecuteStoreQuery<AngkalkDto>(QUERYANGKALK, parameters.ToArray()).FirstOrDefault();


                parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysangkalk", Value = sysangkalk });
                rval.zahlplan = ctx.ExecuteStoreQuery<SlDto>(QUERYSLPOS, parameters.ToArray()).ToList();



                return rval;
            }
        }

        /// <summary>
        /// loads antkalk details from db
        /// </summary>
        /// <param name="sysantkalk"></param>
        /// <returns></returns>
        public AntkalkDto getAntkalkDetails(long sysantkalk)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysantkalk", Value = sysantkalk });
                AntkalkDto rval = ctx.ExecuteStoreQuery<AntkalkDto>(QUERYANTKALK, parameters.ToArray()).FirstOrDefault();


                parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysantkalk", Value = sysantkalk });
                rval.zahlplan = ctx.ExecuteStoreQuery<SlDto>(QUERYSLPOSANT, parameters.ToArray()).ToList();
                


                return rval;
            }
        }

        /// <summary>
        /// loads angvar details from db
        /// </summary>
        /// <param name="sysangvar"></param>
        /// <returns></returns>
        public AngvarDto getAngvarDetails(long sysangvar)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysangvar", Value = sysangvar });
                AngvarDto rval = ctx.ExecuteStoreQuery<AngvarDto>(QUERYANGVAR, parameters.ToArray()).FirstOrDefault();
                //ggf Mappen wenn über EDMX gelesen wird:
                //rval = Mapper.Map<ANGVAR, AngvarDto>(queryResult, rval);
                return rval;
            }
        }

        /// <summary>
        /// loads angob details from db
        /// </summary>
        /// <param name="sysangob"></param>
        /// <returns></returns>
        public AngobDto getAngobDetails(long sysangob)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysangob", Value = sysangob });
                AngobDto rval = ctx.ExecuteStoreQuery<AngobDto>(QUERYANGOB, parameters.ToArray()).FirstOrDefault();


                List<Devart.Data.Oracle.OracleParameter> obpar = new List<Devart.Data.Oracle.OracleParameter>();
                obpar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysobini", Value = rval.sysOb });
                rval.zusatzdaten = ctx.ExecuteStoreQuery<AngobIniDto>(QUERYANGOBINI, obpar.ToArray()).FirstOrDefault();
                List<Devart.Data.Oracle.OracleParameter> austpar = new List<Devart.Data.Oracle.OracleParameter>();
                austpar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysangob", Value = rval.sysOb });
                rval.ausstattungen = ctx.ExecuteStoreQuery<ObjektAustDto>(QUERYANGOBAUST, austpar.ToArray()).ToList();


                //ggf Mappen wenn über EDMX gelesen wird:
                //rval = Mapper.Map<ANGOB, AngobDto>(queryResult, rval);
                return rval;
            }
        }

        /// <summary>
        /// loads ob details from db
        /// </summary>
        /// <param name="sysangob"></param>
        /// <returns></returns>
        public ObDto getObDetails(long sysob)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                DbConnection con = (ctx.Connection as EntityConnection).StoreConnection;
                
                ObDto rval = con.Query<ObDto>(QUERYOB, new { sysob = sysob }).FirstOrDefault();


                
                
                rval.ausstattungen = con.Query<ObjektAustDto>(QUERYOBAUST, new { sysob = sysob }).ToList();
                //ggf Mappen wenn über EDMX gelesen wird:
                //rval = Mapper.Map<ANGOB, AngobDto>(queryResult, rval);

                //ob.sysob=obbrief.sysobbrief

                try
                {
                    if (rval.sysVT > 0)
                    {
                        BmwVtInfosDto infos = ctx.ExecuteStoreQuery<BmwVtInfosDto>("select * from Table(CIC.BMW_GET_KM_INFO(" + rval.sysVT.ToString() + "))", null).FirstOrDefault();
                        rval.kmbmw = infos.km;
                        rval.datum = infos.datum;
                        rval.source = infos.source;
                    }
                }
                catch (Exception)
                {

                }

                if (sysob != 0)
                {
                    try
                    {
                        
                        rval.obbrief = con.Query<ObbriefDto>("select * from cic.obbrief where cic.obbrief.sysobbrief = " + sysob.ToString()).FirstOrDefault();
                    }
                    catch (Exception)
                    {
                        rval.obbrief = null;
                    }
                }


                
                rval.zusatzdaten = con.Query<ObIniDto>(QUERYOBINI,new {sysobini=rval.sysOb } ).FirstOrDefault();

                String mandant = ctx.ExecuteStoreQuery<String>("select mandant from lsadd").FirstOrDefault();
                if ("HCSD".Equals(mandant) && rval.schwacke != null)
                {
                    if(rval.obbrief==null)
                        rval.obbrief = new ObbriefDto();

                    rval.obbrief.treibstoff = ctx.ExecuteStoreQuery<String>("select textlong from etgtxttabel,etgtype where etgtype.TXTFUELTYPECD2=etgtxttabel.code and NATCODE='" + rval.schwacke+"'", null).FirstOrDefault();
                    rval.obbrief.getriebe = ctx.ExecuteStoreQuery<String>("select textlong from etgtxttabel,etgtype where etgtype.TXTTRANSTYPECD2=etgtxttabel.code and NATCODE='" + rval.schwacke + "'", null).FirstOrDefault();
                    ObbriefDto tmpbrief = con.Query<ObbriefDto>("select totwgt zulgew,seat sitze,door splatz,kw from etgtype where NATCODE='" + rval.schwacke + "'").FirstOrDefault();
                    if (tmpbrief != null) {
                        rval.obbrief.zulgew = tmpbrief.zulgew;
                        rval.anzahlSitze =(int) tmpbrief.sitze;
                        rval.anzahlTueren = (int)tmpbrief.splatz;
                        rval.obbrief.kw = tmpbrief.kw;
                    }

                    rval.obbrief.tank = ctx.ExecuteStoreQuery<long>("select FUELCAP from ETGTECHNIC where NATCODE='" + rval.schwacke + "'", null).FirstOrDefault();
                    if (rval.zusatzdaten == null)
                        rval.zusatzdaten = new ObIniDto();

                    String secfuel = ctx.ExecuteStoreQuery<String>("select SECFUELTYPCD2 from etgtype where NATCODE='" + rval.schwacke + "'", null).FirstOrDefault();
                    rval.zusatzdaten.hybrid = isHybrid(secfuel)?1:0;
                }


                return rval;
            }
        }
        public static bool isHybrid(String secfueltypcd2)
        {
            if (secfueltypcd2 == null) return false;
            bool isHybrid = secfueltypcd2 == "00180010";
            if (isHybrid) return true;

            switch (secfueltypcd2)
            {
                case "00100012":
                case "00100003":
                case "00100001":
                case "00100002":
                case "00100011":
                case "00100004":
                    return true;

                default:
                    return false;
            }
        }

        /// <summary>
        /// Returns all OpportunityDetails
        /// </summary>
        /// <param name="sysob"></param>
        /// <returns></returns>
        virtual public OpportunityDto getOpportunityDetails(long sysopportunity)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysoppo", Value = sysopportunity });
                OpportunityDto rval = ctx.ExecuteStoreQuery<OpportunityDto>(QUERYOPP, parameters.ToArray()).FirstOrDefault();
                //ggf Mappen wenn über EDMX gelesen wird:
                //rval = Mapper.Map<PERSON, KundeDto>(queryResult, rval);
                return rval;
            }
        }

        /// <summary>
        /// Returns all Oppotask Details
        /// </summary>
        /// <param name="sysOppotask"></param>
        /// <returns></returns>
        public OppotaskDto getOppotaskDetails(long sysOppotask)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysOppotask", Value = sysOppotask });
                OppotaskDto rval = ctx.ExecuteStoreQuery<OppotaskDto>(QUERYOPPOTASK, parameters.ToArray()).FirstOrDefault();

                if ("VT".Equals(rval.area) && rval.sysid > 0)
                {
                    parameters = new List<Devart.Data.Oracle.OracleParameter>();

                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = rval.sysid });
                    OppotaskDto addInfos = ctx.ExecuteStoreQuery<OppotaskDto>("select  angob.configid sa3angebot, angebot.angebot, angebot.sysid sysangebot from angebot, vt,angob where angob.sysob = angebot.sysid and angebot.sysid=vt.sysangebot and vt.sysid=:sysid", parameters.ToArray()).FirstOrDefault();
                    if (addInfos != null)
                    {
                        rval.angebot = addInfos.angebot;
                        rval.sa3angebot = addInfos.sa3angebot;
                        rval.sysangebot = addInfos.sysangebot;
                    }
                }
                double daydiff = (rval.duedate - DateTime.Now).TotalDays;
                if (daydiff > 2)
                    rval.phase = 0;
                else if (daydiff <= 2 && daydiff >= 0)
                    rval.phase = 1;
                else rval.phase = 2;


                if (rval.syswfuser > 0)
                {
                    rval.ownershipsf = ctx.ExecuteStoreQuery<int>(@"select 1 from dual where exists (SELECT roletype.typ FROM perole, roletype    WHERE roletype.typ in (3, 9, 14, 4) and roletype.sysroletype = perole.sysroletype and (validfrom is null or validfrom <= trunc(sysdate)) AND 
(validuntil is null or validuntil >= trunc(sysdate))   AND sysperson IN (SELECT person.sysperson FROM person,puser WHERE puser.syspuser =person.syspuser and puser.syswfuser=" + rval.syswfuser + "))", null).FirstOrDefault();
                }

                return rval;
            }
        }

        /// <summary>
        /// Returns all ContactDetails
        /// </summary>
        /// <param name="syscontact"></param>
        /// <returns></returns>
        public ContactDto getContactDetails(long syscontact)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syscontact", Value = syscontact });
                ContactDto rval = ctx.ExecuteStoreQuery<ContactDto>(QUERYCONTACT, parameters.ToArray()).FirstOrDefault();

                //long? sysptrelate = (from c in ctx.PTRELATE where c.SYSPERSON1 == rval.sysPerson && c.SYSPERSON2==rval.sysPartner select c.SYSPTRELATE).FirstOrDefault();
                //rval.sysPtrelate = sysptrelate.HasValue ? sysptrelate.Value : 0;

                //ggf Mappen wenn über EDMX gelesen wird:
                //rval = Mapper.Map<PERSON, KundeDto>(queryResult, rval);
                return rval;
            }
        }

        /// <summary>
        /// Returns all KontoDetails
        /// </summary>
        /// <param name="syskonto"></param>
        /// <returns></returns>
        public KontoDto getKontoDetails(long syskonto)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syskonto", Value = syskonto });
                KontoDto rval = ctx.ExecuteStoreQuery<KontoDto>(QUERYKONTO, parameters.ToArray()).FirstOrDefault();
                //ggf Mappen wenn über EDMX gelesen wird:
                //rval = Mapper.Map<PERSON, KundeDto>(queryResult, rval);
                return rval;
            }
        }

        /// <summary>
        /// Returns all ItkontoDetails
        /// </summary>
        /// <param name="sysitkonto"></param>
        /// <returns></returns>
        public ItkontoDto getItkontoDetails(long sysitkonto)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysitkonto", Value = sysitkonto });
                ItkontoDto rval = ctx.ExecuteStoreQuery<ItkontoDto>(QUERYITKONTO, parameters.ToArray()).FirstOrDefault();
                //ggf Mappen wenn über EDMX gelesen wird:
                //rval = Mapper.Map<PERSON, KundeDto>(queryResult, rval);
                return rval;
            }
        }

        /// <summary>
        /// Returns all CampDetails
        /// </summary>
        /// <param name="syskonto"></param>
        /// <returns></returns>
        public CampDto getCampDetails(long syscamp)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syscamp", Value = syscamp });
                CampDto rval = ctx.ExecuteStoreQuery<CampDto>(QUERYCAMP, parameters.ToArray()).FirstOrDefault();
                //ggf Mappen wenn über EDMX gelesen wird:
                //rval = Mapper.Map<PERSON, KundeDto>(queryResult, rval);
                return rval;
            }
        }

        /// <summary>
        /// Returns all WfuserDetails
        /// </summary>
        /// <param name="syskonto"></param>
        /// <returns></returns>
        public WfuserDto getWfuserDetails(long syswfuser)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syswfuser", Value = syswfuser });
                WfuserDto rval = ctx.ExecuteStoreQuery<WfuserDto>(QUERYWFUSER, parameters.ToArray()).FirstOrDefault();
                //ggf Mappen wenn über EDMX gelesen wird:
                //rval = Mapper.Map<PERSON, KundeDto>(queryResult, rval);
                return rval;
            }
        }

        /// <summary>
        /// Returns all AdresseDetails
        /// </summary>
        /// <param name="sysadresse"></param>
        /// <returns></returns>
        public AdresseDto getAdresseDetails(long sysadresse)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysadresse", Value = sysadresse });
                AdresseDto rval = ctx.ExecuteStoreQuery<AdresseDto>(QUERYADRESSE, parameters.ToArray()).FirstOrDefault();
                //ggf Mappen wenn über EDMX gelesen wird:
                //rval = Mapper.Map<PERSON, KundeDto>(queryResult, rval);
                return rval;
            }
        }

       

        /// <summary>
        ///  Returns all FinanzierungDetails
        /// </summary>
        /// <param name="sysnkk"></param>
        /// <returns></returns>
        public FinanzierungDto getFinanzierungDetails(long sysnkk)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                DbConnection con = (ctx.Connection as EntityConnection).StoreConnection;
                FinanzierungDto rval = con.Query<FinanzierungDto>(QUERYFINANZIERUNG, new { sysnkk = sysnkk }).FirstOrDefault();

                
                long syswaehrung = ctx.ExecuteStoreQuery<long>("select syswaehrung from nkonto where sysnkonto=" + rval.sysnkonto, null).FirstOrDefault();
                //nkk.sysnkonto > nkonto.syswaehrung > waehrung.code
                rval.waehrung = getWaehrungDetail(syswaehrung);

                long sysob = ctx.ExecuteStoreQuery<long>("select sysob from ob where sysnkk=" + rval.sysnkk, null).FirstOrDefault();
                if (sysob > 0)
                {
                    rval.ob = getObDetails(sysob);
                    if(rval.ob!=null)
                        rval.SYSOB = rval.ob.sysOb;
                }
                //akt. Saldo
                rval.saldo = ctx.ExecuteStoreQuery<double>("select CIC.MDBS_GET_SALDO_NKK("+rval.sysnkk+",sysdate,'FI.ISINTREL=1') from dual", null).FirstOrDefault();
                //aktueller Zinssatz
                rval.aktZins = ctx.ExecuteStoreQuery<double>("select intkrate.intrate from intkdate, intkrate, intk, nkk where intkrate.sysintkdate = intkdate.sysintkdate and intkdate.sysintk = intk.sysintk and nkk.sysintks = intk.sysintk and intkdate.validfrom > nkk.endevf and intkdate.validfrom < sysdate and nkk.sysnkk = "+rval.sysnkk, null).FirstOrDefault();
                //nächste Abschlagzahlung
                FinanzierungDto tmpSl = con.Query<FinanzierungDto>(@"select slpos.BETRAG nextsl, slpos.VALUTA nextsldate
                                            from slpos, sl, sllink
                                            where sllink.syssl = sl.syssl
                                            and sl.syssl = slpos.syssl
                                            and slpos.gezogen = 0
                                            and sl.inaktiv = 0
                                            and sllink.gebiet = 'NKK'
                                            and sllink.sysid = " + rval.sysnkk + " order by slpos.valuta").FirstOrDefault();
                if(tmpSl!=null)
                { 
                    rval.nextsl = tmpSl.nextsl;
                    rval.nextsldate = tmpSl.nextsldate;
                }
                //bel. Tilgungen
                rval.belTilg = ctx.ExecuteStoreQuery<double>(@"select nvl(sum(slpos.betrag), 0)
                                from slpos, sl, sllink
                                where slpos.syssl = sl.syssl
                                and sl.syssl = sllink.syssl
                                and sllink.sysid = " + rval.sysnkk + " and gezogen = 1 and slpos.inaktiv = 0 and sllink.gebiet = 'NKK'", null).FirstOrDefault();
                //bel. Zinsen
                rval.belZins = ctx.ExecuteStoreQuery<double>(@"select nvl(sum(nkkabpos.betrag), 0)
                                            from nkkabschl, nkkabpos, nkk
                                            where nkkabpos.sysnkkabschl = nkkabschl.sysnkkabschl
                                            and nkkabschl.sysnkk = nkk.sysnkk
                                            and nkkabschl.bis >  nkk.endevf
                                            and nkkabschl.storno = 0
                                            and nkk.sysnkk = "+rval.sysnkk, null).FirstOrDefault();
                rval.zustand = ctx.ExecuteStoreQuery<String>(@"select zustandmap.zustandextern 
                                    from cic.nkk,
                                    (select statedef.zustand AS zustandextern, attributdef.attribut 
                                    zustandintern from statedef, state, attribut, attributdef where 
                                    attribut.sysattributdef = attributdef.sysattributdef and 
                                    attribut.sysstatedef = statedef.sysstatedef and attribut.sysstate = state.sysstate and state.syswftable = 180 and statedef.art = 1) zustandmap 
                                    where zustandmap.zustandintern=nkk.zustand and nkk.sysnkk=" + rval.sysnkk, null).FirstOrDefault();








                //long sysklinie = ctx.ExecuteStoreQuery<long>(@"select sysklinie from klinie where sysperson in (select sysperson from rvt where sysrvt 
                                                       //     in (select sysrvt from nkk where sysnkk =" + rval.sysnkk + "))",null).FirstOrDefault();

                rval.sysklinie = ctx.ExecuteStoreQuery<long>(@"select sysklinie from hdklinie where sysrvt  in  (select sysrvt from nkk where sysnkk =" + rval.sysnkk + ")", null).FirstOrDefault();
                
                String mandant = ctx.ExecuteStoreQuery<String>("select mandant from lsadd").FirstOrDefault();
                if ("HCSD".Equals(mandant))
                {
					////———————————————————————————————————————————————————————————————————————————
					//// MB ORIG: 
					//if (rval.sysvt > 0)
					//{
					//	String vtzustand = ctx.ExecuteStoreQuery<String>(@"select zustand from vt where sysid=" + rval.sysvt, null).FirstOrDefault();
					//	if (vtzustand == null) vtzustand = "NEU";
					//	vtzustand = vtzustand.ToUpper();
					//	FinanzierungDto vs = ctx.ExecuteStoreQuery<FinanzierungDto>(@"select status versandstatus,option6 lagerortbrief from vtobsich where sysvt=" + rval.sysvt, null).FirstOrDefault();
					//	//rval.lagerortbrief = vs.lagerortbrief;
					//	rval.versandstatus = vs.versandstatus;
					//	if (rval.versandstatus != null && rval.versandstatus.Length > 0)
					//	{
					//		if ("INAKTIV".Equals(vtzustand))
					//			rval.versandart = "endgültig angefordert";
					//	}

					//}
					//rval.lagerortbrief = ctx.ExecuteStoreQuery<String>("select standortbrief from ob where sysnkk=" + rval.sysnkk, null).FirstOrDefault();
					//rval.datumversand = ctx.ExecuteStoreQuery<DateTime>("select CIC_SYS.to_oradate(finishdate) from eaihot where code = 'PS_BEAUFTRAGUNG' and  oltable='OB' and sysoltable=" + sysob + " order by syseaihot desc", null).FirstOrDefault();

					//long countanf = ctx.ExecuteStoreQuery<long>("select count(*) from eaihot where code = 'PS_BEAUFTRAGUNG' and  oltable='OB' and sysoltable=" + sysob + " order by syseaihot desc", null).FirstOrDefault();
					//if (countanf > 0)
					//	rval.versandart = "temporär angefordert";
					//long entncount = ctx.ExecuteStoreQuery<long>("select count(*) from eaihot where outputparameter4='Endgültig entnommen' and code = 'PS_BEAUFTRAGUNG' and  oltable='OB' and sysoltable=" + sysob + " order by syseaihot desc", null).FirstOrDefault();
					//if (entncount > 0)
					//	rval.versandart = "endgültig angefordert";
					////———————————————————————————————————————————————————————————————————————————

					// rh 20171018: read from OBDEPOT tabel joind with sysob
					if (sysob > 0)
                    {
						//FinanzierungDto vs = ctx.ExecuteStoreQuery<FinanzierungDto> (@"SELECT ORT lagerortbrief, versandart,  STATUS versandstatus, DATUM datumversand FROM obdepot where sysob = " + sysob + " ORDER BY SYSOBDEPOT DESC", null).FirstOrDefault ();
						//if (vs != null)
						//{
						//	rval.lagerortbrief = vs.lagerortbrief;
						//	rval.versandstatus = vs.versandstatus;
						//	rval.versandart = vs.versandart;
						//	rval.datumversand = vs.datumversand;
						//}

						// rh 20171023:  
						// versandstatus UND datumversand
						FinanzierungDto vs = con.Query<FinanzierungDto> (@"SELECT STATUS versandstatus, DATUM datumversand FROM obdepot WHERE sysob = " + sysob + " AND STATUS >= 1 ORDER BY DATUM DESC, ZEIT DESC", null).FirstOrDefault ();
						if (vs != null)
						{
							rval.versandstatus = vs.versandstatus;
							rval.datumversand = vs.datumversand;
						}

						// ORT lagerortbrief,  --?? AND lagerortbrief IS NOT NULL
						vs = con.Query<FinanzierungDto> (@"SELECT ORT lagerortbrief FROM obdepot WHERE sysob = " + sysob + " ORDER BY DATUM DESC, ZEIT DESC", null).FirstOrDefault ();
						if (vs != null)
						{
							rval.lagerortbrief = vs.lagerortbrief;
						}

						// versandart
						vs = con.Query<FinanzierungDto> (@"SELECT CASE WHEN (versandart >= 1 AND versandart <= 4) THEN versandart ELSE null END as versandart FROM obdepot WHERE sysob = " + sysob + " AND versandart >= 1 ORDER BY DATUM DESC, ZEIT DESC", null).FirstOrDefault ();
						if (vs != null)
						{
							rval.versandart = vs.versandart;
						}




					}
				}
                
                return rval;
            }
        }



        public KreditlinieDto getKreditlinieDetail(long sysklinie)
        {
            KreditlinieDto rval = new KreditlinieDto();

            using (DdCtExtended ctx = new DdCtExtended())
            {
                rval = ctx.ExecuteStoreQuery<KreditlinieDto>(@"SELECT klinie.sysklinie, klinie.beginn,klinie.ende,klinart.bezeichnung art,waehrung.CODE waehrung, limitextern gesamtlimit 
                                        FROM hdklinie, klinie,klinart, waehrung  
                                        WHERE klinie.sysklinie = hdklinie.sysklinie and klinart.sysklinart=klinie.sysklinart and waehrung.syswaehrung=klinie.syswaehrung 
                                        AND klinie.sysklinie = " + sysklinie, null).FirstOrDefault();
           



                        ICASBo bo = Cic.One.Web.BO.BOFactoryFactory.getInstance().getCASBo();
                        if (sysklinie > 0)
                            if (bo != null)
                            {
                                Cic.OpenOne.Common.DTO.iCASEvaluateDto ieval = new Cic.OpenOne.Common.DTO.iCASEvaluateDto();
                                ieval.area = "KLINIE";
                                String KLINIE = "_KLINIE('KLINIE', KLINIE:SYSKLINIE, 'UTIL', '', '', 1, 0)";
                                ieval.expression = new String[] { KLINIE };
                                ieval.sysID = new long[] { sysklinie };
                                Cic.OpenOne.Common.DTO.CASEvaluateResult[] er = null;
                                try
                                {
                                    er = bo.getEvaluation(ieval, sysWfuser);
                                    if (er[0].clarionResult[0] != null && !"".Equals(er[0].clarionResult[0]))
                                        rval.ausschoepfung = double.Parse(er[0].clarionResult[0], CultureInfo.InvariantCulture);

                                }
                                catch (Exception e)
                                {
                                    _log.Warn("CAS-Evaluation failed for KLINIE", e);
                                }

                                rval.ausschoepfungP = Math.Round(rval.ausschoepfung / rval.gesamtlimit * 100,2);
                                rval.frei = rval.gesamtlimit - rval.ausschoepfung;
                                

                                ieval.area = "KLINIE";
                                KLINIE = "_KLINIE('KLINIE',KLINIE:SYSKLINIE,'UTIL',0,'NKK.ENDEVF >= trunc(sysdate)',1,0)";
                                ieval.expression = new String[] { KLINIE };
                                ieval.sysID = new long[] { sysklinie };
                                Cic.OpenOne.Common.DTO.CASEvaluateResult[] erFF = null;
                                try
                                {
                                    erFF = bo.getEvaluation(ieval, sysWfuser);
                                    if (erFF[0].clarionResult[0] != null && !"".Equals(erFF[0].clarionResult[0]))
                                        rval.ausschoepfungFF = double.Parse(erFF[0].clarionResult[0], CultureInfo.InvariantCulture);

                                }
                                catch (Exception e)
                                {
                                    _log.Warn("CAS-Evaluation failed for KLINIE", e);
                                }

                                ieval.area = "KLINIE";
                                KLINIE = "_KLINIE('KLINIE',KLINIE:SYSKLINIE,'UTIL',0,'NKK.ENDEVF < trunc(sysdate)',1,0)";
                                ieval.expression = new String[] { KLINIE };
                                ieval.sysID = new long[] { sysklinie };
                                Cic.OpenOne.Common.DTO.CASEvaluateResult[] erAF = null;
                                try
                                {
                                    erAF = bo.getEvaluation(ieval, sysWfuser);
                                    if (erAF[0].clarionResult[0] != null && !"".Equals(erAF[0].clarionResult[0]))
                                        rval.ausschoepfungAF = double.Parse(erAF[0].clarionResult[0], CultureInfo.InvariantCulture);

                                }
                                catch (Exception e)
                                {
                                    _log.Warn("CAS-Evaluation failed for KLINIE", e);
                                }

                                rval.ausschoepfungAFP =  Math.Round(rval.ausschoepfungAF / rval.gesamtlimit * 100,2);
                                rval.ausschoepfungFFP =  Math.Round(rval.ausschoepfungFF / rval.gesamtlimit * 100,2);
                                }
                          
                        }
                        return rval;


        }

        /// <summary>
        /// Returns all ItadresseDetails
        /// </summary>
        /// <param name="sysitadresse"></param>
        /// <returns></returns>
        public ItadresseDto getItadresseDetails(long sysitadresse)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysitadresse", Value = sysitadresse });
                ItadresseDto rval = ctx.ExecuteStoreQuery<ItadresseDto>(QUERYITADRESSE, parameters.ToArray()).FirstOrDefault();
                //ggf Mappen wenn über EDMX gelesen wird:
                //rval = Mapper.Map<PERSON, KundeDto>(queryResult, rval);
                return rval;
            }
        }

        /// <summary>
        /// Returns all PtaskDetails
        /// </summary>
        /// <param name="sysptask"></param>
        /// <returns></returns>
        public PtaskDto getPtaskDetails(long sysptask)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysptask", Value = sysptask });
                PtaskDto rval = ctx.ExecuteStoreQuery<PtaskDto>(QUERYPTASK, parameters.ToArray()).FirstOrDefault();
                //ggf Mappen wenn über EDMX gelesen wird:
                //rval = Mapper.Map<PERSON, KundeDto>(queryResult, rval);
                return rval;
            }
        }

        /// <summary>
        /// Returns all Nkonto Details
        /// </summary>
        /// <param name="sysnkonto"></param>
        /// <returns></returns>
        public NkontoDto getNkontoDetails(long sysnkonto)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysnkonto", Value = sysnkonto });
                NkontoDto rval = ctx.ExecuteStoreQuery<NkontoDto>(QUERYNKONTO, parameters.ToArray()).FirstOrDefault();
                //ggf Mappen wenn über EDMX gelesen wird:
                //rval = Mapper.Map<PERSON, KundeDto>(queryResult, rval);
                return rval;
            }
        }

        /// <summary>
        /// Returns all Printset Details
        /// </summary>
        /// <param name="sysprintset"></param>
        /// <returns></returns>
        public PrintsetDto getPrintsetDetails(long sysprintset)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysprintset", Value = sysprintset });
                PrintsetDto rval = ctx.ExecuteStoreQuery<PrintsetDto>(QUERYPRINTSET, parameters.ToArray()).FirstOrDefault();
                return rval;
            }
        }

        /// <summary>
        /// Returns all Prtlgset Details
        /// </summary>
        /// <param name="sysprtlgset"></param>
        /// <returns></returns>
        public PrtlgsetDto getPrtlgsetDetails(long sysprtlgset)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysprtlgset", Value = sysprtlgset });
                PrtlgsetDto rval = ctx.ExecuteStoreQuery<PrtlgsetDto>(QUERYPRTLGSET, parameters.ToArray()).FirstOrDefault();
                return rval;
            }
        }

        /// <summary>
        /// Returns all ApptmtDetails
        /// </summary>
        /// <param name="sysptask"></param>
        /// <returns></returns>
        public ApptmtDto getApptmtDetails(long sysapptmt)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysapptmt", Value = sysapptmt });
                ApptmtDto rval = ctx.ExecuteStoreQuery<ApptmtDto>(QUERYAPPTMT, parameters.ToArray()).FirstOrDefault();
                //ggf Mappen wenn über EDMX gelesen wird:
                //rval = Mapper.Map<PERSON, KundeDto>(queryResult, rval);
                return rval;
            }
        }

        /// <summary>
        /// Returns all ReminderDetails
        /// </summary>
        /// <param name="sysptask"></param>
        /// <returns></returns>
        public ReminderDto getReminderDetails(long sysreminder)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysreminder", Value = sysreminder });
                ReminderDto rval = ctx.ExecuteStoreQuery<ReminderDto>(QUERYREMINDER, parameters.ToArray()).FirstOrDefault();
                //ggf Mappen wenn über EDMX gelesen wird:
                //rval = Mapper.Map<PERSON, KundeDto>(queryResult, rval);
                return rval;
            }
        }

        /// <summary>
        /// Returns all Mailmsg Details
        /// </summary>
        /// <param name="sysptask"></param>
        /// <returns></returns>
        public MailmsgDto getMailmsgDetails(long sysmailmsg)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysmailmsg", Value = sysmailmsg });
                MailmsgDto mailmsg = ctx.ExecuteStoreQuery<MailmsgDto>(QUERYMAILMSG, parameters.ToArray()).FirstOrDefault();

                //replace mail-tags with name and remove closing mail tag for Editor in Frontend
                if (mailmsg != null && mailmsg.content != null)
                {
                    mailmsg.content = Regex.Replace(mailmsg.content, "<([^/][A-Z0-9._%+-]+@[A-Z0-9.-]+\\.[A-Z]{2,4})>", "$1", RegexOptions.IgnoreCase);
                    mailmsg.content = Regex.Replace(mailmsg.content, "<([/][A-Z0-9._%+-]+@[A-Z0-9.-]+\\.[A-Z]{2,4})>", "", RegexOptions.IgnoreCase);
                }

                return mailmsg;
            }
        }

        /// <summary>
        /// Returns all Memo Details
        /// </summary>
        /// <param name="syswfmmemo"></param>
        /// <returns></returns>
        public MemoDto getMemoDetails(long syswfmmemo)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syswfmmemo", Value = syswfmmemo });
                MemoDto rval = ctx.ExecuteStoreQuery<MemoDto>(QUERYMEMO, parameters.ToArray()).FirstOrDefault();
                if (rval.NOTIZMEMO != null)
                {
                    //remove old clob content after new content
                    if (rval.NOTIZMEMO.IndexOf('\0') > 0)
                    {
                        rval.NOTIZMEMO = rval.NOTIZMEMO.Substring(0, rval.NOTIZMEMO.IndexOf('\0'));
                    }
                    //m.NOTIZMEMO = System.Web.HttpUtility.HtmlEncode(m.NOTIZMEMO);
                    rval.NOTIZMEMO = rval.NOTIZMEMO.Trim();
                }

                
                return rval;
            }
        }

        /// <summary>
        /// Returns all Prun Details
        /// </summary>
        /// <param name="sysptask"></param>
        /// <returns></returns>
        public PrunDto getPrunDetails(long sysprun)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysprun", Value = sysprun });
                PrunDto rval = ctx.ExecuteStoreQuery<PrunDto>(QUERYPRUN, parameters.ToArray()).FirstOrDefault();
                //ggf Mappen wenn über EDMX gelesen wird:
                //rval = Mapper.Map<PERSON, KundeDto>(queryResult, rval);
                return rval;
            }
        }

        /// <summary>
        /// Returns all Prunart Details
        /// </summary>
        /// <param name="sysprunart"></param>
        /// <returns></returns>
        public PrunartDto getPrunartDetails(long sysprunart)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                if(sysprunart==0)
                {
                    return ctx.ExecuteStoreQuery<PrunartDto>(QUERYPRUNARTACTIVE,null).FirstOrDefault();
                }
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysprunart", Value = sysprunart });
                return ctx.ExecuteStoreQuery<PrunartDto>(QUERYPRUNART, parameters.ToArray()).FirstOrDefault();
                
                
            }
        }

        /// <summary>
        /// Returns all Fileatt Details
        /// </summary>
        /// <param name="sysfileatt"></param>
        /// <returns></returns>
        public FileattDto getFileattDetails(long sysfileatt)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysfileatt", Value = sysfileatt });
                FileattDto rval = ctx.ExecuteStoreQuery<FileattDto>(QUERYFILEATT, parameters.ToArray()).FirstOrDefault();
                //ggf Mappen wenn über EDMX gelesen wird:
                //rval = Mapper.Map<PERSON, KundeDto>(queryResult, rval);
                return rval;
            }
        }

        /// <summary>
        /// Returns all Fileatt Details
        /// </summary>
        /// <param name="sysfileatt"></param>
        /// <returns></returns>
        public FileattDto getFileattDetails(string area, long sysid)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = sysid });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "area", Value = area.ToUpper() });
                FileattDto rval = ctx.ExecuteStoreQuery<FileattDto>(QUERYFILEATTENTITY, parameters.ToArray()).FirstOrDefault();
                //ggf Mappen wenn über EDMX gelesen wird:
                //rval = Mapper.Map<PERSON, KundeDto>(queryResult, rval);
                return rval;
            }
        }

        /// <summary>
        /// Returns all Dmsdoc Details
        /// </summary>
        /// <param name="sysfileatt"></param>
        /// <returns></returns>
        public DmsdocDto getDmsdocDetails(long sysdmsdoc)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysdmsdoc", Value = sysdmsdoc });
                DmsdocDto rval = ctx.ExecuteStoreQuery<DmsdocDto>(QUERYDMSDOC, parameters.ToArray()).FirstOrDefault();
                //ggf Mappen wenn über EDMX gelesen wird:
                //rval = Mapper.Map<PERSON, KundeDto>(queryResult, rval);
                return rval;
            }
        }

        /// <summary>
        /// Returns all Dmsdoc Details
        /// </summary>
        /// <param name="sysDmsdoc"></param>
        /// <returns></returns>
        public List<DmsdocDto> getDmsdocDetails(string area, long sysid)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = sysid });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "area", Value = area });
                return ctx.ExecuteStoreQuery<DmsdocDto>(QUERYDMSDOCENTITY, parameters.ToArray()).ToList();

            }
        }


        /// <summary>
        /// Returns all Prproduct Details
        /// </summary>
        /// <param name="sysprproduct"></param>
        /// <returns></returns>
        public PrproductDto getPrproductDetails(long sysprproduct)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysprproduct", Value = sysprproduct });
                PrproductDto rval = ctx.ExecuteStoreQuery<PrproductDto>(QUERYPRPRODUCT, parameters.ToArray()).FirstOrDefault();
                //ggf Mappen wenn über EDMX gelesen wird:
                //rval = Mapper.Map<PERSON, KundeDto>(queryResult, rval);
                return rval;
            }
        }

        /// <summary>
        /// Returns all itemcat Details
        /// </summary>
        /// <param name="sysitemcat"></param>
        /// <returns></returns>
        public ItemcatDto getItemcatDetails(long sysitemcat)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysitemcat", Value = sysitemcat });
                ItemcatDto rval = ctx.ExecuteStoreQuery<ItemcatDto>(QUERYITEMCAT, parameters.ToArray()).FirstOrDefault();
                //ggf Mappen wenn über EDMX gelesen wird:
                //rval = Mapper.Map<PERSON, KundeDto>(queryResult, rval);
                return rval;
            }
        }

        /// <summary>
        /// Returns all ctlang Details
        /// </summary>
        /// <param name="sysctlang"></param>
        /// <returns></returns>
        public CtlangDto getCtlangDetails(long sysctlang)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysctlang", Value = sysctlang });
                CtlangDto rval = ctx.ExecuteStoreQuery<CtlangDto>(QUERYCTLANG, parameters.ToArray()).FirstOrDefault();
                //ggf Mappen wenn über EDMX gelesen wird:
                //rval = Mapper.Map<PERSON, KundeDto>(queryResult, rval);
                return rval;
            }
        }

        /// <summary>
        /// Returns all land Details
        /// </summary>
        /// <param name="sysland"></param>
        /// <returns></returns>
        public LandDto getLandDetails(long sysland)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysland", Value = sysland });
                LandDto rval = ctx.ExecuteStoreQuery<LandDto>(QUERYLAND, parameters.ToArray()).FirstOrDefault();
                //ggf Mappen wenn über EDMX gelesen wird:
                //rval = Mapper.Map<PERSON, KundeDto>(queryResult, rval);
                return rval;
            }
        }

        /// <summary>
        /// Returns all branche Details
        /// </summary>
        /// <param name="sysbranche"></param>
        /// <returns></returns>
        public BrancheDto getBrancheDetails(long sysbranche)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysbranche", Value = sysbranche });
                BrancheDto rval = ctx.ExecuteStoreQuery<BrancheDto>(QUERYBRANCHE, parameters.ToArray()).FirstOrDefault();
                //ggf Mappen wenn über EDMX gelesen wird:
                //rval = Mapper.Map<PERSON, KundeDto>(queryResult, rval);
                return rval;
            }
        }
		
		/// <summary>
		/// Returns all SLA Details
		/// </summary>
		/// <param name="sysid"></param>
		/// <param name="sysctlang"></param>
		/// <returns></returns>
		public List<SlaDto> getSlaDetails (long sysid, string isocode)
		{
			using (DdOlExtended ctx = new DdOlExtended ())
			{
				List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter> ();
				parameters.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = "isocode", Value = isocode });
				parameters.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = sysid });
				List<SlaDto> listSLA = ctx.ExecuteStoreQuery<SlaDto> (QUERYSLA, parameters.ToArray ()).ToList ();
				return listSLA;
				// return ctx.ExecuteStoreQuery<SlaDto> (QUERYSLA, parameters.ToArray ()).ToList ();
			}
		}


        /// <summary>
        /// Returns all Account Details
        /// </summary>
        /// <param name="sysaccount"></param>
        /// <returns></returns>
        public AccountDto getAccountDetails(long sysaccount)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                DbConnection con = (ctx.Connection as EntityConnection).StoreConnection;

                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysaccount", Value = sysaccount });
                long start = (long)(DateTime.Now.TimeOfDay.TotalMilliseconds);
                
                //AccountDto rval = ctx.ExecuteStoreQuery<AccountDto>(QUERYACCOUNT, parameters.ToArray()).FirstOrDefault();
                AccountDto rval = con.Query<AccountDto>(QUERYACCOUNT, new { sysaccount=sysaccount}).FirstOrDefault();
                _log.Debug("Duration A: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));
                if (rval == null) return null;
                rval.sysstaat = con.Query<long>(QUERYSTAAT, new { sysland = rval.sysland, sysstaat=rval.sysstaat }).FirstOrDefault();
                start = (long)(DateTime.Now.TimeOfDay.TotalMilliseconds);

                if (rval.sysadmadd > 0)
                {
                    parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysadmadd", Value = rval.sysadmadd });
                    rval.bomitarbeiter = ctx.ExecuteStoreQuery<String>(QUERYBOMITARBEITER, parameters.ToArray()).FirstOrDefault();
                }
                _log.Debug("Duration B: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));
                start = (long)(DateTime.Now.TimeOfDay.TotalMilliseconds);

                AccountExtDataDto extData = con.Query<AccountExtDataDto>(QUERYKDEXT, new { syskd = sysaccount }).FirstOrDefault();
                rval.zusatzDaten = extData;


                AccountDto kontakt = con.Query<AccountDto>(QUERYKDKONT, new { syskd = sysaccount }).FirstOrDefault();
                if (kontakt != null)
                {
                    rval.panrede = kontakt.panrede;
                    rval.plastname = kontakt.plastname;
                    rval.pfirstname = kontakt.pfirstname;
                    rval.pstrasse = kontakt.pstrasse;
                    rval.pplz = kontakt.pplz;
                    rval.port = kontakt.port;
                    rval.phsnr = kontakt.phsnr;
                    rval.psysland = kontakt.psysland;
                    rval.psysstaat = kontakt.psysstaat;
                }
                _log.Debug("Duration C: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));
                start = (long)(DateTime.Now.TimeOfDay.TotalMilliseconds);

                /*Cic.OpenOne.Common.BO.ICASBo bo = Cic.One.Web.BO.BOFactoryFactory.getInstance().getCASBo();
                if (bo != null)
                {
                    //_OP:XTD... ermittelt die offenen Posten zu der Person (siehe auch die sparsame Funktionsbeschreibung). Dabei wird ein Filter berücksichtigt, der über das folgende SQL-Statement abgerufen werden kann:
                    String filter = ctx.ExecuteStoreQuery<string>("SELECT evalop FROM cicconf where syscicconf=1").FirstOrDefault();

                    Cic.OpenOne.Common.DTO.iCASEvaluateDto ieval = new Cic.OpenOne.Common.DTO.iCASEvaluateDto();
                    ieval.area = "PERSON";
                    ieval.expression = new String[] { "_OP:XTD('PERSON',PERSON:SysPERSON,'"+ filter + "',0)" };
                    ieval.sysID = new long[] { rval.sysperson };
                    Cic.OpenOne.Common.DTO.CASEvaluateResult[] er = null;

                    try
                    {
                        er = bo.getEvaluation(ieval, sysWfuser);
                        if (er[0].clarionResult[0] != null && !"".Equals(er[0].clarionResult[0]))
                            rval.zusatzDaten.opos = double.Parse(er[0].clarionResult[0], CultureInfo.InvariantCulture);
                    }
                    catch (Exception e)
                    {
                        _log.Warn("CAS-Evaluation failed for PERSON opos", e);
                    }
                }*/
                _log.Debug("Duration CAS: " + (DateTime.Now.TimeOfDay.TotalMilliseconds - start));
                start = (long)(DateTime.Now.TimeOfDay.TotalMilliseconds);

                //ggf Mappen wenn über EDMX gelesen wird:
                //rval = Mapper.Map<PERSON, KundeDto>(queryResult, rval);
                return rval;
            }
        }

        /// <summary>
        /// Returns all finance-Data
        /// </summary>
        /// <param name="syskd"></param>
        /// <param name="area"></param>
        /// <param name="sysid"></param>
        /// <returns></returns>
        public ogetFinanzDatenDto getFinanzdatenByArea(long syskd, string area, long sysid)
        {
			String query = "";
            ogetFinanzDatenDto finanzDaten = new ogetFinanzDatenDto();
			// rh 20170630: PRB: syskd != 0, obwohl syskd NUR IM ELSE-Zweig abgefragt wird:
			// ORIG: if (syskd != 0 && area != null && sysid != 0)
			// VLLT sollte es heissen: if (syskd == 0 && area != null && sysid != 0)
			// Abgehärteter Zugriff:
			if (area != null && sysid != 0)
			{ 
                if (area == "PERSON")
                    query = "select budget1,saldo from Kremo where sysperson = " + sysid + " order by syskremo desc";
                if (area == "ANGEBOT")
                    query = "select budget1,saldo from Kremo where sysangebot= " + sysid + " order by syskremo desc";
                if (area == "ANTRAG")
                    query = "select budget1,saldo from Kremo where sysantrag = " + sysid + " order by syskremo desc";
                if (area == "VT")
                    query = "select budget1,saldo from Kremo,vt where kremo.sysantrag=vt.sysantrag and vt.sysid=" + sysid + " order by syskremo desc";
            }
            else
            {
				if (syskd == 0)
				{
					_log.Warn ("WARNING! QUERY budget1, saldo from Kremo with syskd=0 AND area OR sysid NOT SET?!? QUERY will return 0-BUDGET!");
					if (sysid != 0)
					{
						_log.Warn ("Second TRY: SET sysid => syskd:  QUERY budget1, saldo from Kremo with sysid=:sysid");
						syskd = sysid;
					}
				}
				query = "select budget1,saldo from Kremo where sysperson = " + syskd + " order by syskremo desc";
            }

			using (DdOlExtended ctx = new DdOlExtended ())
            {
				DbConnection con = (ctx.Connection as EntityConnection).StoreConnection;
				finanzDaten = con.Query<ogetFinanzDatenDto> (query, null).FirstOrDefault ();
            }
            
            return finanzDaten;

        }

        /// <summary>
        /// delivers the pkz or ukz for the it or person 
        /// optionally for the subarea like angebot/antrag and its id
        /// </summary>
        /// <param name="area"></param>
        /// <param name="sysid"></param>
        /// <param name="subarea"></param>
        /// <param name="subsysid"></param>
        /// <returns></returns>
        public ogetZusatzdaten getZusatzdatenDetail(String area, long sysid, String subarea, long subsysid)
        {
            if (sysid==0||area==null)
                return null;

            ogetZusatzdaten rval = new ogetZusatzdaten();

            using (DdCtExtended ctx = new DdCtExtended())
            {
                DbConnection con = (ctx.Connection as EntityConnection).StoreConnection;
                if("PERSON".Equals(area))//newest pkz/ukz for person
                {
                    int kdtyptyp = ctx.ExecuteStoreQuery<int>("select typ from kdtyp,person where person.syskdtyp=kdtyp.syskdtyp and person.sysperson=" + sysid, null).FirstOrDefault();
                    if (kdtyptyp == 1)
                    {
                        
                        PkzDto pkz = null;
                        if (subsysid > 0 && "ANTRAG".Equals(subarea))//if given, use the one on antrag
                            pkz = con.Query<PkzDto>(QUERY_PERSONPKZACTIVEANTRAG, new { sysperson = sysid,sysantrag=subsysid }).FirstOrDefault();
                        if(pkz==null)
                            pkz = con.Query<PkzDto>(QUERY_PERSONPKZACTIVE, new { sysperson = sysid }).FirstOrDefault();
                        if (pkz == null)
                            pkz = con.Query<PkzDto>(QUERY_PERSONPKZACTIVELAST, new { sysperson = sysid }).FirstOrDefault();
                        if (pkz != null)
                        {
                            rval.pkz = pkz; 
                        }
                    }
                    if (kdtyptyp == 3)
                    {
                        UkzDto ukz = null;
                        if (subsysid > 0 && "ANTRAG".Equals(subarea))
                            ukz = con.Query<UkzDto>(QUERY_PERSONUKZACTIVEANTRAG, new { sysperson = sysid, sysantrag = subsysid }).FirstOrDefault();
                        if (ukz == null)
                            ukz = con.Query<UkzDto>(QUERY_PERSONUKZACTIVE, new { sysperson = sysid }).FirstOrDefault();
                        if (ukz == null)
                            ukz = con.Query<UkzDto>(QUERY_PERSONUKZACTIVELAST, new { sysperson = sysid }).FirstOrDefault();
                        if (ukz != null)
                        {
                            rval.ukz = ukz;
                        }
                    }
                }
                else
                {
                    int kdtyptyp = ctx.ExecuteStoreQuery<int>("select typ from kdtyp,it where it.syskdtyp=kdtyp.syskdtyp and it.sysit=" + sysid, null).FirstOrDefault();

                    if (kdtyptyp == 1)
                    {
                        PkzDto itpkz = null;
                        if (subsysid > 0 && "ANGEBOT".Equals(subarea))
                        {
                            itpkz = con.Query<PkzDto>("select itpkz.*,kredrate kredtrate,anzahlvollstr anzvollstr,itpkz.sysitpkz syspkz from itpkz where sysit=:sysid and sysangebot>0 and sysangebot=:sysangebot order by sysitpkz desc", new { sysid = sysid, sysangebot = subsysid }).FirstOrDefault();
                        }
                        if (subsysid > 0 && "ANTRAG".Equals(subarea))
                        {
                            itpkz = con.Query<PkzDto>("select itpkz.*,kredrate kredtrate,anzahlvollstr anzvollstr,itpkz.sysitpkz syspkz from itpkz where sysit=:sysid  and sysantrag>0 and sysantrag=:sysantrag order by sysitpkz desc", new { sysid = sysid, sysantrag = subsysid }).FirstOrDefault();
                        }
                        if (itpkz == null)//kein pkz für gegebenes angebot
                        {
                            itpkz = con.Query<PkzDto>("select itpkz.*,kredrate kredtrate,anzahlvollstr anzvollstr,itpkz.sysitpkz syspkz from itpkz where sysit=:sysid and sysangebot>0 order by sysitpkz desc", new { sysid = sysid }).FirstOrDefault();
                        }
                        //jüngster satz ohne angebot
                        if (itpkz == null)
                        {
                            itpkz = con.Query<PkzDto>("select itpkz.*,kredrate kredtrate,anzahlvollstr anzvollstr,itpkz.sysitpkz syspkz from itpkz where sysit=:sysid order by sysitpkz desc", new { sysid = sysid }).FirstOrDefault();
                        }
                        rval.pkz = itpkz;
                    }
                    if (kdtyptyp == 3)
                    {
                        UkzDto itukz = null;
                        if (subsysid > 0 && "ANGEBOT".Equals(subarea))
                        {
                            itukz = con.Query<UkzDto>("select itukz.*,obligoeigen obliboeigen, itukz.sysitukz sysukz from itukz where sysit=:sysid and sysangebot>0 and sysangebot=:sysangebot order by sysitukz desc", new { sysid = sysid,sysangebot=subsysid }).FirstOrDefault();
                        }
                        if (subsysid > 0 && "ANTRAG".Equals(subarea))
                        {
                            itukz = con.Query<UkzDto>("select itukz.*,obligoeigen obliboeigen, itukz.sysitukz sysukz from itukz where sysit=:sysid and sysantrag>0 and sysantrag=:sysantrag order by sysitukz desc", new { sysid = sysid,sysantrag=subsysid }).FirstOrDefault();
                        }
                        
                        if (itukz == null)
                        {
                            itukz = con.Query<UkzDto>("select itukz.*,obligoeigen obliboeigen, itukz.sysitukz sysukz from itukz where sysit=:sysid and sysangebot>0 order by sysitukz desc", new { sysid = sysid }).FirstOrDefault();
                        }
                        if (itukz == null)
                        {
                            itukz = con.Query<UkzDto>("select itukz.*,obligoeigen obliboeigen, itukz.sysitukz sysukz from itukz where sysit=:sysid order by sysitukz desc", new { sysid = sysid }).FirstOrDefault();
                        }
                        rval.ukz = itukz;
               
                    }

                }
          
             
            }

            return rval;
        }

        /// <summary>
        /// delivers additional customer data for this request
        /// </summary>
        /// <param name="syskd">customer</param>
        /// <param name="sysantrag">request</param>
        /// <returns>additional customer data</returns>
        public ogetZusatzdaten getZusatzdatenDetail(long syskd, long sysantrag)
        {
            if (syskd == 0 || sysantrag == 0)
                return null;

            ogetZusatzdaten rval = new ogetZusatzdaten();

            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syskd", Value = syskd });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysantrag", Value = sysantrag });
                rval.pkz = ctx.ExecuteStoreQuery<PkzDto>("select * from pkz where sysperson=:syskd and sysantrag=:sysantrag", parameters.ToArray()).FirstOrDefault();

                parameters.Clear();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syskd", Value = syskd });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysantrag", Value = sysantrag });
                rval.ukz = ctx.ExecuteStoreQuery<UkzDto>("select * from ukz where sysperson=:syskd and sysantrag=:sysantrag", parameters.ToArray()).FirstOrDefault();
            }

            return rval;
        }


        /// <summary>
        /// delivers additional customer data for this request
        /// </summary>
        /// <param name="syskd">customer</param>
        /// <param name="sysantrag">request</param>
        /// <returns>additional customer data</returns>
        public ogetZusatzdaten getZusatzdatenDetailByAngebot(long sysit, long sysangebot)
        {

            if (sysit== 0 || sysangebot == 0)
                return null;

            ogetZusatzdaten rval = new ogetZusatzdaten();
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = sysit });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysangebot", Value = sysangebot });
                rval.pkz = ctx.ExecuteStoreQuery<PkzDto>("select * from itpkz where sysit=:sysit and sysangebot=:sysangebot", parameters.ToArray()).FirstOrDefault();

                parameters.Clear();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = sysit });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysangebot", Value = sysangebot });
                rval.ukz  = ctx.ExecuteStoreQuery<UkzDto>("select * from itukz where sysit=:sysit and sysangebot=:sysangebot", parameters.ToArray()).FirstOrDefault();

            }
            return rval;
        }
        /// <summary>
        /// Returns all WktAccount Details
        /// </summary>
        /// <param name="syswktaccount"></param>
        /// <returns></returns>
        public WktaccountDto getWktAccountDetails(long syswktaccount)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();

                WktaccountDto rval = null;
                if (syswktaccount < 0)
                {
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysaccount", Value = -1 * syswktaccount });
                    rval = ctx.ExecuteStoreQuery<WktaccountDto>(QUERYWKTACCOUNTIT, parameters.ToArray()).FirstOrDefault();

                    parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = -1 * syswktaccount });
                    WktaccountDto tmpAccount = ctx.ExecuteStoreQuery<WktaccountDto>(QUERYWKTITOPTS, parameters.ToArray()).FirstOrDefault();
                    if (tmpAccount != null && rval != null)
                    {
                        rval.dauerrechnung = tmpAccount.dauerrechnung;
                        rval.monatsrechnungart = tmpAccount.monatsrechnungart;
                        if (tmpAccount.dauerrechnung > 0)
                        {
                            rval.monatsrechnung = 0;
                        }
                        else
                        {
                            rval.monatsrechnung = 1;
                        }
                    }
                }
                else
                {
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysaccount", Value = syswktaccount });
                    rval = ctx.ExecuteStoreQuery<WktaccountDto>(QUERYWKTACCOUNTPERSON, parameters.ToArray()).FirstOrDefault();

                    parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysaccount", Value = syswktaccount });
                    WktaccountDto tmpAccount = ctx.ExecuteStoreQuery<WktaccountDto>(QUERYWKTACCOUNTOPTS, parameters.ToArray()).FirstOrDefault();
                    if (tmpAccount != null && rval != null)
                    {
                        rval.dauerrechnung = tmpAccount.dauerrechnung;
                        rval.monatsrechnungart = tmpAccount.monatsrechnungart;

                        rval.monatsrechnung = 0;
                        if (rval.dauerrechnung == 1)//1=Dauerrechnung
                        {
                            rval.monatsrechnung = 0;
                        }
                        else if (rval.dauerrechnung == 2)//2=Monatsrechnung
                        {
                            rval.monatsrechnung = 1;
                            rval.dauerrechnung = 0;
                        }
                    }
                }
                if (rval != null)
                {
                    rval = getWktPartnerDetails(rval);
                }
                return rval;
            }
        }

        /// <summary>
        /// Returns all PreadFlag Details
		/// from entry with sysid (ATTENTION! NOT syspread here!)
        /// </summary>
        /// <param name="sysid"></param>
        /// <returns></returns>
        public PreadDto getPreadFlagDetails (long sysid)
        {
			PreadDto rval = null;

            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();

				parameters.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = sysid });
                rval = ctx.ExecuteStoreQuery<PreadDto>(QUERYPREADFLAG, parameters.ToArray()).FirstOrDefault();
			}
			return rval;
        }



        /// <summary>
        /// Returns all Account Details
        /// </summary>
        /// <param name="sysaccount"></param>
        /// <returns></returns>
        public PartnerDto getPartnerDetails(long syspartner)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();

                PartnerDto rval = null;
                if (syspartner > 0)
                {
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syspartner", Value = syspartner });
                    rval = ctx.ExecuteStoreQuery<PartnerDto>(QUERYPARTNER, parameters.ToArray()).FirstOrDefault();
                }
                else
                {
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syspartner", Value = syspartner * -1 });
                    rval = ctx.ExecuteStoreQuery<PartnerDto>(QUERYPARTNER2, parameters.ToArray()).FirstOrDefault();
                }

                //ggf Mappen wenn über EDMX gelesen wird:
                //rval = Mapper.Map<PERSON, KundeDto>(queryResult, rval);
                return rval;
            }
        }

        /// <summary>
        /// Fills the wktaccount with partner info
        /// </summary>
        /// <param name="sysaccount"></param>
        /// <returns></returns>
        public WktaccountDto getWktPartnerDetails(WktaccountDto wktAccount)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();

                PARTNER partner = null;
                if (wktAccount.sysit > 0)
                {
                    List<Devart.Data.Oracle.OracleParameter> pars = new List<Devart.Data.Oracle.OracleParameter>();
                    pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = wktAccount.sysit });
                    partner = ctx.ExecuteStoreQuery<PARTNER>("select * from partner where sysit=:sysit", pars.ToArray()).FirstOrDefault();
                }
                else if (wktAccount.sysperson > 0)
                {
                    List<Devart.Data.Oracle.OracleParameter> pars = new List<Devart.Data.Oracle.OracleParameter>();
                    pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperson", Value = wktAccount.sysperson });
                    partner = ctx.ExecuteStoreQuery<PARTNER>("select * from partner where sysperson=:sysperson", pars.ToArray()).FirstOrDefault();
                }

                if (partner == null) return wktAccount;

                wktAccount.plastname = partner.LASTNAME;
                wktAccount.pfirstname = partner.FIRSTNAME;
                wktAccount.panrede = partner.ANREDE;
                wktAccount.ptitel = partner.TITEL;
                wktAccount.ptelefon = partner.TELEFON;
                wktAccount.pmobil = partner.MOBIL;
                wktAccount.pemail = partner.EMAIL;
                wktAccount.pfax = partner.FAX;

                return wktAccount;
                //todo automapper 2
                //return Mapper.Map<PARTNER, WktaccountDto>(p,wktaccount);

            }
        }

        /// <summary>
        /// Returns all Beteiligter Details
        /// </summary>
        /// <param name="sysaccount"></param>
        /// <returns></returns>
        public BeteiligterDto getBeteiligterDetails(long syspartner)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();

                BeteiligterDto rval = null;
                if (syspartner > 0)
                {
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syscrmnm", Value = syspartner });
                    rval = ctx.ExecuteStoreQuery<BeteiligterDto>(QUERYBETEILIGTER, parameters.ToArray()).FirstOrDefault();
                }
                else
                {
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperson", Value = syspartner * -1 });
                    rval = ctx.ExecuteStoreQuery<BeteiligterDto>(QUERYBETEILIGTER2, parameters.ToArray()).FirstOrDefault();
                }

                //ggf Mappen wenn über EDMX gelesen wird:
                //rval = Mapper.Map<PERSON, KundeDto>(queryResult, rval);
                return rval;
            }
        }



        /// <summary>
        /// Returns all Adrtp Details
        /// </summary>
        /// <param name="sysadrtp"></param>
        /// <returns></returns>
        public AdrtpDto getAdrtpDetails(long sysadrtp)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysadrtp", Value = sysadrtp });
                AdrtpDto rval = ctx.ExecuteStoreQuery<AdrtpDto>(QUERYADRTP, parameters.ToArray()).FirstOrDefault();
                //ggf Mappen wenn über EDMX gelesen wird:
                //rval = Mapper.Map<PERSON, KundeDto>(queryResult, rval);
                return rval;
            }
        }


        /// <summary>
        /// Returns all Adrtp Details
        /// </summary>
        /// <param name="sysadrtp"></param>
        /// <returns></returns>
        public StrasseDto getStrasseDetails(long sysstrasse)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysstrasse", Value = sysstrasse });
                StrasseDto rval = ctx.ExecuteStoreQuery<StrasseDto>(QUERYSTRASSE, parameters.ToArray()).FirstOrDefault();
                //ggf Mappen wenn über EDMX gelesen wird:
                //rval = Mapper.Map<PERSON, KundeDto>(queryResult, rval);
                return rval;
            }
        }


		// rh 20161220: 
		public void ePosChange ()
		{
			Cic.OpenOne.CarConfigurator.DAO.EaiparDao eaiParDao = new Cic.OpenOne.CarConfigurator.DAO.EaiparDao ();
			// public const 
			string RW_VERL_VERFUEGBAR_WEB_DEFAULT = "SELECT 1 FROM dual, vt WHERE vt.attribut !='saldiert' AND vt.rw>7000 AND vt.rlz<6 AND vt.sysvart=1";
			String newQueryString = eaiParDao.getEaiParFileByCode ("RW_VERL_VERFUEGBAR_WEB", RW_VERL_VERFUEGBAR_WEB_DEFAULT);

		}


        /// <summary>
        /// Returns PUser Details
        /// </summary>
        /// <param name="syswfuser"></param>
        /// <returns></returns>
        public PuserDto getPuserDetails(long syswfuser)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                CredentialContext cctx = new CredentialContext();
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syswfuser", Value = syswfuser });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "isocode", Value = cctx.getMembershipInfo().ISOLanguageCode });
                PuserDto rval = ctx.ExecuteStoreQuery<PuserDto>(QUERYPUSER, parameters.ToArray()).FirstOrDefault();

				//—————————————————————————————————————————————
				// GET pendingChanges (rh: 20161221)
				//—————————————————————————————————————————————
				// GET perole herausfinden // tester = 1911;
				//select sysperole from perole,wfuser where wfuser.sysperson=perole.sysperson and wfuser.syswfuser=:syswfuser;
				List<Devart.Data.Oracle.OracleParameter> parX = new List<Devart.Data.Oracle.OracleParameter> ();
				parX.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = "syswfuser", Value = syswfuser });
				long sysperole = ctx.ExecuteStoreQuery<long> ("select sysperole from perole,wfuser where wfuser.sysperson=perole.sysperson and wfuser.syswfuser=:syswfuser", parX.ToArray ()).FirstOrDefault ();

				String query = ctx.ExecuteStoreQuery<String> ("select paramfile query from EAIPAR where code='NE_PEROLE_QUERY'", null).FirstOrDefault ();

				////———————————————————————————————————————————————
				//// ALTER query due to missing field names Update
				//// ToDo rh 20170111: REMOVE WHEN Query is UPDATED  
				////———————————————————————————————————————————————
				//query = @"select * from 
				//	( 
				//	SELECT FIELDS.DBFIELD, DECODE(NVL(CHANGES.SYSDD,0),0, 'NO_CHANGES',CHANGES.NEWVALUE) NEW_VALUE 
				//	FROM   
				//	/*Liste der schwebenden Änderungen*/ 
				//	...					
				//	'AHVCODE' as AHVPflicht, 'INACTIVEFLAG' as Inaktiveflag, 'APPROVAL' as Freigabecode ) )";

//				//———————————————————————————————————————————————
//				// NEW query WITH adminflag and vtflag fields 
//				// rh/Stas 20170116: REMOVE WHEN Query is in DB  
//				// rh 2010117: sysctlangkorr --> sysctlang
//				// rh 2010302: anrede --> anredecode
//				//———————————————————————————————————————————————
//				query = @"SELECT * FROM 
//				 		(SELECT FIELDS.DBFIELD, DECODE(NVL(CHANGES.SYSDD,0),0, 'NO_CHANGES',CHANGES.NEWVALUE) NEW_VALUE 
//				FROM 
//				/*Liste der schwebenden Änderungen*/ 
//				(SELECT dd.sysdd, DD.DBFIELD, NEPROPVAL.NEWVALUE 
//				/*Debug-Felder: --NEPROCDD.BEZEICHNUNG,, NEPROPVAL.OLDVALUE, NEPROCDD.SYSNEPROC, DD.FIELD, DD.DBTABLE, NEPROP.AREA, NEPROP.SYSID*/ 
//				, ROW_NUMBER () OVER (PARTITION BY FIELD ORDER BY FIELD, NEPROP.SYSNEPROP desc) seq_no 
//				FROM NEPROCDD, DD, NEPROP, NEPROPVAL 
//				WHERE NEPROCDD.SYSDD = DD.SYSDD(+) 
//				AND NEPROP.SYSNEPROC = NEPROCDD.SYSNEPROC 
//				AND NEPROP.SYSNEPROP = NEPROPVAL.SYSNEPROP 
//				AND NEPROPVAL.SYSDD = NEPROCDD.SYSDD 
//				AND neprop.processedflag = 0 
//				AND NEPROP.AREA = 'PEROLE' 
//				AND NEPROP.SYSID = :SYSPEROLE 
//				UNION ALL
//				/*NEDELETE Position RGR*/
//				SELECT decode(rgr.name,'EPOS_ADMIN',1000000001,'EPOS_VERTRAGSABSCHLUSS',1000000002) SYSDD
//				, decode(rgr.name,'EPOS_ADMIN','ADMINFLAG','EPOS_VERTRAGSABSCHLUSS','VTFLAG') DBFIELD 
//				, NVL2(NEPROPVAL.OLDVALUE,'1','0') NEWVALUE
//				, ROW_NUMBER () OVER (PARTITION BY rgr.name ORDER BY rgr.name, NEPROP.SYSNEPROP desc) seq_no 
//				FROM  RGM,RGR, NEPROP, NEPROPVAL 
//				WHERE NEPROP.SYSNEPROP = NEPROPVAL.SYSNEPROP 
//				AND rgm.sysrgm = NEPROPVAL.NEWVALUE
//				AND rgr.sysrgr = rgm.sysrgr
//				AND NEPROPVAL.AREA = 'RGM'
//				AND neprop.processedflag = 0 
//				AND NEPROP.AREA = 'PEROLE' 
//				AND NEPROP.SYSID = :SYSPEROLE 
//				ORDER BY 1,4 DESC
//				) CHANGES 
//				/*Liste der Felder für die schwebenden Änderungen*/ 
//				, (SELECT DD.DBFIELD, DD.SYSDD 
//				FROM NEPROCDD, DD 
//				WHERE NEPROCDD.SYSDD = DD.SYSDD 
//				union all
//				select 'ADMINFLAG' DBFIELD,1000000001 SYSDD from dual union all
//				select 'VTFLAG' DBFIELD,1000000002  SYSDD from dual
//				ORDER BY 2 DESC 
//				) FIELDS 
//				WHERE CHANGES.SYSDD = FIELDS.SYSDD 
//				/*Es werden nur die letzten schwebenden Änderungen beachtet*/ 
//				AND CHANGES.SEQ_NO  =1 
//				) 
//				PIVOT (max(NEW_VALUE) 
//				FOR DBFIELD IN 
//				('SYSROLEATTRIB' as Rolle, 'SYSRGR' as sysperole, 'VORNAME' as vorname, 'TELEFON' as telefon, 'SYSLANDNAT' as syslandnat, 
//				'STRASSE' as strasse, 'QUELLSTEUERPFLICHT' as steuerflag, 'PLZ' as plz, 'ORT' as ort, 'NAME' as name, 'HSNR' as hsnr, 
//				'GEBDATUM' as gebdatum, 'FAX' as fax, 'AHVNUMMER' as ahv, 'VALIDUNTIL' as validuntil, 'INAKTIVGRUND' as inaktivgrund, 
//				'OPTION7' as mobile, 'OPTION3' as email, 'SYSSTAAT' as sysstaat, 'SYSCTLANG' as sysctlang, 'SYSLAND' as sysland, 
//				'ANREDECODE' as anredecode, 'SYSBLZ' as sysblz, 'KONTOINHABER' as kontoinhaber, 'IBAN' as iban, 
//				'AHVCODE' as AHVPflicht, 'INACTIVEFLAG' as Inaktiveflag, 'APPROVAL' as Freigabecode, 'ADMINFLAG' as adminflag, 'VTFLAG' as vtflag ) )
//				";

				//—————————————————————————————————————————————— 
				// ALT: 'OPTION7' as mobile, 'OPTION3' as email, 'SYSSTAAT' as sysstaat, 'SYSCTLANG' as sysctlangkorr, 'SYSLAND' as sysland, 
				//—————————————————————————————————————————————— 
				
				List<Devart.Data.Oracle.OracleParameter> pars = new List<Devart.Data.Oracle.OracleParameter> ();
				pars.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = "SYSPEROLE", Value = sysperole });
				rval.pendingChanges = ctx.ExecuteStoreQuery<PuserPendingChangesDto> (query, pars.ToArray ()).FirstOrDefault ();



                if (rval.syswfuser > 0 && rval.sysperole > 0)//Anzahl anderer Admins
                {
                    pars = new List<Devart.Data.Oracle.OracleParameter>();
                    pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "akt_SysPerole", Value = rval.sysperole });
                    pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "NewValidBis", Value = DateTime.Now.AddDays(1) });
                    rval.othersAdminCount = ctx.ExecuteStoreQuery<long>("SELECT COUNT(rgm.sysrgm) FROM rgm, rgr, wfuser, perole ma WHERE rgr.name ='EPOS_ADMIN' AND rgr.sysrgr = rgm.sysrgr AND rgm.syswfuser = wfuser.syswfuser AND wfuser.sysperson = ma.sysperson AND ma.sysperole IN (SELECT perole.sysperole FROM perole, perole akt WHERE (perole.validuntil = TO_DATE ('01.01.0111','dd.mm.yyyy') OR perole.validuntil >= :NewValidBis  OR perole.validuntil IS NULL) AND (perole.validfrom = TO_DATE ('01.01.0111','dd.mm.yyyy') OR perole.validfrom <= :NewValidBis  OR perole.validfrom IS NULL) AND perole.sysperson > 0 AND perole.INACTIVEFLAG = 0 AND perole.sysperole <> akt.sysperole AND perole.sysparent = akt.sysparent AND akt.sysperole = :akt_SysPerole)", pars.ToArray()).FirstOrDefault();
                    
                }
                return rval;
            }
        }
		
        /// <summary>
        /// Returns all Kontotp Details
        /// </summary>
        /// <param name="sysKontotp"></param>
        /// <returns></returns>
        public KontotpDto getKontotpDetails(long syskontotp)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syskontotp", Value = syskontotp });
                KontotpDto rval = ctx.ExecuteStoreQuery<KontotpDto>(QUERYKONTOTP, parameters.ToArray()).FirstOrDefault();
                //ggf Mappen wenn über EDMX gelesen wird:
                //rval = Mapper.Map<PERSON, KundeDto>(queryResult, rval);
                return rval;
            }
        }

        /// <summary>
        /// Returns all Blz Details
        /// </summary>
        /// <param name="sysBlz"></param>
        /// <returns></returns>
        public BlzDto getBlzDetails(long sysblz)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysblz", Value = sysblz });
                BlzDto rval = ctx.ExecuteStoreQuery<BlzDto>(QUERYBLZ, parameters.ToArray()).FirstOrDefault();
                //ggf Mappen wenn über EDMX gelesen wird:
                //rval = Mapper.Map<PERSON, KundeDto>(queryResult, rval);
                return rval;
            }
        }

        /// <summary>
        /// Returns all Wfexec Details
        /// </summary>
        /// <param name="syswfexec"></param>
        /// <returns></returns>
        public WfexecDto getWfexecDetails(long syswfexec)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syswfexec", Value = syswfexec });
                WfexecDto rval = ctx.ExecuteStoreQuery<WfexecDto>(QUERYWFEXEC, parameters.ToArray()).FirstOrDefault();
                //ggf Mappen wenn über EDMX gelesen wird:
                //rval = Mapper.Map<PERSON, KundeDto>(queryResult, rval);
                return rval;
            }
        }
        /// <summary>
        /// Returns all Ptrelate Details
        /// </summary>
        /// <param name="sysPtrelate"></param>
        /// <returns></returns>
        public PtrelateDto getPtrelateDetails(long sysptrelate)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysptrelate", Value = sysptrelate });
                PtrelateDto rval = ctx.ExecuteStoreQuery<PtrelateDto>(QUERYPTRELATE, parameters.ToArray()).FirstOrDefault();
                //ggf Mappen wenn über EDMX gelesen wird:
                //rval = Mapper.Map<PERSON, KundeDto>(queryResult, rval);
                return rval;
            }
        }

        /// <summary>
        /// Returns all Crmnm Details
        /// </summary>
        /// <param name="sysCrmnm"></param>
        /// <returns></returns>
        public CrmnmDto getCrmnmDetails(long syscrmnm)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syscrmnm", Value = syscrmnm });
                CrmnmDto rval = ctx.ExecuteStoreQuery<CrmnmDto>(QUERYCRMNM, parameters.ToArray()).FirstOrDefault();
                //ggf Mappen wenn über EDMX gelesen wird:
                //rval = Mapper.Map<PERSON, KundeDto>(queryResult, rval);
                return rval;
            }
        }


        /// <summary>
        /// Returns all Ddlkprub Details
        /// </summary>
        /// <param name="sysddlkprub"></param>
        /// <returns></returns>
        public DdlkprubDto getDdlkprubDetails(long sysddlkprub)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysddlkprub", Value = sysddlkprub });
                DdlkprubDto rval = ctx.ExecuteStoreQuery<DdlkprubDto>(QUERYDDLKPRUB, parameters.ToArray()).FirstOrDefault();
                //ggf Mappen wenn über EDMX gelesen wird:
                //rval = Mapper.Map<PERSON, KundeDto>(queryResult, rval);
                return rval;
            }
        }

        /// <summary>
        /// Returns all Ddlkpcol Details
        /// </summary>
        /// <param name="sysPtrelate"></param>
        /// <returns></returns>
        public DdlkpcolDto getDdlkpcolDetails(long sysddlkpcol)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysddlkpcol", Value = sysddlkpcol });
                DdlkpcolDto rval = ctx.ExecuteStoreQuery<DdlkpcolDto>(QUERYDDLKPCOL, parameters.ToArray()).FirstOrDefault();
                //ggf Mappen wenn über EDMX gelesen wird:
                //rval = Mapper.Map<PERSON, KundeDto>(queryResult, rval);
                return rval;
            }
        }

        /// <summary>
        /// Returns all sysddlkppos Details
        /// </summary>
        /// <param name="sysPtrelate"></param>
        /// <returns></returns>
        public DdlkpposDto getDdlkpposDetails(long sysddlkppos)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysddlkppos", Value = sysddlkppos });
                DdlkpposDto rval = ctx.ExecuteStoreQuery<DdlkpposDto>(QUERYDDLKPPOS, parameters.ToArray()).FirstOrDefault();
                //ggf Mappen wenn über EDMX gelesen wird:
                //rval = Mapper.Map<PERSON, KundeDto>(queryResult, rval);
                return rval;
            }
        }

        /// <summary>
        /// Returns all Ddlkpspos Details
        /// </summary>
        /// <param name="sysddlkpspos"></param>
        /// <returns></returns>
        public DdlkpsposDto getDdlkpsposDetails(long sysddlkpspos)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysddlkpspos", Value = sysddlkpspos });
                DdlkpsposDto rval = ctx.ExecuteStoreQuery<DdlkpsposDto>(QUERYDDLKPSPOS, parameters.ToArray()).FirstOrDefault();
                //ggf Mappen wenn über EDMX gelesen wird:
                //rval = Mapper.Map<PERSON, KundeDto>(queryResult, rval);
                return rval;
            }
        }
        
        /// <summary>
        /// Returns the checklist data for the antrag
        /// </summary>
        /// <param name="sysId"></param>
        /// <returns></returns>
        public ChklistDto getChklistDetails(igetChecklistDetailDto input)
        {
           
            List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = input.sysid });
            using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended pe = new Cic.OpenOne.Common.Model.Prisma.PrismaExtended())
            {
                ChklistDto rval = new ChklistDto();
                rval.sysid = input.sysid;
                ChklistDateDto dates = pe.ExecuteStoreQuery<ChklistDateDto>("select edatum receiveDate, udatum vtDate,antoption.ulon05 vtclatime,str07 art from antrag,antoption where antoption.sysid=antrag.sysid and antrag.sysid=" + input.sysid, null).FirstOrDefault();
                rval.receiveDate = dates.receiveDate;
                rval.art = dates.art;
                rval.vtDate = DateTimeHelper.CreateDate(dates.vtDate, dates.vtclatime);
                String query = QUERY_SALES;
                if (input.salesFlag == 0)
                    query = QUERY_PAYMENTS;
                rval.rows = pe.ExecuteStoreQuery<ChklistEntryDto>(query, parameters.ToArray()).ToList();
                 if (input.salesFlag == 0)
                 {
                     if(rval.art!=null && rval.art!="3")
                     {
                         //filter rval.art
                         List<ChklistEntryDto> nrval = new List<ChklistEntryDto>();
                         long lastafl = 0;
                         String lastpp = "";
                         foreach(ChklistEntryDto et in rval.rows)
                         {
                             if (et.psart == null)
                             {
                                 nrval.Add(et);
                                 lastafl = 0;
                                 continue;
                             }
                             if(et.psart.Equals(rval.art))
                             {
                                 nrval.Add(et);
                                 lastafl = 0;
                                 continue;
                             }
                             if(lastafl>0 && et.sysratingauflage==lastafl && lastpp!=null && !lastpp.Equals(et.pp)) continue;
                             et.code = null;
                             et.description = null;
                             nrval.Add(et);
                             lastafl = et.sysratingauflage;
                             lastpp = et.pp;


                         }
                         rval.rows = nrval;
                     }
                 }

                long enabled = pe.ExecuteStoreQuery<long>("select count(*) from antrag where FLAGFREIGABEAUSZ=1  and sysid=" + input.sysid, null).FirstOrDefault();
                if (enabled > 0)
                {
                    rval.geldfluss = 1;
                }
                enabled = pe.ExecuteStoreQuery<long>("select count(*) from antrag where FLAGFREIGABEFORM=1 and sysid=" + input.sysid, null).FirstOrDefault();
                if (enabled > 0)
                {
                    rval.schlusskontrolle = 1;
                }
                rval.pruefung = 0;
                enabled = pe.ExecuteStoreQuery<long>("select count(*) from antrag where FLAGSK=1 and sysid=" + input.sysid, null).FirstOrDefault();
                if (enabled > 0)
                {
                    rval.pruefung = 1;
                }

                else
                {
                    long disabled = pe.ExecuteStoreQuery<long>("select count(*) from antrag where FLAGFREIGABEFORM=0 and FLAGFREIGABEAUSZ=0  and sysid=" + input.sysid, null).FirstOrDefault();
                    if (disabled > 0)
                    {
                        rval.geldfluss = 0;
                        rval.schlusskontrolle = 0;
                    }
                }

                foreach (ChklistEntryDto entry in rval.rows)
                {
                    if (entry.flagnokstep.HasValue && entry.flagnokstep.Value == 0)
                        entry.flagnokstep = null;
                }

                return rval;
            }
            
        }

        /// <summary>
        /// Returns all Camptp Details
        /// </summary>
        /// <param name="syscamptp"></param>
        /// <returns></returns>
        public CamptpDto getCamptpDetails(long syscamptp)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syscamptp", Value = syscamptp });
                CamptpDto rval = ctx.ExecuteStoreQuery<CamptpDto>(QUERYCAMPTP, parameters.ToArray()).FirstOrDefault();
                //ggf Mappen wenn über EDMX gelesen wird:
				//rval = Mapper.Map<PERSON, CamptpDto>(queryResult, rval);
                return rval;
            }
        }

        /// <summary>
        /// Returns all zinstab Details
        /// </summary>
        /// <param name="syszinstab"></param>
        /// <returns></returns>
        public ZinstabDto getZinstabDetails(long syszinstab)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syszinstab", Value = syszinstab });
                ZinstabDto rval = ctx.ExecuteStoreQuery<ZinstabDto>(QUERYZINSTAB, parameters.ToArray()).FirstOrDefault();
                return rval;
            }
        }

        /// <summary>
        /// Returns all Oppotp Details
        /// </summary>
        /// <param name="sysoppotp"></param>
        /// <returns></returns>
        public OppotpDto getOppotpDetails(long sysoppotp)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysoppotp", Value = sysoppotp });
                OppotpDto rval = ctx.ExecuteStoreQuery<OppotpDto>(QUERYOPPOTP, parameters.ToArray()).FirstOrDefault();
                //ggf Mappen wenn über EDMX gelesen wird:
                //rval = Mapper.Map<PERSON, KundeDto>(queryResult, rval);
                return rval;
            }
        }

        /// <summary>
        /// Returns all Crmpr Details
        /// </summary>
        /// <param name="syscrmpr"></param>
        /// <returns></returns>
        public CrmprDto getCrmprDetails(long syscrmpr)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syscrmpr", Value = syscrmpr });
                CrmprDto rval = ctx.ExecuteStoreQuery<CrmprDto>(QUERYCRMPR, parameters.ToArray()).FirstOrDefault();
                //ggf Mappen wenn über EDMX gelesen wird:
                //rval = Mapper.Map<PERSON, KundeDto>(queryResult, rval);
                return rval;
            }
        }

        /// <summary>
        /// Returns all contacttp Details
        /// </summary>
        /// <param name="syssyscontacttp"></param>
        /// <returns></returns>
        public ContacttpDto getContacttpDetails(long syscontacttp)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syscontacttp", Value = syscontacttp });
                ContacttpDto rval = ctx.ExecuteStoreQuery<ContacttpDto>(QUERYCONTACTTP, parameters.ToArray()).FirstOrDefault();
                //ggf Mappen wenn über EDMX gelesen wird:
                //rval = Mapper.Map<PERSON, KundeDto>(queryResult, rval);
                return rval;
            }
        }

        /// <summary>
        /// Returns all Itemcatm Details
        /// </summary>
        /// <param name="sysitemcatm"></param>
        /// <returns></returns>
        public ItemcatmDto getItemcatmDetails(long sysitemcatm)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysitemcatm", Value = sysitemcatm });
                ItemcatmDto rval = ctx.ExecuteStoreQuery<ItemcatmDto>(QUERYITEMCATM, parameters.ToArray()).FirstOrDefault();
                //ggf Mappen wenn über EDMX gelesen wird:
                //rval = Mapper.Map<PERSON, KundeDto>(queryResult, rval);
                return rval;
            }
        }


        /// <summary>
        /// Returns all Recurr Details
        /// </summary>
        /// <param name="sysRecurr"></param>
        /// <returns></returns>
        public RecurrDto getRecurrDetails(long sysRecurr)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysrecurr", Value = sysRecurr });
                RecurrDto rval = ctx.ExecuteStoreQuery<RecurrDto>(QUERYRECURR, parameters.ToArray()).FirstOrDefault();
                //ggf Mappen wenn über EDMX gelesen wird:
                //rval = Mapper.Map<PERSON, KundeDto>(queryResult, rval);
                return rval;
            }
        }

        /// <summary>
        /// Returns all Ptype Details
        /// </summary>
        /// <param name="sysPtype"></param>
        /// <returns></returns>
        public PtypeDto getPtypeDetails(long sysptype)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysptype", Value = sysptype });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "isocode", Value = this.isoCode});
                PtypeDto rval = ctx.ExecuteStoreQuery<PtypeDto>(QUERYPTYPE, parameters.ToArray()).FirstOrDefault();

                parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysptype", Value = sysptype });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syswfuser", Value = this.sysWfuser });
                rval.sysptask = ctx.ExecuteStoreQuery<long>(QUERYPTYPEPTASK, parameters.ToArray()).FirstOrDefault();
                List<Devart.Data.Oracle.OracleParameter> parameters2 = new List<Devart.Data.Oracle.OracleParameter>();
                parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysptype", Value = sysptype });
                List<String> checkers = ctx.ExecuteStoreQuery<String>(QUERYPCHECKER, parameters2.ToArray()).ToList();
                if (checkers != null && checkers.Count > 0)
                    rval.plist = String.Join("/ ", checkers);
                else rval.plist = "";

                if(rval.art==20)//nur Sammelaufgaben zeigen die geprüften Mitarbeiter an
                { 
                    parameters2 = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysptype", Value = sysptype });
                    parameters2.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syswfuser", Value = this.sysWfuser });
                    rval.subjects = ctx.ExecuteStoreQuery<PsubjectDto>(QUERYPSUBJECTSLIST, parameters2.ToArray()).ToList();
                }
                return rval;
            }
        }

        /// <summary>
        /// Returns all Prunstep Details
        /// </summary>
        /// <param name="sysprunstep"></param>
        /// <returns></returns>
        public PrunstepDto getPrunstepDetails(long sysprunstep)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysprunstep", Value = sysprunstep });
                PrunstepDto rval = ctx.ExecuteStoreQuery<PrunstepDto>(QUERYPRUNSTEP, parameters.ToArray()).FirstOrDefault();
                //ggf Mappen wenn über EDMX gelesen wird:
                //rval = Mapper.Map<PERSON, KundeDto>(queryResult, rval);
                return rval;
            }
        }

        /// <summary>
        /// Returns all Pstep Details
        /// </summary>
        /// <param name="sysPstep"></param>
        /// <returns></returns>
        public PstepDto getPstepDetails(long syspstep)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syspstep", Value = syspstep });
                PstepDto rval = ctx.ExecuteStoreQuery<PstepDto>(QUERYPSTEP, parameters.ToArray()).FirstOrDefault();
                //ggf Mappen wenn über EDMX gelesen wird:
                //rval = Mapper.Map<PERSON, KundeDto>(queryResult, rval);
                return rval;
            }
        }

        /// <summary>
        /// Returns all Prkgroup Details
        /// </summary>
        /// <param name="sysprkgroup"></param>
        /// <returns></returns>
        public PrkgroupDto getPrkgroupDetails(long sysprkgroup)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysprkgroup", Value = sysprkgroup });
                PrkgroupDto rval = ctx.ExecuteStoreQuery<PrkgroupDto>(QUERYPRKGROUP, parameters.ToArray()).FirstOrDefault();
                //ggf Mappen wenn über EDMX gelesen wird:
                //rval = Mapper.Map<PERSON, KundeDto>(queryResult, rval);
                return rval;
            }
        }

        /// <summary>
        /// Returns all Prkgroupm Details
        /// </summary>
        /// <param name="sysprkgroupm"></param>
        /// <returns></returns>
        public PrkgroupmDto getPrkgroupmDetails(long sysprkgroupm)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysprkgroupm", Value = sysprkgroupm });
                PrkgroupmDto rval = ctx.ExecuteStoreQuery<PrkgroupmDto>(QUERYPRKGROUPM, parameters.ToArray()).FirstOrDefault();
                //ggf Mappen wenn über EDMX gelesen wird:
                //rval = Mapper.Map<PERSON, KundeDto>(queryResult, rval);
                return rval;
            }
        }

        /// <summary>
        /// Returns all Prkgroupz Details
        /// </summary>
        /// <param name="sysPrkgroupz"></param>
        /// <returns></returns>
        public PrkgroupzDto getPrkgroupzDetails(long sysprkgroupz)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysprkgroupz", Value = sysprkgroupz });
                PrkgroupzDto rval = ctx.ExecuteStoreQuery<PrkgroupzDto>(QUERYPRKGROUPZ, parameters.ToArray()).FirstOrDefault();
                //ggf Mappen wenn über EDMX gelesen wird:
                //rval = Mapper.Map<PERSON, KundeDto>(queryResult, rval);
                return rval;
            }
        }

        /// <summary>
        /// Returns all Prkgroups Details
        /// </summary>
        /// <param name="sysprkgroups"></param>
        /// <returns></returns>
        public PrkgroupsDto getPrkgroupsDetails(long sysprkgroups)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysprkgroups", Value = sysprkgroups });
                PrkgroupsDto rval = ctx.ExecuteStoreQuery<PrkgroupsDto>(QUERYPRKGROUPS, parameters.ToArray()).FirstOrDefault();
                //ggf Mappen wenn über EDMX gelesen wird:
                //rval = Mapper.Map<PERSON, KundeDto>(queryResult, rval);
                return rval;
            }
        }

        /// <summary>
        /// Returns all Seg Details
        /// </summary>
        /// <param name="sysseg"></param>
        /// <returns></returns>
        public SegDto getSegDetails(long sysseg)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysseg", Value = sysseg });
                SegDto rval = ctx.ExecuteStoreQuery<SegDto>(QUERYSEG, parameters.ToArray()).FirstOrDefault();
                //ggf Mappen wenn über EDMX gelesen wird:
                //rval = Mapper.Map<PERSON, KundeDto>(queryResult, rval);
                return rval;
            }
        }

        /// <summary>
        /// Returns all Segc Details
        /// </summary>
        /// <param name="syssegc"></param>
        /// <returns></returns>
        public SegcDto getSegcDetails(long syssegc)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syssegc", Value = syssegc });
                SegcDto rval = ctx.ExecuteStoreQuery<SegcDto>(QUERYSEGC, parameters.ToArray()).FirstOrDefault();
                //ggf Mappen wenn über EDMX gelesen wird:
                //rval = Mapper.Map<PERSON, KundeDto>(queryResult, rval);
                return rval;
            }
        }

        /// <summary>
        /// Returns all Stickynote Details
        /// </summary>
        /// <param name="sysstickynote"></param>
        /// <returns></returns>
        public StickynoteDto getStickynoteDetails(long sysstickynote)
        {
            using (DdOwExtended ctxOw = new DdOwExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysstickynote", Value = sysstickynote });
                StickynoteDto rval = ctxOw.ExecuteStoreQuery<StickynoteDto>(QUERYSTICKYNOTE, parameters.ToArray()).FirstOrDefault();
                //ggf Mappen wenn über EDMX gelesen wird:
                //rval = Mapper.Map<PERSON, KundeDto>(queryResult, rval);
                return rval;
            }
        }

        /// <summary>
        /// Returns all Stickytype Details
        /// </summary>
        /// <param name="sysStickytype"></param>
        /// <returns></returns>
        public StickytypeDto getStickytypeDetails(long sysstickytype)
        {
            using (DdOwExtended ctxOw = new DdOwExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysstickytype", Value = sysstickytype });
                StickytypeDto rval = ctxOw.ExecuteStoreQuery<StickytypeDto>(QUERYSTICKYTYPE, parameters.ToArray()).FirstOrDefault();
                //ggf Mappen wenn über EDMX gelesen wird:
                //rval = Mapper.Map<PERSON, KundeDto>(queryResult, rval);
                return rval;
            }
        }

        /// <summary>
        /// Returns all Wfsignature Details
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public WfsignatureDto getWfsignatureDetail(long input)
        {
            using (DdOwExtended ctxOw = new DdOwExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syswfsignature", Value = input });
                WfsignatureDto rval = ctxOw.ExecuteStoreQuery<WfsignatureDto>(QUERYWFSIGNATURE, parameters.ToArray()).FirstOrDefault();
                //ggf Mappen wenn über EDMX gelesen wird:
                //rval = Mapper.Map<PERSON, KundeDto>(queryResult, rval);
                return rval;
            }
        }


        /// <summary>
        /// Returns all Wfsignature Details over type
        /// </summary>
        /// <param name="wfsignatureType"></param>
        /// <param name="sysWfUser"></param>
        /// <returns></returns>
        public WfsignatureDto getWfsignatureDetail(WfsignatureType wfsignatureType, long sysWfUser)
        {
            using (DdOwExtended ctxOw = new DdOwExtended())
            {
                WFSIGNATURE rval = (from sig in ctxOw.WFSIGNATURE
                                    where sig.WFUSER.SYSWFUSER == sysWfUser && (sig.TYP == (int)WfsignatureType.Allgemein || sig.TYP == (int)wfsignatureType)
                                    orderby sig.TYP descending
                                    select sig).FirstOrDefault();

                return Mapper.Map(rval, new WfsignatureDto());
            }
        }


        /// <summary>
        /// Lädt alle Fileatts für eine Area und eine sysid
        /// </summary>
        /// <param name="area"></param>
        /// <param name="sysid"></param>
        /// <returns></returns>
        public List<FileattDto> getFileatts(string area, long sysid)
        {
            using (DdOwExtended ctxOw = new DdOwExtended())
            {
                if (area.ToLower() == "mailmsg")
                {
                    var fileatts = (from p in ctxOw.FILEATT
                                    where p.MAILMSG.SYSMAILMSG == sysid
                                    select p).ToList();

                    return Mapper.Map(fileatts, new List<FileattDto>());
                }
                else if (area.ToLower() == "ptask")
                {
                    var fileatts = (from p in ctxOw.FILEATT
                                    where p.PTASK.SYSPTASK == sysid
                                    select p).ToList();

                    return Mapper.Map(fileatts, new List<FileattDto>());
                }
                else if (area.ToLower() == "apptmt")
                {
                    var fileatts = (from p in ctxOw.FILEATT
                                    where p.APPTMT.SYSAPPTMT == sysid
                                    select p).ToList();

                    return Mapper.Map(fileatts, new List<FileattDto>());
                }
                else
                {
                    var fileatts = (from p in ctxOw.FILEATT
                                    where p.SYSID.HasValue && p.SYSID.Value == sysid &&
                                          p.AREA.ToLower() == area.ToLower()
                                    select p).ToList();

                    return Mapper.Map(fileatts, new List<FileattDto>());
                }
            }

        }

        /// <summary>
        /// Returns Angebot Details
        /// </summary>
        /// <param name="sysStickytype"></param>
        /// <returns></returns>
        public AngebotDto getAngebotDetails(long sysangebot)
        {
            using (DdOlExtended ctx = new DdOlExtended())
            {

                ANGEBOT angebotOutput = (from ang in ctx.ANGEBOT
                                         where ang.SYSID == sysangebot
                                         select ang).FirstOrDefault();
                if (angebotOutput == null)
                {
                    throw new ArgumentException("Angebot does not exist: sysId = " + sysangebot);
                }

                AngebotDto rval = Mapper.Map<ANGEBOT, AngebotDto>(angebotOutput);

                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = sysangebot });
                rval.options = ctx.ExecuteStoreQuery<AngAntOptionDto>(QUERYANGEBOTOPTS, parameters.ToArray()).FirstOrDefault();
                if (rval.options == null)
                    rval.options = new AngAntOptionDto();

                parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysangebot", Value = sysangebot });
                rval.options.mycalccount = ctx.ExecuteStoreQuery<long>(QUERYANGEBOTMYCALCS, parameters.ToArray()).FirstOrDefault();

                rval.ls = getLsaddDetail(angebotOutput.SYSLS);
                rval.waehrung = getWaehrungDetail(angebotOutput.SYSWAEHRUNG);

                if (rval.sysKd > 0)
                {
                    rval.kundeName = ctx.ExecuteStoreQuery<String>("select NAME|| ', ' ||VORNAME from person where sysperson=" + rval.sysKd, null).FirstOrDefault();
                }
                if (rval.sysIt > 0)
                {
                    rval.interessentName = ctx.ExecuteStoreQuery<String>("select IT.NAME|| ', ' ||IT.VORNAME from it where sysit=" + rval.sysIt, null).FirstOrDefault();
                }

                if (!angebotOutput.ANGVARList.IsLoaded)
                    angebotOutput.ANGVARList.Load();
                List<AngvarDto> angvarDtoList = new List<AngvarDto>();
                foreach (ANGVAR angvar in angebotOutput.ANGVARList)
                {
                    AngvarDto angvardto = Mapper.Map<ANGVAR, AngvarDto>(angvar);
                    parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysangvar", Value = angvar.SYSANGVAR });

                    //ANGOB
                    List<AngobDto> angobList = null;
                    angobList = ctx.ExecuteStoreQuery<AngobDto>(QUERYANGOBANGEBOT, parameters.ToArray()).ToList();
                    angvardto.angobList = angobList;
                    foreach (AngobDto angob in angobList)
                    {
                        List<Devart.Data.Oracle.OracleParameter> obpar = new List<Devart.Data.Oracle.OracleParameter>();
                        obpar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysobini", Value = angob.sysOb });
                        angob.zusatzdaten = ctx.ExecuteStoreQuery<AngobIniDto>(QUERYANGOBINI, obpar.ToArray()).FirstOrDefault();
                        List<Devart.Data.Oracle.OracleParameter> austpar = new List<Devart.Data.Oracle.OracleParameter>();
                        austpar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysangob", Value = angob.sysOb });
                        angob.ausstattungen = ctx.ExecuteStoreQuery<ObjektAustDto>(QUERYANGOBAUST, austpar.ToArray()).ToList();
                    }

                    //ANGOBSL
                    List<AngobslDto> angobslList = null;
                    parameters.Clear();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysangvar", Value = angvar.SYSANGVAR });
                    angobslList = ctx.ExecuteStoreQuery<AngobslDto>(QUERYANGOBSL, parameters.ToArray()).ToList();
                    //ANGOBSLPOS
                    foreach (AngobslDto angoblsItem in angobslList)
                    {
                        parameters.Clear();
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysvtobsl", Value = angoblsItem.sysid });
                        List<AngobslposDto> angoblsposList = null;
                        angoblsposList = ctx.ExecuteStoreQuery<AngobslposDto>(QUERYANGOBSLPOS, parameters.ToArray()).ToList();
                        angoblsItem.angobslposList = angoblsposList;
                    }
                    angvardto.angobslList = angobslList;
                    angvarDtoList.Add(angvardto);
                }
                rval.varianten = angvarDtoList;


                if (!angebotOutput.ADMADDReference.IsLoaded)
                    angebotOutput.ADMADDReference.Load();
                if (angebotOutput.ADMADD != null)
                {
                    AdmaddDto am = new AdmaddDto();
                    am.sysAdmadd = angebotOutput.ADMADD.SYSADMADD;
                    am.bezeichnung = angebotOutput.ADMADD.BEZEICHNUNG;
                    rval.aussendienstmitarbeiter = am;
                }
                VpfiladdDto vertriebspartner = getVpfiladdDetail(sysangebot, QUERYVPFILADDANGEBOT);
                if (vertriebspartner != null)
                    rval.vertriebspartner = vertriebspartner;
                //TODO EDMX
                rval.beschrdeutsch = ctx.ExecuteStoreQuery<String>("select beschrdeutsch from angebot where sysid=" + sysangebot, null).FirstOrDefault();
                rval.extratingcode = ctx.ExecuteStoreQuery<String>("select extratingcode from angebot where sysid=" + sysangebot, null).FirstOrDefault();


                //Produkt Info
                rval.produkt = getProduktInfoAngebotDetails(angebotOutput.SYSID);


                return rval;
            }
        }

        /// <summary>
        /// angebot product details
        /// </summary>
        /// <param name="sysang">id angebot</param>
        /// <returns>product information</returns>
        public ProduktInfoDto getProduktInfoAngebotDetails(long sysang)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                //Produkt Info
                ObjectResult<ProduktInfoDto> result = ctx.ExecuteStoreQuery<ProduktInfoDto>("SELECT prproduct.name productname,ratebrutto,angkalk.sysprproduct, angkalk.lz laufzeit, angkalk.ahk barkaufpreis, angkalk.sz anzahlungEintausch, angkalk.rw restrate, angkalk.zinseff effJahreszins, angkalk.ll kmProJahr, angkalk.rw indikativerRestwert, bgintern, angkalk.bgextern kreditlimit, angkalk.auszahlung auszahlungBank, initladung auszahlungKarte, angkalk.zinsrap effZinssatz, angkalk.bgextern kreditbetrag, angkalk.auszahlung auszahlungsbetrag1, angkalk.rate, angkalk.syswaehrung waehrung, angkalk.rw restwert, angkalk.depot kaution, angkalk.zins nominalJahreszins, angkalk.sysobtyp, card.emboss nameAufKarte FROM angkalk,card,prproduct WHERE angkalk.sysprproduct=prproduct.sysprproduct(+) and angkalk.sysangebot=card.sysangebot(+) and angkalk.sysangebot=" + sysang);
                List<ProduktInfoDto> infos = new List<ProduktInfoDto>(result.AsEnumerable());
                foreach (ProduktInfoDto produkt in infos)
                {
                    if (produkt.sysprproduct == 0)
                        continue;

                    return produkt;
                }
                return new ProduktInfoDto();
            }
        }
         /// <summary>
        /// VT product details
        /// </summary>
        /// <param name="sysvt">id VT</param>
        /// <returns>product information</returns>
        public ProduktInfoDto getProduktInfoVTDetails(long sysvt)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                //Produkt Info
                ObjectResult<ProduktInfoDto> result = ctx.ExecuteStoreQuery<ProduktInfoDto>("SELECT  prproduct.name productname,ratebrutto,kalk.sysprproduct, lz laufzeit, anzahlung anzahlungEintausch, rate, zinssz effZinssatz, syswaehrung waehrung, rw restwert, sysobtyp, ahk barkaufpreis,  rw restrate, 0 effJahreszins, 0 kmProJahr, rw indikativerRestwert, bgextern kreditlimit, 0 auszahlungBank, 0 auszahlungKarte,  bgextern kreditbetrag, 0 auszahlungsbetrag1,  depot kaution, zins nominalJahreszins FROM kalk,prproduct WHERE kalk.sysprproduct=prproduct.sysprproduct(+) and kalk.sysvt=" + sysvt);
               
            
                List<ProduktInfoDto> infos = new List<ProduktInfoDto>(result.AsEnumerable());
                foreach (ProduktInfoDto produkt in infos)
                {
                    if (produkt.sysprproduct == 0)
                        continue;

                    return produkt;
                }
                return new ProduktInfoDto();
            }
        }
        


        /// <summary>
        /// Fetches all Auflagen for Antrag
        /// </summary>
        /// <param name="sysantrag"></param>
        /// <returns></returns>
        public List<RatingAuflageDto> getAuflagen(long sysantrag, String isoCode)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysAntrag", Value = sysantrag });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "isoCode", Value = isoCode });
                return ctx.ExecuteStoreQuery<RatingAuflageDto>(QUERYAUFLAGEN, parameters.ToArray()).ToList();

            }
        }
        /// <summary>
        /// Fetches all Rules for Antrag
        /// </summary>
        /// <param name="sysantrag"></param>
        /// <returns></returns>
        public List<AuskunftRegelDto> getAuskunftRegeln(long sysantrag, String isoCode)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysAntrag", Value = sysantrag });
                //parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "isoCode", Value = isoCode });

                return ctx.ExecuteStoreQuery<AuskunftRegelDto>(QUERYREGELN, parameters.ToArray()).ToList();
            }
        }


        /// <summary>
        /// antrag product details
        /// </summary>
        /// <param name="sysant">id antrag</param>
        /// <returns>product information</returns>
        public ProduktInfoDto getProduktInfoAntragDetails(DdOlExtended ctx, long sysant)
        {

            DbConnection con = (ctx.Connection as EntityConnection).StoreConnection;
                //Produkt Info
            List<ProduktInfoDto> infos =
                con.Query<ProduktInfoDto>("SELECT prproduct.name productname,antkalk.ratebrutto,antkalk.sysprproduct, antkalk.lz laufzeit, antkalk.ahk barkaufpreis, antkalk.sz anzahlungEintausch, antkalk.rw restrate, antkalk.zinseff effJahreszins, antkalk.ll kmProJahr, antob.rw indikativerRestwert, antkalk.bgextern kreditlimit, antkalk.auszahlung auszahlungBank, antkalk.initladung auszahlungKarte, antkalk.zinsrap effZinssatz,antkalk.bgintern bgintern, antkalk.bgextern kreditbetrag, antkalk.auszahlung auszahlungsbetrag1, antkalk.rate, antkalk.syswaehrung waehrung, antkalk.rw restwert, antkalk.depot kaution, antkalk.zins nominalJahreszins, antkalk.sysobtyp, card.emboss nameAufKarte, antrag.vertriebsweg vertriebsweg  FROM antkalk,card,antob, antrag,prproduct WHERE antkalk.sysprproduct=prproduct.sysprproduct(+) and antkalk.sysprproduct>0 and antkalk.sysantrag=card.sysantrag(+) and antkalk.sysantrag = antob.sysantrag(+) and antrag.sysid = antkalk.sysantrag and antrag.sysid=:sysid", new { sysid = sysant }).ToList();
                    //ctx.ExecuteStoreQuery<ProduktInfoDto>("SELECT ratebrutto,sysprproduct, lz laufzeit, ahk barkaufpreis, sz anzahlungEintausch, rw restrate, zinseff effJahreszins, ll kmProJahr, rw indikativerRestwert, bgextern kreditlimit, auszahlung auszahlungBank, initladung auszahlungKarte, zinsrap effZinssatz, bgextern kreditbetrag, auszahlung auszahlungsbetrag1, rate, syswaehrung waehrung, rw restwert, depot kaution, zins nominalJahreszins, sysobtyp, card.emboss nameAufKarte FROM antkalk,card WHERE antkalk.sysantrag=card.sysantrag(+) and antkalk.sysantrag=" + sysant);
                foreach (ProduktInfoDto produkt in infos)
                {
                    if (produkt.sysprproduct == 0)
                        continue;

                    return produkt;
                }
                return new ProduktInfoDto();
            
        }
        /// <summary>
        /// antrag product details
        /// </summary>
        /// <param name="sysant">id antrag</param>
        /// <returns>product information</returns>
        public ProduktInfoDto getProduktInfoAntragDetails( long sysant)
        {

            using (DdOlExtended ctx = new DdOlExtended())
            {
                return getProduktInfoAntragDetails(ctx, sysant);
            }

        }

        /// <summary>
        /// Returns Angebot Details
        /// </summary>
        /// <param name="sysStickytype"></param>
        /// <returns></returns>
        public virtual AntragDto getAntragDetails(long sysantrag)
        {
            
                using (DdOlExtended ctx = new DdOlExtended())
                {
                    DbConnection con = (ctx.Connection as EntityConnection).StoreConnection;


                    AntragDto rval = con.Query<AntragDto>("select antrag.*,KUNDE.NAME kundename,KUNDE.VORNAME kundevorname, KUNDE.ORT kundeOrt, kunde.strasse kundestrasse,kunde.plz kundeplz, kunde.hsnr kundeHausnr from antrag,person kunde where antrag.syskd=kunde.sysperson(+) and antrag.sysid=:sysid", new { sysid = sysantrag }).FirstOrDefault();
                    if(rval==null)
                        throw new ArgumentException("Antrag does not exist: sysId = " + sysantrag);
               
                    rval.options = con.Query<AngAntOptionDto>(QUERYANTRAGOPTS, new { sysid = sysantrag }).FirstOrDefault();
                    if (rval.options == null)
                        rval.options = new AngAntOptionDto();

                    rval.ls = getLsaddDetail(rval.sysls);
                    rval.waehrung = getWaehrungDetail(rval.syswaehrung);

                   

                    //ANTOB
                    rval.antob = con.Query<AntobDto>(QUERYANTOB, new { sysantrag=sysantrag}).FirstOrDefault();
                    if(rval.antob==null)
                        rval.antob = con.Query<AntobDto>(QUERYANTOB2, new { sysantrag = sysantrag }).FirstOrDefault();

                    if (rval.antob!=null)
                        rval.antob.ausstattungen = con.Query<ObjektAustDto>(QUERYANTOBAUST, new { sysantob = rval.antob.sysOb }).ToList();

                    //ANTOBSL
                    /*List<AntobslDto> antobslList = null;
                    parameters.Clear();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysangvar", Value = sysantrag });
                    antobslList = ctx.ExecuteStoreQuery<AntobslDto>(QUERYANTOBSL, parameters.ToArray()).ToList();*/
                    //ANTOBSLPOS
                    /*foreach (AntobslDto antoblsItem in antobslList)
                    {
                        parameters.Clear();
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysvtobsl", Value = antoblsItem.sysid});
                        List<AntobslposDto> antoblsposList = null;
                        antoblsposList = ctx.ExecuteStoreQuery<AntobslposDto>(QUERYANTOBSLPOS, parameters.ToArray()).ToList();
                        antoblsItem.antobslposList = antoblsposList;
                    }
                    rval.antoblsList= antobslList;*/

                    //
                    rval.sysMa = ctx.ExecuteStoreQuery<long>("select sysperson from antobsich where sysantrag="+sysantrag+" and sysperson>0 and sysperson!="+rval.sysKd+" and rang in (120,130,800)", null).FirstOrDefault();

                    if (rval.sysBerater > 0)
                        rval.sysBerater = ctx.ExecuteStoreQuery<long>("select sysperson from wfuser where syswfuser=" + rval.sysBerater).FirstOrDefault();
                   
                    if (rval.sysadm>0)
                    {
                        AdmaddDto am = new AdmaddDto();
                        am.sysAdmadd = rval.sysadm;
                        am.bezeichnung = ctx.ExecuteStoreQuery<String>("select bezeichnung from admadd where sysperson="+rval.sysadm,null).FirstOrDefault();
                        rval.aussendienstmitarbeiter = am;
                    }
                    VpfiladdDto vertriebspartner = getVpfiladdDetail(sysantrag, QUERYVPFILADDANTRAG);
                    if (vertriebspartner != null)
                    {
                        rval.vertriebspartner = vertriebspartner;
                    }

                    //Produkt Info
                    rval.produkt = getProduktInfoAntragDetails(ctx,rval.sysID);
                    rval.konto = con.Query<KontoDto>(QUERYANTKONTO, new { sysantrag = sysantrag }).FirstOrDefault();

                    rval.auflagen = con.Query<RatingAuflageDto>(QUERYAUFLAGEN2, new { sysid = sysantrag }).ToList();

                    return rval;
                }
            

        }

        /// <summary>
        /// update abwicklungsort for ANGEBOT/ANTRAG
        /// </summary>
        /// <param name="input"></param>
        public bool updateAbwicklungsort(iupdateAbwicklungsortDto input)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                long oldBerater = 0;
                
                if ("ANGEBOT".Equals(input.area))
                {
                    oldBerater = ctx.ExecuteStoreQuery<long>("select sysberater from angebot where sysid="+input.sysid,null).FirstOrDefault();
                    String query = "update angebot set sysabwicklung=:sysabwicklung, sysberater=:sysberater where sysid=:sysangebot";
                    if(input.sysabwicklungsort>0 &&input.sysberater==0)
                        query = "update angebot set sysabwicklung=:sysabwicklung where sysid=:sysangebot";
                    if(input.sysabwicklungsort==0 &&input.sysberater>=0)
                        query = "update angebot set sysberater=:sysberater where sysid=:sysangebot";

                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    if(input.sysberater>0)
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysberater", Value = input.sysberater });
                    if(input.sysabwicklungsort>0)
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysabwicklung", Value = input.sysabwicklungsort });
                    
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysangebot", Value = input.sysid });

                    ctx.ExecuteStoreCommand(
                        query,
                        parameters.ToArray());
                }
                if ("ANTRAG".Equals(input.area))
                {
                    oldBerater = ctx.ExecuteStoreQuery<long>("select sysberater from antrag where sysid=" + input.sysid, null).FirstOrDefault();
                    String query = "update antrag set sysabwicklung=:sysabwicklung, sysberater=:sysberater where sysid=:sysantrag";
                    if(input.sysabwicklungsort>0 &&input.sysberater==0)
                        query = "update antrag set sysabwicklung=:sysabwicklung where sysid=:sysantrag";
                    if(input.sysabwicklungsort==0 &&input.sysberater>=0)
                        query = "update antrag set sysberater=:sysberater where sysid=:sysantrag";

                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    if (input.sysberater > 0)
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysberater", Value = input.sysberater });
                    if (input.sysabwicklungsort > 0)
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysabwicklung", Value = input.sysabwicklungsort });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysantrag", Value = input.sysid });

                    ctx.ExecuteStoreCommand(query,
                        parameters.ToArray());
                }

                if(input.sysberater>=0)//Benutzeränderung->Casestep setzen
                {
                    List<Devart.Data.Oracle.OracleParameter> ipar = new List<Devart.Data.Oracle.OracleParameter>();
                    ipar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syswfuser", Value = sysWfuser });
                    ipar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "time", Value = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(DateTime.Now) });
                    ipar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "area", Value = input.area });
                    ipar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = input.sysid });
                    ipar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "param1", Value = oldBerater });
                    
                    ctx.ExecuteStoreCommand("insert into bpcasestep(param1,steptext,stepbyuser,stepdate,steptime,historyflag,oltable,sysoltable,mastercase) values(:param1,'Bearbeiter Sales gelöscht',:syswfuser,sysdate,:time,1,:area,:sysid,'WF_Abwicklung')", ipar.ToArray());


                    ipar = new List<Devart.Data.Oracle.OracleParameter>();
                    ipar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syswfuser", Value = sysWfuser });
                    ipar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "time", Value = Cic.OpenOne.Common.Util.DateTimeHelper.DateTimeToClarionTime(DateTime.Now) });
                    ipar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "area", Value = input.area });
                    ipar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = input.sysid });
                    ipar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "param1", Value = input.sysberater });

                    ctx.ExecuteStoreCommand("insert into bpcasestep(param1,steptext,stepbyuser,stepdate,steptime,historyflag,oltable,sysoltable,mastercase) values(:param1,'Bearbeiter Sales zugewiesen',:syswfuser,sysdate,:time,1,:area,:sysid,'WF_Abwicklung')", ipar.ToArray());
                }
            }
            return true;
        }

        /// <summary>
        /// get anciliary details for ANGEBOT/ANTRAG
        /// </summary>
        /// <param name="input"></param>
        public ogetAnciliaryDetailDto getAnciliaryDetail(igetAnciliaryDetailDto input)
        {
            ICASBo bo = Cic.One.Web.BO.BOFactoryFactory.getInstance().getCASBo();
            using (DdCtExtended ctx = new DdCtExtended())
            {
                if ("ANGEBOT".Equals(input.area))
                {
                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysangebot", Value = input.sysid });

                    ogetAnciliaryDetailDto rval= ctx.ExecuteStoreQuery<ogetAnciliaryDetailDto>(
                        "select sysabwicklung,sysberater,angebot nummer,locked from angebot where sysid=:sysangebot",
                        parameters.ToArray()).FirstOrDefault();


                    if (input.field == AnciliaryField.LOCKED)
                    {
                        return rval;
                    }
                           
                    Cic.OpenOne.Common.DTO.iCASEvaluateDto ieval = new Cic.OpenOne.Common.DTO.iCASEvaluateDto();
                    ieval.area = input.area;
                    String expr = "_F('GET_CALLBACK_SMS','"+input.area+"',"+input.sysid+","+sysWfuser+")";
                    ieval.expression = new String[] { expr };
                    ieval.sysID = new long[] { input.sysid };
                    Cic.OpenOne.Common.DTO.CASEvaluateResult[] er = null;
                    try
                    {
                        er = bo.getEvaluation(ieval, sysWfuser);
                        if (er[0].clarionResult[0] != null && !"".Equals(er[0].clarionResult[0]))
                            rval.smstext = er[0].clarionResult[0] ;

                    }
                    catch (Exception e)
                    {
                        _log.Info("CAS-Evaluation failed for SMSTEXT", e);
                    }
                    return rval;
                }
                else if ("PERSON".Equals(input.area) || "IT".Equals(input.area))
                {
                    
                    ogetAnciliaryDetailDto rval = new ogetAnciliaryDetailDto();
                 
                    Cic.OpenOne.Common.DTO.iCASEvaluateDto ieval = new Cic.OpenOne.Common.DTO.iCASEvaluateDto();
                    ieval.area = input.area;
                    String expr = "_F('GET_CALLBACK_SMS','" + input.area + "'," + input.sysid + "," + sysWfuser + ")";
                    ieval.expression = new String[] { expr };
                    ieval.sysID = new long[] { input.sysid };
                    Cic.OpenOne.Common.DTO.CASEvaluateResult[] er = null;
                    try
                    {
                        er = bo.getEvaluation(ieval, sysWfuser);
                        if (er[0].clarionResult[0] != null && !"".Equals(er[0].clarionResult[0]))
                            rval.smstext = er[0].clarionResult[0];

                    }
                    catch (Exception e)
                    {
                        _log.Info("CAS-Evaluation failed for SMSTEXT", e);
                    }
                    return rval;
                }
                else if ("ANTRAG".Equals(input.area))
                {
                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysantrag", Value = input.sysid });

                    ogetAnciliaryDetailDto rval= ctx.ExecuteStoreQuery<ogetAnciliaryDetailDto>(
                        "select sysabwicklung,sysberater,antrag nummer,locked from antrag where sysid=:sysantrag",
                        parameters.ToArray()).FirstOrDefault();
                    if(input.field==AnciliaryField.LOCKED)
                    {
                        return rval;
                    }
                    Cic.OpenOne.Common.DTO.iCASEvaluateDto ieval = new Cic.OpenOne.Common.DTO.iCASEvaluateDto();
                    ieval.area = input.area;
                    String expr = "_F('GET_CALLBACK_SMS','" + input.area + "'," + input.sysid + "," + sysWfuser + ")";
                    ieval.expression = new String[] { expr };
                    ieval.sysID = new long[] { input.sysid };
                    Cic.OpenOne.Common.DTO.CASEvaluateResult[] er = null;
                    try
                    {
                        er = bo.getEvaluation(ieval, sysWfuser);
                        if (er[0].clarionResult[0] != null && !"".Equals(er[0].clarionResult[0]))
                            rval.smstext = er[0].clarionResult[0];

                    }
                    catch (Exception e)
                    {
                        _log.Info("CAS-Evaluation failed for SMSTEXT", e);
                    }
                    return rval;
                }

            }
            return null;
        }

        /// <summary>
        /// update smstext for ANTRAG
        /// </summary>
        /// <param name="input"></param>
        public virtual bool updateSMSText(iupdateSMSTextDto input)
        {
            return false;//not supported here, only in gatebanknow-Dao
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sysangebot"></param>
        /// <param name="syswfuser"></param>
        public void updateLegitimationMethode(long sysangebot, long syswfuser, long sysit, string legitimationMethode)
        {


            using (DdOlExtended ctx = new DdOlExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = sysit });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysangebot", Value = sysangebot });
                
                long sysitpkz = ctx.ExecuteStoreQuery<long>("select sysitpkz from itpkz where sysit=:sysit and sysangebot=:sysangebot", parameters.ToArray()).FirstOrDefault();
                bool legitimated = false;
                if (legitimationMethode != null)
                {
                    try
                    {
                        long methodcode = long.Parse(legitimationMethode);
                        if (methodcode > 1)
                            legitimated = true;
                    }
                    catch (Exception) { }
                }
                if (sysitpkz>0)
                {
                    ITPKZ itpkz = (from p in ctx.ITPKZ
                                  where p.SYSITPKZ ==sysitpkz
                                  select p).FirstOrDefault();
                    itpkz.LEGITMETHODCODE = legitimationMethode;
                    itpkz.LEGITDATUM = DateTime.Now;
                    itpkz.LEGITABNEHMER = syswfuser.ToString();
                    
                    List<Devart.Data.Oracle.OracleParameter> pars = new List<Devart.Data.Oracle.OracleParameter>();
                    pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p1", Value = sysitpkz });
                    pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "flag", Value = legitimated?1:0 });
                    ctx.ExecuteStoreCommand("UPDATE itpkz set KDIDENTFLAG=:flag where SYSITPKZ=:p1", pars.ToArray());
                    
                }
                else
                {
                    parameters.Clear();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = sysit });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysangebot", Value = sysangebot });
                    long sysitukz= ctx.ExecuteStoreQuery<long>("select sysitukz from itukz where sysit=:sysit and sysangebot=:sysangebot", parameters.ToArray()).FirstOrDefault();

                    if (sysitukz > 0)
                    {
                        ITUKZ itukz = (from p in ctx.ITUKZ
                                       where p.SYSITUKZ == sysitukz
                                       select p).FirstOrDefault();
                        itukz.LEGITMETHODCODE = legitimationMethode;
                        itukz.LEGITDATUM = DateTime.Now;
                        itukz.LEGITABNEHMER = syswfuser.ToString();
                        List<Devart.Data.Oracle.OracleParameter> pars = new List<Devart.Data.Oracle.OracleParameter>();
                        pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p1", Value = sysitukz });
                        pars.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "flag", Value = legitimated ? 1 : 0 });
                        ctx.ExecuteStoreCommand("UPDATE itukz set KDIDENTFLAG=:flag where SYSITUKZ=:p1", pars.ToArray());
                    }

                }
                ctx.SaveChanges();

            }

        }


        /// <summary>
        /// delivers additional customer data for this request
        /// </summary>
        /// <param name="syskd">customer</param>
        /// <param name="sysantrag">request</param>
        /// <returns>additional customer data</returns>
        public ogetZusatzdaten getZusatzdatenITDetail(long sysit, long sysangebot)
        {
            if (sysit == 0 || sysangebot == 0)
                return null;

            ogetZusatzdaten rval = new ogetZusatzdaten();

            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysit", Value = sysit });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysangebot", Value = sysit });
                rval.pkz = ctx.ExecuteStoreQuery<PkzDto>("select * from pkz where sysit=:sysit and sysangebot=:sysangebot", parameters.ToArray()).FirstOrDefault();

                parameters.Clear();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syskd", Value = sysit });
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysantrag", Value = sysangebot });
                rval.ukz = ctx.ExecuteStoreQuery<UkzDto>("select * from ukz where sysit=:sysit and sysangebot=:sysangebot", parameters.ToArray()).FirstOrDefault();
            }

            return rval;
        }

        /// <summary>
        /// Returns Vertrag Details
        /// </summary>
        /// <param name="sysStickytype"></param>
        /// <returns></returns>
        virtual public VertragDto getVertragDetails(long sysvertrag)
        {

            using (DdCtExtended ctx = new DdCtExtended())
            {
                DbConnection con = (ctx.Connection as EntityConnection).StoreConnection;
                /*
                VT vertragOutput = (from ant in ctx.VT
                                    where ant.SYSID == sysvertrag
                                    select ant).FirstOrDefault();
                if (vertragOutput == null)
                {
                    throw new ArgumentException("Vertrag does not exist: sysId = " + sysvertrag);
                }

                VertragDto rval = Mapper.Map<VT, VertragDto>(vertragOutput);
                 * */
                // orig 20180214: VertragDto rval = con.Query<VertragDto>("select vt.* from vt where sysid=:sysid", new { sysid = sysvertrag }).FirstOrDefault();
				VertragDto rval = con.Query<VertragDto> (
					"select vt.*, kd.name kundeName, kd.strasse || ' ' || kd.hsnr kundestrasse, kd.plz kundeplz, kd.ort kundeort, kd.telefon kundetelnr, kd.email kundeemail " +
						"from vt, cic.person kd " +
						"where sysid=:sysid and vt.syskd = kd.sysperson (+) ", new { sysid = sysvertrag }).FirstOrDefault ();

				if (rval==null)
                    throw new ArgumentException("Vertrag does not exist: sysId = " + sysvertrag);

                if (rval.konstellation != null)
                {
                    rval.konstellation = rval.konstellation.Trim();
                }
                //TODO EDMX Mapping
                //rval.gesamt = ctx.ExecuteStoreQuery<double>("SELECT GESAMT from VT where sysid=" + sysvertrag, null).FirstOrDefault();
                
                if (rval.sysKd > 0)
                {
					// now SET in con.Query<VertragDto>:  rval.kundeName = ctx.ExecuteStoreQuery<String>("SELECT name from person where sysperson=" + rval.sysKd, null).FirstOrDefault();
					rval.sysPerson = rval.sysKd;
                }

                rval.ls = getLsaddDetail (rval.sysls);
                rval.waehrung = getWaehrungDetail (rval.syswaehrung);

                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysvt", Value = sysvertrag });

                //ANTOB
                List<ObDto> obList = null;

                obList = con.Query<ObDto>(QUERYOBVT, new { sysvt = sysvertrag }).ToList();//ctx.ExecuteStoreQuery<ObDto>(QUERYOBVT, parameters.ToArray()).ToList();
                rval.obList = obList;

                if (obList != null)
                {

                    foreach (ObDto angob in obList)
                    {
                        //List<Devart.Data.Oracle.OracleParameter> obpar = new List<Devart.Data.Oracle.OracleParameter>();
                        //obpar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysobini", Value = angob.sysOb });
                        angob.zusatzdaten = con.Query<ObIniDto>(QUERYOBINI, new { sysobini = angob.sysOb }).FirstOrDefault();//ctx.ExecuteStoreQuery<ObIniDto>(QUERYOBINI, obpar.ToArray()).FirstOrDefault();
                    }

                    parameters.Clear();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysob", Value = sysvertrag });
                    rval.depot = ctx.ExecuteStoreQuery<long> (QUERYVTDEPOT, parameters.ToArray()).FirstOrDefault();

                }

                //ANTOBSL
                parameters.Clear();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysvt", Value = sysvertrag });
                rval.vtobslList = ctx.ExecuteStoreQuery<VtobslDto> (QUERYVTOBSL, parameters.ToArray()).ToList();
                //ANTOBSLPOS
                if(rval.vtobslList!=null)
                { 
                    foreach (VtobslDto vtobslItem in rval.vtobslList)
                    {
                        parameters.Clear();
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysvtobsl", Value = vtobslItem.sysid });
                        vtobslItem.vtobslpostList = ctx.ExecuteStoreQuery<VtobslposDto> (QUERYVTOBSLPOS, parameters.ToArray()).ToList();
                    }
                }
               

                if (rval.sysadm>0)
                {
                    AdmaddDto am = new AdmaddDto();
                    am.sysAdmadd = rval.sysadm;
                    am.bezeichnung = ctx.ExecuteStoreQuery<String> ("select bezeichnung from admadd where sysperson="+rval.sysadm,null).FirstOrDefault();
                    rval.aussendienstmitarbeiter = am;
                }
                VpfiladdDto vertriebspartner = getVpfiladdDetail (sysvertrag, QUERYVPFILADDVERTRAG);
                if (vertriebspartner != null)
                    rval.vertriebspartner = vertriebspartner;


                //Produkt Info
                parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysvt", Value = rval.sysID });

                rval.produkt = ctx.ExecuteStoreQuery<ProduktInfoDto>("SELECT sysprproduct, lz laufzeit, anzahlung anzahlungEintausch, rate, zinssz effZinssatz, syswaehrung waehrung, rw restwert, sysobtyp FROM kalk WHERE sysvt=:sysvt", parameters.ToArray()).FirstOrDefault();
               
                //other info
                parameters.Clear();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysvt", Value = rval.sysID });
                VertragInfoDto other = ctx.ExecuteStoreQuery<VertragInfoDto>("select ulon04, dat13, option8, option7 from vtoption where sysid=:sysvt", parameters.ToArray()).FirstOrDefault();
                rval.aufschubfrist = other.ulon04;
                rval.zinsGarantBis = other.dat13;
                rval.ratenpause = other.option8;
                rval.ratenpause += other.option7;

                parameters.Clear();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysvt", Value = rval.sysID });
                rval.auftrag = ctx.ExecuteStoreQuery<AuftragDto> ("select gbetrag from auftrag where sysvt=:sysvt and typ=141", parameters.ToArray()).FirstOrDefault();

                parameters.Clear();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysvt", Value = rval.sysID });
                rval.gesperrt = ctx.ExecuteStoreQuery<int> ("SELECT nkklinie.gesperrt FROM penkonto JOIN nkklinie using (sysnkonto) WHERE penkonto.rang = 10000 AND penkonto.sysvt = :sysvt", parameters.ToArray()).FirstOrDefault();

                parameters.Clear();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysvt", Value = rval.sysID });
                rval.accountid = ctx.ExecuteStoreQuery<string> ("SELECT accountid FROM card WHERE card.sysvt = :sysvt", parameters.ToArray()).FirstOrDefault();

                parameters.Clear();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysvt", Value = rval.sysID });
                rval.intkband = ctx.ExecuteStoreQuery<IntkBandDto> ("SELECT intkband.lowerb, intkband.intrate, intkband.redrate FROM penkonto JOIN nkk using (sysnkonto) JOIN intkdate on nkk.sysintks=intkdate.sysintk JOIN intkband using (sysintkdate) WHERE penkonto.rang = 10000 AND nkk.SYSNKKPARENT = 0  AND penkonto.sysvt = :sysvt ORDER BY validfrom DESC", parameters.ToArray()).ToList();

                parameters.Clear();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysantrag", Value = rval.sysAntrag });
                rval.version = ctx.ExecuteStoreQuery<string>("select wftzvar.value FROM wftzust JOIN wftable using (syswftable) JOIN wftzvar using (syswftzust) WHERE wftzust.syslease=:sysantrag AND wftable.syscode='ANTRAG' AND wftzvar.code='Version Vertrag'", parameters.ToArray()).FirstOrDefault();

                parameters.Clear();
                parameters.Add(new OracleParameter { ParameterName = "sysvt", Value = rval.sysID });
                rval.mwst = ctx.ExecuteStoreQuery<double> ("select mwst.prozent FROM vt join mwst using (sysmwst) WHERE vt.sysid=:sysvt", parameters.ToArray()).FirstOrDefault();

                return rval;
            }


        }

        /// <summary>
        /// Returns Vorgang Details
        /// </summary>
        /// <param name="sysId"></param>
        /// <param name="area"></param>
        /// <returns></returns>
        public VorgangDto getVorgangDetails(long sysId, string area)
        {

            if (sysId > 0 && area.Length > 0)
            {
                using (DdCtExtended ctx = new DdCtExtended())
                {
                    VorgangDto rval = new VorgangDto();

                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = sysId });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "area", Value = area });

                    rval = ctx.ExecuteStoreQuery<VorgangDto>("select * from cic.VC_ANGANT where area=:area and sysid=:sysid", parameters.ToArray()).FirstOrDefault();
                    if (rval == null)
                    {
                        parameters = new List<Devart.Data.Oracle.OracleParameter>();
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = sysId });
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "area", Value = area });

                        rval = ctx.ExecuteStoreQuery<VorgangDto>("select * from VC_VORGANG_RED where area=:area and sysid=:sysid", parameters.ToArray()).FirstOrDefault();
                    }
                    if (rval == null)
                    {
                        parameters = new List<Devart.Data.Oracle.OracleParameter>();
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = sysId });

                        rval = ctx.ExecuteStoreQuery<VorgangDto>("select 'ANGEBOT' area, angebot nummer, angebot.* from angebot where sysid=:sysid", parameters.ToArray()).FirstOrDefault();
                    }
					//// rh 20170403: ToDo: subst for REAL data 
					//// rh 20170403: emulate different SlaIndicators: 
					//switch (sysId % 3)						
					//{	
					//case 2:
					//	rval.slaindicator = "red";
					//	break;
					//case 1:
					//	rval.slaindicator = "orange";
					//	break;
					//case 0:
					//default:
					//	rval.slaindicator = "black";
					//	break;
					//}

                    return rval;
                }
            }
            else
            {
                return null;
            }


        }

        public LsaddDto getLsaddDetail(long? sysLsadd)
        {
            if (sysLsadd != null && sysLsadd > 0)
                using (DdCtExtended ctx = new DdCtExtended())
                {
                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syslsadd", Value = sysLsadd });
                    LsaddDto rval = new LsaddDto();
                    rval = ctx.ExecuteStoreQuery<LsaddDto>(QUERYLSADD, parameters.ToArray()).FirstOrDefault();
                    return rval;
                }
            else
                return null;

        }

        public WaehrungDto getWaehrungDetail(long? sysWaehrung)
        {
            if (sysWaehrung != null && sysWaehrung > 0)
                using (DdCtExtended ctx = new DdCtExtended())
                {
                    DbConnection con = (ctx.Connection as EntityConnection).StoreConnection;
                    return con.Query<WaehrungDto>(QUERYWAEHRUNG, new { syswaehrung = sysWaehrung }).FirstOrDefault();
                }
            else
                return null;
        }

        public AdmaddDto getAdmaddDetail(long sysAdmadd)
        {
            if (sysAdmadd > 0)
                using (DdCtExtended ctx = new DdCtExtended())
                {
                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysadmadd", Value = sysAdmadd });
                    AdmaddDto rval = ctx.ExecuteStoreQuery<AdmaddDto>(QUERYADMADD, parameters.ToArray()).FirstOrDefault();
                    return rval;
                }
            else
                return null;
        }

        public VpfiladdDto getVpfiladdDetail(long sysid, string query)
        {
            if (sysid > 0)
                using (DdCtExtended ctx = new DdCtExtended())
                {
                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = sysid });
                    VpfiladdDto rval = new VpfiladdDto();
                    rval = ctx.ExecuteStoreQuery<VpfiladdDto>(query, parameters.ToArray()).FirstOrDefault();
                    return rval;
                }
            else
                return null;
        }

        public virtual ZekDto getZek(long syszek)
        {
            return null;
        }

        
        /// <summary>
        /// check if entry of given type with given id exists in database
        /// </summary>
        /// <param name="area">entry type</param>
        /// <param name="sysid">primary key</param>
        /// <returns>exists</returns>
        public bool getExists(string area, long sysid)
        {
            if (sysid == 0 || area == null || area.Length == 0)
                return false;

            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = sysid });

                int? exists = null;

                switch (area)
                {
                    case "VT":
                        exists = ctx.ExecuteStoreQuery<int?>("select 1 from vt where sysid=:sysid", parameters.ToArray()).FirstOrDefault();
                        break;
					//case "ANGEBOT":	// rh 20170322: ANG als ANT behandeln
					//	exists = ctx.ExecuteStoreQuery<int?> ("select 1 from angebot where sysid=:sysid", parameters.ToArray ()).FirstOrDefault ();
					//	break;
					case "ANTRAG":
						exists = ctx.ExecuteStoreQuery<int?> ("select 1 from antrag where sysid=:sysid", parameters.ToArray ()).FirstOrDefault ();
						if (exists == null)	// if no ANT exists, let's look for an ANG (rh: 20170322)
						{ 
							// rh 20170322: Allow ZEK OK even if only an ANG exists (lt. MG) 
							List<Devart.Data.Oracle.OracleParameter> parameters2 = new List<Devart.Data.Oracle.OracleParameter> ();
							parameters2.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = sysid });
							exists = ctx.ExecuteStoreQuery<int?> ("select 1 from angebot where sysid=:sysid", parameters2.ToArray ()).FirstOrDefault ();
						}
						break;
					case "PERSON":
                        exists = ctx.ExecuteStoreQuery<int?>("select 1 from person where sysperson=:sysid", parameters.ToArray()).FirstOrDefault();
                        break;
                }

                return exists.HasValue;
            }
        }

        /// <summary>
        /// get sysid by "nice" number and area
        /// </summary>
        /// <param name="area">entry type</param>
        /// <param name="number">"nice" unique number</param>
        /// <returns>sysid</returns>
        public long getSysidFromNumber(string area, string number)
        {
            if (area == null || area.Length == 0 || number == null || number.Length == 0)
                return 0;

            using (DdCtExtended ctx = new DdCtExtended())
            {
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "nr", Value = number });

                long? sysid = null;

                switch (area)
                {
                    case "VT":
                        sysid = ctx.ExecuteStoreQuery<long?>("select sysid from vt where vertrag=:nr", parameters.ToArray()).FirstOrDefault();
                        break;
                    case "ANTRAG":
                        sysid = ctx.ExecuteStoreQuery<long?>("select sysid from antrag where antrag=:nr", parameters.ToArray()).FirstOrDefault();
						if (sysid == null)	// if no ANT exists, let's look for an ANG (rh: 20170322)
						{
							// rh 20170322: Allow ZEK OK even if only an ANG exists (lt. MG) 
							List<Devart.Data.Oracle.OracleParameter> parameters2 = new List<Devart.Data.Oracle.OracleParameter> ();
							parameters2.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = "nr", Value = number });
							sysid = ctx.ExecuteStoreQuery<long?> ("select sysid from angebot where angebot=:nr", parameters2.ToArray ()).FirstOrDefault ();
						}
                        break;
					case "ANGEBOT":	// rh 20170322: Allow ZEK OK even if only an ANG exists (lt. MG) 
						sysid = ctx.ExecuteStoreQuery<long?> ("select sysid from angebot where angebot=:nr", parameters.ToArray ()).FirstOrDefault ();
						break;
					case "PERSON":
                        sysid = long.Parse(number);
                        break;
                }

                return sysid.HasValue ? sysid.Value : 0;
            }
        }

        /// <summary>
        /// returns process data
        /// </summary>
        /// <param name="sysprocess">process id</param>
        /// <returns>process data</returns>
        public virtual ProcessDto getProcess(long sysprocess)
        {
            if (sysprocess > 0)
                using (DdCtExtended ctx = new DdCtExtended())
                {
                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysprocess", Value = sysprocess });
                    ProcessDto rval = new ProcessDto();
                    rval = ctx.ExecuteStoreQuery<ProcessDto>(QUERYPROCESS, parameters.ToArray()).FirstOrDefault();
                    return rval;
                }
            else
                return null;
        }

        #endregion


        private static String QUERYDELETE = "delete from cic.{0} where sys{0} ={1}";
        public string[] allowedForDeletion = { "recurr" };
        /// <summary>
        /// Löscht eine Entity
        /// </summary>
        /// <param name="area">Area</param>
        /// <param name="sysid">Sysid</param>
        public void deleteEntity(string area, long sysid)
        {
            if (allowedForDeletion.Contains(area.ToLower()))
            {
                using (DdCtExtended ctx = new DdCtExtended())
                {
                    string query = string.Format(QUERYDELETE, area, sysid);
                    int result = ctx.ExecuteStoreCommand(query);
                }
            }
            else
            {
                throw new Exception("Es ist nicht erlaubt, aus der Area " + area.ToUpper() + " zu löschen.");
            }
        }

        /// <summary>
        /// Returns the it id for a person
        /// uses only it in peuni of current perole
        /// when multiple it's found uses the most recent
        /// </summary>
        /// <param name="sysperson"></param>
        /// <param name="peuni">true to use sightfield</param>
        /// <returns></returns>
        public long getItIdFromPerson(long sysperson, bool peuni)
        {
            if (sysperson == 0) return 0;

            using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new Cic.OpenOne.Common.Model.Prisma.PrismaExtended())
            {
                if(peuni)
                    return ctx.ExecuteStoreQuery<long>("select sysit from it where sysperson=" + sysperson + " and sysit in (SELECT sysid FROM peuni, perolecache WHERE area = 'IT' AND peuni.sysperole = perolecache.syschild AND perolecache.sysparent = " + sysPerole + ") order by sysit desc", null).FirstOrDefault();
                else
                    return ctx.ExecuteStoreQuery<long>("select sysit from it where sysperson=" + sysperson +" order by sysit desc", null).FirstOrDefault();
            }
        }

        /// <summary>
        /// Returns all active insurances for the offer
        /// </summary>
        /// <param name="sysangebot"></param>
        /// <returns></returns>
        public List<Cic.OpenOne.Common.DTO.AngAntVsDto> getAngebotVersicherung(long sysangebot)
        {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                Cic.OpenOne.Common.DAO.ITranslateDao tdao = Cic.OpenOne.Common.DAO.CommonDaoFactory.getInstance().getTranslateDao();
                List<Cic.OpenOne.Common.DTO.CTLUT_Data> translations = tdao.readoutTranslationList("'VSTYP', 'RSVTYP', 'FSTYP'", isoCode);
				// ALT rh 20170705: "select angvs.*,vstyp.BEZEICHNUNG vsTypBezeichnung,person.name vsBezeichnung from angvs, vstyp, vsart, person where person.sysperson(+)=angvs.sysvs and angvs.sysvstyp = vstyp.sysvstyp and vstyp.sysvsart = vsart.sysvsart and angvs.sysangebot =" + sysangebot).ToList();
				List<Cic.OpenOne.Common.DTO.AngAntVsDto> rval = ctx.ExecuteStoreQuery<Cic.OpenOne.Common.DTO.AngAntVsDto>(
					"select distinct vstyp.BEZEICHNUNG vsTypBezeichnung, angvs.sysvstyp, person.name vsBezeichnung from angvs, vstyp, vsart, person where person.sysperson(+)=angvs.sysvs and angvs.sysvstyp = vstyp.sysvstyp and vstyp.sysvsart = vsart.sysvsart and angvs.sysangebot =" + sysangebot).ToList ();
                foreach (Cic.OpenOne.Common.DTO.AngAntVsDto vs in rval)
                {
                    Cic.OpenOne.Common.DTO.CTLUT_Data Translation = tdao.RetrieveEntry(vs.sysvstyp, "VSTYP", translations);
                    if (Translation != null)
                    {
                        vs.vsTypBezeichnung = Translation.Bezeichnung;
                        //item.code = Translation.Name;
                        //item.beschreibung = Translation.Description;
                    }
                }
                return rval;
            }

        }

  

        /// <summary>
        /// accept EPOS Conditions of current user
        /// </summary>
        public virtual void acceptEPOSConditions()
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        public void setRiskMailContact(ref isendRiskmailDto input) {
            using (DdCtExtended ctx = new DdCtExtended())
            {
                String queryEmpfaenger = "SELECT cfgvar.wert FROM cfg,cfgsec,cfgvar WHERE UPPER(TRIM(cfg.code)) = 'MAILGATEWAY' AND cfg.syscfg = cfgsec.syscfg AND UPPER(TRIM(cfgsec.code)) = 'MAIL' AND cfgsec.syscfgsec = cfgvar.syscfgsec AND UPPER(TRIM(cfgvar.code)) = 'RISK_MAIL'";

                input.empfaenger = ctx.ExecuteStoreQuery<String>(queryEmpfaenger, null).FirstOrDefault();

                String queryAbsender = "SELECT cfgvar.wert FROM cfg,cfgsec,cfgvar WHERE UPPER(TRIM(cfg.code)) = 'MAILGATEWAY' AND cfg.syscfg = cfgsec.syscfg AND UPPER(TRIM(cfgsec.code)) = 'MAIL' AND cfgsec.syscfgsec = cfgvar.syscfgsec AND UPPER(TRIM(cfgvar.code)) = 'ABSENDER'";

                input.absender = ctx.ExecuteStoreQuery<String>(queryAbsender, null).FirstOrDefault();


            }


        }

        /// <summary>
        /// Performs the 4-eyes principle for the area/id setting the given result of the current user
        /// </summary>
        /// <param name="input"></param>
        /// <param name="output"></param>
        public void fourEyesPrinciple(ifourEyesDto input, ofourEyesDto output)
        {
            using (Cic.OpenOne.Common.Model.DdOw.DdOwExtended ctx = new Cic.OpenOne.Common.Model.DdOw.DdOwExtended())
            {
                ofourEyesDto tmpout = ctx.ExecuteStoreQuery<ofourEyesDto>("select ratingsyswer syswer1, ratingsyswer2 syswer2 from bonitaet where sysantrag = " + input.sysId + " order by 1 desc").FirstOrDefault();

                ofourEyesDto ids = ctx.ExecuteStoreQuery<ofourEyesDto>(@"select ratingposition.sysratingposition syswer1,ratingposition.sysratingergebnis syswer2 from antrag,rating,ratingergebnis,ratingposition
                        where antrag.sysid=rating.SYSID and rating.sysrating=ratingergebnis.sysrating and RATINGERGEBNIS.SYSRATINGERGEBNIS=ratingposition.sysratingergebnis and antrag.sysid="+input.sysId+" and ratingposition.syscrtuser="+input.syswfuser).FirstOrDefault();
                
                //Entscheidung aktualisieren
                if (ids!=null && ids.syswer1 > 0 && input.value>0)
                {
                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "st", Value = input.value });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "us", Value = input.syswfuser });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "antwort", Value = input.bemerkung });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = ids.syswer1 });
                    ctx.ExecuteStoreCommand("update ratingposition set status=:st, syscrtuser=:us, histantwort=:antwort where sysratingposition=:id", parameters.ToArray());
                }
                else if(input.value>0)
                {

                    long sysratingergebnis = ctx.ExecuteStoreQuery<long>(@"select ratingergebnis.sysratingergebnis from antrag,rating,ratingergebnis
                        where antrag.sysid=rating.SYSID and rating.sysrating=ratingergebnis.sysrating and antrag.sysid=" + input.sysId ).FirstOrDefault();

                    RATINGPOSITION rp = new RATINGPOSITION();
                    rp.SYSCRTUSER = input.syswfuser;
                    rp.SYSCRTDATE = DateTime.Now;
                    rp.RATINGERGEBNISReference.EntityKey = ctx.getEntityKey(typeof(RATINGERGEBNIS), sysratingergebnis);
                    rp.STATUS = (int)input.value;
                    rp.HISTANTWORT = input.bemerkung;
                    ctx.AddToRATINGPOSITION(rp);
                    ctx.SaveChanges();
                }
                if (input.value > 0 && tmpout!=null)
                {
                    if (tmpout.syswer1 == input.syswfuser || tmpout.syswer2 == input.syswfuser)//bereits hinterlegt;
                        ;
                    else if (tmpout.syswer1 == 0)
                    {
                        ctx.ExecuteStoreCommand("update bonitaet set ratingsyswer=" + input.syswfuser + " where sysantrag=" + input.sysId);
                    }
                    else if (tmpout.syswer2 == 0)
                    {
                        ctx.ExecuteStoreCommand("update bonitaet set ratingsyswer2=" + input.syswfuser + " where sysantrag=" + input.sysId);
                    }
                }

                //aktuelle Entscheidungen zurückgeben
                tmpout = ctx.ExecuteStoreQuery<ofourEyesDto>("select ratingsyswer syswer1, ratingsyswer2 syswer2 from bonitaet where sysantrag = " + input.sysId + " order by 1 desc").FirstOrDefault();
                if (tmpout != null)
                {
                    output.syswer1 = tmpout.syswer1;
                    output.syswer2 = tmpout.syswer2;
                }
				if(output.syswer1>0)
				{
					tmpout = ctx.ExecuteStoreQuery<ofourEyesDto>(@"select ratingposition.status value1, ratingposition.histantwort bemerkung1 from antrag,rating,ratingergebnis,ratingposition
                        where antrag.sysid=rating.SYSID and rating.sysrating=ratingergebnis.sysrating and RATINGERGEBNIS.SYSRATINGERGEBNIS=ratingposition.sysratingergebnis and antrag.sysid=" + input.sysId + " and ratingposition.syscrtuser=" + output.syswer1).FirstOrDefault();
						output.value1=tmpout.value1;
						output.bemerkung1=tmpout.bemerkung1;
                        output.benutzer1 = ctx.ExecuteStoreQuery<String>("select vorname||' '||name from wfuser where syswfuser=" + output.syswer1).FirstOrDefault();
				}
				if(output.syswer2>0)
				{
					tmpout = ctx.ExecuteStoreQuery<ofourEyesDto>(@"select ratingposition.status value1, ratingposition.histantwort bemerkung1 from antrag,rating,ratingergebnis,ratingposition
                        where antrag.sysid=rating.SYSID and rating.sysrating=ratingergebnis.sysrating and RATINGERGEBNIS.SYSRATINGERGEBNIS=ratingposition.sysratingergebnis and antrag.sysid=" + input.sysId + " and ratingposition.syscrtuser=" + output.syswer2).FirstOrDefault();
						output.value2=tmpout.value1;
						output.bemerkung2=tmpout.bemerkung1;
                        output.benutzer2 = ctx.ExecuteStoreQuery<String>("select vorname||' '||name from wfuser where syswfuser=" + output.syswer2).FirstOrDefault();
				}

				//both same answer, write back to antrag when something was written
				if(output.value1==output.value2 && output.syswer1>0&&output.syswer2>0&& input.value>0)
				{
					String nstatus = ctx.ExecuteStoreQuery<String>("select value from ddlkppos where code='ENTSCHEIDUNG' and id="+input.value).FirstOrDefault();
					List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();

                    String bem = output.bemerkung1 + " / " + output.bemerkung2;
                    if (bem!=null && bem.Length > 180)
                        bem = bem.Substring(0, 180);
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "st", Value = nstatus });
					parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = input.sysId });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "p2", Value = bem });
                    ctx.ExecuteStoreCommand("update antrag set attribut=:st,BESCHRDEUTSCH=:p2 where sysid=:id", parameters.ToArray());
				}
				

            }
        }
    }
}
