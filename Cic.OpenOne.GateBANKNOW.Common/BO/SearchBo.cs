using System;
using System.Collections.Generic;
using System.Linq;
using Devart.Data.Oracle;
using Cic.OpenOne.GateBANKNOW.Common.DTO.Search;
using Cic.OpenOne.GateBANKNOW.Common.DTO;
using Cic.OpenOne.GateBANKNOW.Common.DAO;
using Cic.OpenOne.Common.Util.Logging;
using System.Reflection;
using Cic.OpenOne.Common.Util.Collection;
using System.Text;


namespace Cic.OpenOne.GateBANKNOW.Common.BO
{
    

    /// <summary>
    /// Query Patterns for optimizing the oracle-search-behaviour in PTA-Environment
    /// </summary>
    class QueryInfoDataType2 : QueryInfoData
    {
            public QueryInfoDataType2()
            {
                _queryStructure = new QueryStructure();
                _queryStructure.countQuery = "select count(*) from  {0} where {1}";//0=3, 1=4
                _queryStructure.completeQuery = "select  {0} from {1} where {2}";//0=2, 1=3, 2=4
                _queryStructure.partialQuery = "select {0} FROM {1},  (SELECT * FROM (SELECT rownum rnum, a.* from (  select  {2} from {3} where {4} ) a   WHERE rownum <= {5}) WHERE rnum > {6}) b   where  {7}";
            }

            public QueryInfoDataType2(QueryInfoDataType2 org)
                    : base(org)
            {

            }

            override public object[] getCompleteParams()
            {
                return new object[] { resultFields_0, searchTables_3, searchConditions_4 };
               
            }
            override public object[] getCountParams()
            {
                return new object[] { searchTables_3, searchConditions_4 };

            }
            override public object[] getPartialParams()
            {
                return new object[] { resultFields_0, resultTables_1, idFields_2, searchTables_3, searchConditions_4, pageSize_5, startRow_6, resultConditions_7 };

            }

            public override QueryInfoData clone()
            {
                return new QueryInfoDataType2(this);
            }
    }
    /// <summary>
    /// Default Search Query Pattern
    /// </summary>
    class QueryInfoDataType1 : QueryInfoData
    {
        public QueryInfoDataType1()
        {
            _queryStructure = new QueryStructure();
            _queryStructure.countQuery = "select count(*) from  {0} where {1}";//0=3, 1=4
            _queryStructure.completeQuery = "select  {0} from {1} where {2}";//0=2, 1=3, 2=4
            _queryStructure.partialQuery = "SELECT * FROM (SELECT rownum rnum, a.* FROM( select {0} from {1} where {2} ) a WHERE rownum <= {3}) WHERE rnum > {4}";//2=4, 3=5, 4=6
        }

        public QueryInfoDataType1(QueryInfoDataType1 org)
                : base(org)
        {

        }

        override public object[] getCompleteParams()
        {
            return new object[] { resultFields_0, searchTables_3, searchConditions_4 };

        }
        override public object[] getCountParams()
        {
            return new object[] { resultTables_1, searchConditions_4 };

        }
        override public object[] getPartialParams()
        {
            return new object[] { resultFields_0, resultTables_1, searchConditions_4, pageSize_5, startRow_6 };

        }
        public override QueryInfoData clone()
        {
            return new QueryInfoDataType1(this);
        }
    }

    /// <summary>
    /// Generic Search BO for all Types of Entities
    /// </summary>
    /// <typeparam name="R"></typeparam>
    public class SearchBo<R> : ISearch<R>
    {
        private static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        
        private string entity = typeof(R).ToString();
        private string entityTable;        
        private string entityField;

        private SearchDao<R> dao = new SearchDao<R>();
        private SearchDao<int> daoCount = new SearchDao<int>();
        private Dictionary<Type, string> typeTableMap = new Dictionary<Type, string>();
        private Dictionary<Type, string> typePkeyMap = new Dictionary<Type, string>();
        private Dictionary<Type, string> typeITkeyMap = new Dictionary<Type, string>();
        private QueryInfoData extQueryInfo;

        private Dictionary<Type, Dictionary<String, String>> typeFilterConfigMap = new Dictionary<Type, Dictionary<String, String>>();

        private long permissionId = 0;
        private string permissionType;
        private bool permissionNeeded = false;
        private bool superOptimizedSearchEnabled = true;//enables result caching-logic
        private bool deliverQueryParameters4cache = false;// Für 1*N Suche

        /// <summary>
        /// Constructor
        /// </summary>
        public SearchBo()
        {
            typeTableMap[typeof(AngebotDto)] = "ANGEBOT";
            typeTableMap[typeof(KundeDto)] = "IT";
            typeTableMap[typeof(VertragDto)] = "ANTRAG";
            typeTableMap[typeof(AntragDto)] = "ANTRAG";
            typeTableMap[typeof(FileattDto)] = "FILEATT";
            typeTableMap[typeof(FileDto)] = "DMSDOC";
            typeTableMap[typeof(DmsDocDto)] = "DMSDOC";

            typePkeyMap[typeof(AngebotDto)] = "ANGEBOT.SYSID";
            typePkeyMap[typeof(AntragDto)] = "ANTRAG.SYSID";
            typePkeyMap[typeof(KundeDto)] = "IT.SYSIT";
            typePkeyMap[typeof(VertragDto)] = "ANTRAG.SYSID";
            typePkeyMap[typeof(FileattDto)] = "FILEATT.SYSFILEATT";
            typePkeyMap[typeof(FileDto)] = "DMSDOC.SYSDMSDOC";
            typePkeyMap[typeof(DmsDocDto)] = "DMSDOC.SYSDMSDOC";

            typeITkeyMap[typeof(AngebotDto)] = "ANGEBOT.SYSIT";
            typeITkeyMap[typeof(AntragDto)] = "ANTRAG.SYSIT";
            typeITkeyMap[typeof(VertragDto)] = "ANTRAG.SYSKD";

            //map some filterfields to a certain where-condition where {0} will be the filter-field given from outside and {1} will be the paramtername (: has to be inside the special-filter-string!)
            Dictionary<string, string> filterMap = new Dictionary<string, string>();
            //for like
            filterMap["PERSON.NAME"] = "exists (select 1 from person WHERE UPPER({0}) LIKE UPPER(:{1}) and antrag.syskd=person.sysperson)";
            filterMap["PERSON.VORNAME"] = "exists (select 1 from person WHERE UPPER({0}) LIKE UPPER(:{1}) and antrag.syskd=person.sysperson)";
            filterMap["PERSON.ORT"] = "exists (select 1 from person WHERE UPPER({0}) LIKE UPPER(:{1}) and antrag.syskd=person.sysperson)";
            filterMap["WFZUSTMAP.SYSCODE"] = "exists (select 1  from wfzust wfzustMap, wftzust where wftzust.syswfzust = wfzustMap.syswfzust and wftzust.syslease=vt.sysid and UPPER({0}) LIKE UPPER(:{1})  )";
            //for equal
            filterMap["VTRUEKMAP.ZUSTAND"] = "EXISTS (SELECT 1 FROM vtruek vtruekmap WHERE sysvtruek = (SELECT MAX(sysvtruek) FROM vtruek WHERE vtruek.sysvt = vt.sysid)  and upper({0}) = Upper(:{1}) )";
            typeFilterConfigMap[typeof(VertragDto)] = filterMap;

            filterMap = new Dictionary<string, string>();
            filterMap["ANTRAG.ANTRAG"] = "exists (select 1 from antrag WHERE UPPER({0}) LIKE UPPER(:{1}) and ANTRAG.SYSIT=IT.SYSIT)";
            filterMap["ANGEBOT.ANGEBOT"] = "exists (select 1 from angebot WHERE UPPER({0}) LIKE UPPER(:{1}) and angebot.SYSIT=IT.SYSIT)";
            typeFilterConfigMap[typeof(KundeDto)] = filterMap;

            filterMap = new Dictionary<string, string>();
            //for like
            filterMap["IT.NAME"] = "exists (select 1 from it WHERE UPPER({0}) LIKE UPPER(:{1}) and ANTRAG.SYSIT=IT.SYSIT)";
            filterMap["IT.VORNAME"] = "exists (select 1 from it WHERE UPPER({0}) LIKE UPPER(:{1}) and ANTRAG.SYSIT=IT.SYSIT)";
            filterMap["IT.ORT"] = "exists (select 1 from it WHERE UPPER({0}) LIKE UPPER(:{1}) and ANTRAG.SYSIT=IT.SYSIT)";
            filterMap["ZUSTANDMAP.EXTERNERZUSTAND"] = @"exists( select 1 from  (SELECT extstate.zustand ExternerZustand
                                  FROM attribut,    attributdef,    state,    statedef extstate,    statedef intstate,    wftable
                                  WHERE attribut.sysstate    = state.sysstate
                                  AND attribut.sysattributdef= attributdef.sysattributdef
                                  AND attribut.sysstatedef   = extstate.sysstatedef
                                  AND state.sysstatedef      =intstate.sysstatedef
                                  AND state.syswftable       = wftable.syswftable
                                  AND wftable.syscode        = 'ANTRAG'
                                  and antrag.zustand      =intstate.zustand AND antrag.attribut=attributdef.attribut
                                  ) Zustandmap where ( UPPER({0}) LIKE UPPER(:{1})) )";


            typeFilterConfigMap[typeof(AntragDto)] = filterMap;

        }

        /// <summary>
        /// Constructor (parameterized)
        /// </summary>
        /// <param name="tableName"></param>
        public SearchBo(String tableName)
        {
            typeTableMap[typeof(R)] = tableName;
        }

        /// <summary>
        /// Allows setting the query from outside
        /// </summary>
        /// <param name="queryInfo"></param>
        public SearchBo(QueryInfoData queryInfo):this()
        {
            this.extQueryInfo = queryInfo;
        }

        /// <summary>
        /// Search 
        /// </summary>
        /// <param name="param">Search Parameters</param>
        /// <returns>Resultset</returns>
        public oSearchDto<R> search(iSearchDto param)
        {
            int retries = 1 + Cic.OpenOne.GateBANKNOW.Common.Settings.Default.SQLRetryCount;
            for (int i = 0; i < retries; i++)
            {
                String debugInfo = "";
                try
                {

                    return searchInternal(param, out debugInfo);
                }
                catch (Exception ex)
                {

                    if (i == (retries - 1))
                    {
                        _log.Error("Retrying failed for " + debugInfo + ": ", ex);
                        throw ex;
                    }
                    else
                    {
                        _log.Warn("Retrying search " + debugInfo + " because of " + ex.Message, ex);
                    }
                }
            }
            throw new Exception("Error in Query");

        }

        private static String getParams(object[] par)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(": ");
            if(par!=null)
            {
                foreach (object o in par)
                {
                    sb.Append(((Devart.Data.Oracle.OracleParameter)o).ParameterName);
                    sb.Append("=");
                    sb.Append(((Devart.Data.Oracle.OracleParameter)o).Value.ToString());
                    sb.Append(";");
                }
            }
            return sb.ToString();
        }

        private oSearchDto<R> searchInternal(iSearchDto param, out String debugInfo)
        {
            oSearchDto<R> rval = new oSearchDto<R>();
            debugInfo = "";
            if (!typeTableMap.ContainsKey(typeof(R)))
            {
                _log.Error("Unknown Table-Mapping for Search of type " + typeof(R));
                return rval;
            }
            entityTable = typeTableMap[typeof(R)];
            entityField = typePkeyMap[typeof(R)];

            if (param.pageSize == 0)
            {
                param.pageSize = 1;
            }

            String query = createQuery(param);
            object[] pars = deliverQueryParametersValues(param);
            if (query.IndexOf(":") < 0)
            {
                pars = null;
            }

            debugInfo = query + getParams(pars);

            if (_log.IsDebugEnabled)
            {
                _log.Debug("Search with query: " + query);
                if(pars!=null)
                {
                    _log.Debug("Parameters: " + pars.Length + " " + string.Join(";", pars));
                }
            }

            //delivers count for filtered search only
            if (param.searchType == SearchType.Informative)
            {
                String key = getParamKey(param, query+permissionId);
                if (!getCacheCount(entityTable).ContainsKey(key))
                {
                    getCacheCount(this.entityTable)[key] = this.daoCount.search(query, pars).First();
                }

                rval.searchCountFiltered = getCacheCount(entityTable)[key];
            }
            //delivers paged result
            else if (param.searchType == SearchType.Partial)
            {
                param.searchType = SearchType.Informative;
                object[] parsCount = deliverQueryParametersValues(param);
                
                String cquery = createQuery(param);
                String key = getParamKey(param, cquery + permissionId);


                if (!getCacheCount(entityTable).ContainsKey(key))
                {
                    getCacheCount(this.entityTable)[key] = this.daoCount.search(cquery, parsCount).First();
                }
                rval.searchCountMax = getCacheCount(entityTable)[key];
                rval.results = dao.search(query, pars).ToArray<R>();
                rval.searchCountFiltered = rval.results.Length;

                rval.searchNumPages = (int)Math.Ceiling(rval.searchCountMax * 1.0M / (1.0M * param.pageSize));
            }
            else//Complete
            {
                rval.results = dao.search(query, pars).ToArray<R>();
                rval.searchCountFiltered = rval.results.Length;
            }
            return rval;
        }
        /// <summary>
        /// creates a unique key for the input paramset with its filters to determine changed queries 
        /// </summary>
        /// <param name="param"></param>
        /// <param name="prefix"></param>
        /// <returns></returns>
        private static String getParamKey(iSearchDto param, String prefix)
        {
            StringBuilder sb = new StringBuilder(prefix);
            sb.Append(": ");
            if (param.filters != null)
            {
                foreach (Filter fp in param.filters)
                {

                    String searchPattern = getTrimed(fp.value);
                    if (searchPattern == null || searchPattern.Length == 0)
                    {
                        continue;
                    }

                    sb.Append(fp.fieldname);
                    sb.Append(fp.value);
                    sb.Append(fp.filterType);
                   
                }
            }
            return sb.ToString();
        }
       
       
        /// <summary>
        /// Set Permission
        /// </summary>
        /// <param name="permissionId"></param>
        /// <param name="permissionNeeded"></param>
        /// <param name="permissionType"></param>
        public void setPermission(long permissionId, bool permissionNeeded, string permissionType)
        {
            this.permissionId = permissionId;
            this.permissionNeeded = permissionNeeded;
            this.permissionType = permissionType;

        }

        /// <summary>
        /// Create Searchquery
        /// </summary>
        /// <param name="param">Parameters</param>
        /// <returns>Query</returns>
        private string createQuery(iSearchDto param)
        {

            if (param == null)
            {
                return "";
            }

            superOptimizedSearchEnabled = true;
            deliverQueryParameters4cache = false;// Für 1*N Suche
            QueryInfoData infoData = new QueryInfoDataType1();
            infoData.resultFields_0 = entityTable + ".*";
            infoData.resultTables_1 = entityTable;
            infoData.searchTables_3 = entityTable;
            infoData.searchConditions_4 =" 1=1 ";

           
            
            String peunisatz = " AND " + entityField + " IN  (SELECT sysid FROM peuni, perolecache WHERE area = '"+entityTable+"' AND peuni.sysperole = perolecache.syschild AND perolecache.sysparent = '"+permissionId+" ')";

           
            #region Entity Specific Queries
            if (typeof(R) == typeof(AntragDto))
            {
                String vsextQuery = new EaiparDao().getEaiParFileByCode("EXT_VS_VERFUEGBAR_ANTRAG", "select 0 from dual");
                vsextQuery = vsextQuery.Replace(":sysperole", "" + permissionId);
                String ddpposfield = @"(SELECT 
                                          NVL(DECODE(ddlkpcol.type,2,kppos.wert, ddlkpspos.value),ddlkpspos.value) value
                                      FROM ddlkpcol,
                                        ddlkpspos,
                                        (SELECT ddlkppos.sysddlkppos,
                                          vc_ddlkppos.id,
                                          NVL(vc_ddlkppos.actualterm, ddlkppos.value) Wert
                                        FROM cic.vc_ddlkppos,
                                          ddlkppos
                                        WHERE vc_ddlkppos.sysddlkppos = ddlkppos.sysddlkppos
                                        AND ddlkppos.code             = 'ANTRAGSZUSTAND'
                                        AND vc_ddlkppos.sysctlang     = 1
                                        ) KPPOS
                                      WHERE ddlkpspos.sysddlkpcol=ddlkpcol.sysddlkpcol
                                      AND ddlkpcol.code          ='ANTRAGSZUSTAND'
                                      AND ddlkpspos.area         = 'ANTRAG'
                                      AND KPPOS.id (+)           = ddlkpspos.value
                                      and ddlkpspos.sysid = antrag.sysid
                                      ) zustandBemerkung";

                infoData = new QueryInfoDataType2();
                infoData.resultFields_0 = "(" + vsextQuery + ") extvscode, antrag.*,  prproduct.name as prProductBezeichnung, PRPRODUCT.sysprproduct as sysprprod,  "+ddpposfield+", (select ausdatum from vt where vt.sysid = antrag.sysvt) auszahlungsdatum ";
                infoData.resultTables_1 = "antrag_v antrag,antkalk_v antkalk,prproduct_v prproduct, antob, antobbrief ";
                infoData.idFields_2 = typePkeyMap[typeof(AntragDto)];
                infoData.searchTables_3 = "antrag_V antrag , IT_V IT, antob, antobbrief";

                infoData.searchConditions_4 = "ANTRAG.SYSIT=IT.SYSIT(+) and antrag.sysid=antob.sysantrag(+) and antob.sysob=antobbrief.SYSANTOBBRIEF(+)";
                infoData.resultConditions_7 = "b.sysid=antrag.sysid and ANTRAG.SYSID=ANTKALK.SYSANTRAG(+) and antkalk.sysprproduct=prproduct.sysprproduct(+)  and antrag.sysid=antob.sysantrag(+) and antob.sysob=antobbrief.SYSANTOBBRIEF(+) ";
                if (superOptimizedSearchEnabled)
                {
                    infoData.searchTables_3 = "antrag_V antrag , IT_V IT,antkalk_v antkalk,prproduct_v prproduct, antob, antobbrief ";
                    infoData.searchConditions_4 = "ANTRAG.SYSIT=IT.SYSIT(+) and ANTRAG.SYSID=ANTKALK.SYSANTRAG(+) and antkalk.sysprproduct=prproduct.sysprproduct(+) and antrag.sysid=antob.sysantrag(+) and antob.sysob=antobbrief.SYSANTOBBRIEF(+)";
                }


               
                
            }
            else if (typeof(R) == typeof(AngebotDto))
            {
               
                infoData.resultFields_0 = " ANGEBOT.*";
                infoData.resultTables_1 = "ANGEBOT,IT";
                //BRN10 PREISSCHILD filter
                infoData.searchConditions_4 = "ANGEBOT.SYSIT=IT.SYSIT(+)  AND (UPPER(ANGEBOT.ZUSTAND) NOT LIKE UPPER('PREISSCHILD')) ";
                if (superOptimizedSearchEnabled)
                {
                    infoData.searchTables_3 = "ANGEBOT,IT";
                }
              
            }
            else if (typeof(R) == typeof(VertragDto))
            {

                String extQuery = new EaiparDao().getEaiParFileByCode("RW_VERL_VERFUEGBAR_WEB", AngAntDao.RW_VERL_VERFUEGBAR_WEB_DEFAULT);
                String buchwertQuery = new EaiparDao().getEaiParFileByCode(BuchwertBo.BW_PAR_FILE_CODE, BuchwertBo.BW_CALC_ALLOWED);
                String vsextQuery = new EaiparDao().getEaiParFileByCode("EXT_VS_VERFUEGBAR_VT", "select 0 from dual");

                vsextQuery = vsextQuery.Replace(":sysperole", ""+permissionId);
                String wfzustMapField = "(select LISTAGG(wfzust.syscode,',') within Group (order by syscode) as syscode  from wfzust, wftzust where wftzust.syswfzust = wfzust.syswfzust and syslease=vt.sysid) wfzustSyscode";
                String vtruekField = "(SELECT zustand FROM vtruek WHERE sysvtruek IN (SELECT MAX(sysvtruek) FROM vtruek WHERE vtruek.sysvt = vt.sysid)) vtruekZustand";
                infoData.resultFields_0 = "(" + vsextQuery + ") extvscode, (" + extQuery + ") isExtendible,(" + buchwertQuery + ") isBuchwertCalculationAllowed, VT.RW rw, VT.ende vtende, VT.attribut zustandExtern, ANTRAG.*, antobbrief.fident chassisnummer,antobbrief.stammnummer stammnummer, antobbrief.ecodestatus, antob.objektvt model,antob.fabrikat marke,antob.kennzeichen kontrollschild, antob.kennzeichen, antob.bezeichnung,vart.bezeichnung as vertragsartbezeichnung,vt.endekz vtendekz, vt.zustand vtzustand, vt.sysrwga vtsysrwga, VT.vertrag vtvertrag," + wfzustMapField+","+vtruekField;
                infoData.resultTables_1 = "VT,ANTRAG,Person, antob, antobbrief,vart";
                infoData.searchConditions_4 = "antrag.syskd=person.sysperson(+) and antrag.sysid=antob.sysantrag(+) and antob.sysob=antobbrief.SYSANTOBBRIEF(+) and VART.sysvart = antrag.sysvart and vt.sysantrag=antrag.sysid";
                deliverQueryParameters4cache = true;
                if (superOptimizedSearchEnabled)
                {
                    infoData.searchTables_3 = "VT,ANTRAG,Person, antob, antobbrief,vart";
                }
                
            }

            else if (typeof(R) == typeof(FileattDto))
            {
                
                infoData.resultFields_0 = "FILEATT.* ";
                infoData.resultTables_1= "FILEATT ";
                infoData.searchTables_3 = "FILEATT ";
                peunisatz = " AND FILEATT.SYSID IN  (SELECT sysid FROM peuni, perolecache WHERE area = FILEATT.AREA AND peuni.sysperole = perolecache.syschild AND perolecache.sysparent = '" + permissionId + " ')";
                infoData.searchConditions_4 = "1=1 ";
            }

            else if (typeof(R) == typeof(FileDto))
            {
                superOptimizedSearchEnabled = false;
                infoData.resultFields_0 = "DMSDOCAREA.AREA area, DMSDOCAREA.SYSID sysId, DMSDOCAREA.SYSDMSDOC sysFile, length(DMSDOC.INHALT) fileSize, dmsdoc.sysdmsdoc, DMSDOC.DATEINAME filename, case when DMSDOC.ungueltigflag > 0 then 0 else 1 end activFlag, DMSDOC.UNGUELTIGFLAG,  DMSDOC.BEMERKUNG description, DMSDOC.GEDRUCKTVON syscrtuser, DMSDOC.GEDRUCKTAM sysCrtDate,  DMSDOC.GEDRUCKTUM sysCrtTime ";
                infoData.resultTables_1 = "DMSDOCAREA, DMSDOC";
                infoData.searchTables_3 = "DMSDOCAREA, DMSDOC ";
                peunisatz = " AND DMSDOCAREA.SYSID IN  (SELECT sysid FROM peuni, perolecache WHERE area = DMSDOCAREA.AREA AND peuni.sysperole = perolecache.syschild AND perolecache.sysparent = '" + permissionId + " ')";
                infoData.searchConditions_4 = "dmsdocarea.sysdmsdoc = dmsdoc.sysdmsdoc and DMSDOC.ungueltigflag = 0";
            }
           
            if (extQueryInfo != null) { 
                infoData = extQueryInfo.clone();
                superOptimizedSearchEnabled = extQueryInfo.optimizeQuery;
            }

            #endregion Entity Specific Queries

            
            
            if (superOptimizedSearchEnabled)
            {
                //optimization-idea
                //when unknown query:
                //  now outer rows-query/conditions
                //  fetch only primary key
                //  generate a cached id-list for this query
                //always:
                //  fetch the section to use from the id-list by given parameters skip and pageSize
                //  use inner query with joins and just one search-condition: primary-key in in(<section-ids>) for the detail-fetching of all fields
                object[] parsCache = deliverQueryParametersValues(param);
                String resultIdCacheKey = getParamKey(param, entityTable + "_" + permissionId + deliveryQuerySort(param));

                if (!getCacheResultId(entityTable).ContainsKey(resultIdCacheKey) || hasCacheChanged(entityTable,resultIdCacheKey))//id cache noch nicht befüllt
                {
                    String orgFields = infoData.resultFields_0;
                    infoData.resultFields_0 = entityField;//0,3,4
                    String orgWhere = infoData.searchConditions_4;
                    if (permissionNeeded)
                    {
                        infoData.searchConditions_4 += peunisatz;
                    }
                    infoData.searchConditions_4 += deliverQueryParametersString(param);
                    infoData.searchConditions_4 += deliveryQuerySort(param);
                    
                    
                    String idQuery = String.Format(infoData.queryStruct.completeQuery, infoData.getCompleteParams());
                    SearchDao<long> sd = new SearchDao<long>();
                    if (_log.IsDebugEnabled)
                    {
                        _log.Debug("Search Id's with query: " + idQuery);
                        if (parsCache != null)
                        {
                            _log.Debug("Parameters: " + parsCache.Length + " " + string.Join(";", parsCache));
                        }
                    }
                    getCacheResultId(entityTable)[resultIdCacheKey] = sd.search(idQuery, parsCache);
                    infoData.resultFields_0 = orgFields;
                    if (!deliverQueryParameters4cache)
                    {
                        infoData.searchConditions_4 = orgWhere;
                    }
                    else
                    {
                        infoData.searchConditions_4 = orgWhere + deliverQueryParametersString(param);
                    }
                }
                List<long> ids = getCacheResultId(entityTable)[resultIdCacheKey];

                //simple logic to fetch the size by the rval of this method:
                if (param.searchType == SearchType.Informative)
                {
                    return "select " + ids.Count + " from dual";
                }

                //avoid range-fetching errors:
                int useSize = param.pageSize;
                if (param.skip > ids.Count)
                {
                    param.skip = ids.Count - 1 - useSize;
                    
                }
                if (param.skip < 0)
                {
                    param.skip = 0;
                }
                if (param.skip + useSize >= ids.Count)
                {
                    useSize = ids.Count - param.skip;
                }
                ids = ids.GetRange(param.skip, useSize);
                //avoid error on empty list
                if (ids.Count == 0)
                {
                    ids.Add(-1);
                }
                //build the simple id fech query
                infoData.searchConditions_4 += " and " + entityField + " in (" + string.Join(",", ids) + ") " + deliveryQuerySort(param);

                return String.Format(infoData.queryStruct.completeQuery, infoData.getCompleteParams());
            }


            if (permissionNeeded)
            {
                infoData.searchConditions_4 += peunisatz;
            }
            infoData.pageSize_5 = (param.skip + param.pageSize).ToString();
            infoData.startRow_6 = param.skip.ToString();

            switch (param.searchType)
            {
                case SearchType.Informative:
                    infoData.searchConditions_4 += deliverQueryParametersString(param);

                    return String.Format(infoData.queryStruct.countQuery,infoData.getCountParams());
                  
                case SearchType.Partial:
                    infoData.searchConditions_4 += deliverQueryParametersString(param);
                    infoData.searchConditions_4 += deliveryQuerySort(param);

                    return String.Format(infoData.queryStruct.partialQuery, infoData.getPartialParams());
                  
                case SearchType.Complete:
                    infoData.searchConditions_4 += deliverQueryParametersString(param);
                    infoData.searchConditions_4 += deliveryQuerySort(param);

                    return String.Format(infoData.queryStruct.completeQuery, infoData.getCompleteParams());


            }
            return "";
        }

        /// <summary>
        /// Deliver Query Parameter Values
        /// </summary>
        /// <param name="isearchDto">Input Parameters</param>
        /// <returns>Return Data</returns>
        private object[] deliverQueryParametersValues(iSearchDto isearchDto)
        {
            List<OracleParameter> ParametersList = new List<OracleParameter>();
            if (isearchDto.filters != null)
            {
                foreach (Filter fp in isearchDto.filters)
                {
                    
                    String searchPattern = getTrimed(fp.value);
                    if (searchPattern == null || searchPattern.Length == 0)
                    {
                        continue;
                    }

                    if (searchPattern.ToUpper() == "NULL" && fp.filterType == FilterType.Equal)
                    {
                        continue;
                    }

                    string[] fieldList;
                    char[] separ = { ',' };
                    int scount = 0;
                    switch (fp.filterType)
                    {
                        case FilterType.Like:

                            fieldList = fp.fieldname.Split(separ);
                            scount = fieldList.Count();

                            for (int i = 0; i < scount; i++)
                            {
                                ParametersList.Add(new Devart.Data.Oracle.OracleParameter(getParameterName(fp.filterType, fieldList[i]), "%" + searchPattern + "%"));
                            }
                            break;
                        case FilterType.NotLike:

                            fieldList = fp.fieldname.Split(separ);
                            scount = fieldList.Count();

                            for (int i = 0; i < scount; i++)
                            {
                                ParametersList.Add(new Devart.Data.Oracle.OracleParameter(getParameterName(fp.filterType, fieldList[i]), "%" + searchPattern + "%"));
                            }
                            break;
                        case FilterType.Equal:
                            fieldList = fp.fieldname.Split(separ);
                            scount = fieldList.Count();

                            for (int i = 0; i < scount; i++)
                            {
                                ParametersList.Add(new Devart.Data.Oracle.OracleParameter(getParameterName(fp.filterType, fieldList[i]), searchPattern));
                            }
                            break;
                        case FilterType.Begins:
                            fieldList = fp.fieldname.Split(separ);
                            scount = fieldList.Count();

                            for (int i = 0; i < scount; i++)
                            {
                                ParametersList.Add(new Devart.Data.Oracle.OracleParameter(getParameterName(fp.filterType, fieldList[i]), searchPattern + "%"));
                            }
                            break;
                        case FilterType.Ends:
                            fieldList = fp.fieldname.Split(separ);
                            scount = fieldList.Count();

                            for (int i = 0; i < scount; i++)
                            {
                                ParametersList.Add(new Devart.Data.Oracle.OracleParameter(getParameterName(fp.filterType, fieldList[i]), "%" + searchPattern));
                            }
                            break;
                        case FilterType.GT:
                            fieldList = fp.fieldname.Split(separ);
                            scount = fieldList.Count();

                            for (int i = 0; i < scount; i++)
                            {
                                ParametersList.Add(new Devart.Data.Oracle.OracleParameter(getParameterName(fp.filterType, fieldList[i]), fp.value));
                            }
                            break;
                        case FilterType.LT:
                            fieldList = fp.fieldname.Split(separ);
                            scount = fieldList.Count();

                            for (int i = 0; i < scount; i++)
                            {
                                ParametersList.Add(new Devart.Data.Oracle.OracleParameter(getParameterName(fp.filterType, fieldList[i]), fp.value));
                            }
                            break;

                        case FilterType.RegEx:
                            fieldList = fp.fieldname.Split(separ);
                            scount = fieldList.Count();

                            for (int i = 0; i < scount; i++)
                            {
                                ParametersList.Add(new Devart.Data.Oracle.OracleParameter(getParameterName(fp.filterType, fieldList[i]), fp.value));
                            }
                            break;

                        case FilterType.WEB:

                            ParametersList.Add(new Devart.Data.Oracle.OracleParameter(getParameterName(fp.filterType, fp.fieldname), fp.value));

                            break;

                        case FilterType.GE:
                            fieldList = fp.fieldname.Split(separ);
                            scount = fieldList.Count();

                            for (int i = 0; i < scount; i++)
                            {
                                ParametersList.Add(new Devart.Data.Oracle.OracleParameter(getParameterName(fp.filterType, fieldList[i]), fp.value));
                            }
                            break;

                        case FilterType.LE:
                            fieldList = fp.fieldname.Split(separ);
                            scount = fieldList.Count();

                            for (int i = 0; i < scount; i++)
                            {
                                ParametersList.Add(new Devart.Data.Oracle.OracleParameter(getParameterName(fp.filterType, fieldList[i]), fp.value));
                            }
                            break;

                        case FilterType.DateEqual:
                            fieldList = fp.fieldname.Split(separ);
                            scount = fieldList.Count();

                            for (int i = 0; i < scount; i++)
                            {
                                ParametersList.Add(new Devart.Data.Oracle.OracleParameter(getParameterName(fp.filterType, fieldList[i]), searchPattern));
                            }
                            break;

                        case FilterType.DateGT:
                            fieldList = fp.fieldname.Split(separ);
                            scount = fieldList.Count();

                            for (int i = 0; i < scount; i++)
                            {
                                ParametersList.Add(new Devart.Data.Oracle.OracleParameter(getParameterName(fp.filterType, fieldList[i]), searchPattern));
                            }
                            break;

                        case FilterType.DateLT:
                            fieldList = fp.fieldname.Split(separ);
                            scount = fieldList.Count();

                            for (int i = 0; i < scount; i++)
                            {
                                ParametersList.Add(new Devart.Data.Oracle.OracleParameter(getParameterName(fp.filterType, fieldList[i]), searchPattern));
                            }
                            break;

                        case FilterType.DateGE:
                            fieldList = fp.fieldname.Split(separ);
                            scount = fieldList.Count();

                            for (int i = 0; i < scount; i++)
                            {
                                ParametersList.Add(new Devart.Data.Oracle.OracleParameter(getParameterName(fp.filterType, fieldList[i]), searchPattern));
                            }
                            break;

                        case FilterType.DateLE:
                            fieldList = fp.fieldname.Split(separ);
                            scount = fieldList.Count();

                            for (int i = 0; i < scount; i++)
                            {
                                ParametersList.Add(new Devart.Data.Oracle.OracleParameter(getParameterName(fp.filterType, fieldList[i]), searchPattern));
                            }
                            break;
                    }
                }
            }

            return ParametersList.ToArray();
        }

        private string getParameterName(FilterType filterType, string p)
        {
            return "p" + (int)filterType + p.Trim().Replace('.', '_');
        }

        /// <summary>
        /// Deliver Query Parameters String
        /// </summary>
        /// <param name="isearchDto">Input Data</param>
        /// <returns>Parameter string</returns>
        private string deliverQueryParametersString(iSearchDto isearchDto)
        {
            System.Text.StringBuilder queryBuilder = new System.Text.StringBuilder();
            if (isearchDto.filters != null)
            {

                foreach (Filter fp in isearchDto.filters)
                {
                    String searchPattern  = getTrimed(fp.value);
                    if (searchPattern == null || searchPattern.Length == 0 && fp.filterType != FilterType.Null && fp.filterType !=FilterType.NotNull)
                    {
                        continue;
                    }

                    string[] fieldList;
                    char[] separ = { ',' };
                    int scount = 0;
                    switch (fp.filterType)
                    {
                        case FilterType.Like:
                            fieldList = fp.fieldname.Split(separ);
                            scount = fieldList.Count();
                            queryBuilder.Append(" AND (");
                            for (int i = 0; i < scount; i++)
                            {
                                if (i > 0)
                                {
                                    queryBuilder.Append(" OR ");
                                }
                                String field = fieldList[i].Trim().ToUpper();
                                String pname = getParameterName(fp.filterType, fieldList[i]);
                                if (typeFilterConfigMap.ContainsKey(typeof(R)) && typeFilterConfigMap[typeof(R)].ContainsKey(field))
                                {
                                    String specialFilter = typeFilterConfigMap[typeof (R)][field];
                                    specialFilter = String.Format(specialFilter, new object[] { field, pname });
                                    queryBuilder.Append(specialFilter);    
                                }
                                else
                                {
                                    queryBuilder.Append(" UPPER(" + field + ") LIKE UPPER(:" + pname + ")");    
                                }
                                


                            }

                            queryBuilder.Append(")");
                            break;
                        case FilterType.NotLike:
                            fieldList = fp.fieldname.Split(separ);
                            scount = fieldList.Count();
                            queryBuilder.Append(" AND (");
                            for (int i = 0; i < scount; i++)
                            {
                                if (i > 0)
                                {
                                    queryBuilder.Append(" OR ");
                                }
                                queryBuilder.Append(" UPPER(" + fieldList[i].Trim() + ") NOT LIKE UPPER(:" + getParameterName(fp.filterType, fieldList[i]) + ")");
                            }

                            queryBuilder.Append(")");
                            break;

                        case FilterType.Equal:
                            fieldList = fp.fieldname.Split(separ);
                            scount = fieldList.Count();
                            if (searchPattern.ToUpper().Equals("NULL"))
                            {
                                queryBuilder.Append(" AND (");
                                for (int i = 0; i < scount; i++)
                                {
                                    if (i > 0)
                                    {
                                        queryBuilder.Append(" OR ");
                                    }

                                    queryBuilder.Append(fieldList[i].Trim() + " IS NULL ");
                                }
                                queryBuilder.Append(")");

                            }
                            else
                            {
                                queryBuilder.Append(" AND (");
                                for (int i = 0; i < scount; i++)
                                {
                                    if (i > 0)
                                    {
                                        queryBuilder.Append(" OR ");
                                    }

                                    String field = fieldList[i].Trim().ToUpper();
                                    String pname = getParameterName(fp.filterType, fieldList[i]);
                                    if (typeFilterConfigMap.ContainsKey(typeof(R)) && typeFilterConfigMap[typeof(R)].ContainsKey(field))
                                    {
                                        String specialFilter = typeFilterConfigMap[typeof(R)][field];
                                        specialFilter = String.Format(specialFilter, new object[] { field, pname });
                                        queryBuilder.Append(specialFilter);
                                    }
                                    else
                                    {
                                        queryBuilder.Append(field + " = :" + pname);
                                    }

                                    
                                }
                                queryBuilder.Append(")");
                            }
                            break;

                        case FilterType.Begins:
                            fieldList = fp.fieldname.Split(separ);
                            scount = fieldList.Count();
                            queryBuilder.Append(" AND (");
                            for (int i = 0; i < scount; i++)
                            {
                                if (i > 0)
                                {
                                    queryBuilder.Append(" OR ");
                                }
                                queryBuilder.Append(" UPPER(" + fieldList[i].Trim() + ") LIKE UPPER(:" + getParameterName(fp.filterType, fieldList[i]) + ")");
                            }
                            queryBuilder.Append(")");
                            break;

                        case FilterType.Ends:
                            fieldList = fp.fieldname.Split(separ);
                            scount = fieldList.Count();
                            queryBuilder.Append(" AND (");
                            for (int i = 0; i < scount; i++)
                            {
                                if (i > 0)
                                {
                                    queryBuilder.Append(" OR ");
                                }
                                queryBuilder.Append(fieldList[i].Trim() + " LIKE UPPER(:" + getParameterName(fp.filterType, fieldList[i]) + ")");
                            }
                            queryBuilder.Append(")");
                            break;

                        case FilterType.GT:
                            fieldList = fp.fieldname.Split(separ);
                            scount = fieldList.Count();
                            queryBuilder.Append(" AND (");
                            for (int i = 0; i < scount; i++)
                            {
                                if (i > 0)
                                {
                                    queryBuilder.Append(" OR ");
                                }
                                queryBuilder.Append(fieldList[i].Trim() + " > :" + getParameterName(fp.filterType, fieldList[i]) );
                            }
                            queryBuilder.Append(")");
                            break;

                        case FilterType.LT:
                            fieldList = fp.fieldname.Split(separ);
                            scount = fieldList.Count();
                            queryBuilder.Append(" AND (");
                            for (int i = 0; i < scount; i++)
                            {
                                if (i > 0)
                                {
                                    queryBuilder.Append(" OR ");
                                }
                                queryBuilder.Append(fieldList[i].Trim() + " < :" + getParameterName(fp.filterType, fieldList[i]) );
                            }
                            queryBuilder.Append(")");
                            break;


                        case FilterType.RegEx:
                            fieldList = fp.fieldname.Split(separ);
                            scount = fieldList.Count();
                            queryBuilder.Append(" AND (");
                            for (int i = 0; i < scount; i++)
                            {
                                if (i > 0)
                                {
                                    queryBuilder.Append(" OR ");
                                }
                                queryBuilder.Append(" REGEXP_LIKE(:" + fieldList[i].Trim() + ", :" + getParameterName(fp.filterType, fieldList[i]) + ")");
                            }
                            queryBuilder.Append(")");
                            break;

                        case FilterType.SQL:


                            queryBuilder.Append(" AND (" + fp.fieldname.Trim() + "  " + fp.value + ")");

                            break;

                        case FilterType.WEB:

                            queryBuilder.Append(" AND " + getWebQuery(fp));
                            break;

                        case FilterType.Null:
                            fieldList = fp.fieldname.Split(separ);
                            scount = fieldList.Count();
                            queryBuilder.Append(" AND (");
                            for (int i = 0; i < scount; i++)
                            {
                                if (i > 0)
                                {
                                    queryBuilder.Append(" OR ");
                                }
                                queryBuilder.Append(fieldList[i].Trim() + " IS NULL ");
                            }
                            queryBuilder.Append(")");
                            break;

                        case FilterType.NotNull:
                            fieldList = fp.fieldname.Split(separ);
                            scount = fieldList.Count();
                            queryBuilder.Append(" AND (");
                            for (int i = 0; i < scount; i++)
                            {
                                if (i > 0)
                                {
                                    queryBuilder.Append(" OR ");
                                }
                                queryBuilder.Append(fieldList[i].Trim() + " IS NOT NULL ");
                            }
                            queryBuilder.Append(")");
                            break;


                        case FilterType.GE:
                            fieldList = fp.fieldname.Split(separ);
                            scount = fieldList.Count();
                            queryBuilder.Append(" AND (");
                            for (int i = 0; i < scount; i++)
                            {
                                if (i > 0)
                                {
                                    queryBuilder.Append(" OR ");
                                }
                                queryBuilder.Append(fieldList[i].Trim() + " >= :" + getParameterName(fp.filterType, fieldList[i]) );
                            }
                            queryBuilder.Append(")");
                            break;

                        case FilterType.LE:
                            fieldList = fp.fieldname.Split(separ);
                            scount = fieldList.Count();
                            queryBuilder.Append(" AND (");
                            for (int i = 0; i < scount; i++)
                            {
                                if (i > 0)
                                {
                                    queryBuilder.Append(" OR ");
                                }
                                queryBuilder.Append(fieldList[i].Trim() + " <= :" + getParameterName(fp.filterType, fieldList[i]) );
                            }
                            queryBuilder.Append(")");
                            break;

                        case FilterType.DateEqual:
                            fieldList = fp.fieldname.Split(separ);
                            scount = fieldList.Count();
                            queryBuilder.Append(" AND (");
                            for (int i = 0; i < scount; i++)
                            {
                                if (i > 0)
                                {
                                    queryBuilder.Append(" OR ");
                                }
                                queryBuilder.Append("trunc(" + fieldList[i].Trim() + ") = to_date(:" + getParameterName(fp.filterType, fieldList[i]) + ", 'yyyy-mm-dd') AND " + fieldList[i].Trim() + " IS NOT NULL");
                            }
                            queryBuilder.Append(")");
                            break;



                        case FilterType.DateGT:
                            fieldList = fp.fieldname.Split(separ);
                            scount = fieldList.Count();
                            queryBuilder.Append(" AND (");
                            for (int i = 0; i < scount; i++)
                            {
                                if (i > 0)
                                {
                                    queryBuilder.Append(" OR ");
                                }
                                queryBuilder.Append("trunc(" + fieldList[i].Trim() + ")  > to_date(:" + getParameterName(fp.filterType, fieldList[i]) + ", 'yyyy-mm-dd') AND " + fieldList[i].Trim() + " IS NOT NULL");
                            }
                            queryBuilder.Append(")");
                            break;

                        case FilterType.DateLT:
                            fieldList = fp.fieldname.Split(separ);
                            scount = fieldList.Count();
                            queryBuilder.Append(" AND (");
                            for (int i = 0; i < scount; i++)
                            {
                                if (i > 0)
                                {
                                    queryBuilder.Append(" OR ");
                                }
                                queryBuilder.Append("trunc(" + fieldList[i].Trim() + ")  < to_date( :" + getParameterName(fp.filterType, fieldList[i]) + ", 'yyyy-mm-dd') AND " + fieldList[i].Trim() + " IS NOT NULL");
                            }
                            queryBuilder.Append(")");
                            break;



                        case FilterType.DateGE:
                            fieldList = fp.fieldname.Split(separ);
                            scount = fieldList.Count();
                            queryBuilder.Append(" AND (");
                            for (int i = 0; i < scount; i++)
                            {
                                if (i > 0)
                                {
                                    queryBuilder.Append(" OR ");
                                }
                                queryBuilder.Append("trunc(" + fieldList[i].Trim() + ")  >= to_date( :" + getParameterName(fp.filterType, fieldList[i]) + ", 'yyyy-mm-dd') AND " + fieldList[i].Trim() + " IS NOT NULL");
                            }
                            queryBuilder.Append(")");
                            break;

                        case FilterType.DateLE:
                            fieldList = fp.fieldname.Split(separ);
                            scount = fieldList.Count();
                            queryBuilder.Append(" AND (");
                            for (int i = 0; i < scount; i++)
                            {
                                if (i > 0)
                                {
                                    queryBuilder.Append(" OR ");
                                }
                                queryBuilder.Append("trunc(" + fieldList[i].Trim() + ")  <= to_date( :" + getParameterName(fp.filterType, fieldList[i]) + ", 'yyyy-mm-dd') AND " + fieldList[i].Trim() + " IS NOT NULL");
                            }
                            queryBuilder.Append(")");
                            break;


                    }
                }
            }
            return queryBuilder.ToString();
        }

        private String[] whiteListSort = { "ERFASSUNG", "NAME", "IT.NAME", "LAUFNUMMER", "ERFASSUNGZEIT","SYSWFTX", "GEDRUCKTAM"};

        /// <summary>
        /// Delivery Query Sort
        /// </summary>
        /// <param name="isearchDto">Input Data</param>
        /// <returns>Sort String</returns>
        public string deliveryQuerySort(iSearchDto isearchDto)
        {
            System.Text.StringBuilder queryBuilder = new System.Text.StringBuilder();
            if (isearchDto.sortFields != null)
            {
                foreach (Sorting s in isearchDto.sortFields)
                {
                    if (s.fieldname != null && s.fieldname.Trim() != "")
                    {
                        String fname = s.fieldname.Trim().ToUpper();
                        if(whiteListSort.Contains(fname))
                        {
                            queryBuilder.Append(fname + " " + s.order + ",");
                        }
                    }


                }

                if (queryBuilder != null && queryBuilder.Length>0)
                {
                    char[] endChar = { ',' };

                    return " ORDER BY " + queryBuilder.ToString().TrimEnd(endChar);
                }
                else
                {
                    return "";
                }
            }
            else
            {
                return "";
            }
        }

        /// <summary>
        /// Delivery of Query Group
        /// </summary>
        /// <param name="isearchDto">Input Data</param>
        /// <returns>Group query String</returns>
        public string deliveryQueryGroup(iSearchDto isearchDto)
        {
            System.Text.StringBuilder queryBuilder = new System.Text.StringBuilder();

            foreach (Grouping g in isearchDto.groupFields)
            {
                if (g.fieldname != null && g.fieldname != "")
                {
                    queryBuilder.Append(g.fieldname + ",");
                }
            }
            if (queryBuilder != null)
            {
                char[] endChar = { ',' };
                return " GROUP BY " + queryBuilder.ToString().TrimEnd(endChar);
            }
            else
            {
                return "";
            }
        }

        private static string getTrimed(string value)
        {
            if (value != null)
            {
                return value.Trim();
            }
            else
            {
                return value;
            }
        }
      

        /// <summary>
        /// Webquery auslösen
        /// </summary>
        /// <param name="filter">Filter</param>
        /// <returns>Rückgabedaten</returns>
        public string getWebQuery(Filter filter)
        {
            string[] fieldlist;
            string querystring = "1=1";
            if (filter.fieldname == null)
            {
                fieldlist = new string[0];

            }
            else
            {
                char[] separ = { ',' };
                fieldlist = filter.fieldname.Split(separ);
                string[] sp;
                int[] styp = null;
                int scount = 0;
                string[] st = filter.value.Split(' ');

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
                        querystring = querystring + " AND ( ";

                        for (int i = 0; i < fieldlist.Length; i++)
                        {
                            if (i > 0)
                            {
                                querystring = querystring + " OR ";
                            }
                            querystring = querystring + "UPPER(" + fieldlist[i] + ") LIKE UPPER('%" + toDB(sp[pos]) + "%')";
                            /*   try
                               {
                                   string dat = de.sirconic.util.ParseDate.getDatum(sp[pos]);
                                   if (!sp[pos].equals(dat)) {
                                   querystring = querystring + " OR ";
                                   querystring = querystring + "UPPER(" + befstring + Fieldlist[i] + aftstring + ") LIKE UPPER('%" + toDB(sp[pos]) + "%')";

                                    }
                               }
                               catch (Exception Ex)
                               {
                               }*/
                        }
                        querystring = querystring + " )";

                    }
                }

            }

            return querystring;
        }


        private string toDB(string text)
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
        /// returns true when the cache has been changed since last update 
        /// </summary>
        /// <param name="entityName"></param>
        /// <param name="cacheEntryKey"></param>
        /// <returns></returns>
        private bool hasCacheChanged(String entityName, String cacheEntryKey)
        {
            double ageofCache = Cic.OpenOne.Common.BO.Search.SearchCache.resultIdCache.getAge(cacheEntryKey);
            return Cic.OpenOne.Common.BO.Search.SearchCache.hasEntityChanged(entityName, ageofCache);
        }

        /// <summary>
        /// Returns the ID-Cache
        /// </summary>
        /// <param name="entityName"></param>
        /// <returns></returns>
        private CacheDictionary<String, List<long>> getCacheResultId(String entityName)
        {
            return Cic.OpenOne.Common.BO.Search.SearchCache.resultIdCache;
        }
        /// <summary>
        /// Returns the count cache
        /// </summary>
        /// <param name="entityName"></param>
        /// <returns></returns>
        private CacheDictionary<String, int> getCacheCount(String entityName)
        {
            return Cic.OpenOne.Common.BO.Search.SearchCache.countCache;
        }

    }
}
