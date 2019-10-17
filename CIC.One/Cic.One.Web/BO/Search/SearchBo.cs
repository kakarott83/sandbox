using System;
using System.Collections.Generic;
using System.Linq;
using Devart.Data.Oracle;
using Cic.OpenOne.Common.Util.Logging;
using System.Reflection;
using Cic.OpenOne.Common.Util.Collection;
using System.Text;
using Cic.One.Web.BO.Search;
using Cic.One.Web.Service.DAO;
using Cic.One.DTO;
using Cic.One.Utils.Util;
using System.Globalization;
using System.Data.Objects;
using Cic.OpenOne.Common.Model.DdOl;
using Cic.One.Web.DAO;
using Cic.OpenOne.Common.BO.Search;
using Cic.OpenOne.Common.Util.Config;
using Cic.One.Workflow.BO;

namespace Cic.One.Web.BO.Search
{
   

    public class SearchBo<R, T> : SearchBo<R>
    {

        /// <summary>
        /// Constructor
        /// </summary>
        public SearchBo(ISearchQueryInfoFactory factory)
            : base(factory)
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public SearchBo(ISearchQueryInfoFactory factory, long permissionId)
            : base(factory, permissionId)
        {
        }

        /// <summary>
        /// Constructor (parametriezed)
        /// </summary>
        /// <param name="tableName"></param>
        public SearchBo(QueryInfoData infoData)
            : base(infoData)
        {
        }


        /// <summary>
        /// Search 
        /// </summary>
        /// <param name="param">Search Parameters</param>
        /// <returns>Resultset</returns>
        public new oSearchDto<T> search(iSearchDto param)
        {
            oSearchDto<R> found = base.search(param);
            oSearchDto<T> result = dao.PostConvert<T>(found, permissionId);
            return result;
        }


    }


    /// <summary>
    /// Generic Search BO for all Types of Entities
    /// </summary>
    /// <typeparam name="R"></typeparam>
    public class SearchBo<R> : ISearch<R>
    {


        protected static readonly ILog _log = Log.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        protected string entity = typeof(R).ToString();

        protected ISearchDao<R> dao = new SearchDao<R>();
        protected SearchDao<int> daoCount = new SearchDao<int>();

        protected long permissionId = -1;
        private String isoCode = null;

        protected QueryInfoData infoData;

        #region Constructors
        /// <summary>
        /// Constructor fetching Query-Info from factory, no permissions used
        /// </summary>
        public SearchBo(ISearchQueryInfoFactory factory)
        {
            this.infoData = factory.getQueryInfo<R>();
        }

        /// <summary>
        /// Constructor fetching Query-Info from factory, with permissions used
        /// </summary>
        public SearchBo(ISearchQueryInfoFactory factory, long permissionId)
        {
            this.infoData = factory.getQueryInfo<R>();
            setPermission(permissionId);
        }
        /// <summary>
        /// Constructor fetching Query-Info from factory, with permissions used
        /// fetches the queryinfo from the given wfvconfig, if available
        /// </summary>
        public SearchBo(ISearchQueryInfoFactory factory, long permissionId, String wfvid, String isoCode)
        {
            this.infoData = factory.getQueryInfo(wfvid);
            this.isoCode = isoCode;
            if (this.infoData == null)
            {
                this.infoData = factory.getQueryInfo<R>();
            }
            setPermission(permissionId);
        }

        /// <summary>
        /// Constructor with Query-Info, no permissions used
        /// </summary>
        /// <param name="tableName"></param>
        public SearchBo(QueryInfoData infoData)
        {
            this.infoData = infoData;
        }

        /// <summary>
        /// Constructor with Query-Info an permissionId
        /// </summary>
        /// <param name="tableName"></param>
        public SearchBo(QueryInfoData infoData, long permissionId)
        {
            this.infoData = infoData;
            setPermission(permissionId);
        }

        /// <summary>
        /// Constructor with Query-Info an permissionId and dao
        /// </summary>
        /// <param name="tableName"></param>
        public SearchBo(QueryInfoData infoData, long permissionId, ISearchDao<R> searchDao)
        {
            this.infoData = infoData;
            setPermission(permissionId);
            this.dao = searchDao;
        }
        #endregion 


        #region PublicMethods
        /// <summary>
        /// Main Search routine, also supporting Exports
        /// </summary>
        /// <param name="param">Search Parameters</param>
        /// <returns>Resultset</returns>
        public oSearchDto<R> search(iSearchDto param)
        {

            if(this.isoCode!=null)
            {
                
                Filter f = (from t in param.filters
                                where t.filterType==FilterType.REPLACE && t.fieldname.Equals("ISOCODE")
                                select t).FirstOrDefault();
                if (f == null) // add ISOCODE as filter, available as {ISOCODE} in the queries
                {
                    List<Filter> tfilter = param.filters.ToList();
                    Filter langFilter = new Filter();
                    langFilter.filterType = FilterType.REPLACE;
                    langFilter.fieldname = "ISOCODE";
                    langFilter.value = isoCode;
                    tfilter.Add(langFilter);
                    param.filters = tfilter.ToArray();
                }
            }
            // add one equal-filter else some queries wont return all results?! Dapper Problem?!
            List<Filter> tfilter2 = null;
            if(param.filters!=null)
                tfilter2=param.filters.ToList();
            if (tfilter2 == null)
                tfilter2 = new List<Filter>();
            Filter tf = new Filter();
            tf.filterType = FilterType.Equal;
            tf.fieldname = "'X'";
            tf.value = "X";
            tfilter2.Add(tf);
            param.filters = tfilter2.ToArray();


            if (param.csvExport != null) // export does the quick path with enumerable
                return export(param);

            param = groupOrFilters(param);
            int retries = 1;
            for (int i = 0; i < retries; i++)
            {
                String debugInfo = "";
                try
                {
                    oSearchDto<R> found = searchInternal (param, infoData, out debugInfo);
					// infoData ermittlen
                    dao.PostPrepare (found, permissionId,infoData);
                    return found;
                }
                catch (Exception ex)
                {

                    if (i == (retries - 1))
                    {
                        _log.Error("Retrying failed for " + debugInfo + ": ", ex);
                        throw ex;
                    }
                    else _log.Warn("Retrying search " + debugInfo + " because of " + ex.Message, ex);
                }
            }
            throw new Exception("Error in Query");
        }

        /// <summary>
        /// Search directly as queryable for postprocessing without huge memory footprint
        /// dao-postprocessing WONT occur here!
        /// Will always use Complete-Query!
        /// </summary>
        /// <param name="param">Search Parameters</param>
        /// <returns>Resultset</returns>
        public IEnumerable<R> searchQueryable(ObjectContext ctx, iSearchDto param)
        {
            param = groupOrFilters(param);
            int retries = 1;
            for (int i = 0; i < retries; i++)
            {
                String debugInfo = "";
                try
                {
                    return searchInternalQueryable(ctx, param, infoData, out debugInfo);
                }
                catch (Exception ex)
                {

                    if (i == (retries - 1))
                    {
                        _log.Error("Retrying failed for " + debugInfo + ": ", ex);
                        throw ex;
                    }
                    else _log.Warn("Retrying search " + debugInfo + " because of " + ex.Message, ex);
                }
            }
            throw new Exception("Error in Query");
        }

        /// <summary>
        /// Performs a complete search and exports to given format, also supported via search
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        public oSearchDto<R> export(iSearchDto param)
        {
            param = groupOrFilters(param);
            int retries = 1;
            for (int i = 0; i < retries; i++)
            {
                String debugInfo = "";
                try
                {
                    using (DdOlExtended ctx = new DdOlExtended())
                    {
                        IEnumerable<R> toExport = searchInternalQueryable(ctx, param, infoData, out debugInfo);
                        oSearchDto<R> rval = new oSearchDto<R>();
                        StringBuilder line = new StringBuilder();
                        String NEWLINE = "\n";
                        line.Append(param.csvExport.header);
                        line.Append(NEWLINE);
                        if (param.csvExport.fields.EndsWith(param.csvExport.separator))
                            param.csvExport.fields = param.csvExport.fields.Substring(0, param.csvExport.fields.Length - 1);
                        String[] fields = param.csvExport.fields.Split(param.csvExport.separator.ToCharArray()[0]);
                        Type typeParameterType = typeof(R);
                        MethodInfo[] meths = typeParameterType.GetMethods();
                        List<MethodInfo> fieldMethods = new List<MethodInfo>();
                        Dictionary<MethodInfo, EncloseInfo> fieldEnclosed = new Dictionary<MethodInfo, EncloseInfo>();
                        foreach (String field in fields)
                        {
                            EncloseInfo ei = new EncloseInfo();
                            String useField = field;
                            if (field.StartsWith("\""))
                            {
                                ei.encloseChar = "\"";
                                useField = field.Substring(1, field.Length - 2);
                            }
                            else if (field.StartsWith("'"))
                            {
                                ei.encloseChar = "'";
                                useField = field.Substring(1, field.Length - 2);
                            }
                            MethodInfo m = getFieldMethod(meths, useField);
                            if (m != null)
                            {
                                fieldMethods.Add(m);
                                fieldEnclosed[m] = ei;
                            }
                        }

                        foreach (R result in toExport)
                        {
                            foreach (MethodInfo m in fieldMethods)
                            {

                                if (m == null)
                                {
                                    line.Append(param.csvExport.separator);
                                    continue;
                                }
                                Object fieldRval = m.Invoke(result, (Object[])null);
                                String value = null;
                                if (fieldRval == null)
                                {
                                    line.Append(param.csvExport.separator);
                                    continue;
                                }
                                EncloseInfo ei = fieldEnclosed[m];
                                if (ei.encloseChar != null)
                                    line.Append(ei.encloseChar);
                                if (fieldRval.GetType().Equals(typeof(DateTime)))
                                {
                                    value = String.Format(param.csvExport.dateFormat, (DateTime)fieldRval);
                                }
                                else
                                {
                                    value = fieldRval.ToString();
                                    value = value.Replace("'", "");
                                    value = value.Replace(param.csvExport.separator, "");
                                }
                                line.Append(value);
                                if (ei.encloseChar != null)
                                    line.Append(ei.encloseChar);
                                line.Append(param.csvExport.separator);
                            }
                            line.Append(NEWLINE);
                        }
                        //recode utf8 to target
                        Encoding iso = Encoding.GetEncoding(param.csvExport.encoding);
                        Encoding utf8 = Encoding.UTF8;
                        byte[] utfBytes = utf8.GetBytes(line.ToString());
                        byte[] isoBytes = Encoding.Convert(utf8, iso, utfBytes);
                        rval.export = isoBytes;

                        return rval;
                    }
                }
                catch (Exception ex)
                {

                    if (i == (retries - 1))
                    {
                        _log.Error("Retrying failed for " + debugInfo + ": ", ex);
                        throw ex;
                    }
                    else _log.Warn("Retrying search " + debugInfo + " because of " + ex.Message, ex);
                }
            }
            throw new Exception("Error in Query");
        }

        /// <summary>
        /// Set Permission
        /// </summary>
        /// <param name="permissionId"></param>
        /// <param name="permissionNeeded"></param>
        /// <param name="permissionType"></param>
        public void setPermission(long permissionId)
        {
            this.permissionId = permissionId;
        }

        #endregion 

        #region Export
        /// <summary>
        /// Gets the Method for the given fieldName
        /// </summary>
        /// <param name="meths"></param>
        /// <param name="field"></param>
        /// <returns></returns>
        private static MethodInfo getFieldMethod(MethodInfo[] meths, String field)
        {

            foreach (MethodInfo m in meths)
            {
                if (m.GetParameters().Count() != 0)
                {
                    continue;
                }
                if (!m.Name.StartsWith("get_"))
                {
                    continue;
                }
                if (m.Name.ToUpper().Equals("GET_" + field.ToUpper()))
                {
                    return m;
                }
            }
            return null;
        }

        
        
        #endregion 

       
        /// <summary>
        /// performs a search returning a queryable Enumeration
        /// </summary>
        /// <param name="ctx"></param>
        /// <param name="param"></param>
        /// <param name="infoData"></param>
        /// <param name="debugInfo"></param>
        /// <returns></returns>
        protected IEnumerable<R> searchInternalQueryable(ObjectContext ctx, iSearchDto param, QueryInfoData infoData, out String debugInfo)
        {

            debugInfo = "";

            if (param.pageSize == 0) param.pageSize = 1;

            String query = createQuery(param, infoData, out debugInfo);
            object[] pars = deliverQueryParametersValues(param);
            if (query.IndexOf(":") < 0) pars = null;

            return dao.search(ctx, query, pars);

        }

        /// <summary>
        /// Main internal search logic determining the type of search (partial, info, complete)
        /// </summary>
        /// <param name="param"></param>
        /// <param name="infoData"></param>
        /// <param name="debugInfo"></param>
        /// <returns></returns>
        protected oSearchDto<R> searchInternal(iSearchDto param, QueryInfoData infoData, out String debugInfo)
        {
            oSearchDto<R> rval = new oSearchDto<R>();
            debugInfo = "";

            if (param.pageSize == 0) param.pageSize = 1;

            //query for original search type
            String query = createQuery(param, infoData, out debugInfo);
            object[] pars = deliverQueryParametersValues(param);
            if (query.IndexOf(":") < 0) pars = null;
            rval.fields = dao.getFields();
            //delivers count for filtered search only
            if (param.searchType == SearchType.Informative)
            {
                String key = getParamKey(param, infoData.entityTable + "_" + query + permissionId);
                if (!getCacheCount(infoData.entityTable).ContainsKey(key))
                    getCacheCount(infoData.entityTable)[key] = daoCount.search(query, pars).First();

                rval.searchCountFiltered = getCacheCount(infoData.entityTable)[key];
            }
            //delivers paged result
            else if (param.searchType == SearchType.Partial)
            {
                //no count for rownum mode, max displayed results would be max the size of rownum
                if (infoData.searchMode == SearchMode.RowNum || infoData.searchMode == SearchMode.NoCount)
                {
                    List<R> allResults = dao.search(query, pars);

                    rval.searchCountMax = allResults.Count();

                    if (param.searchType == SearchType.Complete)
                        param.pageSize = rval.searchCountMax;

                    //avoid range-fetching errors:
                    int useSize = param.pageSize;
                    if (param.skip > rval.searchCountMax)
                    {
                        param.skip = rval.searchCountMax - 1 - useSize;

                    }
                    if (param.skip < 0) param.skip = 0;
                    if (param.skip + useSize >= rval.searchCountMax)
                    {
                        useSize = rval.searchCountMax - param.skip;
                    }
                    rval.results = allResults.GetRange(param.skip, useSize).ToArray<R>();

                    rval.searchCountFiltered = rval.results.Length;


                    rval.searchNumPages = (int)Math.Ceiling(rval.searchCountMax * 1.0M / (1.0M * param.pageSize));
                    param.searchType = SearchType.Partial;//reset param
                }
                else
                {
                    param.searchType = SearchType.Informative;

                    String cquery = createQuery(param, infoData, out debugInfo);
                    String key = getParamKey(param, infoData.entityTable + "_" + cquery + permissionId);

                    if (!getCacheCount(infoData.entityTable).ContainsKey(key))
                    {
                        object[] parsCount = deliverQueryParametersValues(param);
                        getCacheCount(infoData.entityTable)[key] = daoCount.search(cquery, parsCount).First();
                    }
                    rval.searchCountMax = getCacheCount(infoData.entityTable)[key];


                    rval.results = dao.search(query, pars).ToArray<R>();
                    //handle case where the count-cache delivers zero but meanwhile a value has been inserted and the list stays empty because the cache is not yet invalidated
                    if (rval.results.Length > rval.searchCountMax)
                        rval.searchCountMax = rval.results.Length;

                    rval.searchCountFiltered = rval.results.Length;


                    rval.searchNumPages = (int)Math.Ceiling(rval.searchCountMax * 1.0M / (1.0M * param.pageSize));
                    param.searchType = SearchType.Partial;//reset param
                }
            }
            else//Complete, no paging
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
                foreach (Filter fp in param.filters)
                {

                    String searchPattern = getTrimed(fp.value);


                    if (fp.values != null && fp.values.Count() > 0)
                    {
                        searchPattern = String.Join("_", fp.values).Trim();
                    }
                    if (searchPattern == null || searchPattern.Length == 0)
                    {
                        continue;
                    }

                    if (fp.orFilter)
                    {

                        sb.Append(String.Join("_", fp.groupFields));
                        sb.Append(fp.value);
                        sb.Append(fp.filterType);
                        sb.Append(searchPattern);
                    }
                    else if (fp.values != null && fp.values.Count() > 0)
                    {
                        sb.Append(getFieldParam(fp.fieldname));
                        sb.Append(searchPattern);
                        sb.Append(fp.filterType);
                    }
                    else
                    {
                        sb.Append(getFieldParam(fp.fieldname));
                        sb.Append(searchPattern);
                        sb.Append(fp.filterType);
                    }
                }
            return sb.ToString();
        }

        private static String getParams(object[] par)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(": ");
            if (par != null)
                foreach (object o in par)
                {
                    sb.Append(((Devart.Data.Oracle.OracleParameter)o).ParameterName);
                    sb.Append("=");
                    sb.Append(((Devart.Data.Oracle.OracleParameter)o).Value.ToString());
                    sb.Append(";");
                }
            return sb.ToString();
        }

        /// <summary>
        /// Searches for or-Filters with same groupname or same fieldname and builds a or-expression filter removing the others
        /// </summary>
        /// <param name="param"></param>
        /// <returns></returns>
        private iSearchDto groupOrFilters(iSearchDto param)
        {
            if (param.filters == null || param.filters.Length == 0) return param;
            Dictionary<String, Filter> groupFilters = new Dictionary<String, Filter>();
            Dictionary<String, List<String>> orValues = new Dictionary<String, List<String>>();
            Dictionary<String, List<String>> orFields = new Dictionary<String, List<String>>();
            List<Filter> newFilters = new List<Filter>();
            //from or-filters build one or filter with all different values
            foreach (Filter lf in param.filters)
            {
                if (!lf.orFilter && !lf.andFilter)
                {
                    newFilters.Add(lf);
                    continue;
                }

                String groupName = lf.groupName;
                if (groupName == null || groupName.Length == 0)
                {
                    groupName = lf.fieldname;
                }
                if (lf.groupFields != null && lf.groupFields.Length > 0)//already a correctly build or-filter from caller, but doesnt have to be complete.
                {
                    if (lf.groupFields.Length == 1 && lf.groupFields[0].Contains(','))//when only one groupfield split this when , is inside
                    {
                        lf.groupFields = lf.groupFields[0].Split(',');
                    }
                    if (!groupFilters.ContainsKey(groupName))
                    {
                        groupFilters.Add(groupName, lf);
                        orValues[groupName] = lf.values.ToList();
                        orFields[groupName] = lf.groupFields.ToList();
                    }
                    else
                    {
                        orValues[groupName].AddRange(lf.values);
                        orFields[groupName].AddRange(lf.groupFields);
                    }
                    //newFilters.Add(lf);
                    continue;
                }
                //filter doesnt define orfields, so add them from plain filter name/value field
                if (!groupFilters.ContainsKey(groupName))
                {
                    groupFilters[groupName] = lf;
                    orFields[groupName] = new List<String>();
                    orValues[groupName] = new List<String>();
                }
                orFields[groupName].Add(lf.fieldname);
                if (lf.values != null && lf.values.Length > 0)
                {
                    orValues[groupName].AddRange(lf.values);
                }
                else
                {
                    orValues[groupName].Add(lf.value);
                }
            }
            foreach (String key in groupFilters.Keys)
            {
                Filter f = groupFilters[key];
                f.groupFields = orFields[key].ToArray();
                f.values = orValues[key].ToArray();
                newFilters.Add(f);
            }
            param.filters = newFilters.ToArray();
            return param;
        }

       
        
        /// <summary>
        /// Create Searchquery from QueryInfoData
        /// </summary>
        /// <param name="param">Parameters</param>
        /// <returns>Query</returns>
        private string createQuery(iSearchDto param, QueryInfoData infoDataIn, out String debugInfo)
        {
            if (param == null)
            {
                debugInfo = "no Parameters, aborting Search";
                return "";
            }
            //External parameter to force cache flush
            if (param.flushCache)
                SearchCache.entityChanged(infoDataIn.entityTable);

            debugInfo = "";
            QueryInfoData infoData = infoDataIn.clone();
            String permFilter = null;
            if (param.filters != null)
            {
                foreach (Filter filter in param.filters)
                {
                    if (filter.filterType != FilterType.REPLACE)
                        continue;
                    infoData.replaceStrings.Add(filter.fieldname, filter.value);
                }

                List<Filter> permF = param.filters.Where(f => f.filterType == FilterType.OWNER).ToList();
                if (permF != null && permF.Count > 0)
                {
                    permFilter = "(";
                    foreach (Filter f in permF)
                    {
                        if (permFilter.Length > 1)
                            permFilter += " OR ";

                        permFilter += f.fieldname + "=" + f.value;
                    }
                    permFilter += ")";
                }
            }

            #region optimized
            String querySort = deliveryQuerySort(param);
            infoData.orderby = querySort;
            if (infoData.optimized && querySort.Length > 0 && String.IsNullOrEmpty(infoDataIn.groupby)) //only for small  tables less than a few thousand entries!
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

                String resultIdCacheKey = getParamKey(param, infoData.entityTable + "_" + permissionId + querySort);


                if (!getCacheResultId(infoData.entityTable).ContainsKey(resultIdCacheKey))//id cache noch nicht befüllt
                {
                    String orgFields = infoData.resultFields;
                    if (!checkSortFieldDiffersEntityField(param, infoData.entityField))
                        infoData.resultFields += "distinct ";  //distinct does not work when sortfield differs from entityField!

                    infoData.resultFields =  infoData.entityField;//0,3,4 //hole nur die ID, nichts anderes

                    if (permissionId > 0)
                    {
                        infoData.addAdditionalSearchConditions(infoData.getPermissionCondition(permissionId, permFilter));
                        infoData.addAdditionalTables(infoData.permissionTables);
                    }
                    infoData.addAdditionalSearchConditions(deliverQueryParametersString(param));

                    //infoData.addAdditionalSearchConditions(querySort);
                    if(infoData.searchMode==SearchMode.RowNum)
                    {
                        infoData.addAdditionalSearchConditions(" and rownum<=100 ");
                    }

                    String idQuery = infoData.getCompleteQuery();
                    SearchDao<long> sd = new SearchDao<long>();
                    debugInfo = idQuery + getParams(parsCache);

                    if (_log.IsDebugEnabled)
                    {
                        _log.Debug("Search with query: " + idQuery + getParams(parsCache));
                    }


                    getCacheResultId(infoData.entityTable)[resultIdCacheKey] = sd.search(idQuery, parsCache);

                    infoData.resultFields = orgFields;
                    infoData.clearAdditionalSearchConditions();

                }
                List<long> ids = getCacheResultId(infoData.entityTable)[resultIdCacheKey];
                //autolearning optimizer, disableoptimized when once too many results
                if (ids.Count() > 2500)
                {
                    infoDataIn.optimized = false;
                    _log.Debug("Disable optimized search for " + infoData.entityTable);
                }

                //simple logic to fetch the size by the rval of this method:
                if (param.searchType == SearchType.Informative)
                    return "select " + ids.Count + " from dual";

                if (param.searchType == SearchType.Complete)
                    param.pageSize = ids.Count;

                //avoid range-fetching errors:
                int useSize = param.pageSize;
                if (param.skip > ids.Count)
                {
                    param.skip = ids.Count - 1 - useSize;

                }
                if (param.skip < 0) param.skip = 0;
                if (param.skip + useSize >= ids.Count)
                {
                    useSize = ids.Count - param.skip;
                }
                ids = ids.GetRange(param.skip, useSize);
                //avoid error on empty list
                if (ids.Count == 0)
                    ids.Add(-1);
                //build the simple id fetch query

                // if (!deliveryQuerySort(param).Equals(""))
                infoData.addAdditionalSearchConditions(" and " + infoData.entityField + " in (" + string.Join(",", ids) + ") ");// + querySort);
                // else
                //    infoData.addAdditionalSearchConditions(" and " + infoData.entityField + " in (" + string.Join(",", ids) + ") " + " ORDER BY " + infoData.entityTable + ".SYS" + infoData.entityTable + " DESC");

                return infoData.getCompleteQuery();
            }
            #endregion

            String orgCond = infoData.searchConditions;
            if (permissionId > 0)
            {
                infoData.addAdditionalSearchConditions(infoData.getPermissionCondition(permissionId, permFilter));
                infoData.addAdditionalTables(infoData.permissionTables);
            }
           
            infoData.pageSize = (param.skip + param.pageSize).ToString();
            infoData.startRow = param.skip.ToString();

            String rval = "";

            switch (param.searchType)
            {
                case SearchType.Informative:
                    infoData.addAdditionalSearchConditions(deliverQueryParametersString(param));

                    rval = infoData.getCountQuery();

                    break;
                case SearchType.Partial:
                   

                    if (infoData.searchMode == SearchMode.RowNum)
                    {
                        infoData.addAdditionalSearchConditions(" and rownum<=100 ");
                        infoData.addAdditionalSearchConditions(deliverQueryParametersString(param));
                        //infoData.addAdditionalSearchConditions(querySort);
                        rval = infoData.getCompleteQuery();
                    }
                    else if (infoData.searchMode == SearchMode.NoCount)
                    {
                        infoData.addAdditionalSearchConditions(deliverQueryParametersString(param));
                        rval = infoData.getCompleteQuery();
                    }
                    else
                    {
                        infoData.addAdditionalSearchConditions(deliverQueryParametersString(param));
                        //infoData.addAdditionalSearchConditions(querySort);
                        rval = infoData.getPartialQuery();
                    }

                    break;

                case SearchType.Complete:
                    infoData.addAdditionalSearchConditions(deliverQueryParametersString(param));

                    //infoData.addAdditionalSearchConditions(querySort);

                    rval = infoData.getCompleteQuery();

                    break;

            }

            infoData.clearAdditionalSearchConditions();
            return rval;


        }

        #region private Query independent methods
        /// <summary>
        /// Deliver Query Parameter Values
        /// </summary>
        /// <param name="isearchDto">Input Parameters</param>
        /// <returns>Return Data</returns>
        private object[] deliverQueryParametersValues(iSearchDto isearchDto)
        {
            List<OracleParameter> ParametersList = new List<OracleParameter>();

            if(infoData!=null && infoData is QueryInfoDataType5 && isearchDto.ctx!=null)
            {
                ViewMeta vm = ((QueryInfoDataType5)infoData).getQueryConfig();
                if(vm.query!=null && vm.query.expressions!=null && vm.query.expressions.Count>0 && vm.query.query.IndexOf(":p1")>-1)
                {
                    
                    List<String> results = BPEWorkflowService.EvaluateMultiple(Cic.One.Web.BO.BOFactoryFactory.getInstance().getCASBo(),"SYSTEM",0,
                        isearchDto.ctx.sysWFUSER, vm.query.expressions);
                    int idx = 1;
                    foreach(String expOrg in vm.query.expressions)
                    {
                        String result = "0";
                        if(results.Count>(idx-1))
                            result = results[idx-1];
                        if (expOrg.ToUpper().Equals("WF:USERID"))
                            result = ""+isearchDto.ctx.sysWFUSER;

                        if (vm.query.query.IndexOf(":p"+idx)>-1)
                        {
                            ParametersList.Add(new Devart.Data.Oracle.OracleParameter("p"+idx, result));
                        }
                        else
                        {
                            _log.Warn("WFLIST-Query does not contain defined parameter p" + idx + ": " + vm.query.query);
                        }
                        idx++;
                    }
                }
            }

            if (isearchDto.filters != null)
            {
                int pidx = 0;
                foreach (Filter fp in isearchDto.filters)
                {

                    String searchPattern = getTrimed(fp.value);

                    if (fp.values != null && fp.values.Count() > 0)
                    {
                        searchPattern = String.Join("_", fp.values).Trim();
                    }
                    if (searchPattern == null || searchPattern.Length == 0)
                    {
                        continue;
                    }


                    pidx++;
                    string[] fieldList;
                    char[] separ = { ',' };
                    int scount = 0;
                    switch (fp.filterType)
                    {
                        case FilterType.EXISTS:
                            fieldList = fp.fieldname.Split(separ);
                            scount = fieldList.Count();


                            if (String.IsNullOrEmpty(fp.filterquery))//for the case the value is used from a dropdown
                                break;
                            for (int i = 0; i < scount; i++)
                            {
                                ParametersList.Add(new Devart.Data.Oracle.OracleParameter("p" + pidx + getFieldParam(fieldList[i]), searchPattern ));
                            }
                            break;
                        case FilterType.Like:

                            fieldList = fp.fieldname.Split(separ);
                            scount = fieldList.Count();
                            searchPattern = searchPattern.Replace('*', '%');
                            for (int i = 0; i < scount; i++)
                            {
                                ParametersList.Add(new Devart.Data.Oracle.OracleParameter("p" + pidx + getFieldParam(fieldList[i]), "%" + searchPattern + "%"));
                            }
                            break;
                        case FilterType.Like2://always ends with %, other % optionally by entering or entering *

                            fieldList = fp.fieldname.Split(separ);
                            scount = fieldList.Count();
                            searchPattern = searchPattern.Replace('*', '%');
                            if (!searchPattern.EndsWith("%"))
                                searchPattern += "%";
                            for (int i = 0; i < scount; i++)
                            {
                                ParametersList.Add(new Devart.Data.Oracle.OracleParameter("p" + pidx + getFieldParam(fieldList[i]), searchPattern ));
                            }
                            break;
                        case FilterType.Equal:
                        case FilterType.NotEqual:
                            if (fp.orFilter || fp.andFilter)
                            {
                                for (int i = 0; i < fp.values.Length; i++)
                                {
                                    if (fp.groupFields != null && fp.groupFields.Length > 1)
                                    {
                                        for (int t = 0; t < fp.groupFields.Length; t++)
                                        {
                                            ParametersList.Add(new Devart.Data.Oracle.OracleParameter("p" + pidx + (i * t) + getFieldParam(fp.fieldname), fp.values[i]));
                                        }
                                    }
                                    else ParametersList.Add(new Devart.Data.Oracle.OracleParameter("p" + pidx + i + getFieldParam(fp.fieldname), fp.values[i]));
                                }
                            }
                            else
                            {
                                fieldList = fp.fieldname.Split(separ);
                                scount = fieldList.Count();

                                for (int i = 0; i < scount; i++)
                                {
                                    ParametersList.Add(new Devart.Data.Oracle.OracleParameter("p" + pidx + getFieldParam(fieldList[i]), searchPattern));
                                }
                            }
                            break;
                        case FilterType.OrEqual:
                            if (fp.orFilter || fp.andFilter)
                            {
                                for (int i = 0; i < fp.values.Length; i++)
                                {
                                    ParametersList.Add(new Devart.Data.Oracle.OracleParameter("p" + pidx + i + getFieldParam(fp.fieldname), fp.values[i]));
                                }
                            }
                            else
                            {
                                fieldList = fp.fieldname.Split(separ);
                                scount = fieldList.Count();

                                for (int i = 0; i < scount; i++)
                                {
                                    ParametersList.Add(new Devart.Data.Oracle.OracleParameter("p" + pidx + getFieldParam(fieldList[i]), searchPattern));
                                }
                            }
                            break;
                        case FilterType.Begins:
                            fieldList = fp.fieldname.Split(separ);
                            scount = fieldList.Count();
                            searchPattern = searchPattern.Replace('*', '%');
                            for (int i = 0; i < scount; i++)
                            {
                                ParametersList.Add(new Devart.Data.Oracle.OracleParameter("p" + pidx + getFieldParam(fieldList[i]), searchPattern + "%"));
                            }
                            break;
                        case FilterType.Ends:
                            fieldList = fp.fieldname.Split(separ);
                            scount = fieldList.Count();
                            searchPattern = searchPattern.Replace('*', '%');
                            for (int i = 0; i < scount; i++)
                            {
                                ParametersList.Add(new Devart.Data.Oracle.OracleParameter("p" + pidx + getFieldParam(fieldList[i]), "%" + searchPattern));
                            }
                            break;
                        case FilterType.GT:
                            fieldList = fp.fieldname.Split(separ);
                            scount = fieldList.Count();

                            for (int i = 0; i < scount; i++)
                            {
                                ParametersList.Add(new Devart.Data.Oracle.OracleParameter("p" + pidx + getFieldParam(fieldList[i]), fp.value));
                            }
                            break;
                        case FilterType.LT:
                            fieldList = fp.fieldname.Split(separ);
                            scount = fieldList.Count();

                            for (int i = 0; i < scount; i++)
                            {
                                ParametersList.Add(new Devart.Data.Oracle.OracleParameter("p" + pidx + getFieldParam(fieldList[i]), fp.value));
                            }
                            break;

                        case FilterType.RegEx:
                            fieldList = fp.fieldname.Split(separ);
                            scount = fieldList.Count();

                            for (int i = 0; i < scount; i++)
                            {
                                ParametersList.Add(new Devart.Data.Oracle.OracleParameter("p" + pidx + getFieldParam(fieldList[i]), fp.value));
                            }
                            break;

                        case FilterType.WEB:

                            //ParametersList.Add(new Devart.Data.Oracle.OracleParameter("p" + fp.fieldname.Replace('.', '_'), fp.value));

                            break;
                        case FilterType.WEB2:

                            //ParametersList.Add(new Devart.Data.Oracle.OracleParameter("p" + fp.fieldname.Replace('.', '_'), fp.value));

                            break;

                        case FilterType.GE:
                            fieldList = fp.fieldname.Split(separ);
                            scount = fieldList.Count();

                            for (int i = 0; i < scount; i++)
                            {
                                ParametersList.Add(new Devart.Data.Oracle.OracleParameter("p" + pidx + getFieldParam(fieldList[i]), fp.value));
                            }
                            break;

                        case FilterType.LE:
                            fieldList = fp.fieldname.Split(separ);
                            scount = fieldList.Count();

                            for (int i = 0; i < scount; i++)
                            {
                                ParametersList.Add(new Devart.Data.Oracle.OracleParameter("p" + pidx + getFieldParam(fieldList[i]), fp.value));
                            }
                            break;

                        case FilterType.DateEqual:
                            fieldList = fp.fieldname.Split(separ);
                            scount = fieldList.Count();

                            for (int i = 0; i < scount; i++)
                            {
                                ParametersList.Add(new Devart.Data.Oracle.OracleParameter("p" + pidx + getFieldParam(fieldList[i]), searchPattern));
                            }
                            break;

                        case FilterType.DateGT:
                            fieldList = fp.fieldname.Split(separ);
                            scount = fieldList.Count();

                            for (int i = 0; i < scount; i++)
                            {
                                ParametersList.Add(new Devart.Data.Oracle.OracleParameter("p" + pidx + getFieldParam(fieldList[i]), searchPattern));
                            }
                            break;
                        case FilterType.BETWEEN:


                            if (fp.values == null || fp.values.Length != 2) continue;


                            if (fp.values[0].Trim().Length > 0) ParametersList.Add(new Devart.Data.Oracle.OracleParameter("p" + pidx + getFieldParam(fp.fieldname), fp.values[0].Trim()));
                            if (fp.values[1].Trim().Length > 0) ParametersList.Add(new Devart.Data.Oracle.OracleParameter("p2" + pidx + getFieldParam(fp.fieldname), fp.values[1].Trim()));

                            break;

                        case FilterType.BETWEENNUMBER:
                            fieldList = fp.fieldname.Split(separ);


                            if (fp.values == null || fp.values.Length != 2) continue;


                            if (fp.values[0].Trim().Length > 0) ParametersList.Add(new Devart.Data.Oracle.OracleParameter("p" + pidx + getFieldParam(fp.fieldname), fp.values[0].Trim()));
                            if (fp.values[1].Trim().Length > 0) ParametersList.Add(new Devart.Data.Oracle.OracleParameter("p2" + pidx + getFieldParam(fp.fieldname), fp.values[1].Trim()));


                            break;

                        case FilterType.DateLT:
                            fieldList = fp.fieldname.Split(separ);
                            scount = fieldList.Count();

                            for (int i = 0; i < scount; i++)
                            {
                                ParametersList.Add(new Devart.Data.Oracle.OracleParameter("p" + pidx + getFieldParam(fieldList[i]), searchPattern));
                            }
                            break;

                        case FilterType.DateGE:
                            fieldList = fp.fieldname.Split(separ);
                            scount = fieldList.Count();

                            for (int i = 0; i < scount; i++)
                            {
                                ParametersList.Add(new Devart.Data.Oracle.OracleParameter("p" + pidx + getFieldParam(fieldList[i]), searchPattern));
                            }
                            break;

                        case FilterType.DateLE:
                            fieldList = fp.fieldname.Split(separ);
                            scount = fieldList.Count();

                            for (int i = 0; i < scount; i++)
                            {
                                ParametersList.Add(new Devart.Data.Oracle.OracleParameter("p" + pidx + getFieldParam(fieldList[i]), searchPattern));
                            }
                            break;
                    }
                }
            }



            return ParametersList.ToArray();
        }

        /// <summary>
        /// Converts a given fieldname-String (may be sql!) to a valid parameter-name for oracle
        /// </summary>
        /// <param name="field"></param>
        /// <returns></returns>
        private static String getFieldParam(String field)
        {
            //(trim(wf.name)||' '||trim(wf.vorname))
            String rval = field.Trim().Replace('.', '_').Replace('(', '_').Replace(')', '_').Replace('|', '_').Replace('\'', '_').Replace(' ', '_');
            return Math.Abs(rval.GetHashCode()).ToString();
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
                int pidx = 0;
                foreach (Filter fp in isearchDto.filters)
                {
                    String searchPattern = getTrimed(fp.value);
                    if (fp.values != null && fp.values.Count() > 0)
                    {
                        searchPattern = String.Join("_", fp.values).Trim();
                    }
                    if (searchPattern == null || searchPattern.Length == 0)
                        continue;

                    if (searchPattern.Length > 40 && fp.filterType != FilterType.SQL)
                    {
                        searchPattern = searchPattern.Substring(0, 40);
                    }
                    pidx++;
                    string[] fieldList;
                    char[] separ = { ',' };
                    int scount = 0;
                    switch (fp.filterType)
                    {
                        case FilterType.EXISTS:
                            fieldList = fp.fieldname.Split(separ);
                            scount = fieldList.Count();
                            queryBuilder.Append(" AND (");
                            for (int i = 0; i < scount; i++)
                            {
                                if (i > 0)
                                {
                                    queryBuilder.Append(" OR ");
                                }
                                String pname = ":p" + pidx + getFieldParam(fieldList[i]);
                                String query = fp.filterquery;
                                if (String.IsNullOrEmpty(query))
                                    query = fp.value;//use this when exists is used in a dropdown with the query as value
                                queryBuilder.Append(" " + String.Format(query, new object[] { pname }) + " ");
                            }
                            queryBuilder.Append(")");
                            break;
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
                                String pname = ":p" + pidx + getFieldParam(fieldList[i]);
                                if (field.IndexOf("SELECT") > -1)
                                {
                                    queryBuilder.Append(" " + field + " LIKE UPPER(" + pname + ")");
                                }
                                else
                                {
                                    queryBuilder.Append(" UPPER(" + field + ") LIKE UPPER(" + pname + ")");
                                }
                            }

                            queryBuilder.Append(")");
                            break;
                        case FilterType.Like2:
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
                                String pname = ":p" + pidx + getFieldParam(fieldList[i]);
                                if (field.IndexOf("SELECT") > -1)
                                {
                                    queryBuilder.Append(" " + field + " LIKE UPPER(" + pname + ")");
                                }
                                else
                                {
                                    queryBuilder.Append(" UPPER(" + field + ") LIKE UPPER(" + pname + ")");
                                }
                            }

                            queryBuilder.Append(")");
                            break;
                        case FilterType.Equal:
                            if (fp.orFilter)
                            {
                                queryBuilder.Append(" AND (");
                                for (int i = 0; i < fp.values.Length; i++)
                                {
                                    if (i > 0)
                                    {
                                        queryBuilder.Append(" OR ");
                                    }
                                    if (fp.groupFields != null && fp.groupFields.Length > 1)
                                    {
                                        queryBuilder.Append(" ( ");
                                        for (int t = 0; t < fp.groupFields.Length; t++)
                                        {
                                            if (t > 0)
                                            {
                                                queryBuilder.Append(" OR ");
                                            }
                                            queryBuilder.Append(fp.groupFields[t].Trim() + " = :p" + pidx + (i * t) + getFieldParam(fp.fieldname));
                                        }
                                        queryBuilder.Append(" ) ");
                                    }
                                    else
                                    {
                                        queryBuilder.Append(fp.fieldname.Trim() + " = :p" + pidx + i + getFieldParam(fp.fieldname));
                                    }
                                }
                                queryBuilder.Append(")");
                            }
                            else
                            {
                                fieldList = fp.fieldname.Split(separ);
                                scount = fieldList.Count();
                                queryBuilder.Append(" AND (");
                                for (int i = 0; i < scount; i++)
                                {
                                    if (i > 0)
                                    {
                                        queryBuilder.Append(" OR ");
                                    }
                                    queryBuilder.Append(fieldList[i].Trim() + " = :p" + pidx + getFieldParam(fieldList[i]));
                                }
                                queryBuilder.Append(")");
                            }
                            break;

                        case FilterType.NotEqual:
                            if (fp.orFilter)
                            {
                                queryBuilder.Append(" AND (");
                                for (int i = 0; i < fp.values.Length; i++)
                                {
                                    if (i > 0)
                                    {
                                        queryBuilder.Append(" OR ");
                                    }
                                    queryBuilder.Append(fp.fieldname.Trim() + " != :p" + pidx + i + getFieldParam(fp.fieldname));
                                }
                                queryBuilder.Append(")");
                            }
                            else
                            {
                                fieldList = fp.fieldname.Split(separ);
                                scount = fieldList.Count();
                                queryBuilder.Append(" AND (");
                                for (int i = 0; i < scount; i++)
                                {
                                    if (i > 0)
                                    {
                                        queryBuilder.Append(" OR ");
                                    }
                                    queryBuilder.Append(fieldList[i].Trim() + " != :p" + pidx + getFieldParam(fieldList[i]));
                                }
                                queryBuilder.Append(")");
                            }
                            break;

                        case FilterType.OrEqual:
                            if (fp.orFilter)
                            {
                                queryBuilder.Append(" OR (");
                                for (int i = 0; i < fp.values.Length; i++)
                                {
                                    if (i > 0)
                                    {
                                        queryBuilder.Append(" OR ");
                                    }
                                    queryBuilder.Append(fp.fieldname.Trim() + " = :p" + pidx + i + getFieldParam(fp.fieldname));
                                }
                                queryBuilder.Append(")");
                            }
                            else
                            {
                                fieldList = fp.fieldname.Split(separ);
                                scount = fieldList.Count();
                                queryBuilder.Append(" OR (");
                                for (int i = 0; i < scount; i++)
                                {
                                    if (i > 0)
                                    {
                                        queryBuilder.Append(" OR ");
                                    }
                                    queryBuilder.Append(fieldList[i].Trim() + " = :p" + pidx + getFieldParam(fieldList[i]));
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
                                queryBuilder.Append(" UPPER(" + fieldList[i].Trim() + ") LIKE UPPER(:p" + pidx + getFieldParam(fieldList[i]) + ")");
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
                                queryBuilder.Append(fieldList[i].Trim() + " LIKE UPPER(:p" + pidx + getFieldParam(fieldList[i]) + ")");
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
                                queryBuilder.Append(fieldList[i].Trim() + " > :p" + pidx + getFieldParam(fieldList[i]));
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
                                queryBuilder.Append(fieldList[i].Trim() + " < :p" + pidx + getFieldParam(fieldList[i]));
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
                                queryBuilder.Append(" REGEXP_LIKE(" + fieldList[i].Trim() + ", :p" + pidx + getFieldParam(fieldList[i]) + ")");
                            }
                            queryBuilder.Append(")");
                            break;

                        case FilterType.SQL:

                            if (fp.values != null && fp.values.Length > 0)
                            {
                                String op = "OR";
                                if (fp.andFilter)
                                    op = "AND";
                                queryBuilder.Append(" AND (");
                                for (int i = 0; i < fp.values.Length; i++)
                                {
                                    if (i > 0) queryBuilder.Append(op);
                                    queryBuilder.Append("(" + fp.fieldname.Trim() + "  " + fp.values[i] + ")");
                                }
                                queryBuilder.Append(" ) ");
                            }
                            else
                            {
                                queryBuilder.Append(" AND (" + fp.fieldname.Trim() + "  " + fp.value + ")");
                            }
                            break;

                        case FilterType.WEB:

                            queryBuilder.Append(" AND " + getWebQuery(fp));
                            break;
                        case FilterType.WEB2://all panel-title fields use this

                            queryBuilder.Append(" AND " + getWeb2Query(fp));
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
                                queryBuilder.Append(fieldList[i].Trim() + " >= :p" + pidx + getFieldParam(fieldList[i]));
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
                                queryBuilder.Append(fieldList[i].Trim() + " <= :p" + pidx + getFieldParam(fieldList[i]));
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
                                queryBuilder.Append("trunc(" + fieldList[i].Trim() + ") = to_date(:p" + pidx + getFieldParam(fieldList[i]) + ", 'yyyy-mm-dd') AND " + fieldList[i].Trim() + " IS NOT NULL");
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
                                queryBuilder.Append("trunc(" + fieldList[i].Trim() + ")  > to_date(:p" + pidx + getFieldParam(fieldList[i]) + ", 'yyyy-mm-dd') AND " + fieldList[i].Trim() + " IS NOT NULL");
                            }
                            queryBuilder.Append(")");
                            break;

                        case FilterType.BETWEEN:
                            fieldList = fp.fieldname.Split(separ);

                            if (fp.values == null || fp.values.Length != 2) continue;
                            queryBuilder.Append(" AND (");


                            queryBuilder.Append("( " + fp.fieldname.Trim() + " IS NOT NULL ");
                            if (fp.values[0].Trim().Length > 0)
                                queryBuilder.Append(" AND trunc(" + fp.fieldname.Trim() + ")  >= to_date(:p" + pidx + getFieldParam(fp.fieldname) + ", 'yyyy-mm-dd') ");
                            if (fp.values[1].Trim().Length > 0)
                                queryBuilder.Append(" AND trunc(" + fp.fieldname.Trim() + ")  <= to_date(:p2" + pidx + getFieldParam(fp.fieldname) + ", 'yyyy-mm-dd')");
                            queryBuilder.Append(")");

                            queryBuilder.Append(")");
                            break;
                        case FilterType.BETWEENNUMBER:
                            fieldList = fp.fieldname.Split(separ);

                            if (fp.values == null || fp.values.Length != 2) continue;
                            if (fp.values[0].Trim().Length > 0 && fp.values[1].Trim().Length > 0)// && !fp.values[0].Trim().Equals(fp.values[1].Trim()))
                            {
                                queryBuilder.Append(" AND (");

                                queryBuilder.Append("( " + fp.fieldname.Trim() + " IS NOT NULL ");
                                if (fp.values[0].Trim().Length > 0)
                                    queryBuilder.Append(" AND " + fp.fieldname.Trim() + "  >= :p" + pidx + getFieldParam(fp.fieldname));
                                if (fp.values[1].Trim().Length > 0)
                                    queryBuilder.Append(" AND " + fp.fieldname.Trim() + "  <= :p2" + pidx + getFieldParam(fp.fieldname));
                                queryBuilder.Append(")");


                                queryBuilder.Append(")");
                            }
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
                                queryBuilder.Append("trunc(" + fieldList[i].Trim() + ")  < to_date( :p" + pidx + getFieldParam(fieldList[i]) + ", 'yyyy-mm-dd') AND " + fieldList[i].Trim() + " IS NOT NULL");
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
                                queryBuilder.Append("trunc(" + fieldList[i].Trim() + ")  >= to_date(:p" + pidx + getFieldParam(fieldList[i]) + ", 'yyyy-mm-dd') AND " + fieldList[i].Trim() + " IS NOT NULL");
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
                                queryBuilder.Append("trunc(" + fieldList[i].Trim() + ")  <= to_date( :p" + pidx + getFieldParam(fieldList[i]) + ", 'yyyy-mm-dd') AND " + fieldList[i].Trim() + " IS NOT NULL");
                            }
                            queryBuilder.Append(")");
                            break;


                    }
                }
            }
            return queryBuilder.ToString();
        }

        /// <summary>
        /// when sort fields differ from entityfield the optimization distinct does not work
        /// </summary>
        /// <param name="isearchDto"></param>
        /// <param name="entityField"></param>
        /// <returns></returns>
        private bool checkSortFieldDiffersEntityField(iSearchDto isearchDto,String entityField)
        {
            if (isearchDto.sortFields != null && isearchDto.sortFields.Length > 0)
            {
                foreach (Sorting s in isearchDto.sortFields)
                {
                    if (s.fieldname != null && s.fieldname != "")
                        if (!s.fieldname.ToUpper().Equals(entityField.ToUpper()))
                            return true;
                }
                
            }
            
            return false;//no sort, so no differ
            
        }
        /// <summary>
        /// Delivery Query Sort
        /// </summary>
        /// <param name="isearchDto">Input Data</param>
        /// <returns>Sort String</returns>
        private string deliveryQuerySort(iSearchDto isearchDto)
        {
            System.Text.StringBuilder queryBuilder = new System.Text.StringBuilder();
            if (isearchDto.sortFields != null && isearchDto.sortFields.Length > 0)
            {
                foreach (Sorting s in isearchDto.sortFields)
                {
                    if (s.fieldname != null && s.fieldname != "")
                        queryBuilder.Append(s.fieldname + " " + s.order + ",");


                }

                if (queryBuilder != null)
                {
                    char[] endChar = { ',' };

                    return "  " + queryBuilder.ToString().TrimEnd(endChar);
                }
                else
                    return "";
            }
            else
                return "";
        }

        /// <summary>
        /// Delivery of Query Group
        /// </summary>
        /// <param name="isearchDto">Input Data</param>
        /// <returns>Group query String</returns>
        private string deliveryQueryGroup(iSearchDto isearchDto)
        {
            System.Text.StringBuilder queryBuilder = new System.Text.StringBuilder();

            foreach (Grouping g in isearchDto.groupFields)
            {
                if (g.fieldname != null && g.fieldname != "")
                    queryBuilder.Append(g.fieldname + ",");


            }
            if (queryBuilder != null)
            {
                char[] endChar = { ',' };
                return " GROUP BY " + queryBuilder.ToString().TrimEnd(endChar);
            }
            else
                return "";
        }

        /// <summary>
        /// Trims the String
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        private static string getTrimed(string value)
        {
            if (value != null)
                return value.Trim();
            else
                return value;
        }

        /// <summary>
        /// Webquery auslösen
        /// </summary>
        /// <param name="filter">Filter</param>
        /// <returns>Rückgabedaten</returns>
        private string getWebQuery(Filter filter)
        {
            string[] fieldlist;
            string querystring = "1=1";
            if (filter.fieldname == null)
            {
                return querystring;
            }

            char[] separ = { ',' };
            fieldlist = filter.fieldname.Split(separ);
            string[] sp;
            int[] styp = null;
            int scount = 0;
            string[] st = filter.value.Split(' ');
            //disallow too many tokens
            if (st.Length > 5)
            {
                string[] nst = new string[5];
                Array.Copy(st, nst, 5);
                st = nst;
            }


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



            return querystring;
        }

        /// <summary>
        /// Webquery auslösen
        /// </summary>
        /// <param name="filter">Filter</param>
        /// <returns>Rückgabedaten</returns>
        private string getWeb2Query(Filter filter)
        {
            string[] fieldlist;
            StringBuilder querystring = new StringBuilder("1=1");
            if (filter.fieldname == null)
            {

                return querystring.ToString();
            }

            char[] separ = { ',' };
            fieldlist = filter.fieldname.Split(separ);
            string[] sp;
            int[] styp = null;
            int scount = 0;
            string[] st = filter.value.Split(' ');
            //disallow too many tokens
            if (st.Length > 5)
            {
                string[] nst = new string[5];
                Array.Copy(st, nst, 5);
                st = nst;
            }

            scount = st.Length;

            sp = new string[scount * 2];
            styp = new int[scount * 2];
            int pos = 0;
            while (pos < st.Length)
            {
                sp[pos] = st[pos];
                //test for date-input token
                DateTime? date = DateParser.parseDate(sp[pos].Trim(), null);
                if (!(date == null || date.Value.Year < 1900))
                {
                    styp[pos] = 4;//recognized date
                }

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
                if (sp[pos].IndexOf("#") == 0)
                {
                    sp[pos] = sp[pos].Substring(1);
                    styp[pos] = 4;
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

            //When this is set to 1, the search will use all tokens with a prefix and suffix % automatically
            long wildcard = AppConfig.Instance.GetCfgEntry("SETUP.NET", "SEARCH", "WILDCARD", 1);
            for (pos = 0; pos < scount; pos++)
            {
                if (styp[pos] == 3)
                {
                    querystring.Append(" AND ( ");

                    for (int i = 0; i < fieldlist.Length; i++)
                    {
                        if (i > 0)
                        {
                            querystring.Append(" OR ");
                        }
                        querystring.Append("UPPER(" + fieldlist[i] + ") = UPPER('" + toDB(sp[pos]) + "')");
                    }
                    querystring.Append(" )");
                }
                else if (styp[pos] == 2)
                {
                    querystring.Append(" AND ( ");

                    for (int i = 0; i < fieldlist.Length; i++)
                    {
                        if (i > 0)
                        {
                            querystring.Append(" OR ");
                        }
                        querystring.Append("SOUNDEX(" + fieldlist[i] + ")" + " = SOUNDEX('" + toDB(sp[pos]) + "')");
                    }
                    querystring.Append(" )");
                }
                else if (styp[pos] == 1)
                {
                    querystring.Append(" AND NOT( ");

                    for (int i = 0; i < fieldlist.Length; i++)
                    {
                        if (i > 0)
                        {
                            querystring.Append(" OR ");
                        }
                        querystring.Append(" UPPER(" + fieldlist[i] + ") LIKE UPPER('%" + toDB(sp[pos]) + "%')");
                    }
                    querystring.Append(" )");

                }
                else if (styp[pos] == 4)//Datesearch
                {
                    DateTime? date = DateParser.parseDate(sp[pos].Trim(), null);
                    if (date == null || date.Value.Year < 1900) continue;
                    String dstr = date.Value.ToString("dd-MMM-yy", CultureInfo.InvariantCulture).ToUpper();
                    querystring.Append(" AND ( ");

                    for (int i = 0; i < fieldlist.Length; i++)
                    {
                        if (i > 0)
                        {
                            querystring.Append(" OR ");
                        }
                        querystring.Append(" (to_char(" + fieldlist[i] + ") ='" + dstr + "' AND " + fieldlist[i] + " is not null) ");
                    }
                    querystring.Append(" )");


                }
                else
                {
                    querystring.Append(" AND ( UPPER( ");

                    for (int i = 0; i < fieldlist.Length; i++)
                    {
                        if (i > 0)
                        {
                            querystring.Append(" || '|' || ");
                        }
                        querystring.Append(fieldlist[i]);

                    }
                    String wc = "";
                    if (wildcard > 0)
                        wc = "%";
                    String sv = toDB(sp[pos]);
                    sv = sv.Replace('*', '%');
                    querystring.Append(" ) like UPPER('" + wc + sv + wc+"'))");

                }
            }



            return querystring.ToString();
        }

        /// <summary>
        /// Converts the String to a escaped db string
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        private string toDB(string text)
        {
            if (text == null)
            {
                return "";
            }
            System.Text.StringBuilder sbuf = new System.Text.StringBuilder(text);
            for (int i = 0; i < sbuf.Length; i++)
            {
                if (sbuf.ToString().ElementAt(i) == '\'')
                {
                    sbuf.Insert(i++, '\'');
                }
            }
            text = sbuf.ToString();

            return "" + text + "";
        }

        /// <summary>
        /// Returns the ID-Cache
        /// </summary>
        /// <param name="entityName"></param>
        /// <returns></returns>
        private CacheDictionary<String, List<long>> getCacheResultId(String entityName)
        {
            return SearchCache.resultIdCache;
        }
        /// <summary>
        /// Returns the count cache
        /// </summary>
        /// <param name="entityName"></param>
        /// <returns></returns>
        private CacheDictionary<String, int> getCacheCount(String entityName)
        {
            return SearchCache.countCache;
        }

        #endregion

    }
    /// <summary>
    /// Holds info about characters enclosing a CSV Field
    /// </summary>
    class EncloseInfo
    {
        public String encloseChar { get; set; }
    }
}