using System;
using System.Collections.Generic;
using System.Linq;
using Cic.One.Web.DAO;
using Cic.One.DTO;
using Cic.OpenOne.Common.DTO;
using Cic.OpenOne.Common.Util;
using Cic.OpenOne.Common.Util.Logging;
using System.Reflection;
using Cic.One.Web.DAO.Mail;
using Cic.OpenOne.Common.Util.Security;
using Cic.One.Web.BO.Mail;
using System.Text.RegularExpressions;
using Cic.OpenOne.Common.Util.Config;
using CIC.ASS.SearchService.DTO;
using CIC.ASS.Common.BO;
using Cic.One.Web.Service.DAO;
using Cic.OpenOne.Common.BO;
using Cic.OpenOne.Common.DAO;
using Cic.One.Utils.Util.Extension;
using Cic.OpenOne.CarConfigurator.BO;
using Cic.P000001.Common;
using System.Collections;
using Cic.One.Web.BO;
using CIC.ASS.Common.DTO;
using System.Text;
using System.Globalization;


namespace Cic.One.Web.BO.Search
{
    /// <summary>
    /// Factory for searches from the GUI Dropdown
    /// </summary>
    public class XproInfoFactory
    {

        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static volatile XproInfoFactory _self;
        private static readonly object InstanceLocker = new Object();
        private static Dictionary<Type, String> hints = new Dictionary<Type, string>();
        protected Dictionary<XproEntityType, XproInfoBaseDao> dictionary;
        protected Dictionary<String, XproInfoBaseDao> dictionaryCode;

        protected Dictionary<String, long> mailboxIds = new Dictionary<string,long>();
        private long lastMailId = 100000000;
        //Fields corresponding to account-search-hint index
        private static String ACCOUNT_INDEX_FIELDS = "PERSON.NAME, PERSON.CODE, PERSON.VORNAME,  PERSON.PLZ, PERSON.ORT, PERSON.STRASSE, PERSON.TELEFON, PERSON.MATCHCODE";

        public static XproInfoFactory getInstance()
        {
             if (_self == null)
            {
                lock (InstanceLocker)
                {
                    if (_self == null)
                        _self = new XproInfoFactory();
                }
            }
            return _self;
        }

        protected XproInfoFactory()
        {
            hints[typeof(Cic.One.DTO.AccountDto)] = AppConfig.Instance.GetEntry("HINT", "ACCOUNTS", "/*+ index(person person_accounts_ext) */", "SETUP.NET");
            dictionary = new Dictionary<XproEntityType, XproInfoBaseDao>();
            dictionaryCode = new Dictionary<String, XproInfoBaseDao>();
            registerDDLKPPOSSearches();
            registerProviders();
            CreateDictionaryListsBo("de-DE");
        }
       

        /// <summary>
        /// Converts a string to its enum type
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static XproEntityType ConvertStringToXproEnum(string str)
        {
            return (XproEntityType)Enum.Parse(typeof(XproEntityType), str.ToUpper());
        }

        /// <summary>
        /// Registers all searches defined in DDLKPPOSType
        /// this searches will all use the same business logic method for the search
        /// </summary>
        private void registerDDLKPPOSSearches()
        {
            //register all DDLKPPOS-Enums as xpro-search function
            foreach (DDLKPPOSType code in Enum.GetValues(typeof(DDLKPPOSType)))
            {
				XproEntityType type = ConvertStringToXproEnum(code.ToString());
                if (!dictionary.ContainsKey(type))
                {
                    DDLKPPOSType temp = code;
                    dictionary.Add(type, new XproInfoDao()
                    {
                        QueryItemFunction2 = (input) =>
                        {
                            var bo = CreateDictionaryListsBo(input.isoCode);
                            return bo.listByCode(EnumUtil.GetStringValue(temp), input.domainId).FirstOrDefault((a) => a.sysID == input.EntityId);
                        },
                        QueryItemsFunction = (filter) =>
                        {
                            var bo = CreateDictionaryListsBo(filter.isoCode);
                            return bo.listByCode(EnumUtil.GetStringValue(temp),filter.domainId);
                        },
                    });
                }

                //TODO error? falls schon eins vorhanden?
            }
        }

        /// <summary>
        /// Creates a html-designed panel for the given detail information
        /// the panel will be displayed in GUI-Dropdowns
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="title"></param>
        /// <param name="desc1"></param>
        /// <param name="desc2"></param>
        /// <param name="desc3"></param>
        /// <returns></returns>
        protected static String createPanel(XproEntityDto rval, String entity, String title, String desc1, String desc2, String desc3, String indicator, String id)
        {
            rval.desc1 = desc1;
            rval.desc2 = desc2;
            rval.desc3 = desc3;
            rval.indicator = indicator;
            rval.title = title;
            rval.beschreibung = entity;
         
            return null;
        }

        /// <summary>
        /// Register special searches, where every search-type is implemented in its own business logic method
        /// </summary>
        protected virtual void registerProviders()
        {
            if (LuceneFactory.getInstance().indexAvailable())
            {
                #region Lucene
                dictionary.Add(XproEntityType.IT,
                    new XproInfoDao<SearchResult>()
                    {
                        QueryItemFunction = (id) =>
                        {
                            if (id == 0) return new SearchResult();
                            var bo = CreateEntityBo();
                            ItDto acc = bo.getItDetails(id);
                            if (acc == null) return new SearchResult();
                            return new SearchResult { title = acc.name + " " + acc.vorname, description1 = acc.strasse, description2 = acc.plz + " " + acc.ort, description3 = acc.telefon };
                        },
                        QueryItemsFunction = (input) =>
                        {
                            String filter = input.Filter != null ? input.Filter : "*";
                            if (!filter.EndsWith("*"))
                                filter = filter + "*";

                            SearchEntityResult[] ser = LuceneBO.getInstance().search(filter, input.sysperole.ToString(), "INT", null, null, null);
                            if (ser != null && ser.Length == 1)
                            {
                                return ser[0].results;

                            }
                            return null;


                        },
                        CreateBezeichnung = (item) => item.title,
                        CreateBeschreibung = (item,rval) => createPanel(rval,"PERSON", item.title, item.description1, item.description2, item.description3, item.indicator, item.id)
                    }
                 );
                dictionary.Add(XproEntityType.ACCOUNTS,
                    new XproInfoDao<SearchResult>()
                    {
                        QueryItemFunction = (id) =>
                        {
                            if (id == 0) return new SearchResult();
                            var bo = CreateEntityBo();
                            AccountDto acc = bo.getAccountDetails(id);
                            if (acc == null) return new SearchResult();
                            return new SearchResult { title = acc.name + " " + acc.vorname, description1 = acc.strasse, description2 = acc.plz + " " + acc.ort, description3 = acc.telefon };
                        },
                        QueryItemsFunction = (input) =>
                        {
                            String filter = input.Filter != null ? input.Filter : "*";
                            if (!filter.EndsWith("*"))
                                filter = filter + "*";

                            SearchEntityResult[] ser = LuceneBO.getInstance().search(filter, input.sysperole.ToString(), "PERSON", null, null, null);
                            if (ser != null && ser.Length == 1)
                            {
                                return ser[0].results;

                            }
                            return null;


                        },
                        CreateBezeichnung = (item) => item.title,
                        CreateBeschreibung = (item,rval) => createPanel(rval,"PERSON", item.title, item.description1, item.description2, item.description3, item.indicator, item.id)
                    }
                 );


                dictionary.Add(XproEntityType.WKTACCOUNTS,
                    new XproInfoDao<SearchResult>()
                    {
                        QueryItemFunction = (id) =>
                        {
                            var bo = CreateEntityBo();
                            WktaccountDto acc = bo.getWktAccountDetails(id);
                            if (acc == null) return new SearchResult();
                            return new SearchResult { title = acc.name + " " + acc.vorname, description1 = acc.strasse, description2 = acc.plz + " " + acc.ort, description3 = acc.telefon };
                        },
                        QueryItemsFunction = (input) =>
                        {
                            String filter = input.Filter != null ? input.Filter : "*";
                            if (!filter.EndsWith("*"))
                                filter = filter + "*";

                            SearchEntityResult[] ser = LuceneBO.getInstance().search(filter, input.sysperole.ToString(), "WKTACCOUNT", null, null, null);
                            if (ser != null && ser.Length == 1)
                            {

                                int[] ids = (from t in ser[0].results
                                             where long.Parse(t.id) < 0
                                             select -1 * int.Parse(t.id)).ToArray();
                                if (ids != null && ids.Length > 0)
                                {
                                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                                    {
                                        long[] validIds = ctx.ExecuteStoreQuery<long>("select -1*sysit from it where sysit in (" + String.Join(",", ids) + ")", null).ToArray();
                                        return (from t in ser[0].results
                                                where long.Parse(t.id) > 0 || validIds.Contains(long.Parse(t.id))
                                                select t).ToArray();
                                    }
                                }
                                else
                                {
                                    return ser[0].results;
                                }

                            }
                            return null;


                        },
                        CreateBezeichnung = (item) => item.title,
                        CreateBeschreibung = (item,rval) => createPanel(rval,"PERSON", item.title, item.description1, item.description2, item.description3, item.indicator, item.id)
                    }
                 );

                dictionary.Add(XproEntityType.PARTNER,
                    new XproInfoDao<PartnerDto>()
                    {
                        QueryItemFunction = (id) =>
                        {
                            var bo = CreateEntityBo();
                            return bo.getPartnerDetails(id);
                        },
                        QueryItemsFunction = (input) =>
                        {
                            String filter = input.Filter != null ? input.Filter : "*";
                            if (!filter.EndsWith("*"))
                                filter = filter + "*";

                            String orgPartner = null;
                            if (input.filters.Length > 0 && input.filters[0].fieldname.ToLower().IndexOf("ptrelate.sysperson1") > -1)
                                orgPartner = input.filters[0].value;

                            CIC.ASS.Common.DTO.QueryPreprocessorConfig pp = null;
                            if (!(orgPartner == null || orgPartner.Length == 0 || long.Parse(orgPartner)==0))//PTRELATE suche nach filter nicht auf beziehungzu-id einschränken wenn nicht übergeben
                            {
                                pp = new QueryPreprocessorConfig();
                                pp.type = QueryPreprocessorType.OR;
                                pp.query1 = "sysperson1";
                                pp.query2 = orgPartner;
                                pp.boost1 = 0.2f;
                                pp.boost2 = 0.8f;//relation-entities preferred
                                pp.occur1 = Lucene.Net.Search.Occur.MUST;//the user-expression must occur (the searched user must be in person)
                                pp.occur2 = Lucene.Net.Search.Occur.SHOULD;//relation-entities should be used, but dont have to, if none found
                               
                            }
                                
                                
                            SearchEntityResult[] ser = LuceneBO.getInstance().search(filter, input.sysperole.ToString(), "PERSON", null,pp , null);


                            if (ser == null || ser.Length == 0)
                                return null;

                            List<long> ids = (from sr in ser[0].results
                                              select long.Parse(sr.id)).ToList<long>();
                            if (ids.Count == 0) return null;

                            String idsIn = String.Join(",", ids);
                            /*
                             * zwei schritte zum befüllen der partner-info, lucene liefert alle kombiniert
                                --alle ansprechpartner
                                select PTRELATE.*,person2.NAME beziehungzu, PERSON.SYSPERSON, PERSON.NAME, PERSON.CODE, PERSON.VORNAME,  PERSON.PLZ, PERSON.ORT, PERSON.STRASSE, PERSON.TELEFON, PERSON.HSNR
                                from person, ptrelate, person2 where person.sysperson=ptrelate.sysperson2 and person2.sysperson=ptrelate.sysperson1  and person.sysperson in (722968,722967,720058,720896,721469,722971) and sysperson1=722966;

                                --alle nicht verknüpften, person.sysperson von oben aus in-liste entfernen
                                select -1*person.sysperson sysid, person.name, person.vorname, person.sysperson
                                from person where person.sysperson in (720058,720896,721469,722971);
                             * 
                             */
                            SearchDao<PartnerDto> sd = new SearchDao<PartnerDto>();
                            List<Devart.Data.Oracle.OracleParameter> parameters = new List<Devart.Data.Oracle.OracleParameter>();
                            parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "orgPartner", Value = orgPartner });

                            List<PartnerDto> result = new List<PartnerDto>();
                            if (orgPartner != null)
                            {
                                result = sd.search("select PTRELATE.*,person2.NAME beziehungzu, PERSON.SYSPERSON, PERSON.NAME, PERSON.CODE, PERSON.VORNAME,  PERSON.PLZ, PERSON.ORT, PERSON.STRASSE, PERSON.TELEFON, PERSON.HSNR from person, ptrelate, person2 where PERSON.SYSPERSON=PTRELATE.SYSPERSON2(+) and   ptrelate.sysperson1=person2.sysperson(+)  and person.sysperson in ("
                                 + idsIn + ") and sysperson1=:orgPartner", parameters.ToArray());
                                SearchDao<PartnerDto>.fillPartnerRelationInfos(result);
                                List<long> idsFound = (from sr in result
                                                       select sr.sysperson).ToList<long>();
                                ids.RemoveAll(x => idsFound.Contains(x));
                                idsIn = String.Join(",", ids);
                            }

                            if (idsIn.Length > 0)
                            {
                                parameters = new List<Devart.Data.Oracle.OracleParameter>();
                                parameters.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "orgPartner", Value = orgPartner });
                                List<PartnerDto> partners = sd.search("select PERSON.SYSPERSON*-1 AS SYSPTRELATE, PERSON.SYSPERSON, PERSON.NAME, PERSON.CODE, PERSON.VORNAME,  PERSON.PLZ, PERSON.ORT, PERSON.STRASSE, PERSON.TELEFON, PERSON.HSNR from person where person.sysperson in ("
                                + idsIn + ")", null);
                                result.AddRange(partners);
                            }
                            return result.ToArray();


                        },
                        CreateBezeichnung = (item) => (item.vorname != null ? item.vorname + " " : "") + item.name,
                        CreateBeschreibung = (item,rval) => createPanel(rval,"PERSON", item.name + " " + item.vorname, item.strasse, item.plz + " " + item.ort, item.beziehung != null ? (item.beziehung + " zu " + item.beziehungzu + ", " + item.beziehungstyp + ", " + item.beziehungsfunktion) : "", item.indicatorContent, "" + item.entityId)

                    }
                 );
                dictionary.Add(XproEntityType.BETEILIGTER,
               new XproInfoDao<BeteiligterDto>()
               {
                   QueryItemFunction = (id) =>
                   {
                       var bo = CreateEntityBo();
                       return bo.getBeteiligterDetails(id);
                   },
                   QueryItemsFunction = (input) =>
                   {

                       String filter = input.Filter != null ? input.Filter : "*";
                       if (!filter.EndsWith("*"))
                           filter = filter + "*";

                       QueryPreprocessorConfig pp = new QueryPreprocessorConfig();
                       pp.type = QueryPreprocessorType.OR;
                       pp.query1 = "sysidparentoppo";
                       pp.query2 = "not sysidparentoppo:0";
                       pp.boost1 = 0.2f;
                       pp.boost2 = 0.8f;//relation-entities preferred
                       pp.occur1 = Lucene.Net.Search.Occur.MUST;//the user-expression must occur
                       pp.occur2 = Lucene.Net.Search.Occur.SHOULD;//relation-entities should be used, but dont have to
                               

                       SearchEntityResult[] ser = LuceneBO.getInstance().search(filter, input.sysperole.ToString(), "PERSON", null, pp, null);
                       if (ser == null || ser.Length == 0)
                           return null;

                       List<long> ids = (from sr in ser[0].results
                                         select long.Parse(sr.id)).ToList<long>();
                       if (ids.Count == 0) return null;

                       String idsIn = String.Join(",", ids);

                       SearchDao<BeteiligterDto> sd = new SearchDao<BeteiligterDto>();

                       List<BeteiligterDto> result = new List<BeteiligterDto>();

                       result = sd.search("select CRMNM.*,PERSON.SYSPERSON, PERSON.NAME, PERSON.CODE, PERSON.VORNAME,  PERSON.PLZ, PERSON.ORT, PERSON.STRASSE, PERSON.TELEFON, PERSON.HSNR from CIC.CRMNM CRMNM, CIC.PERSON PERSON where CRMNM.SYSIDCHILD = PERSON.SYSPERSON and CRMNM.PARENTAREA='OPPO' AND  CRMNM.CHILDAREA='PERSON' and person.name is not null and person.sysperson in ("
                       + idsIn + ") ", null);

                       List<long> idsFound = (from sr in result
                                              select sr.sysperson).ToList<long>();
                       ids.RemoveAll(x => idsFound.Contains(x));
                       idsIn = String.Join(",", ids);


                       if (idsIn.Length > 0)
                       {

                           List<BeteiligterDto> partners = sd.search("select PERSON.SYSPERSON*-1 AS SYSCRMNM, PERSON.SYSPERSON, PERSON.NAME, PERSON.CODE, PERSON.VORNAME,  PERSON.PLZ, PERSON.ORT, PERSON.STRASSE, PERSON.TELEFON, PERSON.HSNR from person where person.name is not null and person.sysperson in ("
                           + idsIn + ")", null);
                           result.AddRange(partners);
                       }
                       return result.ToArray();

                   },
                   CreateBezeichnung = (item) => (item.vorname != null ? item.vorname + " " : "") + item.name,
                   CreateBeschreibung = (item,rval) => createPanel(rval,"PERSON", item.name + " " + item.vorname, item.strasse, item.plz + " " + item.ort, item.syscrmnm > 0 ? "Beteiligter" : "", item.indicatorContent, "" + item.entityId)


               }
            );

                dictionary.Add(XproEntityType.OPPORTUNITY, new XproInfoDao<OpportunityDto>()
                {
                    QueryItemFunction = (id) =>
                    {
                        var bo = CreateEntityBo();
                        return bo.getOpportunityDetails(id);
                    },
                    QueryItemsFunction = (input) =>
                    {

                        String filter = input.Filter != null ? input.Filter : "*";
                        if (!filter.EndsWith("*"))
                            filter = filter + "*";

                        String sysidchildoppo = null;
                        if (input.filters.Length > 0 && input.filters[0].fieldname.ToLower().IndexOf("sysperson") > -1)
                            sysidchildoppo = input.filters[0].value;


                        QueryPreprocessorConfig pp = null;

                        if (sysidchildoppo != null)
                        {
                            pp = new QueryPreprocessorConfig();
                            pp.type = QueryPreprocessorType.OR;
                            pp.query1 = "sysidchildoppo";
                            pp.query2 = sysidchildoppo;
                            pp.boost1 = 0.2f;
                            pp.boost2 = 0.8f;//relation-entities preferred
                            pp.occur1 = Lucene.Net.Search.Occur.MUST;//the user-expression must occur
                            pp.occur2 = Lucene.Net.Search.Occur.SHOULD;//relation-entities should be used, but dont have to
                        }
                        SearchEntityResult[] ser = LuceneBO.getInstance().search(filter, input.sysperole.ToString(), "OPPO", null, pp, null);
                        if (ser == null || ser.Length == 0)
                            return null;

                        List<long> ids = (from sr in ser[0].results
                                          select long.Parse(sr.id)).ToList<long>();
                        if (ids.Count == 0) return null;

                        String idsIn = String.Join(",", ids);

                        SearchDao<OpportunityDto> sd = new SearchDao<OpportunityDto>();
                        String addFilter = "";
                        String pFilter = "";
                        if (sysidchildoppo != null)
                        {
                            addFilter = " and sysidchild=" + sysidchildoppo + " ";
                            pFilter = " or oppo.sysperson=" + sysidchildoppo;
                        }
                        List<OpportunityDto> result = sd.search("select OPPO.*, OPPOTP.NAME oppoTpBezeichnung, PERSON.NAME personName from CIC.OPPO OPPO, CIC.OPPOTP OPPOTP, CIC.PERSON PERSON, CIC.CRMNM CRMNM where OPPO.SYSOPPOTP  = OPPOTP.SYSOPPOTP(+) AND OPPO.sysperson    = PERSON.SYSPERSON and OPPO.SYSOPPO      =CRMNM.SYSIDPARENT(+) AND OPPO.SYSOPPO     IN ("
                            + idsIn + ") AND ((CRMNM.PARENTAREA  ='OPPO' AND CRMNM.CHILDAREA   ='PERSON' AND crmnm.sysidchild IS NOT NULL" + addFilter + ") " + pFilter + ")", null);


                        return result.Distinct(new EnityComparator<OpportunityDto>()).ToArray();


                    },
                    CreateBezeichnung = (item) => item.name,
                    CreateBeschreibung = (item,rval) => createPanel(rval,"OPPO", item.name, item.personName, item.description, "", item.indicatorContent, "" + item.entityId)

                });


                #endregion




            }
            else//old search without lucene
            {
                #region non-lucene
                dictionary.Add(XproEntityType.ACCOUNTS,
                               new XproInfoDao<AccountDto>()
                               {
                                   QueryItemFunction = (id) =>
                                   {
                                       var bo = CreateEntityBo();
                                       return bo.getAccountDetails(id);
                                   },
                                   QueryItemsFunction = (input) =>
                                   {
                                       QueryInfoData infoData = SearchQueryFactoryFactory.getInstance().getQueryInfo<AccountDto>();
                                       /*QueryInfoData infoData = new QueryInfoDataType1("PERSON", "PERSON.SYSPERSON");
                                       infoData.resultFields = "PERSON.SYSPERSON, PERSON.NAME, PERSON.CODE, PERSON.VORNAME,  PERSON.PLZ, PERSON.ORT, PERSON.STRASSE, PERSON.TELEFON, PERSON.HSNR";
                                       infoData.resultTables = "CIC.PERSON PERSON ";
                                       infoData.searchTables = "CIC.PERSON PERSON ";
                                       infoData.searchConditions = " person.name is not null";
                                       infoData.permissionCondition = " and person.sysperson in (SELECT sysid FROM peuni, perolecache WHERE area = 'PERSON' AND peuni.sysperole = perolecache.syschild AND perolecache.sysparent = {0} and {1}) ";
                                       infoData.queryStruct.partialQuery = "SELECT * FROM (SELECT rownum rnum, a.* FROM( select  " + hints[typeof(Cic.One.DTO.AccountDto)] + " {0} from {1} where {2} ) a WHERE rownum <= {3}) WHERE rnum > {4}";
                                       infoData.optimized = false;*/

                                       var bo = CreateSearchBo<AccountDto>(infoData, input.sysperole);
                                       Sorting[] sorting = new Sorting[] { new Sorting() { fieldname = "PERSON.NAME", order = SortOrder.Asc } };
                                       iSearchDto iSearch = CreateISearchDto(ACCOUNT_INDEX_FIELDS, input.Filter, input.filters, sorting);
                                       iSearch.pageSize = 5;
                                       iSearch.amount = 5;
                                       oSearchDto<AccountDto> rSearch = bo.search(iSearch);
                                       int addRows = 0;
                                       if (rSearch.searchCountMax > iSearch.amount)
                                       {
                                           addRows++;
                                       }
                                       AccountDto[] rval = new AccountDto[rSearch.results.Length + addRows];
                                       rSearch.results.CopyTo(rval, 0);


                                       if (addRows > 0)
                                       {
                                           rval[rSearch.results.Length] = new AccountDto();
                                           rval[rSearch.results.Length].name = "Mehr als 5 Treffer vorhanden. Verfeinern Sie die Suchanfrage";
                                       }
                                       return rval;

                                   },
                                   CreateBezeichnung = (item) => (item.vorname != null ? item.vorname + "  " : "") + item.name,
                                   CreateBeschreibung = (item,rval) => createPanel(rval,"PERSON", item.name + " " + item.vorname, item.strasse, item.plz + " " + item.ort, item.telefon, item.indicatorContent, "" + item.entityId)


                               }
                            );

                dictionary.Add(XproEntityType.PARTNER,
               new XproInfoDao<PartnerDto>()
               {
                   QueryItemFunction = (id) =>
                   {
                       var bo = CreateEntityBo();
                       return bo.getPartnerDetails(id);
                   },
                   QueryItemsFunction = (input) =>
                   {
                       //Zuerst personen mit partnerbeziehung
                       QueryInfoData infoData = new QueryInfoDataType1("PTRELATE", "PTRELATE.SYSPTRELATE");
                       infoData.resultFields = "PTRELATE.*,person2.NAME beziehungzu, PERSON.SYSPERSON, PERSON.NAME, PERSON.CODE, PERSON.VORNAME,  PERSON.PLZ, PERSON.ORT, PERSON.STRASSE, PERSON.TELEFON, PERSON.HSNR";
                       infoData.resultTables = " CIC.PERSON PERSON,CIC.PTRELATE PTRELATE, CIC.PERSON PERSON2 ";
                       infoData.searchTables = " CIC.PERSON PERSON,CIC.PTRELATE PTRELATE, CIC.PERSON PERSON2 ";
                       infoData.searchConditions = "PERSON.SYSPERSON=PTRELATE.SYSPERSON2(+) and   ptrelate.sysperson1=person2.sysperson and person.name is not null";
                       infoData.queryStruct.partialQuery = "SELECT * FROM (SELECT rownum rnum, a.* FROM( select  " + hints[typeof(Cic.One.DTO.AccountDto)] + " {0} from {1} where {2} ) a WHERE rownum <= {3}) WHERE rnum > {4}";
                       infoData.optimized = false;

                       Sorting[] sorting = new Sorting[] { new Sorting() { fieldname = "PERSON.NAME", order = SortOrder.Asc } };
                       var bo = CreateSearchBo<PartnerDto>(infoData, input.sysperole);
                       iSearchDto iSearch = CreateISearchDto(ACCOUNT_INDEX_FIELDS, input.Filter, input.filters, sorting);
                       iSearch.pageSize = 15;
                       iSearch.amount = 15;
                       PartnerDto[] rval1 = bo.search(iSearch).results;

                       //dann potentiell neue partner aus personentabelle die noch gar keine beziehung haben, hier aber nur 20 stück
                       infoData = new QueryInfoDataType1("PERSON", "PERSON.SYSPERSON");
                       infoData.resultFields = "PERSON.SYSPERSON*-1 AS SYSPTRELATE, PERSON.SYSPERSON, PERSON.NAME, PERSON.CODE, PERSON.VORNAME,  PERSON.PLZ, PERSON.ORT, PERSON.STRASSE, PERSON.TELEFON, PERSON.HSNR";
                       infoData.resultTables = "CIC.PERSON PERSON ";
                       infoData.searchTables = "CIC.PERSON PERSON ";
                       infoData.searchConditions = " person.name is not null";
                       infoData.queryStruct.partialQuery = "SELECT * FROM (SELECT rownum rnum, a.* FROM( select  " + hints[typeof(Cic.One.DTO.AccountDto)] + " {0} from {1} where {2} ) a WHERE rownum <= {3}) WHERE rnum > {4}";
                       infoData.optimized = false;
                       bo = CreateSearchBo<PartnerDto>(infoData, input.sysperole);
                       iSearch = CreateISearchDto(ACCOUNT_INDEX_FIELDS, input.Filter, sorting);
                       iSearch.pageSize = 20;
                       iSearch.amount = 20;
                       oSearchDto<PartnerDto> rSearch = bo.search(iSearch);

                       int addRows = 1;
                       PartnerDto[] rval2 = rSearch.results;
                       if (rSearch.searchCountMax > iSearch.amount)
                       {
                           addRows++;
                       }

                       PartnerDto[] rval = new PartnerDto[rval1.Length + rval2.Length + addRows];
                       rval1.CopyTo(rval, 0);
                       rval[rval1.Length] = new PartnerDto();
                       rval2.CopyTo(rval, rval1.Length + 1);
                       if (rSearch.searchCountMax > iSearch.amount)
                       {
                           rval[rval.Length - 1] = new PartnerDto();
                           rval[rval.Length - 1].name = "Mehr Treffer vorhanden. Verfeinern Sie ggf die Suchanfrage";
                       }
                       return rval;
                   },
                   CreateBezeichnung = (item) => (item.vorname != null ? item.vorname + " " : "") + item.name,
                   CreateBeschreibung = (item,rval) => createPanel(rval,"PERSON", item.name + " " + item.vorname, item.strasse, item.plz + " " + item.ort, item.beziehung != null ? (item.beziehung + " zu " + item.beziehungzu + ", " + item.beziehungstyp + ", " + item.beziehungsfunktion) : "", item.indicatorContent, "" + item.entityId)

               }
            );
                dictionary.Add(XproEntityType.BETEILIGTER,
               new XproInfoDao<BeteiligterDto>()
               {
                   QueryItemFunction = (id) =>
                   {
                       var bo = CreateEntityBo();
                       return bo.getBeteiligterDetails(id);
                   },
                   QueryItemsFunction = (input) =>
                   {
                       QueryInfoData infoData = new QueryInfoDataType1("CRMNM", "CRMNM.SYSCRMNM");
                       infoData.resultFields = "CRMNM.*,PERSON.SYSPERSON, PERSON.NAME, PERSON.CODE, PERSON.VORNAME,  PERSON.PLZ, PERSON.ORT, PERSON.STRASSE, PERSON.TELEFON, PERSON.HSNR";
                       infoData.resultTables = "CIC.CRMNM CRMNM, CIC.PERSON PERSON ";
                       infoData.searchTables = "CIC.CRMNM CRMNM, CIC.PERSON PERSON ";
                       infoData.searchConditions = "CRMNM.SYSIDCHILD = PERSON.SYSPERSON(+) and CRMNM.PARENTAREA='OPPO' AND  CRMNM.CHILDAREA='PERSON' and person.name is not null";
                       infoData.queryStruct.partialQuery = "SELECT * FROM (SELECT rownum rnum, a.* FROM( select  " + hints[typeof(Cic.One.DTO.AccountDto)] + " {0} from {1} where {2} ) a WHERE rownum <= {3}) WHERE rnum > {4}";
                       infoData.optimized = false;

                       Sorting[] sorting = new Sorting[] { new Sorting() { fieldname = "PERSON.NAME", order = SortOrder.Asc } };
                       var bo = CreateSearchBo<BeteiligterDto>(infoData, input.sysperole);
                       iSearchDto iSearch = CreateISearchDto(ACCOUNT_INDEX_FIELDS, input.Filter, input.filters, sorting);
                       iSearch.pageSize = 5;
                       iSearch.amount = 5;
                       BeteiligterDto[] rval1 = bo.search(iSearch).results;


                       infoData = new QueryInfoDataType1("PERSON", "PERSON.SYSPERSON");
                       infoData.resultFields = "PERSON.SYSPERSON*-1 AS SYSCRMNM, PERSON.SYSPERSON, PERSON.NAME, PERSON.CODE, PERSON.VORNAME,  PERSON.PLZ, PERSON.ORT, PERSON.STRASSE, PERSON.TELEFON, PERSON.HSNR";
                       infoData.resultTables = "CIC.PERSON PERSON ";
                       infoData.searchTables = "CIC.PERSON PERSON ";
                       infoData.searchConditions = " person.name is not null";
                       infoData.queryStruct.partialQuery = "SELECT * FROM (SELECT rownum rnum, a.* FROM( select  " + hints[typeof(Cic.One.DTO.AccountDto)] + " {0} from {1} where {2} ) a WHERE rownum <= {3}) WHERE rnum > {4}";
                       infoData.optimized = false;
                       bo = CreateSearchBo<BeteiligterDto>(infoData, input.sysperole);
                       iSearch = CreateISearchDto(ACCOUNT_INDEX_FIELDS, input.Filter, sorting);
                       iSearch.pageSize = 5;
                       iSearch.amount = 5;
                       oSearchDto<BeteiligterDto> rSearch = bo.search(iSearch);

                       int addRows = 1;
                       BeteiligterDto[] rval2 = rSearch.results;
                       if (rSearch.searchCountMax > iSearch.amount)
                       {
                           addRows++;
                       }

                       BeteiligterDto[] rval = new BeteiligterDto[rval1.Length + rval2.Length + addRows];
                       rval1.CopyTo(rval, 0);
                       rval[rval1.Length] = new BeteiligterDto();
                       rval2.CopyTo(rval, rval1.Length + 1);
                       if (rSearch.searchCountMax > iSearch.amount)
                       {
                           rval[rval.Length - 1] = new BeteiligterDto();
                           rval[rval.Length - 1].name = "Mehr als 5 Treffer vorhanden. Verfeinern Sie die Suchanfrage";
                       }
                       return rval;
                   },
                   CreateBezeichnung = (item) => (item.vorname != null ? item.vorname + " " : "") + item.name,
                   CreateBeschreibung = (item,rval) => createPanel(rval,"PERSON", item.name + " " + item.vorname, item.strasse, item.plz + " " + item.ort, item.syscrmnm > 0 ? "Beteiligter" : "", item.indicatorContent, "" + item.entityId),

               }
            );
                dictionary.Add(XproEntityType.OPPORTUNITY, new XproInfoDao<OpportunityDto>()
                {
                    QueryItemFunction = (id) =>
                    {
                        var bo = CreateEntityBo();
                        return bo.getOpportunityDetails(id);
                    },
                    QueryItemsFunction = (input) =>
                    {
                        var bo = CreateSearchBo<OpportunityDto>(input.sysperole);

                        OpportunityDto[] rval1 = bo.search(CreateISearchDto("OPPO.NAME, OPPO.DESCRIPTION", input.Filter, input.filters)).results;
                        List<OpportunityDto> rList1 = new List<OpportunityDto>(rval1);

                        //if a person-filter is given we can also look in crmnm
                        long sysperson = 0;
                        if (input.filters != null)
                        {

                            List<Filter> filters = new List<Filter>();
                            foreach (Filter f in input.filters)
                            {
                                if (f.fieldname.ToLower().IndexOf("sysperson") > -1)
                                {
                                    sysperson = long.Parse(input.filters[0].value);
                                }
                                else filters.Add(f);
                            }
                            input.filters = filters.ToArray();
                        }

                        QueryInfoDataType1 infoData = new QueryInfoDataType1("OPPO", "OPPO.SYSOPPO");
                        infoData.resultFields = "OPPO.*, OPPOTP.NAME oppoTpBezeichnung, PERSON.NAME personName ";
                        infoData.resultTables = "CIC.OPPO OPPO, CIC.OPPOTP OPPOTP, CIC.PERSON PERSON, CIC.CRMNM CRMNM";
                        infoData.searchTables = "CIC.OPPO OPPO, CIC.OPPOTP OPPOTP, CIC.PERSON PERSON, CIC.CRMNM CRMNM";
                        if (sysperson > 0)
                            infoData.searchConditions = " OPPO.SYSOPPOTP = OPPOTP.SYSOPPOTP(+) and crmnm.sysidchild = PERSON.SYSPERSON and OPPO.SYSOPPO=CRMNM.SYSIDPARENT and CRMNM.PARENTAREA='OPPO' and CRMNM.CHILDAREA='PERSON' and crmnm.sysidchild is not null and crmnm.sysidchild=" + sysperson;
                        else infoData.searchConditions = " OPPO.SYSOPPOTP = OPPOTP.SYSOPPOTP(+) and crmnm.sysidchild = PERSON.SYSPERSON and OPPO.SYSOPPO=CRMNM.SYSIDPARENT and CRMNM.PARENTAREA='OPPO' and CRMNM.CHILDAREA='PERSON' and crmnm.sysidchild is not null ";

                        bo = CreateSearchBo<OpportunityDto>(infoData, input.sysperole);
                        iSearchDto iSearch = CreateISearchDto(ACCOUNT_INDEX_FIELDS, input.Filter, input.filters);
                        iSearch.searchType = Cic.One.DTO.SearchType.Complete;
                        OpportunityDto[] rval2 = bo.search(iSearch).results;
                        List<OpportunityDto> rList2 = new List<OpportunityDto>(rval2);

                        return rList1.Union(rval2, new EnityComparator<OpportunityDto>()).ToArray<OpportunityDto>();


                    },
                    CreateBezeichnung = (item) => item.name,
                    CreateBeschreibung = (item,rval) => createPanel(rval,"OPPO", item.name, item.personName, "", "", item.indicatorContent, "" + item.entityId)

                });
                #endregion
            }//end old search without lucene
            

            dictionary.Add(XproEntityType.LAND, new XproInfoDao()
            {
                QueryItemFunction2 = (input) =>
                {
                    var bo = CreateDictionaryListsBo(input.isoCode);
                    return bo.listLaender().FirstOrDefault((a) => a.sysID == input.EntityId);
                },
                QueryItemsFunction = (filter) =>
                {
                    var bo = CreateDictionaryListsBo(filter.isoCode);
                    //already sorted by favorites!!!
                    return bo.listLaender().Where(a => a.bezeichnung.ToLower().StartsWith(filter.Filter.ToLower())).ToArray();
                        //.OrderBy(o => o.bezeichnung).ToArray();

                },
            });


            

            dictionary.Add(XproEntityType.STRASSE, new XproInfoDao<StrasseDto>()
                               {
                                   QueryItemFunction = (id) =>
                                   {
                                       var bo = CreateEntityBo();
                                       return bo.getStrasseDetails(id);
                                   },
                                   QueryItemsFunction = (input) =>
                                   {
                                       QueryInfoData infoData = SearchQueryFactoryFactory.getInstance().getQueryInfo<StrasseDto>();

                                       var bo = CreateSearchBo<StrasseDto>(infoData, input.sysperole);
                                      
                                       List<Filter> filterList = new List<Filter>();
                                       Filter f = new Filter();
                                       f.fieldname = "STRASSE.STRBEZ";
                                       f.filterType = FilterType.Begins;
                                       f.value = input.Filter;
                                       if (f.value != null && f.value.Length > 5) 
                                            f.value = f.value.Substring(0, 5);
                                       if (f.value != null && f.value.Length > 0) 
                                           filterList.Add(f);

                                       if (input.filters.Length > 0)
                                       {
                                           String fieldValues = (from s in input.filters
                                                                 where s.fieldname.ToLower().IndexOf("plz,ort") > -1
                                                                 select s.value).FirstOrDefault();
                                           String[] vals = fieldValues.Split(',');
                                           f = new Filter();
                                           f.fieldname = "STRASSE.PLZ";
                                           f.filterType = FilterType.Begins;
                                           f.value = vals[0];
                                           if (vals[0] != null && vals[0].Length > 0)
                                               filterList.Add(f);
                                           f = new Filter();
                                           f.fieldname = "PLZ.ORT";
                                           f.filterType = FilterType.Begins;
                                           f.value = vals[1];
                                           if (vals[1] != null && vals[1].Length > 0)
                                               filterList.Add(f);

                                       }
                                       Filter[] filters = filterList.ToArray();

                                       iSearchDto iSearch = CreateISearchDto("STRASSE.STRBEZ", null, filters, null);
                                       iSearch.pageSize = 20;
                                       iSearch.amount = 20;
                                       oSearchDto<StrasseDto> rSearch = bo.search(iSearch);
                                       int addRows = 0;
                                       if (rSearch.searchCountMax > iSearch.amount)
                                       {
                                           addRows++;
                                       }
                                       StrasseDto[] rval = new StrasseDto[rSearch.results.Length + addRows];
                                       rSearch.results.CopyTo(rval, 0);


                                       if (addRows > 0)
                                       {
                                           rval[rSearch.results.Length] = new StrasseDto();
                                           rval[rSearch.results.Length].strasse = "Mehr als 20 Treffer vorhanden. Verfeinern Sie die Suchanfrage";
                                       }
                                       return rval;

                                   },
                                   CreateBezeichnung = (item) => (item.plz!= null ? item.strasse + "  " : "") + item.plz,
                                   CreateBeschreibung = (item,rval) => createPanel(rval,"STRASSE",item.strasse,""+item.plz+" "+item.ort,item.bezirk+" - "+item.landBezeichnung,"","", "" + item.entityId)

                         });


            dictionary.Add(XproEntityType.STAAT, new XproInfoDao()
            {
                QueryItemFunction2 = (input) =>
                {
                    var bo = CreateDictionaryListsBo(input.isoCode);
                    return bo.listKantone(input.isoCode).FirstOrDefault((a) => a.sysID == input.EntityId);
                },
                QueryItemsFunction = (input) =>
                {

                    var bo = CreateDictionaryListsBo(input.isoCode);

                    return bo.listKantone(input.isoCode);

                },
            });

            dictionary.Add(XproEntityType.BRANCHE, new XproInfoDao<DropListDto>()
            {
                QueryItemFunction2 = (input) =>
                {
                    var bo = CreateDictionaryListsBo(input.isoCode);
                    return bo.listBranchen().FirstOrDefault((a) => a.sysID == input.EntityId);
                },
                QueryItemsFunction = (filter) =>
                {
                    var bo = CreateDictionaryListsBo(filter.isoCode);
                    //DropListDto[] bol = bo.listBranchen();
                    //foreach (DropListDto i in bol)
                    //{
                    //    i.beschreibung = i.bezeichnung;
                    //}
                    return bo.listBranchen().Where(a => a.bezeichnung != null && a.bezeichnung.ToLower().Contains(filter.Filter.ToLower())).OrderBy(a=>a.bezeichnung).ToArray<DropListDto>();
                },
                CreateBeschreibung = (item,rval) => item.bezeichnung,
            });



            //INTSTRCT
            dictionaryCode.Add("INTSTRCT", new XproInfoDao<DropListDto>()
            {
                QueryItemFunction = (id) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = id });
                        return ctx.ExecuteStoreQuery<DropListDto>("select intstrct.name bezeichnung, intstrct.sysintstrct sysid,  intstrct.sysintstrct code from INTSTRCT where sysintstrct =:id", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        return ctx.ExecuteStoreQuery<DropListDto>("select intstrct.description bezeichnung, intstrct.sysintstrct sysid,  intstrct.sysintstrct code from INTSTRCT order by sysintstrct", null).ToArray();
                    }
                },
                CreateBezeichnung = (item) => item.bezeichnung,
                CreateBeschreibung = (item, rval) => createPanel(rval, "INTSTRCT", item.bezeichnung, "", "", "", "", "" + item.sysID),
            });


            //RISIKOKLASSE
            dictionaryCode.Add("RISIKOKLASSE", new XproInfoDao<DropListDto>()
            {
                QueryItemFunction = (id) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = id });
                        return ctx.ExecuteStoreQuery<DropListDto>("select risikokl.bezeichnung bezeichnung, risikokl.sysrisikokl sysid, risikokl.code code from RISIKOKL where sysrisikokl =:id", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        return ctx.ExecuteStoreQuery<DropListDto>("select risikokl.bezeichnung bezeichnung, risikokl.sysrisikokl sysid, risikokl.code code from RISIKOKL", null).ToArray();
                    }
                },
                CreateBezeichnung = (item) => item.bezeichnung,
                CreateBeschreibung = (item, rval) => createPanel(rval, "RISIKOKLASSE", item.bezeichnung, "", "", "", "", "" + item.sysID),
            });




            dictionary.Add(XproEntityType.CTLANG, new XproInfoDao()
            {
                QueryItemFunction2 = (input) =>
                {
                    var bo = CreateDictionaryListsBo(input.isoCode);
                    return bo.listSprachen().FirstOrDefault((a) => a.sysID == input.EntityId);
                },
                QueryItemsFunction = (filter) =>
                {
                    var bo = CreateDictionaryListsBo(filter.isoCode);
                    return bo.listSprachen();
                },
            });
            //Replace the original Follow_type search.
            dictionary.Remove(XproEntityType.FOLLOW_TYPE);
            dictionary.Add(XproEntityType.FOLLOW_TYPE, new XproInfoDao()
            {
                QueryItemFunction2 = (input) =>
                {
                    var bo = CreateDictionaryListsBo(input.isoCode);
                    return bo.listById(DDLKPPOSType.FOLLOW_TYPE).FirstOrDefault((a) => a.sysID == input.EntityId);
                },
                QueryItemsFunction = (filter) =>
                {
                    String prefix = "-1";
                    if (filter.Filter != null)
                        prefix = filter.Filter.ToUpper() + "_";
                    var bo = CreateDictionaryListsBo(filter.isoCode);
                    return bo.listById(DDLKPPOSType.FOLLOW_TYPE).Where(a => a.code.StartsWith(prefix)).ToArray<DropListDto>();
                },
            });


            dictionary.Add(XproEntityType.CAMP, new XproInfoDao<CampDto>()
            {
                QueryItemFunction = (id) =>
                {
                    var bo = CreateEntityBo();
                    return bo.getCampDetails(id);
                },
                QueryItemsFunction = (input) =>
                {
                    var bo = CreateSearchBo<CampDto>();
                    if (input.filters == null || input.Filter==null)
                        return new CampDto[0];

                    

                    try
                    {
                        var results = bo.search(CreateISearchDto("CAMP.description,CAMP.name", input.Filter, "CAMP.name", SortOrder.Asc, input.filters.ToList())).results;
                        if (input.filters.Length > 0)
                        {
                            long val = Convert.ToInt32(input.filters[0].value);

                            var list = results.ToList();
                            RemoveResults(list, val);
                            return list.ToArray();
                        }
                        return results;
                    }
                    catch
                    {
                        return new CampDto[0];
                    }
                   
                },
                CreateBezeichnung = (item) => item.name,
                // CreateBeschreibung = (item) => item.name + " - " + item.description + " - " + item.status + " - " + item.actualCosts,
                CreateBeschreibung = (item,rval) => createPanel(rval,"CAMP", item.name, item.description, item.status.ToString(), item.actualCosts.ToString(), item.indicatorContent, "" + item.entityId),
            });


            dictionary.Add(XproEntityType.ANTRAGZUSTAND, new XproInfoDao<AntragzustandDto>()
            {
                QueryItemFunction = (id) =>
                {
                    //var bo = CreateCRMEntityBo();
                    //return bo.getWfuserDetails(id);
                    return null;
                },
                QueryItemsFunction = (input) =>
                {
                    var bo = CreateSearchBo<AntragzustandDto>();
                    return bo.search(CreateISearchDto("ANTRAG.ZUSTAND", input.Filter)).results;
                },
                CreateBezeichnung = (item) => item.zustand.Trim(),
                CreateBeschreibung = (item,rval) => createPanel(rval,"Antragzustand", item.zustand.Trim(), "", "", "", "", ""),
            });

            dictionary.Add(XproEntityType.VTZUSTAND, new XproInfoDao<XproEntityDto>()
            {
                QueryItemFunction = (id) =>
                {
                    return new XproEntityDto(id, id.ToString(), id.ToString());
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        return ctx.ExecuteStoreQuery<XproEntityDto>("select zustand title, min(rownum) sysid from vt where not zustand is null group by zustand order by zustand").ToArray();
                    }
                }
            });

            dictionary.Add(XproEntityType.ECODE178STATUS, new XproInfoDao<DropListDto>()
            {
                QueryItemFunction = (id) =>
                {
                    return new XproEntityDto(id, id.ToString(), id.ToString());
                    /*DropListDto ret=new DropListDto();
                    ret.sysID=1;
                    ret.bezeichnung=""+id;
                    ret.code = "" + id;
                    return ret;*/
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        return ctx.ExecuteStoreQuery<DropListDto>("select value bezeichnung, tooltip beschreibung, domainid code, sysddlkppos sysid from ddlkppos where code='ECODE178STATUS' and activeflag=1").ToArray();
                        //return ctx.ExecuteStoreQuery<DropListDto>("select designcode code, sysptasktyp sysid, description beschreibung, name bezeichnung from ptasktyp where activeflag=1 order by rang", null).ToArray();
                    }
                }
            });

            dictionary.Add(XproEntityType.ZEK_SICHERSTELLUNG, new XproInfoDao<XproEntityDto>()
            {
                QueryItemFunction = (id) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = id });
                        return ctx.ExecuteStoreQuery<XproEntityDto>("select value title, id sysid from ddlkppos where activeflag=1 and code='ZEK_SICHERSTELLUNG' and id=:id", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        return ctx.ExecuteStoreQuery<XproEntityDto>("select value title, id sysid from ddlkppos where activeflag=1 and code='ZEK_SICHERSTELLUNG'").ToArray();
                    }
                }
            });

            dictionaryCode.Add("ZEK_IKO_ABFRAGE", new XproInfoDao<XproEntityDto>()
            {
                QueryItemFunction = (id) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = id });
                        return ctx.ExecuteStoreQuery<XproEntityDto>("select value title, id sysid from ddlkppos where activeflag=1 and code='ZEK_IKO_ABFRAGE' and id=:id", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        return ctx.ExecuteStoreQuery<XproEntityDto>("select value title, id sysid from ddlkppos where activeflag=1 and code='ZEK_IKO_ABFRAGE'").ToArray();
                    }
                }
            });

          

            dictionary.Add(XproEntityType.ATTRIBUT, new XproInfoDao<XproEntityDto>()
            {
                QueryItemFunction = (id) =>
                {
                    return new XproEntityDto(id, id.ToString(), id.ToString());
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        return ctx.ExecuteStoreQuery<XproEntityDto>("select ATTRIBUT title, min(rownum) sysid from vt where not ATTRIBUT is null group by ATTRIBUT order by attribut").ToArray();
                    }
                }
            });

            dictionary.Add(XproEntityType.WFUSER, new XproInfoDao<WfuserDto>()
            {
                QueryItemFunction = (id) =>
                {
                    var bo = CreateEntityBo();
                    return bo.getWfuserDetails(id);
                },
                QueryItemsFunction = (input) =>
                {
                    var bo = CreateSearchBo<WfuserDto>();
                    return bo.search(CreateISearchDto("WFUSER.name,WFUSER.vorname,WFUSER.code", input.Filter, "WFUSER.NAME", SortOrder.Asc)).results;
                },
                CreateBezeichnung = (item) => item.name + " " + item.vorname,
                CreateBeschreibung = (item,rval) => createPanel(rval,"WFUSER", item.name + " " + item.vorname, item.code, "", "", item.indicatorContent, "" + item.entityId),
            });
            dictionary.Add(XproEntityType.PUSER, new XproInfoDao<WfuserDto>()
            {
                QueryItemFunction = (id) =>
                {
                    var bo = CreateEntityBo();
                    return bo.getWfuserDetails(id);
                },
                QueryItemsFunction = (input) =>
                {
                    IAuthenticationBo bo = BOFactoryFactory.getInstance().getAuthenticationBo();
                    return bo.getPermittedUsers(input.sysperole);
                },
                CreateBezeichnung = (item) => item.name + " " + item.vorname,
                CreateBeschreibung = (item, rval) => createPanel(rval, "PUSER", item.name + " " + item.vorname, item.code, "", "", item.indicatorContent, "" + item.entityId),
            });




            dictionary.Add(XproEntityType.OPPO_TYP, new XproInfoDao<OppotpDto>()
            {
                QueryItemFunction = (id) =>
                {
                    var bo = CreateEntityBo();
                    return bo.getOppotpDetails(id);
                },
                QueryItemsFunction = (input) =>
                {
                    var bo = CreateSearchBo<OppotpDto>();
                    return bo.search(CreateISearchDto("OPPOTP.NAME, OPPOTP.DESCRIPTION", input.Filter)).results;
                },
                CreateBezeichnung = (item) => item.name,
                CreateBeschreibung = (item,rval) => createPanel(rval,"OPPOTP", item.name, item.description, "", "", item.indicatorContent, "" + item.entityId),

            });

            dictionary.Add(XproEntityType.ZINSTAB, new XproInfoDao<ZinstabDto>()
           {
               QueryItemFunction = (id) =>
               {
                   var bo = CreateEntityBo();
                   return bo.getZinstabDetails(id);
               },
               QueryItemsFunction = (input) =>
               {
                   var bo = CreateSearchBo<ZinstabDto>();
                   return bo.search(CreateISearchDto("ZINSTAB.SYSZINSTAB", input.Filter)).results;
               },
               CreateBezeichnung = (item) => item.bezeichnung,
               CreateBeschreibung = (item,rval) => createPanel(rval,"ZINSTAB", item.bezeichnung, "", "", "", item.indicatorContent, "" + item.entityId),
           });

            dictionary.Add(XproEntityType.CAMP_TYP, new XproInfoDao<CamptpDto>()
            {
                QueryItemFunction = (id) =>
                {
                    var bo = CreateEntityBo();
                    return bo.getCamptpDetails(id);
                },
                QueryItemsFunction = (input) =>
                {
                    var bo = CreateSearchBo<CamptpDto>();
                    return bo.search(CreateISearchDto("CAMPTP.NAME, CAMPTP.DESCRIPTION", input.Filter)).results;
                },
                CreateBezeichnung = (item) => item.name,
                CreateBeschreibung = (item,rval) => createPanel(rval,"CAMPTP", item.name, item.description, "", "", item.indicatorContent, "" + item.entityId),

            });

            dictionary.Add(XproEntityType.ADRTP, new XproInfoDao<AdrtpDto>()
            {
                QueryItemFunction = (id) =>
                {
                    var bo = CreateEntityBo();
                    return bo.getAdrtpDetails(id);
                },
                QueryItemsFunction = (input) =>
                {
                    var bo = CreateSearchBo<AdrtpDto>();
                    return bo.search(CreateISearchDto("ADRTYP.BEZEICHNUNG", input.Filter)).results;
                },
                CreateBezeichnung = (item) => item.bezeichnung,
                CreateBeschreibung = (item,rval) => createPanel(rval,"ADRTP", item.bezeichnung, item.beschreibung, "", "", "", "" + item.entityId)

            });

            dictionary.Add(XproEntityType.EXCHANGE_CATEGORIES, CreateXproInfo<ItemcatDto>("ITEMCAT.NAME, ITEMCAT.DESCRIPTION, ITEMCAT.DESIGNCODE",
                item => item.name,
                (item,rval) => createPanel(rval,"Exchangekat", item.name, item.description, "", "", "", "" + item.entityId)
            ));



            #region exchange
            dictionary.Add(XproEntityType.EXCHANGE_CONTACTS, new XproInfoDao<XproEntityDto>()
            {
                QueryItemFunction = (id) => null,
                QueryItemsFunction = (input) =>
                    {
                        
                        var member = new CredentialContext().getMembershipInfo();
                        var mailDao = MailDaoFactory.getInstance().getMailDao(member.sysWFUSER);

                        EWSMailBo bo = new EWSMailBo(mailDao);
                        if (mailDao == null || mailDao.getUser() == null)
                        {
                            throw new Exception("Für den Benutzer ist kein Exchange Account hinterlegt.");
                        }

                        var result = bo.FindContacts(new ifindContact() { ResolveNameSearchLocation = MResolveNameSearchLocationEnum.ContactsThenDirectory, NameToResolve = input.Filter, ReturnContactDetails = false });

                        //string emailRegex = "(^([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$)";
                        //Simplerer Regex
                        string emailRegex = @"\b[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\b";
                        Match m = Regex.Match(input.Filter, emailRegex, RegexOptions.IgnoreCase);

                        if (m.Success)
                            result.Items.Insert(0, new MNameResolutionDto() { Mailbox = new MEmailAddressDto() { Name = m.Value, Address = m.Value } });

                        
                        List<AccountDto> accounts = null;
                        if (LuceneFactory.getInstance().indexAvailable())
                        {
                            String filter = input.Filter != null ? input.Filter : "*";
                            if (!filter.EndsWith("*"))
                                filter = filter + "*";

                            SearchEntityResult[] ser = new CIC.ASS.SearchService.BO.SearchBO().searchEntities(filter, input.sysperole.ToString(), "Account", null);
                            if (ser != null && ser.Length == 1)
                            {
                                List<long> ids = (from sr in ser[0].results
                                                  select long.Parse(sr.id)).ToList<long>();
                                if (ids.Count > 0)
                                {

                                    String idsIn = String.Join(",", ids);

                                    SearchDao<AccountDto> sd = new SearchDao<AccountDto>();

                                    accounts = sd.search("select person.name,person.vorname,person.email from CIC.PERSON PERSON where person.email is not null and person.sysperson in ("
                                        + idsIn + ") ", null);
                                }

                            }
                        }
                        else
                        {

                            QueryInfoData infoData = new QueryInfoDataType1("PERSON", "PERSON.SYSPERSON");
                            infoData.resultFields = "PERSON.NAME, PERSON.VORNAME, PERSON.EMAIL";
                            infoData.resultTables = "CIC.PERSON PERSON ";
                            infoData.searchTables = "CIC.PERSON PERSON ";
                            infoData.searchConditions = " person.name is not null and person.email is not null";
                            infoData.permissionCondition = " and person.sysperson in (SELECT sysid FROM peuni, perolecache WHERE area = 'PERSON' AND peuni.sysperole = perolecache.syschild AND perolecache.sysparent = {0} and {1}) ";
                            infoData.queryStruct.partialQuery = "SELECT * FROM (SELECT rownum rnum, a.* FROM( select {0} from {1} where {2} ) a WHERE rownum <= {3}) WHERE rnum > {4}";

                            var searchBo = CreateSearchBo<AccountDto>(infoData, input.sysperole);
                            accounts = searchBo.search(CreateISearchDto("PERSON.NAME, PERSON.CODE, PERSON.VORNAME, PERSON.EMAIL", input.Filter, "PERSON.NAME", SortOrder.Asc)).results.ToList();

                        }
                        foreach(MNameResolutionDto rd in result.Items)
                        {
                            if(!mailboxIds.ContainsKey(rd.Mailbox.Address))
                            {
                                mailboxIds[rd.Mailbox.Address] = lastMailId++;
                            }

                        }
                        List<XproEntityDto> rval = new List<XproEntityDto>();
                        var conv = from r in result.Items
                                   select new XproEntityDto()
                                   {
                                       bezeichnung = r.Mailbox.Name,
                                       beschreibung =  r.Mailbox.Name+";"+r.Mailbox.Address,
                                       desc1 = r.Mailbox.Name,
                                       title = r.Mailbox.Name + ";" + r.Mailbox.Address,
                                       sysID = mailboxIds[r.Mailbox.Address]
                                   };
                        rval.AddRange(conv);
                        if (accounts != null)
                        {
                            var results = from account in accounts
                                          let match = Regex.Match(account.email, emailRegex, RegexOptions.IgnoreCase)
                                          where match.Success
                                          select new XproEntityDto()
                                          {
                                              bezeichnung =  (account.vorname != null ? account.vorname + " " : "") + account.name,
                                              beschreibung = (account.vorname != null ? account.vorname + " " : "") + account.name+";"+match.Value,
                                              desc1 = match.Value,
                                              title =  (account.vorname != null ? account.vorname + " " : "") + account.name+";"+match.Value,
                                              sysID = account.sysperson
                                          };

                            rval.AddRange(results);

                        }
                        return rval.ToArray();
                    }/*,
                CreateBezeichnung = item => item.Mailbox.Name + ";" + item.Mailbox.Address,
                
                CreateBeschreibung = (item,rval) => createPanel(rval,"MAILMSG", item.Mailbox.Name, item.Mailbox.Address, "", "", "", ""),*/

                 

            });
            #endregion

            dictionary.Add(XproEntityType.ADMADD, new XproInfoDao<AdmaddDto>()
            {
                QueryItemFunction = (id) =>
                {
                    var bo = CreateEntityBo();
                    return bo.getAdmaddDetail(id);
                },
                QueryItemsFunction = (input) =>
                {
                    var bo = CreateSearchBo<AdmaddDto>();
                    return bo.search(CreateISearchDto("ADMADD.BEZEICHNUNG,ADMADD.ORGA", input.Filter)).results;
                },
                CreateBezeichnung = (item) => item.bezeichnung,
                CreateBeschreibung = (item,rval) => createPanel(rval,"ADMADD", item.bezeichnung, item.orga, "", "", item.indicatorContent, "" + item.entityId),

            });
            dictionary.Add(XproEntityType.KONTO_TYP, CreateXproInfo<KontotpDto>(null,
                item => item.bezeichnung,
                (item,rval) => createPanel(rval,"KONTOTP", item.bezeichnung, item.beschreibung, (item.rang == 0) ? "" : ("<br/>Rang: " + item.rang), "", item.indicatorContent, "")
            ));

            dictionary.Add(XproEntityType.RAHMEN, CreateXproInfo<RahmenDto>(null,
              item => item.rahmen,
              (item, rval) => createPanel(rval, "RVT", item.rahmen, item.beschreibung, "", "", item.indicatorContent, "")
          ));

            dictionary.Add(XproEntityType.KONTO_BLZ, CreateXproInfo<BlzDto>("BLZ.SYSBLZ, BLZ.NAME, BLZ.BIC",
                item => item.name,
                (item, rval) => createPanel(rval, "BLZ", item.name, item.blz,item.bic, "", item.indicatorContent, ""),
                new Sorting[] { new Sorting() { fieldname = "BLZ.NAME", order = SortOrder.Asc } }
            ));
            dictionary.Add(XproEntityType.BANK_BLZ, CreateXproInfo<BlzDto>("BLZ.BLZ, BLZ.NAME, BLZ.BIC",
                item => item.name,
                (item, rval) => createPanel(rval, "BLZ", item.name, item.blz, item.bic, "", item.indicatorContent, ""),
                new Sorting[] { new Sorting() { fieldname = "BLZ.NAME", order = SortOrder.Asc } }
            ));


            dictionary.Add(XproEntityType.CONTACT_TYP, CreateXproInfo<ContacttpDto>(null,
                item => item.name,
                (item, rval) => createPanel(rval, "CONTACTTP", item.name, item.designCode, "", "", item.indicatorContent, ""),
                new Sorting[] { new Sorting() { fieldname = "CONTACTTP.NAME", order = SortOrder.Asc } }
            ));
            dictionary.Add(XproEntityType.PTASK_TYP, new XproInfoDao<DropListDto>()
            {
                QueryItemFunction = (id) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysptasktp", Value = id });
                        return ctx.ExecuteStoreQuery<DropListDto>("select designcode code, sysptasktyp sysid, description beschreibung, name bezeichnung from ptasktyp where sysptasktyp=:sysptasktp", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {

                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        return ctx.ExecuteStoreQuery<DropListDto>("select designcode code, sysptasktyp sysid, description beschreibung, name bezeichnung from ptasktyp where activeflag=1 order by rang", null).ToArray();
                    }
                },
                CreateBezeichnung = (item) => item.bezeichnung,
                CreateBeschreibung = (item,rval) => createPanel(rval,"PTASKTYP", item.bezeichnung, item.beschreibung, item.code, "", "", "" + item.sysID),
            });

            //Fahrzeugsuche
            dictionary.Add(XproEntityType.OBTYP, new XproInfoDao<XproEntityDto>()//dieser typ definiert sein descriptionpanel selbst, es wird kein createBeschreibung aufgerufen!
            {
                QueryItemFunction = (id) =>
                {
                    if (id == 0)
                        return new XproEntityDto();

                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = id});
                        return ctx.ExecuteStoreQuery<XproEntityDto>("select sysobtyp sysid, bezeichnung title,bezeichnung, 'Preis: '||grund||' EUR' desc1, 'AFA: '||afa desc1 from obtyp where sysobtyp=:sysid", par.ToArray()).FirstOrDefault();
                    }

                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "filter", Value = "%"+input.Filter+"%" });


                        /*if (input.filters.Length > 0 && input.filters[0].value.Length > 0)
                        {
                            par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "filter", Value = input.filters[0].value });
                            query += ", bproleusr where bproleusr.syswfuser=wfuser.syswfuser and bproleusr.namebprole = :filter";
                        }
                        return ctx.ExecuteStoreQuery<XproEntityDto>(query, par.ToArray()).ToArray();
                        */

                        return ctx.ExecuteStoreQuery<XproEntityDto>("select sysobtyp sysid, bezeichnung title,bezeichnung, 'Preis: '||grund||' EUR' desc1, 'AFA: '||afa desc1 from obtyp where UPPER(bezeichnung) like UPPER(:filter)", par.ToArray()).ToArray();
                    }


                }
            });

          

            dictionary.Add(XproEntityType.FSTYP, new XproInfoDao<XproEntityDto>()//dieser typ definiert sein descriptionpanel selbst, es wird kein createBeschreibung aufgerufen!
            {
                QueryItemFunction = (id) =>
                {
                    if (id == 0)
                        return new XproEntityDto();

                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = id });
                        //return ctx.ExecuteStoreQuery<XproEntityDto>("select fstyp.sysfstyp sysid,fsart.beschreibung||' ('||fstyp.beschreibung||')' title from fstyp, fsart where fstyp.sysfsart=fsart.sysfsart and fstyp.sysfstyp=:sysid", par.ToArray()).FirstOrDefault();
                        return ctx.ExecuteStoreQuery<XproEntityDto>("select sltyp.syssltyp sysid, bezeichnung title from sltyp where sltyp.syssltyp=:sysid", par.ToArray()).FirstOrDefault();
                    }

                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "filter", Value = "%" + input.Filter.ToUpper() + "%" });
                        //return ctx.ExecuteStoreQuery<XproEntityDto>("select fstyp.sysfstyp sysid,fsart.beschreibung||' ('||fstyp.beschreibung||')' title from fstyp, fsart where fstyp.sysfsart=fsart.sysfsart and (upper(fsart.beschreibung) like (:filter) or upper(fstyp.beschreibung) like (:filter))", par.ToArray()).ToArray();
                        return ctx.ExecuteStoreQuery<XproEntityDto>("select sltyp.syssltyp sysid, bezeichnung title from sltyp where bezeichnung like 'FS%' and upper(bezeichnung) like (:filter) order by bezeichnung", par.ToArray()).ToArray();
                    }


                }
            });

            dictionary.Add(XproEntityType.BESUCHSBERICHT_TYP, CreateXproEnumInfo<BesuchsberichtTyp>());
            dictionary.Add(XproEntityType.JANEIN, CreateXproEnumInfo<JaNein>());
            dictionary.Add(XproEntityType.BESUCHSBERICHT_MARKTSTELLUNG, CreateXproEnumInfo<Marktstellung>());
            dictionary.Add(XproEntityType.BESUCHSBERICHT_INVESTTYP, CreateXproEnumInfo<BesuchsberichtInvestitionstyp>());
            dictionary.Add(XproEntityType.BESUCHSBERICHT_EIGENTUEMERTYP, CreateXproEnumInfo<EigentuemerTyp>());
            dictionary.Add(XproEntityType.BESUCHSBERICHT_GFQUALIFIKATION, CreateXproEnumInfo<GeschaeftsfuehrungQualifikation>());
            dictionary.Add(XproEntityType.BESUCHSBERICHT_AFATYP, CreateXproEnumInfo<Afatyp>());
            dictionary.Add(XproEntityType.BESUCHSBERICHT_GESCHAEFTSTYP, CreateXproEnumInfo<GeschaeftsTyp>());
            dictionary.Add(XproEntityType.BESUCHSBERICHT_PRIVATGEWERBLICH, CreateXproEnumInfo<PrivatGewerblich>());
            dictionary.Add(XproEntityType.BESUCHSBERICHT_KONDITIONEN, CreateXproEnumInfo<Konditionen>());
            dictionary.Add(XproEntityType.BESUCHSBERICHT_MVORIGINALANTRAG, CreateXproEnumInfo<MVOriginalantrag>());

            dictionary.Add(XproEntityType.HAENDLER_GEBIET, new XproInfoDao<DropListDto>()
            {
                QueryItemFunction = (id) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = id });
                        return ctx.ExecuteStoreQuery<DropListDto>("select gebiet code, "+id+" sysid, gebiet bezeichnung from (select gebiet,rownum r from (select distinct gebiet,rownum from gebiete_v where gebiet is not null)) where r=:id", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {

                        return ctx.ExecuteStoreQuery<DropListDto>("select gebiet code, rownum sysid, gebiet bezeichnung from (select distinct gebiet from gebiete_v) where gebiet is not null  order by gebiet", null).ToArray();
                        //List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        //par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "filter", Value = "%" + input.Filter.ToUpper() + "%" });
                        //return ctx.ExecuteStoreQuery<DropListDto>("select ort code, ort sysid, ort bezeichnung, ort beschreibung from (select distinct trim(ort) ort from person, perole where perole.sysperson=person.sysperson and perole.SYSROLETYPE=6 and upper(trim(ort)) like (:filter) order by trim(ort))", par.ToArray()).ToArray();
                    }
                },
                CreateBezeichnung = (item) => item.bezeichnung,
                CreateBeschreibung = (item, rval) => createPanel(rval, "PERSON", item.bezeichnung, "", "", "", "", "" + item.sysID),
            });
            dictionary.Add(XproEntityType.HAENDLER, new XproInfoDao<DropListDto>()
            {
                QueryItemFunction = (id) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = id });
                        return ctx.ExecuteStoreQuery<DropListDto>("select person.sysperson sysid, person.sysperson code, person.name||' '||person.vorname||' '||person.ort bezeichnung, person.name||' '||person.vorname||' '||person.ort  beschreibung from person, perole where perole.sysperson=person.sysperson and perole.SYSROLETYPE=6 and person.sysperson=:id", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "filter", Value = "%" + input.Filter.ToUpper() + "%" });
                        return ctx.ExecuteStoreQuery<DropListDto>("select sysid, code, gruppe||case when gruppe is null then '' else ': ' end||bezeichnung bezeichnung,  bezeichnung||' '||gruppe beschreibung from (select person.sysperson sysid, person.sysperson code, trim(person.name)||' '||trim(person.vorname)||' '||trim(person.ort) bezeichnung, (select trim(person.name) from person,perole hgp where hgp.sysperson=person.sysperson and  hgp.sysperole=hdp.sysparent and hgp.sysroletype=12) gruppe from person, perole hdp where hdp.sysperson=person.sysperson and hdp.SYSROLETYPE=6   and upper(person.name||' '||person.vorname||' '||person.ort) like (:filter) order by gruppe, person.name)", par.ToArray()).ToArray();
                    }
                },
                CreateBezeichnung = (item) => item.bezeichnung,
                CreateBeschreibung = (item, rval) => createPanel(rval, "PERSON", item.bezeichnung, "", "", "", "", "" + item.sysID),
            });
            dictionary.Add(XproEntityType.VERTRIEBSMA, new XproInfoDao<DropListDto>()
            {
                QueryItemFunction = (id) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = id });
                        return ctx.ExecuteStoreQuery<DropListDto>("select syswfuser sysid, syswfuser code,pr.name bezeichnung, pr.name beschreibung from puser, perole pr,person where puser.syspuser=person.syspuser and pr.sysperson=person.sysperson and syswfuser=:id", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        
                        
                        String syschguser = "-1";
                        long syshaendler = -1;
                        if (input.filters.Length>0)
                        {
                            String fieldValues = (from s in input.filters
                                          where s.fieldname.ToLower().IndexOf("syschguser,sysid") > -1
                                         select s.value).FirstOrDefault();
                            String[] vals = fieldValues.Split(',');
                            syschguser = vals[0];
                            syshaendler = ctx.ExecuteStoreQuery<long>("select sysvpfil from vt where sysid=" + vals[1], null).FirstOrDefault();
                                
                        }
                      
                        long sysperole = ctx.ExecuteStoreQuery<long>("select sysparent from perole where sysperole=" + input.sysperole, null).FirstOrDefault();

                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        //par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "filter", Value = "%" + input.Filter.ToUpper() + "%" });
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperole", Value = sysperole });
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syschguser", Value = syschguser });
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syshaendler", Value = syshaendler });
                        //es gibt immer mindestens den aktuellen user, 2 sf cic user mit fixem code oder die verkäufer vom händler oder alle im Gesichtskreis des Benutzers
                        List<DropListDto> rval =  ctx.ExecuteStoreQuery<DropListDto>("select syswfuser sysid, syswfuser code,pr.name bezeichnung, pr.name beschreibung from puser, perole pr,person where puser.syspuser=person.syspuser and pr.sysperson=person.sysperson and person.code in ('999999-191126','999997-191128') order by pr.name",null).ToList();


                        
                        //es gibt immer mindestens den aktuellen user, 2 sf cic user mit fixem code oder die verkäufer vom händler oder alle im Gesichtskreis des Benutzers
                        List<DropListDto> rval2 = ctx.ExecuteStoreQuery<DropListDto>("select syswfuser sysid, syswfuser code,pr.name bezeichnung, pr.name beschreibung from puser, perole pr,person where puser.syspuser=person.syspuser and pr.sysperson=person.sysperson and (puser.syswfuser=:syschguser or pr.sysperole in (select sysperole from perole where perole.sysperole!=:sysperole connect by prior perole.sysperole = perole.sysparent start with perole.sysperole=:sysperole  )  or pr.sysperole in (select sysperole from perole where perole.sysperole!=:sysperole connect by prior perole.sysperole = perole.sysparent start with perole.sysperson=:syshaendler  ) ) order by pr.name", par.ToArray()).ToList();
                        rval.AddRange(rval2);
                        return rval.ToArray();
                    }
                },
                CreateBezeichnung = (item) => item.bezeichnung,
                CreateBeschreibung = (item, rval) => createPanel(rval, "PERSON", item.bezeichnung, "", "", "", "", "" + item.sysID),
            });
            dictionary.Add(XproEntityType.INTTYPE, new XproInfoDao<DropListDto>()
            {
                QueryItemFunction = (id) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = id });
                        return ctx.ExecuteStoreQuery<DropListDto>("select inttype sysid, inttype code,name bezeichnung, name beschreibung from inttype where inttype=:id", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {


                        return ctx.ExecuteStoreQuery<DropListDto>("select inttype sysid, inttype code,name bezeichnung, name beschreibung from inttype ", null).ToArray();
                    }
                },
                CreateBezeichnung = (item) => item.bezeichnung,
                CreateBeschreibung = (item, rval) => createPanel(rval, "ZINSTYP", item.bezeichnung, "", "", "", "", "" + item.sysID),
            });


           /* String aidaFilter = AppConfig.Instance.getValueFromDb("AIDA", "FILTERS", "VT");
            if (aidaFilter != null && aidaFilter.Length > 0)
            {
                aidaFilter = " AND " + aidaFilter;
            }
            else aidaFilter = "";*/
            String aidaFilter = " AND (VT.VERTRIEBSWEG NOT LIKE '%Fleet%' and vt.sysls NOT LIKE 3 and vt.sysls NOT LIKE 4 and vt.SYSVART < 600 and vt.DATAKTIV > to_date('01.01.1800','DD.MM.YYYY') ) ";

            dictionary.Add(XproEntityType.VTVART, new XproInfoDao<DropListDto>()
            {
                QueryItemFunction = (id) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = id });
                        return ctx.ExecuteStoreQuery<DropListDto>("select * from (select rownum sysid, a.f code, a.f bezeichnung, a.f beschreibung from (select distinct trim(vt.vart) f  from vt where 1=1 " + aidaFilter + " order by trim(vt.vart)) a) where sysid=:id", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        return ctx.ExecuteStoreQuery<DropListDto>("select rownum sysid, a.f code, a.f bezeichnung, a.f beschreibung from (select distinct trim(vt.vart) f  from vt where 1=1 " + aidaFilter + " order by trim(vt.vart)) a", null).ToArray();
                    }
                },
                CreateBezeichnung = (item) => item.bezeichnung,
                CreateBeschreibung = (item, rval) => createPanel(rval, "VTVART", item.bezeichnung, "", "", "", "", "" + item.sysID),
            });
            dictionary.Add(XproEntityType.MANDANT, new XproInfoDao<DropListDto>()
            {
                QueryItemFunction = (id) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = id });
                        return ctx.ExecuteStoreQuery<DropListDto>("select * from (select * from (select a.f sysid, lsadd.mandant code, lsadd.bezeichnung from (select distinct vt.sysls f  from vt where  1=1 " + aidaFilter + ") a, lsadd where a.f=lsadd.syslsadd)) where sysid=:id", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        return ctx.ExecuteStoreQuery<DropListDto>("select * from (select a.f sysid, lsadd.mandant code, lsadd.bezeichnung from (select distinct vt.sysls f  from vt where  1=1 " + aidaFilter+") a, lsadd where a.f=lsadd.syslsadd)" , null).ToArray();
                    }
                },
                CreateBezeichnung = (item) => item.bezeichnung,
                CreateBeschreibung = (item, rval) => createPanel(rval, "MANDANT", item.bezeichnung, "", "", "", "", "" + item.sysID),
            });

            dictionary.Add(XproEntityType.VERTRIEBSWEG, new XproInfoDao<DropListDto>()
            {
                QueryItemFunction = (id) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = id });
                        return ctx.ExecuteStoreQuery<DropListDto>("select * from (select rownum sysid, a.f code, a.f bezeichnung, a.f beschreibung from (select distinct trim(vt.vertriebsweg) f  from vt where 1=1 " + aidaFilter + " order by trim(vt.vertriebsweg)) a) where sysid=:id", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        return ctx.ExecuteStoreQuery<DropListDto>("select rownum sysid, a.f code, a.f bezeichnung, a.f beschreibung from (select distinct trim(vt.vertriebsweg) f  from vt where 1=1 " + aidaFilter + " order by trim(vt.vertriebsweg)) a", null).ToArray();
                    }
                },
                CreateBezeichnung = (item) => item.bezeichnung,
                CreateBeschreibung = (item, rval) => createPanel(rval, "VERTRIEBSWEG", item.bezeichnung, "", "", "", "", "" + item.sysID),
            });

            dictionary.Add(XproEntityType.TARIFKZ, new XproInfoDao<DropListDto>()
            {
                QueryItemFunction = (id) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = id });
                        return ctx.ExecuteStoreQuery<DropListDto>(@"select * from (select a.id sysid, a.f code, a.f bezeichnung, a.f beschreibung from (
select  trim(konstellation) f, min(sysid) id from vt where  sysid=:id and konstellation is not null group by trim(konstellation) order by trim(konstellation) ) a)", par.ToArray()).FirstOrDefault();
                        //return ctx.ExecuteStoreQuery<DropListDto>("select * from (select rownum sysid, a.f code, a.f bezeichnung, a.f beschreibung from (select distinct trim(vt.konstellation) f  from vt where 1=1 " + aidaFilter + " order by trim(vt.konstellation)) a) where sysid=:id", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "filter", Value = "%" + input.Filter + "%" });
                        return ctx.ExecuteStoreQuery<DropListDto>(@"select a.id sysid, a.f code, a.f bezeichnung, a.f beschreibung from (
select trim(konstellation) f, min(sysid) id from vt  
where konstellation is not null and upper(trim(konstellation)) like UPPER(:filter) 
 " + aidaFilter + " group by trim(konstellation) ) a order by a.f", par.ToArray()).ToArray();


                       // return ctx.ExecuteStoreQuery<DropListDto>("select rownum sysid, a.f code, a.f bezeichnung, a.f beschreibung from (select distinct trim(vt.konstellation) f  from vt where 1=1 " + aidaFilter + " order by trim(vt.konstellation)) a", null).ToArray();
                    }
                },
                CreateBezeichnung = (item) => item.bezeichnung,
                CreateBeschreibung = (item, rval) => createPanel(rval, "TARIFKZ", item.bezeichnung, "", "", "", "", "" + item.sysID),
            });
            dictionary.Add(XproEntityType.VTSTATUS, new XproInfoDao<DropListDto>()
            {
                QueryItemFunction = (id) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {

                        List<DropListDto> rval = new List<DropListDto>();
                        rval.Add(new DropListDto() { beschreibung = "Aktiv", bezeichnung = "Aktiv", code = "AKTIV", sysID = 1 });
                        rval.Add(new DropListDto() { beschreibung = "Vor Ende Kunde", bezeichnung = "Vor Ende Kunde", code = "VOR_ENDE_KUNDE", sysID = 1 });
                        rval.Add(new DropListDto() { beschreibung = "Vor Ende Händler", bezeichnung = "Vor Ende Händler", code = "VOR_ENDE_HÄNDLER", sysID = 1 });

                        return (from a in rval
                                where a.sysID==id
                                select a).FirstOrDefault();
                        /*List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = id });
                        return ctx.ExecuteStoreQuery<DropListDto>("select * from (select rownum sysid, a.f code, a.f bezeichnung, a.f beschreibung from (select distinct trim(vt.zustand) f  from vt where 1=1 " + aidaFilter + " order by trim(vt.zustand)) a) where sysid=:id", par.ToArray()).FirstOrDefault();*/
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<DropListDto> rval = new List<DropListDto>();
                        rval.Add(new DropListDto(){beschreibung="Aktiv",bezeichnung="Aktiv",code="AKTIV",sysID=1});
                        rval.Add(new DropListDto(){beschreibung="Vor Ende Kunde",bezeichnung="Vor Ende Kunde",code="VOR_ENDE_KUNDE",sysID=1});
                        rval.Add(new DropListDto(){beschreibung="Vor Ende Händler",bezeichnung="Vor Ende Händler",code="VOR_ENDE_HÄNDLER",sysID=1});
                        //return ctx.ExecuteStoreQuery<DropListDto>("select rownum sysid, a.f code, a.f bezeichnung, a.f beschreibung from (select distinct trim(vt.zustand) f  from vt where 1=1 " + aidaFilter + " order by trim(vt.zustand)) a", null).ToArray();
                        return rval.ToArray();
                    }
                },
                CreateBezeichnung = (item) => item.bezeichnung,
                CreateBeschreibung = (item, rval) => createPanel(rval, "VTSTATUS", item.bezeichnung, "", "", "", "", "" + item.sysID),
            });
            dictionary.Add(XproEntityType.HAENDLER_NUMMER, new XproInfoDao<DropListDto>()
            {
                QueryItemFunction = (id) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = id });
                        return ctx.ExecuteStoreQuery<DropListDto>("select hd.sysperson sysid ,hd.code,hd.matchcode||' '||hd.code bezeichnung, hd.matchcode beschreibung from hd,perole where hd.sysperson=perole.sysperson and hd.sysperson=:id", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "filter", Value = "%" + input.Filter + "%" });
                        return ctx.ExecuteStoreQuery<DropListDto>("select hd.sysperson sysid ,hd.code,hd.matchcode||' '||hd.code bezeichnung, hd.matchcode beschreibung from hd,perole where hd.sysperson=perole.sysperson and  (code like '1%' or code like '2%') and length(code)=6 and aktivkz=1 and upper(hd.matchcode||' '||hd.code) like UPPER(:filter)  order by matchcode", par.ToArray()).ToArray();
                    }
                },
                CreateBezeichnung = (item) => item.bezeichnung,
                CreateBeschreibung = (item, rval) => createPanel(rval, "HAENDLER_NUMMER", item.bezeichnung, "", "", "", "", "" + item.sysID),
            });
            dictionary.Add(XproEntityType.OBINVENTAR, new XproInfoDao<DropListDto>()
            {
                QueryItemFunction = (id) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = id });
                        return ctx.ExecuteStoreQuery<DropListDto>("select * from (select rownum sysid, a.f code, a.f bezeichnung, a.f beschreibung from (select distinct trim(ob.inventar) f  from ob,vt where ob.sysvt=vt.sysid " + aidaFilter + " order by trim(ob.inventar)) a) where sysid=:id", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        return ctx.ExecuteStoreQuery<DropListDto>("select rownum sysid, a.f code, a.f bezeichnung, a.f beschreibung from (select distinct trim(ob.inventar) f  from ob,vt where ob.sysvt=vt.sysid " + aidaFilter + " order by trim(ob.inventar)) a", null).ToArray();
                    }
                },
                CreateBezeichnung = (item) => item.bezeichnung,
                CreateBeschreibung = (item, rval) => createPanel(rval, "OBINVENTAR", item.bezeichnung, "", "", "", "", "" + item.sysID),
            });
            dictionary.Add(XproEntityType.BAUREIHE, new XproInfoDao<DropListDto>()
            {
                QueryItemFunction = (id) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = id });
                        return ctx.ExecuteStoreQuery<DropListDto>(@"select * from (select a.id sysid, a.f code, a.f bezeichnung, a.f beschreibung from (
select sklasse f, min(sysschwacke) id from schwacke where sklasse is not null and  marke in ('BMW','Mini') group by sklasse order by trim(sklasse) ) a) where sysid=:id", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "filter", Value = "%" + input.Filter + "%" });
                        return ctx.ExecuteStoreQuery<DropListDto>(@"select a.id sysid, a.f code, a.f bezeichnung, a.f beschreibung from (
select sklasse f, min(sysschwacke) id from schwacke  
where sklasse is not null and  marke in ('BMW','Mini') and upper(trim(sklasse)) like UPPER(:filter) 
group by sklasse order by trim(sklasse) ) a", par.ToArray()).ToArray();
                    }
                },
                CreateBezeichnung = (item) => item.bezeichnung,
                CreateBeschreibung = (item, rval) => createPanel(rval, "BAUREIHE", item.bezeichnung, "", "", "", "", "" + item.sysID),
            });
            dictionary.Add(XproEntityType.MODELLCODE, new XproInfoDao<DropListDto>()
            {
                QueryItemFunction = (id) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = id });
                        return ctx.ExecuteStoreQuery<DropListDto>(@"select * from (select a.id sysid, a.f code, a.f bezeichnung, a.f beschreibung from (
                       select code f, min(sysobtypmap) id from obtypmap  where length(trim(code)) =4 and obtypmap.art = 10 group by code order by trim(code)
                        ) a) where sysid=:id", par.ToArray()).FirstOrDefault();
                            //select  distinct trim(motor) f  from ob,vt where ob.sysvt=vt.sysid " + aidaFilter + "  order by trim(motor) 
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "filter", Value = "%" + input.Filter + "%" });
                        return ctx.ExecuteStoreQuery<DropListDto>("select a.id sysid, a.f code, a.f bezeichnung, a.f beschreibung from (select code f, min(sysobtypmap) id from obtypmap  where length(trim(code)) =4 and obtypmap.art = 10 and upper(trim(code)) like UPPER(:filter) group by code order by trim(code) ) a", par.ToArray()).ToArray();
                                //select  distinct trim(motor) f  from ob,vt where ob.sysvt=vt.sysid " + aidaFilter + " and  
                    }
                },
                CreateBezeichnung = (item) => item.bezeichnung,
                CreateBeschreibung = (item, rval) => createPanel(rval, "MODELLCODE", item.bezeichnung, "", "", "", "", "" + item.sysID),
            });
            dictionary.Add(XproEntityType.MARKE, new XproInfoDao<DropListDto>()
            {
                QueryItemFunction = (id) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = id });
                        return ctx.ExecuteStoreQuery<DropListDto>("select * from (select rownum sysid, a.f code, a.f bezeichnung, a.f beschreibung from (select  distinct trim(hersteller) f  from ob,vt where ob.sysvt=vt.sysid " + aidaFilter + "  order by trim(hersteller) ) a) where sysid=:id", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "filter", Value = "%" + input.Filter + "%" });
                        return ctx.ExecuteStoreQuery<DropListDto>("select rownum sysid, a.f code, a.f bezeichnung, a.f beschreibung from (select  distinct trim(hersteller) f  from ob,vt where ob.sysvt=vt.sysid " + aidaFilter + " and  upper(trim(hersteller)) like UPPER(:filter) order by trim(hersteller) ) a", par.ToArray()).ToArray();
                    }
                },
                CreateBezeichnung = (item) => item.bezeichnung,
                CreateBeschreibung = (item, rval) => createPanel(rval, "MARKE", item.bezeichnung, "", "", "", "", "" + item.sysID),
            });
            dictionary.Add(XproEntityType.VERTRIEBSEBENE, new XproInfoDao<DropListDto>()
            {
                QueryItemFunction = (id) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = id });
                        return ctx.ExecuteStoreQuery<DropListDto>("select sysperole sysid,name code,name bezeichnung, name beschreibung from perole  where sysperole=:id", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperole", Value = input.sysperole });
                        //par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "filter", Value = "%" + input.Filter + "%" });
                        return ctx.ExecuteStoreQuery<DropListDto>("select sysperole sysid,name code,lpad('-',(level-1)*2,'-')||name bezeichnung, name beschreibung from perole  where sysroletype!=7 and sysperole in (select syschild from perolecache where sysparent=:sysperole) connect by prior sysperole=sysparent start with sysroletype=14", par.ToArray()).ToArray();
                    }
                },
                CreateBezeichnung = (item) => item.bezeichnung,
                CreateBeschreibung = (item, rval) => createPanel(rval, "VERTRIEBSEBENE", item.bezeichnung, "", "", "", "", "" + item.sysID),
            });
            dictionary.Add(XproEntityType.VERTRIEBSEBENEVK, new XproInfoDao<DropListDto>()
            {
                QueryItemFunction = (id) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = id });
                        return ctx.ExecuteStoreQuery<DropListDto>("select sysperole sysid,name code,name bezeichnung, name beschreibung from perole  where sysperole=:id", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperole", Value = input.sysperole });


                        StringBuilder sb = new StringBuilder();
                        if (input.filters != null && input.filters.Length > 0)
                        {
                            if (input.filters[0].values != null && input.filters[0].values.Length > 0)
                            {
                                sb.Append("where (path like ");
                                String[] values = input.filters[0].values.Distinct().ToArray();
                                int i = 0;
                                foreach (String value in values)
                                {
                                    if (i > 0)
                                        sb.Append(" OR path like ");
                                    sb.Append("'%/");
                                    sb.Append(value);
                                    sb.Append("/%'");
                                    i++;
                                }
                                sb.Append(")");
                            }
                            else
                            {
                                sb.Append(" where (path like 'OFF') ");
                            }
                        }
                        else
                        {
                            sb.Append(" where (path like 'OFF') ");
                        }

                        return ctx.ExecuteStoreQuery<DropListDto>(@"select sysid, name bezeichnung, name code, name beschreibung from (select  '/'||SYS_CONNECT_BY_PATH(sysperole,'/')||'/' path, sysperole sysid, name from perole  where 
sysroletype=7 
and sysperole in (select syschild from perolecache where sysparent=:sysperole) 
connect by prior sysperole=sysparent start with sysroletype=14) "+sb.ToString(), par.ToArray()).ToArray();//where path like '%/987/%'
                    }
                },
                CreateBezeichnung = (item) => item.bezeichnung,
                CreateBeschreibung = (item, rval) => createPanel(rval, "VERTRIEBSEBENEVK", item.bezeichnung, "", "", "", "", "" + item.sysID),
            });


            //Fahrerliste
            dictionary.Add(XproEntityType.FAHRER, new XproInfoDao<XproEntityDto>()//dieser typ definiert sein descriptionpanel selbst, es wird kein createBeschreibung aufgerufen!
            {
                QueryItemFunction = (id) =>
                {
                    if (id == 0)
                        return new XproEntityDto();

                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = id });
                        return ctx.ExecuteStoreQuery<XproEntityDto>("select perole.sysperson sysid, perole.name title from perole where sysperson=:sysid", par.ToArray()).FirstOrDefault();
                    }

                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        String pkey="-1";
                        String query="select perole.sysperson sysid, perole.name title from perole, roletype, perole pp where perole.sysroletype=roletype.sysroletype and roletype.typ=17 and perole.sysparent=pp.sysperole and pp.sysperson=:filter";
                        if (input.filters.Length > 0 && input.filters[0].fieldname.ToLower().IndexOf("syswktaccount") > -1)
                        {
                            //build new filters for wktaccount inputfilter
                            String wktacc = input.filters[0].value;
                            long syswktaccount = long.Parse(wktacc);

                            if (syswktaccount < 0)
                            {
                                pkey = "" + syswktaccount * -1;
                                query = "select perole.sysperson sysid, perole.name title from perole, roletype, perole pp, it where it.sysperson=pp.sysperson and  perole.sysroletype=roletype.sysroletype and roletype.typ=17 and perole.sysparent=pp.sysperole and it.sysit=:filter";
                            }
                            else
                            {
                                pkey = wktacc;
                            }
                        }
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "filter", Value =  pkey  });
                        return ctx.ExecuteStoreQuery<XproEntityDto>(query, par.ToArray()).ToArray();
                    }


                }
            });

            //Lieferantenliste
            dictionary.Add(XproEntityType.LIEFERANT, new XproInfoDao<XproEntityDto>()//dieser typ definiert sein descriptionpanel selbst, es wird kein createBeschreibung aufgerufen!
            {
                QueryItemFunction = (id) =>
                {
                    if (id == 0)
                        return new XproEntityDto();

                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = id });
                        return ctx.ExecuteStoreQuery<XproEntityDto>("select sysperson sysid, perole.name title from perole, roletype where roletype.sysroletype=perole.sysroletype and roletype.typ=18 and sysperson=:sysid", par.ToArray()).FirstOrDefault();
                    }

                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        
                        //par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "filter", Value =  "%"+input.Filter+"%"  });
                        return ctx.ExecuteStoreQuery<XproEntityDto>("select sysperson sysid, perole.name title from perole, roletype where roletype.sysroletype=perole.sysroletype and roletype.typ=18",null).ToArray();
                    }


                }
            });

            //Kundentyp
            dictionary.Add(XproEntityType.KDTYP, new XproInfoDao<DropListDto>()
            {
                QueryItemFunction2 = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = input.EntityId });
                        return ctx.ExecuteStoreQuery<DropListDto>("select syskdtyp sysid, syskdtyp code, name bezeichnung, name beschreibung from kdtyp where syskdtyp=:id", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        
                        return ctx.ExecuteStoreQuery<DropListDto>("select syskdtyp sysid, syskdtyp code, name bezeichnung, name beschreibung from kdtyp", null).ToArray();
                    }
                },
                CreateBezeichnung = (item) => item.bezeichnung,
                CreateBeschreibung = (item, rval) => createPanel(rval, "PERSON", item.bezeichnung, "", "", "", "", "" + item.sysID),
            });


            //Vttyp
            dictionary.Add(XproEntityType.VTTYP, new XproInfoDao<DropListDto>()
            {
                QueryItemFunction2 = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = input.EntityId });
                        return ctx.ExecuteStoreQuery<DropListDto>("select sysvttyp sysid, sysvttyp code, bezeichnung, bezeichnung beschreibung from vttyp where sysvttyp=:id", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {

                        return ctx.ExecuteStoreQuery<DropListDto>("select sysvttyp sysid, sysvttyp code, bezeichnung, bezeichnung beschreibung from vttyp", null).ToArray();
                    }
                },
                CreateBezeichnung = (item) => item.bezeichnung,
                CreateBeschreibung = (item, rval) => createPanel(rval, "VTTYP", item.bezeichnung, "", "", "", "", "" + item.sysID),
            });


            //Rollen der Benutzer
            dictionary.Add(XproEntityType.BPROLE, new XproInfoDao<XproEntityDto>()
            {
                QueryItemFunction = (id) =>
                {
                    if (id == 0)
                        return new XproEntityDto();

                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = id });
                        return ctx.ExecuteStoreQuery<XproEntityDto>("select sysbprole sysID, namebprole title, namebprole bezeichnung, description beschreibung from bprole where sysbprole=:sysid", par.ToArray()).FirstOrDefault();
                    }

                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "filter", Value = "%" + input.Filter + "%" });
                        return ctx.ExecuteStoreQuery<XproEntityDto>("select sysbprole sysID, namebprole title, namebprole bezeichnung, description beschreibung from bprole where description like :filter", par.ToArray()).ToArray();
                    }
                },
                CreateBezeichnung = (item) => item.bezeichnung,
                CreateBeschreibung = (item, rval) => createPanel(rval, "BPROLE", item.title, item.bezeichnung, "", item.beschreibung, item.code, item.sysID.ToString()),
            });


            //Benutzer der Rollen
            dictionary.Add(XproEntityType.BPWFUSER, new XproInfoDao<XproEntityDto>()
            {
                QueryItemFunction = (id) =>
                {
                    if (id == 0)
                        return new XproEntityDto();

                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = id });
                        return ctx.ExecuteStoreQuery<XproEntityDto>("select syswfuser sysID, code title, code, vorname bezeichnung, name beschreibung, vorname desc1, name desc2 from wfuser where syswfuser=:sysid", par.ToArray()).FirstOrDefault();
                    }

                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        string query = "select wfuser.syswfuser sysID, wfuser.vorname||' '||wfuser.name||' ('||wfuser.code||')' title, wfuser.code code, wfuser.vorname bezeichnung, wfuser.name beschreibung, wfuser.vorname desc1, wfuser.name desc2 from wfuser";
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();

                        if (input.filters.Length > 0 && input.filters[0].value.Length > 0)
                        {
                            par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "filter", Value = input.filters[0].value });
                            query += ", bproleusr where bproleusr.syswfuser=wfuser.syswfuser and bproleusr.namebprole = :filter";
                        }
                        return ctx.ExecuteStoreQuery<XproEntityDto>(query, par.ToArray()).ToArray();
                    }
                },
                CreateBezeichnung = (item) => item.desc1 + " " + item.desc2,
                CreateBeschreibung = (item, rval) => createPanel(rval, "BPWFUSER", item.title, item.desc1, item.desc2, item.beschreibung, item.code, item.sysID.ToString()),
            });

            //Lieferantenliste
            dictionary.Add(XproEntityType.PLZ, new XproInfoDao<XproEntityDto>()//dieser typ definiert sein descriptionpanel selbst, es wird kein createBeschreibung aufgerufen!
            {
                QueryItemFunction = (id) =>
                {
                    if (id == 0)
                        return new XproEntityDto();

                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value =id });
                        String GETPLZBYPLZ = "select sysplz sysid, plz title,sysstaat desc1,l.sysland desc2, ort beschreibung ,l.countryname desc3 from PLZ,land l where l.sysland=plz.sysland and sysplz = :sysid";
                        //par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "filter", Value =  "%"+input.Filter+"%"  });
                        return ctx.ExecuteStoreQuery<XproEntityDto>(GETPLZBYPLZ, par.ToArray()).FirstOrDefault();
                    }

                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {

                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "postleitzahl", Value = input.Filter });
                        String GETPLZBYPLZ = "select sysplz sysid, plz title,sysstaat desc1,l.sysland desc2, ort beschreibung ,l.countryname desc3 from PLZ,land l where l.sysland=plz.sysland and PLZ = :postleitzahl";
                        //par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "filter", Value =  "%"+input.Filter+"%"  });
                        return ctx.ExecuteStoreQuery<XproEntityDto>(GETPLZBYPLZ, par.ToArray()).ToArray();
                    }


                }
            });


            dictionary.Add(XproEntityType.WFMMKAT, new XproInfoDao<DropListDto>()
            {
                QueryItemFunction = (id) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "syskat", Value = id });
                        return ctx.ExecuteStoreQuery<DropListDto>("select syswfmmkat sysid, beschreibung bezeichnung, typ indicator from wfmmkat where syswfmmkat=:syskat", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        return ctx.ExecuteStoreQuery<DropListDto>("select syswfmmkat sysid, beschreibung bezeichnung from wfmmkat where activeflag=1").ToArray();
                    }
                }
            });


            //VERSANDART
            dictionaryCode.Add("VERSANDART", new XproInfoDao<DropListDto>()
            {
                QueryItemFunction = (id) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = id });
                        return ctx.ExecuteStoreQuery<DropListDto>("select ddkeys2.rang sysid, ddkeys2.value bezeichnung, ddkeys2.BEMERKUNG ,ddkeys2.rang code from ddkeys1, ddkey,ddkeys2 where ddkeys1.ddkeyid = ddkey.sysddkey and ddkey.description = 'Versandart' and ddkeys1.value='Wholesale Versandarten' and ddkeys1.sysddkeys1=ddkeys2.ddkeyid and ddkeys2.rang=:id", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        return ctx.ExecuteStoreQuery<DropListDto>("select ddkeys2.rang sysid, ddkeys2.value bezeichnung, ddkeys2.BEMERKUNG ,ddkeys2.rang code from ddkeys1, ddkey,ddkeys2 where ddkeys1.ddkeyid = ddkey.sysddkey and ddkey.description = 'Versandart' and ddkeys1.value='Wholesale Versandarten' and ddkeys1.sysddkeys1=ddkeys2.ddkeyid order by ddkeys2.rang", null).ToArray();
                    }
                },
                CreateBezeichnung = (item) => item.bezeichnung,
                CreateBeschreibung = (item, rval) => createPanel(rval, "VERSANDART", item.bezeichnung, "", "", "", "", "" + item.sysID),
            });
            //Marke_BN
            dictionaryCode.Add("MARKEBN", new XproInfoDao<DropListDto>()
            {
                QueryItemFunction = (id) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = id });
                        return ctx.ExecuteStoreQuery<DropListDto>("select sysid, title code, title||' '||otype title, bezeichnung||' '||otype bezeichnung from(select id2 sysid, bezeichnung title,(case art when 130 then 'MOT' else 'PKW' end) otype, bezeichnung from vc_obtyp2 where  id2=:id)", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "filter", Value = "%" + input.Filter + "%" });
                        return ctx.ExecuteStoreQuery<DropListDto>("select sysid, title code, title||' '||otype title, bezeichnung||' '||otype bezeichnung from(select id2 sysid, bezeichnung title,(case art when 130 then 'MOT' else 'PKW' end) otype, bezeichnung from vc_obtyp2 where  upper(trim(bezeichnung)) like UPPER(:filter) order by trim(bezeichnung))", par.ToArray()).ToArray();
                    }
                },
                CreateBezeichnung = (item) => item.bezeichnung,
                CreateBeschreibung = (item, rval) => createPanel(rval, "MARKE", item.bezeichnung, "", "", "", "", "" + item.sysID),
            });
            //Marke_BN
            dictionaryCode.Add("OBTYPBN", new XproInfoDao<DropListDto>()
            {
                QueryItemFunction = (id) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = id });
                        return ctx.ExecuteStoreQuery<DropListDto>("select id5 sysid, bezeichnung title, bezeichnung from vc_obtyp5 where id5=:id", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "filter", Value = "%" + input.Filter + "%" });
                        String query = "select vc_obtyp5.id5 sysid, vc_obtyp5.bezeichnung title,vc_obtyp5.bezeichnung from vc_obtyp2, vc_obtyp3, vc_obtyp4, vc_obtyp5 where vc_obtyp2.id2=vc_obtyp3.id2 and vc_obtyp3.id3=vc_obtyp4.id3 and vc_obtyp4.id4=vc_obtyp5.id4  and UPPER(vc_obtyp5.bezeichnung) like UPPER(:filter) ";
                        if (input.filters.Length > 0 && input.filters[0].value.Length > 0)
                        {
                            par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "hersteller", Value = input.filters[0].value });
                            query += " and vc_obtyp2.bezeichnung=:hersteller";
                        }
                        query += " order by vc_obtyp5.bezeichnung ";
                        return ctx.ExecuteStoreQuery<DropListDto>(query, par.ToArray()).ToArray();
                    }
                },
                CreateBezeichnung = (item) => item.bezeichnung,
                CreateBeschreibung = (item, rval) => createPanel(rval, "MARKE", item.bezeichnung, "", "", "", "", "" + item.sysID),
            });

            //VERSANDGRUND
            dictionaryCode.Add("VERSANDGRUND", new XproInfoDao<DropListDto>()
            {
                QueryItemFunction = (id) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = id });
                        return ctx.ExecuteStoreQuery<DropListDto>("select ddkeys2.rang sysid, ddkeys2.value bezeichnung, ddkeys2.BEMERKUNG ,ddkeys2.rang code from ddkeys1, ddkey,ddkeys2 where ddkeys1.ddkeyid = ddkey.sysddkey and ddkey.description = 'Versandgrund' and ddkeys1.value='Wholesale Versandgründe' and ddkeys1.sysddkeys1=ddkeys2.ddkeyid and ddkeys2.rang=:id", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.DdOl.DdOlExtended ctx = new OpenOne.Common.Model.DdOl.DdOlExtended())
                    {
                        
                        String paramhmd2 = "";
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        CIC.Database.OL.EF4.Model.PEROLE pe = Cic.OpenOne.Common.Model.DdOl.PeRoleUtil.FindRootPEROLEObjByRoleCode(ctx, input.sysperole, "HD");
                        if (pe == null||!pe.SYSPERSON.HasValue) return null;

                        long sysbrand = ctx.ExecuteStoreQuery<long>("select brand.sysbrand from brand,prhgroupm,prbrandm where brand.sysbrand=prbrandm.sysbrand and prbrandm.sysprhgroup=prhgroupm.sysprhgroup and prhgroupm.sysperole in (select sysperole from perole,roletype where perole.sysroletype=roletype.sysroletype and (roletype.typ=6) connect by PRIOR sysparent = sysperole start with sysperson=" + pe.SYSPERSON.Value + ")", null).FirstOrDefault();
                        if(sysbrand==4)//im Wholesale-Frontoffice für HMD (Hyundai) muss in der Auswahlliste der Versandgründe der Eintrag mit dem Rang 4 "Kundenbestellung zu Lagerfahrzeug" ausgeblendet werden, da bei HMD ein solcher Ordertypwechsel über das InvoiceInterface ausgelöst wird.
                        {
                            
                            paramhmd2 = "_Hyundai";
                        }
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperson", Value = pe.SYSPERSON.Value });
                        return ctx.ExecuteStoreQuery<DropListDto>(@"select ddkeys2.rang sysid, ddkeys2.value bezeichnung, ddkeys2.BEMERKUNG ,ddkeys2.rang code
                                                                from ddkeys1, ddkey,ddkeys2
                                                                where ddkeys1.ddkeyid = ddkey.sysddkey
                                                                and ddkey.description = 'Versandgrund"+paramhmd2+"' "+
                                                                @" and ddkeys1.value='Wholesale Versandgründe'
                                                                and ddkeys1.sysddkeys1=ddkeys2.ddkeyid
                                                                and ddkeys2.rang != case when (select nvl(nettingflag,0) from hd where sysperson = :sysperson) = 0 then 9 else -1 end order by ddkeys2.rang", par.ToArray()).ToArray();
                    }
                },
                CreateBezeichnung = (item) => item.bezeichnung,
                CreateBeschreibung = (item, rval) => createPanel(rval, "VERSANDGRUND", item.bezeichnung, "", "", "", "", "" + item.sysID),
            });


            dictionaryCode.Add("STRVERKEHRSAMT", new XproInfoDao<XproEntityDto>()
            {
                QueryItemFunction = (id) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = id });
                        return ctx.ExecuteStoreQuery<XproEntityDto>("select sysperson sysid, code,ort||' '||plz title, vorname||' '||name desc1, strasse||' '||hsnr desc2, ort||' '||plz desc3 from person where flaglu9=1 and person.sysperson=:id", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();

                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "filter", Value = "%" + input.Filter.ToUpper() + "%" });
                        return ctx.ExecuteStoreQuery<XproEntityDto>("select sysperson sysid, code,ort||' '||plz title, vorname||' '||name desc1, strasse||' '||hsnr desc2, ort||' '||plz desc3 from person where flaglu9=1   and upper(person.name||' '||person.vorname||' '||person.ort||' '||person.strasse||' '||person.plz) like (:filter) order by person.name", par.ToArray()).ToArray();
                    }
                }
            });

			// FILIALE  (rh 20170612)
			dictionaryCode.Add ("FILIALE", new XproInfoDao<XproEntityDto> ()
			{
				QueryItemFunction = (id) =>
				{
					using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended ())
					{
						List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter> ();
						par.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = id });
						return ctx.ExecuteStoreQuery<XproEntityDto> ("select sysperson sysid, code, ort||' '||plz title, vorname||' '||name desc1, strasse||' '||hsnr desc2, ort||' '||plz desc3 from person where person.sysperson=:id", par.ToArray ()).FirstOrDefault ();
					}
				},
				QueryItemsFunction = (input) =>
				{
					using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended ())
					{
						List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter> ();

						// par.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = "filter", Value = "%" + input.Filter.ToUpper () + "%" });
						//// für: AND upper (person.NAME ||' '  ||person.vorname  ||' '  ||person.ort  ||' '  ||person.strasse  ||' '  ||person.plz) LIKE (:filter)
						par.Add (new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperson", Value = input.filters[0].value });

						return ctx.ExecuteStoreQuery<XproEntityDto> (@"select person.sysperson sysid, person.code,person.ort||' '||person.plz title, person.vorname||' '||person.name desc1, person.strasse||' '||person.hsnr desc2, person.ort||' '||person.plz desc3 from perole, perole pperole,person where person.sysperson=perole.sysperson and perole.sysroletype=15 and perole.sysparent=pperole.sysperole and pperole.sysroletype=2 and  pperole.sysperson=:sysperson order by perole.name", par.ToArray ()).ToArray ();
					}
				}
			});



            //Fahrzeugzubehör
            dictionary.Add(XproEntityType.ETGADDITION, new XproInfoDao<XproEntityDto>()
            {
                QueryItemFunction = (id) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = id });

                        string q = "select obtyp.schwacke code,etgaddition.id sysid,  etgeqtext.text || ' (' || etgaddition.id || ')' title,etgeqtext.text || ' (' || etgaddition.id || ')' bezeichnung,etgaddition.id desc1, price2||' EUR' desc2, price2 beschreibung from obtyp,etgaddition,etgeqtext where ETGADDITION.natcode = schwacke AND etgeqtext.eqcode = etgaddition.eqcode and etgaddition.id=:id";
                        return ctx.ExecuteStoreQuery<XproEntityDto>(q, par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        if (input.filters.Length > 0 && input.filters[0].fieldname.ToLower().IndexOf("sysobtyp") > -1)
                        {
                            par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysobtyp", Value = input.filters[0].value });

                            string flagpack = "";
                            if (input.filters.Length > 0 && input.filters[1].fieldname.ToLower().IndexOf("flagpack") > -1)
                            {
                                try
                                {
                                    int f = Convert.ToInt32(input.filters[1].value);
                                    if (f == 1)
                                    {
                                        flagpack = " and flagpack = 1 ";
                                    }
                                    else
                                    {
                                        flagpack = " and flagpack != 1 ";
                                    }
                                }
                                catch
                                {
                                }
                            }

                            string besch = "";
                            if ((input.Filter != null) && (input.Filter.Length > 0))
                            {
                                long input_long = 0;
                                bool input_is_number = false;
                                try
                                {
                                    input_is_number = long.TryParse(input.Filter, out input_long);
                                }
                                catch
                                {
                                    input_is_number = false;
                                    input_long = 0;
                                }

                                if (input_is_number)
                                {
                                    besch = " and (upper(etgeqtext.text) LIKE '" + input.Filter.ToUpper() + "%' or etgaddition.id LIKE '" + input.Filter + "%') ";
                                }
                                else
                                {
                                    besch = " and upper(etgeqtext.text) LIKE '" + input.Filter.ToUpper() + "%' ";
                                }
                            }

                            //string q = "select obtyp.schwacke code,etgaddition.id sysid, etgeqtext.text beschreibung, etgaddition.price1 bezeichnung from obtyp,etgaddition,etgeqtext where ETGADDITION.natcode = schwacke AND etgeqtext.eqcode = etgaddition.eqcode " + add;
                            string q = "select price2 code,etgaddition.id sysid, etgeqtext.text || ' (' || etgaddition.id || ')' title,etgaddition.id desc1, price2||' EUR' desc2, etgeqtext.text || ' (' || etgaddition.id || ')' bezeichnung from obtyp,etgaddition,etgeqtext where etgaddition.flag!=4 and ETGADDITION.natcode = schwacke AND etgeqtext.eqcode = etgaddition.eqcode and obtyp.sysobtyp =:sysobtyp " + flagpack + besch;
                            //string q = "select obtyp.schwacke code,etgaddition.id sysid, etgeqtext.text bezeichnung, etgaddition.price1 beschreibung from obtyp,etgaddition,etgeqtext where ETGADDITION.natcode = obtyp.schwacke AND etgeqtext.eqcode = etgaddition.eqcode and obtyp.sysobtyp =:sysobtyp";
                            return ctx.ExecuteStoreQuery<XproEntityDto>(q, par.ToArray()).ToArray();
                        }
                        return new XproEntityDto[0];
                    }
                },
                CreateBezeichnung = (item) => item.beschreibung + "(" + item.sysID.ToString() + ")",
                CreateBeschreibung = (item, rval) => createPanel(rval, "ETGADDITION", item.beschreibung + "(" + item.sysID.ToString() + ")", "", "", "", "", "" + item.sysID),
            });

            ///Korrespondenzsprache
            dictionaryCode.Add("CTLANGKORR", new XproInfoDao<XproEntityDto>()
            {
                QueryItemFunction = (id) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {

                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = id });
                        return ctx.ExecuteStoreQuery<XproEntityDto>("select sysctlang sysid, isocode code, languagename title, isocode desc1 from ctlang where cic.mdbs_getfavorite('FAVORITEN','SPRACHE2', trim(upper(languagename))) is not null  and sysctlang=:id", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        return ctx.ExecuteStoreQuery<XproEntityDto>("select sysctlang sysid, isocode code, languagename title, isocode desc1 from ctlang where cic.mdbs_getfavorite('FAVORITEN','SPRACHE2', trim(upper(languagename))) is not null  order by cic.mdbs_getfavorite('FAVORITEN','SPRACHE2', trim(upper(languagename))), languagename", null).ToArray();
                    }
                }
            });

            //Abklärungs-Rollen
            dictionaryCode.Add("BPROLE2", new XproInfoDao<XproEntityDto>()
            {
                QueryItemFunction = (id) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {

                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = id });
                        return ctx.ExecuteStoreQuery<XproEntityDto>("select sysbprole sysID, namebprole code,namebprole title, namebprole desc1 from bprole where sysbprole=:id", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        return ctx.ExecuteStoreQuery<XproEntityDto>(@"select sysbprole sysID, namebprole code,namebprole title, namebprole desc1 
                                            from bprole
                                           where bprole.namebprole in (
                                        'SALES_FF_SD',
                                        'SALES_FF_SA',
                                        'SALES_FF_Breite',
                                        'SALES_WFA',
                                        'SALES_KF',
                                        'PAYMENTS_WFA',
                                        'DECISION_WFA',
                                        'FRAUD_WFA',
                                        'SALES_FF')", null).ToArray();
                    }
                }
            });

            //Abklärungs-Rollen
            dictionaryCode.Add("BPROLEUSERS", new XproInfoDao<XproEntityDto>()
            {
                QueryItemFunction = (id) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {

                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = id });
                        return ctx.ExecuteStoreQuery<XproEntityDto>(@"select 
                            wfuser.syswfuser sysID, wfuser.syswfuser code,wfuser.vorname||' '||wfuser.name title, wfuser.vorname||' '||wfuser.name desc1 
                                from wfuser where syswfuser=:id", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {

                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        if (input.filters.Length > 0 && input.filters[0].value.Length > 0)
                        {
                            par.Add(new Devart.Data.Oracle.OracleParameter {ParameterName = "sysbprole", Value = input.filters[0].value});
                        }
                        else
                        {
                            par.Add(new Devart.Data.Oracle.OracleParameter {ParameterName = "sysbprole", Value = -1});
                        }
                        return ctx.ExecuteStoreQuery<XproEntityDto>(@"select 
bproleusr.syswfuser sysID, bproleusr.syswfuser code,wfuser.vorname||' '||wfuser.name title, wfuser.vorname||' '||wfuser.name desc1 
    from bproleusr
       , bprole,wfuser
   where  wfuser.syswfuser=bproleusr.syswfuser and bproleusr.SYSWFUSER=wfuser.syswfuser and bproleusr.namebprole=bprole.namebprole and bprole.sysbprole=:sysbprole order by wfuser.vorname, wfuser.name", par.ToArray()).ToArray();
                    }
                }
            });

            dictionaryCode.Add("PROZESSSCHRITT", new XproInfoDao<DropListDto>()
            {
                QueryItemFunction = (id) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = id });
                        return ctx.ExecuteStoreQuery<DropListDto>("select * from (select rownum sysid, a.f code, a.f bezeichnung, a.f beschreibung from (select rownum sysid, stepdescription code, stepdescription bezeichnung, stepdescription beschreibung from (select distinct stepdescription from cic.vc_bplistener order by stepdescription)) a) where sysid=:id", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        return ctx.ExecuteStoreQuery<DropListDto>("select rownum sysid, stepdescription code, stepdescription bezeichnung, stepdescription beschreibung from (select distinct stepdescription from cic.vc_bplistener order by stepdescription)", null).ToArray();
                    }
                },
                CreateBezeichnung = (item) => item.bezeichnung,
                CreateBeschreibung = (item, rval) => createPanel(rval, "PROZESSSCHRITT", item.bezeichnung, "", "", "", "", "" + item.sysID),
            });
            //BPELANE-List filtered by sysperole
            dictionaryCode.Add("BPELANEPEROLE", new XproInfoDao<DropListDto>()
            {
                QueryItemFunction2 = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperole", Value = input.sysperole });
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysbprole", Value = input.EntityId });
                        return ctx.ExecuteStoreQuery<DropListDto>("SELECT bprole.sysbprole sysid,namebplane code, namebplane bezeichnung, namebplane beschreibung FROM bproleusr a1, bplane a2, bprole,wfuser,perole  WHERE bprole.namebprole=a1.namebprole and a1.namebprole = a2.namebprole and a1.syswfuser =wfuser.syswfuser and perole.sysperson=wfuser.sysperson and perole.sysperole=:sysperole and bprole.sysbprole=:sysbprole", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        
                        return ctx.ExecuteStoreQuery<DropListDto>("SELECT bprole.sysbprole sysid,namebplane code, namebplane bezeichnung, namebplane beschreibung FROM bproleusr a1, bplane a2, bprole,wfuser,perole  WHERE bprole.namebprole=a1.namebprole and a1.namebprole = a2.namebprole and a1.syswfuser =wfuser.syswfuser and perole.sysperson=wfuser.sysperson and perole.sysperole="+input.sysperole, null).ToArray();
                    }
                }
            });
            //BPELANE-List complete
            dictionaryCode.Add("BPELANE", new XproInfoDao<DropListDto>()
            {
                QueryItemFunction2 = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysbprole", Value = input.EntityId });
                        return ctx.ExecuteStoreQuery<DropListDto>("SELECT bprole.sysbprole sysid,namebplane code, namebplane bezeichnung, namebplane beschreibung FROM bplane, bprole  WHERE bprole.namebprole    = bplane.namebprole and bprole.sysbprole=:sysbprole", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {

                        return ctx.ExecuteStoreQuery<DropListDto>("SELECT bprole.sysbprole sysid,namebplane code, namebplane bezeichnung, namebplane beschreibung FROM bplane, bprole  WHERE substr(bplane.description,1,3) = 'WFA' and bprole.namebprole    = bplane.namebprole order by namebplane", null).ToArray();
                    }
                }
            });

            dictionaryCode.Add("PROZESSMONITOR_KPICODE", new XproInfoDao<DropListDto>()
            {
                QueryItemFunction2 = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = input.EntityId });
                        return ctx.ExecuteStoreQuery<DropListDto>("select sysddlkppos sysid,id code, value bezeichnung, value beschreibung from ddlkppos where sysddlkppos=:id", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        
                        return ctx.ExecuteStoreQuery<DropListDto>("select sysddlkppos sysid,id code, value bezeichnung, value beschreibung from ddlkppos where code='PROZESSMONITOR_KPI'", null).ToArray();
                    }
                }
            });



            dictionary.Add(XproEntityType.SEG, new XproInfoDao<DropListDto>()
            {
                QueryItemFunction2 = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();

                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = input.EntityId });
                        return ctx.ExecuteStoreQuery<DropListDto>("select seg.sysseg sysid,  seg.sysseg code, seg.name bezeichnung, seg.description beschreibung from seg where sysseg=:id", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {

                        return ctx.ExecuteStoreQuery<DropListDto>("select seg.sysseg sysid,sysseg code, seg.name bezeichnung, seg.description beschreibung from seg order by seg.sysseg", null).ToArray();
                    }
                }
            });

            dictionary.Add(XproEntityType.SEGPOS, new XproInfoDao<DropListDto>()
            {
                QueryItemFunction2 = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();

                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "id", Value = input.EntityId });
                        return ctx.ExecuteStoreQuery<DropListDto>("select segpos.syssegpos sysid,  segpos.syssegpos code, segpos.name bezeichnung, segpos.name beschreibung from segpos where syssegpos=:id", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {

                        return ctx.ExecuteStoreQuery<DropListDto>("select segpos.syssegpos sysid,syssegpos code, segpos.name bezeichnung, segpos.name beschreibung from segpos where art=2 and activeflag = 1 order by segpos.syssegpos", null).ToArray();
                    }
                }
            });


            //Fahrzeugsuche
            dictionaryCode.Add("ANTRAGOBTYP", new XproInfoDao<XproEntityDto>()//dieser typ definiert sein descriptionpanel selbst, es wird kein createBeschreibung aufgerufen!
            {
                QueryItemFunction = (id) =>
                {
                    if (id == 0)
                        return new XproEntityDto();

                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysid", Value = id });
                        return ctx.ExecuteStoreQuery<XproEntityDto>("select sysobtyp sysid, bezeichnung title,bezeichnung, 'Preis: '||grund||' EUR' desc1, 'AFA: '||afa desc1 from obtyp where sysobtyp=:sysid", par.ToArray()).FirstOrDefault();
                    }

                },
                QueryItemsFunction = (input) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "filter", Value = "%" + input.Filter + "%" });


                        /*if (input.filters.Length > 0 && input.filters[0].value.Length > 0)
                        {
                            par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "filter", Value = input.filters[0].value });
                            query += ", bproleusr where bproleusr.syswfuser=wfuser.syswfuser and bproleusr.namebprole = :filter";
                        }
                        return ctx.ExecuteStoreQuery<XproEntityDto>(query, par.ToArray()).ToArray();
                        */

                        return ctx.ExecuteStoreQuery<XproEntityDto>("select obtyp.sysobtyp sysid, obtyp.bezeichnung||' (Antrag '||antrag.antrag||')' title,obtyp.bezeichnung||' (Antrag '||antrag.antrag||')' bezeichnung, 'Preis: '||obtyp.grund||' EUR' desc1, 'AFA: '||obtyp.afa desc1 from obtyp,antob,antrag where antrag.sysid=antob.sysantrag and antob.sysobtyp=obtyp.sysobtyp and UPPER(obtyp.bezeichnung||antob.objekt) like UPPER(:filter)", par.ToArray()).ToArray();
                    }


                }
            });
            //Refibanken PERSON.FLAGBN=1
            dictionaryCode.Add("REFIBANK", new XproInfoDao<DropListDto>()
            {
                QueryItemFunction = (id) =>
                {
                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        List<Devart.Data.Oracle.OracleParameter> par = new List<Devart.Data.Oracle.OracleParameter>();
                        par.Add(new Devart.Data.Oracle.OracleParameter { ParameterName = "sysperson", Value = id });
                        return ctx.ExecuteStoreQuery<DropListDto>("select code, sysperson sysid, name bezeichnung,  matchcode beschreibung from person where flagbn=1 and aktivkz=1 and sysperson=:sysperson", par.ToArray()).FirstOrDefault();
                    }
                },
                QueryItemsFunction = (input) =>
                {

                    using (Cic.OpenOne.Common.Model.Prisma.PrismaExtended ctx = new OpenOne.Common.Model.Prisma.PrismaExtended())
                    {
                        return ctx.ExecuteStoreQuery<DropListDto>("select code, sysperson sysid,trim(name)||' '||vorname bezeichnung, matchcode beschreibung from person where flagbn=1 and aktivkz=1 order by name", null).ToArray();
                    }
                },
                CreateBezeichnung = (item) => item.bezeichnung,
                CreateBeschreibung = (item, rval) => createPanel(rval, "PERSON", item.bezeichnung, item.beschreibung, item.code, "", "", "" + item.sysID),
            });
             dictionaryCode.Add("CREFOACCOUNT", new XproInfoDao<XproEntityDto>()
            {
                QueryItemFunction = (id) =>
                {
                    if (id == 0)
                        return new XproEntityDto();

                    var member = new CredentialContext().getMembershipInfo();
                    CIC.Bas.Framework.OpenLease.Subscriptions.LookupVariableDto[] vars = new CIC.Bas.Framework.OpenLease.Subscriptions.LookupVariableDto[1];
                    vars[0] = new CIC.Bas.Framework.OpenLease.Subscriptions.LookupVariableDto();
                    vars[0].LookupVariableName = "CrefoSearchSuccessL";
                    vars[0].VariableName = "VARS";
                    vars[0].Value = "VARS";


                    CIC.Bas.Framework.OpenLease.Subscriptions.SubscriptionDto[] subscriptions = new CIC.Bas.Framework.OpenLease.Subscriptions.SubscriptionDto[3];
                    subscriptions[0] = new CIC.Bas.Framework.OpenLease.Subscriptions.SubscriptionDto();
                    subscriptions[0].ObjectKey = "CrefoSearchSuccessL";
                    subscriptions[0].ObjectName = "CREFO";
                    subscriptions[0].ObjectType = "L";
                    subscriptions[1] = new CIC.Bas.Framework.OpenLease.Subscriptions.SubscriptionDto();
                    subscriptions[1].ObjectName = "CrefoSearchResultQ";
                    subscriptions[1].ObjectType = "Q";
                    subscriptions[2] = new CIC.Bas.Framework.OpenLease.Subscriptions.SubscriptionDto();
                    subscriptions[2].ObjectName = "CrefoSearchQ";
                    subscriptions[2].ObjectType = "Q";

                    CIC.Bas.Framework.OpenLease.Subscriptions.QueueDto[] queues = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueDto[2];
                    queues[0] = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueDto();
                    queues[0].Name = "CrefoSearchResultQ";
                    queues[0].Records = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordDto[1];
                    queues[0].Records[0] = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordDto();
                    queues[0].Records[0].Values = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordValueDto[2];
                    queues[0].Records[0].Values[0] = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordValueDto();
                    queues[0].Records[0].Values[0].Value = "F01";
                    queues[0].Records[0].Values[0].VariableName = "country";
                    queues[0].Records[0].Values[1] = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordValueDto();
                    queues[0].Records[0].Values[1].Value = "F02";
                    queues[0].Records[0].Values[1].VariableName = "DE";

                    queues[1] = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueDto();
                    queues[1].Name = "CrefoSearchQ";
                    queues[1].Records = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordDto[2];
                    queues[1].Records[0] = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordDto();
                    //for every key=value we need 2 records, the first with F01 = varname the second with F02=value for varname
                    /*•	searchtype
                    •	identificationnumber
                    •	companyname
                    •	street
                    •	housenumber
                    •	housenumberaffix
                    •	postcode
                    •	city
                    •	country
                    •	legalform
                    •	website
                    •	email
                    •	registertype
                    •	registerid
                    •	vatid
                    */
                    //currently we search for companyname and country:
                    queues[1].Records[0].Values = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordValueDto[2];
                    queues[1].Records[0].Values[0] = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordValueDto();
                    queues[1].Records[0].Values[0].Value = "identificationnumber";
                    queues[1].Records[0].Values[0].VariableName = "F01";
                    queues[1].Records[0].Values[1] = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordValueDto();
                    queues[1].Records[0].Values[1].Value = ""+id;
                    queues[1].Records[0].Values[1].VariableName = "F02";

                    queues[1].Records[1] = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordDto();
                    queues[1].Records[1].Values = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordValueDto[2];
                    queues[1].Records[1].Values[0] = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordValueDto();
                    queues[1].Records[1].Values[0].Value = "country";
                    queues[1].Records[1].Values[0].VariableName = "F01";
                    queues[1].Records[1].Values[1] = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordValueDto();
                    queues[1].Records[1].Values[1].Value = "DE";
                    queues[1].Records[1].Values[1].VariableName = "F02";

                    CIC.Bas.Framework.OpenLease.Subscriptions.SubscriptionsDataDto rval = BOS.getFormulaData(1, "SYSTEM", "CREFOSEARCH", vars, subscriptions, queues, member.sysWFUSER);
                    String success = (from lv in rval.Variables
                                      where lv.LookupVariableName.Equals("CrefoSearchSuccessL")
                                      select lv.Value).FirstOrDefault();
                    CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordDto[] records = (from lv in rval.Queues
                                                                                          where lv.Name.Equals("CrefoSearchResultQ")
                                                                                          select lv.Records).FirstOrDefault();
                    List<XproEntityDto> rval2 = new List<XproEntityDto>();
                    foreach (CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordDto arec in records)
                    {


                        var crefoid = (from v in arec.Values
                                       where v.VariableName.Equals("F01")
                                       select v.Value).FirstOrDefault();
                        if ("morehits".Equals(crefoid))
                            continue;
                        var name = (from v in arec.Values
                                    where v.VariableName.Equals("F05")
                                    select v.Value).FirstOrDefault();
                        var strasse = (from v in arec.Values
                                       where v.VariableName.Equals("F12")
                                       select v.Value).FirstOrDefault() +
                                     (from v in arec.Values
                                      where v.VariableName.Equals("F07")
                                      select v.Value).FirstOrDefault();
                        var ort = (from v in arec.Values
                                   where v.VariableName.Equals("F09")
                                   select v.Value).FirstOrDefault() +
                                     (from v in arec.Values
                                      where v.VariableName.Equals("F03")
                                      select v.Value).FirstOrDefault();
                        rval2.Add(new XproEntityDto
                        {
                            sysID = long.Parse(crefoid),
                            title = name,
                            code = crefoid,
                            desc1 = strasse,
                            desc2 = ort
                        });
                    }
                    if (rval2.Count == 0)
                        return new XproEntityDto();

                    return rval2[0];
                },
                QueryItemsFunction = (input) =>
                {
                    var member = new CredentialContext().getMembershipInfo();
                    CIC.Bas.Framework.OpenLease.Subscriptions.LookupVariableDto[] vars = new CIC.Bas.Framework.OpenLease.Subscriptions.LookupVariableDto[1];
                    vars[0] = new CIC.Bas.Framework.OpenLease.Subscriptions.LookupVariableDto();
                    vars[0].LookupVariableName = "CrefoSearchSuccessL";
                    vars[0].VariableName = "VARS";
                    vars[0].Value = "VARS";
                    
                    
                    CIC.Bas.Framework.OpenLease.Subscriptions.SubscriptionDto[] subscriptions = new CIC.Bas.Framework.OpenLease.Subscriptions.SubscriptionDto[3];
                    subscriptions[0] = new CIC.Bas.Framework.OpenLease.Subscriptions.SubscriptionDto();
                    subscriptions[0].ObjectKey = "CrefoSearchSuccessL";
                    subscriptions[0].ObjectName = "CREFO";
                    subscriptions[0].ObjectType = "L";
                    subscriptions[1] = new CIC.Bas.Framework.OpenLease.Subscriptions.SubscriptionDto();
                    subscriptions[1].ObjectName = "CrefoSearchResultQ";
                    subscriptions[1].ObjectType = "Q";
                    subscriptions[2] = new CIC.Bas.Framework.OpenLease.Subscriptions.SubscriptionDto();
                    subscriptions[2].ObjectName = "CrefoSearchQ";
                    subscriptions[2].ObjectType = "Q";

                    CIC.Bas.Framework.OpenLease.Subscriptions.QueueDto[] queues = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueDto[2];
                    queues[0] = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueDto();
                    queues[0].Name = "CrefoSearchResultQ";
                    queues[0].Records = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordDto[1];
                    queues[0].Records[0] = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordDto();
                    queues[0].Records[0].Values = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordValueDto[2];
                    queues[0].Records[0].Values[0] = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordValueDto();
                    queues[0].Records[0].Values[0].Value="F01";
                    queues[0].Records[0].Values[0].VariableName="country";
                    queues[0].Records[0].Values[1] = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordValueDto();
                    queues[0].Records[0].Values[1].Value = "F02";
                    queues[0].Records[0].Values[1].VariableName = "DE";

                    queues[1] = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueDto();
                    queues[1].Name = "CrefoSearchQ";
                    queues[1].Records = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordDto[2];
                    queues[1].Records[0] = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordDto();
                    //for every key=value we need 2 records, the first with F01 = varname the second with F02=value for varname
                    /*•	searchtype
                    •	identificationnumber
                    •	companyname
                    •	street
                    •	housenumber
                    •	housenumberaffix
                    •	postcode
                    •	city
                    •	country
                    •	legalform
                    •	website
                    •	email
                    •	registertype
                    •	registerid
                    •	vatid
                    */
                    //currently we search for companyname and country:
                    queues[1].Records[0].Values = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordValueDto[2];
                    queues[1].Records[0].Values[0] = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordValueDto();
                    queues[1].Records[0].Values[0].Value = "companyname";
                    queues[1].Records[0].Values[0].VariableName = "F01";
                    queues[1].Records[0].Values[1] = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordValueDto();
                    queues[1].Records[0].Values[1].Value = input.Filter;
                    queues[1].Records[0].Values[1].VariableName = "F02";

                    queues[1].Records[1] = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordDto();
                    queues[1].Records[1].Values = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordValueDto[2];
                    queues[1].Records[1].Values[0] = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordValueDto();
                    queues[1].Records[1].Values[0].Value = "country";
                    queues[1].Records[1].Values[0].VariableName = "F01";
                    queues[1].Records[1].Values[1] = new CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordValueDto();
                    queues[1].Records[1].Values[1].Value = "DE";
                    queues[1].Records[1].Values[1].VariableName = "F02";

                    CIC.Bas.Framework.OpenLease.Subscriptions.SubscriptionsDataDto rval = BOS.getFormulaData(1, "SYSTEM", "CREFOSEARCH", vars, subscriptions, queues, member.sysWFUSER);
                    String success = (from lv in rval.Variables
                                                                                     where lv.LookupVariableName.Equals("CrefoSearchSuccessL")
                                                                                     select lv.Value).FirstOrDefault();
                    CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordDto[] records = (from lv in rval.Queues
                                                                                where lv.Name.Equals("CrefoSearchResultQ")
                                                                                select lv.Records).FirstOrDefault();
                    List<XproEntityDto> rval2 = new List<XproEntityDto>();
                    foreach(CIC.Bas.Framework.OpenLease.Subscriptions.QueueRecordDto arec in records)
                    {
                        

                        var crefoid = (from v in arec.Values
                                     where v.VariableName.Equals("F01")
                                     select v.Value).FirstOrDefault();
                        if("morehits".Equals(crefoid))
                            continue;
                        var name = (from v in arec.Values
                                     where v.VariableName.Equals("F05")
                                     select v.Value).FirstOrDefault();
                        var strasse = (from v in arec.Values
                                     where v.VariableName.Equals("F12")
                                       select v.Value).FirstOrDefault() + " " +
                                     (from v in arec.Values
                                     where v.VariableName.Equals("F07")
                                     select v.Value).FirstOrDefault();
                        var ort = (from v in arec.Values
                                     where v.VariableName.Equals("F09")
                                     select v.Value).FirstOrDefault()+" "+
                                     (from v in arec.Values
                                     where v.VariableName.Equals("F03")
                                     select v.Value).FirstOrDefault();
                        rval2.Add(new XproEntityDto{
                                sysID = long.Parse(crefoid),
                                title = name,
                                code = crefoid,
                                desc1 = strasse,
                                desc2 = ort
                        });
                    }
                   

                    return rval2.ToArray();
                

                }
            });
            

          
            
        }

        /// <summary>
        /// Removes all parents with the given id
        /// </summary>
        /// <param name="list"></param>
        /// <param name="id"></param>
        private static void RemoveResults(List<CampDto> list, long id)
        {
            if (id == 0)
                return;

            List<long> toRemove = new List<long>();
            for (int i = 0; i < list.Count; i++)
            {
                CampDto camp = list[i];
                if (camp.sysCampParent == id)
                {
                    toRemove.Add(camp.sysCamp);
                    list.Remove(camp);
                    i--;
                }
            }
            foreach (long l in toRemove)
            {
                if (list.Count == 0)
                    return;
                RemoveResults(list, l);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static XproInfoBaseDao CreateXproEnumInfo<T>() where T : struct, IComparable
        {
            return new XproInfoDao<T>()
            {
                QueryItemFunction = (id) =>
                {
                    return (T)Enum.ToObject(typeof(T), id);
                },
                QueryItemsFunction = (input) =>
                {
                    List<T> result = new List<T>();
                    foreach (T item in Enum.GetValues(typeof(T)))
                    {
                        result.Add(item);
                    }
                    return result.ToArray();
                },
                CreateBeschreibung = (item,rval) => (item is Enum ? (item as Enum).Description() : item.ToString()),
                CreateBezeichnung = item => (item is Enum ? (item as Enum).Description() : item.ToString()),
            };
        }

        private static XproInfoBaseDao CreateXproInfo<T>(string columns, Func<T, string> CreateBezeichnung, Func<T,XproEntityDto, string> CreateBeschreibung, Sorting[] sorting = null)
        {
            return new XproInfoDao<T>()
            {
                QueryItemFunction = (id) =>
                {
                    var bo = CreateEntityBo();
                    return bo.getDetails<T>(id);
                },
                QueryItemsFunction = (input) =>
                {
                    var bo = CreateSearchBo<T>();
                    return bo.search(CreateISearchDto(columns, input.Filter, sorting)).results;
                },
                CreateBeschreibung = CreateBeschreibung,
                CreateBezeichnung = CreateBezeichnung
            };
        }

        protected virtual DictionaryListsBo CreateDictionaryListsBo(String isoCode)
        {
            return new DictionaryListsBo(DAOFactoryFactory.getInstance().getDictionaryListsDao(), new CachedTranslateDao(), isoCode);
        }

        protected static EntityBo CreateEntityBo()
        {
            return new EntityBo(DAOFactoryFactory.getInstance().getEntityDao());
        }

        protected static SearchBo<T> CreateSearchBo<T>()
        {
            return new SearchBo<T>(SearchQueryFactoryFactory.getInstance());
        }
        private static SearchBo<T> CreateSearchBo<T>(long permissionId)
        {
            return new SearchBo<T>(SearchQueryFactoryFactory.getInstance(), permissionId);
        }
        private static SearchBo<T> CreateSearchBo<T>(QueryInfoData data)
        {
            return new SearchBo<T>(data);
        }
        private static SearchBo<T> CreateSearchBo<T>(QueryInfoData data, long permissionId)
        {
            SearchBo<T> rval = new SearchBo<T>(data);
            rval.setPermission(permissionId);
            return rval;
        }

        protected static iSearchDto CreateISearchDto(string searchcolumns, string filter, Sorting[] sorting = null)
        {
            if (filter != null) filter = filter.Trim();

            return new iSearchDto()
            {
                filters = (string.IsNullOrEmpty(searchcolumns) ? null : new Filter[]
                            {
                                new Filter()
                                {
                                    fieldname = searchcolumns,
                                    filterType = FilterType.WEB2,
                                    value = filter
                                }
                            }),
                amount = 100,
                pageSize = 50,
                skip = 0,
                searchType = Cic.One.DTO.SearchType.Partial,
                sortFields = sorting
            };
        }

        protected static iSearchDto CreateISearchDto(string searchcolumns, string filter, Filter[] filters, Sorting[] sorting = null)
        {

            List<Filter> allFilters = new List<Filter>(filters);
            if (filter != null)
            {
                filter = filter.Trim();
                allFilters.Add(new Filter()
                {
                    fieldname = searchcolumns,
                    filterType = FilterType.WEB2,
                    value = filter
                });
            }
            return new iSearchDto()
            {
                filters = allFilters.ToArray(),
                amount = 100,
                pageSize = 50,
                skip = 0,
                searchType = Cic.One.DTO.SearchType.Partial,
                sortFields = sorting
            };
        }

        protected static iSearchDto CreateISearchDto(string searchcolumns, string filter, string sortfield, SortOrder sortOrder, List<Filter> filters = null)
        {
            if (filter != null) filter = filter.Trim();

            if (filters == null)
                filters = new List<Filter>();

            filters.Add(new Filter()
                                {
                                    fieldname = searchcolumns,
                                    filterType = FilterType.WEB2,
                                    value = filter
                                });

            return new iSearchDto()
            {
                filters = filters.ToArray(),
                amount = 100,
                pageSize = 50,
                skip = 0,
                searchType = Cic.One.DTO.SearchType.Partial,
                sortFields = new Sorting[]
                {
                    new Sorting()
                    {
                        fieldname= sortfield,
                        order = sortOrder
                    }
                }
            };
        }


        public XproEntityDto[] getXproItems(IXproLoaderDao loader, igetXproItemsDto input)
        {
            if (dictionary.ContainsKey(input.Area))
            {
                XproInfoBaseDao info = dictionary[input.Area];
                return info.getXproItems(loader, input);
            }
            if (input.areaCode != null && dictionaryCode.ContainsKey(input.areaCode))
            {
                XproInfoBaseDao info = dictionaryCode[input.areaCode];
                return info.getXproItems(loader, input);
            }
            if(input.Area==XproEntityType.STRING)
            {
                XproInfoDao xd = new XproInfoDao()
                {
                    QueryItemFunction2 = (inputPar) =>
                    {
                        var bo = CreateDictionaryListsBo(inputPar.isoCode);
                        return bo.listByCode(inputPar.areaCode, inputPar.domainId).FirstOrDefault((a) => a.sysID == inputPar.EntityId);
                    },
                    QueryItemsFunction = (filter) =>
                    {




                        

                        var bo = CreateDictionaryListsBo(filter.isoCode);
                       
                        
                        if (filter.domainId!=null)
                            return bo.listByCode(filter.areaCode, filter.domainId);
                        else if (filter.domain.HasValue)
                            return bo.listByCode(filter.areaCode, filter.domain.Value.ToString());
                        else
                            return bo.listByCode(filter.areaCode, null);
                    },
                };
                return xd.getXproItems(loader, input);
            }
            _log.Warn("Area: " + input.Area+"/"+input.areaCode + " not found");
            return null;
        }
       
        public XproEntityDto getXproItem(IXproLoaderDao loader, igetXproItemDto input)
        {
            if (dictionary.ContainsKey(input.Area))
            {
                XproInfoBaseDao info = dictionary[input.Area];
                var data = info.getXproItem(loader, input);

                if (data == null)
                {
                    _log.Warn("Entity Id " + input.EntityId + " not found for Area: " + input.Area);
                    return null;
                }
                return data;
            }
            if (input.areaCode!=null && dictionaryCode.ContainsKey(input.areaCode))
            {
                XproInfoBaseDao info = dictionaryCode[input.areaCode];
                var data = info.getXproItem(loader, input);

                if (data == null)
                {
                    _log.Warn("Entity Id " + input.EntityId + " not found for Area: " + input.Area);
                    return null;
                }
                return data;
            }
            if (input.Area == XproEntityType.STRING)
            {
                XproInfoDao xd = new XproInfoDao()
                {
                    QueryItemFunction2 = (inputPar) =>
                    {
                        var bo = CreateDictionaryListsBo(inputPar.isoCode);
                        return bo.listByCode(inputPar.areaCode, inputPar.domainId).FirstOrDefault((a) => a.sysID == inputPar.EntityId);
                    },
                    QueryItemsFunction = (filter) =>
                    {
                        var bo = CreateDictionaryListsBo(filter.isoCode);


                        if (filter.domainId != null)
                            return bo.listByCode(filter.areaCode, filter.domainId);
                        else if (filter.domain.HasValue)
                            return bo.listByCode(filter.areaCode, filter.domain.Value.ToString());
                        else
                            return bo.listByCode(filter.areaCode, null);
                    },
                };
                return xd.getXproItem(loader, input);
            }
            _log.Warn("Area: " + input.Area + " not found");
            return null;
        }
    }





}