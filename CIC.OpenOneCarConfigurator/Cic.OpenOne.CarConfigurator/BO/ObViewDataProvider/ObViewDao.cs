using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Cic.P000001.Common;
using Cic.OpenOne.Common.Model.DdOl;
using System.Data.Objects;
using Cic.OpenOne.Common.Util.Config;
using Cic.OpenOne.Common.Util.Logging;
using System.Reflection;
using Cic.OpenOne.CarConfigurator.DAO;

namespace Cic.OpenOne.CarConfigurator.BO.ObViewDataProvider
{
    public enum CUSTOMER_CODE
    {
        BMW=100,
        BankNOW=200,
        HCE=300

        /// 100 = BMW
        /// 200 = BankNOW
        /// 300 = Hyundai - HEK Enabled in obviewDao
    }

    class ObViewDao : IObViewDao
    {
        /* Level=Ebene
         0=VehicleType 
                1=BrandCode
                    2=GroupCode
                        3=ModelCode
                            4=NatCode*/
        private static string QUERYVIEWSEARCH = " from vc_obtyp1, vc_obtyp2, vc_obtyp3, vc_obtyp4, vc_obtyp5 left outer join obtypmap on obtypmap.sysobtyp=vc_obtyp5.id5 and obtypmap.art is null where vc_obtyp1.id1=vc_obtyp2.id1 and vc_obtyp2.id2=vc_obtyp3.id2 and vc_obtyp3.id3=vc_obtyp4.id3 and vc_obtyp4.id4=vc_obtyp5.id4 ";
        private static string QUERYVIEWSEARCHHEK = " from vc_obtyp1, vc_obtyp2, vc_obtyp3, vc_obtyp4, vc_obtyp5 left outer join obtypmap on obtypmap.sysobtyp=vc_obtyp5.id5 and obtypmap.art is null, nkk, ob,rvt,person,perole where vc_obtyp5.id5=ob.sysobtyp and ob.sysnkk=nkk.sysnkk and (nkk.zustand='aktiv' or nkk.zustand='gebucht') and nkk.sysrvt=rvt.sysrvt and rvt.sysperson=person.sysperson and vc_obtyp1.id1=vc_obtyp2.id1 and vc_obtyp2.id2=vc_obtyp3.id2 and vc_obtyp3.id3=vc_obtyp4.id3 and vc_obtyp4.id4=vc_obtyp5.id4  and PERSON.SYSPERSON=PEROLE.SYSPERSON  and perole.sysperole in (select sysperole from perole,roletype where roletype.code='HD' and roletype.sysroletype=perole.sysroletype connect by prior perole.sysparent=perole.sysperole start with perole.sysperole=:sysperole)";
        //typen
        private static string QUERYVIEWPREFIXLEVEL0 = "select vc_obtyp1.*, id1 id from vc_obtyp1 where id1 in (select distinct vc_obtyp1.id1 ";
        //marken
        private static string QUERYVIEWPREFIXLEVEL1 = "select vc_obtyp2.*, id2 id from vc_obtyp2 where id2 in (select distinct vc_obtyp2.id2 ";
        //modellgruppen
        private static string QUERYVIEWPREFIXLEVEL2 = "select vc_obtyp3.*, id3 id from vc_obtyp3 where id3 in (select distinct vc_obtyp3.id3 ";
        //Modell
        private static string QUERYVIEWPREFIXLEVEL3 = "select vc_obtyp4.*, id4 id from vc_obtyp4 where id4 in (select distinct vc_obtyp4.id4 ";
        //typ
        private static string QUERYVIEWPREFIXLEVEL4 = "select vc_obtyp1.id1||'>'||vc_obtyp2.id2||'>'||vc_obtyp3.id3||'>'||vc_obtyp4.id4||'>'||vc_obtyp5.id5 path,vc_obtyp5.*, id5 id, vc_obtyp1.bezeichnung fahrzeugart, vc_obtyp2.bezeichnung marke, vc_obtyp3.bezeichnung baureihe, vc_obtyp4.bezeichnung modell  ";
        private static string QUERYVIEWPREFIXLEVEL4HEK = "select ob.sysobart,' EZ: '||to_date(ob.erstzul,'dd-MM-yyyy')||' KM: '||ob.kmstand||' Farbe: '||trim(ob.farbea)||' VIN: '||trim(ob.serie) bezeichnung3,ob.sysob, vc_obtyp1.id1||'>'||vc_obtyp2.id2||'>'||vc_obtyp3.id3||'>'||vc_obtyp4.id4||'>'||vc_obtyp5.id5 path,vc_obtyp5.id5,vc_obtyp5.id4,vc_obtyp5.bezeichnung,vc_obtyp5.aufbau,vc_obtyp5.getriebe,vc_obtyp5.treibstoff,vc_obtyp5.emission,vc_obtyp5.baumonat,vc_obtyp5.baujahr,ob.serie typengenehmigung,vc_obtyp5.art,vc_obtyp5.schwacke,vc_obtyp5.neupreisbrutto,vc_obtyp5.neupreisnetto,vc_obtyp5.leistung,vc_obtyp5.baubismonat,vc_obtyp5.baubisjahr,vc_obtyp5.bezeichnung2,ob.serie vin, id5 id, vc_obtyp1.bezeichnung fahrzeugart, vc_obtyp2.bezeichnung marke, vc_obtyp3.bezeichnung baureihe, vc_obtyp4.bezeichnung modell  ";

        //Filter for all sources of Eurotax (without brand specific imports like Porsche)
        private static string FILTER_IMPORTSOURCE = " and vc_obtyp2.importsource!=3 ";

        //private static string QUERYHEK = "select ob.sysobtyp from obtypmap,nkk, ob,rvt,person where ob.sysnkk=nkk.sysnkk and (nkk.zustand='aktiv' or nkk.zustand='gebucht') and nkk.sysrvt=rvt.sysrvt and rvt.sysperson=person.sysperson and ob.sysobtyp=obtypmap.sysobtyp(+) ";

        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static bool HEK = false;
        private const String BRAND_KIA = "KIA";
        private const String BRAND_HYUNDAI = "HYUNDAI";

        /// <summary>
        /// returns the next level nodes, filtered by setting-filters
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="treeNode"></param>
        /// <param name="allLevels"></param>
        /// <returns></returns>
        public Cic.P000001.Common.TreeNode[] getNextLevel(Cic.P000001.Common.Setting setting, Cic.P000001.Common.TreeNode treeNode, bool allLevels)
        {
            initTranslation();

            ObViewKey key = new ObViewKey(setting, treeNode);
            int level = treeNode == null ? 0 : treeNode.Level.Number + 1;//the NEXT level from the given current one

            if (setting.customerCode == (int)CUSTOMER_CODE.HCE)//HCE has HEK query
                HEK = true;

            List<ObViewDto> obviews = getResults(setting.SearchParams, level, key, false,setting.sysperole);

            List<TreeNode> rval = new List<TreeNode>();
            int cnt = 0;
            foreach (ObViewDto obDto in obviews)
            {
                if (setting != null && setting.customerCode == (int)CUSTOMER_CODE.BMW)
                {
                    if (level == 0)
                    {
                        obDto.bezeichnung = getTranslation(setting.SelectedLanguage, obDto.bezeichnung);
                    }
                    if (level == 1 && setting.hasFilter())
                    {
                        if (setting.filter(obDto.bezeichnung)) continue;
                    }
                }
                if (setting != null && setting.customerCode == (int)CUSTOMER_CODE.HCE)//HCE
                {
                    if (level == 0)//Wurzelknoten-Übersetzung
                    {
                        obDto.bezeichnung = getTranslation(setting.SelectedLanguage, obDto.bezeichnung);
                        obDto.bezeichnung2 = "";
                    }
                    if (level == 1 && setting.hasFilter())
                    {
                        if (setting.filter(obDto.bezeichnung)) continue;
                    }
                }
                rval.Add(new TreeNode(obDto, level, key));
                cnt++;
                if (cnt > 200) break;
            }
            if (level == 0 && setting != null && setting.customerCode == (int)CUSTOMER_CODE.HCE)//reverse level 0 for hce
            {
                rval.Reverse();
            }
            if (level == 1 && setting != null && setting.customerCode == (int)CUSTOMER_CODE.HCE)//reverse level 0 for hce
            {
                using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new Common.Model.Prisma.PrismaExtended())
                {
                    String firstBrand = ctx.ExecuteStoreQuery<String>("select brand.name from brand,prhgroupm,prbrandm where brand.sysbrand=prbrandm.sysbrand and prbrandm.sysprhgroup=prhgroupm.sysprhgroup and prhgroupm.sysperole in (select perole.sysperole from perole where exists (select 1 from prhgroupm where prhgroupm.sysperole=perole.sysperole) and rownum=1 connect by PRIOR sysparent = sysperole start with sysperole=" + setting.sysperole + ")", null).FirstOrDefault();
                    String secondBrand = BRAND_KIA;
                    if (firstBrand == null)
                        firstBrand = BRAND_HYUNDAI;
                    if (BRAND_KIA.Equals(firstBrand))
                        secondBrand = BRAND_HYUNDAI;
                    /* //Uncomment this to sort the second brand beneath the first (eg HYUNDAI, KIA, otherbrands)
                    TreeNode tn1 = (from f in rval
                                   where f.DisplayName.Equals(secondBrand)
                                   select f).FirstOrDefault();
                    if (tn1 != null)
                    {
                        rval.Remove(tn1);
                        rval.Insert(0, tn1);
                    }*/
                    TreeNode tn = (from f in rval
                          where f.DisplayName.Equals(firstBrand)
                          select f).FirstOrDefault();
                    if (tn != null)
                    {
                        rval.Remove(tn);
                        rval.Insert(0, tn);
                    }
                }
            }

            return rval.ToArray();
        }

        private const string CnstCarsText = "Car";
        private const string CnstCommercialVehiclesText = "CV";
        private const string CnstMotorcyclesText = "MOT";
        private Dictionary<string, Dictionary<string, string>> translation = new Dictionary<string, Dictionary<string, string>>();

        private void initTranslation()
        {
            Dictionary<string, string> t = new Dictionary<string, string>();
            t[CnstCarsText] = "PKW";
            t[CnstCommercialVehiclesText] = "Leicht-LKW / Transporter";
            t[CnstMotorcyclesText] = "Motorräder";

            translation["de-at"] = t;
            translation["deat"] = t;
            translation["de"] = t;
            translation["de-ch"] = t;
            translation["dech"] = t;


            t = new Dictionary<string, string>();
            t["LN"] = "Leichte Nutzfahrzeuge";
            t["PW"] = "Personenwagen";
            translation["de-de"] = t;
        }

        private string getTranslation(string language, string key)
        {
            Dictionary<string, string> t;
            String lan = language.ToLower();
            if (!translation.ContainsKey(lan))
                t = translation["de"];
            t = translation[lan];
            if (!t.ContainsKey(key))
                return key;
            return t[key];

        }


        private List<ObViewDto> getResults(TypedSearchParam[] searchParams, int level, ObViewKey key, bool reverse, long sysperole)
        {

            StringBuilder query = null;
            StringBuilder queryhek = new StringBuilder(QUERYVIEWPREFIXLEVEL4HEK + QUERYVIEWSEARCHHEK);
            List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
            List<Devart.Data.Oracle.OracleParameter> parametershek = new List<Devart.Data.Oracle.OracleParameter>();
            parametershek.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperole", Value = sysperole });
            String ordersuffix = "";
            bool isVIN = false;//controls search via vin
            bool isHEK = false;
            bool doStdSearch = true;
            String vinCode = null;

            
            String sourceFilter = FILTER_IMPORTSOURCE;
            switch (level)
            {

                case (0):

                    query = new StringBuilder(QUERYVIEWPREFIXLEVEL0 + QUERYVIEWSEARCH);
                    ordersuffix = " order by vc_obtyp1.bezeichnung ";
                    break;
                case (1)://ab hier einschränkung über gewählte nodes

                    query = new StringBuilder(QUERYVIEWPREFIXLEVEL1 + QUERYVIEWSEARCH);
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = key.VehicleType });
                    query.Append(" and vc_obtyp2.id1 =:id ");
                    if (reverse)
                    {
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id2", Value = key.BrandCode });
                        query.Append(" and vc_obtyp2.id2 =:id2 ");
                    }
                    ordersuffix = " order by vc_obtyp2.bezeichnung ";
                    break;
                case (2):
                    query = new StringBuilder(QUERYVIEWPREFIXLEVEL2 + QUERYVIEWSEARCH);
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = key.BrandCode });
                    query.Append(" and vc_obtyp3.id2 =:id ");
                    if (reverse)
                    {
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id2", Value = key.GroupCode });
                        query.Append(" and vc_obtyp3.id3 =:id2 ");
                    }
                    ordersuffix = " order by vc_obtyp3.bezeichnung ";
                    break;
                case (3):
                    query = new StringBuilder(QUERYVIEWPREFIXLEVEL3 + QUERYVIEWSEARCH);
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = key.GroupCode });
                    query.Append(" and vc_obtyp4.id3 =:id ");
                    if (reverse)
                    {
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id2", Value = key.ModelCode });
                        query.Append(" and vc_obtyp4.id4 =:id2 ");
                    }
                    ordersuffix = " order by vc_obtyp4.bezeichnung ";
                    break;
                case (4):
                    query = new StringBuilder(QUERYVIEWPREFIXLEVEL4 + QUERYVIEWSEARCH);
                    if (key.ModelCode > 0)
                    {
                        query.Append(" and vc_obtyp5.id4 =:id ");
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = key.ModelCode });
                    }
                    ordersuffix = " order by vc_obtyp5.baujahr desc,vc_obtyp5.bezeichnung ";
                    break;
            }



            if (searchParams != null)
            {
                bool partnerSearch = false;
                foreach (TypedSearchParam searchParam in searchParams)
                {
                    if (searchParam.searchType == SearchType.TYPENCODE && searchParam.Pattern != "")
                    {

                    }
                    if (searchParam.searchType == SearchType.PARTNER && searchParam.Pattern != "")
                    {
                        partnerSearch = true;
                    }
                }
                foreach (TypedSearchParam searchParam in searchParams)
                {
                    switch (searchParam.searchType)
                    {
                        case (SearchType.CO2):
                            if (searchParam.Pattern == null || searchParam.Pattern.Length == 0) break;
                            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "emission", Value = searchParam.Pattern });
                            query.Append(" and vc_obtyp5.emission <=:emission ");
                            parametershek.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "emission", Value = searchParam.Pattern });
                            queryhek.Append(" and vc_obtyp5.emission <=:emission ");
                            isHEK = true;
                            break;
                        case (SearchType.TREIBSTOFF):
                            //  if (!typencode)
                            {
                                if (searchParam.Pattern == null || searchParam.Pattern.Length == 0) break;
                                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "treibstoff", Value = searchParam.Pattern });
                                parametershek.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "treibstoff", Value = searchParam.Pattern });
                                //query.Append(" and vc_obtyp5.treibstoff=:treibstoff ");
                                query.Append(" and vc_obtyp5.treibstoff in (select dd.id from ddlkppos dd, ddlkppos dd2 where dd.code = 'TREIBSTOFF' and dd2.code='TREIBSTOFFCODE' and dd2.value=dd.domainid and dd2.id=:treibstoff) ");
                                queryhek.Append(" and vc_obtyp5.treibstoff in (select dd.id from ddlkppos dd, ddlkppos dd2 where dd.code = 'TREIBSTOFF' and dd2.code='TREIBSTOFFCODE' and dd2.value=dd.domainid and dd2.id=:treibstoff) ");
                                isHEK = true;
                            }
                            break;
                        case (SearchType.GETRIEBEART):
                            //  if (!typencode)
                            {
                                if (searchParam.Pattern == null || searchParam.Pattern.Length == 0) break;
                                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "getriebeart", Value = searchParam.Pattern });
                                parametershek.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "getriebeart", Value = searchParam.Pattern });
                                //query.Append(" and vc_obtyp5.getriebe=:getriebeart ");
                                query.Append(" and vc_obtyp5.getriebe in (select dd.id from ddlkppos dd, ddlkppos dd2 where dd.code = 'GETRIEBEART' and dd2.code='GETRIEBEARTCODE' and dd2.value=dd.domainid and dd2.id=:getriebeart) ");
                                queryhek.Append(" and vc_obtyp5.getriebe in (select dd.id from ddlkppos dd, ddlkppos dd2 where dd.code = 'GETRIEBEART' and dd2.code='GETRIEBEARTCODE' and dd2.value=dd.domainid and dd2.id=:getriebeart) ");
                                isHEK = true;
                            }
                            break;
                        case (SearchType.AUFBAU):
                            //   if (!typencode)
                            {
                                if (searchParam.Pattern == null || searchParam.Pattern.Length == 0) break;
                                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "aufbau", Value = searchParam.Pattern });
                                parametershek.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "aufbau", Value = searchParam.Pattern });
                                query.Append(" and vc_obtyp5.aufbau in (select dd.id from ddlkppos dd, ddlkppos dd2 where dd.code = 'AUFBAU' and dd2.code='AUFBAUCODE' and dd2.value=dd.domainid and dd2.id=:aufbau) ");
                                queryhek.Append(" and vc_obtyp5.aufbau in (select dd.id from ddlkppos dd, ddlkppos dd2 where dd.code = 'AUFBAU' and dd2.code='AUFBAUCODE' and dd2.value=dd.domainid and dd2.id=:aufbau) ");
                                isHEK = true;
                            }
                            break;
                        case (SearchType.MODELL):
                            if (searchParam.Pattern == null || searchParam.Pattern.Length == 0) break;
                            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "modell", Value = searchParam.Pattern });
                            parametershek.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "modell", Value = searchParam.Pattern });
                            query.Append(" and upper(vc_obtyp5.bezeichnung||vc_obtyp5.bezeichnung2) like upper('%'||:modell||'%') ");
                            queryhek.Append(" and upper(vc_obtyp5.bezeichnung||vc_obtyp5.bezeichnung2) like upper('%'||:modell||'%') ");
                            isHEK = true;
                            break;
                        case (SearchType.MARKEBEZEICHNUNG):
                            if (searchParam.Pattern == null || searchParam.Pattern.Length == 0) break;
                            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "markebezeichnung", Value = searchParam.Pattern });
                            parametershek.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "markebezeichnung", Value = searchParam.Pattern });
                            query.Append(" and upper(vc_obtyp2.bezeichnung) like upper('%'||:markebezeichnung||'%') ");
                            queryhek.Append(" and upper(vc_obtyp2.bezeichnung) like upper('%'||:markebezeichnung||'%') ");
                            isHEK = true;
                            break;
                        case (SearchType.MARKE):
                            if (!partnerSearch)//disable for pfc because the marke might be the wrong one
                            {
                                if (searchParam.Pattern == null || searchParam.Pattern.Length == 0) break;
                                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "marke", Value = searchParam.Pattern });
                                parametershek.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "marke", Value = searchParam.Pattern });
                                query.Append(" and  vc_obtyp2.id2=:marke ");
                                queryhek.Append(" and  vc_obtyp2.id2=:marke ");
                                isHEK = true;
                            }
                            else if (partnerSearch)//disable for pfc because the marke might be the wrong one
                            {
                                if (searchParam.Pattern == null || searchParam.Pattern.Length == 0) break;
                                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "marke", Value = searchParam.Pattern });
                                parametershek.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "marke", Value = searchParam.Pattern });
                                query.Append(" and vc_obtyp2.bezeichnung=(select bezeichnung from vc_obtyp2 where id2=:marke) ");
                                queryhek.Append(" and vc_obtyp2.bezeichnung=(select bezeichnung from vc_obtyp2 where id2=:marke) ");
                                isHEK = true;
                            }
                            break;
                        case (SearchType.OBJEKTTYP):
                            if (searchParam.Pattern == null || searchParam.Pattern.Length == 0) break;
                            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "obtypparam", Value = int.Parse(searchParam.Pattern) });
                            parametershek.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "obtypparam", Value = int.Parse(searchParam.Pattern) });
                            query.Append(" and  vc_obtyp1.art=:obtypparam ");
                            queryhek.Append(" and  vc_obtyp1.art=:obtypparam ");
                            isHEK = true;
                            break;
                        case (SearchType.SCHWACKE):
                            if (searchParam.Pattern == null || searchParam.Pattern.Length == 0) break;
                            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "schwacke", Value = searchParam.Pattern });
                            parametershek.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "schwacke", Value = searchParam.Pattern });
                            query.Append(" and vc_obtyp5.schwacke like '%'||:schwacke||'%' ");
                            queryhek.Append(" and vc_obtyp5.schwacke like '%'||:schwacke||'%' ");
                            isHEK = true;
                            break;
                        case (SearchType.TYPENCODE):
                            if (searchParam.Pattern == null || searchParam.Pattern.Length == 0) break;
                            
                            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "typencode", Value = searchParam.Pattern });
                            parametershek.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "typencode", Value = searchParam.Pattern });

                            query.Append(" and exists(select 1 from etgtcert where etgtcert.natcode= vc_obtyp5.schwacke  and etgtcert.num  like UPPER ('%'||:typencode||'%')) ");
                            queryhek.Append(" and exists(select 1 from etgtcert where etgtcert.natcode= vc_obtyp5.schwacke  and etgtcert.num  like UPPER ('%'||:typencode||'%')) ");
                            isHEK = true;
                            break;
                        case (SearchType.ZEITRAUMVON):
                            //  if (!typencode)
                            {
                                if (searchParam.Pattern == null || searchParam.Pattern.Length == 0) break;
                                DateTime d = getDateTime(searchParam.Pattern);
                                String month = d.Month.ToString();
                                if (month.Length < 2) month = "0" + month;
                                String year = d.Year.ToString();
                                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "baumonat", Value = month });
                                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "baujahr", Value = year });

                                parametershek.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "baumonat", Value = month });
                                parametershek.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "baujahr", Value = year });

                                query.Append(" and ((vc_obtyp5.baumonat>=:baumonat and vc_obtyp5.baujahr=:baujahr) or (vc_obtyp5.baujahr>:baujahr))");
                                queryhek.Append(" and ((vc_obtyp5.baumonat>=:baumonat and vc_obtyp5.baujahr=:baujahr) or (vc_obtyp5.baujahr>:baujahr))");
                                isHEK = true;
                            }
                            break;
                        case (SearchType.ZEITRAUMBIS):
                            //   if (!typencode)
                            {
                                if (searchParam.Pattern == null || searchParam.Pattern.Length == 0) break;

                                DateTime dbis = getDateTime(searchParam.Pattern);
                                String monthbis = dbis.Month.ToString();
                                if (monthbis.Length < 2) monthbis = "0" + monthbis;
                                String yearbis = dbis.Year.ToString();
                                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "baumonatbis", Value = monthbis });
                                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "baujahrbis", Value = yearbis });

                                parametershek.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "baumonatbis", Value = monthbis });
                                parametershek.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "baujahrbis", Value = yearbis });

                                query.Append(" and ((vc_obtyp5.baumonat<=:baumonatbis and vc_obtyp5.baujahr=:baujahrbis) or (vc_obtyp5.baujahr<:baujahrbis))");
                                queryhek.Append(" and ((vc_obtyp5.baumonat<=:baumonatbis and vc_obtyp5.baujahr=:baujahrbis) or (vc_obtyp5.baujahr<:baujahrbis))");
                                isHEK = true;
                            }
                            break;
                        case(SearchType.FUZZY):
                            if (searchParam.Pattern == null || searchParam.Pattern.Length == 0) break;
                            query.Append(getFuzzyQuery("vc_obtyp5.bezeichnung,vc_obtyp5.bezeichnung2,vc_obtyp4.bezeichnung,vc_obtyp2.bezeichnung", searchParam.Pattern, false, false, 0, 1000));
                            query.Append(getFuzzyQuery("vc_obtyp5.schwacke", searchParam.Pattern, false,true,10000,999999999));
                            query.Append(" and vc_obtyp5.neupreisbrutto is not null ");

                            queryhek.Append(getFuzzyQuery("vc_obtyp5.bezeichnung,vc_obtyp5.bezeichnung2,vc_obtyp4.bezeichnung,vc_obtyp2.bezeichnung", searchParam.Pattern, false, false, 0, 1000));
                            queryhek.Append(getFuzzyQuery("vc_obtyp5.schwacke", searchParam.Pattern, false, true, 10000, 999999999));
                            queryhek.Append(" and vc_obtyp5.neupreisbrutto is not null ");

                            isHEK = true;
                            break;
                        case (SearchType.ID):
                            if (searchParam.Pattern == null || searchParam.Pattern.Length == 0) break;
                            query.Append(" and vc_obtyp5.id5=:obid ");
                            queryhek.Append(" and vc_obtyp5.id5=:obid ");
                            
                            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "obid", Value = searchParam.Pattern });
                            parametershek.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "obid", Value = searchParam.Pattern });
                            isHEK = true;
                            break;
                       
                        case (SearchType.KWVON):
                            if (searchParam.Pattern == null || searchParam.Pattern.Length == 0) break;
                            query.Append(" and vc_obtyp5.leistung>=:kwvon ");
                            queryhek.Append(" and vc_obtyp5.leistung>=:kwvon ");

                            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "kwvon", Value = searchParam.Pattern });
                            parametershek.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "kwvon", Value = searchParam.Pattern });
                            isHEK = true;
                            break;
                        case (SearchType.KWBIS):
                            if (searchParam.Pattern == null || searchParam.Pattern.Length == 0) break;
                            query.Append(" and vc_obtyp5.leistung<=:kwbis ");
                            queryhek.Append(" and vc_obtyp5.leistung<=:kwbis ");

                            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "kwbis", Value = searchParam.Pattern });
                            parametershek.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "kwbis", Value = searchParam.Pattern });
                            isHEK = true;
                            break;
                        case (SearchType.TSN):
                            if (searchParam.Pattern == null || searchParam.Pattern.Length == 0) break;
                            queryhek.Append(" and obtypmap.tsn like UPPER ('%" + searchParam.Pattern + "%') ");
                            query.Append(" and obtypmap.tsn like UPPER ('%" + searchParam.Pattern + "%') ");
                            isHEK = true;
                            break;
                        case (SearchType.HSN):
                            if (searchParam.Pattern == null || searchParam.Pattern.Length == 0) break;
                            queryhek.Append(" and obtypmap.hsn like UPPER ('%" + searchParam.Pattern + "%') ");
                            query.Append(" and obtypmap.hsn like UPPER ('%" + searchParam.Pattern + "%') ");
                            isHEK = true;
                            break;
                        case (SearchType.KOMMNR):
                            if (searchParam.Pattern == null || searchParam.Pattern.Length == 0) break;
                            queryhek.Append(" and ob.specification like UPPER ('%" + searchParam.Pattern + "%') ");
                            isHEK = true;
                            doStdSearch = false;//not necessary, because no search criteria in vc_obtyp
                            break;
                        case (SearchType.VIN):
                            if (searchParam.Pattern == null || searchParam.Pattern.Length == 0) break;
                            queryhek.Append(" and (ob.fgnr like UPPER ('%" + searchParam.Pattern + "%') ");
                            queryhek.Append(" or ob.serie like UPPER ('%" + searchParam.Pattern + "%')) ");
                            isVIN = true;
                            isHEK = true;
                            doStdSearch = false;//not necessary, because no search criteria in vc_obtyp
                            vinCode = searchParam.Pattern;
                            break;
                        case (SearchType.PARTNER)://strategischer Partner sucht in anderem Objektbaum mit anderer importsource und anderer importtable, hier reicht die Einschränkung für die oberste Ebene
                            if (searchParam.Pattern == null || searchParam.Pattern.Length == 0) break;
                            //importsource ist konstant 3, importtable ist partner abhängig
                            sourceFilter = " and ( (vc_obtyp2.importsource=3 and UPPER(vc_obtyp2.importtable) = UPPER(:is2) )  or (vc_obtyp2.importsource<3 and vc_obtyp2.bezeichnung!=UPPER(:is3))  ) ";
                            // " and vc_obtyp2.importsource=3 "; //that would be ONLY the brandsource

                            

                            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "is2", Value = searchParam.Pattern });
                            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "is3", Value = searchParam.Pattern });

                            parametershek.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "is2", Value = searchParam.Pattern });
                            parametershek.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "is3", Value = searchParam.Pattern });


                            isHEK = true;                            
                            break;
                    }
                }
            }
            queryhek.Append(sourceFilter);
            query.Append(sourceFilter);

            if (level != 4)
                query.Append(" ) ");
            query.Append(ordersuffix);

            List<ObViewDto> rval = new List<ObViewDto>();

            if (HEK && level == 0 && isHEK)
            {
               /* StringBuilder hquery = new StringBuilder(QUERYVIEWPREFIXLEVEL4 + QUERYVIEWSEARCH);
                hquery.Append(" and vc_obtyp5.id5 in (");
                hquery.Append(queryhek);
                hquery.Append(")");
                */
                try
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new Common.Model.Prisma.PrismaExtended())
                    {

                        String hekStr = queryhek.ToString();
                        _log.Debug("HEK-Search: " + hekStr);
                        List<ObViewDto> rval2 = ctx.ExecuteStoreQuery<ObViewDto>(hekStr, parametershek.ToArray()).ToList();
                        //set level 4 (child note for vehicle, even if we currently search level 0 ) - hek searches will result in the root of the result tree
                        foreach (ObViewDto o in rval2)
                        {
                            o.level = 4;
                        }
                        rval.AddRange(rval2);
                    }
                }catch(Exception ex2)
                {
                    _log.Warn("Failure in HEK-Search", ex2);
                }
            }
            if(isVIN)
            {
                

                if (level > 0) return rval;

                if (vinCode == null || vinCode.Trim().Length != 17) return rval;

                VinSearch.vinsearchSoapPortClient vs = new VinSearch.vinsearchSoapPortClient();
                Cic.OpenOne.CarConfigurator.VinSearch.ETGHeaderType header = getVinHeader();
                Cic.OpenOne.CarConfigurator.VinSearch.VinDecodeInputType vinInput = new Cic.OpenOne.CarConfigurator.VinSearch.VinDecodeInputType();
                
                vinInput.VinCode = vinCode;
                vinInput.ServiceId = getVinServiceId();
                vinInput.ExtendedOutput = true;
                vinInput.Settings = new VinSearch.ETGsettingType();
                vinInput.Settings.ISOcountryCode = Cic.OpenOne.CarConfigurator.VinSearch.ISOcountryType.DE;
                vinInput.Settings.ISOlanguageCode = Cic.OpenOne.CarConfigurator.VinSearch.ISOlanguageType.DE;

                try
                {
                    Cic.OpenOne.CarConfigurator.VinSearch.VinDecodeOutputType outData = vs.VinDecode(ref header, vinInput);
                    if (outData != null && outData.StatusCode == 0)
                    {
                        ObViewDto data = new ObViewDto();
                        data.art = 100;
                        Cic.OpenOne.CarConfigurator.VinSearch.VehicleType v = outData.Vehicle[0];
                        if (outData.ProductionDateSpecified)
                        {
                            data.baujahr = "" + outData.ProductionDate.Year;
                            data.baumonat = "" + outData.ProductionDate.Month;
                        }
                        else
                        {
                            data.baujahr = "" + v.PeriodOfBuildDetails.PoBFromYear;
                            data.baumonat = "" + v.PeriodOfBuildDetails.PoBFromMonth;
                        }

                        if (v.EngineKWSpecified)
                            data.leistung = v.EngineKW;
                        data.schwacke = v.TypeETGCode;
                        data.bezeichnung = v.TypeDescription;
                        data.aufbau = v.BodyType;
                        data.getriebe = v.GearType;
                        data.treibstoff = v.FuelType;
                        data.emission = "0";
                        data.typengenehmigung = outData.VinCode;
                        if (v.PriceBruttoSpecified)
                            data.neupreisbrutto = (double)v.PriceBrutto;
                        if (v.PriceNettoSpecified)
                            data.neupreisnetto = (double)v.PriceNetto;
                        data.modell = v.ModelDescription;
                        data.marke = v.MakeDescription;
                        data.baubisjahr = "" + v.PeriodOfBuildDetails.PoBUntilYear;
                        data.baubismonat = "" + v.PeriodOfBuildDetails.PoBUntilMonth;

                        using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new Common.Model.Prisma.PrismaExtended())
                        {
                            List<Devart.Data.Oracle.OracleParameter> etpar = new List<Devart.Data.Oracle.OracleParameter>();
                            etpar.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "schwacke", Value = data.schwacke });
                            data.id = ctx.ExecuteStoreQuery<String>("select sysobtyp from obtyp where schwacke=:schwacke", etpar.ToArray()).FirstOrDefault();
                        }
                        data.level = 4;
                        if(data.id!=null&&data.id.Length>0)
                        {
                            rval.Add(data);
                        }
                        else
                        {
                            _log.Error("VINSEARCH Failed for "+vinCode+" - no OBTYP found for schwacke "+data.schwacke);
                        }
                    }
                }catch(Exception e)
                {
                    _log.Error("VINSEARCH Failed with " + e.Message, e);
                }

            }
            if (isVIN)
                return rval;

            if (!doStdSearch)
                return rval;

            using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new Common.Model.Prisma.PrismaExtended())
            {
                String fzQuery = query.ToString();
                _log.Debug("FZ-Search: " + fzQuery);
                List<ObViewDto> rval2 = ctx.ExecuteStoreQuery<ObViewDto>(fzQuery, parameters.ToArray()).ToList();
                if(rval!=null && rval.Count>0)
                {
                    rval.AddRange(rval2);
                    return rval;
                }
                return rval2;
            }
        }

        private Cic.OpenOne.CarConfigurator.VinSearch.ETGHeaderType getVinHeader()
        {
            Cic.OpenOne.CarConfigurator.VinSearch.ETGHeaderType header = new Cic.OpenOne.CarConfigurator.VinSearch.ETGHeaderType();

            
            Cic.OpenOne.CarConfigurator.VinSearch.LoginDataType LoginData = new Cic.OpenOne.CarConfigurator.VinSearch.LoginDataType();
            LoginData.Name = AppConfig.Instance.GetEntry("EUROTAX", "VINSERVICE_USER", "test", "SETUP.NET");
            LoginData.Password = AppConfig.Instance.GetEntry("EUROTAX", "VINSERVICE_PASSWORD", "test", "SETUP.NET");
            header.Originator = new Cic.OpenOne.CarConfigurator.VinSearch.OriginatorType();
            header.Originator.LoginData = LoginData;
            header.Originator.Signature = AppConfig.Instance.GetEntry("EUROTAX", "VINSERVICE_SIGNATURE", "HCE", "SETUP.NET");
            header.VersionRequest = Cic.OpenOne.CarConfigurator.VinSearch.VersionType.Item110;
            return header;
        }

        private string getVinServiceId()
        {
            return AppConfig.Instance.GetEntry("EUROTAX", "VINSERVICE_ID", "4148a6fe1f7c9a4807b789d8b1dc580bdfb0bbb5", "SETUP.NET");
            
        }

        private DateTime getDateTime(String date)
        {
            return System.DateTime.ParseExact(date, "yyyy.MM.dd", System.Globalization.CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// builds a dynamic search-condition in sql
        /// </summary>
        /// <param name="fields">comma separated fieldlist</param>
        /// <param name="values">space separated value list</param>
        /// <returns></returns>
        private static string getFuzzyQuery(String fields, String values, bool filterNumbers, bool filterStrings, int minNumLength, int maxNumLength)
        {
            string[] fieldlist;
            string querystring = "";
            if (fields == null)
            {

                return querystring;
            }

            char[] separ = { ',' };
            fieldlist = fields.Split(separ);
            string[] sp;
            int[] styp = null;
            int scount = 0;
            string[] st = values.Split(' ');
            List<string> nvalues = new List<string>();
            foreach (string s in st)
            {
                
                try
                {
                    int num = int.Parse(s);
                    //number
                    if (!filterNumbers && num >= minNumLength && num<=maxNumLength)
                        nvalues.Add(s);
                }
                catch (Exception e)
                {
                    //no number but string
                    if(!filterStrings)
                        nvalues.Add(s);
                }
            }
            st = nvalues.ToArray();

            scount = st.Length;
            sp = new string[scount * 2];
            styp = new int[scount * 2];
            int pos = 0;
            while (pos < st.Length)
            {
                sp[pos] = st[pos];
                if (sp[pos].IndexOf("-") == 0)
                {
                    sp[pos] = sp[pos].Substring(1);
                    styp[pos] = 1;
                }
                if (sp[pos].IndexOf("+") == 0)
                {
                    sp[pos] = sp[pos].Substring(1);
                    styp[pos] = 0;
                }
                if (sp[pos].IndexOf("=") == 0)
                {
                    sp[pos] = sp[pos].Substring(1);
                    styp[pos] = 3;
                }
                if (sp[pos].IndexOf("~") == 0)
                {
                    sp[pos] = sp[pos].Substring(1);
                    styp[pos] = 2;
                }
                if (sp[pos].IndexOf("\"") == 0)
                {
                    sp[pos] = sp[pos].Substring(1);
                    while (pos + 1 < st.Length && sp[pos].IndexOf("\"") != sp[pos].Length - 1)
                    {
                        sp[pos] = sp[pos] + " " + st[pos + 1];
                    }
                    sp[pos] = sp[pos].Substring(0, sp[pos].Length - 1);
                }
                pos++;

            }

            scount = pos;


            for (pos = 0; pos < scount; pos++)
            {
                if (styp[pos] == 3)
                {
                    querystring = querystring + " AND ( ";

                    for (int i = 0; i < fieldlist.Length; i++)
                    {
                        if (i > 0)
                        {
                            querystring = querystring + " OR ";
                        }
                        querystring = querystring + "UPPER(" + fieldlist[i] + ") = UPPER('" + toDB(sp[pos]) + "')";
                    }
                    querystring = querystring + " )";
                }
                else if (styp[pos] == 2)
                {
                    querystring = querystring + " AND ( ";

                    for (int i = 0; i < fieldlist.Length; i++)
                    {
                        if (i > 0)
                        {
                            querystring = querystring + " OR ";
                        }
                        querystring = querystring + "SOUNDEX(" + fieldlist[i] + ")" + " = SOUNDEX('" + toDB(sp[pos]) + "')";
                    }
                    querystring = querystring + " )";
                }
                else if (styp[pos] == 1)
                {
                    querystring = querystring + " AND NOT( ";

                    for (int i = 0; i < fieldlist.Length; i++)
                    {
                        if (i > 0)
                        {
                            querystring = querystring + " OR ";
                        }
                        querystring = querystring + " UPPER(" + fieldlist[i] + ") LIKE UPPER('%" + toDB(sp[pos]) + "%')";
                    }
                    querystring = querystring + " )";

                }
                else
                {
                    querystring = querystring + " AND ( UPPER( ";

                    for (int i = 0; i < fieldlist.Length; i++)
                    {
                        if (i > 0)
                        {
                            querystring += " || '|' || ";
                        }
                        querystring += fieldlist[i];

                    }

                    querystring += " ) like UPPER('%" + toDB(sp[pos]) + "%'))";

                }
            }



            return querystring;
        }


        private static string toDB(string text)
        {
            if (text == null)
            {
                return "";
            }
            System.Text.StringBuilder sbuf = new System.Text.StringBuilder(text);
            for (int i = 0; i < sbuf.Length; i++)
            {
                if (sbuf.ToString().ElementAt(i) == '\'' || sbuf.ToString().ElementAt(i) == '\\')
                {
                    sbuf.Insert(i++, '\\');
                }
            }
            text = sbuf.ToString();

            return "" + text + "";
        }

        /// <summary>
        /// returns the previous level nodes, filtered by setting-filters
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="treeNode"></param>
        /// <param name="allLevels"></param>
        /// <returns></returns>
        public Cic.P000001.Common.TreeNode[] getPreviousLevel(Cic.P000001.Common.Setting setting, Cic.P000001.Common.TreeNode treeNode, bool allLevels)
        {

            List<TreeNode> rval = new List<TreeNode>();
            TreeNode n = treeNode;
            do
            {
                n = getPreviousLevel(setting, n);
                if (n == null) break;

                rval.Add(n);
                if (allLevels)
                {
                    if (n.ParentKey.Equals("0")) break;
                }
                if (!allLevels) break;
            } while (true);

            return rval.ToArray();
        }

        private TreeNode getPreviousLevel(Cic.P000001.Common.Setting setting, Cic.P000001.Common.TreeNode treeNode)
        {
            ObViewKey key = new ObViewKey(setting, treeNode);
            int level = treeNode == null ? 0 : treeNode.Level.Number - 1;//the PREVIOUS level from the given current one
            if (level < 0) level = 0;

            List<ObViewDto> obviews = getResults(null, level, key, true,setting.sysperole);
            if (obviews == null || obviews.Count == 0) return null;

            key = key.getPreviousLevelKey();

            return new TreeNode(obviews[0], level, key);

        }
    }

    public interface IObViewDao
    {
        /// <summary>
        /// returns the next level nodes, filtered by setting-filters
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="treeNode"></param>
        /// <param name="allLevels"></param>
        /// <returns></returns>
        Cic.P000001.Common.TreeNode[] getNextLevel(Cic.P000001.Common.Setting setting, Cic.P000001.Common.TreeNode treeNode, bool allLevels);

        /// <summary>
        /// returns the previous level nodes, filtered by setting-filters
        /// </summary>
        /// <param name="setting"></param>
        /// <param name="treeNode"></param>
        /// <param name="allLevels"></param>
        /// <returns></returns>
        Cic.P000001.Common.TreeNode[] getPreviousLevel(Cic.P000001.Common.Setting setting, Cic.P000001.Common.TreeNode treeNode, bool allLevels);
    }
}
