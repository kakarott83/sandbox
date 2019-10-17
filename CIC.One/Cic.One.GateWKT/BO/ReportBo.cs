using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Cic.One.DTO;
using Cic.One.Web.BO.Search;
using Cic.OpenOne.Common.Util.Config;
using Cic.OpenOne.Common.Util.Security;
using System.Text;
using Cic.One.Web.BO;
using Cic.OpenOne.Common.DTO;

namespace Cic.One.GateWKT.BO
{
    public class ReportBo : Cic.One.Web.BO.ReportBo
    {
        /// <summary>
        /// Returns a list of results for the defined Report
        /// </summary>
        /// <param name="input"></param>
        /// <param name="ctx"></param>
        /// <returns></returns>
        override
        public oSearchDto<ReportDto> getReportData(iSearchDto input, CredentialContext ctx)
        {
            oSearchDto<ReportDto> rval;
            input.searchType = SearchType.Complete;
            input.sortFields = null;//no sorting, need speed

            //handle special aida filters
            String filterOne = "";
            String filterThree = "";
            String filterFour = "";
            Filter[] orgfilters = input.filters;
            if (orgfilters != null && orgfilters.Length > 0)
            {
                Filter[] peroleids = (from t in orgfilters
                                      where t.fieldname.Equals("sysperole")
                                      select t).ToArray();
                Filter[] markenv = (from t in orgfilters
                                    where (t.groupName != null && t.groupName.Equals("mknv"))
                                    select t).ToArray();
                Filter[] otherFilters = (from t in orgfilters
                                         where !t.fieldname.Equals("sysperole") && (t.groupName == null || !t.groupName.Equals("mknv"))
                                         select t).ToArray();
                /*if (iSearch.queryId.Equals("FWA"))
                {
                    //Remove baureihe filter for fwa
                    otherFilters = (from t in otherFilters
                                    where (t.groupName == null || !t.groupName.Equals("bror"))
                                    select t).ToArray();
                }*/
                input.filters = otherFilters;
                StringBuilder peroleFilter = new StringBuilder();
                StringBuilder markenvFilter = new StringBuilder();
                foreach (Filter f in peroleids)
                {
                    if (f.values == null || f.values.Length == 0) continue;
                    if (peroleFilter.Length > 0)
                        peroleFilter.Append(",");
                    peroleFilter.Append(String.Join(",", f.values));
                }
                if (peroleFilter.Length > 0)
                {
                    filterOne = " and  vt.sysid in (SELECT sysid FROM peuni, perolecache WHERE area = 'VT' AND peuni.sysperole = perolecache.syschild AND perolecache.sysparent in (" + peroleFilter.ToString() + ")) ";
                    filterThree = " and  vt2.sysid in (SELECT sysid FROM peuni, perolecache WHERE area = 'VT' AND peuni.sysperole = perolecache.syschild AND perolecache.sysparent in (" + peroleFilter.ToString() + ")) ";
                }
                else//wenn keine perole-einschränkung auf jeden Fall die des Users verwenden!
                {
                    filterOne = " and  vt.sysid in (SELECT sysid FROM peuni, perolecache WHERE area = 'VT' AND peuni.sysperole = perolecache.syschild AND perolecache.sysparent in (" + ctx.getMembershipInfo().sysPEROLE + ")) ";
                    filterThree = " and  vt2.sysid in (SELECT sysid FROM peuni, perolecache WHERE area = 'VT' AND peuni.sysperole = perolecache.syschild AND perolecache.sysparent in (" + ctx.getMembershipInfo().sysPEROLE + ")) ";
                }
                foreach (Filter f in markenv)
                {
                    if (f.values == null || f.values.Length == 0) continue;

                    if (markenvFilter.Length > 0)
                        markenvFilter.Append(",");
                    markenvFilter.Append(String.Join("','", f.values));
                }
                if (markenvFilter.Length > 0)
                {
                    filterFour =// " and ((select max(upper(bezeichnung)) from obtyp where level = 4 connect by prior sysobtypp=sysobtyp start with obtyp.schwacke=ob2.schwacke) in ('" + markenvFilter.ToString().ToUpper() + "') or trim(ob2.hersteller) in ('" + markenvFilter.ToString().ToUpper() + "'))";
                        " and trim(ob2.hersteller) in ('" + markenvFilter.ToString()+ "')";
                }
            }

            String aidaFilter = " AND (VT.VERTRIEBSWEG NOT LIKE '%Fleet%' and vt.sysls NOT LIKE 3 and vt.sysls NOT LIKE 4 and vt.SYSVART < 600 and vt.DATAKTIV > to_date('01.01.1800','DD.MM.YYYY') ) ";


            
                reports["RRCSC"] = getReportById("RRCSC");
                if (reports["RRCSC"] == null)
                    reports["RRCSC"] =//sum(ret) as y5, sum(csc) as y4 ,count(*) as y3,
                        @"select ende x1, sum(ret)/count(*)*100 as y2, sum(csc)/count(*)*100 as y1 from (

select
      to_char(VT.ENDE,'MON yyyy', 'NLS_DATE_LANGUAGE=GERMAN') ende,
      to_char(VT.ENDE,'yyyy-mm') endesort,
    
    (
        SELECT least(COUNT(*),1)  FROM VT VT2,OB OB2 WHERE   VT2.SYSKD = VT.SYSKD AND  vt2.sysid!=vt.sysid and
    VT2.SYSID = OB2.SYSVT
    and TRIM(OB.SERIE) != TRIM(OB2.SERIE)
    and ( ADD_MONTHS(VT.ENDE,-6) <= VT2.BEGINN AND ADD_MONTHS(VT.ENDE,3) >= VT2.BEGINN) 
    AND VT2.LZ >11
    AND (VT2.VERTRIEBSWEG NOT LIKE '%Fleet%' and vt2.sysls NOT LIKE 3 and vt2.sysls NOT LIKE 4 and vt2.SYSVART < 600 and vt2.DATAKTIV > to_date('01.01.1800','DD.MM.YYYY') )
     and vt2.sysvpfil not in (151737,64340,83112,82242,176047) and vt2.syskd not in (151737,64340,83112,82242,176047)
     
       {3}
        {4}
  )CSC,
(
        SELECT least(COUNT(*),1)  FROM VT VT2,OB OB2 WHERE   VT2.SYSKD = VT.SYSKD AND  vt2.sysid!=vt.sysid and
    VT2.SYSID = OB2.SYSVT
    and ( ADD_MONTHS(VT.ENDE,-6) <= VT2.BEGINN AND ADD_MONTHS(VT.ENDE,3) >= VT2.BEGINN) 
    AND VT2.LZ >11
    AND (VT2.VERTRIEBSWEG NOT LIKE '%Fleet%' and vt2.sysls NOT LIKE 3 and vt2.sysls NOT LIKE 4 and vt2.SYSVART < 600 and vt2.DATAKTIV > to_date('01.01.1800','DD.MM.YYYY') )
    and vt2.sysvpfil not in (151737,64340,83112,82242,176047) and vt2.syskd not in (151737,64340,83112,82242,176047)
     
       {3}
        {4}
  )RET
from
   VT left outer join oppo on vt.sysid=oppo.sysid and oppo.area='VT',PERSON,OB left outer join SCHWACKE on ob.schwacke=schwacke.schwacke, PERSON HD, PERSON VK
where {0} and 
  VT.SYSKD = PERSON.SYSPERSON AND
  VT.SYSID = OB.SYSVT AND
  hd.sysperson=vt.sysvpfil and
  vk.sysperson=vt.sysberatadda
  and vt.sysvpfil not in (151737,64340,83112,82242,176047) and vt.syskd not in (151737,64340,83112,82242,176047)
  and vt.zustand in ('NORMALABLAUF','VORZEITIGER ABLAUF')
  {1}
 {2}
     

) group by ende,endesort order by endesort";



                reports["RRCSC"] = String.Format(reports["RRCSC"], new object[] { "{0}", filterOne, aidaFilter, filterThree, filterFour });



                reports["FWA"] = getReportById("FWA");
                if (reports["FWA"] == null)
                    reports["FWA"] = @"select baureihe x1, round(count(baureihe)/sum(count(baureihe)) over () *100,2) y1  from 
(
select case when trim(hersteller)='BMW' then
          case when upper(baureihe) like '%MINI%' then 'MINI'
          when baureihe in ('BMW','Car','Peugeot') then 'Fremdmarke'
          when baureihe like '3er%' then '3er'
          when baureihe like '3%' then '3er' 
          when baureihe like '5%' then '5er' 
          when baureihe like '5er%' then '5er'
          when baureihe like '% Reihe%' then substr(baureihe,0,2) 
          when baureihe like '%-Reihe%' then substr(baureihe,0,instr(baureihe,'-Reihe')-1) 
          when baureihe is null then 'Fremdmarke'
          when trim(baureihe) ='Zweiräder' then 'BMW Mot'
         else baureihe end 
     else 'Fremdmarke' end baureihe  from (
select
      case when trim(ob2.hersteller)='BMW' then 
         case when  (select trim(max(bezeichnung)) from obtyp where level = 3 connect by prior sysobtypp=sysobtyp start with obtyp.schwacke=trim(ob2.schwacke)) is not null
         then (select trim(max(bezeichnung)) from obtyp where level = 3 connect by prior sysobtypp=sysobtyp start with obtyp.schwacke=trim(ob2.schwacke))
         else (select ecode from schwacke where schwacke=ob2.schwacke) end
      else 'Fremdmarke' end baureihe
      ,ob2.hersteller hersteller
      --,ob2.schwacke||' '||ob2.objektvt||' '||ob2.hersteller info
      
from
   VT left outer join oppo on vt.sysid=oppo.sysid and oppo.area='VT',PERSON,OB left outer join SCHWACKE on ob.schwacke=schwacke.schwacke, PERSON HD, PERSON VK, VT VT2,OB OB2
where {0}  and 
  VT2.SYSKD = VT.SYSKD AND
  VT2.SYSID = OB2.SYSVT AND
  trim(OB2.SERIE)!=trim(OB.SERIE) and 
 ( ADD_MONTHS(VT.ENDE,-6) <= VT2.BEGINN AND ADD_MONTHS(VT.ENDE,3) >= VT2.BEGINN) and

  VT.SYSKD = PERSON.SYSPERSON AND
  VT.SYSID = OB.SYSVT AND
  hd.sysperson=vt.sysvpfil and
  vk.sysperson=vt.sysberatadda
  and vt.vertrag not like '%V%'
  and vt.sysvpfil not in (151737,64340,83112,82242,176047) and vt.syskd not in (151737,64340,83112,82242,176047)
  and vt2.sysvpfil not in (151737,64340,83112,82242,176047) and vt2.syskd not in (151737,64340,83112,82242,176047)
  and vt.zustand in ('NORMALABLAUF','VORZEITIGER ABLAUF')
  AND (VT2.VERTRIEBSWEG NOT LIKE '%Fleet%' and vt2.sysls NOT LIKE 3 and vt2.sysls NOT LIKE 4 and vt2.SYSVART < 600 and vt2.DATAKTIV > to_date('01.01.1800','DD.MM.YYYY') ) 
  AND (VT.VERTRIEBSWEG NOT LIKE '%Fleet%' and vt.sysls NOT LIKE 3 and vt.sysls NOT LIKE 4 and vt.SYSVART < 600 and vt.DATAKTIV > to_date('01.01.1800','DD.MM.YYYY') ) 
  {1}
{3}
{4}
 
     
)

) group by baureihe order by round(count(baureihe)/sum(count(baureihe)) over () *100,2) desc";
                reports["FWA"] = String.Format(reports["FWA"], new object[] { "{0}", filterOne, "", filterThree, filterFour });



                reports["HNDLOY"] = getReportById("HNDLOY");
                if (reports["HNDLOY"] == null)
                    reports["HNDLOY"] = @"select 'Gleicher Händler %' x1,case when (sum(sh)+sum(dh))=0 then 0 else round(sum(sh)/(sum(sh)+sum(dh))*100 ,2) end y1 from (
select least(SHST,1)  sh,  least(DHST,1)  dh from 
(

select
    (
        SELECT COUNT(*)  FROM VT VT2,OB OB2 WHERE   VT2.SYSKD = VT.SYSKD AND vt2.sysid!=vt.sysid
    and VT2.SYSID = OB2.SYSVT
    and VT2.sysvpfil = VT.sysvpfil 
    and trim(OB2.SERIE)!=trim(OB.SERIE)
   
    and ( ADD_MONTHS(VT.ENDE,-6) <= VT2.BEGINN AND ADD_MONTHS(VT.ENDE,3) >= VT2.BEGINN) 
              AND (VT2.VERTRIEBSWEG NOT LIKE '%Fleet%' and vt2.sysls NOT LIKE 3 and vt2.sysls NOT LIKE 4 and vt2.SYSVART < 600 and vt2.DATAKTIV > to_date('01.01.1800','DD.MM.YYYY') ) 
    and vt2.sysvpfil not in (151737,64340,83112,82242,176047) and vt2.syskd not in (151737,64340,83112,82242,176047)
       {3} {4}
  )SHST,
    (
        SELECT COUNT(*)  FROM VT VT2,OB OB2 WHERE   VT2.SYSKD = VT.SYSKD AND  vt2.sysid!=vt.sysid
    and VT2.SYSID = OB2.SYSVT
    and VT2.sysvpfil != VT.sysvpfil 
    and trim(OB2.SERIE)!=trim(OB.SERIE) 
   
     and ( ADD_MONTHS(VT.ENDE,-6) <= VT2.BEGINN AND ADD_MONTHS(VT.ENDE,3) >= VT2.BEGINN) 
        AND (VT2.VERTRIEBSWEG NOT LIKE '%Fleet%' and vt2.sysls NOT LIKE 3 and vt2.sysls NOT LIKE 4 and vt2.SYSVART < 600 and vt2.DATAKTIV > to_date('01.01.1800','DD.MM.YYYY') ) 
        and vt2.sysvpfil not in (151737,64340,83112,82242,176047) and vt2.syskd not in (151737,64340,83112,82242,176047)
      {3} {4}
  )DHST
from
   VT left outer join oppo on vt.sysid=oppo.sysid and oppo.area='VT',PERSON,OB left outer join SCHWACKE on ob.schwacke=schwacke.schwacke, PERSON HD, PERSON VK
where {0} and 
  VT.SYSKD = PERSON.SYSPERSON AND
  VT.SYSID = OB.SYSVT AND
  hd.sysperson=vt.sysvpfil and
  vk.sysperson=vt.sysberatadda
  and vt.zustand in ('NORMALABLAUF','VORZEITIGER ABLAUF')
  and vt.sysvpfil not in (151737,64340,83112,82242,176047) and vt.syskd not in (151737,64340,83112,82242,176047)
  {1}
  AND (VT.VERTRIEBSWEG NOT LIKE '%Fleet%' and vt.sysls NOT LIKE 3 and vt.sysls NOT LIKE 4 and vt.SYSVART < 600 and vt.DATAKTIV > to_date('01.01.1800','DD.MM.YYYY') ) 
     
)

)

union all

select 'Neuer Händler %' x1,case when (sum(sh)+sum(dh))=0 then 0 else round(sum(dh)/(sum(sh)+sum(dh))*100 ,2) end y1 from (
select least(SHST,1)  sh, least(DHST,1)  dh from 
(

select
    (
        SELECT COUNT(*)  FROM VT VT2,OB OB2 WHERE   VT2.SYSKD = VT.SYSKD AND  vt2.sysid!=vt.sysid 
      and VT2.SYSID = OB2.SYSVT
    and VT2.sysvpfil = VT.sysvpfil 
    and trim(OB2.SERIE)!=trim(OB.SERIE) 
     and ( ADD_MONTHS(VT.ENDE,-6) <= VT2.BEGINN AND ADD_MONTHS(VT.ENDE,3) >= VT2.BEGINN) 
      AND (VT2.VERTRIEBSWEG NOT LIKE '%Fleet%' and vt2.sysls NOT LIKE 3 and vt2.sysls NOT LIKE 4 and vt2.SYSVART < 600 and vt2.DATAKTIV > to_date('01.01.1800','DD.MM.YYYY') ) 
    and vt2.sysvpfil not in (151737,64340,83112,82242,176047) and vt2.syskd not in (151737,64340,83112,82242,176047)
      {3}  {4}
  )SHST,
    (
        SELECT COUNT(*)  FROM VT VT2,OB OB2 WHERE   VT2.SYSKD = VT.SYSKD AND  vt2.sysid!=vt.sysid
    and VT2.SYSID = OB2.SYSVT
    and VT2.sysvpfil != VT.sysvpfil 
    and trim(OB2.SERIE)!=trim(OB.SERIE) 
     and ( ADD_MONTHS(VT.ENDE,-6) <= VT2.BEGINN AND ADD_MONTHS(VT.ENDE,3) >= VT2.BEGINN) 
      AND (VT2.VERTRIEBSWEG NOT LIKE '%Fleet%' and vt2.sysls NOT LIKE 3 and vt2.sysls NOT LIKE 4 and vt2.SYSVART < 600 and vt2.DATAKTIV > to_date('01.01.1800','DD.MM.YYYY') ) 
        and vt2.sysvpfil not in (151737,64340,83112,82242,176047) and vt2.syskd not in (151737,64340,83112,82242,176047)
      {3}  {4}
  )DHST
from
   VT left outer join oppo on vt.sysid=oppo.sysid and oppo.area='VT',PERSON,OB left outer join SCHWACKE on ob.schwacke=schwacke.schwacke, PERSON HD, PERSON VK
where {0}  and 
  VT.SYSKD = PERSON.SYSPERSON AND
  VT.SYSID = OB.SYSVT AND
  hd.sysperson=vt.sysvpfil and
  vk.sysperson=vt.sysberatadda
  and vt.zustand in ('NORMALABLAUF','VORZEITIGER ABLAUF')
  and vt.sysvpfil not in (151737,64340,83112,82242,176047) and vt.syskd not in (151737,64340,83112,82242,176047)
  {1}
  AND (VT.VERTRIEBSWEG NOT LIKE '%Fleet%' and vt.sysls NOT LIKE 3 and vt.sysls NOT LIKE 4 and vt.SYSVART < 600 and vt.DATAKTIV > to_date('01.01.1800','DD.MM.YYYY') ) 
     
)

)

";
                reports["HNDLOY"] = String.Format(reports["HNDLOY"], new object[] { "{0}", filterOne, aidaFilter, filterThree, filterFour });




                reports["CAMP"] = getReportById("CAMP");
                if (reports["CAMP"] == null)
                    reports["CAMP"] = @"SELECT camp.name x1,(select value from ddlkppos where code='OPPO_STATUS' and id=oppo.status) x2,
  COUNT(oppo.status) y2,
  (select value from ddlkppos where code='OPPO_AGRESULT' and id=oppo.extresultat) x3,
  COUNT(oppo.extresultat) y3,
  (select value from ddlkppos where code='OPPO_RESULT' and id=oppo.resultat) x4,
  COUNT(oppo.resultat) y4
FROM oppo,
  iam,
  iamtype,camp, vt
WHERE {0} and vt.sysid=oppo.sysid and oppo.area='VT' and camp.syscamp=oppo.syscamp and oppo.sysiam =iam.sysiam
AND iam.sysiamtype=iamtype.sysiamtype
AND iamtype.code  ='CAMP'
AND camp.status in (2,3)
{1}
{2}
GROUP BY camp.name, oppo.status,
  oppo.extresultat,
  oppo.resultat order by camp.name, oppo.status,oppo.extresultat,oppo.resultat";
                reports["CAMP"] = String.Format(reports["CAMP"], new object[] { "{0}", filterOne, aidaFilter });

            




            if (!reports.ContainsKey(input.queryId))
            {
                throw new ArgumentException("Report Id not supported");
            }
            //this query must incorporate permisson-fetching if necessary!
            QueryInfoData infoData = new QueryInfoDataType3(reports[input.queryId]);
            infoData.optimized = false;

            SearchBo<ReportDto> s = new SearchBo<ReportDto>(infoData);
            rval = s.search(input);
            rval.searchCountMax = rval.searchCountFiltered;
            return rval;
        }
    }
}