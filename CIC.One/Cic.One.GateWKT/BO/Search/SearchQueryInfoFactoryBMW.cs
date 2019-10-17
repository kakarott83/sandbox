using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.One.Web.BO.Search;
using Cic.OpenOne.Common.Util.Config;

namespace Cic.One.GateWKT.BO.Search
{
    /// <summary>
    /// Overrides the SearchQuery Factory for BMW
    /// 
    /// </summary>
    public class SearchQueryInfoFactoryBMW : SearchQueryInfoFactory
    {
        public override QueryInfoData getQueryInfo<T>()
        {
            QueryInfoData infoData = null;
            if (typeof(T).Equals(typeof(Cic.One.DTO.OppotaskDto)))
            {
                infoData = new QueryInfoDataType1("OPPOTASK", "OPPOTASK.SYSOPPOTASK");
                infoData.resultFields = "OPPOTASK.*,case when crt.syswfuser is null then 'Automatisch generiert' else trim(crt.name)||' '||trim(crt.vorname) end crtuserName, trim(wf.name)||' '||trim(wf.vorname) wfuserName, vt.vertrag gebietName, trim(kd.name)||' '||trim(kd.vorname) kdName,trim(vk.name)||' '||trim(vk.vorname) vkName, trim(hd.name)||' '||trim(hd.vorname) hdName,gebiete.gebiet hdOrt, (select iamtype.code from iamtype, oppo, iam where oppo.sysiam=iam.sysiam and iam.sysiamtype=iamtype.sysiamtype and oppo.sysoppo=oppotask.sysoppo) oppoiamcode  ";
                infoData.resultTables = "vt, person kd, person vk, person hd,OPPOTASK left outer join wfuser wf on wf.syswfuser=oppotask.syswfuser left outer join wfuser crt on crt.syswfuser=oppotask.syscrtuser left outer join oppo on oppo.sysoppo=oppotask.sysoppo left outer join camp on  camp.syscamp=oppo.syscamp,gebiete_v gebiete,iam,iamtype ";
                infoData.searchTables = "vt, person kd, person vk, person hd,OPPOTASK left outer join wfuser wf on wf.syswfuser=oppotask.syswfuser left outer join wfuser crt on crt.syswfuser=oppotask.syscrtuser left outer join oppo on oppo.sysoppo=oppotask.sysoppo left outer join camp on  camp.syscamp=oppo.syscamp,gebiete_v gebiete,iam,iamtype ";
                infoData.searchConditions = " kd.sysperson=vt.syskd and vk.sysperson=vt.sysberataddb and hd.sysperson=vt.sysvpfil and vt.sysid=OPPOTASK.sysid and OPPOTASK.area='VT'  and gebiete.sysperson=vt.sysvpfil and oppo.sysoppo=oppotask.sysoppo and oppo.sysiam=iam.sysiam and iamtype.sysiamtype = iam.sysiamtype";
                infoData.permissionCondition = " and vt.sysid in (SELECT sysid FROM peuni, perolecache WHERE area = 'VT' AND peuni.sysperole = perolecache.syschild AND perolecache.sysparent = {0} and {1}) ";
                infoData.optimized = false;
            }
            else if (typeof(T).Equals(typeof(Cic.One.DTO.OpportunityDto)))
            {
                infoData = new QueryInfoDataType1("OPPO", "OPPO.SYSOPPO");
                infoData.resultFields = "OPPO.*, iamtype.code iamcode ,trim(wf.name)||' '||trim(wf.vorname) wfuserName, vt.vertrag gebietName,trim(vk.name)||' '||trim(vk.vorname) vkName, trim(hd.name)||' '||trim(hd.vorname) hdName,vt.ENDE,vt.vart, obini.erstzul, trim(ob.hersteller) marke, trim(ob.objektvt) modell,trim(ob.kennzeichen) kennzeichen,trim(ob.serie) serie,camp.name||(select camp.name from oppo op,camp where op.syscamp=camp.syscamp and op.area=oppo.area and op.sysid=vt.sysid and op.sysiam!=oppo.sysiam and  camp.status>1 and 3>camp.status and rownum=1)  campName,(select max(oppotask.phase) from oppotask where oppotask.sysoppo=oppo.sysoppo) worstActivityStatus,(select min(oppotask.status) from oppotask where oppotask.sysoppo=oppo.sysoppo) bestActivityStatus,gebiete.gebiet hdOrt,kd.privatflag kundeprivat,vt.beginn vtbeginn,vt.ende vtende,trim(kd.titel) kundetitel, trim(kd.rechtsform) kdart, trim(ob.fzart) fzart, trim(ob.fahrer) fahrer, trim(lsadd.mandant) mandant, trim(vt.vertriebsweg) vertriebsweg,trim(ob.schwacke) schwacke,trim(hd.code) hdCode,trim(vt.konstellation) konstellation, (select sklasse from schwacke where schwacke.schwacke=ob.schwacke) baureihe ";
                infoData.resultTables = "CIC.OPPO OPPO  left outer join camp on  camp.syscamp=oppo.syscamp left outer join wfuser wf on wf.syswfuser=oppo.syswfuser , vt, person kd, person vk, person hd, ob,obini,gebiete_v gebiete, lsadd,iam,iamtype ";
                infoData.searchTables = "CIC.OPPO OPPO left outer join camp on  camp.syscamp=oppo.syscamp left outer join wfuser wf on wf.syswfuser=oppo.syswfuser , vt, person kd, person vk, person hd, ob,obini,gebiete_v gebiete, lsadd,iam,iamtype ";
                infoData.searchConditions = " kd.sysperson=vt.syskd and vk.sysperson=vt.sysberataddb and hd.sysperson=vt.sysvpfil and vt.sysid=oppo.sysid and oppo.area='VT' and ob.sysvt=vt.sysid and obini.sysobini=ob.sysob and gebiete.sysperson=vt.sysvpfil and lsadd.syslsadd=vt.sysls and oppo.sysiam=iam.sysiam and iamtype.sysiamtype = iam.sysiamtype";
                infoData.permissionCondition = " and vt.sysid in (SELECT sysid FROM peuni, perolecache WHERE area = 'VT' AND peuni.sysperole = perolecache.syschild AND perolecache.sysparent = {0} and {1}) ";
                infoData.optimized = false;

            }
            else if (typeof(T).Equals(typeof(Cic.One.DTO.IefDto)))
            {
                infoData = new QueryInfoDataType1("VT", "VT.SYSID");
                infoData.resultFields = "vt.sysid sysvt, vt.vertrag vertrag, trim(kd.name)||' '||trim(kd.vorname) kdName,trim(vk.name)||' '||trim(vk.vorname) vkName, trim(hd.name)||' '||trim(hd.vorname) hdName,gebiete.gebiet hdOrt,vt.ENDE,vt.vart,  trim(ob.hersteller) marke, trim(ob.objektvt) modell,trim(ob.serie) serie, trim(vt.konstellation) konstellation ,obini.erstzul,vtobligo.risikogr8 iediffp,kalk.sz+kalk.anzahlung sz, kalk.rate,kalk.depot, kalk.rw, vtobligo.RISIKOGR2 eurotaxblau, vtobligo.risikogr1 bonusff, risikogr4 aufloesewert, risikogr5 aufloesewert1, risikogr6 aufloesewert2, risikogr9 iediff";
                infoData.resultTables = "vt, person kd, person vk, person hd, ob,vtobligo,kalk,obini,gebiete_v gebiete ";
                infoData.searchTables = "vt, person kd, person vk, person hd, ob,vtobligo,kalk,obini,gebiete_v gebiete ";
                infoData.searchConditions = "kd.sysperson=vt.syskd and vk.sysperson=vt.sysberataddb and hd.sysperson=vt.sysvpfil and ob.sysvt=vt.sysid and vtobligo.sysvtobligo=vt.sysid and kalk.sysob=ob.sysob and obini.sysobini=ob.sysob  and gebiete.sysperson=vt.sysvpfil and vt.zustand in ('VOR_ENDE_KUNDE','AKTIV','VOR_ENDE_HÄNDLER') and ((kalk.syskalktyp=42 and add_months(sysdate,12)<vt.ende) or kalk.syskalktyp!=42)";
                infoData.permissionCondition = " and vt.sysid in (SELECT sysid FROM peuni, perolecache WHERE area = 'VT' AND peuni.sysperole = perolecache.syschild AND perolecache.sysparent = {0} and {1}) ";
                String filter = AppConfig.Instance.getValueFromDb("AIDA", "FILTERS", "VT");
                if (filter != null && filter.Length > 0)
                {
                    infoData.searchConditions += " AND " + filter;
                }
                infoData.optimized = false;

            }
            else if (typeof(T).Equals(typeof(Cic.One.DTO.CampDto)))
            {
                infoData = new QueryInfoDataType1("CAMP", "CAMP.SYSCAMP");
                infoData.resultFields = "camp.*,(select count(*) from oppo o where o.syscamp=camp.syscamp) children, camptp.name sysCampTpBezeichnung ";
                infoData.resultTables = "CIC.CAMP CAMP, CIC.CAMPTP CAMPTP ";
                infoData.searchTables = "CIC.CAMP CAMP, CIC.CAMPTP CAMPTP  ";
                infoData.searchConditions = " CAMP.SYSCAMPTP = CAMPTP.SYSCAMPTP(+) ";
                infoData.optimized = false;

            }
            else if (typeof(T).Equals(typeof(Cic.One.DTO.VertragToCampDto)))
            {
                infoData = new QueryInfoDataType1("VT", "VT.SYSID");
                infoData.resultFields = "(select sysoppo from oppo where oppo.sysid = vt.sysid and oppo.syscamp=camp.syscamp) assigned,kd.sysperson syspersonkd, VT.SYSID, trim(KD.TELEFON) telefon, trim(KD.PTELEFON) ptelefon, trim(KD.HANDY) handy, trim(KD.HANDY2) handy2, trim(KD.EMAIL) email, trim(KD.EMAIL2) email2,trim(kd.vorname) kdvorname, trim(KD.NAME) kdname, trim(kd.strasse||' '||kd.hsnr) kdstrasse, trim(kd.ort) kdort, trim(kd.plz) kdplz, camp.validfrom, camp.validuntil, camp.name, camp.description,(select puser.syswfuser from puser, person where person.sysperson=vt.sysberataddb and puser.syspuser=person.syspuser) syswfuser ";
                infoData.resultTables = "CIC.VT VT, CIC.PERSON KD, CIC.OB OB, CIC.KALK KALK, CIC.OBINI OBINI,CIC.PERSON HD,vtobligo, CIC.KALKFS KALKFS,schwacke,camp ";
                infoData.searchTables = "CIC.VT VT, CIC.PERSON KD, CIC.OB OB, CIC.KALK KALK, CIC.OBINI OBINI,CIC.PERSON HD,vtobligo, CIC.KALKFS KALKFS,schwacke,camp ";
                infoData.searchConditions = "  VT.SYSKD          =KD.SYSPERSON(+) AND VT.SYSID            =OB.SYSVT AND OB.SYSOB            =KALK.SYSOB AND OB.SYSOB=OBINI.SYSOBINI AND vt.sysvpfil = hd.sysperson(+) AND vt.sysid =vtobligo.sysvtobligo(+) AND kalk.syskalk = kalkfs.syskalkfs(+) and schwacke.schwacke=ob.schwacke and vt.zustand in ('VOR_ENDE_KUNDE','AKTIV','VOR_ENDE_HÄNDLER')";
                infoData.permissionCondition = " and vt.sysid in (SELECT sysid FROM peuni, perolecache WHERE area = 'VT' AND peuni.sysperole = perolecache.syschild AND perolecache.sysparent = {0} and {1}) ";
                String filter = AppConfig.Instance.getValueFromDb("AIDA", "FILTERS", "VT");
                if (filter != null && filter.Length > 0)
                {
                    infoData.searchConditions += " AND " + filter;
                }
                infoData.optimized = false;
            }
            else if (typeof(T).Equals(typeof(Cic.One.DTO.VertragDto)))
            {
                infoData = new QueryInfoDataType1("VT", "VT.SYSID");
                infoData.resultFields = "VT.*, KD.NAME|| ', ' ||KD.VORNAME kundeName, ob.hersteller marke, ob.objektvt modell";
                infoData.resultTables = "CIC.VT VT, CIC.PERSON KD, CIC.OB OB, CIC.KALK KALK, CIC.OBINI OBINI,CIC.PERSON HD,vtobligo, CIC.KALKFS KALKFS, schwacke ";
                infoData.searchTables = "CIC.VT VT, CIC.PERSON KD, CIC.OB OB, CIC.KALK KALK, CIC.OBINI OBINI,CIC.PERSON HD,vtobligo, CIC.KALKFS KALKFS, schwacke ";
                infoData.searchConditions = " VT.SYSKD          =KD.SYSPERSON(+) AND VT.SYSID            =OB.SYSVT AND OB.SYSOB            =KALK.SYSOB AND OB.SYSOB=OBINI.SYSOBINI AND vt.sysvpfil = hd.sysperson(+) AND vt.sysid =vtobligo.sysvtobligo(+) AND kalk.syskalk = kalkfs.syskalkfs(+) AND schwacke.schwacke   =ob.schwacke and vt.zustand in ('VOR_ENDE_KUNDE','AKTIV','VOR_ENDE_HÄNDLER')";

                String filter = AppConfig.Instance.getValueFromDb("AIDA", "FILTERS", "VT");
                if (filter != null && filter.Length > 0)
                {
                    infoData.searchConditions += " AND " + filter;
                }
                infoData.permissionCondition = " and vt.sysid in (SELECT sysid FROM peuni, perolecache WHERE area = 'VT' AND peuni.sysperole = perolecache.syschild AND perolecache.sysparent = {0} and {1}) ";
                infoData.optimized = false;
            }
            else
            {
                return base.getQueryInfo<T>();
            }
            return infoData;
        }
    }
}
