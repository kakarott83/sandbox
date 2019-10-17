using Cic.One.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Cic.One.Web.BO.Search
{
    /// <summary>
    /// Default Search Query Pattern
    /// </summary>
    public class QueryInfoDataType1 : QueryInfoData
    {
        public static String overridePartialQuery = null;
        public static String partialQueryDefault = "select {0} from {1} where {2} {5} {6} offset {4} rows fetch first {3} rows only";
        public static String partialQueryDefault2 = "SELECT /*+FIRST_ROWS*/ * FROM (SELECT rownum rnum, a.* FROM( select {0} from {1} where {2} {5} {6}) a ) WHERE rownum <= {3} and rnum > {4}";

        public QueryInfoDataType1()
        {
            _queryStructure = new QueryStructure();
            _queryStructure.countQuery = "select count(*) from  {0} where {1} {2} {3}";
            _queryStructure.completeQuery = "select  {0} from {1} where {2} {3} {4}";
            _queryStructure.partialQuery = partialQueryDefault;
            if (overridePartialQuery != null)
                _queryStructure.partialQuery = overridePartialQuery;
            searchConditions = " 1=1 ";
        }

        public QueryInfoDataType1(String entityTable, String entityField)
            : this()
        {
            this.entityField = entityField;
            this.entityTable = entityTable;
            resultFields = entityTable + ".*";
            resultTables = entityTable;
            searchTables = entityTable;
            searchConditions = " 1=1 ";
        }
        /// <summary>
        /// Creates a search-Query from the query configuration using the given primary-key fetching prefix
        /// </summary>
        /// <param name="query"></param>
        /// <param name="pkeyPrefix"></param>
        public QueryInfoDataType1(Query query, String pkeyPrefix, SearchMode sm)
            : this()
        {
            this.entityField = query.pkey;
            this.entityTable = query.table;
            this.searchMode = sm;
            resultFields = "";
            
            if(pkeyPrefix!=null && pkeyPrefix.Length>0)
                resultFields += pkeyPrefix + ", ";
            
            resultFields+=query.fields;
            groupby = query.group;
            resultTables = query.tables;
            searchTables = query.tables;
            searchConditions = query.where;
            permissionCondition = query.permissioncondition;
            optimized = false;
        }
        public QueryInfoDataType1(QueryInfoDataType1 org)
            : base(org)
        {

        }
        override public QueryInfoData clone()
        {
            return new QueryInfoDataType1(this);
        }
        override protected object[] getCompleteParams()
        {
            return new object[] { resultFields, searchTables + getAdditionalTables(), getSearchConditions(), getGroupBy(), getOrderBy() };

        }
        override protected object[] getCountParams()
        {
            return new object[] { resultTables + getAdditionalTables(), getSearchConditions(), getGroupBy(), getOrderBy() };

        }
        override protected object[] getPartialParams()
        {
            return new object[] { resultFields, resultTables + getAdditionalTables(), getSearchConditions(), pageSize, startRow, getGroupBy(), getOrderBy() };

        }

    }
}