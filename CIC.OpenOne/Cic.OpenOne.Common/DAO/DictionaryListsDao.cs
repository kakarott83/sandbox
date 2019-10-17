using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Model.DdOd;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.OpenOne.Common.Model.DdOw;
using Cic.OpenOne.Common.Util;
using Cic.OpenOne.Common.Util.Collection;
using Cic.OpenOne.Common.Util.Logging;
using AutoMapper;
using Cic.OpenOne.Common.BO.Search;
using System.Data.Common;
using System.Data.EntityClient;
using Devart.Data.Oracle;
using CIC.Database.OL.EF4.Model;
using Cic.OpenOne.Common.Model.Prisma;
using Cic.OpenOne.Common.Model.DdCt;

namespace Cic.OpenOne.Common.DAO
{
    /// <summary>
    /// Data Access Object for dictionary lists
    /// </summary>
    public class DictionaryListsDao : IDictionaryListsDao
    {
        /// <summary>
        /// The logger
        /// </summary>
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private static string QUERYDDLKPPOSDNULL = "SELECT TOOLTIP,ACTUALTERM,ORIGTERM,CODE,SYSDDLKPPOS,ID FROM cic.vc_ddlkppos vc_ddlkppos, ctlang WHERE vc_ddlkppos.activeflag = 1 and vc_ddlkppos.code = :code  AND vc_ddlkppos.sysctlang =ctlang.sysctlang and isocode=:isocode and domainid is null order by rank";
        private static string QUERYDDLKPPOSDNOTNULL = "SELECT TOOLTIP,ACTUALTERM,ORIGTERM,CODE,SYSDDLKPPOS,ID FROM cic.vc_ddlkppos vc_ddlkppos, ctlang WHERE  vc_ddlkppos.activeflag = 1 and vc_ddlkppos.code = :code  AND vc_ddlkppos.sysctlang =ctlang.sysctlang and isocode=:isocode and domainid=:domainid order by rank";
        private static string QUERYDDLKPPOSNOLANG = "select TOOLTIP, value ACTUALTERM, value ORIGTERM,CODE,SYSDDLKPPOS,ID  from cic.ddlkppos where activeflag = 1 and code =:code order by rank";
        private static string QUERYDDLKPPOSDNOLANG = "select TOOLTIP, value ACTUALTERM, value ORIGTERM,CODE,SYSDDLKPPOS,ID  from cic.ddlkppos where activeflag = 1 and code =:code and domainid=:domainid order by rank";

        private static string QUERYAUFBAU = "SELECT vc_ddlkppos.TOOLTIP,ACTUALTERM,ORIGTERM,vc_ddlkppos.CODE,vc_ddlkppos.SYSDDLKPPOS,vc_ddlkppos.ID FROM   cic.vc_ddlkppos vc_ddlkppos, ctlang , ddlkppos dd2  WHERE vc_ddlkppos.activeflag = 1 and dd2.code='AUFBAUCODE' and dd2.value=vc_ddlkppos.domainid and vc_ddlkppos.code ='AUFBAU'  AND vc_ddlkppos.sysctlang =ctlang.sysctlang and isocode=:isocode  order by vc_ddlkppos.rank";
        private static string QUERYGETRIEBEART = "SELECT vc_ddlkppos.TOOLTIP,ACTUALTERM,ORIGTERM,vc_ddlkppos.CODE,vc_ddlkppos.SYSDDLKPPOS,vc_ddlkppos.ID FROM   cic.vc_ddlkppos vc_ddlkppos, ctlang , ddlkppos dd2  WHERE vc_ddlkppos.activeflag = 1 and dd2.code='GETRIEBEARTCODE' and dd2.value=vc_ddlkppos.domainid and vc_ddlkppos.code ='GETRIEBEART'  AND vc_ddlkppos.sysctlang =ctlang.sysctlang and isocode=:isocode  order by vc_ddlkppos.rank";
        private static string QUERYTREIBSTOFFART = "SELECT vc_ddlkppos.TOOLTIP,ACTUALTERM,ORIGTERM,vc_ddlkppos.CODE,vc_ddlkppos.SYSDDLKPPOS,vc_ddlkppos.ID FROM   cic.vc_ddlkppos vc_ddlkppos, ctlang , ddlkppos dd2  WHERE vc_ddlkppos.activeflag = 1 and dd2.code='TREIBSTOFFCODE' and dd2.value=vc_ddlkppos.domainid and vc_ddlkppos.code ='TREIBSTOFF'  AND vc_ddlkppos.sysctlang =ctlang.sysctlang and isocode=:isocode  order by vc_ddlkppos.rank";

        private static string QUERYSLAPAUSE = "SELECT ID, ACTUALTERM, ORIGTERM, TOOLTIP, vc_ddlkppos.CODE, vc_ddlkppos.SYSDDLKPPOS FROM cic.vc_ddlkppos, ctlang WHERE vc_ddlkppos.activeflag = 1 AND vc_ddlkppos.sysctlang = ctlang.sysctlang AND ctlang.isocode = :isocode AND code = 'SLA_BREAK_REASON' ORDER BY cic.vc_ddlkppos.SYSCTLANG, rank";

        private static string BRANCHEQUERY = "select sysbranche sysid, sysbranche code, bezeichnung, beschreibung from branche where activeflag=1";
        private static string QUERYBRAND = "select  vc_obtyp2.id2 , vc_obtyp2.bezeichnung bezeichnung from vc_obtyp2, vc_obtyp1 where vc_obtyp1.id1=vc_obtyp2.id1 and vc_obtyp1.art=:obartid and vc_obtyp2.importsource<3 order by vc_obtyp1.bezeichnung,vc_obtyp2.bezeichnung";
        private static string QUERYBRANDFALLBACK = "select  vc_obtyp2.id2 , vc_obtyp2.bezeichnung bezeichnung from vc_obtyp2, vc_obtyp1 where vc_obtyp1.id1=vc_obtyp2.id1  order by vc_obtyp1.bezeichnung,vc_obtyp2.bezeichnung";

        private static string STAATQUERY = "select sysstaat sysid, code, '-' beschreibung, staat bezeichnung from staat where sysland=10 order by staat";
        private static string LANDQUERY = "select sysland sysid, iso code, iso beschreibung, countryname bezeichnung from land where activeflag=1 and cic.mdbs_getfavorite('FAVORITEN','LAND', trim(upper(countryname))) is not null  order by cic.mdbs_getfavorite('FAVORITEN','LAND', trim(upper(countryname))), countryname";
        private static string LANDQUERYISO3 = "select sysland sysid, iso code, ahskennzeichen beschreibung,countryname bezeichnung from land where activeflag=1 and cic.mdbs_getfavorite('FAVORITEN','LAND', trim(upper(countryname))) is not null  order by cic.mdbs_getfavorite('FAVORITEN','LAND', trim(upper(countryname))), countryname";
        private static string LANGQUERY = "select sysctlang sysid, isocode code, isocode beschreibung, languagename bezeichnung from ctlang where activeflag=1 and cic.mdbs_getfavorite('FAVORITEN','SPRACHE', trim(upper(languagename))) is not null  order by cic.mdbs_getfavorite('FAVORITEN','SPRACHE', trim(upper(languagename))), languagename";
        private static string LANGQUERY_VTSET = "select sysctlang sysid, isocode code, isocode beschreibung, languagename bezeichnung from ctlang where activeflag=1 and cic.mdbs_getfavorite('FAVORITEN','SPRACHE_VTSET', trim(upper(languagename))) is not null  order by cic.mdbs_getfavorite('FAVORITEN','SPRACHE_VTSET', trim(upper(languagename))), languagename";
        private static string NATQUERY = "select sysland sysid, iso code, iso beschreibung, countryname bezeichnung from land where activeflag=1 and notaddressrelevant = 0  and ISO is not null order by cic.mdbs_getfavorite('FAVORITEN','NATIONALITAETEN', trim(upper(countryname))), countryname";

        private static string INSURANCEQUERY = "select /*+ FIRST_ROWS */ name from person where flaglu6 = 1";

        private static String QUERYDDLKPRUB = "select *  from cic.ddlkprub ddlkprub where ddlkprub=:ddlkprub";
        private static String QUERYDDLKPCOL = "select *  from cic.ddlkpcol ddlkpcol where sysddlkpcol=:sysddlkppos";
        private static String QUERYDDLKPPOS = "select *  from cic.ddlkppos ddlkppos where ddlkppos=:ddlkppos";
        private static String QUERYDDLKPSPOS = "select *  from cic.ddlkpspos ddlkpspos where sysddlkpspos=:sysddlkpspos";
        private static String QUERYDDLKPSPOSAREA = "select * from CIC.DDLKPSPOS DDLKPSPOS where area=:area and sysid=:sysid";
        private static String QUERYDDLKPRUBS = "select *  from cic.ddlkprub ddlkprub order by ddlkprub.rank ";
        private static String QUERYDDLKPCOLS = "select *  from cic.ddlkpcol ddlkpcol order by ddlkpcol.rank ";
        private static String QUERYDDLKPPOSCOL = "select *  from cic.ddlkppos ddlkppos order by sysddlkpcol asc";

        private static CacheDictionary<String, DropListDto[]> listCache = CacheFactory<String, DropListDto[]>.getInstance().createCache(1000 * 60 * 60 * 24 * 10);

        private static CacheDictionary<String, InsuranceDto[]> insuranceCache = CacheFactory<String, InsuranceDto[]>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);
        private static CacheDictionary<String, FremdbankDto[]> fremdbankCache = CacheFactory<String, FremdbankDto[]>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);

        private static CacheDictionary<long, List<DdlkpposDto>> DdlkpposCache = CacheFactory<long, List<DdlkpposDto>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);
        private static CacheDictionary<long, List<DdlkpcolDto>> DdlkpcolCache = CacheFactory<long, List<DdlkpcolDto>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);
        private static CacheDictionary<String, List<DdlkprubDto>> DdlkprubCache = CacheFactory<String, List<DdlkprubDto>>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);

        private static CacheDictionary<long, int> prioHaendlerCache = CacheFactory<long, int>.getInstance().createCache(CacheDao.getInstance().getCacheDuration(CacheCategory.Data), CacheCategory.Data);

        // Konstanten für listFremdBanken
        private const String EIGENEBANK = "BANK-now AG";
        private const String FREMDEBANK = "Andere";

        private const int ABLTYPEIGEN = 1;
        private const int ABLTYPFREMD = 2;

        // Die Bank muss trotzdem angezeigt werden, wenn kein Konto gefunden wird.
        private static String QUERYBANKINFO =
            "SELECT /*+ FIRST_ROWS */ UnterAbfrage.sysperson, UnterAbfrage.name, UnterAbfrage.ort, konto.kontonr as kontoNummer, UnterAbfrage.kontoArt, blz.blz as clearingNr " +
                " FROM konto, blz, (SELECT /*+ index(person person_bn) */ person.sysperson, person.name, person.ort, peoption.int01 as kontoArt, " +
                " (SELECT  MAX(konto.syskonto) FROM konto  WHERE konto.sysperson = person.sysperson and konto.rang between 20 and 29) as MaxSysKonto " +
                " FROM person, peoption  " +
                " WHERE person.flagbn = 1 and peoption.sysid = person.sysperson ) UnterAbfrage " +
                " WHERE UnterAbfrage.MaxSysKonto = konto.syskonto and blz.sysblz = konto.sysblz" +
                " union all  " +
                " SELECT /*+ index(person person_bn) */ person.sysperson, person.name, person.ort, null as kontoNummer, peoption.int01 as kontoArt, null as clearingNr " +
                " FROM person, peoption " +
                " WHERE person.flagbn = 1 and peoption.sysid = person.sysperson and " +
                " (SELECT MAX(konto.syskonto) as maxkontonr FROM konto " +
                " WHERE konto.sysperson = person.sysperson and konto.rang between 20 and 29) is null";

        private static String QUERYPRIO = @"select count (*) 
                                            from ddlkpcol C, ddlkppos P, ddlkpspos SP, perole
                                            where
                                                C.SYSDDLKPCOL=SP.SYSDDLKPCOL
                                                and sp.value=p.id
                                                and p.code=c.code
                                                and perole.sysperole = sp.sysid
                                                and perole.sysperson = :sysperson
                                                and c.code='STRATEG_GRUPPE'
                                                and p.value='Porsche'";

        // Holds Brand Fields
        class BrandFields
        {
            public string bezeichnung { get; set; }
            public long id2 { get; set; }
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
            return findByDDLKPPOSCode(code, isoCode, domainId, false);
        }
        /// <summary>
        /// finds a dictionary entry in DDLKPPOS by its CODE
        /// </summary>
        /// <param name="code">the CODE of the dictionary entries</param>
        /// <param name="isoCode">the isoCode of the language</param>
        /// <param name="domainId">the domainId</param>
        /// <param name="stringid">when the ddlkppos.id is a string and no number, set to true, the unique id will then be the database key</param>
        /// <returns>array of DropListDtos</returns>
        virtual public DropListDto[] findByDDLKPPOSCode(DDLKPPOSType code, String isoCode, String domainId, bool stringid)
        {
            return findByDDLKPPOSCode(EnumUtil.GetStringValue(code), isoCode, domainId, stringid);
        }

        /// <summary>
        /// finds a dictionary entry in DDLKPPOS by its CODE
        /// </summary>
        /// <param name="code">the CODE of the dictionary entries as String</param>
        /// <param name="isoCode">the isoCode of the language</param>
        /// <param name="domainId">the domainId</param>
        /// <param name="stringid">when the ddlkppos.id is a string and no number, set to true, the unique id will then be the database key</param>
        /// <returns>array of DropListDtos</returns>
        virtual public DropListDto[] findByDDLKPPOSCode(String code, String isoCode, String domainId, bool stringid)
        {
            List<DropListDto> dropListDtoList = new List<DropListDto>();

            using (DdOdExtended odCtx = new DdOdExtended())
            {


                String query = QUERYDDLKPPOSDNULL;
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                if (isoCode != null)
                {
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "code", Value = code });
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "isocode", Value = isoCode });
                    if (domainId != null)
                    {
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "domainid", Value = domainId });
                        query = QUERYDDLKPPOSDNOTNULL;
                    }
                }
                else
                {
                    parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "code", Value = code });
                    query = QUERYDDLKPPOSNOLANG;
                    if (domainId != null)
                    {
                        parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "domainid", Value = domainId });
                        query = QUERYDDLKPPOSDNOLANG;
                    }
                }
                List<DDLKPPOSData> values = odCtx.ExecuteStoreQuery<DDLKPPOSData>(query, parameters.ToArray()).ToList();

                foreach (DDLKPPOSData ddlkppos in values)
                {
                    String term = (ddlkppos.ACTUALTERM != null && ddlkppos.ACTUALTERM.Length > 0) ? ddlkppos.ACTUALTERM : ddlkppos.ORIGTERM;
                    //long sysId=0;
                    //long.TryParse(ddlkppos.ID, out sysId);

                    if (stringid)
                    {
                        dropListDtoList.Add(
                                 new DropListDto()
                                 {
                                     sysID = ddlkppos.SYSDDLKPPOS,
                                     code = ddlkppos.ID,
                                     beschreibung = ddlkppos.TOOLTIP,
                                     bezeichnung = term
                                 });
                    }
                    else
                    {
                        dropListDtoList.Add(
                                 new DropListDto()
                                 {
                                     sysID = long.Parse(ddlkppos.ID),//ddlkppos.SYSDDLKPPOS,
                                     code = ddlkppos.ID,
                                     beschreibung = ddlkppos.TOOLTIP,
                                     bezeichnung = term
                                 });
                    }
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
        /// Delivers all Aufbau Codes 
        /// </summary>
        /// <param name="isoCode"></param>
        /// <returns></returns>
        virtual public DropListDto[] deliverAufbau(String isoCode)
        {
            List<DropListDto> dropListDtoList = new List<DropListDto>();

            using (DdOdExtended odCtx = new DdOdExtended())
            {
                String query = QUERYAUFBAU;
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "isocode", Value = isoCode });
                List<DDLKPPOSData> values = odCtx.ExecuteStoreQuery<DDLKPPOSData>(query, parameters.ToArray()).ToList();

                foreach (DDLKPPOSData ddlkppos in values)
                {
                    String term = (ddlkppos.ACTUALTERM != null && ddlkppos.ACTUALTERM.Length > 0) ? ddlkppos.ACTUALTERM : ddlkppos.ORIGTERM;
                    dropListDtoList.Add(
                        new DropListDto()
                        {
                            sysID = long.Parse(ddlkppos.ID),
                            code = ddlkppos.ID,
                            beschreibung = ddlkppos.TOOLTIP,
                            bezeichnung = term
                        });
                }
            }
            return dropListDtoList.ToArray();
        }

        /// <summary>
        /// Delivers all Treibstoffart Codes 
        /// </summary>
        /// <param name="isoCode"></param>
        /// <returns></returns>
        virtual public DropListDto[] deliverTreibstoffart(String isoCode)
        {
            List<DropListDto> dropListDtoList = new List<DropListDto>();

            using (DdOdExtended odCtx = new DdOdExtended())
            {
                String query = QUERYTREIBSTOFFART;
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "isocode", Value = isoCode });
                List<DDLKPPOSData> values = odCtx.ExecuteStoreQuery<DDLKPPOSData>(query, parameters.ToArray()).ToList();

                foreach (DDLKPPOSData ddlkppos in values)
                {
                    String term = (ddlkppos.ACTUALTERM != null && ddlkppos.ACTUALTERM.Length > 0) ? ddlkppos.ACTUALTERM : ddlkppos.ORIGTERM;
                    dropListDtoList.Add(
                        new DropListDto()
                        {
                            sysID = long.Parse(ddlkppos.ID),
                            code = ddlkppos.ID,
                            beschreibung = ddlkppos.TOOLTIP,
                            bezeichnung = term
                        });
                }
            }
            return dropListDtoList.ToArray();
        }


        /// <summary>
        /// Delivers all Getriebeart Codes 
        /// </summary>
        /// <param name="isoCode"></param>
        /// <returns></returns>
        virtual public DropListDto[] deliverGetriebeart(String isoCode)
        {
            List<DropListDto> dropListDtoList = new List<DropListDto>();


            using (DdOdExtended odCtx = new DdOdExtended())
            {
                String query = QUERYGETRIEBEART;
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "isocode", Value = isoCode });
                List<DDLKPPOSData> values = odCtx.ExecuteStoreQuery<DDLKPPOSData>(query, parameters.ToArray()).ToList();

                foreach (DDLKPPOSData ddlkppos in values)
                {
                    String term = (ddlkppos.ACTUALTERM != null && ddlkppos.ACTUALTERM.Length > 0) ? ddlkppos.ACTUALTERM : ddlkppos.ORIGTERM;
                    dropListDtoList.Add(
                        new DropListDto()
                        {
                            sysID = long.Parse(ddlkppos.ID),
                            code = ddlkppos.ID,
                            beschreibung = ddlkppos.TOOLTIP,
                            bezeichnung = term
                        });
                }
            }
            return dropListDtoList.ToArray();
        }


        /// <summary>
        /// Delivers all SlaPause reasons 
        /// </summary>
        /// <param name="isoCode"></param>
        /// <returns></returns>
        virtual public DropListDto[] deliverSlaPause(String isoCode)
        {
            List<DropListDto> dropListDtoList = new List<DropListDto>();

            using (DdOdExtended odCtx = new DdOdExtended())
            {
                String query = QUERYSLAPAUSE;
                List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "isocode", Value = isoCode });
                List<DDLKPPOSData> values = odCtx.ExecuteStoreQuery<DDLKPPOSData>(query, parameters.ToArray()).ToList();

                foreach (DDLKPPOSData ddlkppos in values)
                {
                    String term = (ddlkppos.ACTUALTERM != null && ddlkppos.ACTUALTERM.Length > 0) ? ddlkppos.ACTUALTERM : ddlkppos.ORIGTERM;
                    dropListDtoList.Add
                    (
                        new DropListDto()
                        {
                            sysID = long.Parse(ddlkppos.ID),
                            code = ddlkppos.ID,
                            beschreibung = ddlkppos.TOOLTIP,
                            bezeichnung = term
                        }
                    );
                }
            }
            return dropListDtoList.ToArray();
        }


        /// <summary>
        /// Delivers a list of STAATen
        /// </summary>
        /// <returns>list of STAATen</returns>
        virtual public DropListDto[] deliverSTAAT()
        {
            if (!listCache.ContainsKey("STAAT"))
            {
                List<DropListDto> dropListDtoList = new List<DropListDto>();

                _log.Debug("deliverSTAAT");
                using (DdCtExtended ctx = new DdCtExtended())
                {


                    dropListDtoList = ctx.ExecuteStoreQuery<DropListDto>(STAATQUERY, null).ToList();
                    

                }

                listCache["STAAT"] = dropListDtoList.ToArray();
            }
            return listCache["STAAT"];
        }

        /// <summary>
        /// Delivers a list of countrys (Länder)
        /// </summary>
        /// <returns>list of countrys</returns>
        virtual public DropListDto[] deliverLAND()
        {
            if (!listCache.ContainsKey("LAND"))
            {
                List<DropListDto> dropListDtoList = new List<DropListDto>();

                _log.Debug("deliverLand");
                using (DdCtExtended ctx = new DdCtExtended())
                {
                    

                    dropListDtoList = ctx.ExecuteStoreQuery<DropListDto>(LANDQUERY, null).ToList();

                 
                }

                listCache["LAND"] = dropListDtoList.ToArray();
            }
            return listCache["LAND"];
        }

        /// <summary>
        /// Delivers a list of countrys (Nationalitäten)
        /// </summary>
        /// <returns>list of countrys</returns>
        virtual public DropListDto[] deliverNATIONALITIES()
        {
            if (!listCache.ContainsKey("NATIONALITIES"))
            {
                List<DropListDto> dropListDtoList = new List<DropListDto>();

                _log.Debug("deliverNATIONALITIES");
                using (DdCtExtended ctx = new DdCtExtended())
                {
                   
                    dropListDtoList = ctx.ExecuteStoreQuery<DropListDto>(NATQUERY, null).ToList();

                   
                }
                listCache["NATIONALITIES"] = dropListDtoList.ToArray();
            }
            return listCache["NATIONALITIES"];
        }

        /// <summary>
        /// Delivers a list of languages
        /// </summary>
        /// <returns>list of languages</returns>
        virtual public DropListDto[] deliverCTLANG()
        {
            if (!listCache.ContainsKey("CTLANG"))
            {
                List<DropListDto> dropListDtoList = new List<DropListDto>();

                _log.Debug("deliverCTLANG");
                using (DdOwExtended owCtx = new DdOwExtended())
                {
                   

                    dropListDtoList = owCtx.ExecuteStoreQuery<DropListDto>(LANGQUERY, null).ToList();
                    
                }
                listCache["CTLANG"] = dropListDtoList.ToArray();
            }
            return listCache["CTLANG"];
        }

        /// <summary>
        /// Delivers a list of languages
        /// </summary>
        /// <returns>list of languages</returns>
        virtual public DropListDto[] deliverCTLANG_PRINT()
        {
            if (!listCache.ContainsKey("CTLANG_PRINT"))
            {
                List<DropListDto> dropListDtoList = new List<DropListDto>();

                _log.Debug("deliverCTLANG_PRINT");
                using (DdOwExtended owCtx = new DdOwExtended())
                {
                    

                    dropListDtoList= owCtx.ExecuteStoreQuery<DropListDto>(LANGQUERY_VTSET, null).ToList();
                    
                }
                listCache["CTLANG_PRINT"] = dropListDtoList.ToArray();
            }
            return listCache["CTLANG_PRINT"];
        }

        /// <summary>
        /// Delivers a list of countrys (Länder)
        /// </summary>
        /// <returns>list of countrys</returns>
        virtual public DropListDto[] deliverISO3LAND()
        {
            if (!listCache.ContainsKey("ISO3LAND"))
            {

                List<DropListDto> dropListDtoList = new List<DropListDto>();

                _log.Debug("deliverISO3LAND");
                using (DdCtExtended ctx = new DdCtExtended())
                {
                    
                    dropListDtoList = ctx.ExecuteStoreQuery<DropListDto>(LANDQUERYISO3, null).ToList();
                }
                listCache["ISO3LAND"] = dropListDtoList.ToArray();
            }
            return listCache["ISO3LAND"];
        }

        /// <summary>
        /// delivers a list of brands
        /// </summary>
        /// <returns>array of DropListDtos</returns>
        virtual public DropListDto[] deliverBRAND(String isocode, int obart)
        {
            List<DropListDto> dropListDtoList = new List<DropListDto>();

            _log.Debug("deliverBRAND");
            using (DdCtExtended ctx = new DdCtExtended())
            {
                

                List<BrandFields> values = null;
                if (obart == 0)
                {
                    values = ctx.ExecuteStoreQuery<BrandFields>(QUERYBRANDFALLBACK, null).ToList();
                }
                else
                {
                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "obartid", Value = obart });

                    values = ctx.ExecuteStoreQuery<BrandFields>(QUERYBRAND, parameters.ToArray()).ToList();
                }
                foreach (BrandFields brand in values)
                {
                    dropListDtoList.Add(
                        new DropListDto()
                        {
                            sysID = (long)brand.id2,
                            code = brand.id2.ToString(),
                            beschreibung = brand.bezeichnung,
                            bezeichnung = brand.bezeichnung
                        });
                }
            }
            return dropListDtoList.ToArray();
        }



        /// <summary>
        /// Branchenliste zurückgeben
        /// </summary>
        /// <returns>array of DropListDtos</returns>
        virtual public DropListDto[] deliverBranche()
        {
            if (!listCache.ContainsKey("BRANCHE"))
            {
                List<DropListDto> dropListDtoList = new List<DropListDto>();

                _log.Debug("deliverBranche");

                using (PrismaExtended olCtx = new PrismaExtended())
                {
                   
                    dropListDtoList = olCtx.ExecuteStoreQuery<DropListDto>(BRANCHEQUERY, null).ToList();
                }
                listCache["BRANCHE"] = dropListDtoList.ToArray();

            }
            return listCache["BRANCHE"];
        }

        /// <summary>
        /// delivers Insurance
        /// </summary>
        /// <returns>InsuranceDto</returns>
        virtual public InsuranceDto[] deliverInsurance()
        {
            if (!insuranceCache.ContainsKey("INSURANCES"))
            {
                List<InsuranceDto> rval = new List<InsuranceDto>();
                using (DdOdExtended odCtx = new DdOdExtended())
                {
                    String query = INSURANCEQUERY;
                    List<String> values = odCtx.ExecuteStoreQuery<String>(query).ToList();
                    values = (from v in values
                              orderby v
                              select v).ToList();
                    foreach (String value in values)
                    {
                        rval.Add(new InsuranceDto()
                        {
                            name = value
                        });
                    }
                }
                insuranceCache["INSURANCES"] = rval.ToArray();
            }
            return insuranceCache["INSURANCES"];
        }


        /// <summary>
        /// Liefert eine Liste der Fremdbanken
        /// Ticket#2012070910000019 — Neuer Webservice Ermittlung Fremdbanken.
        /// Dieser Service ist nur für die Kreditfinanzierung.
        /// </summary>
        /// <returns>FremdbankDto</returns>
        virtual public FremdbankDto[] listFremdBanken()
        {
            if (!fremdbankCache.ContainsKey("FREMDBANKEN"))
            {

                List<FremdbankDto> bankInfoListe = new List<FremdbankDto>();
                using (DdCtExtended ctx = new DdCtExtended())
                {
                    bankInfoListe = ctx.ExecuteStoreQuery<FremdbankDto>(QUERYBANKINFO).ToList();
                }

                foreach (var bankInfo in bankInfoListe)
                {
                    bankInfo.sysabltyp = ABLTYPFREMD;

                    // Die Bank wird trotzdem angezeigt, wenn kein Konto gefunden wird.
                    if (String.IsNullOrEmpty(bankInfo.kontoNummer) && String.IsNullOrEmpty(bankInfo.clearingNr))
                    {
                        // Ort, Kontonr und Clearingnummer werden in diesem Fall leer zurückgeliefert
                        bankInfo.ort = String.Empty;
                        bankInfo.kontoNummer = String.Empty;
                        bankInfo.clearingNr = String.Empty;
                    }
                    else
                    {
                        // Die Kontoart legt fest, ob die Felder Ort, Clearing- und Kontonummer Inhalte haben sollen, die an Centralway übergeben werden.
                        if ((new long[] { 2, 3, 5 }).Contains(bankInfo.kontoArt) == false) bankInfo.ort = String.Empty;
                        if ((new long[] { 2 }).Contains(bankInfo.kontoArt) == false) bankInfo.kontoNummer = String.Empty;
                        if ((new long[] { 2, 3, 4, 5 }).Contains(bankInfo.kontoArt) == false) bankInfo.clearingNr = String.Empty;
                    }
                }

                // Die 2 Banken werden immer zurückgegeben:
                // Der Eintrag „BANK-now AG“ wird benötigt, um eine Eigenablöse zu identifizieren.
                bankInfoListe.Add(new FremdbankDto() { name = EIGENEBANK, sysabltyp = ABLTYPEIGEN, ort = String.Empty, kontoNummer = String.Empty, clearingNr = String.Empty, kontoArt = 0 });
                // „Andere“ wird selektiert, wenn die Bank nicht bekannt ist. 
                bankInfoListe.Add(new FremdbankDto() { name = FREMDEBANK, sysabltyp = ABLTYPFREMD, ort = String.Empty, kontoNummer = String.Empty, clearingNr = String.Empty, kontoArt = 0 });
                // In beiden Fällen sind Ort, Konto- und Clearingnummer leer.

                // Ticket#2012081310000232 Liste der Banken sortieren
                bankInfoListe = bankInfoListe.OrderBy(bankInfo => bankInfo.name).ToList();


                fremdbankCache["FREMDBANKEN"] = bankInfoListe.ToArray();
            }
            return fremdbankCache["FREMDBANKEN"];
        }




        /// <summary>
        /// updates/creates ddlkppos
        /// </summary>
        /// <param name="ddlkpspos"></param>
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
                    if (pos.sysddlkpspos > 0 && ((pos.value == null || "".Equals(pos.value)) && pos.content == null))
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
                    else if (pos.sysddlkpspos == 0 && ((pos.value == null || "".Equals(pos.value)) && pos.content == null))
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

                //TODO - add CONTENT to EDMX!!!!
                DbConnection con = (ctx.Connection as EntityConnection).StoreConnection;
                con.Open();
                try
                {
                    DbCommand cmd = con.CreateCommand();

                    // query values with a stored procedure with two out parameters
                    cmd.CommandText = "update DDLKPSPOS set content=:content where sysddlkpspos=:id";
                    cmd.CommandType = System.Data.CommandType.Text;

                    cmd.Parameters.Add(new OracleParameter("content", OracleDbType.Clob));
                    cmd.Parameters.Add(new OracleParameter("id", OracleDbType.Long));

                    int i = 0;
                    foreach (DDLKPSPOS pos in result)
                    {
                        if (ddlkpspos[i] != null && ddlkpspos[i].content != null)
                        {
                            cmd.Parameters["content"].Value = ddlkpspos[i].content;
                            cmd.Parameters["id"].Value = pos.SYSDDLKPSPOS;
                            //Execute Stored Procedure
                            cmd.ExecuteNonQuery();
                        }
                        i++;
                    }


                    cmd.Dispose();
                }
                catch (Exception ex)
                {
                    _log.Error("Error setting ddlkppos content", ex);
                    throw new Exception("Funktion fehlgeschlagen: " + ex.Message);
                }
                finally
                {
                    con.Close();
                }

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
        /// returns select items for a rub entry
        /// </summary>
        /// <param name="sysddlkpcol"></param>
        /// <returns></returns>
        public List<DdlkpposDto> getDdlkppos(long sysddlkpcol)
        {
            if (!DdlkpposCache.ContainsKey(sysddlkpcol))
            {
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
        public List<DdlkprubDto> getDdlkprubs(String crmarea)
        {
            if (!DdlkprubCache.ContainsKey(crmarea))
            {
                using (DdCtExtended ctx = new DdCtExtended())
                {
                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    //parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "area", Value = crmarea });
                    List<DdlkprubDto> pos = ctx.ExecuteStoreQuery<DdlkprubDto>(QUERYDDLKPRUBS, parameters.ToArray()).ToList();
                    List<DdlkprubDto> clist = null;
                    String lastsyskey = null;
                    foreach (DdlkprubDto p in pos)
                    {
                        if (p.area == null) continue;
                        if (!p.area.Equals(lastsyskey))
                        {
                            lastsyskey = p.area;
                            if (!DdlkprubCache.ContainsKey(lastsyskey))
                            {
                                clist = new List<DdlkprubDto>();
                                DdlkprubCache[lastsyskey] = clist;
                            }
                            else
                                clist = DdlkprubCache[lastsyskey];

                        }
                        clist.Add(p);
                    }

                }
            }
            return DdlkprubCache[crmarea];



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
        /// <param name="sysddlkpcol"></param>
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
        /// <param name="sysddlkppos"></param>
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
        /// returns priority flag for salesmen
        /// </summary>
        /// <param name="syshaendler">salesman id</param>
        /// <returns>priority</returns>
        public int getPrioHaendler(long syshaendler)
        {
            if (!prioHaendlerCache.ContainsKey(syshaendler))
            {
                using (DdCtExtended ctx = new DdCtExtended())
                {
                    List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                    parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperson", Value = syshaendler });
                    int prio = ctx.ExecuteStoreQuery<int>(QUERYPRIO, parameters.ToArray()).FirstOrDefault();
                    prioHaendlerCache.Add(syshaendler, prio);
                    return prio;
                }
            }

            return prioHaendlerCache[syshaendler];
        }
    }
}